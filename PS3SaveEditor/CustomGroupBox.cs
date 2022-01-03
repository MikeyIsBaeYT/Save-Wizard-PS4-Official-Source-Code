// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CustomGroupBox
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Drawing;
using System.Windows.Forms;

namespace PS3SaveEditor
{
  public class CustomGroupBox : GroupBox
  {
    protected override void OnPaint(PaintEventArgs e)
    {
      int num = !Util.IsUnixOrMacOSX() ? this.ClientRectangle.Height - 6 : this.ClientRectangle.Height - 5;
      Graphics graphics = e.Graphics;
      Pen white = Pens.White;
      Rectangle clientRectangle = this.ClientRectangle;
      int left = clientRectangle.Left;
      clientRectangle = this.ClientRectangle;
      int y = clientRectangle.Top + 4;
      clientRectangle = this.ClientRectangle;
      int width = clientRectangle.Width - 1;
      int height = num;
      Rectangle rect = new Rectangle(left, y, width, height);
      graphics.DrawRectangle(white, rect);
    }
  }
}
