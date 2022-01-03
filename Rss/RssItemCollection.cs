// Decompiled with JetBrains decompiler
// Type: Rss.RssItemCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public class RssItemCollection : CollectionBase
  {
    private DateTime latestPubDate = RssDefault.DateTime;
    private DateTime oldestPubDate = RssDefault.DateTime;
    private bool pubDateChanged = true;

    public RssItem this[int index]
    {
      get => (RssItem) this.List[index];
      set
      {
        this.pubDateChanged = true;
        this.List[index] = (object) value;
      }
    }

    public int Add(RssItem item)
    {
      this.pubDateChanged = true;
      return this.List.Add((object) item);
    }

    public bool Contains(RssItem rssItem) => this.List.Contains((object) rssItem);

    public void CopyTo(RssItem[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssItem rssItem) => this.List.IndexOf((object) rssItem);

    public void Insert(int index, RssItem item)
    {
      this.pubDateChanged = true;
      this.List.Insert(index, (object) item);
    }

    public void Remove(RssItem item)
    {
      this.pubDateChanged = true;
      this.List.Remove((object) item);
    }

    public DateTime LatestPubDate()
    {
      this.CalculatePubDates();
      return this.latestPubDate;
    }

    public DateTime OldestPubDate()
    {
      this.CalculatePubDates();
      return this.oldestPubDate;
    }

    private void CalculatePubDates()
    {
      if (!this.pubDateChanged)
        return;
      this.pubDateChanged = false;
      this.latestPubDate = DateTime.MinValue;
      this.oldestPubDate = DateTime.MaxValue;
      foreach (RssItem rssItem in (IEnumerable) this.List)
      {
        if (rssItem.PubDate != RssDefault.DateTime & rssItem.PubDate > this.latestPubDate)
          this.latestPubDate = rssItem.PubDate;
      }
      if (this.latestPubDate == DateTime.MinValue)
        this.latestPubDate = RssDefault.DateTime;
      foreach (RssItem rssItem in (IEnumerable) this.List)
      {
        if (rssItem.PubDate != RssDefault.DateTime & rssItem.PubDate < this.oldestPubDate)
          this.oldestPubDate = rssItem.PubDate;
      }
      if (this.oldestPubDate == DateTime.MaxValue)
        this.oldestPubDate = RssDefault.DateTime;
    }
  }
}
