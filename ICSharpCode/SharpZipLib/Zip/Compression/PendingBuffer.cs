// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.PendingBuffer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
  public class PendingBuffer
  {
    private byte[] buffer_;
    private int start;
    private int end;
    private uint bits;
    private int bitCount;

    public PendingBuffer()
      : this(4096)
    {
    }

    public PendingBuffer(int bufferSize) => this.buffer_ = new byte[bufferSize];

    public void Reset() => this.start = this.end = this.bitCount = 0;

    public void WriteByte(int value) => this.buffer_[this.end++] = (byte) value;

    public void WriteShort(int value)
    {
      this.buffer_[this.end++] = (byte) value;
      this.buffer_[this.end++] = (byte) (value >> 8);
    }

    public void WriteInt(int value)
    {
      this.buffer_[this.end++] = (byte) value;
      this.buffer_[this.end++] = (byte) (value >> 8);
      this.buffer_[this.end++] = (byte) (value >> 16);
      this.buffer_[this.end++] = (byte) (value >> 24);
    }

    public void WriteBlock(byte[] block, int offset, int length)
    {
      Array.Copy((Array) block, offset, (Array) this.buffer_, this.end, length);
      this.end += length;
    }

    public int BitCount => this.bitCount;

    public void AlignToByte()
    {
      if (this.bitCount > 0)
      {
        this.buffer_[this.end++] = (byte) this.bits;
        if (this.bitCount > 8)
          this.buffer_[this.end++] = (byte) (this.bits >> 8);
      }
      this.bits = 0U;
      this.bitCount = 0;
    }

    public void WriteBits(int b, int count)
    {
      this.bits |= (uint) (b << this.bitCount);
      this.bitCount += count;
      if (this.bitCount < 16)
        return;
      this.buffer_[this.end++] = (byte) this.bits;
      this.buffer_[this.end++] = (byte) (this.bits >> 8);
      this.bits >>= 16;
      this.bitCount -= 16;
    }

    public void WriteShortMSB(int s)
    {
      this.buffer_[this.end++] = (byte) (s >> 8);
      this.buffer_[this.end++] = (byte) s;
    }

    public bool IsFlushed => this.end == 0;

    public int Flush(byte[] output, int offset, int length)
    {
      if (this.bitCount >= 8)
      {
        this.buffer_[this.end++] = (byte) this.bits;
        this.bits >>= 8;
        this.bitCount -= 8;
      }
      if (length > this.end - this.start)
      {
        length = this.end - this.start;
        Array.Copy((Array) this.buffer_, this.start, (Array) output, offset, length);
        this.start = 0;
        this.end = 0;
      }
      else
      {
        Array.Copy((Array) this.buffer_, this.start, (Array) output, offset, length);
        this.start += length;
      }
      return length;
    }

    public byte[] ToByteArray()
    {
      byte[] numArray = new byte[this.end - this.start];
      Array.Copy((Array) this.buffer_, this.start, (Array) numArray, 0, numArray.Length);
      this.start = 0;
      this.end = 0;
      return numArray;
    }
  }
}
