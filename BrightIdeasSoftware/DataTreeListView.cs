// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DataTreeListView
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace BrightIdeasSoftware
{
  public class DataTreeListView : TreeListView
  {
    private TreeDataSourceAdapter adapter;

    [Category("Data")]
    [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
    public virtual object DataSource
    {
      get => this.Adapter.DataSource;
      set => this.Adapter.DataSource = value;
    }

    [Category("Data")]
    [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design", typeof (UITypeEditor))]
    [DefaultValue("")]
    public virtual string DataMember
    {
      get => this.Adapter.DataMember;
      set => this.Adapter.DataMember = value;
    }

    [Category("Data")]
    [Description("The name of the property/column that holds the key of a row")]
    [DefaultValue(null)]
    public virtual string KeyAspectName
    {
      get => this.Adapter.KeyAspectName;
      set => this.Adapter.KeyAspectName = value;
    }

    [Category("Data")]
    [Description("The name of the property/column that holds the key of the parent of a row")]
    [DefaultValue(null)]
    public virtual string ParentKeyAspectName
    {
      get => this.Adapter.ParentKeyAspectName;
      set => this.Adapter.ParentKeyAspectName = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual object RootKeyValue
    {
      get => this.Adapter.RootKeyValue;
      set => this.Adapter.RootKeyValue = value;
    }

    [Category("Data")]
    [Description("The parent id value that identifies a row as a root object")]
    [DefaultValue(null)]
    public virtual string RootKeyValueString
    {
      get => Convert.ToString(this.Adapter.RootKeyValue);
      set => this.Adapter.RootKeyValue = (object) value;
    }

    [Category("Data")]
    [Description("Should the keys columns (id and parent id) be shown to the user?")]
    [DefaultValue(true)]
    public virtual bool ShowKeyColumns
    {
      get => this.Adapter.ShowKeyColumns;
      set => this.Adapter.ShowKeyColumns = value;
    }

    protected TreeDataSourceAdapter Adapter
    {
      get
      {
        if (this.adapter == null)
          this.adapter = new TreeDataSourceAdapter(this);
        return this.adapter;
      }
      set => this.adapter = value;
    }
  }
}
