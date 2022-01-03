// Decompiled with JetBrains decompiler
// Type: Rss.RssChannelCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  [Serializable]
  public class RssChannelCollection : CollectionBase
  {
    public RssChannel this[int index]
    {
      get => (RssChannel) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(RssChannel channel) => this.List.Add((object) channel);

    public bool Contains(RssChannel rssChannel) => this.List.Contains((object) rssChannel);

    public void CopyTo(RssChannel[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssChannel rssChannel) => this.List.IndexOf((object) rssChannel);

    public void Insert(int index, RssChannel channel) => this.List.Insert(index, (object) channel);

    public void Remove(RssChannel channel) => this.List.Remove((object) channel);
  }
}
