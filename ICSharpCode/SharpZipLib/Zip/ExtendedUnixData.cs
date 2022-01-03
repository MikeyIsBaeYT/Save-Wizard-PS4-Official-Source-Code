// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.ExtendedUnixData
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class ExtendedUnixData : ITaggedData
  {
    private ExtendedUnixData.Flags _flags;
    private DateTime _modificationTime = new DateTime(1970, 1, 1);
    private DateTime _lastAccessTime = new DateTime(1970, 1, 1);
    private DateTime _createTime = new DateTime(1970, 1, 1);

    public short TagID => 21589;

    public void SetData(byte[] data, int index, int count)
    {
      using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
      {
        using (ZipHelperStream zipHelperStream = new ZipHelperStream((Stream) memoryStream))
        {
          this._flags = (ExtendedUnixData.Flags) zipHelperStream.ReadByte();
          DateTime dateTime;
          if ((this._flags & ExtendedUnixData.Flags.ModificationTime) != (ExtendedUnixData.Flags) 0 && count >= 5)
          {
            int seconds = zipHelperStream.ReadLEInt();
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0);
            this._modificationTime = dateTime.ToLocalTime();
          }
          if ((uint) (this._flags & ExtendedUnixData.Flags.AccessTime) > 0U)
          {
            int seconds = zipHelperStream.ReadLEInt();
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            dateTime = dateTime.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0);
            this._lastAccessTime = dateTime.ToLocalTime();
          }
          if ((uint) (this._flags & ExtendedUnixData.Flags.CreateTime) <= 0U)
            return;
          int seconds1 = zipHelperStream.ReadLEInt();
          dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
          dateTime = dateTime.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds1, 0);
          this._createTime = dateTime.ToLocalTime();
        }
      }
    }

    public byte[] GetData()
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ZipHelperStream zipHelperStream = new ZipHelperStream((Stream) memoryStream))
        {
          zipHelperStream.IsStreamOwner = false;
          zipHelperStream.WriteByte((byte) this._flags);
          if ((uint) (this._flags & ExtendedUnixData.Flags.ModificationTime) > 0U)
          {
            int totalSeconds = (int) (this._modificationTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
            zipHelperStream.WriteLEInt(totalSeconds);
          }
          if ((uint) (this._flags & ExtendedUnixData.Flags.AccessTime) > 0U)
          {
            int totalSeconds = (int) (this._lastAccessTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
            zipHelperStream.WriteLEInt(totalSeconds);
          }
          if ((uint) (this._flags & ExtendedUnixData.Flags.CreateTime) > 0U)
          {
            int totalSeconds = (int) (this._createTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
            zipHelperStream.WriteLEInt(totalSeconds);
          }
          return memoryStream.ToArray();
        }
      }
    }

    public static bool IsValidValue(DateTime value) => value >= new DateTime(1901, 12, 13, 20, 45, 52) || value <= new DateTime(2038, 1, 19, 3, 14, 7);

    public DateTime ModificationTime
    {
      get => this._modificationTime;
      set
      {
        if (!ExtendedUnixData.IsValidValue(value))
          throw new ArgumentOutOfRangeException(nameof (value));
        this._flags |= ExtendedUnixData.Flags.ModificationTime;
        this._modificationTime = value;
      }
    }

    public DateTime AccessTime
    {
      get => this._lastAccessTime;
      set
      {
        if (!ExtendedUnixData.IsValidValue(value))
          throw new ArgumentOutOfRangeException(nameof (value));
        this._flags |= ExtendedUnixData.Flags.AccessTime;
        this._lastAccessTime = value;
      }
    }

    public DateTime CreateTime
    {
      get => this._createTime;
      set
      {
        if (!ExtendedUnixData.IsValidValue(value))
          throw new ArgumentOutOfRangeException(nameof (value));
        this._flags |= ExtendedUnixData.Flags.CreateTime;
        this._createTime = value;
      }
    }

    private ExtendedUnixData.Flags Include
    {
      get => this._flags;
      set => this._flags = value;
    }

    [System.Flags]
    public enum Flags : byte
    {
      ModificationTime = 1,
      AccessTime = 2,
      CreateTime = 4,
    }
  }
}
