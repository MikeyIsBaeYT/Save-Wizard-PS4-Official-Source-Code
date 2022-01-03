// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.DiffResults
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class DiffResults : Form
  {
    private IContainer components = (IContainer) null;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn StartAddress;
    private DataGridViewTextBoxColumn EndAddress;
    private DataGridViewTextBoxColumn Bytes;
    private Button btnClose;

    public event EventHandler OnDiffRowSelected;

    public Dictionary<long, byte> Differences
    {
      set
      {
        this.dataGridView1.Rows.Clear();
        foreach (long key in value.Keys)
        {
          int index = this.dataGridView1.Rows.Add();
          this.dataGridView1.Rows[index].Cells[0].Value = (object) key.ToString("X8");
          if (value[key] != (byte) 1)
            this.dataGridView1.Rows[index].Cells[1].Value = (object) (key + (long) value[key]).ToString("X8");
          this.dataGridView1.Rows[index].Cells[2].Value = (object) value[key].ToString("X2");
        }
      }
    }

    public DiffResults()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleDiffResults;
      this.dataGridView1.Columns[0].HeaderText = PS3SaveEditor.Resources.Resources.colStartAddr;
      this.dataGridView1.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colEndAddr;
      this.dataGridView1.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colBytes;
      this.CenterToScreen();
      this.dataGridView1.RowStateChanged += new DataGridViewRowStateChangedEventHandler(this.dataGridView1_RowStateChanged);
      this.FormClosing += new FormClosingEventHandler(this.DiffResults_FormClosing);
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
    }

    private void DiffResults_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = true;
      this.Hide();
    }

    private void dataGridView1_RowStateChanged(
      object sender,
      DataGridViewRowStateChangedEventArgs e)
    {
      if (e.StateChanged != DataGridViewElementStates.Selected || this.OnDiffRowSelected == null)
        return;
      this.OnDiffRowSelected(sender, EventArgs.Empty);
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Hide();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridView1 = new DataGridView();
      this.StartAddress = new DataGridViewTextBoxColumn();
      this.EndAddress = new DataGridViewTextBoxColumn();
      this.Bytes = new DataGridViewTextBoxColumn();
      this.btnClose = new Button();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AllowUserToResizeColumns = false;
      this.dataGridView1.AllowUserToResizeRows = false;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.StartAddress, (DataGridViewColumn) this.EndAddress, (DataGridViewColumn) this.Bytes);
      this.dataGridView1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.dataGridView1.MultiSelect = false;
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = Util.ScaleSize(new Size(322, 213));
      this.dataGridView1.TabIndex = 0;
      this.StartAddress.HeaderText = "Start Address";
      this.StartAddress.Name = "StartAddress";
      this.StartAddress.ReadOnly = true;
      this.EndAddress.HeaderText = "End Address";
      this.EndAddress.Name = "EndAddress";
      this.EndAddress.ReadOnly = true;
      this.EndAddress.Width = 120;
      this.Bytes.HeaderText = "Bytes";
      this.Bytes.Name = "Bytes";
      this.Bytes.ReadOnly = true;
      this.Bytes.Width = 90;
      this.btnClose.Location = new Point(Util.ScaleSize(26), Util.ScaleSize(239));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(75, 23));
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(346, 274));
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.dataGridView1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DiffResults);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = nameof (DiffResults);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
