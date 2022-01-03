// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVGroupComparer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVGroupComparer : IComparer<OLVGroup>
  {
    private SortOrder sortOrder;

    public OLVGroupComparer(SortOrder order) => this.sortOrder = order;

    public int Compare(OLVGroup x, OLVGroup y)
    {
      int num = x.SortValue == null || y.SortValue == null ? string.Compare(x.Header, y.Header, StringComparison.CurrentCultureIgnoreCase) : x.SortValue.CompareTo((object) y.SortValue);
      if (this.sortOrder == SortOrder.Descending)
        num = -num;
      return num;
    }
  }
}
