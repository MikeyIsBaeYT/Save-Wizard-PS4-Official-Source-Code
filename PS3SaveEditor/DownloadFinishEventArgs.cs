// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.DownloadFinishEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace PS3SaveEditor
{
  public class DownloadFinishEventArgs : EventArgs
  {
    private bool m_status;
    private string m_error;

    public bool Status => this.m_status;

    public string Error => this.m_error;

    public DownloadFinishEventArgs(bool status, string error)
    {
      this.m_status = status;
      this.m_error = error;
    }
  }
}
