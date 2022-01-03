// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FastObjectListView
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class FastObjectListView : VirtualObjectListView
  {
    public FastObjectListView()
    {
      this.VirtualListDataSource = (IVirtualListDataSource) new FastObjectListDataSource(this);
      this.GroupingStrategy = (IVirtualGroups) new FastListGroupingStrategy();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IEnumerable FilteredObjects => (IEnumerable) ((FastObjectListDataSource) this.VirtualListDataSource).FilteredObjectList;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IEnumerable Objects
    {
      get => (IEnumerable) ((FastObjectListDataSource) this.VirtualListDataSource).ObjectList;
      set => base.Objects = value;
    }

    public override void Unsort()
    {
      this.ShowGroups = false;
      this.PrimarySortColumn = (OLVColumn) null;
      this.PrimarySortOrder = SortOrder.None;
      this.SetObjects(this.Objects);
    }
  }
}
