// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.RearrangingDropSink
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class RearrangingDropSink : SimpleDropSink
  {
    public RearrangingDropSink()
    {
      this.CanDropBetween = true;
      this.CanDropOnBackground = true;
      this.CanDropOnItem = false;
    }

    public RearrangingDropSink(bool acceptDropsFromOtherLists)
      : this()
    {
      this.AcceptExternal = acceptDropsFromOtherLists;
    }

    protected override void OnModelCanDrop(ModelDropEventArgs args)
    {
      base.OnModelCanDrop(args);
      if (args.Handled)
        return;
      args.Effect = DragDropEffects.Move;
      if (!this.AcceptExternal && args.SourceListView != this.ListView)
      {
        args.Effect = DragDropEffects.None;
        args.DropTargetLocation = DropTargetLocation.None;
        args.InfoMessage = "This list doesn't accept drops from other lists";
      }
      if (args.DropTargetLocation != DropTargetLocation.Background || args.SourceListView != this.ListView)
        return;
      args.Effect = DragDropEffects.None;
      args.DropTargetLocation = DropTargetLocation.None;
    }

    protected override void OnModelDropped(ModelDropEventArgs args)
    {
      base.OnModelDropped(args);
      if (args.Handled)
        return;
      this.RearrangeModels(args);
    }

    public virtual void RearrangeModels(ModelDropEventArgs args)
    {
      switch (args.DropTargetLocation)
      {
        case DropTargetLocation.Background:
          this.ListView.AddObjects((ICollection) args.SourceModels);
          break;
        case DropTargetLocation.AboveItem:
          this.ListView.MoveObjects(args.DropTargetIndex, (ICollection) args.SourceModels);
          break;
        case DropTargetLocation.BelowItem:
          this.ListView.MoveObjects(args.DropTargetIndex + 1, (ICollection) args.SourceModels);
          break;
        default:
          return;
      }
      if (args.SourceListView == this.ListView)
        return;
      args.SourceListView.RemoveObjects((ICollection) args.SourceModels);
    }
  }
}
