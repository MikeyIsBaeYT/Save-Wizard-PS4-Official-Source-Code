// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ResignMessage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class ResignMessage : Form
  {
    private IContainer components = (IContainer) null;
    private CheckBox chkDeleteExisting;
    private Label lblResignSuccess;
    private Panel panel1;
    private Button btnOK;

    public bool DeleteExisting { get; set; }

    public ResignMessage()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleResignInfo;
      this.lblResignSuccess.Text = PS3SaveEditor.Resources.Resources.lblResignSuccess;
      this.chkDeleteExisting.Text = PS3SaveEditor.Resources.Resources.chkDeleteExisting;
      this.btnOK.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.CenterToScreen();
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
    }

    public ResignMessage(bool showDelete)
      : this()
    {
      this.chkDeleteExisting.Visible = showDelete;
      this.chkDeleteExisting.Checked = showDelete;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DeleteExisting = this.chkDeleteExisting.Checked;
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
      this.chkDeleteExisting = new CheckBox();
      this.lblResignSuccess = new Label();
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.chkDeleteExisting.AutoSize = true;
      this.chkDeleteExisting.Location = new Point(Util.ScaleSize(71), Util.ScaleSize(50));
      this.chkDeleteExisting.Name = "chkDeleteExisting";
      this.chkDeleteExisting.Size = Util.ScaleSize(new Size(137, 17));
      this.chkDeleteExisting.TabIndex = 0;
      this.chkDeleteExisting.Text = "Delete the original save";
      this.chkDeleteExisting.UseVisualStyleBackColor = true;
      this.lblResignSuccess.AutoSize = true;
      this.lblResignSuccess.Location = new Point(Util.ScaleSize(86), Util.ScaleSize(17));
      this.lblResignSuccess.Name = "lblResignSuccess";
      this.lblResignSuccess.Size = Util.ScaleSize(new Size(110, 13));
      this.lblResignSuccess.TabIndex = 1;
      this.lblResignSuccess.Text = "Re-signing successful";
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.chkDeleteExisting);
      this.panel1.Controls.Add((Control) this.lblResignSuccess);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(277, 115));
      this.panel1.TabIndex = 2;
      this.btnOK.Location = new Point(Util.ScaleSize(102), Util.ScaleSize(83));
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(297, 135));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ResignMessage);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = nameof (ResignMessage);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
