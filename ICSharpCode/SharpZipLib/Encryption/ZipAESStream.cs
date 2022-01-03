// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.ZipAESStream
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
  internal class ZipAESStream : CryptoStream
  {
    private const int AUTH_CODE_LENGTH = 10;
    private Stream _stream;
    private ZipAESTransform _transform;
    private byte[] _slideBuffer;
    private int _slideBufStartPos;
    private int _slideBufFreePos;
    private const int CRYPTO_BLOCK_SIZE = 16;
    private int _blockAndAuth;

    public ZipAESStream(Stream stream, ZipAESTransform transform, CryptoStreamMode mode)
      : base(stream, (ICryptoTransform) transform, mode)
    {
      this._stream = stream;
      this._transform = transform;
      this._slideBuffer = new byte[1024];
      this._blockAndAuth = 26;
      if ((uint) mode > 0U)
        throw new Exception("ZipAESStream only for read");
    }

    public override int Read(byte[] outBuffer, int offset, int count)
    {
      int num1 = 0;
      while (num1 < count)
      {
        int count1 = this._blockAndAuth - (this._slideBufFreePos - this._slideBufStartPos);
        if (this._slideBuffer.Length - this._slideBufFreePos < count1)
        {
          int index = 0;
          int slideBufStartPos = this._slideBufStartPos;
          while (slideBufStartPos < this._slideBufFreePos)
          {
            this._slideBuffer[index] = this._slideBuffer[slideBufStartPos];
            ++slideBufStartPos;
            ++index;
          }
          this._slideBufFreePos -= this._slideBufStartPos;
          this._slideBufStartPos = 0;
        }
        this._slideBufFreePos += this._stream.Read(this._slideBuffer, this._slideBufFreePos, count1);
        int num2 = this._slideBufFreePos - this._slideBufStartPos;
        if (num2 >= this._blockAndAuth)
        {
          this._transform.TransformBlock(this._slideBuffer, this._slideBufStartPos, 16, outBuffer, offset);
          num1 += 16;
          offset += 16;
          this._slideBufStartPos += 16;
        }
        else
        {
          if (num2 > 10)
          {
            int inputCount = num2 - 10;
            this._transform.TransformBlock(this._slideBuffer, this._slideBufStartPos, inputCount, outBuffer, offset);
            num1 += inputCount;
            this._slideBufStartPos += inputCount;
          }
          else if (num2 < 10)
            throw new Exception("Internal error missed auth code");
          byte[] authCode = this._transform.GetAuthCode();
          for (int index = 0; index < 10; ++index)
          {
            if ((int) authCode[index] != (int) this._slideBuffer[this._slideBufStartPos + index])
              throw new Exception("AES Authentication Code does not match. This is a super-CRC check on the data in the file after compression and encryption. \r\nThe file may be damaged.");
          }
          break;
        }
      }
      return num1;
    }

    public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
  }
}
