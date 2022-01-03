// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.BytePositionInfo
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Be.Windows.Forms
{
  internal struct BytePositionInfo
  {
    private int _characterPosition;
    private long _index;

    public BytePositionInfo(long index, int characterPosition)
    {
      this._index = index;
      this._characterPosition = characterPosition;
    }

    public int CharacterPosition => this._characterPosition;

    public long Index => this._index;
  }
}
