// Decompiled with JetBrains decompiler
// Type: Rss.RssCategoryCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  [Serializable]
  public class RssCategoryCollection : CollectionBase
  {
    public RssCategory this[int index]
    {
      get => (RssCategory) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(RssCategory rssCategory) => this.List.Add((object) rssCategory);

    public bool Contains(RssCategory rssCategory) => this.List.Contains((object) rssCategory);

    public void CopyTo(RssCategory[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssCategory rssCategory) => this.List.IndexOf((object) rssCategory);

    public void Insert(int index, RssCategory rssCategory) => this.List.Insert(index, (object) rssCategory);

    public void Remove(RssCategory rssCategory) => this.List.Remove((object) rssCategory);
  }
}
