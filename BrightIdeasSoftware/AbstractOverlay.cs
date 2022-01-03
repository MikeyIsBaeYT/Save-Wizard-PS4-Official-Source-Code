// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractOverlay
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;

namespace BrightIdeasSoftware
{
  public class AbstractOverlay : ITransparentOverlay, IOverlay
  {
    private int transparency = 128;

    public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r)
    {
    }

    [Category("ObjectListView")]
    [Description("How transparent should this overlay be")]
    [DefaultValue(128)]
    [NotifyParentProperty(true)]
    public int Transparency
    {
      get => this.transparency;
      set => this.transparency = Math.Min((int) byte.MaxValue, Math.Max(0, value));
    }
  }
}
