// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractVirtualListDataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractVirtualListDataSource : IVirtualListDataSource, IFilterableDataSource
  {
    protected VirtualObjectListView listView;

    public AbstractVirtualListDataSource(VirtualObjectListView listView) => this.listView = listView;

    public virtual object GetNthObject(int n) => (object) null;

    public virtual int GetObjectCount() => -1;

    public virtual int GetObjectIndex(object model) => -1;

    public virtual void PrepareCache(int from, int to)
    {
    }

    public virtual int SearchText(string value, int first, int last, OLVColumn column) => -1;

    public virtual void Sort(OLVColumn column, SortOrder order)
    {
    }

    public virtual void AddObjects(ICollection modelObjects)
    {
    }

    public virtual void RemoveObjects(ICollection modelObjects)
    {
    }

    public virtual void SetObjects(IEnumerable collection)
    {
    }

    public virtual void UpdateObject(int index, object modelObject)
    {
    }

    public static int DefaultSearchText(
      string value,
      int first,
      int last,
      OLVColumn column,
      IVirtualListDataSource source)
    {
      if (first <= last)
      {
        for (int n = first; n <= last; ++n)
        {
          if (column.GetStringValue(source.GetNthObject(n)).StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      else
      {
        for (int n = first; n >= last; --n)
        {
          if (column.GetStringValue(source.GetNthObject(n)).StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      return -1;
    }

    public virtual void ApplyFilters(IModelFilter modelFilter, IListFilter listFilter)
    {
    }
  }
}
