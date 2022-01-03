// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.HeaderStateStyle
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  [TypeConverter(typeof (ExpandableObjectConverter))]
  public class HeaderStateStyle
  {
    private Font font;
    private Color foreColor;
    private Color backColor;
    private Color frameColor;
    private float frameWidth;

    [DefaultValue(null)]
    public Font Font
    {
      get => this.font;
      set => this.font = value;
    }

    [DefaultValue(typeof (Color), "")]
    public Color ForeColor
    {
      get => this.foreColor;
      set => this.foreColor = value;
    }

    [DefaultValue(typeof (Color), "")]
    public Color BackColor
    {
      get => this.backColor;
      set => this.backColor = value;
    }

    [DefaultValue(typeof (Color), "")]
    public Color FrameColor
    {
      get => this.frameColor;
      set => this.frameColor = value;
    }

    [DefaultValue(0.0f)]
    public float FrameWidth
    {
      get => this.frameWidth;
      set => this.frameWidth = value;
    }
  }
}
