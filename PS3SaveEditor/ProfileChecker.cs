// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ProfileChecker
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class ProfileChecker : Form
  {
    private string psnId;
    private int m_registerMode = 0;
    private int previousDriveNum = 0;
    private const string REGISTER_PSNID = "{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}";
    private IContainer components = (IContainer) null;
    private Panel panelInstructions;
    private Button btnNext;
    private Label lblInstructions;
    private Label label1;
    private Label lblTitle1;
    private Panel panelProfileName;
    private Label lblDriveLetter;
    private TextBox txtProfileName;
    private Label lblInstructionPage2;
    private Label label4;
    private Label lblPageTitle;
    private Panel panelFinish;
    private Label lblFinish;
    private Label label8;
    private Label lblTitleFinish;
    private Label lblInstructionPage1;
    private Label lblInstruciton3;
    private Label lblInstruction2;
    private Label lblInstrucionRed;
    private Label lblInstruction1;
    private Label lblFooter2;
    private Label lblUserName;
    private Label lblInstruction2Page2;
    private Panel panel1;

    public ProfileChecker(int regMode = 0, string psn = null, string drive = null)
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.CenterToScreen();
      this.txtProfileName.MaxLength = 32;
      this.Text = string.Format(PS3SaveEditor.Resources.Resources.titlePSNAdd, (object) Util.PRODUCT_NAME);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblTitle1.Text = PS3SaveEditor.Resources.Resources.lblPSNAddTitle;
      this.lblInstructions.Text = PS3SaveEditor.Resources.Resources.lblInstructionsPage1;
      this.lblInstruction1.Text = PS3SaveEditor.Resources.Resources.lblInstruction1;
      this.lblInstrucionRed.Text = PS3SaveEditor.Resources.Resources.lblInstruction1Red;
      this.lblInstruction2.Text = PS3SaveEditor.Resources.Resources.lblInstruction_2;
      this.lblInstruciton3.Text = PS3SaveEditor.Resources.Resources.lblInstruction3;
      this.lblInstructionPage1.Text = PS3SaveEditor.Resources.Resources.lblPage1;
      this.btnNext.Text = PS3SaveEditor.Resources.Resources.btnPage1;
      this.lblPageTitle.Text = PS3SaveEditor.Resources.Resources.lblPSNAddTitle;
      this.lblInstructionPage2.Text = PS3SaveEditor.Resources.Resources.lblPage2;
      this.lblUserName.Text = PS3SaveEditor.Resources.Resources.lblUserName;
      this.lblInstruction2Page2.Text = PS3SaveEditor.Resources.Resources.lblPage21;
      this.lblFooter2.Text = PS3SaveEditor.Resources.Resources.lblInstructionPage2;
      this.panelProfileName.Visible = false;
      this.panelFinish.Visible = false;
      this.lblTitleFinish.Text = PS3SaveEditor.Resources.Resources.titlePSNAdd;
      this.lblFinish.Text = PS3SaveEditor.Resources.Resources.lblInstuctionPage3;
      Control.CheckForIllegalCrossThreadCalls = false;
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.txtProfileName.TextChanged += new EventHandler(this.txtProfileName_TextChanged);
      this.DialogResult = DialogResult.Cancel;
      this.m_registerMode = regMode;
      if (regMode > 0)
      {
        if (regMode == 1)
        {
          this.panelInstructions.Visible = false;
          this.panelProfileName.Visible = true;
          this.btnNext.Enabled = false;
          this.btnNext.Text = PS3SaveEditor.Resources.Resources.btnPage2;
        }
        this.psnId = psn;
        this.lblDriveLetter.Text = string.Format(PS3SaveEditor.Resources.Resources.lblDrive, (object) drive);
      }
      else
        this.Load += new EventHandler(this.ProfileChecker_Load);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void txtProfileName_TextChanged(object sender, EventArgs e)
    {
      this.txtProfileName.Text = this.txtProfileName.Text.Trim('.', '"', '/', '\\', '[', ']', ':', ';', '|', '=', ',', '?', '%', '<', '>', '&');
      this.txtProfileName.SelectionStart = this.txtProfileName.Text.Length;
      this.btnNext.Enabled = this.txtProfileName.Text.Length > 0;
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      if (this.panelInstructions.Visible)
      {
        this.panelInstructions.Visible = false;
        this.panelProfileName.Visible = true;
        this.btnNext.Enabled = false;
        this.btnNext.Text = PS3SaveEditor.Resources.Resources.btnPage2;
      }
      else if (this.panelProfileName.Visible)
      {
        int errorCode;
        if (this.RegisterPSNID(this.psnId, this.txtProfileName.Text, out errorCode))
        {
          if (this.m_registerMode > 0)
          {
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
          else
          {
            this.panelProfileName.Visible = false;
            this.panelFinish.Visible = true;
            this.btnNext.Text = PS3SaveEditor.Resources.Resources.btnOK;
          }
        }
        else if (errorCode == 4121)
        {
          int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errPSNNameUsed, PS3SaveEditor.Resources.Resources.msgError);
        }
        else
        {
          int num2 = (int) Util.ShowMessage("Error occurred while registering PSN ID.");
        }
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool RegisterPSNID(string psnId, string name, out int errorCode)
    {
      errorCode = 0;
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      webClientEx.Encoding = Encoding.UTF8;
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(Encoding.UTF8.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.UTF8.GetBytes(string.Format("{{\"action\":\"REGISTER_PSNID\",\"userid\":\"{0}\",\"psnid\":\"{1}\",\"friendly_name\":\"{2}\"}}", (object) Util.GetUserId(), (object) psnId.Trim(), (object) name.Trim())))), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
      if (dictionary.ContainsKey("status") && (string) dictionary["status"] == "OK")
        return true;
      if (dictionary.ContainsKey("code"))
        errorCode = Convert.ToInt32(dictionary["code"]);
      return false;
    }

    private void ProfileChecker_Load(object sender, EventArgs e)
    {
      this.btnNext.Enabled = false;
      this.CheckDrives();
      if (!Util.IsUnixOrMacOSX() || this.btnNext.Enabled)
        return;
      System.Timers.Timer timer = new System.Timers.Timer();
      this.previousDriveNum = DriveInfo.GetDrives().Length;
      timer.Elapsed += (ElapsedEventHandler) ((s, e2) =>
      {
        DriveInfo[] drives = DriveInfo.GetDrives();
        if (this.previousDriveNum == drives.Length)
          return;
        this.previousDriveNum = drives.Length;
        this.CheckDrives();
      });
      timer.Interval = 10000.0;
      timer.Enabled = true;
    }

    private void CheckDrives()
    {
      this.btnNext.Enabled = false;
      this.lblDriveLetter.Text = string.Format(PS3SaveEditor.Resources.Resources.lblDrive, (object) "---");
      foreach (DriveInfo drive in DriveInfo.GetDrives())
      {
        bool flag1;
        if (Util.IsUnixOrMacOSX())
        {
          bool flag2 = (drive.Name.Contains("media") || drive.Name.Contains("Volumes")) && Directory.Exists(drive.ToString() + "/PS4");
          flag1 = drive.IsReady && drive.DriveType == DriveType.Removable | flag2;
        }
        else
          flag1 = drive.IsReady && drive.DriveType == DriveType.Removable;
        if (flag1 && Util.IsPathToCheats(drive.Name))
        {
          this.psnId = Path.GetFileName(Directory.GetDirectories(Path.Combine(drive.Name, "PS4", "SAVEDATA"))[0]);
          long result = 0;
          if (long.TryParse(this.psnId, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          {
            this.btnNext.Enabled = true;
            this.lblDriveLetter.Text = string.Format(PS3SaveEditor.Resources.Resources.lblDrive, (object) drive.RootDirectory.Name);
            break;
          }
        }
      }
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 537)
        new Thread(new ThreadStart(this.CheckDrives)).Start();
      base.WndProc(ref m);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProfileChecker));
      this.panelInstructions = new Panel();
      this.lblInstructionPage1 = new Label();
      this.lblInstruciton3 = new Label();
      this.lblInstruction2 = new Label();
      this.lblInstrucionRed = new Label();
      this.lblInstruction1 = new Label();
      this.lblInstructions = new Label();
      this.label1 = new Label();
      this.lblTitle1 = new Label();
      this.btnNext = new Button();
      this.panelProfileName = new Panel();
      this.lblFooter2 = new Label();
      this.lblUserName = new Label();
      this.lblInstruction2Page2 = new Label();
      this.lblDriveLetter = new Label();
      this.txtProfileName = new TextBox();
      this.lblInstructionPage2 = new Label();
      this.label4 = new Label();
      this.lblPageTitle = new Label();
      this.panelFinish = new Panel();
      this.lblFinish = new Label();
      this.label8 = new Label();
      this.lblTitleFinish = new Label();
      this.panel1 = new Panel();
      this.panelInstructions.SuspendLayout();
      this.panelProfileName.SuspendLayout();
      this.panelFinish.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panelInstructions.BackColor = Color.White;
      this.panelInstructions.Controls.Add((Control) this.lblInstructionPage1);
      this.panelInstructions.Controls.Add((Control) this.lblInstruciton3);
      this.panelInstructions.Controls.Add((Control) this.lblInstruction2);
      this.panelInstructions.Controls.Add((Control) this.lblInstrucionRed);
      this.panelInstructions.Controls.Add((Control) this.lblInstruction1);
      this.panelInstructions.Controls.Add((Control) this.lblInstructions);
      this.panelInstructions.Controls.Add((Control) this.label1);
      this.panelInstructions.Controls.Add((Control) this.lblTitle1);
      this.panelInstructions.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.panelInstructions.Name = "panelInstructions";
      this.panelInstructions.Size = Util.ScaleSize(new Size(570, 340));
      this.panelInstructions.TabIndex = 0;
      this.lblInstructionPage1.ForeColor = Color.Black;
      this.lblInstructionPage1.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(290));
      this.lblInstructionPage1.Name = "lblInstructionPage1";
      this.lblInstructionPage1.Size = Util.ScaleSize(new Size(541, 23));
      this.lblInstructionPage1.TabIndex = 7;
      this.lblInstruciton3.ForeColor = Color.Black;
      this.lblInstruciton3.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(243));
      this.lblInstruciton3.Name = "lblInstruciton3";
      this.lblInstruciton3.Size = Util.ScaleSize(new Size(541, 28));
      this.lblInstruciton3.TabIndex = 6;
      this.lblInstruction2.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(170));
      this.lblInstruction2.Name = "lblInstruction2";
      this.lblInstruction2.Size = Util.ScaleSize(new Size(541, 61));
      this.lblInstruction2.TabIndex = 5;
      this.lblInstrucionRed.ForeColor = Color.Red;
      this.lblInstrucionRed.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(110));
      this.lblInstrucionRed.Name = "lblInstrucionRed";
      this.lblInstrucionRed.Size = Util.ScaleSize(new Size(541, 35));
      this.lblInstrucionRed.TabIndex = 4;
      this.lblInstruction1.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(93));
      this.lblInstruction1.Name = "lblInstruction1";
      this.lblInstruction1.Size = Util.ScaleSize(new Size(541, 26));
      this.lblInstruction1.TabIndex = 3;
      this.lblInstructions.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(63));
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = Util.ScaleSize(new Size(541, 26));
      this.lblInstructions.TabIndex = 2;
      this.label1.BorderStyle = BorderStyle.FixedSingle;
      this.label1.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(48));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(537, 1));
      this.label1.TabIndex = 1;
      this.lblTitle1.AutoSize = true;
      this.lblTitle1.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(16f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle1.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(13));
      this.lblTitle1.Name = "lblTitle1";
      this.lblTitle1.Size = Util.ScaleSize(new Size(0, 26));
      this.lblTitle1.TabIndex = 0;
      this.btnNext.Location = new Point(Util.ScaleSize(259), Util.ScaleSize(379));
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = Util.ScaleSize(new Size(75, 23));
      this.btnNext.TabIndex = 1;
      this.btnNext.Text = "Next";
      this.btnNext.UseVisualStyleBackColor = true;
      this.panelProfileName.BackColor = Color.White;
      this.panelProfileName.Controls.Add((Control) this.lblFooter2);
      this.panelProfileName.Controls.Add((Control) this.lblUserName);
      this.panelProfileName.Controls.Add((Control) this.lblInstruction2Page2);
      this.panelProfileName.Controls.Add((Control) this.lblDriveLetter);
      this.panelProfileName.Controls.Add((Control) this.txtProfileName);
      this.panelProfileName.Controls.Add((Control) this.lblInstructionPage2);
      this.panelProfileName.Controls.Add((Control) this.label4);
      this.panelProfileName.Controls.Add((Control) this.lblPageTitle);
      this.panelProfileName.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.panelProfileName.Name = "panelProfileName";
      this.panelProfileName.Size = Util.ScaleSize(new Size(570, 340));
      this.panelProfileName.TabIndex = 2;
      this.lblFooter2.Location = new Point(Util.ScaleSize(20), Util.ScaleSize(283));
      this.lblFooter2.Name = "lblFooter2";
      this.lblFooter2.Size = Util.ScaleSize(new Size(532, 20));
      this.lblFooter2.TabIndex = 7;
      this.lblUserName.Location = new Point(Util.ScaleSize(63), Util.ScaleSize(190));
      this.lblUserName.Name = "lblUserName";
      this.lblUserName.Size = Util.ScaleSize(new Size(193, 20));
      this.lblUserName.TabIndex = 6;
      this.lblInstruction2Page2.Location = new Point(Util.ScaleSize(20), Util.ScaleSize(155));
      this.lblInstruction2Page2.Name = "lblInstruction2Page2";
      this.lblInstruction2Page2.Size = Util.ScaleSize(new Size(532, 20));
      this.lblInstruction2Page2.TabIndex = 5;
      this.lblDriveLetter.Location = new Point(Util.ScaleSize(63), Util.ScaleSize(86));
      this.lblDriveLetter.Name = "lblDriveLetter";
      this.lblDriveLetter.Size = Util.ScaleSize(new Size(300, 13));
      this.lblDriveLetter.TabIndex = 4;
      this.txtProfileName.Location = new Point(Util.ScaleSize(63), Util.ScaleSize(213));
      this.txtProfileName.Name = "txtProfileName";
      this.txtProfileName.Size = Util.ScaleSize(new Size(485, 20));
      this.txtProfileName.TabIndex = 3;
      this.lblInstructionPage2.Location = new Point(Util.ScaleSize(20), Util.ScaleSize(61));
      this.lblInstructionPage2.Name = "lblInstructionPage2";
      this.lblInstructionPage2.Size = Util.ScaleSize(new Size(532, 20));
      this.lblInstructionPage2.TabIndex = 2;
      this.label4.BorderStyle = BorderStyle.FixedSingle;
      this.label4.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(48));
      this.label4.Name = "label4";
      this.label4.Size = Util.ScaleSize(new Size(538, 1));
      this.label4.TabIndex = 1;
      this.lblPageTitle.AutoSize = true;
      this.lblPageTitle.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(16f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPageTitle.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(13));
      this.lblPageTitle.Name = "lblPageTitle";
      this.lblPageTitle.Size = Util.ScaleSize(new Size(0, 26));
      this.lblPageTitle.TabIndex = 0;
      this.panelFinish.BackColor = Color.White;
      this.panelFinish.Controls.Add((Control) this.lblFinish);
      this.panelFinish.Controls.Add((Control) this.label8);
      this.panelFinish.Controls.Add((Control) this.lblTitleFinish);
      this.panelFinish.Location = new Point(Util.ScaleSize(12), Util.ScaleSize(12));
      this.panelFinish.Name = "panelFinish";
      this.panelFinish.Size = Util.ScaleSize(new Size(570, 340));
      this.panelFinish.TabIndex = 3;
      this.lblFinish.Location = new Point(Util.ScaleSize(18), Util.ScaleSize(61));
      this.lblFinish.Name = "lblFinish";
      this.lblFinish.Size = Util.ScaleSize(new Size(532, 25));
      this.lblFinish.TabIndex = 2;
      this.label8.BorderStyle = BorderStyle.FixedSingle;
      this.label8.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(48));
      this.label8.Name = "label8";
      this.label8.Size = Util.ScaleSize(new Size(537, 1));
      this.label8.TabIndex = 1;
      this.lblTitleFinish.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(16f), FontStyle.Bold);
      this.lblTitleFinish.Location = new Point(Util.ScaleSize(14), Util.ScaleSize(13));
      this.lblTitleFinish.Name = "lblTitleFinish";
      this.lblTitleFinish.Size = Util.ScaleSize(new Size(537, 26));
      this.lblTitleFinish.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.panelProfileName);
      this.panel1.Controls.Add((Control) this.panelFinish);
      this.panel1.Controls.Add((Control) this.panelInstructions);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(10));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(594, 363));
      this.panel1.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(614, 410));
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnNext);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProfileChecker);
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = nameof (ProfileChecker);
      this.panelInstructions.ResumeLayout(false);
      this.panelInstructions.PerformLayout();
      this.panelProfileName.ResumeLayout(false);
      this.panelProfileName.PerformLayout();
      this.panelFinish.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
