// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.EbcdicByteCharProvider
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Text;

namespace Be.Windows.Forms
{
  public class EbcdicByteCharProvider : IByteCharConverter
  {
    private Encoding _ebcdicEncoding = Encoding.GetEncoding(500);

    public char ToChar(byte b)
    {
      string str = this._ebcdicEncoding.GetString(new byte[1]
      {
        b
      });
      return str.Length > 0 ? str[0] : '.';
    }

    public byte ToByte(char c)
    {
      byte[] bytes = this._ebcdicEncoding.GetBytes(new char[1]
      {
        c
      });
      return bytes.Length != 0 ? bytes[0] : (byte) 0;
    }

    public override string ToString() => "EBCDIC (Code Page 500)";
  }
}
