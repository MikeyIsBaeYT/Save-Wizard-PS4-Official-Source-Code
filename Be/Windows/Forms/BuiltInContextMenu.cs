// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.BuiltInContextMenu
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Be.Windows.Forms
{
  [TypeConverter(typeof (ExpandableObjectConverter))]
  public sealed class BuiltInContextMenu : Component
  {
    private HexBox _hexBox;
    private ContextMenuStrip _contextMenuStrip;
    private ToolStripMenuItem _cutToolStripMenuItem;
    private ToolStripMenuItem _copyToolStripMenuItem;
    private ToolStripMenuItem _pasteToolStripMenuItem;
    private ToolStripMenuItem _selectAllToolStripMenuItem;
    private string _copyMenuItemText;
    private string _cutMenuItemText;
    private string _pasteMenuItemText;
    private string _selectAllMenuItemText = (string) null;
    private Image _cutMenuItemImage = (Image) null;
    private Image _copyMenuItemImage = (Image) null;
    private Image _pasteMenuItemImage = (Image) null;
    private Image _selectAllMenuItemImage = (Image) null;

    internal BuiltInContextMenu(HexBox hexBox)
    {
      this._hexBox = hexBox;
      this._hexBox.ByteProviderChanged += new EventHandler(this.HexBox_ByteProviderChanged);
    }

    private void HexBox_ByteProviderChanged(object sender, EventArgs e) => this.CheckBuiltInContextMenu();

    private void CheckBuiltInContextMenu()
    {
      if (Util.DesignMode)
        return;
      if (this._contextMenuStrip == null)
      {
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        this._cutToolStripMenuItem = new ToolStripMenuItem(this.CutMenuItemTextInternal, this.CutMenuItemImage, new EventHandler(this.CutMenuItem_Click));
        contextMenuStrip.Items.Add((ToolStripItem) this._cutToolStripMenuItem);
        this._copyToolStripMenuItem = new ToolStripMenuItem(this.CopyMenuItemTextInternal, this.CopyMenuItemImage, new EventHandler(this.CopyMenuItem_Click));
        contextMenuStrip.Items.Add((ToolStripItem) this._copyToolStripMenuItem);
        this._pasteToolStripMenuItem = new ToolStripMenuItem(this.PasteMenuItemTextInternal, this.PasteMenuItemImage, new EventHandler(this.PasteMenuItem_Click));
        contextMenuStrip.Items.Add((ToolStripItem) this._pasteToolStripMenuItem);
        contextMenuStrip.Items.Add((ToolStripItem) new ToolStripSeparator());
        this._selectAllToolStripMenuItem = new ToolStripMenuItem(this.SelectAllMenuItemTextInternal, this.SelectAllMenuItemImage, new EventHandler(this.SelectAllMenuItem_Click));
        contextMenuStrip.Items.Add((ToolStripItem) this._selectAllToolStripMenuItem);
        contextMenuStrip.Opening += new CancelEventHandler(this.BuildInContextMenuStrip_Opening);
        this._contextMenuStrip = contextMenuStrip;
      }
      if (this._hexBox.ByteProvider == null && this._hexBox.ContextMenuStrip != null)
      {
        this._hexBox.ContextMenuStrip = (ContextMenuStrip) null;
      }
      else
      {
        if (this._hexBox.ByteProvider == null || this._hexBox.ContextMenuStrip != null)
          return;
        this._hexBox.ContextMenuStrip = this._contextMenuStrip;
      }
    }

    private void BuildInContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      this._cutToolStripMenuItem.Enabled = this._hexBox.CanCut();
      this._copyToolStripMenuItem.Enabled = this._hexBox.CanCopy();
      this._pasteToolStripMenuItem.Enabled = this._hexBox.CanPaste();
      this._selectAllToolStripMenuItem.Enabled = this._hexBox.CanSelectAll();
    }

    private void CutMenuItem_Click(object sender, EventArgs e) => this._hexBox.Copy();

    private void CopyMenuItem_Click(object sender, EventArgs e) => this._hexBox.Copy();

    private void PasteMenuItem_Click(object sender, EventArgs e) => this._hexBox.Copy();

    private void SelectAllMenuItem_Click(object sender, EventArgs e) => this._hexBox.SelectAll();

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    [Localizable(true)]
    public string CopyMenuItemText
    {
      get => this._copyMenuItemText;
      set => this._copyMenuItemText = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    [Localizable(true)]
    public string CutMenuItemText
    {
      get => this._cutMenuItemText;
      set => this._cutMenuItemText = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    [Localizable(true)]
    public string PasteMenuItemText
    {
      get => this._pasteMenuItemText;
      set => this._pasteMenuItemText = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    [Localizable(true)]
    public string SelectAllMenuItemText
    {
      get => this._selectAllMenuItemText;
      set => this._selectAllMenuItemText = value;
    }

    internal string CutMenuItemTextInternal => !string.IsNullOrEmpty(this.CutMenuItemText) ? this.CutMenuItemText : "Cut";

    internal string CopyMenuItemTextInternal => !string.IsNullOrEmpty(this.CopyMenuItemText) ? this.CopyMenuItemText : "Copy";

    internal string PasteMenuItemTextInternal => !string.IsNullOrEmpty(this.PasteMenuItemText) ? this.PasteMenuItemText : "Paste";

    internal string SelectAllMenuItemTextInternal => !string.IsNullOrEmpty(this.SelectAllMenuItemText) ? this.SelectAllMenuItemText : "SelectAll";

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    public Image CutMenuItemImage
    {
      get => this._cutMenuItemImage;
      set => this._cutMenuItemImage = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    public Image CopyMenuItemImage
    {
      get => this._copyMenuItemImage;
      set => this._copyMenuItemImage = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    public Image PasteMenuItemImage
    {
      get => this._pasteMenuItemImage;
      set => this._pasteMenuItemImage = value;
    }

    [Category("BuiltIn-ContextMenu")]
    [DefaultValue(null)]
    public Image SelectAllMenuItemImage
    {
      get => this._selectAllMenuItemImage;
      set => this._selectAllMenuItemImage = value;
    }
  }
}
