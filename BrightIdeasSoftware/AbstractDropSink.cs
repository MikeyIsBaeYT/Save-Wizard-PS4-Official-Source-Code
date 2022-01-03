// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractDropSink
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractDropSink : IDropSink
  {
    private ObjectListView listView;

    public virtual ObjectListView ListView
    {
      get => this.listView;
      set => this.listView = value;
    }

    public virtual void DrawFeedback(Graphics g, Rectangle bounds)
    {
    }

    public virtual void Drop(DragEventArgs args) => this.Cleanup();

    public virtual void Enter(DragEventArgs args)
    {
    }

    public virtual void Leave() => this.Cleanup();

    public virtual void Over(DragEventArgs args)
    {
    }

    public virtual void GiveFeedback(GiveFeedbackEventArgs args) => args.UseDefaultCursors = true;

    public virtual void QueryContinue(QueryContinueDragEventArgs args)
    {
    }

    protected virtual void Cleanup()
    {
    }
  }
}
