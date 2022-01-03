// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DateTimePortion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  [Flags]
  public enum DateTimePortion
  {
    Year = 1,
    Month = 2,
    Day = 4,
    Hour = 8,
    Minute = 16, // 0x00000010
    Second = 32, // 0x00000020
  }
}
