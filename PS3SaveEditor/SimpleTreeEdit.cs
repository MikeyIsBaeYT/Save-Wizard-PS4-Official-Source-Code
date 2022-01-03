// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SimpleTreeEdit
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using BrightIdeasSoftware;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace PS3SaveEditor
{
  public class SimpleTreeEdit : Form
  {
    private game m_game;
    private bool m_bCheatsModified = false;
    private bool m_bShowOnly = false;
    private TreeListView treeListView;
    private List<string> m_gameFiles;
    private IContainer components = (IContainer) null;
    private Label lblGameName;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem editCodeToolStripMenuItem;
    private ToolStripMenuItem deleteCodeToolStripMenuItem;
    private Panel panel1;
    private Button btnClose;
    private Button btnApply;
    private ComboBox cbProfile;
    private Label lblProfile;
    private Panel panel2;
    private ImageList imageList1;
    private ToolStripMenuItem addCodeToolStripMenuItem;

    public game GameItem => this.m_game;

    public SimpleTreeEdit(game gameItem, bool bShowOnly, List<string> files = null)
    {
      this.m_bShowOnly = bShowOnly;
      this.m_gameFiles = files;
      this.m_game = game.Copy(gameItem);
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.btnApply.Enabled = false;
      this.SetLabels();
      this.CenterToScreen();
      this.lblProfile.Visible = false;
      this.cbProfile.Visible = false;
      this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblProfile.Visible = false;
      this.cbProfile.Visible = false;
      this.lblGameName.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblGameName.ForeColor = System.Drawing.Color.White;
      this.lblGameName.Visible = false;
      this.btnApply.BackColor = SystemColors.ButtonFace;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnClose.BackColor = SystemColors.ButtonFace;
      this.btnClose.ForeColor = System.Drawing.Color.Black;
      this.lblProfile.Text = PS3SaveEditor.Resources.Resources.lblProfile;
      this.lblGameName.Text = gameItem.name;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.treeListView = new TreeListView();
      this.panel2.Controls.Add((Control) this.treeListView);
      this.treeListView.Dock = DockStyle.Fill;
      this.treeListView.ItemChecked += new ItemCheckedEventHandler(this.treeListView_ItemChecked);
      this.treeListView.OwnerDraw = true;
      this.treeListView.DrawSubItem += new DrawListViewSubItemEventHandler(this.treeListView_DrawSubItem);
      this.treeListView.RowHeight = 20;
      this.treeListView.BorderStyle = BorderStyle.FixedSingle;
      this.treeListView.GridLines = false;
      this.Resize += new EventHandler(this.SimpleTreeEdit_Resize);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.addCodeToolStripMenuItem.Click += new EventHandler(this.addCodeToolStripMenuItem_Click);
      this.m_bShowOnly = bShowOnly;
      this.FillTree(bShowOnly);
      this.SimpleTreeEdit_Resize((object) null, (EventArgs) null);
    }

    private void treeListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, System.Drawing.Color.FromArgb(0, 138, 213), System.Drawing.Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void SimpleTreeEdit_Resize(object sender, EventArgs e)
    {
      this.btnApply.Left = this.panel1.Width / 2 - this.btnApply.Width - 1;
      this.btnClose.Left = this.panel1.Width / 2 + 1;
      if (this.treeListView.Items.Count > this.treeListView.Height / this.treeListView.RowHeight)
      {
        this.treeListView.Columns[0].Width = (this.treeListView.Columns[0] as OLVColumn).MinimumWidth = (this.treeListView.Width - SystemInformation.VerticalScrollBarWidth - 2) / 2;
        this.treeListView.Columns[1].Width = (this.treeListView.Columns[1] as OLVColumn).MinimumWidth = (this.treeListView.Width - SystemInformation.VerticalScrollBarWidth - 2) / 2;
      }
      else
      {
        this.treeListView.Columns[0].Width = (this.treeListView.Columns[0] as OLVColumn).MinimumWidth = (this.treeListView.Width - 2) / 2;
        this.treeListView.Columns[1].Width = (this.treeListView.Columns[1] as OLVColumn).MinimumWidth = (this.treeListView.Width - 2) / 2;
      }
      this.Invalidate(true);
    }

    private void treeListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (e.Item.Text == PS3SaveEditor.Resources.Resources.lblNoCheats || !(this.treeListView.GetModelObject(e.Item.Index) is cheat))
      {
        e.Item.Checked = false;
      }
      else
      {
        this.btnApply.Enabled = false;
        foreach (object checkedObject in (IEnumerable) this.treeListView.CheckedObjects)
        {
          if (checkedObject is cheat)
          {
            this.btnApply.Enabled = true;
            break;
          }
        }
        if (!e.Item.Checked || this.treeListView.CheckedObjects.Count <= 0 || !(this.treeListView.GetParent(this.treeListView.GetModelObject(e.Item.Index)) is group parent2) || !(parent2.type == "1"))
          return;
        foreach (object child in this.treeListView.GetChildren((object) parent2))
        {
          if (child != this.treeListView.GetModelObject(e.Item.Index))
            this.treeListView.ModelToItem(child).Checked = false;
        }
      }
    }

    private void FillTree(bool showOnly)
    {
      this.treeListView.CanExpandGetter = (TreeListView.CanExpandGetterDelegate) (x =>
      {
        switch (x)
        {
          case file _:
            return true;
          case group _:
            group group = x as group;
            return group.cheats.Count > 0 || group._group != null;
          default:
            return false;
        }
      });
      this.treeListView.CheckBoxes = !showOnly;
      this.BackColor = System.Drawing.Color.White;
      this.treeListView.UseCustomSelectionColors = true;
      this.treeListView.HighlightBackgroundColor = System.Drawing.Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.treeListView.RowFormatter = (RowFormatterDelegate) (olvItem =>
      {
        if (olvItem.Selected)
        {
          olvItem.BackColor = System.Drawing.Color.FromArgb(0, 175, (int) byte.MaxValue);
        }
        else
        {
          olvItem.UseItemStyleForSubItems = true;
          if (olvItem.RowObject is file)
            olvItem.BackColor = System.Drawing.Color.White;
          if (!(olvItem.RowObject is string))
            return;
          olvItem.Font = new Font(olvItem.Font, FontStyle.Italic);
        }
      });
      this.treeListView.PrimarySortOrder = SortOrder.None;
      this.treeListView.SecondarySortOrder = SortOrder.None;
      this.treeListView.UseCellFormatEvents = true;
      this.treeListView.FormatCell += new EventHandler<FormatCellEventArgs>(this.treeListView_FormatCell);
      this.treeListView.ChildrenGetter = (TreeListView.ChildrenGetterDelegate) (x =>
      {
        ArrayList arrayList = new ArrayList();
        switch (x)
        {
          case file file2:
            if (file2.TotalCheats == 0)
              arrayList.Add((object) PS3SaveEditor.Resources.Resources.lblNoCheats);
            if (file2.Cheats.Count > 0)
            {
              foreach (cheat cheat in file2.Cheats)
              {
                if (cheat.id != "-1")
                  arrayList.Add((object) cheat);
              }
            }
            if (file2.groups.Count > 0)
              arrayList.AddRange((ICollection) file2.groups);
            if (file2.ucfilename == file2.filename || file2.ucfilename == null || Util.IsMatch(file2.ucfilename, file2.filename))
            {
              using (List<cheat>.Enumerator enumerator = file2.Cheats.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  cheat current = enumerator.Current;
                  if (current.id == "-1")
                    arrayList.Add((object) current);
                }
                break;
              }
            }
            else
              break;
          case group _:
            group group = x as group;
            arrayList.AddRange((ICollection) group.cheats);
            if (group._group != null)
              arrayList.AddRange((ICollection) group._group);
            break;
        }
        return (IEnumerable) arrayList;
      });
      OLVColumn olvColumn1 = new OLVColumn(PS3SaveEditor.Resources.Resources.colName, "Name");
      olvColumn1.AspectGetter = (AspectGetterDelegate) (x =>
      {
        switch (x)
        {
          case file _:
            return (object) (x as file).VisibleFileName;
          case group _:
            return (object) (x as group).name;
          case cheat _:
            return (object) (x as cheat).name;
          case string _:
            return x;
          default:
            return (object) ("!Missing!" + x);
        }
      });
      this.treeListView.FullRowSelect = true;
      olvColumn1.Width = 300;
      OLVColumn olvColumn2 = new OLVColumn(PS3SaveEditor.Resources.Resources.colComment, "Description");
      olvColumn2.AspectGetter = (AspectGetterDelegate) (x =>
      {
        switch (x)
        {
          case cheat _:
            return (object) (x as cheat).note;
          case group _:
            return (object) (x as group).note;
          default:
            return (object) "";
        }
      });
      olvColumn2.Width = 300;
      this.treeListView.Columns.Add((ColumnHeader) olvColumn1);
      this.treeListView.Columns.Add((ColumnHeader) olvColumn2);
      if (showOnly)
      {
        List<file> fileList = new List<file>();
        for (int index = 0; index < this.m_game.containers._containers.Count; ++index)
        {
          if (this.m_game.containers._containers[index].GetAllCheats().Count > 0)
            fileList.AddRange((IEnumerable<file>) this.m_game.containers._containers[index].files._files);
        }
        this.treeListView.Roots = fileList.Count <= 0 ? (IEnumerable) this.m_game.containers._containers[0].files._files : (IEnumerable) fileList;
        for (int index = 0; index < this.m_game.containers._containers.Count; ++index)
        {
          foreach (object file3 in this.m_game.containers._containers[index].files._files)
            this.treeListView.Expand(file3);
        }
      }
      else
      {
        List<file> fileList = new List<file>();
        container targetGameFolder = this.m_game.GetTargetGameFolder();
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
              file file4 = file.Copy(gameFile2);
              file4.original_filename = file4.filename;
              file4.filename = gameFile1;
              targetGameFolder.files._files.Add(file4);
            }
          }
          targetGameFolder.files._files.Sort((Comparison<file>) ((f1, f2) => f1.VisibleFileName.CompareTo(f2.VisibleFileName)));
        }
        MainForm.FillLocalCheats(ref this.m_game);
        foreach (file file5 in targetGameFolder.files._files)
        {
          if (!file5.IsHidden || file5.internals != null && file5.internals.files != null && file5.internals.files.Count != 0)
          {
            if (file5.internals != null)
            {
              foreach (file file6 in file5.internals.files)
                fileList.Add(file6);
            }
            if (!file5.IsHidden)
              fileList.Add(file5);
          }
        }
        this.treeListView.Roots = (IEnumerable) fileList;
        fileList.Reverse();
        foreach (object model in fileList)
          this.treeListView.Expand(model);
        if (this.treeListView.Items.Count > 0)
          this.treeListView.GetItem(0).EnsureVisible();
      }
      this.treeListView.BeforeSorting += new EventHandler<BeforeSortingEventArgs>(this.treeListView_BeforeSorting);
      this.treeListView.CellClick += new EventHandler<CellClickEventArgs>(this.treeListView_CellClick);
      this.treeListView.Expanded += new EventHandler<TreeBranchExpandedEventArgs>(this.treeListView_ExpandedCollapsed);
      this.treeListView.Collapsed += new EventHandler<TreeBranchCollapsedEventArgs>(this.treeListView_ExpandedCollapsed);
      this.treeListView.MultiSelect = false;
      this.treeListView.ShowItemToolTips = true;
      this.treeListView.AutoSizeColumns();
      this.treeListView.LowLevelScroll(0, 0);
    }

    private void treeListView_ExpandedCollapsed(object sender, EventArgs e)
    {
      if (this.treeListView.Items.Count > this.treeListView.Height / this.treeListView.RowHeight)
      {
        this.treeListView.Columns[0].Width = (this.treeListView.Columns[0] as OLVColumn).MinimumWidth = (this.treeListView.Width - SystemInformation.VerticalScrollBarWidth - 2) / 2;
        this.treeListView.Columns[1].Width = (this.treeListView.Columns[1] as OLVColumn).MinimumWidth = (this.treeListView.Width - SystemInformation.VerticalScrollBarWidth - 2) / 2;
      }
      else
      {
        this.treeListView.Columns[0].Width = (this.treeListView.Columns[0] as OLVColumn).MinimumWidth = (this.treeListView.Width - 2) / 2;
        this.treeListView.Columns[1].Width = (this.treeListView.Columns[1] as OLVColumn).MinimumWidth = (this.treeListView.Width - 2) / 2;
      }
    }

    private void treeListView_CellClick(object sender, CellClickEventArgs e)
    {
      if (e.Item != null && e.Item.Text == "Add Cheat...")
        this.addCodeToolStripMenuItem_Click((object) null, (EventArgs) null);
      else if (e.Model is group)
      {
        if (this.treeListView.IsExpanded(e.Model))
          this.treeListView.Collapse(e.Model);
        else
          this.treeListView.Expand(e.Model);
      }
    }

    private void treeListView_BeforeSorting(object sender, BeforeSortingEventArgs e) => e.Canceled = true;

    private void treeListView_FormatCell(object sender, FormatCellEventArgs e)
    {
      if (!(e.Model is cheat) || !((e.Model as cheat).id == "-1"))
        return;
      e.Item.ForeColor = System.Drawing.Color.Blue;
    }

    private void dgCheats_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.ColumnIndex != 0)
        ;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 274 && ((int) m.WParam == 61728 || (int) m.WParam == 61488))
        this.Invalidate(true);
      base.WndProc(ref m);
    }

    private void dgCheats_MouseDown(object sender, MouseEventArgs e)
    {
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
      this.addCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuAddCheatCode;
      this.editCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuEditCheatCode;
      this.deleteCodeToolStripMenuItem.Text = PS3SaveEditor.Resources.Resources.mnuDeleteCheatCode;
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
    }

    private void RefreshValue()
    {
    }

    private void AddDependencies(file f, List<string> saveFiles)
    {
      if (f.dependency == null)
        return;
      saveFiles.Add(f.GetDependencyFile(this.m_game.GetTargetGameFolder(), this.m_game.LocalSaveFolder));
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      bool flag = false;
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        List<string> files = new List<string>();
        foreach (object checkedObject in (IEnumerable) this.treeListView.CheckedObjects)
        {
          flag = true;
          if (checkedObject is cheat cheat4)
          {
            cheat4.Selected = true;
            object model = checkedObject;
            do
            {
              model = this.treeListView.GetParent(model);
            }
            while (!(model is file file6));
            if (files.IndexOf(file6.filename) < 0)
              files.Add(file6.filename);
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

    private void btnApply_Click1(object sender, EventArgs e)
    {
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (targetGameFolder == null)
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        bool flag = false;
        List<string> stringList = new List<string>();
        stringList.Add(Path.Combine(this.m_game.LocalSaveFolder, "PARAM.PFD"));
        stringList.Add(Path.Combine(this.m_game.LocalSaveFolder, "PARAM.SFO"));
        foreach (object checkedObject in (IEnumerable) this.treeListView.CheckedObjects)
        {
          flag = true;
          cheat cheat = checkedObject as cheat;
          this.m_game.GetCheat(cheat.id, cheat.name).Selected = true;
          object model = (object) cheat;
          do
          {
            model = this.treeListView.GetParent(model);
          }
          while (!(model is file));
          file file1 = model as file;
          string str1;
          if (file1.GetParent(targetGameFolder) != null)
          {
            string saveFile = file1.GetParent(targetGameFolder).GetSaveFile(this.m_game.LocalSaveFolder);
            if (saveFile != null)
            {
              string str2 = Path.Combine(this.m_game.LocalSaveFolder, saveFile);
              if (stringList.IndexOf(str2) < 0)
              {
                stringList.Add(str2);
                this.AddDependencies(file1.GetParent(targetGameFolder), stringList);
              }
              str1 = Path.Combine(this.m_game.LocalSaveFolder, file1.filename);
            }
            else
              goto label_24;
          }
          else
          {
            string saveFile = file1.GetSaveFile(this.m_game.LocalSaveFolder);
            if (saveFile != null)
              str1 = Path.Combine(this.m_game.LocalSaveFolder, saveFile);
            else
              goto label_24;
          }
          if (stringList.IndexOf(str1) < 0)
          {
            stringList.Add(str1);
            if (file1.internals != null)
            {
              foreach (file file2 in file1.internals.files)
              {
                string saveFile = file2.GetSaveFile(this.m_game.LocalSaveFolder);
                if (stringList.IndexOf(saveFile) < 0)
                  stringList.Add(saveFile);
              }
            }
          }
label_24:;
        }
        if (!flag)
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgSelectCheat, PS3SaveEditor.Resources.Resources.msgError);
        }
        else
        {
          if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnOverwrite, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return;
          if (new SimpleSaveUploader(this.m_game, (string) this.cbProfile.SelectedItem, stringList).ShowDialog() == DialogResult.OK)
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

    private void addCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.treeListView.SelectedObjects.Count == 0)
        return;
      List<string> existingCodes = new List<string>();
      foreach (file file in this.m_game.GetTargetGameFolder().files._files)
      {
        foreach (cheat cheat in file.Cheats)
          existingCodes.Add(cheat.name);
      }
      AddCode addCode = new AddCode(existingCodes);
      if (addCode.ShowDialog() != DialogResult.OK)
        return;
      this.m_bCheatsModified = true;
      cheat cheat1 = new cheat("-1", addCode.Description, addCode.Comment);
      cheat1.code = addCode.Code;
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        if (!(this.treeListView.SelectedObjects[0] is file file2))
        {
          int selectedIndex = this.treeListView.SelectedIndex;
          do
          {
            if (!(this.treeListView.GetModelObject(selectedIndex) is file))
              --selectedIndex;
            else
              goto label_16;
          }
          while (selectedIndex >= 0);
          goto label_20;
label_16:
          file2 = this.treeListView.GetModelObject(selectedIndex) as file;
label_20:;
        }
        if (file2 != null)
        {
          file2.Cheats.Add(cheat1);
          this.treeListView.RefreshObject((object) file2);
          this.SaveUserCheats();
        }
      }
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

    private void SaveUserCheats2()
    {
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errNoSavedata, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        string xml = "<usercheats></usercheats>";
        string str = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + MainForm.USER_CHEATS_FILE;
        if (File.Exists(str))
          xml = File.ReadAllText(str);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
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
              if (file.GetParent(targetGameFolder) != null)
                element1.SetAttribute("name", file.filename);
              else
                element1.SetAttribute("name", Path.GetFileName(file.GetSaveFile(this.m_game.LocalSaveFolder)));
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
      if (!(this.treeListView.SelectedObjects[0] is cheat selectedObject))
        return;
      List<string> existingCodes = new List<string>();
      foreach (file file in this.m_game.GetTargetGameFolder().files._files)
      {
        foreach (cheat cheat in file.Cheats)
        {
          if (cheat.name != selectedObject.name)
            existingCodes.Add(cheat.name);
        }
      }
      AddCode addCode = new AddCode(selectedObject, existingCodes);
      if (addCode.ShowDialog() == DialogResult.OK)
      {
        selectedObject.code = addCode.Code;
        selectedObject.name = addCode.Description;
        selectedObject.note = addCode.Comment;
        this.treeListView.RefreshObject((object) selectedObject);
        foreach (file file in this.m_game.GetTargetGameFolder().files._files)
        {
          foreach (cheat cheat in file.Cheats)
          {
            if (cheat.name == selectedObject.name && cheat.id == "-1")
            {
              cheat.code = selectedObject.code;
              break;
            }
          }
        }
        this.SaveUserCheats();
        this.m_bCheatsModified = true;
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      if (this.m_game.GetTargetGameFolder() == null)
      {
        this.editCodeToolStripMenuItem.Enabled = false;
        this.deleteCodeToolStripMenuItem.Enabled = false;
        this.addCodeToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.addCodeToolStripMenuItem.Enabled = false;
        this.editCodeToolStripMenuItem.Enabled = false;
        this.deleteCodeToolStripMenuItem.Enabled = false;
        if (this.treeListView.SelectedObjects.Count == 1)
        {
          int? quickmode = this.m_game.GetTargetGameFolder().quickmode;
          int num = 0;
          if (quickmode.GetValueOrDefault() > num && quickmode.HasValue)
          {
            e.Cancel = true;
          }
          else
          {
            int selectedIndex = this.treeListView.SelectedIndex;
            object selectedObject = this.treeListView.SelectedObjects[0];
            if (selectedObject is cheat && (selectedObject as cheat).id == "-1")
            {
              cheat cheat = selectedObject as cheat;
              this.editCodeToolStripMenuItem.Enabled = cheat.id == "-1";
              this.deleteCodeToolStripMenuItem.Enabled = cheat.id == "-1";
            }
            if (!(selectedObject is file) && !(selectedObject is cheat))
              return;
            this.addCodeToolStripMenuItem.Enabled = true;
          }
        }
        else
          e.Cancel = true;
      }
    }

    private void dgCheats_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
    }

    private void deleteCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmDelete, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo) != DialogResult.Yes || !(this.treeListView.SelectedObjects[0] is cheat selectedObject))
        return;
      object parent = this.treeListView.GetParent((object) selectedObject);
      if (parent is file)
      {
        file file = parent as file;
        file.Cheats.Remove(selectedObject);
        this.treeListView.RefreshObject((object) file);
        this.SaveUserCheats();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SimpleTreeEdit));
      this.lblGameName = new Label();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.addCodeToolStripMenuItem = new ToolStripMenuItem();
      this.editCodeToolStripMenuItem = new ToolStripMenuItem();
      this.deleteCodeToolStripMenuItem = new ToolStripMenuItem();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.lblProfile = new Label();
      this.cbProfile = new ComboBox();
      this.btnClose = new Button();
      this.btnApply = new Button();
      this.imageList1 = new ImageList(this.components);
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
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
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.lblProfile);
      this.panel1.Controls.Add((Control) this.cbProfile);
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Controls.Add((Control) this.btnApply);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(11));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(633, 276));
      this.panel1.TabIndex = 1;
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.BackColor = SystemColors.AppWorkspace;
      this.panel2.ContextMenuStrip = this.contextMenuStrip1;
      this.panel2.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(13));
      this.panel2.Name = "panel2";
      this.panel2.Size = Util.ScaleSize(new Size(609, 230));
      this.panel2.TabIndex = 18;
      this.lblProfile.Anchor = AnchorStyles.Bottom;
      this.lblProfile.AutoSize = true;
      this.lblProfile.ForeColor = System.Drawing.Color.White;
      this.lblProfile.Location = new Point(Util.ScaleSize(71), Util.ScaleSize(249));
      this.lblProfile.Name = "lblProfile";
      this.lblProfile.Size = Util.ScaleSize(new Size(39, 13));
      this.lblProfile.TabIndex = 17;
      this.lblProfile.Text = "Profile:";
      this.cbProfile.Anchor = AnchorStyles.Bottom;
      this.cbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProfile.FormattingEnabled = true;
      this.cbProfile.Location = new Point(Util.ScaleSize(117), Util.ScaleSize(245));
      this.cbProfile.Name = "cbProfile";
      this.cbProfile.Size = Util.ScaleSize(new Size(112, 21));
      this.cbProfile.TabIndex = 16;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.btnClose.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnClose.ForeColor = System.Drawing.Color.White;
      this.btnClose.Location = new Point(Util.ScaleSize(315), Util.ScaleSize(248));
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
      this.btnApply.Location = new Point(Util.ScaleSize((int) byte.MaxValue), Util.ScaleSize(248));
      this.btnApply.MaximumSize = Util.ScaleSize(new Size(60, 23));
      this.btnApply.MinimumSize = Util.ScaleSize(new Size(60, 23));
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = Util.ScaleSize(new Size(60, 23));
      this.btnApply.TabIndex = 10;
      this.btnApply.Text = "Patch && Download Save";
      this.btnApply.UseVisualStyleBackColor = false;
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "file.png");
      this.imageList1.Images.SetKeyName(1, "group.png");
      this.imageList1.Images.SetKeyName(2, "cheat.png");
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(654, 294));
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblGameName);
      this.ForeColor = System.Drawing.Color.Black;
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.MinimumSize = Util.ScaleSize(new Size(550, 330));
      this.Name = nameof (SimpleTreeEdit);
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Simple Edit";
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
