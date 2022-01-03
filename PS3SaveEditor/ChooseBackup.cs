// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ChooseBackup
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class ChooseBackup : Form
  {
    private string m_saveFolder;
    private string m_save;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private ListBox lstBackups;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem deleteToolStripMenuItem;
    private Label lblGameName;
    private Button btnCancel;
    private Button btnRestore;

    public ChooseBackup(string gameName, string save, string saveFolder)
    {
      this.m_save = save;
      this.m_saveFolder = saveFolder;
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblGameName.BackColor = Color.Transparent;
      this.lblGameName.ForeColor = Color.White;
      this.CenterToScreen();
      this.deleteToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuDelete;
      this.lblGameName.Text = gameName;
      this.btnRestore.Text = PS3SaveEditor.Resources.Resources.btnRestore;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.Text = PS3SaveEditor.Resources.Resources.titleChooseBackup;
      this.LoadBackups();
      this.lstBackups.DisplayMember = "Timestamp";
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void LoadBackups()
    {
      List<BackupItem> backupItemList = new List<BackupItem>();
      foreach (string file in Directory.GetFiles(Util.GetBackupLocation(), this.m_save + "*"))
      {
        string fileName = Path.GetFileName(file);
        int num = fileName.LastIndexOf('.');
        if (num > 19)
          backupItemList.Add(new BackupItem()
          {
            BackupFile = file,
            Timestamp = fileName.Substring(num - 19, 19)
          });
      }
      this.lstBackups.DataSource = (object) backupItemList;
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.lstBackups.SelectedItem == null)
        return;
      File.Delete((this.lstBackups.SelectedItem as BackupItem).BackupFile);
      this.LoadBackups();
    }

    private void btnRestore_Click(object sender, EventArgs e)
    {
      if (this.lstBackups.SelectedItem == null)
        return;
      string backupFile = (this.lstBackups.SelectedItem as BackupItem).BackupFile;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnRestore, this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      try
      {
        int num1 = (int) new RestoreBackup(backupFile, Path.GetDirectoryName(this.m_saveFolder)).ShowDialog();
        int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgRestored);
      }
      catch (Exception ex)
      {
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.panel1 = new Panel();
      this.btnCancel = new Button();
      this.btnRestore = new Button();
      this.lstBackups = new ListBox();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.deleteToolStripMenuItem = new ToolStripMenuItem();
      this.lblGameName = new Label();
      this.panel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnRestore);
      this.panel1.Controls.Add((Control) this.lstBackups);
      this.panel1.Controls.Add((Control) this.lblGameName);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(262, 240));
      this.panel1.TabIndex = 0;
      this.btnCancel.Location = new Point(Util.ScaleSize(140), Util.ScaleSize(201));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnRestore.Location = new Point(Util.ScaleSize(39), Util.ScaleSize(201));
      this.btnRestore.Name = "btnRestore";
      this.btnRestore.Size = Util.ScaleSize(new Size(75, 23));
      this.btnRestore.TabIndex = 2;
      this.btnRestore.Text = "Restore";
      this.btnRestore.UseVisualStyleBackColor = true;
      this.btnRestore.Click += new EventHandler(this.btnRestore_Click);
      this.lstBackups.ContextMenuStrip = this.contextMenuStrip1;
      this.lstBackups.FormattingEnabled = true;
      this.lstBackups.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(30));
      this.lstBackups.Name = "lstBackups";
      this.lstBackups.Size = Util.ScaleSize(new Size(235, 160));
      this.lstBackups.TabIndex = 1;
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.deleteToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = Util.ScaleSize(new Size(108, 26));
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = Util.ScaleSize(new Size(107, 22));
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
      this.lblGameName.AutoSize = true;
      this.lblGameName.BackColor = Color.FromArgb(102, 102, 102);
      this.lblGameName.ForeColor = Color.White;
      this.lblGameName.Location = new Point(Util.ScaleSize(9), Util.ScaleSize(9));
      this.lblGameName.Name = "lblGameName";
      this.lblGameName.Size = Util.ScaleSize(new Size(0, 13));
      this.lblGameName.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(284, 262));
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChooseBackup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = nameof (ChooseBackup);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
