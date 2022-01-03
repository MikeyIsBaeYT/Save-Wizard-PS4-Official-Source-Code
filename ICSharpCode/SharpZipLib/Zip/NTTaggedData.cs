// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.NTTaggedData
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class NTTaggedData : ITaggedData
  {
    private DateTime _lastAccessTime = DateTime.FromFileTime(0L);
    private DateTime _lastModificationTime = DateTime.FromFileTime(0L);
    private DateTime _createTime = DateTime.FromFileTime(0L);

    public short TagID => 10;

    public void SetData(byte[] data, int index, int count)
    {
      using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
      {
        using (ZipHelperStream zipHelperStream = new ZipHelperStream((Stream) memoryStream))
        {
          zipHelperStream.ReadLEInt();
          while (zipHelperStream.Position < zipHelperStream.Length)
          {
            int num1 = zipHelperStream.ReadLEShort();
            int num2 = zipHelperStream.ReadLEShort();
            if (num1 == 1)
            {
              if (num2 < 24)
                break;
              this._lastModificationTime = DateTime.FromFileTime(zipHelperStream.ReadLELong());
              this._lastAccessTime = DateTime.FromFileTime(zipHelperStream.ReadLELong());
              this._createTime = DateTime.FromFileTime(zipHelperStream.ReadLELong());
              break;
            }
            zipHelperStream.Seek((long) num2, SeekOrigin.Current);
          }
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
          zipHelperStream.WriteLEInt(0);
          zipHelperStream.WriteLEShort(1);
          zipHelperStream.WriteLEShort(24);
          zipHelperStream.WriteLELong(this._lastModificationTime.ToFileTime());
          zipHelperStream.WriteLELong(this._lastAccessTime.ToFileTime());
          zipHelperStream.WriteLELong(this._createTime.ToFileTime());
          return memoryStream.ToArray();
        }
      }
    }

    public static bool IsValidValue(DateTime value)
    {
      bool flag = true;
      try
      {
        value.ToFileTimeUtc();
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    public DateTime LastModificationTime
    {
      get => this._lastModificationTime;
      set => this._lastModificationTime = NTTaggedData.IsValidValue(value) ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public DateTime CreateTime
    {
      get => this._createTime;
      set => this._createTime = NTTaggedData.IsValidValue(value) ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public DateTime LastAccessTime
    {
      get => this._lastAccessTime;
      set => this._lastAccessTime = NTTaggedData.IsValidValue(value) ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
