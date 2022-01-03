// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.RestoreBackup
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class RestoreBackup : Form
  {
    private string m_backupFile;
    private string m_destFolder;
    private bool m_bActivated = false;
    private RestoreBackup.UpdateProgressDelegate UpdateProgress;
    private RestoreBackup.CloseDelegate CloseForm;
    private IContainer components = (IContainer) null;
    private PS4ProgressBar pbProgress;
    private Label lblProgress;
    private Panel panel1;

    public RestoreBackup(string backupFile, string destFolder)
    {
      this.m_backupFile = backupFile;
      this.m_destFolder = destFolder;
      this.UpdateProgress = new RestoreBackup.UpdateProgressDelegate(this.UpdateProgressSafe);
      this.CloseForm = new RestoreBackup.CloseDelegate(this.CloseFormSafe);
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblProgress.BackColor = Color.Transparent;
      this.lblProgress.ForeColor = Color.White;
      this.lblProgress.Text = PS3SaveEditor.Resources.Resources.lblRestoring;
      this.CenterToScreen();
      this.Load += new EventHandler(this.RestoreBackup_Load);
      this.Activated += new EventHandler(this.RestoreBackup_Activated);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void RestoreBackup_Activated(object sender, EventArgs e)
    {
      if (this.m_bActivated)
        return;
      this.m_bActivated = true;
    }

    private void RestoreBackup_Load(object sender, EventArgs e) => new Thread(new ThreadStart(this.ExtractBackup)).Start();

    private void UpdateProgressSafe(int val) => this.pbProgress.Value = val;

    private void ExtractBackup()
    {
      ZipFile zipFile = ZipFile.Read(this.m_backupFile);
      zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.zipFile_ExtractProgress);
      zipFile.ExtractAll(this.m_destFolder, ExtractExistingFileAction.InvokeExtractProgressEvent);
    }

    private void zipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
    {
      if (e.EventType == ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite)
        e.CurrentEntry.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
      if (e.TotalBytesToTransfer > 100L)
        this.pbProgress.Invoke((Delegate) this.UpdateProgress, (object) (int) (e.BytesTransferred * 100L / e.TotalBytesToTransfer));
      if (e.EventType != ZipProgressEventType.Extracting_AfterExtractAll)
        return;
      this.Invoke((Delegate) this.CloseForm, (object) true);
    }

    private void CloseFormSafe(bool bSuccess)
    {
      if (!bSuccess)
        this.DialogResult = DialogResult.Abort;
      else
        this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbProgress = new PS4ProgressBar();
      this.lblProgress = new Label();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pbProgress.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(27));
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = Util.ScaleSize(new Size(257, 20));
      this.pbProgress.TabIndex = 0;
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new Point(Util.ScaleSize(5), Util.ScaleSize(9));
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = Util.ScaleSize(new Size(0, 13));
      this.lblProgress.TabIndex = 1;
      this.panel1.Controls.Add((Control) this.lblProgress);
      this.panel1.Controls.Add((Control) this.pbProgress);
      this.panel1.Location = new Point(Util.ScaleSize(9), Util.ScaleSize(9));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(263, 73));
      this.panel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(284, 94));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RestoreBackup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Restore Backup";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void UpdateProgressDelegate(int value);

    private delegate void CloseDelegate(bool bSuccess);
  }
}
