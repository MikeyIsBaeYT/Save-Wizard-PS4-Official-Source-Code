// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.AdvancedEdit
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Be.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class AdvancedEdit : Form
  {
    private DynamicByteProvider provider;
    private game m_game;
    private bool m_bTextMode;
    private Dictionary<string, byte[]> m_saveFilesData;
    private List<string> m_DirtyFiles;
    private string m_cursaveFile;
    private bool _resizeInProgress = false;
    private int _previousSelectionIndex = -1;
    private Dictionary<string, Stack<ActionItem>> m_undoList = new Dictionary<string, Stack<ActionItem>>();
    private Dictionary<string, Stack<ActionItem>> m_redoList = new Dictionary<string, Stack<ActionItem>>();
    private PS3SaveEditor.Search m_searchForm;
    private IContainer components = (IContainer) null;
    private HexBox hexBox1;
    private Label lblCheatCodes;
    private Label lblCheats;
    private Button btnApply;
    private Label lblOffset;
    private Label lblOffsetValue;
    private Panel panel1;
    private ListBox lstCheats;
    private ListBox lstValues;
    private Label lblGameName;
    private Button btnClose;
    private Label label1;
    private Button btnFindPrev;
    private Button btnFind;
    private Label lblAddress;
    private Label lblDataHex;
    private Label lblDataAscii;
    private ComboBox cbProfile;
    private Label lblProfile;
    private RichTextBox txtSaveData;
    private ComboBox cbSaveFiles;
    private ToolStrip toolStrip1;
    private ToolStripButton toolStripButtonSearch;
    private ToolStripButton toolStripButtonUndo;
    private ToolStripButton toolStripButtonRedo;
    private ToolStripButton toolStripButtonGoto;
    private ToolStripButton toolStripButtonExport;
    private Label lblCurrentFile;
    private Label lblLengthVal;
    private Label lblLength;
    private ToolStripButton toolStripButtonImportFile;

    public bool TextMode
    {
      get => this.m_bTextMode;
      set
      {
        this.m_searchForm.TextMode = value;
        this.m_bTextMode = value;
      }
    }

    public AdvancedEdit(game game, Dictionary<string, byte[]> data)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.KeyDown += new KeyEventHandler(this.AdvancedEdit_KeyDown);
      this.btnFindPrev.Click += new EventHandler(this.button1_Click);
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.hexBox1.KeyDown += new KeyEventHandler(this.hexBox1_KeyDown);
      this.hexBox1.SelectionBackColor = System.Drawing.Color.FromArgb(0, 175, (int) byte.MaxValue);
      this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(204, 240, (int) byte.MaxValue);
      this.lstCheats.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstCheats.DrawItem += new DrawItemEventHandler(this.lstCheats_DrawItem);
      this.lstValues.DrawMode = DrawMode.OwnerDrawFixed;
      this.lstValues.DrawItem += new DrawItemEventHandler(this.lstValues_DrawItem);
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
      this.m_searchForm = new PS3SaveEditor.Search(this);
      this.TextMode = false;
      this.btnApply.BackColor = SystemColors.ButtonFace;
      this.btnApply.ForeColor = System.Drawing.Color.Black;
      this.btnClose.BackColor = SystemColors.ButtonFace;
      this.btnClose.ForeColor = System.Drawing.Color.Black;
      this.btnFind.BackColor = SystemColors.ButtonFace;
      this.btnFind.ForeColor = System.Drawing.Color.Black;
      this.btnFindPrev.BackColor = SystemColors.ButtonFace;
      this.btnFindPrev.ForeColor = System.Drawing.Color.Black;
      this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.label1.BackColor = this.lblAddress.BackColor = this.lblCheatCodes.BackColor = this.lblCheats.BackColor = this.lblGameName.BackColor = this.lblOffset.BackColor = this.lblOffsetValue.BackColor = this.lblProfile.BackColor = System.Drawing.Color.Transparent;
      this.lblDataHex.BackColor = this.lblDataAscii.BackColor = System.Drawing.Color.Transparent;
      this.lblProfile.Visible = false;
      this.cbProfile.Visible = false;
      this.m_DirtyFiles = new List<string>();
      this.m_saveFilesData = data;
      this.btnFind.Text = PS3SaveEditor.Resources.Resources.btnFind;
      this.btnFindPrev.Text = PS3SaveEditor.Resources.Resources.btnFindPrev;
      this.lblProfile.Text = PS3SaveEditor.Resources.Resources.lblProfile;
      this.label1.Text = PS3SaveEditor.Resources.Resources.lblSearch;
      this.lblAddress.Text = PS3SaveEditor.Resources.Resources.lblAddressExtra;
      this.lblDataHex.Text = PS3SaveEditor.Resources.Resources.lblDataHex;
      this.lblDataAscii.Text = PS3SaveEditor.Resources.Resources.lblDataAscii;
      this.lblCurrentFile.Text = PS3SaveEditor.Resources.Resources.lblCurrentFile;
      this.lblLength.Text = PS3SaveEditor.Resources.Resources.lblLength;
      this.SetLabels();
      this.FillProfiles();
      this.lblGameName.Text = game.name;
      this.m_game = game;
      this.CenterToScreen();
      this.btnApply.Enabled = false;
      this.lstValues.SelectedIndexChanged += new EventHandler(this.lstValues_SelectedIndexChanged);
      this.lstCheats.SelectedIndexChanged += new EventHandler(this.lstCheats_SelectedIndexChanged);
      this.cbSaveFiles.SelectedIndexChanged += new EventHandler(this.cbSaveFiles_SelectedIndexChanged);
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (MainForm3.GetSysVer(this.m_game.LocalSaveFolder) == "All")
        this.toolStripButtonImportFile.Visible = false;
      if (targetGameFolder != null)
      {
        this.cbSaveFiles.Sorted = true;
        foreach (object key in data.Keys)
          this.cbSaveFiles.Items.Add(key);
        if (this.cbSaveFiles.Items.Count > 0)
          this.cbSaveFiles.SelectedIndex = 0;
      }
      if (this.cbSaveFiles.Items.Count == 1)
        this.cbSaveFiles.Enabled = false;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      if (this.lstCheats.Items.Count > 0)
        this.lstCheats.SelectedIndex = 0;
      this.ResizeBegin += (EventHandler) ((s, e) =>
      {
        this.SuspendLayout();
        this.panel1.BackColor = System.Drawing.Color.FromArgb(0, 138, 213);
        this._resizeInProgress = true;
      });
      this.ResizeEnd += (EventHandler) ((s, e) =>
      {
        this.ResumeLayout(true);
        this._resizeInProgress = false;
        this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
        this.Invalidate(true);
      });
      this.SizeChanged += (EventHandler) ((s, e) =>
      {
        if (this.WindowState != FormWindowState.Maximized)
          return;
        this._resizeInProgress = false;
        this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
        this.Invalidate(true);
      });
      this.Disposed += new EventHandler(this.AdvancedEdit_Disposed);
      this.toolStripButtonExport.Click += new EventHandler(this.toolStripButtonExport_Click);
      this.toolStripButtonGoto.Click += new EventHandler(this.toolStripButtonGoto_Click);
      this.toolStripButtonImportFile.Click += new EventHandler(this.toolStripButtonImportFile_Click);
      this.toolStripButtonUndo.Click += new EventHandler(this.toolStripButtonUndo_Click);
      this.toolStripButtonRedo.Click += new EventHandler(this.toolStripButtonRedo_Click);
      this.toolStripButtonSearch.Click += new EventHandler(this.toolStripButtonSearch_Click);
      this.cbSaveFiles.Width = Math.Min(200, this.ComboBoxWidth(this.cbSaveFiles));
    }

    private void toolStripButtonImportFile_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      byte[] data = File.ReadAllBytes(openFileDialog.FileName);
      if (data.Length < Util.GetMinFileSize() || data.Length > Util.GetMaxFileSize())
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errFileSizeOutOfRange);
      }
      else
      {
        if (this.m_DirtyFiles.IndexOf(this.m_cursaveFile) < 0)
          this.m_DirtyFiles.Add(this.m_cursaveFile);
        this.m_undoList[this.m_cursaveFile].Clear();
        this.hexBox1.DifferDict.Clear();
        this.provider = new DynamicByteProvider(data);
        this.provider.Changed += new EventHandler<ByteProviderChanged>(this.provider_Changed);
        this.hexBox1.ByteProvider = (IByteProvider) this.provider;
        this.hexBox1.Refresh();
        this.btnApply.Enabled = true;
      }
    }

    private void AdvancedEdit_Disposed(object sender, EventArgs e)
    {
      if (!this.m_searchForm.IsDisposed)
        this.m_searchForm.Dispose();
      if (!this.hexBox1.IsDisposed)
        this.hexBox1.Dispose();
      if (this.provider != null && this.provider.Bytes != null)
        this.provider.Bytes.Clear();
      this.m_saveFilesData.Clear();
      GC.Collect();
    }

    private void toolStripButtonSearch_Click(object sender, EventArgs e)
    {
      this.m_searchForm.Hide();
      this.m_searchForm.Show((IWin32Window) this);
    }

    private void toolStripButtonRedo_Click(object sender, EventArgs e)
    {
      if (this.m_redoList[this.m_cursaveFile].Count > 0)
      {
        ActionItem actionItem = this.m_redoList[this.m_cursaveFile].Pop();
        this.m_undoList[this.m_cursaveFile].Push(actionItem);
        this.hexBox1.ScrollByteIntoView(actionItem.Location);
        this.hexBox1.ByteProvider.WriteByte(actionItem.Location, actionItem.NewValue, true);
        this.hexBox1.Refresh();
      }
      this.toolStripButtonUndo.Enabled = this.m_undoList[this.m_cursaveFile].Count != 0;
      this.toolStripButtonRedo.Enabled = this.m_redoList[this.m_cursaveFile].Count != 0;
    }

    private void toolStripButtonUndo_Click(object sender, EventArgs e)
    {
      if (this.m_undoList[this.m_cursaveFile].Count > 0)
      {
        ActionItem actionItem = this.m_undoList[this.m_cursaveFile].Pop();
        this.m_redoList[this.m_cursaveFile].Push(actionItem);
        this.hexBox1.ScrollByteIntoView(actionItem.Location);
        this.hexBox1.ByteProvider.WriteByte(actionItem.Location, actionItem.Value, true);
        this.hexBox1.Refresh();
      }
      this.toolStripButtonUndo.Enabled = this.m_undoList[this.m_cursaveFile].Count != 0;
      this.toolStripButtonRedo.Enabled = this.m_redoList[this.m_cursaveFile].Count != 0;
    }

    private void toolStripButtonGoto_Click(object sender, EventArgs e) => this.DoGoTo();

    private void toolStripButtonExport_Click(object sender, EventArgs e)
    {
      byte[] bytes = this.m_saveFilesData[this.m_cursaveFile];
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.FileName = this.m_cursaveFile;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      File.WriteAllBytes(saveFileDialog.FileName, bytes);
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 274 && m.WParam == new IntPtr(61488))
      {
        this.panel1.BackColor = System.Drawing.Color.FromArgb(0, 138, 213);
        this._resizeInProgress = true;
      }
      base.WndProc(ref m);
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

    private void lstValues_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics1 = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        Graphics graphics2 = graphics1;
        SolidBrush solidBrush = new SolidBrush(System.Drawing.Color.FromArgb(72, 187, 97));
        Rectangle bounds = e.Bounds;
        int left = bounds.Left;
        bounds = e.Bounds;
        int top = bounds.Top;
        int width = this.lstValues.Width;
        bounds = e.Bounds;
        int height = bounds.Height;
        Rectangle rect = new Rectangle(left, top, width, height);
        graphics2.FillRectangle((Brush) solidBrush, rect);
        e.Graphics.DrawString((string) this.lstValues.Items[e.Index], e.Font, (Brush) new SolidBrush(System.Drawing.Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstValues.Items[e.Index], e.Font, (Brush) new SolidBrush(System.Drawing.Color.Black), (RectangleF) new Rectangle(e.Bounds.Left, e.Bounds.Top, this.lstValues.Width, e.Bounds.Height), StringFormat.GenericDefault);
      e.DrawFocusRectangle();
    }

    private void lstCheats_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      Graphics graphics = e.Graphics;
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
      {
        graphics.FillRectangle((Brush) new SolidBrush(System.Drawing.Color.FromArgb(0, 175, (int) byte.MaxValue)), e.Bounds);
        e.Graphics.DrawString((string) this.lstCheats.Items[e.Index], e.Font, (Brush) new SolidBrush(System.Drawing.Color.White), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      }
      else
        e.Graphics.DrawString((string) this.lstCheats.Items[e.Index], e.Font, (Brush) new SolidBrush(System.Drawing.Color.Black), (RectangleF) e.Bounds, StringFormat.GenericDefault);
      e.DrawFocusRectangle();
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
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, System.Drawing.Color.FromArgb(0, 138, 213), System.Drawing.Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void txtSaveData_TextChanged(object sender, EventArgs e)
    {
      file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.m_cursaveFile);
      string str = "";
      if (gameFile != null && gameFile.TextMode > 0)
        str = gameFile.TextMode != 1 ? (gameFile.TextMode != 3 ? Encoding.ASCII.GetString(this.m_saveFilesData[this.m_cursaveFile]) : Encoding.Unicode.GetString(this.m_saveFilesData[this.m_cursaveFile])) : Encoding.UTF8.GetString(this.m_saveFilesData[this.m_cursaveFile]);
      string[] strArray1 = str.Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
      string[] strArray2 = this.txtSaveData.Text.Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length == strArray2.Length)
      {
        bool flag = false;
        for (int index = 0; index < strArray1.Length; ++index)
        {
          if (strArray1[index] != strArray2[index])
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
      }
      this.btnApply.Enabled = true;
      if (this.m_DirtyFiles.IndexOf(this.m_cursaveFile) >= 0)
        return;
      this.m_DirtyFiles.Add(this.m_cursaveFile);
    }

    private void SetLabels()
    {
      this.lblOffset.Text = PS3SaveEditor.Resources.Resources.lblOffset;
      this.lblCheatCodes.Text = PS3SaveEditor.Resources.Resources.lblCodes;
      this.lblCheats.Text = PS3SaveEditor.Resources.Resources.lblCheats;
      this.btnApply.Text = PS3SaveEditor.Resources.Resources.btnApplyDownload;
      this.btnClose.Text = PS3SaveEditor.Resources.Resources.btnClose;
      this.Text = PS3SaveEditor.Resources.Resources.titleAdvEdit;
    }

    private void hexBox1_SelectionStartChanged(object sender, EventArgs e) => this.lblOffsetValue.Text = "0x" + string.Format("{0:X}", (object) ((long) this.hexBox1.BytesPerLine * (this.hexBox1.CurrentLine - 1L) + (this.hexBox1.CurrentPositionInLine - 1L))).PadLeft(8, '0');

    protected override void OnClosed(EventArgs e)
    {
      if (this.provider != null && this.provider.Bytes != null)
        this.provider.Bytes.Clear();
      if (!this.hexBox1.IsDisposed)
        this.hexBox1.Dispose();
      base.OnClosed(e);
    }

    private void provider_LengthChanged(object sender, EventArgs e)
    {
    }

    private void provider_Changed(object sender, ByteProviderChanged e)
    {
      this.btnApply.Enabled = true;
      if (!this.hexBox1.DifferDict.ContainsKey(e.Index))
        this.hexBox1.DifferDict[e.Index] = e.OldValue;
      if (this.m_DirtyFiles.IndexOf(this.m_cursaveFile) < 0)
        this.m_DirtyFiles.Add(this.m_cursaveFile);
      if (!this.m_undoList.ContainsKey(this.m_cursaveFile))
        this.m_undoList.Add(this.m_cursaveFile, new Stack<ActionItem>());
      this.m_undoList[this.m_cursaveFile].Push(new ActionItem()
      {
        Location = e.Index,
        Value = e.OldValue,
        NewValue = e.NewValue
      });
      this.toolStripButtonUndo.Enabled = true;
    }

    private void lstCheats_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.m_bTextMode)
        return;
      this.lstValues.Items.Clear();
      int selectedIndex = this.lstCheats.SelectedIndex;
      string a = this.cbSaveFiles.SelectedItem.ToString();
      if (selectedIndex < 0)
        return;
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (targetGameFolder != null)
      {
        foreach (file file in targetGameFolder.files._files)
        {
          List<string> saveFiles = this.m_game.GetSaveFiles();
          if (saveFiles != null)
          {
            foreach (string path in saveFiles)
            {
              if (Path.GetFileName(path) == a || Util.IsMatch(a, file.filename))
              {
                cheat cheat = file.GetCheat(this.lstCheats.Items[selectedIndex].ToString());
                if (cheat != null)
                {
                  string code = cheat.code;
                  if (!string.IsNullOrEmpty(code))
                  {
                    string[] strArray = code.Trim().Split(' ', '\r', '\n');
                    for (int index = 0; index < strArray.Length - 1; index += 2)
                      this.lstValues.Items.Add((object) (strArray[index] + " " + strArray[index + 1]));
                  }
                }
              }
            }
          }
        }
      }
    }

    private void lstValues_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstValues.SelectedIndex < 0 || this.m_bTextMode || this.lstValues.Items[0].ToString()[0] == 'F')
        return;
      this.hexBox1.SelectAddresses.Clear();
      int bitWriteCode;
      long memLocation = cheat.GetMemLocation(this.lstValues.Items[this.lstValues.SelectedIndex].ToString().Split(' ')[0], out bitWriteCode);
      if (this.provider.Length <= memLocation)
        return;
      this.hexBox1.SelectAddresses.Add(memLocation, cheat.GetBitCodeBytes(bitWriteCode));
      this.hexBox1.ScrollByteIntoView(memLocation);
      this.hexBox1.Invalidate();
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.warnOverwriteAdv, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      if (!this.m_bTextMode)
      {
        this.provider.ApplyChanges();
        if (this.m_cursaveFile == null)
          this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
        this.m_saveFilesData[this.m_cursaveFile] = this.provider.Bytes.ToArray();
      }
      else
      {
        file gameFile = this.m_game.GetGameFile(this.m_game.GetTargetGameFolder(), this.m_cursaveFile);
        this.m_saveFilesData[this.m_cursaveFile] = gameFile.TextMode != 1 ? (gameFile.TextMode != 3 ? Encoding.ASCII.GetBytes(this.txtSaveData.Text) : Encoding.Unicode.GetBytes(this.txtSaveData.Text)) : Encoding.UTF8.GetBytes(this.txtSaveData.Text);
      }
      if (this.m_game.GetTargetGameFolder() == null)
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errSaveData, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        this.m_game.GetTargetGameFolder();
        List<string> dirtyFiles = this.m_DirtyFiles;
        List<string> selectedSaveFiles = new List<string>();
        foreach (string path1 in dirtyFiles)
        {
          string path2 = Path.Combine(ZipUtil.GetPs3SeTempFolder(), "_file_" + Path.GetFileName(path1));
          File.WriteAllBytes(path2, this.m_saveFilesData[Path.GetFileName(path1)]);
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
        string selectedItem = (string) this.cbProfile.SelectedItem;
        if (new AdvancedSaveUploaderForEncrypt(selectedSaveFiles.ToArray(), this.m_game, selectedItem, "encrypt").ShowDialog() == DialogResult.OK)
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgAdvModeFinish, PS3SaveEditor.Resources.Resources.msgInfo);
        }
        File.Delete(path);
        Directory.Delete(ZipUtil.GetPs3SeTempFolder(), true);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
      this.Dispose();
    }

    private void txtSearchValue_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void hexBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3)
        return;
      this.Search(e.Shift, false, this.m_searchForm.GetSearchMode());
    }

    private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
        this.Search(false, true, this.m_searchForm.GetSearchMode());
      if (this.m_bTextMode || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back || e.Control || e.KeyCode == Keys.Home || e.KeyCode == Keys.End || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 && !e.Shift || this.m_searchForm.SearchText.SelectionStart == 1 && e.KeyCode == Keys.X && this.m_searchForm.SearchText.Text[0] == '0' || this.m_searchForm.SearchText.Text.StartsWith("0x") && e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)
        return;
      e.SuppressKeyPress = true;
    }

    public void Search(bool bBackward, bool bStart, SearchMode mode)
    {
      if (this.m_bTextMode)
      {
        this.SerachText(bBackward, bStart);
      }
      else
      {
        MemoryStream memoryStream = new MemoryStream((this.hexBox1.ByteProvider as DynamicByteProvider).Bytes.GetBytes());
        BinaryReader reader = new BinaryReader((Stream) memoryStream);
        if (bStart)
        {
          reader.BaseStream.Position = 0L;
          this.hexBox1.SelectionStart = 0L;
          this.hexBox1.SelectionLength = 0L;
        }
        else if (this.hexBox1.SelectionStart >= 0L)
          reader.BaseStream.Position = this.hexBox1.SelectionStart + this.hexBox1.SelectionLength;
        long position = reader.BaseStream.Position;
        uint val1;
        uint val2;
        byte[] val3;
        int searchValues = this.GetSearchValues(mode, out val1, out val2, out val3);
        this.lblLengthVal.Text = "0x" + string.Format("{0:X}", (object) searchValues).PadLeft(8, '0');
        if (searchValues == 0)
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidHex, PS3SaveEditor.Resources.Resources.msgError);
          this.m_searchForm.SearchText.Focus();
        }
        else if (searchValues < 0)
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errIncorrectValue, PS3SaveEditor.Resources.Resources.msgError);
          this.m_searchForm.SearchText.Focus();
        }
        else
        {
          for (; reader.BaseStream.Position >= 0L && reader.BaseStream.Position < reader.BaseStream.Length + (bBackward ? (long) searchValues : (long) (1 - searchValues)); reader.BaseStream.Position = position)
          {
            bool flag = true;
            uint num = 0;
            if (mode == SearchMode.Text)
            {
              byte[] buffer = new byte[searchValues];
              reader.BaseStream.Read(buffer, 0, searchValues);
              for (int index = 0; index < searchValues; ++index)
              {
                if ((int) buffer[index] != (int) val3[index])
                {
                  flag = false;
                  break;
                }
              }
            }
            else
            {
              flag = false;
              num = this.ReadValue(reader, searchValues, bBackward);
            }
            if (((mode == SearchMode.Text ? 0 : ((int) num == (int) val1 ? 1 : ((int) num == (int) val2 ? 1 : 0))) | (flag ? 1 : 0)) != 0)
            {
              this.hexBox1.Select(reader.BaseStream.Position - (long) searchValues, (long) searchValues);
              this.hexBox1.ScrollByteIntoView(reader.BaseStream.Position);
              this.hexBox1.Focus();
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
        int end = this.txtSaveData.Text.Length;
        if (bReverse)
        {
          options |= RichTextBoxFinds.Reverse;
          end = start - text.Length;
          start = 0;
          if (end < 0)
            end = this.txtSaveData.Text.Length - 1;
        }
        int num2 = this.txtSaveData.Find(text, start, end, options);
        if (num2 >= 0)
          num1 = num2;
      }
      return num1;
    }

    private void SerachText(bool bBackward, bool bStart)
    {
      int start = 0;
      if (!bStart)
        start = this.txtSaveData.SelectionStart + this.txtSaveData.SelectionLength;
      this.lblLengthVal.Text = string.Concat((object) this.m_searchForm.Text.Length);
      if (this.FindMyText(this.m_searchForm.SearchText.Text, start, bBackward) >= 0)
        return;
      this.txtSaveData.Select(0, 0);
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

    private int GetSearchValues(SearchMode mode, out uint val1, out uint val2, out byte[] val3)
    {
      uint num1 = 0;
      int num2 = 0;
      val3 = Encoding.ASCII.GetBytes(this.m_searchForm.SearchText.Text);
      try
      {
        switch (mode)
        {
          case SearchMode.Hex:
            num1 = uint.Parse(this.m_searchForm.SearchText.Text, NumberStyles.HexNumber);
            num2 = this.m_searchForm.SearchText.Text.Length;
            int num3;
            switch (num2)
            {
              case 1:
              case 2:
              case 4:
              case 6:
                num3 = 0;
                break;
              default:
                num3 = num2 != 8 ? 1 : 0;
                break;
            }
            if (num3 != 0)
            {
              val1 = val2 = 0U;
              return 0;
            }
            break;
          case SearchMode.Decimal:
            num1 = uint.Parse(this.m_searchForm.SearchText.Text);
            num2 = num1.ToString("X").Length;
            break;
          case SearchMode.Float:
            num1 = BitConverter.ToUInt32(BitConverter.GetBytes(float.Parse(this.m_searchForm.SearchText.Text)), 0);
            num2 = 8;
            break;
        }
      }
      catch (Exception ex)
      {
        val1 = 0U;
        val2 = 0U;
        return -1;
      }
      int num4;
      switch (num2)
      {
        case 1:
        case 2:
          num4 = 1;
          break;
        case 3:
        case 4:
          num4 = 2;
          break;
        case 5:
        case 6:
          num4 = 3;
          break;
        case 7:
        case 8:
          num4 = 4;
          break;
        default:
          num4 = 4;
          break;
      }
      val1 = num1;
      switch (num4)
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
      if (mode == SearchMode.Text)
        num4 = val3.Length;
      return num4;
    }

    private void txtSearchValue_TextChanged(object sender, EventArgs e)
    {
      if (this.m_bTextMode)
        return;
      if (!this.m_searchForm.SearchText.Text.StartsWith("0x"))
      {
        try
        {
          int.Parse(this.m_searchForm.SearchText.Text);
        }
        catch (OverflowException ex)
        {
          this.m_searchForm.SearchText.Text = this.m_searchForm.SearchText.Text.Substring(0, this.m_searchForm.SearchText.Text.Length - 1);
          this.m_searchForm.SearchText.SelectionStart = this.m_searchForm.SearchText.Text.Length;
        }
        catch (Exception ex)
        {
          this.m_searchForm.SearchText.Text = "";
        }
      }
      if (this.m_searchForm.SearchText.Text.Length <= 0)
        return;
      this.btnFind.Enabled = true;
      this.btnFindPrev.Enabled = true;
    }

    private void btnFind_Click(object sender, EventArgs e) => this.Search(false, false, this.m_searchForm.GetSearchMode());

    private void button1_Click(object sender, EventArgs e) => this.Search(true, false, this.m_searchForm.GetSearchMode());

    private void AdvancedEdit_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.m_bTextMode || e.KeyCode != Keys.G || e.Modifiers != Keys.Control)
        return;
      this.DoGoTo();
    }

    private void DoGoTo()
    {
      Goto @goto = new Goto(this.provider.Length);
      if (@goto.ShowDialog() != DialogResult.OK)
        return;
      if (@goto.AddressLocation < this.provider.Length)
      {
        this.hexBox1.ScrollByteIntoView(@goto.AddressLocation);
        this.hexBox1.Select(@goto.AddressLocation, 1L);
        this.hexBox1.Invalidate();
      }
      else
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidAddress);
      }
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

    private void cbSaveFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.m_bTextMode && this.provider != null && this.provider.Length > 0L)
        this.provider.ApplyChanges();
      if (this.cbSaveFiles.SelectedIndex == this._previousSelectionIndex)
        return;
      this._previousSelectionIndex = this.cbSaveFiles.SelectedIndex;
      container targetGameFolder = this.m_game.GetTargetGameFolder();
      if (!string.IsNullOrEmpty(this.m_cursaveFile) && this.m_saveFilesData.ContainsKey(this.m_cursaveFile))
      {
        file gameFile = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
        this.m_saveFilesData[this.m_cursaveFile] = gameFile.TextMode != 0 ? (gameFile.TextMode != 2 ? (gameFile.TextMode != 3 ? Encoding.UTF8.GetBytes(this.txtSaveData.Text) : Encoding.Unicode.GetBytes(this.txtSaveData.Text)) : Encoding.ASCII.GetBytes(this.txtSaveData.Text)) : this.provider.Bytes.ToArray();
      }
      this.lstCheats.Items.Clear();
      this.lstValues.Items.Clear();
      this.m_cursaveFile = this.cbSaveFiles.SelectedItem.ToString();
      List<cheat> cheats = this.m_game.GetCheats(this.m_game.LocalSaveFolder.Substring(0, this.m_game.LocalSaveFolder.Length - 4), this.m_cursaveFile);
      if (cheats != null)
      {
        foreach (cheat cheat in cheats)
        {
          if (cheat.id == "-1")
            this.lstCheats.Items.Add((object) cheat.name);
        }
      }
      if (this.lstCheats.Items.Count > 0)
        this.lstCheats.SelectedIndex = 0;
      file gameFile1 = this.m_game.GetGameFile(targetGameFolder, this.m_cursaveFile);
      if (gameFile1 != null && gameFile1.TextMode > 0)
      {
        this.txtSaveData.Visible = true;
        this.hexBox1.Visible = false;
        if (gameFile1.TextMode == 1)
          this.txtSaveData.Text = Encoding.UTF8.GetString(this.m_saveFilesData[this.m_cursaveFile]);
        else if (gameFile1.TextMode == 3)
          this.txtSaveData.Text = Encoding.Unicode.GetString(this.m_saveFilesData[this.m_cursaveFile]);
        else
          this.txtSaveData.Text = Encoding.ASCII.GetString(this.m_saveFilesData[this.m_cursaveFile]);
        this.TextMode = true;
        this.txtSaveData.TextChanged += new EventHandler(this.txtSaveData_TextChanged);
        this.lblAddress.Visible = false;
        this.lblDataHex.Visible = false;
        this.lblDataAscii.Visible = false;
        this.lblOffset.Visible = false;
        this.txtSaveData.HideSelection = false;
      }
      else
      {
        this.TextMode = false;
        this.hexBox1.Visible = true;
        this.lblAddress.Visible = true;
        this.lblDataHex.Visible = true;
        this.lblDataAscii.Visible = true;
        this.lblOffset.Visible = true;
        this.txtSaveData.HideSelection = true;
        this.txtSaveData.Visible = false;
        this.provider = new DynamicByteProvider(this.m_saveFilesData[this.m_cursaveFile]);
        this.provider.Changed += new EventHandler<ByteProviderChanged>(this.provider_Changed);
        if (!this.m_undoList.ContainsKey(this.m_cursaveFile))
          this.m_undoList.Add(this.m_cursaveFile, new Stack<ActionItem>());
        if (!this.m_redoList.ContainsKey(this.m_cursaveFile))
          this.m_redoList.Add(this.m_cursaveFile, new Stack<ActionItem>());
        this.toolStripButtonUndo.Enabled = this.m_undoList[this.m_cursaveFile].Count != 0;
        this.toolStripButtonRedo.Enabled = this.m_redoList[this.m_cursaveFile].Count != 0;
        this.provider.LengthChanged += new EventHandler(this.provider_LengthChanged);
        this.hexBox1.ByteProvider = (IByteProvider) this.provider;
        this.hexBox1.BytesPerLine = 16;
        this.hexBox1.UseFixedBytesPerLine = true;
        this.hexBox1.VScrollBarVisible = true;
        this.hexBox1.LineInfoVisible = true;
        this.hexBox1.StringViewVisible = true;
        this.hexBox1.SelectionStartChanged += new EventHandler(this.hexBox1_SelectionStartChanged);
        this.hexBox1.SelectionLengthChanged += new EventHandler(this.hexBox1_SelectionLengthChanged);
      }
    }

    private void hexBox1_SelectionLengthChanged(object sender, EventArgs e) => this.lblLengthVal.Text = string.Concat((object) this.hexBox1.SelectionLength);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AdvancedEdit));
      this.lblCheatCodes = new Label();
      this.lblCheats = new Label();
      this.btnApply = new Button();
      this.lblOffset = new Label();
      this.lblOffsetValue = new Label();
      this.panel1 = new Panel();
      this.lblLengthVal = new Label();
      this.lblLength = new Label();
      this.lblCurrentFile = new Label();
      this.cbSaveFiles = new ComboBox();
      this.txtSaveData = new RichTextBox();
      this.lblProfile = new Label();
      this.cbProfile = new ComboBox();
      this.btnFindPrev = new Button();
      this.btnFind = new Button();
      this.lblAddress = new Label();
      this.lblDataHex = new Label();
      this.lblDataAscii = new Label();
      this.label1 = new Label();
      this.lstCheats = new ListBox();
      this.lstValues = new ListBox();
      this.lblGameName = new Label();
      this.btnClose = new Button();
      this.hexBox1 = new HexBox();
      this.toolStrip1 = new ToolStrip();
      this.toolStripButtonSearch = new ToolStripButton();
      this.toolStripButtonUndo = new ToolStripButton();
      this.toolStripButtonRedo = new ToolStripButton();
      this.toolStripButtonGoto = new ToolStripButton();
      this.toolStripButtonExport = new ToolStripButton();
      this.toolStripButtonImportFile = new ToolStripButton();
      this.panel1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      this.lblCheatCodes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCheatCodes.AutoSize = true;
      this.lblCheatCodes.ForeColor = System.Drawing.Color.White;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lblCheatCodes.Location = new Point(Util.ScaleSize(654), Util.ScaleSize(146));
      else
        this.lblCheatCodes.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(146));
      this.lblCheatCodes.Name = "lblCheatCodes";
      this.lblCheatCodes.Size = Util.ScaleSize(new Size(71, 13));
      this.lblCheatCodes.TabIndex = 4;
      this.lblCheatCodes.Text = "Cheat Codes:";
      this.lblCheats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblCheats.AutoSize = true;
      this.lblCheats.ForeColor = System.Drawing.Color.White;
      this.lblCheats.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(43));
      this.lblCheats.Name = "lblCheats";
      this.lblCheats.Size = Util.ScaleSize(new Size(43, 13));
      this.lblCheats.TabIndex = 5;
      this.lblCheats.Text = "Cheats:";
      this.btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnApply.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnApply.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnApply.ForeColor = System.Drawing.Color.White;
      this.btnApply.Location = new Point(Util.ScaleSize(725), Util.ScaleSize(317));
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = Util.ScaleSize(new Size(57, 23));
      this.btnApply.TabIndex = 6;
      this.btnApply.Text = "Apply && Download";
      this.btnApply.UseVisualStyleBackColor = false;
      this.lblOffset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblOffset.AutoSize = true;
      this.lblOffset.ForeColor = System.Drawing.Color.White;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lblOffset.Location = new Point(Util.ScaleSize(410), Util.ScaleSize(322));
      else
        this.lblOffset.Location = new Point(Util.ScaleSize(480), Util.ScaleSize(322));
      this.lblOffset.Name = "lblOffset";
      this.lblOffset.Size = Util.ScaleSize(new Size(38, 13));
      this.lblOffset.TabIndex = 8;
      this.lblOffset.Text = "Offset:";
      this.lblOffsetValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblOffsetValue.AutoSize = true;
      this.lblOffsetValue.ForeColor = System.Drawing.Color.White;
      this.lblOffsetValue.Location = new Point(Util.ScaleSize(518), Util.ScaleSize(322));
      this.lblOffsetValue.Name = "lblOffsetValue";
      this.lblOffsetValue.Size = Util.ScaleSize(new Size(0, 13));
      this.lblOffsetValue.TabIndex = 9;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lblOffsetValue.Padding = new Padding(0, 0, 0, Util.ScaleSize(12));
      this.lblLengthVal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblLengthVal.AutoSize = true;
      this.lblLengthVal.BackColor = System.Drawing.Color.Transparent;
      this.lblLengthVal.ForeColor = System.Drawing.Color.White;
      this.lblLengthVal.Location = new Point(Util.ScaleSize(631), Util.ScaleSize(322));
      this.lblLengthVal.Name = "lblLengthVal";
      this.lblLengthVal.Size = Util.ScaleSize(new Size(0, 13));
      this.lblLengthVal.TabIndex = 29;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lblLengthVal.Padding = new Padding(0, 0, 0, Util.ScaleSize(12));
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.lblLengthVal);
      this.panel1.Controls.Add((Control) this.lblLength);
      this.panel1.Controls.Add((Control) this.lblCurrentFile);
      this.panel1.Controls.Add((Control) this.cbSaveFiles);
      this.panel1.Controls.Add((Control) this.txtSaveData);
      this.panel1.Controls.Add((Control) this.lblProfile);
      this.panel1.Controls.Add((Control) this.cbProfile);
      this.panel1.Controls.Add((Control) this.btnFindPrev);
      this.panel1.Controls.Add((Control) this.btnFind);
      this.panel1.Controls.Add((Control) this.lblAddress);
      this.panel1.Controls.Add((Control) this.lblDataHex);
      this.panel1.Controls.Add((Control) this.lblDataAscii);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.lstCheats);
      this.panel1.Controls.Add((Control) this.lstValues);
      this.panel1.Controls.Add((Control) this.lblGameName);
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Controls.Add((Control) this.lblOffsetValue);
      this.panel1.Controls.Add((Control) this.lblOffset);
      this.panel1.Controls.Add((Control) this.btnApply);
      this.panel1.Controls.Add((Control) this.lblCheats);
      this.panel1.Controls.Add((Control) this.lblCheatCodes);
      this.panel1.Controls.Add((Control) this.hexBox1);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(11));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(856, 348));
      this.panel1.TabIndex = 10;
      this.lblLength.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblLength.AutoSize = true;
      this.lblLength.BackColor = System.Drawing.Color.Transparent;
      this.lblLength.ForeColor = System.Drawing.Color.White;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lblLength.Location = new Point(Util.ScaleSize(558), Util.ScaleSize(322));
      else
        this.lblLength.Location = new Point(Util.ScaleSize(588), Util.ScaleSize(322));
      this.lblLength.Name = "lblLength";
      this.lblLength.Size = Util.ScaleSize(new Size(43, 13));
      this.lblLength.TabIndex = 27;
      this.lblLength.Text = "Length:";
      this.lblCurrentFile.AutoSize = true;
      this.lblCurrentFile.BackColor = System.Drawing.Color.Transparent;
      this.lblCurrentFile.ForeColor = System.Drawing.Color.White;
      this.lblCurrentFile.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(25));
      this.lblCurrentFile.Name = "lblCurrentFile";
      this.lblCurrentFile.Size = Util.ScaleSize(new Size(60, 13));
      this.lblCurrentFile.TabIndex = 26;
      this.lblCurrentFile.Text = "Current file:";
      this.cbSaveFiles.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSaveFiles.FormattingEnabled = true;
      this.cbSaveFiles.Location = new Point(Util.ScaleSize(94), Util.ScaleSize(22));
      this.cbSaveFiles.Name = "cbSaveFiles";
      this.cbSaveFiles.Size = Util.ScaleSize(new Size(121, 21));
      this.cbSaveFiles.Sorted = true;
      this.cbSaveFiles.TabIndex = 25;
      this.txtSaveData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSaveData.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(63));
      this.txtSaveData.Name = "txtSaveData";
      this.txtSaveData.ScrollBars = RichTextBoxScrollBars.Vertical;
      this.txtSaveData.Size = Util.ScaleSize(new Size(666, 244));
      this.txtSaveData.TabIndex = 24;
      this.txtSaveData.Text = "";
      this.txtSaveData.Visible = false;
      this.lblProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblProfile.AutoSize = true;
      this.lblProfile.ForeColor = System.Drawing.Color.White;
      this.lblProfile.Location = new Point(Util.ScaleSize(307), Util.ScaleSize(321));
      this.lblProfile.Name = "lblProfile";
      this.lblProfile.Size = Util.ScaleSize(new Size(39, 13));
      this.lblProfile.TabIndex = 23;
      this.lblProfile.Text = "Profile:";
      this.cbProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cbProfile.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbProfile.FormattingEnabled = true;
      this.cbProfile.Location = new Point(Util.ScaleSize(347), Util.ScaleSize(317));
      this.cbProfile.Name = "cbProfile";
      this.cbProfile.Size = Util.ScaleSize(new Size(112, 21));
      this.cbProfile.TabIndex = 22;
      this.btnFindPrev.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnFindPrev.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnFindPrev.ForeColor = System.Drawing.Color.White;
      this.btnFindPrev.Location = new Point(Util.ScaleSize(221), Util.ScaleSize(316));
      this.btnFindPrev.Name = "btnFindPrev";
      this.btnFindPrev.Size = Util.ScaleSize(new Size(81, 23));
      this.btnFindPrev.TabIndex = 21;
      this.btnFindPrev.Text = "Find Previous";
      this.btnFindPrev.UseVisualStyleBackColor = false;
      this.btnFindPrev.Visible = false;
      this.btnFind.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnFind.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnFind.ForeColor = System.Drawing.Color.White;
      this.btnFind.Location = new Point(Util.ScaleSize(152), Util.ScaleSize(316));
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = Util.ScaleSize(new Size(63, 23));
      this.btnFind.TabIndex = 20;
      this.btnFind.Text = "Find";
      this.btnFind.UseVisualStyleBackColor = false;
      this.btnFind.Visible = false;
      this.lblAddress.Font = new Font("Courier New", Util.ScaleSize(7.8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblAddress.ForeColor = System.Drawing.Color.White;
      this.lblAddress.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(45));
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = Util.ScaleSize(new Size(68, 15));
      this.lblAddress.TabIndex = 17;
      this.lblAddress.Text = "Address";
      this.lblDataHex.Font = new Font("Courier New", Util.ScaleSize(7.8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDataHex.ForeColor = System.Drawing.Color.White;
      if (Util.CurrentPlatform == Util.Platform.Linux)
      {
        switch (Util.ScaleIndex)
        {
          case 0:
            this.lblDataHex.Location = new Point(66, Util.ScaleSize(45));
            break;
          case 1:
            this.lblDataHex.Location = new Point(90, Util.ScaleSize(45));
            break;
          case 2:
            this.lblDataHex.Location = new Point(113, Util.ScaleSize(45));
            break;
          case 3:
            this.lblDataHex.Location = new Point((int) sbyte.MaxValue, Util.ScaleSize(45));
            break;
          case 4:
            this.lblDataHex.Location = new Point(150, Util.ScaleSize(45));
            break;
          case 5:
            this.lblDataHex.Location = new Point(175, Util.ScaleSize(45));
            break;
        }
      }
      else
      {
        switch (Util.ScaleIndex)
        {
          case 0:
            this.lblDataHex.Location = new Point(66, Util.ScaleSize(45));
            break;
          case 1:
            this.lblDataHex.Location = new Point(100, Util.ScaleSize(45));
            break;
          case 2:
            this.lblDataHex.Location = new Point(113, Util.ScaleSize(45));
            break;
          case 3:
            this.lblDataHex.Location = new Point(137, Util.ScaleSize(45));
            break;
          case 4:
            this.lblDataHex.Location = new Point(160, Util.ScaleSize(45));
            break;
          case 5:
            this.lblDataHex.Location = new Point(185, Util.ScaleSize(45));
            break;
        }
      }
      this.lblDataHex.Name = "lblAddress";
      this.lblDataHex.Size = Util.ScaleSize(new Size(80, 15));
      this.lblDataHex.TabIndex = 18;
      this.lblDataHex.Text = "Data (Hex)";
      this.lblDataAscii.Font = new Font("Courier New", Util.ScaleSize(7.8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDataAscii.ForeColor = System.Drawing.Color.White;
      if (Util.CurrentPlatform == Util.Platform.Linux)
      {
        switch (Util.ScaleIndex)
        {
          case 0:
            this.lblDataAscii.Location = new Point(314, Util.ScaleSize(45));
            break;
          case 1:
            this.lblDataAscii.Location = new Point(440, Util.ScaleSize(45));
            break;
          case 2:
            this.lblDataAscii.Location = new Point(562, Util.ScaleSize(45));
            break;
          case 3:
            this.lblDataAscii.Location = new Point(625, Util.ScaleSize(45));
            break;
          case 4:
            this.lblDataAscii.Location = new Point(748, Util.ScaleSize(45));
            break;
          case 5:
            this.lblDataAscii.Location = new Point(872, Util.ScaleSize(45));
            break;
        }
      }
      else
      {
        switch (Util.ScaleIndex)
        {
          case 0:
            this.lblDataAscii.Location = new Point(314, Util.ScaleSize(45));
            break;
          case 1:
            this.lblDataAscii.Location = new Point(497, Util.ScaleSize(45));
            break;
          case 2:
            this.lblDataAscii.Location = new Point(562, Util.ScaleSize(45));
            break;
          case 3:
            this.lblDataAscii.Location = new Point(685, Util.ScaleSize(45));
            break;
          case 4:
            this.lblDataAscii.Location = new Point(808, Util.ScaleSize(45));
            break;
          case 5:
            this.lblDataAscii.Location = new Point(932, Util.ScaleSize(45));
            break;
        }
      }
      this.lblDataAscii.Name = "lblAddress";
      this.lblDataAscii.Size = Util.ScaleSize(new Size(85, 15));
      this.lblDataAscii.TabIndex = 19;
      this.lblDataAscii.Text = "Data (Ascii)";
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.ForeColor = System.Drawing.Color.White;
      this.label1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(321));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(41, 13));
      this.label1.TabIndex = 15;
      this.label1.Text = "Search";
      this.label1.Visible = false;
      this.lstCheats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lstCheats.FormattingEnabled = true;
      this.lstCheats.IntegralHeight = false;
      this.lstCheats.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(63));
      this.lstCheats.Name = "lstCheats";
      this.lstCheats.Size = Util.ScaleSize(new Size(160, 74));
      this.lstCheats.TabIndex = 14;
      this.lstValues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lstValues.FormattingEnabled = true;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.lstValues.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(169));
      else
        this.lstValues.Location = new Point(Util.ScaleSize(684), Util.ScaleSize(162));
      this.lstValues.MultiColumn = true;
      this.lstValues.Name = "lstValues";
      this.lstValues.Size = Util.ScaleSize(new Size(160, 147));
      this.lstValues.TabIndex = 13;
      this.lblGameName.AutoSize = true;
      this.lblGameName.ForeColor = System.Drawing.Color.White;
      this.lblGameName.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(4));
      this.lblGameName.Name = "lblGameName";
      this.lblGameName.Size = Util.ScaleSize(new Size(28, 13));
      this.lblGameName.TabIndex = 12;
      this.lblGameName.Text = "Test";
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.BackColor = System.Drawing.Color.FromArgb(246, 128, 31);
      this.btnClose.ForeColor = System.Drawing.Color.White;
      this.btnClose.Location = new Point(Util.ScaleSize(787), Util.ScaleSize(317));
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = Util.ScaleSize(new Size(57, 23));
      this.btnClose.TabIndex = 10;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = false;
      this.hexBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.hexBox1.Font = new Font("Courier New", Util.ScaleSize(9f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hexBox1.HScrollBarVisible = false;
      this.hexBox1.LineInfoForeColor = System.Drawing.Color.Empty;
      this.hexBox1.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(63));
      this.hexBox1.Name = "hexBox1";
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.hexBox1.ForeColor = System.Drawing.Color.Black;
      this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(100, 60, 188, (int) byte.MaxValue);
      this.hexBox1.Size = Util.ScaleSize(new Size(666, 244));
      this.hexBox1.TabIndex = 0;
      this.toolStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.toolStrip1.Dock = DockStyle.None;
      this.toolStrip1.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.toolStripButtonSearch,
        (ToolStripItem) this.toolStripButtonUndo,
        (ToolStripItem) this.toolStripButtonRedo,
        (ToolStripItem) this.toolStripButtonGoto,
        (ToolStripItem) this.toolStripButtonExport,
        (ToolStripItem) this.toolStripButtonImportFile
      });
      this.toolStrip1.Location = new Point(Util.ScaleSize(682), Util.ScaleSize(13));
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = Util.ScaleSize(new Size(181, 25));
      this.toolStrip1.TabIndex = 11;
      this.toolStrip1.Text = "toolStrip1";
      this.toolStripButtonSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonSearch.Image = (Image) componentResourceManager.GetObject("toolStripButtonSearch.Image");
      this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonSearch.Name = "toolStripButtonSearch";
      this.toolStripButtonSearch.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonSearch.Text = "Search";
      this.toolStripButtonUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonUndo.Image = (Image) componentResourceManager.GetObject("toolStripButtonUndo.Image");
      this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonUndo.Name = "toolStripButtonUndo";
      this.toolStripButtonUndo.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonUndo.Text = "Undo";
      this.toolStripButtonRedo.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonRedo.Image = (Image) componentResourceManager.GetObject("toolStripButtonRedo.Image");
      this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonRedo.Name = "toolStripButtonRedo";
      this.toolStripButtonRedo.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonRedo.Text = "Redo";
      this.toolStripButtonGoto.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonGoto.Image = (Image) componentResourceManager.GetObject("toolStripButtonGoto.Image");
      this.toolStripButtonGoto.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonGoto.Name = "toolStripButtonGoto";
      this.toolStripButtonGoto.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonGoto.Text = "Go to location";
      this.toolStripButtonExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonExport.Image = (Image) componentResourceManager.GetObject("toolStripButtonExport.Image");
      this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonExport.Name = "toolStripButtonExport";
      this.toolStripButtonExport.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonExport.Text = "Export to file";
      this.toolStripButtonImportFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.toolStripButtonImportFile.Image = (Image) componentResourceManager.GetObject("toolStripButtonImportFile.Image");
      this.toolStripButtonImportFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonImportFile.Name = "toolStripButtonImportFile";
      this.toolStripButtonImportFile.Size = Util.ScaleSize(new Size(23, 22));
      this.toolStripButtonImportFile.Text = "Import File";
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(876, 369));
      this.Controls.Add((Control) this.toolStrip1);
      this.Controls.Add((Control) this.panel1);
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.KeyPreview = true;
      this.MinimumSize = Util.ScaleSize(new Size(856, 362));
      this.Name = nameof (AdvancedEdit);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Advanced Edit";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
