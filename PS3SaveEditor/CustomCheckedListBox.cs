// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CustomCheckedListBox
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PS3SaveEditor
{
  public class CustomCheckedListBox : CheckedListBox
  {
    private IContainer components = (IContainer) null;

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      if (e.Index < 0)
        return;
      e.DrawBackground();
      if (e.State == DrawItemState.Selected)
        e.Graphics.FillRectangle((Brush) new SolidBrush(Color.FromArgb(0, 175, (int) byte.MaxValue)), e.Bounds);
      string str = this.Items[e.Index].ToString();
      CheckBoxState state = this.GetItemChecked(e.Index) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
      Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
      Graphics graphics = e.Graphics;
      Rectangle bounds = e.Bounds;
      Point location1 = bounds.Location;
      bounds = e.Bounds;
      int x = bounds.X + glyphSize.Width;
      bounds = e.Bounds;
      int y = bounds.Y;
      Point location2 = new Point(x, y);
      bounds = e.Bounds;
      int width = bounds.Width - glyphSize.Width;
      bounds = e.Bounds;
      int height = bounds.Height;
      Size size = new Size(width, height);
      Rectangle textBounds = new Rectangle(location2, size);
      string checkBoxText = str;
      Font font = this.Font;
      int num = (int) state;
      CheckBoxRenderer.DrawCheckBox(graphics, location1, textBounds, checkBoxText, font, false, (CheckBoxState) num);
      e.DrawFocusRectangle();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() => this.components = (IContainer) new Container();
  }
}
