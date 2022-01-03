// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.ZlibConstants
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zlib
{
  public static class ZlibConstants
  {
    public const int WindowBitsMax = 15;
    public const int WindowBitsDefault = 15;
    public const int Z_OK = 0;
    public const int Z_STREAM_END = 1;
    public const int Z_NEED_DICT = 2;
    public const int Z_STREAM_ERROR = -2;
    public const int Z_DATA_ERROR = -3;
    public const int Z_BUF_ERROR = -5;
    public const int WorkingBufferSizeDefault = 16384;
    public const int WorkingBufferSizeMin = 1024;
  }
}
