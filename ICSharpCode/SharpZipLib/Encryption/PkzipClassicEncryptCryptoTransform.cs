// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.PkzipClassicEncryptCryptoTransform
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
  internal class PkzipClassicEncryptCryptoTransform : 
    PkzipClassicCryptoBase,
    ICryptoTransform,
    IDisposable
  {
    internal PkzipClassicEncryptCryptoTransform(byte[] keyBlock) => this.SetKeys(keyBlock);

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      byte[] outputBuffer = new byte[inputCount];
      this.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
      return outputBuffer;
    }

    public int TransformBlock(
      byte[] inputBuffer,
      int inputOffset,
      int inputCount,
      byte[] outputBuffer,
      int outputOffset)
    {
      for (int index = inputOffset; index < inputOffset + inputCount; ++index)
      {
        byte ch = inputBuffer[index];
        outputBuffer[outputOffset++] = (byte) ((uint) inputBuffer[index] ^ (uint) this.TransformByte());
        this.UpdateKeys(ch);
      }
      return inputCount;
    }

    public bool CanReuseTransform => true;

    public int InputBlockSize => 1;

    public int OutputBlockSize => 1;

    public bool CanTransformMultipleBlocks => true;

    public void Dispose() => this.Reset();
  }
}
