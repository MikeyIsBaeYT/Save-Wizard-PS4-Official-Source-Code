// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.BaseArchiveStorage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public abstract class BaseArchiveStorage : IArchiveStorage
  {
    private FileUpdateMode updateMode_;

    protected BaseArchiveStorage(FileUpdateMode updateMode) => this.updateMode_ = updateMode;

    public abstract Stream GetTemporaryOutput();

    public abstract Stream ConvertTemporaryToFinal();

    public abstract Stream MakeTemporaryCopy(Stream stream);

    public abstract Stream OpenForDirectUpdate(Stream stream);

    public abstract void Dispose();

    public FileUpdateMode UpdateMode => this.updateMode_;
  }
}
