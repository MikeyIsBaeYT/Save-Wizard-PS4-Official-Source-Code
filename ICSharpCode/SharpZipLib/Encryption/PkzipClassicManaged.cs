// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
  public sealed class PkzipClassicManaged : PkzipClassic
  {
    private byte[] key_;

    public override int BlockSize
    {
      get => 8;
      set
      {
        if (value != 8)
          throw new CryptographicException("Block size is invalid");
      }
    }

    public override KeySizes[] LegalKeySizes => new KeySizes[1]
    {
      new KeySizes(96, 96, 0)
    };

    public override void GenerateIV()
    {
    }

    public override KeySizes[] LegalBlockSizes => new KeySizes[1]
    {
      new KeySizes(8, 8, 0)
    };

    public override byte[] Key
    {
      get
      {
        if (this.key_ == null)
          this.GenerateKey();
        return (byte[]) this.key_.Clone();
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.key_ = value.Length == 12 ? (byte[]) value.Clone() : throw new CryptographicException("Key size is illegal");
      }
    }

    public override void GenerateKey()
    {
      this.key_ = new byte[12];
      new Random().NextBytes(this.key_);
    }

    public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
    {
      this.key_ = rgbKey;
      return (ICryptoTransform) new PkzipClassicEncryptCryptoTransform(this.Key);
    }

    public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
    {
      this.key_ = rgbKey;
      return (ICryptoTransform) new PkzipClassicDecryptCryptoTransform(this.Key);
    }
  }
}
