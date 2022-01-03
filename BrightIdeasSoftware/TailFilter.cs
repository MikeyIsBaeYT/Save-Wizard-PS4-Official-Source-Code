// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TailFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace BrightIdeasSoftware
{
  public class TailFilter : AbstractListFilter
  {
    private int count;

    public TailFilter()
    {
    }

    public TailFilter(int numberOfObjects) => this.Count = numberOfObjects;

    public int Count
    {
      get => this.count;
      set => this.count = value;
    }

    public override IEnumerable Filter(IEnumerable modelObjects)
    {
      if (this.Count <= 0)
        return modelObjects;
      ArrayList array = ObjectListView.EnumerableToArray(modelObjects, false);
      if (this.Count > array.Count)
        return (IEnumerable) array;
      object[] objArray = new object[this.Count];
      array.CopyTo(array.Count - this.Count, (Array) objArray, 0, this.Count);
      return (IEnumerable) new ArrayList((ICollection) objArray);
    }
  }
}
