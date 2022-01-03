// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.DeflateStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace Ionic.Zlib
{
  public class DeflateStream : Stream
  {
    internal ZlibBaseStream _baseStream;
    internal Stream _innerStream;
    private bool _disposed;

    public DeflateStream(Stream stream, CompressionMode mode)
      : this(stream, mode, CompressionLevel.Default, false)
    {
    }

    public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level)
      : this(stream, mode, level, false)
    {
    }

    public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
      : this(stream, mode, CompressionLevel.Default, leaveOpen)
    {
    }

    public DeflateStream(
      Stream stream,
      CompressionMode mode,
      CompressionLevel level,
      bool leaveOpen)
    {
      this._innerStream = stream;
      this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
    }

    public virtual FlushType FlushMode
    {
      get => this._baseStream._flushMode;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (DeflateStream));
        this._baseStream._flushMode = value;
      }
    }

    public int BufferSize
    {
      get => this._baseStream._bufferSize;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (DeflateStream));
        if (this._baseStream._workingBuffer != null)
          throw new ZlibException("The working buffer is already set.");
        this._baseStream._bufferSize = value >= 1024 ? value : throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", (object) value, (object) 1024));
      }
    }

    public CompressionStrategy Strategy
    {
      get => this._baseStream.Strategy;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (DeflateStream));
        this._baseStream.Strategy = value;
      }
    }

    public virtual long TotalIn => this._baseStream._z.TotalBytesIn;

    public virtual long TotalOut => this._baseStream._z.TotalBytesOut;

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this._disposed)
          return;
        if (disposing && this._baseStream != null)
          this._baseStream.Close();
        this._disposed = true;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public override bool CanRead
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (DeflateStream));
        return this._baseStream._stream.CanRead;
      }
    }

    public override bool CanSeek => false;

    public override bool CanWrite
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (DeflateStream));
        return this._baseStream._stream.CanWrite;
      }
    }

    public override void Flush()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (DeflateStream));
      this._baseStream.Flush();
    }

    public override long Length => throw new NotImplementedException();

    public override long Position
    {
      get
      {
        if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
          return this._baseStream._z.TotalBytesOut;
        return this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader ? this._baseStream._z.TotalBytesIn : 0L;
      }
      set => throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (DeflateStream));
      return this._baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (DeflateStream));
      this._baseStream.Write(buffer, offset, count);
    }

    public static byte[] CompressString(string s)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Stream compressor = (Stream) new DeflateStream((Stream) memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
        ZlibBaseStream.CompressString(s, compressor);
        return memoryStream.ToArray();
      }
    }

    public static byte[] CompressBuffer(byte[] b)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Stream compressor = (Stream) new DeflateStream((Stream) memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
        ZlibBaseStream.CompressBuffer(b, compressor);
        return memoryStream.ToArray();
      }
    }

    public static string UncompressString(byte[] compressed)
    {
      using (MemoryStream memoryStream = new MemoryStream(compressed))
      {
        Stream decompressor = (Stream) new DeflateStream((Stream) memoryStream, CompressionMode.Decompress);
        return ZlibBaseStream.UncompressString(compressed, decompressor);
      }
    }

    public static byte[] UncompressBuffer(byte[] compressed)
    {
      using (MemoryStream memoryStream = new MemoryStream(compressed))
      {
        Stream decompressor = (Stream) new DeflateStream((Stream) memoryStream, CompressionMode.Decompress);
        return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
      }
    }
  }
}
