// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BarRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class BarRenderer : BaseRenderer
  {
    private bool useStandardBar = true;
    private int padding = 2;
    private Color backgroundColor = Color.AliceBlue;
    private Color frameColor = Color.Black;
    private float frameWidth = 1f;
    private Color fillColor = Color.BlueViolet;
    private Color startColor = Color.CornflowerBlue;
    private Color endColor = Color.DarkBlue;
    private int maximumWidth = 100;
    private int maximumHeight = 16;
    private double minimumValue = 0.0;
    private double maximumValue = 100.0;
    private Pen pen;
    private Brush brush;
    private Brush backgroundBrush;

    public BarRenderer()
    {
    }

    public BarRenderer(int minimum, int maximum)
      : this()
    {
      this.MinimumValue = (double) minimum;
      this.MaximumValue = (double) maximum;
    }

    public BarRenderer(Pen pen, Brush brush)
      : this()
    {
      this.Pen = pen;
      this.Brush = brush;
      this.UseStandardBar = false;
    }

    public BarRenderer(int minimum, int maximum, Pen pen, Brush brush)
      : this(minimum, maximum)
    {
      this.Pen = pen;
      this.Brush = brush;
      this.UseStandardBar = false;
    }

    public BarRenderer(Pen pen, Color start, Color end)
      : this()
    {
      this.Pen = pen;
      this.SetGradient(start, end);
    }

    public BarRenderer(int minimum, int maximum, Pen pen, Color start, Color end)
      : this(minimum, maximum)
    {
      this.Pen = pen;
      this.SetGradient(start, end);
    }

    [Category("ObjectListView")]
    [Description("Should this bar be drawn in the system style?")]
    [DefaultValue(true)]
    public bool UseStandardBar
    {
      get => this.useStandardBar;
      set => this.useStandardBar = value;
    }

    [Category("ObjectListView")]
    [Description("How many pixels in from our cell border will this bar be drawn")]
    [DefaultValue(2)]
    public int Padding
    {
      get => this.padding;
      set => this.padding = value;
    }

    [Category("ObjectListView")]
    [Description("The color of the interior of the bar")]
    [DefaultValue(typeof (Color), "AliceBlue")]
    public Color BackgroundColor
    {
      get => this.backgroundColor;
      set => this.backgroundColor = value;
    }

    [Category("ObjectListView")]
    [Description("What color should the frame of the progress bar be")]
    [DefaultValue(typeof (Color), "Black")]
    public Color FrameColor
    {
      get => this.frameColor;
      set => this.frameColor = value;
    }

    [Category("ObjectListView")]
    [Description("How many pixels wide should the frame of the progress bar be")]
    [DefaultValue(1f)]
    public float FrameWidth
    {
      get => this.frameWidth;
      set => this.frameWidth = value;
    }

    [Category("ObjectListView")]
    [Description("What color should the 'filled in' part of the progress bar be")]
    [DefaultValue(typeof (Color), "BlueViolet")]
    public Color FillColor
    {
      get => this.fillColor;
      set => this.fillColor = value;
    }

    [Category("ObjectListView")]
    [Description("Use a gradient to fill the progress bar starting with this color")]
    [DefaultValue(typeof (Color), "CornflowerBlue")]
    public Color GradientStartColor
    {
      get => this.startColor;
      set => this.startColor = value;
    }

    [Category("ObjectListView")]
    [Description("Use a gradient to fill the progress bar ending with this color")]
    [DefaultValue(typeof (Color), "DarkBlue")]
    public Color GradientEndColor
    {
      get => this.endColor;
      set => this.endColor = value;
    }

    [Category("Behavior")]
    [Description("The progress bar will never be wider than this")]
    [DefaultValue(100)]
    public int MaximumWidth
    {
      get => this.maximumWidth;
      set => this.maximumWidth = value;
    }

    [Category("Behavior")]
    [Description("The progress bar will never be taller than this")]
    [DefaultValue(16)]
    public int MaximumHeight
    {
      get => this.maximumHeight;
      set => this.maximumHeight = value;
    }

    [Category("Behavior")]
    [Description("The minimum data value expected. Values less than this will given an empty bar")]
    [DefaultValue(0.0)]
    public double MinimumValue
    {
      get => this.minimumValue;
      set => this.minimumValue = value;
    }

    [Category("Behavior")]
    [Description("The maximum value for the range. Values greater than this will give a full bar")]
    [DefaultValue(100.0)]
    public double MaximumValue
    {
      get => this.maximumValue;
      set => this.maximumValue = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Pen Pen
    {
      get => this.pen == null && !this.FrameColor.IsEmpty ? new Pen(this.FrameColor, this.FrameWidth) : this.pen;
      set => this.pen = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Brush Brush
    {
      get => this.brush == null && !this.FillColor.IsEmpty ? (Brush) new SolidBrush(this.FillColor) : this.brush;
      set => this.brush = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Brush BackgroundBrush
    {
      get => this.backgroundBrush == null && !this.BackgroundColor.IsEmpty ? (Brush) new SolidBrush(this.BackgroundColor) : this.backgroundBrush;
      set => this.backgroundBrush = value;
    }

    public void SetGradient(Color start, Color end)
    {
      this.GradientStartColor = start;
      this.GradientEndColor = end;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      Rectangle inner = Rectangle.Inflate(r, -this.Padding, -this.Padding);
      inner.Width = Math.Min(inner.Width, this.MaximumWidth);
      inner.Height = Math.Min(inner.Height, this.MaximumHeight);
      Rectangle rectangle1 = this.AlignRectangle(r, inner);
      if (!(this.Aspect is IConvertible aspect))
        return;
      double num = aspect.ToDouble((IFormatProvider) NumberFormatInfo.InvariantInfo);
      Rectangle rectangle2 = Rectangle.Inflate(rectangle1, -1, -1);
      if (num <= this.MinimumValue)
        rectangle2.Width = 0;
      else if (num < this.MaximumValue)
        rectangle2.Width = (int) ((double) rectangle2.Width * (num - this.MinimumValue) / this.MaximumValue);
      if (this.UseStandardBar && ProgressBarRenderer.IsSupported && !this.IsPrinting)
      {
        ProgressBarRenderer.DrawHorizontalBar(g, rectangle1);
        ProgressBarRenderer.DrawHorizontalChunks(g, rectangle2);
      }
      else
      {
        g.FillRectangle(this.BackgroundBrush, rectangle1);
        if (rectangle2.Width > 0)
        {
          ++rectangle2.Width;
          ++rectangle2.Height;
          if (this.GradientStartColor == Color.Empty)
          {
            g.FillRectangle(this.Brush, rectangle2);
          }
          else
          {
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle1, this.GradientStartColor, this.GradientEndColor, LinearGradientMode.Horizontal))
              g.FillRectangle((Brush) linearGradientBrush, rectangle2);
          }
        }
        g.DrawRectangle(this.Pen, rectangle1);
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
  }
}
