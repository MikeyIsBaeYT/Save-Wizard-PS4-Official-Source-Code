// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.IByteProvider
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Be.Windows.Forms
{
  public interface IByteProvider
  {
    byte ReadByte(long index);

    void WriteByte(long index, byte value, bool noEvt = false);

    void InsertBytes(long index, byte[] bs);

    void DeleteBytes(long index, long length);

    long Length { get; }

    event EventHandler LengthChanged;

    bool HasChanges();

    void ApplyChanges();

    event EventHandler<ByteProviderChanged> Changed;

    bool SupportsWriteByte();

    bool SupportsInsertBytes();

    bool SupportsDeleteBytes();
  }
}
