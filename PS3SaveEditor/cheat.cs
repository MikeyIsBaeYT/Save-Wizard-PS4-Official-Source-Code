// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.cheat
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  [XmlRoot("cheat")]
  public class cheat
  {
    public string id { get; set; }

    public string name { get; set; }

    public string note { get; set; }

    public bool Selected { get; set; }

    public string code { get; set; }

    public cheat()
    {
    }

    public cheat(string id, string description, string comment)
    {
      this.id = id;
      this.name = description;
      this.note = comment;
    }

    public string ToEditableString()
    {
      string str = "";
      string[] strArray = this.code.Split(' ');
      for (int index = 0; index < strArray.Length; index += 2)
        str = str + strArray[index] + " " + strArray[index + 1] + "\n";
      return str;
    }

    public string ToString(bool _protected = false)
    {
      string str = "";
      if (this.Selected)
      {
        if (_protected)
        {
          if (!string.IsNullOrEmpty(this.id))
            str += string.Format("<id>{0}</id>", (object) this.id);
        }
        else
          return this.code != null ? string.Format("<code>{0}</code>", (object) this.code) : string.Format("<id>{0}</id>", (object) this.id);
      }
      return str;
    }

    internal static cheat Copy(cheat cheat) => new cheat()
    {
      id = cheat.id,
      name = cheat.name,
      note = cheat.note,
      code = cheat.code
    };

    internal static byte GetBitCodeBytes(int bitCode)
    {
      switch (bitCode)
      {
        case 0:
          return 1;
        case 1:
          return 2;
        case 2:
          return 4;
        default:
          return 4;
      }
    }

    internal static long GetMemLocation(string value, out int bitWriteCode)
    {
      long result;
      long.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result);
      long num = result & 268435455L;
      bitWriteCode = (int) (result >> 28);
      return num;
    }
  }
}
