// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FastListGroupingStrategy
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class FastListGroupingStrategy : AbstractVirtualGroups
  {
    private List<int> indexToGroupMap;

    public override IList<OLVGroup> GetGroups(GroupingParameters parmameters)
    {
      FastObjectListView folv = (FastObjectListView) parmameters.ListView;
      int capacity = 0;
      NullableDictionary<object, List<object>> nullableDictionary = new NullableDictionary<object, List<object>>();
      foreach (object filteredObject in folv.FilteredObjects)
      {
        object groupKey = parmameters.GroupByColumn.GetGroupKey(filteredObject);
        if (!nullableDictionary.ContainsKey(groupKey))
          nullableDictionary[groupKey] = new List<object>();
        nullableDictionary[groupKey].Add(filteredObject);
        ++capacity;
      }
      ModelObjectComparer modelObjectComparer = new ModelObjectComparer(parmameters.SortItemsByPrimaryColumn ? parmameters.ListView.GetColumn(0) : parmameters.PrimarySort, parmameters.PrimarySortOrder, parmameters.SecondarySort, parmameters.SecondarySortOrder);
      foreach (object key in (IEnumerable) nullableDictionary.Keys)
        nullableDictionary[key].Sort((IComparer<object>) modelObjectComparer);
      List<OLVGroup> olvGroupList = new List<OLVGroup>();
      foreach (object key in (IEnumerable) nullableDictionary.Keys)
      {
        string header = parmameters.GroupByColumn.ConvertGroupKeyToTitle(key);
        if (!string.IsNullOrEmpty(parmameters.TitleFormat))
        {
          int count = nullableDictionary[key].Count;
          string format = count == 1 ? parmameters.TitleSingularFormat : parmameters.TitleFormat;
          try
          {
            header = string.Format(format, (object) header, (object) count);
          }
          catch (FormatException ex)
          {
            header = "Invalid group format: " + format;
          }
        }
        OLVGroup group = new OLVGroup(header);
        group.Collapsible = folv.HasCollapsibleGroups;
        group.Key = key;
        group.SortValue = key as IComparable;
        group.Contents = (IList) nullableDictionary[key].ConvertAll<int>((Converter<object, int>) (x => folv.IndexOf(x)));
        group.VirtualItemCount = nullableDictionary[key].Count;
        if (parmameters.GroupByColumn.GroupFormatter != null)
          parmameters.GroupByColumn.GroupFormatter(group, parmameters);
        olvGroupList.Add(group);
      }
      if ((uint) parmameters.GroupByOrder > 0U)
        olvGroupList.Sort(parmameters.GroupComparer ?? (IComparer<OLVGroup>) new OLVGroupComparer(parmameters.GroupByOrder));
      this.indexToGroupMap = new List<int>(capacity);
      this.indexToGroupMap.AddRange((IEnumerable<int>) new int[capacity]);
      for (int index = 0; index < olvGroupList.Count; ++index)
      {
        foreach (int content in (List<int>) olvGroupList[index].Contents)
          this.indexToGroupMap[content] = index;
      }
      return (IList<OLVGroup>) olvGroupList;
    }

    public override int GetGroupMember(OLVGroup group, int indexWithinGroup) => (int) group.Contents[indexWithinGroup];

    public override int GetGroup(int itemIndex) => this.indexToGroupMap[itemIndex];

    public override int GetIndexWithinGroup(OLVGroup group, int itemIndex) => group.Contents.IndexOf((object) itemIndex);
  }
}
