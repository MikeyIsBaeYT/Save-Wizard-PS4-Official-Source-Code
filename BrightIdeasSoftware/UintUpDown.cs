// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.UintUpDown
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  internal class UintUpDown : NumericUpDown
  {
    public UintUpDown()
    {
      this.DecimalPlaces = 0;
      this.Minimum = 0M;
      this.Maximum = 9999999M;
    }

    public uint Value
    {
      get => Decimal.ToUInt32(base.Value);
      set => this.Value = new Decimal(value);
    }
  }
}
