// Decompiled with JetBrains decompiler
// Type: CustomControls.CustomVScrollbar
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using PS3SaveEditor.CustomScrollbar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
  public class CustomVScrollbar : UserControl
  {
    protected Color moChannelColor = Color.Empty;
    protected Image moUpArrowImage = (Image) null;
    protected Image moDownArrowImage = (Image) null;
    protected Image moThumbArrowImage = (Image) null;
    protected Image moThumbTopImage = (Image) null;
    protected Image moThumbTopSpanImage = (Image) null;
    protected Image moThumbBottomImage = (Image) null;
    protected Image moThumbBottomSpanImage = (Image) null;
    protected Image moThumbMiddleImage = (Image) null;
    protected int moLargeChange = 10;
    protected int moSmallChange = 1;
    protected int moMinimum = 0;
    protected int moMaximum = 100;
    protected int moValue = 0;
    private int nClickPoint;
    protected int moThumbTop = 0;
    protected bool moAutoSize = false;
    private bool moThumbDown = false;
    private bool moThumbDragging = false;

    public event EventHandler Scroll = null;

    public event EventHandler ValueChanged = null;

    private int GetThumbHeight()
    {
      int num1 = this.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
      int num2 = (int) ((float) this.LargeChange / (float) this.Maximum * (float) num1);
      float num3;
      if (num2 > num1)
      {
        num2 = num1;
        num3 = (float) num1;
      }
      if (num2 < 56)
      {
        num2 = 56;
        num3 = 56f;
      }
      return num2;
    }

    public CustomVScrollbar()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.SetStyle(ControlStyles.ResizeRedraw, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.moChannelColor = Color.FromArgb(125, 45, 17);
      this.UpArrowImage = (Image) Resource.uparrow;
      this.DownArrowImage = (Image) Resource.downarrow;
      this.ThumbBottomImage = (Image) Resource.ThumbBottom;
      this.ThumbBottomSpanImage = (Image) Resource.ThumbSpanBottom;
      this.ThumbTopImage = (Image) Resource.ThumbTop;
      this.ThumbTopSpanImage = (Image) Resource.ThumbSpanTop;
      this.ThumbMiddleImage = (Image) Resource.ThumbMiddle;
      this.Width = this.UpArrowImage.Width;
      this.MinimumSize = new Size(this.UpArrowImage.Width, this.UpArrowImage.Height + this.DownArrowImage.Height + this.GetThumbHeight());
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("LargeChange")]
    public int LargeChange
    {
      get => this.moLargeChange;
      set
      {
        this.moLargeChange = value;
        this.Invalidate();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("SmallChange")]
    public int SmallChange
    {
      get => this.moSmallChange;
      set
      {
        this.moSmallChange = value;
        this.Invalidate();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Minimum")]
    public int Minimum
    {
      get => this.moMinimum;
      set
      {
        this.moMinimum = value;
        this.Invalidate();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Maximum")]
    public int Maximum
    {
      get => this.moMaximum;
      set
      {
        this.moMaximum = value;
        this.Invalidate();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Value")]
    public int Value
    {
      get => this.moValue;
      set
      {
        this.moValue = value;
        int num1 = this.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
        int num2 = (int) ((float) this.LargeChange / (float) this.Maximum * (float) num1);
        float num3;
        if (num2 > num1)
        {
          num2 = num1;
          num3 = (float) num1;
        }
        if (num2 < 56)
        {
          num2 = 56;
          num3 = 56f;
        }
        int num4 = num1 - num2;
        int num5 = this.Maximum - this.Minimum - this.LargeChange;
        float num6 = 0.0f;
        if ((uint) num5 > 0U)
          num6 = (float) this.moValue / (float) num5;
        this.moThumbTop = (int) (num6 * (float) num4);
        this.Invalidate();
      }
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Channel Color")]
    public Color ChannelColor
    {
      get => this.moChannelColor;
      set => this.moChannelColor = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image UpArrowImage
    {
      get => this.moUpArrowImage;
      set => this.moUpArrowImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image DownArrowImage
    {
      get => this.moDownArrowImage;
      set => this.moDownArrowImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbTopImage
    {
      get => this.moThumbTopImage;
      set => this.moThumbTopImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbTopSpanImage
    {
      get => this.moThumbTopSpanImage;
      set => this.moThumbTopSpanImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbBottomImage
    {
      get => this.moThumbBottomImage;
      set => this.moThumbBottomImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbBottomSpanImage
    {
      get => this.moThumbBottomSpanImage;
      set => this.moThumbBottomSpanImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbMiddleImage
    {
      get => this.moThumbMiddleImage;
      set => this.moThumbMiddleImage = value;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
      if (this.UpArrowImage != null)
        e.Graphics.DrawImage(this.UpArrowImage, new Rectangle(new Point(0, 0), new Size(this.Width, this.UpArrowImage.Height)));
      Brush brush1 = (Brush) new SolidBrush(this.moChannelColor);
      Brush brush2 = (Brush) new SolidBrush(this.moChannelColor);
      e.Graphics.FillRectangle(brush1, new Rectangle(0, this.UpArrowImage.Height, this.Width, this.Height - this.DownArrowImage.Height));
      int num1 = this.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
      float num2 = (float) this.LargeChange / (float) this.Maximum * (float) num1;
      int num3 = (int) num2;
      if (num3 > num1)
      {
        num3 = num1;
        num2 = (float) num1;
      }
      if (num3 < 56)
        num2 = 56f;
      float num4 = (float) (((double) num2 - (double) (this.ThumbMiddleImage.Height + this.ThumbTopImage.Height + this.ThumbBottomImage.Height)) / 2.0);
      int height = (int) num4;
      int y1 = this.moThumbTop + this.UpArrowImage.Height;
      e.Graphics.DrawImage(this.ThumbTopImage, new Rectangle(1, y1, this.Width - 2, this.ThumbTopImage.Height));
      int y2 = y1 + this.ThumbTopImage.Height;
      Rectangle rect = new Rectangle(1, y2, this.Width - 2, height);
      e.Graphics.DrawImage(this.ThumbTopSpanImage, 1f, (float) y2, (float) this.Width - 2f, num4 * 2f);
      int y3 = y2 + height;
      e.Graphics.DrawImage(this.ThumbMiddleImage, new Rectangle(1, y3, this.Width - 2, this.ThumbMiddleImage.Height));
      int y4 = y3 + this.ThumbMiddleImage.Height;
      rect = new Rectangle(1, y4, this.Width - 2, height * 2);
      e.Graphics.DrawImage(this.ThumbBottomSpanImage, rect);
      int y5 = y4 + height;
      e.Graphics.DrawImage(this.ThumbBottomImage, new Rectangle(1, y5, this.Width - 2, height));
      if (this.DownArrowImage == null)
        return;
      e.Graphics.DrawImage(this.DownArrowImage, new Rectangle(new Point(0, this.Height - this.DownArrowImage.Height), new Size(this.Width, this.DownArrowImage.Height)));
    }

    public override bool AutoSize
    {
      get => base.AutoSize;
      set
      {
        base.AutoSize = value;
        if (!base.AutoSize)
          return;
        this.Width = this.moUpArrowImage.Width;
      }
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.Name = nameof (CustomVScrollbar);
      this.MouseDown += new MouseEventHandler(this.CustomScrollbar_MouseDown);
      this.MouseMove += new MouseEventHandler(this.CustomScrollbar_MouseMove);
      this.MouseUp += new MouseEventHandler(this.CustomScrollbar_MouseUp);
      this.ResumeLayout(false);
    }

    private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
    {
      Point client = this.PointToClient(Cursor.Position);
      int num1 = this.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
      int height = (int) ((float) this.LargeChange / (float) this.Maximum * (float) num1);
      float num2;
      if (height > num1)
      {
        height = num1;
        num2 = (float) num1;
      }
      if (height < 56)
      {
        height = 56;
        num2 = 56f;
      }
      int y = this.moThumbTop + this.UpArrowImage.Height;
      if (new Rectangle(new Point(1, y), new Size(this.ThumbMiddleImage.Width, height)).Contains(client))
      {
        this.nClickPoint = client.Y - y;
        this.moThumbDown = true;
      }
      if (new Rectangle(new Point(1, 0), new Size(this.UpArrowImage.Width, this.UpArrowImage.Height)).Contains(client))
      {
        int num3 = this.Maximum - this.Minimum - this.LargeChange;
        int num4 = num1 - height;
        if (num3 > 0 && num4 > 0)
        {
          if (this.moThumbTop - this.SmallChange < 0)
            this.moThumbTop = 0;
          else
            this.moThumbTop -= this.SmallChange;
          this.moValue = (int) ((float) this.moThumbTop / (float) num4 * (float) (this.Maximum - this.LargeChange));
          if (this.ValueChanged != null)
            this.ValueChanged((object) this, new EventArgs());
          if (this.Scroll != null)
            this.Scroll((object) this, new EventArgs());
          this.Invalidate();
        }
      }
      if (!new Rectangle(new Point(1, this.UpArrowImage.Height + num1), new Size(this.UpArrowImage.Width, this.UpArrowImage.Height)).Contains(client))
        return;
      int num5 = this.Maximum - this.Minimum - this.LargeChange;
      int num6 = num1 - height;
      if (num5 > 0 && num6 > 0)
      {
        if (this.moThumbTop + this.SmallChange > num6)
          this.moThumbTop = num6;
        else
          this.moThumbTop += this.SmallChange;
        this.moValue = (int) ((float) this.moThumbTop / (float) num6 * (float) (this.Maximum - this.LargeChange));
        if (this.ValueChanged != null)
          this.ValueChanged((object) this, new EventArgs());
        if (this.Scroll != null)
          this.Scroll((object) this, new EventArgs());
        this.Invalidate();
      }
    }

    private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
    {
      this.moThumbDown = false;
      this.moThumbDragging = false;
    }

    private void MoveThumb(int y)
    {
      int num1 = this.Maximum - this.Minimum;
      int num2 = this.Height - (this.UpArrowImage.Height + this.DownArrowImage.Height);
      int num3 = (int) ((float) this.LargeChange / (float) this.Maximum * (float) num2);
      float num4;
      if (num3 > num2)
      {
        num3 = num2;
        num4 = (float) num2;
      }
      if (num3 < 56)
      {
        num3 = 56;
        num4 = 56f;
      }
      int nClickPoint = this.nClickPoint;
      int num5 = num2 - num3;
      if (!this.moThumbDown || num1 <= 0 || num5 <= 0)
        return;
      int num6 = y - (this.UpArrowImage.Height + nClickPoint);
      int num7;
      this.moThumbTop = num6 >= 0 ? (num6 <= num5 ? y - (this.UpArrowImage.Height + nClickPoint) : (num7 = num5)) : (num7 = 0);
      this.moValue = (int) ((float) this.moThumbTop / (float) num5 * (float) (this.Maximum - this.LargeChange));
      Application.DoEvents();
      this.Invalidate();
    }

    private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.moThumbDown)
        this.moThumbDragging = true;
      if (this.moThumbDragging)
        this.MoveThumb(e.Y);
      if (this.ValueChanged != null)
        this.ValueChanged((object) this, new EventArgs());
      if (this.Scroll == null)
        return;
      this.Scroll((object) this, new EventArgs());
    }
  }
}
