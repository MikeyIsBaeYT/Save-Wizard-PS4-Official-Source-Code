// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.MappedImageRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class MappedImageRenderer : BaseRenderer
  {
    private Hashtable map;
    private object nullImage;

    public static MappedImageRenderer Boolean(
      object trueImage,
      object falseImage)
    {
      return new MappedImageRenderer((object) true, trueImage, (object) false, falseImage);
    }

    public static MappedImageRenderer TriState(
      object trueImage,
      object falseImage,
      object nullImage)
    {
      return new MappedImageRenderer(new object[6]
      {
        (object) true,
        trueImage,
        (object) false,
        falseImage,
        null,
        nullImage
      });
    }

    public MappedImageRenderer() => this.map = new Hashtable();

    public MappedImageRenderer(object key, object image)
      : this()
    {
      this.Add(key, image);
    }

    public MappedImageRenderer(object key1, object image1, object key2, object image2)
      : this()
    {
      this.Add(key1, image1);
      this.Add(key2, image2);
    }

    public MappedImageRenderer(object[] keysAndImages)
      : this()
    {
      if ((uint) (keysAndImages.GetLength(0) % 2) > 0U)
        throw new ArgumentException("Array must have key/image pairs");
      for (int index = 0; index < keysAndImages.GetLength(0); index += 2)
        this.Add(keysAndImages[index], keysAndImages[index + 1]);
    }

    public void Add(object value, object image)
    {
      if (value == null)
        this.nullImage = image;
      else
        this.map[value] = image;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      if (!(this.Aspect is ICollection aspect))
        this.RenderOne(g, r, this.Aspect);
      else
        this.RenderCollection(g, r, aspect);
    }

    protected void RenderCollection(Graphics g, Rectangle r, ICollection imageSelectors)
    {
      ArrayList arrayList = new ArrayList();
      foreach (object imageSelector in (IEnumerable) imageSelectors)
      {
        Image image = imageSelector != null ? (!this.map.ContainsKey(imageSelector) ? (Image) null : this.GetImage(this.map[imageSelector])) : this.GetImage(this.nullImage);
        if (image != null)
          arrayList.Add((object) image);
      }
      this.DrawImages(g, r, (ICollection) arrayList);
    }

    protected void RenderOne(Graphics g, Rectangle r, object selector)
    {
      Image image = (Image) null;
      if (selector == null)
        image = this.GetImage(this.nullImage);
      else if (this.map.ContainsKey(selector))
        image = this.GetImage(this.map[selector]);
      if (image == null)
        return;
      this.DrawAlignedImage(g, r, image);
    }
  }
}
