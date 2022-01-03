// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputBuffer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
  public class InflaterInputBuffer
  {
    private int rawLength;
    private byte[] rawData;
    private int clearTextLength;
    private byte[] clearText;
    private byte[] internalClearText;
    private int available;
    private ICryptoTransform cryptoTransform;
    private Stream inputStream;

    public InflaterInputBuffer(Stream stream)
      : this(stream, 4096)
    {
    }

    public InflaterInputBuffer(Stream stream, int bufferSize)
    {
      this.inputStream = stream;
      if (bufferSize < 1024)
        bufferSize = 1024;
      this.rawData = new byte[bufferSize];
      this.clearText = this.rawData;
    }

    public int RawLength => this.rawLength;

    public byte[] RawData => this.rawData;

    public int ClearTextLength => this.clearTextLength;

    public byte[] ClearText => this.clearText;

    public int Available
    {
      get => this.available;
      set => this.available = value;
    }

    public void SetInflaterInput(Inflater inflater)
    {
      if (this.available <= 0)
        return;
      inflater.SetInput(this.clearText, this.clearTextLength - this.available, this.available);
      this.available = 0;
    }

    public void Fill()
    {
      this.rawLength = 0;
      int num;
      for (int length = this.rawData.Length; length > 0; length -= num)
      {
        num = this.inputStream.Read(this.rawData, this.rawLength, length);
        if (num > 0)
          this.rawLength += num;
        else
          break;
      }
      this.clearTextLength = this.cryptoTransform == null ? this.rawLength : this.cryptoTransform.TransformBlock(this.rawData, 0, this.rawLength, this.clearText, 0);
      this.available = this.clearTextLength;
    }

    public int ReadRawBuffer(byte[] buffer) => this.ReadRawBuffer(buffer, 0, buffer.Length);

    public int ReadRawBuffer(byte[] outBuffer, int offset, int length)
    {
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      int destinationIndex = offset;
      int val1 = length;
      while (val1 > 0)
      {
        if (this.available <= 0)
        {
          this.Fill();
          if (this.available <= 0)
            return 0;
        }
        int length1 = Math.Min(val1, this.available);
        Array.Copy((Array) this.rawData, this.rawLength - this.available, (Array) outBuffer, destinationIndex, length1);
        destinationIndex += length1;
        val1 -= length1;
        this.available -= length1;
      }
      return length;
    }

    public int ReadClearTextBuffer(byte[] outBuffer, int offset, int length)
    {
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length));
      int destinationIndex = offset;
      int val1 = length;
      while (val1 > 0)
      {
        if (this.available <= 0)
        {
          this.Fill();
          if (this.available <= 0)
            return 0;
        }
        int length1 = Math.Min(val1, this.available);
        Array.Copy((Array) this.clearText, this.clearTextLength - this.available, (Array) outBuffer, destinationIndex, length1);
        destinationIndex += length1;
        val1 -= length1;
        this.available -= length1;
      }
      return length;
    }

    public int ReadLeByte()
    {
      if (this.available <= 0)
      {
        this.Fill();
        if (this.available <= 0)
          throw new ZipException("EOF in header");
      }
      byte num = this.rawData[this.rawLength - this.available];
      --this.available;
      return (int) num;
    }

    public int ReadLeShort() => this.ReadLeByte() | this.ReadLeByte() << 8;

    public int ReadLeInt() => this.ReadLeShort() | this.ReadLeShort() << 16;

    public long ReadLeLong() => (long) (uint) this.ReadLeInt() | (long) this.ReadLeInt() << 32;

    public ICryptoTransform CryptoTransform
    {
      set
      {
        this.cryptoTransform = value;
        if (this.cryptoTransform != null)
        {
          if (this.rawData == this.clearText)
          {
            if (this.internalClearText == null)
              this.internalClearText = new byte[this.rawData.Length];
            this.clearText = this.internalClearText;
          }
          this.clearTextLength = this.rawLength;
          if (this.available <= 0)
            return;
          this.cryptoTransform.TransformBlock(this.rawData, this.rawLength - this.available, this.available, this.clearText, this.rawLength - this.available);
        }
        else
        {
          this.clearText = this.rawData;
          this.clearTextLength = this.rawLength;
        }
      }
    }
  }
}
