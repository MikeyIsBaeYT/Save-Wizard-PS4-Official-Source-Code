// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.PkzipClassic
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Checksums;
using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
  public abstract class PkzipClassic : SymmetricAlgorithm
  {
    public static byte[] GenerateKeys(byte[] seed)
    {
      if (seed == null)
        throw new ArgumentNullException(nameof (seed));
      if (seed.Length == 0)
        throw new ArgumentException("Length is zero", nameof (seed));
      uint[] numArray = new uint[3]
      {
        305419896U,
        591751049U,
        878082192U
      };
      for (int index = 0; index < seed.Length; ++index)
      {
        numArray[0] = Crc32.ComputeCrc32(numArray[0], seed[index]);
        numArray[1] = numArray[1] + (uint) (byte) numArray[0];
        numArray[1] = (uint) ((int) numArray[1] * 134775813 + 1);
        numArray[2] = Crc32.ComputeCrc32(numArray[2], (byte) (numArray[1] >> 24));
      }
      return new byte[12]
      {
        (byte) (numArray[0] & (uint) byte.MaxValue),
        (byte) (numArray[0] >> 8 & (uint) byte.MaxValue),
        (byte) (numArray[0] >> 16 & (uint) byte.MaxValue),
        (byte) (numArray[0] >> 24 & (uint) byte.MaxValue),
        (byte) (numArray[1] & (uint) byte.MaxValue),
        (byte) (numArray[1] >> 8 & (uint) byte.MaxValue),
        (byte) (numArray[1] >> 16 & (uint) byte.MaxValue),
        (byte) (numArray[1] >> 24 & (uint) byte.MaxValue),
        (byte) (numArray[2] & (uint) byte.MaxValue),
        (byte) (numArray[2] >> 8 & (uint) byte.MaxValue),
        (byte) (numArray[2] >> 16 & (uint) byte.MaxValue),
        (byte) (numArray[2] >> 24 & (uint) byte.MaxValue)
      };
    }
  }
}
