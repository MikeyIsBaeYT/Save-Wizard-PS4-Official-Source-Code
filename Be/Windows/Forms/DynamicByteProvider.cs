// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.DynamicByteProvider
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Be.Windows.Forms
{
  public class DynamicByteProvider : IByteProvider
  {
    private bool _hasChanges;
    private ByteCollection _bytes;

    public DynamicByteProvider(byte[] data)
      : this(new ByteCollection(data))
    {
    }

    public DynamicByteProvider(ByteCollection bytes) => this._bytes = bytes;

    private void OnChanged(ByteProviderChanged e)
    {
      this._hasChanges = true;
      if (this.Changed == null)
        return;
      this.Changed((object) this, e);
    }

    private void OnLengthChanged(EventArgs e)
    {
      if (this.LengthChanged == null)
        return;
      this.LengthChanged((object) this, e);
    }

    public ByteCollection Bytes => this._bytes;

    public bool HasChanges() => this._hasChanges;

    public void ApplyChanges() => this._hasChanges = false;

    public event EventHandler<ByteProviderChanged> Changed;

    public event EventHandler LengthChanged;

    public byte ReadByte(long index) => this._bytes[(int) index];

    public void WriteByte(long index, byte value, bool noEvt = false)
    {
      byte num = this._bytes[(int) index];
      this._bytes[(int) index] = value;
      if (noEvt)
        return;
      this.OnChanged(new ByteProviderChanged()
      {
        Index = index,
        OldValue = num,
        NewValue = value,
        ChangeType = ChangeType.Insert
      });
    }

    public void DeleteBytes(long index, long length)
    {
      this._bytes.RemoveRange((int) Math.Max(0L, index), (int) Math.Min((long) (int) this.Length, length));
      this.OnLengthChanged(EventArgs.Empty);
      this.OnChanged(new ByteProviderChanged()
      {
        Index = index,
        ChangeType = ChangeType.Delete
      });
    }

    public void InsertBytes(long index, byte[] bs)
    {
      this._bytes.InsertRange((int) index, bs);
      this.OnLengthChanged(EventArgs.Empty);
      this.OnChanged(new ByteProviderChanged()
      {
        Index = index,
        ChangeType = ChangeType.Insert
      });
    }

    public long Length => (long) this._bytes.Count;

    public bool SupportsWriteByte() => true;

    public bool SupportsInsertBytes() => false;

    public bool SupportsDeleteBytes() => false;
  }
}
