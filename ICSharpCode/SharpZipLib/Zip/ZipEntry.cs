// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.ZipEntry
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class ZipEntry : ICloneable
  {
    private ZipEntry.Known known;
    private int externalFileAttributes = -1;
    private ushort versionMadeBy;
    private string name;
    private ulong size;
    private ulong compressedSize;
    private ushort versionToExtract;
    private uint crc;
    private uint dosTime;
    private CompressionMethod method = CompressionMethod.Deflated;
    private byte[] extra;
    private string comment;
    private int flags;
    private long zipFileIndex = -1;
    private long offset;
    private bool forceZip64_;
    private byte cryptoCheckValue_;
    private int _aesVer;
    private int _aesEncryptionStrength;

    public ZipEntry(string name)
      : this(name, 0, 51, CompressionMethod.Deflated)
    {
    }

    internal ZipEntry(string name, int versionRequiredToExtract)
      : this(name, versionRequiredToExtract, 51, CompressionMethod.Deflated)
    {
    }

    internal ZipEntry(
      string name,
      int versionRequiredToExtract,
      int madeByInfo,
      CompressionMethod method)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (name.Length > (int) ushort.MaxValue)
        throw new ArgumentException("Name is too long", nameof (name));
      if (versionRequiredToExtract != 0 && versionRequiredToExtract < 10)
        throw new ArgumentOutOfRangeException(nameof (versionRequiredToExtract));
      this.DateTime = DateTime.Now;
      this.name = name;
      this.versionMadeBy = (ushort) madeByInfo;
      this.versionToExtract = (ushort) versionRequiredToExtract;
      this.method = method;
    }

    [Obsolete("Use Clone instead")]
    public ZipEntry(ZipEntry entry)
    {
      this.known = entry != null ? entry.known : throw new ArgumentNullException(nameof (entry));
      this.name = entry.name;
      this.size = entry.size;
      this.compressedSize = entry.compressedSize;
      this.crc = entry.crc;
      this.dosTime = entry.dosTime;
      this.method = entry.method;
      this.comment = entry.comment;
      this.versionToExtract = entry.versionToExtract;
      this.versionMadeBy = entry.versionMadeBy;
      this.externalFileAttributes = entry.externalFileAttributes;
      this.flags = entry.flags;
      this.zipFileIndex = entry.zipFileIndex;
      this.offset = entry.offset;
      this.forceZip64_ = entry.forceZip64_;
      if (entry.extra == null)
        return;
      this.extra = new byte[entry.extra.Length];
      Array.Copy((Array) entry.extra, 0, (Array) this.extra, 0, entry.extra.Length);
    }

    public bool HasCrc => (uint) (this.known & ZipEntry.Known.Crc) > 0U;

    public bool IsCrypted
    {
      get => (uint) (this.flags & 1) > 0U;
      set
      {
        if (value)
          this.flags |= 1;
        else
          this.flags &= -2;
      }
    }

    public bool IsUnicodeText
    {
      get => (uint) (this.flags & 2048) > 0U;
      set
      {
        if (value)
          this.flags |= 2048;
        else
          this.flags &= -2049;
      }
    }

    internal byte CryptoCheckValue
    {
      get => this.cryptoCheckValue_;
      set => this.cryptoCheckValue_ = value;
    }

    public int Flags
    {
      get => this.flags;
      set => this.flags = value;
    }

    public long ZipFileIndex
    {
      get => this.zipFileIndex;
      set => this.zipFileIndex = value;
    }

    public long Offset
    {
      get => this.offset;
      set => this.offset = value;
    }

    public int ExternalFileAttributes
    {
      get => (this.known & ZipEntry.Known.ExternalAttributes) == ZipEntry.Known.None ? -1 : this.externalFileAttributes;
      set
      {
        this.externalFileAttributes = value;
        this.known |= ZipEntry.Known.ExternalAttributes;
      }
    }

    public int VersionMadeBy => (int) this.versionMadeBy & (int) byte.MaxValue;

    public bool IsDOSEntry => this.HostSystem == 0 || this.HostSystem == 10;

    private bool HasDosAttributes(int attributes)
    {
      bool flag = false;
      if ((uint) (this.known & ZipEntry.Known.ExternalAttributes) > 0U && (this.HostSystem == 0 || this.HostSystem == 10) && (this.ExternalFileAttributes & attributes) == attributes)
        flag = true;
      return flag;
    }

    public int HostSystem
    {
      get => (int) this.versionMadeBy >> 8 & (int) byte.MaxValue;
      set
      {
        this.versionMadeBy &= (ushort) byte.MaxValue;
        this.versionMadeBy |= (ushort) ((value & (int) byte.MaxValue) << 8);
      }
    }

    public int Version
    {
      get
      {
        if (this.versionToExtract > (ushort) 0)
          return (int) this.versionToExtract;
        int num = 10;
        if (this.AESKeySize > 0)
          num = 51;
        else if (this.CentralHeaderRequiresZip64)
          num = 45;
        else if (CompressionMethod.Deflated == this.method)
          num = 20;
        else if (this.IsDirectory)
          num = 20;
        else if (this.IsCrypted)
          num = 20;
        else if (this.HasDosAttributes(8))
          num = 11;
        return num;
      }
    }

    public bool CanDecompress => this.Version <= 51 && (this.Version == 10 || this.Version == 11 || this.Version == 20 || this.Version == 45 || this.Version == 51) && this.IsCompressionMethodSupported();

    public void ForceZip64() => this.forceZip64_ = true;

    public bool IsZip64Forced() => this.forceZip64_;

    public bool LocalHeaderRequiresZip64
    {
      get
      {
        bool flag = this.forceZip64_;
        if (!flag)
        {
          ulong compressedSize = this.compressedSize;
          if (this.versionToExtract == (ushort) 0 && this.IsCrypted)
            compressedSize += 12UL;
          flag = (this.size >= (ulong) uint.MaxValue || compressedSize >= (ulong) uint.MaxValue) && (this.versionToExtract == (ushort) 0 || this.versionToExtract >= (ushort) 45);
        }
        return flag;
      }
    }

    public bool CentralHeaderRequiresZip64 => this.LocalHeaderRequiresZip64 || this.offset >= (long) uint.MaxValue;

    public long DosTime
    {
      get => (this.known & ZipEntry.Known.Time) == ZipEntry.Known.None ? 0L : (long) this.dosTime;
      set
      {
        this.dosTime = (uint) value;
        this.known |= ZipEntry.Known.Time;
      }
    }

    public DateTime DateTime
    {
      get
      {
        uint num1 = Math.Min(59U, (uint) (2 * ((int) this.dosTime & 31)));
        uint num2 = Math.Min(59U, this.dosTime >> 5 & 63U);
        uint num3 = Math.Min(23U, this.dosTime >> 11 & 31U);
        uint num4 = Math.Max(1U, Math.Min(12U, this.dosTime >> 21 & 15U));
        uint num5 = (uint) (((int) (this.dosTime >> 25) & (int) sbyte.MaxValue) + 1980);
        int day = Math.Max(1, Math.Min(DateTime.DaysInMonth((int) num5, (int) num4), (int) (this.dosTime >> 16) & 31));
        return new DateTime((int) num5, (int) num4, day, (int) num3, (int) num2, (int) num1);
      }
      set
      {
        uint num1 = (uint) value.Year;
        uint num2 = (uint) value.Month;
        uint num3 = (uint) value.Day;
        uint num4 = (uint) value.Hour;
        uint num5 = (uint) value.Minute;
        uint num6 = (uint) value.Second;
        if (num1 < 1980U)
        {
          num1 = 1980U;
          num2 = 1U;
          num3 = 1U;
          num4 = 0U;
          num5 = 0U;
          num6 = 0U;
        }
        else if (num1 > 2107U)
        {
          num1 = 2107U;
          num2 = 12U;
          num3 = 31U;
          num4 = 23U;
          num5 = 59U;
          num6 = 59U;
        }
        this.DosTime = (long) ((uint) (((int) num1 - 1980 & (int) sbyte.MaxValue) << 25 | (int) num2 << 21 | (int) num3 << 16 | (int) num4 << 11 | (int) num5 << 5) | num6 >> 1);
      }
    }

    public string Name => this.name;

    public long Size
    {
      get => (this.known & ZipEntry.Known.Size) != ZipEntry.Known.None ? (long) this.size : -1L;
      set
      {
        this.size = (ulong) value;
        this.known |= ZipEntry.Known.Size;
      }
    }

    public long CompressedSize
    {
      get => (this.known & ZipEntry.Known.CompressedSize) != ZipEntry.Known.None ? (long) this.compressedSize : -1L;
      set
      {
        this.compressedSize = (ulong) value;
        this.known |= ZipEntry.Known.CompressedSize;
      }
    }

    public long Crc
    {
      get => (this.known & ZipEntry.Known.Crc) != ZipEntry.Known.None ? (long) this.crc & (long) uint.MaxValue : -1L;
      set
      {
        this.crc = ((ulong) this.crc & 18446744069414584320UL) <= 0UL ? (uint) value : throw new ArgumentOutOfRangeException(nameof (value));
        this.known |= ZipEntry.Known.Crc;
      }
    }

    public CompressionMethod CompressionMethod
    {
      get => this.method;
      set => this.method = ZipEntry.IsCompressionMethodSupported(value) ? value : throw new NotSupportedException("Compression method not supported");
    }

    internal CompressionMethod CompressionMethodForHeader => this.AESKeySize > 0 ? CompressionMethod.WinZipAES : this.method;

    public byte[] ExtraData
    {
      get => this.extra;
      set
      {
        if (value == null)
        {
          this.extra = (byte[]) null;
        }
        else
        {
          this.extra = value.Length <= (int) ushort.MaxValue ? new byte[value.Length] : throw new ArgumentOutOfRangeException(nameof (value));
          Array.Copy((Array) value, 0, (Array) this.extra, 0, value.Length);
        }
      }
    }

    public int AESKeySize
    {
      get
      {
        switch (this._aesEncryptionStrength)
        {
          case 0:
            return 0;
          case 1:
            return 128;
          case 2:
            return 192;
          case 3:
            return 256;
          default:
            throw new ZipException("Invalid AESEncryptionStrength " + (object) this._aesEncryptionStrength);
        }
      }
      set
      {
        switch (value)
        {
          case 0:
            this._aesEncryptionStrength = 0;
            break;
          case 128:
            this._aesEncryptionStrength = 1;
            break;
          case 256:
            this._aesEncryptionStrength = 3;
            break;
          default:
            throw new ZipException("AESKeySize must be 0, 128 or 256: " + (object) value);
        }
      }
    }

    internal byte AESEncryptionStrength => (byte) this._aesEncryptionStrength;

    internal int AESSaltLen => this.AESKeySize / 16;

    internal int AESOverheadSize => 12 + this.AESSaltLen;

    internal void ProcessExtraData(bool localHeader)
    {
      ZipExtraData extraData = new ZipExtraData(this.extra);
      if (extraData.Find(1))
      {
        this.forceZip64_ = true;
        if (extraData.ValueLength < 4)
          throw new ZipException("Extra data extended Zip64 information length is invalid");
        if (localHeader || this.size == (ulong) uint.MaxValue)
          this.size = (ulong) extraData.ReadLong();
        if (localHeader || this.compressedSize == (ulong) uint.MaxValue)
          this.compressedSize = (ulong) extraData.ReadLong();
        if (!localHeader && this.offset == (long) uint.MaxValue)
          this.offset = extraData.ReadLong();
      }
      else if (((int) this.versionToExtract & (int) byte.MaxValue) >= 45 && (this.size == (ulong) uint.MaxValue || this.compressedSize == (ulong) uint.MaxValue))
        throw new ZipException("Zip64 Extended information required but is missing.");
      if (extraData.Find(10))
      {
        if (extraData.ValueLength < 4)
          throw new ZipException("NTFS Extra data invalid");
        extraData.ReadInt();
        while (extraData.UnreadCount >= 4)
        {
          int num = extraData.ReadShort();
          int amount = extraData.ReadShort();
          if (num == 1)
          {
            if (amount >= 24)
            {
              long fileTime = extraData.ReadLong();
              extraData.ReadLong();
              extraData.ReadLong();
              this.DateTime = DateTime.FromFileTime(fileTime);
              break;
            }
            break;
          }
          extraData.Skip(amount);
        }
      }
      else if (extraData.Find(21589))
      {
        int valueLength = extraData.ValueLength;
        if ((extraData.ReadByte() & 1) != 0 && valueLength >= 5)
        {
          int seconds = extraData.ReadInt();
          this.DateTime = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
        }
      }
      if (this.method != CompressionMethod.WinZipAES)
        return;
      this.ProcessAESExtraData(extraData);
    }

    private void ProcessAESExtraData(ZipExtraData extraData)
    {
      if (!extraData.Find(39169))
        throw new ZipException("AES Extra Data missing");
      this.versionToExtract = (ushort) 51;
      this.Flags |= 64;
      int valueLength = extraData.ValueLength;
      if (valueLength < 7)
        throw new ZipException("AES Extra Data Length " + (object) valueLength + " invalid.");
      int num1 = extraData.ReadShort();
      extraData.ReadShort();
      int num2 = extraData.ReadByte();
      int num3 = extraData.ReadShort();
      this._aesVer = num1;
      this._aesEncryptionStrength = num2;
      this.method = (CompressionMethod) num3;
    }

    public string Comment
    {
      get => this.comment;
      set => this.comment = value == null || value.Length <= (int) ushort.MaxValue ? value : throw new ArgumentOutOfRangeException(nameof (value), "cannot exceed 65535");
    }

    public bool IsDirectory
    {
      get
      {
        int length = this.name.Length;
        return length > 0 && (this.name[length - 1] == '/' || this.name[length - 1] == '\\') || this.HasDosAttributes(16);
      }
    }

    public bool IsFile => !this.IsDirectory && !this.HasDosAttributes(8);

    public bool IsCompressionMethodSupported() => ZipEntry.IsCompressionMethodSupported(this.CompressionMethod);

    public object Clone()
    {
      ZipEntry zipEntry = (ZipEntry) this.MemberwiseClone();
      if (this.extra != null)
      {
        zipEntry.extra = new byte[this.extra.Length];
        Array.Copy((Array) this.extra, 0, (Array) zipEntry.extra, 0, this.extra.Length);
      }
      return (object) zipEntry;
    }

    public override string ToString() => this.name;

    public static bool IsCompressionMethodSupported(CompressionMethod method) => method == CompressionMethod.Deflated || method == CompressionMethod.Stored;

    public static string CleanName(string name)
    {
      if (name == null)
        return string.Empty;
      if (Path.IsPathRooted(name))
        name = name.Substring(Path.GetPathRoot(name).Length);
      name = name.Replace("\\", "/");
      while (name.Length > 0 && name[0] == '/')
        name = name.Remove(0, 1);
      return name;
    }

    [System.Flags]
    private enum Known : byte
    {
      None = 0,
      Size = 1,
      CompressedSize = 2,
      Crc = 4,
      Time = 8,
      ExternalAttributes = 16, // 0x10
    }
  }
}
