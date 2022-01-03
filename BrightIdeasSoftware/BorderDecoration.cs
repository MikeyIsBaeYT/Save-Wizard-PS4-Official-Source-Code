// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BorderDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Drawing.Drawing2D;

namespace BrightIdeasSoftware
{
  public class BorderDecoration : AbstractDecoration
  {
    private Pen borderPen;
    private Size boundsPadding = new Size(-1, 2);
    private float cornerRounding = 16f;
    private Brush fillBrush = (Brush) new SolidBrush(Color.FromArgb(64, Color.Blue));
    private Color? fillGradientFrom;
    private Color? fillGradientTo;
    private LinearGradientMode fillGradientMode = LinearGradientMode.Vertical;

    public BorderDecoration()
      : this(new Pen(Color.FromArgb(64, Color.Blue), 1f))
    {
    }

    public BorderDecoration(Pen borderPen) => this.BorderPen = borderPen;

    public BorderDecoration(Pen borderPen, Brush fill)
    {
      this.BorderPen = borderPen;
      this.FillBrush = fill;
    }

    public Pen BorderPen
    {
      get => this.borderPen;
      set => this.borderPen = value;
    }

    public Size BoundsPadding
    {
      get => this.boundsPadding;
      set => this.boundsPadding = value;
    }

    public float CornerRounding
    {
      get => this.cornerRounding;
      set => this.cornerRounding = value;
    }

    public Brush FillBrush
    {
      get => this.fillBrush;
      set => this.fillBrush = value;
    }

    public Color? FillGradientFrom
    {
      get => this.fillGradientFrom;
      set => this.fillGradientFrom = value;
    }

    public Color? FillGradientTo
    {
      get => this.fillGradientTo;
      set => this.fillGradientTo = value;
    }

    public LinearGradientMode FillGradientMode
    {
      get => this.fillGradientMode;
      set => this.fillGradientMode = value;
    }

    public override void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
      Rectangle bounds = this.CalculateBounds();
      if (bounds.IsEmpty)
        return;
      this.DrawFilledBorder(g, bounds);
    }

    protected virtual Rectangle CalculateBounds() => Rectangle.Empty;

    protected void DrawFilledBorder(Graphics g, Rectangle bounds)
    {
      bounds.Inflate(this.BoundsPadding);
      GraphicsPath roundedRect = this.GetRoundedRect((RectangleF) bounds, this.CornerRounding);
      if (this.FillGradientFrom.HasValue && this.FillGradientTo.HasValue)
      {
        if (this.FillBrush != null)
          this.FillBrush.Dispose();
        Rectangle rect = bounds;
        Color? nullable = this.FillGradientFrom;
        Color color1 = nullable.Value;
        nullable = this.FillGradientTo;
        Color color2 = nullable.Value;
        int fillGradientMode = (int) this.FillGradientMode;
        this.FillBrush = (Brush) new LinearGradientBrush(rect, color1, color2, (LinearGradientMode) fillGradientMode);
      }
      if (this.FillBrush != null)
        g.FillPath(this.FillBrush, roundedRect);
      if (this.BorderPen == null)
        return;
      g.DrawPath(this.BorderPen, roundedRect);
    }

    protected GraphicsPath GetRoundedRect(RectangleF rect, float diameter)
    {
      GraphicsPath graphicsPath = new GraphicsPath();
      if ((double) diameter <= 0.0)
      {
        graphicsPath.AddRectangle(rect);
      }
      else
      {
        RectangleF rect1 = new RectangleF(rect.X, rect.Y, diameter, diameter);
        graphicsPath.AddArc(rect1, 180f, 90f);
        rect1.X = rect.Right - diameter;
        graphicsPath.AddArc(rect1, 270f, 90f);
        rect1.Y = rect.Bottom - diameter;
        graphicsPath.AddArc(rect1, 0.0f, 90f);
        rect1.X = rect.Left;
        graphicsPath.AddArc(rect1, 90f, 90f);
        graphicsPath.CloseFigure();
      }
      return graphicsPath;
    }
  }
}
