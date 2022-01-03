// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.HeaderFormatStyle
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class HeaderFormatStyle : Component
  {
    private HeaderStateStyle hotStyle;
    private HeaderStateStyle normalStyle;
    private HeaderStateStyle pressedStyle;

    public HeaderFormatStyle()
    {
      this.Hot = new HeaderStateStyle();
      this.Normal = new HeaderStateStyle();
      this.Pressed = new HeaderStateStyle();
    }

    [Category("Appearance")]
    [Description("How should the header be drawn when the mouse is over it?")]
    public HeaderStateStyle Hot
    {
      get => this.hotStyle;
      set => this.hotStyle = value;
    }

    [Category("Appearance")]
    [Description("How should a column header normally be drawn")]
    public HeaderStateStyle Normal
    {
      get => this.normalStyle;
      set => this.normalStyle = value;
    }

    [Category("Appearance")]
    [Description("How should a column header be drawn when it is pressed")]
    public HeaderStateStyle Pressed
    {
      get => this.pressedStyle;
      set => this.pressedStyle = value;
    }

    public void SetFont(Font font)
    {
      this.Normal.Font = font;
      this.Hot.Font = font;
      this.Pressed.Font = font;
    }

    public void SetForeColor(Color color)
    {
      this.Normal.ForeColor = color;
      this.Hot.ForeColor = color;
      this.Pressed.ForeColor = color;
    }

    public void SetBackColor(Color color)
    {
      this.Normal.BackColor = color;
      this.Hot.BackColor = color;
      this.Pressed.BackColor = color;
    }
  }
}
