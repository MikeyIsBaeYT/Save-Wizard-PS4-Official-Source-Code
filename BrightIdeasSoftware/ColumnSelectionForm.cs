// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ColumnSelectionForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ColumnSelectionForm : Form
  {
    private List<OLVColumn> AllColumns = (List<OLVColumn>) null;
    private List<OLVColumn> RearrangableColumns = new List<OLVColumn>();
    private Dictionary<OLVColumn, bool> MapColumnToVisible = new Dictionary<OLVColumn, bool>();
    private IContainer components = (IContainer) null;
    private ObjectListView objectListView1;
    private Button buttonMoveUp;
    private Button buttonMoveDown;
    private Button buttonShow;
    private Button buttonHide;
    private OLVColumn olvColumn1;
    private Label label1;
    private Button buttonOK;
    private Button buttonCancel;

    public ColumnSelectionForm()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
    }

    public void OpenOn(ObjectListView olv) => this.OpenOn(olv, olv.View);

    public void OpenOn(ObjectListView olv, View view)
    {
      if (view != View.Details && view != View.Tile)
        return;
      this.InitializeForm(olv, view);
      if (this.ShowDialog() != DialogResult.OK)
        return;
      this.Apply(olv, view);
    }

    protected void InitializeForm(ObjectListView olv, View view)
    {
      this.AllColumns = olv.AllColumns;
      this.RearrangableColumns = new List<OLVColumn>((IEnumerable<OLVColumn>) this.AllColumns);
      foreach (OLVColumn rearrangableColumn in this.RearrangableColumns)
        this.MapColumnToVisible[rearrangableColumn] = view != View.Details ? rearrangableColumn.IsTileViewColumn : rearrangableColumn.IsVisible;
      this.RearrangableColumns.Sort((IComparer<OLVColumn>) new ColumnSelectionForm.SortByDisplayOrder(this));
      this.objectListView1.BooleanCheckStateGetter = (BooleanCheckStateGetterDelegate) (rowObject => this.MapColumnToVisible[(OLVColumn) rowObject]);
      this.objectListView1.BooleanCheckStatePutter = (BooleanCheckStatePutterDelegate) ((rowObject, newValue) =>
      {
        OLVColumn key = (OLVColumn) rowObject;
        if (!key.CanBeHidden)
          return true;
        this.MapColumnToVisible[key] = newValue;
        this.EnableControls();
        return newValue;
      });
      this.objectListView1.SetObjects((IEnumerable) this.RearrangableColumns);
      this.EnableControls();
    }

    protected void Apply(ObjectListView olv, View view)
    {
      olv.Freeze();
      if (view == View.Details)
      {
        foreach (OLVColumn allColumn in olv.AllColumns)
          allColumn.IsVisible = this.MapColumnToVisible[allColumn];
      }
      else
      {
        foreach (OLVColumn allColumn in olv.AllColumns)
          allColumn.IsTileViewColumn = this.MapColumnToVisible[allColumn];
      }
      List<OLVColumn> all = this.RearrangableColumns.FindAll((Predicate<OLVColumn>) (x => this.MapColumnToVisible[x]));
      if (view == View.Details)
      {
        olv.ChangeToFilteredColumns(view);
        foreach (OLVColumn olvColumn in all)
        {
          olvColumn.DisplayIndex = all.IndexOf(olvColumn);
          olvColumn.LastDisplayIndex = olvColumn.DisplayIndex;
        }
      }
      else
      {
        OLVColumn allColumn = this.AllColumns[0];
        all.Remove(allColumn);
        olv.Columns.Clear();
        olv.Columns.Add((ColumnHeader) allColumn);
        olv.Columns.AddRange((ColumnHeader[]) all.ToArray());
        olv.CalculateReasonableTileSize();
      }
      olv.Unfreeze();
    }

    private void buttonMoveUp_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.objectListView1.SelectedIndices[0];
      OLVColumn rearrangableColumn = this.RearrangableColumns[selectedIndex];
      this.RearrangableColumns.RemoveAt(selectedIndex);
      this.RearrangableColumns.Insert(selectedIndex - 1, rearrangableColumn);
      this.objectListView1.BuildList();
      this.EnableControls();
    }

    private void buttonMoveDown_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.objectListView1.SelectedIndices[0];
      OLVColumn rearrangableColumn = this.RearrangableColumns[selectedIndex];
      this.RearrangableColumns.RemoveAt(selectedIndex);
      this.RearrangableColumns.Insert(selectedIndex + 1, rearrangableColumn);
      this.objectListView1.BuildList();
      this.EnableControls();
    }

    private void buttonShow_Click(object sender, EventArgs e) => this.objectListView1.SelectedItem.Checked = true;

    private void buttonHide_Click(object sender, EventArgs e) => this.objectListView1.SelectedItem.Checked = false;

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void objectListView1_SelectionChanged(object sender, EventArgs e) => this.EnableControls();

    protected void EnableControls()
    {
      if (this.objectListView1.SelectedIndices.Count == 0)
      {
        this.buttonMoveUp.Enabled = false;
        this.buttonMoveDown.Enabled = false;
        this.buttonShow.Enabled = false;
        this.buttonHide.Enabled = false;
      }
      else
      {
        this.buttonMoveUp.Enabled = (uint) this.objectListView1.SelectedIndices[0] > 0U;
        this.buttonMoveDown.Enabled = this.objectListView1.SelectedIndices[0] < this.objectListView1.GetItemCount() - 1;
        OLVColumn selectedObject = (OLVColumn) this.objectListView1.SelectedObject;
        this.buttonShow.Enabled = !this.MapColumnToVisible[selectedObject] && selectedObject.CanBeHidden;
        this.buttonHide.Enabled = this.MapColumnToVisible[selectedObject] && selectedObject.CanBeHidden;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonMoveUp = new Button();
      this.buttonMoveDown = new Button();
      this.buttonShow = new Button();
      this.buttonHide = new Button();
      this.label1 = new Label();
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.objectListView1 = new ObjectListView();
      this.olvColumn1 = new OLVColumn();
      ((ISupportInitialize) this.objectListView1).BeginInit();
      this.SuspendLayout();
      this.buttonMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonMoveUp.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(31));
      this.buttonMoveUp.Name = "buttonMoveUp";
      this.buttonMoveUp.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonMoveUp.TabIndex = 1;
      this.buttonMoveUp.Text = "Move &Up";
      this.buttonMoveUp.UseVisualStyleBackColor = true;
      this.buttonMoveUp.Click += new EventHandler(this.buttonMoveUp_Click);
      this.buttonMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonMoveDown.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(60));
      this.buttonMoveDown.Name = "buttonMoveDown";
      this.buttonMoveDown.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonMoveDown.TabIndex = 2;
      this.buttonMoveDown.Text = "Move &Down";
      this.buttonMoveDown.UseVisualStyleBackColor = true;
      this.buttonMoveDown.Click += new EventHandler(this.buttonMoveDown_Click);
      this.buttonShow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonShow.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(89));
      this.buttonShow.Name = "buttonShow";
      this.buttonShow.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonShow.TabIndex = 3;
      this.buttonShow.Text = "&Show";
      this.buttonShow.UseVisualStyleBackColor = true;
      this.buttonShow.Click += new EventHandler(this.buttonShow_Click);
      this.buttonHide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonHide.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(118));
      this.buttonHide.Name = "buttonHide";
      this.buttonHide.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonHide.TabIndex = 4;
      this.buttonHide.Text = "&Hide";
      this.buttonHide.UseVisualStyleBackColor = true;
      this.buttonHide.Click += new EventHandler(this.buttonHide_Click);
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BackColor = SystemColors.Control;
      this.label1.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(9));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(366, 19));
      this.label1.TabIndex = 5;
      this.label1.Text = "Choose the columns you want to see in this list. ";
      this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOK.Location = new Point(Util.ScaleSize(198), Util.ScaleSize(304));
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonOK.TabIndex = 6;
      this.buttonOK.Text = "&OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(304));
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = Util.ScaleSize(new Size(87, 23));
      this.buttonCancel.TabIndex = 7;
      this.buttonCancel.Text = "&Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
      this.objectListView1.AllColumns.Add(this.olvColumn1);
      this.objectListView1.AlternateRowBackColor = Color.FromArgb(192, (int) byte.MaxValue, 192);
      this.objectListView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.objectListView1.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
      this.objectListView1.CheckBoxes = true;
      this.objectListView1.Columns.AddRange(new ColumnHeader[1]
      {
        (ColumnHeader) this.olvColumn1
      });
      this.objectListView1.FullRowSelect = true;
      this.objectListView1.HeaderStyle = ColumnHeaderStyle.None;
      this.objectListView1.HideSelection = false;
      this.objectListView1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(31));
      this.objectListView1.MultiSelect = false;
      this.objectListView1.Name = "objectListView1";
      this.objectListView1.ShowGroups = false;
      this.objectListView1.ShowSortIndicators = false;
      this.objectListView1.Size = Util.ScaleSize(new Size(273, 259));
      this.objectListView1.TabIndex = 0;
      this.objectListView1.UseCompatibleStateImageBehavior = false;
      this.objectListView1.View = View.Details;
      this.objectListView1.SelectionChanged += new EventHandler(this.objectListView1_SelectionChanged);
      this.olvColumn1.AspectName = "Text";
      this.olvColumn1.IsVisible = true;
      this.olvColumn1.Text = "Column";
      this.olvColumn1.Width = Util.ScaleSize(267);
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = Util.ScaleSize(new Size(391, 339));
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonHide);
      this.Controls.Add((Control) this.buttonShow);
      this.Controls.Add((Control) this.buttonMoveDown);
      this.Controls.Add((Control) this.buttonMoveUp);
      this.Controls.Add((Control) this.objectListView1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ColumnSelectionForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Column Selection";
      ((ISupportInitialize) this.objectListView1).EndInit();
      this.ResumeLayout(false);
    }

    private class SortByDisplayOrder : IComparer<OLVColumn>
    {
      private ColumnSelectionForm Form;

      public SortByDisplayOrder(ColumnSelectionForm form) => this.Form = form;

      int IComparer<OLVColumn>.Compare(OLVColumn x, OLVColumn y)
      {
        if (this.Form.MapColumnToVisible[x] && !this.Form.MapColumnToVisible[y])
          return -1;
        if (!this.Form.MapColumnToVisible[x] && this.Form.MapColumnToVisible[y])
          return 1;
        return x.DisplayIndex == y.DisplayIndex ? x.Text.CompareTo(y.Text) : x.DisplayIndex - y.DisplayIndex;
      }
    }
  }
}
