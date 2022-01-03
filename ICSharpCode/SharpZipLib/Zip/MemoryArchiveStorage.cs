// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.MemoryArchiveStorage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class MemoryArchiveStorage : BaseArchiveStorage
  {
    private MemoryStream temporaryStream_;
    private MemoryStream finalStream_;

    public MemoryArchiveStorage()
      : base(FileUpdateMode.Direct)
    {
    }

    public MemoryArchiveStorage(FileUpdateMode updateMode)
      : base(updateMode)
    {
    }

    public MemoryStream FinalStream => this.finalStream_;

    public override Stream GetTemporaryOutput()
    {
      this.temporaryStream_ = new MemoryStream();
      return (Stream) this.temporaryStream_;
    }

    public override Stream ConvertTemporaryToFinal()
    {
      this.finalStream_ = this.temporaryStream_ != null ? new MemoryStream(this.temporaryStream_.ToArray()) : throw new ZipException("No temporary stream has been created");
      return (Stream) this.finalStream_;
    }

    public override Stream MakeTemporaryCopy(Stream stream)
    {
      this.temporaryStream_ = new MemoryStream();
      stream.Position = 0L;
      StreamUtils.Copy(stream, (Stream) this.temporaryStream_, new byte[4096]);
      return (Stream) this.temporaryStream_;
    }

    public override Stream OpenForDirectUpdate(Stream stream)
    {
      Stream destination;
      if (stream == null || !stream.CanWrite)
      {
        destination = (Stream) new MemoryStream();
        if (stream != null)
        {
          stream.Position = 0L;
          StreamUtils.Copy(stream, destination, new byte[4096]);
          stream.Close();
        }
      }
      else
        destination = stream;
      return destination;
    }

    public override void Dispose()
    {
      if (this.temporaryStream_ == null)
        return;
      this.temporaryStream_.Close();
    }
  }
}
