// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TreeBranchExpandingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class TreeBranchExpandingEventArgs : CancellableEventArgs
  {
    private object model;
    private OLVListItem item;

    public TreeBranchExpandingEventArgs(object model, OLVListItem item)
    {
      this.Model = model;
      this.Item = item;
    }

    public object Model
    {
      get => this.model;
      private set => this.model = value;
    }

    public OLVListItem Item
    {
      get => this.item;
      private set => this.item = value;
    }
  }
}
