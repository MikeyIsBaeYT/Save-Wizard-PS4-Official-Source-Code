// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.AdvancedEdit2
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PS3SaveEditor
{
  public class AdvancedEdit2 : Form
  {
    private DynamicByteProvider provider_left;
    private DynamicByteProvider provider_right;
    private game m_game;
    private bool m_bTextMode;
    private Dictionary<string, byte[]> m_saveFilesDataLeft;
    private Dictionary<string, byte[]> m_saveFilesDataRight;
    private const int MAX_CHEAT_CODES = 128;
    private List<string> m_DirtyFilesLeft;
    private List<string> m_DirtyFilesRight;
    private string m_cursaveFile;
    private bool _resizeInProgress = false;
    private HexBox activeHexBox;
    private RichTextBox activeTextBox;
    private int _lastTsLeft = 0;
    private DiffResults diffResults = new DiffResults();
    private IContainer components = (IContainer) null;
    private Label lblGameName;
    private Label label2;
    private TextBox txtSearchValue;
    private Button btnStackSearch;
    private Button btnFind;
    private Button btnFindPrev;
    private ComboBox cbSearchMode;
    private ListBox lstSearchVal;
    private Label lblAddress;
    private TextBox txtAddress;
    private Button btnStackAddress;
    private Button btnGo;
    private ListBox lstSearchAddresses;
    private CheckBox chkEnableRight;
    private CheckBox chkSyncScroll;
    private Label label4;
    private ListBox lstCache;
    private Button btnPush;
    private Button btnPop;
    private Label lblCheats;
    private CheckedListBox listViewCheats;
    private Label lblCheatCodes;
    private ListBox lstCheatCodes;
    private ComboBox cbSaveFiles;
    private Button btnApply;
    private Button btnClose;
    private Panel panel1;
    private Label lblOffsetValue;
    private Label lblOffset;
    private Button btnCompare;
    private HexBox hexBoxLeft;
    private Panel panelRight;
    private HexBox hexBoxRight;
    private RichTextBox txtSaveDataLeft;
    private RichTextBox txtSaveDataRight;
    private Panel panelLeft;
    private CustomTableLayoutPanel tableMain;
    private CustomTableLayoutPanel tableTop;
    private CustomTableLayoutPanel tableRight;
    private CustomTableLayoutPanel tableLayoutMiddle;

    public AdvancedEdit2(game game, Dictionary<string, byte[]> data)
    {
      this.InitializeComponent();
      this.CenterToScreen();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.btnApply.BackColor = SystemColors.ButtonFace;
      this.btnApply.ForeColor = Color.Black;
      this.btnClose.BackColor = SystemColors.ButtonFace;
      this.btnClose.ForeColor = Color.Black;
      this.m_DirtyFilesLeft = new List<string>();
      this.m_DirtyFilesRight = new List<string>();
      this.m_saveFilesDataLeft = data;
      this.m_saveFilesDataRight = new Dictionary<string, byte[]>();
      this.tableMain.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.tableTop.BackColor = Color.Transparent;
      this.tableRight.BackColor = Color.Transparent;
      this.lblGameName.BackColor = Color.Transparent;
      this.panel1.BackColor = Color.Transparent;
      this.DoubleBuffered = true;
      this.lblOffset.BackColor = Color.Transparent;
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.SetLabels();
      this.lblGameName.Text = game.name;
      this.m_game = game;
      this.FillCache();
      this.btnApply.Enabled = false;
      this.m_bTextMode = false;
      this.cbSearchMode.SelectedIndex = 0;
      this.txtSaveDataLeft.TextChanged += new EventHandler(this.txtSaveData_TextChanged);
      this.txtSaveDataRight.TextChanged += new EventHandler(this.txtSaveData_TextChanged);
      this.btnCompare.Click += new EventHandler(this.btnCompare_Click);
      this.cbSaveFiles.SelectedIndexChanged += new EventHandler(this.cbSaveFiles_SelectedIndexChanged);
      foreach (object key in data.Keys)
        this.cbSaveFiles.Items.Add(key);
      if (this.cbSaveFiles.Items.Count > 0)
        this.cbSaveFiles.SelectedIndex = 0;
      if (this.cbSaveFiles.Items.Count == 1)
        this.cbSaveFiles.Enabled = false;
      this.FillCheats();
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnPush.Click += new EventHandler(this.btnPush_Click);
      this.btnPop.Click += new EventHandler(this.btnPop_Click);
      this.hexBoxLeft.SelectionBackColor = Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.hexBoxLeft.ShadowSelectionColor = Color.FromArgb(204, 240, (int) byte.MaxValue);
      this.hexBoxRight.SelectionBackColor = Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.hexBoxRight.ShadowSelectionColor = Color.FromArgb(204, 240, (int) byte.MaxValue);
      this.hexBoxLeft.GotFocus += new EventHandler(this.hexBox1_GotFocus);
      this.hexBoxRight.GotFocus += new EventHandler(this.hexBox2_GotFocus);
      this.txtSaveDataLeft.GotFocus += new EventHandler(this.txtSaveDataLeft_GotFocus);
      this.txtSaveDataRight.GotFocus += new EventHandler(this.txtSaveDataRight_GotFocus);
      this.hexBoxLeft.Focus();
      this.lstCache.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstCache.DrawItem += new DrawItemEventHandler(this.lstCache_DrawItem);
      this.lstCheatCodes.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstCheatCodes.DrawItem += new DrawItemEventHandler(this.lstCheatCodes_DrawItem);
      this.lstSearchVal.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstSearchVal.DrawItem += new DrawItemEventHandler(this.lstSearchVal_DrawItem);
      this.lstSearchAddresses.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstSearchAddresses.DrawItem += new DrawItemEventHandler(this.lstSearchAddresses_DrawItem);
      this.activeHexBox = this.hexBoxLeft;
      this.lstCache.KeyDown += new KeyEventHandler(this.lstCache_KeyDown);
      this.panelLeft.Paint += new PaintEventHandler(this.panelLeft_Paint);
      this.listViewCheats.SelectedIndexChanged += new EventHandler(this.listViewCheats_SelectedIndexChanged);
      this.listViewCheats.ItemCheck += new ItemCheckEventHandler(this.listViewCheats_ItemCheck);
      this.listViewCheats.KeyDown += new KeyEventHandler(this.listViewCheats_KeyDown);
      this.txtSearchValue.KeyDown += new KeyEventHandler(this.txtSearchValue_KeyDown);
      this.btnFindPrev.Click += new EventHandler(this.btnFindPrev_Click);
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.tableLayoutMiddle.CellPaint += new TableLayoutCellPaintEventHandler(this.tableLayoutPanel1_CellPaint);
      this.hexBoxLeft.SelectionStartChanged += new EventHandler(this.hexBox1_SelectionStartChanged);
      this.hexBoxRight.SelectionStartChanged += new EventHandler(this.hexBox2_SelectionStartChanged);
      this.hexBoxLeft.VScroll += new EventHandler(this.hexBox1_Scroll);
      this.hexBoxRight.VScroll += new EventHandler(this.hexBox2_Scroll);
      this.hexBoxLeft.HScroll += new EventHandler(this.hexBoxLeft_HScroll);
      this.hexBoxRight.HScroll += new EventHandler(this.hexBoxRight_HScroll);
      this.diffResults.OnDiffRowSelected += new EventHandler(this.diffResults_OnDiffRowSelected);
      this.lstCheatCodes.SelectedIndexChanged += new EventHandler(this.lstCheatCodes_SelectedIndexChanged);
      this.panelLeft.Visible = false;
      this.panelRight.Visible = false;
      this.ResizeBegin += (EventHandler) ((s, e) => this.SuspendLayout());
      this.ResizeEnd += (EventHandler) ((s, e) => this.ResumeLayout(true));
      this.MinimumSize = Util.ScaleSize(new Size(856, 522));
      this.ResizeRedraw = false;
      this.SizeChanged += (EventHandler) ((s, e) =>
      {
        if (this.WindowState != FormWindowState.Maximized)
          return;
        this._resizeInProgress = false;
        this.tableMain.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
        this.tableLayoutMiddle.BackColor = Color.Transparent;
        this.tableRight.BackColor = Color.Transparent;
        this.tableTop.BackColor = Color.Transparent;
        this.Invalidate(true);
      });
      this.cbSaveFiles.Width = Math.Min(200, this.ComboBoxWidth(this.cbSaveFiles));
      this.btnCompare.Width = this.cbSaveFiles.Width;
      this.panelRight.BackColor = Color.FromArgb(102, 164, 201);
      this.btnStackAddress.Click += new EventHandler(this.btnStackAddress_Click);
      this.btnStackSearch.Click += new EventHandler(this.btnStackSearch_Click);
      this.cbSearchMode.SelectedIndexChanged += new EventHandler(this.cbSearchMode_SelectedIndexChanged);
      this.btnGo.Click += new EventHandler(this.btnGo_Click);
      this.lstSearchAddresses.KeyDown += new KeyEventHandler(this.lstSearchAddresses_KeyDown);
      this.lstSearchAddresses.SelectedIndexChanged += new EventHandler(this.lstSearchAddresses_SelectedIndexChanged);
      this.lstSearchAddresses.KeyDown += new KeyEventHandler(this.lstSearchAddresses_KeyDown);
      this.lstSearchVal.KeyDown += new KeyEventHandler(this.lstSearchVal_KeyDown);
      this.lstSearchVal.MouseClick += new MouseEventHandler(this.lstSearchVal_MouseClick);
      this.lstSearchVal.SelectedIndexChanged += new EventHandler(this.lstSearchVal_SelectedIndexChanged);
      this.hexBoxLeft.KeyDown += new KeyEventHandler(this.hexBox1_KeyDown);
      this.hexBoxRight.KeyDown += new KeyEventHandler(this.hexBox2_KeyDown);
      if (Util.IsUnixOrMacOSX())
      {
        this.lblOffset.Location = new Point(Util.ScaleSize(467), 0);
        this.lblOffsetValue.Location = new Point(Util.ScaleSize(517), 0);
      }
      this.chkEnableRight_CheckedChanged((object) null, (EventArgs) null);
    }

    private void lstSearchAddresses_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        graphics.FillRectangle((Brush) new SolidBrush(Color.FromArgb(0, 175, (int) byte.MaxValue)), e.Bounds);
        e.Graphics.DrawString((string) this.lstSearchAddresses.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstSearchAddresses.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.Black), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    private void lstSearchVal_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        graphics.FillRectangle((Brush) new SolidBrush(Color.FromArgb(0, 175, (int) byte.MaxValue)), e.Bounds);
        e.Graphics.DrawString((string) this.lstSearchVal.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstSearchVal.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.Black), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    private void lstCheatCodes_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        graphics.FillRectangle((Brush) new SolidBrush(Color.FromArgb(72, 187, 97)), e.Bounds);
        e.Graphics.DrawString((string) this.lstCheatCodes.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstCheatCodes.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.Black), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    private void lstCache_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        graphics.FillRectangle((Brush) new SolidBrush(Color.FromArgb(204, 240, (int) byte.MaxValue)), e.Bounds);
        e.Graphics.DrawString((string) this.lstCache.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstCache.Items[e.Index], e.Font, (Brush) new SolidBrush(Color.Black), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    private void txtSaveDataRight_GotFocus(object sender, EventArgs e)
    {
      this.activeHexBox = (HexBox) null;
      this.activeTextBox = this.txtSaveDataRight;
      this.activeTextBox.BorderStyle = BorderStyle.Fixed3D;
      this.txtSaveDataLeft.BorderStyle = BorderStyle.None;
      this.tableLayoutMiddle.Invalidate();
    }

    private void txtSaveDataLeft_GotFocus(object sender, EventArgs e)
    {
      this.activeHexBox = (HexBox) null;
      this.activeTextBox = this.txtSaveDataLeft;
      this.activeTextBox.BorderStyle = BorderStyle.Fixed3D;
      this.txtSaveDataRight.BorderStyle = BorderStyle.None;
      this.tableLayoutMiddle.Invalidate();
    }

    private int ComboBoxWidth(ComboBox myCombo)
    {
      int num = 0;
      foreach (object obj in myCombo.Items)
      {
        int width = TextRenderer.MeasureText(myCombo.GetItemText(obj), myCombo.Font).Width;
        if (width > num)
          num = width;
      }
      return num + SystemInformation.VerticalScrollBarWidth;
    }

    private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
    {
      if (e.Row != 0)
        return;
      int num1 = 88;
      int num2 = 24;
      int num3 = 500;
      if (Util.IsUnixOrMacOSX())
      {
        num1 = 80;
        num2 = 21;
        num3 = 450;
      }
      if (e.Column == 0)
      {
        using (Brush brush1 = (Brush) new SolidBrush(this.panelLeft.BackColor))
        {
          Graphics graphics = e.Graphics;
          Brush brush2 = brush1;
          int width1 = this.hexBoxLeft.Width;
          Rectangle rectangle = e.CellBounds;
          int val2 = Math.Min(rectangle.Width, 610);
          int width2 = Math.Min(width1, val2);
          rectangle = this.panelLeft.ClientRectangle;
          int height = rectangle.Height;
          graphics.FillRectangle(brush2, 10, 0, width2, height);
        }
        using (Brush brush = (Brush) new SolidBrush(this.panelLeft.ForeColor))
        {
          e.Graphics.DrawString("Address", this.panelLeft.Font, brush, (PointF) new Point(-this.hexBoxLeft.HScrollBar.Value + 10, 0));
          for (int index = 0; index < 16; ++index)
            e.Graphics.DrawString(index.ToString("X") + "+", this.panelLeft.Font, brush, (PointF) new Point(-this.hexBoxLeft.HScrollBar.Value + num1 + index * num2, 0));
          e.Graphics.DrawString("ASCII", this.panelLeft.Font, brush, (PointF) new Point(-this.hexBoxLeft.HScrollBar.Value + num3, 0));
        }
      }
      else
      {
        Rectangle rect;
        ref Rectangle local = ref rect;
        Rectangle cellBounds1 = e.CellBounds;
        Point location1 = new Point(cellBounds1.Location.X + 8, 0);
        cellBounds1 = e.CellBounds;
        Size size = new Size(Math.Min(cellBounds1.Width, 610), 20);
        local = new Rectangle(location1, size);
        e.Graphics.Clip = new Region(rect);
        using (Brush brush = (Brush) new SolidBrush(this.panelRight.BackColor))
          e.Graphics.FillRectangle(brush, rect);
        using (Brush brush3 = (Brush) new SolidBrush(this.panelRight.ForeColor))
        {
          Graphics graphics1 = e.Graphics;
          Font font1 = this.panelRight.Font;
          Brush brush4 = brush3;
          int num4 = -this.hexBoxRight.HScrollBar.Value;
          Rectangle cellBounds2 = e.CellBounds;
          Point location2 = cellBounds2.Location;
          int x1 = location2.X;
          PointF point1 = (PointF) new Point(num4 + x1 + 10, 0);
          graphics1.DrawString("Address", font1, brush4, point1);
          for (int index = 0; index < 16; ++index)
          {
            Graphics graphics2 = e.Graphics;
            string s = index.ToString("X") + "+";
            Font font2 = this.panelRight.Font;
            Brush brush5 = brush3;
            int num5 = -this.hexBoxRight.HScrollBar.Value;
            cellBounds2 = e.CellBounds;
            location2 = cellBounds2.Location;
            int x2 = location2.X;
            PointF point2 = (PointF) new Point(num5 + x2 + num1 + index * num2, 0);
            graphics2.DrawString(s, font2, brush5, point2);
          }
          Graphics graphics3 = e.Graphics;
          Font font3 = this.panelRight.Font;
          Brush brush6 = brush3;
          int num6 = -this.hexBoxRight.HScrollBar.Value;
          cellBounds2 = e.CellBounds;
          location2 = cellBounds2.Location;
          int x3 = location2.X;
          PointF point3 = (PointF) new Point(num6 + x3 + num3, 0);
          graphics3.DrawString("ASCII", font3, brush6, point3);
        }
      }
    }

    private void panelLeft_Paint(object sender, PaintEventArgs e) => e.Graphics.TranslateTransform((float) this.hexBoxLeft.HScrollBar.Value, 0.0f);

    private void hexBoxRight_HScroll(object sender, EventArgs e) => this.tableLayoutMiddle.Invalidate();

    private void hexBoxLeft_HScroll(object sender, EventArgs e) => this.tableLayoutMiddle.Invalidate();

    private void listViewCheats_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (e.NewValue != CheckState.Checked)
        return;
      this.btnApply.Enabled = true;
    }

    private void lstCheatCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstCheatCodes.SelectedItems.Count != 1)
        return;
      string[] strArray = ((string) this.lstCheatCodes.SelectedItems[0]).Split(' ');
      try
      {
        long index = long.Parse(strArray[0], NumberStyles.HexNumber) & 268435455L;
        if (index <= this.hexBoxLeft.ByteProvider.Length)
        {
          this.hexBoxLeft.ScrollByteIntoView(index);
          if (this.hexBoxRight.ByteProvider != null)
            this.hexBoxRight.ScrollByteIntoView(index);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void diffResults_OnDiffRowSelected(object sender, EventArgs e)
    {
      DataGridView dataGridView = sender as DataGridView;
      if (dataGridView.SelectedRows.Count != 1 || string.IsNullOrEmpty((string) dataGridView.SelectedRows[0].Cells[0].Value))
        return;
      long index = long.Parse((string) dataGridView.SelectedRows[0].Cells[0].Value, NumberStyles.HexNumber);
      this.hexBoxLeft.ScrollByteIntoView(index);
      if (this.hexBoxRight.ByteProvider != null)
        this.hexBoxRight.ScrollByteIntoView(index);
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 274 && m.WParam == new IntPtr(61488))
      {
        this.tableMain.BackColor = Color.FromArgb(0, 138, 213);
        this.tableLayoutMiddle.BackColor = Color.FromArgb(0, 138, 213);
        this.tableRight.BackColor = Color.FromArgb(0, 138, 213);
        this.tableTop.BackColor = Color.FromArgb(0, 138, 213);
        this._resizeInProgress = true;
      }
      base.WndProc(ref m);
    }

    private void listViewCheats_KeyDown(object sender, KeyEventArgs e)
    {
    }

    private void btnFindPrev_Click(object sender, EventArgs e) => this.Search(true, false);

    private void lstCache_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete || this.lstCache.SelectedItem == null || Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnDeleteCache, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      Directory.Delete(Util.GetCacheFolder(this.m_game, (string) this.lstCache.SelectedItem), true);
      this.lstCache.Items.Remove(this.lstCache.SelectedItem);
    }

    private void cbSearchMode_SelectedIndexChanged(object sender, EventArgs e) => this.txtSearchValue.Text = "";

    private void txtAddress_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnGo_Click((object) null, (EventArgs) null);
    }

    private void listViewCheats_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (!e.Item.Checked)
        return;
      this.btnApply.Enabled = true;
    }

    private void listViewCheats_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewCheats.SelectedItems.Count != 1)
        return;
      string str1 = (this.listViewCheats.Tag as List<string>)[this.listViewCheats.SelectedIndex];
      this.lstCheatCodes.Items.Clear();
      string str2 = str1;
      char[] chArray = new char[1]{ '\n' };
      foreach (string str3 in str2.Split(chArray))
      {
        if (!string.IsNullOrEmpty(str3))
          this.lstCheatCodes.Items.Add((object) str3);
      }
    }

    private void hexBox2_GotFocus(object sender, EventArgs e)
    {
      this.activeTextBox = (RichTextBox) null;
      this.activeHexBox = this.hexBoxRight;
      this.panelRight.BackColor = Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.panelRight.ForeColor = Color.White;
      this.panelLeft.BackColor = Color.FromArgb(102, 164, 201);
      this.panelLeft.ForeColor = Color.Black;
      this.tableLayoutMiddle.Invalidate();
    }

    private void hexBox1_GotFocus(object sender, EventArgs e)
    {
      this.activeTextBox = (RichTextBox) null;
      this.activeHexBox = this.hexBoxLeft;
      this.panelLeft.BackColor = Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.panelLeft.ForeColor = Color.White;
      this.panelRight.BackColor = Color.FromArgb(102, 164, 201);
      this.panelRight.ForeColor = Color.Black;
      this.tableLayoutMiddle.Invalidate();
    }

    private void txtSaveData_TextChanged(object sender, EventArgs e)
    {
      this.btnApply.Enabled = true;
      if (sender as RichTextBox == this.txtSaveDataLeft)
      {
        if (this.m_DirtyFilesLeft.IndexOf(this.m_cursaveFile) >= 0)
          return;
        this.m_DirtyFilesLeft.Add(this.m_cursaveFile);
      }
      else if (this.m_DirtyFilesRight.IndexOf(this.m_cursaveFile) < 0)
        this.m_DirtyFilesRight.Add(this.m_cursaveFile);
    }

    private void SetLabels()
    {
      this.lblOffset.Text = PS3SaveEditor.Resources.Resources.lblOffset;
      this.lblCheatCodes.Text = PS3SaveEditor.Resources.Resources.lblCodes;
      this.lblCheats.Text = PS3SaveEditor.Resources.Resources.lblCheats;
      this.btnApply.Text = PS3SaveEditor.Resources.Resources.btnApplyDownload;
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
      this.Text = PS3SaveEditor.Resources.Resources.titleAdvEdit;
      this.cbSearchMode.Items.Add((object) PS3SaveEditor.Resources.Resources.itmHex);
      this.cbSearchMode.Items.Add((object) PS3SaveEditor.Resources.Resources.itmDec);
      this.btnFind.Text = PS3SaveEditor.Resources.Resources.btnFind;
      this.btnFindPrev.Text = PS3SaveEditor.Resources.Resources.btnFindPrev;
      this.btnPop.Text = PS3SaveEditor.Resources.Resources.btnPop;
      this.btnPush.Text = PS3SaveEditor.Resources.Resources.btnPush;
      this.btnStackAddress.Text = PS3SaveEditor.Resources.Resources.btnStack;
      this.btnStackSearch.Text = PS3SaveEditor.Resources.Resources.btnStack;
      this.btnCompare.Text = PS3SaveEditor.Resources.Resources.btnCompare;
      this.chkSyncScroll.Text = PS3SaveEditor.Resources.Resources.chkSyncScroll;
      this.chkEnableRight.Text = PS3SaveEditor.Resources.Resources.chkEnableRight;
    }

    private void hexBox1_SelectionStartChanged(object sender, EventArgs e) => this.lblOffsetValue.Text = "0x" + string.Format("{0:X}", (object) ((long) this.hexBoxLeft.BytesPerLine * (this.hexBoxLeft.CurrentLine - 1L) + (this.hexBoxLeft.CurrentPositionInLine - 1L))).PadLeft(8, '0');

    protected override void OnClosed(EventArgs e)
    {
      this.hexBoxLeft.Dispose();
      this.hexBoxRight.Dispose();
      base.OnClosed(e);
    }

    private void provider_left_Changed(object sender, EventArgs e)
    {
      this.btnApply.Enabled = true;
      if (this.m_DirtyFilesLeft.IndexOf(this.m_cursaveFile) >= 0)
        return;
      this.m_DirtyFilesLeft.Add(this.m_cursaveFile);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this._resizeInProgress)
        return;
      base.OnPaint(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this._resizeInProgress)
        return;
      Rectangle clientRectangle = this.ClientRectangle;
      int num;
      if (clientRectangle.Width != 0)
      {
        clientRectangle = this.ClientRectangle;
        num = clientRectangle.Height == 0 ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private string ApplyUserCheats()
    {
      string str1 = "";
      file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), (string) this.cbSaveFiles.SelectedItem);
      if (gameFile != null)
      {
        List<string> tag = this.listViewCheats.Tag as List<string>;
        foreach (int checkedIndex in this.listViewCheats.CheckedIndices)
        {
          string str2 = tag[checkedIndex];
          str1 = str1 + "<file><id>" + gameFile.id + "</id><filename>" + gameFile.filename + "</filename><cheats><code>" + str2 + "</code></cheats></file>";
        }
      }
      return str1;
    }

    private void txtSaveDataLeft_TextChanged(object sender, EventArgs e)
    {
      this.btnApply.Enabled = true;
      if (this.m_DirtyFilesLeft.IndexOf(this.m_cursaveFile) >= 0)
        return;
      this.m_DirtyFilesLeft.Add(this.m_cursaveFile);
    }

    private Dictionary<long, int> GetDiffs()
    {
      Dictionary<long, int> dictionary = new Dictionary<long, int>();
      byte[] bytes = (this.hexBoxLeft.ByteProvider as DynamicByteProvider).Bytes.GetBytes();
      byte[] numArray = this.m_saveFilesDataLeft[this.m_cursaveFile];
      for (int index1 = 0; index1 < Math.Min(numArray.Length, bytes.Length); ++index1)
      {
        if ((int) bytes[index1] != (int) numArray[index1])
        {
          dictionary.Add((long) index1, 0);
          long key = (long) index1;
          for (int index2 = index1; index2 < Math.Min(numArray.Length, bytes.Length) && (int) bytes[index1] != (int) numArray[index1]; ++index1)
          {
            dictionary[key] = (int) (byte) (dictionary[key] + 1);
            ++index2;
          }
        }
      }
      return dictionary;
    }

    private string DiffsToCheatCodes(Dictionary<long, int> diffs)
    {
      string str = "";
      byte[] array = (this.hexBoxLeft.ByteProvider as DynamicByteProvider).Bytes.ToArray();
      foreach (long key in diffs.Keys)
      {
        int diff = diffs[key];
        for (int index = 0; index < (int) Math.Ceiling((double) diff / 4.0); ++index)
        {
          long num = key + (long) (index * 4);
          str += string.Format("20{0:6X} {1:8X}\r\n", (object) num, (object) BitConverter.ToInt32(array, (int) num));
        }
      }
      return str;
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnOverwriteAdv, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      this.ApplyUserCheats();
      if (!this.m_bTextMode)
      {
        this.provider_left.ApplyChanges();
        if (this.provider_right != null)
          this.provider_right.ApplyChanges();
        if (this.m_cursaveFile == null)
          this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
        this.m_saveFilesDataLeft[this.m_cursaveFile] = this.provider_left.Bytes.ToArray();
        if (this.provider_right != null && this.m_saveFilesDataRight.ContainsKey(this.m_cursaveFile))
          this.m_saveFilesDataRight[this.m_cursaveFile] = this.provider_right.Bytes.ToArray();
      }
      else
      {
        file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.m_cursaveFile);
        this.m_saveFilesDataLeft[this.m_cursaveFile] = gameFile.TextMode != 1 ? (gameFile.TextMode != 3 ? Encoding.ASCII.GetBytes(this.txtSaveDataLeft.Text) : Encoding.Unicode.GetBytes(this.txtSaveDataLeft.Text)) : Encoding.UTF8.GetBytes(this.txtSaveDataLeft.Text);
      }
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errSaveData, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        this.m_game.GetTargetGameFolder();
        List<string> dirtyFilesLeft = this.m_DirtyFilesLeft;
        List<string> selectedSaveFiles = new List<string>();
        foreach (string path1 in dirtyFilesLeft)
        {
          string path2 = Path.Combine(ZipUtil.GetPs3SeTempFolder(), "_file_" + Path.GetFileName(path1));
          File.WriteAllBytes(path2, this.m_saveFilesDataLeft[Path.GetFileName(path1)]);
          if (selectedSaveFiles.IndexOf(path2) < 0)
            selectedSaveFiles.Add(path2);
        }
        List<string> containerFiles = this.m_game.GetContainerFiles();
        string file = this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4);
        string hash = Util.GetHash(file);
        bool cache = Util.GetCache(hash);
        string contents = this.m_game.ToString(selectedSaveFiles, "encrypt");
        if (cache)
        {
          containerFiles.Remove(file);
          contents = contents.Replace("<pfs><name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name></pfs>", "<pfs><name>" + Path.GetFileNameWithoutExtension(this.m_game.LocalSaveFolder) + "</name><md5>" + hash + "</md5></pfs>");
        }
        selectedSaveFiles.AddRange((IEnumerable<string>) containerFiles);
        string path = Util.GetTempFolder() + "ps4_list.xml";
        File.WriteAllText(path, contents);
        selectedSaveFiles.Add(path);
        if (new AdvancedSaveUploaderForEncrypt(selectedSaveFiles.ToArray(), this.m_game, (string) null, "encrypt").ShowDialog() != DialogResult.OK)
          ;
        File.Delete(path);
        Directory.Delete(ZipUtil.GetPs3SeTempFolder(), true);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnApply_Click2(object sender, EventArgs e)
    {
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnOverwriteAdv, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      this.ApplyUserCheats();
      if (!(Util.GetRegistryValue("BackupSaves") == "true"))
        ;
      this.provider_left.ApplyChanges();
      if (this.provider_right != null)
        this.provider_right.ApplyChanges();
      if (this.m_cursaveFile == null)
        this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
      this.m_saveFilesDataLeft[this.m_cursaveFile] = this.provider_left.Bytes.ToArray();
      if (this.provider_right != null && this.m_saveFilesDataRight.ContainsKey(this.m_cursaveFile))
        this.m_saveFilesDataRight[this.m_cursaveFile] = this.provider_right.Bytes.ToArray();
      string path1 = Path.Combine(Util.GetTempFolder(), "root");
      List<string> stringList = new List<string>();
      foreach (string key in this.m_saveFilesDataLeft.Keys)
      {
        string path = Path.Combine(path1, key);
        stringList.Add(path);
        File.WriteAllBytes(path, this.m_saveFilesDataLeft[key]);
      }
      stringList.Add(Path.Combine(Util.GetTempFolder(), this.m_game.id + ".sav"));
      if (new AdvancedSaveUploaderForEncrypt(stringList.ToArray(), this.m_game, "", "encrypt").ShowDialog() != DialogResult.OK)
        ;
      try
      {
        Directory.Delete(Util.GetTempFolder(), true);
      }
      catch (Exception ex)
      {
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void txtSearchValue_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void hexBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3)
        return;
      this.Search(e.Shift, false);
    }

    private void hexBox2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3)
        return;
      this.Search(e.Shift, false);
    }

    private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        this.Search(false, false);
        this.txtSearchValue.Focus();
        e.SuppressKeyPress = true;
      }
      else
      {
        if (this.m_bTextMode || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.Control || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 && !e.Shift || this.cbSearchMode.SelectedIndex == 0 && e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)
          return;
        e.SuppressKeyPress = true;
      }
    }

    private void Search(bool bBackward, bool bStart)
    {
      if (this.m_bTextMode)
      {
        this.SerachText(bBackward, bStart);
      }
      else
      {
        MemoryStream memoryStream = new MemoryStream((this.activeHexBox.ByteProvider as DynamicByteProvider).Bytes.GetBytes());
        BinaryReader reader = new BinaryReader((Stream) memoryStream);
        if (bStart)
        {
          reader.BaseStream.Position = 0L;
          this.activeHexBox.SelectionStart = 0L;
          this.activeHexBox.SelectionLength = 0L;
        }
        else if (this.activeHexBox.SelectionStart >= 0L)
          reader.BaseStream.Position = this.activeHexBox.SelectionStart + this.activeHexBox.SelectionLength;
        long position = reader.BaseStream.Position;
        uint val1;
        uint val2;
        int searchValues = this.GetSearchValues(out val1, out val2);
        if (searchValues == 0)
        {
          int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidHex, PS3SaveEditor.Resources.Resources.msgError);
        }
        else if (searchValues < 0)
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errIncorrectValue, PS3SaveEditor.Resources.Resources.msgError);
        }
        else
        {
          for (; reader.BaseStream.Position >= 0L && reader.BaseStream.Position < reader.BaseStream.Length + (bBackward ? (long) searchValues : (long) (1 - searchValues)); reader.BaseStream.Position = position)
          {
            uint num3 = this.ReadValue(reader, searchValues, bBackward);
            if ((int) num3 == (int) val1 || (int) num3 == (int) val2)
            {
              this.activeHexBox.Select(reader.BaseStream.Position - (long) searchValues, (long) searchValues);
              this.activeHexBox.ScrollByteIntoView(reader.BaseStream.Position);
              this.activeHexBox.Focus();
              break;
            }
            if (bBackward)
            {
              --position;
              if (position < 0L)
                break;
            }
            else
            {
              ++position;
              if (position > reader.BaseStream.Length)
                break;
            }
          }
          reader.Close();
          memoryStream.Close();
          memoryStream.Dispose();
        }
      }
    }

    public int FindMyText(string text, int start, bool bReverse)
    {
      int num1 = -1;
      if (text.Length > 0 && start >= 0)
      {
        RichTextBoxFinds options = RichTextBoxFinds.None;
        int end = this.activeTextBox.Text.Length;
        if (bReverse)
        {
          options |= RichTextBoxFinds.Reverse;
          end = start - text.Length;
          if (end < 0)
            end = this.activeTextBox.Text.Length;
          start = 0;
        }
        int num2 = this.activeTextBox.Find(text, start, end, options);
        if (num2 >= 0)
          num1 = num2;
      }
      return num1;
    }

    private void SerachText(bool bBackward, bool bStart)
    {
      if (this.activeTextBox == null)
        this.activeTextBox = this.txtSaveDataLeft;
      int start = 0;
      if (!bStart)
        start = this.activeTextBox.SelectionStart + this.activeTextBox.SelectionLength;
      int myText = this.FindMyText(this.txtSearchValue.Text, start, bBackward);
      if (myText < 0)
      {
        this.activeTextBox.Select(0, 0);
      }
      else
      {
        this.activeTextBox.Focus();
        this.activeTextBox.Select(myText, this.txtSearchValue.Text.Length);
      }
    }

    private uint ReadValue(BinaryReader reader, int size, bool bBackward)
    {
      if (bBackward)
      {
        if (reader.BaseStream.Position < (long) (2 * size))
          reader.BaseStream.Position = reader.BaseStream.Length - 1L;
        reader.BaseStream.Position -= (long) (2 * size);
      }
      switch (size)
      {
        case 1:
          return (uint) reader.ReadByte();
        case 2:
          return (uint) reader.ReadUInt16();
        case 3:
          return (uint) reader.ReadUInt16() << 8 | (uint) reader.ReadByte();
        default:
          return reader.ReadUInt32();
      }
    }

    private int GetSearchValues(out uint val1, out uint val2)
    {
      uint num1;
      int length;
      try
      {
        string text = this.txtSearchValue.Text;
        if (this.cbSearchMode.SelectedIndex == 0)
        {
          num1 = uint.Parse(text, NumberStyles.HexNumber);
          length = text.Length;
          int num2;
          switch (length)
          {
            case 1:
            case 2:
            case 4:
            case 6:
              num2 = 0;
              break;
            default:
              num2 = length != 8 ? 1 : 0;
              break;
          }
          if (num2 != 0)
          {
            val1 = val2 = 0U;
            return 0;
          }
        }
        else
        {
          num1 = uint.Parse(text);
          length = num1.ToString("X").Length;
        }
      }
      catch (Exception ex)
      {
        val1 = 0U;
        val2 = 0U;
        return -1;
      }
      int num3;
      switch (length)
      {
        case 1:
        case 2:
          num3 = 1;
          break;
        case 3:
        case 4:
          num3 = 2;
          break;
        case 5:
        case 6:
          num3 = 3;
          break;
        case 7:
        case 8:
          num3 = 4;
          break;
        default:
          num3 = 4;
          break;
      }
      val1 = num1;
      switch (num3)
      {
        case 2:
          val2 = (uint) (((int) num1 & (int) byte.MaxValue) << 8) | (num1 & 65280U) >> 8;
          break;
        case 3:
          val2 = (uint) (((int) num1 & 65280) << 8 | (int) ((num1 & 16711680U) >> 8) | (int) num1 & (int) byte.MaxValue);
          break;
        case 4:
          val2 = (uint) (((int) num1 & (int) byte.MaxValue) << 24 | ((int) num1 & 65280) << 8) | (num1 & 16711680U) >> 8 | (num1 & 4278190080U) >> 24;
          break;
        default:
          val2 = num1;
          break;
      }
      return num3;
    }

    private void btnFind_Click(object sender, EventArgs e) => this.Search(false, false);

    private void button1_Click(object sender, EventArgs e) => this.Search(true, false);

    private void AdvancedEdit_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.m_bTextMode || e.KeyCode != Keys.G || e.Modifiers != Keys.Control)
        return;
      PS3SaveEditor.Goto @goto = new PS3SaveEditor.Goto(this.provider_left.Length);
      if (@goto.ShowDialog() == DialogResult.OK)
      {
        if (@goto.AddressLocation < this.provider_left.Length)
        {
          this.activeHexBox.ScrollByteIntoView(@goto.AddressLocation);
          this.activeHexBox.Select(@goto.AddressLocation, 1L);
          this.activeHexBox.Invalidate();
        }
        else
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidAddress);
        }
      }
    }

    private void cbSaveFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.m_bTextMode && this.provider_left != null && this.provider_left.Length > 0L)
        this.provider_left.ApplyChanges();
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (!string.IsNullOrEmpty(this.m_cursaveFile) && this.m_saveFilesDataLeft.ContainsKey(this.m_cursaveFile))
      {
        file gameFile = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
        this.m_saveFilesDataLeft[this.m_cursaveFile] = gameFile.TextMode != 0 || this.provider_left == null ? (gameFile.TextMode != 2 ? (gameFile.TextMode != 3 ? Encoding.UTF8.GetBytes(this.txtSaveDataLeft.Text) : Encoding.Unicode.GetBytes(this.txtSaveDataLeft.Text)) : Encoding.ASCII.GetBytes(this.txtSaveDataLeft.Text)) : this.provider_left.Bytes.ToArray();
        if (this.provider_right != null && this.m_saveFilesDataRight.ContainsKey(this.m_cursaveFile))
          this.m_saveFilesDataRight[this.m_cursaveFile] = this.provider_right.Bytes.ToArray();
      }
      this.FillCheats();
      this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
      this.m_bTextMode = false;
      file gameFile1 = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
      if (gameFile1.TextMode == 0)
      {
        this.hexBoxLeft.Visible = true;
        this.hexBoxRight.Visible = true;
        this.txtSaveDataLeft.Visible = false;
        this.txtSaveDataRight.Visible = false;
        this.RefreshHexBoxes();
        this.tableLayoutMiddle.RowStyles[0].Height = 20f;
        this.lblOffset.Visible = true;
        this.lblOffsetValue.Visible = true;
        this.lstCheatCodes.Enabled = true;
        this.lstSearchAddresses.Enabled = true;
        this.lstSearchVal.Enabled = true;
        this.cbSearchMode.Enabled = true;
        this.btnCompare.Enabled = true;
        this.txtAddress.Enabled = true;
        this.btnStackAddress.Enabled = true;
        this.btnStackSearch.Enabled = true;
        this.txtAddress.Enabled = true;
        this.btnGo.Enabled = true;
      }
      else
      {
        this.tableLayoutMiddle.RowStyles[0].Height = 0.0f;
        this.txtSaveDataLeft.Visible = true;
        this.txtSaveDataRight.Visible = true;
        this.hexBoxLeft.Visible = false;
        this.hexBoxRight.Visible = false;
        this.lstCheatCodes.Enabled = false;
        this.lstSearchAddresses.Enabled = false;
        this.lstSearchVal.Enabled = false;
        this.cbSearchMode.Enabled = false;
        this.btnCompare.Enabled = false;
        this.txtAddress.Enabled = false;
        this.btnStackSearch.Enabled = false;
        this.btnStackAddress.Enabled = false;
        this.txtAddress.Enabled = false;
        this.btnGo.Enabled = false;
        if (gameFile1.TextMode == 1)
          this.txtSaveDataLeft.Text = Encoding.UTF8.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
        else if (gameFile1.TextMode == 3)
          this.txtSaveDataLeft.Text = Encoding.Unicode.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
        else
          this.txtSaveDataLeft.Text = Encoding.ASCII.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
        this.m_bTextMode = true;
        this.lblOffset.Visible = false;
        this.lblOffsetValue.Visible = false;
      }
    }

    private void hexBox2_SelectionStartChanged(object sender, EventArgs e) => this.lblOffsetValue.Text = "0x" + string.Format("{0:X}", (object) ((long) this.hexBoxRight.BytesPerLine * (this.hexBoxRight.CurrentLine - 1L) + (this.hexBoxRight.CurrentPositionInLine - 1L))).PadLeft(8, '0');

    private void hexBox2_Scroll(object sender, EventArgs e)
    {
      if (!this.chkSyncScroll.Checked)
        return;
      this.hexBoxLeft.PerformScrollToLine((long) this.hexBoxRight.VScrollBar.Value);
    }

    private void hexBox1_Scroll(object sender, EventArgs e)
    {
      if (!this.chkSyncScroll.Checked)
        return;
      this.hexBoxRight.PerformScrollToLine((long) this.hexBoxLeft.VScrollBar.Value);
      this._lastTsLeft = Environment.TickCount;
    }

    private void btnPush_Click(object sender, EventArgs e)
    {
      string curCacheFolder = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
      string cacheFolder = Util.GetCacheFolder(this.m_game, curCacheFolder);
      Directory.CreateDirectory(cacheFolder);
      foreach (string key in this.m_saveFilesDataLeft.Keys)
        File.WriteAllBytes(Path.Combine(cacheFolder, key), this.m_saveFilesDataLeft[key]);
      this.lstCache.Items.Add((object) curCacheFolder);
    }

    private void FillCheats()
    {
      this.listViewCheats.Tag = (object) new List<string>();
      this.listViewCheats.Items.Clear();
      this.lstCheatCodes.Items.Clear();
      foreach (file file in this.m_game.GetTargetGameFolder().files._files)
      {
        if (Util.IsMatch((string) this.cbSaveFiles.SelectedItem, file.filename))
        {
          foreach (cheat allCheat in file.GetAllCheats())
          {
            if (!(allCheat.id != "-1"))
            {
              ListViewItem listViewItem = new ListViewItem(allCheat.name);
              this.listViewCheats.Items.Add((object) allCheat.name);
              (this.listViewCheats.Tag as List<string>).Add(allCheat.ToEditableString());
            }
          }
        }
      }
    }

    private void FillCache()
    {
      string cacheFolder = Util.GetCacheFolder(this.m_game, (string) null);
      if (!Directory.Exists(cacheFolder))
        return;
      foreach (string directory in Directory.GetDirectories(cacheFolder))
        this.lstCache.Items.Add((object) Path.GetFileName(directory));
    }

    private void btnPop_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty((string) this.lstCache.SelectedItem))
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgChooseCache);
      }
      else
      {
        string selectedItem = (string) this.lstCache.SelectedItem;
        string cacheFolder = Util.GetCacheFolder(this.m_game, (string) this.lstCache.SelectedItem);
        container targetGameFolder = this.m_game.GetTargetGameFolder();
        this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
        file gameFile = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
        this.LoadCache(cacheFolder);
        if (gameFile.TextMode == 0)
          this.RefreshHexBoxes();
        else
          this.RefreshTextBoxes(gameFile.TextMode);
        this.cbSaveFiles_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void RefreshTextBoxes(int textMode)
    {
      string selectedItem = (string) this.cbSaveFiles.SelectedItem;
      if (this.m_saveFilesDataLeft.ContainsKey(selectedItem))
      {
        if (textMode == 1)
          this.txtSaveDataLeft.Text = Encoding.UTF8.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
        if (textMode == 3)
          this.txtSaveDataLeft.Text = Encoding.Unicode.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
        else
          this.txtSaveDataLeft.Text = Encoding.ASCII.GetString(this.m_saveFilesDataLeft[this.m_cursaveFile]);
      }
      if (!this.m_saveFilesDataRight.ContainsKey(selectedItem))
        return;
      if (textMode == 1)
        this.txtSaveDataRight.Text = Encoding.UTF8.GetString(this.m_saveFilesDataRight[this.m_cursaveFile]);
      if (textMode == 3)
        this.txtSaveDataRight.Text = Encoding.Unicode.GetString(this.m_saveFilesDataRight[this.m_cursaveFile]);
      else
        this.txtSaveDataRight.Text = Encoding.ASCII.GetString(this.m_saveFilesDataRight[this.m_cursaveFile]);
    }

    private void RefreshHexBoxes()
    {
      string selectedItem = (string) this.cbSaveFiles.SelectedItem;
      if (this.m_saveFilesDataLeft.ContainsKey(selectedItem))
      {
        this.provider_left = new DynamicByteProvider(this.m_saveFilesDataLeft[selectedItem]);
        this.provider_left.Changed += new EventHandler<ByteProviderChanged>(this.provider_left_Changed);
        this.hexBoxLeft.ByteProvider = (IByteProvider) this.provider_left;
      }
      if (this.m_saveFilesDataRight.ContainsKey(selectedItem))
      {
        this.provider_right = new DynamicByteProvider(this.m_saveFilesDataRight[selectedItem]);
        this.provider_right.Changed += new EventHandler<ByteProviderChanged>(this.provider_right_Changed);
        this.hexBoxRight.ByteProvider = (IByteProvider) this.provider_right;
      }
      this.hexBoxLeft.HexCasing = HexCasing.Upper;
      this.hexBoxRight.HexCasing = HexCasing.Upper;
      this.hexBoxLeft.LineInfoVisible = true;
      this.hexBoxRight.LineInfoVisible = true;
      this.hexBoxLeft.StringViewVisible = true;
      this.hexBoxRight.StringViewVisible = true;
      this.hexBoxLeft.BytesPerLine = 16;
      this.hexBoxRight.BytesPerLine = 16;
      this.hexBoxLeft.UseFixedBytesPerLine = true;
      this.hexBoxRight.UseFixedBytesPerLine = true;
      this.hexBoxLeft.VScrollBarVisible = true;
      this.hexBoxRight.VScrollBarVisible = true;
      this.hexBoxLeft.HScrollBarVisible = true;
      this.hexBoxRight.HScrollBarVisible = true;
    }

    private void provider_right_Changed(object sender, EventArgs e)
    {
      this.btnApply.Enabled = true;
      if (this.m_DirtyFilesRight.IndexOf(this.m_cursaveFile) >= 0)
        return;
      this.m_DirtyFilesRight.Add(this.m_cursaveFile);
    }

    private void LoadCache(string folder)
    {
      string[] files = Directory.GetFiles(folder);
      if (this.activeHexBox == this.hexBoxLeft || this.activeTextBox == this.txtSaveDataLeft)
      {
        this.m_saveFilesDataLeft.Clear();
        foreach (string path in files)
          this.m_saveFilesDataLeft.Add(path.Replace(folder + Path.DirectorySeparatorChar.ToString(), ""), File.ReadAllBytes(path));
        this.hexBoxLeft.Tag = (object) folder;
      }
      else
      {
        this.m_saveFilesDataRight.Clear();
        foreach (string path in files)
          this.m_saveFilesDataRight.Add(path.Replace(folder + Path.DirectorySeparatorChar.ToString(), ""), File.ReadAllBytes(path));
        this.hexBoxRight.Tag = (object) folder;
      }
    }

    private void btnStackSearch_Click(object sender, EventArgs e)
    {
      if (this.cbSearchMode.SelectedIndex == 0)
        this.lstSearchVal.Items.Add((object) ("0x" + this.txtSearchValue.Text));
      else
        this.lstSearchVal.Items.Add((object) string.Concat((object) long.Parse(this.txtSearchValue.Text)));
    }

    private void btnStackAddress_Click(object sender, EventArgs e)
    {
      try
      {
        this.lstSearchAddresses.Items.Add((object) long.Parse(this.txtAddress.Text, NumberStyles.HexNumber).ToString("X"));
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage("Please enter valid hexadecimal");
      }
    }

    private void lstSearchVal_MouseClick(object sender, MouseEventArgs e)
    {
    }

    private void lstSearchVal_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstSearchVal.SelectedItem == null)
        return;
      string selectedItem = (string) this.lstSearchVal.SelectedItem;
      if (selectedItem.StartsWith("0x"))
      {
        this.cbSearchMode.SelectedIndex = 0;
        this.txtSearchValue.Text = selectedItem.Substring(2);
      }
      else
        this.txtSearchValue.Text = this.lstSearchVal.SelectedItem as string;
      this.Search(false, this.activeHexBox.SelectionLength == 0L);
    }

    private void lstSearchAddresses_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstSearchAddresses.SelectedItem == null)
        return;
      long address = long.Parse((string) this.lstSearchAddresses.SelectedItem, NumberStyles.HexNumber);
      this.txtAddress.Text = this.lstSearchAddresses.SelectedItem as string;
      this.Goto(address);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      ListViewItem listViewItem = new ListViewItem("");
      this.listViewCheats.Items.Add((object) listViewItem);
      listViewItem.BeginEdit();
      listViewItem.Selected = true;
    }

    private void btnApplyCodes_Click(object sender, EventArgs e)
    {
      if (this.listViewCheats.SelectedItems.Count != 1)
        ;
    }

    private void btnFind_Click_1(object sender, EventArgs e) => this.Search(false, false);

    private void Goto(long address)
    {
      if (address >= this.provider_left.Length)
        return;
      this.activeHexBox.ScrollByteIntoView(address);
      this.activeHexBox.Select(address, 1L);
      this.activeHexBox.Invalidate();
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      try
      {
        this.Goto(long.Parse(this.txtAddress.Text, NumberStyles.HexNumber));
      }
      catch
      {
        int num = (int) Util.ShowMessage("Please enter valid address.");
      }
    }

    private void lstSearchVal_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete || this.lstSearchVal.SelectedItems.Count != 1)
        return;
      this.lstSearchVal.Items.Remove(this.lstSearchVal.SelectedItems[0]);
    }

    private void lstSearchAddresses_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete || this.lstSearchAddresses.SelectedItems.Count != 1)
        return;
      this.lstSearchAddresses.Items.Remove(this.lstSearchAddresses.SelectedItems[0]);
    }

    private void SaveUserCheats()
    {
      string xml = "<usercheats></usercheats>";
      string str = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm.USER_CHEATS_FILE;
      if (File.Exists(str))
        xml = File.ReadAllText(str);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      bool flag = false;
      for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; ++i)
      {
        if (this.m_game.id == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value)
          flag = true;
      }
      if (!flag)
      {
        XmlElement element = xmlDocument.CreateElement("game");
        element.SetAttribute("id", this.m_game.id);
        xmlDocument["usercheats"].AppendChild((XmlNode) element);
      }
      for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; ++i)
      {
        if (this.m_game.id == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value)
        {
          XmlElement childNode = xmlDocument["usercheats"].ChildNodes[i] as XmlElement;
          childNode.InnerXml = "";
          foreach (file file in this.m_game.GetTargetGameFolder().files._files)
          {
            XmlElement element1 = xmlDocument.CreateElement("file");
            element1.SetAttribute("name", Path.GetFileName(file.filename));
            childNode.AppendChild((XmlNode) element1);
            foreach (cheat allCheat in file.GetAllCheats())
            {
              if (allCheat.id == "-1")
              {
                XmlElement element2 = xmlDocument.CreateElement("cheat");
                element2.SetAttribute("desc", allCheat.name);
                element2.SetAttribute("comment", allCheat.note);
                element1.AppendChild((XmlNode) element2);
                XmlElement element3 = xmlDocument.CreateElement("code");
                element3.InnerText = allCheat.code;
                element2.AppendChild((XmlNode) element3);
              }
            }
          }
        }
      }
      xmlDocument.Save(str);
    }

    private void btnSaveCodes_Click(object sender, EventArgs e)
    {
      file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), (string) this.cbSaveFiles.SelectedItem);
      List<cheat> cheatList = new List<cheat>();
      foreach (cheat allCheat in gameFile.GetAllCheats())
      {
        if (allCheat.id == "-1")
          cheatList.Add(allCheat);
      }
      foreach (cheat cheat in cheatList)
        gameFile.Cheats.Remove(cheat);
      foreach (ListViewItem listViewItem in (ListBox.ObjectCollection) this.listViewCheats.Items)
      {
        if (!string.IsNullOrEmpty((string) listViewItem.Tag))
          gameFile.Cheats.Add(new cheat("-1", listViewItem.Text, "")
          {
            code = (string) listViewItem.Tag
          });
      }
      this.SaveUserCheats();
    }

    private void btnCompare_Click(object sender, EventArgs e)
    {
      if (this.diffResults.Visible || this.hexBoxLeft.ByteProvider == null || this.hexBoxRight.ByteProvider == null || (this.hexBoxLeft.ByteProvider as DynamicByteProvider).Bytes == null || (this.hexBoxRight.ByteProvider as DynamicByteProvider).Bytes == null)
        return;
      byte[] bytes1 = (this.hexBoxLeft.ByteProvider as DynamicByteProvider).Bytes.GetBytes();
      byte[] bytes2 = (this.hexBoxRight.ByteProvider as DynamicByteProvider).Bytes.GetBytes();
      Dictionary<long, byte> dictionary = new Dictionary<long, byte>();
      for (int index1 = 0; index1 < Math.Min(bytes2.Length, bytes1.Length); ++index1)
      {
        if ((int) bytes1[index1] != (int) bytes2[index1])
        {
          dictionary.Add((long) index1, (byte) 0);
          long key = (long) index1;
          for (int index2 = index1; index2 < Math.Min(bytes2.Length, bytes1.Length) && (int) bytes1[index1] != (int) bytes2[index1]; ++index1)
          {
            ++dictionary[key];
            ++index2;
          }
        }
      }
      foreach (long key in dictionary.Keys)
      {
        this.hexBoxLeft.SelectAddresses = dictionary;
        this.hexBoxRight.SelectAddresses = dictionary;
      }
      if (this.diffResults == null)
        return;
      this.diffResults.Differences = dictionary;
      this.diffResults.Show((IWin32Window) this);
      this.hexBoxLeft.SelectAddresses = dictionary;
      this.hexBoxRight.SelectAddresses = dictionary;
    }

    private void chkEnableRight_CheckedChanged(object sender, EventArgs e)
    {
      this.tableMain.BackColor = Color.FromArgb(0, 138, 213);
      this.tableLayoutMiddle.BackColor = Color.FromArgb(0, 138, 213);
      this.tableRight.BackColor = Color.FromArgb(0, 138, 213);
      this.tableTop.BackColor = Color.FromArgb(0, 138, 213);
      this._resizeInProgress = true;
      this.tableMain.SuspendLayout();
      if (this.chkEnableRight.Checked)
      {
        this.MinimumSize = new Size(Math.Min(1230, Screen.PrimaryScreen.WorkingArea.Width), this.MinimumSize.Height);
        this.Width = Math.Min(1230, Screen.PrimaryScreen.WorkingArea.Width);
        this.tableLayoutMiddle.ColumnStyles[0].SizeType = SizeType.Percent;
        this.tableLayoutMiddle.ColumnStyles[1].SizeType = SizeType.Percent;
        this.tableLayoutMiddle.ColumnStyles[0].Width = 50f;
        this.tableLayoutMiddle.ColumnStyles[1].Width = 50f;
        this.panelRight.BringToFront();
      }
      else
      {
        this.MinimumSize = new Size(800, this.MinimumSize.Height);
        this.Width = Math.Min(890, Screen.PrimaryScreen.WorkingArea.Width);
        this.tableLayoutMiddle.ColumnStyles[0].SizeType = SizeType.Percent;
        this.tableLayoutMiddle.ColumnStyles[1].SizeType = SizeType.Percent;
        this.tableLayoutMiddle.ColumnStyles[0].Width = 100f;
        this.tableLayoutMiddle.ColumnStyles[1].Width = 0.0f;
      }
      this.CenterToScreen();
      this.tableMain.ResumeLayout();
      this.tableMain.Refresh();
      this.tableMain.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.tableLayoutMiddle.BackColor = Color.Transparent;
      this.tableRight.BackColor = Color.Transparent;
      this.tableTop.BackColor = Color.Transparent;
      this._resizeInProgress = false;
      this.Invalidate(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tableMain = new CustomTableLayoutPanel();
      this.tableLayoutMiddle = new CustomTableLayoutPanel();
      this.hexBoxLeft = new HexBox();
      this.panelRight = new Panel();
      this.hexBoxRight = new HexBox();
      this.txtSaveDataLeft = new RichTextBox();
      this.txtSaveDataRight = new RichTextBox();
      this.panelLeft = new Panel();
      this.lblGameName = new Label();
      this.tableTop = new CustomTableLayoutPanel();
      this.label2 = new Label();
      this.txtSearchValue = new TextBox();
      this.btnStackSearch = new Button();
      this.btnFind = new Button();
      this.btnFindPrev = new Button();
      this.cbSearchMode = new ComboBox();
      this.lstSearchVal = new ListBox();
      this.lblAddress = new Label();
      this.txtAddress = new TextBox();
      this.btnStackAddress = new Button();
      this.btnGo = new Button();
      this.lstSearchAddresses = new ListBox();
      this.tableRight = new CustomTableLayoutPanel();
      this.chkEnableRight = new CheckBox();
      this.chkSyncScroll = new CheckBox();
      this.label4 = new Label();
      this.lstCache = new ListBox();
      this.btnPush = new Button();
      this.btnPop = new Button();
      this.lblCheats = new Label();
      this.listViewCheats = new CheckedListBox();
      this.lblCheatCodes = new Label();
      this.lstCheatCodes = new ListBox();
      this.cbSaveFiles = new ComboBox();
      this.btnApply = new Button();
      this.btnClose = new Button();
      this.btnCompare = new Button();
      this.panel1 = new Panel();
      this.lblOffsetValue = new Label();
      this.lblOffset = new Label();
      this.tableMain.SuspendLayout();
      this.tableLayoutMiddle.SuspendLayout();
      this.tableTop.SuspendLayout();
      this.tableRight.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.tableMain.BackColor = Color.FromArgb(204, 204, 204);
      this.tableMain.ColumnCount = 2;
      this.tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 189f));
      this.tableMain.Controls.Add((Control) this.lblGameName, 0, 0);
      this.tableMain.Controls.Add((Control) this.tableTop, 0, 1);
      this.tableMain.Controls.Add((Control) this.tableLayoutMiddle, 0, 2);
      this.tableMain.Controls.Add((Control) this.tableRight, 1, 0);
      this.tableMain.Controls.Add((Control) this.panel1, 0, 3);
      this.tableMain.Dock = DockStyle.Fill;
      this.tableMain.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.tableMain.Name = "tableMain";
      this.tableMain.Padding = new Padding(Util.ScaleSize(8));
      this.tableMain.RowCount = 4;
      this.tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
      this.tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
      this.tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
      this.tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableMain.Size = Util.ScaleSize(new Size(878, 517));
      this.tableMain.TabIndex = 0;
      this.tableLayoutMiddle.BackColor = Color.Transparent;
      this.tableLayoutMiddle.ColumnCount = 2;
      this.tableLayoutMiddle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutMiddle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.0f));
      this.tableLayoutMiddle.Controls.Add((Control) this.hexBoxLeft, 0, 1);
      this.tableLayoutMiddle.Controls.Add((Control) this.panelRight, 1, 0);
      this.tableLayoutMiddle.Controls.Add((Control) this.hexBoxRight, 1, 1);
      this.tableLayoutMiddle.Controls.Add((Control) this.txtSaveDataLeft, 0, 1);
      this.tableLayoutMiddle.Controls.Add((Control) this.txtSaveDataRight, 1, 1);
      this.tableLayoutMiddle.Controls.Add((Control) this.panelLeft, 0, 0);
      this.tableLayoutMiddle.Dock = DockStyle.Fill;
      this.tableLayoutMiddle.Location = new Point(Util.ScaleSize(8), Util.ScaleSize(92));
      this.tableLayoutMiddle.Margin = new Padding(0);
      this.tableLayoutMiddle.MaximumSize = Util.ScaleSize(new Size(1280, 1000));
      this.tableLayoutMiddle.Name = "tableLayoutMiddle";
      this.tableLayoutMiddle.RowCount = 3;
      this.tableLayoutMiddle.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableLayoutMiddle.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutMiddle.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f));
      this.tableLayoutMiddle.Size = Util.ScaleSize(new Size(673, 387));
      this.tableLayoutMiddle.TabIndex = 31;
      this.hexBoxLeft.Dock = DockStyle.Fill;
      this.hexBoxLeft.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hexBoxLeft.HScrollBarVisible = false;
      this.hexBoxLeft.LineInfoForeColor = Color.Empty;
      this.hexBoxLeft.Location = new Point(Util.ScaleSize(676), Util.ScaleSize(23));
      this.hexBoxLeft.Name = "hexBoxLeft";
      this.hexBoxLeft.ShadowSelectionColor = Color.FromArgb(100, 60, 188, (int) byte.MaxValue);
      this.hexBoxLeft.Size = Util.ScaleSize(new Size(1, 361));
      this.hexBoxLeft.TabIndex = 0;
      this.panelRight.Dock = DockStyle.Fill;
      this.panelRight.Font = new Font("Courier New", Util.ScaleSize(9f));
      this.panelRight.Location = new Point(Util.ScaleSize(673), 0);
      this.panelRight.Margin = new Padding(0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = Util.ScaleSize(new Size(1, 20));
      this.panelRight.TabIndex = 29;
      this.hexBoxRight.Dock = DockStyle.Fill;
      this.hexBoxRight.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hexBoxRight.HScrollBarVisible = false;
      this.hexBoxRight.LineInfoForeColor = Color.Empty;
      this.hexBoxRight.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(390));
      this.hexBoxRight.Name = "hexBoxRight";
      this.hexBoxRight.ShadowSelectionColor = Color.FromArgb(100, 60, 188, (int) byte.MaxValue);
      this.hexBoxRight.Size = Util.ScaleSize(new Size(667, 1));
      this.hexBoxRight.TabIndex = 27;
      this.txtSaveDataLeft.Dock = DockStyle.Fill;
      this.txtSaveDataLeft.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(23));
      this.txtSaveDataLeft.Name = "txtSaveDataLeft";
      this.txtSaveDataLeft.Size = Util.ScaleSize(new Size(667, 361));
      this.txtSaveDataLeft.TabIndex = 31;
      this.txtSaveDataLeft.Text = "";
      this.txtSaveDataLeft.Visible = false;
      this.txtSaveDataRight.Dock = DockStyle.Fill;
      this.txtSaveDataRight.Location = new Point(Util.ScaleSize(676), Util.ScaleSize(390));
      this.txtSaveDataRight.Name = "txtSaveDataRight";
      this.txtSaveDataRight.Size = Util.ScaleSize(new Size(1, 1));
      this.txtSaveDataRight.TabIndex = 30;
      this.txtSaveDataRight.Text = "";
      this.txtSaveDataRight.Visible = false;
      this.panelLeft.Dock = DockStyle.Fill;
      this.panelLeft.Font = new Font("Courier New", Util.ScaleSize(9f));
      this.panelLeft.Location = new Point(0, 0);
      this.panelLeft.Margin = new Padding(0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = Util.ScaleSize(new Size(673, 20));
      this.panelLeft.TabIndex = 32;
      this.lblGameName.AutoSize = true;
      this.lblGameName.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(8));
      this.lblGameName.Name = "lblGameName";
      this.lblGameName.Size = Util.ScaleSize(new Size(66, 13));
      this.lblGameName.TabIndex = 0;
      this.lblGameName.Text = "Game Name";
      this.tableTop.ColumnCount = 11;
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
      this.tableTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableTop.Controls.Add((Control) this.label2, 0, 2);
      this.tableTop.Controls.Add((Control) this.txtSearchValue, 1, 2);
      this.tableTop.Controls.Add((Control) this.btnStackSearch, 1, 0);
      this.tableTop.Controls.Add((Control) this.btnFind, 3, 2);
      this.tableTop.Controls.Add((Control) this.btnFindPrev, 4, 2);
      this.tableTop.Controls.Add((Control) this.cbSearchMode, 3, 0);
      this.tableTop.Controls.Add((Control) this.lstSearchVal, 5, 0);
      this.tableTop.Controls.Add((Control) this.lblAddress, 6, 2);
      this.tableTop.Controls.Add((Control) this.txtAddress, 7, 2);
      this.tableTop.Controls.Add((Control) this.btnStackAddress, 7, 0);
      this.tableTop.Controls.Add((Control) this.btnGo, 8, 2);
      this.tableTop.Controls.Add((Control) this.lstSearchAddresses, 9, 0);
      this.tableTop.Dock = DockStyle.Fill;
      this.tableTop.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(34));
      this.tableTop.Margin = new Padding(2);
      this.tableTop.Name = "tableTop";
      this.tableTop.RowCount = 4;
      this.tableTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 23f));
      this.tableTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 2f));
      this.tableTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
      this.tableTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 2f));
      this.tableTop.Size = Util.ScaleSize(new Size(669, 56));
      this.tableTop.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Dock = DockStyle.Fill;
      this.label2.Location = new Point(0, Util.ScaleSize(25));
      this.label2.Margin = new Padding(0);
      this.label2.Name = "label2";
      this.label2.Size = Util.ScaleSize(new Size(45, 22));
      this.label2.TabIndex = 0;
      this.label2.Text = "Search";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.txtSearchValue.Dock = DockStyle.Fill;
      this.txtSearchValue.Location = new Point(Util.ScaleSize(46), Util.ScaleSize(26));
      this.txtSearchValue.Margin = new Padding(1, 1, 1, 2);
      this.txtSearchValue.MinimumSize = Util.ScaleSize(new Size(68, 21));
      this.txtSearchValue.Name = "txtSearchValue";
      this.txtSearchValue.Size = Util.ScaleSize(new Size(68, 20));
      this.txtSearchValue.TabIndex = 1;
      this.btnStackSearch.Dock = DockStyle.Fill;
      this.btnStackSearch.Location = new Point(Util.ScaleSize(45), 0);
      this.btnStackSearch.Margin = new Padding(0);
      this.btnStackSearch.Name = "btnStackSearch";
      this.btnStackSearch.Size = Util.ScaleSize(new Size(70, 23));
      this.btnStackSearch.TabIndex = 2;
      this.btnStackSearch.Text = "Stack";
      this.btnStackSearch.UseVisualStyleBackColor = true;
      this.btnFind.Dock = DockStyle.Fill;
      this.btnFind.Location = new Point(Util.ScaleSize(117), Util.ScaleSize(25));
      this.btnFind.Margin = new Padding(0);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = Util.ScaleSize(new Size(70, 22));
      this.btnFind.TabIndex = 3;
      this.btnFind.Text = "Find";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFindPrev.Dock = DockStyle.Fill;
      this.btnFindPrev.Location = new Point(Util.ScaleSize(189), Util.ScaleSize(25));
      this.btnFindPrev.Margin = new Padding(Util.ScaleSize(2), 0, 0, 0);
      this.btnFindPrev.Name = "btnFindPrev";
      this.btnFindPrev.Size = Util.ScaleSize(new Size(68, 22));
      this.btnFindPrev.TabIndex = 4;
      this.btnFindPrev.Text = "Find Prev.";
      this.btnFindPrev.UseVisualStyleBackColor = true;
      this.cbSearchMode.Dock = DockStyle.Fill;
      this.cbSearchMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSearchMode.IntegralHeight = false;
      this.cbSearchMode.Location = new Point(Util.ScaleSize(118), Util.ScaleSize(1));
      this.cbSearchMode.Margin = new Padding(1, 1, 0, 1);
      this.cbSearchMode.MaximumSize = Util.ScaleSize(new Size(68, 0));
      this.cbSearchMode.Name = "cbSearchMode";
      this.cbSearchMode.Size = Util.ScaleSize(new Size(68, 21));
      this.cbSearchMode.TabIndex = 5;
      this.lstSearchVal.Dock = DockStyle.Fill;
      this.lstSearchVal.FormattingEnabled = true;
      this.lstSearchVal.IntegralHeight = false;
      this.lstSearchVal.Location = new Point(Util.ScaleSize(260), Util.ScaleSize(1));
      this.lstSearchVal.Margin = new Padding(3, 1, 3, 1);
      this.lstSearchVal.Name = "lstSearchVal";
      this.tableTop.SetRowSpan((Control) this.lstSearchVal, 3);
      this.lstSearchVal.Size = Util.ScaleSize(new Size(74, 45));
      this.lstSearchVal.TabIndex = 6;
      this.lblAddress.AutoSize = true;
      this.lblAddress.Dock = DockStyle.Fill;
      this.lblAddress.Location = new Point(Util.ScaleSize(337), Util.ScaleSize(25));
      this.lblAddress.Margin = new Padding(0);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = Util.ScaleSize(new Size(50, 22));
      this.lblAddress.TabIndex = 7;
      this.lblAddress.Text = "Address";
      this.lblAddress.TextAlign = ContentAlignment.MiddleRight;
      this.txtAddress.Dock = DockStyle.Fill;
      this.txtAddress.Location = new Point(Util.ScaleSize(388), Util.ScaleSize(26));
      this.txtAddress.Margin = new Padding(1);
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = Util.ScaleSize(new Size(68, 20));
      this.txtAddress.TabIndex = 8;
      this.btnStackAddress.Dock = DockStyle.Fill;
      this.btnStackAddress.Location = new Point(Util.ScaleSize(387), 0);
      this.btnStackAddress.Margin = new Padding(0);
      this.btnStackAddress.Name = "btnStackAddress";
      this.btnStackAddress.Size = Util.ScaleSize(new Size(70, 23));
      this.btnStackAddress.TabIndex = 9;
      this.btnStackAddress.Text = "Stack";
      this.btnStackAddress.UseVisualStyleBackColor = true;
      this.btnGo.Dock = DockStyle.Fill;
      this.btnGo.Location = new Point(Util.ScaleSize(459), Util.ScaleSize(25));
      this.btnGo.Margin = new Padding(2, 0, 0, 0);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = Util.ScaleSize(new Size(38, 22));
      this.btnGo.TabIndex = 10;
      this.btnGo.Text = "OK";
      this.btnGo.UseVisualStyleBackColor = true;
      this.lstSearchAddresses.Dock = DockStyle.Fill;
      this.lstSearchAddresses.FormattingEnabled = true;
      this.lstSearchAddresses.IntegralHeight = false;
      this.lstSearchAddresses.Location = new Point(Util.ScaleSize(500), Util.ScaleSize(1));
      this.lstSearchAddresses.Margin = new Padding(Util.ScaleSize(3), Util.ScaleSize(1), Util.ScaleSize(3), Util.ScaleSize(1));
      this.lstSearchAddresses.Name = "lstSearchAddresses";
      this.tableTop.SetRowSpan((Control) this.lstSearchAddresses, Util.ScaleSize(3));
      this.lstSearchAddresses.Size = Util.ScaleSize(new Size(74, 45));
      this.lstSearchAddresses.TabIndex = 11;
      this.tableRight.ColumnCount = 2;
      this.tableRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, Util.ScaleSize(50f)));
      this.tableRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, Util.ScaleSize(50f)));
      this.tableRight.Controls.Add((Control) this.chkEnableRight, 0, 2);
      this.tableRight.Controls.Add((Control) this.chkSyncScroll, 0, 3);
      this.tableRight.Controls.Add((Control) this.label4, 0, 4);
      this.tableRight.Controls.Add((Control) this.lstCache, 0, 5);
      this.tableRight.Controls.Add((Control) this.btnPush, 0, 6);
      this.tableRight.Controls.Add((Control) this.btnPop, 1, 6);
      this.tableRight.Controls.Add((Control) this.lblCheats, 0, 7);
      this.tableRight.Controls.Add((Control) this.listViewCheats, 0, 8);
      this.tableRight.Controls.Add((Control) this.lblCheatCodes, 0, 9);
      this.tableRight.Controls.Add((Control) this.lstCheatCodes, 0, 10);
      this.tableRight.Controls.Add((Control) this.cbSaveFiles, 0, 0);
      this.tableRight.Controls.Add((Control) this.btnApply, 0, 12);
      this.tableRight.Controls.Add((Control) this.btnClose, 1, 12);
      this.tableRight.Controls.Add((Control) this.btnCompare, 0, 1);
      this.tableRight.Dock = DockStyle.Fill;
      this.tableRight.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(11));
      this.tableRight.Margin = new Padding(3, 3, 2, 3);
      this.tableRight.Name = "tableRight";
      this.tableRight.RowCount = 13;
      this.tableMain.SetRowSpan((Control) this.tableRight, 4);
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 19f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 19f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
      this.tableRight.Size = Util.ScaleSize(new Size(184, 495));
      this.tableRight.TabIndex = 3;
      this.chkEnableRight.AutoSize = true;
      this.tableRight.SetColumnSpan((Control) this.chkEnableRight, 2);
      this.chkEnableRight.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(47));
      this.chkEnableRight.Margin = new Padding(Util.ScaleSize(3), Util.ScaleSize(3), Util.ScaleSize(3), 0);
      this.chkEnableRight.Name = "chkEnableRight";
      this.chkEnableRight.Size = Util.ScaleSize(new Size(87, 16));
      this.chkEnableRight.TabIndex = 0;
      this.chkEnableRight.Text = "Enable Right";
      this.chkEnableRight.UseVisualStyleBackColor = true;
      this.chkEnableRight.CheckedChanged += new EventHandler(this.chkEnableRight_CheckedChanged);
      this.chkSyncScroll.AutoSize = true;
      this.tableRight.SetColumnSpan((Control) this.chkSyncScroll, 2);
      this.chkSyncScroll.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(66));
      this.chkSyncScroll.Margin = new Padding(Util.ScaleSize(3), Util.ScaleSize(3), Util.ScaleSize(3), 0);
      this.chkSyncScroll.Name = "chkSyncScroll";
      this.chkSyncScroll.Size = Util.ScaleSize(new Size(79, 16));
      this.chkSyncScroll.TabIndex = 1;
      this.chkSyncScroll.Text = "Sync Scroll";
      this.chkSyncScroll.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.tableRight.SetColumnSpan((Control) this.label4, 2);
      this.label4.Dock = DockStyle.Fill;
      this.label4.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(82));
      this.label4.Name = "label4";
      this.label4.Size = Util.ScaleSize(new Size(178, 20));
      this.label4.TabIndex = 2;
      this.label4.Text = "Savedata Cache:";
      this.label4.TextAlign = ContentAlignment.BottomLeft;
      this.tableRight.SetColumnSpan((Control) this.lstCache, 2);
      this.lstCache.Dock = DockStyle.Fill;
      this.lstCache.FormattingEnabled = true;
      this.lstCache.IntegralHeight = false;
      this.lstCache.Location = new Point(Util.ScaleSize(2), Util.ScaleSize(104));
      this.lstCache.Margin = new Padding(2);
      this.lstCache.Name = "lstCache";
      this.lstCache.Size = Util.ScaleSize(new Size(180, 76));
      this.lstCache.TabIndex = 3;
      this.btnPush.Dock = DockStyle.Fill;
      this.btnPush.Location = new Point(Util.ScaleSize(1), Util.ScaleSize(183));
      this.btnPush.Margin = new Padding(Util.ScaleSize(1));
      this.btnPush.Name = "btnPush";
      this.btnPush.Size = Util.ScaleSize(new Size(90, 22));
      this.btnPush.TabIndex = 4;
      this.btnPush.Text = "Push";
      this.btnPush.UseVisualStyleBackColor = true;
      this.btnPop.Dock = DockStyle.Fill;
      this.btnPop.Location = new Point(Util.ScaleSize(93), Util.ScaleSize(183));
      this.btnPop.Margin = new Padding(Util.ScaleSize(1));
      this.btnPop.Name = "btnPop";
      this.btnPop.Size = Util.ScaleSize(new Size(90, 22));
      this.btnPop.TabIndex = 5;
      this.btnPop.Text = "Pop";
      this.btnPop.UseVisualStyleBackColor = true;
      this.lblCheats.AutoSize = true;
      this.tableRight.SetColumnSpan((Control) this.lblCheats, 2);
      this.lblCheats.Dock = DockStyle.Fill;
      this.lblCheats.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(206));
      this.lblCheats.Name = "lblCheats";
      this.lblCheats.Size = Util.ScaleSize(new Size(178, 22));
      this.lblCheats.TabIndex = 6;
      this.lblCheats.Text = "Cheats";
      this.lblCheats.TextAlign = ContentAlignment.BottomLeft;
      this.tableRight.SetColumnSpan((Control) this.listViewCheats, 2);
      this.listViewCheats.Dock = DockStyle.Fill;
      this.listViewCheats.FormattingEnabled = true;
      this.listViewCheats.IntegralHeight = false;
      this.listViewCheats.Location = new Point(Util.ScaleSize(2), Util.ScaleSize(230));
      this.listViewCheats.Margin = new Padding(2);
      this.listViewCheats.Name = "listViewCheats";
      this.listViewCheats.Size = Util.ScaleSize(new Size(180, 76));
      this.listViewCheats.TabIndex = 7;
      this.lblCheatCodes.AutoSize = true;
      this.tableRight.SetColumnSpan((Control) this.lblCheatCodes, 2);
      this.lblCheatCodes.Dock = DockStyle.Fill;
      this.lblCheatCodes.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(308));
      this.lblCheatCodes.Name = "lblCheatCodes";
      this.lblCheatCodes.Size = Util.ScaleSize(new Size(178, 30));
      this.lblCheatCodes.TabIndex = 8;
      this.lblCheatCodes.Text = "Cheat Codes";
      this.lblCheatCodes.TextAlign = ContentAlignment.BottomLeft;
      this.tableRight.SetColumnSpan((Control) this.lstCheatCodes, 2);
      this.lstCheatCodes.Dock = DockStyle.Fill;
      this.lstCheatCodes.FormattingEnabled = true;
      this.lstCheatCodes.IntegralHeight = false;
      this.lstCheatCodes.Location = new Point(Util.ScaleSize(2), Util.ScaleSize(340));
      this.lstCheatCodes.Margin = new Padding(Util.ScaleSize(2));
      this.lstCheatCodes.Name = "lstCheatCodes";
      this.lstCheatCodes.Size = Util.ScaleSize(new Size(180, 76));
      this.lstCheatCodes.TabIndex = 9;
      this.tableRight.SetColumnSpan((Control) this.cbSaveFiles, 2);
      this.cbSaveFiles.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSaveFiles.FormattingEnabled = true;
      this.cbSaveFiles.IntegralHeight = false;
      this.cbSaveFiles.Location = new Point(Util.ScaleSize(2), 0);
      this.cbSaveFiles.Margin = new Padding(Util.ScaleSize(2), 0, Util.ScaleSize(3), Util.ScaleSize(3));
      this.cbSaveFiles.Name = "cbSaveFiles";
      this.cbSaveFiles.Size = Util.ScaleSize(new Size(121, 21));
      this.cbSaveFiles.TabIndex = 10;
      this.btnApply.Dock = DockStyle.Fill;
      this.btnApply.Location = new Point(Util.ScaleSize(1), Util.ScaleSize(472));
      this.btnApply.Margin = new Padding(Util.ScaleSize(1));
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = Util.ScaleSize(new Size(90, 22));
      this.btnApply.TabIndex = 11;
      this.btnApply.Text = "OK";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnClose.Dock = DockStyle.Fill;
      this.btnClose.Location = new Point(Util.ScaleSize(93), Util.ScaleSize(472));
      this.btnClose.Margin = new Padding(Util.ScaleSize(1));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(90, 22));
      this.btnClose.TabIndex = 12;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.tableRight.SetColumnSpan((Control) this.btnCompare, 2);
      this.btnCompare.Location = new Point(Util.ScaleSize(1), Util.ScaleSize(23));
      this.btnCompare.Margin = new Padding(Util.ScaleSize(1), Util.ScaleSize(3), Util.ScaleSize(1), Util.ScaleSize(1));
      this.btnCompare.Name = "btnCompare";
      this.btnCompare.Size = Util.ScaleSize(new Size(122, 20));
      this.btnCompare.TabIndex = 13;
      this.btnCompare.Text = "Compare";
      this.btnCompare.UseVisualStyleBackColor = true;
      this.panel1.Controls.Add((Control) this.lblOffsetValue);
      this.panel1.Controls.Add((Control) this.lblOffset);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(482));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(667, 24));
      this.panel1.TabIndex = 30;
      this.lblOffsetValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblOffsetValue.AutoSize = true;
      this.lblOffsetValue.Location = new Point(Util.ScaleSize(517), Util.ScaleSize(10));
      this.lblOffsetValue.Name = "lblOffsetValue";
      this.lblOffsetValue.Size = Util.ScaleSize(new Size(66, 13));
      this.lblOffsetValue.TabIndex = 1;
      this.lblOffsetValue.Text = "0x00000000";
      this.lblOffset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblOffset.AutoSize = true;
      this.lblOffset.Location = new Point(Util.ScaleSize(467), Util.ScaleSize(10));
      this.lblOffset.Name = "lblOffset";
      this.lblOffset.Size = Util.ScaleSize(new Size(38, 13));
      this.lblOffset.TabIndex = 0;
      this.lblOffset.Text = "Offset:";
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.FromArgb(0, 138, 213);
      this.ClientSize = Util.ScaleSize(new Size(898, 537));
      this.Controls.Add((Control) this.tableMain);
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.Name = nameof (AdvancedEdit2);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.Text = "Advanced Edit";
      this.tableMain.ResumeLayout(false);
      this.tableMain.PerformLayout();
      this.tableLayoutMiddle.ResumeLayout(false);
      this.tableTop.ResumeLayout(false);
      this.tableTop.PerformLayout();
      this.tableRight.ResumeLayout(false);
      this.tableRight.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
