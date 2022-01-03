// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TextDecoration
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;

namespace BrightIdeasSoftware
{
  public class TextDecoration : TextAdornment, IDecoration, IOverlay
  {
    private OLVListItem listItem;
    private OLVListSubItem subItem;

    public TextDecoration() => this.Alignment = ContentAlignment.MiddleRight;

    public TextDecoration(string text)
      : this()
    {
      this.Text = text;
    }

    public TextDecoration(string text, int transparency)
      : this()
    {
      this.Text = text;
      this.Transparency = transparency;
    }

    public TextDecoration(string text, ContentAlignment alignment)
      : this()
    {
      this.Text = text;
      this.Alignment = alignment;
    }

    public TextDecoration(string text, int transparency, ContentAlignment alignment)
      : this()
    {
      this.Text = text;
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

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r) => this.DrawText(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
  }
}
