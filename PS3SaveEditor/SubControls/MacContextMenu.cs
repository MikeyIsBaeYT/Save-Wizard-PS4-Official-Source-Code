// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SubControls.MacContextMenu
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor.Utilities;
using System;
using System.Windows.Forms;

namespace PS3SaveEditor.SubControls
{
  public class MacContextMenu
  {
    private readonly TextBox _txtBox;
    private ContextMenu _macMenu;

    public MacContextMenu(TextBox txtBox) => this._txtBox = txtBox;

    public ContextMenu GetMenu()
    {
      if (this._macMenu == null)
      {
        this._macMenu = new ContextMenu();
        this._macMenu.MenuItems.Add(new MenuItem("Undo", new EventHandler(this.UndoItemClick)));
        this._macMenu.MenuItems.Add(new MenuItem("-"));
        this._macMenu.MenuItems.Add(new MenuItem("Cut", new EventHandler(this.CutItemClick)));
        this._macMenu.MenuItems.Add(new MenuItem("Copy", new EventHandler(this.CopyItemClick)));
        this._macMenu.MenuItems.Add(new MenuItem("Paste", new EventHandler(this.PasteItemClick)));
        this._macMenu.MenuItems.Add(new MenuItem("Delete", new EventHandler(this.DeleteItemClick)));
        this._macMenu.MenuItems.Add(new MenuItem("-"));
        this._macMenu.MenuItems.Add(new MenuItem("Select All", new EventHandler(this.SelectAllItemClick)));
        this._macMenu.MenuItems[0].Enabled = false;
        this._macMenu.Popup += new EventHandler(this.CheckMenuAvailability);
      }
      return this._macMenu;
    }

    private void UndoItemClick(object sender, EventArgs e) => this._txtBox.Undo();

    private void CutItemClick(object sender, EventArgs e)
    {
      ClipboardMac.CopyToClipboard(this._txtBox);
      this._txtBox.SelectedText = string.Empty;
    }

    private void CopyItemClick(object sender, EventArgs e) => ClipboardMac.CopyToClipboard(this._txtBox);

    private void PasteItemClick(object sender, EventArgs e) => ClipboardMac.PasteFromClipboard(this._txtBox);

    private void DeleteItemClick(object sender, EventArgs e) => this._txtBox.SelectedText = string.Empty;

    private void SelectAllItemClick(object sender, EventArgs e) => this._txtBox.SelectAll();

    private void CheckMenuAvailability(object sender, EventArgs e)
    {
      this._macMenu.MenuItems[2].Enabled = this._txtBox.SelectionLength > 0 && !this._txtBox.ReadOnly;
      this._macMenu.MenuItems[3].Enabled = this._txtBox.SelectionLength > 0;
      this._macMenu.MenuItems[5].Enabled = this._txtBox.SelectionLength > 0 && !this._txtBox.ReadOnly;
      this._macMenu.MenuItems[4].Enabled = !this._txtBox.ReadOnly;
      this._macMenu.MenuItems[7].Enabled = this._txtBox.CanSelect;
    }
  }
}
