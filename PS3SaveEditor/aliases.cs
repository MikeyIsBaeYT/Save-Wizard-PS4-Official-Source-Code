// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.aliases
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  public class aliases
  {
    [XmlElement("alias")]
    public List<alias> _aliases;

    public static aliases Copy(aliases a)
    {
      aliases aliases = new aliases();
      if (a != null && a._aliases != null)
      {
        aliases._aliases = new List<alias>();
        foreach (alias alias in a._aliases)
          aliases._aliases.Add(alias.Copy(alias));
      }
      return aliases;
    }
  }
}
