// Decompiled with JetBrains decompiler
// Type: Ionic.Crc.CRC32
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  public class CRC32
  {
    private uint dwPolynomial;
    private long _TotalBytesRead;
    private bool reverseBits;
    private uint[] crc32Table;
    private const int BUFFER_SIZE = 8192;
    private uint _register = uint.MaxValue;

    public long TotalBytesRead => this._TotalBytesRead;

    public int Crc32Result => ~(int) this._register;

    public int GetCrc32(Stream input) => this.GetCrc32AndCopy(input, (Stream) null);

    public int GetCrc32AndCopy(Stream input, Stream output)
    {
      if (input == null)
        throw new Exception("The input stream must not be null.");
      byte[] numArray = new byte[8192];
      int count1 = 8192;
      this._TotalBytesRead = 0L;
      int count2 = input.Read(numArray, 0, count1);
      output?.Write(numArray, 0, count2);
      this._TotalBytesRead += (long) count2;
      while (count2 > 0)
      {
        this.SlurpBlock(numArray, 0, count2);
        count2 = input.Read(numArray, 0, count1);
        output?.Write(numArray, 0, count2);
        this._TotalBytesRead += (long) count2;
      }
      return ~(int) this._register;
    }

    public int ComputeCrc32(int W, byte B) => this._InternalComputeCrc32((uint) W, B);

    internal int _InternalComputeCrc32(uint W, byte B) => (int) this.crc32Table[((int) W ^ (int) B) & (int) byte.MaxValue] ^ (int) (W >> 8);

    public void SlurpBlock(byte[] block, int offset, int count)
    {
      if (block == null)
        throw new Exception("The data buffer must not be null.");
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = offset + index1;
        byte num = block[index2];
        this._register = !this.reverseBits ? this._register >> 8 ^ this.crc32Table[(int) (this._register & (uint) byte.MaxValue ^ (uint) num)] : this._register << 8 ^ this.crc32Table[(int) (this._register >> 24 ^ (uint) num)];
      }
      this._TotalBytesRead += (long) count;
    }

    public void UpdateCRC(byte b)
    {
      if (this.reverseBits)
        this._register = this._register << 8 ^ this.crc32Table[(int) (this._register >> 24 ^ (uint) b)];
      else
        this._register = this._register >> 8 ^ this.crc32Table[(int) (this._register & (uint) byte.MaxValue ^ (uint) b)];
    }

    public void UpdateCRC(byte b, int n)
    {
      while (n-- > 0)
      {
        if (this.reverseBits)
        {
          uint num = this._register >> 24 ^ (uint) b;
          this._register = this._register << 8 ^ this.crc32Table[num >= 0U ? (int) num : (int) num + 256];
        }
        else
        {
          uint num = this._register & (uint) byte.MaxValue ^ (uint) b;
          this._register = this._register >> 8 ^ this.crc32Table[num >= 0U ? (int) num : (int) num + 256];
        }
      }
    }

    private static uint ReverseBits(uint data)
    {
      uint num1 = data;
      uint num2 = (uint) (((int) num1 & 1431655765) << 1 | (int) (num1 >> 1) & 1431655765);
      uint num3 = (uint) (((int) num2 & 858993459) << 2 | (int) (num2 >> 2) & 858993459);
      uint num4 = (uint) (((int) num3 & 252645135) << 4 | (int) (num3 >> 4) & 252645135);
      return (uint) ((int) num4 << 24 | ((int) num4 & 65280) << 8 | (int) (num4 >> 8) & 65280) | num4 >> 24;
    }

    private static byte ReverseBits(byte data)
    {
      uint num1 = (uint) data * 131586U;
      uint num2 = 17055760;
      return (byte) ((uint) (16781313 * ((int) (num1 & num2) + ((int) num1 << 2 & (int) num2 << 1))) >> 24);
    }

    private void GenerateLookupTable()
    {
      this.crc32Table = new uint[256];
      byte data1 = 0;
      do
      {
        uint data2 = (uint) data1;
        for (byte index = 8; index > (byte) 0; --index)
        {
          if (((int) data2 & 1) == 1)
            data2 = data2 >> 1 ^ this.dwPolynomial;
          else
            data2 >>= 1;
        }
        if (this.reverseBits)
          this.crc32Table[(int) CRC32.ReverseBits(data1)] = CRC32.ReverseBits(data2);
        else
          this.crc32Table[(int) data1] = data2;
        ++data1;
      }
      while (data1 > (byte) 0);
    }

    private uint gf2_matrix_times(uint[] matrix, uint vec)
    {
      uint num = 0;
      int index = 0;
      while (vec > 0U)
      {
        if (((int) vec & 1) == 1)
          num ^= matrix[index];
        vec >>= 1;
        ++index;
      }
      return num;
    }

    private void gf2_matrix_square(uint[] square, uint[] mat)
    {
      for (int index = 0; index < 32; ++index)
        square[index] = this.gf2_matrix_times(mat, mat[index]);
    }

    public void Combine(int crc, int length)
    {
      uint[] numArray1 = new uint[32];
      uint[] numArray2 = new uint[32];
      if (length == 0)
        return;
      uint vec = ~this._register;
      uint num1 = (uint) crc;
      numArray2[0] = this.dwPolynomial;
      uint num2 = 1;
      for (int index = 1; index < 32; ++index)
      {
        numArray2[index] = num2;
        num2 <<= 1;
      }
      this.gf2_matrix_square(numArray1, numArray2);
      this.gf2_matrix_square(numArray2, numArray1);
      uint num3 = (uint) length;
      do
      {
        this.gf2_matrix_square(numArray1, numArray2);
        if (((int) num3 & 1) == 1)
          vec = this.gf2_matrix_times(numArray1, vec);
        uint num4 = num3 >> 1;
        if (num4 != 0U)
        {
          this.gf2_matrix_square(numArray2, numArray1);
          if (((int) num4 & 1) == 1)
            vec = this.gf2_matrix_times(numArray2, vec);
          num3 = num4 >> 1;
        }
        else
          break;
      }
      while (num3 > 0U);
      this._register = ~(vec ^ num1);
    }

    public CRC32()
      : this(false)
    {
    }

    public CRC32(bool reverseBits)
      : this(-306674912, reverseBits)
    {
    }

    public CRC32(int polynomial, bool reverseBits)
    {
      this.reverseBits = reverseBits;
      this.dwPolynomial = (uint) polynomial;
      this.GenerateLookupTable();
    }

    public void Reset() => this._register = uint.MaxValue;
  }
}
