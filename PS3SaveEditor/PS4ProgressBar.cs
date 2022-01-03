// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.PS4ProgressBar
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class PS4ProgressBar : ProgressBar
  {
    private IContainer components = (IContainer) null;

    public PS4ProgressBar()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      this.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 181, (int) byte.MaxValue), Color.FromArgb(0, 62, 207), 90f))
      {
        if (this.Value > 0)
          e.Graphics.FillRectangle((Brush) linearGradientBrush, 0.0f, 0.0f, (float) this.ClientRectangle.Width * (float) (this.Value + 1) / (float) this.Maximum, (float) this.ClientRectangle.Height);
        else
          e.Graphics.FillRectangle((Brush) linearGradientBrush, 0.0f, 0.0f, (float) this.ClientRectangle.Width * (float) this.Value / (float) this.Maximum, (float) this.ClientRectangle.Height);
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
      this.SuspendLayout();
      this.Style = ProgressBarStyle.Continuous;
      this.ResumeLayout(false);
    }
  }
}
