// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Import
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using CSUST.Data;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class Import : Form
  {
    private Dictionary<string, object> m_accounts;
    private ZipFile m_zipFile;
    private string m_drive;
    private List<game> m_games;
    private Dictionary<string, object> m_psnIDs = (Dictionary<string, object>) null;
    private string m_expandedGame;
    private Dictionary<string, List<game>> m_map;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Button btnCancel;
    private Button btnImport;
    private CustomDataGridView dgImport;
    private DataGridViewTextBoxColumn ColSelect;
    private DataGridViewTextBoxColumn GameName;
    private DataGridViewTextBoxColumn SysVer;
    private DataGridViewTextBoxColumn Account;

    public Import(
      List<game> games,
      Dictionary<ZipEntry, ZipEntry> entries,
      ZipFile zipFile,
      Dictionary<string, object> accounts,
      string drive)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.Text = PS3SaveEditor.Resources.Resources.titleImport;
      this.m_games = games;
      this.m_psnIDs = accounts;
      this.dgImport.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colGameName;
      this.dgImport.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colSysVer;
      this.dgImport.Columns[3].HeaderText = PS3SaveEditor.Resources.Resources.colProfile;
      this.btnImport.Text = PS3SaveEditor.Resources.Resources.btnImport;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.m_accounts = accounts;
      this.m_drive = drive;
      this.m_zipFile = zipFile;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.PrepareMap(entries, games);
      this.FillSaves((string) null, false);
      this.dgImport.SelectionChanged += new EventHandler(this.dgImport_SelectionChanged);
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.dgImport.CellDoubleClick += new DataGridViewCellEventHandler(this.dgImport_CellDoubleClick);
    }

    private void dgImport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0 || this.dgImport.SelectedCells.Count == 0 || this.dgImport.SelectedCells[0].RowIndex < 0)
        return;
      this.FillSaves(this.dgImport.Rows[this.dgImport.SelectedCells[0].RowIndex].Cells[1].Value as string, this.dgImport.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending);
    }

    private void FillSaves(string expandGame, bool bSortedAsc)
    {
      if (this.m_expandedGame == expandGame)
      {
        expandGame = (string) null;
        this.m_expandedGame = (string) null;
      }
      this.dgImport.Rows.Clear();
      List<string> stringList = new List<string>();
      foreach (string key in this.m_map.Keys)
      {
        string id = key;
        foreach (alias allAlias in (this.m_games.Find((Predicate<game>) (a => a.id == id)) ?? this.m_map[id][0]).GetAllAliases(bSortedAsc))
        {
          string str1 = allAlias.name + " (" + allAlias.id + ")";
          string id1 = allAlias.id;
          if (this.m_map.ContainsKey(allAlias.id))
          {
            List<game> gameList = this.m_map[id1];
            if (stringList.IndexOf(id1) < 0)
            {
              stringList.Add(id1);
              int index1 = this.dgImport.Rows.Add();
              this.dgImport.Rows[index1].Cells[1].Value = (object) allAlias.name;
              if (gameList.Count == 0)
              {
                game game = gameList[0];
                this.dgImport.Rows[index1].Tag = (object) game;
                container targetGameFolder = game.GetTargetGameFolder();
                if (targetGameFolder != null)
                  this.dgImport.Rows[index1].Cells[2].Value = (object) targetGameFolder.GetCheatsCount();
                else
                  this.dgImport.Rows[index1].Cells[2].Value = (object) "N/A";
                this.dgImport.Rows[index1].Cells[0].ToolTipText = "";
                this.dgImport.Rows[index1].Cells[0].Tag = (object) id1;
                string[] strArray = game.PFSZipEntry.FileName.Split('/');
                if (strArray.Length >= 2)
                  this.dgImport.Rows[index1].Cells[2].Value = (object) strArray[strArray.Length - 2];
              }
              else
              {
                DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
                this.dgImport.Rows[index1].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                {
                  Font = new Font("Arial", Util.ScaleSize(7f))
                });
                this.dgImport.Rows[index1].Cells[0].Value = (object) "►";
                string str2 = this.dgImport.Rows[index1].Cells[1].Value as string;
                this.dgImport.Rows[index1].Cells[1].Value = string.IsNullOrEmpty(str2) ? (object) allAlias.id : (object) (str2 + " (" + allAlias.id + ")");
                dataGridViewCellStyle.BackColor = Color.White;
                this.dgImport.Rows[index1].Cells[0].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgImport.Rows[index1].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgImport.Rows[index1].Cells[2].Style.ApplyStyle(dataGridViewCellStyle);
                this.dgImport.Rows[index1].Tag = (object) gameList;
                if (str1 == expandGame || allAlias.id == expandGame)
                {
                  this.dgImport.Rows[index1].Cells[0].Style.ApplyStyle(new DataGridViewCellStyle()
                  {
                    Font = new Font("Arial", Util.ScaleSize(7f))
                  });
                  this.dgImport.Rows[index1].Cells[0].Value = (object) "▼";
                  this.dgImport.Rows[index1].Cells[0].ToolTipText = "";
                  this.dgImport.Rows[index1].Cells[1].Value = string.IsNullOrEmpty(str2) ? (object) allAlias.id : (object) (str2 + " (" + allAlias.id + ")");
                  this.dgImport.Rows[index1].Cells[0].Tag = (object) id1;
                  foreach (game game in gameList)
                  {
                    container container = game.containers._containers[0];
                    if (container != null)
                    {
                      int index2 = this.dgImport.Rows.Add();
                      Match match = Regex.Match(Path.GetFileNameWithoutExtension(game.LocalSaveFolder), container.pfs);
                      if (container.name != null && match.Groups != null && match.Groups.Count > 1)
                        this.dgImport.Rows[index2].Cells[1].Value = (object) ("    " + container.name.Replace("${1}", match.Groups[1].Value));
                      else
                        this.dgImport.Rows[index2].Cells[1].Value = (object) ("    " + (container.name ?? Path.GetFileNameWithoutExtension(game.LocalSaveFolder)));
                      this.dgImport.Rows[index2].Cells[0].Tag = (object) id1;
                      game.name = allAlias.name;
                      this.dgImport.Rows[index2].Tag = (object) game;
                      this.dgImport.Rows[index2].Cells[1].ToolTipText = Path.GetFileNameWithoutExtension(game.LocalSaveFolder);
                      this.dgImport.Rows[index2].Cells[3].Value = this.GetPSNID(game);
                      MemoryStream memoryStream = new MemoryStream();
                      game.BinZipEntry.Extract((Stream) memoryStream);
                      string sysVer = MainForm3.GetSysVer(memoryStream.GetBuffer());
                      memoryStream.Close();
                      memoryStream.Dispose();
                      this.dgImport.Rows[index2].Cells[2].Value = (object) sysVer;
                      if (sysVer == "?")
                        this.dgImport.Rows[index2].Cells[2].ToolTipText = PS3SaveEditor.Resources.Resources.msgUnknownSysVer;
                      else if (sysVer == "All")
                        this.dgImport.Rows[index2].Cells[2].ToolTipText = PS3SaveEditor.Resources.Resources.tooltipV1;
                      else if (sysVer == "4.50+")
                        this.dgImport.Rows[index2].Cells[2].ToolTipText = PS3SaveEditor.Resources.Resources.tooltipV2;
                      else if (sysVer == "4.70")
                        this.dgImport.Rows[index2].Cells[2].ToolTipText = PS3SaveEditor.Resources.Resources.tooltipV3;
                    }
                  }
                  this.m_expandedGame = expandGame;
                }
              }
            }
          }
        }
      }
    }

    private object GetPSNID(game item) => !this.IsValidPSNID(item.PSN_ID) ? (object) (PS3SaveEditor.Resources.Resources.lblUnregistered + " " + item.PSN_ID) : (this.m_psnIDs[item.PSN_ID] as Dictionary<string, object>)["friendly_name"];

    public bool IsValidPSNID(string psnId) => this.m_psnIDs != null && this.m_psnIDs.ContainsKey(psnId);

    private void PrepareMap(Dictionary<ZipEntry, ZipEntry> entries, List<game> games)
    {
      this.m_map = new Dictionary<string, List<game>>();
      foreach (ZipEntry key in entries.Keys)
      {
        string[] strArray1 = key.FileName.Split('/');
        if (strArray1.Length > 1 && strArray1[strArray1.Length - 2].StartsWith("CUSA"))
        {
          string[] strArray2 = key.FileName.Split('/');
          string saveId;
          int onlineSaveIndex = MainForm3.GetOnlineSaveIndex(games, key.FileName, out saveId);
          if (onlineSaveIndex < 0)
            saveId = strArray2[strArray2.Length - 2];
          if (!this.m_map.ContainsKey(saveId))
            this.m_map.Add(saveId, new List<game>());
          string str1 = strArray2[strArray1.Length - 2];
          string str2 = "";
          ZipEntry zipEntry = key;
          ZipEntry entry = entries[key];
          string directoryName = Path.GetDirectoryName(zipEntry.FileName);
          game game = new game()
          {
            id = str1,
            name = "",
            containers = new containers()
            {
              _containers = new List<container>()
              {
                new container() { pfs = Path.GetFileName(key.FileName) }
              }
            },
            PFSZipEntry = zipEntry,
            BinZipEntry = entry,
            ZipFile = this.m_zipFile,
            LocalSaveFolder = Path.Combine(directoryName, Path.GetFileName(entry.FileName))
          };
          if (onlineSaveIndex >= 0)
          {
            str2 = games[onlineSaveIndex].name;
            game = game.Copy(this.m_games[onlineSaveIndex]);
            game.id = saveId;
            game.LocalSaveFolder = Path.Combine(directoryName, Path.GetFileName(entry.FileName));
            game.PFSZipEntry = zipEntry;
            game.BinZipEntry = entry;
            game.ZipFile = this.m_zipFile;
          }
          this.m_map[saveId].Add(game);
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnImport_Click(object sender, EventArgs e)
    {
      if (Util.GetRegistryValue("NoResignMessage") == null)
      {
        int num1 = (int) new ResignInfo().ShowDialog((IWin32Window) this);
      }
      ChooseProfile chooseProfile = new ChooseProfile(this.m_accounts, "");
      if (chooseProfile.ShowDialog() != DialogResult.OK)
        return;
      game tag = this.dgImport.SelectedRows[0].Tag as game;
      ZipEntry pfsZipEntry = tag.PFSZipEntry;
      ZipEntry binZipEntry = tag.BinZipEntry;
      string id = tag.id;
      string str = Path.Combine(this.m_drive, "PS4", "SAVEDATA", chooseProfile.SelectedAccount, id);
      game game = new game()
      {
        id = id,
        name = "",
        containers = new containers()
        {
          _containers = new List<container>()
          {
            new container()
            {
              pfs = this.dgImport.SelectedRows[0].Cells[1].Value as string
            }
          }
        },
        PFSZipEntry = pfsZipEntry,
        BinZipEntry = binZipEntry,
        ZipFile = this.m_zipFile,
        LocalSaveFolder = Path.Combine(str, Path.GetFileName(pfsZipEntry.FileName))
      };
      if (File.Exists(game.LocalSaveFolder) && Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmResignOverwrite, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      if (string.IsNullOrEmpty(this.m_drive) || !Directory.Exists(Path.GetPathRoot(game.LocalSaveFolder)))
      {
        int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errImportNoUSB);
      }
      else
      {
        if (new ResignFilesUplaoder(game, str, chooseProfile.SelectedAccount, new List<string>()).ShowDialog() == DialogResult.OK)
        {
          int num3 = (int) new ResignMessage(false).ShowDialog((IWin32Window) this);
        }
        this.dgImport.ClearSelection();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void dgImport_SelectionChanged(object sender, EventArgs e) => this.btnImport.Enabled = this.dgImport.SelectedRows.Count == 1 && !string.IsNullOrEmpty(this.dgImport.SelectedRows[0].Cells[1].Value as string) && (this.dgImport.SelectedRows[0].Cells[1].Value as string).StartsWith("    ");

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
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
      this.btnImport = new Button();
      this.dgImport = new CustomDataGridView();
      this.ColSelect = new DataGridViewTextBoxColumn();
      this.GameName = new DataGridViewTextBoxColumn();
      this.SysVer = new DataGridViewTextBoxColumn();
      this.Account = new DataGridViewTextBoxColumn();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgImport).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnImport);
      this.panel1.Controls.Add((Control) this.dgImport);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(Util.ScaleSize(12));
      this.panel1.Size = Util.ScaleSize(new Size(580, 353));
      this.panel1.TabIndex = 1;
      this.btnCancel.Location = new Point(Util.ScaleSize(277), Util.ScaleSize(322));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnImport.Location = new Point(Util.ScaleSize(199), Util.ScaleSize(322));
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = Util.ScaleSize(new Size(75, 23));
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = nameof (Import);
      this.btnImport.UseVisualStyleBackColor = true;
      this.dgImport.AllowUserToAddRows = false;
      this.dgImport.AllowUserToDeleteRows = false;
      this.dgImport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgImport.Columns.AddRange((DataGridViewColumn) this.ColSelect, (DataGridViewColumn) this.GameName, (DataGridViewColumn) this.SysVer, (DataGridViewColumn) this.Account);
      this.dgImport.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.dgImport.MultiSelect = false;
      this.dgImport.Name = "dgImport";
      this.dgImport.ReadOnly = true;
      this.dgImport.RowHeadersVisible = false;
      this.dgImport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgImport.Size = Util.ScaleSize(new Size(555, 302));
      this.dgImport.TabIndex = 1;
      this.ColSelect.HeaderText = "";
      this.ColSelect.Name = "Select";
      this.ColSelect.ReadOnly = true;
      this.ColSelect.Width = Util.ScaleSize(20);
      this.GameName.HeaderText = "Game Name";
      this.GameName.Name = "GameName";
      this.GameName.ReadOnly = true;
      this.GameName.Resizable = DataGridViewTriState.True;
      this.GameName.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.GameName.Width = Util.ScaleSize(350);
      this.SysVer.HeaderText = "Sys. Ver";
      this.SysVer.Name = "SysVer";
      this.SysVer.ReadOnly = true;
      this.SysVer.Width = Util.ScaleSize(80);
      this.Account.HeaderText = "Profile/PSN ID";
      this.Account.Name = "Account";
      this.Account.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(600, 373));
      this.Controls.Add((Control) this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Import);
      this.Padding = new Padding(Util.ScaleSize(10));
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (Import);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.dgImport).EndInit();
      this.ResumeLayout(false);
    }
  }
}
