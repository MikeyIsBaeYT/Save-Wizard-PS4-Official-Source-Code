// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.SubItemCheckingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class SubItemCheckingEventArgs : CancellableEventArgs
  {
    private OLVColumn column;
    private OLVListItem listViewItem;
    private CheckState currentValue;
    private CheckState newValue;
    private int subItemIndex;

    public SubItemCheckingEventArgs(
      OLVColumn column,
      OLVListItem item,
      int subItemIndex,
      CheckState currentValue,
      CheckState newValue)
    {
      this.column = column;
      this.listViewItem = item;
      this.subItemIndex = subItemIndex;
      this.currentValue = currentValue;
      this.newValue = newValue;
    }

    public OLVColumn Column => this.column;

    public object RowObject => this.listViewItem.RowObject;

    public OLVListItem ListViewItem => this.listViewItem;

    public CheckState CurrentValue => this.currentValue;

    public CheckState NewValue
    {
      get => this.newValue;
      set => this.newValue = value;
    }

    public int SubItemIndex => this.subItemIndex;
  }
}
