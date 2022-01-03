// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Search
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class Search : Form
  {
    private AdvancedEdit m_editForm;
    private IContainer components = (IContainer) null;
    private Label lblEnterLoc;
    private TextBox txtSearch;
    private Button btnOk;
    private Button btnCancel;
    private Button btnFindPrev;
    private Button btnFind;
    private ComboBox cbSearchType;

    public TextBox SearchText => this.txtSearch;

    public bool TextMode
    {
      set
      {
        if (value)
        {
          this.cbSearchType.SelectedIndex = 1;
          this.cbSearchType.Enabled = false;
        }
        else
        {
          this.cbSearchType.Enabled = true;
          this.cbSearchType.SelectedIndex = 0;
        }
      }
    }

    public Search(AdvancedEdit editForm)
    {
      this.m_editForm = editForm;
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.btnFindPrev.Click += new EventHandler(this.btnFindPrev_Click);
      this.CenterToScreen();
      this.btnOk.Enabled = false;
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.btnFindPrev.Text = PS3SaveEditor.Resources.Resources.btnFindPrev;
      this.btnFind.Text = PS3SaveEditor.Resources.Resources.btnFind;
      this.cbSearchType.SelectedIndex = 0;
      this.FormClosed += new FormClosedEventHandler(this.Search_FormClosed);
      this.FormClosing += new FormClosingEventHandler(this.Search_FormClosing);
    }

    private void Search_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = true;
      this.Hide();
    }

    private void Search_FormClosed(object sender, FormClosedEventArgs e)
    {
    }

    private void btnFindPrev_Click(object sender, EventArgs e) => this.m_editForm.Search(true, false, this.GetSearchMode());

    public SearchMode GetSearchMode()
    {
      switch (this.cbSearchType.SelectedIndex)
      {
        case 0:
          return SearchMode.Hex;
        case 1:
          return SearchMode.Text;
        case 2:
          return SearchMode.Decimal;
        case 3:
          return SearchMode.Float;
        default:
          return SearchMode.Hex;
      }
    }

    private void btnFind_Click(object sender, EventArgs e) => this.m_editForm.Search(false, false, this.GetSearchMode());

    private void btnOk_Click(object sender, EventArgs e) => this.Hide();

    private void btnCancel_Click(object sender, EventArgs e) => this.Hide();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (this.m_editForm.TextMode)
        return;
      if (this.GetSearchMode() == SearchMode.Decimal)
      {
        try
        {
          int.Parse(this.txtSearch.Text);
        }
        catch (OverflowException ex)
        {
          this.txtSearch.Text = this.txtSearch.Text.Substring(0, this.txtSearch.Text.Length - 1);
          this.txtSearch.SelectionStart = this.txtSearch.Text.Length;
        }
        catch (Exception ex)
        {
        }
      }
      if (this.txtSearch.Text.Length <= 0)
        return;
      this.btnFind.Enabled = true;
      this.btnFindPrev.Enabled = true;
    }

    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        this.m_editForm.Search(false, true, this.GetSearchMode());
      if (this.m_editForm.TextMode || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.Control || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 && !e.Shift || this.txtSearch.SelectionStart == 1 && e.KeyCode == Keys.X && this.txtSearch.Text[0] == '0' || this.GetSearchMode() == SearchMode.Hex && e.KeyCode >= Keys.A && e.KeyCode <= Keys.F || this.GetSearchMode() == SearchMode.Text || this.GetSearchMode() == SearchMode.Float && e.KeyCode == Keys.Decimal)
        return;
      e.SuppressKeyPress = true;
    }

    private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblEnterLoc = new Label();
      this.txtSearch = new TextBox();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.btnFindPrev = new Button();
      this.btnFind = new Button();
      this.cbSearchType = new ComboBox();
      this.SuspendLayout();
      this.lblEnterLoc.AutoSize = true;
      this.lblEnterLoc.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(11));
      this.lblEnterLoc.Name = "lblEnterLoc";
      this.lblEnterLoc.Size = Util.ScaleSize(new Size(56, 13));
      this.lblEnterLoc.TabIndex = 0;
      this.lblEnterLoc.Text = "Find what:";
      this.txtSearch.Location = new Point(Util.ScaleSize(70), Util.ScaleSize(9));
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = Util.ScaleSize(new Size(97, 20));
      this.txtSearch.TabIndex = 1;
      this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
      this.txtSearch.KeyDown += new KeyEventHandler(this.txtSearch_KeyDown);
      this.txtSearch.KeyPress += new KeyPressEventHandler(this.txtSearch_KeyPress);
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(Util.ScaleSize(121), Util.ScaleSize(35));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 23));
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Visible = false;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(Util.ScaleSize(193), Util.ScaleSize(35));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnFindPrev.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnFindPrev.BackColor = SystemColors.Control;
      this.btnFindPrev.ForeColor = SystemColors.ControlText;
      this.btnFindPrev.Location = new Point(Util.ScaleSize(86), Util.ScaleSize(35));
      this.btnFindPrev.Name = "btnFindPrev";
      this.btnFindPrev.Size = Util.ScaleSize(new Size(81, 23));
      this.btnFindPrev.TabIndex = 23;
      this.btnFindPrev.Text = "Find Previous";
      this.btnFindPrev.UseVisualStyleBackColor = false;
      this.btnFind.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnFind.BackColor = SystemColors.Control;
      this.btnFind.ForeColor = Color.Black;
      this.btnFind.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(35));
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = Util.ScaleSize(new Size(63, 23));
      this.btnFind.TabIndex = 22;
      this.btnFind.Text = "Find";
      this.btnFind.UseVisualStyleBackColor = false;
      this.cbSearchType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSearchType.FormattingEnabled = true;
      this.cbSearchType.Items.AddRange(new object[4]
      {
        (object) "Hex",
        (object) "Text",
        (object) "Decimal",
        (object) "Float"
      });
      this.cbSearchType.Location = new Point(Util.ScaleSize(184), Util.ScaleSize(10));
      this.cbSearchType.Name = "cbSearchType";
      this.cbSearchType.Size = Util.ScaleSize(new Size(84, 21));
      this.cbSearchType.TabIndex = 24;
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = Util.ScaleSize(new Size(280, 69));
      this.Controls.Add((Control) this.cbSearchType);
      this.Controls.Add((Control) this.btnFindPrev);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtSearch);
      this.Controls.Add((Control) this.lblEnterLoc);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Search);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Find";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
