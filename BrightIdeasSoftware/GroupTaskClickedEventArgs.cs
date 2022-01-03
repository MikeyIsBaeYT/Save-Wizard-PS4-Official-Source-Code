// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.GroupTaskClickedEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class GroupTaskClickedEventArgs : EventArgs
  {
    private readonly OLVGroup group;

    public GroupTaskClickedEventArgs(OLVGroup group) => this.group = group;

    public OLVGroup Group => this.group;
  }
}
