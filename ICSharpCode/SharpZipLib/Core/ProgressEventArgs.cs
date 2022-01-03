// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ProgressEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Core
{
  public class ProgressEventArgs : EventArgs
  {
    private string name_;
    private long processed_;
    private long target_;
    private bool continueRunning_ = true;

    public ProgressEventArgs(string name, long processed, long target)
    {
      this.name_ = name;
      this.processed_ = processed;
      this.target_ = target;
    }

    public string Name => this.name_;

    public bool ContinueRunning
    {
      get => this.continueRunning_;
      set => this.continueRunning_ = value;
    }

    public float PercentComplete => this.target_ > 0L ? (float) ((double) this.processed_ / (double) this.target_ * 100.0) : 0.0f;

    public long Processed => this.processed_;

    public long Target => this.target_;
  }
}
