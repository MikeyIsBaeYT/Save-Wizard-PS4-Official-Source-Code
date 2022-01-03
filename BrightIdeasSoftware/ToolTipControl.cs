// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ToolTipControl
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class ToolTipControl : NativeWindow
  {
    private const int GWL_STYLE = -16;
    private const int WM_GETFONT = 49;
    private const int WM_SETFONT = 48;
    private const int WS_BORDER = 8388608;
    private const int WS_EX_TOPMOST = 8;
    private const int TTM_ADDTOOL = 1074;
    private const int TTM_ADJUSTRECT = 1055;
    private const int TTM_DELTOOL = 1075;
    private const int TTM_GETBUBBLESIZE = 1054;
    private const int TTM_GETCURRENTTOOL = 1083;
    private const int TTM_GETTIPBKCOLOR = 1046;
    private const int TTM_GETTIPTEXTCOLOR = 1047;
    private const int TTM_GETDELAYTIME = 1045;
    private const int TTM_NEWTOOLRECT = 1076;
    private const int TTM_POP = 1052;
    private const int TTM_SETDELAYTIME = 1027;
    private const int TTM_SETMAXTIPWIDTH = 1048;
    private const int TTM_SETTIPBKCOLOR = 1043;
    private const int TTM_SETTIPTEXTCOLOR = 1044;
    private const int TTM_SETTITLE = 1057;
    private const int TTM_SETTOOLINFO = 1078;
    private const int TTF_IDISHWND = 1;
    private const int TTF_CENTERTIP = 2;
    private const int TTF_RTLREADING = 4;
    private const int TTF_SUBCLASS = 16;
    private const int TTF_PARSELINKS = 4096;
    private const int TTS_NOPREFIX = 2;
    private const int TTS_BALLOON = 64;
    private const int TTS_USEVISUALSTYLE = 256;
    private const int TTN_FIRST = -520;
    public const int TTN_SHOW = -521;
    public const int TTN_POP = -522;
    public const int TTN_LINKCLICK = -523;
    public const int TTN_GETDISPINFO = -530;
    private const int TTDT_AUTOMATIC = 0;
    private const int TTDT_RESHOW = 1;
    private const int TTDT_AUTOPOP = 2;
    private const int TTDT_INITIAL = 3;
    private bool hasBorder = true;
    private string title;
    private ToolTipControl.StandardIcons standardIcon;
    private Font font;
    private Hashtable settings;

    internal int WindowStyle
    {
      get => NativeMethods.GetWindowLong(this.Handle, -16);
      set => NativeMethods.SetWindowLong(this.Handle, -16, value);
    }

    public bool IsBalloon
    {
      get => (this.WindowStyle & 64) == 64;
      set
      {
        if (this.IsBalloon == value)
          return;
        int windowStyle = this.WindowStyle;
        int num;
        if (value)
        {
          num = windowStyle | 320;
          if (!ObjectListView.IsVistaOrLater)
            num &= -8388609;
        }
        else
        {
          num = windowStyle & -321;
          if (!ObjectListView.IsVistaOrLater)
          {
            if (this.hasBorder)
              num |= 8388608;
            else
              num &= -8388609;
          }
        }
        this.WindowStyle = num;
      }
    }

    public bool HasBorder
    {
      get => this.hasBorder;
      set
      {
        if (this.hasBorder == value)
          return;
        if (value)
          this.WindowStyle |= 8388608;
        else
          this.WindowStyle &= -8388609;
      }
    }

    public Color BackColor
    {
      get => ColorTranslator.FromWin32((int) NativeMethods.SendMessage(this.Handle, 1046, 0, 0));
      set
      {
        if (ObjectListView.IsVistaOrLater)
          return;
        NativeMethods.SendMessage(this.Handle, 1043, ColorTranslator.ToWin32(value), 0);
      }
    }

    public Color ForeColor
    {
      get => ColorTranslator.FromWin32((int) NativeMethods.SendMessage(this.Handle, 1047, 0, 0));
      set
      {
        if (ObjectListView.IsVistaOrLater)
          return;
        NativeMethods.SendMessage(this.Handle, 1044, ColorTranslator.ToWin32(value), 0);
      }
    }

    public string Title
    {
      get => this.title;
      set
      {
        this.title = !string.IsNullOrEmpty(value) ? (value.Length < 100 ? value : value.Substring(0, 99)) : string.Empty;
        NativeMethods.SendMessageString(this.Handle, 1057, (int) this.standardIcon, this.title);
      }
    }

    public ToolTipControl.StandardIcons StandardIcon
    {
      get => this.standardIcon;
      set
      {
        this.standardIcon = value;
        NativeMethods.SendMessageString(this.Handle, 1057, (int) this.standardIcon, this.title);
      }
    }

    public Font Font
    {
      get
      {
        IntPtr hfont = NativeMethods.SendMessage(this.Handle, 49, 0, 0);
        return hfont == IntPtr.Zero ? Control.DefaultFont : Font.FromHfont(hfont);
      }
      set
      {
        Font font = value ?? Control.DefaultFont;
        if (font == this.font)
          return;
        this.font = font;
        NativeMethods.SendMessage(this.Handle, 48, this.font.ToHfont(), 0);
      }
    }

    public int AutoPopDelay
    {
      get => this.GetDelayTime(2);
      set => this.SetDelayTime(2, value);
    }

    public int InitialDelay
    {
      get => this.GetDelayTime(3);
      set => this.SetDelayTime(3, value);
    }

    public int ReshowDelay
    {
      get => this.GetDelayTime(1);
      set => this.SetDelayTime(1, value);
    }

    private int GetDelayTime(int which) => (int) NativeMethods.SendMessage(this.Handle, 1045, which, 0);

    private void SetDelayTime(int which, int value) => NativeMethods.SendMessage(this.Handle, 1027, which, value);

    public void Create(IntPtr parentHandle)
    {
      if (this.Handle != IntPtr.Zero)
        return;
      this.CreateHandle(new CreateParams()
      {
        ClassName = "tooltips_class32",
        Style = 2,
        ExStyle = 8,
        Parent = parentHandle
      });
      this.SetMaxWidth();
    }

    public void PushSettings()
    {
      if (this.settings != null)
        return;
      this.settings = new Hashtable();
      this.settings[(object) "IsBalloon"] = (object) this.IsBalloon;
      this.settings[(object) "HasBorder"] = (object) this.HasBorder;
      this.settings[(object) "BackColor"] = (object) this.BackColor;
      this.settings[(object) "ForeColor"] = (object) this.ForeColor;
      this.settings[(object) "Title"] = (object) this.Title;
      this.settings[(object) "StandardIcon"] = (object) this.StandardIcon;
      this.settings[(object) "AutoPopDelay"] = (object) this.AutoPopDelay;
      this.settings[(object) "InitialDelay"] = (object) this.InitialDelay;
      this.settings[(object) "ReshowDelay"] = (object) this.ReshowDelay;
      this.settings[(object) "Font"] = (object) this.Font;
    }

    public void PopSettings()
    {
      if (this.settings == null)
        return;
      this.IsBalloon = (bool) this.settings[(object) "IsBalloon"];
      this.HasBorder = (bool) this.settings[(object) "HasBorder"];
      this.BackColor = (Color) this.settings[(object) "BackColor"];
      this.ForeColor = (Color) this.settings[(object) "ForeColor"];
      this.Title = (string) this.settings[(object) "Title"];
      this.StandardIcon = (ToolTipControl.StandardIcons) this.settings[(object) "StandardIcon"];
      this.AutoPopDelay = (int) this.settings[(object) "AutoPopDelay"];
      this.InitialDelay = (int) this.settings[(object) "InitialDelay"];
      this.ReshowDelay = (int) this.settings[(object) "ReshowDelay"];
      this.Font = (Font) this.settings[(object) "Font"];
      this.settings = (Hashtable) null;
    }

    public void AddTool(IWin32Window window) => NativeMethods.SendMessageTOOLINFO(this.Handle, 1074, 0, this.MakeToolInfoStruct(window));

    public void PopToolTip(IWin32Window window) => NativeMethods.SendMessage(this.Handle, 1052, 0, 0);

    public void RemoveToolTip(IWin32Window window) => NativeMethods.SendMessageTOOLINFO(this.Handle, 1075, 0, this.MakeToolInfoStruct(window));

    public void SetMaxWidth() => this.SetMaxWidth(SystemInformation.MaxWindowTrackSize.Width);

    public void SetMaxWidth(int maxWidth) => NativeMethods.SendMessage(this.Handle, 1048, 0, maxWidth);

    private NativeMethods.TOOLINFO MakeToolInfoStruct(IWin32Window window) => new NativeMethods.TOOLINFO()
    {
      hwnd = window.Handle,
      uFlags = 17,
      uId = window.Handle,
      lpszText = (IntPtr) -1
    };

    protected virtual bool HandleNotify(ref Message msg) => false;

    public virtual bool HandleGetDispInfo(ref Message msg)
    {
      this.SetMaxWidth();
      ToolTipShowingEventArgs showingEventArgs = new ToolTipShowingEventArgs();
      showingEventArgs.ToolTipControl = this;
      this.OnShowing(showingEventArgs);
      if (string.IsNullOrEmpty(showingEventArgs.Text))
        return false;
      this.ApplyEventFormatting(showingEventArgs);
      NativeMethods.NMTTDISPINFO lparam = (NativeMethods.NMTTDISPINFO) msg.GetLParam(typeof (NativeMethods.NMTTDISPINFO));
      lparam.lpszText = showingEventArgs.Text;
      lparam.hinst = IntPtr.Zero;
      if (showingEventArgs.RightToLeft == RightToLeft.Yes)
        lparam.uFlags |= 4;
      Marshal.StructureToPtr((object) lparam, msg.LParam, false);
      return true;
    }

    private void ApplyEventFormatting(ToolTipShowingEventArgs args)
    {
      if (!args.IsBalloon.HasValue && !args.BackColor.HasValue && !args.ForeColor.HasValue && args.Title == null && !args.StandardIcon.HasValue && !args.AutoPopDelay.HasValue && args.Font == null)
        return;
      this.PushSettings();
      if (args.IsBalloon.HasValue)
        this.IsBalloon = args.IsBalloon.Value;
      if (args.BackColor.HasValue)
        this.BackColor = args.BackColor.Value;
      if (args.ForeColor.HasValue)
        this.ForeColor = args.ForeColor.Value;
      if (args.StandardIcon.HasValue)
        this.StandardIcon = args.StandardIcon.Value;
      if (args.AutoPopDelay.HasValue)
        this.AutoPopDelay = args.AutoPopDelay.Value;
      if (args.Font != null)
        this.Font = args.Font;
      if (args.Title == null)
        return;
      this.Title = args.Title;
    }

    public virtual bool HandleLinkClick(ref Message msg) => false;

    public virtual bool HandlePop(ref Message msg)
    {
      this.PopSettings();
      return true;
    }

    public virtual bool HandleShow(ref Message msg) => false;

    protected virtual bool HandleReflectNotify(ref Message msg)
    {
      switch (((NativeMethods.NMHEADER) msg.GetLParam(typeof (NativeMethods.NMHEADER))).nhdr.code)
      {
        case -530:
          if (this.HandleGetDispInfo(ref msg))
            return true;
          break;
        case -523:
          if (this.HandleLinkClick(ref msg))
            return true;
          break;
        case -522:
          if (this.HandlePop(ref msg))
            return true;
          break;
        case -521:
          if (this.HandleShow(ref msg))
            return true;
          break;
      }
      return false;
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    protected override void WndProc(ref Message msg)
    {
      switch (msg.Msg)
      {
        case 78:
          if (!this.HandleNotify(ref msg))
            return;
          break;
        case 8270:
          if (!this.HandleReflectNotify(ref msg))
            return;
          break;
      }
      base.WndProc(ref msg);
    }

    public event EventHandler<ToolTipShowingEventArgs> Showing;

    public event EventHandler<EventArgs> Pop;

    protected virtual void OnShowing(ToolTipShowingEventArgs e)
    {
      if (this.Showing == null)
        return;
      this.Showing((object) this, e);
    }

    protected virtual void OnPop(EventArgs e)
    {
      if (this.Pop == null)
        return;
      this.Pop((object) this, e);
    }

    public enum StandardIcons
    {
      None,
      Info,
      Warning,
      Error,
      InfoLarge,
      WarningLarge,
      ErrorLarge,
    }
  }
}
