// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.GroupState
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  [Flags]
  public enum GroupState
  {
    LVGS_NORMAL = 0,
    LVGS_COLLAPSED = 1,
    LVGS_HIDDEN = 2,
    LVGS_NOHEADER = 4,
    LVGS_COLLAPSIBLE = 8,
    LVGS_FOCUSED = 16, // 0x00000010
    LVGS_SELECTED = 32, // 0x00000020
    LVGS_SUBSETED = 64, // 0x00000040
    LVGS_SUBSETLINKFOCUSED = 128, // 0x00000080
    LVGS_ALL = 65535, // 0x0000FFFF
  }
}
