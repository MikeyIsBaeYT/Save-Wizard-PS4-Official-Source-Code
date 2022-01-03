// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CancelPSNIDs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class CancelPSNIDs : Form
  {
    private const string UNREGISTER_PSNID = "{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}";
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn UserName;
    private Button btnCancel;
    private Button btnClose;

    public CancelPSNIDs(Dictionary<string, object> registered)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleCancelAccount;
      this.CenterToScreen();
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancellation;
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.dataGridView1.SelectionChanged += new EventHandler(this.dataGridView1_SelectionChanged);
      this.dataGridView1.MultiSelect = false;
      this.btnCancel.Enabled = false;
      foreach (string key in registered.Keys)
      {
        Dictionary<string, object> dictionary = registered[key] as Dictionary<string, object>;
        int index = this.dataGridView1.Rows.Add();
        this.dataGridView1.Rows[index].Cells[0].Value = dictionary["friendly_name"];
        this.dataGridView1.Rows[index].Tag = (object) key;
        this.dataGridView1.Rows[index].Cells[0].Tag = (object) true;
      }
    }

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
      this.btnCancel.Enabled = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dataGridView1.SelectedRows)
      {
        if ((bool) selectedRow.Cells[0].Tag)
        {
          this.btnCancel.Enabled = true;
          break;
        }
      }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void CancelPSNIDs_Load(object sender, EventArgs e)
    {
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmDeactivateAccount, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.dataGridView1.Rows)
      {
        if (row.Selected && (bool) row.Cells[0].Tag)
          this.UnregisterPSNID((string) row.Tag);
      }
      this.DialogResult = DialogResult.Yes;
      this.Close();
    }

    private bool UnregisterPSNID(string psnId)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Encoding = Encoding.UTF8;
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetAuthBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}", (object) Util.GetUserId(), (object) psnId)))), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      return dictionary.ContainsKey("status") && (string) dictionary["status"] == "OK";
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CancelPSNIDs));
      this.panel1 = new Panel();
      this.dataGridView1 = new DataGridView();
      this.UserName = new DataGridViewTextBoxColumn();
      this.btnCancel = new Button();
      this.btnClose = new Button();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.dataGridView1);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(260, 179));
      this.panel1.TabIndex = 0;
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.UserName);
      this.dataGridView1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(15));
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.RowHeadersVisible = false;
      this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = Util.ScaleSize(new Size(237, 150));
      this.dataGridView1.TabIndex = 0;
      this.UserName.HeaderText = "UserName";
      this.UserName.Name = "UserName";
      this.UserName.ReadOnly = true;
      this.btnCancel.Location = new Point(Util.ScaleSize(62), Util.ScaleSize(195));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnClose.Location = new Point(Util.ScaleSize(143), Util.ScaleSize(195));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(75, 23));
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(284, 228));
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CancelPSNIDs);
      this.ShowInTaskbar = false;
      this.Text = nameof (CancelPSNIDs);
      this.Load += new EventHandler(this.CancelPSNIDs_Load);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
