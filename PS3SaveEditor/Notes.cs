// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Notes
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace PS3SaveEditor
{
  public class Notes : Form
  {
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Button btnOk;
    private HtmlPanel htmlPanel1;

    public Notes(string notes)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.CenterToScreen();
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      string str = Util.ScaleSize(12).ToString() + "px";
      this.htmlPanel1.Text = "<style>*{font:'" + Util.GetFontFamily() + "';font-size:" + str + ";color:#000;} p,div{padding-bottom:4px;} </style>" + "<body>" + notes + "</body>";
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      if (Util.CurrentPlatform != Util.Platform.Linux)
        return;
      this.htmlPanel1.Scroll += new ScrollEventHandler(this.HtmlPanel_Scroll);
    }

    private void btnOk_Click(object sender, EventArgs e) => this.Close();

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private async void HtmlPanel_Scroll(object sender, ScrollEventArgs e)
    {
      await Task.Delay(20);
      this.htmlPanel1.ClearSelection();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.htmlPanel1 = new HtmlPanel();
      this.btnOk = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.htmlPanel1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(Util.ScaleSize(12));
      this.panel1.Size = Util.ScaleSize(new Size(548, 256));
      this.panel1.TabIndex = 0;
      this.htmlPanel1.BackColor = Color.White;
      this.htmlPanel1.BaseStylesheet = (string) null;
      this.htmlPanel1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.htmlPanel1.Name = "htmlPanel1";
      this.htmlPanel1.Text = "<p> </p>";
      this.htmlPanel1.Size = Util.ScaleSize(new Size(524, 206));
      this.htmlPanel1.TabIndex = 2;
      this.btnOk.Location = new Point(Util.ScaleSize(236), Util.ScaleSize(225));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(568, 276));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Notes);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = nameof (Notes);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
