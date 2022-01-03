// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.CompressionMethod
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace ICSharpCode.SharpZipLib.Zip
{
  public enum CompressionMethod
  {
    Stored = 0,
    Deflated = 8,
    Deflate64 = 9,
    BZip2 = 11, // 0x0000000B
    WinZipAES = 99, // 0x00000063
  }
}
