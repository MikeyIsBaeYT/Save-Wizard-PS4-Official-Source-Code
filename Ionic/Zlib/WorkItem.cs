// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.WorkItem
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zlib
{
  internal class WorkItem
  {
    public byte[] buffer;
    public byte[] compressed;
    public int crc;
    public int index;
    public int ordinal;
    public int inputBytesAvailable;
    public int compressedBytesAvailable;
    public ZlibCodec compressor;

    public WorkItem(
      int size,
      CompressionLevel compressLevel,
      CompressionStrategy strategy,
      int ix)
    {
      this.buffer = new byte[size];
      this.compressed = new byte[size + (size / 32768 + 1) * 5 * 2];
      this.compressor = new ZlibCodec();
      this.compressor.InitializeDeflate(compressLevel, false);
      this.compressor.OutputBuffer = this.compressed;
      this.compressor.InputBuffer = this.buffer;
      this.index = ix;
    }
  }
}
