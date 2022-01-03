// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.cheats
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  [XmlRoot("cheats")]
  public class cheats
  {
    [XmlElement("cheat")]
    public List<cheat> _cheats { get; set; }

    [XmlElement("group")]
    public List<group> groups { get; set; }

    public cheats()
    {
      this._cheats = new List<cheat>();
      this.groups = new List<group>();
    }

    public int TotalCheats
    {
      get
      {
        int count = this._cheats.Count;
        foreach (group group in this.groups)
          count += group.TotalCheats;
        return count;
      }
    }
  }
}
