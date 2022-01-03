// Decompiled with JetBrains decompiler
// Type: Rss.RssFeedCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  [Serializable]
  public class RssFeedCollection : CollectionBase
  {
    public RssFeed this[int index]
    {
      get => (RssFeed) this.List[index];
      set => this.List[index] = (object) value;
    }

    public RssFeed this[string url]
    {
      get
      {
        for (int index = 0; index < this.List.Count; ++index)
        {
          if (((RssFeed) this.List[index]).Url == url)
            return this[index];
        }
        return (RssFeed) null;
      }
    }

    public int Add(RssFeed feed) => this.List.Add((object) feed);

    public bool Contains(RssFeed rssFeed) => this.List.Contains((object) rssFeed);

    public void CopyTo(RssFeed[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssFeed rssFeed) => this.List.IndexOf((object) rssFeed);

    public void Insert(int index, RssFeed feed) => this.List.Insert(index, (object) feed);

    public void Remove(RssFeed feed) => this.List.Remove((object) feed);
  }
}
