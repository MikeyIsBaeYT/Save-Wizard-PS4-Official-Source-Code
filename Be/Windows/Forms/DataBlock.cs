// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.DataBlock
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Be.Windows.Forms
{
  internal abstract class DataBlock
  {
    internal DataMap _map;
    internal DataBlock _nextBlock;
    internal DataBlock _previousBlock;

    public abstract long Length { get; }

    public DataMap Map => this._map;

    public DataBlock NextBlock => this._nextBlock;

    public DataBlock PreviousBlock => this._previousBlock;

    public abstract void RemoveBytes(long position, long count);
  }
}
