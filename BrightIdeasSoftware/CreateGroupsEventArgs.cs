// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CreateGroupsEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class CreateGroupsEventArgs : EventArgs
  {
    private GroupingParameters parameters;
    private IList<OLVGroup> groups;
    private bool canceled;

    public CreateGroupsEventArgs(GroupingParameters parms) => this.parameters = parms;

    public GroupingParameters Parameters => this.parameters;

    public IList<OLVGroup> Groups
    {
      get => this.groups;
      set => this.groups = value;
    }

    public bool Canceled
    {
      get => this.canceled;
      set => this.canceled = value;
    }
  }
}
