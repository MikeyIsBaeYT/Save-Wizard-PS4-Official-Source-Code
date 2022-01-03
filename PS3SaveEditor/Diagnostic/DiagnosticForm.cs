// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Diagnostic.DiagnosticForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor.SubControls;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace PS3SaveEditor.Diagnostic
{
  public class DiagnosticForm : Form
  {
    private IContainer components = (IContainer) null;
    private TextBox infoBox;

    public DiagnosticForm()
    {
      this.Font = Util.GetFontForPlatform(this.Font);
      this.InitializeComponent();
      this.FillDiagnosticInfo();
    }

    private void FillDiagnosticInfo() => this.infoBox.Text = string.Format("App version - {0}\r\nOS version - {1}\r\nFramework - {2}\r\nProduct version - {3}", (object) Assembly.GetExecutingAssembly().GetName().Version.ToString(), (object) Util.GetOSVersion(), (object) Util.GetFramework(), (object) Util.pid);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.infoBox = new TextBox();
      this.SuspendLayout();
      this.infoBox.BorderStyle = BorderStyle.None;
      this.infoBox.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.infoBox.Multiline = true;
      this.infoBox.Name = "infoBox";
      this.infoBox.ReadOnly = true;
      this.infoBox.Size = Util.ScaleSize(new Size(352, 150));
      this.infoBox.TabIndex = 0;
      this.infoBox.Text = "test";
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.infoBox.ContextMenu = new MacContextMenu(this.infoBox).GetMenu();
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackgroundImageLayout = ImageLayout.Center;
      this.ClientSize = Util.ScaleSize(new Size(376, 174));
      this.Controls.Add((Control) this.infoBox);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Name = nameof (DiagnosticForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Diagnostic info";
      this.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
