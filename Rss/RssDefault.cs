// Decompiled with JetBrains decompiler
// Type: Rss.RssDefault
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public class RssDefault
  {
    public const string String = "";
    public const int Int = -1;
    public static readonly DateTime DateTime = DateTime.MinValue;
    public static readonly Uri Uri = (Uri) null;

    public static string Check(string input) => input == null ? "" : input;

    public static int Check(int input) => input < -1 ? -1 : input;

    public static Uri Check(Uri input) => input == (Uri) null ? RssDefault.Uri : input;
  }
}
