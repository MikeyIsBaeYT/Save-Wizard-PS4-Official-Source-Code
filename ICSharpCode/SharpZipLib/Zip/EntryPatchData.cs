// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.EntryPatchData
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace ICSharpCode.SharpZipLib.Zip
{
  internal class EntryPatchData
  {
    private long sizePatchOffset_;
    private long crcPatchOffset_;

    public long SizePatchOffset
    {
      get => this.sizePatchOffset_;
      set => this.sizePatchOffset_ = value;
    }

    public long CrcPatchOffset
    {
      get => this.crcPatchOffset_;
      set => this.crcPatchOffset_ = value;
    }
  }
}
