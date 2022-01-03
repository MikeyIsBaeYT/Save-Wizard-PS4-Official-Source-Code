// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.SimpleDragSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class SimpleDragSource : IDragSource
  {
    private bool refreshAfterDrop;

    public SimpleDragSource()
    {
    }

    public SimpleDragSource(bool refreshAfterDrop) => this.RefreshAfterDrop = refreshAfterDrop;

    public bool RefreshAfterDrop
    {
      get => this.refreshAfterDrop;
      set => this.refreshAfterDrop = value;
    }

    public virtual object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item) => button != MouseButtons.Left ? (object) null : this.CreateDataObject(olv);

    public virtual DragDropEffects GetAllowedEffects(object data) => DragDropEffects.All | DragDropEffects.Link;

    public virtual void EndDrag(object dragObject, DragDropEffects effect)
    {
      if (!(dragObject is OLVDataObject olvDataObject) || !this.RefreshAfterDrop)
        return;
      olvDataObject.ListView.RefreshObjects(olvDataObject.ModelObjects);
    }

    protected virtual object CreateDataObject(ObjectListView olv)
    {
      OLVDataObject olvDataObject = new OLVDataObject(olv);
      olvDataObject.CreateTextFormats();
      return (object) olvDataObject;
    }
  }
}
