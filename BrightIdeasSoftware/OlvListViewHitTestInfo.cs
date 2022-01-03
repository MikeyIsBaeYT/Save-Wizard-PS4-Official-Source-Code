// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OlvListViewHitTestInfo
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OlvListViewHitTestInfo
  {
    public HitTestLocation HitTestLocation;
    public HitTestLocationEx HitTestLocationEx;
    public OLVGroup Group;
    public object UserData;
    private OLVListItem item;
    private OLVListSubItem subItem;
    private ListViewHitTestLocations location;
    private ObjectListView listView;
    private int columnIndex;
    private int headerDividerIndex = -1;

    public OlvListViewHitTestInfo(
      OLVListItem olvListItem,
      OLVListSubItem subItem,
      int flags,
      OLVGroup group,
      int iColumn)
    {
      this.item = olvListItem;
      this.subItem = subItem;
      this.location = OlvListViewHitTestInfo.ConvertNativeFlagsToDotNetLocation(olvListItem, flags);
      this.HitTestLocationEx = (HitTestLocationEx) flags;
      this.Group = group;
      this.ColumnIndex = iColumn;
      this.ListView = olvListItem == null ? (ObjectListView) null : (ObjectListView) olvListItem.ListView;
      switch (this.location)
      {
        case ListViewHitTestLocations.Image:
          this.HitTestLocation = HitTestLocation.Image;
          break;
        case ListViewHitTestLocations.Label:
          this.HitTestLocation = HitTestLocation.Text;
          break;
        case ListViewHitTestLocations.StateImage:
          this.HitTestLocation = HitTestLocation.CheckBox;
          break;
        default:
          if ((this.HitTestLocationEx & HitTestLocationEx.LVHT_EX_GROUP_COLLAPSE) == HitTestLocationEx.LVHT_EX_GROUP_COLLAPSE)
          {
            this.HitTestLocation = HitTestLocation.GroupExpander;
            break;
          }
          if ((uint) (this.HitTestLocationEx & HitTestLocationEx.LVHT_EX_GROUP_MINUS_FOOTER_AND_BKGRD) > 0U)
          {
            this.HitTestLocation = HitTestLocation.Group;
            break;
          }
          this.HitTestLocation = HitTestLocation.Nothing;
          break;
      }
    }

    public OlvListViewHitTestInfo(
      ObjectListView olv,
      int iColumn,
      bool isOverCheckBox,
      int iDivider)
    {
      this.ListView = olv;
      this.ColumnIndex = iColumn;
      this.HeaderDividerIndex = iDivider;
      this.HitTestLocation = isOverCheckBox ? HitTestLocation.HeaderCheckBox : (iDivider < 0 ? HitTestLocation.Header : HitTestLocation.HeaderDivider);
    }

    private static ListViewHitTestLocations ConvertNativeFlagsToDotNetLocation(
      OLVListItem hitItem,
      int flags)
    {
      return (8 & flags) == 8 ? (ListViewHitTestLocations) (247 & flags | (hitItem == null ? 256 : 512)) : (ListViewHitTestLocations) (flags & (int) ushort.MaxValue);
    }

    public OLVListItem Item
    {
      get => this.item;
      internal set => this.item = value;
    }

    public OLVListSubItem SubItem
    {
      get => this.subItem;
      internal set => this.subItem = value;
    }

    public ListViewHitTestLocations Location
    {
      get => this.location;
      internal set => this.location = value;
    }

    public ObjectListView ListView
    {
      get => this.listView;
      internal set => this.listView = value;
    }

    public object RowObject => this.Item == null ? (object) null : this.Item.RowObject;

    public int RowIndex => this.Item == null ? -1 : this.Item.Index;

    public int ColumnIndex
    {
      get => this.columnIndex;
      internal set => this.columnIndex = value;
    }

    public int HeaderDividerIndex
    {
      get => this.headerDividerIndex;
      internal set => this.headerDividerIndex = value;
    }

    public OLVColumn Column
    {
      get
      {
        int columnIndex = this.ColumnIndex;
        return columnIndex < 0 || this.ListView == null ? (OLVColumn) null : this.ListView.GetColumn(columnIndex);
      }
    }

    public override string ToString() => string.Format("HitTestLocation: {0}, HitTestLocationEx: {1}, Item: {2}, SubItem: {3}, Location: {4}, Group: {5}, ColumnIndex: {6}", (object) this.HitTestLocation, (object) this.HitTestLocationEx, (object) this.item, (object) this.subItem, (object) this.location, (object) this.Group, (object) this.ColumnIndex);

    internal class HeaderHitTestInfo
    {
      public int ColumnIndex;
      public bool IsOverCheckBox;
      public int OverDividerIndex;
    }
  }
}
