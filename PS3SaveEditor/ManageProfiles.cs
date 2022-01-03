// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ManageProfiles
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
  public class ManageProfiles : Form
  {
    private string m_newPSN_ID;
    private Dictionary<string, object> m_registered = (Dictionary<string, object>) null;
    private const string REGISTER_PSNID = "{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";
    private const string UNREGISTER_PSNID = "{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}";
    private const string RENAME_PSNID = "{{\"action\":\"RENAME_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";
    private IContainer components = (IContainer) null;
    private DataGridView dgProfiles;
    private Button btnSave;
    private Button btnClose;
    private Panel panel1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem deleteToolStripMenuItem;
    private ToolStripMenuItem renameToolStripMenuItem;
    private DataGridViewTextBoxColumn Removable;
    private DataGridViewTextBoxColumn _Name;
    private DataGridViewTextBoxColumn ID;

    public string PsnIDResponse { get; set; }

    public ManageProfiles(string psnid, Dictionary<string, object> registered)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.m_registered = registered;
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
      this.Text = PS3SaveEditor.Resources.Resources.titleManageProfiles;
      this.dgProfiles.Columns[0].HeaderText = PS3SaveEditor.Resources.Resources.colProfileName;
      this.dgProfiles.Font = this.Font;
      this.deleteToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.lblDeleteProfile;
      this.renameToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRenameProfile;
      this.CenterToScreen();
      this.btnClose.BackColor = SystemColors.ButtonFace;
      this.btnClose.ForeColor = Color.Black;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.dgProfiles.CellValidated += new DataGridViewCellEventHandler(this.dgProfiles_CellValidated);
      this.dgProfiles.CurrentCellDirtyStateChanged += new EventHandler(this.dgProfiles_CurrentCellDirtyStateChanged);
      this.dgProfiles.CellValueChanged += new DataGridViewCellEventHandler(this.dgProfiles_CellValueChanged);
      this.dgProfiles.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgProfiles_EditingControlShowing);
      this.dgProfiles.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgProfiles_CellMouseDown);
      this.m_newPSN_ID = psnid;
    }

    private void dgProfiles_EditingControlShowing(
      object sender,
      DataGridViewEditingControlShowingEventArgs e)
    {
      e.Control.KeyPress -= new KeyPressEventHandler(this.Control_KeyPress);
      if (this.dgProfiles.CurrentCell.ColumnIndex != 0)
        return;
      e.Control.KeyPress += new KeyPressEventHandler(this.Control_KeyPress);
    }

    private void Control_KeyPress(object sender, KeyPressEventArgs e)
    {
      ((TextBoxBase) sender).MaxLength = 32;
      if (((TextBoxBase) sender).TextLength >= 32 && e.KeyChar != '\b' || e.KeyChar == '\u0016')
      {
        e.KeyChar = char.MinValue;
        e.Handled = true;
      }
      else
      {
        if (e.KeyChar != '.' && e.KeyChar != '/' && e.KeyChar != '\\' && e.KeyChar != '%' && e.KeyChar != '[' && e.KeyChar != ']' && e.KeyChar != ':' && e.KeyChar != ';' && e.KeyChar != '|' && e.KeyChar != '=' && e.KeyChar != ',' && e.KeyChar != '?' && e.KeyChar != '<' && e.KeyChar != '>' && e.KeyChar != '&')
          return;
        e.KeyChar = char.MinValue;
        e.Handled = true;
      }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void dgProfiles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Right)
        return;
      this.dgProfiles.ClearSelection();
      this.dgProfiles.Rows[e.RowIndex].Selected = true;
    }

    private void dgProfiles_MouseClick(object sender, MouseEventArgs e)
    {
      int rowIndex = this.dgProfiles.HitTest(e.X, e.Y).RowIndex;
      this.dgProfiles.ClearSelection();
      this.dgProfiles.Rows[rowIndex].Selected = true;
    }

    private void dgProfiles_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      if (e.Row.Index != 0)
        return;
      e.Cancel = true;
    }

    private void dgProfiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void dgProfiles_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void dgProfiles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (this.dgProfiles.IsCurrentCellDirty)
        this.dgProfiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
      if (this.dgProfiles.CurrentCell.ColumnIndex == 2)
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgProfiles.Rows)
          row.Cells[2].Value = (object) false;
      }
      if (this.dgProfiles.CurrentCell.ColumnIndex != 2)
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.dgProfiles.Rows)
      {
        if (row.Index == this.dgProfiles.CurrentCell.RowIndex)
          row.Cells[2].Value = (object) true;
      }
    }

    private DateTime TimeStampToDateTime(double unixTimeStamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();

    private void ManageProfiles_Load(object sender, EventArgs e)
    {
      foreach (string key in this.m_registered.Keys)
      {
        Dictionary<string, object> dictionary = this.m_registered[key] as Dictionary<string, object>;
        if (dictionary.ContainsKey("friendly_name") && dictionary.ContainsKey("registration_ts"))
        {
          int index = this.dgProfiles.Rows.Add();
          this.dgProfiles.Rows[index].Cells[0].Value = dictionary["friendly_name"];
          DataGridViewCell cell = this.dgProfiles.Rows[index].Cells[1];
          DateTime dateTime = this.TimeStampToDateTime(Convert.ToDouble(dictionary["registration_ts"]));
          dateTime = dateTime.AddDays(30.0);
          string str = dateTime.ToString("dd/MM/yyyy");
          cell.Value = (object) str;
          this.dgProfiles.Rows[index].Cells[2].Value = (object) key;
          this.dgProfiles.Rows[index].Tag = (object) (bool) (!dictionary.ContainsKey("replaceable") ? 0 : ((bool) dictionary["replaceable"] ? 1 : 0));
          this.dgProfiles.Rows[index].Cells[1].Style.ForeColor = !(bool) this.dgProfiles.Rows[index].Tag ? Color.Red : Color.Green;
        }
      }
      if (string.IsNullOrEmpty(this.m_newPSN_ID))
        return;
      int index1 = this.dgProfiles.Rows.Add();
      this.dgProfiles.Rows[index1].Cells[0].Value = (object) "Enter Name";
      DataGridViewCell cell1 = this.dgProfiles.Rows[index1].Cells[1];
      DateTime dateTime1 = DateTime.Now;
      dateTime1 = dateTime1.AddDays(30.0);
      string str1 = dateTime1.ToString("dd/MM/yyyy");
      cell1.Value = (object) str1;
      this.dgProfiles.Rows[index1].Cells[1].Style.ForeColor = Color.Red;
      this.dgProfiles.Rows[index1].Cells[2].Value = (object) this.m_newPSN_ID;
      this.dgProfiles.CurrentCell = this.dgProfiles.Rows[index1].Cells[0];
      this.dgProfiles.BeginEdit(true);
    }

    private int CheckExistingKey(byte[] key)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgProfiles.Rows)
      {
        if (row.Tag.ToString() == Convert.ToBase64String(key))
          return row.Index;
      }
      for (int index = 0; index < key.Length; ++index)
      {
        if (key[index] > (byte) 0)
          return -2;
      }
      return -1;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.ValidateProfiles())
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errDuplicateProfile);
      }
      else
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgProfiles.Rows)
        {
          string str = (string) row.Cells[2].Value;
          string name = (string) row.Cells[0].Value;
          if (name.Trim().Length == 0 || name.Trim() == "Enter Name")
          {
            this.dgProfiles.CurrentCell = row.Cells[0];
            int num2 = (int) Util.ShowMessage("Please enter valid name for the profile.");
            return;
          }
          if (this.m_registered.ContainsKey(str))
          {
            if ((string) (this.m_registered[str] as Dictionary<string, object>)["friendly_name"] != name)
              this.RenamePSNID(str, name);
          }
          else if (!this.RegisterPSNID(str, name))
          {
            int num3 = (int) Util.ShowMessage("Error occurred while updating PSN ID " + str);
          }
        }
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool RegisterPSNID(string psnId, string name)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      webClientEx.Encoding = Encoding.UTF8;
      string input = Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetAuthBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", (object) Util.GetUserId(), (object) psnId.Trim(), (object) name.Trim()))));
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(input, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      if (!dictionary.ContainsKey("status") || !((string) dictionary["status"] == "OK"))
        return false;
      this.PsnIDResponse = input;
      return true;
    }

    private bool UnregisterPSNID(string psnId)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Encoding = Encoding.UTF8;
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      string input = Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetAuthBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"UNREGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\"}}", (object) Util.GetUserId(), (object) psnId))));
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(input, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      if (!dictionary.ContainsKey("status") || !((string) dictionary["status"] == "OK"))
        return false;
      this.PsnIDResponse = input;
      return true;
    }

    private bool ValidateProfiles()
    {
      for (int index1 = 0; index1 < this.dgProfiles.Rows.Count; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.dgProfiles.Rows.Count; ++index2)
        {
          if (this.dgProfiles.Rows[index1].Cells[0].Value.ToString() == this.dgProfiles.Rows[index2].Cells[0].Value.ToString())
            return false;
        }
      }
      return true;
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgProfiles.SelectedRows.Count != 1)
        return;
      if (this.UnregisterPSNID((string) this.dgProfiles.SelectedRows[0].Cells[2].Value))
      {
        this.dgProfiles.Rows.Remove(this.dgProfiles.SelectedRows[0]);
      }
      else
      {
        int num = (int) Util.ShowMessage("Can not unregister PSN ID");
      }
    }

    private void renameToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgProfiles.SelectedRows.Count != 1)
        return;
      this.dgProfiles.CurrentCell = this.dgProfiles.SelectedRows[0].Cells[0];
      this.dgProfiles.BeginEdit(true);
    }

    private bool RenamePSNID(string psnId, string name)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Encoding = Encoding.UTF8;
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      string input = Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetAuthBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"RENAME_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", (object) Util.GetUserId(), (object) psnId, (object) name))));
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(input, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      this.PsnIDResponse = input;
      return true;
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.dgProfiles.SelectedRows.Count != 1)
        e.Cancel = true;
      else if (!(bool) this.dgProfiles.SelectedRows[0].Tag)
        this.deleteToolStripMenuItem.Enabled = false;
      else
        this.deleteToolStripMenuItem.Enabled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      gridViewCellStyle.Font = Util.GetFontForPlatform(this.Font);
      this.dgProfiles = new DataGridView();
      this.dgProfiles.Font = Util.GetFontForPlatform(this.dgProfiles.Font);
      this.Removable = new DataGridViewTextBoxColumn();
      this._Name = new DataGridViewTextBoxColumn();
      this._Name.MaxInputLength = 32;
      this.ID = new DataGridViewTextBoxColumn();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.contextMenuStrip1.Font = Util.GetFontForPlatform(this.contextMenuStrip1.Font);
      this.deleteToolStripMenuItem = new ToolStripMenuItem();
      this.renameToolStripMenuItem = new ToolStripMenuItem();
      this.btnSave = new Button();
      this.btnClose = new Button();
      this.panel1 = new Panel();
      ((ISupportInitialize) this.dgProfiles).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.dgProfiles.AllowUserToAddRows = false;
      this.dgProfiles.AllowUserToDeleteRows = false;
      gridViewCellStyle.SelectionBackColor = Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.dgProfiles.AlternatingRowsDefaultCellStyle = gridViewCellStyle;
      this.dgProfiles.RowsDefaultCellStyle = gridViewCellStyle;
      this.dgProfiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
      this.dgProfiles.BackgroundColor = Color.FromArgb(175, 175, 175);
      this.dgProfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._Name.MaxInputLength = 32;
      this.dgProfiles.Columns.AddRange((DataGridViewColumn) this._Name, (DataGridViewColumn) this.Removable, (DataGridViewColumn) this.ID);
      this.dgProfiles.ColumnHeadersDefaultCellStyle.Font = Util.GetFontForPlatform(this.dgProfiles.ColumnHeadersDefaultCellStyle.Font);
      this.dgProfiles.ContextMenuStrip = this.contextMenuStrip1;
      this.dgProfiles.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.dgProfiles.Name = "dgProfiles";
      this.dgProfiles.RowHeadersVisible = false;
      this.dgProfiles.RowTemplate.Height = Util.ScaleSize(24);
      this.dgProfiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgProfiles.ShowEditingIcon = false;
      this.dgProfiles.Size = Util.ScaleSize(new Size(356, 202));
      this.dgProfiles.TabIndex = 0;
      this._Name.HeaderText = "Name";
      this._Name.Name = "_Name";
      this._Name.Width = Util.ScaleSize(132);
      this._Name.MaxInputLength = 32;
      this.Removable.HeaderText = "Removable";
      this.Removable.Name = "Removable";
      this.Removable.Width = Util.ScaleSize(95);
      this.Removable.ReadOnly = true;
      this.ID.HeaderText = "PSN ID";
      this.ID.Name = "ID";
      this.ID.Width = Util.ScaleSize(126);
      this.ID.ReadOnly = true;
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.deleteToolStripMenuItem,
        (ToolStripItem) this.renameToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = Util.ScaleSize(new Size(118, 48));
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = Util.ScaleSize(new Size(117, 22));
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
      this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
      this.renameToolStripMenuItem.Size = Util.ScaleSize(new Size(117, 22));
      this.renameToolStripMenuItem.Text = "Rename";
      this.renameToolStripMenuItem.Click += new EventHandler(this.renameToolStripMenuItem_Click);
      this.btnSave.ForeColor = Color.Black;
      this.btnSave.BackColor = SystemColors.ButtonFace;
      this.btnSave.Location = new Point(Util.ScaleSize(118), Util.ScaleSize(217));
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = Util.ScaleSize(new Size(75, 23));
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = PS3SaveEditor.Resources.Resources.btnApply;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.ForeColor = Color.White;
      this.btnClose.Location = new Point(Util.ScaleSize(197), Util.ScaleSize(217));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(75, 23));
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.dgProfiles);
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(380, 244));
      this.panel1.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = Util.ScaleSize(new Size(400, 264));
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ManageProfiles);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Manage Profiles";
      this.Load += new EventHandler(this.ManageProfiles_Load);
      ((ISupportInitialize) this.dgProfiles).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
