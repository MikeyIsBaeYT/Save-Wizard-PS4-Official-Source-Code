// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SerialValidateGG
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class SerialValidateGG : Form
  {
    public const string SERIAL_VALIDATE_URL = "{0}/ps4auth";
    public const string LICNESE_INFO = "{{\"action\":\"ACTIVATE_LICENSE\",\"license\":\"{0}\"}}";
    private const string REGISTER_UID = "{{\"action\":\"REGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}";
    private string m_serial;
    private string m_hash;
    private SerialValidateGG.CloseDelegate CloseForm;
    private SerialValidateGG.UpdateStatusDelegate UpdateStatus;
    private SerialValidateGG.EnableOkDelegate EnableOk;
    private int m_retryCount = 0;
    private string m_userid = "";
    private bool m_bRetry = false;
    private IContainer components = (IContainer) null;
    private Label label1;
    private Panel panel1;
    private Label lblInstruction;
    private Label lblInstruction2;
    private TextBox txtSerial4;
    private TextBox txtSerial3;
    private TextBox txtSerial2;
    private TextBox txtSerial1;
    private Button btnCancel;
    private Button btnOk;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label lblLicHelp;
    private LinkLabel lnkLicSupport;

    public SerialValidateGG()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.btnCancel.Text = PS3SaveEditor.Resources.Resources.btnCancel;
      this.btnOk.Text = PS3SaveEditor.Resources.Resources.btnOK;
      this.BackColor = System.Drawing.Color.FromArgb(80, 29, 11);
      this.panel1.BackColor = System.Drawing.Color.FromArgb(200, 100, 10);
      this.panel1.BackColor = System.Drawing.Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblInstruction.BackColor = System.Drawing.Color.Transparent;
      this.lblInstruction.TextAlign = ContentAlignment.MiddleCenter;
      this.lblInstruction2.BackColor = System.Drawing.Color.Transparent;
      this.label1.BackColor = this.label2.BackColor = this.label3.BackColor = this.label4.BackColor = System.Drawing.Color.Transparent;
      this.UpdateStatus = new SerialValidateGG.UpdateStatusDelegate(this.UpdateStatusSafe);
      this.CloseForm = new SerialValidateGG.CloseDelegate(this.CloseFormSafe);
      this.EnableOk = new SerialValidateGG.EnableOkDelegate(this.EnableOkSafe);
      if (Util.IsUnixOrMacOSX())
      {
        if (this.WindowState == FormWindowState.Minimized)
          this.WindowState = FormWindowState.Normal;
        this.Activate();
      }
      else
        Util.SetForegroundWindow(this.Handle);
      this.CenterToScreen();
      this.txtSerial1.TextChanged += new EventHandler(this.txtSerial_TextChanged);
      this.txtSerial1.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
      this.txtSerial1.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
      this.txtSerial2.TextChanged += new EventHandler(this.txtSerial_TextChanged);
      this.txtSerial2.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
      this.txtSerial2.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
      this.txtSerial3.TextChanged += new EventHandler(this.txtSerial_TextChanged);
      this.txtSerial3.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
      this.txtSerial3.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
      this.txtSerial4.TextChanged += new EventHandler(this.txtSerial_TextChanged);
      this.txtSerial4.KeyDown += new KeyEventHandler(this.txtSerial_KeyDown);
      this.txtSerial4.KeyPress += new KeyPressEventHandler(this.txtSerial_KeyPress);
      this.Text = string.Format(PS3SaveEditor.Resources.Resources.titleSerialEntry, (object) Util.PRODUCT_NAME);
      this.lblInstruction2.Text = PS3SaveEditor.Resources.Resources.lblInstruction2;
      this.lblInstruction.Text = "";
      this.lblInstruction2.Text = PS3SaveEditor.Resources.Resources.lblEnterSerial;
      this.lnkLicSupport.BackColor = this.lblLicHelp.BackColor = System.Drawing.Color.Transparent;
      this.lblLicHelp.Text = PS3SaveEditor.Resources.Resources.lblLicHelp;
      this.lnkLicSupport.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLblSupport_LinkClicked);
      this.Load += new EventHandler(this.SerialValidateGG_Load);
      this.btnOk.Enabled = false;
      if (this.m_serial != null)
        return;
      this.label1.Text = "";
    }

    private void linkLblSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo("http://" + this.lnkLicSupport.Text));

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, System.Drawing.Color.FromArgb(0, 138, 213), System.Drawing.Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void CheckForDevice()
    {
      Thread.Sleep(10000);
      this.m_serial = (string) null;
      this.FindGGUSB();
      if (this.m_serial == null)
        return;
      if (this.label1.IsHandleCreated)
        this.label1.Invoke((Delegate) this.UpdateStatus, (object) "Please wait. Registering Game Genie Save Editor for PS3.");
      this.RegisterSerial();
    }

    private void UpdateStatusSafe(string status) => this.label1.Text = status;

    protected override void WndProc(ref Message m)
    {
      if (m.Msg != 537 || m.WParam.ToInt32() != 32768 || !(m.LParam != IntPtr.Zero))
        ;
      base.WndProc(ref m);
    }

    private void SerialValidateGG_Load(object sender, EventArgs e)
    {
      this.txtSerial1.Select();
      if (this.m_serial == null)
        ;
    }

    private void RegisterSerial()
    {
      try
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        this.m_hash = SerialValidateGG.ComputeHash(this.m_serial);
        if (string.IsNullOrEmpty(Util.GetUID(register: true)))
        {
          int num = (int) Util.ShowMessage("There appears to have been an issue activating. Please contact support.");
        }
        else
        {
          string uriString = string.Format("{0}/ps4auth", (object) Util.GetBaseUrl(), (object) this.m_hash);
          webClientEx.DownloadStringAsync(new Uri(uriString, UriKind.Absolute));
          webClientEx.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.client_DownloadStringCompleted);
        }
      }
      catch (Exception ex)
      {
        int num1 = (int) Util.ShowMessage(ex.Message, ex.StackTrace);
        int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errSerial, PS3SaveEditor.Resources.Resources.msgError);
      }
    }

    public static string ComputeHash(string serial)
    {
      string str = "";
      byte[] buffer = new byte[32];
      byte[] numArray1 = new byte[16];
      byte[] numArray2 = new byte[16]
      {
        (byte) 59,
        (byte) 67,
        (byte) 235,
        (byte) 54,
        (byte) 183,
        (byte) 124,
        (byte) 22,
        (byte) 65,
        (byte) 172,
        (byte) 154,
        (byte) 31,
        (byte) 14,
        (byte) 188,
        (byte) 91,
        (byte) 48,
        (byte) 41
      };
      long num = long.Parse(serial, NumberStyles.HexNumber);
      byte[] numArray3 = (byte[]) null;
      if (serial.Length == 16)
      {
        byte[] bytes = BitConverter.GetBytes(num);
        Array.Reverse((Array) bytes, 0, bytes.Length);
        Array.Copy((Array) bytes, (Array) numArray1, bytes.Length);
        for (int index = 0; index < 8; ++index)
          buffer[index] = (byte) ((uint) numArray1[index] ^ (uint) numArray2[index]);
        Array.Copy((Array) Encoding.ASCII.GetBytes("GameGenie"), 0, (Array) buffer, 8, "GameGenie".Length);
        numArray3 = SHA1.Create().ComputeHash(buffer, 0, 8 + "GameGenie".Length);
      }
      else if (serial.Length == 20)
      {
        byte[] bytes = BitConverter.GetBytes(num);
        Array.Reverse((Array) bytes, 0, bytes.Length);
        Array.Copy((Array) bytes, 0, (Array) numArray1, 4, bytes.Length);
        for (int index = 0; index < 12; ++index)
          buffer[index] = (byte) ((uint) numArray1[index] ^ (uint) numArray2[index]);
        Array.Copy((Array) Encoding.ASCII.GetBytes("GameGenie"), 0, (Array) buffer, 12, "GameGenie".Length);
        numArray3 = SHA1.Create().ComputeHash(buffer, 0, 12 + "GameGenie".Length);
      }
      if (numArray3 != null)
      {
        for (int index = 0; index < numArray3.Length; ++index)
          str += numArray3[index].ToString("X2");
      }
      return str;
    }

    private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errSerial, PS3SaveEditor.Resources.Resources.msgError);
        this.Invoke((Delegate) this.CloseForm, (object) false);
      }
      else
      {
        string result = e.Result;
        if (result == null)
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSerial, PS3SaveEditor.Resources.Resources.msgError);
          this.Invoke((Delegate) this.CloseForm, (object) false);
        }
        else
        {
          if (result.IndexOf('#') > 0)
          {
            string[] strArray = result.Split('#');
            if (strArray.Length > 1)
            {
              if (strArray[0] == "4")
              {
                int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSerial, PS3SaveEditor.Resources.Resources.msgError);
                this.Invoke((Delegate) this.CloseForm, (object) false);
                return;
              }
              if (strArray[0] == "5")
              {
                int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errTooManyTimes, PS3SaveEditor.Resources.Resources.msgError);
                this.Invoke((Delegate) this.CloseForm, (object) false);
                return;
              }
            }
          }
          else
          {
            if (result.ToLower() == "toomanytimes" || result.ToLower().Contains("too many"))
            {
              int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errTooManyTimes, PS3SaveEditor.Resources.Resources.msgError);
              this.Invoke((Delegate) this.CloseForm, (object) false);
              return;
            }
            if (result == null || result.ToLower().Contains("error") || result.ToLower().Contains("not found"))
            {
              string str = result.Replace("ERROR", "");
              if (!str.Contains("1002"))
              {
                if (str.Contains("1014"))
                {
                  int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errOffline, PS3SaveEditor.Resources.Resources.msgInfo);
                  this.Invoke((Delegate) this.CloseForm, (object) false);
                  return;
                }
                if (str.Contains("1005"))
                {
                  int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errTooManyTimes + str, PS3SaveEditor.Resources.Resources.msgError);
                  this.Invoke((Delegate) this.CloseForm, (object) false);
                  return;
                }
                if (str.Contains("1007"))
                {
                  Util.GetUID(true, true);
                  this.RegisterSerial();
                }
                else
                {
                  if (this.m_serial == null)
                  {
                    int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSerial + str, PS3SaveEditor.Resources.Resources.msgError);
                  }
                  else
                  {
                    int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidUSB + str, PS3SaveEditor.Resources.Resources.msgError);
                  }
                  ++this.m_retryCount;
                  if (this.m_retryCount >= 3)
                  {
                    this.Invoke((Delegate) this.CloseForm, (object) false);
                    return;
                  }
                  if (this.m_serial == null)
                    this.btnOk.Invoke((Delegate) this.EnableOk, (object) true);
                  else
                    this.btnOk.Invoke((Delegate) this.EnableOk, (object) false);
                  this.label1.Invoke((Delegate) this.UpdateStatus, (object) "");
                  return;
                }
              }
            }
          }
          RegistryKey currentUser = Registry.CurrentUser;
          RegistryKey subKey = currentUser.CreateSubKey(Util.GetRegistryBase());
          if (this.m_serial == null)
          {
            string s = string.Format("{0}-{1}-{2}-{3}", (object) this.txtSerial1.Text, (object) this.txtSerial2.Text, (object) this.txtSerial3.Text, (object) this.txtSerial4.Text);
            this.m_hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(s)));
            this.m_hash = this.m_hash.Replace("-", "");
          }
          else
            this.m_hash = SerialValidateGG.ComputeHash(this.m_serial);
          subKey.SetValue("Hash", (object) this.m_hash.ToUpper());
          subKey.SetValue("BackupSaves", (object) "true");
          string str1 = string.Format("{0}-{1}-{2}-{3}", (object) this.txtSerial1.Text, (object) this.txtSerial2.Text, (object) this.txtSerial3.Text, (object) this.txtSerial4.Text);
          subKey.SetValue("Serial", (object) str1);
          subKey.Close();
          currentUser.Close();
          try
          {
            if (!this.IsHandleCreated)
              return;
            this.Invoke((Delegate) this.CloseForm, (object) true);
          }
          catch
          {
          }
        }
      }
    }

    private void EnableOkSafe(bool bEnable) => this.btnOk.Enabled = bEnable;

    private void CloseFormSafe(bool bSuccess)
    {
      if (!bSuccess)
        this.DialogResult = DialogResult.Abort;
      else
        this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void FindGGUSB()
    {
      foreach (USB.USBController hostController in USB.GetHostControllers())
        this.ProcessHub(hostController.GetRootHub());
    }

    private void ProcessHub(USB.USBHub hub)
    {
      foreach (USB.USBPort port in hub.GetPorts())
      {
        if (port.IsHub)
          this.ProcessHub(port.GetHub());
        USB.USBDevice device = port.GetDevice();
        if (device != null && device.DeviceManufacturer != null && device.DeviceManufacturer.ToLower() == "dpdev" && device.DeviceProduct != null && device.DeviceProduct.ToLower() == "gamegenie")
          this.m_serial = device.SerialNumber;
      }
    }

    private bool ValidateSerial()
    {
      for (int index = 1; index <= 4; ++index)
      {
        TextBox textBox = this.Controls.Find("txtSerial" + (object) index, true)[0] as TextBox;
        if (textBox.Text.Length < 4)
        {
          textBox.Focus();
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidSerial, this.Text);
          return false;
        }
      }
      return true;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.m_serial = (string) null;
      try
      {
        if (!this.ValidateSerial())
          return;
        this.btnOk.Invoke((Delegate) this.EnableOk, (object) false);
        this.RegisterLicense();
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errConnection);
        if (this.m_serial == null)
          this.btnOk.Enabled = true;
        this.btnCancel.Enabled = true;
      }
    }

    private void RegisterLicense()
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      this.label1.Text = PS3SaveEditor.Resources.Resources.msgWaitSerial;
      webClientEx.Headers[HttpRequestHeader.ContentType] = "application/json";
      webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
      webClientEx.UploadDataCompleted += new UploadDataCompletedEventHandler(this.client_UploadDataCompleted);
      webClientEx.UploadDataAsync(new Uri(string.Format("{0}/ps4auth", (object) Util.GetAuthBaseUrl()), UriKind.Absolute), "POST", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"ACTIVATE_LICENSE\",\"license\":\"{0}\"}}", (object) string.Format("{0}-{1}-{2}-{3}", (object) this.txtSerial1.Text, (object) this.txtSerial2.Text, (object) this.txtSerial3.Text, (object) this.txtSerial4.Text))));
    }

    private void client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
    {
      this.m_userid = "";
      if (e.Error is WebException && !this.m_bRetry)
      {
        this.m_bRetry = true;
        Util.ChangeAuthServer();
        this.btnOk_Click((object) null, (EventArgs) null);
      }
      else if (e.Error != null)
      {
        string str = "";
        if (e.Error is WebException)
        {
          WebException error = e.Error as WebException;
          if (error.Response is HttpWebResponse)
            str = Convert.ToString((object) (error.Response as HttpWebResponse).StatusCode);
        }
        this.btnOk.Invoke((Delegate) this.EnableOk, (object) true);
        if (string.IsNullOrEmpty(str))
        {
          int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errConnection);
        }
        else
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errConnection + " (" + str + ")");
        }
      }
      else
      {
        Dictionary<string, object> res = new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(e.Result), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
        if ((string) res["status"] == "ERROR" && res["code"].ToString() != "4020")
        {
          this.btnOk.Invoke((Delegate) this.EnableOk, (object) true);
          this.label1.Invoke((Delegate) this.UpdateStatus, (object) "");
          Util.ShowErrorMessage(res, PS3SaveEditor.Resources.Resources.errSerial);
        }
        else
        {
          this.m_userid = (string) res["userid"];
          this.RegisterUID();
        }
      }
    }

    private void RegisterUID()
    {
      string uid = Util.GetUID();
      if (string.IsNullOrEmpty(uid))
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errUnknown + " (2000)");
      }
      else
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        webClientEx.Headers[HttpRequestHeader.ContentType] = "application/json";
        webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
        webClientEx.UploadDataCompleted += new UploadDataCompletedEventHandler(this.client2_UploadDataCompleted);
        webClientEx.UploadDataAsync(new Uri(string.Format("{0}/ps4auth", (object) Util.GetAuthBaseUrl()), UriKind.Absolute), "POST", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"REGISTER_UUID\",\"userid\":\"{0}\",\"uuid\":\"{1}\"}}", (object) this.m_userid, (object) uid)));
      }
    }

    private void client2_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        string str = "";
        if (e.Error is WebException)
        {
          WebException error = (WebException) e.Error;
          if (error.Response is HttpWebResponse)
            str = Convert.ToString((object) ((HttpWebResponse) error.Response).StatusCode);
        }
        if (string.IsNullOrEmpty(str))
        {
          int num1 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errUnknown);
        }
        else
        {
          int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errUnknown + "(" + str + ")");
        }
        this.btnOk.Invoke((Delegate) this.EnableOk, (object) true);
        this.m_userid = "";
      }
      else
      {
        Dictionary<string, object> res = new JavaScriptSerializer().Deserialize(Encoding.ASCII.GetString(e.Result), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
        if ((string) res["status"] == "ERROR" && res["code"].ToString() != "4021")
        {
          this.m_userid = "";
          this.btnOk.Invoke((Delegate) this.EnableOk, (object) true);
          this.label1.Invoke((Delegate) this.UpdateStatus, (object) "");
          Util.ShowErrorMessage(res, PS3SaveEditor.Resources.Resources.errUnknown);
        }
        else
        {
          Util.SetRegistryValue("User", this.m_userid);
          this.Invoke((Delegate) this.CloseForm, (object) true);
        }
      }
    }

    private void txtSerial_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete || (sender as TextBox).Text.Length != 4)
        return;
      e.SuppressKeyPress = true;
    }

    private void txtSerial_KeyPress(object sender, KeyPressEventArgs e)
    {
      TextBox textBox1 = sender as TextBox;
      if (textBox1.Name == "txtSerial1" || textBox1.Text.Length != 0 || e.KeyChar != '\b')
        return;
      Control[] controlArray = textBox1.Parent.Controls.Find("txtSerial" + ((char) ((uint) textBox1.Name[9] - 1U)).ToString(), true);
      if (controlArray.Length == 1)
      {
        TextBox textBox2 = controlArray[0] as TextBox;
        if (textBox2.Text.Length > 0)
          textBox2.SelectionStart = textBox2.Text.Length;
        controlArray[0].Focus();
      }
    }

    private void txtSerial_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = sender as TextBox;
      int selectionStart = textBox.SelectionStart;
      textBox.Text = Regex.Replace(textBox.Text, "[^0-9a-zA-Z ]", "").ToUpperInvariant();
      textBox.SelectionStart = selectionStart;
      if (textBox.Name == "txtSerial1")
      {
        string[] strArray = Clipboard.GetText().Split('-');
        if (strArray.Length == 4)
        {
          strArray[0] = strArray[0].Trim();
          strArray[1] = strArray[1].Trim();
          strArray[2] = strArray[2].Trim();
          strArray[3] = strArray[3].Trim();
          if (strArray[0].Length != 4 || strArray[1].Length != 4 || strArray[2].Length != 4 || strArray[3].Length != 4)
            return;
          Clipboard.Clear();
          this.txtSerial1.Text = strArray[0];
          this.txtSerial2.Text = strArray[1];
          this.txtSerial3.Text = strArray[2];
          this.txtSerial4.Text = strArray[3];
        }
      }
      if (textBox.Text.Length == 4)
      {
        Control[] controlArray = textBox.Parent.Controls.Find("txtSerial" + ((char) ((uint) textBox.Name[9] + 1U)).ToString(), true);
        if (controlArray.Length == 1)
          controlArray[0].Focus();
      }
      if (this.txtSerial1.Text.Length == 4 && this.txtSerial2.Text.Length == 4 && this.txtSerial3.Text.Length == 4 && this.txtSerial4.Text.Length == 4)
      {
        this.btnOk.Enabled = true;
        this.btnOk.Focus();
      }
      else
        this.btnOk.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.txtSerial4 = new TextBox();
      this.txtSerial3 = new TextBox();
      this.txtSerial2 = new TextBox();
      this.txtSerial1 = new TextBox();
      this.lblInstruction2 = new Label();
      this.lblInstruction = new Label();
      this.lblLicHelp = new Label();
      this.lnkLicSupport = new LinkLabel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(Util.ScaleSize(65), Util.ScaleSize(78));
      this.label1.Name = "label1";
      this.label1.Size = Util.ScaleSize(new Size(299, 15));
      this.label1.TabIndex = 0;
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.panel1.Controls.Add((Control) this.lnkLicSupport);
      this.panel1.Controls.Add((Control) this.lblLicHelp);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.txtSerial4);
      this.panel1.Controls.Add((Control) this.txtSerial3);
      this.panel1.Controls.Add((Control) this.txtSerial2);
      this.panel1.Controls.Add((Control) this.txtSerial1);
      this.panel1.Controls.Add((Control) this.lblInstruction2);
      this.panel1.Controls.Add((Control) this.lblInstruction);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(Util.ScaleSize(10), Util.ScaleSize(11));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(439, 120));
      this.panel1.TabIndex = 1;
      this.label4.AutoSize = true;
      this.label4.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(Util.ScaleSize(218), Util.ScaleSize(54));
      this.label4.Name = "label4";
      this.label4.Size = Util.ScaleSize(new Size(11, 13));
      this.label4.TabIndex = 12;
      this.label4.Text = "-";
      this.label3.AutoSize = true;
      this.label3.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(Util.ScaleSize(163), Util.ScaleSize(54));
      this.label3.Name = "label3";
      this.label3.Size = Util.ScaleSize(new Size(11, 13));
      this.label3.TabIndex = 11;
      this.label3.Text = "-";
      this.label2.AutoSize = true;
      this.label2.Font = new Font(Util.GetFontFamily(), Util.ScaleSize(8f), FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(Util.ScaleSize(107), Util.ScaleSize(54));
      this.label2.Name = "label2";
      this.label2.Size = Util.ScaleSize(new Size(11, 13));
      this.label2.TabIndex = 10;
      this.label2.Text = "-";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.ForeColor = System.Drawing.Color.Black;
      this.btnCancel.Location = new Point(Util.ScaleSize(3), Util.ScaleSize(54));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = Util.ScaleSize(new Size(55, 23));
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Visible = false;
      this.btnOk.ForeColor = System.Drawing.Color.Black;
      this.btnOk.Location = new Point(Util.ScaleSize(293), Util.ScaleSize(51));
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = Util.ScaleSize(new Size(75, 18));
      this.btnOk.TabIndex = 8;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.txtSerial4.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial4.Location = new Point(Util.ScaleSize(232), Util.ScaleSize(51));
      this.txtSerial4.MaxLength = 4;
      this.txtSerial4.Name = "txtSerial4";
      this.txtSerial4.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial4.TabIndex = 7;
      this.txtSerial3.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial3.Location = new Point(Util.ScaleSize(176), Util.ScaleSize(51));
      this.txtSerial3.MaxLength = 4;
      this.txtSerial3.Name = "txtSerial3";
      this.txtSerial3.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial3.TabIndex = 6;
      this.txtSerial2.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial2.Location = new Point(Util.ScaleSize(120), Util.ScaleSize(51));
      this.txtSerial2.MaxLength = 4;
      this.txtSerial2.Name = "txtSerial2";
      this.txtSerial2.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial2.TabIndex = 5;
      this.txtSerial1.CharacterCasing = CharacterCasing.Upper;
      this.txtSerial1.Location = new Point(Util.ScaleSize(64), Util.ScaleSize(51));
      this.txtSerial1.MaxLength = 4;
      this.txtSerial1.Name = "txtSerial1";
      this.txtSerial1.Size = Util.ScaleSize(new Size(40, 21));
      this.txtSerial1.TabIndex = 4;
      this.lblInstruction2.Location = new Point(Util.ScaleSize(5), Util.ScaleSize(25));
      this.lblInstruction2.Name = "lblInstruction2";
      this.lblInstruction2.Size = Util.ScaleSize(new Size(430, 15));
      this.lblInstruction2.TabIndex = 2;
      this.lblInstruction2.Text = "Sample Text";
      this.lblInstruction2.TextAlign = ContentAlignment.MiddleCenter;
      this.lblInstruction.AutoSize = true;
      this.lblInstruction.Location = new Point(Util.ScaleSize(13), Util.ScaleSize(8));
      this.lblInstruction.Name = "lblInstruction";
      this.lblInstruction.Size = Util.ScaleSize(new Size(0, 15));
      this.lblInstruction.TabIndex = 1;
      this.lblInstruction.TextAlign = ContentAlignment.MiddleCenter;
      this.lblLicHelp.ForeColor = System.Drawing.Color.White;
      this.lblLicHelp.Location = new Point(Util.ScaleSize(2), Util.ScaleSize(98));
      this.lblLicHelp.Name = "lblLicHelp";
      this.lblLicHelp.Size = Util.ScaleSize(new Size(290, 15));
      this.lblLicHelp.TabIndex = 13;
      this.lblLicHelp.TextAlign = ContentAlignment.MiddleRight;
      this.lnkLicSupport.ForeColor = System.Drawing.Color.White;
      this.lnkLicSupport.Location = new Point(Util.ScaleSize(295), Util.ScaleSize(98));
      this.lnkLicSupport.Name = "lnkLicSupport";
      this.lnkLicSupport.Size = Util.ScaleSize(new Size(120, 15));
      this.lnkLicSupport.TabIndex = 14;
      this.lnkLicSupport.TabStop = true;
      this.lnkLicSupport.Text = "www.savewizard.net";
      this.lnkLicSupport.LinkColor = System.Drawing.Color.White;
      this.lnkLicSupport.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = Util.ScaleSize(new Size(459, 142));
      this.Controls.Add((Control) this.panel1);
      this.ForeColor = System.Drawing.Color.White;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SerialValidateGG);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Registering Game Genie";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void CloseDelegate(bool bSuccess);

    private delegate void UpdateStatusDelegate(string status);

    private delegate void EnableOkDelegate(bool bEnable);
  }
}
