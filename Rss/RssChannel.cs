// Decompiled with JetBrains decompiler
// Type: Rss.RssChannel
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssChannel : RssElement
  {
    private string title = "";
    private Uri link = RssDefault.Uri;
    private string description = "";
    private string language = "";
    private string copyright = "";
    private string managingEditor = "";
    private string webMaster = "";
    private DateTime pubDate = RssDefault.DateTime;
    private DateTime lastBuildDate = RssDefault.DateTime;
    private RssCategoryCollection categories = new RssCategoryCollection();
    private string generator = "";
    private string docs = "";
    private RssCloud cloud = (RssCloud) null;
    private int timeToLive = -1;
    private RssImage image = (RssImage) null;
    private RssTextInput textInput = (RssTextInput) null;
    private bool[] skipHours = new bool[24];
    private bool[] skipDays = new bool[7];
    private string rating = "";
    private RssItemCollection items = new RssItemCollection();

    public override string ToString()
    {
      if (this.title != null)
        return this.title;
      return this.description != null ? this.description : nameof (RssChannel);
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

    public string Language
    {
      get => this.language;
      set => this.language = RssDefault.Check(value);
    }

    public RssImage Image
    {
      get => this.image;
      set => this.image = value;
    }

    public string Copyright
    {
      get => this.copyright;
      set => this.copyright = RssDefault.Check(value);
    }

    public string ManagingEditor
    {
      get => this.managingEditor;
      set => this.managingEditor = RssDefault.Check(value);
    }

    public string WebMaster
    {
      get => this.webMaster;
      set => this.webMaster = RssDefault.Check(value);
    }

    public string Rating
    {
      get => this.rating;
      set => this.rating = RssDefault.Check(value);
    }

    public DateTime PubDate
    {
      get => this.pubDate;
      set => this.pubDate = value;
    }

    public DateTime LastBuildDate
    {
      get => this.lastBuildDate;
      set => this.lastBuildDate = value;
    }

    public RssCategoryCollection Categories => this.categories;

    public string Generator
    {
      get => this.generator;
      set => this.generator = RssDefault.Check(value);
    }

    public string Docs
    {
      get => this.docs;
      set => this.docs = RssDefault.Check(value);
    }

    public RssTextInput TextInput
    {
      get => this.textInput;
      set => this.textInput = value;
    }

    public bool[] SkipDays
    {
      get => this.skipDays;
      set => this.skipDays = value;
    }

    public bool[] SkipHours
    {
      get => this.skipHours;
      set => this.skipHours = value;
    }

    public RssCloud Cloud
    {
      get => this.cloud;
      set => this.cloud = value;
    }

    public int TimeToLive
    {
      get => this.timeToLive;
      set => this.timeToLive = RssDefault.Check(value);
    }

    public RssItemCollection Items => this.items;
  }
}
