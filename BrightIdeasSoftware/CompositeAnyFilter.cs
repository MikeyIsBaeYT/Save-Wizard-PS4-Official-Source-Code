// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CompositeAnyFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class CompositeAnyFilter : CompositeFilter
  {
    public CompositeAnyFilter(List<IModelFilter> filters)
      : base((IEnumerable<IModelFilter>) filters)
    {
    }

    public override bool FilterObject(object modelObject)
    {
      foreach (IModelFilter filter in (IEnumerable<IModelFilter>) this.Filters)
      {
        if (filter.Filter(modelObject))
          return true;
      }
      return false;
    }
  }
}
