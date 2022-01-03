// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.MainForm3
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using CSUST.Data;
using Ionic.Zip;
using Microsoft.Win32;
using PS3SaveEditor.Diagnostic;
using PS3SaveEditor.SubControls;
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
using System.Windows.Forms.Layout;
using System.Xml;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  public class MainForm3 : Form
  {
    private Dictionary<string, List<game>> m_dictLocalSaves = new Dictionary<string, List<game>>();
    private Dictionary<string, List<game>> m_dictAllLocalSaves = new Dictionary<string, List<game>>();
    private string m_expandedGame = (string) null;
    private string m_expandedGameResign = (string) null;
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
    private const string SESSION_CLOSAL = "{0}/?q=software_auth2/sessionclose&sessionid={1}";
    private const int INTERNAL_VERION_MAJOR = 1;
    private const int INTERNAL_VERION_MINOR = 0;
    private const int PID_JP_FULL_SINGLE = 2;
    private const int PID_JP_FULL_MULTI = 3;
    private const int PID_CYBER_ZERO_ENG = 4;
    private const int PID_SWPS4MAX = 7;
    private const int PID_SE_JP_AMAZON_MULTI = 8;
    private const int PID_SE_JP_AMAZON_SINGLE = 9;
    private const int PID_JP_FULL_SINGLE_DIRECT = 10;
    private const int PID_JP_FULL_MULTI_DIRECT = 11;
    private const int PID_AMAZON_SGE1 = 13;
    private const int PID_CYBER_SGE1 = 15;
    private const int PID_SWPS4US_AMAZON = 16;
    private const int PID_SWPS4US_PAYPAL = 18;
    private const int PID_CYBER_ZERO_JP = 20;
    private const int PID_SWPS4US_RETAIL = 22;
    private const int PID_CYBER_FULL_ENG = 24;
    private MainForm3.GetTrafficDelegate GetTrafficFunc;
    private List<game> m_games;
    private rblsit m_rblist;
    private DrivesHelper drivesHelper;
    private FormWindowState previousState = FormWindowState.Normal;
    public static string USER_CHEATS_FILE = "swusercheats.xml";
    private int previousDriveNum = 0;
    private bool isRunning = false;
    private bool m_bSerialChecked = false;
    private bool m_sessionInited = false;
    private AutoResetEvent evt;
    private AutoResetEvent evt2;
    private Dictionary<string, object> m_psnIDs = (Dictionary<string, object>) null;
    private int m_psn_quota = 0;
    private int m_psn_remaining = 0;
    private static bool isFirstRunning = true;
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
    private ToolStripMenuItem deleteSaveToolStripMenuItem;
    private Button btnGamesInServer;
    private Panel pnlNoSaves;
    private Panel pnlNoSaves2;
    private Label lblNoSaves;
    private Label lblNoSaves2;
    private Button btnOpenFolder;
    private Label lblBackup;
    private Button btnBrowse;
    private TextBox txtBackupLocation;
    private CheckBox chkBackup;
    private Button btnApply;
    private Label lblRSSSection;
    private Button btnRss;
    private Label lblDiagnosticSection;
    private Button btnDiagnostic;
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
    private Panel panel2;
    private CustomGroupBox gbBackupLocation;
    private CustomGroupBox groupBox1;
    private CustomGroupBox groupBox2;
    private CustomGroupBox gbManageProfile;
    private CustomGroupBox gbProfiles;
    private CustomGroupBox diagnosticBox;
    private Panel panel3;
    private PictureBox picTraffic;
    private PictureBox pictureBox2;
    private PictureBox picVersion;
    private ComboBox cbLanguage;
    private Label lblLanguage;
    private ComboBox cbScale;
    private Label lblScale;
    private Panel tabPageGames;
    private CheckBox chkShowAll;
    private CustomDataGridView dgServerGames;
    private DataGridViewTextBoxColumn Choose;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private Panel tabPageResign;
    private CustomDataGridView dgResign;
    private Button btnResign;
    private Button btnCheats;
    private Button btnImport;
    private DataGridViewTextBoxColumn _Head;
    private DataGridViewTextBoxColumn GameID;
    private DataGridViewTextBoxColumn PSNID;
    private DataGridViewTextBoxColumn SysVer;
    private DataGridViewTextBoxColumn Addded;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem resignToolStripMenuItem1;
    private ToolStripMenuItem registerProfileToolStripMenuItem;
    private ToolStripMenuItem deleteSaveToolStripMenuItem1;
    private ToolStripSeparator toolStripSeparator3;

    public MainForm3()
    {
      this.InitializeComponent();
      this.m_games = new List<game>();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.RegionMap = new Dictionary<int, string>();
      this.chkShowAll.CheckedChanged += new EventHandler(this.chkShowAll_CheckedChanged);
      this.chkShowAll.EnabledChanged += new EventHandler(this.chkShowAll_EnabledChanged);
      this.picTraffic.Visible = false;
      this.ResizeBegin += (EventHandler) ((s, e) => this.SuspendLayout());
      this.SizeChanged += (EventHandler) ((s, e) =>
      {
        this.ResumeLayout(true);
        this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);
        this.Invalidate(true);
        if (this.pnlHome.Visible && (this.WindowState == FormWindowState.Maximized || this.previousState == FormWindowState.Maximized))
          this.ResizeColumns(this.chkShowAll.Checked);
        this.previousState = this.WindowState;
      });
      this.ResizeEnd += (EventHandler) ((s, e) =>
      {
        if (this.pnlHome.Visible)
          this.ResizeColumns(this.chkShowAll.Checked);
        this.ResumeLayout(true);
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
      this.btnDiagnostic.BackColor = SystemColors.ButtonFace;
      this.btnRss.ForeColor = System.Drawing.Color.Black;
      this.btnOpenFolder.ForeColor = System.Drawing.Color.Black;
      this.btnBrowse.ForeColor = System.Drawing.Color.Black;
      this.btnDeactivate.ForeColor = System.Drawing.Color.Black;
      this.btnManageProfiles.ForeColor = System.Drawing.Color.Black;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnDiagnostic.ForeColor = System.Drawing.Color.Black;
      this.tabPageGames.BackColor = this.tabPageGames.BackColor = this.tabPageResign.BackColor = this.pnlBackup.BackColor = this.pnlHome.BackColor = this.pnlHome.BackColor = this.pnlNoSaves.BackColor = this.pnlNoSaves2.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.gbBackupLocation.BackColor = this.gbManageProfile.BackColor = this.groupBox1.BackColor = this.groupBox2.BackColor = this.diagnosticBox.BackColor = System.Drawing.Color.Transparent;
      this.chkShowAll.BackColor = System.Drawing.Color.FromArgb(0, 204, 204, 204);
      this.chkShowAll.ForeColor = System.Drawing.Color.White;
      this.panel2.Visible = false;
      this.registerPSNIDToolStripMenuItem.Visible = false;
      this.resignToolStripMenuItem.Visible = false;
      this.toolStripSeparator1.Visible = false;
      this.CenterToScreen();
      this.SetLabels();
      if (Util.IsUnixOrMacOSX())
      {
        if (this.WindowState == FormWindowState.Minimized)
          this.WindowState = FormWindowState.Normal;
        this.Activate();
      }
      else
        Util.SetForegroundWindow(this.Handle);
      this.cbDrives.SelectedIndexChanged += new EventHandler(this.cbDrives_SelectedIndexChanged);
      this.cbScale.SelectedIndexChanged += new EventHandler(this.cbScale_SelectedIndexChanged);
      this.dgServerGames.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgServerGames_CellMouseDown);
      this.dgServerGames.CellDoubleClick += new DataGridViewCellEventHandler(this.dgServerGames_CellDoubleClick);
      this.dgServerGames.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgServerGames_ColumnHeaderMouseClick);
      this.dgServerGames.ShowCellToolTips = true;
      this.panel2.BackgroundImage = (Image) null;
      List<CultureInfo> cultureInfoList = new List<CultureInfo>();
      string registryValue = Util.GetRegistryValue("Language");
      if (Util.IsUnixOrMacOSX())
        this.cbLanguage.DisplayMember = "DisplayName";
      else
        this.cbLanguage.DisplayMember = "NativeName";
      this.cbLanguage.ValueMember = "Name";
      this.cbLanguage.SelectedValueChanged += new EventHandler(this.cbLanguage_SelectedIndexChanged);
      string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "languages");
      if (Directory.Exists(path))
      {
        foreach (string directory in Directory.GetDirectories(path))
        {
          try
          {
            CultureInfo cultureInfo = new CultureInfo(Path.GetFileNameWithoutExtension(directory));
            cultureInfoList.Add(cultureInfo);
          }
          catch
          {
          }
        }
      }
      if (cultureInfoList.Count == 0)
        cultureInfoList.Add(new CultureInfo("en"));
      this.cbLanguage.DataSource = (object) cultureInfoList;
      if (registryValue == null)
      {
        int stringExact = this.cbLanguage.FindStringExact("English");
        this.cbLanguage.SelectedIndex = stringExact < 0 ? 0 : stringExact;
      }
      else
        this.cbLanguage.SelectedValue = (object) registryValue;
      this.dgResign.CellDoubleClick += new DataGridViewCellEventHandler(this.dgResign_CellDoubleClick);
      this.cbDrives.DrawMode = DrawMode.OwnerDrawFixed;
      this.cbDrives.DrawItem += new DrawItemEventHandler(this.cbDrives_DrawItem);
      this.drivesHelper = new DrivesHelper(this.cbDrives, this.m_games, this.chkShowAll, this.pnlNoSaves, this.btnResign, this.btnImport);
      this.drivesHelper.FillDrives();
      this.cbScale.Items.Add((object) "75%");
      this.cbScale.Items.Add((object) "100%");
      this.cbScale.Items.Add((object) "125%");
      this.cbScale.Items.Add((object) "150%");
      this.cbScale.Items.Add((object) "175%");
      this.cbScale.Items.Add((object) "200%");
      this.cbScale.SelectedIndex = Util.ScaleIndex;
      this.panel1.AutoScroll = true;
      this.Load += new EventHandler(this.MainForm_Load);
      this.btnHome.ChangeUICues += new UICuesEventHandler(this.btnHome_ChangeUICues);
      this.dgServerGames.BackgroundColor = System.Drawing.Color.White;
      this.dgResign.BackgroundColor = System.Drawing.Color.White;
      this.dgServerGames.ScrollBars = ScrollBars.Both;
      this.dgResign.ScrollBars = ScrollBars.Both;
      this.dgResign.SortCompare += new DataGridViewSortCompareEventHandler(this.dgResign_SortCompare);
      this.dgResign.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
      this.dgResign.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
      this.btnCheats.Click += new EventHandler(this.btnCheats_Click);
      this.btnResign.Click += new EventHandler(this.btnResign_Click);
      this.btnImport.Visible = this.tabPageResign.Visible = false;
      this.tabPageGames.BackColor = this.tabPageResign.BackColor = this.pnlHome.BackColor = System.Drawing.Color.Transparent;
      this.tabPageGames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabPageResign.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgResign.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.dgServerGames.Columns[7].SortMode = DataGridViewColumnSortMode.Automatic;
      this.dgServerGames.Columns[7].Visible = false;
      this.resignToolStripMenuItem1.Click += new EventHandler(this.resignToolStripMenuItem1_Click);
      this.contextMenuStrip2.Opening += new CancelEventHandler(this.contextMenuStrip2_Opening);
      this.dgServerGames.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dgServerGames_ColumnWidthChanged);
      if (Util.CurrentPlatform != Util.Platform.MacOS)
        return;
      this.Visible = false;
      this.MainForm_Load((object) null, (EventArgs) null);
    }

    private void cbScale_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = ((ListControl) sender).SelectedIndex;
      if (selectedIndex == Util.ScaleIndex)
        return;
      Util.ScaleIndex = selectedIndex;
      Util.SetRegistryValue("SelectedScaleIndex", selectedIndex.ToString());
      Size size = this.Size;
      int width1 = size.Width;
      size = this.ClientSize;
      int width2 = size.Width;
      int num1 = width1 - width2;
      size = this.Size;
      int height1 = size.Height;
      size = this.ClientSize;
      int height2 = size.Height;
      int num2 = height1 - height2;
      this.MinimumSize = new Size(Util.ScaleSize(780) + num1, Util.ScaleSize(377) + num2);
      this.Size = this.MinimumSize;
      this.simpleToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.advancedToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.resignToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.registerPSNIDToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.restoreFromBackupToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.deleteSaveToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.resignToolStripMenuItem1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.registerProfileToolStripMenuItem.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.deleteSaveToolStripMenuItem1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnHome.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(15));
      this.btnHome.Size = Util.ScaleSize(new Size(237, 61));
      this.btnHelp.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(143));
      this.btnHelp.Size = Util.ScaleSize(new Size(237, 61));
      this.btnOptions.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(79));
      this.btnOptions.Size = Util.ScaleSize(new Size(237, 61));
      this.panel3.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(207));
      this.panel3.Size = Util.ScaleSize(new Size(237, 122));
      this.picVersion.Location = new Point(0, Util.ScaleSize(23));
      this.picVersion.Size = Util.ScaleSize(new Size(237, 26));
      this.pictureBox2.Location = new Point(0, 0);
      this.pictureBox2.Size = Util.ScaleSize(new Size(237, 122));
      this.picTraffic.Location = new Point(0, 0);
      this.picTraffic.Size = Util.ScaleSize(new Size(237, 26));
      this.panel1.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(332));
      this.panel1.Size = Util.ScaleSize(new Size(237, 30));
      this.cbDrives.Location = Util.IsUnixOrMacOSX() ? new Point(Util.ScaleSize(65), Util.ScaleSize(2)) : new Point(Util.ScaleSize(65), Util.ScaleSize(5));
      this.cbDrives.Width = Util.ScaleSize(165);
      this.cbDrives.Height = Util.ScaleSize(21);
      this.cbDrives.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlHome.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlHome.Size = Util.ScaleSize(new Size(511, 347));
      this.pnlBackup.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlBackup.Size = Util.ScaleSize(new Size(508, 347));
      this.btnCheats.Location = new Point(Util.ScaleSize(4), 0);
      this.btnCheats.Size = Util.ScaleSize(new Size(75, 23));
      this.btnResign.Location = new Point(Util.ScaleSize(80), 0);
      this.btnResign.Size = Util.ScaleSize(new Size(75, 23));
      this.btnImport.Location = new Point(Util.ScaleSize(437), Util.ScaleSize(-1));
      this.btnImport.Size = Util.ScaleSize(new Size(75, 23));
      this.chkShowAll.Location = new Point(Util.ScaleSize(415), 0);
      this.chkShowAll.Size = Util.ScaleSize(new Size(97, 16));
      this.tabPageGames.Location = new Point(Util.ScaleSize(4), Util.ScaleSize(22));
      this.tabPageGames.Size = Util.ScaleSize(new Size(507, 325));
      this.tabPageResign.Location = new Point(Util.ScaleSize(4), Util.ScaleSize(22));
      this.tabPageResign.Padding = new Padding(3);
      this.tabPageResign.Size = Util.ScaleSize(new Size(508, 325));
      this.lblNoSaves.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(125));
      this.lblNoSaves.Size = Util.ScaleSize(new Size(481, 20));
      this.lblNoSaves2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(125));
      this.lblNoSaves2.Size = Util.ScaleSize(new Size(481, 20));
      this.pnlNoSaves.Location = new Point(Util.ScaleSize(1), 0);
      this.pnlNoSaves.Size = Util.ScaleSize(new Size(506, 325));
      this.pnlNoSaves2.Location = new Point(Util.ScaleSize(1), 0);
      this.pnlNoSaves2.Size = Util.ScaleSize(new Size(506, 325));
      this.tabPageGames.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabPageResign.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCheats.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnResign.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnImport.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkShowAll.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNoSaves.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNoSaves2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNoSaves.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNoSaves2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gbBackupLocation.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(8));
      this.gbBackupLocation.Size = Util.ScaleSize(new Size(483, 115));
      this.groupBox1.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(128));
      this.groupBox1.Size = Util.ScaleSize(new Size(240, 67));
      this.diagnosticBox.Location = new Point(Util.ScaleSize((int) byte.MaxValue), Util.ScaleSize(128));
      this.diagnosticBox.Size = Util.ScaleSize(new Size(240, 67));
      this.groupBox2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(200));
      this.groupBox2.Size = Util.ScaleSize(new Size(483, 65));
      this.gbManageProfile.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(270));
      this.gbManageProfile.Size = Util.ScaleSize(new Size(483, 65));
      this.btnOpenFolder.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(85));
      this.btnOpenFolder.Size = Util.ScaleSize(new Size(123, 23));
      if (Util.IsUnixOrMacOSX())
      {
        this.chkBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
        this.lblBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(34));
        this.txtBackupLocation.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(54));
        this.txtBackupLocation.Size = Util.ScaleSize(new Size(264, 15));
        this.btnBrowse.Location = new Point(Util.ScaleSize(281), Util.ScaleSize(54));
        this.lblRSSSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
        this.btnRss.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
        this.lblDiagnosticSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
        this.btnDiagnostic.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
        this.lblDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
        this.btnDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(30));
        this.lblScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(12));
        this.cbScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(32));
        this.lblLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(12));
        this.cbLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(32));
        this.lblManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(12));
        this.gbProfiles.Location = new Point(Util.ScaleSize(134), Util.ScaleSize(29));
        this.gbProfiles.Size = Util.ScaleSize(new Size(80, 29));
      }
      else
      {
        this.chkBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
        this.lblBackup.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(40));
        this.txtBackupLocation.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(61));
        this.txtBackupLocation.Size = Util.ScaleSize(new Size(264, 23));
        this.btnBrowse.Location = new Point(Util.ScaleSize(281), Util.ScaleSize(60));
        this.lblRSSSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
        this.btnRss.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(37));
        this.lblDiagnosticSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
        this.btnDiagnostic.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(37));
        this.lblDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
        this.btnDeactivate.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(35));
        this.lblScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(16));
        this.cbScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(36));
        this.lblLanguage.Location = new Point(Util.ScaleSize(332), Util.ScaleSize(16));
        this.cbLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(36));
        this.lblManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
        this.gbProfiles.Location = new Point(Util.ScaleSize(134), Util.ScaleSize(30));
        this.gbProfiles.Size = Util.ScaleSize(new Size(80, 27));
      }
      this.chkBackup.Size = Util.ScaleSize(new Size(96, 17));
      this.lblBackup.Size = Util.ScaleSize(new Size(0, 13));
      this.btnBrowse.Size = Util.ScaleSize(new Size(75, 23));
      this.lblRSSSection.Size = Util.ScaleSize(new Size(295, 13));
      this.btnRss.Size = Util.ScaleSize(new Size(115, 23));
      this.lblDiagnosticSection.Size = Util.ScaleSize(new Size(295, 13));
      this.btnDiagnostic.Size = Util.ScaleSize(new Size(115, 23));
      this.lblDeactivate.Size = Util.ScaleSize(new Size(42, 13));
      this.btnDeactivate.Size = Util.ScaleSize(new Size(115, 23));
      this.lblScale.Size = Util.ScaleSize(new Size(55, 13));
      this.cbScale.Size = Util.ScaleSize(new Size(122, 21));
      this.lblLanguage.Size = Util.ScaleSize(new Size(55, 13));
      this.cbLanguage.Size = Util.ScaleSize(new Size(142, 21));
      this.lblManageProfiles.Size = Util.ScaleSize(new Size(106, 13));
      this.btnManageProfiles.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
      this.btnManageProfiles.Size = Util.ScaleSize(new Size(115, 23));
      this.chkBackup.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblBackup.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBackupLocation.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnBrowse.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOpenFolder.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblRSSSection.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnRss.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDiagnosticSection.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnDiagnostic.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDeactivate.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnDeactivate.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblScale.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cbScale.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblLanguage.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cbLanguage.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblManageProfiles.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnManageProfiles.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gbProfiles.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.RefreshProfiles();
      for (int index = 0; index < this.dgServerGames.RowCount; ++index)
        this.dgServerGames.Rows[index].Height = Util.ScaleSize(24);
      this.dgServerGames.RowTemplate.Height = Util.ScaleSize(24);
      this.dgServerGames.DefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgServerGames.RowHeadersDefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgServerGames.ColumnHeadersDefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      for (int index = 0; index < this.dgResign.RowCount; ++index)
        this.dgResign.Rows[index].Height = Util.ScaleSize(24);
      this.dgResign.RowTemplate.Height = Util.ScaleSize(24);
      this.dgResign.DefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgResign.RowHeadersDefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgResign.ColumnHeadersDefaultCellStyle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      if (!Util.IsUnixOrMacOSX())
        return;
      foreach (ScrollBar control in (ArrangedElementCollection) this.dgServerGames.Controls)
      {
        if (control.GetType() == typeof (VScrollBar))
        {
          control.Location = new Point(this.dgServerGames.Width - control.Width, control.Location.Y);
          control.Height = this.dgServerGames.Height;
          break;
        }
      }
      foreach (ScrollBar control in (ArrangedElementCollection) this.dgResign.Controls)
      {
        if (control.GetType() == typeof (VScrollBar))
        {
          control.Location = new Point(this.dgResign.Width - control.Width, control.Location.Y);
          control.Height = this.dgResign.Height;
          break;
        }
      }
    }

    private void dgServerGames_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
    }

    private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
    {
      if (this.dgResign.SelectedRows.Count != 1)
        e.Cancel = true;
      else if (!(this.dgResign.SelectedRows[0].Tag is game tag2) || (string) this.dgResign.SelectedRows[0].Cells[1].Tag == "U")
        e.Cancel = true;
      else
        this.registerProfileToolStripMenuItem.Visible = !this.m_psnIDs.ContainsKey(tag2.PSN_ID);
    }

    private void resignToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (this.dgResign.SelectedRows.Count != 1)
        return;
      this.DoResign(this.dgResign.SelectedRows[0].Index);
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Zip Files|*.zip";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        ZipFile zipFile = new ZipFile(openFileDialog.FileName);
        IEnumerator<ZipEntry> enumerator1 = zipFile.GetEnumerator();
        Dictionary<ZipEntry, ZipEntry> entries = new Dictionary<ZipEntry, ZipEntry>();
        while (enumerator1.MoveNext())
        {
          ZipEntry current = enumerator1.Current;
          string[] strArray = current.FileName.Split('/');
          if (!current.IsDirectory && current.UncompressedSize > 2048L && strArray.Length > 1 && strArray[strArray.Length - 2].StartsWith("CUSA") && zipFile.EntryFileNames.Contains(current.FileName + ".bin"))
          {
            IEnumerator<ZipEntry> enumerator2 = zipFile.SelectEntries(current.FileName + ".bin", Path.GetDirectoryName(current.FileName)).GetEnumerator();
            if (enumerator2 != null && enumerator2.MoveNext())
            {
              string str = strArray[strArray.Length - 2];
              if (this.IsValidForResign(new game()
              {
                id = str,
                containers = new containers()
                {
                  _containers = new List<container>()
                  {
                    new container() { pfs = strArray[strArray.Length - 1] }
                  }
                }
              }))
                entries.Add(current, enumerator2.Current);
            }
          }
        }
        if (entries.Count > 0)
        {
          string str = this.cbDrives.SelectedItem as string;
          if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str))
            str = string.Format("/Volumes{0}", (object) str);
          else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str))
            str = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str);
          if (new Import(this.m_games, entries, zipFile, this.m_psnIDs, str).ShowDialog((IWin32Window) this) == DialogResult.OK)
            this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
        }
        else
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgNoValidSavesInZip);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgInvalidZip);
      }
    }

    private void btnResign_Click(object sender, EventArgs e)
    {
      if (this.tabPageResign.Visible)
        return;
      this.btnImport.Visible = this.tabPageResign.Visible = true;
      this.chkShowAll.Visible = this.tabPageGames.Visible = false;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.btnImport.Size = new Size(Util.ScaleSize(75), Util.ScaleSize(23));
        this.chkShowAll.Size = new Size(0, 0);
        int num = 0;
        int x = 0;
        foreach (ScrollBar control in (ArrangedElementCollection) this.dgServerGames.Controls)
        {
          if (control.GetType() == typeof (VScrollBar))
          {
            num = control.Height;
            x = control.Location.X;
            break;
          }
        }
        foreach (ScrollBar control in (ArrangedElementCollection) this.dgResign.Controls)
        {
          if (control.GetType() == typeof (VScrollBar))
          {
            control.Height = num;
            control.Location = new Point(x, control.Location.Y);
            break;
          }
        }
      }
      this.btnResign.BackColor = System.Drawing.Color.White;
      this.btnCheats.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
      this.dgResign.Focus();
    }

    private void btnCheats_Click(object sender, EventArgs e)
    {
      if (this.tabPageGames.Visible)
        return;
      this.chkShowAll.Visible = this.tabPageGames.Visible = true;
      this.btnImport.Visible = this.tabPageResign.Visible = false;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.chkShowAll.Size = new Size(Util.ScaleSize(97), Util.ScaleSize(16));
        this.btnImport.Size = new Size(0, 0);
      }
      this.btnCheats.BackColor = System.Drawing.Color.White;
      this.btnResign.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
      this.dgServerGames.Focus();
    }

    private void dgResign_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
    {
      if (e.Column.Index == 1)
      {
        string str1 = e.CellValue1 as string;
        string str2 = e.CellValue2 as string;
        if (str1.IndexOf("    ") >= 0)
        {
          game tag = this.dgResign.Rows[e.RowIndex1].Tag as game;
          str1 = string.IsNullOrEmpty(tag.name) ? tag.id : tag.name + " (" + tag.id + ")";
        }
        if (str2.IndexOf("    ") >= 0)
        {
          game tag = this.dgResign.Rows[e.RowIndex2].Tag as game;
          str2 = string.IsNullOrEmpty(tag.name) ? tag.id : tag.name + " (" + tag.id + ")";
        }
        string[] strArray1 = str1.Split(new string[1]
        {
          " ("
        }, StringSplitOptions.None);
        string[] strArray2 = str2.Split(new string[1]
        {
          " ("
        }, StringSplitOptions.None);
        if (str1 == str2)
        {
          if ((e.CellValue1 as string).IndexOf("    ") >= 0 && (e.CellValue2 as string).IndexOf("    ") >= 0)
          {
            e.SortResult = (e.CellValue1 as string).CompareTo(e.CellValue2 as string);
          }
          else
          {
            if ((e.CellValue1 as string).IndexOf("    ") >= 0)
              e.SortResult = this.dgResign.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Ascending ? 1 : -1;
            if ((e.CellValue2 as string).IndexOf("    ") >= 0)
              e.SortResult = this.dgResign.Columns[1].HeaderCell.SortGlyphDirection == SortOrder.Ascending ? -1 : 1;
          }
          e.Handled = true;
        }
        else
        {
          if (strArray1.Length >= 2 && strArray2.Length >= 2)
            e.SortResult = !(strArray1[0] == strArray2[0]) ? strArray1[0].CompareTo(strArray2[0]) : strArray1[1].CompareTo(strArray2[1]);
          else if (strArray1.Length >= 2 && strArray2.Length == 1)
            e.SortResult = strArray1[0].CompareTo("ZZZZ");
          else if (strArray2.Length >= 2 && strArray1.Length == 1)
            e.SortResult = "ZZZZ".CompareTo(strArray2[0]);
          else if (strArray1.Length == 1 && strArray2.Length == 1)
            e.SortResult = strArray1[0].CompareTo(strArray2[0]);
          e.Handled = true;
        }
      }
      else
        e.Handled = false;
    }

    private void dgResign_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || this.dgResign.SelectedCells.Count == 0 || this.dgResign.SelectedCells[0].RowIndex < 0)
        return;
      int rowIndex = this.dgResign.SelectedCells[0].RowIndex;
      string str = this.dgResign.Rows[rowIndex].Cells[1].Value as string;
      string toolTipText = this.dgResign.Rows[rowIndex].Cells[1].ToolTipText;
      if (!(this.dgResign.Rows[rowIndex].Tag is game))
      {
        if (!(this.dgResign.Rows[this.dgResign.SelectedCells[0].RowIndex].Tag is List<game>))
        {
          if (!(toolTipText == PS3SaveEditor.Resources.Resources.msgUnsupported))
            return;
          int num = (int) Util.ShowMessage(toolTipText);
        }
        else
        {
          int scrollingRowIndex = this.dgResign.FirstDisplayedScrollingRowIndex;
          bool bSortedAsc = this.dgResign.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending;
          int num = e.RowIndex;
          this.FillResignSaves(this.dgResign.Rows[this.dgResign.SelectedCells[0].RowIndex].Cells[1].Value as string, bSortedAsc);
          if (this.m_expandedGameResign != null)
          {
            foreach (DataGridViewRow row in (IEnumerable) this.dgResign.Rows)
            {
              if (row.Cells[1].Value as string == this.m_expandedGameResign)
              {
                num = row.Index;
                break;
              }
            }
          }
          if (this.dgResign.Rows.Count > e.RowIndex + 1)
          {
            this.dgResign.Rows[num + 1].Selected = true;
            this.dgResign.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
          }
          else
          {
            try
            {
              this.dgResign.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      else
      {
        int scrollingRowIndex = this.dgResign.FirstDisplayedScrollingRowIndex;
        if ((string) this.dgResign.Rows[rowIndex].Cells[1].Tag == "U")
          return;
        this.DoResign(rowIndex);
        try
        {
          this.dgResign.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void DoResign(int index)
    {
      if (this.dgResign.Rows[index].Tag is game tag && tag.LocalSaveFolder != null && !Util.HasWritePermission(tag.LocalSaveFolder))
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errWriteForbidden);
      }
      else
      {
        if (Util.GetRegistryValue("NoResignMessage") == null)
        {
          int num2 = (int) new ResignInfo().ShowDialog((IWin32Window) this);
        }
        System.IO.File.ReadAllBytes(tag.LocalSaveFolder);
        if (this.m_psnIDs.Count == 0)
        {
          int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgNoProfiles);
        }
        else
        {
          ChooseProfile chooseProfile = new ChooseProfile(this.m_psnIDs, tag.PSN_ID);
          if (chooseProfile.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            if (System.IO.File.Exists(tag.LocalSaveFolder.Replace(tag.PSN_ID, chooseProfile.SelectedAccount)) && Util.IsUnixOrMacOSX() && Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmResignOverwrite, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.No)
              return;
            if (new ResignFilesUplaoder(tag, Path.GetDirectoryName(tag.LocalSaveFolder), chooseProfile.SelectedAccount, new List<string>()).ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              ResignMessage resignMessage = new ResignMessage();
              int num4 = (int) resignMessage.ShowDialog((IWin32Window) this);
              if (resignMessage.DeleteExisting)
              {
                System.IO.File.Delete(tag.LocalSaveFolder);
                System.IO.File.Delete(tag.LocalSaveFolder.Substring(0, tag.LocalSaveFolder.Length - 4));
                string directoryName = Path.GetDirectoryName(tag.LocalSaveFolder);
                if (Directory.GetFiles(directoryName).Length == 0)
                  Directory.Delete(directoryName);
              }
              this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
            }
          }
          this.m_expandedGameResign = (string) null;
        }
      }
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
        if (sender != null)
        {
          this.dgServerGames.Rows.Clear();
          this.pnlNoSaves.Visible = false;
          this.pnlNoSaves.SendToBack();
          this.dgServerGames.Columns[0].Visible = false;
          this.dgServerGames.Columns[3].Visible = false;
          this.dgServerGames.Columns[4].Visible = false;
          this.dgServerGames.Columns[7].Visible = true;
          this.dgServerGames.Columns[8].Visible = false;
          this.m_games.Sort((Comparison<game>) ((item1, item2) => item2.acts.CompareTo(item1.acts)));
          this.ShowAllGames();
        }
      }
      else if (sender != null)
      {
        this.dgServerGames.Rows.Clear();
        this.dgServerGames.Columns[0].Visible = true;
        this.dgServerGames.Columns[3].Visible = true;
        this.dgServerGames.Columns[4].Visible = true;
        this.dgServerGames.Columns[7].Visible = false;
        this.dgServerGames.Columns[8].Visible = true;
        this.dgServerGames.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colGameCode;
        this.m_games.Sort((Comparison<game>) ((item1, item2) => (item1.name + item1.LocalSaveFolder).CompareTo(item2.name + item1.LocalSaveFolder)));
        this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      }
      this.dgServerGames.Focus();
    }

    private void ShowAllGames()
    {
      ((ISupportInitialize) this.dgServerGames).BeginInit();
      this.dgServerGames.Rows.Clear();
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      foreach (game game in this.m_games)
      {
        foreach (alias allAlias in game.GetAllAliases(distinct: true))
        {
          if (!(game.name == allAlias.name) || !(game.id != allAlias.id))
          {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();
            dataGridViewRow.CreateCells((DataGridView) this.dgServerGames);
            dataGridViewRow.Tag = (object) game;
            dataGridViewRow.Height = Util.ScaleSize(24);
            dataGridViewRow.Cells[1].Value = (object) allAlias.name;
            dataGridViewRow.Cells[2].Value = (object) game.GetCheatCount();
            dataGridViewRow.Cells[7].Value = game.acts != 0 ? (object) new DateTime(1970, 1, 1).AddSeconds((double) game.acts).ToString("yyyy-MM-dd") : (object) "";
            string exregions = "";
            string region1 = Util.GetRegion(this.RegionMap, game.region, exregions);
            List<string> stringList = new List<string>();
            if (game.name == allAlias.name)
              stringList.Add(game.id);
            if (game.aliases != null && game.aliases._aliases.Count > 0)
            {
              foreach (alias alias in game.aliases._aliases)
              {
                if (!(alias.name != allAlias.name))
                {
                  string region2 = Util.GetRegion(this.RegionMap, alias.region, region1);
                  if (region1.IndexOf(region2) < 0)
                    region1 += region2;
                  stringList.Add(alias.id);
                }
              }
            }
            stringList.Sort();
            dataGridViewRow.Cells[3].Value = (object) region1;
            dataGridViewRow.Cells[1].ToolTipText = string.Format(PS3SaveEditor.Resources.Resources.tootlTipSupported, (object) string.Join(",", stringList.ToArray()));
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
      string str = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value as string;
      string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
      if (this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag && tag.LocalSaveFolder != null && !Util.HasWritePermission(tag.LocalSaveFolder))
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errWriteForbidden);
      }
      else if (tag == null)
      {
        if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is List<game>))
        {
          if (!(toolTipText == PS3SaveEditor.Resources.Resources.msgUnsupported))
            return;
          int num2 = (int) Util.ShowMessage(toolTipText);
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

    [DllImport("user32.dll")]
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
          if (m.LParam != IntPtr.Zero && ((MainForm3.DEV_BROADCAST_HDR) Marshal.PtrToStructure(m.LParam, typeof (MainForm3.DEV_BROADCAST_HDR))).dbch_DeviceType == 2U)
          {
            MainForm3.DEV_BROADCAST_VOLUME structure = (MainForm3.DEV_BROADCAST_VOLUME) Marshal.PtrToStructure(m.LParam, typeof (MainForm3.DEV_BROADCAST_VOLUME));
            for (int index = 0; index < 26; ++index)
            {
              if (((int) (structure.dbcv_unitmask >> index) & 1) == 1)
                this.drivesHelper.FillDrives();
            }
          }
        }
        else if (m.WParam.ToInt32() == 32772 && m.LParam != IntPtr.Zero && ((MainForm3.DEV_BROADCAST_HDR) Marshal.PtrToStructure(m.LParam, typeof (MainForm3.DEV_BROADCAST_HDR))).dbch_DeviceType == 2U)
        {
          MainForm3.DEV_BROADCAST_VOLUME structure = (MainForm3.DEV_BROADCAST_VOLUME) Marshal.PtrToStructure(m.LParam, typeof (MainForm3.DEV_BROADCAST_VOLUME));
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
            this.dgResign.Rows.Clear();
            this.chkShowAll.Checked = true;
            this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = false;
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

    private int InitSession(int attempt)
    {
      WaitingForm waitingForm = new WaitingForm(!string.IsNullOrEmpty(Util.forceServer) ? string.Format("Trying Server from arguments - {0}", (object) Util.forceServer) : string.Format("Trying Random Server {0}", (object) attempt));
      waitingForm.Start();
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
        if (res.ContainsKey("pid") && res.ContainsKey("cid"))
        {
          int int32 = Convert.ToInt32(res["pid"]);
          Convert.ToInt32(res["cid"]);
          Util.pid = int32;
          if (int32 == 16 || int32 == 18 || int32 == 22)
          {
            this.pictureBox2.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.logo_swps4us;
            this.lblLanguage.Visible = false;
            this.cbLanguage.Visible = false;
            Util.PRODUCT_NAME = "Save Wizard for PS4";
            Util.IsHyperkinMode = true;
            this.Text = Util.PRODUCT_NAME;
          }
        }
        if (res.ContainsKey("update"))
        {
          int num = (int) Util.ShowMessage(string.Format("{0} has been upgraded. Please download a BETA version from https://savewizard.net/beta/", (object) Util.PRODUCT_NAME));
          this.Close();
          return -2;
        }
        if (res.ContainsKey("token"))
        {
          if (res.ContainsKey("minfsize"))
            Util.SetMinFileSize(Convert.ToInt32(res["minfsize"]));
          if (res.ContainsKey("maxfsize"))
            Util.SetMaxFileSize(Convert.ToInt32(res["maxfsize"]));
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
        return res.ContainsKey("code") && (res["code"].ToString() == "4041" || res["code"].ToString() == "4045" || res["code"].ToString() == "4005") ? -2 : 0;
      }
      catch (Exception ex)
      {
      }
      finally
      {
        waitingForm.Stop();
      }
      return -1;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      if (!Util.IsUnixOrMacOSX())
      {
        IntPtr systemMenu = MainForm3.GetSystemMenu(this.Handle, false);
        MainForm3.InsertMenu(systemMenu, 5, 3072, 0, string.Empty);
        MainForm3.InsertMenu(systemMenu, 6, 1024, 1000, "About Save Wizard for PS4...");
      }
      if (!this.CheckForVersion())
        return;
      if (new StartupScreen(Util.IsNeedToShowUpdateScreen).ShowDialog((IWin32Window) this) != DialogResult.OK)
        this.Close();
      else if (!this.CheckSerial())
      {
        this.Close();
      }
      else
      {
        this.m_bSerialChecked = true;
        int attempt = 1;
        int num1;
        for (num1 = this.InitSession(attempt); num1 <= 0 && attempt < Util.SERVERS.Length + 1 && string.IsNullOrEmpty(Util.forceServer); num1 = this.InitSession(attempt))
        {
          if (num1 == -2)
          {
            this.Close();
            return;
          }
          ++attempt;
          Util.ChangeServer();
          Thread.Sleep(500);
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
          int num3 = (int) gameListDownloader.ShowDialog();
          if (this.m_psnIDs.Count != 0)
            ;
          try
          {
            this.FillSavesList(gameListDownloader.GameListXml);
          }
          catch (Exception ex)
          {
            int num4 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInternal, PS3SaveEditor.Resources.Resources.msgError);
            this.Close();
            return;
          }
          if (this.cbDrives.Items.Count == 0 || this.cbDrives.Items[0].ToString() == "")
          {
            this.chkShowAll.Checked = true;
            this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = false;
            this.btnHome_Click((object) null, (EventArgs) null);
          }
          else
          {
            this.PrepareLocalSavesMap();
            this.FillLocalSaves((string) null, true);
            this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            this.btnHome_Click((object) string.Empty, (EventArgs) null);
          }
          if (!this.isRunning && Util.IsUnixOrMacOSX())
          {
            System.Timers.Timer timer = new System.Timers.Timer();
            this.previousDriveNum = DriveInfo.GetDrives().Length;
            timer.Elapsed += (ElapsedEventHandler) ((s, e2) =>
            {
              DriveInfo[] drives = DriveInfo.GetDrives();
              if (this.previousDriveNum == drives.Length)
                return;
              this.previousDriveNum = drives.Length;
              this.drivesHelper.FillDrives();
              if (this.cbDrives.Items.Count == 0 || this.cbDrives.Items[0].ToString() == "")
              {
                this.dgResign.Rows.Clear();
                this.chkShowAll.Checked = true;
                this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = false;
              }
            });
            timer.Interval = 10000.0;
            timer.Enabled = true;
            this.isRunning = true;
          }
          MainForm3.isFirstRunning = false;
          this.ShowHideNoSavesPanels();
          if (Util.CurrentPlatform != Util.Platform.MacOS)
            return;
          this.Visible = true;
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
      this.m_dictAllLocalSaves.Clear();
      if (this.cbDrives.SelectedItem == null)
        return;
      string str1 = this.cbDrives.SelectedItem.ToString();
      if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(str1))
        str1 = string.Format("/Volumes{0}", (object) str1);
      else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(str1))
        str1 = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) str1);
      string dataPath = Util.GetDataPath(str1);
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
      foreach (string str2 in array)
      {
        if (System.IO.File.Exists(str2.Substring(0, str2.Length - 4)))
        {
          string saveId;
          int onlineSaveIndex = MainForm3.GetOnlineSaveIndex(this.m_games, str2, out saveId);
          if (onlineSaveIndex >= 0)
          {
            game game = game.Copy(this.m_games[onlineSaveIndex]);
            game.id = saveId;
            game.LocalCheatExists = true;
            game.LocalSaveFolder = str2;
            game.UpdatedTime = this.GetSaveUpdateTime(str2);
            if (game.GetTargetGameFolder() == null)
              game.LocalCheatExists = false;
            try
            {
              MainForm3.FillLocalCheats(ref game);
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
            if (!this.m_dictAllLocalSaves.ContainsKey(game.id))
            {
              List<game> gameList = new List<game>();
              this.m_dictAllLocalSaves.Add(game.id, gameList);
            }
            this.m_dictAllLocalSaves[game.id].Add(game);
          }
          else
          {
            string path = str2.Substring(0, str2.Length - 4);
            if (System.IO.File.Exists(path))
            {
              string fileName = Path.GetFileName(Path.GetDirectoryName(str2));
              game game = new game()
              {
                name = "",
                id = fileName,
                containers = new containers()
                {
                  _containers = new List<container>()
                  {
                    new container()
                    {
                      pfs = Path.GetFileName(path).Substring(0, Path.GetFileName(path).Length)
                    }
                  }
                },
                LocalSaveFolder = str2
              };
              if (!this.m_dictAllLocalSaves.ContainsKey(game.id))
              {
                List<game> gameList = new List<game>();
                this.m_dictAllLocalSaves.Add(game.id, gameList);
              }
              this.m_dictAllLocalSaves[game.id].Add(game);
            }
          }
        }
      }
    }

    private DateTime TimeForComapre(game item)
    {
      List<alias> allAliases = item.GetAllAliases();
      if (allAliases.Count == 0 || allAliases[0] == null)
        return item.UpdatedTime;
      foreach (alias alias in allAliases)
      {
        if (this.m_dictLocalSaves.ContainsKey(alias.id))
        {
          List<game> dictLocalSave = this.m_dictLocalSaves[alias.id];
          return dictLocalSave.Count == 0 ? item.UpdatedTime : this.GetSaveUpdateTime(dictLocalSave[0].LocalSaveFolder);
        }
      }
      return item.UpdatedTime;
    }

    private DateTime GetSaveUpdateTime(string save)
    {
      DateTime lastWriteTime = new FileInfo(save).LastWriteTime;
      foreach (FileInfo file in new DirectoryInfo(Path.GetDirectoryName(save)).GetFiles())
      {
        if (file.LastWriteTime > lastWriteTime)
          lastWriteTime = file.LastWriteTime;
      }
      return lastWriteTime;
    }

    private void FillResignSaves(string expandGame, bool bSortedAsc)
    {
      if (this.m_expandedGameResign == expandGame)
      {
        expandGame = (string) null;
        this.m_expandedGameResign = (string) null;
      }
      ((ISupportInitialize) this.dgResign).BeginInit();
      this.dgResign.Rows.Clear();
      List<string> stringList = new List<string>();
      foreach (string key in this.m_dictAllLocalSaves.Keys)
      {
        string id = key;
        game game1 = this.m_games.Find((Predicate<game>) (a => a.id == id)) ?? this.m_dictAllLocalSaves[id][0];
        foreach (alias allAlias in game1.GetAllAliases(bSortedAsc))
        {
          string str1 = allAlias.name + " (" + allAlias.id + ")";
          string id1 = allAlias.id;
          if (this.m_dictAllLocalSaves.ContainsKey(allAlias.id))
          {
            List<game> dictAllLocalSave = this.m_dictAllLocalSaves[id1];
            if (stringList.IndexOf(id1) < 0)
            {
              stringList.Add(id1);
              int index1 = this.dgResign.Rows.Add();
              this.dgResign.Rows[index1].Cells[1].Value = (object) allAlias.name;
              if (dictAllLocalSave.Count == 0)
              {
                game game2 = dictAllLocalSave[0];
                this.dgResign.Rows[index1].Tag = (object) game2;
                container targetGameFolder = game2.GetTargetGameFolder();
                if (targetGameFolder != null)
                  this.dgResign.Rows[index1].Cells[2].Value = (object) targetGameFolder.GetCheatsCount();
                else
                  this.dgResign.Rows[index1].Cells[2].Value = (object) "N/A";
                this.dgResign.Rows[index1].Cells[0].ToolTipText = "";
                this.dgResign.Rows[index1].Cells[0].Tag = (object) id1;
                this.dgResign.Rows[index1].Cells[2].Value = this.GetPSNID(game2);
                if (!this.IsValidForResign(game2))
                {
                  this.dgResign.Rows[index1].DefaultCellStyle = new DataGridViewCellStyle()
                  {
                    ForeColor = System.Drawing.Color.Gray
                  };
                  this.dgResign.Rows[index1].Cells[1].Tag = (object) "U";
                }
              }
              else
              {
                DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
                this.dgResign.Rows[index1].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                {
                  Font = new Font("Arial", Util.ScaleSize(7f))
                });
                this.dgResign.Rows[index1].Cells[0].Value = (object) "►";
                string str2 = this.dgResign.Rows[index1].Cells[1].Value as string;
                this.dgResign.Rows[index1].Cells[1].Value = string.IsNullOrEmpty(str2) ? (object) allAlias.id : (object) (str2 + " (" + allAlias.id + ")");
                dataGridViewCellStyle.BackColor = System.Drawing.Color.White;
                this.dgResign.Rows[index1].Cells[0].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgResign.Rows[index1].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgResign.Rows[index1].Cells[2].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgResign.Rows[index1].Tag = (object) dictAllLocalSave;
                if (!this.IsValidForResign(game1))
                {
                  this.dgResign.Rows[index1].DefaultCellStyle = new DataGridViewCellStyle()
                  {
                    ForeColor = System.Drawing.Color.Gray
                  };
                  this.dgResign.Rows[index1].Cells[1].Tag = (object) "U";
                }
                if (str1 == expandGame || allAlias.id == expandGame)
                {
                  this.dgResign.Rows[index1].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                  {
                    Font = new Font("Arial", Util.ScaleSize(7f))
                  });
                  this.dgResign.Rows[index1].Cells[0].Value = (object) "▼";
                  this.dgResign.Rows[index1].Cells[0].ToolTipText = "";
                  this.dgResign.Rows[index1].Cells[1].Value = string.IsNullOrEmpty(str2) ? (object) allAlias.id : (object) (str2 + " (" + allAlias.id + ")");
                  this.dgResign.Rows[index1].Cells[0].Tag = (object) id1;
                  foreach (game game3 in dictAllLocalSave)
                  {
                    container targetGameFolder = game3.GetTargetGameFolder();
                    if (targetGameFolder != null)
                    {
                      int index2 = this.dgResign.Rows.Add();
                      Match match = Regex.Match(Path.GetFileNameWithoutExtension(game3.LocalSaveFolder), targetGameFolder.pfs);
                      if (targetGameFolder.name != null && match.Groups != null && match.Groups.Count > 1)
                        this.dgResign.Rows[index2].Cells[1].Value = (object) ("    " + targetGameFolder.name.Replace("${1}", match.Groups[1].Value));
                      else
                        this.dgResign.Rows[index2].Cells[1].Value = (object) ("    " + (targetGameFolder.name ?? Path.GetFileNameWithoutExtension(game3.LocalSaveFolder)));
                      this.dgResign.Rows[index2].Cells[0].Tag = (object) id1;
                      game3.name = allAlias.name;
                      this.dgResign.Rows[index2].Tag = (object) game3;
                      this.dgResign.Rows[index2].Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game3.LocalSaveFolder);
                      this.dgResign.Rows[index2].Cells[3].Value = this.GetPSNID(game3);
                      string sysVer = MainForm3.GetSysVer(game3.LocalSaveFolder);
                      this.dgResign.Rows[index2].Cells[2].Value = (object) sysVer;
                      string str3 = "";
                      string str4 = sysVer;
                      if (!(str4 == "?"))
                      {
                        if (!(str4 == "All"))
                        {
                          if (!(str4 == "4.50+"))
                          {
                            if (!(str4 == "4.70+"))
                            {
                              if (!(str4 == "5.00"))
                              {
                                if (str4 == "5.50")
                                  str3 = PS3SaveEditor.Resources.Resources.tooltipV5;
                              }
                              else
                                str3 = PS3SaveEditor.Resources.Resources.tooltipV4;
                            }
                            else
                              str3 = PS3SaveEditor.Resources.Resources.tooltipV3;
                          }
                          else
                            str3 = PS3SaveEditor.Resources.Resources.tooltipV2;
                        }
                        else
                          str3 = PS3SaveEditor.Resources.Resources.tooltipV1;
                      }
                      else
                        str3 = PS3SaveEditor.Resources.Resources.msgUnknownSysVer;
                      this.dgResign.Rows[index2].Cells[2].ToolTipText = str3;
                      if (!this.IsValidForResign(game3))
                      {
                        this.dgResign.Rows[index2].DefaultCellStyle = new DataGridViewCellStyle()
                        {
                          ForeColor = System.Drawing.Color.Gray
                        };
                        this.dgResign.Rows[index2].Cells[1].Tag = (object) "U";
                      }
                    }
                  }
                  this.m_expandedGameResign = expandGame;
                }
              }
            }
          }
        }
      }
      if (!Util.IsUnixOrMacOSX())
        this.dgResign.Sort(this.dgResign.Columns[1], !bSortedAsc ? ListSortDirection.Descending : ListSortDirection.Ascending);
      this.dgResign.ClearSelection();
      ((ISupportInitialize) this.dgResign).EndInit();
    }

    internal static string GetSysVer(string binFile) => MainForm3.GetSysVer(System.IO.File.ReadAllBytes(binFile));

    internal static string GetSysVer(byte[] buf)
    {
      if (buf.Length <= 8)
        return "?";
      switch (buf[8])
      {
        case 1:
          return "All";
        case 2:
          return "4.50+";
        case 3:
          return "4.70+";
        case 4:
          return "5.00+";
        case 5:
          return "5.50+";
        case 6:
          return "6.00+";
        case 7:
          return "6.50+";
        case 8:
          return "7.00+";
        case 9:
          return "7.50+";
        case 10:
          return "8.00+";
        default:
          return "?";
      }
    }

    private bool IsValidForResign(game item)
    {
      if (this.m_rblist == null)
        return false;
      foreach (rbgame rbgame in this.m_rblist._rbgames)
      {
        if (rbgame.gamecode == item.id)
        {
          if (rbgame.containers == null || rbgame.containers.container == null || rbgame.containers.container.Count == 0)
            return false;
          if (item.LocalSaveFolder != null)
          {
            foreach (string pattern in rbgame.containers.container)
            {
              if (Util.IsMatch(Path.GetFileNameWithoutExtension(item.LocalSaveFolder), pattern))
                return false;
            }
          }
        }
      }
      return true;
    }

    private void FillLocalSaves(string expandGame, bool bSortedAsc)
    {
      if (this.m_expandedGame == expandGame)
      {
        expandGame = (string) null;
        this.m_expandedGame = (string) null;
      }
      this.dgServerGames.Rows.Clear();
      List<string> stringList = new List<string>();
      List<DataGridViewRow> dataGridViewRowList1 = new List<DataGridViewRow>();
      ((ISupportInitialize) this.dgServerGames).BeginInit();
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
              dataGridViewRow1.Height = Util.ScaleSize(24);
              dataGridViewRow1.Cells[1].Value = (object) allAlias.name;
              if (dictLocalSave.Count == 0)
              {
                game game2 = dictLocalSave[0];
                dataGridViewRow1.Tag = (object) game2;
                container targetGameFolder = game2.GetTargetGameFolder();
                dataGridViewRow1.Cells[2].Value = targetGameFolder != null ? (object) targetGameFolder.GetCheatsCount().ToString() : (object) "N/A";
                dataGridViewRow1.Cells[0].ToolTipText = "";
                dataGridViewRow1.Cells[0].Tag = (object) id;
                dataGridViewRow1.Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game2.LocalSaveFolder);
                dataGridViewRow1.Cells[3].Value = (object) id;
                dataGridViewRow1.Cells[5].Value = (object) true;
                dataGridViewRow1.Cells[8].Value = (object) game2.UpdatedTime;
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
                dataGridViewRow1.Cells[7].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Cells[8].Style.ApplyStyle(dataGridViewCellStyle);
                dataGridViewRow1.Tag = (object) dictLocalSave;
                dataGridViewRow1.Cells[5].Value = (object) false;
                if (str == expandGame)
                {
                  dataGridViewRow1.Cells[0].Tag = (object) id;
                  dataGridViewRow1.Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                  {
                    Font = new Font("Arial", Util.ScaleSize(7f))
                  });
                  dataGridViewRow1.Cells[0].Value = (object) "▼";
                  dataGridViewRow1.Cells[0].ToolTipText = "";
                  dataGridViewRow1.Cells[1].Value = (object) (allAlias.name + " (" + allAlias.id + ")");
                  foreach (game game3 in dictLocalSave)
                  {
                    container targetGameFolder = game3.GetTargetGameFolder();
                    if (targetGameFolder != null)
                    {
                      DataGridViewRow dataGridViewRow2 = new DataGridViewRow();
                      dataGridViewRow2.CreateCells((DataGridView) this.dgServerGames);
                      dataGridViewRow2.Height = Util.ScaleSize(24);
                      Match match = Regex.Match(Path.GetFileNameWithoutExtension(game3.LocalSaveFolder), targetGameFolder.pfs);
                      dataGridViewRow2.Cells[1].Value = targetGameFolder.name == null || match.Groups == null || match.Groups.Count <= 1 ? (object) ("    " + (targetGameFolder.name ?? Path.GetFileNameWithoutExtension(game3.LocalSaveFolder))) : (object) ("    " + targetGameFolder.name.Replace("${1}", match.Groups[1].Value));
                      dataGridViewRow2.Cells[0].Tag = (object) id;
                      dataGridViewRow2.Tag = (object) game3;
                      dataGridViewRow2.Cells[2].Value = targetGameFolder != null ? (object) targetGameFolder.GetCheatsCount().ToString() : (object) "N/A";
                      dataGridViewRow2.Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game3.LocalSaveFolder);
                      dataGridViewRow2.Cells[3].Value = (object) id;
                      dataGridViewRow2.Cells[5].Value = (object) true;
                      dataGridViewRow2.Cells[8].Value = (object) game3.UpdatedTime;
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
        string base64String = Convert.ToBase64String(MainForm3.GetParamInfo(sfoPath, out profileId));
        string key1 = profileId.ToString() + ":" + base64String + ":" + Convert.ToBase64String(Util.GetPSNId(Path.GetDirectoryName(sfoPath)));
        if (mapProfiles.ContainsKey(key1))
          return mapProfiles[key1];
        string key2 = profileId.ToString() + ":" + base64String;
        if (mapProfiles.ContainsKey(key2))
          return mapProfiles[key2];
      }
      return "";
    }

    private bool CheckSerial() => Util.GetUserId() != null || new SerialValidateGG().ShowDialog((IWin32Window) this) == DialogResult.OK;

    private void SetLabels()
    {
      this.picVersion.BackgroundImageLayout = ImageLayout.None;
      this.picVersion.Visible = false;
      this.pictureBox2.BackgroundImage = Util.IsHyperkin() ? (Image) PS3SaveEditor.Resources.Resources.logo_swps4us : (Image) PS3SaveEditor.Resources.Resources.logo;
      this.panel1.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.sel_drive;
      this.lblNoSaves.Text = PS3SaveEditor.Resources.Resources.lblNoSaves;
      this.lblNoSaves2.Text = PS3SaveEditor.Resources.Resources.lblNoSaves;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.btnGamesInServer.Text = PS3SaveEditor.Resources.Resources.btnViewAllCheats;
      this.btnApply.Text = PS3SaveEditor.Resources.Resources.btnApply;
      this.btnBrowse.Text = PS3SaveEditor.Resources.Resources.btnBrowse;
      this.chkBackup.Text = PS3SaveEditor.Resources.Resources.chkBackupSaves;
      this.lblBackup.Text = PS3SaveEditor.Resources.Resources.gbBackupLocation;
      this.dgServerGames.Columns[0].HeaderText = "";
      this.dgServerGames.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colGameName;
      this.dgServerGames.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colCheats;
      this.dgServerGames.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colGameCode;
      this.dgServerGames.Columns[4].HeaderText = PS3SaveEditor.Resources.Resources.colProfile;
      this.dgServerGames.Columns[7].HeaderText = PS3SaveEditor.Resources.Resources.colAdded;
      this.dgServerGames.Columns[8].HeaderText = PS3SaveEditor.Resources.Resources.colUpdated;
      this.dgServerGames.Columns[3].Visible = false;
      this.btnRss.Text = PS3SaveEditor.Resources.Resources.btnRss;
      this.btnDeactivate.Text = PS3SaveEditor.Resources.Resources.btnDeactivate;
      this.btnDiagnostic.Text = PS3SaveEditor.Resources.Resources.btnDiagnostic;
      this.simpleToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuSimple;
      this.advancedToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuAdvanced;
      this.deleteSaveToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuDeleteSave;
      this.resignToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuResign;
      this.registerPSNIDToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRegisterPSN;
      this.restoreFromBackupToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRestore;
      this.Text = Util.PRODUCT_NAME;
      this.Text = this.Text + " - " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
      this.btnOpenFolder.Text = PS3SaveEditor.Resources.Resources.btnOpenFolder;
      this.lblDeactivate.Text = PS3SaveEditor.Resources.Resources.lblDeactivate;
      this.lblRSSSection.Text = PS3SaveEditor.Resources.Resources.lblRSSSection;
      this.btnManageProfiles.Text = PS3SaveEditor.Resources.Resources.btnUserAccount;
      this.lblManageProfiles.Text = PS3SaveEditor.Resources.Resources.lblUserAccount;
      this.lblDiagnosticSection.Text = PS3SaveEditor.Resources.Resources.lblDiagnosticSection;
      if (Util.IsHyperkin())
      {
        this.lblLanguage.Visible = false;
        this.cbLanguage.Visible = false;
      }
      this.panel3.BackgroundImageLayout = ImageLayout.Tile;
      this.btnImport.Text = PS3SaveEditor.Resources.Resources.btnImport;
      this.btnCheats.Text = PS3SaveEditor.Resources.Resources.btnCheats;
      this.btnResign.Text = PS3SaveEditor.Resources.Resources.btnResign;
      this.chkShowAll.Text = PS3SaveEditor.Resources.Resources.chkShowAll;
      this.dgResign.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colGameName;
      this.dgResign.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colSysVer;
      this.dgResign.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colProfile;
      this.lblLanguage.Text = PS3SaveEditor.Resources.Resources.lblLanguage;
      this.registerProfileToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuRegisterPSN;
      this.deleteSaveToolStripMenuItem1.Text = PS3SaveEditor.Resources.Resources.mnuDeleteSave;
      if (this.cbDrives.Items.Count <= 0)
        return;
      this.cbDrives.Items[this.cbDrives.Items.Count - 1] = (object) PS3SaveEditor.Resources.Resources.colSelect;
    }

    private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cbLanguage.SelectedItem is CultureInfo selectedItem))
        return;
      Util.SetRegistryValue("Language", selectedItem.Name);
      Thread.CurrentThread.CurrentUICulture = selectedItem;
      this.SetLabels();
      this.Refresh();
      this.btnHome.Invalidate();
      this.btnHelp.Invalidate();
      this.btnOptions.Invalidate();
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
    }

    public static void FillLocalCheats(ref game item)
    {
      string str1 = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm3.USER_CHEATS_FILE;
      if (!System.IO.File.Exists(str1))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.Load(str1);
      }
      catch (Exception ex1)
      {
        string xml = System.IO.File.ReadAllText(str1).Replace("&", "&amp;");
        try
        {
          xmlDocument.LoadXml(xml);
        }
        catch (Exception ex2)
        {
        }
      }
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
                gameFile.ucfilename = attribute;
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
      ((ISupportInitialize) this.dgServerGames).BeginInit();
      this.dgServerGames.Rows.Clear();
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      foreach (game game in this.m_games)
      {
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        dataGridViewRow.CreateCells((DataGridView) this.dgServerGames);
        dataGridViewRow.Cells[1].Value = (object) game.name;
        dataGridViewRow.Cells[2].Value = (object) game.GetCheatCount();
        dataGridViewRow.Cells[3].Value = (object) game.id;
        dataGridViewRowList.Add(dataGridViewRow);
      }
      this.dgServerGames.Rows.AddRange(dataGridViewRowList.ToArray());
      ((ISupportInitialize) this.dgServerGames).EndInit();
    }

    private void FillUnavailableGames()
    {
      if (this.cbDrives.SelectedItem == null)
        return;
      string strBase = this.cbDrives.SelectedItem.ToString();
      switch (Util.CurrentPlatform)
      {
        case Util.Platform.Linux:
          strBase = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) strBase);
          break;
        case Util.Platform.MacOS:
          strBase = string.Format("/Volumes{0}", (object) strBase);
          break;
      }
      string dataPath = Util.GetDataPath(strBase);
      if (!Directory.Exists(dataPath))
        return;
      string[] directories = Directory.GetDirectories(dataPath);
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      foreach (string str1 in directories)
      {
        if (MainForm3.GetOnlineSaveIndex(this.m_games, str1, out string _) == -1)
        {
          string str2 = str1 + Path.DirectorySeparatorChar.ToString() + "PARAM.SFO";
          if (System.IO.File.Exists(str2))
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
            dataGridViewRow.Cells[1].Value = (object) this.GetSaveTitle(str2);
            dataGridViewRow.Cells[3].Value = (object) Path.GetFileName(str1).Substring(0, 9);
            dataGridViewRow.Cells[0].Tag = dataGridViewRow.Cells[3].Value;
            dataGridViewRow.Cells[4].Value = (object) "";
            dataGridViewRow.Tag = (object) str1;
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
      if (sortCol == 8)
      {
        foreach (KeyValuePair<string, List<game>> dictLocalSave in this.m_dictLocalSaves)
        {
          dictLocalSave.Value.Sort((Comparison<game>) ((item1, item2) => item1.UpdatedTime.CompareTo(item2.UpdatedTime)));
          if (bDesc)
            dictLocalSave.Value.Reverse();
        }
      }
      this.m_games.Sort((Comparison<game>) ((item1, item2) =>
      {
        switch (sortCol)
        {
          case 2:
            return item1.GetCheatCount().CompareTo(item2.GetCheatCount());
          case 3:
            return item1.id.CompareTo(item2.id);
          case 7:
            return item1.acts.CompareTo(item2.acts);
          case 8:
            return this.TimeForComapre(item1).CompareTo(this.TimeForComapre(item2));
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
        {
          games games = (games) new XmlSerializer(typeof (games)).Deserialize((TextReader) stringReader);
          this.m_games = games._games;
          this.m_rblist = games.rblist;
        }
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
      this.RefreshProfiles();
      return this.m_psn_quota;
    }

    private void RefreshProfiles()
    {
      this.gbProfiles.Controls.Clear();
      this.gbProfiles.Width = this.m_psn_quota * Util.ScaleSize(18) + Util.ScaleSize(35);
      for (int index = 0; index < this.m_psn_quota; ++index)
      {
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = index >= this.m_psn_quota - this.m_psn_remaining ? (Image) PS3SaveEditor.Resources.Resources.uncheck : (Image) PS3SaveEditor.Resources.Resources.check;
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox.Left = Util.ScaleSize(8) + index * Util.ScaleSize(18);
        pictureBox.Top = Util.ScaleSize(8);
        pictureBox.Size = Util.ScaleSize(new Size(13, 13));
        if (Util.CurrentPlatform == Util.Platform.MacOS)
          pictureBox.MaximumSize = Util.ScaleSize(new Size(18, 35));
        this.gbProfiles.Controls.Add((Control) pictureBox);
      }
      TextBox textBox = new TextBox();
      textBox.Text = string.Format("{0}/{1}", (object) (this.m_psn_quota - this.m_psn_remaining), (object) this.m_psn_quota);
      textBox.Left = this.m_psn_quota * Util.ScaleSize(18) + Util.ScaleSize(8);
      textBox.Top = Util.ScaleSize(9);
      textBox.Width = Util.ScaleSize(26);
      textBox.ForeColor = System.Drawing.Color.White;
      textBox.BorderStyle = BorderStyle.None;
      textBox.BackColor = System.Drawing.Color.FromArgb(102, 132, 162);
      this.gbProfiles.Controls.Add((Control) textBox);
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
        this.advancedToolStripMenuItem.Visible = true;
        int rowIndex = this.dgServerGames.SelectedCells[1].RowIndex;
        if (!(bool) this.dgServerGames.Rows[rowIndex].Cells[5].Value)
          e.Cancel = true;
        else if ((string) this.dgServerGames.Rows[rowIndex].Cells[1].Tag == "U")
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
          if (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag5))
          {
            e.Cancel = true;
          }
          else
          {
            container targetGameFolder = tag5.GetTargetGameFolder();
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
            if (!(MainForm3.GetSysVer(tag5.LocalSaveFolder) == "All"))
              return;
            this.advancedToolStripMenuItem.Enabled = false;
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
        if (!string.IsNullOrEmpty(tag.notes))
        {
          int num3 = (int) new Notes(tag.notes).ShowDialog((IWin32Window) this);
        }
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
            int num4 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSave);
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
        string str = tag.LocalSaveFolder.Substring(0, tag.LocalSaveFolder.Length - 4);
        tag.ToString(new List<string>());
        containerFiles.Remove(str);
        if (!string.IsNullOrEmpty(tag.notes))
        {
          int num3 = (int) new Notes(tag.notes).ShowDialog((IWin32Window) this);
        }
        AdvancedSaveUploaderForEncrypt uploaderForEncrypt = new AdvancedSaveUploaderForEncrypt(containerFiles.ToArray(), tag, (string) null, "decrypt");
        if (uploaderForEncrypt.ShowDialog() == DialogResult.Abort || uploaderForEncrypt.DecryptedSaveData == null || uploaderForEncrypt.DecryptedSaveData.Count <= 0)
          return;
        using (AdvancedEdit advancedEdit = new AdvancedEdit(tag, uploaderForEncrypt.DecryptedSaveData))
        {
          if (advancedEdit.ShowDialog((IWin32Window) this) == DialogResult.OK)
            this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
        }
      }
    }

    private void ResizeColumns(bool showAllChecked)
    {
      int num = this.dgServerGames.Width;
      if (num == 0)
        return;
      this.dgServerGames.Columns[4].Visible = !showAllChecked;
      this.dgServerGames.Columns[8].Visible = !showAllChecked;
      if (showAllChecked)
      {
        this.dgServerGames.Columns[0].Visible = false;
        this.dgServerGames.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      }
      else
      {
        this.dgServerGames.Columns[0].Width = 25;
        num = (this.btnImport.Visible ? this.dgResign.Width : this.dgServerGames.Width) - 25;
        this.dgServerGames.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        this.dgServerGames.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      }
      this.dgResign.Columns[0].Width = 25;
      this.dgResign.Columns[1].Width = (int) ((double) num * 0.600000023841858) >= 5 ? (int) ((double) num * 0.600000023841858) : 5;
      this.dgResign.Columns[2].Width = (int) ((double) num * 0.109999999403954) >= 5 ? (int) ((double) num * 0.109999999403954) : 5;
      this.dgResign.Columns[3].Width = (int) ((double) num * 0.180000007152557) >= 5 ? (int) ((double) num * 0.180000007152557) : 5;
      this.dgResign.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgServerGames.Columns[1].Width = (int) ((double) num * 0.600000023841858) >= 5 ? (int) ((double) num * 0.600000023841858) : 5;
      this.dgServerGames.Columns[2].Width = (int) ((double) num * 0.109999999403954) >= 5 ? (int) ((double) num * 0.109999999403954) : 5;
      this.dgServerGames.Columns[3].Visible = false;
    }

    private void cbDrives_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dgResign.Rows.Clear();
        if (this.cbDrives.SelectedItem == null)
          return;
        this.ResizeColumns(this.chkShowAll.Checked);
        string empty = string.Empty;
        string str1 = this.cbDrives.SelectedItem.ToString();
        string str2;
        if (str1 == PS3SaveEditor.Resources.Resources.colSelect && !MainForm3.isFirstRunning && sender != null && ((Control) sender).Focused)
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
          folderBrowserDialog.Description = !Util.IsUnixOrMacOSX() ? PS3SaveEditor.Resources.Resources.lblSelectCheatsFolder : "Select cheats folder location";
          DialogResult dialogResult = folderBrowserDialog.ShowDialog();
          if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Yes)
          {
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
            string shortPath = Util.GetShortPath(str2);
            if (!this.chkShowAll.Enabled)
            {
              this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = true;
              this.chkShowAll.Checked = false;
            }
            Util.SaveCheatsPathToRegistry(shortPath);
            this.drivesHelper.FillDrives();
            this.cbDrives.SelectedIndex = 0;
          }
          else
          {
            if (!this.chkShowAll.Checked && this.cbDrives.Items.Count < 2)
            {
              this.dgServerGames.Rows.Clear();
              this.ShowHideNoSavesPanels();
            }
            this.cbDrives.SelectedIndex = 0;
            return;
          }
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
            this.btnResign.Enabled = this.btnImport.Enabled = this.chkShowAll.Enabled = true;
            this.chkShowAll.Checked = false;
          }
        }
        else
        {
          if (!this.chkShowAll.Checked)
          {
            this.PrepareLocalSavesMap();
            this.FillLocalSaves((string) null, true);
            this.dgServerGames.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
          }
          else
            this.chkShowAll_CheckedChanged((object) null, (EventArgs) null);
          this.FillResignSaves((string) null, true);
        }
        this.ShowHideNoSavesPanels();
      }
      catch (Exception ex)
      {
        int num = (int) CustomMsgBox.Show(ex.Message);
      }
    }

    private void ShowHideNoSavesPanels()
    {
      if (this.dgServerGames.Rows.Count == 0 && !this.chkShowAll.Checked)
      {
        this.pnlNoSaves.Visible = true;
        this.pnlNoSaves.Location = new Point(Util.ScaleSize(1), 0);
        this.pnlNoSaves.BringToFront();
      }
      else
      {
        this.pnlNoSaves.Visible = false;
        this.pnlNoSaves.Location = new Point(Util.ScaleSize(-9999), 0);
        this.pnlNoSaves.SendToBack();
      }
      if (this.dgResign.Rows.Count == 0)
      {
        this.pnlNoSaves2.Visible = true;
        this.pnlNoSaves2.Location = new Point(Util.ScaleSize(1), 0);
        this.pnlNoSaves2.BringToFront();
      }
      else
      {
        this.pnlNoSaves2.Visible = false;
        this.pnlNoSaves2.Location = new Point(Util.ScaleSize(-9999), 0);
        this.pnlNoSaves2.SendToBack();
      }
    }

    private void FillResignSaves()
    {
      this.dgResign.Rows.Clear();
      if (this.cbDrives.SelectedItem == null)
        return;
      string strBase = this.cbDrives.SelectedItem.ToString();
      switch (Util.CurrentPlatform)
      {
        case Util.Platform.Linux:
          strBase = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) strBase);
          break;
        case Util.Platform.MacOS:
          strBase = string.Format("/Volumes{0}", (object) strBase);
          break;
      }
      if (!Directory.Exists(Util.GetDataPath(strBase)))
        return;
      string[] directories = Directory.GetDirectories(Util.GetDataPath(strBase));
      foreach (string key in this.m_dictAllLocalSaves.Keys)
        ;
      ((ISupportInitialize) this.dgResign).BeginInit();
      foreach (string path in directories)
      {
        if (this.IsValidPSNID(Path.GetFileName(path)))
        {
          foreach (string directory in Directory.GetDirectories(path))
          {
            foreach (string file in Directory.GetFiles(directory, "*.bin"))
            {
              if (new FileInfo(file).Length < 2048L)
              {
                string str = file.Substring(0, file.Length - 4);
                if (System.IO.File.Exists(str))
                {
                  string fileName = Path.GetFileName(directory);
                  game game = new game()
                  {
                    id = fileName,
                    containers = new containers()
                    {
                      _containers = new List<container>()
                      {
                        new container() { pfs = Path.GetFileName(str) }
                      }
                    },
                    LocalSaveFolder = file
                  };
                  int onlineSaveIndex = MainForm3.GetOnlineSaveIndex(this.m_games, str, out string _);
                  int index = this.dgResign.Rows.Add();
                  this.dgResign.Rows[index].Tag = (object) directory;
                  this.dgResign.Rows[index].Cells[0].Tag = (object) game;
                  this.dgResign.Rows[index].Cells[0].Value = onlineSaveIndex >= 0 ? (object) this.m_games[onlineSaveIndex].name : (object) fileName;
                  this.dgResign.Rows[index].Cells[1].Value = this.GetPSNID(game);
                }
              }
            }
          }
        }
      }
      ((ISupportInitialize) this.dgResign).EndInit();
    }

    public static int GetOnlineSaveIndex(List<game> games, string save, out string saveId)
    {
      string fileName = Path.GetFileName(Path.GetDirectoryName(save));
      string withoutExtension = Path.GetFileNameWithoutExtension(save);
      for (int index1 = 0; index1 < games.Count; ++index1)
      {
        saveId = games[index1].id;
        if (fileName.Equals(saveId) || games[index1].IsAlias(fileName, out saveId))
        {
          for (int index2 = 0; index2 < games[index1].containers._containers.Count; ++index2)
          {
            if (withoutExtension == games[index1].containers._containers[index2].pfs || Util.IsMatch(withoutExtension, games[index1].containers._containers[index2].pfs))
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

    private string GetSaveDescription(string sfoFile) => MainForm3.GetParamInfo(sfoFile, "SUB_TITLE") + "\r\n" + MainForm3.GetParamInfo(sfoFile, "DETAIL");

    private string GetSaveTitle(string sfoFile) => MainForm3.GetParamInfo(sfoFile, "TITLE");

    private void btnHome_Click(object sender, EventArgs e)
    {
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.pnlHome.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
        this.pnlBackup.Location = new Point(Util.ScaleSize(5000), Util.ScaleSize(5000));
      }
      else
      {
        this.pnlHome.Visible = true;
        this.pnlBackup.Visible = false;
        if (this.cbDrives.SelectedItem == null)
          this.ResizeColumns(this.chkShowAll.Checked);
      }
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
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.pnlBackup.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
        this.pnlHome.Location = new Point(5000, 5000);
      }
      else
      {
        this.pnlBackup.Visible = true;
        this.pnlHome.Visible = false;
      }
      if (Util.CurrentPlatform == Util.Platform.MacOS)
      {
        this.btnOptions.Image = (Image) PS3SaveEditor.Resources.Resources.home_settings_on;
        this.btnHome.Image = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_off;
        this.btnHelp.Image = (Image) PS3SaveEditor.Resources.Resources.home_help_off;
      }
      else
      {
        this.btnOptions.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_settings_on;
        this.btnHome.BackgroundImage = (Image) PS3SaveEditor.Resources.Resources.home_gamelist_off;
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
      System.IO.File.WriteAllText(Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm3.USER_CHEATS_FILE, contents);
    }

    private bool CheckForVersion() => true;

    private void btnRss_Click(object sender, EventArgs e)
    {
      try
      {
        string url = GameListDownloader.RSS_URL;
        if (!Util.IsHyperkin())
          url = string.Format("{0}/ps4/rss?token={1}", (object) Util.GetBaseUrl(), (object) Util.GetAuthToken());
        int num = (int) new RSSForm(RssFeed.Read(url).Channels[0]).ShowDialog();
      }
      catch (Exception ex)
      {
      }
    }

    private void btnDiagnostic_Click(object sender, EventArgs e)
    {
      try
      {
        using (DiagnosticForm diagnosticForm = new DiagnosticForm())
        {
          int num = (int) diagnosticForm.ShowDialog((IWin32Window) this);
        }
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
          RegistryKey currentUser = Registry.CurrentUser;
          RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase(), true);
          string[] valueNames = registryKey.GetValueNames();
          try
          {
            foreach (string name in valueNames)
            {
              if (!(name == "Location"))
                registryKey.DeleteValue(name);
            }
          }
          catch (Exception ex)
          {
          }
          finally
          {
            registryKey.Close();
            currentUser.Close();
          }
          Util.DeleteRegistryValue("User");
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

    private void btnOpenFolder_Click(object sender, EventArgs e)
    {
      if (Util.CurrentPlatform == Util.Platform.Linux)
        return;
      Process.Start("file://" + this.txtBackupLocation.Text);
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
      if (Util.CurrentPlatform == Util.Platform.Linux)
        return;
      Path.GetDirectoryName(Application.ExecutablePath);
      string str = Util.IsHyperkin() ? "http://www.thesavewizard.com/manual.php" : "http://www.savewizard.net/manuals/swps4m/";
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
      string str = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].Value as string;
      string toolTipText = this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Cells[1].ToolTipText;
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

    private void chkBackup_Click(object sender, EventArgs e)
    {
      if (this.chkBackup.Checked)
        return;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmBackup, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        this.chkBackup.Checked = true;
    }

    private void btnManageProfiles_Click(object sender, EventArgs e)
    {
      ManageProfiles manageProfiles = new ManageProfiles("", this.m_psnIDs);
      int num = (int) manageProfiles.ShowDialog();
      if (!string.IsNullOrEmpty(manageProfiles.PsnIDResponse))
      {
        Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(manageProfiles.PsnIDResponse, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
        if (dictionary.ContainsKey("status") && (string) dictionary["status"] == "OK")
        {
          this.m_psnIDs = !dictionary.ContainsKey("psnid") ? new Dictionary<string, object>() : dictionary["psnid"] as Dictionary<string, object>;
          this.m_psn_quota = Convert.ToInt32(dictionary["psnid_quota"]);
          this.m_psn_remaining = Convert.ToInt32(dictionary["psnid_remaining"]);
        }
      }
      this.RefreshProfiles();
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
        if (this.dgServerGames.SelectedRows.Count != 1)
          return;
        ManageProfiles manageProfiles = new ManageProfiles((this.dgServerGames.SelectedRows[0].Tag as game).PSN_ID, this.m_psnIDs);
        if (manageProfiles.ShowDialog() == DialogResult.OK)
        {
          if (!string.IsNullOrEmpty(manageProfiles.PsnIDResponse))
          {
            Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(manageProfiles.PsnIDResponse, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
            if (dictionary.ContainsKey("status") && (string) dictionary["status"] == "OK")
            {
              this.m_psnIDs = !dictionary.ContainsKey("psnid") ? new Dictionary<string, object>() : dictionary["psnid"] as Dictionary<string, object>;
              this.m_psn_quota = Convert.ToInt32(dictionary["psnid_quota"]);
              this.m_psn_remaining = Convert.ToInt32(dictionary["psnid_remaining"]);
            }
          }
          this.RefreshProfiles();
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
        }
      }
    }

    private void resignToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dgServerGames.SelectedCells.Count == 0)
        return;
      switch (!(this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag is game tag) ? this.dgServerGames.Rows[this.dgServerGames.SelectedCells[0].RowIndex].Tag as string : tag.LocalSaveFolder)
      {
        case null:
          break;
        default:
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
          break;
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
        string uriString = string.Format("{0}/ps4auth", (object) Util.GetBaseUrl(), (object) uid, (object) str1);
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
          int num1 = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errNotRegistered, (object) Util.PRODUCT_NAME) + str3, PS3SaveEditor.Resources.Resources.msgError);
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

    private void registerProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.m_psnIDs.Count >= this.m_psn_quota || this.m_psn_remaining <= 0)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errMaxProfiles, PS3SaveEditor.Resources.Resources.msgInfo);
      }
      else
      {
        if (this.dgResign.SelectedRows.Count != 1)
          return;
        ManageProfiles manageProfiles = new ManageProfiles((this.dgResign.SelectedRows[0].Tag as game).PSN_ID, this.m_psnIDs);
        if (manageProfiles.ShowDialog() == DialogResult.OK)
        {
          if (!string.IsNullOrEmpty(manageProfiles.PsnIDResponse))
          {
            Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(manageProfiles.PsnIDResponse, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
            if (dictionary.ContainsKey("status") && (string) dictionary["status"] == "OK")
            {
              this.m_psnIDs = !dictionary.ContainsKey("psnid") ? new Dictionary<string, object>() : dictionary["psnid"] as Dictionary<string, object>;
              this.m_psn_quota = Convert.ToInt32(dictionary["psnid_quota"]);
              this.m_psn_remaining = Convert.ToInt32(dictionary["psnid_remaining"]);
            }
          }
          this.RefreshProfiles();
          this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
        }
      }
    }

    private void deleteSaveToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      string path = !(this.dgResign.Rows[this.dgResign.SelectedCells[0].RowIndex].Tag is game tag) ? this.dgResign.Rows[this.dgResign.SelectedCells[0].RowIndex].Tag as string : tag.LocalSaveFolder;
      if (path == null)
        return;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmDeleteSave, this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      try
      {
        System.IO.File.Delete(path);
        System.IO.File.Delete(path.Substring(0, tag.LocalSaveFolder.Length - 4));
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errDelete, PS3SaveEditor.Resources.Resources.msgError);
      }
      int scrollingRowIndex = this.dgResign.FirstDisplayedScrollingRowIndex;
      this.cbDrives_SelectedIndexChanged((object) null, (EventArgs) null);
      if (this.dgResign.Rows.Count > scrollingRowIndex && scrollingRowIndex >= 0)
        this.dgResign.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
    }

    private void dgServerGames_Click(object sender, EventArgs e) => this.dgServerGames.Focus();

    private void dgResign_Click(object sender, EventArgs e) => this.dgResign.Focus();

    public int DropDownWidth(ComboBox myCombo)
    {
      int num = 40;
      foreach (object obj in myCombo.Items)
      {
        int width = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
        if (width > num)
          num = width;
      }
      return num;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm3));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
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
      this.tabPageResign = new Panel();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.resignToolStripMenuItem1 = new ToolStripMenuItem();
      this.resignToolStripMenuItem1.Font = Util.GetFontForPlatform(this.resignToolStripMenuItem1.Font);
      this.registerProfileToolStripMenuItem = new ToolStripMenuItem();
      this.registerProfileToolStripMenuItem.Font = Util.GetFontForPlatform(this.registerProfileToolStripMenuItem.Font);
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.deleteSaveToolStripMenuItem1 = new ToolStripMenuItem();
      this.deleteSaveToolStripMenuItem1.Font = Util.GetFontForPlatform(this.deleteSaveToolStripMenuItem1.Font);
      this.tabPageGames = new Panel();
      this.pnlNoSaves = new Panel();
      this.pnlNoSaves2 = new Panel();
      this.lblNoSaves = new Label();
      this.lblNoSaves2 = new Label();
      this.chkShowAll = new CheckBox();
      this.btnResign = new Button();
      this.btnCheats = new Button();
      this.btnImport = new Button();
      this.btnGamesInServer = new Button();
      this.panel1 = new Panel();
      this.cbDrives = new ComboBox();
      this.pnlBackup = new Panel();
      this.Multi = new DataGridViewTextBoxColumn();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.picVersion = new PictureBox();
      this.pictureBox2 = new PictureBox();
      this.picTraffic = new PictureBox();
      this.gbManageProfile = new CustomGroupBox();
      this.gbProfiles = new CustomGroupBox();
      this.lblManageProfiles = new Label();
      this.btnManageProfiles = new Button();
      this.groupBox2 = new CustomGroupBox();
      this.cbLanguage = new ComboBox();
      this.lblLanguage = new Label();
      this.cbScale = new ComboBox();
      this.lblScale = new Label();
      this.lblDeactivate = new Label();
      this.btnDeactivate = new Button();
      this.groupBox1 = new CustomGroupBox();
      this.diagnosticBox = new CustomGroupBox();
      this.lblRSSSection = new Label();
      this.btnRss = new Button();
      this.lblDiagnosticSection = new Label();
      this.btnDiagnostic = new Button();
      this.gbBackupLocation = new CustomGroupBox();
      this.btnOpenFolder = new Button();
      this.lblBackup = new Label();
      this.btnBrowse = new Button();
      this.txtBackupLocation = new TextBox();
      this.chkBackup = new CheckBox();
      this.btnApply = new Button();
      this.dgResign = new CustomDataGridView();
      this._Head = new DataGridViewTextBoxColumn();
      this.GameID = new DataGridViewTextBoxColumn();
      this.SysVer = new DataGridViewTextBoxColumn();
      this.PSNID = new DataGridViewTextBoxColumn();
      this.dgServerGames = new CustomDataGridView();
      this.Choose = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
      this.dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
      this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
      this.Addded = new DataGridViewTextBoxColumn();
      this.contextMenuStrip1.SuspendLayout();
      this.pnlHome.SuspendLayout();
      this.tabPageResign.SuspendLayout();
      this.contextMenuStrip2.SuspendLayout();
      this.tabPageGames.SuspendLayout();
      this.pnlNoSaves.SuspendLayout();
      this.pnlNoSaves2.SuspendLayout();
      this.pnlBackup.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.picVersion).BeginInit();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      ((ISupportInitialize) this.picTraffic).BeginInit();
      this.gbManageProfile.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.diagnosticBox.SuspendLayout();
      this.gbBackupLocation.SuspendLayout();
      ((ISupportInitialize) this.dgResign).BeginInit();
      ((ISupportInitialize) this.dgServerGames).BeginInit();
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
      this.pnlHome.Controls.Add((Control) this.tabPageResign);
      this.pnlHome.Controls.Add((Control) this.tabPageGames);
      this.pnlHome.Controls.Add((Control) this.chkShowAll);
      this.pnlHome.Controls.Add((Control) this.btnResign);
      this.pnlHome.Controls.Add((Control) this.btnCheats);
      this.pnlHome.Controls.Add((Control) this.btnImport);
      this.pnlHome.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlHome.Name = "pnlHome";
      this.pnlHome.Size = Util.ScaleSize(new Size(511, 347));
      this.pnlHome.TabIndex = 8;
      this.tabPageResign.Controls.Add((Control) this.pnlNoSaves2);
      this.tabPageResign.Controls.Add((Control) this.dgResign);
      this.tabPageResign.Location = new Point(Util.ScaleSize(4), Util.ScaleSize(22));
      this.tabPageResign.Name = "tabPageResign";
      this.tabPageResign.Padding = new Padding(3);
      this.tabPageResign.Size = Util.ScaleSize(new Size(508, 325));
      this.tabPageResign.TabIndex = 1;
      this.tabPageResign.Text = "Re-Sign";
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.resignToolStripMenuItem1,
        (ToolStripItem) this.registerProfileToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.deleteSaveToolStripMenuItem1
      });
      this.contextMenuStrip2.Name = "contextMenuStrip2";
      this.contextMenuStrip2.Size = Util.ScaleSize(new Size(163, 76));
      this.resignToolStripMenuItem1.Name = "resignToolStripMenuItem1";
      this.resignToolStripMenuItem1.Size = Util.ScaleSize(new Size(162, 22));
      this.resignToolStripMenuItem1.Text = "Re-sign";
      this.registerProfileToolStripMenuItem.Name = "registerProfileToolStripMenuItem";
      this.registerProfileToolStripMenuItem.Size = Util.ScaleSize(new Size(162, 22));
      this.registerProfileToolStripMenuItem.Text = "Register Profile...";
      this.registerProfileToolStripMenuItem.Click += new EventHandler(this.registerProfileToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = Util.ScaleSize(new Size(159, 6));
      this.deleteSaveToolStripMenuItem1.Name = "deleteSaveToolStripMenuItem1";
      this.deleteSaveToolStripMenuItem1.Size = Util.ScaleSize(new Size(162, 22));
      this.deleteSaveToolStripMenuItem1.Text = "Delete Save";
      this.deleteSaveToolStripMenuItem1.Click += new EventHandler(this.deleteSaveToolStripMenuItem1_Click);
      this.tabPageGames.Controls.Add((Control) this.pnlNoSaves);
      this.tabPageGames.Controls.Add((Control) this.dgServerGames);
      this.tabPageGames.Location = new Point(Util.ScaleSize(4), Util.ScaleSize(22));
      this.tabPageGames.Name = "tabPageGames";
      this.tabPageGames.Size = Util.ScaleSize(new Size(507, 325));
      this.tabPageGames.TabIndex = 0;
      this.tabPageGames.Text = "Cheats";
      this.pnlNoSaves.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNoSaves.Controls.Add((Control) this.lblNoSaves);
      this.pnlNoSaves.Location = new Point(Util.ScaleSize(1), 0);
      this.pnlNoSaves.Name = "pnlNoSaves";
      this.pnlNoSaves.Size = Util.ScaleSize(new Size(506, 325));
      this.pnlNoSaves.TabIndex = 7;
      this.pnlNoSaves.Visible = false;
      this.pnlNoSaves2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlNoSaves2.Controls.Add((Control) this.lblNoSaves2);
      this.pnlNoSaves2.Location = new Point(Util.ScaleSize(1), 0);
      this.pnlNoSaves2.Name = "pnlNoSaves2";
      this.pnlNoSaves2.Size = Util.ScaleSize(new Size(506, 325));
      this.pnlNoSaves2.TabIndex = 7;
      this.pnlNoSaves2.Visible = false;
      this.lblNoSaves.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoSaves.BackColor = System.Drawing.Color.Transparent;
      this.lblNoSaves.ForeColor = System.Drawing.Color.White;
      this.lblNoSaves.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(125));
      this.lblNoSaves.Name = "lblNoSaves";
      this.lblNoSaves.Size = Util.ScaleSize(new Size(481, 20));
      this.lblNoSaves.TabIndex = 10;
      this.lblNoSaves.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNoSaves2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoSaves2.BackColor = System.Drawing.Color.Transparent;
      this.lblNoSaves2.ForeColor = System.Drawing.Color.White;
      this.lblNoSaves2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(125));
      this.lblNoSaves2.Name = "lblNoSaves2";
      this.lblNoSaves2.Size = Util.ScaleSize(new Size(481, 20));
      this.lblNoSaves2.TabIndex = 10;
      this.lblNoSaves2.TextAlign = ContentAlignment.MiddleCenter;
      this.chkShowAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkShowAll.Location = new Point(Util.ScaleSize(415), 0);
      this.chkShowAll.Name = "chkShowAll";
      this.chkShowAll.Size = Util.ScaleSize(new Size(97, 16));
      this.chkShowAll.TabIndex = 13;
      this.chkShowAll.Text = "Show All";
      this.chkShowAll.UseVisualStyleBackColor = true;
      this.btnResign.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
      this.btnResign.FlatAppearance.BorderSize = 0;
      this.btnResign.FlatStyle = FlatStyle.Flat;
      this.btnResign.Location = new Point(Util.ScaleSize(80), 0);
      this.btnResign.Name = "btnResign";
      this.btnResign.Size = Util.ScaleSize(new Size(75, 23));
      this.btnResign.TabIndex = 9;
      this.btnResign.Text = "Re-Sign";
      this.btnResign.UseVisualStyleBackColor = false;
      this.btnCheats.BackColor = System.Drawing.Color.White;
      this.btnCheats.FlatAppearance.BorderSize = 0;
      this.btnCheats.FlatStyle = FlatStyle.Flat;
      this.btnCheats.Location = new Point(Util.ScaleSize(4), 0);
      this.btnCheats.Name = "btnCheats";
      this.btnCheats.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCheats.TabIndex = 8;
      this.btnCheats.Text = "Cheats";
      this.btnCheats.UseVisualStyleBackColor = false;
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.Location = new Point(Util.ScaleSize(437), Util.ScaleSize(-1));
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = Util.ScaleSize(new Size(75, 23));
      this.btnImport.TabIndex = 16;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
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
      this.cbDrives.Location = Util.IsUnixOrMacOSX() ? new Point(Util.ScaleSize(65), Util.ScaleSize(2)) : new Point(Util.ScaleSize(65), Util.ScaleSize(5));
      this.cbDrives.Width = Util.ScaleSize(165);
      this.cbDrives.Height = Util.ScaleSize(21);
      this.cbDrives.Name = "cbDrives";
      this.cbDrives.TabIndex = 3;
      this.panel1.Controls.Add((Control) this.cbDrives);
      this.pnlBackup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBackup.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.pnlBackup.Controls.Add((Control) this.gbManageProfile);
      this.pnlBackup.Controls.Add((Control) this.groupBox2);
      this.pnlBackup.Controls.Add((Control) this.groupBox1);
      this.pnlBackup.Controls.Add((Control) this.diagnosticBox);
      this.pnlBackup.Controls.Add((Control) this.gbBackupLocation);
      this.pnlBackup.Location = new Point(Util.ScaleSize(257), Util.ScaleSize(15));
      this.pnlBackup.Name = "pnlBackup";
      this.pnlBackup.Size = Util.ScaleSize(new Size(508, 347));
      this.pnlBackup.TabIndex = 9;
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
      this.panel2.TabIndex = 12;
      this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.panel3.BackColor = System.Drawing.Color.FromArgb(0, 56, 115);
      this.panel3.BackgroundImage = (Image) componentResourceManager.GetObject("panel3.BackgroundImage");
      this.panel3.Controls.Add((Control) this.picVersion);
      this.panel3.Controls.Add((Control) this.pictureBox2);
      this.panel3.Controls.Add((Control) this.picTraffic);
      this.panel3.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(207));
      this.panel3.Name = "panel3";
      this.panel3.Size = Util.ScaleSize(new Size(237, 122));
      this.panel3.TabIndex = 13;
      this.picVersion.BackgroundImageLayout = ImageLayout.Stretch;
      this.picVersion.Location = new Point(0, Util.ScaleSize(23));
      this.picVersion.Name = "picVersion";
      this.picVersion.Size = Util.ScaleSize(new Size(237, 26));
      this.picVersion.TabIndex = 13;
      this.picVersion.TabStop = false;
      this.pictureBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox2.Location = new Point(0, 0);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = Util.ScaleSize(new Size(237, 122));
      this.pictureBox2.TabIndex = 12;
      this.pictureBox2.TabStop = false;
      this.picTraffic.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.picTraffic.BackgroundImageLayout = ImageLayout.Stretch;
      this.picTraffic.Location = new Point(0, 0);
      this.picTraffic.Name = "picTraffic";
      this.picTraffic.Size = Util.ScaleSize(new Size(237, 26));
      this.picTraffic.TabIndex = 11;
      this.picTraffic.TabStop = false;
      this.gbManageProfile.Controls.Add((Control) this.gbProfiles);
      this.gbManageProfile.Controls.Add((Control) this.lblManageProfiles);
      this.gbManageProfile.Controls.Add((Control) this.btnManageProfiles);
      this.gbManageProfile.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(270));
      this.gbManageProfile.Name = "gbManageProfile";
      this.gbManageProfile.Size = Util.ScaleSize(new Size(483, 65));
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
      this.groupBox2.Controls.Add((Control) this.cbScale);
      this.groupBox2.Controls.Add((Control) this.lblScale);
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
        this.lblLanguage.Location = new Point(Util.ScaleSize(335), Util.ScaleSize(12));
      else
        this.lblLanguage.Location = new Point(Util.ScaleSize(332), Util.ScaleSize(16));
      this.lblLanguage.Name = "lblLanguage";
      this.lblLanguage.Size = Util.ScaleSize(new Size(55, 13));
      this.lblLanguage.TabIndex = 9;
      this.lblLanguage.Text = "Language";
      this.cbScale.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbScale.FormattingEnabled = true;
      if (Util.IsUnixOrMacOSX())
        this.cbScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(32));
      else
        this.cbScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(36));
      this.cbScale.Size = Util.ScaleSize(new Size(122, 21));
      this.cbScale.Name = "cbScale";
      this.cbScale.TabIndex = 9;
      this.cbScale.Visible = true;
      this.lblScale.AutoSize = true;
      this.lblScale.BackColor = System.Drawing.Color.Transparent;
      this.lblScale.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(12));
      else
        this.lblScale.Location = new Point(Util.ScaleSize(195), Util.ScaleSize(16));
      this.lblScale.Name = "lblScale";
      this.lblScale.Size = Util.ScaleSize(new Size(55, 13));
      this.lblScale.TabIndex = 9;
      this.lblScale.Text = "Scale";
      this.lblScale.Visible = true;
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
      this.groupBox1.Size = Util.ScaleSize(new Size(240, 67));
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
      this.diagnosticBox.Controls.Add((Control) this.lblDiagnosticSection);
      this.diagnosticBox.Controls.Add((Control) this.btnDiagnostic);
      this.diagnosticBox.Location = new Point(Util.ScaleSize((int) byte.MaxValue), Util.ScaleSize(128));
      this.diagnosticBox.Name = "diagnosticBox";
      this.diagnosticBox.Size = Util.ScaleSize(new Size(240, 67));
      this.diagnosticBox.TabIndex = 9;
      this.diagnosticBox.TabStop = false;
      this.lblDiagnosticSection.AutoSize = true;
      this.lblDiagnosticSection.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.lblDiagnosticSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      else
        this.lblDiagnosticSection.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(15));
      this.lblDiagnosticSection.Name = "lblDiagnosticSection";
      this.lblDiagnosticSection.Size = Util.ScaleSize(new Size(295, 13));
      this.lblDiagnosticSection.TabIndex = 8;
      this.lblDiagnosticSection.Text = "Diagnostic Section";
      this.btnDiagnostic.ForeColor = System.Drawing.Color.White;
      if (Util.IsUnixOrMacOSX())
        this.btnDiagnostic.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(33));
      else
        this.btnDiagnostic.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(37));
      this.btnDiagnostic.Size = Util.ScaleSize(new Size(115, 23));
      this.btnDiagnostic.Name = "btnDiagnostic";
      this.btnDiagnostic.TabIndex = 0;
      this.btnDiagnostic.Text = "Diagnostic";
      this.btnDiagnostic.UseVisualStyleBackColor = false;
      this.btnDiagnostic.Click += new EventHandler(this.btnDiagnostic_Click);
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
      this.chkBackup.AutoSize = true;
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
      this.dgResign.AllowUserToAddRows = false;
      this.dgResign.AllowUserToDeleteRows = false;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8.25f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.False;
      this.dgResign.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgResign.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgResign.Columns.AddRange((DataGridViewColumn) this._Head, (DataGridViewColumn) this.GameID, (DataGridViewColumn) this.SysVer, (DataGridViewColumn) this.PSNID);
      this.dgResign.ContextMenuStrip = this.contextMenuStrip2;
      this.dgResign.Location = new Point(0, 0);
      this.dgResign.Name = "dgResign";
      this.dgResign.ReadOnly = true;
      this.dgResign.RowHeadersVisible = false;
      this.dgResign.RowTemplate.Height = Util.ScaleSize(24);
      this.dgResign.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgResign.Size = Util.ScaleSize(new Size(508, 325));
      this.dgResign.TabIndex = 0;
      this.dgResign.Click += new EventHandler(this.dgResign_Click);
      this._Head.FillWeight = Util.ScaleSize(20f);
      this._Head.HeaderText = "";
      this._Head.Name = "_Head";
      this._Head.ReadOnly = true;
      this._Head.Width = Util.ScaleSize(20);
      this.GameID.FillWeight = Util.ScaleSize(200f);
      this.GameID.HeaderText = "Game";
      this.GameID.Name = "GameID";
      this.GameID.ReadOnly = true;
      this.GameID.Width = Util.ScaleSize(200);
      this.SysVer.FillWeight = Util.ScaleSize(50f);
      this.SysVer.HeaderText = "Sys. Ver.";
      this.SysVer.Name = "SysVer";
      this.SysVer.ReadOnly = true;
      this.PSNID.FillWeight = Util.ScaleSize(50f);
      this.PSNID.HeaderText = "Profile/PSN ID";
      this.PSNID.Name = "PSNID";
      this.PSNID.ReadOnly = true;
      this.PSNID.Width = Util.ScaleSize(250);
      this.dgServerGames.AllowUserToAddRows = false;
      this.dgServerGames.AllowUserToDeleteRows = false;
      this.dgServerGames.AllowUserToResizeRows = false;
      this.dgServerGames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgServerGames.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Control;
      gridViewCellStyle2.ForeColor = SystemColors.WindowText;
      gridViewCellStyle2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgServerGames.ColumnHeadersDefaultCellStyle = gridViewCellStyle2;
      this.dgServerGames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgServerGames.Columns.AddRange((DataGridViewColumn) this.Choose, (DataGridViewColumn) this.dataGridViewTextBoxColumn1, (DataGridViewColumn) this.dataGridViewTextBoxColumn2, (DataGridViewColumn) this.dataGridViewTextBoxColumn3, (DataGridViewColumn) this.dataGridViewTextBoxColumn4, (DataGridViewColumn) this.dataGridViewCheckBoxColumn1, (DataGridViewColumn) this.dataGridViewTextBoxColumn5, (DataGridViewColumn) this.Addded, (DataGridViewColumn) this.dataGridViewTextBoxColumn6);
      this.dgServerGames.ContextMenuStrip = this.contextMenuStrip1;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Window;
      gridViewCellStyle3.ForeColor = SystemColors.ControlText;
      gridViewCellStyle3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.False;
      this.dgServerGames.DefaultCellStyle = gridViewCellStyle3;
      this.dgResign.DefaultCellStyle = gridViewCellStyle3;
      this.dgServerGames.Location = new Point(0, 0);
      this.dgServerGames.Name = "dgServerGames";
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle4.BackColor = SystemColors.Control;
      gridViewCellStyle4.ForeColor = SystemColors.WindowText;
      gridViewCellStyle4.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle4.WrapMode = DataGridViewTriState.True;
      this.dgServerGames.RowHeadersDefaultCellStyle = gridViewCellStyle4;
      this.dgResign.RowHeadersDefaultCellStyle = gridViewCellStyle4;
      this.dgServerGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
      this.dgServerGames.RowHeadersVisible = false;
      this.dgServerGames.RowHeadersWidth = Util.ScaleSize(25);
      this.dgServerGames.RowTemplate.Height = Util.ScaleSize(24);
      this.dgServerGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgServerGames.Size = Util.ScaleSize(new Size(508, 325));
      this.dgServerGames.TabIndex = 12;
      this.dgServerGames.Click += new EventHandler(this.dgServerGames_Click);
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
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.dataGridViewTextBoxColumn2.DefaultCellStyle = gridViewCellStyle5;
      this.dataGridViewTextBoxColumn2.Frozen = true;
      this.dataGridViewTextBoxColumn2.HeaderText = "Cheats";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      this.dataGridViewTextBoxColumn2.Width = Util.ScaleSize(60);
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
      this.dataGridViewTextBoxColumn3.DefaultCellStyle = gridViewCellStyle6;
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
      this.dataGridViewTextBoxColumn6.HeaderText = "Cheats changing time";
      this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
      this.dataGridViewTextBoxColumn6.ReadOnly = true;
      this.dataGridViewTextBoxColumn6.Visible = false;
      this.dataGridViewTextBoxColumn6.Width = Util.ScaleSize(80);
      this.Addded.HeaderText = "Added";
      this.Addded.Name = "Addded";
      this.Addded.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.FromArgb(0, 44, 101);
      this.ClientSize = Util.ScaleSize(new Size(780, 377));
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOptions);
      this.Controls.Add((Control) this.btnHome);
      this.Controls.Add((Control) this.btnHelp);
      this.Controls.Add((Control) this.pnlHome);
      this.Controls.Add((Control) this.pnlBackup);
      this.DoubleBuffered = true;
      this.Name = nameof (MainForm3);
      this.Text = "PS4 Save Editor";
      this.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
      this.contextMenuStrip1.ResumeLayout(false);
      this.pnlHome.ResumeLayout(false);
      this.tabPageResign.ResumeLayout(false);
      this.contextMenuStrip2.ResumeLayout(false);
      this.tabPageGames.ResumeLayout(false);
      this.pnlNoSaves.ResumeLayout(false);
      this.pnlNoSaves2.ResumeLayout(false);
      this.pnlBackup.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      ((ISupportInitialize) this.picVersion).EndInit();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      ((ISupportInitialize) this.picTraffic).EndInit();
      this.gbManageProfile.ResumeLayout(false);
      this.gbManageProfile.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.diagnosticBox.ResumeLayout(false);
      this.diagnosticBox.PerformLayout();
      this.gbBackupLocation.ResumeLayout(false);
      this.gbBackupLocation.PerformLayout();
      ((ISupportInitialize) this.dgResign).EndInit();
      ((ISupportInitialize) this.dgServerGames).EndInit();
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
