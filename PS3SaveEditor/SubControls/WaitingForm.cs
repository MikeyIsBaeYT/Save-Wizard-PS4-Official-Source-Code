// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SubControls.WaitingForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS3SaveEditor.SubControls
{
  public class WaitingForm : Form
  {
    private IContainer components = (IContainer) null;
    private Label waitLabel;
    private PS4ProgressBar prBar;
    private bool running = false;
    private WaitingForm.UpdateProgressDelegate UpdateProgress;
    private WaitingForm.CloseDelegate CloseForm;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.waitLabel = new Label();
      this.prBar = new PS4ProgressBar();
      this.SuspendLayout();
      this.waitLabel.AutoSize = true;
      this.waitLabel.Location = new Point(Util.ScaleSize(9), Util.ScaleSize(9));
      this.waitLabel.Name = "waitLabel";
      this.waitLabel.Size = Util.ScaleSize(new Size(187, 13));
      this.waitLabel.TabIndex = 0;
      this.waitLabel.Text = "Please wait. File opening in progress...";
      this.prBar.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(43));
      this.prBar.Name = "prBar";
      this.prBar.Size = Util.ScaleSize(new Size(346, 23));
      this.prBar.Style = ProgressBarStyle.Continuous;
      this.prBar.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackgroundImageLayout = ImageLayout.Center;
      this.ClientSize = Util.ScaleSize(new Size(370, 89));
      this.ControlBox = false;
      this.Controls.Add((Control) this.prBar);
      this.Controls.Add((Control) this.waitLabel);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (WaitingForm);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Please wait. File opening in progress...";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public WaitingForm()
    {
      this.InitializeComponent();
      this.waitLabel.ForeColor = Color.White;
      this.waitLabel.BackColor = Color.Transparent;
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Load += new EventHandler(this.WaitingForm_Load);
      this.UpdateProgress = new WaitingForm.UpdateProgressDelegate(this.UpdateProgressSafe);
      this.CloseForm = new WaitingForm.CloseDelegate(this.CloseFormSafe);
    }

    public WaitingForm(string waitingText)
      : this()
    {
      this.waitLabel.Text = waitingText;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    public void Start()
    {
      this.running = true;
      int num;
      Task.Run((Action) (() => num = (int) this.ShowDialog()));
    }

    public void Stop()
    {
      this.running = false;
      this.CloseThisForm(true);
    }

    private void ShowThisProgress()
    {
      int val = 1;
      while (this.running)
      {
        if (val > 100)
          val = 1;
        this.SetProgress(val);
        Thread.Sleep(500);
        ++val;
      }
    }

    private void SetProgress(int val) => this.prBar.Invoke((Delegate) this.UpdateProgress, (object) val);

    private void CloseThisForm(bool bSuccess)
    {
      if (this.IsDisposed)
        return;
      this.Invoke((Delegate) this.CloseForm, (object) bSuccess);
    }

    private void WaitingForm_Load(object sender, EventArgs e) => new Thread(new ThreadStart(this.ShowThisProgress)).Start();

    private void UpdateProgressSafe(int val) => this.prBar.Value = val;

    private void CloseFormSafe(bool bSuccess)
    {
      this.DialogResult = bSuccess ? DialogResult.OK : DialogResult.Abort;
      this.Close();
    }

    private delegate void UpdateProgressDelegate(int value);

    private delegate void CloseDelegate(bool bSuccess);
  }
}
