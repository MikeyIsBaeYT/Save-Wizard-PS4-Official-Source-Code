// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipEntry
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Crc;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d00004")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  public class ZipEntry
  {
    private short _VersionMadeBy;
    private short _InternalFileAttrs;
    private int _ExternalFileAttrs;
    private short _filenameLength;
    private short _extraFieldLength;
    private short _commentLength;
    private ZipCrypto _zipCrypto_forExtract;
    private ZipCrypto _zipCrypto_forWrite;
    internal DateTime _LastModified;
    private DateTime _Mtime;
    private DateTime _Atime;
    private DateTime _Ctime;
    private bool _ntfsTimesAreSet;
    private bool _emitNtfsTimes = true;
    private bool _emitUnixTimes;
    private bool _TrimVolumeFromFullyQualifiedPaths = true;
    internal string _LocalFileName;
    private string _FileNameInArchive;
    internal short _VersionNeeded;
    internal short _BitField;
    internal short _CompressionMethod;
    private short _CompressionMethod_FromZipFile;
    private CompressionLevel _CompressionLevel;
    internal string _Comment;
    private bool _IsDirectory;
    private byte[] _CommentBytes;
    internal long _CompressedSize;
    internal long _CompressedFileDataSize;
    internal long _UncompressedSize;
    internal int _TimeBlob;
    private bool _crcCalculated;
    internal int _Crc32;
    internal byte[] _Extra;
    private bool _metadataChanged;
    private bool _restreamRequiredOnSave;
    private bool _sourceIsEncrypted;
    private bool _skippedDuringSave;
    private uint _diskNumber;
    private static Encoding ibm437 = Encoding.GetEncoding("IBM437");
    private Encoding _actualEncoding;
    internal ZipContainer _container;
    private long __FileDataPosition = -1;
    private byte[] _EntryHeader;
    internal long _RelativeOffsetOfLocalHeader;
    private long _future_ROLH;
    private long _TotalEntrySize;
    private int _LengthOfHeader;
    private int _LengthOfTrailer;
    internal bool _InputUsesZip64;
    private uint _UnsupportedAlgorithmId;
    internal string _Password;
    internal ZipEntrySource _Source;
    internal EncryptionAlgorithm _Encryption;
    internal EncryptionAlgorithm _Encryption_FromZipFile;
    internal byte[] _WeakEncryptionHeader;
    internal Stream _archiveStream;
    private Stream _sourceStream;
    private long? _sourceStreamOriginalPosition;
    private bool _sourceWasJitProvided;
    private bool _ioOperationCanceled;
    private bool _presumeZip64;
    private bool? _entryRequiresZip64;
    private bool? _OutputUsesZip64;
    private bool _IsText;
    private ZipEntryTimestamp _timestamp;
    private static DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static DateTime _win32Epoch = DateTime.FromFileTimeUtc(0L);
    private static DateTime _zeroHour = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private WriteDelegate _WriteDelegate;
    private OpenDelegate _OpenDelegate;
    private CloseDelegate _CloseDelegate;
    private Stream _inputDecryptorStream;
    private int _readExtraDepth;
    private object _outputLock = new object();

    internal bool AttributesIndicateDirectory => this._InternalFileAttrs == (short) 0 && (this._ExternalFileAttrs & 16) == 16;

    internal void ResetDirEntry()
    {
      this.__FileDataPosition = -1L;
      this._LengthOfHeader = 0;
    }

    public string Info
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(string.Format("          ZipEntry: {0}\n", (object) this.FileName)).Append(string.Format("   Version Made By: {0}\n", (object) this._VersionMadeBy)).Append(string.Format(" Needed to extract: {0}\n", (object) this.VersionNeeded));
        if (this._IsDirectory)
          stringBuilder.Append("        Entry type: directory\n");
        else
          stringBuilder.Append(string.Format("         File type: {0}\n", this._IsText ? (object) "text" : (object) "binary")).Append(string.Format("       Compression: {0}\n", (object) this.CompressionMethod)).Append(string.Format("        Compressed: 0x{0:X}\n", (object) this.CompressedSize)).Append(string.Format("      Uncompressed: 0x{0:X}\n", (object) this.UncompressedSize)).Append(string.Format("             CRC32: 0x{0:X8}\n", (object) this._Crc32));
        stringBuilder.Append(string.Format("       Disk Number: {0}\n", (object) this._diskNumber));
        if (this._RelativeOffsetOfLocalHeader > (long) uint.MaxValue)
          stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X16}\n", (object) this._RelativeOffsetOfLocalHeader));
        else
          stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X8}\n", (object) this._RelativeOffsetOfLocalHeader));
        stringBuilder.Append(string.Format("         Bit Field: 0x{0:X4}\n", (object) this._BitField)).Append(string.Format("        Encrypted?: {0}\n", (object) this._sourceIsEncrypted)).Append(string.Format("          Timeblob: 0x{0:X8}\n", (object) this._TimeBlob)).Append(string.Format("              Time: {0}\n", (object) SharedUtilities.PackedToDateTime(this._TimeBlob)));
        stringBuilder.Append(string.Format("         Is Zip64?: {0}\n", (object) this._InputUsesZip64));
        if (!string.IsNullOrEmpty(this._Comment))
          stringBuilder.Append(string.Format("           Comment: {0}\n", (object) this._Comment));
        stringBuilder.Append("\n");
        return stringBuilder.ToString();
      }
    }

    internal static ZipEntry ReadDirEntry(
      ZipFile zf,
      Dictionary<string, object> previouslySeen)
    {
      Stream readStream = zf.ReadStream;
      Encoding encoding = zf.AlternateEncodingUsage == ZipOption.Always ? zf.AlternateEncoding : ZipFile.DefaultEncoding;
      int signature = SharedUtilities.ReadSignature(readStream);
      if (ZipEntry.IsNotValidZipDirEntrySig(signature))
      {
        readStream.Seek(-4L, SeekOrigin.Current);
        if (signature != 101010256 && signature != 101075792 && signature != 67324752)
          throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position 0x{1:X8}", (object) signature, (object) readStream.Position));
        return (ZipEntry) null;
      }
      int num1 = 46;
      byte[] buffer = new byte[42];
      if (readStream.Read(buffer, 0, buffer.Length) != buffer.Length)
        return (ZipEntry) null;
      int num2 = 0;
      ZipEntry zipEntry1 = new ZipEntry();
      zipEntry1.AlternateEncoding = encoding;
      zipEntry1._Source = ZipEntrySource.ZipFile;
      zipEntry1._container = new ZipContainer((object) zf);
      ZipEntry zipEntry2 = zipEntry1;
      byte[] numArray1 = buffer;
      int index1 = num2;
      int num3 = index1 + 1;
      int num4 = (int) numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num3;
      int num5 = index2 + 1;
      int num6 = (int) numArray2[index2] * 256;
      int num7 = (int) (short) (num4 + num6);
      zipEntry2._VersionMadeBy = (short) num7;
      ZipEntry zipEntry3 = zipEntry1;
      byte[] numArray3 = buffer;
      int index3 = num5;
      int num8 = index3 + 1;
      int num9 = (int) numArray3[index3];
      byte[] numArray4 = buffer;
      int index4 = num8;
      int num10 = index4 + 1;
      int num11 = (int) numArray4[index4] * 256;
      int num12 = (int) (short) (num9 + num11);
      zipEntry3._VersionNeeded = (short) num12;
      ZipEntry zipEntry4 = zipEntry1;
      byte[] numArray5 = buffer;
      int index5 = num10;
      int num13 = index5 + 1;
      int num14 = (int) numArray5[index5];
      byte[] numArray6 = buffer;
      int index6 = num13;
      int num15 = index6 + 1;
      int num16 = (int) numArray6[index6] * 256;
      int num17 = (int) (short) (num14 + num16);
      zipEntry4._BitField = (short) num17;
      ZipEntry zipEntry5 = zipEntry1;
      byte[] numArray7 = buffer;
      int index7 = num15;
      int num18 = index7 + 1;
      int num19 = (int) numArray7[index7];
      byte[] numArray8 = buffer;
      int index8 = num18;
      int num20 = index8 + 1;
      int num21 = (int) numArray8[index8] * 256;
      int num22 = (int) (short) (num19 + num21);
      zipEntry5._CompressionMethod = (short) num22;
      ZipEntry zipEntry6 = zipEntry1;
      byte[] numArray9 = buffer;
      int index9 = num20;
      int num23 = index9 + 1;
      int num24 = (int) numArray9[index9];
      byte[] numArray10 = buffer;
      int index10 = num23;
      int num25 = index10 + 1;
      int num26 = (int) numArray10[index10] * 256;
      int num27 = num24 + num26;
      byte[] numArray11 = buffer;
      int index11 = num25;
      int num28 = index11 + 1;
      int num29 = (int) numArray11[index11] * 256 * 256;
      int num30 = num27 + num29;
      byte[] numArray12 = buffer;
      int index12 = num28;
      int num31 = index12 + 1;
      int num32 = (int) numArray12[index12] * 256 * 256 * 256;
      int num33 = num30 + num32;
      zipEntry6._TimeBlob = num33;
      zipEntry1._LastModified = SharedUtilities.PackedToDateTime(zipEntry1._TimeBlob);
      zipEntry1._timestamp |= ZipEntryTimestamp.DOS;
      ZipEntry zipEntry7 = zipEntry1;
      byte[] numArray13 = buffer;
      int index13 = num31;
      int num34 = index13 + 1;
      int num35 = (int) numArray13[index13];
      byte[] numArray14 = buffer;
      int index14 = num34;
      int num36 = index14 + 1;
      int num37 = (int) numArray14[index14] * 256;
      int num38 = num35 + num37;
      byte[] numArray15 = buffer;
      int index15 = num36;
      int num39 = index15 + 1;
      int num40 = (int) numArray15[index15] * 256 * 256;
      int num41 = num38 + num40;
      byte[] numArray16 = buffer;
      int index16 = num39;
      int num42 = index16 + 1;
      int num43 = (int) numArray16[index16] * 256 * 256 * 256;
      int num44 = num41 + num43;
      zipEntry7._Crc32 = num44;
      ZipEntry zipEntry8 = zipEntry1;
      byte[] numArray17 = buffer;
      int index17 = num42;
      int num45 = index17 + 1;
      int num46 = (int) numArray17[index17];
      byte[] numArray18 = buffer;
      int index18 = num45;
      int num47 = index18 + 1;
      int num48 = (int) numArray18[index18] * 256;
      int num49 = num46 + num48;
      byte[] numArray19 = buffer;
      int index19 = num47;
      int num50 = index19 + 1;
      int num51 = (int) numArray19[index19] * 256 * 256;
      int num52 = num49 + num51;
      byte[] numArray20 = buffer;
      int index20 = num50;
      int num53 = index20 + 1;
      int num54 = (int) numArray20[index20] * 256 * 256 * 256;
      long num55 = (long) (uint) (num52 + num54);
      zipEntry8._CompressedSize = num55;
      ZipEntry zipEntry9 = zipEntry1;
      byte[] numArray21 = buffer;
      int index21 = num53;
      int num56 = index21 + 1;
      int num57 = (int) numArray21[index21];
      byte[] numArray22 = buffer;
      int index22 = num56;
      int num58 = index22 + 1;
      int num59 = (int) numArray22[index22] * 256;
      int num60 = num57 + num59;
      byte[] numArray23 = buffer;
      int index23 = num58;
      int num61 = index23 + 1;
      int num62 = (int) numArray23[index23] * 256 * 256;
      int num63 = num60 + num62;
      byte[] numArray24 = buffer;
      int index24 = num61;
      int num64 = index24 + 1;
      int num65 = (int) numArray24[index24] * 256 * 256 * 256;
      long num66 = (long) (uint) (num63 + num65);
      zipEntry9._UncompressedSize = num66;
      zipEntry1._CompressionMethod_FromZipFile = zipEntry1._CompressionMethod;
      ZipEntry zipEntry10 = zipEntry1;
      byte[] numArray25 = buffer;
      int index25 = num64;
      int num67 = index25 + 1;
      int num68 = (int) numArray25[index25];
      byte[] numArray26 = buffer;
      int index26 = num67;
      int num69 = index26 + 1;
      int num70 = (int) numArray26[index26] * 256;
      int num71 = (int) (short) (num68 + num70);
      zipEntry10._filenameLength = (short) num71;
      ZipEntry zipEntry11 = zipEntry1;
      byte[] numArray27 = buffer;
      int index27 = num69;
      int num72 = index27 + 1;
      int num73 = (int) numArray27[index27];
      byte[] numArray28 = buffer;
      int index28 = num72;
      int num74 = index28 + 1;
      int num75 = (int) numArray28[index28] * 256;
      int num76 = (int) (short) (num73 + num75);
      zipEntry11._extraFieldLength = (short) num76;
      ZipEntry zipEntry12 = zipEntry1;
      byte[] numArray29 = buffer;
      int index29 = num74;
      int num77 = index29 + 1;
      int num78 = (int) numArray29[index29];
      byte[] numArray30 = buffer;
      int index30 = num77;
      int num79 = index30 + 1;
      int num80 = (int) numArray30[index30] * 256;
      int num81 = (int) (short) (num78 + num80);
      zipEntry12._commentLength = (short) num81;
      ZipEntry zipEntry13 = zipEntry1;
      byte[] numArray31 = buffer;
      int index31 = num79;
      int num82 = index31 + 1;
      int num83 = (int) numArray31[index31];
      byte[] numArray32 = buffer;
      int index32 = num82;
      int num84 = index32 + 1;
      int num85 = (int) numArray32[index32] * 256;
      int num86 = num83 + num85;
      zipEntry13._diskNumber = (uint) num86;
      ZipEntry zipEntry14 = zipEntry1;
      byte[] numArray33 = buffer;
      int index33 = num84;
      int num87 = index33 + 1;
      int num88 = (int) numArray33[index33];
      byte[] numArray34 = buffer;
      int index34 = num87;
      int num89 = index34 + 1;
      int num90 = (int) numArray34[index34] * 256;
      int num91 = (int) (short) (num88 + num90);
      zipEntry14._InternalFileAttrs = (short) num91;
      ZipEntry zipEntry15 = zipEntry1;
      byte[] numArray35 = buffer;
      int index35 = num89;
      int num92 = index35 + 1;
      int num93 = (int) numArray35[index35];
      byte[] numArray36 = buffer;
      int index36 = num92;
      int num94 = index36 + 1;
      int num95 = (int) numArray36[index36] * 256;
      int num96 = num93 + num95;
      byte[] numArray37 = buffer;
      int index37 = num94;
      int num97 = index37 + 1;
      int num98 = (int) numArray37[index37] * 256 * 256;
      int num99 = num96 + num98;
      byte[] numArray38 = buffer;
      int index38 = num97;
      int num100 = index38 + 1;
      int num101 = (int) numArray38[index38] * 256 * 256 * 256;
      int num102 = num99 + num101;
      zipEntry15._ExternalFileAttrs = num102;
      ZipEntry zipEntry16 = zipEntry1;
      byte[] numArray39 = buffer;
      int index39 = num100;
      int num103 = index39 + 1;
      int num104 = (int) numArray39[index39];
      byte[] numArray40 = buffer;
      int index40 = num103;
      int num105 = index40 + 1;
      int num106 = (int) numArray40[index40] * 256;
      int num107 = num104 + num106;
      byte[] numArray41 = buffer;
      int index41 = num105;
      int num108 = index41 + 1;
      int num109 = (int) numArray41[index41] * 256 * 256;
      int num110 = num107 + num109;
      byte[] numArray42 = buffer;
      int index42 = num108;
      int num111 = index42 + 1;
      int num112 = (int) numArray42[index42] * 256 * 256 * 256;
      long num113 = (long) (uint) (num110 + num112);
      zipEntry16._RelativeOffsetOfLocalHeader = num113;
      zipEntry1.IsText = ((int) zipEntry1._InternalFileAttrs & 1) == 1;
      byte[] numArray43 = new byte[(int) zipEntry1._filenameLength];
      int num114 = readStream.Read(numArray43, 0, numArray43.Length);
      int num115 = num1 + num114;
      zipEntry1._FileNameInArchive = ((int) zipEntry1._BitField & 2048) != 2048 ? SharedUtilities.StringFromBuffer(numArray43, encoding) : SharedUtilities.Utf8StringFromBuffer(numArray43);
      while (previouslySeen.ContainsKey(zipEntry1._FileNameInArchive))
      {
        zipEntry1._FileNameInArchive = ZipEntry.CopyHelper.AppendCopyToFileName(zipEntry1._FileNameInArchive);
        zipEntry1._metadataChanged = true;
      }
      if (zipEntry1.AttributesIndicateDirectory)
        zipEntry1.MarkAsDirectory();
      else if (zipEntry1._FileNameInArchive.EndsWith("/"))
        zipEntry1.MarkAsDirectory();
      zipEntry1._CompressedFileDataSize = zipEntry1._CompressedSize;
      if (((int) zipEntry1._BitField & 1) == 1)
      {
        zipEntry1._Encryption_FromZipFile = zipEntry1._Encryption = EncryptionAlgorithm.PkzipWeak;
        zipEntry1._sourceIsEncrypted = true;
      }
      if (zipEntry1._extraFieldLength > (short) 0)
      {
        zipEntry1._InputUsesZip64 = zipEntry1._CompressedSize == (long) uint.MaxValue || zipEntry1._UncompressedSize == (long) uint.MaxValue || zipEntry1._RelativeOffsetOfLocalHeader == (long) uint.MaxValue;
        num115 += zipEntry1.ProcessExtraField(readStream, zipEntry1._extraFieldLength);
        zipEntry1._CompressedFileDataSize = zipEntry1._CompressedSize;
      }
      if (zipEntry1._Encryption == EncryptionAlgorithm.PkzipWeak)
        zipEntry1._CompressedFileDataSize -= 12L;
      if (((int) zipEntry1._BitField & 8) == 8)
      {
        if (zipEntry1._InputUsesZip64)
          zipEntry1._LengthOfTrailer += 24;
        else
          zipEntry1._LengthOfTrailer += 16;
      }
      zipEntry1.AlternateEncoding = ((int) zipEntry1._BitField & 2048) == 2048 ? Encoding.UTF8 : encoding;
      zipEntry1.AlternateEncodingUsage = ZipOption.Always;
      if (zipEntry1._commentLength > (short) 0)
      {
        byte[] numArray44 = new byte[(int) zipEntry1._commentLength];
        int num116 = readStream.Read(numArray44, 0, numArray44.Length);
        int num117 = num115 + num116;
        zipEntry1._Comment = ((int) zipEntry1._BitField & 2048) != 2048 ? SharedUtilities.StringFromBuffer(numArray44, encoding) : SharedUtilities.Utf8StringFromBuffer(numArray44);
      }
      return zipEntry1;
    }

    internal static bool IsNotValidZipDirEntrySig(int signature) => signature != 33639248;

    public ZipEntry()
    {
      this._CompressionMethod = (short) 8;
      this._CompressionLevel = CompressionLevel.Default;
      this._Encryption = EncryptionAlgorithm.None;
      this._Source = ZipEntrySource.None;
      this.AlternateEncoding = Encoding.GetEncoding("IBM437");
      this.AlternateEncodingUsage = ZipOption.Default;
    }

    public DateTime LastModified
    {
      get => this._LastModified.ToLocalTime();
      set
      {
        this._LastModified = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Local) : value.ToLocalTime();
        this._Mtime = SharedUtilities.AdjustTime_Reverse(this._LastModified).ToUniversalTime();
        this._metadataChanged = true;
      }
    }

    private int BufferSize => this._container.BufferSize;

    public DateTime ModifiedTime
    {
      get => this._Mtime;
      set => this.SetEntryTimes(this._Ctime, this._Atime, value);
    }

    public DateTime AccessedTime
    {
      get => this._Atime;
      set => this.SetEntryTimes(this._Ctime, value, this._Mtime);
    }

    public DateTime CreationTime
    {
      get => this._Ctime;
      set => this.SetEntryTimes(value, this._Atime, this._Mtime);
    }

    public void SetEntryTimes(DateTime created, DateTime accessed, DateTime modified)
    {
      this._ntfsTimesAreSet = true;
      if (created == ZipEntry._zeroHour && created.Kind == ZipEntry._zeroHour.Kind)
        created = ZipEntry._win32Epoch;
      if (accessed == ZipEntry._zeroHour && accessed.Kind == ZipEntry._zeroHour.Kind)
        accessed = ZipEntry._win32Epoch;
      if (modified == ZipEntry._zeroHour && modified.Kind == ZipEntry._zeroHour.Kind)
        modified = ZipEntry._win32Epoch;
      this._Ctime = created.ToUniversalTime();
      this._Atime = accessed.ToUniversalTime();
      this._Mtime = modified.ToUniversalTime();
      this._LastModified = this._Mtime;
      if (!this._emitUnixTimes && !this._emitNtfsTimes)
        this._emitNtfsTimes = true;
      this._metadataChanged = true;
    }

    public bool EmitTimesInWindowsFormatWhenSaving
    {
      get => this._emitNtfsTimes;
      set
      {
        this._emitNtfsTimes = value;
        this._metadataChanged = true;
      }
    }

    public bool EmitTimesInUnixFormatWhenSaving
    {
      get => this._emitUnixTimes;
      set
      {
        this._emitUnixTimes = value;
        this._metadataChanged = true;
      }
    }

    public ZipEntryTimestamp Timestamp => this._timestamp;

    public FileAttributes Attributes
    {
      get => (FileAttributes) this._ExternalFileAttrs;
      set
      {
        this._ExternalFileAttrs = (int) value;
        this._VersionMadeBy = (short) 45;
        this._metadataChanged = true;
      }
    }

    internal string LocalFileName => this._LocalFileName;

    public string FileName
    {
      get => this._FileNameInArchive;
      set
      {
        if (this._container.ZipFile == null)
          throw new ZipException("Cannot rename; this is not supported in ZipOutputStream/ZipInputStream.");
        string name = !string.IsNullOrEmpty(value) ? ZipEntry.NameInArchive(value, (string) null) : throw new ZipException("The FileName must be non empty and non-null.");
        if (this._FileNameInArchive == name)
          return;
        this._container.ZipFile.RemoveEntry(this);
        this._container.ZipFile.InternalAddEntry(name, this);
        this._FileNameInArchive = name;
        this._container.ZipFile.NotifyEntryChanged();
        this._metadataChanged = true;
      }
    }

    public Stream InputStream
    {
      get => this._sourceStream;
      set
      {
        if (this._Source != ZipEntrySource.Stream)
          throw new ZipException("You must not set the input stream for this entry.");
        this._sourceWasJitProvided = true;
        this._sourceStream = value;
      }
    }

    public bool InputStreamWasJitProvided => this._sourceWasJitProvided;

    public ZipEntrySource Source => this._Source;

    public short VersionNeeded => this._VersionNeeded;

    public string Comment
    {
      get => this._Comment;
      set
      {
        this._Comment = value;
        this._metadataChanged = true;
      }
    }

    public bool? RequiresZip64 => this._entryRequiresZip64;

    public bool? OutputUsedZip64 => this._OutputUsesZip64;

    public short BitField => this._BitField;

    public CompressionMethod CompressionMethod
    {
      get => (CompressionMethod) this._CompressionMethod;
      set
      {
        if (value == (CompressionMethod) this._CompressionMethod)
          return;
        this._CompressionMethod = value == CompressionMethod.None || value == CompressionMethod.Deflate ? (short) value : throw new InvalidOperationException("Unsupported compression method.");
        if (this._CompressionMethod == (short) 0)
          this._CompressionLevel = CompressionLevel.None;
        else if (this.CompressionLevel == CompressionLevel.None)
          this._CompressionLevel = CompressionLevel.Default;
        if (this._container.ZipFile != null)
          this._container.ZipFile.NotifyEntryChanged();
        this._restreamRequiredOnSave = true;
      }
    }

    public CompressionLevel CompressionLevel
    {
      get => this._CompressionLevel;
      set
      {
        if (this._CompressionMethod != (short) 8 && (uint) this._CompressionMethod > 0U || value == CompressionLevel.Default && this._CompressionMethod == (short) 8)
          return;
        this._CompressionLevel = value;
        if (value == CompressionLevel.None && this._CompressionMethod == (short) 0)
          return;
        this._CompressionMethod = this._CompressionLevel != CompressionLevel.None ? (short) 8 : (short) 0;
        if (this._container.ZipFile != null)
          this._container.ZipFile.NotifyEntryChanged();
        this._restreamRequiredOnSave = true;
      }
    }

    public long CompressedSize => this._CompressedSize;

    public long UncompressedSize => this._UncompressedSize;

    public double CompressionRatio => this.UncompressedSize == 0L ? 0.0 : 100.0 * (1.0 - 1.0 * (double) this.CompressedSize / (1.0 * (double) this.UncompressedSize));

    public int Crc => this._Crc32;

    public bool IsDirectory => this._IsDirectory;

    public bool UsesEncryption => (uint) this._Encryption_FromZipFile > 0U;

    public EncryptionAlgorithm Encryption
    {
      get => this._Encryption;
      set
      {
        if (value == this._Encryption)
          return;
        this._Encryption = value != EncryptionAlgorithm.Unsupported ? value : throw new InvalidOperationException("You may not set Encryption to that value.");
        this._restreamRequiredOnSave = true;
        if (this._container.ZipFile == null)
          return;
        this._container.ZipFile.NotifyEntryChanged();
      }
    }

    public string Password
    {
      set
      {
        this._Password = value;
        if (this._Password == null)
        {
          this._Encryption = EncryptionAlgorithm.None;
        }
        else
        {
          if (this._Source == ZipEntrySource.ZipFile && !this._sourceIsEncrypted)
            this._restreamRequiredOnSave = true;
          if (this.Encryption == EncryptionAlgorithm.None)
            this._Encryption = EncryptionAlgorithm.PkzipWeak;
        }
      }
      private get => this._Password;
    }

    internal bool IsChanged => this._restreamRequiredOnSave | this._metadataChanged;

    public ExtractExistingFileAction ExtractExistingFile { get; set; }

    public ZipErrorAction ZipErrorAction { get; set; }

    public bool IncludedInMostRecentSave => !this._skippedDuringSave;

    public SetCompressionCallback SetCompression { get; set; }

    [Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
    public bool UseUnicodeAsNecessary
    {
      get => this.AlternateEncoding == Encoding.GetEncoding("UTF-8") && this.AlternateEncodingUsage == ZipOption.AsNecessary;
      set
      {
        if (value)
        {
          this.AlternateEncoding = Encoding.GetEncoding("UTF-8");
          this.AlternateEncodingUsage = ZipOption.AsNecessary;
        }
        else
        {
          this.AlternateEncoding = ZipFile.DefaultEncoding;
          this.AlternateEncodingUsage = ZipOption.Default;
        }
      }
    }

    [Obsolete("This property is obsolete since v1.9.1.6. Use AlternateEncoding and AlternateEncodingUsage instead.", true)]
    public Encoding ProvisionalAlternateEncoding { get; set; }

    public Encoding AlternateEncoding { get; set; }

    public ZipOption AlternateEncodingUsage { get; set; }

    internal static string NameInArchive(string filename, string directoryPathInArchive) => SharedUtilities.NormalizePathForUseInZipFile(directoryPathInArchive != null ? (!string.IsNullOrEmpty(directoryPathInArchive) ? Path.Combine(directoryPathInArchive, Path.GetFileName(filename)) : Path.GetFileName(filename)) : filename);

    internal static ZipEntry CreateFromNothing(string nameInArchive) => ZipEntry.Create(nameInArchive, ZipEntrySource.None, (object) null, (object) null);

    internal static ZipEntry CreateFromFile(string filename, string nameInArchive) => ZipEntry.Create(nameInArchive, ZipEntrySource.FileSystem, (object) filename, (object) null);

    internal static ZipEntry CreateForStream(string entryName, Stream s) => ZipEntry.Create(entryName, ZipEntrySource.Stream, (object) s, (object) null);

    internal static ZipEntry CreateForWriter(string entryName, WriteDelegate d) => ZipEntry.Create(entryName, ZipEntrySource.WriteDelegate, (object) d, (object) null);

    internal static ZipEntry CreateForJitStreamProvider(
      string nameInArchive,
      OpenDelegate opener,
      CloseDelegate closer)
    {
      return ZipEntry.Create(nameInArchive, ZipEntrySource.JitStream, (object) opener, (object) closer);
    }

    internal static ZipEntry CreateForZipOutputStream(string nameInArchive) => ZipEntry.Create(nameInArchive, ZipEntrySource.ZipOutputStream, (object) null, (object) null);

    private static ZipEntry Create(
      string nameInArchive,
      ZipEntrySource source,
      object arg1,
      object arg2)
    {
      if (string.IsNullOrEmpty(nameInArchive))
        throw new ZipException("The entry name must be non-null and non-empty.");
      ZipEntry zipEntry1 = new ZipEntry()
      {
        _VersionMadeBy = 45,
        _Source = source
      };
      zipEntry1._Mtime = zipEntry1._Atime = zipEntry1._Ctime = DateTime.UtcNow;
      switch (source)
      {
        case ZipEntrySource.None:
          zipEntry1._Source = ZipEntrySource.FileSystem;
          goto case ZipEntrySource.ZipOutputStream;
        case ZipEntrySource.Stream:
          zipEntry1._sourceStream = arg1 as Stream;
          goto case ZipEntrySource.ZipOutputStream;
        case ZipEntrySource.WriteDelegate:
          zipEntry1._WriteDelegate = arg1 as WriteDelegate;
          goto case ZipEntrySource.ZipOutputStream;
        case ZipEntrySource.JitStream:
          zipEntry1._OpenDelegate = arg1 as OpenDelegate;
          zipEntry1._CloseDelegate = arg2 as CloseDelegate;
          goto case ZipEntrySource.ZipOutputStream;
        case ZipEntrySource.ZipOutputStream:
          zipEntry1._LastModified = zipEntry1._Mtime;
          zipEntry1._FileNameInArchive = SharedUtilities.NormalizePathForUseInZipFile(nameInArchive);
          return zipEntry1;
        default:
          string path = arg1 as string;
          if (string.IsNullOrEmpty(path))
            throw new ZipException("The filename must be non-null and non-empty.");
          try
          {
            ZipEntry zipEntry2 = zipEntry1;
            DateTime dateTime = File.GetLastWriteTime(path);
            DateTime universalTime1 = dateTime.ToUniversalTime();
            zipEntry2._Mtime = universalTime1;
            ZipEntry zipEntry3 = zipEntry1;
            dateTime = File.GetCreationTime(path);
            DateTime universalTime2 = dateTime.ToUniversalTime();
            zipEntry3._Ctime = universalTime2;
            ZipEntry zipEntry4 = zipEntry1;
            dateTime = File.GetLastAccessTime(path);
            DateTime universalTime3 = dateTime.ToUniversalTime();
            zipEntry4._Atime = universalTime3;
            if (File.Exists(path) || Directory.Exists(path))
              zipEntry1._ExternalFileAttrs = (int) File.GetAttributes(path);
            zipEntry1._ntfsTimesAreSet = true;
            zipEntry1._LocalFileName = Path.GetFullPath(path);
            goto case ZipEntrySource.ZipOutputStream;
          }
          catch (PathTooLongException ex)
          {
            throw new ZipException(string.Format("The path is too long, filename={0}", (object) path), (Exception) ex);
          }
      }
    }

    internal void MarkAsDirectory()
    {
      this._IsDirectory = true;
      if (this._FileNameInArchive.EndsWith("/"))
        return;
      this._FileNameInArchive += "/";
    }

    public bool IsText
    {
      get => this._IsText;
      set => this._IsText = value;
    }

    public override string ToString() => string.Format("ZipEntry::{0}", (object) this.FileName);

    internal Stream ArchiveStream
    {
      get
      {
        if (this._archiveStream == null)
        {
          if (this._container.ZipFile != null)
          {
            ZipFile zipFile = this._container.ZipFile;
            zipFile.Reset(false);
            this._archiveStream = zipFile.StreamForDiskNumber(this._diskNumber);
          }
          else
            this._archiveStream = this._container.ZipOutputStream.OutputStream;
        }
        return this._archiveStream;
      }
    }

    private void SetFdpLoh()
    {
      long position = this.ArchiveStream.Position;
      try
      {
        this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
      }
      catch (IOException ex)
      {
        throw new BadStateException(string.Format("Exception seeking  entry({0}) offset(0x{1:X8}) len(0x{2:X8})", (object) this.FileName, (object) this._RelativeOffsetOfLocalHeader, (object) this.ArchiveStream.Length), (Exception) ex);
      }
      byte[] buffer = new byte[30];
      this.ArchiveStream.Read(buffer, 0, buffer.Length);
      short num1 = (short) ((int) buffer[26] + (int) buffer[27] * 256);
      short num2 = (short) ((int) buffer[28] + (int) buffer[29] * 256);
      this.ArchiveStream.Seek((long) ((int) num1 + (int) num2), SeekOrigin.Current);
      this._LengthOfHeader = 30 + (int) num2 + (int) num1 + ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
      this.__FileDataPosition = this._RelativeOffsetOfLocalHeader + (long) this._LengthOfHeader;
      this.ArchiveStream.Seek(position, SeekOrigin.Begin);
    }

    internal static int GetLengthOfCryptoHeaderBytes(EncryptionAlgorithm a)
    {
      if (a == EncryptionAlgorithm.None)
        return 0;
      if (a == EncryptionAlgorithm.PkzipWeak)
        return 12;
      throw new ZipException("internal error");
    }

    internal long FileDataPosition
    {
      get
      {
        if (this.__FileDataPosition == -1L)
          this.SetFdpLoh();
        return this.__FileDataPosition;
      }
    }

    private int LengthOfHeader
    {
      get
      {
        if (this._LengthOfHeader == 0)
          this.SetFdpLoh();
        return this._LengthOfHeader;
      }
    }

    public void Extract() => this.InternalExtract(".", (Stream) null, (string) null);

    public void Extract(ExtractExistingFileAction extractExistingFile)
    {
      this.ExtractExistingFile = extractExistingFile;
      this.InternalExtract(".", (Stream) null, (string) null);
    }

    public void Extract(Stream stream) => this.InternalExtract((string) null, stream, (string) null);

    public void Extract(string baseDirectory) => this.InternalExtract(baseDirectory, (Stream) null, (string) null);

    public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
    {
      this.ExtractExistingFile = extractExistingFile;
      this.InternalExtract(baseDirectory, (Stream) null, (string) null);
    }

    public void ExtractWithPassword(string password) => this.InternalExtract(".", (Stream) null, password);

    public void ExtractWithPassword(string baseDirectory, string password) => this.InternalExtract(baseDirectory, (Stream) null, password);

    public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
    {
      this.ExtractExistingFile = extractExistingFile;
      this.InternalExtract(".", (Stream) null, password);
    }

    public void ExtractWithPassword(
      string baseDirectory,
      ExtractExistingFileAction extractExistingFile,
      string password)
    {
      this.ExtractExistingFile = extractExistingFile;
      this.InternalExtract(baseDirectory, (Stream) null, password);
    }

    public void ExtractWithPassword(Stream stream, string password) => this.InternalExtract((string) null, stream, password);

    public CrcCalculatorStream OpenReader()
    {
      if (this._container.ZipFile == null)
        throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
      return this.InternalOpenReader(this._Password ?? this._container.Password);
    }

    public CrcCalculatorStream OpenReader(string password)
    {
      if (this._container.ZipFile == null)
        throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
      return this.InternalOpenReader(password);
    }

    internal CrcCalculatorStream InternalOpenReader(string password)
    {
      this.ValidateCompression();
      this.ValidateEncryption();
      this.SetupCryptoForExtract(password);
      if (this._Source != ZipEntrySource.ZipFile)
        throw new BadStateException("You must call ZipFile.Save before calling OpenReader");
      long length = this._CompressionMethod_FromZipFile == (short) 0 ? this._CompressedFileDataSize : this.UncompressedSize;
      Stream archiveStream = this.ArchiveStream;
      this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
      this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
      return new CrcCalculatorStream(this.GetExtractDecompressor(this._inputDecryptorStream), length);
    }

    private void OnExtractProgress(long bytesWritten, long totalBytesToWrite)
    {
      if (this._container.ZipFile == null)
        return;
      this._ioOperationCanceled = this._container.ZipFile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
    }

    private void OnBeforeExtract(string path)
    {
      if (this._container.ZipFile == null || this._container.ZipFile._inExtractAll)
        return;
      this._ioOperationCanceled = this._container.ZipFile.OnSingleEntryExtract(this, path, true);
    }

    private void OnAfterExtract(string path)
    {
      if (this._container.ZipFile == null || this._container.ZipFile._inExtractAll)
        return;
      this._container.ZipFile.OnSingleEntryExtract(this, path, false);
    }

    private void OnExtractExisting(string path)
    {
      if (this._container.ZipFile == null)
        return;
      this._ioOperationCanceled = this._container.ZipFile.OnExtractExisting(this, path);
    }

    private static void ReallyDelete(string fileName)
    {
      if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
        File.SetAttributes(fileName, FileAttributes.Normal);
      File.Delete(fileName);
    }

    private void WriteStatus(string format, params object[] args)
    {
      if (this._container.ZipFile == null || !this._container.ZipFile.Verbose)
        return;
      this._container.ZipFile.StatusMessageTextWriter.WriteLine(format, args);
    }

    private void InternalExtract(string baseDir, Stream outstream, string password)
    {
      if (this._container == null)
        throw new BadStateException("This entry is an orphan");
      if (this._container.ZipFile == null)
        throw new InvalidOperationException("Use Extract() only with ZipFile.");
      this._container.ZipFile.Reset(false);
      if (this._Source != ZipEntrySource.ZipFile)
        throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
      this.OnBeforeExtract(baseDir);
      this._ioOperationCanceled = false;
      string outFileName = (string) null;
      Stream output = (Stream) null;
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        this.ValidateCompression();
        this.ValidateEncryption();
        if (this.ValidateOutput(baseDir, outstream, out outFileName))
        {
          this.WriteStatus("extract dir {0}...", (object) outFileName);
          this.OnAfterExtract(baseDir);
        }
        else
        {
          if (outFileName != null && File.Exists(outFileName))
          {
            flag1 = true;
            switch (this.CheckExtractExistingFile(baseDir, outFileName))
            {
              case 1:
                return;
              case 2:
                return;
            }
          }
          string password1 = password ?? this._Password ?? this._container.Password;
          if ((uint) this._Encryption_FromZipFile > 0U)
          {
            if (password1 == null)
              throw new BadPasswordException();
            this.SetupCryptoForExtract(password1);
          }
          if (outFileName != null)
          {
            this.WriteStatus("extract file {0}...", (object) outFileName);
            outFileName += ".tmp";
            string directoryName = Path.GetDirectoryName(outFileName);
            if (!Directory.Exists(directoryName))
              Directory.CreateDirectory(directoryName);
            else if (this._container.ZipFile != null)
              flag2 = this._container.ZipFile._inExtractAll;
            output = (Stream) new FileStream(outFileName, FileMode.Create, FileAccess.ReadWrite);
          }
          else
          {
            this.WriteStatus("extract entry {0} to stream...", (object) this.FileName);
            output = outstream;
          }
          if (this._ioOperationCanceled)
            return;
          int one = this.ExtractOne(output);
          if (this._ioOperationCanceled)
            return;
          this.VerifyCrcAfterExtract(one);
          if (outFileName != null)
          {
            output.Close();
            output = (Stream) null;
            string sourceFileName = outFileName;
            string str = (string) null;
            outFileName = sourceFileName.Substring(0, sourceFileName.Length - 4);
            if (flag1)
            {
              str = outFileName + ".PendingOverwrite";
              File.Move(outFileName, str);
            }
            File.Move(sourceFileName, outFileName);
            this._SetTimes(outFileName, true);
            if (str != null && File.Exists(str))
              ZipEntry.ReallyDelete(str);
            if (flag2 && this.FileName.IndexOf('/') != -1 && this._container.ZipFile[Path.GetDirectoryName(this.FileName)] == null)
              this._SetTimes(Path.GetDirectoryName(outFileName), false);
            if (((int) this._VersionMadeBy & 65280) == 2560 || ((int) this._VersionMadeBy & 65280) == 0)
              File.SetAttributes(outFileName, (FileAttributes) this._ExternalFileAttrs);
          }
          this.OnAfterExtract(baseDir);
        }
      }
      catch (Exception ex)
      {
        this._ioOperationCanceled = true;
        throw;
      }
      finally
      {
        if (this._ioOperationCanceled)
        {
          if (outFileName != null)
          {
            try
            {
              output?.Close();
              if (File.Exists(outFileName) && !flag1)
                File.Delete(outFileName);
            }
            finally
            {
            }
          }
        }
      }
    }

    internal void VerifyCrcAfterExtract(int actualCrc32)
    {
      if (actualCrc32 != this._Crc32)
        throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + string.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", (object) this._Crc32, (object) actualCrc32));
    }

    private int CheckExtractExistingFile(string baseDir, string targetFileName)
    {
      int num = 0;
      while (true)
      {
        switch (this.ExtractExistingFile)
        {
          case ExtractExistingFileAction.OverwriteSilently:
            goto label_2;
          case ExtractExistingFileAction.DoNotOverwrite:
            goto label_3;
          case ExtractExistingFileAction.InvokeExtractProgressEvent:
            if (num <= 0)
            {
              this.OnExtractExisting(baseDir);
              if (!this._ioOperationCanceled)
              {
                ++num;
                continue;
              }
              goto label_7;
            }
            else
              goto label_5;
          default:
            goto label_8;
        }
      }
label_2:
      this.WriteStatus("the file {0} exists; will overwrite it...", (object) targetFileName);
      return 0;
label_3:
      this.WriteStatus("the file {0} exists; not extracting entry...", (object) this.FileName);
      this.OnAfterExtract(baseDir);
      return 1;
label_5:
      throw new ZipException(string.Format("The file {0} already exists.", (object) targetFileName));
label_7:
      return 2;
label_8:
      throw new ZipException(string.Format("The file {0} already exists.", (object) targetFileName));
    }

    private void _CheckRead(int nbytes)
    {
      if (nbytes == 0)
        throw new BadReadException(string.Format("bad read of entry {0} from compressed archive.", (object) this.FileName));
    }

    private int ExtractOne(Stream output)
    {
      int num1 = 0;
      Stream archiveStream = this.ArchiveStream;
      try
      {
        archiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
        byte[] buffer = new byte[this.BufferSize];
        long num2 = this._CompressionMethod_FromZipFile != (short) 0 ? this.UncompressedSize : this._CompressedFileDataSize;
        this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
        Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
        long bytesWritten = 0;
        using (CrcCalculatorStream calculatorStream = new CrcCalculatorStream(extractDecompressor))
        {
          while (num2 > 0L)
          {
            int count = num2 > (long) buffer.Length ? buffer.Length : (int) num2;
            int num3 = calculatorStream.Read(buffer, 0, count);
            this._CheckRead(num3);
            output.Write(buffer, 0, num3);
            num2 -= (long) num3;
            bytesWritten += (long) num3;
            this.OnExtractProgress(bytesWritten, this.UncompressedSize);
            if (this._ioOperationCanceled)
              break;
          }
          num1 = calculatorStream.Crc;
        }
      }
      finally
      {
        if (archiveStream is ZipSegmentedStream zipSegmentedStream2)
        {
          zipSegmentedStream2.Dispose();
          this._archiveStream = (Stream) null;
        }
      }
      return num1;
    }

    internal Stream GetExtractDecompressor(Stream input2)
    {
      switch (this._CompressionMethod_FromZipFile)
      {
        case 0:
          return input2;
        case 8:
          return (Stream) new DeflateStream(input2, CompressionMode.Decompress, true);
        default:
          return (Stream) null;
      }
    }

    internal Stream GetExtractDecryptor(Stream input) => this._Encryption_FromZipFile != EncryptionAlgorithm.PkzipWeak ? input : (Stream) new ZipCipherStream(input, this._zipCrypto_forExtract, CryptoMode.Decrypt);

    internal void _SetTimes(string fileOrDirectory, bool isFile)
    {
      try
      {
        if (this._ntfsTimesAreSet)
        {
          if (isFile)
          {
            if (!File.Exists(fileOrDirectory))
              return;
            File.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
            File.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
            File.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
          }
          else if (Directory.Exists(fileOrDirectory))
          {
            Directory.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
            Directory.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
            Directory.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
          }
        }
        else
        {
          DateTime lastWriteTime = SharedUtilities.AdjustTime_Reverse(this.LastModified);
          if (isFile)
            File.SetLastWriteTime(fileOrDirectory, lastWriteTime);
          else
            Directory.SetLastWriteTime(fileOrDirectory, lastWriteTime);
        }
      }
      catch (IOException ex)
      {
        this.WriteStatus("failed to set time on {0}: {1}", (object) fileOrDirectory, (object) ex.Message);
      }
    }

    private string UnsupportedAlgorithm
    {
      get
      {
        string empty = string.Empty;
        string str;
        switch (this._UnsupportedAlgorithmId)
        {
          case 0:
            str = "--";
            break;
          case 26113:
            str = "DES";
            break;
          case 26114:
            str = "RC2";
            break;
          case 26115:
            str = "3DES-168";
            break;
          case 26121:
            str = "3DES-112";
            break;
          case 26126:
            str = "PKWare AES128";
            break;
          case 26127:
            str = "PKWare AES192";
            break;
          case 26128:
            str = "PKWare AES256";
            break;
          case 26370:
            str = "RC2";
            break;
          case 26400:
            str = "Blowfish";
            break;
          case 26401:
            str = "Twofish";
            break;
          case 26625:
            str = "RC4";
            break;
          default:
            str = string.Format("Unknown (0x{0:X4})", (object) this._UnsupportedAlgorithmId);
            break;
        }
        return str;
      }
    }

    private string UnsupportedCompressionMethod
    {
      get
      {
        string empty = string.Empty;
        string str;
        switch (this._CompressionMethod)
        {
          case 0:
            str = "Store";
            break;
          case 1:
            str = "Shrink";
            break;
          case 8:
            str = "DEFLATE";
            break;
          case 9:
            str = "Deflate64";
            break;
          case 12:
            str = "BZIP2";
            break;
          case 14:
            str = "LZMA";
            break;
          case 19:
            str = "LZ77";
            break;
          case 98:
            str = "PPMd";
            break;
          default:
            str = string.Format("Unknown (0x{0:X4})", (object) this._CompressionMethod);
            break;
        }
        return str;
      }
    }

    internal void ValidateEncryption()
    {
      if (this.Encryption == EncryptionAlgorithm.PkzipWeak || (uint) this.Encryption <= 0U)
        return;
      if (this._UnsupportedAlgorithmId > 0U)
        throw new ZipException(string.Format("Cannot extract: Entry {0} is encrypted with an algorithm not supported by DotNetZip: {1}", (object) this.FileName, (object) this.UnsupportedAlgorithm));
      throw new ZipException(string.Format("Cannot extract: Entry {0} uses an unsupported encryption algorithm ({1:X2})", (object) this.FileName, (object) (int) this.Encryption));
    }

    private void ValidateCompression()
    {
      if (this._CompressionMethod_FromZipFile != (short) 0 && this._CompressionMethod_FromZipFile != (short) 8)
        throw new ZipException(string.Format("Entry {0} uses an unsupported compression method (0x{1:X2}, {2})", (object) this.FileName, (object) this._CompressionMethod_FromZipFile, (object) this.UnsupportedCompressionMethod));
    }

    private void SetupCryptoForExtract(string password)
    {
      if (this._Encryption_FromZipFile == EncryptionAlgorithm.None || this._Encryption_FromZipFile != EncryptionAlgorithm.PkzipWeak)
        return;
      if (password == null)
        throw new ZipException("Missing password.");
      this.ArchiveStream.Seek(this.FileDataPosition - 12L, SeekOrigin.Begin);
      this._zipCrypto_forExtract = ZipCrypto.ForRead(password, this);
    }

    private bool ValidateOutput(string basedir, Stream outstream, out string outFileName)
    {
      if (basedir != null)
      {
        string str = this.FileName.Replace("\\", "/");
        if (str.IndexOf(':') == 1)
          str = str.Substring(2);
        if (str.StartsWith("/"))
          str = str.Substring(1);
        outFileName = !this._container.ZipFile.FlattenFoldersOnExtract ? Path.Combine(basedir, str) : Path.Combine(basedir, str.IndexOf('/') != -1 ? Path.GetFileName(str) : str);
        if (!this.IsDirectory && !this.FileName.EndsWith("/"))
          return false;
        if (!Directory.Exists(outFileName))
        {
          Directory.CreateDirectory(outFileName);
          this._SetTimes(outFileName, false);
        }
        else if (this.ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
          this._SetTimes(outFileName, false);
        return true;
      }
      if (outstream == null)
        throw new ArgumentNullException(nameof (outstream));
      outFileName = (string) null;
      return this.IsDirectory || this.FileName.EndsWith("/");
    }

    private void ReadExtraField()
    {
      ++this._readExtraDepth;
      long position = this.ArchiveStream.Position;
      this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
      byte[] buffer = new byte[30];
      this.ArchiveStream.Read(buffer, 0, buffer.Length);
      int num1 = 26;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = (int) numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      int num5 = (int) numArray2[index2] * 256;
      short num6 = (short) (num3 + num5);
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num7 = index3 + 1;
      int num8 = (int) numArray3[index3];
      byte[] numArray4 = buffer;
      int index4 = num7;
      int num9 = index4 + 1;
      int num10 = (int) numArray4[index4] * 256;
      short extraFieldLength = (short) (num8 + num10);
      this.ArchiveStream.Seek((long) num6, SeekOrigin.Current);
      this.ProcessExtraField(this.ArchiveStream, extraFieldLength);
      this.ArchiveStream.Seek(position, SeekOrigin.Begin);
      --this._readExtraDepth;
    }

    private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
    {
      int num1 = 0;
      ze._RelativeOffsetOfLocalHeader = ze.ArchiveStream.Position;
      int signature1 = SharedUtilities.ReadEntrySignature(ze.ArchiveStream);
      int num2 = num1 + 4;
      if (ZipEntry.IsNotValidSig(signature1))
      {
        ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
        if (ZipEntry.IsNotValidZipDirEntrySig(signature1) && signature1 != 101010256)
          throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position  0x{1:X8}", (object) signature1, (object) ze.ArchiveStream.Position));
        return false;
      }
      byte[] buffer1 = new byte[26];
      int num3 = ze.ArchiveStream.Read(buffer1, 0, buffer1.Length);
      if (num3 != buffer1.Length)
        return false;
      int num4 = num2 + num3;
      int num5 = 0;
      ZipEntry zipEntry1 = ze;
      byte[] numArray1 = buffer1;
      int index1 = num5;
      int num6 = index1 + 1;
      int num7 = (int) numArray1[index1];
      byte[] numArray2 = buffer1;
      int index2 = num6;
      int num8 = index2 + 1;
      int num9 = (int) numArray2[index2] * 256;
      int num10 = (int) (short) (num7 + num9);
      zipEntry1._VersionNeeded = (short) num10;
      ZipEntry zipEntry2 = ze;
      byte[] numArray3 = buffer1;
      int index3 = num8;
      int num11 = index3 + 1;
      int num12 = (int) numArray3[index3];
      byte[] numArray4 = buffer1;
      int index4 = num11;
      int num13 = index4 + 1;
      int num14 = (int) numArray4[index4] * 256;
      int num15 = (int) (short) (num12 + num14);
      zipEntry2._BitField = (short) num15;
      ZipEntry zipEntry3 = ze;
      ZipEntry zipEntry4 = ze;
      byte[] numArray5 = buffer1;
      int index5 = num13;
      int num16 = index5 + 1;
      int num17 = (int) numArray5[index5];
      byte[] numArray6 = buffer1;
      int index6 = num16;
      int num18 = index6 + 1;
      int num19 = (int) numArray6[index6] * 256;
      int num20;
      short num21 = (short) (num20 = (int) (short) (num17 + num19));
      zipEntry4._CompressionMethod = (short) num20;
      int num22 = (int) num21;
      zipEntry3._CompressionMethod_FromZipFile = (short) num22;
      ZipEntry zipEntry5 = ze;
      byte[] numArray7 = buffer1;
      int index7 = num18;
      int num23 = index7 + 1;
      int num24 = (int) numArray7[index7];
      byte[] numArray8 = buffer1;
      int index8 = num23;
      int num25 = index8 + 1;
      int num26 = (int) numArray8[index8] * 256;
      int num27 = num24 + num26;
      byte[] numArray9 = buffer1;
      int index9 = num25;
      int num28 = index9 + 1;
      int num29 = (int) numArray9[index9] * 256 * 256;
      int num30 = num27 + num29;
      byte[] numArray10 = buffer1;
      int index10 = num28;
      int num31 = index10 + 1;
      int num32 = (int) numArray10[index10] * 256 * 256 * 256;
      int num33 = num30 + num32;
      zipEntry5._TimeBlob = num33;
      ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
      ze._timestamp |= ZipEntryTimestamp.DOS;
      if (((int) ze._BitField & 1) == 1)
      {
        ze._Encryption_FromZipFile = ze._Encryption = EncryptionAlgorithm.PkzipWeak;
        ze._sourceIsEncrypted = true;
      }
      ZipEntry zipEntry6 = ze;
      byte[] numArray11 = buffer1;
      int index11 = num31;
      int num34 = index11 + 1;
      int num35 = (int) numArray11[index11];
      byte[] numArray12 = buffer1;
      int index12 = num34;
      int num36 = index12 + 1;
      int num37 = (int) numArray12[index12] * 256;
      int num38 = num35 + num37;
      byte[] numArray13 = buffer1;
      int index13 = num36;
      int num39 = index13 + 1;
      int num40 = (int) numArray13[index13] * 256 * 256;
      int num41 = num38 + num40;
      byte[] numArray14 = buffer1;
      int index14 = num39;
      int num42 = index14 + 1;
      int num43 = (int) numArray14[index14] * 256 * 256 * 256;
      int num44 = num41 + num43;
      zipEntry6._Crc32 = num44;
      ZipEntry zipEntry7 = ze;
      byte[] numArray15 = buffer1;
      int index15 = num42;
      int num45 = index15 + 1;
      int num46 = (int) numArray15[index15];
      byte[] numArray16 = buffer1;
      int index16 = num45;
      int num47 = index16 + 1;
      int num48 = (int) numArray16[index16] * 256;
      int num49 = num46 + num48;
      byte[] numArray17 = buffer1;
      int index17 = num47;
      int num50 = index17 + 1;
      int num51 = (int) numArray17[index17] * 256 * 256;
      int num52 = num49 + num51;
      byte[] numArray18 = buffer1;
      int index18 = num50;
      int num53 = index18 + 1;
      int num54 = (int) numArray18[index18] * 256 * 256 * 256;
      long num55 = (long) (uint) (num52 + num54);
      zipEntry7._CompressedSize = num55;
      ZipEntry zipEntry8 = ze;
      byte[] numArray19 = buffer1;
      int index19 = num53;
      int num56 = index19 + 1;
      int num57 = (int) numArray19[index19];
      byte[] numArray20 = buffer1;
      int index20 = num56;
      int num58 = index20 + 1;
      int num59 = (int) numArray20[index20] * 256;
      int num60 = num57 + num59;
      byte[] numArray21 = buffer1;
      int index21 = num58;
      int num61 = index21 + 1;
      int num62 = (int) numArray21[index21] * 256 * 256;
      int num63 = num60 + num62;
      byte[] numArray22 = buffer1;
      int index22 = num61;
      int num64 = index22 + 1;
      int num65 = (int) numArray22[index22] * 256 * 256 * 256;
      long num66 = (long) (uint) (num63 + num65);
      zipEntry8._UncompressedSize = num66;
      if ((uint) ze._CompressedSize == uint.MaxValue || (uint) ze._UncompressedSize == uint.MaxValue)
        ze._InputUsesZip64 = true;
      byte[] numArray23 = buffer1;
      int index23 = num64;
      int num67 = index23 + 1;
      int num68 = (int) numArray23[index23];
      byte[] numArray24 = buffer1;
      int index24 = num67;
      int num69 = index24 + 1;
      int num70 = (int) numArray24[index24] * 256;
      short num71 = (short) (num68 + num70);
      byte[] numArray25 = buffer1;
      int index25 = num69;
      int num72 = index25 + 1;
      int num73 = (int) numArray25[index25];
      byte[] numArray26 = buffer1;
      int index26 = num72;
      int num74 = index26 + 1;
      int num75 = (int) numArray26[index26] * 256;
      short extraFieldLength = (short) (num73 + num75);
      byte[] numArray27 = new byte[(int) num71];
      int num76 = ze.ArchiveStream.Read(numArray27, 0, numArray27.Length);
      int num77 = num4 + num76;
      if (((int) ze._BitField & 2048) == 2048)
      {
        ze.AlternateEncoding = Encoding.UTF8;
        ze.AlternateEncodingUsage = ZipOption.Always;
      }
      ze._FileNameInArchive = ze.AlternateEncoding.GetString(numArray27, 0, numArray27.Length);
      if (ze._FileNameInArchive.EndsWith("/"))
        ze.MarkAsDirectory();
      int num78 = num77 + ze.ProcessExtraField(ze.ArchiveStream, extraFieldLength);
      ze._LengthOfTrailer = 0;
      if (!ze._FileNameInArchive.EndsWith("/") && ((int) ze._BitField & 8) == 8)
      {
        long position = ze.ArchiveStream.Position;
        bool flag = true;
        long num79 = 0;
        int num80 = 0;
        while (flag)
        {
          ++num80;
          if (ze._container.ZipFile != null)
            ze._container.ZipFile.OnReadBytes(ze);
          long signature2 = SharedUtilities.FindSignature(ze.ArchiveStream, 134695760);
          if (signature2 == -1L)
            return false;
          num79 += signature2;
          if (ze._InputUsesZip64)
          {
            byte[] buffer2 = new byte[20];
            if (ze.ArchiveStream.Read(buffer2, 0, buffer2.Length) != 20)
              return false;
            int num81 = 0;
            ZipEntry zipEntry9 = ze;
            byte[] numArray28 = buffer2;
            int index27 = num81;
            int num82 = index27 + 1;
            int num83 = (int) numArray28[index27];
            byte[] numArray29 = buffer2;
            int index28 = num82;
            int num84 = index28 + 1;
            int num85 = (int) numArray29[index28] * 256;
            int num86 = num83 + num85;
            byte[] numArray30 = buffer2;
            int index29 = num84;
            int num87 = index29 + 1;
            int num88 = (int) numArray30[index29] * 256 * 256;
            int num89 = num86 + num88;
            byte[] numArray31 = buffer2;
            int index30 = num87;
            int startIndex1 = index30 + 1;
            int num90 = (int) numArray31[index30] * 256 * 256 * 256;
            int num91 = num89 + num90;
            zipEntry9._Crc32 = num91;
            ze._CompressedSize = BitConverter.ToInt64(buffer2, startIndex1);
            int startIndex2 = startIndex1 + 8;
            ze._UncompressedSize = BitConverter.ToInt64(buffer2, startIndex2);
            num74 = startIndex2 + 8;
            ze._LengthOfTrailer += 24;
          }
          else
          {
            byte[] buffer3 = new byte[12];
            if (ze.ArchiveStream.Read(buffer3, 0, buffer3.Length) != 12)
              return false;
            int num92 = 0;
            ZipEntry zipEntry10 = ze;
            byte[] numArray32 = buffer3;
            int index31 = num92;
            int num93 = index31 + 1;
            int num94 = (int) numArray32[index31];
            byte[] numArray33 = buffer3;
            int index32 = num93;
            int num95 = index32 + 1;
            int num96 = (int) numArray33[index32] * 256;
            int num97 = num94 + num96;
            byte[] numArray34 = buffer3;
            int index33 = num95;
            int num98 = index33 + 1;
            int num99 = (int) numArray34[index33] * 256 * 256;
            int num100 = num97 + num99;
            byte[] numArray35 = buffer3;
            int index34 = num98;
            int num101 = index34 + 1;
            int num102 = (int) numArray35[index34] * 256 * 256 * 256;
            int num103 = num100 + num102;
            zipEntry10._Crc32 = num103;
            ZipEntry zipEntry11 = ze;
            byte[] numArray36 = buffer3;
            int index35 = num101;
            int num104 = index35 + 1;
            int num105 = (int) numArray36[index35];
            byte[] numArray37 = buffer3;
            int index36 = num104;
            int num106 = index36 + 1;
            int num107 = (int) numArray37[index36] * 256;
            int num108 = num105 + num107;
            byte[] numArray38 = buffer3;
            int index37 = num106;
            int num109 = index37 + 1;
            int num110 = (int) numArray38[index37] * 256 * 256;
            int num111 = num108 + num110;
            byte[] numArray39 = buffer3;
            int index38 = num109;
            int num112 = index38 + 1;
            int num113 = (int) numArray39[index38] * 256 * 256 * 256;
            long num114 = (long) (uint) (num111 + num113);
            zipEntry11._CompressedSize = num114;
            ZipEntry zipEntry12 = ze;
            byte[] numArray40 = buffer3;
            int index39 = num112;
            int num115 = index39 + 1;
            int num116 = (int) numArray40[index39];
            byte[] numArray41 = buffer3;
            int index40 = num115;
            int num117 = index40 + 1;
            int num118 = (int) numArray41[index40] * 256;
            int num119 = num116 + num118;
            byte[] numArray42 = buffer3;
            int index41 = num117;
            int num120 = index41 + 1;
            int num121 = (int) numArray42[index41] * 256 * 256;
            int num122 = num119 + num121;
            byte[] numArray43 = buffer3;
            int index42 = num120;
            num74 = index42 + 1;
            int num123 = (int) numArray43[index42] * 256 * 256 * 256;
            long num124 = (long) (uint) (num122 + num123);
            zipEntry12._UncompressedSize = num124;
            ze._LengthOfTrailer += 16;
          }
          flag = num79 != ze._CompressedSize;
          if (flag)
          {
            ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
            num79 += 4L;
          }
        }
        ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
      }
      ze._CompressedFileDataSize = ze._CompressedSize;
      if (((int) ze._BitField & 1) == 1)
      {
        ze._WeakEncryptionHeader = new byte[12];
        num78 += ZipEntry.ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
        ze._CompressedFileDataSize -= 12L;
      }
      ze._LengthOfHeader = num78;
      ze._TotalEntrySize = (long) ze._LengthOfHeader + ze._CompressedFileDataSize + (long) ze._LengthOfTrailer;
      return true;
    }

    internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
    {
      int num = s.Read(buffer, 0, 12);
      return num == 12 ? num : throw new ZipException(string.Format("Unexpected end of data at position 0x{0:X8}", (object) s.Position));
    }

    private static bool IsNotValidSig(int signature) => signature != 67324752;

    internal static ZipEntry ReadEntry(ZipContainer zc, bool first)
    {
      ZipFile zipFile = zc.ZipFile;
      Stream readStream = zc.ReadStream;
      Encoding alternateEncoding = zc.AlternateEncoding;
      ZipEntry zipEntry = new ZipEntry();
      zipEntry._Source = ZipEntrySource.ZipFile;
      zipEntry._container = zc;
      zipEntry._archiveStream = readStream;
      zipFile?.OnReadEntry(true, (ZipEntry) null);
      if (first)
        ZipEntry.HandlePK00Prefix(readStream);
      if (!ZipEntry.ReadHeader(zipEntry, alternateEncoding))
        return (ZipEntry) null;
      zipEntry.__FileDataPosition = zipEntry.ArchiveStream.Position;
      readStream.Seek(zipEntry._CompressedFileDataSize + (long) zipEntry._LengthOfTrailer, SeekOrigin.Current);
      ZipEntry.HandleUnexpectedDataDescriptor(zipEntry);
      if (zipFile != null)
      {
        zipFile.OnReadBytes(zipEntry);
        zipFile.OnReadEntry(false, zipEntry);
      }
      return zipEntry;
    }

    internal static void HandlePK00Prefix(Stream s)
    {
      if (SharedUtilities.ReadInt(s) == 808471376)
        return;
      s.Seek(-4L, SeekOrigin.Current);
    }

    private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
    {
      Stream archiveStream = entry.ArchiveStream;
      if ((long) (uint) SharedUtilities.ReadInt(archiveStream) == (long) entry._Crc32)
      {
        if ((long) SharedUtilities.ReadInt(archiveStream) == entry._CompressedSize)
        {
          if ((long) SharedUtilities.ReadInt(archiveStream) == entry._UncompressedSize)
            return;
          archiveStream.Seek(-12L, SeekOrigin.Current);
        }
        else
          archiveStream.Seek(-8L, SeekOrigin.Current);
      }
      else
        archiveStream.Seek(-4L, SeekOrigin.Current);
    }

    internal static int FindExtraFieldSegment(byte[] extra, int offx, ushort targetHeaderId)
    {
      int num1;
      short num2;
      for (int index1 = offx; index1 + 3 < extra.Length; index1 = num1 + (int) num2)
      {
        byte[] numArray1 = extra;
        int index2 = index1;
        int num3 = index2 + 1;
        int num4 = (int) numArray1[index2];
        byte[] numArray2 = extra;
        int index3 = num3;
        int num5 = index3 + 1;
        int num6 = (int) numArray2[index3] * 256;
        if ((int) (ushort) (num4 + num6) == (int) targetHeaderId)
          return num5 - 2;
        byte[] numArray3 = extra;
        int index4 = num5;
        int num7 = index4 + 1;
        int num8 = (int) numArray3[index4];
        byte[] numArray4 = extra;
        int index5 = num7;
        num1 = index5 + 1;
        int num9 = (int) numArray4[index5] * 256;
        num2 = (short) (num8 + num9);
      }
      return -1;
    }

    internal int ProcessExtraField(Stream s, short extraFieldLength)
    {
      int num1 = 0;
      if (extraFieldLength > (short) 0)
      {
        byte[] numArray1 = this._Extra = new byte[(int) extraFieldLength];
        num1 = s.Read(numArray1, 0, numArray1.Length);
        long posn = s.Position - (long) num1;
        int num2;
        short dataSize;
        for (int index1 = 0; index1 + 3 < numArray1.Length; index1 = num2 + (int) dataSize + 4)
        {
          num2 = index1;
          byte[] numArray2 = numArray1;
          int index2 = index1;
          int num3 = index2 + 1;
          int num4 = (int) numArray2[index2];
          byte[] numArray3 = numArray1;
          int index3 = num3;
          int num5 = index3 + 1;
          int num6 = (int) numArray3[index3] * 256;
          ushort num7 = (ushort) (num4 + num6);
          byte[] numArray4 = numArray1;
          int index4 = num5;
          int num8 = index4 + 1;
          int num9 = (int) numArray4[index4];
          byte[] numArray5 = numArray1;
          int index5 = num8;
          int j = index5 + 1;
          int num10 = (int) numArray5[index5] * 256;
          dataSize = (short) (num9 + num10);
          int num11;
          switch (num7)
          {
            case 1:
              num11 = this.ProcessExtraFieldZip64(numArray1, j, dataSize, posn);
              break;
            case 10:
              num11 = this.ProcessExtraFieldWindowsTimes(numArray1, j, dataSize, posn);
              break;
            case 23:
              num11 = this.ProcessExtraFieldPkwareStrongEncryption(numArray1, j);
              break;
            case 21589:
              num11 = this.ProcessExtraFieldUnixTimes(numArray1, j, dataSize, posn);
              break;
            case 22613:
              num11 = this.ProcessExtraFieldInfoZipTimes(numArray1, j, dataSize, posn);
              break;
          }
        }
      }
      return num1;
    }

    private int ProcessExtraFieldPkwareStrongEncryption(byte[] Buffer, int j)
    {
      j += 2;
      this._UnsupportedAlgorithmId = (uint) (ushort) ((int) Buffer[j++] + (int) Buffer[j++] * 256);
      this._Encryption_FromZipFile = this._Encryption = EncryptionAlgorithm.Unsupported;
      return j;
    }

    private int ProcessExtraFieldZip64(byte[] buffer, int j, short dataSize, long posn)
    {
      this._InputUsesZip64 = true;
      int remainingData = dataSize <= (short) 28 ? (int) dataSize : throw new BadReadException(string.Format("  Inconsistent size (0x{0:X4}) for ZIP64 extra field at position 0x{1:X16}", (object) dataSize, (object) posn));
      ZipEntry.Func<long> func = (ZipEntry.Func<long>) (() =>
      {
        if (remainingData < 8)
          throw new BadReadException(string.Format("  Missing data for ZIP64 extra field, position 0x{0:X16}", (object) posn));
        long int64 = BitConverter.ToInt64(buffer, j);
        j += 8;
        remainingData -= 8;
        return int64;
      });
      if (this._UncompressedSize == (long) uint.MaxValue)
        this._UncompressedSize = func();
      if (this._CompressedSize == (long) uint.MaxValue)
        this._CompressedSize = func();
      if (this._RelativeOffsetOfLocalHeader == (long) uint.MaxValue)
        this._RelativeOffsetOfLocalHeader = func();
      return j;
    }

    private int ProcessExtraFieldInfoZipTimes(byte[] buffer, int j, short dataSize, long posn)
    {
      if (dataSize != (short) 12 && dataSize != (short) 8)
        throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for InfoZip v1 extra field at position 0x{1:X16}", (object) dataSize, (object) posn));
      int int32_1 = BitConverter.ToInt32(buffer, j);
      this._Mtime = ZipEntry._unixEpoch.AddSeconds((double) int32_1);
      j += 4;
      int int32_2 = BitConverter.ToInt32(buffer, j);
      this._Atime = ZipEntry._unixEpoch.AddSeconds((double) int32_2);
      j += 4;
      this._Ctime = DateTime.UtcNow;
      this._ntfsTimesAreSet = true;
      this._timestamp |= ZipEntryTimestamp.InfoZip1;
      return j;
    }

    private int ProcessExtraFieldUnixTimes(byte[] buffer, int j, short dataSize, long posn)
    {
      int remainingData = dataSize == (short) 13 || dataSize == (short) 9 || dataSize == (short) 5 ? (int) dataSize : throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for Extended Timestamp extra field at position 0x{1:X16}", (object) dataSize, (object) posn));
      ZipEntry.Func<DateTime> func = (ZipEntry.Func<DateTime>) (() =>
      {
        int int32 = BitConverter.ToInt32(buffer, j);
        j += 4;
        remainingData -= 4;
        return ZipEntry._unixEpoch.AddSeconds((double) int32);
      });
      if (dataSize == (short) 13 || this._readExtraDepth > 0)
      {
        byte num = buffer[j++];
        remainingData--;
        if (((int) num & 1) != 0 && remainingData >= 4)
          this._Mtime = func();
        this._Atime = ((int) num & 2) == 0 || remainingData < 4 ? DateTime.UtcNow : func();
        this._Ctime = ((int) num & 4) == 0 || remainingData < 4 ? DateTime.UtcNow : func();
        this._timestamp |= ZipEntryTimestamp.Unix;
        this._ntfsTimesAreSet = true;
        this._emitUnixTimes = true;
      }
      else
        this.ReadExtraField();
      return j;
    }

    private int ProcessExtraFieldWindowsTimes(byte[] buffer, int j, short dataSize, long posn)
    {
      if (dataSize != (short) 32)
        throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for NTFS times extra field at position 0x{1:X16}", (object) dataSize, (object) posn));
      j += 4;
      short num1 = (short) ((int) buffer[j] + (int) buffer[j + 1] * 256);
      short num2 = (short) ((int) buffer[j + 2] + (int) buffer[j + 3] * 256);
      j += 4;
      if (num1 == (short) 1 && num2 == (short) 24)
      {
        this._Mtime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(buffer, j));
        j += 8;
        this._Atime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(buffer, j));
        j += 8;
        this._Ctime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(buffer, j));
        j += 8;
        this._ntfsTimesAreSet = true;
        this._timestamp |= ZipEntryTimestamp.Windows;
        this._emitNtfsTimes = true;
      }
      return j;
    }

    internal void WriteCentralDirectoryEntry(Stream s)
    {
      byte[] buffer = new byte[4096];
      int num1 = 0;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      numArray1[index1] = (byte) 80;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num3 = index2 + 1;
      numArray2[index2] = (byte) 75;
      byte[] numArray3 = buffer;
      int index3 = num3;
      int num4 = index3 + 1;
      numArray3[index3] = (byte) 1;
      byte[] numArray4 = buffer;
      int index4 = num4;
      int num5 = index4 + 1;
      numArray4[index4] = (byte) 2;
      byte[] numArray5 = buffer;
      int index5 = num5;
      int num6 = index5 + 1;
      int num7 = (int) (byte) ((uint) this._VersionMadeBy & (uint) byte.MaxValue);
      numArray5[index5] = (byte) num7;
      byte[] numArray6 = buffer;
      int index6 = num6;
      int num8 = index6 + 1;
      int num9 = (int) (byte) (((int) this._VersionMadeBy & 65280) >> 8);
      numArray6[index6] = (byte) num9;
      short num10 = this.VersionNeeded != (short) 0 ? this.VersionNeeded : (short) 20;
      if (!this._OutputUsesZip64.HasValue)
        this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always);
      short num11 = this._OutputUsesZip64.Value ? (short) 45 : num10;
      byte[] numArray7 = buffer;
      int index7 = num8;
      int num12 = index7 + 1;
      int num13 = (int) (byte) ((uint) num11 & (uint) byte.MaxValue);
      numArray7[index7] = (byte) num13;
      byte[] numArray8 = buffer;
      int index8 = num12;
      int num14 = index8 + 1;
      int num15 = (int) (byte) (((int) num11 & 65280) >> 8);
      numArray8[index8] = (byte) num15;
      byte[] numArray9 = buffer;
      int index9 = num14;
      int num16 = index9 + 1;
      int num17 = (int) (byte) ((uint) this._BitField & (uint) byte.MaxValue);
      numArray9[index9] = (byte) num17;
      byte[] numArray10 = buffer;
      int index10 = num16;
      int num18 = index10 + 1;
      int num19 = (int) (byte) (((int) this._BitField & 65280) >> 8);
      numArray10[index10] = (byte) num19;
      byte[] numArray11 = buffer;
      int index11 = num18;
      int num20 = index11 + 1;
      int num21 = (int) (byte) ((uint) this._CompressionMethod & (uint) byte.MaxValue);
      numArray11[index11] = (byte) num21;
      byte[] numArray12 = buffer;
      int index12 = num20;
      int num22 = index12 + 1;
      int num23 = (int) (byte) (((int) this._CompressionMethod & 65280) >> 8);
      numArray12[index12] = (byte) num23;
      byte[] numArray13 = buffer;
      int index13 = num22;
      int num24 = index13 + 1;
      int num25 = (int) (byte) (this._TimeBlob & (int) byte.MaxValue);
      numArray13[index13] = (byte) num25;
      byte[] numArray14 = buffer;
      int index14 = num24;
      int num26 = index14 + 1;
      int num27 = (int) (byte) ((this._TimeBlob & 65280) >> 8);
      numArray14[index14] = (byte) num27;
      byte[] numArray15 = buffer;
      int index15 = num26;
      int num28 = index15 + 1;
      int num29 = (int) (byte) ((this._TimeBlob & 16711680) >> 16);
      numArray15[index15] = (byte) num29;
      byte[] numArray16 = buffer;
      int index16 = num28;
      int num30 = index16 + 1;
      int num31 = (int) (byte) (((long) this._TimeBlob & 4278190080L) >> 24);
      numArray16[index16] = (byte) num31;
      byte[] numArray17 = buffer;
      int index17 = num30;
      int num32 = index17 + 1;
      int num33 = (int) (byte) (this._Crc32 & (int) byte.MaxValue);
      numArray17[index17] = (byte) num33;
      byte[] numArray18 = buffer;
      int index18 = num32;
      int num34 = index18 + 1;
      int num35 = (int) (byte) ((this._Crc32 & 65280) >> 8);
      numArray18[index18] = (byte) num35;
      byte[] numArray19 = buffer;
      int index19 = num34;
      int num36 = index19 + 1;
      int num37 = (int) (byte) ((this._Crc32 & 16711680) >> 16);
      numArray19[index19] = (byte) num37;
      byte[] numArray20 = buffer;
      int index20 = num36;
      int num38 = index20 + 1;
      int num39 = (int) (byte) (((long) this._Crc32 & 4278190080L) >> 24);
      numArray20[index20] = (byte) num39;
      if (this._OutputUsesZip64.Value)
      {
        for (int index21 = 0; index21 < 8; ++index21)
          buffer[num38++] = byte.MaxValue;
      }
      else
      {
        byte[] numArray21 = buffer;
        int index22 = num38;
        int num40 = index22 + 1;
        int num41 = (int) (byte) ((ulong) this._CompressedSize & (ulong) byte.MaxValue);
        numArray21[index22] = (byte) num41;
        byte[] numArray22 = buffer;
        int index23 = num40;
        int num42 = index23 + 1;
        int num43 = (int) (byte) ((this._CompressedSize & 65280L) >> 8);
        numArray22[index23] = (byte) num43;
        byte[] numArray23 = buffer;
        int index24 = num42;
        int num44 = index24 + 1;
        int num45 = (int) (byte) ((this._CompressedSize & 16711680L) >> 16);
        numArray23[index24] = (byte) num45;
        byte[] numArray24 = buffer;
        int index25 = num44;
        int num46 = index25 + 1;
        int num47 = (int) (byte) ((this._CompressedSize & 4278190080L) >> 24);
        numArray24[index25] = (byte) num47;
        byte[] numArray25 = buffer;
        int index26 = num46;
        int num48 = index26 + 1;
        int num49 = (int) (byte) ((ulong) this._UncompressedSize & (ulong) byte.MaxValue);
        numArray25[index26] = (byte) num49;
        byte[] numArray26 = buffer;
        int index27 = num48;
        int num50 = index27 + 1;
        int num51 = (int) (byte) ((this._UncompressedSize & 65280L) >> 8);
        numArray26[index27] = (byte) num51;
        byte[] numArray27 = buffer;
        int index28 = num50;
        int num52 = index28 + 1;
        int num53 = (int) (byte) ((this._UncompressedSize & 16711680L) >> 16);
        numArray27[index28] = (byte) num53;
        byte[] numArray28 = buffer;
        int index29 = num52;
        num38 = index29 + 1;
        int num54 = (int) (byte) ((this._UncompressedSize & 4278190080L) >> 24);
        numArray28[index29] = (byte) num54;
      }
      byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
      short length = (short) encodedFileNameBytes.Length;
      byte[] numArray29 = buffer;
      int index30 = num38;
      int num55 = index30 + 1;
      int num56 = (int) (byte) ((uint) length & (uint) byte.MaxValue);
      numArray29[index30] = (byte) num56;
      byte[] numArray30 = buffer;
      int index31 = num55;
      int num57 = index31 + 1;
      int num58 = (int) (byte) (((int) length & 65280) >> 8);
      numArray30[index31] = (byte) num58;
      this._presumeZip64 = this._OutputUsesZip64.Value;
      this._Extra = this.ConstructExtraField(true);
      short num59 = this._Extra == null ? (short) 0 : (short) this._Extra.Length;
      byte[] numArray31 = buffer;
      int index32 = num57;
      int num60 = index32 + 1;
      int num61 = (int) (byte) ((uint) num59 & (uint) byte.MaxValue);
      numArray31[index32] = (byte) num61;
      byte[] numArray32 = buffer;
      int index33 = num60;
      int num62 = index33 + 1;
      int num63 = (int) (byte) (((int) num59 & 65280) >> 8);
      numArray32[index33] = (byte) num63;
      int count = this._CommentBytes == null ? 0 : this._CommentBytes.Length;
      if (count + num62 > buffer.Length)
        count = buffer.Length - num62;
      byte[] numArray33 = buffer;
      int index34 = num62;
      int num64 = index34 + 1;
      int num65 = (int) (byte) (count & (int) byte.MaxValue);
      numArray33[index34] = (byte) num65;
      byte[] numArray34 = buffer;
      int index35 = num64;
      int num66 = index35 + 1;
      int num67 = (int) (byte) ((count & 65280) >> 8);
      numArray34[index35] = (byte) num67;
      int num68;
      if (this._container.ZipFile != null && (uint) this._container.ZipFile.MaxOutputSegmentSize > 0U)
      {
        byte[] numArray35 = buffer;
        int index36 = num66;
        int num69 = index36 + 1;
        int num70 = (int) (byte) (this._diskNumber & (uint) byte.MaxValue);
        numArray35[index36] = (byte) num70;
        byte[] numArray36 = buffer;
        int index37 = num69;
        num68 = index37 + 1;
        int num71 = (int) (byte) ((this._diskNumber & 65280U) >> 8);
        numArray36[index37] = (byte) num71;
      }
      else
      {
        byte[] numArray37 = buffer;
        int index38 = num66;
        int num72 = index38 + 1;
        numArray37[index38] = (byte) 0;
        byte[] numArray38 = buffer;
        int index39 = num72;
        num68 = index39 + 1;
        numArray38[index39] = (byte) 0;
      }
      byte[] numArray39 = buffer;
      int index40 = num68;
      int num73 = index40 + 1;
      int num74 = this._IsText ? 1 : 0;
      numArray39[index40] = (byte) num74;
      byte[] numArray40 = buffer;
      int index41 = num73;
      int num75 = index41 + 1;
      numArray40[index41] = (byte) 0;
      byte[] numArray41 = buffer;
      int index42 = num75;
      int num76 = index42 + 1;
      int num77 = (int) (byte) (this._ExternalFileAttrs & (int) byte.MaxValue);
      numArray41[index42] = (byte) num77;
      byte[] numArray42 = buffer;
      int index43 = num76;
      int num78 = index43 + 1;
      int num79 = (int) (byte) ((this._ExternalFileAttrs & 65280) >> 8);
      numArray42[index43] = (byte) num79;
      byte[] numArray43 = buffer;
      int index44 = num78;
      int num80 = index44 + 1;
      int num81 = (int) (byte) ((this._ExternalFileAttrs & 16711680) >> 16);
      numArray43[index44] = (byte) num81;
      byte[] numArray44 = buffer;
      int index45 = num80;
      int num82 = index45 + 1;
      int num83 = (int) (byte) (((long) this._ExternalFileAttrs & 4278190080L) >> 24);
      numArray44[index45] = (byte) num83;
      int dstOffset;
      if (this._RelativeOffsetOfLocalHeader > (long) uint.MaxValue)
      {
        byte[] numArray45 = buffer;
        int index46 = num82;
        int num84 = index46 + 1;
        numArray45[index46] = byte.MaxValue;
        byte[] numArray46 = buffer;
        int index47 = num84;
        int num85 = index47 + 1;
        numArray46[index47] = byte.MaxValue;
        byte[] numArray47 = buffer;
        int index48 = num85;
        int num86 = index48 + 1;
        numArray47[index48] = byte.MaxValue;
        byte[] numArray48 = buffer;
        int index49 = num86;
        dstOffset = index49 + 1;
        numArray48[index49] = byte.MaxValue;
      }
      else
      {
        byte[] numArray49 = buffer;
        int index50 = num82;
        int num87 = index50 + 1;
        int num88 = (int) (byte) ((ulong) this._RelativeOffsetOfLocalHeader & (ulong) byte.MaxValue);
        numArray49[index50] = (byte) num88;
        byte[] numArray50 = buffer;
        int index51 = num87;
        int num89 = index51 + 1;
        int num90 = (int) (byte) ((this._RelativeOffsetOfLocalHeader & 65280L) >> 8);
        numArray50[index51] = (byte) num90;
        byte[] numArray51 = buffer;
        int index52 = num89;
        int num91 = index52 + 1;
        int num92 = (int) (byte) ((this._RelativeOffsetOfLocalHeader & 16711680L) >> 16);
        numArray51[index52] = (byte) num92;
        byte[] numArray52 = buffer;
        int index53 = num91;
        dstOffset = index53 + 1;
        int num93 = (int) (byte) ((this._RelativeOffsetOfLocalHeader & 4278190080L) >> 24);
        numArray52[index53] = (byte) num93;
      }
      Buffer.BlockCopy((Array) encodedFileNameBytes, 0, (Array) buffer, dstOffset, (int) length);
      int num94 = dstOffset + (int) length;
      if (this._Extra != null)
      {
        Buffer.BlockCopy((Array) this._Extra, 0, (Array) buffer, num94, (int) num59);
        num94 += (int) num59;
      }
      if ((uint) count > 0U)
      {
        Buffer.BlockCopy((Array) this._CommentBytes, 0, (Array) buffer, num94, count);
        num94 += count;
      }
      s.Write(buffer, 0, num94);
    }

    private byte[] ConstructExtraField(bool forCentralDirectory)
    {
      List<byte[]> numArrayList = new List<byte[]>();
      if (this._container.Zip64 == Zip64Option.Always || this._container.Zip64 == Zip64Option.AsNecessary && (!forCentralDirectory || this._entryRequiresZip64.Value))
      {
        int length = 4 + (forCentralDirectory ? 28 : 16);
        byte[] numArray1 = new byte[length];
        int num1 = 0;
        int num2;
        if (this._presumeZip64 | forCentralDirectory)
        {
          byte[] numArray2 = numArray1;
          int index1 = num1;
          int num3 = index1 + 1;
          numArray2[index1] = (byte) 1;
          byte[] numArray3 = numArray1;
          int index2 = num3;
          num2 = index2 + 1;
          numArray3[index2] = (byte) 0;
        }
        else
        {
          byte[] numArray4 = numArray1;
          int index3 = num1;
          int num4 = index3 + 1;
          numArray4[index3] = (byte) 153;
          byte[] numArray5 = numArray1;
          int index4 = num4;
          num2 = index4 + 1;
          numArray5[index4] = (byte) 153;
        }
        byte[] numArray6 = numArray1;
        int index5 = num2;
        int num5 = index5 + 1;
        int num6 = (int) (byte) (length - 4);
        numArray6[index5] = (byte) num6;
        byte[] numArray7 = numArray1;
        int index6 = num5;
        int destinationIndex1 = index6 + 1;
        numArray7[index6] = (byte) 0;
        Array.Copy((Array) BitConverter.GetBytes(this._UncompressedSize), 0, (Array) numArray1, destinationIndex1, 8);
        int destinationIndex2 = destinationIndex1 + 8;
        Array.Copy((Array) BitConverter.GetBytes(this._CompressedSize), 0, (Array) numArray1, destinationIndex2, 8);
        int destinationIndex3 = destinationIndex2 + 8;
        if (forCentralDirectory)
        {
          Array.Copy((Array) BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, (Array) numArray1, destinationIndex3, 8);
          int destinationIndex4 = destinationIndex3 + 8;
          Array.Copy((Array) BitConverter.GetBytes(0), 0, (Array) numArray1, destinationIndex4, 4);
        }
        numArrayList.Add(numArray1);
      }
      if (this._ntfsTimesAreSet && this._emitNtfsTimes)
      {
        byte[] numArray8 = new byte[36];
        int num7 = 0;
        byte[] numArray9 = numArray8;
        int index7 = num7;
        int num8 = index7 + 1;
        numArray9[index7] = (byte) 10;
        byte[] numArray10 = numArray8;
        int index8 = num8;
        int num9 = index8 + 1;
        numArray10[index8] = (byte) 0;
        byte[] numArray11 = numArray8;
        int index9 = num9;
        int num10 = index9 + 1;
        numArray11[index9] = (byte) 32;
        byte[] numArray12 = numArray8;
        int index10 = num10;
        int num11 = index10 + 1;
        numArray12[index10] = (byte) 0;
        int num12 = num11 + 4;
        byte[] numArray13 = numArray8;
        int index11 = num12;
        int num13 = index11 + 1;
        numArray13[index11] = (byte) 1;
        byte[] numArray14 = numArray8;
        int index12 = num13;
        int num14 = index12 + 1;
        numArray14[index12] = (byte) 0;
        byte[] numArray15 = numArray8;
        int index13 = num14;
        int num15 = index13 + 1;
        numArray15[index13] = (byte) 24;
        byte[] numArray16 = numArray8;
        int index14 = num15;
        int destinationIndex5 = index14 + 1;
        numArray16[index14] = (byte) 0;
        Array.Copy((Array) BitConverter.GetBytes(this._Mtime.ToFileTime()), 0, (Array) numArray8, destinationIndex5, 8);
        int destinationIndex6 = destinationIndex5 + 8;
        Array.Copy((Array) BitConverter.GetBytes(this._Atime.ToFileTime()), 0, (Array) numArray8, destinationIndex6, 8);
        int destinationIndex7 = destinationIndex6 + 8;
        Array.Copy((Array) BitConverter.GetBytes(this._Ctime.ToFileTime()), 0, (Array) numArray8, destinationIndex7, 8);
        int num16 = destinationIndex7 + 8;
        numArrayList.Add(numArray8);
      }
      if (this._ntfsTimesAreSet && this._emitUnixTimes)
      {
        int length = 9;
        if (!forCentralDirectory)
          length += 8;
        byte[] numArray17 = new byte[length];
        int num17 = 0;
        byte[] numArray18 = numArray17;
        int index15 = num17;
        int num18 = index15 + 1;
        numArray18[index15] = (byte) 85;
        byte[] numArray19 = numArray17;
        int index16 = num18;
        int num19 = index16 + 1;
        numArray19[index16] = (byte) 84;
        byte[] numArray20 = numArray17;
        int index17 = num19;
        int num20 = index17 + 1;
        int num21 = (int) (byte) (length - 4);
        numArray20[index17] = (byte) num21;
        byte[] numArray21 = numArray17;
        int index18 = num20;
        int num22 = index18 + 1;
        numArray21[index18] = (byte) 0;
        byte[] numArray22 = numArray17;
        int index19 = num22;
        int destinationIndex8 = index19 + 1;
        numArray22[index19] = (byte) 7;
        Array.Copy((Array) BitConverter.GetBytes((int) (this._Mtime - ZipEntry._unixEpoch).TotalSeconds), 0, (Array) numArray17, destinationIndex8, 4);
        int destinationIndex9 = destinationIndex8 + 4;
        if (!forCentralDirectory)
        {
          Array.Copy((Array) BitConverter.GetBytes((int) (this._Atime - ZipEntry._unixEpoch).TotalSeconds), 0, (Array) numArray17, destinationIndex9, 4);
          int destinationIndex10 = destinationIndex9 + 4;
          Array.Copy((Array) BitConverter.GetBytes((int) (this._Ctime - ZipEntry._unixEpoch).TotalSeconds), 0, (Array) numArray17, destinationIndex10, 4);
          int num23 = destinationIndex10 + 4;
        }
        numArrayList.Add(numArray17);
      }
      byte[] numArray = (byte[]) null;
      if (numArrayList.Count > 0)
      {
        int length = 0;
        int destinationIndex = 0;
        for (int index = 0; index < numArrayList.Count; ++index)
          length += numArrayList[index].Length;
        numArray = new byte[length];
        for (int index = 0; index < numArrayList.Count; ++index)
        {
          Array.Copy((Array) numArrayList[index], 0, (Array) numArray, destinationIndex, numArrayList[index].Length);
          destinationIndex += numArrayList[index].Length;
        }
      }
      return numArray;
    }

    private string NormalizeFileName()
    {
      string str1 = this.FileName.Replace("\\", "/");
      string str2;
      if (this._TrimVolumeFromFullyQualifiedPaths && this.FileName.Length >= 3 && this.FileName[1] == ':' && str1[2] == '/')
        str2 = str1.Substring(3);
      else if (this.FileName.Length >= 4 && str1[0] == '/' && str1[1] == '/')
      {
        int num = str1.IndexOf('/', 2);
        str2 = num != -1 ? str1.Substring(num + 1) : throw new ArgumentException("The path for that entry appears to be badly formatted");
      }
      else
        str2 = this.FileName.Length < 3 || str1[0] != '.' || str1[1] != '/' ? str1 : str1.Substring(2);
      return str2;
    }

    private byte[] GetEncodedFileNameBytes()
    {
      string s = this.NormalizeFileName();
      switch (this.AlternateEncodingUsage)
      {
        case ZipOption.Default:
          if (this._Comment != null && (uint) this._Comment.Length > 0U)
            this._CommentBytes = ZipEntry.ibm437.GetBytes(this._Comment);
          this._actualEncoding = ZipEntry.ibm437;
          return ZipEntry.ibm437.GetBytes(s);
        case ZipOption.Always:
          if (this._Comment != null && (uint) this._Comment.Length > 0U)
            this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
          this._actualEncoding = this.AlternateEncoding;
          return this.AlternateEncoding.GetBytes(s);
        default:
          byte[] bytes1 = ZipEntry.ibm437.GetBytes(s);
          string str = ZipEntry.ibm437.GetString(bytes1, 0, bytes1.Length);
          this._CommentBytes = (byte[]) null;
          if (str != s)
          {
            byte[] bytes2 = this.AlternateEncoding.GetBytes(s);
            if (this._Comment != null && (uint) this._Comment.Length > 0U)
              this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
            this._actualEncoding = this.AlternateEncoding;
            return bytes2;
          }
          this._actualEncoding = ZipEntry.ibm437;
          if (this._Comment == null || this._Comment.Length == 0)
            return bytes1;
          byte[] bytes3 = ZipEntry.ibm437.GetBytes(this._Comment);
          if (ZipEntry.ibm437.GetString(bytes3, 0, bytes3.Length) != this.Comment)
          {
            byte[] bytes4 = this.AlternateEncoding.GetBytes(s);
            this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
            this._actualEncoding = this.AlternateEncoding;
            return bytes4;
          }
          this._CommentBytes = bytes3;
          return bytes1;
      }
    }

    private bool WantReadAgain() => this._UncompressedSize >= 16L && this._CompressionMethod != (short) 0 && this.CompressionLevel != CompressionLevel.None && this._CompressedSize >= this._UncompressedSize && (this._Source != ZipEntrySource.Stream || this._sourceStream.CanSeek) && (this._zipCrypto_forWrite == null || this.CompressedSize - 12L > this.UncompressedSize);

    private void MaybeUnsetCompressionMethodForWriting(int cycle)
    {
      if (cycle > 1)
        this._CompressionMethod = (short) 0;
      else if (this.IsDirectory)
      {
        this._CompressionMethod = (short) 0;
      }
      else
      {
        if (this._Source == ZipEntrySource.ZipFile)
          return;
        if (this._Source == ZipEntrySource.Stream)
        {
          if (this._sourceStream != null && this._sourceStream.CanSeek && this._sourceStream.Length == 0L)
          {
            this._CompressionMethod = (short) 0;
            return;
          }
        }
        else if (this._Source == ZipEntrySource.FileSystem && SharedUtilities.GetFileLength(this.LocalFileName) == 0L)
        {
          this._CompressionMethod = (short) 0;
          return;
        }
        if (this.SetCompression != null)
          this.CompressionLevel = this.SetCompression(this.LocalFileName, this._FileNameInArchive);
        if (this.CompressionLevel != CompressionLevel.None || this.CompressionMethod != CompressionMethod.Deflate)
          return;
        this._CompressionMethod = (short) 0;
      }
    }

    internal void WriteHeader(Stream s, int cycle)
    {
      this._future_ROLH = s is CountingStream countingStream ? countingStream.ComputedPosition : s.Position;
      int num1 = 0;
      byte[] numArray1 = new byte[30];
      byte[] numArray2 = numArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      numArray2[index1] = (byte) 80;
      byte[] numArray3 = numArray1;
      int index2 = num2;
      int num3 = index2 + 1;
      numArray3[index2] = (byte) 75;
      byte[] numArray4 = numArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      numArray4[index3] = (byte) 3;
      byte[] numArray5 = numArray1;
      int index4 = num4;
      int num5 = index4 + 1;
      numArray5[index4] = (byte) 4;
      this._presumeZip64 = this._container.Zip64 == Zip64Option.Always || this._container.Zip64 == Zip64Option.AsNecessary && !s.CanSeek;
      short num6 = this._presumeZip64 ? (short) 45 : (short) 20;
      byte[] numArray6 = numArray1;
      int index5 = num5;
      int num7 = index5 + 1;
      int num8 = (int) (byte) ((uint) num6 & (uint) byte.MaxValue);
      numArray6[index5] = (byte) num8;
      byte[] numArray7 = numArray1;
      int index6 = num7;
      int num9 = index6 + 1;
      int num10 = (int) (byte) (((int) num6 & 65280) >> 8);
      numArray7[index6] = (byte) num10;
      byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
      short length = (short) encodedFileNameBytes.Length;
      if (this._Encryption == EncryptionAlgorithm.None)
        this._BitField &= (short) -2;
      else
        this._BitField |= (short) 1;
      if (this._actualEncoding.CodePage == Encoding.UTF8.CodePage)
        this._BitField |= (short) 2048;
      if (this.IsDirectory || cycle == 99)
      {
        this._BitField &= (short) -9;
        this._BitField &= (short) -2;
        this.Encryption = EncryptionAlgorithm.None;
        this.Password = (string) null;
      }
      else if (!s.CanSeek)
        this._BitField |= (short) 8;
      byte[] numArray8 = numArray1;
      int index7 = num9;
      int num11 = index7 + 1;
      int num12 = (int) (byte) ((uint) this._BitField & (uint) byte.MaxValue);
      numArray8[index7] = (byte) num12;
      byte[] numArray9 = numArray1;
      int index8 = num11;
      int num13 = index8 + 1;
      int num14 = (int) (byte) (((int) this._BitField & 65280) >> 8);
      numArray9[index8] = (byte) num14;
      if (this.__FileDataPosition == -1L)
      {
        this._CompressedSize = 0L;
        this._crcCalculated = false;
      }
      this.MaybeUnsetCompressionMethodForWriting(cycle);
      byte[] numArray10 = numArray1;
      int index9 = num13;
      int num15 = index9 + 1;
      int num16 = (int) (byte) ((uint) this._CompressionMethod & (uint) byte.MaxValue);
      numArray10[index9] = (byte) num16;
      byte[] numArray11 = numArray1;
      int index10 = num15;
      int num17 = index10 + 1;
      int num18 = (int) (byte) (((int) this._CompressionMethod & 65280) >> 8);
      numArray11[index10] = (byte) num18;
      if (cycle == 99)
        this.SetZip64Flags();
      this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
      byte[] numArray12 = numArray1;
      int index11 = num17;
      int num19 = index11 + 1;
      int num20 = (int) (byte) (this._TimeBlob & (int) byte.MaxValue);
      numArray12[index11] = (byte) num20;
      byte[] numArray13 = numArray1;
      int index12 = num19;
      int num21 = index12 + 1;
      int num22 = (int) (byte) ((this._TimeBlob & 65280) >> 8);
      numArray13[index12] = (byte) num22;
      byte[] numArray14 = numArray1;
      int index13 = num21;
      int num23 = index13 + 1;
      int num24 = (int) (byte) ((this._TimeBlob & 16711680) >> 16);
      numArray14[index13] = (byte) num24;
      byte[] numArray15 = numArray1;
      int index14 = num23;
      int num25 = index14 + 1;
      int num26 = (int) (byte) (((long) this._TimeBlob & 4278190080L) >> 24);
      numArray15[index14] = (byte) num26;
      byte[] numArray16 = numArray1;
      int index15 = num25;
      int num27 = index15 + 1;
      int num28 = (int) (byte) (this._Crc32 & (int) byte.MaxValue);
      numArray16[index15] = (byte) num28;
      byte[] numArray17 = numArray1;
      int index16 = num27;
      int num29 = index16 + 1;
      int num30 = (int) (byte) ((this._Crc32 & 65280) >> 8);
      numArray17[index16] = (byte) num30;
      byte[] numArray18 = numArray1;
      int index17 = num29;
      int num31 = index17 + 1;
      int num32 = (int) (byte) ((this._Crc32 & 16711680) >> 16);
      numArray18[index17] = (byte) num32;
      byte[] numArray19 = numArray1;
      int index18 = num31;
      int num33 = index18 + 1;
      int num34 = (int) (byte) (((long) this._Crc32 & 4278190080L) >> 24);
      numArray19[index18] = (byte) num34;
      if (this._presumeZip64)
      {
        for (int index19 = 0; index19 < 8; ++index19)
          numArray1[num33++] = byte.MaxValue;
      }
      else
      {
        byte[] numArray20 = numArray1;
        int index20 = num33;
        int num35 = index20 + 1;
        int num36 = (int) (byte) ((ulong) this._CompressedSize & (ulong) byte.MaxValue);
        numArray20[index20] = (byte) num36;
        byte[] numArray21 = numArray1;
        int index21 = num35;
        int num37 = index21 + 1;
        int num38 = (int) (byte) ((this._CompressedSize & 65280L) >> 8);
        numArray21[index21] = (byte) num38;
        byte[] numArray22 = numArray1;
        int index22 = num37;
        int num39 = index22 + 1;
        int num40 = (int) (byte) ((this._CompressedSize & 16711680L) >> 16);
        numArray22[index22] = (byte) num40;
        byte[] numArray23 = numArray1;
        int index23 = num39;
        int num41 = index23 + 1;
        int num42 = (int) (byte) ((this._CompressedSize & 4278190080L) >> 24);
        numArray23[index23] = (byte) num42;
        byte[] numArray24 = numArray1;
        int index24 = num41;
        int num43 = index24 + 1;
        int num44 = (int) (byte) ((ulong) this._UncompressedSize & (ulong) byte.MaxValue);
        numArray24[index24] = (byte) num44;
        byte[] numArray25 = numArray1;
        int index25 = num43;
        int num45 = index25 + 1;
        int num46 = (int) (byte) ((this._UncompressedSize & 65280L) >> 8);
        numArray25[index25] = (byte) num46;
        byte[] numArray26 = numArray1;
        int index26 = num45;
        int num47 = index26 + 1;
        int num48 = (int) (byte) ((this._UncompressedSize & 16711680L) >> 16);
        numArray26[index26] = (byte) num48;
        byte[] numArray27 = numArray1;
        int index27 = num47;
        num33 = index27 + 1;
        int num49 = (int) (byte) ((this._UncompressedSize & 4278190080L) >> 24);
        numArray27[index27] = (byte) num49;
      }
      byte[] numArray28 = numArray1;
      int index28 = num33;
      int num50 = index28 + 1;
      int num51 = (int) (byte) ((uint) length & (uint) byte.MaxValue);
      numArray28[index28] = (byte) num51;
      byte[] numArray29 = numArray1;
      int index29 = num50;
      int num52 = index29 + 1;
      int num53 = (int) (byte) (((int) length & 65280) >> 8);
      numArray29[index29] = (byte) num53;
      this._Extra = this.ConstructExtraField(false);
      short num54 = this._Extra == null ? (short) 0 : (short) this._Extra.Length;
      byte[] numArray30 = numArray1;
      int index30 = num52;
      int num55 = index30 + 1;
      int num56 = (int) (byte) ((uint) num54 & (uint) byte.MaxValue);
      numArray30[index30] = (byte) num56;
      byte[] numArray31 = numArray1;
      int index31 = num55;
      int num57 = index31 + 1;
      int num58 = (int) (byte) (((int) num54 & 65280) >> 8);
      numArray31[index31] = (byte) num58;
      byte[] buffer = new byte[num57 + (int) length + (int) num54];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) buffer, 0, num57);
      Buffer.BlockCopy((Array) encodedFileNameBytes, 0, (Array) buffer, num57, encodedFileNameBytes.Length);
      int num59 = num57 + encodedFileNameBytes.Length;
      if (this._Extra != null)
      {
        Buffer.BlockCopy((Array) this._Extra, 0, (Array) buffer, num59, this._Extra.Length);
        num59 += this._Extra.Length;
      }
      this._LengthOfHeader = num59;
      if (s is ZipSegmentedStream zipSegmentedStream)
      {
        zipSegmentedStream.ContiguousWrite = true;
        uint segment = zipSegmentedStream.ComputeSegment(num59);
        this._future_ROLH = (int) segment == (int) zipSegmentedStream.CurrentSegment ? zipSegmentedStream.Position : 0L;
        this._diskNumber = segment;
      }
      if (this._container.Zip64 == Zip64Option.Default && (uint) this._RelativeOffsetOfLocalHeader >= uint.MaxValue)
        throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
      s.Write(buffer, 0, num59);
      if (zipSegmentedStream != null)
        zipSegmentedStream.ContiguousWrite = false;
      this._EntryHeader = buffer;
    }

    private int FigureCrc32()
    {
      if (!this._crcCalculated)
      {
        Stream input = (Stream) null;
        if (this._Source == ZipEntrySource.WriteDelegate)
        {
          CrcCalculatorStream calculatorStream = new CrcCalculatorStream(Stream.Null);
          this._WriteDelegate(this.FileName, (Stream) calculatorStream);
          this._Crc32 = calculatorStream.Crc;
        }
        else if (this._Source != ZipEntrySource.ZipFile)
        {
          if (this._Source == ZipEntrySource.Stream)
          {
            this.PrepSourceStream();
            input = this._sourceStream;
          }
          else if (this._Source == ZipEntrySource.JitStream)
          {
            if (this._sourceStream == null)
              this._sourceStream = this._OpenDelegate(this.FileName);
            this.PrepSourceStream();
            input = this._sourceStream;
          }
          else if (this._Source != ZipEntrySource.ZipOutputStream)
            input = (Stream) File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
          this._Crc32 = new CRC32().GetCrc32(input);
          if (this._sourceStream == null)
            input.Dispose();
        }
        this._crcCalculated = true;
      }
      return this._Crc32;
    }

    private void PrepSourceStream()
    {
      if (this._sourceStream == null)
        throw new ZipException(string.Format("The input stream is null for entry '{0}'.", (object) this.FileName));
      if (this._sourceStreamOriginalPosition.HasValue)
        this._sourceStream.Position = this._sourceStreamOriginalPosition.Value;
      else if (this._sourceStream.CanSeek)
        this._sourceStreamOriginalPosition = new long?(this._sourceStream.Position);
      else if (this.Encryption == EncryptionAlgorithm.PkzipWeak && this._Source != ZipEntrySource.ZipFile && ((int) this._BitField & 8) != 8)
        throw new ZipException("It is not possible to use PKZIP encryption on a non-seekable input stream");
    }

    internal void CopyMetaData(ZipEntry source)
    {
      this.__FileDataPosition = source.__FileDataPosition;
      this.CompressionMethod = source.CompressionMethod;
      this._CompressionMethod_FromZipFile = source._CompressionMethod_FromZipFile;
      this._CompressedFileDataSize = source._CompressedFileDataSize;
      this._UncompressedSize = source._UncompressedSize;
      this._BitField = source._BitField;
      this._Source = source._Source;
      this._LastModified = source._LastModified;
      this._Mtime = source._Mtime;
      this._Atime = source._Atime;
      this._Ctime = source._Ctime;
      this._ntfsTimesAreSet = source._ntfsTimesAreSet;
      this._emitUnixTimes = source._emitUnixTimes;
      this._emitNtfsTimes = source._emitNtfsTimes;
    }

    private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
    {
      if (this._container.ZipFile == null)
        return;
      this._ioOperationCanceled = this._container.ZipFile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
    }

    private void _WriteEntryData(Stream s)
    {
      Stream input = (Stream) null;
      long num1 = -1;
      try
      {
        num1 = s.Position;
      }
      catch (Exception ex)
      {
      }
      try
      {
        long num2 = this.SetInputAndFigureFileLength(ref input);
        CountingStream entryCounter = new CountingStream(s);
        Stream stream1;
        Stream stream2;
        if ((ulong) num2 > 0UL)
        {
          stream1 = this.MaybeApplyEncryption((Stream) entryCounter);
          stream2 = this.MaybeApplyCompression(stream1, num2);
        }
        else
          stream1 = stream2 = (Stream) entryCounter;
        CrcCalculatorStream output = new CrcCalculatorStream(stream2, true);
        if (this._Source == ZipEntrySource.WriteDelegate)
        {
          this._WriteDelegate(this.FileName, (Stream) output);
        }
        else
        {
          byte[] buffer = new byte[this.BufferSize];
          int count;
          while ((uint) (count = SharedUtilities.ReadWithRetry(input, buffer, 0, buffer.Length, this.FileName)) > 0U)
          {
            output.Write(buffer, 0, count);
            this.OnWriteBlock(output.TotalBytesSlurped, num2);
            if (this._ioOperationCanceled)
              break;
          }
        }
        this.FinishOutputStream(s, entryCounter, stream1, stream2, output);
      }
      finally
      {
        if (this._Source == ZipEntrySource.JitStream)
        {
          if (this._CloseDelegate != null)
            this._CloseDelegate(this.FileName, input);
        }
        else if (input is FileStream)
          input.Dispose();
      }
      if (this._ioOperationCanceled)
        return;
      this.__FileDataPosition = num1;
      this.PostProcessOutput(s);
    }

    private long SetInputAndFigureFileLength(ref Stream input)
    {
      long num = -1;
      if (this._Source == ZipEntrySource.Stream)
      {
        this.PrepSourceStream();
        input = this._sourceStream;
        try
        {
          num = this._sourceStream.Length;
        }
        catch (NotSupportedException ex)
        {
        }
      }
      else if (this._Source == ZipEntrySource.ZipFile)
      {
        this._sourceStream = (Stream) this.InternalOpenReader(this._Encryption_FromZipFile == EncryptionAlgorithm.None ? (string) null : this._Password ?? this._container.Password);
        this.PrepSourceStream();
        input = this._sourceStream;
        num = this._sourceStream.Length;
      }
      else if (this._Source == ZipEntrySource.JitStream)
      {
        if (this._sourceStream == null)
          this._sourceStream = this._OpenDelegate(this.FileName);
        this.PrepSourceStream();
        input = this._sourceStream;
        try
        {
          num = this._sourceStream.Length;
        }
        catch (NotSupportedException ex)
        {
        }
      }
      else if (this._Source == ZipEntrySource.FileSystem)
      {
        FileShare share = (FileShare) (3 | 4);
        input = (Stream) File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, share);
        num = input.Length;
      }
      return num;
    }

    internal void FinishOutputStream(
      Stream s,
      CountingStream entryCounter,
      Stream encryptor,
      Stream compressor,
      CrcCalculatorStream output)
    {
      if (output == null)
        return;
      output.Close();
      switch (compressor)
      {
        case DeflateStream _:
          compressor.Close();
          break;
        case ParallelDeflateOutputStream _:
          compressor.Close();
          break;
      }
      encryptor.Flush();
      encryptor.Close();
      this._LengthOfTrailer = 0;
      this._UncompressedSize = output.TotalBytesSlurped;
      this._CompressedFileDataSize = entryCounter.BytesWritten;
      this._CompressedSize = this._CompressedFileDataSize;
      this._Crc32 = output.Crc;
      this.StoreRelativeOffset();
    }

    internal void PostProcessOutput(Stream s)
    {
      CountingStream countingStream = s as CountingStream;
      if (this._UncompressedSize == 0L && this._CompressedSize == 0L)
      {
        if (this._Source == ZipEntrySource.ZipOutputStream)
          return;
        if (this._Password != null)
        {
          int num1 = 0;
          if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
            num1 = 12;
          if (this._Source == ZipEntrySource.ZipOutputStream && !s.CanSeek)
            throw new ZipException("Zero bytes written, encryption in use, and non-seekable output.");
          if ((uint) this.Encryption > 0U)
          {
            s.Seek((long) (-1 * num1), SeekOrigin.Current);
            s.SetLength(s.Position);
            countingStream?.Adjust((long) num1);
            this._LengthOfHeader -= num1;
            this.__FileDataPosition -= (long) num1;
          }
          this._Password = (string) null;
          this._BitField &= (short) -2;
          int num2 = 6;
          byte[] entryHeader1 = this._EntryHeader;
          int index1 = num2;
          int num3 = index1 + 1;
          int num4 = (int) (byte) ((uint) this._BitField & (uint) byte.MaxValue);
          entryHeader1[index1] = (byte) num4;
          byte[] entryHeader2 = this._EntryHeader;
          int index2 = num3;
          int num5 = index2 + 1;
          int num6 = (int) (byte) (((int) this._BitField & 65280) >> 8);
          entryHeader2[index2] = (byte) num6;
        }
        this.CompressionMethod = CompressionMethod.None;
        this.Encryption = EncryptionAlgorithm.None;
      }
      else if (this._zipCrypto_forWrite != null && this.Encryption == EncryptionAlgorithm.PkzipWeak)
        this._CompressedSize += 12L;
      int num7 = 8;
      byte[] entryHeader3 = this._EntryHeader;
      int index3 = num7;
      int num8 = index3 + 1;
      int num9 = (int) (byte) ((uint) this._CompressionMethod & (uint) byte.MaxValue);
      entryHeader3[index3] = (byte) num9;
      byte[] entryHeader4 = this._EntryHeader;
      int index4 = num8;
      int num10 = index4 + 1;
      int num11 = (int) (byte) (((int) this._CompressionMethod & 65280) >> 8);
      entryHeader4[index4] = (byte) num11;
      int num12 = 14;
      byte[] entryHeader5 = this._EntryHeader;
      int index5 = num12;
      int num13 = index5 + 1;
      int num14 = (int) (byte) (this._Crc32 & (int) byte.MaxValue);
      entryHeader5[index5] = (byte) num14;
      byte[] entryHeader6 = this._EntryHeader;
      int index6 = num13;
      int num15 = index6 + 1;
      int num16 = (int) (byte) ((this._Crc32 & 65280) >> 8);
      entryHeader6[index6] = (byte) num16;
      byte[] entryHeader7 = this._EntryHeader;
      int index7 = num15;
      int num17 = index7 + 1;
      int num18 = (int) (byte) ((this._Crc32 & 16711680) >> 16);
      entryHeader7[index7] = (byte) num18;
      byte[] entryHeader8 = this._EntryHeader;
      int index8 = num17;
      int num19 = index8 + 1;
      int num20 = (int) (byte) (((long) this._Crc32 & 4278190080L) >> 24);
      entryHeader8[index8] = (byte) num20;
      this.SetZip64Flags();
      short num21 = (short) ((int) this._EntryHeader[26] + (int) this._EntryHeader[27] * 256);
      short num22 = (short) ((int) this._EntryHeader[28] + (int) this._EntryHeader[29] * 256);
      if (this._OutputUsesZip64.Value)
      {
        this._EntryHeader[4] = (byte) 45;
        this._EntryHeader[5] = (byte) 0;
        for (int index9 = 0; index9 < 8; ++index9)
          this._EntryHeader[num19++] = byte.MaxValue;
        int num23 = 30 + (int) num21;
        byte[] entryHeader9 = this._EntryHeader;
        int index10 = num23;
        int num24 = index10 + 1;
        entryHeader9[index10] = (byte) 1;
        byte[] entryHeader10 = this._EntryHeader;
        int index11 = num24;
        int num25 = index11 + 1;
        entryHeader10[index11] = (byte) 0;
        int destinationIndex1 = num25 + 2;
        Array.Copy((Array) BitConverter.GetBytes(this._UncompressedSize), 0, (Array) this._EntryHeader, destinationIndex1, 8);
        int destinationIndex2 = destinationIndex1 + 8;
        Array.Copy((Array) BitConverter.GetBytes(this._CompressedSize), 0, (Array) this._EntryHeader, destinationIndex2, 8);
      }
      else
      {
        this._EntryHeader[4] = (byte) 20;
        this._EntryHeader[5] = (byte) 0;
        int num26 = 18;
        byte[] entryHeader11 = this._EntryHeader;
        int index12 = num26;
        int num27 = index12 + 1;
        int num28 = (int) (byte) ((ulong) this._CompressedSize & (ulong) byte.MaxValue);
        entryHeader11[index12] = (byte) num28;
        byte[] entryHeader12 = this._EntryHeader;
        int index13 = num27;
        int num29 = index13 + 1;
        int num30 = (int) (byte) ((this._CompressedSize & 65280L) >> 8);
        entryHeader12[index13] = (byte) num30;
        byte[] entryHeader13 = this._EntryHeader;
        int index14 = num29;
        int num31 = index14 + 1;
        int num32 = (int) (byte) ((this._CompressedSize & 16711680L) >> 16);
        entryHeader13[index14] = (byte) num32;
        byte[] entryHeader14 = this._EntryHeader;
        int index15 = num31;
        int num33 = index15 + 1;
        int num34 = (int) (byte) ((this._CompressedSize & 4278190080L) >> 24);
        entryHeader14[index15] = (byte) num34;
        byte[] entryHeader15 = this._EntryHeader;
        int index16 = num33;
        int num35 = index16 + 1;
        int num36 = (int) (byte) ((ulong) this._UncompressedSize & (ulong) byte.MaxValue);
        entryHeader15[index16] = (byte) num36;
        byte[] entryHeader16 = this._EntryHeader;
        int index17 = num35;
        int num37 = index17 + 1;
        int num38 = (int) (byte) ((this._UncompressedSize & 65280L) >> 8);
        entryHeader16[index17] = (byte) num38;
        byte[] entryHeader17 = this._EntryHeader;
        int index18 = num37;
        int num39 = index18 + 1;
        int num40 = (int) (byte) ((this._UncompressedSize & 16711680L) >> 16);
        entryHeader17[index18] = (byte) num40;
        byte[] entryHeader18 = this._EntryHeader;
        int index19 = num39;
        num10 = index19 + 1;
        int num41 = (int) (byte) ((this._UncompressedSize & 4278190080L) >> 24);
        entryHeader18[index19] = (byte) num41;
        if ((uint) num22 > 0U)
        {
          int num42 = 30 + (int) num21;
          if ((short) ((int) this._EntryHeader[num42 + 2] + (int) this._EntryHeader[num42 + 3] * 256) == (short) 16)
          {
            byte[] entryHeader19 = this._EntryHeader;
            int index20 = num42;
            int num43 = index20 + 1;
            entryHeader19[index20] = (byte) 153;
            byte[] entryHeader20 = this._EntryHeader;
            int index21 = num43;
            num10 = index21 + 1;
            entryHeader20[index21] = (byte) 153;
          }
        }
      }
      if (((int) this._BitField & 8) != 8 || this._Source == ZipEntrySource.ZipOutputStream && s.CanSeek)
      {
        if (s is ZipSegmentedStream zipSegmentedStream2 && (int) this._diskNumber != (int) zipSegmentedStream2.CurrentSegment)
        {
          using (Stream stream = ZipSegmentedStream.ForUpdate(this._container.ZipFile.Name, this._diskNumber))
          {
            stream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
            stream.Write(this._EntryHeader, 0, this._EntryHeader.Length);
          }
        }
        else
        {
          s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
          s.Write(this._EntryHeader, 0, this._EntryHeader.Length);
          countingStream?.Adjust((long) this._EntryHeader.Length);
          s.Seek(this._CompressedSize, SeekOrigin.Current);
        }
      }
      if (((int) this._BitField & 8) != 8 || this.IsDirectory)
        return;
      byte[] buffer = new byte[16 + (this._OutputUsesZip64.Value ? 8 : 0)];
      int destinationIndex3 = 0;
      Array.Copy((Array) BitConverter.GetBytes(134695760), 0, (Array) buffer, destinationIndex3, 4);
      int destinationIndex4 = destinationIndex3 + 4;
      Array.Copy((Array) BitConverter.GetBytes(this._Crc32), 0, (Array) buffer, destinationIndex4, 4);
      int destinationIndex5 = destinationIndex4 + 4;
      if (this._OutputUsesZip64.Value)
      {
        Array.Copy((Array) BitConverter.GetBytes(this._CompressedSize), 0, (Array) buffer, destinationIndex5, 8);
        int destinationIndex6 = destinationIndex5 + 8;
        Array.Copy((Array) BitConverter.GetBytes(this._UncompressedSize), 0, (Array) buffer, destinationIndex6, 8);
        num10 = destinationIndex6 + 8;
      }
      else
      {
        byte[] numArray1 = buffer;
        int index22 = destinationIndex5;
        int num44 = index22 + 1;
        int num45 = (int) (byte) ((ulong) this._CompressedSize & (ulong) byte.MaxValue);
        numArray1[index22] = (byte) num45;
        byte[] numArray2 = buffer;
        int index23 = num44;
        int num46 = index23 + 1;
        int num47 = (int) (byte) ((this._CompressedSize & 65280L) >> 8);
        numArray2[index23] = (byte) num47;
        byte[] numArray3 = buffer;
        int index24 = num46;
        int num48 = index24 + 1;
        int num49 = (int) (byte) ((this._CompressedSize & 16711680L) >> 16);
        numArray3[index24] = (byte) num49;
        byte[] numArray4 = buffer;
        int index25 = num48;
        int num50 = index25 + 1;
        int num51 = (int) (byte) ((this._CompressedSize & 4278190080L) >> 24);
        numArray4[index25] = (byte) num51;
        byte[] numArray5 = buffer;
        int index26 = num50;
        int num52 = index26 + 1;
        int num53 = (int) (byte) ((ulong) this._UncompressedSize & (ulong) byte.MaxValue);
        numArray5[index26] = (byte) num53;
        byte[] numArray6 = buffer;
        int index27 = num52;
        int num54 = index27 + 1;
        int num55 = (int) (byte) ((this._UncompressedSize & 65280L) >> 8);
        numArray6[index27] = (byte) num55;
        byte[] numArray7 = buffer;
        int index28 = num54;
        int num56 = index28 + 1;
        int num57 = (int) (byte) ((this._UncompressedSize & 16711680L) >> 16);
        numArray7[index28] = (byte) num57;
        byte[] numArray8 = buffer;
        int index29 = num56;
        num10 = index29 + 1;
        int num58 = (int) (byte) ((this._UncompressedSize & 4278190080L) >> 24);
        numArray8[index29] = (byte) num58;
      }
      s.Write(buffer, 0, buffer.Length);
      this._LengthOfTrailer += buffer.Length;
    }

    private void SetZip64Flags()
    {
      this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long) uint.MaxValue || this._UncompressedSize >= (long) uint.MaxValue || this._RelativeOffsetOfLocalHeader >= (long) uint.MaxValue);
      if (this._container.Zip64 == Zip64Option.Default && this._entryRequiresZip64.Value)
        throw new ZipException("Compressed or Uncompressed size, or offset exceeds the maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
      this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
    }

    internal void PrepOutputStream(
      Stream s,
      long streamLength,
      out CountingStream outputCounter,
      out Stream encryptor,
      out Stream compressor,
      out CrcCalculatorStream output)
    {
      outputCounter = new CountingStream(s);
      if ((ulong) streamLength > 0UL)
      {
        encryptor = this.MaybeApplyEncryption((Stream) outputCounter);
        compressor = this.MaybeApplyCompression(encryptor, streamLength);
      }
      else
        encryptor = compressor = (Stream) outputCounter;
      output = new CrcCalculatorStream(compressor, true);
    }

    private Stream MaybeApplyCompression(Stream s, long streamLength)
    {
      if (this._CompressionMethod != (short) 8 || (uint) this.CompressionLevel <= 0U)
        return s;
      if (this._container.ParallelDeflateThreshold == 0L || streamLength > this._container.ParallelDeflateThreshold && this._container.ParallelDeflateThreshold > 0L)
      {
        if (this._container.ParallelDeflater == null)
        {
          this._container.ParallelDeflater = new ParallelDeflateOutputStream(s, this.CompressionLevel, this._container.Strategy, true);
          if (this._container.CodecBufferSize > 0)
            this._container.ParallelDeflater.BufferSize = this._container.CodecBufferSize;
          if (this._container.ParallelDeflateMaxBufferPairs > 0)
            this._container.ParallelDeflater.MaxBufferPairs = this._container.ParallelDeflateMaxBufferPairs;
        }
        ParallelDeflateOutputStream parallelDeflater = this._container.ParallelDeflater;
        parallelDeflater.Reset(s);
        return (Stream) parallelDeflater;
      }
      DeflateStream deflateStream = new DeflateStream(s, CompressionMode.Compress, this.CompressionLevel, true);
      if (this._container.CodecBufferSize > 0)
        deflateStream.BufferSize = this._container.CodecBufferSize;
      deflateStream.Strategy = this._container.Strategy;
      return (Stream) deflateStream;
    }

    private Stream MaybeApplyEncryption(Stream s) => this.Encryption == EncryptionAlgorithm.PkzipWeak ? (Stream) new ZipCipherStream(s, this._zipCrypto_forWrite, CryptoMode.Encrypt) : s;

    private void OnZipErrorWhileSaving(Exception e)
    {
      if (this._container.ZipFile == null)
        return;
      this._ioOperationCanceled = this._container.ZipFile.OnZipErrorSaving(this, e);
    }

    internal void Write(Stream s)
    {
      CountingStream countingStream = s as CountingStream;
      ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
      bool flag1 = false;
      do
      {
        try
        {
          if (this._Source == ZipEntrySource.ZipFile && !this._restreamRequiredOnSave)
          {
            this.CopyThroughOneEntry(s);
            break;
          }
          if (this.IsDirectory)
          {
            this.WriteHeader(s, 1);
            this.StoreRelativeOffset();
            this._entryRequiresZip64 = new bool?(this._RelativeOffsetOfLocalHeader >= (long) uint.MaxValue);
            this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
            if (zipSegmentedStream == null)
              break;
            this._diskNumber = zipSegmentedStream.CurrentSegment;
            break;
          }
          int cycle = 0;
          bool flag2;
          do
          {
            ++cycle;
            this.WriteHeader(s, cycle);
            this.WriteSecurityMetadata(s);
            this._WriteEntryData(s);
            this._TotalEntrySize = (long) this._LengthOfHeader + this._CompressedFileDataSize + (long) this._LengthOfTrailer;
            flag2 = cycle <= 1 && s.CanSeek && this.WantReadAgain();
            if (flag2)
            {
              if (zipSegmentedStream != null)
                zipSegmentedStream.TruncateBackward(this._diskNumber, this._RelativeOffsetOfLocalHeader);
              else
                s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
              s.SetLength(s.Position);
              countingStream?.Adjust(this._TotalEntrySize);
            }
          }
          while (flag2);
          this._skippedDuringSave = false;
          flag1 = true;
        }
        catch (Exception ex)
        {
          ZipErrorAction zipErrorAction = this.ZipErrorAction;
          int num1 = 0;
          while (this.ZipErrorAction != ZipErrorAction.Throw)
          {
            if (this.ZipErrorAction == ZipErrorAction.Skip || this.ZipErrorAction == ZipErrorAction.Retry)
            {
              long num2 = countingStream != null ? countingStream.ComputedPosition : s.Position;
              long offset = num2 - this._future_ROLH;
              if (offset > 0L)
              {
                s.Seek(offset, SeekOrigin.Current);
                long position = s.Position;
                s.SetLength(s.Position);
                countingStream?.Adjust(num2 - position);
              }
              if (this.ZipErrorAction == ZipErrorAction.Skip)
              {
                this.WriteStatus("Skipping file {0} (exception: {1})", (object) this.LocalFileName, (object) ex.ToString());
                this._skippedDuringSave = true;
                flag1 = true;
                goto label_36;
              }
              else
              {
                this.ZipErrorAction = zipErrorAction;
                goto label_36;
              }
            }
            else if (num1 > 0)
            {
              throw;
            }
            else
            {
              if (this.ZipErrorAction == ZipErrorAction.InvokeErrorEvent)
              {
                this.OnZipErrorWhileSaving(ex);
                if (this._ioOperationCanceled)
                {
                  flag1 = true;
                  goto label_36;
                }
              }
              ++num1;
            }
          }
          throw;
        }
label_36:;
      }
      while (!flag1);
    }

    internal void StoreRelativeOffset() => this._RelativeOffsetOfLocalHeader = this._future_ROLH;

    internal void NotifySaveComplete()
    {
      this._Encryption_FromZipFile = this._Encryption;
      this._CompressionMethod_FromZipFile = this._CompressionMethod;
      this._restreamRequiredOnSave = false;
      this._metadataChanged = false;
      this._Source = ZipEntrySource.ZipFile;
    }

    internal void WriteSecurityMetadata(Stream outstream)
    {
      if (this.Encryption == EncryptionAlgorithm.None)
        return;
      string password = this._Password;
      if (this._Source == ZipEntrySource.ZipFile && password == null)
        password = this._container.Password;
      if (password == null)
      {
        this._zipCrypto_forWrite = (ZipCrypto) null;
      }
      else
      {
        if (this.Encryption != EncryptionAlgorithm.PkzipWeak)
          return;
        this._zipCrypto_forWrite = ZipCrypto.ForWrite(password);
        Random random = new Random();
        byte[] numArray = new byte[12];
        random.NextBytes(numArray);
        if (((int) this._BitField & 8) == 8)
        {
          this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
          numArray[11] = (byte) (this._TimeBlob >> 8 & (int) byte.MaxValue);
        }
        else
        {
          this.FigureCrc32();
          numArray[11] = (byte) (this._Crc32 >> 24 & (int) byte.MaxValue);
        }
        byte[] buffer = this._zipCrypto_forWrite.EncryptMessage(numArray, numArray.Length);
        outstream.Write(buffer, 0, buffer.Length);
        this._LengthOfHeader += buffer.Length;
      }
    }

    private void CopyThroughOneEntry(Stream outStream)
    {
      if (this.LengthOfHeader == 0)
        throw new BadStateException("Bad header length.");
      if (this._metadataChanged || this.ArchiveStream is ZipSegmentedStream || outStream is ZipSegmentedStream || this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default || !this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always)
        this.CopyThroughWithRecompute(outStream);
      else
        this.CopyThroughWithNoChange(outStream);
      this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long) uint.MaxValue || this._UncompressedSize >= (long) uint.MaxValue || this._RelativeOffsetOfLocalHeader >= (long) uint.MaxValue);
      this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
    }

    private void CopyThroughWithRecompute(Stream outstream)
    {
      byte[] buffer1 = new byte[this.BufferSize];
      CountingStream countingStream = new CountingStream(this.ArchiveStream);
      long offsetOfLocalHeader = this._RelativeOffsetOfLocalHeader;
      int lengthOfHeader = this.LengthOfHeader;
      this.WriteHeader(outstream, 0);
      this.StoreRelativeOffset();
      if (!this.FileName.EndsWith("/"))
      {
        long num = offsetOfLocalHeader + (long) lengthOfHeader;
        int cryptoHeaderBytes = ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
        long offset = num - (long) cryptoHeaderBytes;
        this._LengthOfHeader += cryptoHeaderBytes;
        countingStream.Seek(offset, SeekOrigin.Begin);
        long compressedSize = this._CompressedSize;
        while (compressedSize > 0L)
        {
          int count1 = compressedSize > (long) buffer1.Length ? buffer1.Length : (int) compressedSize;
          int count2 = countingStream.Read(buffer1, 0, count1);
          outstream.Write(buffer1, 0, count2);
          compressedSize -= (long) count2;
          this.OnWriteBlock(countingStream.BytesRead, this._CompressedSize);
          if (this._ioOperationCanceled)
            break;
        }
        if (((int) this._BitField & 8) == 8)
        {
          int count = 16;
          if (this._InputUsesZip64)
            count += 8;
          byte[] buffer2 = new byte[count];
          countingStream.Read(buffer2, 0, count);
          if (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default)
          {
            outstream.Write(buffer2, 0, 8);
            if (this._CompressedSize > (long) uint.MaxValue)
              throw new InvalidOperationException("ZIP64 is required");
            outstream.Write(buffer2, 8, 4);
            if (this._UncompressedSize > (long) uint.MaxValue)
              throw new InvalidOperationException("ZIP64 is required");
            outstream.Write(buffer2, 16, 4);
            this._LengthOfTrailer -= 8;
          }
          else if (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always)
          {
            byte[] buffer3 = new byte[4];
            outstream.Write(buffer2, 0, 8);
            outstream.Write(buffer2, 8, 4);
            outstream.Write(buffer3, 0, 4);
            outstream.Write(buffer2, 12, 4);
            outstream.Write(buffer3, 0, 4);
            this._LengthOfTrailer += 8;
          }
          else
            outstream.Write(buffer2, 0, count);
        }
      }
      this._TotalEntrySize = (long) this._LengthOfHeader + this._CompressedFileDataSize + (long) this._LengthOfTrailer;
    }

    private void CopyThroughWithNoChange(Stream outstream)
    {
      byte[] buffer = new byte[this.BufferSize];
      CountingStream countingStream1 = new CountingStream(this.ArchiveStream);
      countingStream1.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
      if (this._TotalEntrySize == 0L)
        this._TotalEntrySize = (long) this._LengthOfHeader + this._CompressedFileDataSize + (long) this._LengthOfTrailer;
      this._RelativeOffsetOfLocalHeader = outstream is CountingStream countingStream2 ? countingStream2.ComputedPosition : outstream.Position;
      long totalEntrySize = this._TotalEntrySize;
      while (totalEntrySize > 0L)
      {
        int count1 = totalEntrySize > (long) buffer.Length ? buffer.Length : (int) totalEntrySize;
        int count2 = countingStream1.Read(buffer, 0, count1);
        outstream.Write(buffer, 0, count2);
        totalEntrySize -= (long) count2;
        this.OnWriteBlock(countingStream1.BytesRead, this._TotalEntrySize);
        if (this._ioOperationCanceled)
          break;
      }
    }

    [Conditional("Trace")]
    private void TraceWriteLine(string format, params object[] varParams)
    {
      lock (this._outputLock)
      {
        int hashCode = Thread.CurrentThread.GetHashCode();
        Console.ForegroundColor = (ConsoleColor) (hashCode % 8 + 8);
        Console.Write("{0:000} ZipEntry.Write ", (object) hashCode);
        Console.WriteLine(format, varParams);
        Console.ResetColor();
      }
    }

    private class CopyHelper
    {
      private static Regex re = new Regex(" \\(copy (\\d+)\\)$");
      private static int callCount = 0;

      internal static string AppendCopyToFileName(string f)
      {
        ++ZipEntry.CopyHelper.callCount;
        if (ZipEntry.CopyHelper.callCount > 25)
          throw new OverflowException("overflow while creating filename");
        int num1 = 1;
        int num2 = f.LastIndexOf(".");
        if (num2 == -1)
        {
          Match match = ZipEntry.CopyHelper.re.Match(f);
          if (match.Success)
          {
            string str = string.Format(" (copy {0})", (object) (int.Parse(match.Groups[1].Value) + 1));
            f = f.Substring(0, match.Index) + str;
          }
          else
          {
            string str = string.Format(" (copy {0})", (object) num1);
            f += str;
          }
        }
        else
        {
          Match match = ZipEntry.CopyHelper.re.Match(f.Substring(0, num2));
          if (match.Success)
          {
            string str = string.Format(" (copy {0})", (object) (int.Parse(match.Groups[1].Value) + 1));
            f = f.Substring(0, match.Index) + str + f.Substring(num2);
          }
          else
          {
            string str = string.Format(" (copy {0})", (object) num1);
            f = f.Substring(0, num2) + str + f.Substring(num2);
          }
        }
        return f;
      }
    }

    private delegate T Func<T>();
  }
}
