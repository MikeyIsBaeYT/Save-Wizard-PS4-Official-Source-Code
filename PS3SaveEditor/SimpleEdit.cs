// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SimpleEdit
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using CSUST.Data;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace PS3SaveEditor
{
  public class SimpleEdit : Form
  {
    private game m_game;
    private bool m_bCheatsModified = false;
    private bool m_bShowOnly = false;
    private List<string> m_gameFiles;
    private IContainer components = (IContainer) null;
    private Label lblGameName;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem addCodeToolStripMenuItem;
    private ToolStripMenuItem editCodeToolStripMenuItem;
    private ToolStripMenuItem deleteCodeToolStripMenuItem;
    private Panel panel1;
    private Button btnApplyCodes;
    private DataGridViewTextBoxColumn ColLocation;
    private DataGridViewTextBoxColumn Value;
    private Label label1;
    private Button btnClose;
    private Button btnApply;
    private CustomDataGridView dgCheatCodes;
    private CustomDataGridView dgCheats;
    private ComboBox cbProfile;
    private Label lblProfile;
    private DataGridViewCheckBoxColumn ColSelect;
    private DataGridViewTextBoxColumn Description;
    private DataGridViewTextBoxColumn Comment;

    public game GameItem => this.m_game;

    public SimpleEdit(game gameItem, bool bShowOnly, List<string> files = null)
    {
      this.m_bShowOnly = bShowOnly;
      this.m_game = game.Copy(gameItem);
      this.m_gameFiles = files;
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
      this.btnApply.Enabled = false;
      this.btnApply.BackColor = SystemColors.ButtonFace;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnClose.BackColor = SystemColors.ButtonFace;
      this.btnClose.ForeColor = System.Drawing.Color.Black;
      this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblProfile.Visible = false;
      this.cbProfile.Visible = false;
      this.label1.Visible = false;
      this.dgCheatCodes.Visible = false;
      this.lblGameName.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblGameName.ForeColor = System.Drawing.Color.White;
      this.lblGameName.Visible = false;
      this.SetLabels();
      this.CenterToScreen();
      this.FillProfiles();
      this.lblProfile.Text = PS3SaveEditor.Resources.Resources.lblProfile;
      this.lblGameName.Text = gameItem.name;
      this.dgCheats.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseClick);
      this.dgCheats.CellMouseDown += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseDown);
      this.dgCheats.CellValidated += new DataGridViewCellEventHandler(this.dgCheats_CellValidated);
      this.dgCheats.CellValueChanged += new DataGridViewCellEventHandler(this.dgCheats_CellValueChanged);
      this.dgCheats.CurrentCellDirtyStateChanged += new EventHandler(this.dgCheats_CurrentCellDirtyStateChanged);
      this.dgCheats.CellMouseUp += new DataGridViewCellMouseEventHandler(this.dgCheats_CellMouseUp);
      this.dgCheats.MouseDown += new MouseEventHandler(this.dgCheats_MouseDown);
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnApplyCodes.Click += new EventHandler(this.btnApplyCodes_Click);
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.Resize += new EventHandler(this.SimpleEdit_ResizeEnd);
      this.SimpleEdit_ResizeEnd((object) null, (EventArgs) null);
      this.btnApplyCodes.Text = PS3SaveEditor.Resources.Resources.btnApply;
      this.label1.Text = PS3SaveEditor.Resources.Resources.lblCheatCodes;
      this.FillCheats((string) null);
    }

    protected override void OnResizeBegin(EventArgs e)
    {
      this.SuspendLayout();
      base.OnResizeBegin(e);
    }

    protected override void OnResizeEnd(EventArgs e)
    {
      base.OnResizeEnd(e);
      this.ResumeLayout();
      this.SimpleEdit_ResizeEnd((object) null, (EventArgs) null);
    }

    private void dgCheats_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex != 0)
        return;
      this.dgCheats.EndEdit();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, System.Drawing.Color.FromArgb(0, 138, 213), System.Drawing.Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void dgCheats_MouseDown(object sender, MouseEventArgs e)
    {
      Point location = e.Location;
      if (this.dgCheats.HitTest(location.X, location.Y).Type == DataGridViewHitTestType.Cell)
        return;
      this.dgCheats.ClearSelection();
    }

    private void FillProfiles()
    {
      this.cbProfile.Items.Add((object) "None");
      using (RegistryKey currentUser = Registry.CurrentUser)
      {
        using (RegistryKey subKey = currentUser.CreateSubKey(Util.GetRegistryBase() + "\\Profiles"))
        {
          string str = (string) subKey.GetValue((string) null);
          foreach (string valueName in subKey.GetValueNames())
          {
            if (!string.IsNullOrEmpty(valueName))
            {
              int num = this.cbProfile.Items.Add((object) valueName);
              if ((string) subKey.GetValue(valueName) == str)
                this.cbProfile.SelectedIndex = num;
            }
          }
        }
      }
      if (this.cbProfile.SelectedIndex >= 0)
        return;
      this.cbProfile.SelectedIndex = 0;
    }

    private void dgCheats_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (!this.dgCheats.IsCurrentCellDirty)
        return;
      this.dgCheats.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private bool ValidateOneGroup(string curChecked)
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dgCheats.Rows)
      {
        if ("one".Equals(row.Tag) && row.Cells[0].Value != null && (bool) row.Cells[0].Value && (string) row.Cells[1].Tag != curChecked)
          row.Cells[0].Value = (object) false;
      }
      return true;
    }

    private void dgCheats_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      bool flag = false;
      if (e.ColumnIndex == 0)
      {
        foreach (DataGridViewRow row in (IEnumerable) this.dgCheats.Rows)
        {
          if (row.Cells[0].Value != null && (bool) row.Cells[0].Value && (string) row.Cells[0].Tag != "GameFile" && (string) row.Cells[0].Tag != "CheatGroup")
          {
            flag = true;
            break;
          }
        }
      }
      this.btnApply.Enabled = flag;
    }

    private void dgCheats_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != 0 || this.m_game.GetTargetGameFolder() != null)
        return;
      int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
    }

    private void SimpleEdit_ResizeEnd(object sender, EventArgs e)
    {
      this.btnApply.Left = this.Width / 2 - this.btnApply.Width - 2;
      this.btnClose.Left = this.Width / 2 + 2;
      this.lblProfile.Left = this.btnApply.Left - this.cbProfile.Width - this.lblProfile.Width - 30;
      this.cbProfile.Left = this.lblProfile.Left + this.lblProfile.Width + 5;
      this.dgCheats.Columns[1].Width = (this.dgCheats.Width - 2 - this.dgCheats.Columns[0].Width) / 2;
      this.dgCheats.Columns[2].Width = (this.dgCheats.Width - 2 - this.dgCheats.Columns[0].Width) / 2;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (this.m_bCheatsModified)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
      base.OnClosing(e);
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void SetLabels()
    {
      this.Text = PS3SaveEditor.Resources.Resources.titleSimpleEdit;
      this.btnApply.Text = PS3SaveEditor.Resources.Resources.btnApplyPatch;
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
      this.dgCheats.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
      this.dgCheats.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
      this.dgCheats.RowTemplate.Height = Util.ScaleSize(24);
      this.dgCheats.Columns[0].HeaderText = "";
      this.dgCheats.Columns[1].HeaderText = PS3SaveEditor.Resources.Resources.colDesc;
      this.dgCheats.Columns[2].HeaderText = PS3SaveEditor.Resources.Resources.colComment;
      this.addCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuAddCheatCode;
      this.editCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuEditCheatCode;
      this.deleteCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuDeleteCheatCode;
    }

    private void FillCheats(string highlight)
    {
      this.dgCheats.Rows.Clear();
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (targetGameFolder != null)
      {
        this.ColSelect.Visible = true;
        if (this.m_game.GetAllCheats().Count == 0)
        {
          int index = this.dgCheats.Rows.Add(new DataGridViewRow());
          this.dgCheats.Rows[index].Height = Util.ScaleSize(24);
          DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
          dataGridViewCellStyle.ForeColor = System.Drawing.Color.Gray;
          this.dgCheats.Rows[index].Cells[0].Tag = (object) "NoCheats";
          dataGridViewCellStyle.Font = new Font(this.dgCheats.Font, FontStyle.Italic);
          this.dgCheats.Rows[index].Cells[1].Style.ApplyStyle(dataGridViewCellStyle);
          this.dgCheats.Rows[index].Cells[1].Value = (object) PS3SaveEditor.Resources.Resources.lblNoCheats;
        }
        if (targetGameFolder.preprocess == 1 && this.m_gameFiles != null && this.m_gameFiles.Count > 0)
        {
          container gameFolder = container.Copy(targetGameFolder);
          List<file> files = gameFolder.files._files;
          targetGameFolder.files._files = new List<file>();
          this.m_gameFiles.Sort();
          foreach (string gameFile1 in this.m_gameFiles)
          {
            file gameFile2 = file.GetGameFile(gameFolder, this.m_game.LocalSaveFolder, gameFile1);
            if (gameFile2 != null)
            {
              file file = file.Copy(gameFile2);
              file.original_filename = file.filename;
              file.filename = gameFile1;
              targetGameFolder.files._files.Add(file);
            }
          }
          targetGameFolder.files._files.Sort((Comparison<file>) ((f1, f2) => f1.VisibleFileName.CompareTo(f2.VisibleFileName)));
        }
        MainForm.FillLocalCheats(ref this.m_game);
        foreach (file file in targetGameFolder.files._files)
        {
          if (targetGameFolder.files._files.Count > 1)
          {
            int index = this.dgCheats.Rows.Add(new DataGridViewRow());
            this.dgCheats.Rows[index].Height = Util.ScaleSize(24);
            this.dgCheats.Rows[index].Cells[1].Value = (object) file.VisibleFileName;
            this.dgCheats.Rows[index].Cells[2].Value = (object) "";
            this.dgCheats.Rows[index].Cells[1].Tag = (object) file.id;
            this.dgCheats.Rows[index].Cells[0].Tag = (object) "GameFile";
            this.dgCheats.Rows[index].Tag = (object) file.filename;
          }
          foreach (cheat cheat in file.cheats._cheats)
          {
            int index = this.dgCheats.Rows.Add(new DataGridViewRow());
            this.dgCheats.Rows[index].Height = Util.ScaleSize(24);
            this.dgCheats.Rows[index].Cells[1].Value = (object) cheat.name;
            this.dgCheats.Rows[index].Cells[2].Value = (object) cheat.note;
            this.dgCheats.Rows[index].Cells[1].Tag = (object) cheat.id;
            this.dgCheats.Rows[index].Cells[0].Tag = (object) file.filename;
            if (cheat.id == "-1")
            {
              this.dgCheats.Rows[index].Tag = (object) "UserCheat";
              this.dgCheats.Rows[index].Cells[1].Tag = (object) cheat.code;
            }
          }
          foreach (group group in file.groups)
            this.FillGroupCheats(file, group, file.filename, 0);
        }
      }
      else if (this.m_bShowOnly)
      {
        this.ColSelect.Visible = false;
        this.btnApply.Enabled = false;
        foreach (container container in this.m_game.containers._containers)
        {
          foreach (file file in container.files._files)
          {
            foreach (cheat cheat in file.cheats._cheats)
            {
              int index = this.dgCheats.Rows.Add();
              this.dgCheats.Rows[index].Height = Util.ScaleSize(24);
              this.dgCheats.Rows[index].Cells[1].Value = (object) cheat.name;
              this.dgCheats.Rows[index].Cells[2].Value = (object) cheat.note;
            }
            foreach (group group in file.groups)
              this.FillGroupCheats(file, group, file.filename, 0);
          }
        }
      }
      this.RefreshValue();
    }

    private void FillFileCheats(container target, file file, string saveFile)
    {
      for (int index1 = 0; index1 < file.Cheats.Count; ++index1)
      {
        cheat cheat = file.Cheats[index1];
        int index2 = this.dgCheats.Rows.Add(new DataGridViewRow());
        this.dgCheats.Rows[index2].Height = Util.ScaleSize(24);
        this.dgCheats.Rows[index2].Cells[1].Value = (object) cheat.name;
        this.dgCheats.Rows[index2].Cells[2].Value = (object) cheat.note;
        if (cheat.id == "-1")
        {
          this.dgCheats.Rows[index2].Cells[1].Style.ApplyStyle(new DataGridViewCellStyle()
          {
            ForeColor = System.Drawing.Color.Blue
          });
          this.dgCheats.Rows[index2].Cells[0].Tag = (object) "UserCheat";
          this.dgCheats.Rows[index2].Cells[1].Tag = (object) Path.GetFileName(saveFile);
          this.dgCheats.Rows[index2].Tag = (object) file.GetParent(target);
        }
        else
        {
          this.dgCheats.Rows[index2].Cells[0].Tag = (object) saveFile;
          this.dgCheats.Rows[index2].Cells[1].Tag = (object) cheat.id;
        }
      }
      if (file.groups.Count <= 0)
        return;
      foreach (group group in file.groups)
        this.FillGroupCheats(file, group, saveFile, 0);
    }

    private void FillGroupCheats(file file, group g, string saveFile, int level)
    {
      int index1 = this.dgCheats.Rows.Add(new DataGridViewRow());
      this.dgCheats.Rows[index1].Height = Util.ScaleSize(24);
      this.dgCheats.Rows[index1].Cells[0].Tag = (object) "CheatGroup";
      if (level > 0)
        this.dgCheats.Rows[index1].Cells[1].Value = (object) (new string(' ', level * 4) + g.name);
      else
        this.dgCheats.Rows[index1].Cells[1].Value = (object) g.name;
      this.dgCheats.Rows[index1].Cells[2].Value = (object) g.note;
      this.dgCheats.Rows[index1].Cells[2].Value = (object) "";
      foreach (cheat cheat in g.cheats)
      {
        int index2 = this.dgCheats.Rows.Add(new DataGridViewRow());
        this.dgCheats.Rows[index2].Height = Util.ScaleSize(24);
        this.dgCheats.Rows[index2].Cells[1].Value = (object) (new string(' ', (level + 1) * 4) + cheat.name);
        this.dgCheats.Rows[index2].Cells[0].Tag = (object) saveFile;
        this.dgCheats.Rows[index2].Cells[1].Tag = (object) cheat.id;
        this.dgCheats.Rows[index2].Tag = (object) g.options;
      }
      if (g._group == null)
        return;
      foreach (group g1 in g._group)
        this.FillGroupCheats(file, g1, saveFile, level + 1);
    }

    private bool ContainsGameFile(List<file> allGameFile, file @internal)
    {
      foreach (file file in allGameFile)
      {
        if (file.id == @internal.id)
          return true;
      }
      return false;
    }

    private void dgCheats_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      this.RefreshValue();
      if (e.RowIndex < 0 || e.ColumnIndex != 2 || !(this.dgCheats.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is string str) || str.IndexOf("http://") < 0)
        return;
      int startIndex = str.IndexOf("http://");
      int num = str.IndexOf(' ', startIndex);
      if (num > 0)
        Process.Start(str.Substring(str.IndexOf("http"), num - startIndex));
      else
        Process.Start(str.Substring(str.IndexOf("http")));
    }

    private void RefreshValue()
    {
      this.dgCheatCodes.Rows.Clear();
      int num = -1;
      if (this.dgCheats.SelectedCells.Count == 1)
        num = this.dgCheats.SelectedCells[0].RowIndex;
      if (this.dgCheats.SelectedRows.Count == 1)
        num = this.dgCheats.SelectedRows[0].Index;
      if (num >= 0 || this.dgCheats.Rows.Count <= 0)
        ;
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      bool flag = false;
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (targetGameFolder == null)
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        List<string> files = new List<string>();
        for (int index = 0; index < this.dgCheats.Rows.Count; ++index)
        {
          if (this.dgCheats.Rows[index].Cells[0].Value != null && (bool) this.dgCheats.Rows[index].Cells[0].Value)
          {
            foreach (file file in new List<file>((IEnumerable<file>) targetGameFolder.files._files))
            {
              foreach (cheat allCheat in file.GetAllCheats())
              {
                if (((string) this.dgCheats.Rows[index].Cells[1].Tag == allCheat.id || (string) this.dgCheats.Rows[index].Tag == "UserCheat" && allCheat.id == "-1" && allCheat.name == (string) this.dgCheats.Rows[index].Cells[1].Value) && (this.m_gameFiles == null || !((string) this.dgCheats.Rows[index].Cells[0].Tag != file.filename)))
                {
                  allCheat.Selected = true;
                  if (files.IndexOf(file.filename) < 0)
                    files.Add(file.filename);
                }
              }
            }
            flag = true;
          }
        }
        if (!flag)
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgSelectCheat, PS3SaveEditor.Resources.Resources.msgError);
        }
        else
        {
          if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnOverwrite, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return;
          if (new SimpleSaveUploader(this.m_game, (string) this.cbProfile.SelectedItem, files).ShowDialog() == DialogResult.OK)
          {
            int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgQuickModeFinish, PS3SaveEditor.Resources.Resources.msgInfo);
          }
          this.Close();
        }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void dgCheats_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e) => this.RefreshValue();

    private void btnApplyCodes_Click(object sender, EventArgs e)
    {
      if (this.dgCheatCodes.Tag == null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgNoCheats, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        this.m_game.GetTargetGameFolder();
        int tag = (int) this.dgCheatCodes.Tag;
      }
    }

    private void addCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<string> existingCodes = new List<string>();
      foreach (file file in this.m_game.GetTargetGameFolder().files._files)
      {
        foreach (cheat cheat in file.Cheats)
          existingCodes.Add(cheat.name);
      }
      AddCode addCode = new AddCode(existingCodes);
      if (addCode.ShowDialog() == DialogResult.OK)
      {
        cheat cheat = new cheat("-1", addCode.Description, addCode.Comment);
        cheat.code = addCode.Code;
        if (this.m_game.GetTargetGameFolder() == null)
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
          return;
        }
        string selectedSaveFile = this.GetSelectedSaveFile();
        this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), selectedSaveFile).Cheats.Add(cheat);
        this.SaveUserCheats();
        this.m_bCheatsModified = true;
      }
      this.FillCheats(addCode.Description);
    }

    private string GetSelectedSaveFile()
    {
      int index1 = this.dgCheats.SelectedRows[0].Index;
      if (this.dgCheats.Rows[index1].Cells[0].Tag != null && (string) this.dgCheats.Rows[index1].Cells[0].Tag == "GameFile")
        return this.dgCheats.Rows[index1].Cells[1].Value.ToString();
      for (int index2 = index1; index2 >= 0; --index2)
      {
        if ((string) this.dgCheats.Rows[index2].Cells[0].Tag == "GameFile")
          return this.dgCheats.Rows[index2].Tag.ToString();
      }
      return (string) null;
    }

    private void SaveUserCheats()
    {
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        string xml1 = "<usercheats></usercheats>";
        string str = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm.USER_CHEATS_FILE;
        if (File.Exists(str))
          xml1 = File.ReadAllText(str);
        XmlDocument xmlDocument = new XmlDocument();
        try
        {
          xmlDocument.LoadXml(xml1);
        }
        catch (Exception ex1)
        {
          try
          {
            string xml2 = xml1.Replace("&", "&amp;");
            xmlDocument.LoadXml(xml2);
          }
          catch (Exception ex2)
          {
          }
        }
        bool flag = false;
        container targetGameFolder = this.m_game.GetTargetGameFolder();
        for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; ++i)
        {
          if (this.m_game.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value)
            flag = true;
        }
        if (!flag)
        {
          XmlElement element = xmlDocument.CreateElement("game");
          element.SetAttribute("id", this.m_game.id + targetGameFolder.key);
          xmlDocument["usercheats"].AppendChild((XmlNode) element);
        }
        for (int i = 0; i < xmlDocument["usercheats"].ChildNodes.Count; ++i)
        {
          if (this.m_game.id + targetGameFolder.key == xmlDocument["usercheats"].ChildNodes[i].Attributes["id"].Value)
          {
            XmlElement childNode = xmlDocument["usercheats"].ChildNodes[i] as XmlElement;
            childNode.InnerXml = "";
            List<file> fileList = new List<file>((IEnumerable<file>) targetGameFolder.files._files);
            foreach (file file in targetGameFolder.files._files)
            {
              if (file.internals != null && file.internals.files.Count > 0)
                fileList.AddRange((IEnumerable<file>) file.internals.files);
            }
            foreach (file file in fileList)
            {
              XmlElement element1 = xmlDocument.CreateElement("file");
              element1.SetAttribute("name", file.filename);
              childNode.AppendChild((XmlNode) element1);
              foreach (cheat cheat in file.Cheats)
              {
                if (cheat.id == "-1")
                {
                  XmlElement element2 = xmlDocument.CreateElement("cheat");
                  element2.SetAttribute("desc", cheat.name);
                  element2.SetAttribute("comment", cheat.note);
                  element1.AppendChild((XmlNode) element2);
                  XmlElement element3 = xmlDocument.CreateElement("code");
                  element3.InnerText = cheat.code;
                  element2.AppendChild((XmlNode) element3);
                }
              }
            }
          }
        }
        xmlDocument.Save(str);
      }
    }

    private void editCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int index1 = this.dgCheats.SelectedRows[0].Index;
      file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.dgCheats.Rows[index1].Cells[0].Tag.ToString());
      cheat cheat1 = (cheat) null;
      foreach (cheat cheat2 in gameFile.Cheats)
      {
        if (cheat2.name == this.dgCheats.Rows[index1].Cells[1].Value.ToString())
        {
          cheat1 = cheat2;
          break;
        }
      }
      if (cheat1 == null)
        return;
      List<string> existingCodes = new List<string>();
      foreach (file file in this.m_game.GetTargetGameFolder().files._files)
      {
        foreach (cheat cheat3 in file.Cheats)
        {
          if (cheat3.name != this.dgCheats.Rows[index1].Cells[1].Value.ToString())
            existingCodes.Add(cheat3.name);
        }
      }
      AddCode addCode = new AddCode(cheat1, existingCodes);
      if (addCode.ShowDialog() == DialogResult.OK)
      {
        cheat cheat4 = new cheat("-1", addCode.Description, addCode.Comment);
        cheat4.code = addCode.Code;
        for (int index2 = 0; index2 < gameFile.Cheats.Count; ++index2)
        {
          if (gameFile.Cheats[index2].name == this.dgCheats.Rows[index1].Cells[1].Value.ToString())
            gameFile.Cheats[index2] = cheat4;
        }
        this.SaveUserCheats();
        this.m_bCheatsModified = true;
      }
      this.FillCheats(addCode.Description);
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.m_game.GetTargetGameFolder() == null)
        e.Cancel = true;
      else if (this.dgCheats.SelectedRows.Count == 1)
      {
        container targetGameFolder = this.m_game.GetTargetGameFolder();
        int? quickmode = targetGameFolder.quickmode;
        int num = 0;
        if (quickmode.GetValueOrDefault() > num && quickmode.HasValue)
        {
          e.Cancel = true;
        }
        else
        {
          int index = this.dgCheats.SelectedRows[0].Index;
          if (this.dgCheats.Rows[index].Cells[0].Tag != null && (this.dgCheats.Rows[index].Cells[0].Tag.ToString() == "NoCheats" || this.dgCheats.Rows[index].Cells[0].Tag.ToString() == "GameFile"))
            e.Cancel = false;
          else if (targetGameFolder.files._files.Count != 1)
            ;
          this.editCodeToolStripMenuItem.Enabled = (string) this.dgCheats.Rows[index].Tag == "UserCheat";
          this.deleteCodeToolStripMenuItem.Enabled = (string) this.dgCheats.Rows[index].Tag == "UserCheat";
        }
      }
      else
        e.Cancel = true;
    }

    private void dgCheats_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.RowIndex < 0 || e.Button != MouseButtons.Right)
        return;
      this.dgCheats.ClearSelection();
      this.dgCheats.Rows[e.RowIndex].Selected = true;
    }

    private void deleteCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int index1 = this.dgCheats.SelectedRows[0].Index;
      if (index1 < 0)
        return;
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmDelete, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.dgCheats.Rows[index1].Cells[0].Tag.ToString());
        for (int index2 = 0; index2 < gameFile.Cheats.Count; ++index2)
        {
          if (gameFile.Cheats[index2].name == this.dgCheats.Rows[index1].Cells[1].Value.ToString())
          {
            gameFile.Cheats.RemoveAt(index2);
            break;
          }
        }
        this.SaveUserCheats();
        this.FillCheats((string) null);
        this.m_bCheatsModified = true;
      }
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
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.lblGameName = new Label();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.addCodeToolStripMenuItem = new ToolStripMenuItem();
      this.editCodeToolStripMenuItem = new ToolStripMenuItem();
      this.deleteCodeToolStripMenuItem = new ToolStripMenuItem();
      this.panel1 = new Panel();
      this.lblProfile = new Label();
      this.cbProfile = new ComboBox();
      this.btnApplyCodes = new Button();
      this.dgCheatCodes = new CustomDataGridView();
      this.ColLocation = new DataGridViewTextBoxColumn();
      this.Value = new DataGridViewTextBoxColumn();
      this.label1 = new Label();
      this.dgCheats = new CustomDataGridView();
      this.ColSelect = new DataGridViewCheckBoxColumn();
      this.Description = new DataGridViewTextBoxColumn();
      this.Comment = new DataGridViewTextBoxColumn();
      this.btnClose = new Button();
      this.btnApply = new Button();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgCheatCodes).BeginInit();
      ((ISupportInitialize) this.dgCheats).BeginInit();
      this.SuspendLayout();
      this.lblGameName.AutoSize = true;
      this.lblGameName.Location = new Point(Util.ScaleSize(17), Util.ScaleSize(9));
      this.lblGameName.Name = "lblGameName";
      this.lblGameName.Size = Util.ScaleSize(new Size(0, 13));
      this.lblGameName.TabIndex = 0;
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.addCodeToolStripMenuItem,
        (ToolStripItem) this.editCodeToolStripMenuItem,
        (ToolStripItem) this.deleteCodeToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = Util.ScaleSize(new Size(139, 70));
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.addCodeToolStripMenuItem.Name = "addCodeToolStripMenuItem";
      this.addCodeToolStripMenuItem.Size = Util.ScaleSize(new Size(138, 22));
      this.addCodeToolStripMenuItem.Text = "Add Code";
      this.addCodeToolStripMenuItem.Click += new EventHandler(this.addCodeToolStripMenuItem_Click);
      this.editCodeToolStripMenuItem.Name = "editCodeToolStripMenuItem";
      this.editCodeToolStripMenuItem.Size = Util.ScaleSize(new Size(138, 22));
      this.editCodeToolStripMenuItem.Text = "Edit Code";
      this.editCodeToolStripMenuItem.Click += new EventHandler(this.editCodeToolStripMenuItem_Click);
      this.deleteCodeToolStripMenuItem.Name = "deleteCodeToolStripMenuItem";
      this.deleteCodeToolStripMenuItem.Size = Util.ScaleSize(new Size(138, 22));
      this.deleteCodeToolStripMenuItem.Text = "Delete Code";
      this.deleteCodeToolStripMenuItem.Click += new EventHandler(this.deleteCodeToolStripMenuItem_Click);
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.lblProfile);
      this.panel1.Controls.Add((Control) this.cbProfile);
      this.panel1.Controls.Add((Control) this.btnApplyCodes);
      this.panel1.Controls.Add((Control) this.dgCheatCodes);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.dgCheats);
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Controls.Add((Control) this.btnApply);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(11));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(634, 277));
      this.panel1.TabIndex = 1;
      this.lblProfile.Anchor = AnchorStyles.Bottom;
      this.lblProfile.AutoSize = true;
      this.lblProfile.ForeColor = System.Drawing.Color.White;
      this.lblProfile.Location = new Point(Util.ScaleSize(72), Util.ScaleSize(250));
      this.lblProfile.Name = "lblProfile";
      this.lblProfile.Size = Util.ScaleSize(new Size(39, 13));
      this.lblProfile.TabIndex = 17;
      this.lblProfile.Text = "Profile:";
      this.cbProfile.Anchor = AnchorStyles.Bottom;
      this.cbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProfile.FormattingEnabled = true;
      this.cbProfile.Location = new Point(Util.ScaleSize(118), Util.ScaleSize(246));
      this.cbProfile.Name = "cbProfile";
      this.cbProfile.Size = Util.ScaleSize(new Size(112, 21));
      this.cbProfile.TabIndex = 16;
      this.btnApplyCodes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnApplyCodes.Location = new Point(Util.ScaleSize(551), Util.ScaleSize(175));
      this.btnApplyCodes.Name = "btnApplyCodes";
      this.btnApplyCodes.Size = Util.ScaleSize(new Size(75, 23));
      this.btnApplyCodes.TabIndex = 15;
      this.btnApplyCodes.Text = "Apply";
      this.btnApplyCodes.UseVisualStyleBackColor = true;
      this.btnApplyCodes.Visible = false;
      this.dgCheatCodes.AllowUserToAddRows = false;
      this.dgCheatCodes.AllowUserToDeleteRows = false;
      this.dgCheatCodes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dgCheatCodes.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = Util.CurrentPlatform != Util.Platform.MacOS ? SystemColors.Control : System.Drawing.Color.White;
      gridViewCellStyle1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgCheatCodes.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgCheatCodes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgCheatCodes.Columns.AddRange((DataGridViewColumn) this.ColLocation, (DataGridViewColumn) this.Value);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = Util.CurrentPlatform != Util.Platform.MacOS ? SystemColors.Window : System.Drawing.Color.White;
      gridViewCellStyle2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgCheatCodes.DefaultCellStyle = gridViewCellStyle2;
      this.dgCheatCodes.Location = new Point(Util.ScaleSize(539), Util.ScaleSize(37));
      this.dgCheatCodes.Name = "dgCheatCodes";
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dgCheatCodes.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dgCheatCodes.Size = new Size(Util.ScaleSize(35), Util.ScaleSize(48));
      this.dgCheatCodes.TabIndex = 14;
      this.dgCheatCodes.Visible = false;
      this.dgCheatCodes.BackgroundColor = System.Drawing.Color.White;
      this.dgCheatCodes.BackColor = System.Drawing.Color.White;
      this.ColLocation.HeaderText = "Location";
      this.ColLocation.Name = "Location";
      this.ColLocation.Width = Util.ScaleSize(70);
      this.Value.HeaderText = "Value";
      this.Value.MaxInputLength = 13;
      this.Value.Name = "Value";
      this.Value.Width = Util.ScaleSize(70);
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(Util.ScaleSize(452), Util.ScaleSize(-1));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(71, 13));
      this.label1.TabIndex = 13;
      this.label1.Text = "Cheat Codes:";
      this.label1.Visible = false;
      this.dgCheats.AllowUserToAddRows = false;
      this.dgCheats.AllowUserToDeleteRows = false;
      this.dgCheats.AllowUserToResizeRows = false;
      this.dgCheats.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dgCheats.BackgroundColor = System.Drawing.Color.FromArgb(175, 175, 175);
      this.dgCheats.BorderStyle = BorderStyle.None;
      this.dgCheats.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle4.BackColor = SystemColors.Control;
      gridViewCellStyle4.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle4.WrapMode = DataGridViewTriState.True;
      this.dgCheats.ColumnHeadersDefaultCellStyle = gridViewCellStyle4;
      this.dgCheats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgCheats.Columns.AddRange((DataGridViewColumn) this.ColSelect, (DataGridViewColumn) this.Description, (DataGridViewColumn) this.Comment);
      this.dgCheats.ContextMenuStrip = this.contextMenuStrip1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.False;
      this.dgCheats.DefaultCellStyle = gridViewCellStyle5;
      this.dgCheats.GridColor = System.Drawing.Color.FromArgb(175, 175, 175);
      this.dgCheats.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(13));
      this.dgCheats.Name = "dgCheats";
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle6.BackColor = SystemColors.Control;
      gridViewCellStyle6.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
      gridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.dgCheats.RowHeadersDefaultCellStyle = gridViewCellStyle6;
      this.dgCheats.RowHeadersVisible = false;
      this.dgCheats.RowHeadersWidth = Util.ScaleSize(25);
      this.dgCheats.RowTemplate.Height = Util.ScaleSize(24);
      this.dgCheats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgCheats.Size = Util.ScaleSize(new Size(610, 222));
      this.dgCheats.TabIndex = 12;
      this.dgCheats.BackgroundColor = System.Drawing.Color.White;
      this.dgCheats.BackColor = System.Drawing.Color.White;
      this.ColSelect.HeaderText = PS3SaveEditor.Resources.Resources.btnSaves;
      this.ColSelect.Name = "Select";
      this.ColSelect.Width = Util.ScaleSize(30);
      this.Description.HeaderText = "Description";
      this.Description.Name = "Description";
      this.Description.ReadOnly = true;
      this.Description.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Description.Width = Util.ScaleSize(240);
      this.Comment.HeaderText = "Comment";
      this.Comment.Name = "Comment";
      this.Comment.ReadOnly = true;
      this.Comment.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Comment.Width = Util.ScaleSize(340);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.btnClose.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnClose.ForeColor = System.Drawing.Color.White;
      this.btnClose.Location = new Point(Util.ScaleSize(318), Util.ScaleSize(246));
      this.btnClose.MaximumSize = Util.ScaleSize(new Size(60, 23));
      this.btnClose.MinimumSize = Util.ScaleSize(new Size(60, 23));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(60, 23));
      this.btnClose.TabIndex = 11;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.btnApply.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnApply.ForeColor = System.Drawing.Color.White;
      this.btnApply.Location = new Point(Util.ScaleSize(254), Util.ScaleSize(246));
      this.btnApply.MaximumSize = Util.ScaleSize(new Size(60, 23));
      this.btnApply.MinimumSize = Util.ScaleSize(new Size(60, 23));
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = Util.ScaleSize(new Size(60, 23));
      this.btnApply.TabIndex = 10;
      this.btnApply.Text = "Patch && Download Save";
      this.btnApply.UseVisualStyleBackColor = false;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(654, 298));
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblGameName);
      this.ForeColor = System.Drawing.Color.Black;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.MinimumSize = Util.ScaleSize(new Size(550, 336));
      this.Name = nameof (SimpleEdit);
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Simple Edit";
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.dgCheatCodes).EndInit();
      ((ISupportInitialize) this.dgCheats).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
