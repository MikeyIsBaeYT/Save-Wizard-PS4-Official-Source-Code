// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.DynamicDiskDataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class DynamicDiskDataSource : IDynamicDataSource
  {
    public Stream GetSource(ZipEntry entry, string name)
    {
      Stream stream = (Stream) null;
      if (name != null)
        stream = (Stream) File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
      return stream;
    }
  }
}
