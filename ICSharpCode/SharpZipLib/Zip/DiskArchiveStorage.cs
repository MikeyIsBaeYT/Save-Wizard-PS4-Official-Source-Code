// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.DiskArchiveStorage
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class DiskArchiveStorage : BaseArchiveStorage
  {
    private Stream temporaryStream_;
    private string fileName_;
    private string temporaryName_;

    public DiskArchiveStorage(ZipFile file, FileUpdateMode updateMode)
      : base(updateMode)
    {
      this.fileName_ = file.Name != null ? file.Name : throw new ZipException("Cant handle non file archives");
    }

    public DiskArchiveStorage(ZipFile file)
      : this(file, FileUpdateMode.Safe)
    {
    }

    public override Stream GetTemporaryOutput()
    {
      if (this.temporaryName_ != null)
      {
        this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.temporaryName_, true);
        this.temporaryStream_ = (Stream) File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
      }
      else
      {
        this.temporaryName_ = Path.GetTempFileName();
        this.temporaryStream_ = (Stream) File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
      }
      return this.temporaryStream_;
    }

    public override Stream ConvertTemporaryToFinal()
    {
      if (this.temporaryStream_ == null)
        throw new ZipException("No temporary stream has been created");
      Stream stream1 = (Stream) null;
      string tempFileName = DiskArchiveStorage.GetTempFileName(this.fileName_, false);
      bool flag = false;
      Stream stream2;
      try
      {
        this.temporaryStream_.Close();
        File.Move(this.fileName_, tempFileName);
        File.Move(this.temporaryName_, this.fileName_);
        flag = true;
        File.Delete(tempFileName);
        stream2 = (Stream) File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
      }
      catch (Exception ex)
      {
        stream1 = (Stream) null;
        if (!flag)
        {
          File.Move(tempFileName, this.fileName_);
          File.Delete(this.temporaryName_);
        }
        throw;
      }
      return stream2;
    }

    public override Stream MakeTemporaryCopy(Stream stream)
    {
      stream.Close();
      this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.fileName_, true);
      File.Copy(this.fileName_, this.temporaryName_, true);
      this.temporaryStream_ = (Stream) new FileStream(this.temporaryName_, FileMode.Open, FileAccess.ReadWrite);
      return this.temporaryStream_;
    }

    public override Stream OpenForDirectUpdate(Stream stream)
    {
      Stream stream1;
      if (stream == null || !stream.CanWrite)
      {
        stream?.Close();
        stream1 = (Stream) new FileStream(this.fileName_, FileMode.Open, FileAccess.ReadWrite);
      }
      else
        stream1 = stream;
      return stream1;
    }

    public override void Dispose()
    {
      if (this.temporaryStream_ == null)
        return;
      this.temporaryStream_.Close();
    }

    private static string GetTempFileName(string original, bool makeTempFile)
    {
      string str = (string) null;
      if (original == null)
      {
        str = Path.GetTempFileName();
      }
      else
      {
        int num = 0;
        int second = DateTime.Now.Second;
        while (str == null)
        {
          ++num;
          string path = string.Format("{0}.{1}{2}.tmp", (object) original, (object) second, (object) num);
          if (!File.Exists(path))
          {
            if (makeTempFile)
            {
              try
              {
                using (File.Create(path))
                  ;
                str = path;
              }
              catch
              {
                second = DateTime.Now.Second;
              }
            }
            else
              str = path;
          }
        }
      }
      return str;
    }
  }
}
