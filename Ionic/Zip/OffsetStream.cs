// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.OffsetStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace Ionic.Zip
{
  internal class OffsetStream : Stream, IDisposable
  {
    private long _originalPosition;
    private Stream _innerStream;

    public OffsetStream(Stream s)
    {
      this._originalPosition = s.Position;
      this._innerStream = s;
    }

    public override int Read(byte[] buffer, int offset, int count) => this._innerStream.Read(buffer, offset, count);

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

    public override bool CanRead => this._innerStream.CanRead;

    public override bool CanSeek => this._innerStream.CanSeek;

    public override bool CanWrite => false;

    public override void Flush() => this._innerStream.Flush();

    public override long Length => this._innerStream.Length;

    public override long Position
    {
      get => this._innerStream.Position - this._originalPosition;
      set => this._innerStream.Position = this._originalPosition + value;
    }

    public override long Seek(long offset, SeekOrigin origin) => this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;

    public override void SetLength(long value) => throw new NotImplementedException();

    void IDisposable.Dispose() => this.Close();

    public override void Close() => base.Close();
  }
}
