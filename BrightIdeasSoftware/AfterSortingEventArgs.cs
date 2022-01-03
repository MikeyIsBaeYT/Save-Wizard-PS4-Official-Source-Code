// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AfterSortingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AfterSortingEventArgs : EventArgs
  {
    private OLVColumn columnToGroupBy;
    private SortOrder groupByOrder;
    private OLVColumn columnToSort;
    private SortOrder sortOrder;
    private OLVColumn secondaryColumnToSort;
    private SortOrder secondarySortOrder;

    public AfterSortingEventArgs(
      OLVColumn groupColumn,
      SortOrder groupOrder,
      OLVColumn column,
      SortOrder order,
      OLVColumn column2,
      SortOrder order2)
    {
      this.columnToGroupBy = groupColumn;
      this.groupByOrder = groupOrder;
      this.columnToSort = column;
      this.sortOrder = order;
      this.secondaryColumnToSort = column2;
      this.secondarySortOrder = order2;
    }

    public AfterSortingEventArgs(BeforeSortingEventArgs args)
    {
      this.columnToGroupBy = args.ColumnToGroupBy;
      this.groupByOrder = args.GroupByOrder;
      this.columnToSort = args.ColumnToSort;
      this.sortOrder = args.SortOrder;
      this.secondaryColumnToSort = args.SecondaryColumnToSort;
      this.secondarySortOrder = args.SecondarySortOrder;
    }

    public OLVColumn ColumnToGroupBy => this.columnToGroupBy;

    public SortOrder GroupByOrder => this.groupByOrder;

    public OLVColumn ColumnToSort => this.columnToSort;

    public SortOrder SortOrder => this.sortOrder;

    public OLVColumn SecondaryColumnToSort => this.secondaryColumnToSort;

    public SortOrder SecondarySortOrder => this.secondarySortOrder;
  }
}
