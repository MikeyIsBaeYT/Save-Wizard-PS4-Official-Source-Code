// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ImageAdornment
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace BrightIdeasSoftware
{
  public class ImageAdornment : GraphicAdornment
  {
    private Image image;
    private bool shrinkToWidth;

    [Category("ObjectListView")]
    [Description("The image that will be drawn")]
    [DefaultValue(null)]
    [NotifyParentProperty(true)]
    public Image Image
    {
      get => this.image;
      set => this.image = value;
    }

    [Category("ObjectListView")]
    [Description("Will the image be shrunk to fit within its width?")]
    [DefaultValue(false)]
    public bool ShrinkToWidth
    {
      get => this.shrinkToWidth;
      set => this.shrinkToWidth = value;
    }

    public virtual void DrawImage(Graphics g, Rectangle r)
    {
      if (this.ShrinkToWidth)
        this.DrawScaledImage(g, r, this.Image, this.Transparency);
      else
        this.DrawImage(g, r, this.Image, this.Transparency);
    }

    public virtual void DrawImage(Graphics g, Rectangle r, Image image, int transparency)
    {
      if (image == null)
        return;
      this.DrawImage(g, r, image, image.Size, transparency);
    }

    public virtual void DrawImage(
      Graphics g,
      Rectangle r,
      Image image,
      Size sz,
      int transparency)
    {
      if (image == null)
        return;
      Rectangle alignedRectangle = this.CreateAlignedRectangle(r, sz);
      try
      {
        this.ApplyRotation(g, alignedRectangle);
        this.DrawTransparentBitmap(g, alignedRectangle, image, transparency);
      }
      finally
      {
        this.UnapplyRotation(g);
      }
    }

    public virtual void DrawScaledImage(Graphics g, Rectangle r, Image image, int transparency)
    {
      if (image == null)
        return;
      Size size = image.Size;
      if (image.Width > r.Width)
      {
        float num = (float) r.Width / (float) image.Width;
        size.Height = (int) ((double) image.Height * (double) num);
        size.Width = r.Width - 1;
      }
      this.DrawImage(g, r, image, size, transparency);
    }

    protected virtual void DrawTransparentBitmap(
      Graphics g,
      Rectangle r,
      Image image,
      int transparency)
    {
      ImageAttributes imageAttributes = (ImageAttributes) null;
      if (transparency != (int) byte.MaxValue)
      {
        imageAttributes = new ImageAttributes();
        float[][] newColorMatrix = new float[5][]
        {
          new float[5]{ 1f, 0.0f, 0.0f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 1f, 0.0f, 0.0f, 0.0f },
          new float[5]{ 0.0f, 0.0f, 1f, 0.0f, 0.0f },
          new float[5]
          {
            0.0f,
            0.0f,
            0.0f,
            (float) transparency / (float) byte.MaxValue,
            0.0f
          },
          new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1f }
        };
        imageAttributes.SetColorMatrix(new ColorMatrix(newColorMatrix));
      }
      Graphics graphics = g;
      Image image1 = image;
      Rectangle destRect = r;
      Size size = image.Size;
      int width = size.Width;
      size = image.Size;
      int height = size.Height;
      ImageAttributes imageAttr = imageAttributes;
      graphics.DrawImage(image1, destRect, 0, 0, width, height, GraphicsUnit.Pixel, imageAttr);
    }
  }
}
