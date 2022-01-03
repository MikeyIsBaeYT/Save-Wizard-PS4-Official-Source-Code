// Decompiled with JetBrains decompiler
// Type: Rss.RssGuid
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Rss
{
  [Serializable]
  public class RssGuid : RssElement
  {
    private DBBool permaLink = DBBool.Null;
    private string name = "";

    public DBBool PermaLink
    {
      get => this.permaLink;
      set => this.permaLink = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = RssDefault.Check(value);
    }
  }
}
