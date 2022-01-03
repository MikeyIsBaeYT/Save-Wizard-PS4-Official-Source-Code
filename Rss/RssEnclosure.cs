// Decompiled with JetBrains decompiler
// Type: Rss.RssEnclosure
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssEnclosure : RssElement
  {
    private Uri uri = RssDefault.Uri;
    private int length = -1;
    private string type = "";

    public Uri Url
    {
      get => this.uri;
      set => this.uri = RssDefault.Check(value);
    }

    public int Length
    {
      get => this.length;
      set => this.length = RssDefault.Check(value);
    }

    public string Type
    {
      get => this.type;
      set => this.type = RssDefault.Check(value);
    }
  }
}
