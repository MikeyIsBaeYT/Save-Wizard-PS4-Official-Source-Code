// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.AddCode
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor.SubControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class AddCode : Form
  {
    private const int MAX_CHEAT_CODES = 128;
    private AddCode.Mode m_bMode = AddCode.Mode.ADD_MODE;
    public string Code;
    private List<string> m_existingCodes;
    private IContainer components = (IContainer) null;
    private TextBox txtDescription;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn ColLocation;
    private DataGridViewTextBoxColumn Value;
    private Button btnSave;
    private Button btnCancel;
    private TextBox txtCode;
    private Label lblCodes;
    private TextBox txtComment;
    private Label lblComment;
    private Panel panel1;
    private Label lblDescription;

    public string Description { get; set; }

    public string Comment { get; set; }

    public AddCode(List<string> existingCodes)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.m_existingCodes = existingCodes;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblCodes.BackColor = Color.Transparent;
      this.lblComment.BackColor = Color.Transparent;
      this.lblDescription.BackColor = Color.Transparent;
      this.lblDescription.Text = PS3SaveEditor.Resources.Resources.lblDescription;
      this.lblComment.Text = PS3SaveEditor.Resources.Resources.lblComment;
      this.lblCodes.Text = PS3SaveEditor.Resources.Resources.lblCodes;
      this.btnSave.Text = PS3SaveEditor.Resources.Resources.btnSave;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.CenterToScreen();
      this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
      this.dataGridView1.KeyDown += new KeyEventHandler(this.dataGridView1_KeyDown);
      this.m_bMode = AddCode.Mode.ADD_MODE;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    public AddCode(cheat item, List<string> existingCodes)
    {
      this.m_bMode = AddCode.Mode.EDIT_MODE;
      this.m_existingCodes = existingCodes;
      this.InitializeComponent();
      this.Text = PS3SaveEditor.Resources.Resources.titleCodeEntry;
      this.Text = PS3SaveEditor.Resources.Resources.titleEditCheat;
      this.lblDescription.Text = PS3SaveEditor.Resources.Resources.lblDescription;
      this.lblComment.Text = PS3SaveEditor.Resources.Resources.lblComment;
      this.lblCodes.Text = PS3SaveEditor.Resources.Resources.lblCodes;
      this.btnSave.Text = PS3SaveEditor.Resources.Resources.btnSave;
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.CenterToScreen();
      this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
      this.dataGridView1.KeyDown += new KeyEventHandler(this.dataGridView1_KeyDown);
      this.txtCode.Text = item.ToEditableString();
      this.txtDescription.Text = item.name;
      this.txtComment.Text = item.note;
    }

    private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.F || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 || e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9 || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
        return;
      e.SuppressKeyPress = true;
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
        return;
      string s = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
      int result = 0;
      if (!int.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
      {
        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) null;
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidHexCode, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object) result.ToString("X8");
    }

    public static byte[] ConvertHexStringToByteArray(string hexString)
    {
      byte[] numArray = (uint) (hexString.Length % 2) <= 0U ? new byte[hexString.Length / 2] : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", (object) hexString));
      for (int index = 0; index < numArray.Length; ++index)
      {
        string s = hexString.Substring(index * 2, 2);
        numArray[index] = byte.Parse(s, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      return numArray;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDescription.Text.Trim()))
      {
        int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidDesc, PS3SaveEditor.Resources.Resources.msgError);
      }
      else if (this.m_existingCodes.IndexOf(this.txtDescription.Text) >= 0)
      {
        int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errCheatExists, PS3SaveEditor.Resources.Resources.msgError);
      }
      else if (this.txtCode.Text.Trim().Length == 0)
      {
        int num3 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidCode, PS3SaveEditor.Resources.Resources.msgError);
      }
      else
      {
        foreach (string line in this.txtCode.Lines)
        {
          if (line.Trim().Length != 17 && (uint) line.Trim().Length > 0U)
          {
            int num4 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidCode, PS3SaveEditor.Resources.Resources.msgError);
            return;
          }
        }
        if (this.txtCode.Lines[0][0] == 'F')
        {
          if (this.txtCode.Lines.Length > 16)
          {
            int num5 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidFCode, PS3SaveEditor.Resources.Resources.msgError);
            return;
          }
          string str = this.txtCode.Text.Replace(" ", "").Replace("\r\n", "");
          if ((int) this.GetCRC(Encoding.ASCII.GetBytes(str.Substring(0, str.Length - 8))) != (int) uint.Parse(str.Substring(str.Length - 8, 8), NumberStyles.HexNumber))
          {
            int num6 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidCode, PS3SaveEditor.Resources.Resources.msgError);
            return;
          }
        }
        if (Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgConfirmCode, PS3SaveEditor.Resources.Resources.warnTitle, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
        this.Description = this.txtDescription.Text;
        this.Comment = this.txtComment.Text;
        this.Code = this.txtCode.Text.Replace("\r\n", " ").TrimEnd();
        this.Code = this.Code.Replace("\n", " ").TrimEnd();
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void txtCheatCode_TextChanged(object sender, EventArgs e)
    {
      int selectionStart = this.txtCode.SelectionStart;
      int length = this.txtCode.Lines.Length;
      if (length > 1 && (this.txtCode.Lines[length - 2].Length < 17 || this.txtCode.Lines[length - 1].Length == 0))
        --length;
      if (length > 128)
      {
        string[] lines = new string[128];
        Array.Copy((Array) this.txtCode.Lines, (Array) lines, 128);
        this.SetLinesToCode(lines);
        int num = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.errMaxCodes, (object) 128), this.Text);
        this.txtCode.SelectionStart = this.txtCode.TextLength;
        this.txtCode.SelectionLength = 0;
      }
      else
      {
        if (length <= 0)
          return;
        this.SetLinesToCode(this.txtCode.Lines);
      }
    }

    private void txtCheatCode_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
        return;
      int num = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
      int index;
      switch (Util.CurrentPlatform)
      {
        case Util.Platform.Linux:
          index = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart) - 1;
          break;
        case Util.Platform.MacOS:
          index = this.getCurrentLine();
          break;
        default:
          index = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
          break;
      }
      string[] lines = this.txtCode.Lines;
      if ((uint) lines.Length > 0U)
      {
        string str = lines[index];
        if (num > 0 && num >= str.Length)
        {
          e.SuppressKeyPress = true;
          return;
        }
      }
      if (num >= 17)
        e.SuppressKeyPress = true;
      if (num == 8)
        ++this.txtCode.SelectionStart;
    }

    private int getCurrentLine()
    {
      int selectionStart = this.txtCode.SelectionStart;
      int index1 = 0;
      string[] lines = this.txtCode.Lines;
      for (int index2 = lines[index1].Length + 2; index2 < selectionStart; index2 += lines[index1].Length + 2)
        ++index1;
      return index1;
    }

    private void SetLinesToCode(string[] lines)
    {
      string str1 = "";
      int index1 = 0;
      int num1 = this.txtCode.SelectionStart;
      for (int index2 = 0; index2 < lines.Length; ++index2)
      {
        if (index2 < lines.Length - 1 || lines[index2].Length > 0)
        {
          string str2 = lines[index1].Replace(" ", "");
          for (int index3 = 0; index3 < str2.Length; ++index3)
          {
            if ((str2[index3] < '0' || str2[index3] > '9') && (str2[index3] < 'a' || str2[index3] > 'f') && (str2[index3] < 'A' || str2[index3] > 'F'))
              str2 = str2.Remove(index3, 1);
          }
          string str3 = str2.Length <= 8 ? str2 + Environment.NewLine : str2.Substring(0, 8) + " " + str2.Substring(8, Math.Min(8, str2.Length - 8)) + Environment.NewLine;
          str1 += str3;
          ++index1;
        }
      }
      lines = this.txtCode.Lines;
      int num2 = 0;
      foreach (string line in lines)
      {
        if (line.Length > 0 && line.Length > 17)
          num1 = (num2 + 1) * 18;
        ++num2;
      }
      this.txtCode.Text = str1;
      if (num1 <= 0)
        return;
      this.txtCode.SelectionStart = num1;
      this.txtCode.ScrollToCaret();
    }

    private void HandleCodeBackSpace(ref KeyPressEventArgs e)
    {
      int num = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
      if (num < 0)
        num = this.txtCode.SelectionStart;
      string[] lines = this.txtCode.Lines;
      int index;
      switch (Util.CurrentPlatform)
      {
        case Util.Platform.Linux:
          index = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart) - 1;
          break;
        case Util.Platform.MacOS:
          index = this.getCurrentLine();
          break;
        default:
          index = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
          break;
      }
      if (lines.Length == 0)
        return;
      if (num == 0 && this.txtCode.SelectionStart > 0 && lines[index].Length > 0)
      {
        e.Handled = true;
        this.txtCode.SelectionStart -= 2;
      }
      else
      {
        if (num != 9)
          return;
        --this.txtCode.SelectionStart;
      }
    }

    private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\b')
        this.HandleCodeBackSpace(ref e);
      else if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar >= 'a' && e.KeyChar <= 'f' || e.KeyChar >= 'A' && e.KeyChar <= 'F')
      {
        int length = this.txtCode.Lines.Length;
        if (length > 1 && this.txtCode.Lines[length - 2].Length < 17)
          --length;
        if (length > 128)
        {
          e.Handled = true;
          int num = (int) Util.ShowMessage(string.Format(PS3SaveEditor.Resources.Resources.msgMaxCheats, (object) 128), this.Text);
        }
        else
        {
          int index1;
          switch (Util.CurrentPlatform)
          {
            case Util.Platform.Linux:
              index1 = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart) - 1;
              break;
            case Util.Platform.MacOS:
              index1 = this.getCurrentLine();
              break;
            default:
              index1 = this.txtCode.GetLineFromCharIndex(this.txtCode.SelectionStart);
              break;
          }
          string str = "";
          string[] lines = this.txtCode.Lines;
          if ((uint) this.txtCode.Lines.Length > 0U)
            str = this.txtCode.Lines[index1];
          else
            lines = new string[1];
          int index2 = this.txtCode.SelectionStart - this.txtCode.GetFirstCharIndexOfCurrentLine();
          if (index2 < 0)
            index2 = this.txtCode.SelectionStart;
          if (index2 > 17)
            index2 = 17;
          int selectionStart = this.txtCode.SelectionStart;
          if (index2 == 17)
          {
            this.txtCode.GetFirstCharIndexFromLine(index1 + 1);
            char[] chArray = lines[index1 + 1].ToCharArray();
            if (chArray.Length == 0)
              chArray = new char[1];
            chArray[0] = e.KeyChar;
            lines[index1 + 1] = new string(chArray);
            this.SetLinesToCode(lines);
            this.txtCode.SelectionStart = this.txtCode.GetFirstCharIndexFromLine(index1 + 1) + 1;
            if (this.txtCode.SelectionStart > 0)
              this.txtCode.ScrollToCaret();
            e.Handled = true;
          }
          else
          {
            char[] charArray = str.ToCharArray();
            if (charArray.Length == 17)
            {
              if (index2 == 8)
              {
                charArray[index2 + 1] = e.KeyChar;
                lines[index1] = new string(charArray);
                this.SetLinesToCode(lines);
                this.txtCode.SelectionStart += 2;
                e.Handled = true;
              }
              else
              {
                charArray[index2] = e.KeyChar;
                lines[index1] = new string(charArray);
                this.SetLinesToCode(lines);
                ++this.txtCode.SelectionStart;
                e.Handled = true;
              }
            }
            else if (index2 == 8 && charArray.Length == 8)
            {
              char[] chArray = new char[charArray.Length + 2];
              Array.Copy((Array) charArray, (Array) chArray, 8);
              chArray[8] = ' ';
              chArray[9] = e.KeyChar;
              lines[index1] = new string(chArray);
              this.SetLinesToCode(lines);
              this.txtCode.SelectionStart += 2;
              e.Handled = true;
            }
            else if (index2 == 8 && charArray.Length > 8)
            {
              char[] chArray = new char[charArray.Length + 1];
              Array.Copy((Array) charArray, (Array) chArray, 8);
              chArray[8] = ' ';
              chArray[9] = e.KeyChar;
              Array.Copy((Array) charArray, 9, (Array) chArray, 10, charArray.Length - 9);
              lines[index1] = new string(chArray);
              this.SetLinesToCode(lines);
              this.txtCode.SelectionStart += 2;
              e.Handled = true;
            }
            else
            {
              if (index2 <= 8)
                return;
              char[] chArray = new char[charArray.Length + 1];
              Array.Copy((Array) charArray, (Array) chArray, index2);
              chArray[index2] = e.KeyChar;
              Array.Copy((Array) charArray, index2, (Array) chArray, index2 + 1, charArray.Length - index2);
              lines[index1] = new string(chArray);
              this.SetLinesToCode(lines);
              ++this.txtCode.SelectionStart;
              e.Handled = true;
            }
          }
        }
      }
      else if (e.KeyChar == '\u0001')
      {
        this.txtCode.SelectAll();
      }
      else
      {
        if (e.KeyChar == '\u0003' || e.KeyChar == '\r' || e.KeyChar == '\u0018' || e.KeyChar == '\u0016' || e.KeyChar == '\u001A')
          return;
        e.Handled = true;
      }
    }

    private uint GetCRC(byte[] data)
    {
      Crc32Net crc32Net = new Crc32Net();
      crc32Net.ComputeHash(data);
      return crc32Net.CrcValue;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.txtDescription = new TextBox();
      this.dataGridView1 = new DataGridView();
      this.ColLocation = new DataGridViewTextBoxColumn();
      this.Value = new DataGridViewTextBoxColumn();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.txtCode = new TextBox();
      this.lblCodes = new Label();
      this.txtComment = new TextBox();
      this.lblComment = new Label();
      this.panel1 = new Panel();
      this.lblDescription = new Label();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.txtDescription.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(28));
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = Util.ScaleSize(new Size(181, 20));
      this.txtDescription.TabIndex = 0;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.txtDescription.ContextMenu = new MacContextMenu(this.txtDescription).GetMenu();
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.ColLocation, (DataGridViewColumn) this.Value);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dataGridView1.DefaultCellStyle = gridViewCellStyle2;
      this.dataGridView1.Location = new Point(Util.ScaleSize(265), Util.ScaleSize(52));
      this.dataGridView1.Name = "dataGridView1";
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dataGridView1.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dataGridView1.Size = Util.ScaleSize(new Size(12, 10));
      this.dataGridView1.TabIndex = 2;
      this.dataGridView1.Visible = false;
      this.ColLocation.HeaderText = "Location";
      this.ColLocation.Name = "Location";
      this.Value.HeaderText = "Value";
      this.Value.Name = "Value";
      this.btnSave.Location = new Point(Util.ScaleSize(25), Util.ScaleSize(282));
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = Util.ScaleSize(new Size(75, 23));
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Location = new Point(Util.ScaleSize(104), Util.ScaleSize(282));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(75, 23));
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.txtCode.CharacterCasing = CharacterCasing.Upper;
      this.txtCode.Font = new Font("Courier New", Util.ScaleSize(8f), FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtCode.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(112));
      this.txtCode.Multiline = true;
      this.txtCode.Name = "txtCode";
      this.txtCode.ScrollBars = ScrollBars.Vertical;
      this.txtCode.Size = Util.ScaleSize(new Size(181, 165));
      this.txtCode.TabIndex = 2;
      this.txtCode.TextChanged += new EventHandler(this.txtCheatCode_TextChanged);
      this.txtCode.KeyDown += new KeyEventHandler(this.txtCheatCode_KeyDown);
      this.txtCode.KeyPress += new KeyPressEventHandler(this.txtCode_KeyPress);
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.txtCode.ContextMenu = new MacContextMenu(this.txtCode).GetMenu();
      this.lblCodes.BackColor = Color.Transparent;
      this.lblCodes.ForeColor = Color.White;
      this.lblCodes.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(97));
      this.lblCodes.Name = "lblCodes";
      this.lblCodes.Size = Util.ScaleSize(new Size(111, 13));
      this.lblCodes.TabIndex = 6;
      this.lblCodes.Text = "Cheat Codes:";
      this.txtComment.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(70));
      this.txtComment.Name = "txtComment";
      this.txtComment.Size = Util.ScaleSize(new Size(181, 20));
      this.txtComment.TabIndex = 1;
      if (Util.CurrentPlatform == Util.Platform.MacOS)
        this.txtComment.ContextMenu = new MacContextMenu(this.txtComment).GetMenu();
      this.lblComment.BackColor = Color.Transparent;
      this.lblComment.ForeColor = Color.White;
      this.lblComment.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(55));
      this.lblComment.Name = "lblComment";
      this.lblComment.Size = Util.ScaleSize(new Size(93, 13));
      this.lblComment.TabIndex = 7;
      this.lblComment.Text = "Comment:";
      this.panel1.AutoScroll = true;
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.panel1.Controls.Add((Control) this.lblDescription);
      this.panel1.Controls.Add((Control) this.txtComment);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Controls.Add((Control) this.lblComment);
      this.panel1.Controls.Add((Control) this.lblCodes);
      this.panel1.Controls.Add((Control) this.txtCode);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.txtDescription);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(11));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(205, 315));
      this.panel1.TabIndex = 8;
      this.lblDescription.BackColor = Color.Transparent;
      this.lblDescription.ForeColor = Color.White;
      this.lblDescription.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(13));
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = Util.ScaleSize(new Size(93, 13));
      this.lblDescription.TabIndex = 8;
      this.lblDescription.Text = "Description:";
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(225, 337));
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.dataGridView1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddCode);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Add Cheat";
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum Mode
    {
      ADD_MODE,
      EDIT_MODE,
    }
  }
}
