// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipEntryTimestamp
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Ionic.Zip
{
  [Flags]
  public enum ZipEntryTimestamp
  {
    None = 0,
    DOS = 1,
    Windows = 2,
    Unix = 4,
    InfoZip1 = 8,
  }
}
