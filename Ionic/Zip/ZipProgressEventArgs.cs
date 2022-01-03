// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Ionic.Zip
{
  public class ZipProgressEventArgs : EventArgs
  {
    private int _entriesTotal;
    private bool _cancel;
    private ZipEntry _latestEntry;
    private ZipProgressEventType _flavor;
    private string _archiveName;
    private long _bytesTransferred;
    private long _totalBytesToTransfer;

    internal ZipProgressEventArgs()
    {
    }

    internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
    {
      this._archiveName = archiveName;
      this._flavor = flavor;
    }

    public int EntriesTotal
    {
      get => this._entriesTotal;
      set => this._entriesTotal = value;
    }

    public ZipEntry CurrentEntry
    {
      get => this._latestEntry;
      set => this._latestEntry = value;
    }

    public bool Cancel
    {
      get => this._cancel;
      set => this._cancel |= value;
    }

    public ZipProgressEventType EventType
    {
      get => this._flavor;
      set => this._flavor = value;
    }

    public string ArchiveName
    {
      get => this._archiveName;
      set => this._archiveName = value;
    }

    public long BytesTransferred
    {
      get => this._bytesTransferred;
      set => this._bytesTransferred = value;
    }

    public long TotalBytesToTransfer
    {
      get => this._totalBytesToTransfer;
      set => this._totalBytesToTransfer = value;
    }
  }
}
