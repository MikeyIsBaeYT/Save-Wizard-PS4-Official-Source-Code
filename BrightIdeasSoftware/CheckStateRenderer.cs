// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CheckStateRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class CheckStateRenderer : BaseRenderer
  {
    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.Column == null)
        return;
      r = this.ApplyCellPadding(r);
      CheckState checkState = this.Column.GetCheckState(this.RowObject);
      if (this.IsPrinting)
      {
        string key = "checkbox-checked";
        if (checkState == CheckState.Unchecked)
          key = "checkbox-unchecked";
        if (checkState == CheckState.Indeterminate)
          key = "checkbox-indeterminate";
        this.DrawAlignedImage(g, r, this.ListView.SmallImageList.Images[key]);
      }
      else
      {
        r = this.CalculateCheckBoxBounds(g, r);
        CheckBoxRenderer.DrawCheckBox(g, r.Location, this.GetCheckBoxState(checkState));
      }
    }

    protected override Rectangle HandleGetEditRectangle(
      Graphics g,
      Rectangle cellBounds,
      OLVListItem item,
      int subItemIndex,
      Size preferredSize)
    {
      return this.CalculatePaddedAlignedBounds(g, cellBounds, preferredSize);
    }

    protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      if (!this.CalculateCheckBoxBounds(g, this.Bounds).Contains(x, y))
        return;
      hti.HitTestLocation = HitTestLocation.CheckBox;
    }
  }
}
