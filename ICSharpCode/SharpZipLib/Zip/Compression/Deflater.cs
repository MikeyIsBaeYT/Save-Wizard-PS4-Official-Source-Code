// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Deflater
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
  public class Deflater
  {
    public const int BEST_COMPRESSION = 9;
    public const int BEST_SPEED = 1;
    public const int DEFAULT_COMPRESSION = -1;
    public const int NO_COMPRESSION = 0;
    public const int DEFLATED = 8;
    private const int IS_SETDICT = 1;
    private const int IS_FLUSHING = 4;
    private const int IS_FINISHING = 8;
    private const int INIT_STATE = 0;
    private const int SETDICT_STATE = 1;
    private const int BUSY_STATE = 16;
    private const int FLUSHING_STATE = 20;
    private const int FINISHING_STATE = 28;
    private const int FINISHED_STATE = 30;
    private const int CLOSED_STATE = 127;
    private int level;
    private bool noZlibHeaderOrFooter;
    private int state;
    private long totalOut;
    private DeflaterPending pending;
    private DeflaterEngine engine;

    public Deflater()
      : this(-1, false)
    {
    }

    public Deflater(int level)
      : this(level, false)
    {
    }

    public Deflater(int level, bool noZlibHeaderOrFooter)
    {
      if (level == -1)
        level = 6;
      else if (level < 0 || level > 9)
        throw new ArgumentOutOfRangeException(nameof (level));
      this.pending = new DeflaterPending();
      this.engine = new DeflaterEngine(this.pending);
      this.noZlibHeaderOrFooter = noZlibHeaderOrFooter;
      this.SetStrategy(DeflateStrategy.Default);
      this.SetLevel(level);
      this.Reset();
    }

    public void Reset()
    {
      this.state = this.noZlibHeaderOrFooter ? 16 : 0;
      this.totalOut = 0L;
      this.pending.Reset();
      this.engine.Reset();
    }

    public int Adler => this.engine.Adler;

    public long TotalIn => this.engine.TotalIn;

    public long TotalOut => this.totalOut;

    public void Flush() => this.state |= 4;

    public void Finish() => this.state |= 12;

    public bool IsFinished => this.state == 30 && this.pending.IsFlushed;

    public bool IsNeedingInput => this.engine.NeedsInput();

    public void SetInput(byte[] input) => this.SetInput(input, 0, input.Length);

    public void SetInput(byte[] input, int offset, int count)
    {
      if ((uint) (this.state & 8) > 0U)
        throw new InvalidOperationException("Finish() already called");
      this.engine.SetInput(input, offset, count);
    }

    public void SetLevel(int level)
    {
      if (level == -1)
        level = 6;
      else if (level < 0 || level > 9)
        throw new ArgumentOutOfRangeException(nameof (level));
      if (this.level == level)
        return;
      this.level = level;
      this.engine.SetLevel(level);
    }

    public int GetLevel() => this.level;

    public void SetStrategy(DeflateStrategy strategy) => this.engine.Strategy = strategy;

    public int Deflate(byte[] output) => this.Deflate(output, 0, output.Length);

    public int Deflate(byte[] output, int offset, int length)
    {
      int num1 = length;
      if (this.state == (int) sbyte.MaxValue)
        throw new InvalidOperationException("Deflater closed");
      if (this.state < 16)
      {
        int num2 = 30720;
        int num3 = this.level - 1 >> 1;
        if (num3 < 0 || num3 > 3)
          num3 = 3;
        int num4 = num2 | num3 << 6;
        if ((uint) (this.state & 1) > 0U)
          num4 |= 32;
        this.pending.WriteShortMSB(num4 + (31 - num4 % 31));
        if ((uint) (this.state & 1) > 0U)
        {
          int adler = this.engine.Adler;
          this.engine.ResetAdler();
          this.pending.WriteShortMSB(adler >> 16);
          this.pending.WriteShortMSB(adler & (int) ushort.MaxValue);
        }
        this.state = 16 | this.state & 12;
      }
      while (true)
      {
        int num5 = this.pending.Flush(output, offset, length);
        offset += num5;
        this.totalOut += (long) num5;
        length -= num5;
        if (length != 0 && this.state != 30)
        {
          if (!this.engine.Deflate((uint) (this.state & 4) > 0U, (uint) (this.state & 8) > 0U))
          {
            if (this.state != 16)
            {
              if (this.state == 20)
              {
                if ((uint) this.level > 0U)
                {
                  for (int index = 8 + (-this.pending.BitCount & 7); index > 0; index -= 10)
                    this.pending.WriteBits(2, 10);
                }
                this.state = 16;
              }
              else if (this.state == 28)
              {
                this.pending.AlignToByte();
                if (!this.noZlibHeaderOrFooter)
                {
                  int adler = this.engine.Adler;
                  this.pending.WriteShortMSB(adler >> 16);
                  this.pending.WriteShortMSB(adler & (int) ushort.MaxValue);
                }
                this.state = 30;
              }
            }
            else
              break;
          }
        }
        else
          goto label_26;
      }
      return num1 - length;
label_26:
      return num1 - length;
    }

    public void SetDictionary(byte[] dictionary) => this.SetDictionary(dictionary, 0, dictionary.Length);

    public void SetDictionary(byte[] dictionary, int index, int count)
    {
      this.state = (uint) this.state <= 0U ? 1 : throw new InvalidOperationException();
      this.engine.SetDictionary(dictionary, index, count);
    }
  }
}
