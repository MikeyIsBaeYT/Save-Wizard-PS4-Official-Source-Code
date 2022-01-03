// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.IDragSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public interface IDragSource
  {
    object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item);

    DragDropEffects GetAllowedEffects(object dragObject);

    void EndDrag(object dragObject, DragDropEffects effect);
  }
}
