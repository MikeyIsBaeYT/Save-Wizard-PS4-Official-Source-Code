// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DataSourceAdapter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class DataSourceAdapter : IDisposable
  {
    private bool autoGenerateColumns = true;
    private object dataSource;
    private string dataMember = "";
    private ObjectListView listView;
    private CurrencyManager currencyManager = (CurrencyManager) null;
    private bool isChangingIndex = false;
    private bool alreadyFreezing = false;

    public DataSourceAdapter(ObjectListView olv)
    {
      this.ListView = olv != null ? olv : throw new ArgumentNullException(nameof (olv));
      this.BindListView(this.ListView);
    }

    ~DataSourceAdapter() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public virtual void Dispose(bool fromUser)
    {
      this.UnbindListView(this.ListView);
      this.UnbindDataSource();
    }

    public bool AutoGenerateColumns
    {
      get => this.autoGenerateColumns;
      set => this.autoGenerateColumns = value;
    }

    public virtual object DataSource
    {
      get => this.dataSource;
      set
      {
        this.dataSource = value;
        this.RebindDataSource(true);
      }
    }

    public virtual string DataMember
    {
      get => this.dataMember;
      set
      {
        if (!(this.dataMember != value))
          return;
        this.dataMember = value;
        this.RebindDataSource();
      }
    }

    public ObjectListView ListView
    {
      get => this.listView;
      internal set => this.listView = value;
    }

    protected CurrencyManager CurrencyManager
    {
      get => this.currencyManager;
      set => this.currencyManager = value;
    }

    protected virtual void BindListView(ObjectListView olv)
    {
      if (olv == null)
        return;
      olv.Freezing += new EventHandler<FreezeEventArgs>(this.HandleListViewFreezing);
      olv.SelectedIndexChanged += new EventHandler(this.HandleListViewSelectedIndexChanged);
      olv.BindingContextChanged += new EventHandler(this.HandleListViewBindingContextChanged);
    }

    protected virtual void UnbindListView(ObjectListView olv)
    {
      if (olv == null)
        return;
      olv.Freezing -= new EventHandler<FreezeEventArgs>(this.HandleListViewFreezing);
      olv.SelectedIndexChanged -= new EventHandler(this.HandleListViewSelectedIndexChanged);
      olv.BindingContextChanged -= new EventHandler(this.HandleListViewBindingContextChanged);
    }

    protected virtual void BindDataSource()
    {
      if (this.CurrencyManager == null)
        return;
      this.CurrencyManager.MetaDataChanged += new EventHandler(this.HandleCurrencyManagerMetaDataChanged);
      this.CurrencyManager.PositionChanged += new EventHandler(this.HandleCurrencyManagerPositionChanged);
      this.CurrencyManager.ListChanged += new ListChangedEventHandler(this.CurrencyManagerListChanged);
    }

    protected virtual void UnbindDataSource()
    {
      if (this.CurrencyManager == null)
        return;
      this.CurrencyManager.MetaDataChanged -= new EventHandler(this.HandleCurrencyManagerMetaDataChanged);
      this.CurrencyManager.PositionChanged -= new EventHandler(this.HandleCurrencyManagerPositionChanged);
      this.CurrencyManager.ListChanged -= new ListChangedEventHandler(this.CurrencyManagerListChanged);
    }

    protected virtual void RebindDataSource() => this.RebindDataSource(false);

    protected virtual void RebindDataSource(bool forceDataInitialization)
    {
      CurrencyManager currencyManager = (CurrencyManager) null;
      if (this.ListView != null && this.ListView.BindingContext != null && this.DataSource != null)
        currencyManager = this.ListView.BindingContext[this.DataSource, this.DataMember] as CurrencyManager;
      if (this.CurrencyManager != currencyManager)
      {
        this.UnbindDataSource();
        this.CurrencyManager = currencyManager;
        this.BindDataSource();
        forceDataInitialization = true;
      }
      if (!forceDataInitialization)
        return;
      this.InitializeDataSource();
    }

    protected virtual void InitializeDataSource()
    {
      if (this.ListView.Frozen || this.CurrencyManager == null)
        return;
      this.CreateColumnsFromSource();
      this.CreateMissingAspectGettersAndPutters();
      this.SetListContents();
      this.ListView.AutoSizeColumns();
    }

    protected virtual void SetListContents() => this.ListView.Objects = (IEnumerable) this.CurrencyManager.List;

    protected virtual void CreateColumnsFromSource()
    {
      if (this.CurrencyManager == null || this.ListView.IsDesignMode || !this.AutoGenerateColumns)
        return;
      if (!(Generator.Instance is Generator generator1))
        generator1 = new Generator();
      Generator generator2 = generator1;
      PropertyDescriptorCollection itemProperties = this.CurrencyManager.GetItemProperties();
      if (itemProperties.Count == 0)
        return;
      foreach (PropertyDescriptor propertyDescriptor in itemProperties)
      {
        if (this.ShouldCreateColumn(propertyDescriptor))
        {
          OLVColumn column = generator2.MakeColumnFromPropertyDescriptor(propertyDescriptor);
          this.ConfigureColumn(column, propertyDescriptor);
          this.ListView.AllColumns.Add(column);
        }
      }
      generator2.PostCreateColumns(this.ListView);
    }

    protected virtual bool ShouldCreateColumn(PropertyDescriptor property) => !this.ListView.AllColumns.Exists((Predicate<OLVColumn>) (x => x.AspectName == property.Name)) && !(property.PropertyType == typeof (IBindingList)) && property.Attributes[typeof (OLVIgnoreAttribute)] == null;

    protected virtual void ConfigureColumn(OLVColumn column, PropertyDescriptor property)
    {
      column.LastDisplayIndex = this.ListView.AllColumns.Count;
      if (!(property.PropertyType == typeof (byte[])))
        return;
      column.Renderer = (IRenderer) new ImageRenderer();
    }

    protected virtual void CreateMissingAspectGettersAndPutters()
    {
      foreach (OLVColumn allColumn in this.ListView.AllColumns)
      {
        OLVColumn column = allColumn;
        if (column.AspectGetter == null && !string.IsNullOrEmpty(column.AspectName))
          column.AspectGetter = (AspectGetterDelegate) (row => !(row is DataRowView dataRowView10) ? column.GetAspectByName(row) : (dataRowView10.Row.RowState == DataRowState.Detached ? (object) null : dataRowView10[column.AspectName]));
        if (column.IsEditable && column.AspectPutter == null && !string.IsNullOrEmpty(column.AspectName))
          column.AspectPutter = (AspectPutterDelegate) ((row, newValue) =>
          {
            if (!(row is DataRowView dataRowView12))
              column.PutAspectByName(row, newValue);
            else if (dataRowView12.Row.RowState != DataRowState.Detached)
              dataRowView12[column.AspectName] = newValue;
          });
      }
    }

    protected virtual void CurrencyManagerListChanged(object sender, ListChangedEventArgs e)
    {
      if (this.ListView.Frozen)
        return;
      switch (e.ListChangedType)
      {
        case ListChangedType.Reset:
          this.HandleListChangedReset(e);
          break;
        case ListChangedType.ItemAdded:
          this.HandleListChangedItemAdded(e);
          break;
        case ListChangedType.ItemDeleted:
          this.HandleListChangedItemDeleted(e);
          break;
        case ListChangedType.ItemMoved:
          this.HandleListChangedItemMoved(e);
          break;
        case ListChangedType.ItemChanged:
          this.HandleListChangedItemChanged(e);
          break;
        case ListChangedType.PropertyDescriptorAdded:
        case ListChangedType.PropertyDescriptorDeleted:
        case ListChangedType.PropertyDescriptorChanged:
          this.HandleListChangedMetadataChanged(e);
          break;
      }
    }

    protected virtual void HandleListChangedMetadataChanged(ListChangedEventArgs e) => this.InitializeDataSource();

    protected virtual void HandleListChangedItemMoved(ListChangedEventArgs e) => this.InitializeDataSource();

    protected virtual void HandleListChangedItemDeleted(ListChangedEventArgs e) => this.InitializeDataSource();

    protected virtual void HandleListChangedItemAdded(ListChangedEventArgs e)
    {
      if (this.CurrencyManager.List[e.NewIndex] is DataRowView dataRowView && dataRowView.IsNew)
        return;
      this.InitializeDataSource();
    }

    protected virtual void HandleListChangedReset(ListChangedEventArgs e) => this.InitializeDataSource();

    protected virtual void HandleListChangedItemChanged(ListChangedEventArgs e) => this.ListView.RefreshObject(this.CurrencyManager.List[e.NewIndex]);

    protected virtual void HandleCurrencyManagerMetaDataChanged(object sender, EventArgs e) => this.InitializeDataSource();

    protected virtual void HandleCurrencyManagerPositionChanged(object sender, EventArgs e)
    {
      int position = this.CurrencyManager.Position;
      if (position < 0 || position >= this.ListView.GetItemCount())
        return;
      if (this.isChangingIndex)
        return;
      try
      {
        this.isChangingIndex = true;
        this.ChangePosition(position);
      }
      finally
      {
        this.isChangingIndex = false;
      }
    }

    protected virtual void ChangePosition(int index)
    {
      this.ListView.SelectedObject = this.CurrencyManager.List[index];
      if (this.ListView.SelectedIndices.Count <= 0)
        return;
      this.ListView.EnsureVisible(this.ListView.SelectedIndices[0]);
    }

    protected virtual void HandleListViewSelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isChangingIndex)
        return;
      if (this.ListView.SelectedIndices.Count != 1 || this.CurrencyManager == null)
        return;
      try
      {
        this.isChangingIndex = true;
        this.CurrencyManager.Position = this.CurrencyManager.List.IndexOf(this.ListView.SelectedObject);
      }
      finally
      {
        this.isChangingIndex = false;
      }
    }

    protected virtual void HandleListViewFreezing(object sender, FreezeEventArgs e)
    {
      if (this.alreadyFreezing || e.FreezeLevel != 0)
        return;
      try
      {
        this.alreadyFreezing = true;
        this.RebindDataSource(true);
      }
      finally
      {
        this.alreadyFreezing = false;
      }
    }

    protected virtual void HandleListViewBindingContextChanged(object sender, EventArgs e) => this.RebindDataSource(false);
  }
}
