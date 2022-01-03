// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.NullableDictionary`2
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  internal class NullableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
  {
    private bool hasNullKey;
    private TValue nullValue;

    public new TValue this[TKey key]
    {
      get
      {
        if ((object) key != null)
          return base[key];
        if (this.hasNullKey)
          return this.nullValue;
        throw new KeyNotFoundException();
      }
      set
      {
        if ((object) key == null)
        {
          this.hasNullKey = true;
          this.nullValue = value;
        }
        else
          base[key] = value;
      }
    }

    public new bool ContainsKey(TKey key) => (object) key == null ? this.hasNullKey : base.ContainsKey(key);

    public IList Keys
    {
      get
      {
        ArrayList arrayList = new ArrayList((ICollection) base.Keys);
        if (this.hasNullKey)
          arrayList.Add((object) null);
        return (IList) arrayList;
      }
    }

    public IList<TValue> Values
    {
      get
      {
        List<TValue> objList = new List<TValue>((IEnumerable<TValue>) base.Values);
        if (this.hasNullKey)
          objList.Add(this.nullValue);
        return (IList<TValue>) objList;
      }
    }
  }
}
