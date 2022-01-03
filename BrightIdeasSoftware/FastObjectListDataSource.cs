// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FastObjectListDataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class FastObjectListDataSource : AbstractVirtualListDataSource
  {
    private ArrayList fullObjectList = new ArrayList();
    private ArrayList filteredObjectList = new ArrayList();
    private IModelFilter modelFilter;
    private IListFilter listFilter;
    private readonly Dictionary<object, int> objectsToIndexMap = new Dictionary<object, int>();

    public FastObjectListDataSource(FastObjectListView listView)
      : base((VirtualObjectListView) listView)
    {
    }

    public override object GetNthObject(int n) => n >= 0 && n < this.filteredObjectList.Count ? this.filteredObjectList[n] : (object) null;

    public override int GetObjectCount() => this.filteredObjectList.Count;

    public override int GetObjectIndex(object model)
    {
      int num;
      return model != null && this.objectsToIndexMap.TryGetValue(model, out num) ? num : -1;
    }

    public override int SearchText(string text, int first, int last, OLVColumn column)
    {
      if (first <= last)
      {
        for (int n = first; n <= last; ++n)
        {
          if (column.GetStringValue(this.listView.GetNthItemInDisplayOrder(n).RowObject).StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      else
      {
        for (int n = first; n >= last; --n)
        {
          if (column.GetStringValue(this.listView.GetNthItemInDisplayOrder(n).RowObject).StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      return -1;
    }

    public override void Sort(OLVColumn column, SortOrder sortOrder)
    {
      if ((uint) sortOrder > 0U)
      {
        ModelObjectComparer modelObjectComparer = new ModelObjectComparer(column, sortOrder, this.listView.SecondarySortColumn, this.listView.SecondarySortOrder);
        this.fullObjectList.Sort((IComparer) modelObjectComparer);
        this.filteredObjectList.Sort((IComparer) modelObjectComparer);
      }
      this.RebuildIndexMap();
    }

    public override void AddObjects(ICollection modelObjects)
    {
      foreach (object modelObject in (IEnumerable) modelObjects)
      {
        if (modelObject != null)
          this.fullObjectList.Add(modelObject);
      }
      this.FilterObjects();
      this.RebuildIndexMap();
    }

    public override void RemoveObjects(ICollection modelObjects)
    {
      List<int> intList = new List<int>();
      foreach (object modelObject in (IEnumerable) modelObjects)
      {
        int objectIndex = this.GetObjectIndex(modelObject);
        if (objectIndex >= 0)
          intList.Add(objectIndex);
        this.fullObjectList.Remove(modelObject);
      }
      intList.Sort();
      intList.Reverse();
      foreach (int itemIndex in intList)
        this.listView.SelectedIndices.Remove(itemIndex);
      this.FilterObjects();
      this.RebuildIndexMap();
    }

    public override void SetObjects(IEnumerable collection)
    {
      this.fullObjectList = ObjectListView.EnumerableToArray(collection, true);
      this.FilterObjects();
      this.RebuildIndexMap();
    }

    public override void UpdateObject(int index, object modelObject)
    {
      if (index < 0 || index >= this.filteredObjectList.Count)
        return;
      int index1 = this.fullObjectList.IndexOf(this.filteredObjectList[index]);
      if (index1 < 0)
        return;
      this.fullObjectList[index1] = modelObject;
      this.filteredObjectList[index] = modelObject;
      this.objectsToIndexMap[modelObject] = index;
    }

    public override void ApplyFilters(IModelFilter iModelFilter, IListFilter iListFilter)
    {
      this.modelFilter = iModelFilter;
      this.listFilter = iListFilter;
      this.SetObjects((IEnumerable) this.fullObjectList);
    }

    public ArrayList ObjectList => this.fullObjectList;

    public ArrayList FilteredObjectList => this.filteredObjectList;

    protected void RebuildIndexMap()
    {
      this.objectsToIndexMap.Clear();
      for (int index = 0; index < this.filteredObjectList.Count; ++index)
        this.objectsToIndexMap[this.filteredObjectList[index]] = index;
    }

    protected void FilterObjects()
    {
      if (!this.listView.UseFiltering || this.modelFilter == null && this.listFilter == null)
      {
        this.filteredObjectList = new ArrayList((ICollection) this.fullObjectList);
      }
      else
      {
        IEnumerable collection = this.listFilter == null ? (IEnumerable) this.fullObjectList : this.listFilter.Filter((IEnumerable) this.fullObjectList);
        if (this.modelFilter == null)
        {
          this.filteredObjectList = ObjectListView.EnumerableToArray(collection, false);
        }
        else
        {
          this.filteredObjectList = new ArrayList();
          foreach (object modelObject in collection)
          {
            if (this.modelFilter.Filter(modelObject))
              this.filteredObjectList.Add(modelObject);
          }
        }
      }
    }
  }
}
