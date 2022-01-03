// Decompiled with JetBrains decompiler
// Type: Rss.RssModuleItemCollectionCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public class RssModuleItemCollectionCollection : CollectionBase
  {
    public RssModuleItemCollection this[int index]
    {
      get => (RssModuleItemCollection) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(RssModuleItemCollection rssModuleItemCollection) => this.List.Add((object) rssModuleItemCollection);

    public bool Contains(RssModuleItemCollection rssModuleItemCollection) => this.List.Contains((object) rssModuleItemCollection);

    public void CopyTo(RssModuleItemCollection[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssModuleItemCollection rssModuleItemCollection) => this.List.IndexOf((object) rssModuleItemCollection);

    public void Insert(int index, RssModuleItemCollection rssModuleItemCollection) => this.List.Insert(index, (object) rssModuleItemCollection);

    public void Remove(RssModuleItemCollection rssModuleItemCollection) => this.List.Remove((object) rssModuleItemCollection);
  }
}
