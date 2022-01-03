// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.HyperlinkStyle
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class HyperlinkStyle : Component
  {
    private CellStyle normalStyle;
    private CellStyle overStyle;
    private CellStyle visitedStyle;
    private Cursor overCursor;

    public HyperlinkStyle()
    {
      this.Normal = new CellStyle();
      this.Normal.ForeColor = Color.Blue;
      this.Over = new CellStyle();
      this.Over.FontStyle = FontStyle.Underline;
      this.Visited = new CellStyle();
      this.Visited.ForeColor = Color.Purple;
      this.OverCursor = Cursors.Hand;
    }

    [Category("Appearance")]
    [Description("How should hyperlinks be drawn")]
    public CellStyle Normal
    {
      get => this.normalStyle;
      set => this.normalStyle = value;
    }

    [Category("Appearance")]
    [Description("How should hyperlinks be drawn when the mouse is over them?")]
    public CellStyle Over
    {
      get => this.overStyle;
      set => this.overStyle = value;
    }

    [Category("Appearance")]
    [Description("How should hyperlinks be drawn after they have been clicked")]
    public CellStyle Visited
    {
      get => this.visitedStyle;
      set => this.visitedStyle = value;
    }

    [Category("Appearance")]
    [Description("What cursor should be shown when the mouse is over a link?")]
    public Cursor OverCursor
    {
      get => this.overCursor;
      set => this.overCursor = value;
    }
  }
}
