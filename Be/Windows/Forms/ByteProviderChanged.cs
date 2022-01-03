// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.ByteProviderChanged
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Be.Windows.Forms
{
  public class ByteProviderChanged : EventArgs
  {
    public long Index { get; set; }

    public byte OldValue { get; set; }

    public byte NewValue { get; set; }

    public ChangeType ChangeType { get; set; }
  }
}
