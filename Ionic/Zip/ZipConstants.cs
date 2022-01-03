// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipConstants
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zip
{
  internal static class ZipConstants
  {
    public const uint PackedToRemovableMedia = 808471376;
    public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792;
    public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008;
    public const uint EndOfCentralDirectorySignature = 101010256;
    public const int ZipEntrySignature = 67324752;
    public const int ZipEntryDataDescriptorSignature = 134695760;
    public const int SplitArchiveSignature = 134695760;
    public const int ZipDirEntrySignature = 33639248;
    public const int AesKeySize = 192;
    public const int AesBlockSize = 128;
    public const ushort AesAlgId128 = 26126;
    public const ushort AesAlgId192 = 26127;
    public const ushort AesAlgId256 = 26128;
  }
}
