// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.GroupStateChangedEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class GroupStateChangedEventArgs : EventArgs
  {
    private readonly OLVGroup group;
    private readonly GroupState oldState;
    private readonly GroupState newState;

    public GroupStateChangedEventArgs(OLVGroup group, GroupState oldState, GroupState newState)
    {
      this.group = group;
      this.oldState = oldState;
      this.newState = newState;
    }

    public bool Collapsed => (this.oldState & GroupState.LVGS_COLLAPSED) != GroupState.LVGS_COLLAPSED && (this.newState & GroupState.LVGS_COLLAPSED) == GroupState.LVGS_COLLAPSED;

    public bool Focused => (this.oldState & GroupState.LVGS_FOCUSED) != GroupState.LVGS_FOCUSED && (this.newState & GroupState.LVGS_FOCUSED) == GroupState.LVGS_FOCUSED;

    public bool Selected => (this.oldState & GroupState.LVGS_SELECTED) != GroupState.LVGS_SELECTED && (this.newState & GroupState.LVGS_SELECTED) == GroupState.LVGS_SELECTED;

    public bool Uncollapsed => (this.oldState & GroupState.LVGS_COLLAPSED) == GroupState.LVGS_COLLAPSED && (this.newState & GroupState.LVGS_COLLAPSED) != GroupState.LVGS_COLLAPSED;

    public bool Unfocused => (this.oldState & GroupState.LVGS_FOCUSED) == GroupState.LVGS_FOCUSED && (this.newState & GroupState.LVGS_FOCUSED) != GroupState.LVGS_FOCUSED;

    public bool Unselected => (this.oldState & GroupState.LVGS_SELECTED) == GroupState.LVGS_SELECTED && (this.newState & GroupState.LVGS_SELECTED) != GroupState.LVGS_SELECTED;

    public OLVGroup Group => this.group;

    public GroupState OldState => this.oldState;

    public GroupState NewState => this.newState;
  }
}
