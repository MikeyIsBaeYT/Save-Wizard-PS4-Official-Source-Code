// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.StaticDiskDataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class StaticDiskDataSource : IStaticDataSource
  {
    private string fileName_;

    public StaticDiskDataSource(string fileName) => this.fileName_ = fileName;

    public Stream GetSource() => (Stream) File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
  }
}
