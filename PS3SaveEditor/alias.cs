// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.alias
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace PS3SaveEditor
{
  public class alias
  {
    public string id;
    public string name;
    public int acts;
    public string diskcode;
    public int region;

    public static alias Copy(alias alias) => new alias()
    {
      id = alias.id,
      region = alias.region,
      name = alias.name,
      diskcode = alias.diskcode
    };
  }
}
