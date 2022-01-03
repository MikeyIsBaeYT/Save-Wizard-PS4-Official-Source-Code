// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TreeDataSourceAdapter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.ComponentModel;

namespace BrightIdeasSoftware
{
  public class TreeDataSourceAdapter : DataSourceAdapter
  {
    private string keyAspectName;
    private string parentKeyAspectName;
    private object rootKeyValue;
    private bool showKeyColumns = true;
    private readonly DataTreeListView treeListView;
    private Munger keyMunger;
    private Munger parentKeyMunger;

    public TreeDataSourceAdapter(DataTreeListView tlv)
      : base((ObjectListView) tlv)
    {
      this.treeListView = tlv;
      this.treeListView.CanExpandGetter = (BrightIdeasSoftware.TreeListView.CanExpandGetterDelegate) (model => this.CalculateHasChildren(model));
      this.treeListView.ChildrenGetter = (BrightIdeasSoftware.TreeListView.ChildrenGetterDelegate) (model => this.CalculateChildren(model));
    }

    public virtual string KeyAspectName
    {
      get => this.keyAspectName;
      set
      {
        if (this.keyAspectName == value)
          return;
        this.keyAspectName = value;
        this.keyMunger = new Munger(this.KeyAspectName);
        this.InitializeDataSource();
      }
    }

    public virtual string ParentKeyAspectName
    {
      get => this.parentKeyAspectName;
      set
      {
        if (this.parentKeyAspectName == value)
          return;
        this.parentKeyAspectName = value;
        this.parentKeyMunger = new Munger(this.ParentKeyAspectName);
        this.InitializeDataSource();
      }
    }

    public virtual object RootKeyValue
    {
      get => this.rootKeyValue;
      set
      {
        if (object.Equals(this.rootKeyValue, value))
          return;
        this.rootKeyValue = value;
        this.InitializeDataSource();
      }
    }

    public virtual bool ShowKeyColumns
    {
      get => this.showKeyColumns;
      set => this.showKeyColumns = value;
    }

    protected DataTreeListView TreeListView => this.treeListView;

    protected override void InitializeDataSource()
    {
      base.InitializeDataSource();
      this.TreeListView.RebuildAll(true);
    }

    protected override void SetListContents() => this.TreeListView.Roots = this.CalculateRoots();

    protected override bool ShouldCreateColumn(PropertyDescriptor property) => (this.ShowKeyColumns || !(property.Name == this.KeyAspectName) && !(property.Name == this.ParentKeyAspectName)) && base.ShouldCreateColumn(property);

    protected override void HandleListChangedItemChanged(ListChangedEventArgs e)
    {
      if (e.PropertyDescriptor != null && (e.PropertyDescriptor.Name == this.KeyAspectName || e.PropertyDescriptor.Name == this.ParentKeyAspectName))
        this.InitializeDataSource();
      else
        base.HandleListChangedItemChanged(e);
    }

    protected override void ChangePosition(int index)
    {
      for (object parent = this.CalculateParent(this.CurrencyManager.List[index]); parent != null && !this.TreeListView.IsExpanded(parent); parent = this.CalculateParent(parent))
        this.TreeListView.Expand(parent);
      base.ChangePosition(index);
    }

    private IEnumerable CalculateRoots()
    {
      foreach (object x in (IEnumerable) this.CurrencyManager.List)
      {
        object parentKey = this.GetParentValue(x);
        if (object.Equals(this.RootKeyValue, parentKey))
          yield return x;
        parentKey = (object) null;
      }
    }

    private bool CalculateHasChildren(object model)
    {
      object keyValue = this.GetKeyValue(model);
      if (keyValue == null)
        return false;
      foreach (object model1 in (IEnumerable) this.CurrencyManager.List)
      {
        object parentValue = this.GetParentValue(model1);
        if (object.Equals(keyValue, parentValue))
          return true;
      }
      return false;
    }

    private IEnumerable CalculateChildren(object model)
    {
      object keyValue = this.GetKeyValue(model);
      if (keyValue != null)
      {
        foreach (object x in (IEnumerable) this.CurrencyManager.List)
        {
          object parentKey = this.GetParentValue(x);
          if (object.Equals(keyValue, parentKey))
            yield return x;
          parentKey = (object) null;
        }
      }
    }

    private object CalculateParent(object model)
    {
      object parentValue = this.GetParentValue(model);
      if (parentValue == null)
        return (object) null;
      foreach (object model1 in (IEnumerable) this.CurrencyManager.List)
      {
        object keyValue = this.GetKeyValue(model1);
        if (object.Equals(parentValue, keyValue))
          return model1;
      }
      return (object) null;
    }

    private object GetKeyValue(object model) => this.keyMunger == null ? (object) null : this.keyMunger.GetValue(model);

    private object GetParentValue(object model) => this.parentKeyMunger == null ? (object) null : this.parentKeyMunger.GetValue(model);
  }
}
