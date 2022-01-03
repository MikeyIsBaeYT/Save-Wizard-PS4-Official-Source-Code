// Decompiled with JetBrains decompiler
// Type: Rss.RssModuleCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public class RssModuleCollection : CollectionBase
  {
    public RssModule this[int index]
    {
      get => (RssModule) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(RssModule rssModule) => this.List.Add((object) rssModule);

    public bool Contains(RssModule rssModule) => this.List.Contains((object) rssModule);

    public void CopyTo(RssModule[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssModule rssModule) => this.List.IndexOf((object) rssModule);

    public void Insert(int index, RssModule rssModule) => this.List.Insert(index, (object) rssModule);

    public void Remove(RssModule rssModule) => this.List.Remove((object) rssModule);
  }
}
