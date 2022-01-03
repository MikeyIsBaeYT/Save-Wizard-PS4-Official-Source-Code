// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.games
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  [XmlRoot("games", Namespace = "")]
  public class games
  {
    [XmlElement("regions")]
    public regions regions;
    [XmlElement("game")]
    public List<game> _games;
    [XmlElement("saves")]
    public saves _saves;

    [XmlElement("rblist")]
    public rblsit rblist { get; set; }
  }
}
