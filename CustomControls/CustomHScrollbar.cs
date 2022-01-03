// Decompiled with JetBrains decompiler
// Type: CustomControls.CustomHScrollbar
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
  public class CustomHScrollbar : UserControl
  {
    protected Color moChannelColor = Color.Empty;
    protected Image moUpArrowImage = (Image) null;
    protected Image moDownArrowImage = (Image) null;
    protected Image moThumbArrowImage = (Image) null;
    protected Image moThumbRightImage = (Image) null;
    protected Image moThumbRightSpanImage = (Image) null;
    protected Image moThumbLeftImage = (Image) null;
    protected Image moThumbLeftSpanImage = (Image) null;
    protected Image moThumbMiddleImage = (Image) null;
    protected int moLargeChange = 10;
    protected int moSmallChange = 1;
    protected int moMinimum = 0;
    protected int moMaximum = 100;
    protected int moValue = 0;
    private int nClickPoint;
    protected int moThumbRight = 0;
    protected bool moAutoSize = false;
    private bool moThumbDown = false;
    private bool moThumbDragging = false;

    public event EventHandler Scroll = null;

    public event EventHandler ValueChanged = null;

    private int GetThumbWidth()
    {
      int num1 = this.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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

    public CustomHScrollbar()
    {
      this.InitializeComponent();
      this.Font = Util.GetFontForPlatform(this.Font);
      this.SetStyle(ControlStyles.ResizeRedraw, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.DoubleBuffer, true);
      this.moChannelColor = Color.FromArgb(51, 166, 3);
      this.LeftArrowImage = (Image) Resource.leftarrow;
      this.RightArrowImage = (Image) Resource.rightarrow;
      this.ThumbLeftImage = (Image) Resource.ThumbLeft;
      this.ThumbLeftSpanImage = (Image) Resource.ThumbSpanLeft;
      this.ThumbRightImage = (Image) Resource.ThumbRight;
      this.ThumbRightSpanImage = (Image) Resource.ThumbSpanRight;
      this.ThumbMiddleImage = (Image) Resource.ThumbMiddleH;
      this.Height = this.LeftArrowImage.Height;
      this.MinimumSize = new Size(this.LeftArrowImage.Width + this.RightArrowImage.Width + this.GetThumbWidth(), this.LeftArrowImage.Height);
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
        int num1 = this.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
        this.moThumbRight = (int) (num6 * (float) num4);
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
    public Image LeftArrowImage
    {
      get => this.moUpArrowImage;
      set => this.moUpArrowImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image RightArrowImage
    {
      get => this.moDownArrowImage;
      set => this.moDownArrowImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbRightImage
    {
      get => this.moThumbRightImage;
      set => this.moThumbRightImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbRightSpanImage
    {
      get => this.moThumbRightSpanImage;
      set => this.moThumbRightSpanImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbLeftImage
    {
      get => this.moThumbLeftImage;
      set => this.moThumbLeftImage = value;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Skin")]
    [Description("Up Arrow Graphic")]
    public Image ThumbLeftSpanImage
    {
      get => this.moThumbLeftSpanImage;
      set => this.moThumbLeftSpanImage = value;
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
      if (this.LeftArrowImage != null)
        e.Graphics.DrawImage(this.LeftArrowImage, new Rectangle(new Point(0, 0), new Size(this.LeftArrowImage.Width, this.Height)));
      Brush brush1 = (Brush) new SolidBrush(this.moChannelColor);
      Brush brush2 = (Brush) new SolidBrush(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      e.Graphics.FillRectangle(brush2, new Rectangle(this.LeftArrowImage.Width, 0, this.Width - this.RightArrowImage.Width, 1));
      e.Graphics.FillRectangle(brush2, new Rectangle(this.LeftArrowImage.Width, this.Height - 1, this.Width - this.RightArrowImage.Width, this.Height));
      e.Graphics.FillRectangle(brush1, new Rectangle(this.LeftArrowImage.Width, 1, this.Width - this.RightArrowImage.Width, this.Height - 2));
      int num1 = this.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
      float num2 = (float) this.LargeChange / (float) this.Maximum * (float) num1;
      int num3 = (int) num2;
      if (num3 > num1)
      {
        num3 = num1;
        num2 = (float) num1;
      }
      if (num3 < 56)
        num2 = 56f;
      float num4 = (float) (((double) num2 - (double) (this.ThumbMiddleImage.Width + this.ThumbRightImage.Width + this.ThumbRightImage.Width)) / 2.0);
      int width = (int) num4;
      int x1 = this.moThumbRight + this.LeftArrowImage.Width;
      e.Graphics.DrawImage(this.ThumbLeftImage, new Rectangle(x1, 1, this.ThumbLeftImage.Width, this.Height - 2));
      int x2 = x1 + this.ThumbLeftImage.Width;
      Rectangle rect = new Rectangle(x2, 1, width, this.Height - 2);
      e.Graphics.DrawImage(this.ThumbLeftSpanImage, (float) x2, 1f, num4 * 2f, (float) this.Height - 2f);
      int x3 = x2 + width;
      e.Graphics.DrawImage(this.ThumbMiddleImage, new Rectangle(x3, 1, this.ThumbMiddleImage.Width, this.Height - 2));
      int x4 = x3 + this.ThumbMiddleImage.Width;
      rect = new Rectangle(x4, 1, width * 2, this.Height - 2);
      e.Graphics.DrawImage(this.ThumbRightSpanImage, rect);
      int x5 = x4 + width;
      e.Graphics.DrawImage(this.ThumbRightImage, new Rectangle(x5, 1, width, this.Height - 2));
      if (this.RightArrowImage == null)
        return;
      e.Graphics.DrawImage(this.RightArrowImage, new Rectangle(new Point(this.Width - this.RightArrowImage.Width, 0), new Size(this.RightArrowImage.Width, this.Height)));
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
      this.Name = nameof (CustomHScrollbar);
      this.MouseDown += new MouseEventHandler(this.CustomScrollbar_MouseDown);
      this.MouseMove += new MouseEventHandler(this.CustomScrollbar_MouseMove);
      this.MouseUp += new MouseEventHandler(this.CustomScrollbar_MouseUp);
      this.ResumeLayout(false);
    }

    private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
    {
      Point client = this.PointToClient(Cursor.Position);
      int num1 = this.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
      int width = (int) ((float) this.LargeChange / (float) this.Maximum * (float) num1);
      float num2;
      if (width > num1)
      {
        width = num1;
        num2 = (float) num1;
      }
      if (width < 56)
      {
        width = 56;
        num2 = 56f;
      }
      int x = this.moThumbRight + this.LeftArrowImage.Width;
      if (new Rectangle(new Point(x, 1), new Size(width, this.ThumbMiddleImage.Height)).Contains(client))
      {
        this.nClickPoint = client.Y - x;
        this.moThumbDown = true;
      }
      if (new Rectangle(new Point(1, 0), new Size(this.LeftArrowImage.Width, this.LeftArrowImage.Height)).Contains(client))
      {
        int num3 = this.Maximum - this.Minimum - this.LargeChange;
        int num4 = num1 - width;
        if (num3 > 0 && num4 > 0)
        {
          if (this.moThumbRight - this.SmallChange < 0)
            this.moThumbRight = 0;
          else
            this.moThumbRight -= this.SmallChange;
          this.moValue = (int) ((float) this.moThumbRight / (float) num4 * (float) (this.Maximum - this.LargeChange));
          if (this.ValueChanged != null)
            this.ValueChanged((object) this, new EventArgs());
          if (this.Scroll != null)
            this.Scroll((object) this, new EventArgs());
          this.Invalidate();
        }
      }
      if (!new Rectangle(new Point(this.LeftArrowImage.Width + num1, 1), new Size(this.LeftArrowImage.Width, this.LeftArrowImage.Height)).Contains(client))
        return;
      int num5 = this.Maximum - this.Minimum - this.LargeChange;
      int num6 = num1 - width;
      if (num5 > 0 && num6 > 0)
      {
        if (this.moThumbRight + this.SmallChange > num6)
          this.moThumbRight = num6;
        else
          this.moThumbRight += this.SmallChange;
        this.moValue = (int) ((float) this.moThumbRight / (float) num6 * (float) (this.Maximum - this.LargeChange));
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

    private void MoveThumb(int x)
    {
      int num1 = this.Maximum - this.Minimum;
      int num2 = this.Width - (this.LeftArrowImage.Width + this.RightArrowImage.Width);
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
      int num6 = x - (this.LeftArrowImage.Width + nClickPoint);
      int num7;
      this.moThumbRight = num6 >= 0 ? (num6 <= num5 ? x - (this.LeftArrowImage.Width + nClickPoint) : (num7 = num5)) : (num7 = 0);
      this.moValue = (int) ((float) this.moThumbRight / (float) num5 * (float) (this.Maximum - this.LargeChange));
      Application.DoEvents();
      this.Invalidate();
    }

    private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.moThumbDown)
        this.moThumbDragging = true;
      if (this.moThumbDragging)
        this.MoveThumb(e.X);
      if (this.ValueChanged != null)
        this.ValueChanged((object) this, new EventArgs());
      if (this.Scroll == null)
        return;
      this.Scroll((object) this, new EventArgs());
    }
  }
}
