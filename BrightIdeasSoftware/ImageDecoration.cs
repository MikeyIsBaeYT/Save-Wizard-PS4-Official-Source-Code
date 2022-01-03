// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ImageDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class ImageDecoration : ImageAdornment, IDecoration, IOverlay
  {
    private OLVListItem listItem;
    private OLVListSubItem subItem;

    public ImageDecoration() => this.Alignment = ContentAlignment.MiddleRight;

    public ImageDecoration(Image image)
      : this()
    {
      this.Image = image;
    }

    public ImageDecoration(Image image, int transparency)
      : this()
    {
      this.Image = image;
      this.Transparency = transparency;
    }

    public ImageDecoration(Image image, ContentAlignment alignment)
      : this()
    {
      this.Image = image;
      this.Alignment = alignment;
    }

    public ImageDecoration(Image image, int transparency, ContentAlignment alignment)
      : this()
    {
      this.Image = image;
      this.Transparency = transparency;
      this.Alignment = alignment;
    }

    public OLVListItem ListItem
    {
      get => this.listItem;
      set => this.listItem = value;
    }

    public OLVListSubItem SubItem
    {
      get => this.subItem;
      set => this.subItem = value;
    }

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r) => this.DrawImage(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
  }
}
