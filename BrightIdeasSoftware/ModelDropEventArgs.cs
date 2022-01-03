// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ModelDropEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;

namespace BrightIdeasSoftware
{
  public class ModelDropEventArgs : OlvDropEventArgs
  {
    private IList dragModels;
    private ArrayList toBeRefreshed = new ArrayList();
    private ObjectListView sourceListView;
    private object targetModel;

    public IList SourceModels
    {
      get => this.dragModels;
      internal set
      {
        this.dragModels = value;
        if (!(this.SourceListView is TreeListView sourceListView))
          return;
        foreach (object sourceModel in (IEnumerable) this.SourceModels)
        {
          object parent = sourceListView.GetParent(sourceModel);
          if (!this.toBeRefreshed.Contains(parent))
            this.toBeRefreshed.Add(parent);
        }
      }
    }

    public ObjectListView SourceListView
    {
      get => this.sourceListView;
      internal set => this.sourceListView = value;
    }

    public object TargetModel
    {
      get => this.targetModel;
      internal set => this.targetModel = value;
    }

    public void RefreshObjects()
    {
      if (this.SourceListView is TreeListView sourceListView)
      {
        foreach (object sourceModel in (IEnumerable) this.SourceModels)
        {
          object parent = sourceListView.GetParent(sourceModel);
          if (!this.toBeRefreshed.Contains(parent))
            this.toBeRefreshed.Add(parent);
        }
      }
      this.toBeRefreshed.AddRange((ICollection) this.SourceModels);
      if (this.ListView == this.SourceListView)
      {
        this.toBeRefreshed.Add(this.TargetModel);
        this.ListView.RefreshObjects((IList) this.toBeRefreshed);
      }
      else
      {
        this.SourceListView.RefreshObjects((IList) this.toBeRefreshed);
        this.ListView.RefreshObject(this.TargetModel);
      }
    }
  }
}
