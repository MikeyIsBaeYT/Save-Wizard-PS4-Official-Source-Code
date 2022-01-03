// Decompiled with JetBrains decompiler
// Type: Rss.RssImage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssImage : RssElement
  {
    private string title = "";
    private string description = "";
    private Uri uri = RssDefault.Uri;
    private Uri link = RssDefault.Uri;
    private int width = -1;
    private int height = -1;

    public Uri Url
    {
      get => this.uri;
      set => this.uri = RssDefault.Check(value);
    }

    public string Title
    {
      get => this.title;
      set => this.title = RssDefault.Check(value);
    }

    public Uri Link
    {
      get => this.link;
      set => this.link = RssDefault.Check(value);
    }

    public string Description
    {
      get => this.description;
      set => this.description = RssDefault.Check(value);
    }

    public int Width
    {
      get => this.width;
      set => this.width = RssDefault.Check(value);
    }

    public int Height
    {
      get => this.height;
      set => this.height = RssDefault.Check(value);
    }
  }
}
