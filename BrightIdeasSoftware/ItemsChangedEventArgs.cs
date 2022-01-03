// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ItemsChangedEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class ItemsChangedEventArgs : EventArgs
  {
    private int oldObjectCount;
    private int newObjectCount;

    public ItemsChangedEventArgs()
    {
    }

    public ItemsChangedEventArgs(int oldObjectCount, int newObjectCount)
    {
      this.oldObjectCount = oldObjectCount;
      this.newObjectCount = newObjectCount;
    }

    public int OldObjectCount => this.oldObjectCount;

    public int NewObjectCount => this.newObjectCount;
  }
}
