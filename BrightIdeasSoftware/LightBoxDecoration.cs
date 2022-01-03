// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.LightBoxDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class LightBoxDecoration : BorderDecoration
  {
    public LightBoxDecoration()
    {
      this.BoundsPadding = new Size(-1, 4);
      this.CornerRounding = 8f;
      this.FillBrush = (Brush) new SolidBrush(Color.FromArgb(72, Color.Black));
    }

    public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      if (!r.Contains(olv.PointToClient(Cursor.Position)))
        return;
      Rectangle rowBounds = this.RowBounds;
      if (rowBounds.IsEmpty)
      {
        if (olv.View != View.Tile)
          return;
        g.FillRectangle(this.FillBrush, r);
      }
      else
      {
        using (Region region = new Region(r))
        {
          rowBounds.Inflate(this.BoundsPadding);
          region.Exclude(this.GetRoundedRect((RectangleF) rowBounds, this.CornerRounding));
          Region clip = g.Clip;
          g.Clip = region;
          g.FillRectangle(this.FillBrush, r);
          g.Clip = clip;
        }
      }
    }
  }
}
