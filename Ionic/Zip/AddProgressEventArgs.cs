// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.AddProgressEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Ionic.Zip
{
  public class AddProgressEventArgs : ZipProgressEventArgs
  {
    internal AddProgressEventArgs()
    {
    }

    private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static AddProgressEventArgs AfterEntry(
      string archiveName,
      ZipEntry entry,
      int entriesTotal)
    {
      AddProgressEventArgs progressEventArgs = new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry);
      progressEventArgs.EntriesTotal = entriesTotal;
      progressEventArgs.CurrentEntry = entry;
      return progressEventArgs;
    }

    internal static AddProgressEventArgs Started(string archiveName) => new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);

    internal static AddProgressEventArgs Completed(string archiveName) => new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
  }
}
