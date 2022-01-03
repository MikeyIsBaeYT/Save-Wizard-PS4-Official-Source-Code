// Decompiled with JetBrains decompiler
// Type: Rss.RssItem
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssItem : RssElement
  {
    private string title = "";
    private Uri link = RssDefault.Uri;
    private string description = "";
    private string author = "";
    private RssCategoryCollection categories = new RssCategoryCollection();
    private string comments = "";
    private RssEnclosure enclosure = (RssEnclosure) null;
    private RssGuid guid = (RssGuid) null;
    private DateTime pubDate = RssDefault.DateTime;
    private RssSource source = (RssSource) null;

    public override string ToString()
    {
      if (this.title != null)
        return this.title;
      return this.description != null ? this.description : nameof (RssItem);
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

    public string Author
    {
      get => this.author;
      set => this.author = RssDefault.Check(value);
    }

    public RssCategoryCollection Categories => this.categories;

    public string Comments
    {
      get => this.comments;
      set => this.comments = RssDefault.Check(value);
    }

    public RssSource Source
    {
      get => this.source;
      set => this.source = value;
    }

    public RssEnclosure Enclosure
    {
      get => this.enclosure;
      set => this.enclosure = value;
    }

    public RssGuid Guid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public DateTime PubDate
    {
      get => this.pubDate;
      set => this.pubDate = value;
    }
  }
}
