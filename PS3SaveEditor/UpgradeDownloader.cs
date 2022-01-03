// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.UpgradeDownloader
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class UpgradeDownloader : Form
  {
    private string m_url;
    private string tempFile;
    private IContainer components = (IContainer) null;
    private PS4ProgressBar pbProgress;
    private Label lblStatus;
    private Panel panel1;

    public UpgradeDownloader(string url)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.m_url = url;
      this.lblStatus.Text = PS3SaveEditor.Resources.Resources.lblDownloadStatus;
      this.Text = PS3SaveEditor.Resources.Resources.titleUpgrader;
      this.CenterToScreen();
      this.lblStatus.BackColor = Color.Transparent;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.Load += new EventHandler(this.UpgradeDownloader_Load);
    }

    private void UpgradeDownloader_Load(object sender, EventArgs e)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");
      this.tempFile = Path.GetTempFileName();
      webClientEx.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.client_DownloadProgressChanged);
      webClientEx.DownloadFileCompleted += new AsyncCompletedEventHandler(this.client_DownloadFileCompleted);
      webClientEx.DownloadFileAsync(new Uri(this.m_url, UriKind.Absolute), this.tempFile, (object) this.tempFile);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errUpgrade);
      }
      else
      {
        new Process()
        {
          StartInfo = new ProcessStartInfo("msiexec", "/i \"" + this.tempFile + "\"")
        }.Start();
        this.Close();
      }
    }

    private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => this.pbProgress.Value = e.ProgressPercentage;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbProgress = new PS4ProgressBar();
      this.lblStatus = new Label();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pbProgress.Location = new Point(Util.ScaleSize(8), Util.ScaleSize(56));
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = Util.ScaleSize(new Size(409, 23));
      this.pbProgress.TabIndex = 0;
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(39));
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = Util.ScaleSize(new Size(143, 13));
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "Downloading latest version...";
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.lblStatus);
      this.panel1.Controls.Add((Control) this.pbProgress);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(432, 131));
      this.panel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(452, 155));
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UpgradeDownloader);
      this.ShowIcon = false;
      this.Text = "Downloading Latest Version";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
