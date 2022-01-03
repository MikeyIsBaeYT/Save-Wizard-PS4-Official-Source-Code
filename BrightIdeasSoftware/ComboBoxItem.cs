// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ComboBoxItem
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class ComboBoxItem
  {
    private readonly string description;
    private readonly object key;

    public ComboBoxItem(object key, string description)
    {
      this.key = key;
      this.description = description;
    }

    public object Key => this.key;

    public override string ToString() => this.description;
  }
}
