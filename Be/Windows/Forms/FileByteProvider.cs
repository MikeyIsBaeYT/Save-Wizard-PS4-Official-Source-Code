// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.FileByteProvider
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.IO;

namespace Be.Windows.Forms
{
  public class FileByteProvider : IByteProvider, IDisposable
  {
    private FileByteProvider.WriteCollection _writes = new FileByteProvider.WriteCollection();
    private string _fileName;
    private FileStream _fileStream;
    private bool _readOnly;

    public event EventHandler<ByteProviderChanged> Changed;

    public FileByteProvider(string fileName)
    {
      this._fileName = fileName;
      try
      {
        this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
      }
      catch
      {
        try
        {
          this._fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
          this._readOnly = true;
        }
        catch
        {
          throw;
        }
      }
    }

    ~FileByteProvider() => this.Dispose();

    private void OnChanged(ByteProviderChanged e)
    {
      if (this.Changed == null)
        return;
      this.Changed((object) this, e);
    }

    public string FileName => this._fileName;

    public bool HasChanges() => this._writes.Count > 0;

    public void ApplyChanges()
    {
      if (this._readOnly)
        throw new Exception("File is in read-only mode.");
      if (!this.HasChanges())
        return;
      IDictionaryEnumerator enumerator = this._writes.GetEnumerator();
      while (enumerator.MoveNext())
      {
        long key = (long) enumerator.Key;
        byte num = (byte) enumerator.Value;
        if (this._fileStream.Position != key)
          this._fileStream.Position = key;
        this._fileStream.Write(new byte[1]{ num }, 0, 1);
      }
      this._writes.Clear();
    }

    public void RejectChanges() => this._writes.Clear();

    public event EventHandler LengthChanged;

    public byte ReadByte(long index)
    {
      if (this._writes.Contains(index))
        return this._writes[index];
      if (this._fileStream.Position != index)
        this._fileStream.Position = index;
      return (byte) this._fileStream.ReadByte();
    }

    public long Length => this._fileStream.Length;

    public void WriteByte(long index, byte value, bool noEvt = false)
    {
      if (this._writes.Contains(index))
        this._writes[index] = value;
      else
        this._writes.Add(index, value);
      if (!noEvt)
        return;
      this.OnChanged(new ByteProviderChanged());
    }

    public void DeleteBytes(long index, long length) => throw new NotSupportedException("FileByteProvider.DeleteBytes");

    public void InsertBytes(long index, byte[] bs) => throw new NotSupportedException("FileByteProvider.InsertBytes");

    public bool SupportsWriteByte() => !this._readOnly;

    public bool SupportsInsertBytes() => false;

    public bool SupportsDeleteBytes() => false;

    public void Dispose()
    {
      if (this._fileStream != null)
      {
        this._fileName = (string) null;
        this._fileStream.Close();
        this._fileStream = (FileStream) null;
      }
      GC.SuppressFinalize((object) this);
    }

    private class WriteCollection : DictionaryBase
    {
      public byte this[long index]
      {
        get => (byte) this.Dictionary[(object) index];
        set => this.Dictionary[(object) index] = (object) value;
      }

      public void Add(long index, byte value) => this.Dictionary.Add((object) index, (object) value);

      public bool Contains(long index) => this.Dictionary.Contains((object) index);
    }
  }
}
