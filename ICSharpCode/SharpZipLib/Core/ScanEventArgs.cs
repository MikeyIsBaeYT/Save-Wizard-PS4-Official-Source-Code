// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ScanEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Core
{
  public class ScanEventArgs : EventArgs
  {
    private string name_;
    private bool continueRunning_ = true;

    public ScanEventArgs(string name) => this.name_ = name;

    public string Name => this.name_;

    public bool ContinueRunning
    {
      get => this.continueRunning_;
      set => this.continueRunning_ = value;
    }
  }
}
