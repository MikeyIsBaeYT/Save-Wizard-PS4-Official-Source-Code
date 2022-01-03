// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ModelObjectComparer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ModelObjectComparer : IComparer, IComparer<object>
  {
    private OLVColumn column;
    private SortOrder sortOrder;
    private ModelObjectComparer secondComparer;

    public ModelObjectComparer(OLVColumn col, SortOrder order)
    {
      this.column = col;
      this.sortOrder = order;
    }

    public ModelObjectComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
      : this(col, order)
    {
      if (col == col2 || col2 == null || (uint) order2 <= 0U)
        return;
      this.secondComparer = new ModelObjectComparer(col2, order2);
    }

    public int Compare(object x, object y)
    {
      object x1 = this.column.GetValue(x);
      object y1 = this.column.GetValue(y);
      if (this.sortOrder == SortOrder.None)
        return 0;
      bool flag1 = x1 == null || x1 == DBNull.Value;
      bool flag2 = y1 == null || y1 == DBNull.Value;
      int num = !(flag1 | flag2) ? this.CompareValues(x1, y1) : (!(flag1 & flag2) ? (flag1 ? -1 : 1) : 0);
      if (this.sortOrder == SortOrder.Descending)
        num = -num;
      if (num == 0 && this.secondComparer != null)
        num = this.secondComparer.Compare(x, y);
      return num;
    }

    public int CompareValues(object x, object y)
    {
      switch (x)
      {
        case string strA:
          return string.Compare(strA, (string) y, StringComparison.CurrentCultureIgnoreCase);
        case IComparable comparable:
          return comparable.CompareTo(y);
        default:
          return 0;
      }
    }
  }
}
