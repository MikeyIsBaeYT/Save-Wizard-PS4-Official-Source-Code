// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DescribedTaskRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class DescribedTaskRenderer : BaseRenderer
  {
    private Font titleFont;
    private Color titleColor;
    private Font descriptionFont;
    private Color descriptionColor = Color.DimGray;
    private int imageTextSpace = 4;
    private string descriptionAspectName;
    private Munger descriptionGetter;

    [Category("ObjectListView")]
    [Description("The font that will be used to draw the title of the task")]
    [DefaultValue(null)]
    public Font TitleFont
    {
      get => this.titleFont;
      set => this.titleFont = value;
    }

    [Browsable(false)]
    public Font TitleFontOrDefault => this.TitleFont ?? this.ListView.Font;

    [Category("ObjectListView")]
    [Description("The color of the title")]
    [DefaultValue(typeof (Color), "")]
    public Color TitleColor
    {
      get => this.titleColor;
      set => this.titleColor = value;
    }

    [Browsable(false)]
    public Color TitleColorOrDefault => this.IsItemSelected || this.TitleColor.IsEmpty ? this.GetForegroundColor() : this.TitleColor;

    [Category("ObjectListView")]
    [Description("The font that will be used to draw the description of the task")]
    [DefaultValue(null)]
    public Font DescriptionFont
    {
      get => this.descriptionFont;
      set => this.descriptionFont = value;
    }

    [Browsable(false)]
    public Font DescriptionFontOrDefault => this.DescriptionFont ?? this.ListView.Font;

    [Category("ObjectListView")]
    [Description("The color of the description")]
    [DefaultValue(typeof (Color), "DimGray")]
    public Color DescriptionColor
    {
      get => this.descriptionColor;
      set => this.descriptionColor = value;
    }

    [Browsable(false)]
    public Color DescriptionColorOrDefault => this.DescriptionColor.IsEmpty || this.IsItemSelected && !this.ListView.UseTranslucentSelection ? this.GetForegroundColor() : this.DescriptionColor;

    [Category("ObjectListView")]
    [Description("The number of pixels that that will be left between the image and the text")]
    [DefaultValue(4)]
    public int ImageTextSpace
    {
      get => this.imageTextSpace;
      set => this.imageTextSpace = value;
    }

    [Category("ObjectListView")]
    [Description("The name of the aspect of the model object that contains the task description")]
    [DefaultValue(null)]
    public string DescriptionAspectName
    {
      get => this.descriptionAspectName;
      set => this.descriptionAspectName = value;
    }

    protected virtual string GetDescription()
    {
      if (string.IsNullOrEmpty(this.DescriptionAspectName))
        return string.Empty;
      if (this.descriptionGetter == null)
        this.descriptionGetter = new Munger(this.DescriptionAspectName);
      return this.descriptionGetter.GetValue(this.RowObject) as string;
    }

    public override void Render(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      r = this.ApplyCellPadding(r);
      this.DrawDescribedTask(g, r, this.Aspect as string, this.GetDescription(), this.GetImage());
    }

    protected virtual void DrawDescribedTask(
      Graphics g,
      Rectangle r,
      string title,
      string description,
      Image image)
    {
      Rectangle rectangle = this.ApplyCellPadding(r);
      Rectangle rect = rectangle;
      if (image != null)
      {
        g.DrawImage(image, rectangle.Location);
        int num = image.Width + this.ImageTextSpace;
        rect.X += num;
        rect.Width -= num;
      }
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection)
      {
        using (SolidBrush solidBrush = new SolidBrush(this.GetTextBackgroundColor()))
          g.FillRectangle((Brush) solidBrush, rect);
      }
      if (!string.IsNullOrEmpty(title))
      {
        using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap))
        {
          format.Trimming = StringTrimming.EllipsisCharacter;
          format.Alignment = StringAlignment.Near;
          format.LineAlignment = StringAlignment.Near;
          Font titleFontOrDefault = this.TitleFontOrDefault;
          using (SolidBrush solidBrush = new SolidBrush(this.TitleColorOrDefault))
            g.DrawString(title, titleFontOrDefault, (Brush) solidBrush, (RectangleF) rect, format);
          SizeF sizeF = g.MeasureString(title, titleFontOrDefault, rect.Width, format);
          rect.Y += (int) sizeF.Height;
          rect.Height -= (int) sizeF.Height;
        }
      }
      if (string.IsNullOrEmpty(description))
        return;
      using (StringFormat format = new StringFormat())
      {
        format.Trimming = StringTrimming.EllipsisCharacter;
        using (SolidBrush solidBrush = new SolidBrush(this.DescriptionColorOrDefault))
          g.DrawString(description, this.DescriptionFontOrDefault, (Brush) solidBrush, (RectangleF) rect, format);
      }
    }

    protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      if (!this.Bounds.Contains(x, y))
        return;
      hti.HitTestLocation = HitTestLocation.Text;
    }
  }
}
