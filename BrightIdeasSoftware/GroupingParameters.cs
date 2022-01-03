// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.GroupingParameters
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class GroupingParameters
  {
    private ObjectListView listView;
    private OLVColumn groupByColumn;
    private SortOrder groupByOrder;
    private IComparer<OLVGroup> groupComparer;
    private IComparer<OLVListItem> itemComparer;
    private OLVColumn primarySort;
    private SortOrder primarySortOrder;
    private OLVColumn secondarySort;
    private SortOrder secondarySortOrder;
    private string titleFormat;
    private string titleSingularFormat;
    private bool sortItemsByPrimaryColumn;

    public GroupingParameters(
      ObjectListView olv,
      OLVColumn groupByColumn,
      SortOrder groupByOrder,
      OLVColumn column,
      SortOrder order,
      OLVColumn secondaryColumn,
      SortOrder secondaryOrder,
      string titleFormat,
      string titleSingularFormat,
      bool sortItemsByPrimaryColumn)
    {
      this.ListView = olv;
      this.GroupByColumn = groupByColumn;
      this.GroupByOrder = groupByOrder;
      this.PrimarySort = column;
      this.PrimarySortOrder = order;
      this.SecondarySort = secondaryColumn;
      this.SecondarySortOrder = secondaryOrder;
      this.SortItemsByPrimaryColumn = sortItemsByPrimaryColumn;
      this.TitleFormat = titleFormat;
      this.TitleSingularFormat = titleSingularFormat;
    }

    public ObjectListView ListView
    {
      get => this.listView;
      set => this.listView = value;
    }

    public OLVColumn GroupByColumn
    {
      get => this.groupByColumn;
      set => this.groupByColumn = value;
    }

    public SortOrder GroupByOrder
    {
      get => this.groupByOrder;
      set => this.groupByOrder = value;
    }

    public IComparer<OLVGroup> GroupComparer
    {
      get => this.groupComparer;
      set => this.groupComparer = value;
    }

    public IComparer<OLVListItem> ItemComparer
    {
      get => this.itemComparer;
      set => this.itemComparer = value;
    }

    public OLVColumn PrimarySort
    {
      get => this.primarySort;
      set => this.primarySort = value;
    }

    public SortOrder PrimarySortOrder
    {
      get => this.primarySortOrder;
      set => this.primarySortOrder = value;
    }

    public OLVColumn SecondarySort
    {
      get => this.secondarySort;
      set => this.secondarySort = value;
    }

    public SortOrder SecondarySortOrder
    {
      get => this.secondarySortOrder;
      set => this.secondarySortOrder = value;
    }

    public string TitleFormat
    {
      get => this.titleFormat;
      set => this.titleFormat = value;
    }

    public string TitleSingularFormat
    {
      get => this.titleSingularFormat;
      set => this.titleSingularFormat = value;
    }

    public bool SortItemsByPrimaryColumn
    {
      get => this.sortItemsByPrimaryColumn;
      set => this.sortItemsByPrimaryColumn = value;
    }
  }
}
