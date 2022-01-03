// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.Generator
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class Generator : IGenerator
  {
    private static IGenerator instance;

    public static IGenerator Instance
    {
      get => Generator.instance ?? (Generator.instance = (IGenerator) new Generator());
      set => Generator.instance = value;
    }

    public static void GenerateColumns(ObjectListView olv, IEnumerable enumerable) => Generator.GenerateColumns(olv, enumerable, false);

    public static void GenerateColumns(
      ObjectListView olv,
      IEnumerable enumerable,
      bool allProperties)
    {
      if (enumerable != null)
      {
        IEnumerator enumerator = enumerable.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            Generator.Instance.GenerateAndReplaceColumns(olv, current.GetType(), allProperties);
            return;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable4)
            disposable4.Dispose();
        }
      }
      Generator.Instance.GenerateAndReplaceColumns(olv, (Type) null, allProperties);
    }

    public static void GenerateColumns(ObjectListView olv, Type type) => Generator.Instance.GenerateAndReplaceColumns(olv, type, false);

    public static void GenerateColumns(ObjectListView olv, Type type, bool allProperties) => Generator.Instance.GenerateAndReplaceColumns(olv, type, allProperties);

    public static IList<OLVColumn> GenerateColumns(Type type) => Generator.Instance.GenerateColumns(type, false);

    public virtual void GenerateAndReplaceColumns(
      ObjectListView olv,
      Type type,
      bool allProperties)
    {
      IList<OLVColumn> columns = this.GenerateColumns(type, allProperties);
      if (olv is TreeListView tlv)
        this.TryGenerateChildrenDelegates(tlv, type);
      this.ReplaceColumns(olv, columns);
    }

    public virtual IList<OLVColumn> GenerateColumns(Type type, bool allProperties)
    {
      List<OLVColumn> olvColumnList = new List<OLVColumn>();
      if (type == (Type) null)
        return (IList<OLVColumn>) olvColumnList;
      foreach (PropertyInfo property in type.GetProperties())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) property, typeof (OLVIgnoreAttribute)) == null)
        {
          if (!(Attribute.GetCustomAttribute((MemberInfo) property, typeof (OLVColumnAttribute)) is OLVColumnAttribute customAttribute))
          {
            if (allProperties)
              olvColumnList.Add(this.MakeColumnFromPropertyInfo(property));
          }
          else
            olvColumnList.Add(this.MakeColumnFromAttribute(property, customAttribute));
        }
      }
      int num1 = 0;
      foreach (ColumnHeader columnHeader in olvColumnList)
      {
        if (columnHeader.DisplayIndex >= 0)
          ++num1;
      }
      int num2 = num1;
      foreach (OLVColumn olvColumn in olvColumnList)
      {
        if (olvColumn.DisplayIndex < 0)
          olvColumn.DisplayIndex = num2++;
      }
      olvColumnList.Sort((Comparison<OLVColumn>) ((x, y) => x.DisplayIndex.CompareTo(y.DisplayIndex)));
      return (IList<OLVColumn>) olvColumnList;
    }

    protected virtual void ReplaceColumns(ObjectListView olv, IList<OLVColumn> columns)
    {
      olv.Reset();
      if (columns == null || columns.Count == 0)
        return;
      olv.AllColumns.AddRange((IEnumerable<OLVColumn>) columns);
      this.PostCreateColumns(olv);
    }

    public virtual void PostCreateColumns(ObjectListView olv)
    {
      if (olv.AllColumns.Exists((Predicate<OLVColumn>) (x => x.CheckBoxes)))
        olv.UseSubItemCheckBoxes = true;
      if (olv.AllColumns.Exists((Predicate<OLVColumn>) (x => x.Index > 0 && (x.ImageGetter != null || !string.IsNullOrEmpty(x.ImageAspectName)))))
        olv.ShowImagesOnSubItems = true;
      olv.RebuildColumns();
      olv.AutoSizeColumns();
    }

    protected virtual OLVColumn MakeColumnFromAttribute(
      PropertyInfo pinfo,
      OLVColumnAttribute attr)
    {
      return this.MakeColumn(pinfo.Name, this.DisplayNameToColumnTitle(pinfo.Name), pinfo.CanWrite, pinfo.PropertyType, attr);
    }

    protected virtual OLVColumn MakeColumnFromPropertyInfo(PropertyInfo pinfo) => this.MakeColumn(pinfo.Name, this.DisplayNameToColumnTitle(pinfo.Name), pinfo.CanWrite, pinfo.PropertyType, (OLVColumnAttribute) null);

    public virtual OLVColumn MakeColumnFromPropertyDescriptor(PropertyDescriptor pd)
    {
      OLVColumnAttribute attribute = pd.Attributes[typeof (OLVColumnAttribute)] as OLVColumnAttribute;
      return this.MakeColumn(pd.Name, this.DisplayNameToColumnTitle(pd.DisplayName), !pd.IsReadOnly, pd.PropertyType, attribute);
    }

    protected virtual OLVColumn MakeColumn(
      string aspectName,
      string title,
      bool editable,
      Type propertyType,
      OLVColumnAttribute attr)
    {
      OLVColumn column = this.MakeColumn(aspectName, title, attr);
      column.Name = attr == null || string.IsNullOrEmpty(attr.Name) ? aspectName : attr.Name;
      this.ConfigurePossibleBooleanColumn(column, propertyType);
      if (attr == null)
      {
        column.IsEditable = editable;
        return column;
      }
      column.AspectToStringFormat = attr.AspectToStringFormat;
      if (attr.IsCheckBoxesSet)
        column.CheckBoxes = attr.CheckBoxes;
      column.DisplayIndex = attr.DisplayIndex;
      column.FillsFreeSpace = attr.FillsFreeSpace;
      if (attr.IsFreeSpaceProportionSet)
        column.FreeSpaceProportion = attr.FreeSpaceProportion;
      column.GroupWithItemCountFormat = attr.GroupWithItemCountFormat;
      column.GroupWithItemCountSingularFormat = attr.GroupWithItemCountSingularFormat;
      column.Hyperlink = attr.Hyperlink;
      column.ImageAspectName = attr.ImageAspectName;
      column.IsEditable = attr.IsEditableSet ? attr.IsEditable : editable;
      column.IsTileViewColumn = attr.IsTileViewColumn;
      column.IsVisible = attr.IsVisible;
      column.MaximumWidth = attr.MaximumWidth;
      column.MinimumWidth = attr.MinimumWidth;
      column.Tag = (object) attr.Tag;
      if (attr.IsTextAlignSet)
        column.TextAlign = attr.TextAlign;
      column.ToolTipText = attr.ToolTipText;
      if (attr.IsTriStateCheckBoxesSet)
        column.TriStateCheckBoxes = attr.TriStateCheckBoxes;
      column.UseInitialLetterForGroup = attr.UseInitialLetterForGroup;
      column.Width = attr.Width;
      if (attr.GroupCutoffs != null && attr.GroupDescriptions != null)
        column.MakeGroupies(attr.GroupCutoffs, attr.GroupDescriptions);
      return column;
    }

    protected virtual OLVColumn MakeColumn(
      string aspectName,
      string title,
      OLVColumnAttribute attr)
    {
      return new OLVColumn(attr == null || string.IsNullOrEmpty(attr.Title) ? title : attr.Title, aspectName);
    }

    protected virtual string DisplayNameToColumnTitle(string displayName) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(displayName.Replace("_", " "), "(\\p{Ll})(\\p{Lu})", "$1 $2"));

    protected virtual void ConfigurePossibleBooleanColumn(OLVColumn column, Type propertyType)
    {
      if (propertyType != typeof (bool) && propertyType != typeof (bool?) && propertyType != typeof (CheckState))
        return;
      column.CheckBoxes = true;
      column.TextAlign = HorizontalAlignment.Center;
      column.Width = 32;
      column.TriStateCheckBoxes = propertyType == typeof (bool?) || propertyType == typeof (CheckState);
    }

    protected virtual void TryGenerateChildrenDelegates(TreeListView tlv, Type type)
    {
      foreach (PropertyInfo property in type.GetProperties())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) property, typeof (OLVChildrenAttribute)) is OLVChildrenAttribute)
        {
          this.GenerateChildrenDelegates(tlv, property);
          break;
        }
      }
    }

    protected virtual void GenerateChildrenDelegates(TreeListView tlv, PropertyInfo pinfo)
    {
      Munger childrenGetter = new Munger(pinfo.Name);
      tlv.CanExpandGetter = (TreeListView.CanExpandGetterDelegate) (x =>
      {
        try
        {
          return !ObjectListView.IsEnumerableEmpty(childrenGetter.GetValueEx(x) as IEnumerable);
        }
        catch (MungerException ex)
        {
          return false;
        }
      });
      tlv.ChildrenGetter = (TreeListView.ChildrenGetterDelegate) (x =>
      {
        try
        {
          return childrenGetter.GetValueEx(x) as IEnumerable;
        }
        catch (MungerException ex)
        {
          return (IEnumerable) null;
        }
      });
    }
  }
}
