// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Streams.StreamManipulator
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
  public class StreamManipulator
  {
    private byte[] window_;
    private int windowStart_;
    private int windowEnd_;
    private uint buffer_;
    private int bitsInBuffer_;

    public int PeekBits(int bitCount)
    {
      if (this.bitsInBuffer_ < bitCount)
      {
        if (this.windowStart_ == this.windowEnd_)
          return -1;
        this.buffer_ |= (uint) (((int) this.window_[this.windowStart_++] & (int) byte.MaxValue | ((int) this.window_[this.windowStart_++] & (int) byte.MaxValue) << 8) << this.bitsInBuffer_);
        this.bitsInBuffer_ += 16;
      }
      return (int) ((long) this.buffer_ & (long) ((1 << bitCount) - 1));
    }

    public void DropBits(int bitCount)
    {
      this.buffer_ >>= bitCount;
      this.bitsInBuffer_ -= bitCount;
    }

    public int GetBits(int bitCount)
    {
      int num = this.PeekBits(bitCount);
      if (num >= 0)
        this.DropBits(bitCount);
      return num;
    }

    public int AvailableBits => this.bitsInBuffer_;

    public int AvailableBytes => this.windowEnd_ - this.windowStart_ + (this.bitsInBuffer_ >> 3);

    public void SkipToByteBoundary()
    {
      this.buffer_ >>= this.bitsInBuffer_ & 7;
      this.bitsInBuffer_ &= -8;
    }

    public bool IsNeedingInput => this.windowStart_ == this.windowEnd_;

    public int CopyBytes(byte[] output, int offset, int length)
    {
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      if ((uint) (this.bitsInBuffer_ & 7) > 0U)
        throw new InvalidOperationException("Bit buffer is not byte aligned!");
      int num1 = 0;
      while (this.bitsInBuffer_ > 0 && length > 0)
      {
        output[offset++] = (byte) this.buffer_;
        this.buffer_ >>= 8;
        this.bitsInBuffer_ -= 8;
        --length;
        ++num1;
      }
      if (length == 0)
        return num1;
      int num2 = this.windowEnd_ - this.windowStart_;
      if (length > num2)
        length = num2;
      Array.Copy((Array) this.window_, this.windowStart_, (Array) output, offset, length);
      this.windowStart_ += length;
      if ((uint) (this.windowStart_ - this.windowEnd_ & 1) > 0U)
      {
        this.buffer_ = (uint) this.window_[this.windowStart_++] & (uint) byte.MaxValue;
        this.bitsInBuffer_ = 8;
      }
      return num1 + length;
    }

    public void Reset()
    {
      this.buffer_ = 0U;
      this.windowStart_ = this.windowEnd_ = this.bitsInBuffer_ = 0;
    }

    public void SetInput(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), "Cannot be negative");
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), "Cannot be negative");
      if (this.windowStart_ < this.windowEnd_)
        throw new InvalidOperationException("Old input was not completely processed");
      int num = offset + count;
      if (offset > num || num > buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (count));
      if ((uint) (count & 1) > 0U)
      {
        this.buffer_ |= (uint) (((int) buffer[offset++] & (int) byte.MaxValue) << this.bitsInBuffer_);
        this.bitsInBuffer_ += 8;
      }
      this.window_ = buffer;
      this.windowStart_ = offset;
      this.windowEnd_ = num;
    }
  }
}
