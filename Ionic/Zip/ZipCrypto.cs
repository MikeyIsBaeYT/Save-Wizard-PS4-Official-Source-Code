// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipCrypto
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Crc;
using System;
using System.IO;

namespace Ionic.Zip
{
  internal class ZipCrypto
  {
    private uint[] _Keys = new uint[3]
    {
      305419896U,
      591751049U,
      878082192U
    };
    private CRC32 crc32 = new CRC32();

    private ZipCrypto()
    {
    }

    public static ZipCrypto ForWrite(string password)
    {
      ZipCrypto zipCrypto = new ZipCrypto();
      if (password == null)
        throw new BadPasswordException("This entry requires a password.");
      zipCrypto.InitCipher(password);
      return zipCrypto;
    }

    public static ZipCrypto ForRead(string password, ZipEntry e)
    {
      Stream archiveStream = e._archiveStream;
      e._WeakEncryptionHeader = new byte[12];
      byte[] encryptionHeader = e._WeakEncryptionHeader;
      ZipCrypto zipCrypto = new ZipCrypto();
      if (password == null)
        throw new BadPasswordException("This entry requires a password.");
      zipCrypto.InitCipher(password);
      ZipEntry.ReadWeakEncryptionHeader(archiveStream, encryptionHeader);
      byte[] numArray = zipCrypto.DecryptMessage(encryptionHeader, encryptionHeader.Length);
      if ((int) numArray[11] != (int) (byte) (e._Crc32 >> 24 & (int) byte.MaxValue))
      {
        if (((int) e._BitField & 8) != 8)
          throw new BadPasswordException("The password did not match.");
        if ((int) numArray[11] != (int) (byte) (e._TimeBlob >> 8 & (int) byte.MaxValue))
          throw new BadPasswordException("The password did not match.");
      }
      return zipCrypto;
    }

    private byte MagicByte
    {
      get
      {
        ushort num = (ushort) ((uint) (ushort) (this._Keys[2] & (uint) ushort.MaxValue) | 2U);
        return (byte) ((int) num * ((int) num ^ 1) >> 8);
      }
    }

    public byte[] DecryptMessage(byte[] cipherText, int length)
    {
      if (cipherText == null)
        throw new ArgumentNullException(nameof (cipherText));
      if (length > cipherText.Length)
        throw new ArgumentOutOfRangeException(nameof (length), "Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.");
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
      {
        byte byteValue = (byte) ((uint) cipherText[index] ^ (uint) this.MagicByte);
        this.UpdateKeys(byteValue);
        numArray[index] = byteValue;
      }
      return numArray;
    }

    public byte[] EncryptMessage(byte[] plainText, int length)
    {
      if (plainText == null)
        throw new ArgumentNullException("plaintext");
      if (length > plainText.Length)
        throw new ArgumentOutOfRangeException(nameof (length), "Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.");
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
      {
        byte byteValue = plainText[index];
        numArray[index] = (byte) ((uint) plainText[index] ^ (uint) this.MagicByte);
        this.UpdateKeys(byteValue);
      }
      return numArray;
    }

    public void InitCipher(string passphrase)
    {
      byte[] byteArray = SharedUtilities.StringToByteArray(passphrase);
      for (int index = 0; index < passphrase.Length; ++index)
        this.UpdateKeys(byteArray[index]);
    }

    private void UpdateKeys(byte byteValue)
    {
      this._Keys[0] = (uint) this.crc32.ComputeCrc32((int) this._Keys[0], byteValue);
      this._Keys[1] = this._Keys[1] + (uint) (byte) this._Keys[0];
      this._Keys[1] = (uint) ((int) this._Keys[1] * 134775813 + 1);
      this._Keys[2] = (uint) this.crc32.ComputeCrc32((int) this._Keys[2], (byte) (this._Keys[1] >> 24));
    }
  }
}
