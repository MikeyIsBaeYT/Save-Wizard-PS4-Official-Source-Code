// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.SimpleDropSink
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class SimpleDropSink : AbstractDropSink
  {
    private DropTargetLocation acceptableLocations;
    private bool acceptExternal = true;
    private bool autoScroll = true;
    private BillboardOverlay billboard;
    private int dropTargetIndex = -1;
    private DropTargetLocation dropTargetLocation;
    private int dropTargetSubItemIndex = -1;
    private Color feedbackColor;
    private int keyState;
    private Timer timer;
    private int scrollAmount;
    private bool originalFullRowSelect;
    private ModelDropEventArgs dropEventArgs;

    public SimpleDropSink()
    {
      this.timer = new Timer();
      this.timer.Interval = 250;
      this.timer.Tick += new EventHandler(this.timer_Tick);
      this.CanDropOnItem = true;
      this.FeedbackColor = Color.FromArgb(180, Color.MediumBlue);
      this.billboard = new BillboardOverlay();
    }

    public DropTargetLocation AcceptableLocations
    {
      get => this.acceptableLocations;
      set => this.acceptableLocations = value;
    }

    public bool AcceptExternal
    {
      get => this.acceptExternal;
      set => this.acceptExternal = value;
    }

    public bool AutoScroll
    {
      get => this.autoScroll;
      set => this.autoScroll = value;
    }

    public BillboardOverlay Billboard
    {
      get => this.billboard;
      set => this.billboard = value;
    }

    public bool CanDropBetween
    {
      get => (this.AcceptableLocations & DropTargetLocation.BetweenItems) == DropTargetLocation.BetweenItems;
      set
      {
        if (value)
          this.AcceptableLocations |= DropTargetLocation.BetweenItems;
        else
          this.AcceptableLocations &= ~DropTargetLocation.BetweenItems;
      }
    }

    public bool CanDropOnBackground
    {
      get => (this.AcceptableLocations & DropTargetLocation.Background) == DropTargetLocation.Background;
      set
      {
        if (value)
          this.AcceptableLocations |= DropTargetLocation.Background;
        else
          this.AcceptableLocations &= ~DropTargetLocation.Background;
      }
    }

    public bool CanDropOnItem
    {
      get => (this.AcceptableLocations & DropTargetLocation.Item) == DropTargetLocation.Item;
      set
      {
        if (value)
          this.AcceptableLocations |= DropTargetLocation.Item;
        else
          this.AcceptableLocations &= ~DropTargetLocation.Item;
      }
    }

    public bool CanDropOnSubItem
    {
      get => (this.AcceptableLocations & DropTargetLocation.SubItem) == DropTargetLocation.SubItem;
      set
      {
        if (value)
          this.AcceptableLocations |= DropTargetLocation.SubItem;
        else
          this.AcceptableLocations &= ~DropTargetLocation.SubItem;
      }
    }

    public int DropTargetIndex
    {
      get => this.dropTargetIndex;
      set
      {
        if (this.dropTargetIndex == value)
          return;
        this.dropTargetIndex = value;
        this.ListView.Invalidate();
      }
    }

    public OLVListItem DropTargetItem => this.ListView.GetItem(this.DropTargetIndex);

    public DropTargetLocation DropTargetLocation
    {
      get => this.dropTargetLocation;
      set
      {
        if (this.dropTargetLocation == value)
          return;
        this.dropTargetLocation = value;
        this.ListView.Invalidate();
      }
    }

    public int DropTargetSubItemIndex
    {
      get => this.dropTargetSubItemIndex;
      set
      {
        if (this.dropTargetSubItemIndex == value)
          return;
        this.dropTargetSubItemIndex = value;
        this.ListView.Invalidate();
      }
    }

    public Color FeedbackColor
    {
      get => this.feedbackColor;
      set => this.feedbackColor = value;
    }

    public bool IsAltDown => (this.KeyState & 32) == 32;

    public bool IsAnyModifierDown => (uint) (this.KeyState & 44) > 0U;

    public bool IsControlDown => (this.KeyState & 8) == 8;

    public bool IsLeftMouseButtonDown => (this.KeyState & 1) == 1;

    public bool IsMiddleMouseButtonDown => (this.KeyState & 16) == 16;

    public bool IsRightMouseButtonDown => (this.KeyState & 2) == 2;

    public bool IsShiftDown => (this.KeyState & 4) == 4;

    public int KeyState
    {
      get => this.keyState;
      set => this.keyState = value;
    }

    public event EventHandler<OlvDropEventArgs> CanDrop;

    public event EventHandler<OlvDropEventArgs> Dropped;

    public event EventHandler<ModelDropEventArgs> ModelCanDrop;

    public event EventHandler<ModelDropEventArgs> ModelDropped;

    protected override void Cleanup()
    {
      this.DropTargetLocation = DropTargetLocation.None;
      this.ListView.FullRowSelect = this.originalFullRowSelect;
      this.Billboard.Text = (string) null;
    }

    public override void DrawFeedback(Graphics g, Rectangle bounds)
    {
      g.SmoothingMode = ObjectListView.SmoothingMode;
      switch (this.DropTargetLocation)
      {
        case DropTargetLocation.Background:
          this.DrawFeedbackBackgroundTarget(g, bounds);
          break;
        case DropTargetLocation.Item:
          this.DrawFeedbackItemTarget(g, bounds);
          break;
        case DropTargetLocation.AboveItem:
          this.DrawFeedbackAboveItemTarget(g, bounds);
          break;
        case DropTargetLocation.BelowItem:
          this.DrawFeedbackBelowItemTarget(g, bounds);
          break;
      }
      if (this.Billboard == null)
        return;
      this.Billboard.Draw(this.ListView, g, bounds);
    }

    public override void Drop(DragEventArgs args)
    {
      this.TriggerDroppedEvent(args);
      this.timer.Stop();
      this.Cleanup();
    }

    public override void Enter(DragEventArgs args)
    {
      this.originalFullRowSelect = this.ListView.FullRowSelect;
      this.ListView.FullRowSelect = false;
      this.dropEventArgs = new ModelDropEventArgs();
      this.dropEventArgs.DropSink = this;
      this.dropEventArgs.ListView = this.ListView;
      this.dropEventArgs.DataObject = (object) args.Data;
      if (args.Data is OLVDataObject data)
      {
        this.dropEventArgs.SourceListView = data.ListView;
        this.dropEventArgs.SourceModels = data.ModelObjects;
      }
      this.Over(args);
    }

    public override void Over(DragEventArgs args)
    {
      this.KeyState = args.KeyState;
      Point client = this.ListView.PointToClient(new Point(args.X, args.Y));
      args.Effect = this.CalculateDropAction(args, client);
      this.CheckScrolling(client);
    }

    protected virtual void TriggerDroppedEvent(DragEventArgs args)
    {
      this.dropEventArgs.Handled = false;
      if (this.dropEventArgs.SourceListView != null)
        this.OnModelDropped(this.dropEventArgs);
      if (this.dropEventArgs.Handled)
        return;
      this.OnDropped((OlvDropEventArgs) this.dropEventArgs);
    }

    protected virtual void OnCanDrop(OlvDropEventArgs args)
    {
      if (this.CanDrop == null)
        return;
      this.CanDrop((object) this, args);
    }

    protected virtual void OnDropped(OlvDropEventArgs args)
    {
      if (this.Dropped == null)
        return;
      this.Dropped((object) this, args);
    }

    protected virtual void OnModelCanDrop(ModelDropEventArgs args)
    {
      if (!this.AcceptExternal && args.SourceListView != null && args.SourceListView != this.ListView)
      {
        args.Effect = DragDropEffects.None;
        args.DropTargetLocation = DropTargetLocation.None;
        args.InfoMessage = "This list doesn't accept drops from other lists";
      }
      else
      {
        if (this.ModelCanDrop == null)
          return;
        this.ModelCanDrop((object) this, args);
      }
    }

    protected virtual void OnModelDropped(ModelDropEventArgs args)
    {
      if (this.ModelDropped == null)
        return;
      this.ModelDropped((object) this, args);
    }

    private void timer_Tick(object sender, EventArgs e) => this.HandleTimerTick();

    protected virtual void HandleTimerTick()
    {
      if (this.IsLeftMouseButtonDown && (Control.MouseButtons & MouseButtons.Left) != MouseButtons.Left || this.IsMiddleMouseButtonDown && (Control.MouseButtons & MouseButtons.Middle) != MouseButtons.Middle || this.IsRightMouseButtonDown && (Control.MouseButtons & MouseButtons.Right) != MouseButtons.Right)
      {
        this.timer.Stop();
        this.Cleanup();
      }
      else
      {
        Point client = this.ListView.PointToClient(Cursor.Position);
        Rectangle clientRectangle = this.ListView.ClientRectangle;
        clientRectangle.Inflate(30, 30);
        if (!clientRectangle.Contains(client))
          return;
        this.ListView.LowLevelScroll(0, this.scrollAmount);
      }
    }

    protected virtual void CalculateDropTarget(OlvDropEventArgs args, Point pt)
    {
      DropTargetLocation dropTargetLocation = DropTargetLocation.None;
      int num1 = -1;
      int num2 = 0;
      if (this.CanDropOnBackground)
        dropTargetLocation = DropTargetLocation.Background;
      OlvListViewHitTestInfo listViewHitTestInfo1 = this.ListView.OlvHitTest(pt.X, pt.Y);
      if (listViewHitTestInfo1.Item != null && this.CanDropOnItem)
      {
        dropTargetLocation = DropTargetLocation.Item;
        num1 = listViewHitTestInfo1.Item.Index;
        if (listViewHitTestInfo1.SubItem != null && this.CanDropOnSubItem)
          num2 = listViewHitTestInfo1.Item.SubItems.IndexOf((ListViewItem.ListViewSubItem) listViewHitTestInfo1.SubItem);
      }
      if (this.CanDropBetween && this.ListView.GetItemCount() > 0)
      {
        if (dropTargetLocation == DropTargetLocation.Item)
        {
          int num3 = pt.Y - 3;
          Rectangle bounds = listViewHitTestInfo1.Item.Bounds;
          int top = bounds.Top;
          if (num3 <= top)
            dropTargetLocation = DropTargetLocation.AboveItem;
          int num4 = pt.Y + 3;
          bounds = listViewHitTestInfo1.Item.Bounds;
          int bottom = bounds.Bottom;
          if (num4 >= bottom)
            dropTargetLocation = DropTargetLocation.BelowItem;
        }
        else
        {
          OlvListViewHitTestInfo listViewHitTestInfo2 = this.ListView.OlvHitTest(pt.X, pt.Y + 3);
          if (listViewHitTestInfo2.Item != null)
          {
            num1 = listViewHitTestInfo2.Item.Index;
            dropTargetLocation = DropTargetLocation.AboveItem;
          }
          else
          {
            OlvListViewHitTestInfo listViewHitTestInfo3 = this.ListView.OlvHitTest(pt.X, pt.Y - 3);
            if (listViewHitTestInfo3.Item != null)
            {
              num1 = listViewHitTestInfo3.Item.Index;
              dropTargetLocation = DropTargetLocation.BelowItem;
            }
          }
        }
      }
      args.DropTargetLocation = dropTargetLocation;
      args.DropTargetIndex = num1;
      args.DropTargetSubItemIndex = num2;
    }

    public virtual DragDropEffects CalculateDropAction(DragEventArgs args, Point pt)
    {
      this.CalculateDropTarget((OlvDropEventArgs) this.dropEventArgs, pt);
      this.dropEventArgs.MouseLocation = pt;
      this.dropEventArgs.InfoMessage = (string) null;
      this.dropEventArgs.Handled = false;
      if (this.dropEventArgs.SourceListView != null)
      {
        this.dropEventArgs.TargetModel = this.ListView.GetModelObject(this.dropEventArgs.DropTargetIndex);
        this.OnModelCanDrop(this.dropEventArgs);
      }
      if (!this.dropEventArgs.Handled)
        this.OnCanDrop((OlvDropEventArgs) this.dropEventArgs);
      this.UpdateAfterCanDropEvent((OlvDropEventArgs) this.dropEventArgs);
      return this.dropEventArgs.Effect;
    }

    public DragDropEffects CalculateStandardDropActionFromKeys()
    {
      if (!this.IsControlDown)
        return DragDropEffects.Move;
      return this.IsShiftDown ? DragDropEffects.Link : DragDropEffects.Copy;
    }

    protected virtual void CheckScrolling(Point pt)
    {
      if (!this.AutoScroll)
        return;
      Rectangle contentRectangle = this.ListView.ContentRectangle;
      int rowHeightEffective = this.ListView.RowHeightEffective;
      int num = rowHeightEffective;
      if (this.ListView.View == View.Tile)
        num /= 2;
      if (pt.Y <= contentRectangle.Top + num)
      {
        this.timer.Interval = pt.Y <= contentRectangle.Top + num / 2 ? 100 : 350;
        this.timer.Start();
        this.scrollAmount = -rowHeightEffective;
      }
      else if (pt.Y >= contentRectangle.Bottom - num)
      {
        this.timer.Interval = pt.Y >= contentRectangle.Bottom - num / 2 ? 100 : 350;
        this.timer.Start();
        this.scrollAmount = rowHeightEffective;
      }
      else
        this.timer.Stop();
    }

    protected virtual void UpdateAfterCanDropEvent(OlvDropEventArgs args)
    {
      this.DropTargetIndex = args.DropTargetIndex;
      this.DropTargetLocation = args.DropTargetLocation;
      this.DropTargetSubItemIndex = args.DropTargetSubItemIndex;
      if (this.Billboard == null)
        return;
      Point mouseLocation = args.MouseLocation;
      mouseLocation.Offset(5, 5);
      if (this.Billboard.Text != this.dropEventArgs.InfoMessage || this.Billboard.Location != mouseLocation)
      {
        this.Billboard.Text = this.dropEventArgs.InfoMessage;
        this.Billboard.Location = mouseLocation;
        this.ListView.Invalidate();
      }
    }

    protected virtual void DrawFeedbackBackgroundTarget(Graphics g, Rectangle bounds)
    {
      float width = 12f;
      Rectangle rect = bounds;
      rect.Inflate((int) -(double) width / 2, (int) -(double) width / 2);
      using (Pen pen = new Pen(Color.FromArgb(128, this.FeedbackColor), width))
      {
        using (GraphicsPath roundedRect = this.GetRoundedRect(rect, 30f))
          g.DrawPath(pen, roundedRect);
      }
    }

    protected virtual void DrawFeedbackItemTarget(Graphics g, Rectangle bounds)
    {
      if (this.DropTargetItem == null)
        return;
      Rectangle dropTargetRectangle = this.CalculateDropTargetRectangle(this.DropTargetItem, this.DropTargetSubItemIndex);
      dropTargetRectangle.Inflate(1, 1);
      float diameter = (float) (dropTargetRectangle.Height / 3);
      using (GraphicsPath roundedRect = this.GetRoundedRect(dropTargetRectangle, diameter))
      {
        using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(48, this.FeedbackColor)))
          g.FillPath((Brush) solidBrush, roundedRect);
        using (Pen pen = new Pen(this.FeedbackColor, 3f))
          g.DrawPath(pen, roundedRect);
      }
    }

    protected virtual void DrawFeedbackAboveItemTarget(Graphics g, Rectangle bounds)
    {
      if (this.DropTargetItem == null)
        return;
      Rectangle dropTargetRectangle = this.CalculateDropTargetRectangle(this.DropTargetItem, this.DropTargetSubItemIndex);
      this.DrawBetweenLine(g, dropTargetRectangle.Left, dropTargetRectangle.Top, dropTargetRectangle.Right, dropTargetRectangle.Top);
    }

    protected virtual void DrawFeedbackBelowItemTarget(Graphics g, Rectangle bounds)
    {
      if (this.DropTargetItem == null)
        return;
      Rectangle dropTargetRectangle = this.CalculateDropTargetRectangle(this.DropTargetItem, this.DropTargetSubItemIndex);
      this.DrawBetweenLine(g, dropTargetRectangle.Left, dropTargetRectangle.Bottom, dropTargetRectangle.Right, dropTargetRectangle.Bottom);
    }

    protected GraphicsPath GetRoundedRect(Rectangle rect, float diameter)
    {
      GraphicsPath graphicsPath = new GraphicsPath();
      RectangleF rect1 = new RectangleF((float) rect.X, (float) rect.Y, diameter, diameter);
      graphicsPath.AddArc(rect1, 180f, 90f);
      rect1.X = (float) rect.Right - diameter;
      graphicsPath.AddArc(rect1, 270f, 90f);
      rect1.Y = (float) rect.Bottom - diameter;
      graphicsPath.AddArc(rect1, 0.0f, 90f);
      rect1.X = (float) rect.Left;
      graphicsPath.AddArc(rect1, 90f, 90f);
      graphicsPath.CloseFigure();
      return graphicsPath;
    }

    protected virtual Rectangle CalculateDropTargetRectangle(OLVListItem item, int subItem)
    {
      if (subItem > 0)
        return item.SubItems[subItem].Bounds;
      Rectangle cellTextBounds = this.ListView.CalculateCellTextBounds(item, subItem);
      if (item.IndentCount > 0)
      {
        int width = this.ListView.SmallImageSize.Width;
        cellTextBounds.X += width * item.IndentCount;
        cellTextBounds.Width -= width * item.IndentCount;
      }
      return cellTextBounds;
    }

    protected virtual void DrawBetweenLine(Graphics g, int x1, int y1, int x2, int y2)
    {
      using (Brush brush = (Brush) new SolidBrush(this.FeedbackColor))
      {
        int num1 = x1;
        int num2 = y1;
        using (GraphicsPath path = new GraphicsPath())
        {
          path.AddLine(num1, num2 + 5, num1, num2 - 5);
          path.AddBezier(num1, num2 - 6, num1 + 3, num2 - 2, num1 + 6, num2 - 1, num1 + 11, num2);
          path.AddBezier(num1 + 11, num2, num1 + 6, num2 + 1, num1 + 3, num2 + 2, num1, num2 + 6);
          path.CloseFigure();
          g.FillPath(brush, path);
        }
        int num3 = x2;
        int num4 = y2;
        using (GraphicsPath path = new GraphicsPath())
        {
          path.AddLine(num3, num4 + 6, num3, num4 - 6);
          path.AddBezier(num3, num4 - 7, num3 - 3, num4 - 2, num3 - 6, num4 - 1, num3 - 11, num4);
          path.AddBezier(num3 - 11, num4, num3 - 6, num4 + 1, num3 - 3, num4 + 2, num3, num4 + 7);
          path.CloseFigure();
          g.FillPath(brush, path);
        }
      }
      using (Pen pen = new Pen(this.FeedbackColor, 3f))
        g.DrawLine(pen, x1, y1, x2, y2);
    }
  }
}
