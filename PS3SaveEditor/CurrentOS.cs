// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CurrentOS
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace PS3SaveEditor
{
  public static class CurrentOS
  {
    public static bool IsWindows { get; private set; }

    public static bool IsUnix { get; private set; }

    public static bool IsMac { get; private set; }

    public static bool IsLinux { get; private set; }

    public static bool IsUnknown { get; private set; }

    public static bool Is32bit { get; private set; }

    public static bool Is64bit { get; private set; }

    public static bool Is64BitProcess => IntPtr.Size == 8;

    public static bool Is32BitProcess => IntPtr.Size == 4;

    public static string Name { get; private set; }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWow64Process([In] IntPtr hProcess, out bool wow64Process);

    private static bool Is64bitWindows
    {
      get
      {
        if ((Environment.OSVersion.Version.Major != 5 || Environment.OSVersion.Version.Minor < 1) && Environment.OSVersion.Version.Major < 6)
          return false;
        using (Process currentProcess = Process.GetCurrentProcess())
        {
          bool wow64Process;
          return CurrentOS.IsWow64Process(currentProcess.Handle, out wow64Process) && wow64Process;
        }
      }
    }

    static CurrentOS()
    {
      CurrentOS.IsWindows = Path.DirectorySeparatorChar == '\\';
      if (CurrentOS.IsWindows)
      {
        CurrentOS.Name = Environment.OSVersion.VersionString;
        CurrentOS.Name = CurrentOS.Name.Replace("Microsoft ", "");
        CurrentOS.Name = CurrentOS.Name.Replace("  ", " ");
        CurrentOS.Name = CurrentOS.Name.Replace(" )", ")");
        CurrentOS.Name = CurrentOS.Name.Trim();
        CurrentOS.Name = CurrentOS.Name.Replace("NT 6.2", "8 %bit 6.2");
        CurrentOS.Name = CurrentOS.Name.Replace("NT 6.1", "7 %bit 6.1");
        CurrentOS.Name = CurrentOS.Name.Replace("NT 6.0", "Vista %bit 6.0");
        CurrentOS.Name = CurrentOS.Name.Replace("NT 5.", "XP %bit 5.");
        CurrentOS.Name = CurrentOS.Name.Replace("%bit", CurrentOS.Is64bitWindows ? "64bit" : "32bit");
        if (CurrentOS.Is64bitWindows)
          CurrentOS.Is64bit = true;
        else
          CurrentOS.Is32bit = true;
      }
      else
      {
        string str = CurrentOS.ReadProcessOutput("uname");
        if (str.Contains("Darwin"))
        {
          CurrentOS.IsUnix = true;
          CurrentOS.IsMac = true;
          CurrentOS.Name = "MacOS X " + CurrentOS.ReadProcessOutput("sw_vers", "-productVersion");
          CurrentOS.Name = CurrentOS.Name.Trim();
          if (CurrentOS.ReadProcessOutput("uname", "-m").Contains("x86_64"))
            CurrentOS.Is64bit = true;
          else
            CurrentOS.Is32bit = true;
          CurrentOS.Name = CurrentOS.Name + " " + (CurrentOS.Is32bit ? "32bit" : "64bit");
        }
        else if (str.Contains("Linux"))
        {
          CurrentOS.IsUnix = true;
          CurrentOS.IsLinux = true;
          CurrentOS.Name = CurrentOS.ReadProcessOutput("lsb_release", "-d");
          CurrentOS.Name = CurrentOS.Name.Substring(CurrentOS.Name.IndexOf(":") + 1);
          CurrentOS.Name = CurrentOS.Name.Trim();
          if (CurrentOS.ReadProcessOutput("uname", "-m").Contains("x86_64"))
            CurrentOS.Is64bit = true;
          else
            CurrentOS.Is32bit = true;
          CurrentOS.Name = CurrentOS.Name + " " + (CurrentOS.Is32bit ? "32bit" : "64bit");
        }
        else if (str != "")
          CurrentOS.IsUnix = true;
        else
          CurrentOS.IsUnknown = true;
      }
    }

    private static string ReadProcessOutput(string name) => CurrentOS.ReadProcessOutput(name, (string) null);

    private static string ReadProcessOutput(string name, string args)
    {
      try
      {
        Process process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        if (args != null && args != "")
          process.StartInfo.Arguments = " " + args;
        process.StartInfo.FileName = name;
        process.Start();
        string str = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        if (str == null)
          str = "";
        return str.Trim();
      }
      catch
      {
        return "";
      }
    }
  }
}
