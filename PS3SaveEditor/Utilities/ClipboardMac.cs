// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.Utilities.ClipboardMac
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PS3SaveEditor.Utilities
{
  public static class ClipboardMac
  {
    public static void CopyToClipboard(TextBox textBoxSource)
    {
      try
      {
        using (Process process = new Process())
        {
          process.StartInfo = new ProcessStartInfo("pbcopy", "-pboard general -Prefer txt");
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = false;
          process.StartInfo.RedirectStandardInput = true;
          process.Start();
          process.StandardInput.Write(textBoxSource.SelectedText);
          process.StandardInput.Close();
          process.WaitForExit();
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static void PasteFromClipboard(TextBox textBoxTarget)
    {
      try
      {
        using (Process process = new Process())
        {
          process.StartInfo = new ProcessStartInfo("pbpaste", "-pboard general");
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.Start();
          string end = process.StandardOutput.ReadToEnd();
          textBoxTarget.Paste(end);
          process.StandardInput.Close();
          process.WaitForExit();
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
