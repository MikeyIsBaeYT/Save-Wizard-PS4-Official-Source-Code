// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Util
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  internal class Util
  {
    public static string PRODUCT_NAME = Util.IsHyperkin() ? "Save Wizard for PS4" : "Save Wizard for PS4 MAX";
    public static string[] AUTH_SERVERS = new string[5]
    {
      "ps4as1.savewizard.net:8082",
      "ps4as2.savewizard.net:8082",
      "ps4as3.savewizard.net:8082",
      "ps4as4.savewizard.net:8082",
      "ps4as5.savewizard.net:8082"
    };
    public static string[] SERVERS = new string[9]
    {
      "ps4gs1.savewizard.net:8082",
      "ps4gs2.savewizard.net:8082",
      "ps4gs3.savewizard.net:8082",
      "ps4gs4.savewizard.net:8082",
      "ps4gs5.savewizard.net:8082",
      "ps4gs6.savewizard.net:8082",
      "ps4gs7.savewizard.net:8082",
      "ps4gs8.savewizard.net:8082",
      "ps4gs9.savewizard.net:8082"
    };
    public static int CURRENT_SERVER = new Random().Next(0, Util.SERVERS.Length);
    public static int CURRENT_AUTH_SERVER = new Random().Next(0, Util.AUTH_SERVERS.Length);
    public static int CURRENT_USER_PWD = new Random().Next(0, Program.HTACCESS_USER.Length);
    public static string AvailableVersion = "0.0";
    private static string VERSION_FILE_URL = "https://www.savewizard.net/downloads/swps4mbeta.txt";
    public static string forceServer = string.Empty;
    public static string forceAuthServer = string.Empty;
    public static int pid = 0;
    private static string SessionToken;
    private static int MinFileSize = 0;
    private static int MaxFileSize = int.MaxValue;
    private static int _scaleIndex = -1;

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("__Internal", EntryPoint = "mono_get_runtime_build_info")]
    public static extern string GetMonoVersion();

    public static Util.Platform CurrentPlatform { get; private set; } = Util.GetRunningPlatform();

    public static bool IsHyperkinMode { get; set; } = false;

    public static bool IsHyperkin() => Util.IsHyperkinMode;

    private static Util.Platform GetRunningPlatform()
    {
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Win32S:
        case PlatformID.Win32Windows:
        case PlatformID.Win32NT:
        case PlatformID.WinCE:
          return Util.Platform.Windows;
        case PlatformID.Unix:
          return Directory.Exists("/Applications") & Directory.Exists("/System") & Directory.Exists("/Users") & Directory.Exists("/Volumes") ? Util.Platform.MacOS : Util.Platform.Linux;
        case PlatformID.MacOSX:
          return Util.Platform.MacOS;
        default:
          return Util.Platform.Other;
      }
    }

    internal static bool IsUnixOrMacOSX()
    {
      int platform = (int) Environment.OSVersion.Platform;
      int num;
      switch (platform)
      {
        case 4:
        case 6:
          num = 1;
          break;
        default:
          num = platform == 128 ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public static bool IsOldMono()
    {
      if (Util.CurrentPlatform == Util.Platform.Windows)
        return false;
      string monoVersion = Util.GetMonoVersion();
      Version result;
      Version.TryParse(monoVersion.Substring(0, monoVersion.IndexOf("(")), out result);
      return result < Version.Parse("5.12.0.226");
    }

    public static bool IsBigScreen() => Screen.PrimaryScreen.Bounds.Width > 2000;

    public static string GetFontFamily()
    {
      if (Util.CurrentPlatform == Util.Platform.Linux)
        return "Noto Sans CJK SC";
      return Util.CurrentPlatform == Util.Platform.MacOS ? "Arial Unicode MS" : "Microsoft Sans Serif";
    }

    public static Font GetFontForPlatform(Font defaultFont)
    {
      float emSize = Util.ScaleSize(defaultFont.Size);
      if (Util.CurrentPlatform == Util.Platform.Linux)
        return new Font("Noto Sans CJK SC", emSize, defaultFont.Style, defaultFont.Unit, defaultFont.GdiCharSet);
      return Util.CurrentPlatform == Util.Platform.MacOS ? new Font("Arial Unicode MS", emSize, defaultFont.Style, defaultFont.Unit, defaultFont.GdiCharSet) : new Font("Microsoft Sans Serif", emSize, defaultFont.Style, defaultFont.Unit, defaultFont.GdiCharSet);
    }

    public static string GetBackupLocation()
    {
      string registryValue = Util.GetRegistryValue("Location");
      if (!string.IsNullOrEmpty(registryValue))
      {
        try
        {
          Directory.CreateDirectory(registryValue);
          Path.GetFullPath(registryValue);
          return Path.IsPathRooted(registryValue) ? registryValue : throw new Exception();
        }
        catch
        {
          Util.DeleteRegistryValue("Location");
          if (Util.GetRegistryValue("BackupSaves") != "false")
          {
            int num = (int) Util.ShowMessage(PS3SaveEditor.Resources.Resources.msgBadBackupPath, PS3SaveEditor.Resources.Resources.msgInfo);
          }
        }
      }
      string str1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar.ToString();
      string str2 = str1 + "PS4Saves_Backup";
      string path = str1 + "Save Wizard for PS4" + Path.DirectorySeparatorChar.ToString() + "PS4 Saves Backup";
      Directory.CreateDirectory(path);
      return path;
    }

    public static string GetGamelistPath()
    {
      string path = Path.Combine(Path.GetTempPath(), "SWP");
      Directory.CreateDirectory(path);
      return path + Path.DirectorySeparatorChar.ToString() + "gamelist";
    }

    internal static string GetOSVersion()
    {
      string str = string.Empty;
      try
      {
        str = !Util.IsUnixOrMacOSX() ? Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", (object) "").ToString() : CurrentOS.Name;
      }
      catch
      {
      }
      return string.Format("{0} ({1})", (object) str, (object) Environment.OSVersion.ToString());
    }

    public static string GetProductID()
    {
      string empty = string.Empty;
      try
      {
      }
      catch
      {
      }
      return string.Format("{0} ({1})", (object) empty, (object) Util.pid.ToString());
    }

    internal static string GetFramework()
    {
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      try
      {
        if (Type.GetType("Mono.Runtime") != (Type) null && Util.IsUnixOrMacOSX())
          return string.Format("Mono {0}", (object) Util.GetMonoVersion());
        if (Type.GetType("Core.Runtime") != (Type) null)
          return string.Format(".Net Core {0}", (object) entryAssembly.ImageRuntimeVersion);
      }
      catch
      {
      }
      return string.Format(".Net Framework {0} ({1})", (object) (Util.Get45or451FromRegistry() ?? "< 4.5"), (object) entryAssembly.ImageRuntimeVersion);
    }

    private static string Get45or451FromRegistry()
    {
      using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
        return Util.CheckFor45DotVersion(Convert.ToInt32(registryKey.GetValue("Release")));
    }

    private static string CheckFor45DotVersion(int releaseKey)
    {
      if (releaseKey >= 461808)
        return "4.7.2 or later";
      if (releaseKey >= 461308)
        return "4.7.1 or later";
      if (releaseKey >= 460798)
        return "4.7 or later";
      if (releaseKey >= 394802)
        return "4.6.2 or later";
      if (releaseKey >= 394254)
        return "4.6.1 or later";
      if (releaseKey >= 393295)
        return "4.6 or later";
      if (releaseKey >= 393273)
        return "4.6 RC or later";
      if (releaseKey >= 379893)
        return "4.5.2 or later";
      if (releaseKey >= 378675)
        return "4.5.1 or later";
      return releaseKey >= 378389 ? "4.5 or later" : (string) null;
    }

    internal static string GetUserId() => Util.GetRegistryValue("User");

    internal static void ChangeServer() => Util.CURRENT_SERVER = new Random().Next(0, Util.SERVERS.Length);

    internal static void ChangeAuthServer() => Util.CURRENT_AUTH_SERVER = new Random().Next(0, Util.AUTH_SERVERS.Length);

    internal static string GetRegistryValue(string key)
    {
      RegistryKey currentUser = Registry.CurrentUser;
      RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase());
      if (registryKey == null)
        return (string) null;
      try
      {
        string str = registryKey.GetValue(key) as string;
        registryKey.Close();
        currentUser.Close();
        return str;
      }
      catch (Exception ex)
      {
      }
      return (string) null;
    }

    internal static void DeleteRegistryValue(string key)
    {
      RegistryKey currentUser = Registry.CurrentUser;
      RegistryKey registryKey = currentUser.OpenSubKey(Util.GetRegistryBase(), true);
      if (registryKey == null)
        return;
      try
      {
        registryKey.DeleteValue(key);
      }
      catch (Exception ex)
      {
      }
      finally
      {
        registryKey.Close();
        currentUser.Close();
      }
    }

    internal static void SetRegistryValue(string key, string value)
    {
      RegistryKey currentUser = Registry.CurrentUser;
      RegistryKey subKey = currentUser.CreateSubKey(Util.GetRegistryBase());
      if (subKey == null)
        return;
      subKey.SetValue(key, (object) value);
      subKey.Close();
      currentUser.Close();
    }

    internal static bool IsMatch(string a, string pattern) => Regex.IsMatch(a, "^" + pattern + "$");

    internal static string GetAdapterName(bool blackListed = false) => blackListed ? (string) null : Util.GetRegistryValue("Adapter");

    internal static string GetUID(bool blackListed = false, bool register = false)
    {
      if (Util.IsUnixOrMacOSX())
        return Util.GetUIDInMonoRunning();
      string uidFromWmi = Util.GetUIDFromWMI(Environment.ExpandEnvironmentVariables("%SYSTEMDRIVE%"));
      return uidFromWmi.Substring(uidFromWmi.IndexOf('{') + 1, uidFromWmi.IndexOf('}') - uidFromWmi.IndexOf('{') - 1);
    }

    internal static string GetDataPath(string strBase)
    {
      if (string.IsNullOrWhiteSpace(strBase))
        return strBase;
      string str;
      if (Util.IsUnixOrMacOSX())
        str = strBase + "/PS4/SAVEDATA";
      else if (strBase.Substring(strBase.Length - 1, 1) != "\\" && !strBase.ToLowerInvariant().Contains("ps4"))
        str = strBase + "\\PS4\\SAVEDATA";
      else if (strBase.Substring(strBase.Length - 1, 1) != "\\" && strBase.ToLowerInvariant().Contains("ps4") && !strBase.ToLowerInvariant().Contains("savedata"))
      {
        str = strBase + "\\PS4\\SAVEDATA";
      }
      else
      {
        if (strBase.Substring(strBase.Length - 1, 1) != "\\" && strBase.ToLowerInvariant().Contains("ps4") && strBase.ToLowerInvariant().Contains("savedata"))
          return strBase;
        str = strBase + "PS4\\SAVEDATA";
      }
      return str;
    }

    private static string GetUIDInMonoRunning()
    {
      try
      {
        if (Util.CurrentPlatform != Util.Platform.MacOS)
          return "ade15a18-a80b-469c-ab20-eb2df3f88156";
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
          FileName = "sh",
          Arguments = "-c \"ioreg -rd1 -c IOPlatformExpertDevice | awk '/IOPlatformUUID/'\"",
          UseShellExecute = false,
          CreateNoWindow = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          RedirectStandardInput = true,
          UserName = Environment.UserName
        };
        StringBuilder stringBuilder = new StringBuilder();
        using (Process process = Process.Start(startInfo))
        {
          process.WaitForExit();
          stringBuilder.Append(process.StandardOutput.ReadToEnd());
        }
        return stringBuilder.ToString(26, 36);
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(Util.PRODUCT_NAME + " can not start. It didn't get unique device id");
      }
      return (string) null;
    }

    internal static string GetSerial() => Util.GetRegistryValue("Serial");

    internal static string GetHtaccessUser() => Program.HTACCESS_USER[Util.CURRENT_USER_PWD];

    internal static string GetHtaccessPwd() => Program.HTACCESS_PWD[Util.CURRENT_USER_PWD];

    internal static NetworkCredential GetNetworkCredential() => new NetworkCredential(Util.GetHtaccessUser(), Util.GetHtaccessPwd());

    internal static string GetBaseUrl() => string.IsNullOrEmpty(Util.forceServer) ? "http://" + Util.SERVERS[Util.CURRENT_SERVER] : "http://" + Util.forceServer;

    internal static string GetAuthBaseUrl() => string.IsNullOrEmpty(Util.forceAuthServer) ? "http://" + Util.AUTH_SERVERS[Util.CURRENT_AUTH_SERVER] : "http://" + Util.forceAuthServer;

    internal static void SetAuthToken(string Token) => Util.SessionToken = Token;

    internal static string GetAuthToken() => Util.SessionToken;

    internal static string GetUserAgent() => "Save Wizard for PS4 " + "1.0.7422.29556" + " (" + Util.GetRunningPlatform().ToString() + ")";

    private static string GetUIDFromWMI(string sysDrive)
    {
      try
      {
        ManagementObjectCollection objectCollection = new ManagementObjectSearcher("SELECT * FROM   Win32_Volume WHERE  DriveLetter = '" + sysDrive + "'").Get();
        string str = (string) null;
        foreach (ManagementObject managementObject in objectCollection)
        {
          if (str == null)
          {
            object propertyValue = managementObject.GetPropertyValue("DeviceID");
            if (propertyValue != null)
              str = propertyValue.ToString();
            str.Substring(str.IndexOf('{') + 1).TrimEnd('\\').TrimEnd('}');
          }
          else
            break;
        }
        return str;
      }
      catch (Exception ex)
      {
        int num = (int) Util.ShowMessage(Util.PRODUCT_NAME + " can not start. Please make sure Windows Management Instrumentation service is running.");
      }
      return (string) null;
    }

    internal static void ClearTemp()
    {
      try
      {
        foreach (string file in Directory.GetFiles(Util.GetTempFolder()))
        {
          if (file.IndexOf("Log.txt") <= 0)
            System.IO.File.Delete(file);
        }
      }
      catch (Exception ex)
      {
      }
    }

    internal static string GetTempFolder()
    {
      string path = Path.Combine(Path.GetTempPath(), "SWPS4MAX");
      Directory.CreateDirectory(path);
      return path + "/";
    }

    internal static string GetRegistryBase() => "SOFTWARE\\DataPower\\Save Wizard for PS4";

    internal static string GetDevLogFile() => Path.Combine(Util.GetTempFolder(), "DevLog.txt");

    internal static string GetRegion(Dictionary<int, string> regionMap, int p, string exregions)
    {
      string str = "";
      foreach (int key in regionMap.Keys)
      {
        if ((p & key) > 0 && exregions.IndexOf(regionMap[key]) < 0)
          str = str + "[" + regionMap[key] + "]";
      }
      return str;
    }

    internal static byte[] GetPSNId(string saveFolder) => Encoding.UTF8.GetBytes(MainForm.GetParamInfo(Path.Combine(saveFolder, "PARAM.SFO"), "ACCOUNT_ID"));

    internal static bool GetCache(string hash)
    {
      try
      {
        WebClientEx webClientEx = new WebClientEx();
        webClientEx.Headers[HttpRequestHeader.UserAgent] = Util.GetUserAgent();
        webClientEx.Credentials = (ICredentials) Util.GetNetworkCredential();
        return Encoding.ASCII.GetString(webClientEx.UploadData(Util.GetBaseUrl() + "/request_cache?token=" + Util.GetAuthToken(), Encoding.ASCII.GetBytes("{\"pfs\":\"" + hash + "\"}"))).IndexOf("true") > 0;
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    internal static string GetHash(byte[] buf) => BitConverter.ToString(MD5.Create().ComputeHash(buf)).Replace("-", "").ToLower();

    internal static string GetHash(string file)
    {
      using (FileStream fileStream = System.IO.File.OpenRead(file))
        return BitConverter.ToString(MD5.Create().ComputeHash((Stream) fileStream)).Replace("-", "").ToLower();
    }

    internal static string GetCacheFolder(game game, string curCacheFolder)
    {
      string path1 = Path.Combine(Path.Combine(Util.GetBackupLocation(), "cache"), game.id);
      return string.IsNullOrEmpty(curCacheFolder) ? path1 : Path.Combine(path1, curCacheFolder);
    }

    internal static void ShowErrorMessage(Dictionary<string, object> res, string msgfallback)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      if (res != null)
      {
        if (res.ContainsKey("code"))
          str1 = Convert.ToString(res["code"]);
        if (res.ContainsKey("id"))
          str2 = res["id"] as string;
        if (res.ContainsKey("pid"))
        {
          str4 = res["pid"] as string;
          Util.pid = Convert.ToInt32(str4);
        }
        if (res.ContainsKey("cid"))
          str3 = res["cid"] as string;
      }
      string str5 = PS3SaveEditor.Resources.Resources.ResourceManager.GetString("err" + str1);
      if (res.ContainsKey("msg"))
      {
        Dictionary<string, object> re = res["msg"] as Dictionary<string, object>;
        string name = Thread.CurrentThread.CurrentUICulture.Name;
        string letterIsoLanguageName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        if (re != null && re.ContainsKey(name))
          str5 = re[name] as string;
        else if (re != null && re.ContainsKey(letterIsoLanguageName))
          str5 = re[letterIsoLanguageName] as string;
      }
      if (string.IsNullOrEmpty(str5))
        str5 = msgfallback;
      if (res.ContainsKey("onhold") && str5.IndexOf("onhold", StringComparison.InvariantCultureIgnoreCase) >= 0)
      {
        int result = 0;
        if (int.TryParse(res["onhold"] as string, out result))
        {
          int num = result / 3600000;
          str5 = str5.Replace("{onhold}", num.ToString());
        }
      }
      string linkUrl = (string) null;
      if (str5 == null)
        return;
      if (str5.IndexOf("%support%", StringComparison.InvariantCultureIgnoreCase) > 0)
        linkUrl = !Util.IsHyperkin() ? "https://www.savewizard.net/support-sw-issue.php" : "http://www.thesavewizard.com/support.php";
      if (str2 != null && !string.IsNullOrEmpty(str2.ToString()))
        linkUrl = !Util.IsHyperkin() ? "https://www.savewizard.net/support-sw-issue.php?error=" + str2 : "http://www.thesavewizard.com/support.php";
      string str6 = str5.Replace("%prod_ln%", Util.PRODUCT_NAME).Replace("%support%", PS3SaveEditor.Resources.Resources.support);
      int num1 = (int) new LinkMessageBox((str2 == null ? str6 + string.Format(" ({0})", (object) str1) : str6 + string.Format(" ({0}:{1})", (object) str1, (object) str2)) + string.Format("{0}", (object) Environment.NewLine) + string.Format("PID is ({0})", (object) str4), linkUrl).ShowDialog((IWin32Window) Form.ActiveForm);
    }

    internal static void SetMinFileSize(int v) => Util.MinFileSize = v;

    internal static void SetMaxFileSize(int v) => Util.MaxFileSize = v;

    internal static int GetMinFileSize() => Util.MinFileSize;

    internal static int GetMaxFileSize() => Util.MaxFileSize;

    internal static bool HasWritePermission(string folderPath)
    {
      string path = Path.Combine(Path.GetDirectoryName(folderPath), "file.test");
      try
      {
        System.IO.File.WriteAllText(path, "test text");
        System.IO.File.Delete(path);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    internal static bool IsNeedToShowUpdateScreen
    {
      get
      {
        if (Util.AvailableVersion == "0.0")
          Util.AvailableVersion = Util.readVersionFromSite();
        return new Version(Util.AvailableVersion) > new Version(AboutBox1.AssemblyVersion);
      }
    }

    private static string readVersionFromSite()
    {
      string str = "0.0";
      try
      {
        str = new StreamReader(new WebClient().OpenRead(Util.VERSION_FILE_URL)).ReadToEnd();
      }
      catch
      {
      }
      return str;
    }

    internal static string GetCheatsLocationFromRegistry()
    {
      string empty = string.Empty;
      try
      {
        string registryValue = Util.GetRegistryValue("CheatsLocalPath");
        if (Directory.Exists(registryValue) && Util.IsPathToCheats(registryValue) || registryValue == null)
          return registryValue;
        Util.DeleteRegistryValue("CheatsLocalPath");
      }
      catch
      {
      }
      return (string) null;
    }

    internal static bool IsPathToCheats(string pathToCheats)
    {
      if (string.IsNullOrEmpty(pathToCheats))
        return false;
      if (Util.CurrentPlatform == Util.Platform.MacOS && !Directory.Exists(pathToCheats))
        pathToCheats = string.Format("/Volumes{0}", (object) pathToCheats);
      else if (Util.CurrentPlatform == Util.Platform.Linux && !Directory.Exists(pathToCheats))
        pathToCheats = string.Format("/media/{0}{1}", (object) Environment.UserName, (object) pathToCheats);
      if (!Directory.Exists(pathToCheats))
        return false;
      DirectoryInfo directoryInfo = new DirectoryInfo(pathToCheats);
      if (!directoryInfo.Exists)
        return false;
      DirectoryInfo[] directories = directoryInfo.GetDirectories();
      if (directories != null && (uint) directories.Length > 0U)
      {
        foreach (DirectoryInfo directory1 in directoryInfo.GetDirectories())
        {
          if (directory1.FullName.ToLowerInvariant().Contains("ps4"))
          {
            foreach (FileSystemInfo directory2 in directory1.GetDirectories())
            {
              if (directory2.FullName.ToLowerInvariant().Contains("savedata"))
                return true;
            }
          }
        }
      }
      return false;
    }

    internal static string GetShortPath(string folderPath)
    {
      int length = folderPath.ToLowerInvariant().LastIndexOf("ps4", StringComparison.InvariantCultureIgnoreCase);
      if (length < 0)
        return folderPath;
      string pathToCheats = folderPath.Substring(0, length);
      return Util.IsPathToCheats(pathToCheats) ? pathToCheats : folderPath;
    }

    internal static void SaveCheatsPathToRegistry(string folderPath) => Util.SetRegistryValue("CheatsLocalPath", folderPath);

    public static DialogResult ShowMessage(string text) => Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(text) : MessageBox.Show(text);

    public static DialogResult ShowMessage(string text, string caption) => Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) : MessageBox.Show(text, caption);

    public static DialogResult ShowMessage(Form owner, string text) => Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(owner, text) : MessageBox.Show((IWin32Window) owner, text);

    public static DialogResult ShowMessage(
      string text,
      string caption,
      MessageBoxButtons buttons)
    {
      return Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1) : MessageBox.Show(text, caption, buttons);
    }

    public static DialogResult ShowMessage(Form owner, string text, string caption) => Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(owner, text, caption) : MessageBox.Show((IWin32Window) owner, text, caption);

    public static DialogResult ShowMessage(
      string text,
      string caption,
      MessageBoxButtons buttons,
      MessageBoxIcon icon)
    {
      return Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(text, caption, buttons, icon, MessageBoxDefaultButton.Button1) : MessageBox.Show(text, caption, buttons, icon);
    }

    public static DialogResult ShowMessage(
      string text,
      string caption,
      MessageBoxButtons buttons,
      MessageBoxIcon icon,
      MessageBoxDefaultButton defaultButton)
    {
      return Util.IsUnixOrMacOSX() || Util.ScaleIndex > 1 ? CustomMsgBox.Show(text, caption, buttons, icon, defaultButton) : MessageBox.Show(text, caption, buttons, icon, defaultButton);
    }

    internal static void ProcedArguments(string[] args)
    {
      foreach (string str in args)
      {
        if (str.ToLowerInvariant().Contains("tryserver="))
        {
          if (str.Contains("/"))
            Util.SERVERS = str.Substring(10).Split('/');
          else
            Util.forceServer = str.Substring(10);
        }
        if (str.ToLowerInvariant().Contains("tryauthserver="))
        {
          if (str.Contains("/"))
            Util.AUTH_SERVERS = str.Substring(14).Split('/');
          else
            Util.forceAuthServer = str.Substring(14);
        }
      }
    }

    public static int ScaleIndex
    {
      get
      {
        if (Util._scaleIndex >= 0)
          return Util._scaleIndex;
        try
        {
          int.TryParse(Util.GetRegistryValue("SelectedScaleIndex"), out Util._scaleIndex);
          if (Util._scaleIndex == 0)
            Util._scaleIndex = Util.IsBigScreen() ? 5 : 1;
        }
        catch
        {
        }
        return Util._scaleIndex;
      }
      set
      {
        Util._scaleIndex = value;
        Util.SetRegistryValue("SelectedScaleIndex", value.ToString());
      }
    }

    public static int ScaleSize(int size)
    {
      switch (Util.ScaleIndex)
      {
        case 0:
          return size * 75 / 100;
        case 1:
          return size;
        case 2:
          return size * 125 / 100;
        case 3:
          return size * 150 / 100;
        case 4:
          return size * 175 / 100;
        case 5:
          return size * 200 / 100;
        default:
          return size;
      }
    }

    public static float ScaleSize(float size) => (float) Util.ScaleSize((int) size);

    public static Size ScaleSize(Size size) => new Size(Util.ScaleSize(size.Width), Util.ScaleSize(size.Height));

    public enum Platform
    {
      Windows,
      Linux,
      MacOS,
      Other,
    }
  }
}
