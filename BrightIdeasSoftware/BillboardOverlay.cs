// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BillboardOverlay
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class BillboardOverlay : TextOverlay
  {
    private Point location;

    public BillboardOverlay()
    {
      this.Transparency = (int) byte.MaxValue;
      this.BackColor = Color.PeachPuff;
      this.TextColor = Color.Black;
      this.BorderColor = Color.Empty;
      this.Font = new Font("Tahoma", 10f);
    }

    public Point Location
    {
      get => this.location;
      set => this.location = value;
    }

    public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      if (string.IsNullOrEmpty(this.Text))
        return;
      Rectangle textBounds = this.CalculateTextBounds(g, r, this.Text);
      textBounds.Location = this.Location;
      if (textBounds.Right > r.Width)
        textBounds.X = Math.Max(r.Left, r.Width - textBounds.Width);
      if (textBounds.Bottom > r.Height)
        textBounds.Y = Math.Max(r.Top, r.Height - textBounds.Height);
      this.DrawBorderedText(g, textBounds, this.Text, (int) byte.MaxValue);
    }
  }
}
