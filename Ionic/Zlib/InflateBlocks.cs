// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InflateBlocks
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Ionic.Zlib
{
  internal sealed class InflateBlocks
  {
    private const int MANY = 1440;
    internal static readonly int[] border = new int[19]
    {
      16,
      17,
      18,
      0,
      8,
      7,
      9,
      6,
      10,
      5,
      11,
      4,
      12,
      3,
      13,
      2,
      14,
      1,
      15
    };
    private InflateBlocks.InflateBlockMode mode;
    internal int left;
    internal int table;
    internal int index;
    internal int[] blens;
    internal int[] bb = new int[1];
    internal int[] tb = new int[1];
    internal InflateCodes codes = new InflateCodes();
    internal int last;
    internal ZlibCodec _codec;
    internal int bitk;
    internal int bitb;
    internal int[] hufts;
    internal byte[] window;
    internal int end;
    internal int readAt;
    internal int writeAt;
    internal object checkfn;
    internal uint check;
    internal InfTree inftree = new InfTree();

    internal InflateBlocks(ZlibCodec codec, object checkfn, int w)
    {
      this._codec = codec;
      this.hufts = new int[4320];
      this.window = new byte[w];
      this.end = w;
      this.checkfn = checkfn;
      this.mode = InflateBlocks.InflateBlockMode.TYPE;
      int num = (int) this.Reset();
    }

    internal uint Reset()
    {
      uint check = this.check;
      this.mode = InflateBlocks.InflateBlockMode.TYPE;
      this.bitk = 0;
      this.bitb = 0;
      this.readAt = this.writeAt = 0;
      if (this.checkfn != null)
        this._codec._Adler32 = this.check = Adler.Adler32(0U, (byte[]) null, 0, 0);
      return check;
    }

    internal int Process(int r)
    {
      int nextIn = this._codec.NextIn;
      int availableBytesIn = this._codec.AvailableBytesIn;
      int num1 = this.bitb;
      int num2 = this.bitk;
      int destinationIndex = this.writeAt;
      int num3 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
      int num4;
      int num5;
      while (true)
      {
        switch (this.mode)
        {
          case InflateBlocks.InflateBlockMode.TYPE:
            for (; num2 < 3; num2 += 8)
            {
              if ((uint) availableBytesIn > 0U)
              {
                r = 0;
                --availableBytesIn;
                num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
              }
              else
              {
                this.bitb = num1;
                this.bitk = num2;
                this._codec.AvailableBytesIn = availableBytesIn;
                this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                this._codec.NextIn = nextIn;
                this.writeAt = destinationIndex;
                return this.Flush(r);
              }
            }
            int num6 = num1 & 7;
            this.last = num6 & 1;
            switch ((uint) num6 >> 1)
            {
              case 0:
                int num7 = num1 >> 3;
                int num8 = num2 - 3;
                int num9 = num8 & 7;
                num1 = num7 >> num9;
                num2 = num8 - num9;
                this.mode = InflateBlocks.InflateBlockMode.LENS;
                break;
              case 1:
                int[] bl1 = new int[1];
                int[] bd1 = new int[1];
                int[][] tl1 = new int[1][];
                int[][] td1 = new int[1][];
                InfTree.inflate_trees_fixed(bl1, bd1, tl1, td1, this._codec);
                this.codes.Init(bl1[0], bd1[0], tl1[0], 0, td1[0], 0);
                num1 >>= 3;
                num2 -= 3;
                this.mode = InflateBlocks.InflateBlockMode.CODES;
                break;
              case 2:
                num1 >>= 3;
                num2 -= 3;
                this.mode = InflateBlocks.InflateBlockMode.TABLE;
                break;
              case 3:
                goto label_9;
            }
            break;
          case InflateBlocks.InflateBlockMode.LENS:
            for (; num2 < 32; num2 += 8)
            {
              if ((uint) availableBytesIn > 0U)
              {
                r = 0;
                --availableBytesIn;
                num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
              }
              else
              {
                this.bitb = num1;
                this.bitk = num2;
                this._codec.AvailableBytesIn = availableBytesIn;
                this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                this._codec.NextIn = nextIn;
                this.writeAt = destinationIndex;
                return this.Flush(r);
              }
            }
            if ((~num1 >> 16 & (int) ushort.MaxValue) == (num1 & (int) ushort.MaxValue))
            {
              this.left = num1 & (int) ushort.MaxValue;
              num1 = num2 = 0;
              this.mode = this.left != 0 ? InflateBlocks.InflateBlockMode.STORED : (this.last != 0 ? InflateBlocks.InflateBlockMode.DRY : InflateBlocks.InflateBlockMode.TYPE);
              break;
            }
            goto label_15;
          case InflateBlocks.InflateBlockMode.STORED:
            if (availableBytesIn != 0)
            {
              if (num3 == 0)
              {
                if (destinationIndex == this.end && (uint) this.readAt > 0U)
                {
                  destinationIndex = 0;
                  num3 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
                }
                if (num3 == 0)
                {
                  this.writeAt = destinationIndex;
                  r = this.Flush(r);
                  destinationIndex = this.writeAt;
                  num3 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
                  if (destinationIndex == this.end && (uint) this.readAt > 0U)
                  {
                    destinationIndex = 0;
                    num3 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
                  }
                  if (num3 == 0)
                    goto label_26;
                }
              }
              r = 0;
              int length = this.left;
              if (length > availableBytesIn)
                length = availableBytesIn;
              if (length > num3)
                length = num3;
              Array.Copy((Array) this._codec.InputBuffer, nextIn, (Array) this.window, destinationIndex, length);
              nextIn += length;
              availableBytesIn -= length;
              destinationIndex += length;
              num3 -= length;
              if ((uint) (this.left -= length) <= 0U)
              {
                this.mode = this.last != 0 ? InflateBlocks.InflateBlockMode.DRY : InflateBlocks.InflateBlockMode.TYPE;
                break;
              }
              break;
            }
            goto label_18;
          case InflateBlocks.InflateBlockMode.TABLE:
            for (; num2 < 14; num2 += 8)
            {
              if ((uint) availableBytesIn > 0U)
              {
                r = 0;
                --availableBytesIn;
                num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
              }
              else
              {
                this.bitb = num1;
                this.bitk = num2;
                this._codec.AvailableBytesIn = availableBytesIn;
                this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                this._codec.NextIn = nextIn;
                this.writeAt = destinationIndex;
                return this.Flush(r);
              }
            }
            int num10;
            this.table = num10 = num1 & 16383;
            if ((num10 & 31) <= 29 && (num10 >> 5 & 31) <= 29)
            {
              int length = 258 + (num10 & 31) + (num10 >> 5 & 31);
              if (this.blens == null || this.blens.Length < length)
                this.blens = new int[length];
              else
                Array.Clear((Array) this.blens, 0, length);
              num1 >>= 14;
              num2 -= 14;
              this.index = 0;
              this.mode = InflateBlocks.InflateBlockMode.BTREE;
              goto case InflateBlocks.InflateBlockMode.BTREE;
            }
            else
              goto label_39;
          case InflateBlocks.InflateBlockMode.BTREE:
            while (this.index < 4 + (this.table >> 10))
            {
              for (; num2 < 3; num2 += 8)
              {
                if ((uint) availableBytesIn > 0U)
                {
                  r = 0;
                  --availableBytesIn;
                  num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
                }
                else
                {
                  this.bitb = num1;
                  this.bitk = num2;
                  this._codec.AvailableBytesIn = availableBytesIn;
                  this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                  this._codec.NextIn = nextIn;
                  this.writeAt = destinationIndex;
                  return this.Flush(r);
                }
              }
              this.blens[InflateBlocks.border[this.index++]] = num1 & 7;
              num1 >>= 3;
              num2 -= 3;
            }
            while (this.index < 19)
              this.blens[InflateBlocks.border[this.index++]] = 0;
            this.bb[0] = 7;
            num4 = this.inftree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, this._codec);
            if ((uint) num4 <= 0U)
            {
              this.index = 0;
              this.mode = InflateBlocks.InflateBlockMode.DTREE;
              goto case InflateBlocks.InflateBlockMode.DTREE;
            }
            else
              goto label_53;
          case InflateBlocks.InflateBlockMode.DTREE:
            while (true)
            {
              int table1 = this.table;
              if (this.index < 258 + (table1 & 31) + (table1 >> 5 & 31))
              {
                int index1;
                for (index1 = this.bb[0]; num2 < index1; num2 += 8)
                {
                  if ((uint) availableBytesIn > 0U)
                  {
                    r = 0;
                    --availableBytesIn;
                    num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
                  }
                  else
                  {
                    this.bitb = num1;
                    this.bitk = num2;
                    this._codec.AvailableBytesIn = availableBytesIn;
                    this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                    this._codec.NextIn = nextIn;
                    this.writeAt = destinationIndex;
                    return this.Flush(r);
                  }
                }
                int huft1 = this.hufts[(this.tb[0] + (num1 & InternalInflateConstants.InflateMask[index1])) * 3 + 1];
                int huft2 = this.hufts[(this.tb[0] + (num1 & InternalInflateConstants.InflateMask[huft1])) * 3 + 2];
                if (huft2 < 16)
                {
                  num1 >>= huft1;
                  num2 -= huft1;
                  this.blens[this.index++] = huft2;
                }
                else
                {
                  int index2 = huft2 == 18 ? 7 : huft2 - 14;
                  int num11 = huft2 == 18 ? 11 : 3;
                  for (; num2 < huft1 + index2; num2 += 8)
                  {
                    if ((uint) availableBytesIn > 0U)
                    {
                      r = 0;
                      --availableBytesIn;
                      num1 |= ((int) this._codec.InputBuffer[nextIn++] & (int) byte.MaxValue) << num2;
                    }
                    else
                    {
                      this.bitb = num1;
                      this.bitk = num2;
                      this._codec.AvailableBytesIn = availableBytesIn;
                      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
                      this._codec.NextIn = nextIn;
                      this.writeAt = destinationIndex;
                      return this.Flush(r);
                    }
                  }
                  int num12 = num1 >> huft1;
                  int num13 = num2 - huft1;
                  int num14 = num11 + (num12 & InternalInflateConstants.InflateMask[index2]);
                  num1 = num12 >> index2;
                  num2 = num13 - index2;
                  int index3 = this.index;
                  int table2 = this.table;
                  if (index3 + num14 <= 258 + (table2 & 31) + (table2 >> 5 & 31) && (huft2 != 16 || index3 >= 1))
                  {
                    int num15 = huft2 == 16 ? this.blens[index3 - 1] : 0;
                    do
                    {
                      this.blens[index3++] = num15;
                    }
                    while ((uint) --num14 > 0U);
                    this.index = index3;
                  }
                  else
                    goto label_70;
                }
              }
              else
                break;
            }
            this.tb[0] = -1;
            int[] bl2 = new int[1]{ 9 };
            int[] bd2 = new int[1]{ 6 };
            int[] tl2 = new int[1];
            int[] td2 = new int[1];
            int table = this.table;
            num5 = this.inftree.inflate_trees_dynamic(257 + (table & 31), 1 + (table >> 5 & 31), this.blens, bl2, bd2, tl2, td2, this.hufts, this._codec);
            if ((uint) num5 <= 0U)
            {
              this.codes.Init(bl2[0], bd2[0], this.hufts, tl2[0], this.hufts, td2[0]);
              this.mode = InflateBlocks.InflateBlockMode.CODES;
              goto case InflateBlocks.InflateBlockMode.CODES;
            }
            else
              goto label_77;
          case InflateBlocks.InflateBlockMode.CODES:
            this.bitb = num1;
            this.bitk = num2;
            this._codec.AvailableBytesIn = availableBytesIn;
            this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
            this._codec.NextIn = nextIn;
            this.writeAt = destinationIndex;
            r = this.codes.Process(this, r);
            if (r == 1)
            {
              r = 0;
              nextIn = this._codec.NextIn;
              availableBytesIn = this._codec.AvailableBytesIn;
              num1 = this.bitb;
              num2 = this.bitk;
              destinationIndex = this.writeAt;
              num3 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
              if (this.last == 0)
              {
                this.mode = InflateBlocks.InflateBlockMode.TYPE;
                break;
              }
              goto label_85;
            }
            else
              goto label_82;
          case InflateBlocks.InflateBlockMode.DRY:
            goto label_86;
          case InflateBlocks.InflateBlockMode.DONE:
            goto label_89;
          case InflateBlocks.InflateBlockMode.BAD:
            goto label_90;
          default:
            goto label_91;
        }
      }
label_9:
      int num16 = num1 >> 3;
      int num17 = num2 - 3;
      this.mode = InflateBlocks.InflateBlockMode.BAD;
      this._codec.Message = "invalid block type";
      r = -3;
      this.bitb = num16;
      this.bitk = num17;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_15:
      this.mode = InflateBlocks.InflateBlockMode.BAD;
      this._codec.Message = "invalid stored block lengths";
      r = -3;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_18:
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_26:
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_39:
      this.mode = InflateBlocks.InflateBlockMode.BAD;
      this._codec.Message = "too many length or distance symbols";
      r = -3;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_53:
      r = num4;
      if (r == -3)
      {
        this.blens = (int[]) null;
        this.mode = InflateBlocks.InflateBlockMode.BAD;
      }
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_70:
      this.blens = (int[]) null;
      this.mode = InflateBlocks.InflateBlockMode.BAD;
      this._codec.Message = "invalid bit length repeat";
      r = -3;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_77:
      if (num5 == -3)
      {
        this.blens = (int[]) null;
        this.mode = InflateBlocks.InflateBlockMode.BAD;
      }
      r = num5;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_82:
      return this.Flush(r);
label_85:
      this.mode = InflateBlocks.InflateBlockMode.DRY;
label_86:
      this.writeAt = destinationIndex;
      r = this.Flush(r);
      destinationIndex = this.writeAt;
      int num18 = destinationIndex < this.readAt ? this.readAt - destinationIndex - 1 : this.end - destinationIndex;
      if (this.readAt != this.writeAt)
      {
        this.bitb = num1;
        this.bitk = num2;
        this._codec.AvailableBytesIn = availableBytesIn;
        this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
        this._codec.NextIn = nextIn;
        this.writeAt = destinationIndex;
        return this.Flush(r);
      }
      this.mode = InflateBlocks.InflateBlockMode.DONE;
label_89:
      r = 1;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_90:
      r = -3;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
label_91:
      r = -2;
      this.bitb = num1;
      this.bitk = num2;
      this._codec.AvailableBytesIn = availableBytesIn;
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this.writeAt = destinationIndex;
      return this.Flush(r);
    }

    internal void Free()
    {
      int num = (int) this.Reset();
      this.window = (byte[]) null;
      this.hufts = (int[]) null;
    }

    internal void SetDictionary(byte[] d, int start, int n)
    {
      Array.Copy((Array) d, start, (Array) this.window, 0, n);
      this.readAt = this.writeAt = n;
    }

    internal int SyncPoint() => this.mode == InflateBlocks.InflateBlockMode.LENS ? 1 : 0;

    internal int Flush(int r)
    {
      for (int index = 0; index < 2; ++index)
      {
        int num = index != 0 ? this.writeAt - this.readAt : (this.readAt <= this.writeAt ? this.writeAt : this.end) - this.readAt;
        if (num == 0)
        {
          if (r == -5)
            r = 0;
          return r;
        }
        if (num > this._codec.AvailableBytesOut)
          num = this._codec.AvailableBytesOut;
        if (num != 0 && r == -5)
          r = 0;
        this._codec.AvailableBytesOut -= num;
        this._codec.TotalBytesOut += (long) num;
        if (this.checkfn != null)
          this._codec._Adler32 = this.check = Adler.Adler32(this.check, this.window, this.readAt, num);
        Array.Copy((Array) this.window, this.readAt, (Array) this._codec.OutputBuffer, this._codec.NextOut, num);
        this._codec.NextOut += num;
        this.readAt += num;
        if (this.readAt == this.end && index == 0)
        {
          this.readAt = 0;
          if (this.writeAt == this.end)
            this.writeAt = 0;
        }
        else
          ++index;
      }
      return r;
    }

    private enum InflateBlockMode
    {
      TYPE,
      LENS,
      STORED,
      TABLE,
      BTREE,
      DTREE,
      CODES,
      DRY,
      DONE,
      BAD,
    }
  }
}
