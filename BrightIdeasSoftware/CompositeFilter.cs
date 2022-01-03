// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CompositeFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public abstract class CompositeFilter : IModelFilter
  {
    private IList<IModelFilter> filters = (IList<IModelFilter>) new List<IModelFilter>();

    public CompositeFilter()
    {
    }

    public CompositeFilter(IEnumerable<IModelFilter> filters)
    {
      foreach (IModelFilter filter in filters)
      {
        if (filter != null)
          this.Filters.Add(filter);
      }
    }

    public IList<IModelFilter> Filters
    {
      get => this.filters;
      set => this.filters = value;
    }

    public virtual bool Filter(object modelObject) => this.Filters == null || this.Filters.Count == 0 || this.FilterObject(modelObject);

    public abstract bool FilterObject(object modelObject);
  }
}
