// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Program
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public static class Program
  {
    private static Form mainForm;
    public static string[] HTACCESS_USER = new string[2]
    {
      "savewizard_1",
      "savewizard_1"
    };
    public static string[] HTACCESS_PWD = new string[2]
    {
      "Wd2l#@vqjun)3K",
      "Wd2l#@vqjun)3K"
    };

    [STAThread]
    public static void Main(string[] args)
    {
      if ((uint) args.Length > 0U)
      {
        foreach (string str in args)
        {
          if (str == "--version")
          {
            if (Util.IsUnixOrMacOSX())
            {
              Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version.ToString());
              return;
            }
            int num = (int) Util.ShowMessage(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            return;
          }
        }
        Util.ProcedArguments(args);
      }
      SingleInstanceApplication instanceApplication = new SingleInstanceApplication();
      instanceApplication.StartupNextInstance += new StartupNextInstanceEventHandler(Program.OnAppStartupNextInstance);
      Program.mainForm = (Form) new MainForm3();
      if (Util.IsUnixOrMacOSX() && Util.IsOldMono())
      {
        int num1 = (int) CustomMsgBox.Show(PS3SaveEditor.Resources.Resources.OldMonoMsg);
      }
      instanceApplication.Run(Program.mainForm);
    }

    private static void OnAppStartupNextInstance(object sender, StartupNextInstanceEventArgs e)
    {
      if (Program.mainForm.WindowState == FormWindowState.Minimized)
        Program.mainForm.WindowState = FormWindowState.Normal;
      Program.mainForm.Activate();
    }

    private static void CreateMacMenu()
    {
    }

    private static void Terminate()
    {
    }

    private static void MaxOpenFiles()
    {
    }
  }
}
