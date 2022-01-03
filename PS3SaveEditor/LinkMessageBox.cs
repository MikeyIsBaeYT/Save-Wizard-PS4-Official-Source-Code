// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.LinkMessageBox
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class LinkMessageBox : Form
  {
    private string m_url;
    private IContainer components = (IContainer) null;
    private Label lblMessage;
    private LinkLabel linkLabel1;
    private Button btnOK;

    public LinkMessageBox(string message, string linkUrl)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.m_url = linkUrl;
      this.Text = Util.PRODUCT_NAME;
      if (!string.IsNullOrEmpty(linkUrl))
        this.linkLabel1.Click += new EventHandler(this.linkLabel1_Click);
      else
        this.linkLabel1.Visible = false;
      this.lblMessage.Text = message;
      this.linkLabel1.Text = PS3SaveEditor.Resources.Resources.lnkContactSupport;
      this.btnOK.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void linkLabel1_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo()
    {
      Verb = "open",
      FileName = this.m_url,
      UseShellExecute = true
    });

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMessage = new Label();
      this.linkLabel1 = new LinkLabel();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.lblMessage.AutoSize = false;
      this.lblMessage.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(11.25f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblMessage.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(16));
      this.lblMessage.MaximumSize = Util.ScaleSize(new Size(520, 140));
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = Util.ScaleSize(new Size(520, 180));
      this.lblMessage.TabIndex = 0;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(108));
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = Util.ScaleSize(new Size(84, 13));
      this.linkLabel1.TabIndex = 1;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "Contact Support";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(Util.ScaleSize(214), Util.ScaleSize(108));
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = Util.ScaleSize(new Size(503, 143));
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.linkLabel1);
      this.Controls.Add((Control) this.lblMessage);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LinkMessageBox);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (LinkMessageBox);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
