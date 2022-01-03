// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.SimpleItemStyle
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class SimpleItemStyle : Component, IItemStyle
  {
    private Font font;
    private FontStyle fontStyle;
    private Color foreColor;
    private Color backColor;

    [DefaultValue(null)]
    public Font Font
    {
      get => this.font;
      set => this.font = value;
    }

    [DefaultValue(FontStyle.Regular)]
    public FontStyle FontStyle
    {
      get => this.fontStyle;
      set => this.fontStyle = value;
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
  }
}
