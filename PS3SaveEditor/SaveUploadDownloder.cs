// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.SaveUploadDownloder
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using Ionic.Crc;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class SaveUploadDownloder : UserControl
  {
    private SaveUploadDownloder.UpdateProgressDelegate UpdateProgress;
    private SaveUploadDownloder.UpdateStatusDelegate UpdateStatus;
    private static string UPLOAD_URL = "/request?token={0}";
    private const string SESSION_CHECK_URL = "{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}";
    private Thread t = (Thread) null;
    private long start = 0;
    private IContainer components = (IContainer) null;
    private Label lblStatus;
    private PS4ProgressBar pbProgress;
    private PS4ProgressBar pbOverallProgress;
    private Label lblCurrentProgress;
    private Label lblTotalProgress;

    public ManualResetEvent AbortEvent { get; set; }

    public ProgressBar ProgressBar { get; set; }

    public Label StatusLabel { get; set; }

    public string Action { get; set; }

    public game Game { get; set; }

    public bool IsUpload { get; set; }

    public string FilePath { get; set; }

    public string[] Files { get; set; }

    public string SaveId { get; set; }

    public string OutputFolder { get; set; }

    public string ListResult { get; set; }

    public List<string> OrderedEntries { get; set; }

    public event SaveUploadDownloder.DownloadStartEventHandler DownloadStart;

    public event SaveUploadDownloder.UploadStartEventHandler UploadStart;

    public event SaveUploadDownloder.DownloadFinishEventHandler DownloadFinish;

    public event SaveUploadDownloder.UploadFinishEventHandler UploadFinish;

    public string Profile { get; set; }

    public SaveUploadDownloder()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.BackColor = Color.FromArgb(200, 100, 10);
      this.AbortEvent = new ManualResetEvent(false);
      this.BackColor = Color.FromArgb((int) sbyte.MaxValue, 204, 204, 204);
      this.lblStatus.BackColor = Color.Transparent;
      this.lblCurrentProgress.BackColor = Color.Transparent;
      this.Disposed += new EventHandler(this.SaveUploadDownloder_Disposed);
      this.lblTotalProgress.BackColor = Color.Transparent;
      this.lblStatus.ForeColor = Color.White;
      this.lblCurrentProgress.ForeColor = Color.White;
      this.lblTotalProgress.ForeColor = Color.White;
      this.UpdateProgress = new SaveUploadDownloder.UpdateProgressDelegate(this.UpdateProgressSafe);
      this.UpdateStatus = new SaveUploadDownloder.UpdateStatusDelegate(this.UpdateStatusSafe);
      this.ProgressBar = (ProgressBar) this.pbProgress;
      this.StatusLabel = this.lblStatus;
    }

    private void SaveUploadDownloder_Disposed(object sender, EventArgs e) => this.AbortEvent.Dispose();

    private bool CheckCompressability()
    {
      using (FileStream fileStream = System.IO.File.OpenRead(this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)))
      {
        if (fileStream.Length < 1048576L)
          return false;
        fileStream.Seek(fileStream.Length - 1048576L, SeekOrigin.Begin);
        byte[] buffer1 = new byte[1048576];
        byte[] buffer2 = new byte[1048576];
        fileStream.Read(buffer1, 0, 1048576);
        using (MemoryStream memoryStream1 = new MemoryStream())
        {
          using (MemoryStream memoryStream2 = new MemoryStream(buffer1))
          {
            using (ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) memoryStream1))
            {
              zipOutputStream.CompressionLevel = CompressionLevel.BestCompression;
              zipOutputStream.PutNextEntry("temp");
              StreamUtils.Copy((Stream) memoryStream2, (Stream) zipOutputStream, buffer2);
              return (double) memoryStream1.Length / (double) buffer1.Length < 0.8;
            }
          }
        }
      }
    }

    private bool BackupSaveData()
    {
      if (Util.GetRegistryValue("BackupSaves") == "false" || this.Action == "resign")
        return this.CheckCompressability();
      string path = (string) null;
      if (this.Game != null)
        path = Path.GetDirectoryName(this.Game.LocalSaveFolder);
      if (!System.IO.File.Exists(this.FilePath))
        return false;
      this.SetStatus(PS3SaveEditor.Resources.Resources.lblBackingUp);
      string str = Util.GetBackupLocation() + Path.DirectorySeparatorChar.ToString() + this.Game.PSN_ID + "_" + Path.GetFileName(path) + "_" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".bak";
      this.SetProgress(0, new int?(30));
      string asZipFile = ZipUtil.GetAsZipFile(new string[2]
      {
        this.Game.LocalSaveFolder,
        this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)
      }, new ZipUtil.OnZipProgress(this.OnProgress));
      System.IO.File.Copy(asZipFile, str, true);
      System.IO.File.Delete(asZipFile);
      return (double) (new FileInfo(str).Length / new FileInfo(this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)).Length) < 0.7;
    }

    protected virtual void RaiseDownloadFinishEvent(bool bSuccess, string error) => this.DownloadFinish((object) this, new DownloadFinishEventArgs(bSuccess, error));

    protected virtual void RaiseUploadFinishEvent() => this.UploadFinish((object) this, new EventArgs());

    protected virtual void RaiseUploadStartEvent() => this.UploadStart((object) this, new EventArgs());

    protected virtual void RaiseDownloadStartEvent() => this.DownloadStart((object) this, new EventArgs());

    public void Start()
    {
      this.t = new Thread(new ThreadStart(this.UploadFile));
      string registryValue = Util.GetRegistryValue("Language");
      if (registryValue != null)
        this.t.CurrentUICulture = new CultureInfo(registryValue);
      this.t.Start();
    }

    public void SetStatus(string status)
    {
      if (!this.IsHandleCreated)
        return;
      this.lblStatus.Invoke((Delegate) this.UpdateStatus, (object) status);
    }

    private void UpdateStatusSafe(string status) => this.lblStatus.Text = status;

    private void SetProgress(int val, int? overall)
    {
      if (this.start <= 0L)
        ;
      if (!this.IsHandleCreated)
        return;
      this.pbProgress.Invoke((Delegate) this.UpdateProgress, (object) val, (object) overall);
    }

    private void UpdateProgressSafe(int val, int? overall)
    {
      this.pbProgress.Value = val;
      if (!overall.HasValue)
        return;
      this.pbOverallProgress.Value = overall.Value;
    }

    public bool OnProgress(int progress)
    {
      if (this.AbortEvent.WaitOne(0))
        return false;
      this.SetProgress(progress, new int?());
      return true;
    }

    public static void ErrorMessage(Form Parent, string errorMessage, string title = null)
    {
      if (Parent != null && Parent.InvokeRequired)
      {
        if (title != null)
        {
          int num;
          Parent.Invoke((Delegate) (() => num = (int) Util.ShowMessage(Parent, errorMessage, title)));
        }
        else
        {
          int num;
          Parent.Invoke((Delegate) (() => num = (int) Util.ShowMessage(Parent, errorMessage)));
        }
      }
      else if (title != null)
      {
        int num1 = (int) Util.ShowMessage(Parent, errorMessage, title);
      }
      else
      {
        int num2 = (int) Util.ShowMessage(Parent, errorMessage);
      }
    }

    private void UploadFile()
    {
      this.SetStatus(PS3SaveEditor.Resources.Resources.lblCheckSession);
      this.SetStatus(PS3SaveEditor.Resources.Resources.lblPreparing);
      if (this.Game.PFSZipEntry != null)
      {
        string path = Path.Combine(Util.GetTempFolder(), Path.GetFileName(this.Game.PFSZipEntry.FileName));
        using (CrcCalculatorStream calculatorStream = this.Game.PFSZipEntry.OpenReader())
        {
          using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
          {
            byte[] buffer = new byte[4096];
            StreamUtils.Copy((Stream) calculatorStream, (Stream) fileStream, buffer, (ProgressHandler) ((a, e) => this.OnProgress((int) e.PercentComplete)), TimeSpan.FromSeconds(1.0), (object) this, "Extract");
          }
        }
        this.Game.LocalSaveFolder = path + ".bin";
        using (CrcCalculatorStream calculatorStream = this.Game.BinZipEntry.OpenReader())
        {
          using (FileStream fileStream = new FileStream(this.Game.LocalSaveFolder, FileMode.Create, FileAccess.Write))
          {
            byte[] buffer = new byte[4096];
            StreamUtils.Copy((Stream) calculatorStream, (Stream) fileStream, buffer);
          }
        }
      }
      string pfsHash = (string) null;
      if (this.Files != null)
      {
        this.SetProgress(0, new int?(0));
        if (this.Action == "decrypt" || this.Action == "list" || this.Action == "patch" || this.Action == "resign")
        {
          int length1 = 4096;
          if (Util.CurrentPlatform == Util.Platform.MacOS)
            length1 = 4194304;
          long num1 = 0;
          using (FileStream fileStream = System.IO.File.OpenRead(this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4)))
          {
            using (HashAlgorithm hashAlgorithm = (HashAlgorithm) MD5.Create())
            {
              long length2 = fileStream.Length;
              byte[] buffer = new byte[length1];
              int num2 = fileStream.Read(buffer, 0, buffer.Length);
              long num3 = num1 + (long) num2;
              do
              {
                int inputCount = num2;
                byte[] numArray = buffer;
                buffer = new byte[length1];
                num2 = fileStream.Read(buffer, 0, buffer.Length);
                num3 += (long) num2;
                if (num2 == 0)
                  hashAlgorithm.TransformFinalBlock(numArray, 0, inputCount);
                else
                  hashAlgorithm.TransformBlock(numArray, 0, inputCount, numArray, 0);
                this.SetProgress((int) ((double) num3 * 100.0 / (double) length2), new int?(0));
                if (this.AbortEvent.WaitOne(0))
                {
                  fileStream.Close();
                  this.RaiseDownloadFinishEvent(false, "Abort");
                  return;
                }
              }
              while ((uint) num2 > 0U);
              pfsHash = BitConverter.ToString(hashAlgorithm.Hash).Replace("-", "").ToLower();
              List<string> lstSaveFiles = new List<string>((IEnumerable<string>) this.Files);
              string contents;
              if (this.Action == "decrypt" || this.Action == "list" || this.Action == "patch" || this.Action == "resign")
              {
                contents = (!(this.Action == "decrypt") && !(this.Action == "list") ? this.Game.ToString(true, lstSaveFiles) : this.Game.ToString(new List<string>())).Replace("<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name>", "<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name><md5>" + pfsHash + "</md5>");
                if (this.Action == "resign")
                  contents = contents.Replace("mode=\"patch\"", "mode=\"resign\"").Replace("</pfs>", "</pfs><psnid>" + this.Profile + "</psnid>");
                if (lstSaveFiles.IndexOf(this.Game.LocalSaveFolder) < 0)
                  lstSaveFiles.Add(this.Game.LocalSaveFolder);
              }
              else
              {
                contents = this.Game.ToString(true, lstSaveFiles).Replace("<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name>", "<name>" + Path.GetFileNameWithoutExtension(this.Game.LocalSaveFolder) + "</name><md5>" + pfsHash + "</md5>");
                lstSaveFiles = this.Game.GetContainerFiles();
              }
              string path = Path.Combine(Util.GetTempFolder(), "ps4_list.xml");
              System.IO.File.WriteAllText(path, contents);
              lstSaveFiles.Add(path);
              this.Files = lstSaveFiles.ToArray();
            }
          }
          this.SetProgress(0, new int?(20));
        }
        this.FilePath = ZipUtil.GetAsZipFile(this.Files, this.Profile, new ZipUtil.OnZipProgress(this.OnProgress));
      }
      bool flag = false;
      if (this.Action == "patch" || this.Action == "resign" || this.Action == "encrypt")
        flag = this.BackupSaveData();
      if (this.Action == "decrypt" || this.Action == "list")
        flag = this.CheckCompressability();
      this.SetProgress(0, new int?(40));
      NameValueCollection nvc = new NameValueCollection();
      nvc.Add("form_id", "request_form");
      if (this.Game != null)
      {
        nvc.Add("gamecode", this.Game.id);
        if (!string.IsNullOrEmpty(this.Game.diskcode))
          nvc.Add("diskcode", this.Game.diskcode);
      }
      else if (!string.IsNullOrEmpty(this.OutputFolder) && this.Action != "download")
        nvc.Add("gameid", Path.GetFileName(this.OutputFolder).Substring(0, 9));
      if (this.SaveId != null)
        nvc.Add("saveid", this.SaveId);
      nvc.Add("action", this.Action);
      string str = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
      if (this.Action == "decrypt" || this.Action == "list" || this.Action == "patch" || this.Action == "resign")
      {
        if (flag)
        {
          this.SetStatus(PS3SaveEditor.Resources.Resources.lblCompressing);
          this.SetProgress(0, new int?(50));
          str = ZipUtil.GetAsZipFile(new string[1]
          {
            str
          }, new ZipUtil.OnZipProgress(this.OnProgress));
        }
        this.RaiseUploadStartEvent();
        if (!this.UploadChunks(str, pfsHash))
        {
          if (flag)
            System.IO.File.Delete(str);
          if (!this.AbortEvent.WaitOne(0))
            return;
          this.RaiseDownloadFinishEvent(false, "Abort");
          return;
        }
        if (flag)
          System.IO.File.Delete(str);
      }
      this.HttpUploadFile(string.Format("{0}{1}", (object) Util.GetBaseUrl(), (object) string.Format(SaveUploadDownloder.UPLOAD_URL, (object) Util.GetAuthToken())), this.FilePath, "files[input_zip_file]", "application/x-zip-compressed", nvc);
    }

    private bool CheckSession()
    {
      WebClientEx webClientEx = new WebClientEx();
      webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
      return !Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/ps4auth", Encoding.ASCII.GetBytes(string.Format("{{\"action\":\"SESSION_REFRESH\",\"userid\":\"{0}\",\"token\":\"{1}\"}}", (object) Util.GetUserId(), (object) Util.GetAuthToken())))).Contains("ERROR");
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
      if (this.t != null)
        this.t.Abort();
      base.OnHandleDestroyed(e);
    }

    public bool UploadChunks_(string file)
    {
      int num1 = 8;
      int count1 = 1048576;
      string fileName = Path.GetFileName(file);
      string hash1 = Util.GetHash(file);
      string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
      byte[] bytes1 = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
      long length = new FileInfo(file).Length;
      int num2 = (int) Math.Ceiling((double) length / 1024.0 * 1024.0);
      List<int> remainingChunks = this.GetRemainingChunks(fileName, hash1, boundary, length);
      if (remainingChunks != null && remainingChunks.Count == 0)
        return true;
      if (remainingChunks == null)
        return false;
      long num3 = (long) (remainingChunks.Count * count1);
      long num4 = 0;
      using (FileStream fileStream = System.IO.File.OpenRead(file))
      {
        for (int index1 = 0; index1 < remainingChunks.Count; index1 += num1)
        {
          HttpWebRequest webRequest = this.GetWebRequest(boundary);
          string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
          Stream requestStream = webRequest.GetRequestStream();
          int index2 = index1;
          int num5 = 1;
          while (index2 < Math.Min(remainingChunks.Count, index1 + num1))
          {
            fileStream.Seek((long) (index2 * 1024 * 1024), SeekOrigin.Begin);
            requestStream.Write(bytes1, 0, bytes1.Length);
            byte[] numArray = new byte[(int) Math.Min((long) count1, length - (long) (index2 * 1024 * 1024))];
            int count2 = fileStream.Read(numArray, 0, count1);
            string hash2 = Util.GetHash(numArray);
            byte[] bytes2 = Encoding.UTF8.GetBytes(string.Format(format, (object) ("chunk" + (object) num5 + "_md5"), (object) hash2));
            requestStream.Write(bytes2, 0, bytes2.Length);
            requestStream.Write(bytes1, 0, bytes1.Length);
            byte[] bytes3 = Encoding.UTF8.GetBytes(string.Format(format, (object) ("chunk" + (object) num5 + "id"), (object) string.Concat((object) remainingChunks[index2])));
            requestStream.Write(bytes3, 0, bytes3.Length);
            string s = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", (object) ("files[chunk" + (object) num5 + "]"), (object) ("chunk" + (object) remainingChunks[index2]), (object) "application/octet-stream");
            requestStream.Write(bytes1, 0, bytes1.Length);
            byte[] bytes4 = Encoding.UTF8.GetBytes(s);
            requestStream.Write(bytes4, 0, bytes4.Length);
            requestStream.Write(numArray, 0, count2);
            num4 += (long) count2;
            this.SetProgress((int) (num4 * 100L / num3), new int?(40));
            ++index2;
            ++num5;
          }
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes5 = Encoding.UTF8.GetBytes(string.Format(format, (object) "op", (object) "Submit"));
          requestStream.Write(bytes5, 0, bytes5.Length);
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes6 = Encoding.UTF8.GetBytes(string.Format(format, (object) "form_id", (object) "chunk_upload_form"));
          requestStream.Write(bytes6, 0, bytes6.Length);
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes7 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs_md5", (object) hash1));
          requestStream.Write(bytes7, 0, bytes7.Length);
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes8 = Encoding.UTF8.GetBytes(string.Format(format, (object) "total_chunks", (object) num2));
          requestStream.Write(bytes8, 0, bytes8.Length);
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes9 = Encoding.UTF8.GetBytes(string.Format(format, (object) "gamecode", (object) this.Game.id));
          requestStream.Write(bytes9, 0, bytes9.Length);
          if (!string.IsNullOrEmpty(this.Game.diskcode))
          {
            requestStream.Write(bytes1, 0, bytes1.Length);
            byte[] bytes10 = Encoding.UTF8.GetBytes(string.Format(format, (object) "diskcode", (object) this.Game.diskcode));
            requestStream.Write(bytes10, 0, bytes10.Length);
          }
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes11 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs", (object) fileName));
          requestStream.Write(bytes11, 0, bytes11.Length);
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes12 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs_size", (object) length));
          requestStream.Write(bytes12, 0, bytes12.Length);
          byte[] bytes13 = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
          requestStream.Write(bytes13, 0, bytes13.Length);
          requestStream.Close();
          HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
          if (response.StatusCode == HttpStatusCode.OK)
          {
            using (Stream responseStream = response.GetResponseStream())
            {
              using (StreamReader streamReader = new StreamReader(responseStream))
              {
                long contentLength = response.ContentLength;
                if (streamReader.ReadToEnd().IndexOf("true") > 0)
                  return true;
              }
            }
          }
        }
      }
      return false;
    }

    private List<int> GetRemainingChunks(
      string pfsFileName,
      string pfsHash,
      string boundary,
      long fileSize)
    {
      try
      {
        List<int> intList1 = new List<int>();
        int num = (int) Math.Ceiling((double) fileSize / 1024.0 * 1024.0);
        HttpWebRequest webRequest = this.GetWebRequest(boundary);
        Stream requestStream = webRequest.GetRequestStream();
        string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        byte[] bytes1 = Encoding.ASCII.GetBytes(boundary);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes2 = Encoding.UTF8.GetBytes(string.Format(format, (object) "op", (object) "Submit"));
        requestStream.Write(bytes2, 0, bytes2.Length);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes3 = Encoding.UTF8.GetBytes(string.Format(format, (object) "form_id", (object) "chunk_upload_form"));
        requestStream.Write(bytes3, 0, bytes3.Length);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes4 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs_md5", (object) pfsHash));
        requestStream.Write(bytes4, 0, bytes4.Length);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes5 = Encoding.UTF8.GetBytes(string.Format(format, (object) "total_chunks", (object) num));
        requestStream.Write(bytes5, 0, bytes5.Length);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes6 = Encoding.UTF8.GetBytes(string.Format(format, (object) "gamecode", (object) this.Game.id));
        requestStream.Write(bytes6, 0, bytes6.Length);
        if (!string.IsNullOrEmpty(this.Game.diskcode))
        {
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes7 = Encoding.UTF8.GetBytes(string.Format(format, (object) "diskcode", (object) this.Game.diskcode));
          requestStream.Write(bytes7, 0, bytes7.Length);
        }
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes8 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs", (object) pfsFileName));
        requestStream.Write(bytes8, 0, bytes8.Length);
        requestStream.Write(bytes1, 0, bytes1.Length);
        byte[] bytes9 = Encoding.UTF8.GetBytes(string.Format(format, (object) "pfs_size", (object) fileSize));
        requestStream.Write(bytes9, 0, bytes9.Length);
        byte[] bytes10 = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        requestStream.Write(bytes10, 0, bytes10.Length);
        requestStream.Close();
        HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
        if (response.StatusCode == HttpStatusCode.OK)
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
              long contentLength = response.ContentLength;
              string end = streamReader.ReadToEnd();
              if (end.IndexOf("true") > 0)
                return intList1;
              Dictionary<string, object> dictionary1 = new JavaScriptSerializer().Deserialize(end, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
              if (dictionary1.ContainsKey("remaining_chunks"))
              {
                Dictionary<string, object> dictionary2 = dictionary1["remaining_chunks"] as Dictionary<string, object>;
                List<int> intList2 = new List<int>();
                foreach (string key in dictionary2.Keys)
                {
                  if (!(bool) dictionary2[key])
                    intList2.Add(int.Parse(key));
                }
                return intList2;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return (List<int>) null;
      }
      return (List<int>) null;
    }

    private HttpWebRequest GetWebRequest(string boundary)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Util.GetBaseUrl() + "/chunk_upload?token=" + Util.GetAuthToken());
      httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
      httpWebRequest.AllowWriteStreamBuffering = true;
      httpWebRequest.PreAuthenticate = true;
      httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
      httpWebRequest.Method = "POST";
      httpWebRequest.UserAgent = Util.GetUserAgent();
      httpWebRequest.ProtocolVersion = new Version(1, 1);
      httpWebRequest.KeepAlive = true;
      ServicePointManager.Expect100Continue = false;
      httpWebRequest.Credentials = (ICredentials) Util.GetNetworkCredential();
      httpWebRequest.Timeout = 600000;
      httpWebRequest.ReadWriteTimeout = 600000;
      httpWebRequest.SendChunked = true;
      string str = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
      httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
      httpWebRequest.Headers.Add("Authorization", str);
      return httpWebRequest;
    }

    public bool UploadChunks(string file, string pfsHash)
    {
      int val2 = 8;
      int count = 1048576;
      string str1 = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
      string fileName = Path.GetFileName(str1);
      if (string.IsNullOrEmpty(pfsHash))
        pfsHash = Util.GetHash(str1);
      List<int> intList = new List<int>();
      string str2 = "---------------------------" + DateTime.Now.Ticks.ToString("x");
      byte[] bytes1 = Encoding.ASCII.GetBytes("\r\n--" + str2 + "\r\n");
      int num1 = 0;
      try
      {
        using (FileStream fileStream = System.IO.File.Open(file, FileMode.Open))
        {
          int num2 = (int) Math.Ceiling((double) fileStream.Length / (double) count);
          long num3 = 0;
          long length1 = fileStream.Length;
          long num4 = length1;
          bool flag = true;
          this.SetProgress(0, new int?(60));
          while (true)
          {
            try
            {
              if (this.AbortEvent.WaitOne(0))
              {
                fileStream.Close();
                return false;
              }
              HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Util.GetBaseUrl() + "/chunk_upload?token=" + Util.GetAuthToken());
              httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
              httpWebRequest.AllowWriteStreamBuffering = true;
              httpWebRequest.PreAuthenticate = true;
              httpWebRequest.ContentType = "multipart/form-data; boundary=" + str2;
              httpWebRequest.Method = "POST";
              httpWebRequest.UserAgent = Util.GetUserAgent();
              httpWebRequest.ProtocolVersion = new Version(1, 1);
              httpWebRequest.KeepAlive = true;
              ServicePointManager.Expect100Continue = false;
              httpWebRequest.Credentials = (ICredentials) Util.GetNetworkCredential();
              httpWebRequest.Timeout = 600000;
              httpWebRequest.ReadWriteTimeout = 600000;
              httpWebRequest.SendChunked = true;
              string str3 = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
              httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
              httpWebRequest.Headers.Add("Authorization", str3);
              string format1 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
              long num5 = 0L + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "form_id", (object) "chunk_upload_form")).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "op", (object) "Submit")).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "pfs_md5", (object) pfsHash)).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "total_chunks", (object) num2)).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "gamecode", (object) this.Game.id)).Length;
              if (!string.IsNullOrEmpty(this.Game.diskcode))
                num5 = num5 + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "diskcode", (object) this.Game.diskcode)).Length;
              long num6 = num5 + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "pfs", (object) fileName)).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) "pfs_size", (object) length1)).Length;
              Dictionary<int, byte[]> dictionary1 = new Dictionary<int, byte[]>();
              if (!flag)
              {
                int index = 0;
                int num7 = 1;
                while (index < Math.Min(intList.Count, val2))
                {
                  byte[] numArray1 = new byte[count];
                  fileStream.Seek((long) ((intList[index] - 1) * count), SeekOrigin.Begin);
                  int length2 = fileStream.Read(numArray1, 0, count);
                  if (length2 < count)
                  {
                    byte[] numArray2 = new byte[length2];
                    Array.Copy((Array) numArray1, (Array) numArray2, length2);
                    numArray1 = numArray2;
                  }
                  dictionary1.Add(intList[index], numArray1);
                  string hash = Util.GetHash(numArray1);
                  num6 = num6 + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) ("chunk" + (object) num7 + "_md5"), (object) hash)).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format(format1, (object) ("chunk" + (object) num7 + "id"), (object) string.Concat((object) intList[index]))).Length + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", (object) ("files[chunk" + (object) num7 + "]"), (object) ("chunk" + (object) intList[index]), (object) "application/octet-stream")).Length + (long) length2;
                  ++index;
                  ++num7;
                }
              }
              if (!flag && dictionary1.Count == 0)
                return false;
              byte[] bytes2 = Encoding.ASCII.GetBytes("\r\n--" + str2 + "--\r\n");
              long num8 = num6 + (long) bytes2.Length;
              httpWebRequest.ContentLength = num8;
              Stream requestStream = httpWebRequest.GetRequestStream();
              string format2 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
              if (!flag)
              {
                int index = 0;
                int num9 = 1;
                while (index < Math.Min(intList.Count, val2))
                {
                  requestStream.Write(bytes1, 0, bytes1.Length);
                  byte[] numArray = dictionary1[intList[index]];
                  int length3 = numArray.Length;
                  string hash = Util.GetHash(numArray);
                  byte[] bytes3 = Encoding.UTF8.GetBytes(string.Format(format2, (object) ("chunk" + (object) num9 + "_md5"), (object) hash));
                  requestStream.Write(bytes3, 0, bytes3.Length);
                  requestStream.Write(bytes1, 0, bytes1.Length);
                  byte[] bytes4 = Encoding.UTF8.GetBytes(string.Format(format2, (object) ("chunk" + (object) num9 + "id"), (object) string.Concat((object) intList[index])));
                  requestStream.Write(bytes4, 0, bytes4.Length);
                  string s = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", (object) ("files[chunk" + (object) num9 + "]"), (object) ("chunk" + (object) intList[index]), (object) "application/octet-stream");
                  requestStream.Write(bytes1, 0, bytes1.Length);
                  byte[] bytes5 = Encoding.UTF8.GetBytes(s);
                  requestStream.Write(bytes5, 0, bytes5.Length);
                  requestStream.Write(numArray, 0, length3);
                  num3 += (long) length3;
                  this.SetProgress(Math.Min(100, (int) (num3 * 100L / num4)), new int?());
                  ++index;
                  ++num9;
                }
              }
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes6 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "op", (object) "Submit"));
              requestStream.Write(bytes6, 0, bytes6.Length);
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes7 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "form_id", (object) "chunk_upload_form"));
              requestStream.Write(bytes7, 0, bytes7.Length);
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes8 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "pfs_md5", (object) pfsHash));
              requestStream.Write(bytes8, 0, bytes8.Length);
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes9 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "total_chunks", (object) num2));
              requestStream.Write(bytes9, 0, bytes9.Length);
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes10 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "gamecode", (object) this.Game.id));
              requestStream.Write(bytes10, 0, bytes10.Length);
              if (!string.IsNullOrEmpty(this.Game.diskcode))
              {
                requestStream.Write(bytes1, 0, bytes1.Length);
                byte[] bytes11 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "diskcode", (object) this.Game.diskcode));
                requestStream.Write(bytes11, 0, bytes11.Length);
              }
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes12 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "pfs", (object) fileName));
              requestStream.Write(bytes12, 0, bytes12.Length);
              requestStream.Write(bytes1, 0, bytes1.Length);
              byte[] bytes13 = Encoding.UTF8.GetBytes(string.Format(format2, (object) "pfs_size", (object) length1));
              requestStream.Write(bytes13, 0, bytes13.Length);
              byte[] bytes14 = Encoding.ASCII.GetBytes("\r\n--" + str2 + "--\r\n");
              requestStream.Write(bytes14, 0, bytes14.Length);
              requestStream.Close();
              HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
              if (response.StatusCode == HttpStatusCode.OK)
              {
                using (Stream responseStream = response.GetResponseStream())
                {
                  using (StreamReader streamReader = new StreamReader(responseStream))
                  {
                    long contentLength = response.ContentLength;
                    string end = streamReader.ReadToEnd();
                    if (end.IndexOf("true") > 0)
                    {
                      response.Close();
                      requestStream.Dispose();
                      return true;
                    }
                    Dictionary<string, object> res = new JavaScriptSerializer().Deserialize(end, typeof (Dictionary<string, object>)) as Dictionary<string, object>;
                    if (res.ContainsKey("remaining_chunks"))
                    {
                      Dictionary<string, object> dictionary2 = res["remaining_chunks"] as Dictionary<string, object>;
                      intList = new List<int>();
                      foreach (string key in dictionary2.Keys)
                      {
                        if (!(bool) dictionary2[key])
                          intList.Add(int.Parse(key));
                      }
                      if (intList.Count == 0)
                        return false;
                    }
                    if (res.ContainsKey("status") && (string) res["status"] == "ERROR")
                    {
                      Util.ShowErrorMessage(res, PS3SaveEditor.Resources.Resources.errUnknown);
                      this.RaiseDownloadFinishEvent(false, "Abort");
                      return false;
                    }
                  }
                }
                response.Close();
                requestStream.Dispose();
                if (flag)
                {
                  num4 = (long) (intList.Count * count);
                  flag = false;
                }
              }
              else
              {
                ++num1;
                if (num1 > 3)
                  return false;
              }
            }
            catch (Exception ex)
            {
              ++num1;
              if (num1 > 3)
              {
                SaveUploadDownloder.ErrorMessage(this.ParentForm, PS3SaveEditor.Resources.Resources.errUnknown);
                this.RaiseDownloadFinishEvent(false, "Abort");
                return false;
              }
            }
          }
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        SaveUploadDownloder.ErrorMessage(this.ParentForm, ex.Message, "UnauthorizedAccess");
        this.RaiseDownloadFinishEvent(false, "Abort");
      }
      return false;
    }

    public void HttpUploadFile(
      string url,
      string file,
      string paramName,
      string contentType,
      NameValueCollection nvc)
    {
      string error = "";
      string str1 = "---------------------------" + DateTime.Now.Ticks.ToString("x");
      byte[] bytes1 = Encoding.ASCII.GetBytes("\r\n--" + str1 + "\r\n");
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
      httpWebRequest.AllowWriteStreamBuffering = true;
      httpWebRequest.PreAuthenticate = true;
      httpWebRequest.ContentType = "multipart/form-data; boundary=" + str1;
      httpWebRequest.Method = "POST";
      httpWebRequest.UserAgent = Util.GetUserAgent();
      httpWebRequest.ProtocolVersion = new Version(1, 1);
      httpWebRequest.KeepAlive = true;
      ServicePointManager.Expect100Continue = false;
      httpWebRequest.Credentials = (ICredentials) Util.GetNetworkCredential();
      httpWebRequest.Timeout = 600000;
      httpWebRequest.ReadWriteTimeout = 600000;
      httpWebRequest.SendChunked = true;
      string str2 = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
      httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
      httpWebRequest.Headers.Add("Authorization", str2);
      string format1 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
      long num1 = 0;
      foreach (string key in nvc.Keys)
      {
        num1 += (long) bytes1.Length;
        byte[] bytes2 = Encoding.UTF8.GetBytes(string.Format(format1, (object) key, (object) nvc[key]));
        num1 += (long) bytes2.Length;
      }
      long num2 = num1 + (long) bytes1.Length + (long) Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", (object) paramName, (object) file, (object) contentType)).Length;
      bool bSuccess = true;
      if (file != null)
      {
        long num3 = num2 + new FileInfo(file).Length + (long) Encoding.ASCII.GetBytes("\r\n--" + str1 + "--\r\n").Length;
        httpWebRequest.ContentLength = num3;
        this.start = DateTime.Now.Ticks;
        try
        {
          Stream requestStream = httpWebRequest.GetRequestStream();
          string format2 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
          foreach (string key in nvc.Keys)
          {
            requestStream.Write(bytes1, 0, bytes1.Length);
            byte[] bytes3 = Encoding.UTF8.GetBytes(string.Format(format2, (object) key, (object) nvc[key]));
            requestStream.Write(bytes3, 0, bytes3.Length);
          }
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes4 = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", (object) paramName, (object) Path.GetFileName(file), (object) contentType));
          requestStream.Write(bytes4, 0, bytes4.Length);
          FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
          byte[] buffer = new byte[4096];
          long num4 = 0;
          long length = fileStream.Length;
          int count;
          while ((uint) (count = fileStream.Read(buffer, 0, buffer.Length)) > 0U)
          {
            num4 += (long) count;
            this.SetProgress((int) (num4 * 100L / length), new int?(80));
            requestStream.Write(buffer, 0, count);
          }
          fileStream.Close();
          byte[] bytes5 = Encoding.ASCII.GetBytes("\r\n--" + str1 + "--\r\n");
          requestStream.Write(bytes5, 0, bytes5.Length);
          requestStream.Close();
          System.IO.File.Delete(file);
        }
        catch (Exception ex)
        {
          this.RaiseDownloadFinishEvent(false, PS3SaveEditor.Resources.Resources.errConnection);
          return;
        }
      }
      else
      {
        Stream requestStream = httpWebRequest.GetRequestStream();
        string format3 = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        foreach (string key in nvc.Keys)
        {
          requestStream.Write(bytes1, 0, bytes1.Length);
          byte[] bytes6 = Encoding.UTF8.GetBytes(string.Format(format3, (object) key, (object) nvc[key]));
          requestStream.Write(bytes6, 0, bytes6.Length);
        }
        requestStream.Write(bytes1, 0, bytes1.Length);
        requestStream.Close();
      }
      this.RaiseUploadFinishEvent();
      this.SetProgress(0, new int?(80));
      WebResponse webResponse = (WebResponse) null;
      try
      {
        webResponse = httpWebRequest.GetResponse();
        long contentLength = webResponse.ContentLength;
        this.RaiseDownloadStartEvent();
        using (Stream responseStream = webResponse.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
          {
            string end = streamReader.ReadToEnd();
            if (this.Action == "list" && end.IndexOf("[") == 0)
            {
              this.ListResult = end;
            }
            else
            {
              try
              {
                if (new JavaScriptSerializer().Deserialize(end, typeof (Dictionary<string, object>)) is Dictionary<string, object> res8 && res8.ContainsKey("status") && (string) res8["status"] == "OK")
                {
                  string zipFile = (string) res8["zip"];
                  this.SetProgress(0, new int?(80));
                  bSuccess = this.DownloadZip(zipFile, 0L);
                }
                else
                {
                  string str3 = "";
                  if (res8 != null && res8.ContainsKey("code"))
                    str3 = res8["code"].ToString();
                  Util.ShowErrorMessage(res8, PS3SaveEditor.Resources.Resources.errUnknown);
                  this.RaiseDownloadFinishEvent(false, "Abort");
                  error = (string) null;
                  bSuccess = false;
                }
              }
              catch (Exception ex)
              {
                error = PS3SaveEditor.Resources.Resources.errUnknown;
                bSuccess = false;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        webResponse?.Close();
        bSuccess = false;
      }
      finally
      {
      }
      this.RaiseDownloadFinishEvent(bSuccess, error);
    }

    private bool DownloadZip(string zipFile, long start, int retry = 0)
    {
      long num1 = 0;
      long start1 = start;
      try
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(Util.GetBaseUrl() + zipFile) as HttpWebRequest;
        httpWebRequest.Method = "GET";
        httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        httpWebRequest.PreAuthenticate = true;
        httpWebRequest.UserAgent = Util.GetUserAgent();
        httpWebRequest.Credentials = (ICredentials) Util.GetNetworkCredential();
        httpWebRequest.Timeout = 300000;
        httpWebRequest.ReadWriteTimeout = 300000;
        string str1 = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
        httpWebRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
        httpWebRequest.Headers.Add("Authorization", str1);
        httpWebRequest.AddRange(start);
        WebResponse response = httpWebRequest.GetResponse();
        num1 = response.ContentLength;
        using (Stream responseStream = response.GetResponseStream())
        {
          byte[] buffer = new byte[4096];
          string tempFileName = Path.GetTempFileName();
          using (FileStream fileStream = System.IO.File.OpenWrite(tempFileName))
          {
            while (true)
            {
              int count = responseStream.Read(buffer, 0, buffer.Length);
              if (count != 0)
              {
                fileStream.Write(buffer, 0, count);
                start1 += (long) count;
                this.SetProgress((int) (start1 * 100L / num1), new int?());
              }
              else
                break;
            }
          }
          this.SetProgress(0, new int?(90));
          string str2 = this.EnsureSpace();
          try
          {
            this.OrderedEntries = this.ExtractZip(tempFileName);
            if (this.Action == "resign" && !this.OrderedEntries.Contains(Path.GetFileName(this.Game.LocalSaveFolder)))
            {
              if (!Directory.Exists(Path.GetPathRoot(this.OutputFolder)))
              {
                int num2 = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.errResignNoUSB);
                return false;
              }
              Directory.CreateDirectory(this.OutputFolder);
              System.IO.File.Copy(this.Game.LocalSaveFolder, Path.Combine(this.OutputFolder, Path.GetFileName(this.Game.LocalSaveFolder)), true);
            }
          }
          catch (Exception ex)
          {
            if (str2 != null)
            {
              if (this.Action == "resign")
                System.IO.File.Copy(str2, this.OutputFolder);
              else
                System.IO.File.Copy(str2, this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4));
            }
          }
          if (str2 != null)
            System.IO.File.Delete(str2);
          return true;
        }
      }
      catch (Exception ex)
      {
        return (num1 <= 0L || start1 != num1) && retry <= 3 && this.DownloadZip(zipFile, start1, retry++);
      }
    }

    private string EnsureSpace()
    {
      if (this.Action == "decrypt" || this.Action == "list")
        return (string) null;
      DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(this.Game.LocalSaveFolder));
      string str = this.Game.LocalSaveFolder.Substring(0, this.Game.LocalSaveFolder.Length - 4);
      if (driveInfo.AvailableFreeSpace > new FileInfo(str).Length)
        return (string) null;
      string destFileName = Path.GetTempFileName();
      if (Util.GetRegistryValue("BackupSaves") == "false")
        System.IO.File.Copy(str, destFileName, true);
      else
        destFileName = (string) null;
      System.IO.File.Delete(str);
      return destFileName;
    }

    private List<string> ExtractZip(string tempFile)
    {
      List<string> stringList = new List<string>();
      ZipFile zipFile = new ZipFile(tempFile);
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) zipFile.Entries)
        stringList.Add(entry.FileName);
      zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.zipFile_ExtractProgress);
      zipFile.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
      zipFile.ExtractAll(this.OutputFolder);
      stringList.Reverse();
      return stringList;
    }

    private void zipFile_ExtractProgress(object sender, ExtractProgressEventArgs e)
    {
      if (e.EventType != ZipProgressEventType.Extracting_EntryBytesWritten)
        return;
      this.SetProgress((int) (e.BytesTransferred * 100L / e.TotalBytesToTransfer), new int?());
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
      this.lblCurrentProgress = new Label();
      this.lblTotalProgress = new Label();
      this.pbOverallProgress = new PS4ProgressBar();
      this.pbProgress = new PS4ProgressBar();
      this.SuspendLayout();
      this.lblStatus.ForeColor = Color.White;
      this.lblStatus.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(10));
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = Util.ScaleSize(new Size(400, 13));
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "Text";
      this.lblCurrentProgress.AutoSize = true;
      this.lblCurrentProgress.ForeColor = Color.White;
      this.lblCurrentProgress.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(30));
      this.lblCurrentProgress.Name = "lblCurrentProgress";
      this.lblCurrentProgress.Size = Util.ScaleSize(new Size(85, 13));
      this.lblCurrentProgress.TabIndex = 3;
      this.lblCurrentProgress.Text = "Current Progress";
      this.lblTotalProgress.AutoSize = true;
      this.lblTotalProgress.ForeColor = Color.White;
      this.lblTotalProgress.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(91));
      this.lblTotalProgress.Name = "lblTotalProgress";
      this.lblTotalProgress.Size = Util.ScaleSize(new Size(75, 13));
      this.lblTotalProgress.TabIndex = 4;
      this.lblTotalProgress.Text = "Total Progress";
      this.pbOverallProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pbOverallProgress.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(113));
      this.pbOverallProgress.Name = "pbOverallProgress";
      this.pbOverallProgress.Size = Util.ScaleSize(new Size(424, 23));
      this.pbOverallProgress.TabIndex = 2;
      this.pbProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pbProgress.Location = new Point(Util.ScaleSize(11), Util.ScaleSize(52));
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = Util.ScaleSize(new Size(424, 23));
      this.pbProgress.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(Util.ScaleSize(6f), Util.ScaleSize(13f));
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.FromArgb(102, 102, 102);
      this.Controls.Add((Control) this.lblTotalProgress);
      this.Controls.Add((Control) this.lblCurrentProgress);
      this.Controls.Add((Control) this.pbOverallProgress);
      this.Controls.Add((Control) this.pbProgress);
      this.Controls.Add((Control) this.lblStatus);
      this.Name = nameof (SaveUploadDownloder);
      this.Size = Util.ScaleSize(new Size(446, 151));
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void UpdateProgressDelegate(int value, int? overall);

    private delegate void UpdateStatusDelegate(string status);

    public delegate void DownloadStartEventHandler(object sender, EventArgs e);

    public delegate void UploadStartEventHandler(object sender, EventArgs e);

    public delegate void DownloadFinishEventHandler(object sender, DownloadFinishEventArgs e);

    public delegate void UploadFinishEventHandler(object sender, EventArgs e);
  }
}
