// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.AboutBox1
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  internal class AboutBox1 : Form
  {
    private IContainer components = (IContainer) null;
    private PictureBox pictureBox1;
    private Label lblVersion;
    private Label lblDesc;
    private Label lblCopyright;
    private LinkLabel linkLabel1;
    private Button btnOk;
    private Label osLabel;
    private Label frameworkVersion;
    private Label frameworkLabel;
    private Label osVersion;

    public AboutBox1()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = string.Format("About {0}", (object) this.AssemblyTitle);
      this.pictureBox1.Image = (Image) PS3SaveEditor.Resources.Resources.ps3se1;
      this.lblDesc.Visible = false;
      this.linkLabel1.Text = Util.IsHyperkin() ? "http://www.thesavewizard.com" : "http://www.savewizard.net/";
      this.lblVersion.Text = string.Format("Version {0}", (object) AboutBox1.AssemblyVersion);
      this.osVersion.Text = Util.GetOSVersion();
      this.frameworkVersion.Text = Util.GetFramework();
      this.lblCopyright.Text = this.AssemblyCopyright;
      this.lblDesc.Text = this.AssemblyCompany + (Util.CURRENT_SERVER == 0 ? "" : ".");
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
    }

    public string AssemblyTitle
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
        if ((uint) customAttributes.Length > 0U)
        {
          AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute) customAttributes[0];
          if (assemblyTitleAttribute.Title != "")
            return assemblyTitleAttribute.Title;
        }
        return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
      }
    }

    public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public string AssemblyDescription
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute) customAttributes[0]).Description;
      }
    }

    public string AssemblyProduct
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyProductAttribute) customAttributes[0]).Product;
      }
    }

    public string AssemblyCopyright
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) customAttributes[0]).Copyright;
      }
    }

    public string AssemblyCompany
    {
      get
      {
        object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
        return customAttributes.Length == 0 ? "" : ((AssemblyCompanyAttribute) customAttributes[0]).Company;
      }
    }

    private void btnOk_Click(object sender, EventArgs e) => this.Close();

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo(this.linkLabel1.Text));

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AboutBox1));
      this.lblVersion = new Label();
      this.lblDesc = new Label();
      this.lblCopyright = new Label();
      this.linkLabel1 = new LinkLabel();
      this.btnOk = new Button();
      this.pictureBox1 = new PictureBox();
      this.osLabel = new Label();
      this.frameworkVersion = new Label();
      this.frameworkLabel = new Label();
      this.osVersion = new Label();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(11));
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = Util.ScaleSize(new Size(0, 13));
      this.lblVersion.TabIndex = 2;
      this.lblDesc.AutoSize = true;
      this.lblDesc.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(30));
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = Util.ScaleSize(new Size(124, 13));
      this.lblDesc.TabIndex = 3;
      this.lblDesc.Text = "CYBER PS4 Save Editor";
      this.lblCopyright.AutoSize = true;
      this.lblCopyright.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(112));
      this.lblCopyright.Name = "lblCopyright";
      this.lblCopyright.Size = Util.ScaleSize(new Size(232, 13));
      this.lblCopyright.TabIndex = 4;
      this.lblCopyright.Text = "Copyright © CYBER Gadget. All rights reserved.";
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(133));
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = Util.ScaleSize(new Size(123, 13));
      this.linkLabel1.TabIndex = 5;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "http://cybergadget.co.jp";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.btnOk.Location = new Point(Util.ScaleSize(339), Util.ScaleSize(129));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOk.TabIndex = 6;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(11));
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = Util.ScaleSize(new Size(32, 32));
      this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.osLabel.AutoSize = true;
      this.osLabel.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(53));
      this.osLabel.Name = "osLabel";
      this.osLabel.Size = Util.ScaleSize(new Size(25, 13));
      this.osLabel.TabIndex = 7;
      this.osLabel.Text = "OS:";
      this.frameworkVersion.AutoSize = true;
      this.frameworkVersion.Location = new Point(Util.ScaleSize(131), Util.ScaleSize(76));
      this.frameworkVersion.Name = "frameworkVersion";
      this.frameworkVersion.Size = Util.ScaleSize(new Size(0, 13));
      this.frameworkVersion.TabIndex = 9;
      this.frameworkLabel.AutoSize = true;
      this.frameworkLabel.Location = new Point(Util.ScaleSize(59), Util.ScaleSize(76));
      this.frameworkLabel.Name = "frameworkLabel";
      this.frameworkLabel.Size = Util.ScaleSize(new Size(62, 13));
      this.frameworkLabel.TabIndex = 10;
      this.frameworkLabel.Text = "Framework:";
      this.osVersion.AutoSize = true;
      this.osVersion.Location = new Point(Util.ScaleSize(131), Util.ScaleSize(53));
      this.osVersion.Name = "osVersion";
      this.osVersion.Size = Util.ScaleSize(new Size(0, 13));
      this.osVersion.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = Util.ScaleSize(new Size(426, 164));
      this.Controls.Add((Control) this.osVersion);
      this.Controls.Add((Control) this.frameworkLabel);
      this.Controls.Add((Control) this.frameworkVersion);
      this.Controls.Add((Control) this.osLabel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.linkLabel1);
      this.Controls.Add((Control) this.lblCopyright);
      this.Controls.Add((Control) this.lblDesc);
      this.Controls.Add((Control) this.lblVersion);
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutBox1);
      this.Padding = new Padding(Util.ScaleSize(9));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "About PS4 Save Editor";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
