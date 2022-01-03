// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ResignInfo
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class ResignInfo : Form
  {
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Button btnOk;
    private CheckBox chkDontShow;
    private Label textBox1;

    public ResignInfo()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.CenterToScreen();
      this.Load += new EventHandler(this.ResignInfo_Load);
      this.textBox1.Text = PS3SaveEditor.Resources.Resources.descResign;
      this.Text = PS3SaveEditor.Resources.Resources.titleResignMessage;
      this.chkDontShow.Text = PS3SaveEditor.Resources.Resources.chkDontShowResign;
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
    }

    private void ResignInfo_Load(object sender, EventArgs e) => this.btnOk.Focus();

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.chkDontShow.Checked)
        Util.SetRegistryValue("NoResignMessage", "yes");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResignInfo));
      this.panel1 = new Panel();
      this.btnOk = new Button();
      this.chkDontShow = new CheckBox();
      this.textBox1 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.chkDontShow);
      this.panel1.Controls.Add((Control) this.textBox1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(566, 184));
      this.panel1.TabIndex = 0;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(Util.ScaleSize(243), Util.ScaleSize(153));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.chkDontShow.AutoSize = true;
      this.chkDontShow.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(154));
      this.chkDontShow.Name = "chkDontShow";
      this.chkDontShow.Size = Util.ScaleSize(new Size(179, 17));
      this.chkDontShow.TabIndex = 1;
      this.chkDontShow.Text = "Do not show this message again";
      this.chkDontShow.UseVisualStyleBackColor = true;
      this.textBox1.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(10));
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = Util.ScaleSize(new Size(539, 135));
      this.textBox1.TabIndex = 0;
      this.textBox1.Text = componentResourceManager.GetString("textBox1.Text");
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(586, 204));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ResignInfo);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Important Information";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
