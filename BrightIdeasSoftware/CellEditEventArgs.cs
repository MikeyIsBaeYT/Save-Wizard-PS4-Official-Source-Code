// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CellEditEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class CellEditEventArgs : EventArgs
  {
    public bool Cancel;
    public Control Control;
    private OLVColumn column;
    private object rowObject;
    private OLVListItem listViewItem;
    private object newValue;
    private int subItemIndex;
    private object value;
    private Rectangle cellBounds;
    private bool autoDispose = true;

    public CellEditEventArgs(
      OLVColumn column,
      Control control,
      Rectangle r,
      OLVListItem item,
      int subItemIndex)
    {
      this.Control = control;
      this.column = column;
      this.cellBounds = r;
      this.listViewItem = item;
      this.rowObject = item.RowObject;
      this.subItemIndex = subItemIndex;
      this.value = column.GetValue(item.RowObject);
    }

    public OLVColumn Column => this.column;

    public object RowObject => this.rowObject;

    public OLVListItem ListViewItem => this.listViewItem;

    public object NewValue
    {
      get => this.newValue;
      set => this.newValue = value;
    }

    public int SubItemIndex => this.subItemIndex;

    public object Value => this.value;

    public Rectangle CellBounds => this.cellBounds;

    public bool AutoDispose
    {
      get => this.autoDispose;
      set => this.autoDispose = value;
    }
  }
}
