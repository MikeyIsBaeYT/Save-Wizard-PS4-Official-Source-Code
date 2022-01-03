// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.internals
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  public class internals
  {
    public internals() => this.files = new List<file>();

    [XmlElement("file")]
    public List<file> files { get; set; }

    public static internals Copy(internals i)
    {
      internals internals = new internals();
      foreach (file file in i.files)
        internals.files.Add(file.Copy(file));
      return internals;
    }
  }
}
