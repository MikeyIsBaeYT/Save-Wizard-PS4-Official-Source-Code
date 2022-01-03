// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.Adler
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zlib
{
  public sealed class Adler
  {
    private static readonly uint BASE = 65521;
    private static readonly int NMAX = 5552;

    public static uint Adler32(uint adler, byte[] buf, int index, int len)
    {
      if (buf == null)
        return 1;
      uint num1 = adler & (uint) ushort.MaxValue;
      uint num2 = adler >> 16 & (uint) ushort.MaxValue;
      while (len > 0)
      {
        int num3 = len < Adler.NMAX ? len : Adler.NMAX;
        len -= num3;
        for (; num3 >= 16; num3 -= 16)
        {
          uint num4 = num1 + (uint) buf[index++];
          uint num5 = num2 + num4;
          uint num6 = num4 + (uint) buf[index++];
          uint num7 = num5 + num6;
          uint num8 = num6 + (uint) buf[index++];
          uint num9 = num7 + num8;
          uint num10 = num8 + (uint) buf[index++];
          uint num11 = num9 + num10;
          uint num12 = num10 + (uint) buf[index++];
          uint num13 = num11 + num12;
          uint num14 = num12 + (uint) buf[index++];
          uint num15 = num13 + num14;
          uint num16 = num14 + (uint) buf[index++];
          uint num17 = num15 + num16;
          uint num18 = num16 + (uint) buf[index++];
          uint num19 = num17 + num18;
          uint num20 = num18 + (uint) buf[index++];
          uint num21 = num19 + num20;
          uint num22 = num20 + (uint) buf[index++];
          uint num23 = num21 + num22;
          uint num24 = num22 + (uint) buf[index++];
          uint num25 = num23 + num24;
          uint num26 = num24 + (uint) buf[index++];
          uint num27 = num25 + num26;
          uint num28 = num26 + (uint) buf[index++];
          uint num29 = num27 + num28;
          uint num30 = num28 + (uint) buf[index++];
          uint num31 = num29 + num30;
          uint num32 = num30 + (uint) buf[index++];
          uint num33 = num31 + num32;
          num1 = num32 + (uint) buf[index++];
          num2 = num33 + num1;
        }
        if ((uint) num3 > 0U)
        {
          do
          {
            num1 += (uint) buf[index++];
            num2 += num1;
          }
          while ((uint) --num3 > 0U);
        }
        num1 %= Adler.BASE;
        num2 %= Adler.BASE;
      }
      return num2 << 16 | num1;
    }
  }
}
