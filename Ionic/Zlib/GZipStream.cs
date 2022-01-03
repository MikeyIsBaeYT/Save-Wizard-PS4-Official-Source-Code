// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.GZipStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
  public class GZipStream : Stream
  {
    public DateTime? LastModified;
    private int _headerByteCount;
    internal ZlibBaseStream _baseStream;
    private bool _disposed;
    private bool _firstReadDone;
    private string _FileName;
    private string _Comment;
    private int _Crc32;
    internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");

    public string Comment
    {
      get => this._Comment;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (GZipStream));
        this._Comment = value;
      }
    }

    public string FileName
    {
      get => this._FileName;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (GZipStream));
        this._FileName = value;
        if (this._FileName == null)
          return;
        if (this._FileName.IndexOf("/") != -1)
          this._FileName = this._FileName.Replace("/", "\\");
        if (this._FileName.EndsWith("\\"))
          throw new Exception("Illegal filename");
        if (this._FileName.IndexOf("\\") == -1)
          return;
        this._FileName = Path.GetFileName(this._FileName);
      }
    }

    public int Crc32 => this._Crc32;

    public GZipStream(Stream stream, CompressionMode mode)
      : this(stream, mode, CompressionLevel.Default, false)
    {
    }

    public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level)
      : this(stream, mode, level, false)
    {
    }

    public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
      : this(stream, mode, CompressionLevel.Default, leaveOpen)
    {
    }

    public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen) => this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);

    public virtual FlushType FlushMode
    {
      get => this._baseStream._flushMode;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (GZipStream));
        this._baseStream._flushMode = value;
      }
    }

    public int BufferSize
    {
      get => this._baseStream._bufferSize;
      set
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (GZipStream));
        if (this._baseStream._workingBuffer != null)
          throw new ZlibException("The working buffer is already set.");
        this._baseStream._bufferSize = value >= 1024 ? value : throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", (object) value, (object) 1024));
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
        {
          this._baseStream.Close();
          this._Crc32 = this._baseStream.Crc32;
        }
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
          throw new ObjectDisposedException(nameof (GZipStream));
        return this._baseStream._stream.CanRead;
      }
    }

    public override bool CanSeek => false;

    public override bool CanWrite
    {
      get
      {
        if (this._disposed)
          throw new ObjectDisposedException(nameof (GZipStream));
        return this._baseStream._stream.CanWrite;
      }
    }

    public override void Flush()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (GZipStream));
      this._baseStream.Flush();
    }

    public override long Length => throw new NotImplementedException();

    public override long Position
    {
      get
      {
        if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
          return this._baseStream._z.TotalBytesOut + (long) this._headerByteCount;
        return this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader ? this._baseStream._z.TotalBytesIn + (long) this._baseStream._gzipHeaderByteCount : 0L;
      }
      set => throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (GZipStream));
      int num = this._baseStream.Read(buffer, offset, count);
      if (!this._firstReadDone)
      {
        this._firstReadDone = true;
        this.FileName = this._baseStream._GzipFileName;
        this.Comment = this._baseStream._GzipComment;
      }
      return num;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value) => throw new NotImplementedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (GZipStream));
      if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
      {
        if (!this._baseStream._wantCompress)
          throw new InvalidOperationException();
        this._headerByteCount = this.EmitHeader();
      }
      this._baseStream.Write(buffer, offset, count);
    }

    private int EmitHeader()
    {
      byte[] numArray1 = this.Comment == null ? (byte[]) null : GZipStream.iso8859dash1.GetBytes(this.Comment);
      byte[] numArray2 = this.FileName == null ? (byte[]) null : GZipStream.iso8859dash1.GetBytes(this.FileName);
      int num1 = this.Comment == null ? 0 : numArray1.Length + 1;
      int num2 = this.FileName == null ? 0 : numArray2.Length + 1;
      byte[] buffer = new byte[10 + num1 + num2];
      int num3 = 0;
      byte[] numArray3 = buffer;
      int index1 = num3;
      int num4 = index1 + 1;
      numArray3[index1] = (byte) 31;
      byte[] numArray4 = buffer;
      int index2 = num4;
      int num5 = index2 + 1;
      numArray4[index2] = (byte) 139;
      byte[] numArray5 = buffer;
      int index3 = num5;
      int num6 = index3 + 1;
      numArray5[index3] = (byte) 8;
      byte num7 = 0;
      if (this.Comment != null)
        num7 ^= (byte) 16;
      if (this.FileName != null)
        num7 ^= (byte) 8;
      byte[] numArray6 = buffer;
      int index4 = num6;
      int destinationIndex1 = index4 + 1;
      int num8 = (int) num7;
      numArray6[index4] = (byte) num8;
      if (!this.LastModified.HasValue)
        this.LastModified = new DateTime?(DateTime.Now);
      Array.Copy((Array) BitConverter.GetBytes((int) (this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, (Array) buffer, destinationIndex1, 4);
      int num9 = destinationIndex1 + 4;
      byte[] numArray7 = buffer;
      int index5 = num9;
      int num10 = index5 + 1;
      numArray7[index5] = (byte) 0;
      byte[] numArray8 = buffer;
      int index6 = num10;
      int destinationIndex2 = index6 + 1;
      numArray8[index6] = byte.MaxValue;
      if ((uint) num2 > 0U)
      {
        Array.Copy((Array) numArray2, 0, (Array) buffer, destinationIndex2, num2 - 1);
        int num11 = destinationIndex2 + (num2 - 1);
        byte[] numArray9 = buffer;
        int index7 = num11;
        destinationIndex2 = index7 + 1;
        numArray9[index7] = (byte) 0;
      }
      if ((uint) num1 > 0U)
      {
        Array.Copy((Array) numArray1, 0, (Array) buffer, destinationIndex2, num1 - 1);
        int num12 = destinationIndex2 + (num1 - 1);
        byte[] numArray10 = buffer;
        int index8 = num12;
        int num13 = index8 + 1;
        numArray10[index8] = (byte) 0;
      }
      this._baseStream._stream.Write(buffer, 0, buffer.Length);
      return buffer.Length;
    }

    public static byte[] CompressString(string s)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Stream compressor = (Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
        ZlibBaseStream.CompressString(s, compressor);
        return memoryStream.ToArray();
      }
    }

    public static byte[] CompressBuffer(byte[] b)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Stream compressor = (Stream) new GZipStream((Stream) memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
        ZlibBaseStream.CompressBuffer(b, compressor);
        return memoryStream.ToArray();
      }
    }

    public static string UncompressString(byte[] compressed)
    {
      using (MemoryStream memoryStream = new MemoryStream(compressed))
      {
        Stream decompressor = (Stream) new GZipStream((Stream) memoryStream, CompressionMode.Decompress);
        return ZlibBaseStream.UncompressString(compressed, decompressor);
      }
    }

    public static byte[] UncompressBuffer(byte[] compressed)
    {
      using (MemoryStream memoryStream = new MemoryStream(compressed))
      {
        Stream decompressor = (Stream) new GZipStream((Stream) memoryStream, CompressionMode.Decompress);
        return ZlibBaseStream.UncompressBuffer(compressed, decompressor);
      }
    }
  }
}
