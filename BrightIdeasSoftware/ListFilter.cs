// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ListFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ListFilter : AbstractListFilter
  {
    private ListFilter.ListFilterDelegate function;

    public ListFilter(ListFilter.ListFilterDelegate function) => this.Function = function;

    public ListFilter.ListFilterDelegate Function
    {
      get => this.function;
      set => this.function = value;
    }

    public override IEnumerable Filter(IEnumerable modelObjects) => this.Function == null ? modelObjects : this.Function(modelObjects);

    public delegate IEnumerable ListFilterDelegate(IEnumerable rowObjects);
  }
}
