// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.FileDataBlock
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Be.Windows.Forms
{
  internal sealed class FileDataBlock : DataBlock
  {
    private long _length;
    private long _fileOffset;

    public FileDataBlock(long fileOffset, long length)
    {
      this._fileOffset = fileOffset;
      this._length = length;
    }

    public long FileOffset => this._fileOffset;

    public override long Length => this._length;

    public void SetFileOffset(long value) => this._fileOffset = value;

    public void RemoveBytesFromEnd(long count)
    {
      if (count > this._length)
        throw new ArgumentOutOfRangeException(nameof (count));
      this._length -= count;
    }

    public void RemoveBytesFromStart(long count)
    {
      if (count > this._length)
        throw new ArgumentOutOfRangeException(nameof (count));
      this._fileOffset += count;
      this._length -= count;
    }

    public override void RemoveBytes(long position, long count)
    {
      if (position > this._length)
        throw new ArgumentOutOfRangeException(nameof (position));
      if (position + count > this._length)
        throw new ArgumentOutOfRangeException(nameof (count));
      long num = position;
      long fileOffset1 = this._fileOffset;
      long length = this._length - count - num;
      long fileOffset2 = this._fileOffset + position + count;
      if (num > 0L && length > 0L)
      {
        this._fileOffset = fileOffset1;
        this._length = num;
        this._map.AddAfter((DataBlock) this, (DataBlock) new FileDataBlock(fileOffset2, length));
      }
      else if (num > 0L)
      {
        this._fileOffset = fileOffset1;
        this._length = num;
      }
      else
      {
        this._fileOffset = fileOffset2;
        this._length = length;
      }
    }
  }
}
