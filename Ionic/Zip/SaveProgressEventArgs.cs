// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SaveProgressEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zip
{
  public class SaveProgressEventArgs : ZipProgressEventArgs
  {
    private int _entriesSaved;

    internal SaveProgressEventArgs(
      string archiveName,
      bool before,
      int entriesTotal,
      int entriesSaved,
      ZipEntry entry)
      : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
    {
      this.EntriesTotal = entriesTotal;
      this.CurrentEntry = entry;
      this._entriesSaved = entriesSaved;
    }

    internal SaveProgressEventArgs()
    {
    }

    internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static SaveProgressEventArgs ByteUpdate(
      string archiveName,
      ZipEntry entry,
      long bytesXferred,
      long totalBytes)
    {
      SaveProgressEventArgs progressEventArgs = new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead);
      progressEventArgs.ArchiveName = archiveName;
      progressEventArgs.CurrentEntry = entry;
      progressEventArgs.BytesTransferred = bytesXferred;
      progressEventArgs.TotalBytesToTransfer = totalBytes;
      return progressEventArgs;
    }

    internal static SaveProgressEventArgs Started(string archiveName) => new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);

    internal static SaveProgressEventArgs Completed(string archiveName) => new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);

    public int EntriesSaved => this._entriesSaved;
  }
}
