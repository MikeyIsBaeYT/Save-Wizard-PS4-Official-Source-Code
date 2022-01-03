// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DropTargetLocation
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  [Flags]
  public enum DropTargetLocation
  {
    None = 0,
    Background = 1,
    Item = 2,
    BetweenItems = 4,
    AboveItem = 8,
    BelowItem = 16, // 0x00000010
    SubItem = 32, // 0x00000020
    RightOfItem = 64, // 0x00000040
    LeftOfItem = 128, // 0x00000080
  }
}
