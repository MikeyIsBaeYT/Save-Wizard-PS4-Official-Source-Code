// Decompiled with JetBrains decompiler
// Type: CSUST.Data.CustomDataGridView
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CSUST.Data
{
  public class CustomDataGridView : DataGridView
  {
    private Pen borderPen;
    private Brush brSelection;

    public CustomDataGridView()
    {
      this.brSelection = (Brush) new SolidBrush(Color.FromArgb(0, 175, (int) byte.MaxValue));
      this.borderPen = new Pen(Color.FromArgb(168, 173, 179), 1f);
    }

    [Description("Set cell background color, Colindex -1 denotes any col.")]
    public event EventHandler<CellBackColorEventArgs> SetCellBackColor;

    private void DrawCellBackColor(DataGridViewCellPaintingEventArgs e)
    {
      if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
        base.OnCellPainting(e);
      else if (this.SetCellBackColor == null)
      {
        base.OnCellPainting(e);
      }
      else
      {
        CellBackColorEventArgs e1 = new CellBackColorEventArgs(e.RowIndex, e.ColumnIndex);
        this.SetCellBackColor((object) this, e1);
        if (e1.BackColor == Color.Empty)
        {
          base.OnCellPainting(e);
        }
        else
        {
          using (SolidBrush solidBrush = new SolidBrush(e1.BackColor))
          {
            using (Pen pen = new Pen(this.GridColor))
            {
              Rectangle rect1;
              ref Rectangle local1 = ref rect1;
              Point location1 = e.CellBounds.Location;
              Rectangle cellBounds = e.CellBounds;
              Size size1 = cellBounds.Size;
              local1 = new Rectangle(location1, size1);
              Rectangle rect2;
              ref Rectangle local2 = ref rect2;
              cellBounds = e.CellBounds;
              Point location2 = cellBounds.Location;
              cellBounds = e.CellBounds;
              Size size2 = cellBounds.Size;
              local2 = new Rectangle(location2, size2);
              --rect1.X;
              --rect1.Y;
              --rect2.Width;
              --rect2.Height;
              e.Graphics.DrawRectangle(pen, rect1);
              e.Graphics.FillRectangle((Brush) solidBrush, rect2);
            }
          }
          e.PaintContent(e.CellBounds);
          e.Handled = true;
        }
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.DrawRectangle(this.borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }

    protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
    {
      if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
      {
        if (this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null && (this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "GameFile" || this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "CheatGroup" || this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "NoCheats"))
        {
          if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
          {
            e.Graphics.FillRectangle(this.brSelection, e.CellBounds);
            Graphics graphics1 = e.Graphics;
            Pen gray1 = Pens.Gray;
            Rectangle cellBounds = e.CellBounds;
            int left1 = cellBounds.Left;
            cellBounds = e.CellBounds;
            int top1 = cellBounds.Top;
            cellBounds = e.CellBounds;
            int right1 = cellBounds.Right;
            cellBounds = e.CellBounds;
            int top2 = cellBounds.Top;
            graphics1.DrawLine(gray1, left1, top1, right1, top2);
            Graphics graphics2 = e.Graphics;
            Pen gray2 = Pens.Gray;
            cellBounds = e.CellBounds;
            int left2 = cellBounds.Left;
            cellBounds = e.CellBounds;
            int bottom1 = cellBounds.Bottom;
            cellBounds = e.CellBounds;
            int right2 = cellBounds.Right;
            cellBounds = e.CellBounds;
            int bottom2 = cellBounds.Bottom;
            graphics2.DrawLine(gray2, left2, bottom1, right2, bottom2);
            e.Handled = true;
          }
          else
          {
            if (this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "NoCheats")
            {
              e.Graphics.DrawRectangle(Pens.White, new Rectangle(e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Width, e.CellBounds.Height - 2));
              Graphics graphics = e.Graphics;
              Brush white = Brushes.White;
              Rectangle cellBounds = e.CellBounds;
              int left = cellBounds.Left;
              cellBounds = e.CellBounds;
              int y = cellBounds.Top + 1;
              cellBounds = e.CellBounds;
              int width = cellBounds.Width;
              cellBounds = e.CellBounds;
              int height = cellBounds.Height - 2;
              Rectangle rect = new Rectangle(left, y, width, height);
              graphics.FillRectangle(white, rect);
            }
            else if (this.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString() == "CheatGroup")
            {
              e.Graphics.FillRectangle(Brushes.White, e.CellBounds.Left, e.CellBounds.Top + 1, e.CellBounds.Width, e.CellBounds.Height - 1);
            }
            else
            {
              e.Graphics.DrawRectangle(Pens.Gray, e.CellBounds);
              e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
            }
            e.Handled = true;
          }
        }
        else
        {
          if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
          {
            e.Graphics.FillRectangle(this.brSelection, e.CellBounds);
          }
          else
          {
            Brush brush = (Brush) new SolidBrush(e.CellStyle.BackColor);
            e.Graphics.FillRectangle(brush, e.CellBounds);
            brush.Dispose();
          }
          Graphics graphics3 = e.Graphics;
          Pen gray3 = Pens.Gray;
          Rectangle cellBounds = e.CellBounds;
          int left3 = cellBounds.Left;
          cellBounds = e.CellBounds;
          int top3 = cellBounds.Top;
          cellBounds = e.CellBounds;
          int right3 = cellBounds.Right;
          cellBounds = e.CellBounds;
          int top4 = cellBounds.Top;
          graphics3.DrawLine(gray3, left3, top3, right3, top4);
          Graphics graphics4 = e.Graphics;
          Pen gray4 = Pens.Gray;
          cellBounds = e.CellBounds;
          int left4 = cellBounds.Left;
          cellBounds = e.CellBounds;
          int bottom3 = cellBounds.Bottom;
          cellBounds = e.CellBounds;
          int right4 = cellBounds.Right;
          cellBounds = e.CellBounds;
          int bottom4 = cellBounds.Bottom;
          graphics4.DrawLine(gray4, left4, bottom3, right4, bottom4);
          e.PaintContent(e.CellBounds);
          e.Handled = true;
        }
      }
      else
        base.OnCellPainting(e);
    }
  }
}
