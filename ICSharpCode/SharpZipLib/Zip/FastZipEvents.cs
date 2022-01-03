// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.FastZipEvents
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using System;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class FastZipEvents
  {
    public ProcessDirectoryHandler ProcessDirectory;
    public ProcessFileHandler ProcessFile;
    public ProgressHandler Progress;
    public CompletedFileHandler CompletedFile;
    public DirectoryFailureHandler DirectoryFailure;
    public FileFailureHandler FileFailure;
    private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);

    public bool OnDirectoryFailure(string directory, Exception e)
    {
      bool flag = false;
      DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
      if (directoryFailure != null)
      {
        ScanFailureEventArgs e1 = new ScanFailureEventArgs(directory, e);
        directoryFailure((object) this, e1);
        flag = e1.ContinueRunning;
      }
      return flag;
    }

    public bool OnFileFailure(string file, Exception e)
    {
      FileFailureHandler fileFailure = this.FileFailure;
      bool flag = fileFailure != null;
      if (flag)
      {
        ScanFailureEventArgs e1 = new ScanFailureEventArgs(file, e);
        fileFailure((object) this, e1);
        flag = e1.ContinueRunning;
      }
      return flag;
    }

    public bool OnProcessFile(string file)
    {
      bool flag = true;
      ProcessFileHandler processFile = this.ProcessFile;
      if (processFile != null)
      {
        ScanEventArgs e = new ScanEventArgs(file);
        processFile((object) this, e);
        flag = e.ContinueRunning;
      }
      return flag;
    }

    public bool OnCompletedFile(string file)
    {
      bool flag = true;
      CompletedFileHandler completedFile = this.CompletedFile;
      if (completedFile != null)
      {
        ScanEventArgs e = new ScanEventArgs(file);
        completedFile((object) this, e);
        flag = e.ContinueRunning;
      }
      return flag;
    }

    public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
    {
      bool flag = true;
      ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
      if (processDirectory != null)
      {
        DirectoryEventArgs e = new DirectoryEventArgs(directory, hasMatchingFiles);
        processDirectory((object) this, e);
        flag = e.ContinueRunning;
      }
      return flag;
    }

    public TimeSpan ProgressInterval
    {
      get => this.progressInterval_;
      set => this.progressInterval_ = value;
    }
  }
}
