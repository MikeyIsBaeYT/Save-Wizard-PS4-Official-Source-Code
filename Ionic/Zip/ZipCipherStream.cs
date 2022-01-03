// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipCipherStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace Ionic.Zip
{
  internal class ZipCipherStream : Stream
  {
    private ZipCrypto _cipher;
    private Stream _s;
    private CryptoMode _mode;

    public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
    {
      this._cipher = cipher;
      this._s = s;
      this._mode = mode;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this._mode == CryptoMode.Encrypt)
        throw new NotSupportedException("This stream does not encrypt via Read()");
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      byte[] numArray1 = new byte[count];
      int length = this._s.Read(numArray1, 0, count);
      byte[] numArray2 = this._cipher.DecryptMessage(numArray1, length);
      for (int index = 0; index < length; ++index)
        buffer[offset + index] = numArray2[index];
      return length;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      if (this._mode == CryptoMode.Decrypt)
        throw new NotSupportedException("This stream does not Decrypt via Write()");
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (count == 0)
        return;
      byte[] plainText;
      if ((uint) offset > 0U)
      {
        plainText = new byte[count];
        for (int index = 0; index < count; ++index)
          plainText[index] = buffer[offset + index];
      }
      else
        plainText = buffer;
      byte[] buffer1 = this._cipher.EncryptMessage(plainText, count);
      this._s.Write(buffer1, 0, buffer1.Length);
    }

    public override bool CanRead => this._mode == CryptoMode.Decrypt;

    public override bool CanSeek => false;

    public override bool CanWrite => this._mode == CryptoMode.Encrypt;

    public override void Flush()
    {
    }

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
  }
}
