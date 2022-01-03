// Decompiled with JetBrains decompiler
// Type: Rss.RssTextInput
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssTextInput : RssElement
  {
    private string title = "";
    private string description = "";
    private string name = "";
    private Uri link = RssDefault.Uri;

    public string Title
    {
      get => this.title;
      set => this.title = RssDefault.Check(value);
    }

    public string Description
    {
      get => this.description;
      set => this.description = RssDefault.Check(value);
    }

    public string Name
    {
      get => this.name;
      set => this.name = RssDefault.Check(value);
    }

    public Uri Link
    {
      get => this.link;
      set => this.link = RssDefault.Check(value);
    }
  }
}
