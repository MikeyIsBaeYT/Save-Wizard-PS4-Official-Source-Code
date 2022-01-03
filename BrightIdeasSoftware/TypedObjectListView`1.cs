// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TypedObjectListView`1
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class TypedObjectListView<T> where T : class
  {
    private ObjectListView olv;
    private TypedObjectListView<T>.TypedCheckStateGetterDelegate checkStateGetter;
    private TypedObjectListView<T>.TypedCheckStatePutterDelegate checkStatePutter;

    public TypedObjectListView(ObjectListView olv) => this.olv = olv;

    public virtual T CheckedObject => (T) this.olv.CheckedObject;

    public virtual IList<T> CheckedObjects
    {
      get
      {
        IList checkedObjects = this.olv.CheckedObjects;
        List<T> objList = new List<T>(checkedObjects.Count);
        foreach (object obj in (IEnumerable) checkedObjects)
          objList.Add((T) obj);
        return (IList<T>) objList;
      }
      set => this.olv.CheckedObjects = (IList) value;
    }

    public virtual ObjectListView ListView
    {
      get => this.olv;
      set => this.olv = value;
    }

    public virtual IList<T> Objects
    {
      get
      {
        List<T> objList = new List<T>(this.olv.GetItemCount());
        for (int index = 0; index < this.olv.GetItemCount(); ++index)
          objList.Add(this.GetModelObject(index));
        return (IList<T>) objList;
      }
      set => this.olv.SetObjects((IEnumerable) value);
    }

    public virtual T SelectedObject
    {
      get => (T) this.olv.SelectedObject;
      set => this.olv.SelectedObject = (object) value;
    }

    public virtual IList<T> SelectedObjects
    {
      get
      {
        List<T> objList = new List<T>(this.olv.SelectedIndices.Count);
        foreach (int selectedIndex in this.olv.SelectedIndices)
          objList.Add((T) this.olv.GetModelObject(selectedIndex));
        return (IList<T>) objList;
      }
      set => this.olv.SelectedObjects = (IList) value;
    }

    public virtual TypedColumn<T> GetColumn(int i) => new TypedColumn<T>(this.olv.GetColumn(i));

    public virtual TypedColumn<T> GetColumn(string name) => new TypedColumn<T>(this.olv.GetColumn(name));

    public virtual T GetModelObject(int index) => (T) this.olv.GetModelObject(index);

    public virtual TypedObjectListView<T>.TypedCheckStateGetterDelegate CheckStateGetter
    {
      get => this.checkStateGetter;
      set
      {
        this.checkStateGetter = value;
        if (value == null)
          this.olv.CheckStateGetter = (CheckStateGetterDelegate) null;
        else
          this.olv.CheckStateGetter = (CheckStateGetterDelegate) (x => this.checkStateGetter((T) x));
      }
    }

    public virtual TypedObjectListView<T>.TypedBooleanCheckStateGetterDelegate BooleanCheckStateGetter
    {
      set
      {
        if (value == null)
          this.olv.BooleanCheckStateGetter = (BooleanCheckStateGetterDelegate) null;
        else
          this.olv.BooleanCheckStateGetter = (BooleanCheckStateGetterDelegate) (x => value((T) x));
      }
    }

    public virtual TypedObjectListView<T>.TypedCheckStatePutterDelegate CheckStatePutter
    {
      get => this.checkStatePutter;
      set
      {
        this.checkStatePutter = value;
        if (value == null)
          this.olv.CheckStatePutter = (CheckStatePutterDelegate) null;
        else
          this.olv.CheckStatePutter = (CheckStatePutterDelegate) ((x, newValue) => this.checkStatePutter((T) x, newValue));
      }
    }

    public virtual TypedObjectListView<T>.TypedBooleanCheckStatePutterDelegate BooleanCheckStatePutter
    {
      set
      {
        if (value == null)
          this.olv.BooleanCheckStatePutter = (BooleanCheckStatePutterDelegate) null;
        else
          this.olv.BooleanCheckStatePutter = (BooleanCheckStatePutterDelegate) ((x, newValue) => value((T) x, newValue));
      }
    }

    public virtual TypedObjectListView<T>.TypedCellToolTipGetterDelegate CellToolTipGetter
    {
      set
      {
        if (value == null)
          this.olv.CellToolTipGetter = (CellToolTipGetterDelegate) null;
        else
          this.olv.CellToolTipGetter = (CellToolTipGetterDelegate) ((col, x) => value(col, (T) x));
      }
    }

    public virtual HeaderToolTipGetterDelegate HeaderToolTipGetter
    {
      get => this.olv.HeaderToolTipGetter;
      set => this.olv.HeaderToolTipGetter = value;
    }

    public virtual void GenerateAspectGetters()
    {
      for (int i = 0; i < this.ListView.Columns.Count; ++i)
        this.GetColumn(i).GenerateAspectGetter();
    }

    public delegate CheckState TypedCheckStateGetterDelegate(T rowObject) where T : class;

    public delegate bool TypedBooleanCheckStateGetterDelegate(T rowObject) where T : class;

    public delegate CheckState TypedCheckStatePutterDelegate(
      T rowObject,
      CheckState newValue)
      where T : class;

    public delegate bool TypedBooleanCheckStatePutterDelegate(T rowObject, bool newValue) where T : class;

    public delegate string TypedCellToolTipGetterDelegate(OLVColumn column, T modelObject) where T : class;
  }
}
