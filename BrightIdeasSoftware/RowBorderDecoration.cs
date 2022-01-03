// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.RowBorderDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class RowBorderDecoration : BorderDecoration
  {
    private int leftColumn = -1;
    private int rightColumn = -1;

    public int LeftColumn
    {
      get => this.leftColumn;
      set => this.leftColumn = value;
    }

    public int RightColumn
    {
      get => this.rightColumn;
      set => this.rightColumn = value;
    }

    protected override Rectangle CalculateBounds()
    {
      Rectangle rowBounds = this.RowBounds;
      if (this.ListItem == null)
        return rowBounds;
      if (this.LeftColumn >= 0)
      {
        Rectangle subItemBounds = this.ListItem.GetSubItemBounds(this.LeftColumn);
        if (!subItemBounds.IsEmpty)
        {
          rowBounds.Width = rowBounds.Right - subItemBounds.Left;
          rowBounds.X = subItemBounds.Left;
        }
      }
      if (this.RightColumn >= 0)
      {
        Rectangle subItemBounds = this.ListItem.GetSubItemBounds(this.RightColumn);
        if (!subItemBounds.IsEmpty)
          rowBounds.Width = subItemBounds.Right - rowBounds.Left;
      }
      return rowBounds;
    }
  }
}
