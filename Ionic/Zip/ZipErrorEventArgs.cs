// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipErrorEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Ionic.Zip
{
  public class ZipErrorEventArgs : ZipProgressEventArgs
  {
    private Exception _exc;

    private ZipErrorEventArgs()
    {
    }

    internal static ZipErrorEventArgs Saving(
      string archiveName,
      ZipEntry entry,
      Exception exception)
    {
      ZipErrorEventArgs zipErrorEventArgs = new ZipErrorEventArgs();
      zipErrorEventArgs.EventType = ZipProgressEventType.Error_Saving;
      zipErrorEventArgs.ArchiveName = archiveName;
      zipErrorEventArgs.CurrentEntry = entry;
      zipErrorEventArgs._exc = exception;
      return zipErrorEventArgs;
    }

    public Exception Exception => this._exc;

    public string FileName => this.CurrentEntry.LocalFileName;
  }
}
