// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.MemoryDataBlock
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Be.Windows.Forms
{
  internal sealed class MemoryDataBlock : DataBlock
  {
    private byte[] _data;

    public MemoryDataBlock(byte data) => this._data = new byte[1]
    {
      data
    };

    public MemoryDataBlock(byte[] data) => this._data = data != null ? (byte[]) data.Clone() : throw new ArgumentNullException(nameof (data));

    public override long Length => (long) this._data.Length;

    public byte[] Data => this._data;

    public void AddByteToEnd(byte value)
    {
      byte[] numArray = new byte[(long) this._data.Length + 1L];
      this._data.CopyTo((Array) numArray, 0);
      numArray[(long) numArray.Length - 1L] = value;
      this._data = numArray;
    }

    public void AddByteToStart(byte value)
    {
      byte[] numArray = new byte[(long) this._data.Length + 1L];
      numArray[0] = value;
      this._data.CopyTo((Array) numArray, 1);
      this._data = numArray;
    }

    public void InsertBytes(long position, byte[] data)
    {
      byte[] numArray = new byte[(long) this._data.Length + (long) data.Length];
      if (position > 0L)
        Array.Copy((Array) this._data, 0L, (Array) numArray, 0L, position);
      Array.Copy((Array) data, 0L, (Array) numArray, position, (long) data.Length);
      if (position < (long) this._data.Length)
        Array.Copy((Array) this._data, position, (Array) numArray, position + (long) data.Length, (long) this._data.Length - position);
      this._data = numArray;
    }

    public override void RemoveBytes(long position, long count)
    {
      byte[] numArray = new byte[(long) this._data.Length - count];
      if (position > 0L)
        Array.Copy((Array) this._data, 0L, (Array) numArray, 0L, position);
      if (position + count < (long) this._data.Length)
        Array.Copy((Array) this._data, position + count, (Array) numArray, position, (long) numArray.Length - position);
      this._data = numArray;
    }
  }
}
