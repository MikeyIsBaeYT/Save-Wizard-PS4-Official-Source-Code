// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BaseRenderer
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
  [Browsable(true)]
  [ToolboxItem(true)]
  public class BaseRenderer : AbstractRenderer
  {
    private bool canWrap;
    private Rectangle? cellPadding;
    private StringAlignment? cellVerticalAlignment;
    private ImageList imageList;
    private int spacing = 1;
    private bool useGdiTextRendering = true;
    private object aspect;
    private Rectangle bounds;
    private OLVColumn column;
    private DrawListViewItemEventArgs drawItemEventArgs;
    private DrawListViewSubItemEventArgs eventArgs;
    private Font font;
    private bool isItemSelected;
    private bool isPrinting;
    private OLVListItem listItem;
    private ObjectListView objectListView;
    private object rowObject;
    private OLVListSubItem listSubItem;
    private Brush textBrush;
    private bool useCustomCheckboxImages;

    [Category("Appearance")]
    [Description("Can the renderer wrap text that does not fit completely within the cell")]
    [DefaultValue(false)]
    public bool CanWrap
    {
      get => this.canWrap;
      set
      {
        this.canWrap = value;
        if (!this.canWrap)
          return;
        this.UseGdiTextRendering = false;
      }
    }

    [Category("ObjectListView")]
    [Description("The number of pixels that renderer will leave empty around the edge of the cell")]
    [DefaultValue(null)]
    public Rectangle? CellPadding
    {
      get => this.cellPadding;
      set => this.cellPadding = value;
    }

    [Category("ObjectListView")]
    [Description("How will cell values be vertically aligned?")]
    [DefaultValue(null)]
    public virtual StringAlignment? CellVerticalAlignment
    {
      get => this.cellVerticalAlignment;
      set => this.cellVerticalAlignment = value;
    }

    [Browsable(false)]
    protected virtual Rectangle? EffectiveCellPadding
    {
      get
      {
        if (this.cellPadding.HasValue)
          return new Rectangle?(this.cellPadding.Value);
        Rectangle? nullable;
        int num1;
        if (this.OLVSubItem != null)
        {
          nullable = this.OLVSubItem.CellPadding;
          num1 = nullable.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          nullable = this.OLVSubItem.CellPadding;
          return new Rectangle?(nullable.Value);
        }
        int num2;
        if (this.ListItem != null)
        {
          nullable = this.ListItem.CellPadding;
          num2 = nullable.HasValue ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 != 0)
        {
          nullable = this.ListItem.CellPadding;
          return new Rectangle?(nullable.Value);
        }
        int num3;
        if (this.Column != null)
        {
          nullable = this.Column.CellPadding;
          num3 = nullable.HasValue ? 1 : 0;
        }
        else
          num3 = 0;
        if (num3 != 0)
        {
          nullable = this.Column.CellPadding;
          return new Rectangle?(nullable.Value);
        }
        int num4;
        if (this.ListView != null)
        {
          nullable = this.ListView.CellPadding;
          num4 = nullable.HasValue ? 1 : 0;
        }
        else
          num4 = 0;
        if (num4 != 0)
        {
          nullable = this.ListView.CellPadding;
          return new Rectangle?(nullable.Value);
        }
        nullable = new Rectangle?();
        return nullable;
      }
    }

    [Browsable(false)]
    protected virtual StringAlignment EffectiveCellVerticalAlignment
    {
      get
      {
        if (this.cellVerticalAlignment.HasValue)
          return this.cellVerticalAlignment.Value;
        StringAlignment? verticalAlignment;
        int num1;
        if (this.OLVSubItem != null)
        {
          verticalAlignment = this.OLVSubItem.CellVerticalAlignment;
          num1 = verticalAlignment.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
        {
          verticalAlignment = this.OLVSubItem.CellVerticalAlignment;
          return verticalAlignment.Value;
        }
        int num2;
        if (this.ListItem != null)
        {
          verticalAlignment = this.ListItem.CellVerticalAlignment;
          num2 = verticalAlignment.HasValue ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 != 0)
        {
          verticalAlignment = this.ListItem.CellVerticalAlignment;
          return verticalAlignment.Value;
        }
        int num3;
        if (this.Column != null)
        {
          verticalAlignment = this.Column.CellVerticalAlignment;
          num3 = verticalAlignment.HasValue ? 1 : 0;
        }
        else
          num3 = 0;
        if (num3 != 0)
        {
          verticalAlignment = this.Column.CellVerticalAlignment;
          return verticalAlignment.Value;
        }
        return this.ListView != null ? this.ListView.CellVerticalAlignment : StringAlignment.Center;
      }
    }

    [Category("Appearance")]
    [Description("The image list from which keyed images will be fetched for drawing.")]
    [DefaultValue(null)]
    public ImageList ImageList
    {
      get => this.imageList;
      set => this.imageList = value;
    }

    [Category("Appearance")]
    [Description("When rendering multiple images, how many pixels should be between each image?")]
    [DefaultValue(1)]
    public int Spacing
    {
      get => this.spacing;
      set => this.spacing = value;
    }

    [Category("Appearance")]
    [Description("Should text be rendered using GDI routines?")]
    [DefaultValue(true)]
    public bool UseGdiTextRendering
    {
      get => !this.IsPrinting && this.useGdiTextRendering;
      set => this.useGdiTextRendering = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object Aspect
    {
      get
      {
        if (this.aspect == null)
          this.aspect = this.column.GetValue(this.rowObject);
        return this.aspect;
      }
      set => this.aspect = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Rectangle Bounds
    {
      get => this.bounds;
      set => this.bounds = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVColumn Column
    {
      get => this.column;
      set => this.column = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawListViewItemEventArgs DrawItemEvent
    {
      get => this.drawItemEventArgs;
      set => this.drawItemEventArgs = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawListViewSubItemEventArgs Event
    {
      get => this.eventArgs;
      set => this.eventArgs = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Font Font
    {
      get
      {
        if (this.font != null || this.ListItem == null)
          return this.font;
        return this.SubItem == null || this.ListItem.UseItemStyleForSubItems ? this.ListItem.Font : this.SubItem.Font;
      }
      set => this.font = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ImageList ImageListOrDefault => this.ImageList ?? this.ListView.SmallImageList;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsDrawBackground => !this.IsPrinting;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsItemSelected
    {
      get => this.isItemSelected;
      set => this.isItemSelected = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsPrinting
    {
      get => this.isPrinting;
      set => this.isPrinting = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVListItem ListItem
    {
      get => this.listItem;
      set => this.listItem = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ObjectListView ListView
    {
      get => this.objectListView;
      set => this.objectListView = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVListSubItem OLVSubItem => this.listSubItem;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object RowObject
    {
      get => this.rowObject;
      set => this.rowObject = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVListSubItem SubItem
    {
      get => this.listSubItem;
      set => this.listSubItem = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Brush TextBrush
    {
      get => this.textBrush == null ? (Brush) new SolidBrush(this.GetForegroundColor()) : this.textBrush;
      set => this.textBrush = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UseCustomCheckboxImages
    {
      get => this.useCustomCheckboxImages;
      set => this.useCustomCheckboxImages = value;
    }

    private void ClearState()
    {
      this.Event = (DrawListViewSubItemEventArgs) null;
      this.DrawItemEvent = (DrawListViewItemEventArgs) null;
      this.Aspect = (object) null;
      this.Font = (Font) null;
      this.TextBrush = (Brush) null;
    }

    protected virtual Rectangle AlignRectangle(Rectangle outer, Rectangle inner)
    {
      Rectangle rectangle = new Rectangle(outer.Location, inner.Size);
      if (inner.Width < outer.Width)
        rectangle.X = this.AlignHorizontally(outer, inner);
      if (inner.Height < outer.Height)
        rectangle.Y = this.AlignVertically(outer, inner);
      return rectangle;
    }

    protected int AlignHorizontally(Rectangle outer, Rectangle inner)
    {
      switch (this.Column == null ? 0 : (int) this.Column.TextAlign)
      {
        case 0:
          return outer.Left + 1;
        case 1:
          return outer.Right - inner.Width - 1;
        case 2:
          return outer.Left + (outer.Width - inner.Width) / 2;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected int AlignVertically(Rectangle outer, Rectangle inner) => this.AlignVertically(outer, inner.Height);

    protected int AlignVertically(Rectangle outer, int innerHeight)
    {
      switch (this.EffectiveCellVerticalAlignment)
      {
        case StringAlignment.Near:
          return outer.Top + 1;
        case StringAlignment.Center:
          return outer.Top + (outer.Height - innerHeight) / 2;
        case StringAlignment.Far:
          return outer.Bottom - innerHeight - 1;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected virtual Rectangle CalculateAlignedRectangle(Graphics g, Rectangle r)
    {
      if (this.Column == null || this.Column.TextAlign == HorizontalAlignment.Left)
        return r;
      int width = this.CalculateCheckBoxWidth(g) + this.CalculateImageWidth(g, this.GetImageSelector()) + this.CalculateTextWidth(g, this.GetText());
      return width >= r.Width ? r : this.AlignRectangle(r, new Rectangle(0, 0, width, r.Height));
    }

    protected Rectangle CalculateCheckBoxBounds(Graphics g, Rectangle cellBounds)
    {
      Size size = !this.UseCustomCheckboxImages || this.ListView.StateImageList == null ? CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.CheckedNormal) : this.ListView.StateImageList.ImageSize;
      return this.AlignRectangle(cellBounds, new Rectangle(0, 0, size.Width, size.Height));
    }

    protected virtual int CalculateCheckBoxWidth(Graphics g)
    {
      if (!this.ListView.CheckBoxes || !this.ColumnIsPrimary)
        return 0;
      return this.UseCustomCheckboxImages && this.ListView.StateImageList != null ? this.ListView.StateImageList.ImageSize.Width : CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width + 6;
    }

    protected virtual int CalculateImageWidth(Graphics g, object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return 0;
      ImageList imageListOrDefault = this.ImageListOrDefault;
      if (imageListOrDefault != null)
      {
        num2 = -1;
        if (!(imageSelector is int num2) && imageSelector is string key2)
          num2 = imageListOrDefault.Images.IndexOfKey(key2);
        if (num2 >= 0)
          return imageListOrDefault.ImageSize.Width;
      }
      return imageSelector is Image image ? image.Width : 0;
    }

    protected virtual int CalculateTextWidth(Graphics g, string txt)
    {
      if (string.IsNullOrEmpty(txt))
        return 0;
      if (this.UseGdiTextRendering)
      {
        Size proposedSize = new Size(int.MaxValue, int.MaxValue);
        return TextRenderer.MeasureText((IDeviceContext) g, txt, this.Font, proposedSize, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix).Width;
      }
      using (StringFormat format = new StringFormat())
      {
        format.Trimming = StringTrimming.EllipsisCharacter;
        return 1 + (int) g.MeasureString(txt, this.Font, int.MaxValue, format).Width;
      }
    }

    public virtual Color GetBackgroundColor()
    {
      if (!this.ListView.Enabled)
        return SystemColors.Control;
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && this.ListView.FullRowSelect)
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightBackgroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
      }
      return this.SubItem == null || this.ListItem.UseItemStyleForSubItems ? this.ListItem.BackColor : this.SubItem.BackColor;
    }

    public virtual Color GetForegroundColor()
    {
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && (this.ColumnIsPrimary || this.ListView.FullRowSelect))
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightForegroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightForegroundColorOrDefault;
      }
      return this.SubItem == null || this.ListItem.UseItemStyleForSubItems ? this.ListItem.ForeColor : this.SubItem.ForeColor;
    }

    protected virtual Image GetImage() => this.GetImage(this.GetImageSelector());

    protected virtual Image GetImage(object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return (Image) null;
      ImageList imageListOrDefault = this.ImageListOrDefault;
      if (imageListOrDefault != null)
      {
        switch (imageSelector)
        {
          case int index2:
            return index2 < 0 || index2 >= imageListOrDefault.Images.Count ? (Image) null : imageListOrDefault.Images[index2];
          case string key2:
            return imageListOrDefault.Images.ContainsKey(key2) ? imageListOrDefault.Images[key2] : (Image) null;
        }
      }
      return imageSelector as Image;
    }

    protected virtual object GetImageSelector() => this.ColumnIsPrimary ? this.ListItem.ImageSelector : this.OLVSubItem.ImageSelector;

    protected virtual string GetText() => this.SubItem == null ? this.ListItem.Text : this.SubItem.Text;

    protected virtual Color GetTextBackgroundColor()
    {
      if (this.IsItemSelected && !this.ListView.UseTranslucentSelection && (this.ColumnIsPrimary || this.ListView.FullRowSelect))
      {
        if (this.ListView.Focused)
          return this.ListView.HighlightBackgroundColorOrDefault;
        if (!this.ListView.HideSelection)
          return this.ListView.UnfocusedHighlightBackgroundColorOrDefault;
      }
      return this.SubItem == null || this.ListItem.UseItemStyleForSubItems ? this.ListItem.BackColor : this.SubItem.BackColor;
    }

    public override bool RenderItem(
      DrawListViewItemEventArgs e,
      Graphics g,
      Rectangle itemBounds,
      object rowObject)
    {
      this.ClearState();
      this.DrawItemEvent = e;
      this.ListItem = (OLVListItem) e.Item;
      this.SubItem = (OLVListSubItem) null;
      this.ListView = (ObjectListView) this.ListItem.ListView;
      this.Column = this.ListView.GetColumn(0);
      this.RowObject = rowObject;
      this.Bounds = itemBounds;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      return this.OptionalRender(g, itemBounds);
    }

    public override bool RenderSubItem(
      DrawListViewSubItemEventArgs e,
      Graphics g,
      Rectangle cellBounds,
      object rowObject)
    {
      this.ClearState();
      this.Event = e;
      this.ListItem = (OLVListItem) e.Item;
      this.SubItem = (OLVListSubItem) e.SubItem;
      this.ListView = (ObjectListView) this.ListItem.ListView;
      this.Column = (OLVColumn) e.Header;
      this.RowObject = rowObject;
      this.Bounds = cellBounds;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      bool flag = this.OptionalRender(g, cellBounds);
      using (Pen pen = new Pen(Color.FromArgb(160, 160, 160), 1f))
        g.DrawLine(pen, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
      return flag;
    }

    public override void HitTest(OlvListViewHitTestInfo hti, int x, int y)
    {
      this.ClearState();
      this.ListView = hti.ListView;
      this.ListItem = hti.Item;
      this.SubItem = hti.SubItem;
      this.Column = hti.Column;
      this.RowObject = hti.RowObject;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      this.Bounds = this.SubItem != null ? this.ListItem.GetSubItemBounds(this.Column.Index) : this.ListItem.Bounds;
      using (Graphics graphics = this.ListView.CreateGraphics())
        this.HandleHitTest(graphics, hti, x, y);
    }

    public override Rectangle GetEditRectangle(
      Graphics g,
      Rectangle cellBounds,
      OLVListItem item,
      int subItemIndex,
      Size preferredSize)
    {
      this.ClearState();
      this.ListView = (ObjectListView) item.ListView;
      this.ListItem = item;
      this.SubItem = item.GetSubItem(subItemIndex);
      this.Column = this.ListView.GetColumn(subItemIndex);
      this.RowObject = item.RowObject;
      this.IsItemSelected = this.ListItem.Selected && this.ListItem.Enabled;
      this.Bounds = cellBounds;
      return this.HandleGetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
    }

    public virtual bool OptionalRender(Graphics g, Rectangle r)
    {
      if (this.ListView.View != View.Details)
        return false;
      this.Render(g, r);
      return true;
    }

    public virtual void Render(Graphics g, Rectangle r) => this.StandardRender(g, r);

    protected virtual void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
    {
      Rectangle alignedRectangle = this.CalculateAlignedRectangle(g, this.Bounds);
      this.StandardHitTest(g, hti, alignedRectangle, x, y);
    }

    protected virtual Rectangle HandleGetEditRectangle(
      Graphics g,
      Rectangle cellBounds,
      OLVListItem item,
      int subItemIndex,
      Size preferredSize)
    {
      return this.GetType() == typeof (BaseRenderer) ? this.StandardGetEditRectangle(g, cellBounds, preferredSize) : cellBounds;
    }

    protected void StandardRender(Graphics g, Rectangle r)
    {
      this.DrawBackground(g, r);
      if (this.ColumnIsPrimary)
      {
        r.X += 3;
        --r.Width;
      }
      r = this.ApplyCellPadding(r);
      this.DrawAlignedImageAndText(g, r);
      if (!ObjectListView.ShowCellPaddingBounds)
        return;
      g.DrawRectangle(Pens.Purple, r);
    }

    public virtual Rectangle ApplyCellPadding(Rectangle r)
    {
      Rectangle? effectiveCellPadding = this.EffectiveCellPadding;
      if (!effectiveCellPadding.HasValue)
        return r;
      Rectangle rectangle = effectiveCellPadding.Value;
      r.Width -= rectangle.Right;
      r.Height -= rectangle.Bottom;
      r.Offset(rectangle.Location);
      return r;
    }

    protected void StandardHitTest(
      Graphics g,
      OlvListViewHitTestInfo hti,
      Rectangle bounds,
      int x,
      int y)
    {
      Rectangle r = bounds;
      if (this.ColumnIsPrimary && !(this is TreeListView.TreeRenderer))
      {
        r.X += 3;
        --r.Width;
      }
      Rectangle cellBounds = this.ApplyCellPadding(r);
      int num = 0;
      if (this.ColumnIsPrimary && this.ListView.CheckBoxes)
      {
        Rectangle checkBoxBounds = this.CalculateCheckBoxBounds(g, cellBounds);
        checkBoxBounds.Inflate(2, 2);
        if (checkBoxBounds.Contains(x, y))
        {
          hti.HitTestLocation = HitTestLocation.CheckBox;
          return;
        }
        num = checkBoxBounds.Width;
      }
      cellBounds.X += num;
      cellBounds.Width -= num;
      int imageWidth = this.CalculateImageWidth(g, this.GetImageSelector());
      Rectangle rectangle1 = cellBounds;
      rectangle1.Width = imageWidth;
      if (rectangle1.Contains(x, y))
      {
        if (this.Column != null && this.Column.Index > 0 && this.Column.CheckBoxes)
          hti.HitTestLocation = HitTestLocation.CheckBox;
        else
          hti.HitTestLocation = HitTestLocation.Image;
      }
      else
      {
        cellBounds.X += imageWidth;
        cellBounds.Width -= imageWidth;
        int textWidth = this.CalculateTextWidth(g, this.GetText());
        Rectangle rectangle2 = cellBounds;
        rectangle2.Width = textWidth;
        if (rectangle2.Contains(x, y))
          hti.HitTestLocation = HitTestLocation.Text;
        else
          hti.HitTestLocation = HitTestLocation.InCell;
      }
    }

    protected Rectangle StandardGetEditRectangle(
      Graphics g,
      Rectangle cellBounds,
      Size preferredSize)
    {
      Rectangle alignedRectangle = this.CalculateAlignedRectangle(g, cellBounds);
      Rectangle paddedAlignedBounds = this.CalculatePaddedAlignedBounds(g, alignedRectangle, preferredSize);
      int num = this.CalculateCheckBoxWidth(g) + this.CalculateImageWidth(g, this.GetImageSelector());
      if (this.ColumnIsPrimary && this.ListItem.IndentCount > 0)
      {
        int width = this.ListView.SmallImageSize.Width;
        num += width * this.ListItem.IndentCount;
      }
      if (num > 0)
      {
        paddedAlignedBounds.X += num;
        paddedAlignedBounds.Width = Math.Max(paddedAlignedBounds.Width - num, 40);
      }
      return paddedAlignedBounds;
    }

    protected Rectangle CalculatePaddedAlignedBounds(
      Graphics g,
      Rectangle cellBounds,
      Size preferredSize)
    {
      Rectangle outer = this.ApplyCellPadding(cellBounds);
      return this.AlignRectangle(outer, new Rectangle(0, 0, outer.Width, preferredSize.Height));
    }

    protected virtual void DrawAlignedImage(Graphics g, Rectangle r, Image image)
    {
      if (image == null)
        return;
      Rectangle inner = new Rectangle(r.Location, image.Size);
      if (image.Height > r.Height)
      {
        float num = (float) r.Height / (float) image.Height;
        inner.Width = (int) ((double) image.Width * (double) num);
        inner.Height = r.Height - 1;
      }
      Rectangle rect = this.AlignRectangle(r, inner);
      if (this.ListItem.Enabled)
        g.DrawImage(image, rect);
      else
        ControlPaint.DrawImageDisabled(g, image, rect.X, rect.Y, this.GetBackgroundColor());
    }

    protected virtual void DrawAlignedImageAndText(Graphics g, Rectangle r) => this.DrawImageAndText(g, this.CalculateAlignedRectangle(g, r));

    protected virtual void DrawBackground(Graphics g, Rectangle r)
    {
      if (!this.IsDrawBackground)
        return;
      using (Brush brush = (Brush) new SolidBrush(this.GetBackgroundColor()))
        g.FillRectangle(brush, r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
    }

    protected virtual int DrawCheckBox(Graphics g, Rectangle r)
    {
      if (!(this.RowObject is cheat))
        return 0;
      if (this.IsPrinting || this.UseCustomCheckboxImages)
      {
        int stateImageIndex = this.ListItem.StateImageIndex;
        return this.ListView.StateImageList == null || stateImageIndex < 0 || stateImageIndex >= this.ListView.StateImageList.Images.Count ? 0 : this.DrawImage(g, r, (object) this.ListView.StateImageList.Images[stateImageIndex]) + 4;
      }
      r = this.CalculateCheckBoxBounds(g, r);
      CheckBoxState checkBoxState = this.GetCheckBoxState(this.ListItem.CheckState);
      object parent = (this.ListView as TreeListView).GetParent(this.RowObject);
      if (parent is group)
      {
        if ((parent as group).type == "1")
          RadioButtonRenderer.DrawRadioButton(g, r.Location, checkBoxState == CheckBoxState.CheckedNormal ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal);
        else
          CheckBoxRenderer.DrawCheckBox(g, r.Location, checkBoxState);
      }
      else
        CheckBoxRenderer.DrawCheckBox(g, r.Location, checkBoxState);
      return CheckBoxRenderer.GetGlyphSize(g, checkBoxState).Width + 6;
    }

    protected virtual CheckBoxState GetCheckBoxState(CheckState checkState)
    {
      if (this.IsCheckBoxDisabled)
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedDisabled;
          case CheckState.Checked:
            return CheckBoxState.CheckedDisabled;
          default:
            return CheckBoxState.MixedDisabled;
        }
      }
      else if (this.IsItemHot)
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedHot;
          case CheckState.Checked:
            return CheckBoxState.CheckedHot;
          default:
            return CheckBoxState.MixedHot;
        }
      }
      else
      {
        switch (checkState)
        {
          case CheckState.Unchecked:
            return CheckBoxState.UncheckedNormal;
          case CheckState.Checked:
            return CheckBoxState.CheckedNormal;
          default:
            return CheckBoxState.MixedNormal;
        }
      }
    }

    protected virtual bool IsCheckBoxDisabled
    {
      get
      {
        if (this.ListItem != null && !this.ListItem.Enabled)
          return true;
        return this.ListView.RenderNonEditableCheckboxesAsDisabled && (this.ListView.CellEditActivation == ObjectListView.CellEditActivateMode.None || this.Column != null && !this.Column.IsEditable);
      }
    }

    protected bool IsItemHot => this.ListView != null && this.ListItem != null && this.ListView.HotRowIndex == this.ListItem.Index && this.ListView.HotColumnIndex == (this.Column == null ? 0 : this.Column.Index) && this.ListView.HotCellHitLocation == HitTestLocation.CheckBox;

    protected virtual int DrawImage(Graphics g, Rectangle r, object imageSelector)
    {
      if (imageSelector == null || imageSelector == DBNull.Value)
        return 0;
      ImageList smallImageList = this.ListView.SmallImageList;
      if (smallImageList != null)
      {
        index2 = -1;
        if (imageSelector is int index2)
        {
          if (index2 >= smallImageList.Images.Count)
            index2 = -1;
        }
        else if (imageSelector is string key4)
          index2 = smallImageList.Images.IndexOfKey(key4);
        if (index2 >= 0)
        {
          if (this.IsPrinting)
          {
            imageSelector = (object) smallImageList.Images[index2];
          }
          else
          {
            if (smallImageList.ImageSize.Height < r.Height)
              r.Y = this.AlignVertically(r, new Rectangle(Point.Empty, smallImageList.ImageSize));
            Rectangle rectangle;
            ref Rectangle local = ref rectangle;
            int x1 = r.X;
            Rectangle bounds = this.Bounds;
            int x2 = bounds.X;
            int x3 = x1 - x2;
            int y1 = r.Y;
            bounds = this.Bounds;
            int y2 = bounds.Y;
            int y3 = y1 - y2;
            int width = r.Width;
            int height = r.Height;
            local = new Rectangle(x3, y3, width, height);
            NativeMethods.DrawImageList(g, smallImageList, index2, rectangle.X, rectangle.Y, this.IsItemSelected, !this.ListItem.Enabled);
            return smallImageList.ImageSize.Width;
          }
        }
      }
      if (!(imageSelector is Image image))
        return 0;
      if (image.Size.Height < r.Height)
        r.Y = this.AlignVertically(r, new Rectangle(Point.Empty, image.Size));
      if (this.ListItem.Enabled)
        g.DrawImageUnscaled(image, r.X, r.Y);
      else
        ControlPaint.DrawImageDisabled(g, image, r.X, r.Y, this.GetBackgroundColor());
      return image.Width;
    }

    protected virtual void DrawImageAndText(Graphics g, Rectangle r)
    {
      if (this.ListView.CheckBoxes && this.ColumnIsPrimary)
      {
        int num = this.DrawCheckBox(g, r);
        r.X += num;
        r.Width -= num;
      }
      int num1 = this.DrawImage(g, r, this.GetImageSelector());
      r.X += num1;
      r.Width -= num1;
      this.DrawText(g, r, this.GetText());
    }

    protected virtual int DrawImages(Graphics g, Rectangle r, ICollection imageSelectors)
    {
      List<Image> imageList = new List<Image>();
      foreach (object imageSelector in (IEnumerable) imageSelectors)
      {
        Image image = this.GetImage(imageSelector);
        if (image != null)
          imageList.Add(image);
      }
      int width = 0;
      int num = 0;
      foreach (Image image in imageList)
      {
        width += image.Width + this.Spacing;
        num = Math.Max(num, image.Height);
      }
      Rectangle rectangle = this.AlignRectangle(r, new Rectangle(0, 0, width, num));
      Color backgroundColor = this.GetBackgroundColor();
      Point location = rectangle.Location;
      foreach (Image image in imageList)
      {
        if (this.ListItem.Enabled)
          g.DrawImage(image, location);
        else
          ControlPaint.DrawImageDisabled(g, image, location.X, location.Y, backgroundColor);
        location.X += image.Width + this.Spacing;
      }
      return width;
    }

    public virtual void DrawText(Graphics g, Rectangle r, string txt)
    {
      if (string.IsNullOrEmpty(txt))
        return;
      if (this.UseGdiTextRendering)
        this.DrawTextGdi(g, r, txt);
      else
        this.DrawTextGdiPlus(g, r, txt);
    }

    protected virtual void DrawTextGdi(Graphics g, Rectangle r, string txt)
    {
      Color backColor = Color.Transparent;
      if (this.IsDrawBackground && this.IsItemSelected && this.ColumnIsPrimary && !this.ListView.FullRowSelect)
        backColor = this.GetTextBackgroundColor();
      TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsTranslateTransform | this.CellVerticalAlignmentAsTextFormatFlag;
      if (!this.CanWrap)
        flags |= TextFormatFlags.SingleLine;
      TextRenderer.DrawText((IDeviceContext) g, txt, this.Font, r, this.GetForegroundColor(), backColor, flags);
    }

    private bool ColumnIsPrimary => this.Column != null && this.Column.Index == 0;

    protected TextFormatFlags CellVerticalAlignmentAsTextFormatFlag
    {
      get
      {
        switch (this.EffectiveCellVerticalAlignment)
        {
          case StringAlignment.Near:
            return TextFormatFlags.Default;
          case StringAlignment.Center:
            return TextFormatFlags.VerticalCenter;
          case StringAlignment.Far:
            return TextFormatFlags.Bottom;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    protected virtual StringFormat StringFormatForGdiPlus
    {
      get
      {
        StringFormat stringFormat = new StringFormat();
        stringFormat.LineAlignment = this.EffectiveCellVerticalAlignment;
        stringFormat.Trimming = StringTrimming.EllipsisCharacter;
        stringFormat.Alignment = this.Column == null ? StringAlignment.Near : this.Column.TextStringAlign;
        if (!this.CanWrap)
          stringFormat.FormatFlags = StringFormatFlags.NoWrap;
        return stringFormat;
      }
    }

    protected virtual void DrawTextGdiPlus(Graphics g, Rectangle r, string txt)
    {
      using (StringFormat formatForGdiPlus = this.StringFormatForGdiPlus)
      {
        Font font = this.Font;
        if (this.IsDrawBackground && this.IsItemSelected && this.ColumnIsPrimary && !this.ListView.FullRowSelect)
        {
          SizeF sizeF = g.MeasureString(txt, font, r.Width, formatForGdiPlus);
          Rectangle rect = r;
          rect.Width = (int) sizeF.Width + 1;
          using (Brush brush = (Brush) new SolidBrush(this.ListView.HighlightBackgroundColorOrDefault))
            g.FillRectangle(brush, rect);
        }
        RectangleF layoutRectangle = (RectangleF) r;
        g.DrawString(txt, font, this.TextBrush, layoutRectangle, formatForGdiPlus);
      }
    }
  }
}
