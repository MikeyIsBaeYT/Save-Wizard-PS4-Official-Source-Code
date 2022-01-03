// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.MainForm
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using CSUST.Data;
using Microsoft.Win32;
using PS3SaveEditor.Utilities;
using Rss;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  public class MainForm : Form
  {
    private Dictionary<string, List<game>> m_dictLocalSaves = new Dictionary<string, List<game>>();
    private string m_expandedGame = (string) null;
    internal const int WM_DEVICECHANGE = 537;
    public const int WM_SYSCOMMAND = 274;
    public const int MF_SEPARATOR = 2048;
    public const int MF_BYPOSITION = 1024;
    public const int MF_STRING = 0;
    public const int IDM_ABOUT = 1000;
    private Dictionary<int, string> RegionMap;
    private const string UNREGISTER_UUID = "{{\"action\":\"UNREGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";
    private const string DESTROY_SESSION = "{{\"action\":\"DESTROY_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";
    private const string SESSION_INIT_URL = "{{\"action\":\"START_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";
    private const string PSNID_INFO = "{{\"action\":\"PSNID_INFO\",\"userid\":\"{0}\"}}";
    private const string ACTIVATE_PACKAGE = "{{\"action\":\"ADD_PACKAGE\",\"license\":\"{0}-{1}-{2}-{3}\",\"userid\":\"{4}\"}}";
    private const string SESSION_CLOSAL = "{0}/?q=software_auth2/sessionclose&sessionid={1}";
    private const int INTERNAL_VERION_MAJOR = 1;
    private const int INTERNAL_VERION_MINOR = 0;
    private int previousDriveNum = 0;
    private bool isRunning = false;
    private MainForm.GetTrafficDelegate GetTrafficFunc;
    private List<game> m_games;
    private DrivesHelper drivesHelper;
    public static string USER_CHEATS_FILE = "swusercheats.xml";
    private bool m_bSerialChecked = false;
    private bool m_sessionInited = false;
    private AutoResetEvent evt;
    private AutoResetEvent evt2;
    private Dictionary<string, object> m_psnIDs = (Dictionary<string, object>) null;
    private int m_psn_quota = 0;
    private int m_psn_remaining = 0;
    private bool isFirstRunning = true;
    private IContainer components = (IContainer) null;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem simpleToolStripMenuItem;
    private ToolStripMenuItem advancedToolStripMenuItem;
    private Button btnHome;
    private Button btnHelp;
    private Button btnOptions;
    private Panel pnlHome;
    private ToolStripMenuItem restoreFromBackupToolStripMenuItem;
    private Panel panel1;
    private ComboBox cbDrives;
    private CustomDataGridView dgServerGames;
    private ToolStripMenuItem deleteSaveToolStripMenuItem;
    private Button btnGamesInServer;
    private CheckBox chkShowAll;
    private Panel pnlNoSaves;
    private Label lblNoSaves;
    private Button btnOpenFolder;
    private Label lblBackup;
    private Button btnBrowse;
    private TextBox txtBackupLocation;
    private CheckBox chkBackup;
    private Button btnApply;
    private Label lblRSSSection;
    private Button btnRss;
    private Label lblDeactivate;
    private Button btnDeactivate;
    private Panel pnlBackup;
    private DataGridViewTextBoxColumn Multi;
    private Label lblManageProfiles;
    private Button btnManageProfiles;
    private ToolStripMenuItem registerPSNIDToolStripMenuItem;
    private ToolStripMenuItem resignToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private DataGridViewTextBoxColumn Choose;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private Panel panel2;
    private CustomGroupBox gbBackupLocation;
    private CustomGroupBox groupBox1;
    private CustomGroupBox groupBox2;
    private CustomGroupBox gbManageProfile;
    private CustomGroupBox gbProfiles;
    private Panel panel3;
    private PictureBox picTraffic;
    private PictureBox pictureBox2;
    private PictureBox picVersion;
    private ComboBox cbLanguage;
    private Label lblLanguage;
    private PictureBox picContact;
    private CustomGroupBox groupBox3;
    private Label label1;
    private Button btnActivatePackage;
    private Label label4;
    private Label label3;
    private Label label2;
    private TextBox txtSerial4;
    private TextBox txtSerial3;
    private TextBox txtSerial2;
    private TextBox txtSerial1;

    public MainForm()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ja");
      this.m_games = new List<game>();
      this.RegionMap = new Dictionary<int, string>();
      this.lblLanguage.Text = PS3SaveEditor.Resources.Resources.lblLanguage;
      this.label1.Text = PS3SaveEditor.Resources.Resources.lblPackageSerial;
      this.groupBox3.Visible = false;
      this.chkShowAll.CheckedChanged += new EventHandler(this.chkShowAll_CheckedChanged);
      this.chkShowAll.EnabledChanged += new EventHandler(this.chkShowAll_EnabledChanged);
      this.picTraffic.Visible = false;
      this.ResizeBegin += (EventHandler) ((s, e) => this.SuspendLayout());
      this.ResizeEnd += (EventHandler) ((s, e) =>
      {
        this.ResumeLayout(true);
        this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);
        this.Invalidate(true);
      });
      this.SizeChanged += (EventHandler) ((s, e) =>
      {
        if (this.WindowState != FormWindowState.Maximized)
          return;
        this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);
        this.Invalidate(true);
      });
      this.txtBackupLocation.ReadOnly = true;
      this.dgServerGames.Columns[0].ReadOnly = true;
      this.MinimumSize = this.Size;
      this.dgServerGames.CellClick += new DataGridViewCellEventHandler(this.dgServerGames_CellClick);
      this.dgServerGames.SelectionChanged += new EventHandler(this.dgServerGames_SelectionChanged);
      this.dgServerGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.btnGamesInServer.Visible = false;
      this.btnRss.BackColor = SystemColors.ButtonFace;
      this.btnOpenFolder.BackColor = SystemColors.ButtonFace;
      this.btnBrowse.BackColor = SystemColors.ButtonFace;
      this.btnDeactivate.BackColor = SystemColors.ButtonFace;
      this.btnManageProfiles.BackColor = SystemColors.ButtonFace;
      this.btnApply.BackColor = SystemColors.ButtonFace;
      this.btnRss.ForeColor = System.Drawing.Color.Black;
      this.btnOpenFolder.ForeColor = System.Drawing.Color.Black;
      this.btnBrowse.ForeColor = System.Drawing.Color.Black;
      this.btnDeactivate.ForeColor = System.Drawing.Color.Black;
      this.btnManageProfiles.ForeColor = System.Drawing.Color.Black;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.pnlBackup.BackColor = this.pnlHome.BackColor = this.pnlHome.BackColor = this.pnlNoSaves.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.gbBackupLocation.BackColor = this.gbManageProfile.BackColor = this.groupBox1.BackColor = this.groupBox2.BackColor = System.Drawing.Color.Transparent;
      this.chkShowAll.BackColor = System.Drawing.Color.FromArgb(0, 204, 204, 204);
      this.chkShowAll.ForeColor = System.Drawing.Color.White;
      this.panel2.Visible = false;
      this.registerPSNIDToolStripMenuItem.Visible = false;
      this.resignToolStripMenuItem.Visible = true;
      this.toolStripSeparator1.Visible = false;
      if (Util.IsUnixOrMacOSX())
      {
        this.groupBox2.Size = Util.ScaleSize(new Size(234, 65));
        if (this.WindowState == FormWindowState.Minimized)
          this.WindowState = FormWindowState.Normal;
        this.Activate();
      }
      else
        Util.SetForegroundWindow(this.Handle);
      this.CenterToScreen();
      this.SetLabels();
      if (!Util.IsUnixOrMacOSX())
        Util.SetForegroundWindow(this.Handle);
      this.cbDrives.SelectedIndexChanged += new EventHandler(this.cbDrives_SelectedIndexChanged);
      this.dgServerGames.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgServerGames_CellMouseDown);
      this.dgServerGames.CellDoubleClick += new DataGridViewCellEventHandler(this.dgServerGames_CellDoubleClick);
      this.dgServerGames.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgServerGames_ColumnHeaderMouseClick);
      this.dgServerGames.ShowCellToolTips = true;
      this.panel2.BackgroundImage = (Image) null;
      string[] directories = Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath));
      string registryValue = Util.GetRegistryValue("Language");
      this.cbLanguage.DisplayMember = "NativeName";
      this.cbLanguage.ValueMember = "Name";
      List<CultureInfo> cultureInfoList = new List<CultureInfo>();
      cultureInfoList.Add(new CultureInfo("en"));
      this.cbLanguage.SelectedValueChanged += new EventHandler(this.cbLanguage_SelectedIndexChanged);
      foreach (string path in directories)
      {
        try
        {
          CultureInfo cultureInfo = new CultureInfo(Path.GetFileNameWithoutExtension(path));
          cultureInfoList.Add(cultureInfo);
        }
        catch
        {
        }
      }
      this.cbLanguage.DataSource = (object) cultureInfoList;
      if (registryValue != null)
        this.cbLanguage.SelectedValue = (object) registryValue;
      else
        this.cbLanguage.SelectedIndex = 0;
      this.cbDrives.DrawMode = DrawMode.OwnerDrawFixed;
      this.cbDrives.DrawItem += new DrawItemEventHandler(this.cbDrives_DrawItem);
      this.drivesHelper = new DrivesHelper(this.cbDrives, this.m_games, this.chkShowAll, this.pnlNoSaves);
      this.drivesHelper.FillDrives();
      this.Load += new EventHandler(this.MainForm_Load);
      this.btnHome.ChangeUICues += new UICuesEventHandler(this.btnHome_ChangeUICues);
      this.dgServerGames.BackgroundColor = System.Drawing.Color.White;
    }

    private void picContact_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo()
    {
      UseShellExecute = true,
      Verb = "open",
      FileName = "http://www.cybergadget.co.jp/contact/inquiry.html"
    });

    private void picContact_MouseLeave(object sender, EventArgs e)
    {
    }

    private void picContact_MouseHover(object sender, EventArgs e)
    {
    }

    private void btnHome_ChangeUICues(object sender, UICuesEventArgs e)
    {
      if (!e.ChangeFocus)
        return;
      this.btnHome.Focus();
    }

    private void chkShowAll_EnabledChanged(object sender, EventArgs e)
    {
      if (this.chkShowAll.Enabled)
      {
        this.chkShowAll.ForeColor = System.Drawing.Color.White;
        this.chkShowAll.FlatStyle = FlatStyle.Standard;
      }
      else
      {
        this.chkShowAll.ForeColor = System.Drawing.Color.FromArgb(190, 190, 190);
        this.chkShowAll.FlatStyle = FlatStyle.Flat;
      }
    }

    private void cbDrives_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (this.cbDrives.SelectedIndex < 0)
        return;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        e.Graphics.FillRectangle((Brush) new SolidBrush(System.Drawing.Color.FromArgb(0, 175, (int) byte.MaxValue)), e.Bounds);
        e.Graphics.DrawString(this.cbDrives.Items[e.Index].ToString(), e.Font, Brushes.White, (PointF) new Point(e.Bounds.X, e.Bounds.Y));
      }
      else
      {
        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        e.Graphics.DrawString(this.cbDrives.Items[e.Index].ToString(), e.Font, Brushes.Black, (PointF) new Point(e.Bounds.X, e.Bounds.Y));
      }
    }

    private void dgServerGames_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgServerGames.SelectedRows == null || this.dgServerGames.SelectedRows.Count <= 0)
        return;
      int index = this.dgServerGames.SelectedRows[0].Index;
      if (Util.IsUnixOrMacOSX())
        this.dgServerGames.SelectionChanged -= new EventHandler(this.dgServerGames_SelectionChanged);
      if (this.chkShowAll.Checked)
        this.dgServerGames.CurrentCell = this.dgServerGames.SelectedRows[0].Cells[1];
      else
        this.dgServerGames.CurrentCell = this.dgServerGames.SelectedRows[0].Cells[0];
      if (Util.IsUnixOrMacOSX())
        this.dgServerGames.SelectionChanged += new EventHandler(this.dgServerGames_SelectionChanged);
    }

    private void MainForm_Resize(object sender, EventArgs e) => this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, System.Drawing.Color.FromArgb(0, 138, 213), System.Drawing.Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void dgServerGames_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
    {
      if (e.Column.Index == 1)
      {
        if (this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Descending)
        {
          e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
          if (e.SortResult == 0)
          {
            if (this.dgServerGames.Rows[e.RowIndex1].Cells[1].Value.ToString().StartsWith("    "))
              e.SortResult = -1;
            if (this.dgServerGames.Rows[e.RowIndex2].Cells[1].Value.ToString().StartsWith("    "))
              e.SortResult = 1;
          }
        }
        else
        {
          e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
          e.SortResult = this.dgServerGames.Rows[e.RowIndex1].Cells[0].Tag.ToString().CompareTo(this.dgServerGames.Rows[e.RowIndex2].Cells[0].Tag.ToString());
          if (e.SortResult == 0)
          {
            if (this.dgServerGames.Rows[e.RowIndex1].Cells[1].Value.ToString().StartsWith("    "))
              e.SortResult = 1;
            if (this.dgServerGames.Rows[e.RowIndex2].Cells[1].Value.ToString().StartsWith("    "))
              e.SortResult = -1;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = false;
    }

    private void dgServerGames_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || this.dgServerGames.SelectedCells.Count == 0 || this.dgServerGames.SelectedCells[0].RowIndex < 0)
        return;
      string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
      if (!(toolTipText == PS3SaveEditor.Resources.Resources.msgUnsupported))
        return;
      int num = (int) Util.ShowMessage(toolTipText);
    }

    private void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkShowAll.Checked)
      {
        this.pnlNoSaves.Visible = false;
        this.pnlNoSaves.SendToBack();
        this.dgServerGames.Columns[3].Visible = false;
        this.ShowAllGames();
      }
      else
      {
        this.dgServerGames.Columns[0].Visible = true;
        this.dgServerGames.Columns[3].Visible = true;
        this.dgServerGames.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colGameCode;
        this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void ShowAllGames()
    {
      this.dgServerGames.Rows.Clear();
      this.dgServerGames.Columns[4].Visible = false;
      this.dgServerGames.Columns[5].Visible = false;
      this.dgServerGames.Columns[7].Visible = false;
      int width = this.dgServerGames.Width;
      if (width == 0)
        return;
      this.dgServerGames.Columns[3].Visible = false;
      this.dgServerGames.Columns[0].Visible = false;
      this.dgServerGames.Columns[1].Width = (int) ((double) width * 0.800000011920929);
      this.dgServerGames.Columns[2].Width = (int) ((double) width * 0.200000002980232);
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      ((ISupportInitialize) this.dgServerGames).BeginInit();
      foreach (game game in this.m_games)
      {
        foreach (alias allAlias in game.GetAllAliases())
        {
          if (!(game.name == allAlias.name) || !(game.id != allAlias.id))
          {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.CreateCells((DataGridView) this.dgServerGames);
            try
            {
              dataGridViewRow.Tag = (object) game;
              dataGridViewRow.Cells[1].Value = (object) allAlias.name;
              dataGridViewRow.Cells[2].Value = (object) game.GetCheatCount();
              string exregions = "";
              string region1 = Util.GetRegion(this.RegionMap, game.region, exregions);
              List<string> stringList = new List<string>();
              stringList.Add(game.id);
              if (game.aliases != null && game.aliases._aliases.Count > 0)
              {
                foreach (alias alias in game.aliases._aliases)
                {
                  string region2 = Util.GetRegion(this.RegionMap, alias.region, region1);
                  if (region1.IndexOf(region2) < 0)
                    region1 += region2;
                  stringList.Add(alias.id);
                }
              }
              stringList.Sort();
              dataGridViewRow.Cells[3].Value = (object) region1;
              dataGridViewRow.Cells[1].ToolTipText = "Supported List: " + string.Join(",", stringList.ToArray());
            }
            catch (Exception ex)
            {
              IDictionary data = ex.Data;
            }
            dataGridViewRowList.Add(dataGridViewRow);
          }
        }
      }
      this.dgServerGames.Rows.AddRange(dataGridViewRowList.ToArray());
      ((ISupportInitialize) this.dgServerGames).EndInit();
    }

    private void dgServerGames_ColumnHeaderMouseClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      if (this.chkShowAll.Checked && e.ColumnIndex == 2)
        return;
      this.SortGames(e.ColumnIndex, this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Descending);
      if (this.chkShowAll.Checked)
        this.ShowAllGames();
      else
        this.FillLocalSaves((string) null, this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
    }

    private void dgServerGames_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || this.dgServerGames.SelectedCells.Count == 0 || this.dgServerGames.SelectedCells[0].RowIndex < 0)
        return;
      string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
      if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game))
      {
        if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is List<game>))
        {
          if (!(toolTipText == PS3SaveEditor.Resources.Resources.msgUnsupported))
            return;
          int num = (int) Util.ShowMessage(toolTipText);
        }
        else
        {
          int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
          this.FillLocalSaves(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value as string, this.dgServerGames.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
          if (this.dgServerGames.Rows.Count > e.RowIndex + 1)
          {
            this.dgServerGames.Rows[e.RowIndex + 1].Selected = true;
            this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
          }
          else
          {
            this.dgServerGames.Rows[Math.Min(e.RowIndex, this.dgServerGames.Rows.Count - 1)].Selected = true;
            try
            {
              this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      else
        this.simpleToolStripMenuItem_Click((object) null, (EventArgs) null);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool InsertMenu(
      IntPtr hMenu,
      int wPosition,
      int wFlags,
      int wIDNewItem,
      string lpNewItem);

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 274)
      {
        if (m.WParam.ToInt32() == 1000)
        {
          int num = (int) new AboutBox1().ShowDialog();
          return;
        }
      }
      else if (m.Msg == 537 && this.m_bSerialChecked)
      {
        if (m.WParam.ToInt32() == 32768)
        {
          if (m.LParam != IntPtr.Zero && ((MainForm.DEV_BROADCAST_HDR) Marshal.PtrToStructure(m.LParam, typeof (MainForm.DEV_BROADCAST_HDR))).dbch_DeviceType == 2U)
          {
            MainForm.DEV_BROADCAST_VOLUME structure = (MainForm.DEV_BROADCAST_VOLUME) Marshal.PtrToStructure(m.LParam, typeof (MainForm.DEV_BROADCAST_VOLUME));
            for (int index = 0; index < 26; ++index)
            {
              if (((int) (structure.dbcv_unitmask >> index) & 1) == 1)
                this.drivesHelper.FillDrives();
            }
          }
        }
        else if (m.WParam.ToInt32() == 32772 && m.LParam != IntPtr.Zero && ((MainForm.DEV_BROADCAST_HDR) Marshal.PtrToStructure(m.LParam, typeof (MainForm.DEV_BROADCAST_HDR))).dbch_DeviceType == 2U)
        {
          MainForm.DEV_BROADCAST_VOLUME structure = (MainForm.DEV_BROADCAST_VOLUME) Marshal.PtrToStructure(m.LParam, typeof (MainForm.DEV_BROADCAST_VOLUME));
          for (int index1 = 0; index1 < 26; ++index1)
          {
            if (((int) (structure.dbcv_unitmask >> index1) & 1) == 1)
            {
              for (int index2 = 0; index2 < this.cbDrives.Items.Count; ++index2)
              {
                if (this.cbDrives.Items[index2].ToString() == string.Format("{0}:\\", (object) (char) (65 + index1)))
                  this.cbDrives.Items.RemoveAt(index2);
              }
            }
          }
          if (this.cbDrives.Items.Count == 0 || this.cbDrives.Items[0].ToString() == "")
          {
            this.chkShowAll.Checked = true;
            this.chkShowAll.Enabled = false;
            this.drivesHelper.FillDrives();
          }
          else
            this.cbDrives.SelectedIndex = 0;
        }
      }
      if (m.Msg == 274 && (int) m.WParam == 61728)
        this.Invalidate(true);
      base.WndProc(ref m);
    }

    private int InitSession()
    {
      try
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        string uid = Util.GetUID();
        if (string.IsNullOrEmpty(uid))
        {
          RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Util.GetRegistryBase(), true);
          try
          {
            registryKey.DeleteValue("Hash");
          }
          catch
          {
          }
          int num = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME), PS3SaveEditor.Resources.Resources.msgError);
          this.Close();
          return 0;
        }
        Dictionary<string, object> res = new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"START_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", (object) Util.GetUserId(), (object) uid)))), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
        if (res.ContainsKey("update"))
        {
          Dictionary<string, object> dictionary = res["update"] as Dictionary<string, object>;
          foreach (string key in dictionary.Keys)
          {
            string url = (string) dictionary[key];
            if (url.IndexOf("msi", StringComparison.CurrentCultureIgnoreCase) > 0)
            {
              int num = (int) new UpgradeDownloader(url).ShowDialog();
              this.Close();
              return 0;
            }
          }
        }
        if (res.ContainsKey("token"))
        {
          Util.SetAuthToken(res["token"] as string);
          new Thread(new ParameterizedThreadStart(this.Pinger)).Start((object) (Convert.ToInt32(res["expiry_ts"]) - Convert.ToInt32(res["current_ts"])));
          new Thread(new ParameterizedThreadStart(this.TrafficPoller)).Start();
          this.GetPSNIDInfo();
          this.m_sessionInited = true;
          return 1;
        }
        if (res.ContainsKey("code") && (res["code"].ToString() == "10009" || res["code"].ToString() == "4071"))
          return -1;
        Util.DeleteRegistryValue("User");
        if (res.ContainsKey("code"))
        {
          Util.ShowErrorMessage(res, string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME));
        }
        else
        {
          int num1 = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME));
        }
        this.Close();
        return 0;
      }
      catch (Exception ex)
      {
        if (ex is WebException)
          return -1;
      }
      return -1;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      if (!this.CheckForVersion())
        return;
      if (!this.CheckSerial())
      {
        this.Close();
      }
      else
      {
        this.m_bSerialChecked = true;
        int num1 = this.InitSession();
        if (num1 < 0)
        {
          Util.ChangeServer();
          num1 = this.InitSession();
        }
        if (num1 == 0)
          return;
        if (num1 < 0)
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errServerConnection);
          this.Close();
        }
        else
        {
          GameListDownloader gameListDownloader = new GameListDownloader();
          if (gameListDownloader.ShowDialog() == DialogResult.OK)
          {
            if (this.m_psnIDs.Count != 0)
              ;
            try
            {
              this.FillSavesList(gameListDownloader.GameListXml);
            }
            catch (Exception ex)
            {
              int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInternal, PS3SaveEditor.Resources.Resources.msgError);
              this.Close();
              return;
            }
            if (this.cbDrives.Items.Count == 0 || this.cbDrives.Items[0].ToString() == "")
            {
              this.chkShowAll.Checked = true;
              this.chkShowAll.Enabled = false;
              this.btnHome_Click((object) null, (EventArgs) null);
            }
            else
            {
              this.PrepareLocalSavesMap();
              this.FillLocalSaves((string) null, true);
              this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
              this.btnHome_Click((object) string.Empty, (EventArgs) null);
            }
          }
          else
            this.Close();
          if (!this.isRunning && Util.IsUnixOrMacOSX())
          {
            System.Timers.Timer timer = new System.Timers.Timer();
            this.previousDriveNum = DriveInfo.GetDrives().Length;
            timer.Elapsed += (ElapsedEventHandler) ((s, e2) =>
            {
              DriveInfo[] drives = DriveInfo.GetDrives();
              if (this.previousDriveNum == drives.Length)
                return;
              this.drivesHelper.FillDrives();
              this.previousDriveNum = drives.Length;
              if (this.cbDrives.Items.Count == 0 || this.cbDrives.Items[0].ToString() == "")
              {
                this.chkShowAll.Checked = true;
                this.chkShowAll.Enabled = false;
              }
            });
            timer.Interval = 10000.0;
            timer.Enabled = true;
            this.isRunning = true;
          }
          this.isFirstRunning = false;
        }
      }
    }

    private void TrafficPoller(object ob) => this.evt2 = new AutoResetEvent(false);

    private void Pinger(object tim)
    {
      int num = (int) tim;
      this.evt = new AutoResetEvent(false);
      string format = "{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}";
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
      while (!this.evt.WaitOne((num - 10) * 1000))
      {
        while (true)
        {
          try
          {
            string input = Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format(format, (object) Util.GetUserId(), (object) Util.GetAuthToken()))));
            if (input.Contains("ERROR"))
              return;
            Dictionary<string, object> dictionary = scriptSerializer.Deserialize(input, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
            if (dictionary.ContainsKey("token"))
            {
              Util.SetAuthToken(dictionary["token"] as string);
              break;
            }
            break;
          }
          catch (Exception ex)
          {
            Thread.Sleep(3000);
          }
        }
      }
    }

    private void PrepareLocalSavesMap()
    {
      this.m_dictLocalSaves.Clear();
      if (this.cbDrives.SelectedItem == null)
        return;
      string str = this.cbDrives.SelectedItem.ToString();
      if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str))
        str = string.Format("/Volumes{0}", (object) str);
      else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str))
        str = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str);
      string dataPath = Util.GetDataPath(str);
      if (!Directory.Exists(dataPath))
        return;
      string[] directories = Directory.GetDirectories(dataPath);
      List<string> stringList = new List<string>();
      foreach (string path in directories)
      {
        if (long.TryParse(Path.GetFileName(path), NumberStyles.HexNumber, (IFormatProvider) null, out long _))
        {
          foreach (string directory in Directory.GetDirectories(path))
          {
            if (Path.GetFileName(directory).StartsWith("CUSA") && Directory.GetFiles(directory).Length != 0)
              stringList.AddRange((IEnumerable<string>) Directory.GetFiles(directory, "*.bin"));
          }
        }
      }
      string[] array = stringList.ToArray();
      Array.Sort<string>(array);
      foreach (string save in array)
      {
        string saveId;
        int onlineSaveIndex = this.GetOnlineSaveIndex(save, out saveId);
        if (onlineSaveIndex >= 0)
        {
          this.dgServerGames.Rows.Add();
          game game = game.Copy(this.m_games[onlineSaveIndex]);
          game.id = saveId;
          game.LocalCheatExists = true;
          game.LocalSaveFolder = save;
          if (game.GetTargetGameFolder() == null)
            game.LocalCheatExists = false;
          try
          {
            MainForm.FillLocalCheats(ref game);
          }
          catch (Exception ex)
          {
          }
          if (!this.m_dictLocalSaves.ContainsKey(game.id))
            this.m_dictLocalSaves.Add(game.id, new List<game>()
            {
              game
            });
          else
            this.m_dictLocalSaves[game.id].Add(game);
        }
      }
    }

    private void FillLocalSaves(string expandGame, bool bSortedAsc)
    {
      if (this.m_expandedGame == expandGame)
      {
        expandGame = (string) null;
        this.m_expandedGame = (string) null;
      }
      ((ISupportInitialize) this.dgServerGames).BeginInit();
      this.dgServerGames.Rows.Clear();
      List<string> stringList = new List<string>();
      List<DataGridViewRow> dataGridViewRowList1 = new List<DataGridViewRow>();
      foreach (game game1 in this.m_games)
      {
        foreach (alias allAlias in game1.GetAllAliases(bSortedAsc))
        {
          string str = allAlias.name + " (" + allAlias.id + ")";
          string id = allAlias.id;
          if (this.m_dictLocalSaves.ContainsKey(allAlias.id))
          {
            List<game> dictLocalSave = this.m_dictLocalSaves[id];
            if (stringList.IndexOf(id) < 0)
            {
              stringList.Add(id);
              List<DataGridViewRow> dataGridViewRowList2 = new List<DataGridViewRow>();
              DataGridViewRow dataGridViewRow1 = new DataGridViewRow();
              dataGridViewRow1.CreateCells((DataGridView) this.dgServerGames);
              dataGridViewRow1.Cells[1].Value = (object) allAlias.name;
              if (dictLocalSave.Count == 0)
              {
                game game2 = dictLocalSave[0];
                game2.diskcode = allAlias.diskcode;
                dataGridViewRow1.Tag = (object) game2;
                container targetGameFolder = game2.GetTargetGameFolder();
                dataGridViewRow1.Cells[2].Value = targetGameFolder != null ? (object) targetGameFolder.GetCheatsCount().ToString() : (object) "N/A";
                dataGridViewRow1.Cells[0].ToolTipText = "";
                dataGridViewRow1.Cells[0].Tag = (object) id;
                dataGridViewRow1.Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game2.LocalSaveFolder);
                dataGridViewRow1.Cells[3].Value = (object) id;
                dataGridViewRow1.Cells[6].Value = (object) true;
                dataGridViewRow1.Cells[4].Value = this.GetPSNID(game2);
                if (!this.IsValidPSNID(game2.PSN_ID))
                {
                  dataGridViewRow1.DefaultCellStyle = new DataGridViewCellStyle()
                  {
                    ForeColor = System.Drawing.Color.Gray
                  };
                  dataGridViewRow1.Cells[1].Tag = (object) "U";
                }
              }
              else
              {
                DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
                dataGridViewRow1.Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                {
                  Font = new Font("Arial", Util.ScaleSize(7f))
                });
                dataGridViewRow1.Cells[0].Value = (object) "►";
                dataGridViewRow1.Cells[1].Value = (object) (dataGridViewRow1.Cells[1].Value.ToString() + " (" + allAlias.id + ")");
                dataGridViewCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewRow1.Cells[0].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Cells[2].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Cells[3].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Cells[4].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Tag = (object) dictLocalSave;
                dataGridViewRow1.Cells[6].Value = (object) false;
                if (str == expandGame)
                {
                  dataGridViewRow1.Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                  {
                    Font = new Font("Arial", Util.ScaleSize(7f))
                  });
                  dataGridViewRow1.Cells[0].Value = (object) "▼";
                  dataGridViewRow1.Cells[0].ToolTipText = "";
                  dataGridViewRow1.Cells[1].Value = (object) (allAlias.name + " (" + allAlias.id + ")");
                  dataGridViewRow1.Cells[0].Tag = (object) id;
                  foreach (game game3 in dictLocalSave)
                  {
                    container targetGameFolder = game3.GetTargetGameFolder();
                    if (targetGameFolder != null)
                    {
                      DataGridViewRow dataGridViewRow2 = new DataGridViewRow();
                      dataGridViewRow2.CreateCells((DataGridView) this.dgServerGames);
                      Match match = Regex.Match(Path.GetFileNameWithoutExtension(game3.LocalSaveFolder), targetGameFolder.pfs);
                      dataGridViewRow2.Cells[1].Value = match.Groups == null || match.Groups.Count <= 1 ? (object) ("    " + (targetGameFolder.name ?? Path.GetFileNameWithoutExtension(game3.LocalSaveFolder))) : (object) ("    " + targetGameFolder.name.Replace("${1}", match.Groups[1].Value));
                      game3.diskcode = allAlias.diskcode;
                      dataGridViewRow2.Cells[0].Tag = (object) id;
                      dataGridViewRow2.Tag = (object) game3;
                      dataGridViewRow2.Cells[2].Value = targetGameFolder != null ? (object) targetGameFolder.GetCheatsCount().ToString() : (object) "N/A";
                      dataGridViewRow2.Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game3.LocalSaveFolder);
                      dataGridViewRow2.Cells[3].Value = (object) id;
                      dataGridViewRow2.Cells[6].Value = (object) true;
                      dataGridViewRow2.Cells[4].Value = this.GetPSNID(game3);
                      if (!this.IsValidPSNID(game3.PSN_ID))
                      {
                        dataGridViewRow2.DefaultCellStyle = new DataGridViewCellStyle()
                        {
                          ForeColor = System.Drawing.Color.Gray
                        };
                        dataGridViewRow2.Cells[1].Tag = (object) "U";
                      }
                      dataGridViewRowList2.Add(dataGridViewRow2);
                    }
                  }
                  this.m_expandedGame = expandGame;
                }
              }
              dataGridViewRowList1.Add(dataGridViewRow1);
              dataGridViewRowList1.AddRange((IEnumerable<DataGridViewRow>) dataGridViewRowList2.ToArray());
            }
          }
        }
      }
      this.dgServerGames.Rows.AddRange(dataGridViewRowList1.ToArray());
      this.FillUnavailableGames();
      this.dgServerGames.ClearSelection();
      ((ISupportInitialize) this.dgServerGames).EndInit();
    }

    private object GetPSNID(game item) => !this.IsValidPSNID(item.PSN_ID) ? (object) (PS3SaveEditor.Resources.Resources.lblUnregistered + " " + item.PSN_ID) : (this.m_psnIDs[item.PSN_ID] as Dictionary<string, object>)["friendly_name"];

    private string GetProfileKey(string sfoPath, Dictionary<string, string> mapProfiles)
    {
      if (System.IO.File.Exists(sfoPath))
      {
        int profileId;
        string base64String = Convert.ToBase64String(MainForm.GetParamInfo(sfoPath, out profileId));
        string key1 = profileId.ToString() + ":" + base64String + ":" + Convert.ToBase64String(Util.GetPSNId(Path.GetDirectoryName(sfoPath)));
        if (mapProfiles.ContainsKey(key1))
          return mapProfiles[key1];
        string key2 = profileId.ToString() + ":" + base64String;
        if (mapProfiles.ContainsKey(key2))
          return mapProfiles[key2];
      }
      return "";
    }

    private bool CheckSerial() => Util.GetRegistryValue("User") != null || new SerialValidateGG().ShowDialog((IWin32Window) this) == DialogResult.OK;

    private void SetLabels()
    {
      this.picTraffic.BackgroundImageLayout = ImageLayout.None;
      this.picVersion.BackgroundImageLayout = ImageLayout.None;
      this.picVersion.Visible = false;
      this.pictureBox2.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.company;
      this.pictureBox2.BackgroundImageLayout = ImageLayout.None;
      this.panel1.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.sel_drive;
      this.lblNoSaves.Text = PS3SaveEditor.Resources.Resources.lblNoSaves;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.btnGamesInServer.Text = PS3SaveEditor.Resources.Resources.btnViewAllCheats;
      this.btnApply.Text = PS3SaveEditor.Resources.Resources.btnApply;
      this.btnBrowse.Text = PS3SaveEditor.Resources.Resources.btnBrowse;
      this.chkBackup.Text = PS3SaveEditor.Resources.Resources.chkBackupSaves;
      this.lblBackup.Text = PS3SaveEditor.Resources.Resources.gbBackupLocation;
      this.dgServerGames.Columns[0].HeaderText = "";
      this.dgServerGames.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colGameName;
      this.dgServerGames.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.dgServerGames.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colCheats;
      this.dgServerGames.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colGameCode;
      this.dgServerGames.Columns[4].HeaderText = PS3SaveEditor.Resources.Resources.colProfile;
      this.dgServerGames.Columns[3].Visible = false;
      this.btnRss.Text = PS3SaveEditor.Resources.Resources.btnRss;
      this.btnDeactivate.Text = PS3SaveEditor.Resources.Resources.btnDeactivate;
      this.simpleToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuSimple;
      this.advancedToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuAdvanced;
      this.deleteSaveToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuDeleteSave;
      this.resignToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuResign;
      this.registerPSNIDToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRegisterPSN;
      this.restoreFromBackupToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRestore;
      this.Text = Util.PRODUCT_NAME;
      this.btnOpenFolder.Text = PS3SaveEditor.Resources.Resources.btnOpenFolder;
      this.lblDeactivate.Text = PS3SaveEditor.Resources.Resources.lblDeactivate;
      this.lblRSSSection.Text = PS3SaveEditor.Resources.Resources.lblRSSSection;
      this.btnManageProfiles.Text = PS3SaveEditor.Resources.Resources.btnUserAccount;
      this.lblManageProfiles.Text = PS3SaveEditor.Resources.Resources.lblUserAccount;
      this.panel3.BackgroundImageLayout = ImageLayout.Tile;
    }

    private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
      CultureInfo selectedItem = this.cbLanguage.SelectedItem as CultureInfo;
      Util.SetRegistryValue("Language", selectedItem.Name);
      Thread.CurrentThread.CurrentUICulture = selectedItem;
      this.SetLabels();
    }

    internal static void FillLocalCheats(ref game item)
    {
      string str1 = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm.USER_CHEATS_FILE;
      if (!System.IO.File.Exists(str1))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(str1);
      for (int i1 = 0; i1 < xmlDocument["usercheats"].ChildNodes.Count; ++i1)
      {
        container targetGameFolder = item.GetTargetGameFolder();
        if (targetGameFolder != null && item.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[i1].Attributes["id"].Value && xmlDocument["usercheats"].ChildNodes[i1].ChildNodes.Count > 0)
        {
          for (int i2 = 0; i2 < xmlDocument["usercheats"].ChildNodes[i1].ChildNodes.Count; ++i2)
          {
            XmlNode childNode1 = xmlDocument["usercheats"].ChildNodes[i1].ChildNodes[i2];
            if ((childNode1 as XmlElement).Name == "file")
            {
              XmlElement xmlElement = childNode1 as XmlElement;
              string attribute = xmlElement.GetAttribute("name");
              file gameFile = item.GetGameFile(targetGameFolder, attribute);
              if (gameFile != null)
              {
                gameFile.ucfilename = attribute;
                for (int index = gameFile.Cheats.Count - 1; index >= 0; --index)
                {
                  if (gameFile.Cheats[index].id == "-1")
                    gameFile.Cheats.Remove(gameFile.Cheats[index]);
                }
                for (int i3 = 0; i3 < xmlElement.ChildNodes.Count; ++i3)
                {
                  XmlNode childNode2 = xmlElement.ChildNodes[i3];
                  cheat cheat = new cheat("-1", childNode2.Attributes["desc"].Value, childNode2.Attributes["comment"].Value);
                  for (int i4 = 0; i4 < childNode2.ChildNodes.Count; ++i4)
                  {
                    string str2 = childNode2.ChildNodes[i4].InnerText.Replace("\r\n", " ").TrimEnd().Replace("\n", " ").TrimEnd();
                    if (str2.Split(' ').Length % 2 == 0)
                      cheat.code = str2;
                  }
                  gameFile?.Cheats.Add(cheat);
                }
              }
            }
            else
            {
              cheat cheat = new cheat("-1", xmlDocument["usercheats"].ChildNodes[i1].ChildNodes[i2].Attributes["desc"].Value, xmlDocument["usercheats"].ChildNodes[i1].ChildNodes[i2].Attributes["comment"].Value);
              for (int i5 = 0; i5 < xmlDocument["usercheats"].ChildNodes[i1].ChildNodes[i2].ChildNodes.Count; ++i5)
              {
                string str3 = xmlDocument["usercheats"].ChildNodes[i1].ChildNodes[i2].ChildNodes[i5].InnerText.Replace("\r\n", " ").TrimEnd().Replace("\n", " ").TrimEnd();
                if (str3.Split(' ').Length == 2)
                  cheat.code = str3;
              }
              if (!string.IsNullOrEmpty(cheat.code) && targetGameFolder != null)
                targetGameFolder.files._files[0].Cheats.Add(cheat);
            }
          }
        }
      }
    }

    private void FillServerGamesList()
    {
      this.dgServerGames.Rows.Clear();
      foreach (game game in this.m_games)
      {
        int index = this.dgServerGames.Rows.Add(new DataGridViewRow());
        this.dgServerGames.Rows[index].Cells[1].Value = (object) game.name;
        this.dgServerGames.Rows[index].Cells[2].Value = (object) game.GetCheatCount();
        this.dgServerGames.Rows[index].Cells[3].Value = (object) game.id;
      }
    }

    private void FillUnavailableGames()
    {
      if (this.cbDrives.SelectedItem == null)
        return;
      string str1 = this.cbDrives.SelectedItem.ToString();
      if (!Directory.Exists(str1 + "PS4\\SAVEDATA"))
        return;
      string[] directories = Directory.GetDirectories(str1 + "PS4\\SAVEDATA");
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      foreach (string str2 in directories)
      {
        if (this.GetOnlineSaveIndex(str2, out string _) == -1)
        {
          string str3 = str2 + Path.DirectorySeparatorChar.ToString() + "PARAM.SFO";
          if (System.IO.File.Exists(str3))
          {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.CreateCells((DataGridView) this.dgServerGames);
            System.Drawing.Color lightSlateGray = System.Drawing.Color.LightSlateGray;
            dataGridViewRow.Cells[0].ToolTipText = PS3SaveEditor.Resources.Resources.msgUnsupported;
            dataGridViewRow.Cells[1].ToolTipText = PS3SaveEditor.Resources.Resources.msgUnsupported;
            dataGridViewRow.Cells[2].ToolTipText = PS3SaveEditor.Resources.Resources.msgUnsupported;
            dataGridViewRow.Cells[3].ToolTipText = PS3SaveEditor.Resources.Resources.msgUnsupported;
            dataGridViewRow.Cells[0].Style.BackColor = lightSlateGray;
            dataGridViewRow.Cells[1].Style.BackColor = lightSlateGray;
            dataGridViewRow.Cells[2].Style.BackColor = lightSlateGray;
            dataGridViewRow.Cells[3].Style.BackColor = lightSlateGray;
            dataGridViewRow.Cells[4].Style.BackColor = lightSlateGray;
            dataGridViewRow.Cells[1].Value = (object) this.GetSaveTitle(str3);
            dataGridViewRow.Cells[3].Value = (object) Path.GetFileName(str2).Substring(0, 9);
            dataGridViewRow.Cells[0].Tag = dataGridViewRow.Cells[3].Value;
            dataGridViewRow.Cells[4].Value = (object) "";
            dataGridViewRow.Tag = (object) str2;
            dataGridViewRowList.Add(dataGridViewRow);
          }
        }
      }
      this.dgServerGames.Rows.AddRange(dataGridViewRowList.ToArray());
    }

    private void dgServerGames_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex < 0 || e.Button != MouseButtons.Right)
        return;
      this.dgServerGames.ClearSelection();
      this.dgServerGames.Rows[e.RowIndex].Selected = true;
    }

    private void SortGames(int sortCol, bool bDesc)
    {
      this.m_games.Sort((Comparison<game>) ((item1, item2) =>
      {
        switch (sortCol)
        {
          case 2:
            return item1.GetCheatCount().CompareTo(item2.GetCheatCount());
          case 3:
            return item1.id.CompareTo(item2.id);
          case 7:
            return string.Compare(item1.diskcode, item2.diskcode);
          default:
            return (item1.name + item1.id).CompareTo(item2.name + item2.id);
        }
      }));
      if (!bDesc)
        return;
      this.m_games.Reverse();
    }

    private void FillSavesList(string xml)
    {
      this.m_games = new List<game>();
      new XmlDocument().PreserveWhitespace = true;
      try
      {
        using (StringReader stringReader = new StringReader(xml))
          this.m_games = ((games) new XmlSerializer(typeof (games)).Deserialize((TextReader) stringReader))._games;
      }
      catch (Exception ex1)
      {
        try
        {
          xml = xml.Replace("&", "&amp;");
          using (StringReader stringReader = new StringReader(xml))
            this.m_games = ((games) new XmlSerializer(typeof (games)).Deserialize((TextReader) stringReader))._games;
        }
        catch (Exception ex2)
        {
          return;
        }
      }
      this.m_games.Sort((Comparison<game>) ((item1, item2) => (item1.name + item1.LocalSaveFolder).CompareTo(item2.name + item1.LocalSaveFolder)));
    }

    private int GetPSNIDInfo()
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"PSNID_INFO\",\"userid\":\"{0}\"}}", (object) Util.GetUserId())))), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      if (!dictionary.ContainsKey("status") || !((string) dictionary["status"] == "OK"))
        return 0;
      this.m_psnIDs = !dictionary.ContainsKey("psnid") ? new Dictionary<string, object>() : dictionary["psnid"] as Dictionary<string, object>;
      this.m_psn_quota = Convert.ToInt32(dictionary["psnid_quota"]);
      this.m_psn_remaining = Convert.ToInt32(dictionary["psnid_remaining"]);
      this.gbProfiles.Controls.Clear();
      this.gbProfiles.Width = this.m_psn_quota * 18 + 35;
      for (int index = 0; index < this.m_psn_quota; ++index)
      {
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = index >= this.m_psn_quota - this.m_psn_remaining ? (Image) PS3SaveEditor.Resources.Resources.uncheck : (Image) PS3SaveEditor.Resources.Resources.check;
        pictureBox.Left = 8 + index * 18;
        pictureBox.Top = 8;
        pictureBox.Width = 18;
        this.gbProfiles.Controls.Add((Control) pictureBox);
      }
      TextBox textBox = new TextBox();
      textBox.Text = string.Format("{0}/{1}", (object) (this.m_psn_quota - this.m_psn_remaining), (object) this.m_psn_quota);
      textBox.Left = this.m_psn_quota * 18 + 8;
      textBox.Top = 9;
      textBox.Width = 26;
      textBox.ForeColor = System.Drawing.Color.White;
      textBox.BorderStyle = BorderStyle.None;
      textBox.BackColor = System.Drawing.Color.FromArgb(102, 132, 162);
      this.gbProfiles.Controls.Add((Control) textBox);
      return this.m_psn_quota;
    }

    public bool IsValidPSNID(string psnId) => this.m_psnIDs != null && this.m_psnIDs.ContainsKey(psnId);

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.chkShowAll.Checked || this.dgServerGames.SelectedCells.Count == 0 || this.cbDrives.Items.Count == 0)
      {
        e.Cancel = true;
      }
      else
      {
        this.simpleToolStripMenuItem.Visible = true;
        this.advancedToolStripMenuItem.Visible = true;
        int rowIndex = this.dgServerGames.SelectedCells[1].RowIndex;
        if (!(bool) this.dgServerGames.Rows[rowIndex].Cells[6].Value)
          e.Cancel = true;
        if ((string) this.dgServerGames.Rows[rowIndex].Cells[1].Tag == "U")
        {
          this.registerPSNIDToolStripMenuItem.Visible = true;
          this.registerPSNIDToolStripMenuItem.Enabled = true;
          this.simpleToolStripMenuItem.Enabled = false;
          this.advancedToolStripMenuItem.Enabled = false;
          this.restoreFromBackupToolStripMenuItem.Enabled = false;
        }
        else
        {
          this.registerPSNIDToolStripMenuItem.Visible = false;
          this.registerPSNIDToolStripMenuItem.Enabled = false;
          this.restoreFromBackupToolStripMenuItem.Enabled = true;
          if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag4))
          {
            e.Cancel = true;
          }
          else
          {
            container targetGameFolder = tag4.GetTargetGameFolder();
            if (targetGameFolder != null)
            {
              ToolStripMenuItem toolStripMenuItem = this.advancedToolStripMenuItem;
              int? quickmode = targetGameFolder.quickmode;
              int num1 = 0;
              int num2 = (quickmode.GetValueOrDefault() > num1 ? (quickmode.HasValue ? 1 : 0) : 0) != 0 ? 0 : 1;
              toolStripMenuItem.Enabled = num2 != 0;
              this.simpleToolStripMenuItem.Enabled = true;
            }
            else
            {
              this.simpleToolStripMenuItem.Enabled = false;
              this.advancedToolStripMenuItem.Enabled = false;
            }
            this.deleteSaveToolStripMenuItem.Visible = true;
            this.restoreFromBackupToolStripMenuItem.Visible = true;
          }
        }
      }
    }

    private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag) || tag.PSN_ID != null && !this.IsValidPSNID(tag.PSN_ID) || this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[2].Value as string == "N/A")
        return;
      List<string> stringList = (List<string>) null;
      if (!this.chkShowAll.Checked)
      {
        stringList = tag.GetContainerFiles();
        if (stringList == null || stringList.Count < 2)
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoFile, PS3SaveEditor.Resources.Resources.msgError);
          return;
        }
      }
      container targetGameFolder = tag.GetTargetGameFolder();
      int num1;
      if (targetGameFolder != null)
      {
        int? locked = targetGameFolder.locked;
        int num2 = 0;
        num1 = locked.GetValueOrDefault() > num2 ? (locked.HasValue ? 1 : 0) : 0;
      }
      else
        num1 = 0;
      if (num1 != 0 && Util.ShowMessage(PS3SaveEditor.Resources.Resources.errProfileLock, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      int rowIndex = this.dgServerGames.SelectedCells[0].RowIndex;
      List<string> files = new List<string>();
      if (!this.chkShowAll.Checked)
      {
        string str = tag.LocalSaveFolder.Substring(0, tag.LocalSaveFolder.Length - 4);
        tag.ToString(new List<string>());
        Util.GetTempFolder();
        if (targetGameFolder.preprocess == 1)
        {
          stringList.Remove(str);
          AdvancedSaveUploaderForEncrypt uploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(stringList.ToArray(), tag, (string) null, "list");
          if (uploaderForEncrypt.ShowDialog((IWin32Window) this) != DialogResult.Abort && !string.IsNullOrEmpty(uploaderForEncrypt.ListResult))
          {
            foreach (object obj in new JavaScriptSerializer().Deserialize(uploaderForEncrypt.ListResult, typeof (ArrayList)) as ArrayList)
              files.Add((string) obj);
          }
          else
          {
            int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSave);
            return;
          }
        }
      }
      if (Util.IsUnixOrMacOSX())
      {
        SimpleEdit simpleEdit = new SimpleEdit(tag, this.chkShowAll.Checked, files);
        if (simpleEdit.ShowDialog() == DialogResult.OK)
        {
          this.dgServerGames.Rows[rowIndex].Tag = (object) simpleEdit.GameItem;
          this.dgServerGames.Rows[rowIndex].Cells[2].Value = (object) simpleEdit.GameItem.GetCheatCount();
          this.PrepareLocalSavesMap();
          string expandedGame = this.m_expandedGame;
          this.m_expandedGame = (string) null;
          int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
          this.FillLocalSaves(expandedGame, this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
          this.dgServerGames.Rows[Math.Min(rowIndex, this.dgServerGames.Rows.Count - 1)].Selected = true;
          try
          {
            this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
          }
          catch (Exception ex)
          {
          }
        }
        else
        {
          int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
          this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
        }
      }
      else
      {
        SimpleTreeEdit simpleTreeEdit = new SimpleTreeEdit(tag, this.chkShowAll.Checked, files);
        if (simpleTreeEdit.ShowDialog() == DialogResult.OK)
        {
          this.dgServerGames.Rows[rowIndex].Tag = (object) simpleTreeEdit.GameItem;
          this.dgServerGames.Rows[rowIndex].Cells[2].Value = (object) simpleTreeEdit.GameItem.GetCheatCount();
          this.PrepareLocalSavesMap();
          string expandedGame = this.m_expandedGame;
          this.m_expandedGame = (string) null;
          int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
          this.FillLocalSaves(expandedGame, this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
          this.dgServerGames.Rows[Math.Min(rowIndex, this.dgServerGames.Rows.Count - 1)].Selected = true;
          try
          {
            this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
          }
          catch (Exception ex)
          {
          }
        }
        else
        {
          int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
          this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
        }
      }
    }

    private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgServerGames.SelectedCells.Count == 0)
        return;
      Util.ClearTemp();
      string str1 = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value as string;
      string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
      game tag = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
      List<string> containerFiles = tag.GetContainerFiles();
      if (containerFiles.Count < 2)
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoFile, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        int? locked = tag.GetTargetGameFolder().locked;
        int num2 = 0;
        if (locked.GetValueOrDefault() > num2 && locked.HasValue && Util.ShowMessage(PS3SaveEditor.Resources.Resources.errProfileLock, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
        string str2 = tag.LocalSaveFolder.Substring(0, tag.LocalSaveFolder.Length - 4);
        tag.ToString(new List<string>());
        containerFiles.Remove(str2);
        AdvancedSaveUploaderForEncrypt uploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(containerFiles.ToArray(), tag, (string) null, "decrypt");
        if (uploaderForEncrypt.ShowDialog() == DialogResult.Abort || uploaderForEncrypt.DecryptedSaveData == null || uploaderForEncrypt.DecryptedSaveData.Count <= 0 || new AdvancedEdit(tag, uploaderForEncrypt.DecryptedSaveData).ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbDrives.SelectedItem == null)
        return;
      this.dgServerGames.Columns[0].Width = 25;
      int width = this.dgServerGames.Width;
      this.dgServerGames.Columns[1].Width = (int) ((double) (width - 25) * 0.5);
      this.dgServerGames.Columns[2].Width = (int) ((double) (width - 25) * 0.25);
      this.dgServerGames.Columns[3].Visible = false;
      this.dgServerGames.Columns[4].Width = (int) ((double) (width - 25) * 0.25);
      this.dgServerGames.Columns[4].Visible = true;
      string empty = string.Empty;
      string str1 = this.cbDrives.SelectedItem.ToString();
      string str2;
      if (str1 == PS3SaveEditor.Resources.Resources.colSelect && !this.isFirstRunning && sender != null && ((Control) sender).Focused)
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = !Util.IsUnixOrMacOSX() ? PS3SaveEditor.Resources.Resources.lblSelectCheatsFolder : "Select cheats folder location";
        DialogResult dialogResult = folderBrowserDialog.ShowDialog();
        if (dialogResult != DialogResult.OK && dialogResult != DialogResult.Yes)
          return;
        if (string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.notSelected);
          this.cbDrives_SelectedIndexChanged(sender, e);
          return;
        }
        str2 = Path.GetFullPath(folderBrowserDialog.SelectedPath).Normalize();
        if (!Util.IsPathToCheats(str2))
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgWrongPath);
          this.cbDrives_SelectedIndexChanged(sender, e);
          return;
        }
        this.cbDrives.Items.Clear();
        this.cbDrives.Items.Add((object) PS3SaveEditor.Resources.Resources.colSelect);
        string shortPath = Util.GetShortPath(str2);
        this.cbDrives.SelectedIndex = this.cbDrives.Items.Add((object) shortPath);
        if (!this.chkShowAll.Enabled)
        {
          this.chkShowAll.Enabled = true;
          this.chkShowAll.Checked = false;
        }
        Util.SaveCheatsPathToRegistry(shortPath);
      }
      else
      {
        if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str1))
          str1 = string.Format("/Volumes{0}", (object) str1);
        else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str1))
          str1 = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str1);
        str2 = Util.GetDataPath(str1);
      }
      bool flag = !string.IsNullOrEmpty(str2) && !str2.StartsWith(PS3SaveEditor.Resources.Resources.colSelect);
      if ((!Directory.Exists(str2) || (uint) Directory.GetDirectories(str2).Length <= 0U) && !flag)
      {
        if (!this.chkShowAll.Enabled || string.IsNullOrEmpty(str2) || str2.StartsWith(PS3SaveEditor.Resources.Resources.colSelect))
        {
          this.chkShowAll.Enabled = true;
          this.chkShowAll.Checked = false;
        }
        if (!this.chkShowAll.Checked)
        {
          this.pnlNoSaves.Visible = true;
          this.pnlNoSaves.BringToFront();
        }
      }
      else if (!this.chkShowAll.Checked)
      {
        this.pnlNoSaves.Visible = false;
        this.pnlNoSaves.SendToBack();
        this.PrepareLocalSavesMap();
        this.FillLocalSaves((string) null, true);
        this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
      }
      else
        this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);
      if (this.dgServerGames.Rows.Count == 0 && !this.chkShowAll.Checked)
      {
        this.pnlNoSaves.Visible = true;
        this.pnlNoSaves.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
        this.pnlNoSaves.BringToFront();
      }
      else
      {
        this.pnlNoSaves.Visible = false;
        this.pnlNoSaves.Location = new Point(Util.ScaleSize(-9999), Util.ScaleSize(12));
        this.pnlNoSaves.SendToBack();
      }
    }

    private int GetOnlineSaveIndex(string save, out string saveId)
    {
      string fileName = Path.GetFileName(Path.GetDirectoryName(save));
      string withoutExtension = Path.GetFileNameWithoutExtension(save);
      for (int index1 = 0; index1 < this.m_games.Count; ++index1)
      {
        saveId = this.m_games[index1].id;
        if (fileName.Equals(this.m_games[index1].id) || this.m_games[index1].IsAlias(fileName, out saveId))
        {
          for (int index2 = 0; index2 < this.m_games[index1].containers._containers.Count; ++index2)
          {
            if (withoutExtension == this.m_games[index1].containers._containers[index2].pfs || Util.IsMatch(withoutExtension, this.m_games[index1].containers._containers[index2].pfs))
              return index1;
          }
        }
      }
      saveId = (string) null;
      return -1;
    }

    private int GetOnlineSaveIndexByGameName(string gameName)
    {
      for (int index = 0; index < this.m_games.Count; ++index)
      {
        if (gameName.Equals(this.m_games[index].name))
          return index;
      }
      return -1;
    }

    public static string GetParamInfo(string sfoFile, string item)
    {
      if (!System.IO.File.Exists(sfoFile))
        return "";
      byte[] bytes = System.IO.File.ReadAllBytes(sfoFile);
      int int32_1 = BitConverter.ToInt32(bytes, 8);
      int int32_2 = BitConverter.ToInt32(bytes, 12);
      int int32_3 = BitConverter.ToInt32(bytes, 16);
      int num = 16;
      for (int index = 0; index < int32_3; ++index)
      {
        short int16 = BitConverter.ToInt16(bytes, index * num + 20);
        int int32_4 = BitConverter.ToInt32(bytes, index * num + 12 + 20);
        if (Encoding.UTF8.GetString(bytes, int32_1 + (int) int16, item.Length) == item)
        {
          int count = 0;
          while (count < bytes.Length && bytes[int32_2 + int32_4 + count] != (byte) 0)
            ++count;
          return Encoding.UTF8.GetString(bytes, int32_2 + int32_4, count);
        }
      }
      return "";
    }

    public static byte[] GetParamInfo(string sfoFile, out int profileId)
    {
      profileId = 0;
      if (!System.IO.File.Exists(sfoFile))
        return (byte[]) null;
      byte[] bytes = System.IO.File.ReadAllBytes(sfoFile);
      int int32_1 = BitConverter.ToInt32(bytes, 8);
      int int32_2 = BitConverter.ToInt32(bytes, 12);
      int int32_3 = BitConverter.ToInt32(bytes, 16);
      int num = 16;
      for (int index = 0; index < int32_3; ++index)
      {
        short int16 = BitConverter.ToInt16(bytes, index * num + 20);
        int int32_4 = BitConverter.ToInt32(bytes, index * num + 12 + 20);
        if (Encoding.UTF8.GetString(bytes, int32_1 + (int) int16, 5) == "PARAM")
        {
          byte[] numArray = new byte[16];
          Array.Copy((Array) bytes, int32_2 + int32_4 + 28, (Array) numArray, 0, 16);
          profileId = BitConverter.ToInt32(bytes, int32_2 + int32_4 + 28 + 16);
          return numArray;
        }
      }
      return (byte[]) null;
    }

    private string GetSaveDescription(string sfoFile) => MainForm.GetParamInfo(sfoFile, "SUB_TITLE") + "\r\n" + MainForm.GetParamInfo(sfoFile, "DETAIL");

    private string GetSaveTitle(string sfoFile) => MainForm.GetParamInfo(sfoFile, "TITLE");

    private void btnHome_Click(object sender, EventArgs e)
    {
      this.pnlHome.Visible = true;
      this.pnlBackup.Visible = false;
      this.pnlHome.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlBackup.Location = new Point(Util.ScaleSize(-9999), Util.ScaleSize(15));
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.btnHome.Image = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_on;
        this.btnOptions.Image = (Image) PS3SaveEditor.Resources.Resources.home_settings_off;
        this.btnHelp.Image = (Image) PS3SaveEditor.Resources.Resources.home_help_off;
      }
      else
      {
        this.btnHome.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_on;
        this.btnOptions.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_settings_off;
        this.btnHelp.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_help_off;
      }
      if (sender == null)
        return;
      this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void btnSaves_Click(object sender, EventArgs e)
    {
      this.pnlHome.Visible = false;
      this.pnlBackup.Visible = false;
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = !Util.IsUnixOrMacOSX() ? PS3SaveEditor.Resources.Resources.lblSelectFolder : "Select Backup Folder Location";
      DialogResult dialogResult = folderBrowserDialog.ShowDialog();
      if (dialogResult != DialogResult.OK && dialogResult != DialogResult.Yes)
        return;
      if (string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.notSelected);
        this.btnBrowse_Click(sender, e);
      }
      else
      {
        string fullPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
          if (fullPath.Contains(drive.Name))
          {
            bool flag1;
            if (Util.IsUnixOrMacOSX())
            {
              bool flag2 = drive.Name.Contains("media") || drive.Name.Contains("Volumes");
              flag1 = drive.IsReady && drive.DriveType == DriveType.Removable | flag2;
            }
            else
              flag1 = drive.IsReady && drive.DriveType == DriveType.Removable;
            if (flag1)
            {
              int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.doNotUseRemovable);
              this.btnBrowse_Click(sender, e);
              return;
            }
          }
        }
        this.txtBackupLocation.Text = folderBrowserDialog.SelectedPath;
        this.btnApply_Click((object) null, (EventArgs) null);
      }
    }

    private void chkBackup_CheckedChanged(object sender, EventArgs e)
    {
      this.txtBackupLocation.Enabled = this.chkBackup.Checked;
      this.btnBrowse.Enabled = this.chkBackup.Checked;
      Util.SetRegistryValue("BackupSaves", this.chkBackup.Checked ? "true" : "false");
    }

    private void btnBackup_Click(object sender, EventArgs e)
    {
      this.pnlHome.Visible = false;
      this.pnlBackup.Visible = true;
      this.pnlHome.Location = new Point(Util.ScaleSize(-9999), Util.ScaleSize(15));
      this.pnlBackup.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.btnHome.Image = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_off;
        this.btnOptions.Image = (Image) PS3SaveEditor.Resources.Resources.home_settings_on;
        this.btnHelp.Image = (Image) PS3SaveEditor.Resources.Resources.home_help_off;
      }
      else
      {
        this.btnHome.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_off;
        this.btnOptions.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_settings_on;
        this.btnHelp.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_help_off;
      }
      this.chkBackup.Checked = Util.GetRegistryValue("BackupSaves") != "false";
      this.txtBackupLocation.Text = Util.GetBackupLocation();
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      Util.SetRegistryValue("Location", this.txtBackupLocation.Text);
      Util.SetRegistryValue("BackupSaves", this.chkBackup.Checked ? "true" : "false");
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        if (this.evt == null)
          return;
        this.evt.Set();
        this.evt2.Set();
        Directory.Delete(Util.GetTempFolder(), true);
        if (!this.m_sessionInited)
          return;
        try
        {
          WebClientEx webClientEx = new WebClientEx();
          webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
          webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
          webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth?token=" + Util.GetAuthToken(), Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"DESTROY_SESSION\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", (object) Util.GetUserId(), (object) Util.GetUID())));
        }
        catch (Exception ex)
        {
        }
      }
      catch
      {
      }
    }

    private void SaveUserCheats()
    {
      string str = "<usercheats>";
      foreach (DataGridViewRow row in (IEnumerable) this.dgServerGames.Rows)
      {
        if (row.Tag != null && row.Tag is game tag1 && tag1.GetTargetGameFolder() != null)
        {
          str += string.Format("<game id=\"{0}\">", (object) Path.GetFileName(tag1.LocalSaveFolder));
          foreach (cheat cheat in tag1.GetTargetGameFolder().files._files[0].Cheats)
          {
            if (cheat.id == "-1")
            {
              str = str + "<cheat desc=\"" + cheat.name + "\" comment=\"" + cheat.note + "\">";
              str += cheat.ToString(false);
              str += "</cheat>";
            }
          }
          str += "</game>";
        }
      }
      string contents = str + "</usercheats>";
      System.IO.File.WriteAllText(Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm.USER_CHEATS_FILE, contents);
    }

    private bool CheckForVersion() => true;

    private void btnRss_Click(object sender, EventArgs e)
    {
      try
      {
        int num = (int) new RSSForm(RssFeed.Read(string.Format("{0}/ps4/rss?token={1}", (object) Util.GetBaseUrl(), (object) Util.GetAuthToken())).Channels[0]).ShowDialog();
      }
      catch (Exception ex)
      {
      }
    }

    private void restoreFromBackupToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgServerGames.SelectedRows.Count != 1)
        return;
      game tag = this.dgServerGames.SelectedRows[0].Tag as game;
      string searchPattern = tag.PSN_ID + "_" + Path.GetFileName(Path.GetDirectoryName(tag.LocalSaveFolder)) + "_" + Path.GetFileNameWithoutExtension(tag.LocalSaveFolder) + "_*";
      string[] files = Directory.GetFiles(Util.GetBackupLocation(), searchPattern);
      if (files.Length == 1)
      {
        int num1 = (int) new RestoreBackup(files[0], Path.GetDirectoryName(tag.LocalSaveFolder)).ShowDialog();
        int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgRestored);
      }
      else if (files.Length == 0)
      {
        int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoBackup);
      }
      else
      {
        int num4 = (int) new ChooseBackup(tag.name, tag.PSN_ID + "_" + Path.GetFileName(Path.GetDirectoryName(tag.LocalSaveFolder)) + "_", tag.LocalSaveFolder).ShowDialog();
      }
    }

    private void btnDeactivate_Click(object sender, EventArgs e)
    {
      if (Util.IsUnixOrMacOSX() && !this.btnDeactivate.Enabled)
        return;
      if (Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.msgDeactivate, (object) Util.PRODUCT_NAME), PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) != DialogResult.Yes || !this.DeactivateLicense())
        return;
      int num = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.msgDeactivated, (object) Util.PRODUCT_NAME), PS3SaveEditor.Resources.Resources.msgInfo);
      this.m_sessionInited = false;
      Application.Restart();
    }

    private bool DeactivateLicense()
    {
      try
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
        Dictionary<string, object> res = new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetAuthBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"UNREGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", (object) Util.GetUserId(), (object) Util.GetUID())))), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
        if ((string) res["status"] == "OK")
        {
          RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Util.GetRegistryBase(), true);
          foreach (string valueName in registryKey.GetValueNames())
          {
            if (valueName != "Location")
              registryKey.DeleteValue(valueName);
          }
          return true;
        }
        if (res.ContainsKey("code"))
        {
          Util.ShowErrorMessage(res, PS3SaveEditor.Resources.Resources.errOffline);
        }
        else
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errOffline, PS3SaveEditor.Resources.Resources.msgError);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errConnection, PS3SaveEditor.Resources.Resources.msgError);
      }
      return false;
    }

    private void btnOpenFolder_Click(object sender, EventArgs e) => Process.Start("file://" + this.txtBackupLocation.Text);

    private void btnHelp_Click(object sender, EventArgs e)
    {
      Path.GetDirectoryName(Application.ExecutablePath);
      string str = "http://www.savewizard.net/manuals/swps4m/";
      Process.Start(new ProcessStartInfo()
      {
        UseShellExecute = true,
        Verb = "open",
        FileName = str
      });
    }

    private void MainForm_ResizeEnd(object sender, EventArgs e)
    {
    }

    private string FindGGUSB()
    {
      ManagementScope scope = new ManagementScope("root\\cimv2");
      WqlObjectQuery wqlObjectQuery1 = new WqlObjectQuery("SELECT * FROM Win32_DiskDrive where Model = 'dpdev GameGenie USB Device'");
      ManagementObjectSearcher managementObjectSearcher1 = new ManagementObjectSearcher(scope, (ObjectQuery) wqlObjectQuery1);
      ManagementBaseObject[] objectCollection1 = new ManagementBaseObject[1];
      ManagementObjectCollection objectCollection2 = managementObjectSearcher1.Get();
      if (objectCollection2.Count > 0)
      {
        objectCollection2.CopyTo(objectCollection1, 0);
        string str1 = ((string) objectCollection1[0].Properties["DeviceID"].Value).Replace("\\\\", "\\\\\\\\").Replace(".\\", ".\\\\");
        string[] strArray = objectCollection1[0].Properties["PNPDeviceID"].Value.ToString().Split('\\', '&');
        WqlObjectQuery wqlObjectQuery2 = new WqlObjectQuery("ASSOCIATORS OF {Win32_DiskDrive.DeviceID=\"" + str1 + "\"} WHERE AssocClass = Win32_DiskDriveToDiskPartition");
        ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher(scope, (ObjectQuery) wqlObjectQuery2);
        ManagementObjectCollection objectCollection3 = managementObjectSearcher2.Get();
        if (objectCollection3.Count == 1)
        {
          objectCollection3.CopyTo(objectCollection1, 0);
          WqlObjectQuery wqlObjectQuery3 = new WqlObjectQuery("ASSOCIATORS OF {Win32_DiskPartition.DeviceID=\"" + (string) objectCollection1[0].Properties["DeviceID"].Value + "\"} WHERE AssocClass = Win32_LogicalDiskToPartition");
          ManagementObjectSearcher managementObjectSearcher3 = new ManagementObjectSearcher(scope, (ObjectQuery) wqlObjectQuery3);
          ManagementObjectCollection objectCollection4 = managementObjectSearcher3.Get();
          if (objectCollection4.Count == 1)
          {
            objectCollection4.CopyTo(objectCollection1, 0);
            string str2 = (string) objectCollection1[0].Properties["DeviceID"].Value;
            managementObjectSearcher3.Dispose();
            managementObjectSearcher2.Dispose();
            managementObjectSearcher1.Dispose();
            return strArray[5];
          }
          managementObjectSearcher3.Dispose();
        }
        managementObjectSearcher2.Dispose();
      }
      managementObjectSearcher1.Dispose();
      return (string) null;
    }

    private void deleteSaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string path = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag ? tag.LocalSaveFolder : this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as string;
      if (path == null)
        return;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmDeleteSave, this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      try
      {
        System.IO.File.Delete(path);
        System.IO.File.Delete(path.Substring(0, tag.LocalSaveFolder.Length - 4));
        string directoryName = Path.GetDirectoryName(path);
        if (Directory.GetFiles(directoryName).Length == 0)
        {
          Directory.Delete(directoryName, true);
          string fullName = Directory.GetParent(directoryName).FullName;
          if (Directory.GetFileSystemEntries(fullName).Length == 0)
            Directory.Delete(fullName);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errDelete, PS3SaveEditor.Resources.Resources.msgError);
      }
      int scrollingRowIndex = this.dgServerGames.FirstDisplayedScrollingRowIndex;
      this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      if (this.dgServerGames.Rows.Count > scrollingRowIndex && scrollingRowIndex >= 0)
        this.dgServerGames.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
    }

    private void btnGamesInServer_Click(object sender, EventArgs e)
    {
    }

    private void chkBackup_Click(object sender, EventArgs e)
    {
      if (this.chkBackup.Checked)
        return;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmBackup, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        this.chkBackup.Checked = true;
    }

    private void btnManageProfiles_Click(object sender, EventArgs e)
    {
      if (Util.IsUnixOrMacOSX() && !this.btnManageProfiles.Enabled)
        return;
      int num = (int) new ManageProfiles("", this.m_psnIDs).ShowDialog();
      this.GetPSNIDInfo();
      this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void registerPSNIDToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.m_psnIDs.Count >= this.m_psn_quota || this.m_psn_remaining <= 0)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errMaxProfiles, PS3SaveEditor.Resources.Resources.msgInfo);
      }
      else
      {
        if (this.dgServerGames.SelectedRows.Count != 1 || new ManageProfiles((this.dgServerGames.SelectedRows[0].Tag as game).PSN_ID, this.m_psnIDs).ShowDialog() != DialogResult.OK)
          return;
        this.GetPSNIDInfo();
        this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private void resignToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgServerGames.SelectedCells.Count == 0)
        return;
      game tag = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as game;
      if (this.m_psnIDs.Count == 0)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgNoProfiles);
      }
      else
      {
        ChooseProfile chooseProfile = new ChooseProfile(this.m_psnIDs, tag.PSN_ID);
        if (chooseProfile.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (!System.IO.File.Exists(tag.LocalSaveFolder.Replace(tag.PSN_ID, chooseProfile.SelectedAccount)))
          ;
        if (new ResignFilesUplaoder(tag, Path.GetDirectoryName(tag.LocalSaveFolder), chooseProfile.SelectedAccount, new List<string>()).ShowDialog((IWin32Window) this) == DialogResult.OK)
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    private bool RegisterSerial()
    {
      try
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        string registryValue = Util.GetRegistryValue("Serial");
        string str1 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(registryValue))).Replace("-", "");
        string uid = Util.GetUID(register: true);
        if (string.IsNullOrEmpty(uid))
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errContactSupport);
          return false;
        }
        string uriString = string.Format("{0}/ps4auth", (object) Util.GetAuthBaseUrl(), (object) uid, (object) str1);
        string str2 = webClientEx.DownloadString(new Uri(uriString, UriKind.Absolute));
        if (str2.IndexOf('#') > 0)
        {
          string[] strArray = str2.Split('#');
          if (strArray.Length > 1)
          {
            if (strArray[0] == "4")
            {
              int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSerial, PS3SaveEditor.Resources.Resources.msgError);
              return false;
            }
            if (strArray[0] == "5")
            {
              int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errTooManyTimes, PS3SaveEditor.Resources.Resources.msgError);
              return false;
            }
          }
        }
        else if (str2 == null || str2.ToLower().Contains("error") || str2.ToLower().Contains("not found"))
        {
          string str3 = str2.Replace("ERROR", "");
          if (str3.Contains("1007"))
          {
            Util.GetUID(true, true);
            return this.RegisterSerial();
          }
          if (str3.Contains("1004"))
          {
            int num = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME) + str3, PS3SaveEditor.Resources.Resources.msgError);
            return false;
          }
          if (str3.Contains("1005"))
          {
            int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errTooManyTimes + str3, PS3SaveEditor.Resources.Resources.msgError);
            return false;
          }
          int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNotRegistered, PS3SaveEditor.Resources.Resources.msgError);
          return false;
        }
        return true;
      }
      catch (Exception ex)
      {
        int num2 = (int) Util.ShowMessage(ex.Message, ex.StackTrace);
        int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errSerial, PS3SaveEditor.Resources.Resources.msgError);
      }
      int num4 = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME), PS3SaveEditor.Resources.Resources.msgError);
      return false;
    }

    private void btnActivatePackage_Click(object sender, EventArgs e)
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"ADD_PACKAGE\",\"license\":\"{0}-{1}-{2}-{3}\",\"userid\":\"{4}\"}}", (object) this.txtSerial1.Text, (object) this.txtSerial2.Text, (object) this.txtSerial3.Text, (object) this.txtSerial4.Text, (object) Util.GetUserId())))));
      if (!dictionary.ContainsKey("status") || !((string) dictionary["status"] == "OK"))
        return;
      int num = (int) Util.ShowMessage("Successfully activated the package. Application will restart now.");
      Application.Restart();
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
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.simpleToolStripMenuItem = new ToolStripMenuItem();
      this.advancedToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.resignToolStripMenuItem = new ToolStripMenuItem();
      this.registerPSNIDToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.restoreFromBackupToolStripMenuItem = new ToolStripMenuItem();
      this.deleteSaveToolStripMenuItem = new ToolStripMenuItem();
      this.btnHome = new Button();
      this.btnHelp = new Button();
      this.btnOptions = new Button();
      this.pnlHome = new Panel();
      this.chkShowAll = new CheckBox();
      this.dgServerGames = new CustomDataGridView();
      this.Choose = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.pnlNoSaves = new Panel();
      this.lblNoSaves = new Label();
      this.btnGamesInServer = new Button();
      this.panel1 = new Panel();
      this.cbDrives = new ComboBox();
      this.pnlBackup = new Panel();
      this.groupBox3 = new CustomGroupBox();
      this.gbManageProfile = new CustomGroupBox();
      this.gbProfiles = new CustomGroupBox();
      this.lblManageProfiles = new Label();
      this.btnManageProfiles = new Button();
      this.groupBox2 = new CustomGroupBox();
      this.cbLanguage = new ComboBox();
      this.lblLanguage = new Label();
      this.lblDeactivate = new Label();
      this.btnDeactivate = new Button();
      this.groupBox1 = new CustomGroupBox();
      this.lblRSSSection = new Label();
      this.btnRss = new Button();
      this.gbBackupLocation = new CustomGroupBox();
      this.btnOpenFolder = new Button();
      this.lblBackup = new Label();
      this.btnBrowse = new Button();
      this.txtBackupLocation = new TextBox();
      this.chkBackup = new CheckBox();
      this.btnApply = new Button();
      this.Multi = new DataGridViewTextBoxColumn();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.picContact = new PictureBox();
      this.picVersion = new PictureBox();
      this.pictureBox2 = new PictureBox();
      this.picTraffic = new PictureBox();
      this.label1 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.txtSerial4 = new TextBox();
      this.txtSerial3 = new TextBox();
      this.txtSerial2 = new TextBox();
      this.txtSerial1 = new TextBox();
      this.btnActivatePackage = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.pnlHome.SuspendLayout();
      ((ISupportInitialize) this.dgServerGames).BeginInit();
      this.pnlNoSaves.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlBackup.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.gbManageProfile.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.gbBackupLocation.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.picContact).BeginInit();
      ((ISupportInitialize) this.picVersion).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.picTraffic).BeginInit();
      this.SuspendLayout();
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.simpleToolStripMenuItem,
        (ToolStripItem) this.advancedToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.resignToolStripMenuItem,
        (ToolStripItem) this.registerPSNIDToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.restoreFromBackupToolStripMenuItem,
        (ToolStripItem) this.deleteSaveToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = Util.ScaleSize(new Size(185, 148));
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.simpleToolStripMenuItem.Name = "simpleToolStripMenuItem";
      this.simpleToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.simpleToolStripMenuItem.Font = Util.GetFontForPlatform(this.simpleToolStripMenuItem.Font);
      this.simpleToolStripMenuItem.Text = "Simple";
      this.simpleToolStripMenuItem.Click += new EventHandler(this.simpleToolStripMenuItem_Click);
      this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
      this.advancedToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.advancedToolStripMenuItem.Font = Util.GetFontForPlatform(this.advancedToolStripMenuItem.Font);
      this.advancedToolStripMenuItem.Text = "Advanced";
      this.advancedToolStripMenuItem.Click += new EventHandler(this.advancedToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = Util.ScaleSize(new Size(181, 6));
      this.resignToolStripMenuItem.Name = "resignToolStripMenuItem";
      this.resignToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.resignToolStripMenuItem.Font = Util.GetFontForPlatform(this.resignToolStripMenuItem.Font);
      this.resignToolStripMenuItem.Text = "Re-sign...";
      this.resignToolStripMenuItem.Click += new EventHandler(this.resignToolStripMenuItem_Click);
      this.registerPSNIDToolStripMenuItem.Name = "registerPSNIDToolStripMenuItem";
      this.registerPSNIDToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.registerPSNIDToolStripMenuItem.Font = Util.GetFontForPlatform(this.registerPSNIDToolStripMenuItem.Font);
      this.registerPSNIDToolStripMenuItem.Text = "Register PSN ID...";
      this.registerPSNIDToolStripMenuItem.Click += new EventHandler(this.registerPSNIDToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = Util.ScaleSize(new Size(181, 6));
      this.restoreFromBackupToolStripMenuItem.Name = "restoreFromBackupToolStripMenuItem";
      this.restoreFromBackupToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.restoreFromBackupToolStripMenuItem.Font = Util.GetFontForPlatform(this.restoreFromBackupToolStripMenuItem.Font);
      this.restoreFromBackupToolStripMenuItem.Text = "Restore from Backup";
      this.restoreFromBackupToolStripMenuItem.Click += new EventHandler(this.restoreFromBackupToolStripMenuItem_Click);
      this.deleteSaveToolStripMenuItem.Name = "deleteSaveToolStripMenuItem";
      this.deleteSaveToolStripMenuItem.Size = Util.ScaleSize(new Size(184, 22));
      this.deleteSaveToolStripMenuItem.Font = Util.GetFontForPlatform(this.deleteSaveToolStripMenuItem.Font);
      this.deleteSaveToolStripMenuItem.Text = "Delete Save";
      this.deleteSaveToolStripMenuItem.Click += new EventHandler(this.deleteSaveToolStripMenuItem_Click);
      this.btnHome.BackColor = System.Drawing.Color.Transparent;
      this.btnHome.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnHome.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 215, (int) byte.MaxValue);
      this.btnHome.FlatAppearance.BorderSize = 0;
      this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
      this.btnHome.FlatStyle = FlatStyle.Flat;
      this.btnHome.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(15));
      this.btnHome.Name = "btnHome";
      this.btnHome.Size = Util.ScaleSize(new Size(237, 61));
      this.btnHome.TabIndex = 3;
      this.btnHome.UseVisualStyleBackColor = false;
      this.btnHome.Click += new EventHandler(this.btnHome_Click);
      this.btnHelp.BackColor = System.Drawing.Color.Transparent;
      this.btnHelp.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnHelp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 215, (int) byte.MaxValue);
      this.btnHelp.FlatAppearance.BorderSize = 0;
      this.btnHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
      this.btnHelp.FlatStyle = FlatStyle.Flat;
      this.btnHelp.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(143));
      this.btnHelp.Name = "btnHelp";
      this.btnHelp.Size = Util.ScaleSize(new Size(237, 61));
      this.btnHelp.TabIndex = 6;
      this.btnHelp.UseVisualStyleBackColor = false;
      this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
      this.btnOptions.BackColor = System.Drawing.Color.Transparent;
      this.btnOptions.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnOptions.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 215, (int) byte.MaxValue);
      this.btnOptions.FlatAppearance.BorderSize = 0;
      this.btnOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
      this.btnOptions.FlatStyle = FlatStyle.Flat;
      this.btnOptions.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(79));
      this.btnOptions.Name = "btnOptions";
      this.btnOptions.Size = Util.ScaleSize(new Size(237, 61));
      this.btnOptions.TabIndex = 5;
      this.btnOptions.UseVisualStyleBackColor = false;
      this.btnOptions.Click += new EventHandler(this.btnBackup_Click);
      this.pnlHome.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlHome.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.pnlHome.Controls.Add((Control) this.chkShowAll);
      this.pnlHome.Controls.Add((Control) this.dgServerGames);
      this.pnlHome.Controls.Add((Control) this.pnlNoSaves);
      this.pnlHome.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlHome.Name = "pnlHome";
      this.pnlHome.Size = Util.ScaleSize(new Size(508, 347));
      this.pnlHome.TabIndex = 8;
      this.chkShowAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkShowAll.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(324));
      this.chkShowAll.Name = "chkShowAll";
      this.chkShowAll.Size = Util.ScaleSize(new Size(97, 16));
      this.chkShowAll.TabIndex = 11;
      this.chkShowAll.Text = "Show All";
      this.chkShowAll.UseVisualStyleBackColor = true;
      this.dgServerGames.AllowUserToAddRows = false;
      this.dgServerGames.AllowUserToDeleteRows = false;
      this.dgServerGames.AllowUserToResizeRows = false;
      this.dgServerGames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgServerGames.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgServerGames.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgServerGames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgServerGames.Columns.AddRange((DataGridViewColumn) this.Choose, (DataGridViewColumn) this.dataGridViewTextBoxColumn1, (DataGridViewColumn) this.dataGridViewTextBoxColumn2, (DataGridViewColumn) this.dataGridViewTextBoxColumn3, (DataGridViewColumn) this.dataGridViewTextBoxColumn4, (DataGridViewColumn) this.dataGridViewCheckBoxColumn1, (DataGridViewColumn) this.dataGridViewTextBoxColumn5);
      this.dgServerGames.ContextMenuStrip = this.contextMenuStrip1;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgServerGames.DefaultCellStyle = gridViewCellStyle2;
      this.dgServerGames.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.dgServerGames.Name = "dgServerGames";
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgServerGames.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgServerGames.RowHeadersVisible = false;
      this.dgServerGames.RowHeadersWidth = Util.ScaleSize(25);
      this.dgServerGames.RowTemplate.Height = Util.ScaleSize(24);
      this.dgServerGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgServerGames.Size = Util.ScaleSize(new Size(484, 304));
      this.dgServerGames.TabIndex = 1;
      this.Choose.Frozen = true;
      this.Choose.HeaderText = "Choose";
      this.Choose.Name = "Choose";
      this.Choose.ReadOnly = true;
      this.Choose.Width = Util.ScaleSize(20);
      this.dataGridViewTextBoxColumn1.FillWeight = Util.ScaleSize(20f);
      this.dataGridViewTextBoxColumn1.Frozen = true;
      this.dataGridViewTextBoxColumn1.HeaderText = "Game Name";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = Util.ScaleSize(240);
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.dataGridViewTextBoxColumn2.DefaultCellStyle = gridViewCellStyle4;
      this.dataGridViewTextBoxColumn2.Frozen = true;
      this.dataGridViewTextBoxColumn2.HeaderText = "Cheats";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = Util.ScaleSize(60);
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.dataGridViewTextBoxColumn3.DefaultCellStyle = gridViewCellStyle5;
      this.dataGridViewTextBoxColumn3.HeaderText = "GameCode";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.ReadOnly = true;
      this.dataGridViewTextBoxColumn3.Width = Util.ScaleSize(80);
      this.dataGridViewTextBoxColumn4.HeaderText = "Client";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.ReadOnly = true;
      this.dataGridViewTextBoxColumn4.Width = Util.ScaleSize(80);
      this.dataGridViewCheckBoxColumn1.HeaderText = "Local Save Exist";
      this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
      this.dataGridViewCheckBoxColumn1.ReadOnly = true;
      this.dataGridViewCheckBoxColumn1.Visible = false;
      this.dataGridViewTextBoxColumn5.HeaderText = "Client";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      this.dataGridViewTextBoxColumn5.ReadOnly = true;
      this.dataGridViewTextBoxColumn5.Visible = false;
      this.pnlNoSaves.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNoSaves.Controls.Add((Control) this.lblNoSaves);
      this.pnlNoSaves.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.pnlNoSaves.Name = "pnlNoSaves";
      this.pnlNoSaves.Size = Util.ScaleSize(new Size(485, 311));
      this.pnlNoSaves.TabIndex = 7;
      this.pnlNoSaves.Visible = false;
      this.lblNoSaves.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoSaves.BackColor = System.Drawing.Color.Transparent;
      this.lblNoSaves.ForeColor = System.Drawing.Color.White;
      this.lblNoSaves.Location = new Point(0, Util.ScaleSize(100));
      this.lblNoSaves.Name = "lblNoSaves";
      this.lblNoSaves.Size = Util.ScaleSize(new Size(480, 13));
      this.lblNoSaves.TabIndex = 10;
      this.lblNoSaves.Text = "No PS4 saves were found on this USB drive.";
      this.lblNoSaves.TextAlign = ContentAlignment.MiddleCenter;
      this.btnGamesInServer.Location = new Point(0, 0);
      this.btnGamesInServer.Name = "btnGamesInServer";
      this.btnGamesInServer.Size = Util.ScaleSize(new Size(75, 23));
      this.btnGamesInServer.TabIndex = 0;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel1.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(332));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(237, 30));
      this.panel1.TabIndex = 11;
      this.cbDrives.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbDrives.FormattingEnabled = true;
      this.cbDrives.IntegralHeight = false;
      if (Util.IsUnixOrMacOSX())
      {
        this.cbDrives.Location = new Point(Util.ScaleSize(65), Util.ScaleSize(2));
        this.cbDrives.Width = Util.ScaleSize(165);
      }
      else
      {
        this.cbDrives.Location = new Point(Util.ScaleSize(185), Util.ScaleSize(5));
        this.cbDrives.Width = Util.ScaleSize(45);
      }
      this.cbDrives.Name = "cbDrives";
      this.cbDrives.TabIndex = 3;
      this.panel1.Controls.Add((Control) this.cbDrives);
      this.pnlBackup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBackup.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.pnlBackup.Controls.Add((Control) this.groupBox3);
      this.pnlBackup.Controls.Add((Control) this.gbManageProfile);
      this.pnlBackup.Controls.Add((Control) this.groupBox2);
      this.pnlBackup.Controls.Add((Control) this.groupBox1);
      this.pnlBackup.Controls.Add((Control) this.gbBackupLocation);
      this.pnlBackup.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlBackup.Name = "pnlBackup";
      this.pnlBackup.Size = Util.ScaleSize(new Size(508, 347));
      this.pnlBackup.TabIndex = 9;
      this.groupBox3.Controls.Add((Control) this.btnActivatePackage);
      this.groupBox3.Controls.Add((Control) this.label4);
      this.groupBox3.Controls.Add((Control) this.label3);
      this.groupBox3.Controls.Add((Control) this.label2);
      this.groupBox3.Controls.Add((Control) this.txtSerial4);
      this.groupBox3.Controls.Add((Control) this.txtSerial3);
      this.groupBox3.Controls.Add((Control) this.txtSerial2);
      this.groupBox3.Controls.Add((Control) this.txtSerial1);
      this.groupBox3.Controls.Add((Control) this.label1);
      this.groupBox3.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(276));
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = Util.ScaleSize(new Size(483, 64));
      this.groupBox3.TabIndex = 11;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "groupBox3";
      this.gbManageProfile.Controls.Add((Control) this.gbProfiles);
      this.gbManageProfile.Controls.Add((Control) this.lblManageProfiles);
      this.gbManageProfile.Controls.Add((Control) this.btnManageProfiles);
      this.gbManageProfile.Location = new Point(Util.ScaleSize(251), Util.ScaleSize(200));
      this.gbManageProfile.Name = "gbManageProfile";
      this.gbManageProfile.Size = Util.ScaleSize(new Size(244, 65));
      this.gbManageProfile.TabIndex = 10;
      this.gbManageProfile.TabStop = false;
      if (Util.IsUnixOrMacOSX())
      {
        this.gbProfiles.Location = new Point(Util.ScaleSize(134), Util.ScaleSize(29));
        this.gbProfiles.Size = Util.ScaleSize(new Size(80, 29));
      }
      else
      {
        this.gbProfiles.Location = new Point(Util.ScaleSize(134), Util.ScaleSize(30));
        this.gbProfiles.Size = Util.ScaleSize(new Size(80, 27));
      }
      this.gbProfiles.Name = "gbProfiles";
      this.gbProfiles.TabIndex = 9;
      this.gbProfiles.TabStop = false;
      this.lblManageProfiles.AutoSize = true;
      this.lblManageProfiles.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(12));
      else
        this.lblManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
      this.lblManageProfiles.Name = "lblManageProfiles";
      this.lblManageProfiles.Size = Util.ScaleSize(new Size(106, 13));
      this.lblManageProfiles.TabIndex = 8;
      this.lblManageProfiles.Text = "Manage PS4 Profiles";
      this.btnManageProfiles.AutoSize = true;
      this.btnManageProfiles.ForeColor = System.Drawing.Color.White;
      this.btnManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
      this.btnManageProfiles.Name = "btnManageProfiles";
      this.btnManageProfiles.Size = Util.ScaleSize(new Size(115, 23));
      this.btnManageProfiles.TabIndex = 0;
      this.btnManageProfiles.Text = "Manage Profiles";
      this.btnManageProfiles.UseVisualStyleBackColor = false;
      this.btnManageProfiles.Click += new EventHandler(this.btnManageProfiles_Click);
      this.groupBox2.Controls.Add((Control) this.cbLanguage);
      this.groupBox2.Controls.Add((Control) this.lblLanguage);
      this.groupBox2.Controls.Add((Control) this.lblDeactivate);
      this.groupBox2.Controls.Add((Control) this.btnDeactivate);
      this.groupBox2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(200));
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = Util.ScaleSize(new Size(483, 65));
      this.groupBox2.TabIndex = 9;
      this.groupBox2.TabStop = false;
      this.cbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbLanguage.FormattingEnabled = true;
      if (Util.IsUnixOrMacOSX())
        this.cbLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(32));
      else
        this.cbLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(36));
      this.cbLanguage.Name = "cbLanguage";
      this.cbLanguage.Size = Util.ScaleSize(new Size(142, 21));
      this.cbLanguage.TabIndex = 10;
      this.lblLanguage.AutoSize = true;
      this.lblLanguage.BackColor = System.Drawing.Color.Transparent;
      this.lblLanguage.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblLanguage.Location = new Point(Util.ScaleSize(333), Util.ScaleSize(12));
      else
        this.lblLanguage.Location = new Point(Util.ScaleSize(332), Util.ScaleSize(16));
      this.lblLanguage.Name = "lblLanguage";
      this.lblLanguage.Size = Util.ScaleSize(new Size(55, 13));
      this.lblLanguage.TabIndex = 9;
      this.lblLanguage.Text = "Language";
      this.lblDeactivate.AutoSize = true;
      this.lblDeactivate.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      else
        this.lblDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
      this.lblDeactivate.Name = "lblDeactivate";
      this.lblDeactivate.Size = Util.ScaleSize(new Size(42, 13));
      this.lblDeactivate.TabIndex = 8;
      this.lblDeactivate.Text = "Testing";
      this.btnDeactivate.AutoSize = true;
      this.btnDeactivate.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.btnDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(30));
      else
        this.btnDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(35));
      this.btnDeactivate.Size = Util.ScaleSize(new Size(115, 23));
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.TabIndex = 0;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.UseVisualStyleBackColor = false;
      this.btnDeactivate.Click += new EventHandler(this.btnDeactivate_Click);
      this.groupBox1.Controls.Add((Control) this.lblRSSSection);
      this.groupBox1.Controls.Add((Control) this.btnRss);
      this.groupBox1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(128));
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = Util.ScaleSize(new Size(483, 67));
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.lblRSSSection.AutoSize = true;
      this.lblRSSSection.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblRSSSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      else
        this.lblRSSSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
      this.lblRSSSection.Name = "lblRSSSection";
      this.lblRSSSection.Size = Util.ScaleSize(new Size(295, 13));
      this.lblRSSSection.TabIndex = 6;
      this.lblRSSSection.Text = "Select below button to check the availability of latest version.";
      this.btnRss.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.btnRss.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
      else
        this.btnRss.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(37));
      this.btnRss.Size = Util.ScaleSize(new Size(115, 23));
      this.btnRss.Name = "btnRss";
      this.btnRss.TabIndex = 0;
      this.btnRss.Text = "Update";
      this.btnRss.UseVisualStyleBackColor = false;
      this.btnRss.Click += new EventHandler(this.btnRss_Click);
      this.gbBackupLocation.Controls.Add((Control) this.btnOpenFolder);
      this.gbBackupLocation.Controls.Add((Control) this.lblBackup);
      this.gbBackupLocation.Controls.Add((Control) this.btnBrowse);
      this.gbBackupLocation.Controls.Add((Control) this.txtBackupLocation);
      this.gbBackupLocation.Controls.Add((Control) this.chkBackup);
      this.gbBackupLocation.Controls.Add((Control) this.btnApply);
      this.gbBackupLocation.ForeColor = System.Drawing.Color.White;
      this.gbBackupLocation.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(8));
      this.gbBackupLocation.Margin = new Padding(0);
      this.gbBackupLocation.Name = "gbBackupLocation";
      this.gbBackupLocation.Padding = new Padding(0);
      this.gbBackupLocation.Size = Util.ScaleSize(new Size(483, 115));
      this.gbBackupLocation.TabIndex = 3;
      this.gbBackupLocation.TabStop = false;
      this.btnOpenFolder.ForeColor = System.Drawing.Color.White;
      this.btnOpenFolder.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(85));
      this.btnOpenFolder.Name = "btnOpenFolder";
      this.btnOpenFolder.Size = Util.ScaleSize(new Size(123, 23));
      this.btnOpenFolder.TabIndex = 3;
      this.btnOpenFolder.Text = "Open Folder";
      this.btnOpenFolder.UseVisualStyleBackColor = false;
      this.btnOpenFolder.Click += new EventHandler(this.btnOpenFolder_Click);
      this.lblBackup.AutoSize = true;
      if (Util.IsUnixOrMacOSX())
        this.lblBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(34));
      else
        this.lblBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(40));
      this.lblBackup.Name = "lblBackup";
      this.lblBackup.Size = Util.ScaleSize(new Size(0, 13));
      this.lblBackup.TabIndex = 5;
      this.btnBrowse.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.btnBrowse.Location = new Point(Util.ScaleSize(281), Util.ScaleSize(54));
      else
        this.btnBrowse.Location = new Point(Util.ScaleSize(281), Util.ScaleSize(60));
      this.btnBrowse.Size = Util.ScaleSize(new Size(75, 23));
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.TabIndex = 1;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = false;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      if (Util.IsUnixOrMacOSX())
      {
        this.txtBackupLocation.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(54));
        this.txtBackupLocation.Size = Util.ScaleSize(new Size(264, 15));
      }
      else
      {
        this.txtBackupLocation.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(61));
        this.txtBackupLocation.Size = Util.ScaleSize(new Size(264, 23));
      }
      this.txtBackupLocation.Name = "txtBackupLocation";
      this.txtBackupLocation.TabIndex = 0;
      this.chkBackup.AutoSize = true;
      this.chkBackup.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.chkBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      else
        this.chkBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
      this.chkBackup.Name = "chkBackup";
      this.chkBackup.Size = Util.ScaleSize(new Size(96, 17));
      this.chkBackup.TabIndex = 0;
      this.chkBackup.Text = "Backup Saves";
      this.chkBackup.UseVisualStyleBackColor = true;
      this.chkBackup.CheckedChanged += new EventHandler(this.chkBackup_CheckedChanged);
      this.chkBackup.Click += new EventHandler(this.chkBackup_Click);
      this.btnApply.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.btnApply.ForeColor = System.Drawing.Color.White;
      this.btnApply.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(85));
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = Util.ScaleSize(new Size(75, 23));
      this.btnApply.TabIndex = 2;
      this.btnApply.Text = "Apply";
      this.btnApply.UseVisualStyleBackColor = false;
      this.btnApply.Visible = false;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.Multi.FillWeight = Util.ScaleSize(20f);
      this.Multi.Frozen = true;
      this.Multi.Name = "Multi";
      this.Multi.ReadOnly = true;
      this.Multi.Width = Util.ScaleSize(20);
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.panel2.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
      this.panel2.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(215));
      this.panel2.Name = "panel2";
      this.panel2.Size = Util.ScaleSize(new Size(237, 3));
      this.panel2.TabIndex = Util.ScaleSize(12);
      this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.panel3.BackColor = System.Drawing.Color.FromArgb(0, 56, 115);
      this.panel3.BackgroundImage = (Image) componentResourceManager.GetObject("panel3.BackgroundImage");
      this.panel3.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel3.Controls.Add((Control) this.picContact);
      this.panel3.Controls.Add((Control) this.picVersion);
      this.panel3.Controls.Add((Control) this.pictureBox2);
      this.panel3.Controls.Add((Control) this.picTraffic);
      this.panel3.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(207));
      this.panel3.Name = "panel3";
      this.panel3.Size = Util.ScaleSize(new Size(237, 122));
      this.panel3.TabIndex = 13;
      this.picContact.BackgroundImageLayout = ImageLayout.None;
      this.picContact.Location = new Point(0, Util.ScaleSize(23));
      this.picContact.Name = "picContact";
      this.picContact.Size = Util.ScaleSize(new Size(237, 24));
      this.picContact.TabIndex = 14;
      this.picContact.TabStop = false;
      this.picVersion.BackgroundImageLayout = ImageLayout.None;
      this.picVersion.Location = new Point(0, Util.ScaleSize(23));
      this.picVersion.Name = "picVersion";
      this.picVersion.Size = Util.ScaleSize(new Size(237, 26));
      this.picVersion.TabIndex = 13;
      this.picVersion.TabStop = false;
      this.pictureBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox2.BackgroundImageLayout = ImageLayout.None;
      this.pictureBox2.Location = new Point(0, Util.ScaleSize(78));
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = Util.ScaleSize(new Size(237, 45));
      this.pictureBox2.TabIndex = 12;
      this.pictureBox2.TabStop = false;
      this.picTraffic.BackgroundImageLayout = ImageLayout.None;
      this.picTraffic.Location = new Point(0, 0);
      this.picTraffic.Name = "picTraffic";
      this.picTraffic.Size = Util.ScaleSize(new Size(237, 26));
      this.picTraffic.TabIndex = 11;
      this.picTraffic.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(14));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(86, 13));
      this.label1.TabIndex = 0;
      this.label1.Text = "lblPackageSerial";
      this.label4.AutoSize = true;
      this.label4.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.ForeColor = System.Drawing.Color.White;
      this.label4.Location = new Point(Util.ScaleSize(165), Util.ScaleSize(37));
      this.label4.Name = "label4";
      this.label4.Size = Util.ScaleSize(new Size(11, 13));
      this.label4.TabIndex = 19;
      this.label4.Text = "-";
      this.label3.AutoSize = true;
      this.label3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.ForeColor = System.Drawing.Color.White;
      this.label3.Location = new Point(Util.ScaleSize(110), Util.ScaleSize(37));
      this.label3.Name = "label3";
      this.label3.Size = Util.ScaleSize(new Size(11, 13));
      this.label3.TabIndex = 18;
      this.label3.Text = "-";
      this.label2.AutoSize = true;
      this.label2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new Point(Util.ScaleSize(54), Util.ScaleSize(37));
      this.label2.Name = "label2";
      this.label2.Size = Util.ScaleSize(new Size(11, 13));
      this.label2.TabIndex = 17;
      this.label2.Text = "-";
      this.txtSerial4.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial4.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSerial4.Location = new Point(Util.ScaleSize(179), Util.ScaleSize(34));
      this.txtSerial4.MaxLength = 4;
      this.txtSerial4.Name = "txtSerial4";
      this.txtSerial4.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial4.TabIndex = 16;
      this.txtSerial3.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial3.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSerial3.Location = new Point(Util.ScaleSize(123), Util.ScaleSize(34));
      this.txtSerial3.MaxLength = 4;
      this.txtSerial3.Name = "txtSerial3";
      this.txtSerial3.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial3.TabIndex = 15;
      this.txtSerial2.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial2.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSerial2.Location = new Point(Util.ScaleSize(67), Util.ScaleSize(34));
      this.txtSerial2.MaxLength = 4;
      this.txtSerial2.Name = "txtSerial2";
      this.txtSerial2.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial2.TabIndex = 14;
      this.txtSerial1.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial1.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSerial1.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(34));
      this.txtSerial1.MaxLength = 4;
      this.txtSerial1.Name = "txtSerial1";
      this.txtSerial1.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial1.TabIndex = 13;
      this.btnActivatePackage.Location = new Point(Util.ScaleSize(237), Util.ScaleSize(35));
      this.btnActivatePackage.Name = "btnActivatePackage";
      this.btnActivatePackage.Size = Util.ScaleSize(new Size(55, 23));
      this.btnActivatePackage.TabIndex = 20;
      this.btnActivatePackage.Text = "Activate";
      this.btnActivatePackage.UseVisualStyleBackColor = true;
      this.btnActivatePackage.Click += new EventHandler(this.btnActivatePackage_Click);
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.FromArgb(0, 44, 101);
      this.ClientSize = Util.ScaleSize(new Size(780, 377));
      this.Controls.Add((Control) this.pnlBackup);
      this.Controls.Add((Control) this.pnlHome);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOptions);
      this.Controls.Add((Control) this.btnHome);
      this.Controls.Add((Control) this.btnHelp);
      this.DoubleBuffered = true;
      this.MinimumSize = Util.ScaleSize(new Size(780, 377));
      this.Name = nameof (MainForm);
      this.Text = "PS4 Save Editor";
      this.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
      this.contextMenuStrip1.ResumeLayout(false);
      this.pnlHome.ResumeLayout(false);
      ((ISupportInitialize) this.dgServerGames).EndInit();
      this.pnlNoSaves.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.pnlBackup.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.gbManageProfile.ResumeLayout(false);
      this.gbManageProfile.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.gbBackupLocation.ResumeLayout(false);
      this.gbBackupLocation.PerformLayout();
      this.panel3.ResumeLayout(false);
      ((ISupportInitialize) this.picContact).EndInit();
      ((ISupportInitialize) this.picVersion).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.picTraffic).EndInit();
      this.ResumeLayout(false);
      typeof (Panel).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.pnlHome, new object[1]
      {
        (object) true
      });
      typeof (Panel).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, (Binder) null, (object) this.pnlBackup, new object[1]
      {
        (object) true
      });
    }

    private delegate void GetTrafficDelegate();

    public struct DEV_BROADCAST_HDR
    {
      public uint dbch_Size;
      public uint dbch_DeviceType;
      public uint dbch_Reserved;
    }

    public struct DEV_BROADCAST_VOLUME
    {
      public uint dbch_Size;
      public uint dbch_DeviceType;
      public uint dbch_Reserved;
      public uint dbcv_unitmask;
      public ushort dbcv_flags;
    }
  }
}
