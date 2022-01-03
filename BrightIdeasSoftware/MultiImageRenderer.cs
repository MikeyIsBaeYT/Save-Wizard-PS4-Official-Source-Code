// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.MultiImageRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class MultiImageRenderer : BaseRenderer
  {
    private object imageSelector;
    private int maxNumberImages = 10;
    private int minimumValue = 0;
    private int maximumValue = 100;

    public MultiImageRenderer()
    {
    }

    public MultiImageRenderer(object imageSelector, int maxImages, int minValue, int maxValue)
      : this()
    {
      this.ImageSelector = imageSelector;
      this.MaxNumberImages = maxImages;
      this.MinimumValue = minValue;
      this.MaximumValue = maxValue;
    }

    [Category("Behavior")]
    [Description("The index of the image that should be drawn")]
    [DefaultValue(-1)]
    public int ImageIndex
    {
      get => this.imageSelector is int ? (int) this.imageSelector : -1;
      set => this.imageSelector = (object) value;
    }

    [Category("Behavior")]
    [Description("The index of the image that should be drawn")]
    [DefaultValue(null)]
    public string ImageName
    {
      get => this.imageSelector as string;
      set => this.imageSelector = (object) value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object ImageSelector
    {
      get => this.imageSelector;
      set => this.imageSelector = value;
    }

    [Category("Behavior")]
    [Description("The maximum number of images that this renderer should draw")]
    [DefaultValue(10)]
    public int MaxNumberImages
    {
      get => this.maxNumberImages;
      set => this.maxNumberImages = value;
    }

    [Category("Behavior")]
    [Description("Values less than or equal to this will have 0 images drawn")]
    [DefaultValue(0)]
    public int MinimumValue
    {
      get => this.minimumValue;
      set => this.minimumValue = value;
    }

    [Category("Behavior")]
    [Description("Values greater than or equal to this will have MaxNumberImages images drawn")]
    [DefaultValue(100)]
    public int MaximumValue
    {
      get => this.maximumValue;
      set => this.maximumValue = value;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      Image image = this.GetImage(this.ImageSelector);
      if (image == null || !(this.Aspect is IConvertible aspect))
        return;
      double num1 = aspect.ToDouble((IFormatProvider) NumberFormatInfo.InvariantInfo);
      int num2 = num1 > (double) this.MinimumValue ? (num1 >= (double) this.MaximumValue ? this.MaxNumberImages : 1 + (int) ((double) this.MaxNumberImages * (num1 - (double) this.MinimumValue) / (double) this.MaximumValue)) : 0;
      int width = image.Width;
      int height = image.Height;
      if (r.Height < image.Height)
      {
        width = (int) ((double) image.Width * (double) r.Height / (double) image.Height);
        height = r.Height;
      }
      Rectangle inner = r;
      inner.Width = this.MaxNumberImages * (width + this.Spacing) - this.Spacing;
      inner.Height = height;
      Rectangle rectangle = this.AlignRectangle(r, inner);
      Color backgroundColor = this.GetBackgroundColor();
      for (int index = 0; index < num2; ++index)
      {
        if (this.ListItem.Enabled)
          g.DrawImage(image, rectangle.X, rectangle.Y, width, height);
        else
          ControlPaint.DrawImageDisabled(g, image, rectangle.X, rectangle.Y, backgroundColor);
        rectangle.X += width + this.Spacing;
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
