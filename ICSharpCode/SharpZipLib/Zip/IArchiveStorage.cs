// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.IArchiveStorage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public interface IArchiveStorage
  {
    FileUpdateMode UpdateMode { get; }

    Stream GetTemporaryOutput();

    Stream ConvertTemporaryToFinal();

    Stream MakeTemporaryCopy(Stream stream);

    Stream OpenForDirectUpdate(Stream stream);

    void Dispose();
  }
}
