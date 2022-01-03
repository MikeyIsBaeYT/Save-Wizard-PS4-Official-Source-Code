// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.NativeMethods
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Runtime.InteropServices;

namespace Be.Windows.Forms
{
  internal static class NativeMethods
  {
    public const int WM_KEYDOWN = 256;
    public const int WM_KEYUP = 257;
    public const int WM_CHAR = 258;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ShowCaret(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyCaret();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetCaretPos(int X, int Y);
  }
}
