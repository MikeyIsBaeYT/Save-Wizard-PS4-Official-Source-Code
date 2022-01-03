// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CellClickEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class CellClickEventArgs : CellEventArgs
  {
    private int clickCount;

    public int ClickCount
    {
      get => this.clickCount;
      set => this.clickCount = value;
    }
  }
}
