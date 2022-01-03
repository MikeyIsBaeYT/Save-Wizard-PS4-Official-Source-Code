// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ToolTipShowingEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ToolTipShowingEventArgs : CellEventArgs
  {
    private ToolTipControl toolTipControl;
    public string Text;
    public RightToLeft RightToLeft;
    public bool? IsBalloon;
    public Color? BackColor;
    public Color? ForeColor;
    public string Title;
    public ToolTipControl.StandardIcons? StandardIcon;
    public int? AutoPopDelay;
    public Font Font;

    public ToolTipControl ToolTipControl
    {
      get => this.toolTipControl;
      internal set => this.toolTipControl = value;
    }
  }
}
