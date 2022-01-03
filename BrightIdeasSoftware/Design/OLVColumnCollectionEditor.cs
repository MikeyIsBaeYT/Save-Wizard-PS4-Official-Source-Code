// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.Design.OLVColumnCollectionEditor
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace BrightIdeasSoftware.Design
{
  public class OLVColumnCollectionEditor : CollectionEditor
  {
    public OLVColumnCollectionEditor(Type t)
      : base(t)
    {
    }

    protected override Type CreateCollectionItemType() => typeof (OLVColumn);

    public override object EditValue(
      ITypeDescriptorContext context,
      IServiceProvider provider,
      object value)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (provider == null)
        throw new ArgumentNullException(nameof (provider));
      ObjectListView instance = context.Instance as ObjectListView;
      base.EditValue(context, provider, (object) instance.AllColumns);
      List<OLVColumn> filteredColumns = instance.GetFilteredColumns(View.Details);
      instance.Columns.Clear();
      instance.Columns.AddRange((ColumnHeader[]) filteredColumns.ToArray());
      return (object) instance.Columns;
    }

    protected override string GetDisplayText(object value) => !(value is OLVColumn olvColumn) || string.IsNullOrEmpty(olvColumn.AspectName) ? base.GetDisplayText(value) : string.Format("{0} ({1})", (object) base.GetDisplayText(value), (object) olvColumn.AspectName);
  }
}
