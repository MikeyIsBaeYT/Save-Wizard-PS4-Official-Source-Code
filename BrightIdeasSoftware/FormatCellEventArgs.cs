// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FormatCellEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class FormatCellEventArgs : FormatRowEventArgs
  {
    private int columnIndex = -1;
    private OLVColumn column;
    private OLVListSubItem subItem;

    public int ColumnIndex
    {
      get => this.columnIndex;
      internal set => this.columnIndex = value;
    }

    public OLVColumn Column
    {
      get => this.column;
      internal set => this.column = value;
    }

    public OLVListSubItem SubItem
    {
      get => this.subItem;
      internal set => this.subItem = value;
    }

    public object CellValue => this.SubItem == null ? (object) null : this.SubItem.ModelValue;
  }
}
