﻿// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.InflaterDynHeader
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
  internal class InflaterDynHeader
  {
    private const int LNUM = 0;
    private const int DNUM = 1;
    private const int BLNUM = 2;
    private const int BLLENS = 3;
    private const int LENS = 4;
    private const int REPS = 5;
    private static readonly int[] repMin = new int[3]
    {
      3,
      3,
      11
    };
    private static readonly int[] repBits = new int[3]
    {
      2,
      3,
      7
    };
    private static readonly int[] BL_ORDER = new int[19]
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
    private byte[] blLens;
    private byte[] litdistLens;
    private InflaterHuffmanTree blTree;
    private int mode;
    private int lnum;
    private int dnum;
    private int blnum;
    private int num;
    private int repSymbol;
    private byte lastLen;
    private int ptr;

    public bool Decode(StreamManipulator input)
    {
      while (true)
      {
        switch (this.mode)
        {
          case 0:
            this.lnum = input.PeekBits(5);
            if (this.lnum >= 0)
            {
              this.lnum += 257;
              input.DropBits(5);
              this.mode = 1;
              goto case 1;
            }
            else
              goto label_3;
          case 1:
            this.dnum = input.PeekBits(5);
            if (this.dnum >= 0)
            {
              ++this.dnum;
              input.DropBits(5);
              this.num = this.lnum + this.dnum;
              this.litdistLens = new byte[this.num];
              this.mode = 2;
              goto case 2;
            }
            else
              goto label_6;
          case 2:
            this.blnum = input.PeekBits(4);
            if (this.blnum >= 0)
            {
              this.blnum += 4;
              input.DropBits(4);
              this.blLens = new byte[19];
              this.ptr = 0;
              this.mode = 3;
              goto case 3;
            }
            else
              goto label_9;
          case 3:
            for (; this.ptr < this.blnum; ++this.ptr)
            {
              int num = input.PeekBits(3);
              if (num < 0)
                return false;
              input.DropBits(3);
              this.blLens[InflaterDynHeader.BL_ORDER[this.ptr]] = (byte) num;
            }
            this.blTree = new InflaterHuffmanTree(this.blLens);
            this.blLens = (byte[]) null;
            this.ptr = 0;
            this.mode = 4;
            goto case 4;
          case 4:
            int symbol;
            while (((symbol = this.blTree.GetSymbol(input)) & -16) == 0)
            {
              this.litdistLens[this.ptr++] = this.lastLen = (byte) symbol;
              if (this.ptr == this.num)
                return true;
            }
            if (symbol >= 0)
            {
              if (symbol >= 17)
                this.lastLen = (byte) 0;
              else if (this.ptr == 0)
                goto label_24;
              this.repSymbol = symbol - 16;
              this.mode = 5;
              goto case 5;
            }
            else
              goto label_20;
          case 5:
            int repBit = InflaterDynHeader.repBits[this.repSymbol];
            int num1 = input.PeekBits(repBit);
            if (num1 >= 0)
            {
              input.DropBits(repBit);
              int num2 = num1 + InflaterDynHeader.repMin[this.repSymbol];
              if (this.ptr + num2 <= this.num)
              {
                while (num2-- > 0)
                  this.litdistLens[this.ptr++] = this.lastLen;
                if (this.ptr != this.num)
                {
                  this.mode = 4;
                  continue;
                }
                goto label_33;
              }
              else
                goto label_29;
            }
            else
              goto label_27;
          default:
            continue;
        }
      }
label_3:
      return false;
label_6:
      return false;
label_9:
      return false;
label_20:
      return false;
label_24:
      throw new SharpZipBaseException();
label_27:
      return false;
label_29:
      throw new SharpZipBaseException();
label_33:
      return true;
    }

    public InflaterHuffmanTree BuildLitLenTree()
    {
      byte[] codeLengths = new byte[this.lnum];
      Array.Copy((Array) this.litdistLens, 0, (Array) codeLengths, 0, this.lnum);
      return new InflaterHuffmanTree(codeLengths);
    }

    public InflaterHuffmanTree BuildDistTree()
    {
      byte[] codeLengths = new byte[this.dnum];
      Array.Copy((Array) this.litdistLens, this.lnum, (Array) codeLengths, 0, this.dnum);
      return new InflaterHuffmanTree(codeLengths);
    }
  }
}
