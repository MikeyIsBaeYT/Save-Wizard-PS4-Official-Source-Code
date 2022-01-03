// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CustomTableLayoutPanel
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class CustomTableLayoutPanel : TableLayoutPanel
  {
    private IContainer components = (IContainer) null;

    public CustomTableLayoutPanel() => this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }
  }
}
