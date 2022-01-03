// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.Version1Renderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  internal class Version1Renderer : AbstractRenderer
  {
    public RenderDelegate RenderDelegate;

    public Version1Renderer(RenderDelegate renderDelegate) => this.RenderDelegate = renderDelegate;

    public override bool RenderSubItem(
      DrawListViewSubItemEventArgs e,
      Graphics g,
      Rectangle cellBounds,
      object rowObject)
    {
      return this.RenderDelegate == null ? base.RenderSubItem(e, g, cellBounds, rowObject) : this.RenderDelegate((EventArgs) e, g, cellBounds, rowObject);
    }
  }
}
