// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InflateManager
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zlib
{
  internal sealed class InflateManager
  {
    private const int PRESET_DICT = 32;
    private const int Z_DEFLATED = 8;
    private InflateManager.InflateManagerMode mode;
    internal ZlibCodec _codec;
    internal int method;
    internal uint computedCheck;
    internal uint expectedCheck;
    internal int marker;
    private bool _handleRfc1950HeaderBytes = true;
    internal int wbits;
    internal InflateBlocks blocks;
    private static readonly byte[] mark = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue
    };

    internal bool HandleRfc1950HeaderBytes
    {
      get => this._handleRfc1950HeaderBytes;
      set => this._handleRfc1950HeaderBytes = value;
    }

    public InflateManager()
    {
    }

    public InflateManager(bool expectRfc1950HeaderBytes) => this._handleRfc1950HeaderBytes = expectRfc1950HeaderBytes;

    internal int Reset()
    {
      this._codec.TotalBytesIn = this._codec.TotalBytesOut = 0L;
      this._codec.Message = (string) null;
      this.mode = this.HandleRfc1950HeaderBytes ? InflateManager.InflateManagerMode.METHOD : InflateManager.InflateManagerMode.BLOCKS;
      int num = (int) this.blocks.Reset();
      return 0;
    }

    internal int End()
    {
      if (this.blocks != null)
        this.blocks.Free();
      this.blocks = (InflateBlocks) null;
      return 0;
    }

    internal int Initialize(ZlibCodec codec, int w)
    {
      this._codec = codec;
      this._codec.Message = (string) null;
      this.blocks = (InflateBlocks) null;
      if (w < 8 || w > 15)
      {
        this.End();
        throw new ZlibException("Bad window size.");
      }
      this.wbits = w;
      this.blocks = new InflateBlocks(codec, this.HandleRfc1950HeaderBytes ? (object) this : (object) (InflateManager) null, 1 << w);
      this.Reset();
      return 0;
    }

    internal int Inflate(FlushType flush)
    {
      if (this._codec.InputBuffer == null)
        throw new ZlibException("InputBuffer is null. ");
      int num1 = 0;
      int r = -5;
      while (true)
      {
        switch (this.mode)
        {
          case InflateManager.InflateManagerMode.METHOD:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              if (((this.method = (int) this._codec.InputBuffer[this._codec.NextIn++]) & 15) != 8)
              {
                this.mode = InflateManager.InflateManagerMode.BAD;
                this._codec.Message = string.Format("unknown compression method (0x{0:X2})", (object) this.method);
                this.marker = 5;
                break;
              }
              if ((this.method >> 4) + 8 > this.wbits)
              {
                this.mode = InflateManager.InflateManagerMode.BAD;
                this._codec.Message = string.Format("invalid window size ({0})", (object) ((this.method >> 4) + 8));
                this.marker = 5;
                break;
              }
              this.mode = InflateManager.InflateManagerMode.FLAG;
              break;
            }
            goto label_4;
          case InflateManager.InflateManagerMode.FLAG:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              int num2 = (int) this._codec.InputBuffer[this._codec.NextIn++] & (int) byte.MaxValue;
              if ((uint) (((this.method << 8) + num2) % 31) > 0U)
              {
                this.mode = InflateManager.InflateManagerMode.BAD;
                this._codec.Message = "incorrect header check";
                this.marker = 5;
                break;
              }
              this.mode = (num2 & 32) == 0 ? InflateManager.InflateManagerMode.BLOCKS : InflateManager.InflateManagerMode.DICT4;
              break;
            }
            goto label_11;
          case InflateManager.InflateManagerMode.DICT4:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck = (uint) ((ulong) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 24) & 4278190080UL);
              this.mode = InflateManager.InflateManagerMode.DICT3;
              break;
            }
            goto label_16;
          case InflateManager.InflateManagerMode.DICT3:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck += (uint) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
              this.mode = InflateManager.InflateManagerMode.DICT2;
              break;
            }
            goto label_19;
          case InflateManager.InflateManagerMode.DICT2:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck += (uint) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
              this.mode = InflateManager.InflateManagerMode.DICT1;
              break;
            }
            goto label_22;
          case InflateManager.InflateManagerMode.DICT1:
            goto label_24;
          case InflateManager.InflateManagerMode.DICT0:
            goto label_27;
          case InflateManager.InflateManagerMode.BLOCKS:
            r = this.blocks.Process(r);
            if (r == -3)
            {
              this.mode = InflateManager.InflateManagerMode.BAD;
              this.marker = 0;
              break;
            }
            if (r == 0)
              r = num1;
            if (r == 1)
            {
              r = num1;
              this.computedCheck = this.blocks.Reset();
              if (this.HandleRfc1950HeaderBytes)
              {
                this.mode = InflateManager.InflateManagerMode.CHECK4;
                break;
              }
              goto label_35;
            }
            else
              goto label_33;
          case InflateManager.InflateManagerMode.CHECK4:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck = (uint) ((ulong) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 24) & 4278190080UL);
              this.mode = InflateManager.InflateManagerMode.CHECK3;
              break;
            }
            goto label_38;
          case InflateManager.InflateManagerMode.CHECK3:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck += (uint) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 16 & 16711680);
              this.mode = InflateManager.InflateManagerMode.CHECK2;
              break;
            }
            goto label_41;
          case InflateManager.InflateManagerMode.CHECK2:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck += (uint) ((int) this._codec.InputBuffer[this._codec.NextIn++] << 8 & 65280);
              this.mode = InflateManager.InflateManagerMode.CHECK1;
              break;
            }
            goto label_44;
          case InflateManager.InflateManagerMode.CHECK1:
            if (this._codec.AvailableBytesIn != 0)
            {
              r = num1;
              --this._codec.AvailableBytesIn;
              ++this._codec.TotalBytesIn;
              this.expectedCheck += (uint) this._codec.InputBuffer[this._codec.NextIn++] & (uint) byte.MaxValue;
              if ((int) this.computedCheck != (int) this.expectedCheck)
              {
                this.mode = InflateManager.InflateManagerMode.BAD;
                this._codec.Message = "incorrect data check";
                this.marker = 5;
                break;
              }
              goto label_50;
            }
            else
              goto label_47;
          case InflateManager.InflateManagerMode.DONE:
            goto label_51;
          case InflateManager.InflateManagerMode.BAD:
            goto label_52;
          default:
            goto label_53;
        }
      }
label_4:
      return r;
label_11:
      return r;
label_16:
      return r;
label_19:
      return r;
label_22:
      return r;
label_24:
      if (this._codec.AvailableBytesIn == 0)
        return r;
      --this._codec.AvailableBytesIn;
      ++this._codec.TotalBytesIn;
      this.expectedCheck += (uint) this._codec.InputBuffer[this._codec.NextIn++] & (uint) byte.MaxValue;
      this._codec._Adler32 = this.expectedCheck;
      this.mode = InflateManager.InflateManagerMode.DICT0;
      return 2;
label_27:
      this.mode = InflateManager.InflateManagerMode.BAD;
      this._codec.Message = "need dictionary";
      this.marker = 0;
      return -2;
label_33:
      return r;
label_35:
      this.mode = InflateManager.InflateManagerMode.DONE;
      return 1;
label_38:
      return r;
label_41:
      return r;
label_44:
      return r;
label_47:
      return r;
label_50:
      this.mode = InflateManager.InflateManagerMode.DONE;
      return 1;
label_51:
      return 1;
label_52:
      throw new ZlibException(string.Format("Bad state ({0})", (object) this._codec.Message));
label_53:
      throw new ZlibException("Stream error.");
    }

    internal int SetDictionary(byte[] dictionary)
    {
      int start = 0;
      int n = dictionary.Length;
      if (this.mode != InflateManager.InflateManagerMode.DICT0)
        throw new ZlibException("Stream error.");
      if ((int) Adler.Adler32(1U, dictionary, 0, dictionary.Length) != (int) this._codec._Adler32)
        return -3;
      this._codec._Adler32 = Adler.Adler32(0U, (byte[]) null, 0, 0);
      if (n >= 1 << this.wbits)
      {
        n = (1 << this.wbits) - 1;
        start = dictionary.Length - n;
      }
      this.blocks.SetDictionary(dictionary, start, n);
      this.mode = InflateManager.InflateManagerMode.BLOCKS;
      return 0;
    }

    internal int Sync()
    {
      if (this.mode != InflateManager.InflateManagerMode.BAD)
      {
        this.mode = InflateManager.InflateManagerMode.BAD;
        this.marker = 0;
      }
      int availableBytesIn;
      if ((availableBytesIn = this._codec.AvailableBytesIn) == 0)
        return -5;
      int nextIn = this._codec.NextIn;
      int index;
      for (index = this.marker; availableBytesIn != 0 && index < 4; --availableBytesIn)
      {
        if ((int) this._codec.InputBuffer[nextIn] == (int) InflateManager.mark[index])
          ++index;
        else
          index = this._codec.InputBuffer[nextIn] <= (byte) 0 ? 4 - index : 0;
        ++nextIn;
      }
      this._codec.TotalBytesIn += (long) (nextIn - this._codec.NextIn);
      this._codec.NextIn = nextIn;
      this._codec.AvailableBytesIn = availableBytesIn;
      this.marker = index;
      if (index != 4)
        return -3;
      long totalBytesIn = this._codec.TotalBytesIn;
      long totalBytesOut = this._codec.TotalBytesOut;
      this.Reset();
      this._codec.TotalBytesIn = totalBytesIn;
      this._codec.TotalBytesOut = totalBytesOut;
      this.mode = InflateManager.InflateManagerMode.BLOCKS;
      return 0;
    }

    internal int SyncPoint(ZlibCodec z) => this.blocks.SyncPoint();

    private enum InflateManagerMode
    {
      METHOD,
      FLAG,
      DICT4,
      DICT3,
      DICT2,
      DICT1,
      DICT0,
      BLOCKS,
      CHECK4,
      CHECK3,
      CHECK2,
      CHECK1,
      DONE,
      BAD,
    }
  }
}
