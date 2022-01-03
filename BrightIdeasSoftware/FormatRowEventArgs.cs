// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FormatRowEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class FormatRowEventArgs : EventArgs
  {
    private ObjectListView listView;
    private OLVListItem item;
    private int rowIndex = -1;
    private int displayIndex = -1;
    private bool useCellFormatEvents;

    public ObjectListView ListView
    {
      get => this.listView;
      internal set => this.listView = value;
    }

    public OLVListItem Item
    {
      get => this.item;
      internal set => this.item = value;
    }

    public object Model => this.Item.RowObject;

    public int RowIndex
    {
      get => this.rowIndex;
      internal set => this.rowIndex = value;
    }

    public int DisplayIndex
    {
      get => this.displayIndex;
      internal set => this.displayIndex = value;
    }

    public bool UseCellFormatEvents
    {
      get => this.useCellFormatEvents;
      set => this.useCellFormatEvents = value;
    }
  }
}
