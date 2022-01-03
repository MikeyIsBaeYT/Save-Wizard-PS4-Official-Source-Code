// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BooleanCellEditor2
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class BooleanCellEditor2 : CheckBox
  {
    public bool? Value
    {
      get
      {
        switch (this.CheckState)
        {
          case CheckState.Checked:
            return new bool?(true);
          case CheckState.Indeterminate:
            return new bool?();
          default:
            return new bool?(false);
        }
      }
      set
      {
        if (value.HasValue)
          this.CheckState = value.Value ? CheckState.Checked : CheckState.Unchecked;
        else
          this.CheckState = CheckState.Indeterminate;
      }
    }

    public HorizontalAlignment TextAlign
    {
      get
      {
        switch (this.CheckAlign)
        {
          case ContentAlignment.MiddleCenter:
            return HorizontalAlignment.Center;
          case ContentAlignment.MiddleRight:
            return HorizontalAlignment.Right;
          default:
            return HorizontalAlignment.Left;
        }
      }
      set
      {
        switch (value)
        {
          case HorizontalAlignment.Left:
            this.CheckAlign = ContentAlignment.MiddleLeft;
            break;
          case HorizontalAlignment.Right:
            this.CheckAlign = ContentAlignment.MiddleRight;
            break;
          case HorizontalAlignment.Center:
            this.CheckAlign = ContentAlignment.MiddleCenter;
            break;
        }
      }
    }
  }
}
