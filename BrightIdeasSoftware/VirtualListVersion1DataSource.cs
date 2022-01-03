// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.VirtualListVersion1DataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class VirtualListVersion1DataSource : AbstractVirtualListDataSource
  {
    private RowGetterDelegate rowGetter;

    public VirtualListVersion1DataSource(VirtualObjectListView listView)
      : base(listView)
    {
    }

    public RowGetterDelegate RowGetter
    {
      get => this.rowGetter;
      set => this.rowGetter = value;
    }

    public override object GetNthObject(int n) => this.RowGetter == null ? (object) null : this.RowGetter(n);

    public override int SearchText(string value, int first, int last, OLVColumn column) => AbstractVirtualListDataSource.DefaultSearchText(value, first, last, column, (IVirtualListDataSource) this);
  }
}
