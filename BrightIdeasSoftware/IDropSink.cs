// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.IDropSink
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public interface IDropSink
  {
    ObjectListView ListView { get; set; }

    void DrawFeedback(Graphics g, Rectangle bounds);

    void Drop(DragEventArgs args);

    void Enter(DragEventArgs args);

    void GiveFeedback(GiveFeedbackEventArgs args);

    void Leave();

    void Over(DragEventArgs args);

    void QueryContinue(QueryContinueDragEventArgs args);
  }
}
