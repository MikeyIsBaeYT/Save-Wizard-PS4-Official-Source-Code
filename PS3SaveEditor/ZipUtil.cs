// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.ZipUtil
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace PS3SaveEditor
{
  internal class ZipUtil
  {
    public static string GetAsZipFile(string[] filePaths, ZipUtil.OnZipProgress onProgress)
    {
      string tempFileName = Path.GetTempFileName();
      File.Delete(tempFileName);
      ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) File.Create(tempFileName));
      zipOutputStream.UseZip64 = UseZip64.Off;
      byte[] buffer = new byte[4096];
      foreach (string filePath in filePaths)
      {
        FileStream fileStream = File.OpenRead(filePath);
        try
        {
          ZipEntry entry = new ZipEntry(Path.GetFileName(filePath));
          entry.DosTime = 0L;
          entry.DateTime = DateTime.MinValue;
          zipOutputStream.PutNextEntry(entry);
          if (onProgress != null)
            StreamUtils.Copy((Stream) fileStream, (Stream) zipOutputStream, buffer, (ProgressHandler) ((snder, e) => e.ContinueRunning = onProgress((int) e.PercentComplete)), TimeSpan.FromSeconds(1.0), (object) null, "");
          else
            StreamUtils.Copy((Stream) fileStream, (Stream) zipOutputStream, buffer);
          if (entry.CompressedSize == 0L)
            break;
        }
        finally
        {
          fileStream.Close();
        }
      }
      zipOutputStream.Finish();
      zipOutputStream.Close();
      return tempFileName;
    }

    public static string GetAsZipFile(
      string[] filePaths,
      string profile,
      ZipUtil.OnZipProgress onProgress)
    {
      string tempFileName1 = Path.GetTempFileName();
      File.Delete(tempFileName1);
      ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) File.Create(tempFileName1));
      zipOutputStream.UseZip64 = UseZip64.Off;
      byte[] buffer = new byte[4096];
      int num1 = 0;
      foreach (string filePath in filePaths)
      {
        if (File.Exists(filePath))
        {
          FileStream fileStream = File.OpenRead(filePath);
          string fileName = Path.GetFileName(filePath);
          try
          {
            if (fileName.ToUpper() == "PARAM.SFO" && profile != "None")
            {
              string tempFileName2 = Path.GetTempFileName();
              File.Delete(tempFileName2);
              File.Copy(filePath, tempFileName2);
              fileStream.Close();
              fileStream = File.OpenRead(tempFileName2);
            }
            ZipEntry entry = new ZipEntry(fileName);
            zipOutputStream.PutNextEntry(entry);
            if (fileStream.Length > 1000000L)
              StreamUtils.Copy((Stream) fileStream, (Stream) zipOutputStream, buffer, (ProgressHandler) ((snder, e) => e.ContinueRunning = onProgress((int) e.PercentComplete)), TimeSpan.FromSeconds(1.0), (object) null, "");
            else
              StreamUtils.Copy((Stream) fileStream, (Stream) zipOutputStream, buffer);
            int num2 = onProgress(num1 * 100 / filePaths.Length) ? 1 : 0;
          }
          finally
          {
            fileStream.Close();
            if (fileName.ToUpper() == "PARAM.SFO" && profile != "None")
            {
              File.SetAttributes(fileStream.Name, FileAttributes.Normal);
              File.Delete(fileStream.Name);
            }
          }
          ++num1;
        }
      }
      zipOutputStream.Finish();
      zipOutputStream.Close();
      return tempFileName1;
    }

    public static string GetPs3SeTempFolder()
    {
      string tempFolder = Util.GetTempFolder();
      if (!Directory.Exists(tempFolder))
        Directory.CreateDirectory(tempFolder);
      return tempFolder;
    }

    public delegate bool OnZipProgress(int progress);
  }
}
