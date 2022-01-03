// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractDragSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class AbstractDragSource : IDragSource
  {
    public virtual object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item) => (object) null;

    public virtual DragDropEffects GetAllowedEffects(object data) => DragDropEffects.None;

    public virtual void EndDrag(object dragObject, DragDropEffects effect)
    {
    }
  }
}
