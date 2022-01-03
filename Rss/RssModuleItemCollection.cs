// Decompiled with JetBrains decompiler
// Type: Rss.RssModuleItemCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public class RssModuleItemCollection : CollectionBase
  {
    private ArrayList _alBindTo = new ArrayList();

    public RssModuleItem this[int index]
    {
      get => (RssModuleItem) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(RssModuleItem rssModuleItem) => this.List.Add((object) rssModuleItem);

    public bool Contains(RssModuleItem rssModuleItem) => this.List.Contains((object) rssModuleItem);

    public void CopyTo(RssModuleItem[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(RssModuleItem rssModuleItem) => this.List.IndexOf((object) rssModuleItem);

    public void Insert(int index, RssModuleItem rssModuleItem) => this.List.Insert(index, (object) rssModuleItem);

    public void Remove(RssModuleItem rssModuleItem) => this.List.Remove((object) rssModuleItem);

    public void BindTo(int itemHashCode) => this._alBindTo.Add((object) itemHashCode);

    public bool IsBoundTo(int itemHashCode) => this._alBindTo.BinarySearch(0, this._alBindTo.Count, (object) itemHashCode, (IComparer) null) >= 0;
  }
}
