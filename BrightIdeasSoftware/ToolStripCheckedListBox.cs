// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ToolStripCheckedListBox
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ToolStripCheckedListBox : ToolStripControlHost
  {
    public ToolStripCheckedListBox()
      : base((Control) new CheckedListBox())
    {
      this.CheckedListBoxControl.MaximumSize = new Size(400, 700);
      this.CheckedListBoxControl.ThreeDCheckBoxes = true;
      this.CheckedListBoxControl.CheckOnClick = true;
      this.CheckedListBoxControl.SelectionMode = SelectionMode.One;
    }

    public CheckedListBox CheckedListBoxControl => this.Control as CheckedListBox;

    public CheckedListBox.ObjectCollection Items => this.CheckedListBoxControl.Items;

    public bool CheckedOnClick
    {
      get => this.CheckedListBoxControl.CheckOnClick;
      set => this.CheckedListBoxControl.CheckOnClick = value;
    }

    public CheckedListBox.CheckedItemCollection CheckedItems => this.CheckedListBoxControl.CheckedItems;

    public void AddItem(object item, bool isChecked)
    {
      this.Items.Add(item);
      if (!isChecked)
        return;
      this.CheckedListBoxControl.SetItemChecked(this.Items.Count - 1, true);
    }

    public void AddItem(object item, CheckState state)
    {
      this.Items.Add(item);
      this.CheckedListBoxControl.SetItemCheckState(this.Items.Count - 1, state);
    }

    public CheckState GetItemCheckState(int i) => this.CheckedListBoxControl.GetItemCheckState(i);

    public void SetItemState(int i, CheckState checkState)
    {
      if (i < 0 || i >= this.Items.Count)
        return;
      this.CheckedListBoxControl.SetItemCheckState(i, checkState);
    }

    public void CheckAll()
    {
      for (int index = 0; index < this.Items.Count; ++index)
        this.CheckedListBoxControl.SetItemChecked(index, true);
    }

    public void UncheckAll()
    {
      for (int index = 0; index < this.Items.Count; ++index)
        this.CheckedListBoxControl.SetItemChecked(index, false);
    }

    protected override void OnSubscribeControlEvents(Control c)
    {
      base.OnSubscribeControlEvents(c);
      ((CheckedListBox) c).ItemCheck += new ItemCheckEventHandler(this.OnItemCheck);
    }

    protected override void OnUnsubscribeControlEvents(Control c)
    {
      base.OnUnsubscribeControlEvents(c);
      ((CheckedListBox) c).ItemCheck -= new ItemCheckEventHandler(this.OnItemCheck);
    }

    public event ItemCheckEventHandler ItemCheck;

    private void OnItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.ItemCheck == null)
        return;
      this.ItemCheck((object) this, e);
    }
  }
}
