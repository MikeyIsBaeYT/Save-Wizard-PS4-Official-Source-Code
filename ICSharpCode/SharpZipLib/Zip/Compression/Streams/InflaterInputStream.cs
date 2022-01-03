// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
  public class InflaterInputStream : Stream
  {
    protected Inflater inf;
    protected InflaterInputBuffer inputBuffer;
    private Stream baseInputStream;
    protected long csize;
    private bool isClosed;
    private bool isStreamOwner = true;

    public InflaterInputStream(Stream baseInputStream)
      : this(baseInputStream, new Inflater(), 4096)
    {
    }

    public InflaterInputStream(Stream baseInputStream, Inflater inf)
      : this(baseInputStream, inf, 4096)
    {
    }

    public InflaterInputStream(Stream baseInputStream, Inflater inflater, int bufferSize)
    {
      if (baseInputStream == null)
        throw new ArgumentNullException(nameof (baseInputStream));
      if (inflater == null)
        throw new ArgumentNullException(nameof (inflater));
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      this.baseInputStream = baseInputStream;
      this.inf = inflater;
      this.inputBuffer = new InflaterInputBuffer(baseInputStream, bufferSize);
    }

    public bool IsStreamOwner
    {
      get => this.isStreamOwner;
      set => this.isStreamOwner = value;
    }

    public long Skip(long count)
    {
      if (count <= 0L)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (this.baseInputStream.CanSeek)
      {
        this.baseInputStream.Seek(count, SeekOrigin.Current);
        return count;
      }
      int count1 = 2048;
      if (count < (long) count1)
        count1 = (int) count;
      byte[] buffer = new byte[count1];
      int num1 = 1;
      long num2;
      for (num2 = count; num2 > 0L && num1 > 0; num2 -= (long) num1)
      {
        if (num2 < (long) count1)
          count1 = (int) num2;
        num1 = this.baseInputStream.Read(buffer, 0, count1);
      }
      return count - num2;
    }

    protected void StopDecrypting() => this.inputBuffer.CryptoTransform = (ICryptoTransform) null;

    public virtual int Available => this.inf.IsFinished ? 0 : 1;

    protected void Fill()
    {
      if (this.inputBuffer.Available <= 0)
      {
        this.inputBuffer.Fill();
        if (this.inputBuffer.Available <= 0)
          throw new SharpZipBaseException("Unexpected EOF");
      }
      this.inputBuffer.SetInflaterInput(this.inf);
    }

    public override bool CanRead => this.baseInputStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => (long) this.inputBuffer.RawLength;

    public override long Position
    {
      get => this.baseInputStream.Position;
      set => throw new NotSupportedException("InflaterInputStream Position not supported");
    }

    public override void Flush() => this.baseInputStream.Flush();

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException("Seek not supported");

    public override void SetLength(long value) => throw new NotSupportedException("InflaterInputStream SetLength not supported");

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException("InflaterInputStream Write not supported");

    public override void WriteByte(byte value) => throw new NotSupportedException("InflaterInputStream WriteByte not supported");

    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      throw new NotSupportedException("InflaterInputStream BeginWrite not supported");
    }

    public override void Close()
    {
      if (this.isClosed)
        return;
      this.isClosed = true;
      if (this.isStreamOwner)
        this.baseInputStream.Close();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this.inf.IsNeedingDictionary)
        throw new SharpZipBaseException("Need a dictionary");
      int count1 = count;
      while (true)
      {
        int num = this.inf.Inflate(buffer, offset, count1);
        offset += num;
        count1 -= num;
        if (count1 != 0 && !this.inf.IsFinished)
        {
          if (this.inf.IsNeedingInput)
            this.Fill();
          else if (num == 0)
            break;
        }
        else
          goto label_9;
      }
      throw new ZipException("Dont know what to do");
label_9:
      return count - count1;
    }
  }
}
