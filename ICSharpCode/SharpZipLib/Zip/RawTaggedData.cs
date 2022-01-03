// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.RawTaggedData
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class RawTaggedData : ITaggedData
  {
    private short _tag;
    private byte[] _data;

    public RawTaggedData(short tag) => this._tag = tag;

    public short TagID
    {
      get => this._tag;
      set => this._tag = value;
    }

    public void SetData(byte[] data, int offset, int count)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      this._data = new byte[count];
      Array.Copy((Array) data, offset, (Array) this._data, 0, count);
    }

    public byte[] GetData() => this._data;

    public byte[] Data
    {
      get => this._data;
      set => this._data = value;
    }
  }
}
