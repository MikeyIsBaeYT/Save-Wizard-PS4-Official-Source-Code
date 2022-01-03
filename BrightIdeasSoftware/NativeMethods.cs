// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.NativeMethods
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  internal static class NativeMethods
  {
    private const int LVM_FIRST = 4096;
    private const int LVM_GETCOLUMN = 4191;
    private const int LVM_GETCOUNTPERPAGE = 4136;
    private const int LVM_GETGROUPINFO = 4245;
    private const int LVM_GETGROUPSTATE = 4188;
    private const int LVM_GETHEADER = 4127;
    private const int LVM_GETTOOLTIPS = 4174;
    private const int LVM_GETTOPINDEX = 4135;
    private const int LVM_HITTEST = 4114;
    private const int LVM_INSERTGROUP = 4241;
    private const int LVM_REMOVEALLGROUPS = 4256;
    private const int LVM_SCROLL = 4116;
    private const int LVM_SETBKIMAGE = 4234;
    private const int LVM_SETCOLUMN = 4192;
    private const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;
    private const int LVM_SETGROUPINFO = 4243;
    private const int LVM_SETGROUPMETRICS = 4251;
    private const int LVM_SETIMAGELIST = 4099;
    private const int LVM_SETITEM = 4172;
    private const int LVM_SETITEMCOUNT = 4143;
    private const int LVM_SETITEMSTATE = 4139;
    private const int LVM_SETSELECTEDCOLUMN = 4236;
    private const int LVM_SETTOOLTIPS = 4170;
    private const int LVM_SUBITEMHITTEST = 4153;
    private const int LVS_EX_SUBITEMIMAGES = 2;
    private const int LVIF_TEXT = 1;
    private const int LVIF_IMAGE = 2;
    private const int LVIF_PARAM = 4;
    private const int LVIF_STATE = 8;
    private const int LVIF_INDENT = 16;
    private const int LVIF_NORECOMPUTE = 2048;
    private const int LVIS_SELECTED = 2;
    private const int LVCF_FMT = 1;
    private const int LVCF_WIDTH = 2;
    private const int LVCF_TEXT = 4;
    private const int LVCF_SUBITEM = 8;
    private const int LVCF_IMAGE = 16;
    private const int LVCF_ORDER = 32;
    private const int LVCFMT_LEFT = 0;
    private const int LVCFMT_RIGHT = 1;
    private const int LVCFMT_CENTER = 2;
    private const int LVCFMT_JUSTIFYMASK = 3;
    private const int LVCFMT_IMAGE = 2048;
    private const int LVCFMT_BITMAP_ON_RIGHT = 4096;
    private const int LVCFMT_COL_HAS_IMAGES = 32768;
    private const int LVBKIF_SOURCE_NONE = 0;
    private const int LVBKIF_SOURCE_HBITMAP = 1;
    private const int LVBKIF_SOURCE_URL = 2;
    private const int LVBKIF_SOURCE_MASK = 3;
    private const int LVBKIF_STYLE_NORMAL = 0;
    private const int LVBKIF_STYLE_TILE = 16;
    private const int LVBKIF_STYLE_MASK = 16;
    private const int LVBKIF_FLAG_TILEOFFSET = 256;
    private const int LVBKIF_TYPE_WATERMARK = 268435456;
    private const int LVBKIF_FLAG_ALPHABLEND = 536870912;
    private const int LVSICF_NOINVALIDATEALL = 1;
    private const int LVSICF_NOSCROLL = 2;
    private const int HDM_FIRST = 4608;
    private const int HDM_HITTEST = 4614;
    private const int HDM_GETITEMRECT = 4615;
    private const int HDM_GETITEM = 4619;
    private const int HDM_SETITEM = 4620;
    private const int HDI_WIDTH = 1;
    private const int HDI_TEXT = 2;
    private const int HDI_FORMAT = 4;
    private const int HDI_BITMAP = 16;
    private const int HDI_IMAGE = 32;
    private const int HDF_LEFT = 0;
    private const int HDF_RIGHT = 1;
    private const int HDF_CENTER = 2;
    private const int HDF_JUSTIFYMASK = 3;
    private const int HDF_RTLREADING = 4;
    private const int HDF_STRING = 16384;
    private const int HDF_BITMAP = 8192;
    private const int HDF_BITMAP_ON_RIGHT = 4096;
    private const int HDF_IMAGE = 2048;
    private const int HDF_SORTUP = 1024;
    private const int HDF_SORTDOWN = 512;
    private const int SB_HORZ = 0;
    private const int SB_VERT = 1;
    private const int SB_CTL = 2;
    private const int SB_BOTH = 3;
    private const int SIF_RANGE = 1;
    private const int SIF_PAGE = 2;
    private const int SIF_POS = 4;
    private const int SIF_DISABLENOSCROLL = 8;
    private const int SIF_TRACKPOS = 16;
    private const int SIF_ALL = 23;
    private const int ILD_NORMAL = 0;
    private const int ILD_TRANSPARENT = 1;
    private const int ILD_MASK = 16;
    private const int ILD_IMAGE = 32;
    private const int ILD_BLEND25 = 2;
    private const int ILD_BLEND50 = 4;
    private const int SWP_NOSIZE = 1;
    private const int SWP_NOMOVE = 2;
    private const int SWP_NOZORDER = 4;
    private const int SWP_NOREDRAW = 8;
    private const int SWP_NOACTIVATE = 16;
    public const int SWP_FRAMECHANGED = 32;
    private const int SWP_ZORDERONLY = 27;
    private const int SWP_SIZEONLY = 30;
    private const int SWP_UPDATE_FRAME = 55;
    private const int MAX_LINKID_TEXT = 48;
    private const int L_MAX_URL_LENGTH = 2084;
    private const uint CLR_DEFAULT = 4278190080;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageLVItem(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.LVITEM lvi);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.LVHITTESTINFO ht);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageRECT(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.RECT r);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessageHDItem(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.HDITEM hdi);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageHDHITTESTINFO(
      IntPtr hWnd,
      int Msg,
      IntPtr wParam,
      [In, Out] NativeMethods.HDHITTESTINFO lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTOOLINFO(
      IntPtr hWnd,
      int Msg,
      int wParam,
      NativeMethods.TOOLINFO lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageLVBKIMAGE(
      IntPtr hWnd,
      int Msg,
      int wParam,
      ref NativeMethods.LVBKIMAGE lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageString(
      IntPtr hWnd,
      int Msg,
      int wParam,
      string lParam);

    [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageIUnknown(
      IntPtr hWnd,
      int msg,
      [MarshalAs(UnmanagedType.IUnknown)] object wParam,
      int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.LVGROUP lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.LVGROUP2 lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int msg,
      int wParam,
      ref NativeMethods.LVGROUPMETRICS lParam);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr objectHandle);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetClientRect(IntPtr hWnd, ref Rectangle r);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetScrollInfo(
      IntPtr hWnd,
      int fnBar,
      NativeMethods.SCROLLINFO scrollInfo);

    [DllImport("user32.dll", EntryPoint = "GetUpdateRect", CharSet = CharSet.Auto)]
    private static extern bool GetUpdateRectInternal(
      IntPtr hWnd,
      ref Rectangle r,
      bool eraseBackground);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    private static extern bool ImageList_Draw(
      IntPtr himl,
      int i,
      IntPtr hdcDst,
      int x,
      int y,
      int fStyle);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    private static extern bool ImageList_DrawIndirect(ref NativeMethods.IMAGELISTDRAWPARAMS parms);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int X,
      int Y,
      int cx,
      int cy,
      uint uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle r);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "ValidateRect", CharSet = CharSet.Auto)]
    private static extern IntPtr ValidatedRectInternal(IntPtr hWnd, ref Rectangle r);

    public static bool SetBackgroundImage(
      ListView lv,
      Image image,
      bool isWatermark,
      bool isTiled,
      int xOffset,
      int yOffset)
    {
      NativeMethods.LVBKIMAGE lParam = new NativeMethods.LVBKIMAGE();
      lParam.ulFlags = 268435456;
      NativeMethods.SendMessageLVBKIMAGE(lv.Handle, 4234, 0, ref lParam);
      lParam.ulFlags = 1;
      IntPtr num = NativeMethods.SendMessageLVBKIMAGE(lv.Handle, 4234, 0, ref lParam);
      if (image is Bitmap bitmap)
      {
        lParam.hBmp = bitmap.GetHbitmap();
        lParam.ulFlags = isWatermark ? 268435456 : (isTiled ? 17 : 1);
        lParam.xOffset = xOffset;
        lParam.yOffset = yOffset;
        num = NativeMethods.SendMessageLVBKIMAGE(lv.Handle, 4234, 0, ref lParam);
      }
      return num != IntPtr.Zero;
    }

    public static bool DrawImageList(
      Graphics g,
      ImageList il,
      int index,
      int x,
      int y,
      bool isSelected,
      bool isDisabled)
    {
      NativeMethods.ImageListDrawItemConstants flags = (NativeMethods.ImageListDrawItemConstants) ((isSelected ? 4 : 0) | 1);
      NativeMethods.ImageListDrawStateConstants stateFlags = isDisabled ? NativeMethods.ImageListDrawStateConstants.ILS_SATURATE : NativeMethods.ImageListDrawStateConstants.ILS_NORMAL;
      try
      {
        IntPtr hdc = g.GetHdc();
        return NativeMethods.DrawImage(il, hdc, index, x, y, flags, 0, 0, stateFlags);
      }
      finally
      {
        g.ReleaseHdc();
      }
    }

    public static bool DrawImage(
      ImageList il,
      IntPtr hdc,
      int index,
      int x,
      int y,
      NativeMethods.ImageListDrawItemConstants flags,
      int cx,
      int cy,
      NativeMethods.ImageListDrawStateConstants stateFlags)
    {
      NativeMethods.IMAGELISTDRAWPARAMS parms = new NativeMethods.IMAGELISTDRAWPARAMS()
      {
        hdcDst = hdc
      };
      parms.cbSize = Marshal.SizeOf(parms.GetType());
      parms.i = index;
      parms.x = x;
      parms.y = y;
      parms.cx = cx;
      parms.cy = cy;
      parms.rgbFg = 4278190080U;
      parms.fStyle = (uint) flags;
      parms.fState = (uint) stateFlags;
      parms.himl = il.Handle;
      return NativeMethods.ImageList_DrawIndirect(ref parms);
    }

    public static void ForceSubItemImagesExStyle(ListView list) => NativeMethods.SendMessage(list.Handle, 4150, 2, 2);

    public static void SetItemCount(ListView list, int count) => NativeMethods.SendMessage(list.Handle, 4143, count, 2);

    public static void SetExtendedStyle(ListView list, int style, int styleMask) => NativeMethods.SendMessage(list.Handle, 4150, styleMask, style);

    public static int GetCountPerPage(ListView list) => (int) NativeMethods.SendMessage(list.Handle, 4136, 0, 0);

    public static void SetSubItemImage(
      ListView list,
      int itemIndex,
      int subItemIndex,
      int imageIndex)
    {
      NativeMethods.SendMessageLVItem(list.Handle, 4172, 0, ref new NativeMethods.LVITEM()
      {
        mask = 2,
        iItem = itemIndex,
        iSubItem = subItemIndex,
        iImage = imageIndex
      });
    }

    public static void SetColumnImage(
      ListView list,
      int columnIndex,
      SortOrder order,
      int imageIndex)
    {
      IntPtr headerControl = NativeMethods.GetHeaderControl(list);
      if (headerControl.ToInt32() == 0)
        return;
      NativeMethods.HDITEM hdi = new NativeMethods.HDITEM();
      hdi.mask = 4;
      IntPtr num = NativeMethods.SendMessageHDItem(headerControl, 4619, columnIndex, ref hdi);
      hdi.fmt &= -7681;
      if (NativeMethods.HasBuiltinSortIndicators())
      {
        if (order == SortOrder.Ascending)
          hdi.fmt |= 1024;
        if (order == SortOrder.Descending)
          hdi.fmt |= 512;
      }
      else
      {
        hdi.mask |= 32;
        hdi.fmt |= 6144;
        hdi.iImage = imageIndex;
      }
      num = NativeMethods.SendMessageHDItem(headerControl, 4620, columnIndex, ref hdi);
    }

    public static bool HasBuiltinSortIndicators() => OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != (Version) null;

    public static Rectangle GetUpdateRect(Control cntl)
    {
      Rectangle r = new Rectangle();
      NativeMethods.GetUpdateRectInternal(cntl.Handle, ref r, false);
      return r;
    }

    public static void ValidateRect(Control cntl, Rectangle r) => NativeMethods.ValidatedRectInternal(cntl.Handle, ref r);

    public static void SelectAllItems(ListView list) => NativeMethods.SetItemState(list, -1, 2, 2);

    public static void DeselectAllItems(ListView list) => NativeMethods.SetItemState(list, -1, 2, 0);

    public static void DeselectOneItem(ListView list, int index) => NativeMethods.SetItemState(list, index, 2, 0);

    public static void SetItemState(ListView list, int itemIndex, int mask, int value) => NativeMethods.SendMessageLVItem(list.Handle, 4139, itemIndex, ref new NativeMethods.LVITEM()
    {
      stateMask = mask,
      state = value
    });

    public static bool Scroll(ListView list, int dx, int dy) => NativeMethods.SendMessage(list.Handle, 4116, dx, dy) != IntPtr.Zero;

    public static IntPtr GetHeaderControl(ListView list) => NativeMethods.SendMessage(list.Handle, 4127, 0, 0);

    public static Point GetColumnSides(ObjectListView lv, int columnIndex)
    {
      Point point = new Point(-1, -1);
      IntPtr headerControl = NativeMethods.GetHeaderControl((ListView) lv);
      if (headerControl == IntPtr.Zero)
        return new Point(-1, -1);
      NativeMethods.RECT r = new NativeMethods.RECT();
      NativeMethods.SendMessageRECT(headerControl, 4615, columnIndex, ref r);
      return new Point(r.left, r.right);
    }

    public static Point GetScrolledColumnSides(ListView lv, int columnIndex)
    {
      IntPtr headerControl = NativeMethods.GetHeaderControl(lv);
      if (headerControl == IntPtr.Zero)
        return new Point(-1, -1);
      NativeMethods.RECT r = new NativeMethods.RECT();
      NativeMethods.SendMessageRECT(headerControl, 4615, columnIndex, ref r);
      int scrollPosition = NativeMethods.GetScrollPosition(lv, true);
      return new Point(r.left - scrollPosition, r.right - scrollPosition);
    }

    public static int GetColumnUnderPoint(IntPtr handle, Point pt) => NativeMethods.HeaderControlHitTest(handle, pt, 6);

    private static int HeaderControlHitTest(IntPtr handle, Point pt, int flag)
    {
      NativeMethods.HDHITTESTINFO lParam = new NativeMethods.HDHITTESTINFO();
      lParam.pt_x = pt.X;
      lParam.pt_y = pt.Y;
      NativeMethods.SendMessageHDHITTESTINFO(handle, 4614, IntPtr.Zero, lParam);
      return (uint) (lParam.flags & flag) > 0U ? lParam.iItem : -1;
    }

    public static int GetDividerUnderPoint(IntPtr handle, Point pt) => NativeMethods.HeaderControlHitTest(handle, pt, 4);

    public static int GetScrollPosition(ListView lv, bool horizontalBar)
    {
      int fnBar = horizontalBar ? 0 : 1;
      NativeMethods.SCROLLINFO scrollInfo = new NativeMethods.SCROLLINFO();
      scrollInfo.fMask = 4;
      return NativeMethods.GetScrollInfo(lv.Handle, fnBar, scrollInfo) ? scrollInfo.nPos : -1;
    }

    public static bool ChangeZOrder(IWin32Window toBeMoved, IWin32Window reference) => NativeMethods.SetWindowPos(toBeMoved.Handle, reference.Handle, 0, 0, 0, 0, 27U);

    public static bool MakeTopMost(IWin32Window toBeMoved)
    {
      IntPtr hWndInsertAfter = (IntPtr) -1;
      return NativeMethods.SetWindowPos(toBeMoved.Handle, hWndInsertAfter, 0, 0, 0, 0, 27U);
    }

    public static bool ChangeSize(IWin32Window toBeMoved, int width, int height) => NativeMethods.SetWindowPos(toBeMoved.Handle, IntPtr.Zero, 0, 0, width, height, 30U);

    public static void ShowWithoutActivate(IWin32Window win) => NativeMethods.ShowWindow(win.Handle, 8);

    public static void SetSelectedColumn(ListView objectListView, ColumnHeader value) => NativeMethods.SendMessage(objectListView.Handle, 4236, value == null ? -1 : value.Index, 0);

    public static int GetTopIndex(ListView lv) => (int) NativeMethods.SendMessage(lv.Handle, 4135, 0, 0);

    public static IntPtr GetTooltipControl(ListView lv) => NativeMethods.SendMessage(lv.Handle, 4174, 0, 0);

    public static IntPtr SetTooltipControl(ListView lv, ToolTipControl tooltip) => NativeMethods.SendMessage(lv.Handle, 4170, 0, tooltip.Handle);

    public static bool HasHorizontalScrollBar(ListView lv) => (uint) (NativeMethods.GetWindowLong(lv.Handle, -16) & 1048576) > 0U;

    public static int GetWindowLong(IntPtr hWnd, int nIndex) => IntPtr.Size == 4 ? (int) NativeMethods.GetWindowLong32(hWnd, nIndex) : (int) (long) NativeMethods.GetWindowLongPtr64(hWnd, nIndex);

    public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong) => IntPtr.Size == 4 ? (int) NativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : (int) (long) NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetBkColor(IntPtr hDC, int clr);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetTextColor(IntPtr hDC, int crColor);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

    [DllImport("uxtheme.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowTheme(IntPtr hWnd, string subApp, string subIdList);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool InvalidateRect(IntPtr hWnd, int ignored, bool erase);

    public static int GetGroupInfo(
      ObjectListView olv,
      int groupId,
      ref NativeMethods.LVGROUP2 group)
    {
      return (int) NativeMethods.SendMessage(olv.Handle, 4245, groupId, ref group);
    }

    public static GroupState GetGroupState(
      ObjectListView olv,
      int groupId,
      GroupState mask)
    {
      return (GroupState) (int) NativeMethods.SendMessage(olv.Handle, 4188, groupId, (int) mask);
    }

    public static int InsertGroup(ObjectListView olv, NativeMethods.LVGROUP2 group) => (int) NativeMethods.SendMessage(olv.Handle, 4241, -1, ref group);

    public static int SetGroupInfo(ObjectListView olv, int groupId, NativeMethods.LVGROUP2 group) => (int) NativeMethods.SendMessage(olv.Handle, 4243, groupId, ref group);

    public static int SetGroupMetrics(ObjectListView olv, NativeMethods.LVGROUPMETRICS metrics) => (int) NativeMethods.SendMessage(olv.Handle, 4251, 0, ref metrics);

    public static void ClearGroups(VirtualObjectListView virtualObjectListView) => NativeMethods.SendMessage(virtualObjectListView.Handle, 4256, 0, 0);

    public static void SetGroupImageList(ObjectListView olv, ImageList il)
    {
      IntPtr lParam = IntPtr.Zero;
      if (il != null)
        lParam = il.Handle;
      NativeMethods.SendMessage(olv.Handle, 4099, 3, lParam);
    }

    public static int HitTest(ObjectListView olv, ref NativeMethods.LVHITTESTINFO hittest) => (int) NativeMethods.SendMessage(olv.Handle, olv.View == View.Details ? 4153 : 4114, -1, ref hittest);

    public struct HDITEM
    {
      public int mask;
      public int cxy;
      public IntPtr pszText;
      public IntPtr hbm;
      public int cchTextMax;
      public int fmt;
      public IntPtr lParam;
      public int iImage;
      public int iOrder;
      public int type;
      public IntPtr pvFilter;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class HDHITTESTINFO
    {
      public int pt_x;
      public int pt_y;
      public int flags;
      public int iItem;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class HDLAYOUT
    {
      public IntPtr prc;
      public IntPtr pwpos;
    }

    public struct IMAGELISTDRAWPARAMS
    {
      public int cbSize;
      public IntPtr himl;
      public int i;
      public IntPtr hdcDst;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public int xBitmap;
      public int yBitmap;
      public uint rgbBk;
      public uint rgbFg;
      public uint fStyle;
      public uint dwRop;
      public uint fState;
      public uint Frame;
      public uint crEffect;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVBKIMAGE
    {
      public int ulFlags;
      public IntPtr hBmp;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszImage;
      public int cchImageMax;
      public int xOffset;
      public int yOffset;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVCOLUMN
    {
      public int mask;
      public int fmt;
      public int cx;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszText;
      public int cchTextMax;
      public int iSubItem;
      public int iImage;
      public int iOrder;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVFINDINFO
    {
      public int flags;
      public string psz;
      public IntPtr lParam;
      public int ptX;
      public int ptY;
      public int vkDirection;
    }

    public struct LVGROUP
    {
      public uint cbSize;
      public uint mask;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszHeader;
      public int cchHeader;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszFooter;
      public int cchFooter;
      public int iGroupId;
      public uint stateMask;
      public uint state;
      public uint uAlign;
    }

    public struct LVGROUP2
    {
      public uint cbSize;
      public uint mask;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszHeader;
      public uint cchHeader;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszFooter;
      public int cchFooter;
      public int iGroupId;
      public uint stateMask;
      public uint state;
      public uint uAlign;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszSubtitle;
      public uint cchSubtitle;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszTask;
      public uint cchTask;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszDescriptionTop;
      public uint cchDescriptionTop;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszDescriptionBottom;
      public uint cchDescriptionBottom;
      public int iTitleImage;
      public int iExtendedImage;
      public int iFirstItem;
      public int cItems;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszSubsetTitle;
      public uint cchSubsetTitle;
    }

    public struct LVGROUPMETRICS
    {
      public uint cbSize;
      public uint mask;
      public uint Left;
      public uint Top;
      public uint Right;
      public uint Bottom;
      public int crLeft;
      public int crTop;
      public int crRight;
      public int crBottom;
      public int crHeader;
      public int crFooter;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVHITTESTINFO
    {
      public int pt_x;
      public int pt_y;
      public int flags;
      public int iItem;
      public int iSubItem;
      public int iGroup;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVITEM
    {
      public int mask;
      public int iItem;
      public int iSubItem;
      public int state;
      public int stateMask;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string pszText;
      public int cchTextMax;
      public int iImage;
      public IntPtr lParam;
      public int iIndent;
      public int iGroupId;
      public int cColumns;
      public IntPtr puColumns;
    }

    public struct NMHDR
    {
      public IntPtr hwndFrom;
      public IntPtr idFrom;
      public int code;
    }

    public struct NMCUSTOMDRAW
    {
      public NativeMethods.NMHDR nmcd;
      public int dwDrawStage;
      public IntPtr hdc;
      public NativeMethods.RECT rc;
      public IntPtr dwItemSpec;
      public int uItemState;
      public IntPtr lItemlParam;
    }

    public struct NMHEADER
    {
      public NativeMethods.NMHDR nhdr;
      public int iItem;
      public int iButton;
      public IntPtr pHDITEM;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LITEM
    {
      public uint mask;
      public int iLink;
      public uint state;
      public uint stateMask;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
      public string szID;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2084)]
      public string szUrl;
    }

    public struct NMLISTVIEW
    {
      public NativeMethods.NMHDR hdr;
      public int iItem;
      public int iSubItem;
      public int uNewState;
      public int uOldState;
      public int uChanged;
      public IntPtr lParam;
    }

    public struct NMLVCUSTOMDRAW
    {
      public NativeMethods.NMCUSTOMDRAW nmcd;
      public int clrText;
      public int clrTextBk;
      public int iSubItem;
      public int dwItemType;
      public int clrFace;
      public int iIconEffect;
      public int iIconPhase;
      public int iPartId;
      public int iStateId;
      public NativeMethods.RECT rcText;
      public uint uAlign;
    }

    public struct NMLVFINDITEM
    {
      public NativeMethods.NMHDR hdr;
      public int iStart;
      public NativeMethods.LVFINDINFO lvfi;
    }

    public struct NMLVGETINFOTIP
    {
      public NativeMethods.NMHDR hdr;
      public int dwFlags;
      public string pszText;
      public int cchTextMax;
      public int iItem;
      public int iSubItem;
      public IntPtr lParam;
    }

    public struct NMLVGROUP
    {
      public NativeMethods.NMHDR hdr;
      public int iGroupId;
      public uint uNewState;
      public uint uOldState;
    }

    public struct NMLVLINK
    {
      public NativeMethods.NMHDR hdr;
      public NativeMethods.LITEM link;
      public int iItem;
      public int iSubItem;
    }

    public struct NMLVSCROLL
    {
      public NativeMethods.NMHDR hdr;
      public int dx;
      public int dy;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NMTTDISPINFO
    {
      public NativeMethods.NMHDR hdr;
      [MarshalAs(UnmanagedType.LPTStr)]
      public string lpszText;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szText;
      public IntPtr hinst;
      public int uFlags;
      public IntPtr lParam;
    }

    public struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SCROLLINFO
    {
      public int cbSize = Marshal.SizeOf(typeof (NativeMethods.SCROLLINFO));
      public int fMask;
      public int nMin;
      public int nMax;
      public int nPage;
      public int nPos;
      public int nTrackPos;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TOOLINFO
    {
      public int cbSize = Marshal.SizeOf(typeof (NativeMethods.TOOLINFO));
      public int uFlags;
      public IntPtr hwnd;
      public IntPtr uId;
      public NativeMethods.RECT rect;
      public IntPtr hinst = IntPtr.Zero;
      public IntPtr lpszText;
      public IntPtr lParam = IntPtr.Zero;
    }

    public struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public int flags;
    }

    [Flags]
    public enum ImageListDrawItemConstants
    {
      ILD_NORMAL = 0,
      ILD_TRANSPARENT = 1,
      ILD_BLEND25 = 2,
      ILD_SELECTED = 4,
      ILD_MASK = 16, // 0x00000010
      ILD_IMAGE = 32, // 0x00000020
      ILD_ROP = 64, // 0x00000040
      ILD_PRESERVEALPHA = 4096, // 0x00001000
      ILD_SCALE = 8192, // 0x00002000
      ILD_DPISCALE = 16384, // 0x00004000
    }

    [Flags]
    public enum ImageListDrawStateConstants
    {
      ILS_NORMAL = 0,
      ILS_GLOW = 1,
      ILS_SHADOW = 2,
      ILS_SATURATE = 4,
      ILS_ALPHA = 8,
    }

    public struct LVITEMINDEX
    {
      public int iItem;
      public int iGroup;
    }

    public struct POINT
    {
      public int x;
      public int y;
    }
  }
}
