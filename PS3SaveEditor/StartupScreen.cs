// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.StartupScreen
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace PS3SaveEditor
{
  public class StartupScreen : Form
  {
    private bool hasUpdate = false;
    private IContainer components = (IContainer) null;
    private Button btnCancel;
    private Button btnAccept;
    private HtmlPanel htmlPanel1;

    public StartupScreen(bool hasUpdate = false)
    {
      this.hasUpdate = hasUpdate;
      this.InitializeComponent();
      this.Text = Util.PRODUCT_NAME;
      this.Font = Util.GetFontForPlatform(this.Font);
      string str = "Please note this is a BETA version of Save Wizard for PS4 MAX which may have issues.<br/>We recommend as a precaution that any valuable saves are backed up manually by hand before using this BETA version.<br/>There is no auto-update feature in this BETA version, so please visit and bookmark <a href='http://www.savewizard.net/downloads/beta.php'>http://www.savewizard.net/downloads/beta.php</a> for future updates.<br/>Any issues should be emailed to <a href='mailto:beta@savewizard.net'>beta@savewizard.net</a>. Where possible, please include the OS you are using, the error message and error code shown and where possible how you caused the problem. Screenshots are acceptable.<br/>By using Save Wizard for PS4 MAX BETA, you agree that you have read both the EULA contained with this product AND that you fully understand that this is BETA grade product AND that its use may be withdrawn at any time.";
      if (hasUpdate)
        str = str + "<h2>New version available - " + Util.AvailableVersion + "</h2>";
      this.htmlPanel1.Text = "<style>*{font:Arial;font-size:" + (Util.ScaleSize(11).ToString() + "px") + ";color:#fff;} body,p,div{padding:0px;margin:0px;} a{color:#fff;}h2{color:red;}</style>" + "<body>" + str + "</body>";
      this.CenterToScreen();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Abort;
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
      this.btnCancel = new Button();
      this.btnAccept = new Button();
      this.htmlPanel1 = new HtmlPanel();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(Util.ScaleSize(270), Util.ScaleSize(190));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(124, 26));
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "I DO NOT ACCEPT";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnAccept.Location = new Point(Util.ScaleSize(169), Util.ScaleSize(190));
      this.btnAccept.Name = "btnAccept";
      this.btnAccept.Size = Util.ScaleSize(new Size(75, 26));
      this.btnAccept.TabIndex = 3;
      this.btnAccept.Text = "I ACCEPT";
      this.btnAccept.UseVisualStyleBackColor = true;
      this.btnAccept.Click += new EventHandler(this.btnAccept_Click);
      this.htmlPanel1.BackColor = Color.Transparent;
      this.htmlPanel1.BaseStylesheet = (string) null;
      this.htmlPanel1.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(8));
      this.htmlPanel1.Name = "htmlPanel1";
      this.htmlPanel1.Text = "<p> </p>";
      if (this.hasUpdate)
        this.htmlPanel1.Size = Util.ScaleSize(new Size(530, 250));
      else
        this.htmlPanel1.Size = Util.ScaleSize(new Size(530, 180));
      this.htmlPanel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(554, 222));
      if (!this.hasUpdate)
      {
        this.Controls.Add((Control) this.btnCancel);
        this.Controls.Add((Control) this.btnAccept);
      }
      this.Controls.Add((Control) this.htmlPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (StartupScreen);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = nameof (StartupScreen);
      this.ResumeLayout(false);
    }
  }
}
