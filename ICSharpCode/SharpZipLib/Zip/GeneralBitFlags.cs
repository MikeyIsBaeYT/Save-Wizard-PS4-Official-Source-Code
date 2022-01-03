// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.GeneralBitFlags
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip
{
  [Flags]
  public enum GeneralBitFlags
  {
    Encrypted = 1,
    Method = 6,
    Descriptor = 8,
    ReservedPKware4 = 16, // 0x00000010
    Patched = 32, // 0x00000020
    StrongEncryption = 64, // 0x00000040
    Unused7 = 128, // 0x00000080
    Unused8 = 256, // 0x00000100
    Unused9 = 512, // 0x00000200
    Unused10 = 1024, // 0x00000400
    UnicodeText = 2048, // 0x00000800
    EnhancedCompress = 4096, // 0x00001000
    HeaderMasked = 8192, // 0x00002000
    ReservedPkware14 = 16384, // 0x00004000
    ReservedPkware15 = 32768, // 0x00008000
  }
}
