// Decompiled with JetBrains decompiler
// Type: Rss.RssSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssSource : RssElement
  {
    private string name = "";
    private Uri uri = RssDefault.Uri;

    public string Name
    {
      get => this.name;
      set => this.name = RssDefault.Check(value);
    }

    public Uri Url
    {
      get => this.uri;
      set => this.uri = RssDefault.Check(value);
    }
  }
}
