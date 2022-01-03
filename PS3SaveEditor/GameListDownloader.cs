// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.GameListDownloader
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Rss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class GameListDownloader : Form
  {
    private GameListDownloader.UpdateProgressDelegate UpdateProgress;
    private GameListDownloader.UpdateStatusDelegate UpdateStatus;
    private GameListDownloader.CloseDelegate CloseForm;
    private bool appClosing = false;
    private static string GAME_LIST_URL = "{0}/games?token={1}";
    public static string RSS_URL = "http://www.thesavewizard.com/app/rss";
    private IContainer components = (IContainer) null;
    private Label lblStatus;
    private PS4ProgressBar pbProgress;
    private Panel panel1;

    public string GameListXml { get; set; }

    public GameListDownloader()
    {
      string registryValue = Util.GetRegistryValue("Language");
      if (registryValue != null)
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(registryValue);
      this.InitializeComponent();
      this.CenterToScreen();
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      this.Font = Util.GetFontForPlatform(this.Font);
      this.ControlBox = false;
      this.BackColor = Color.FromArgb(80, 29, 11);
      this.panel1.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblStatus.BackColor = Color.Transparent;
      this.lblStatus.Text = PS3SaveEditor.Resources.Resources.gamelistDownloaderMsg;
      this.Text = PS3SaveEditor.Resources.Resources.gamelistDownloaderTitle;
      this.Visible = false;
      this.Load += new EventHandler(this.GameListDownloader_Load);
      this.UpdateProgress = new GameListDownloader.UpdateProgressDelegate(this.UpdateProgressSafe);
      this.UpdateStatus = new GameListDownloader.UpdateStatusDelegate(this.UpdateStatusSafe);
      this.CloseForm = new GameListDownloader.CloseDelegate(this.CloseFormSafe);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0, 138, 213), Color.FromArgb(0, 44, 101), 90f))
        e.Graphics.FillRectangle((Brush) linearGradientBrush, this.ClientRectangle);
    }

    private void CloseThisForm(bool bSuccess)
    {
      if (this.IsDisposed)
        return;
      this.Invoke((Delegate) this.CloseForm, (object) bSuccess);
    }

    private void CloseFormSafe(bool bSuccess)
    {
      this.appClosing = true;
      this.DialogResult = bSuccess ? DialogResult.OK : DialogResult.Abort;
      this.Close();
    }

    private void SetStatus(string status) => this.lblStatus.Invoke((Delegate) this.UpdateStatus, (object) status);

    private void UpdateStatusSafe(string status) => this.lblStatus.Text = status;

    private void SetProgress(int val) => this.pbProgress.Invoke((Delegate) this.UpdateProgress, (object) val);

    private void UpdateProgressSafe(int val) => this.pbProgress.Value = val;

    private void GameListDownloader_Load(object sender, EventArgs e)
    {
      Thread thread = new Thread(new ThreadStart(this.GetOnlineGamesList));
      string registryValue = Util.GetRegistryValue("Language");
      if (registryValue != null)
        thread.CurrentUICulture = new CultureInfo(registryValue);
      thread.Start();
      try
      {
        long ticks = DateTime.Now.Ticks;
        if (!Util.IsHyperkin())
          GameListDownloader.RSS_URL = string.Format("{0}/ps4/rss?token={1}", (object) Util.GetBaseUrl(), (object) Util.GetAuthToken());
        RssChannel channel = RssFeed.Read(GameListDownloader.RSS_URL).Channels[0];
        if (channel.Items.Count <= 0)
          return;
        int num = (int) new RSSForm(channel).ShowDialog();
      }
      catch (Exception ex)
      {
      }
    }

    private void GetOnlineGamesList()
    {
      string gamelistPath = Util.GetGamelistPath();
      string str1 = "";
      if (System.IO.File.Exists(gamelistPath))
        str1 = Util.GetHash(gamelistPath);
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(string.Format(GameListDownloader.GAME_LIST_URL + "&checksum={2}", (object) Util.GetBaseUrl(), (object) Util.GetAuthToken(), (object) str1));
        httpWebRequest.Method = "GET";
        httpWebRequest.Credentials = (ICredentials) Util.GetNetworkCredential();
        string str2 = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
        httpWebRequest.UserAgent = Util.GetUserAgent();
        httpWebRequest.Headers.Add("Authorization", str2);
        httpWebRequest.Headers.Add("accept-charset", "UTF-8");
        this.SetStatus(PS3SaveEditor.Resources.Resources.msgConnecting);
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        if (HttpStatusCode.OK == response.StatusCode)
        {
          this.SetStatus(PS3SaveEditor.Resources.Resources.msgDownloadingList);
          Stream responseStream = response.GetResponseStream();
          int num1 = 0;
          int num2 = 60;
          if (response.ContentLength > (long) num2 && System.IO.File.Exists(gamelistPath))
            System.IO.File.Delete(gamelistPath);
          FileStream fileStream = new FileStream(gamelistPath, FileMode.OpenOrCreate, FileAccess.Write);
          byte[] buffer = new byte[1024];
          if (response.ContentLength < (long) num2)
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
              Dictionary<string, object> dictionary = new JavaScriptSerializer().Deserialize(streamReader.ReadToEnd(), typeof (Dictionary<string, object>)) as Dictionary<string, object>;
              if (!dictionary.ContainsKey("status") || !((string) dictionary["status"] == "OK"))
              {
                int count = responseStream.Read(buffer, 0, (int) response.ContentLength);
                fileStream.Write(buffer, 0, count);
              }
            }
          }
          else
          {
            while ((long) num1 < response.ContentLength)
            {
              int count = responseStream.Read(buffer, 0, Math.Min(1024, (int) response.ContentLength - num1));
              fileStream.Write(buffer, 0, count);
              num1 += count;
              this.SetProgress((int) ((long) (num1 * 100) / response.ContentLength));
            }
          }
          this.SetProgress(100);
          fileStream.Close();
          response.Close();
        }
        else
        {
          int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errInvalidResponse);
          this.GameListXml = "";
          this.CloseThisForm(false);
          return;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errConnection, PS3SaveEditor.Resources.Resources.msgError);
        this.GameListXml = "";
        this.CloseThisForm(false);
        throw ex;
      }
      this.GameListXml = "";
      if (System.IO.File.Exists(gamelistPath))
        this.GameListXml = System.IO.File.ReadAllText(gamelistPath);
      if (this.GameListXml == "" || this.GameListXml.IndexOf("ERROR") > 0)
      {
        int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errServer);
        this.GameListXml = "";
        this.CloseThisForm(false);
      }
      else
      {
        try
        {
          using (FileStream fileStream = new FileStream(gamelistPath, FileMode.Open, FileAccess.Read))
          {
            fileStream.Seek(0L, SeekOrigin.Begin);
            using (GZipStream gzipStream = new GZipStream((Stream) fileStream, CompressionMode.Decompress))
            {
              byte[] buffer = new byte[4096];
              using (MemoryStream memoryStream = new MemoryStream())
              {
                int count;
                do
                {
                  count = gzipStream.Read(buffer, 0, 4096);
                  if (count > 0)
                    memoryStream.Write(buffer, 0, count);
                }
                while (count > 0);
                this.GameListXml = Encoding.UTF8.GetString(memoryStream.ToArray());
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(500);
        this.CloseThisForm(true);
      }
    }

    private void GameListDownloader_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.appClosing)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblStatus = new Label();
      this.pbProgress = new PS4ProgressBar();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(26));
      this.lblStatus.Margin = new Padding(Util.ScaleSize(4), 0, Util.ScaleSize(4), 0);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = Util.ScaleSize(new Size(325, 17));
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "Please wait while the game list being downloaded..";
      this.pbProgress.Location = new Point(Util.ScaleSize(15), Util.ScaleSize(57));
      this.pbProgress.Margin = new Padding(Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4));
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = Util.ScaleSize(new Size(536, 23));
      this.pbProgress.TabIndex = 1;
      this.panel1.BackColor = Color.FromArgb(102, 102, 102);
      this.panel1.Controls.Add((Control) this.lblStatus);
      this.panel1.Controls.Add((Control) this.pbProgress);
      this.panel1.Location = new Point(Util.ScaleSize(16), Util.ScaleSize(15));
      this.panel1.Margin = new Padding(Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4));
      this.panel1.Name = "panel1";
      this.panel1.Size = Util.ScaleSize(new Size(569, 128));
      this.panel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = Util.ScaleSize(new Size(604, 160));
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = PS3SaveEditor.Resources.Resources.dp;
      this.Margin = new Padding(Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4), Util.ScaleSize(4));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GameListDownloader);
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = "Downloading Games List from Server";
      this.FormClosing += new FormClosingEventHandler(this.GameListDownloader_FormClosing);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void UpdateProgressDelegate(int value);

    private delegate void UpdateStatusDelegate(string status);

    private delegate void CloseDelegate(bool bSuccess);
  }
}
