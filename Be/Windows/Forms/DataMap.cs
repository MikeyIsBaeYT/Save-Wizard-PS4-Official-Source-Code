// Decompiled with JetBrains decompiler
// Type: Be.Windows.Forms.DataMap
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Be.Windows.Forms
{
  internal class DataMap : ICollection, IEnumerable
  {
    private readonly object _syncRoot = new object();
    internal int _count;
    internal DataBlock _firstBlock;
    internal int _version;

    public DataMap()
    {
    }

    public DataMap(IEnumerable collection)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      foreach (DataBlock block in collection)
        this.AddLast(block);
    }

    public DataBlock FirstBlock => this._firstBlock;

    public void AddAfter(DataBlock block, DataBlock newBlock) => this.AddAfterInternal(block, newBlock);

    public void AddBefore(DataBlock block, DataBlock newBlock) => this.AddBeforeInternal(block, newBlock);

    public void AddFirst(DataBlock block)
    {
      if (this._firstBlock == null)
        this.AddBlockToEmptyMap(block);
      else
        this.AddBeforeInternal(this._firstBlock, block);
    }

    public void AddLast(DataBlock block)
    {
      if (this._firstBlock == null)
        this.AddBlockToEmptyMap(block);
      else
        this.AddAfterInternal(this.GetLastBlock(), block);
    }

    public void Remove(DataBlock block) => this.RemoveInternal(block);

    public void RemoveFirst()
    {
      if (this._firstBlock == null)
        throw new InvalidOperationException("The collection is empty.");
      this.RemoveInternal(this._firstBlock);
    }

    public void RemoveLast()
    {
      if (this._firstBlock == null)
        throw new InvalidOperationException("The collection is empty.");
      this.RemoveInternal(this.GetLastBlock());
    }

    public DataBlock Replace(DataBlock block, DataBlock newBlock)
    {
      this.AddAfterInternal(block, newBlock);
      this.RemoveInternal(block);
      return newBlock;
    }

    public void Clear()
    {
      DataBlock nextBlock;
      for (DataBlock block = this.FirstBlock; block != null; block = nextBlock)
      {
        nextBlock = block.NextBlock;
        this.InvalidateBlock(block);
      }
      this._firstBlock = (DataBlock) null;
      this._count = 0;
      ++this._version;
    }

    private void AddAfterInternal(DataBlock block, DataBlock newBlock)
    {
      newBlock._previousBlock = block;
      newBlock._nextBlock = block._nextBlock;
      newBlock._map = this;
      if (block._nextBlock != null)
        block._nextBlock._previousBlock = newBlock;
      block._nextBlock = newBlock;
      ++this._version;
      ++this._count;
    }

    private void AddBeforeInternal(DataBlock block, DataBlock newBlock)
    {
      newBlock._nextBlock = block;
      newBlock._previousBlock = block._previousBlock;
      newBlock._map = this;
      if (block._previousBlock != null)
        block._previousBlock._nextBlock = newBlock;
      block._previousBlock = newBlock;
      if (this._firstBlock == block)
        this._firstBlock = newBlock;
      ++this._version;
      ++this._count;
    }

    private void RemoveInternal(DataBlock block)
    {
      DataBlock previousBlock = block._previousBlock;
      DataBlock nextBlock = block._nextBlock;
      if (previousBlock != null)
        previousBlock._nextBlock = nextBlock;
      if (nextBlock != null)
        nextBlock._previousBlock = previousBlock;
      if (this._firstBlock == block)
        this._firstBlock = nextBlock;
      this.InvalidateBlock(block);
      --this._count;
      ++this._version;
    }

    private DataBlock GetLastBlock()
    {
      DataBlock dataBlock1 = (DataBlock) null;
      for (DataBlock dataBlock2 = this.FirstBlock; dataBlock2 != null; dataBlock2 = dataBlock2.NextBlock)
        dataBlock1 = dataBlock2;
      return dataBlock1;
    }

    private void InvalidateBlock(DataBlock block)
    {
      block._map = (DataMap) null;
      block._nextBlock = (DataBlock) null;
      block._previousBlock = (DataBlock) null;
    }

    private void AddBlockToEmptyMap(DataBlock block)
    {
      block._map = this;
      block._nextBlock = (DataBlock) null;
      block._previousBlock = (DataBlock) null;
      this._firstBlock = block;
      ++this._version;
      ++this._count;
    }

    public void CopyTo(Array array, int index)
    {
      DataBlock[] dataBlockArray = array as DataBlock[];
      for (DataBlock dataBlock = this.FirstBlock; dataBlock != null; dataBlock = dataBlock.NextBlock)
        dataBlockArray[index++] = dataBlock;
    }

    public int Count => this._count;

    public bool IsSynchronized => false;

    public object SyncRoot => this._syncRoot;

    public IEnumerator GetEnumerator() => (IEnumerator) new DataMap.Enumerator(this);

    internal class Enumerator : IEnumerator, IDisposable
    {
      private DataMap _map;
      private DataBlock _current;
      private int _index;
      private int _version;

      internal Enumerator(DataMap map)
      {
        this._map = map;
        this._version = map._version;
        this._current = (DataBlock) null;
        this._index = -1;
      }

      object IEnumerator.Current
      {
        get
        {
          if (this._index < 0 || this._index > this._map.Count)
            throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
          return (object) this._current;
        }
      }

      public bool MoveNext()
      {
        if (this._version != this._map._version)
          throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
        if (this._index >= this._map.Count)
          return false;
        this._current = ++this._index != 0 ? this._current.NextBlock : this._map.FirstBlock;
        return this._index < this._map.Count;
      }

      void IEnumerator.Reset()
      {
        if (this._version != this._map._version)
          throw new InvalidOperationException("Collection was modified after the enumerator was instantiated.");
        this._index = -1;
        this._current = (DataBlock) null;
      }

      public void Dispose()
      {
      }
    }
  }
}
