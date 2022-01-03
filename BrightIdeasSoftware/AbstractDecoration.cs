// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractDecoration : IDecoration, IOverlay
  {
    private OLVListItem listItem;
    private OLVListSubItem subItem;

    public OLVListItem ListItem
    {
      get => this.listItem;
      set => this.listItem = value;
    }

    public OLVListSubItem SubItem
    {
      get => this.subItem;
      set => this.subItem = value;
    }

    public Rectangle RowBounds => this.ListItem == null ? Rectangle.Empty : this.ListItem.Bounds;

    public Rectangle CellBounds => this.ListItem == null || this.SubItem == null ? Rectangle.Empty : this.ListItem.GetSubItemBounds(this.ListItem.SubItems.IndexOf((ListViewItem.ListViewSubItem) this.SubItem));

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
    }
  }
}
