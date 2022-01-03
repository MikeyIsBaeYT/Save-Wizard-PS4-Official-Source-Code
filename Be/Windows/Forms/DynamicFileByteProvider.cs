// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.DynamicFileByteProvider
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.IO;
using System.Windows.Forms;

namespace Be.Windows.Forms
{
  public sealed class DynamicFileByteProvider : IByteProvider, IDisposable
  {
    private const int COPY_BLOCK_SIZE = 4096;
    private string _fileName;
    private Stream _stream;
    private DataMap _dataMap;
    private long _totalLength;
    private bool _readOnly;

    public DynamicFileByteProvider(string fileName)
      : this(fileName, false)
    {
    }

    public DynamicFileByteProvider(string fileName, bool readOnly)
    {
      this._fileName = fileName;
      try
      {
        this._stream = readOnly ? (Stream) File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) : (Stream) File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
      }
      catch (UnauthorizedAccessException ex)
      {
        int num = (int) CustomMsgBox.Show(ex.Message, "UnauthorizedAccess", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
      }
      catch (Exception ex)
      {
        int num = (int) CustomMsgBox.Show(ex.Message);
      }
      this._readOnly = readOnly;
      this.ReInitialize();
    }

    public DynamicFileByteProvider(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      this._stream = stream.CanSeek ? stream : throw new ArgumentException("stream must supported seek operations(CanSeek)");
      this._readOnly = !stream.CanWrite;
      this.ReInitialize();
    }

    public event EventHandler LengthChanged;

    public event EventHandler<ByteProviderChanged> Changed;

    public byte ReadByte(long index)
    {
      long blockOffset;
      DataBlock dataBlock = this.GetDataBlock(index, out blockOffset);
      return dataBlock is FileDataBlock fileDataBlock ? this.ReadByteFromFile(fileDataBlock.FileOffset + index - blockOffset) : ((MemoryDataBlock) dataBlock).Data[index - blockOffset];
    }

    public void WriteByte(long index, byte value, bool noEvt = false)
    {
      try
      {
        long blockOffset;
        DataBlock dataBlock = this.GetDataBlock(index, out blockOffset);
        if (dataBlock is MemoryDataBlock memoryDataBlock2)
        {
          memoryDataBlock2.Data[index - blockOffset] = value;
        }
        else
        {
          FileDataBlock fileDataBlock1 = (FileDataBlock) dataBlock;
          if (blockOffset == index && dataBlock.PreviousBlock != null && dataBlock.PreviousBlock is MemoryDataBlock previousBlock4)
          {
            previousBlock4.AddByteToEnd(value);
            fileDataBlock1.RemoveBytesFromStart(1L);
            if (fileDataBlock1.Length != 0L)
              return;
            this._dataMap.Remove((DataBlock) fileDataBlock1);
          }
          else if (blockOffset + fileDataBlock1.Length - 1L == index && dataBlock.NextBlock != null && dataBlock.NextBlock is MemoryDataBlock nextBlock6)
          {
            nextBlock6.AddByteToStart(value);
            fileDataBlock1.RemoveBytesFromEnd(1L);
            if (fileDataBlock1.Length != 0L)
              return;
            this._dataMap.Remove((DataBlock) fileDataBlock1);
          }
          else
          {
            FileDataBlock fileDataBlock2 = (FileDataBlock) null;
            if (index > blockOffset)
              fileDataBlock2 = new FileDataBlock(fileDataBlock1.FileOffset, index - blockOffset);
            FileDataBlock fileDataBlock3 = (FileDataBlock) null;
            if (index < blockOffset + fileDataBlock1.Length - 1L)
              fileDataBlock3 = new FileDataBlock(fileDataBlock1.FileOffset + index - blockOffset + 1L, fileDataBlock1.Length - (index - blockOffset + 1L));
            DataBlock block = this._dataMap.Replace(dataBlock, (DataBlock) new MemoryDataBlock(value));
            if (fileDataBlock2 != null)
              this._dataMap.AddBefore(block, (DataBlock) fileDataBlock2);
            if (fileDataBlock3 == null)
              return;
            this._dataMap.AddAfter(block, (DataBlock) fileDataBlock3);
          }
        }
      }
      finally
      {
        if (noEvt)
          this.OnChanged(new ByteProviderChanged());
      }
    }

    public void InsertBytes(long index, byte[] bs)
    {
      try
      {
        long blockOffset;
        DataBlock dataBlock = this.GetDataBlock(index, out blockOffset);
        if (dataBlock is MemoryDataBlock memoryDataBlock2)
        {
          memoryDataBlock2.InsertBytes(index - blockOffset, bs);
        }
        else
        {
          FileDataBlock fileDataBlock1 = (FileDataBlock) dataBlock;
          if (blockOffset == index && dataBlock.PreviousBlock != null && dataBlock.PreviousBlock is MemoryDataBlock previousBlock4)
          {
            previousBlock4.InsertBytes(previousBlock4.Length, bs);
          }
          else
          {
            FileDataBlock fileDataBlock2 = (FileDataBlock) null;
            if (index > blockOffset)
              fileDataBlock2 = new FileDataBlock(fileDataBlock1.FileOffset, index - blockOffset);
            FileDataBlock fileDataBlock3 = (FileDataBlock) null;
            if (index < blockOffset + fileDataBlock1.Length)
              fileDataBlock3 = new FileDataBlock(fileDataBlock1.FileOffset + index - blockOffset, fileDataBlock1.Length - (index - blockOffset));
            DataBlock block = this._dataMap.Replace(dataBlock, (DataBlock) new MemoryDataBlock(bs));
            if (fileDataBlock2 != null)
              this._dataMap.AddBefore(block, (DataBlock) fileDataBlock2);
            if (fileDataBlock3 == null)
              return;
            this._dataMap.AddAfter(block, (DataBlock) fileDataBlock3);
          }
        }
      }
      finally
      {
        this._totalLength += (long) bs.Length;
        this.OnLengthChanged(EventArgs.Empty);
        this.OnChanged(new ByteProviderChanged());
      }
    }

    public void DeleteBytes(long index, long length)
    {
      try
      {
        long val1 = length;
        long blockOffset;
        DataBlock nextBlock;
        for (DataBlock block = this.GetDataBlock(index, out blockOffset); val1 > 0L && block != null; block = val1 > 0L ? nextBlock : (DataBlock) null)
        {
          long length1 = block.Length;
          nextBlock = block.NextBlock;
          long count = Math.Min(val1, length1 - (index - blockOffset));
          block.RemoveBytes(index - blockOffset, count);
          if (block.Length == 0L)
          {
            this._dataMap.Remove(block);
            if (this._dataMap.FirstBlock == null)
              this._dataMap.AddFirst((DataBlock) new MemoryDataBlock(new byte[0]));
          }
          val1 -= count;
          blockOffset += block.Length;
        }
      }
      finally
      {
        this._totalLength -= length;
        this.OnLengthChanged(EventArgs.Empty);
        this.OnChanged(new ByteProviderChanged());
      }
    }

    public long Length => this._totalLength;

    public bool HasChanges()
    {
      if (this._readOnly)
        return false;
      if (this._totalLength != this._stream.Length)
        return true;
      long num = 0;
      for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
      {
        if (!(dataBlock is FileDataBlock fileDataBlock2) || fileDataBlock2.FileOffset != num)
          return true;
        num += fileDataBlock2.Length;
      }
      return num != this._stream.Length;
    }

    public void ApplyChanges()
    {
      if (this._readOnly)
        throw new OperationCanceledException("File is in read-only mode");
      if (this._totalLength > this._stream.Length)
        this._stream.SetLength(this._totalLength);
      long dataOffset = 0;
      for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
      {
        if (dataBlock is FileDataBlock fileBlock2 && fileBlock2.FileOffset != dataOffset)
          this.MoveFileBlock(fileBlock2, dataOffset);
        dataOffset += dataBlock.Length;
      }
      long num = 0;
      for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
      {
        if (dataBlock is MemoryDataBlock memoryDataBlock2)
        {
          this._stream.Position = num;
          for (int offset = 0; (long) offset < memoryDataBlock2.Length; offset += 4096)
            this._stream.Write(memoryDataBlock2.Data, offset, (int) Math.Min(4096L, memoryDataBlock2.Length - (long) offset));
        }
        num += dataBlock.Length;
      }
      this._stream.SetLength(this._totalLength);
      this.ReInitialize();
    }

    public bool SupportsWriteByte() => !this._readOnly;

    public bool SupportsInsertBytes() => !this._readOnly;

    public bool SupportsDeleteBytes() => !this._readOnly;

    ~DynamicFileByteProvider() => this.Dispose();

    public void Dispose()
    {
      if (this._stream != null)
      {
        this._stream.Close();
        this._stream = (Stream) null;
      }
      this._fileName = (string) null;
      this._dataMap = (DataMap) null;
      GC.SuppressFinalize((object) this);
    }

    public bool ReadOnly => this._readOnly;

    private void OnLengthChanged(EventArgs e)
    {
      if (this.LengthChanged == null)
        return;
      this.LengthChanged((object) this, e);
    }

    private void OnChanged(ByteProviderChanged e)
    {
      if (this.Changed == null)
        return;
      this.Changed((object) this, e);
    }

    private DataBlock GetDataBlock(long findOffset, out long blockOffset)
    {
      if (findOffset < 0L || findOffset > this._totalLength)
        throw new ArgumentOutOfRangeException(nameof (findOffset));
      blockOffset = 0L;
      for (DataBlock dataBlock = this._dataMap.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
      {
        if (blockOffset <= findOffset && blockOffset + dataBlock.Length > findOffset || dataBlock.NextBlock == null)
          return dataBlock;
        blockOffset += dataBlock.Length;
      }
      return (DataBlock) null;
    }

    private FileDataBlock GetNextFileDataBlock(
      DataBlock block,
      long dataOffset,
      out long nextDataOffset)
    {
      nextDataOffset = dataOffset + block.Length;
      for (block = block.NextBlock; block != null; block = block.NextBlock)
      {
        if (block is FileDataBlock fileDataBlock2)
          return fileDataBlock2;
        nextDataOffset += block.Length;
      }
      return (FileDataBlock) null;
    }

    private byte ReadByteFromFile(long fileOffset)
    {
      if (this._stream.Position != fileOffset)
        this._stream.Position = fileOffset;
      return (byte) this._stream.ReadByte();
    }

    private void MoveFileBlock(FileDataBlock fileBlock, long dataOffset)
    {
      long nextDataOffset;
      FileDataBlock nextFileDataBlock = this.GetNextFileDataBlock((DataBlock) fileBlock, dataOffset, out nextDataOffset);
      if (nextFileDataBlock != null && dataOffset + fileBlock.Length > nextFileDataBlock.FileOffset)
        this.MoveFileBlock(nextFileDataBlock, nextDataOffset);
      if (fileBlock.FileOffset > dataOffset)
      {
        byte[] buffer = new byte[4096];
        for (long index = 0; index < fileBlock.Length; index += (long) buffer.Length)
        {
          long num = fileBlock.FileOffset + index;
          int count = (int) Math.Min((long) buffer.Length, fileBlock.Length - index);
          this._stream.Position = num;
          this._stream.Read(buffer, 0, count);
          this._stream.Position = dataOffset + index;
          this._stream.Write(buffer, 0, count);
        }
      }
      else
      {
        byte[] buffer = new byte[4096];
        for (long index = 0; index < fileBlock.Length; index += (long) buffer.Length)
        {
          int count = (int) Math.Min((long) buffer.Length, fileBlock.Length - index);
          this._stream.Position = fileBlock.FileOffset + fileBlock.Length - index - (long) count;
          this._stream.Read(buffer, 0, count);
          this._stream.Position = dataOffset + fileBlock.Length - index - (long) count;
          this._stream.Write(buffer, 0, count);
        }
      }
      fileBlock.SetFileOffset(dataOffset);
    }

    private void ReInitialize()
    {
      this._dataMap = new DataMap();
      this._dataMap.AddFirst((DataBlock) new FileDataBlock(0L, this._stream.Length));
      this._totalLength = this._stream.Length;
    }
  }
}
