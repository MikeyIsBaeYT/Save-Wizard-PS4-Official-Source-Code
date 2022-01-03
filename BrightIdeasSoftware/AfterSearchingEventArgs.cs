// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AfterSearchingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class AfterSearchingEventArgs : EventArgs
  {
    private string stringToFind;
    public bool Handled;
    private int indexSelected;

    public AfterSearchingEventArgs(string stringToFind, int indexSelected)
    {
      this.stringToFind = stringToFind;
      this.indexSelected = indexSelected;
    }

    public string StringToFind => this.stringToFind;

    public int IndexSelected => this.indexSelected;
  }
}
