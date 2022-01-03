// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.Util
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Diagnostics;

namespace Be.Windows.Forms
{
  internal static class Util
  {
    private static bool _designMode = Process.GetCurrentProcess().ProcessName.ToLower() == "devenv";

    public static bool DesignMode => Util._designMode;
  }
}
