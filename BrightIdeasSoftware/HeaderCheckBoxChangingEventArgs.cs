// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.HeaderCheckBoxChangingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class HeaderCheckBoxChangingEventArgs : CancelEventArgs
  {
    private OLVColumn column;
    private CheckState newCheckState;

    public OLVColumn Column
    {
      get => this.column;
      internal set => this.column = value;
    }

    public CheckState NewCheckState
    {
      get => this.newCheckState;
      set => this.newCheckState = value;
    }
  }
}
