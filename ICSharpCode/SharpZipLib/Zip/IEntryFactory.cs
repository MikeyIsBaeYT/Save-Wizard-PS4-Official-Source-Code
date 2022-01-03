// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.IEntryFactory
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
  public interface IEntryFactory
  {
    ZipEntry MakeFileEntry(string fileName);

    ZipEntry MakeFileEntry(string fileName, bool useFileSystem);

    ZipEntry MakeDirectoryEntry(string directoryName);

    ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);

    INameTransform NameTransform { get; set; }
  }
}
