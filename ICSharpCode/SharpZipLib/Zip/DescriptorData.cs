// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.DescriptorData
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace ICSharpCode.SharpZipLib.Zip
{
  public class DescriptorData
  {
    private long size;
    private long compressedSize;
    private long crc;

    public long CompressedSize
    {
      get => this.compressedSize;
      set => this.compressedSize = value;
    }

    public long Size
    {
      get => this.size;
      set => this.size = value;
    }

    public long Crc
    {
      get => this.crc;
      set => this.crc = value & (long) uint.MaxValue;
    }
  }
}
