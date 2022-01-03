// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ChooseProfile
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class ChooseProfile : Form
  {
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Button btnCancel;
    private Button btnSelect;
    private ComboBox cbProfiles;

    public string SelectedAccount { get; set; }

    public ChooseProfile(Dictionary<string, object> accounts, string curProfile)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleChooseProfile;
      this.btnSelect.Text = PS3SaveEditor.Resources.Resources.btnApplyPatch;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      List<ProfileItem> profileItemList = new List<ProfileItem>();
      foreach (string key in accounts.Keys)
      {
        if (!(key == curProfile))
          profileItemList.Add(new ProfileItem()
          {
            PSNID = key,
            Name = (accounts[key] as Dictionary<string, object>)["friendly_name"] as string
          });
      }
      this.cbProfiles.DisplayMember = "Name";
      this.cbProfiles.ValueMember = "PSNID";
      this.cbProfiles.DataSource = (object) profileItemList;
      if (this.cbProfiles.Items.Count > 0)
        this.cbProfiles.SelectedIndex = 0;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.Load += new EventHandler(this.ChooseProfile_Load);
    }

    private void ChooseProfile_Load(object sender, EventArgs e)
    {
      if (this.cbProfiles.Items.Count != 0)
        return;
      int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgNoAltProfile);
      this.Close();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.cbProfiles.SelectedValue == null)
        return;
      this.SelectedAccount = this.cbProfiles.SelectedValue as string;
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
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.cbProfiles = new ComboBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnSelect);
      this.panel1.Controls.Add((Control) this.cbProfiles);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Margin = new Padding(Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(246, 68));
      this.panel1.TabIndex = 3;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(Util.ScaleSize(135), Util.ScaleSize(39));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.DialogResult = DialogResult.OK;
      this.btnSelect.Location = new Point(Util.ScaleSize(37), Util.ScaleSize(38));
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = Util.ScaleSize(new Size(75, 23));
      this.btnSelect.TabIndex = 4;
      this.btnSelect.Text = "Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.cbProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProfiles.FormattingEnabled = true;
      this.cbProfiles.Location = new Point(Util.ScaleSize(37), Util.ScaleSize(10));
      this.cbProfiles.Name = "cbProfiles";
      this.cbProfiles.Size = Util.ScaleSize(new Size(173, 21));
      this.cbProfiles.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(266, 88));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChooseProfile);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Choose PSN Account";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
