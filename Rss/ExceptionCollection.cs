// Decompiled with JetBrains decompiler
// Type: Rss.ExceptionCollection
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  [Serializable]
  public class ExceptionCollection : CollectionBase
  {
    private Exception lastException = (Exception) null;

    public Exception this[int index]
    {
      get => (Exception) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(Exception exception)
    {
      foreach (Exception exception1 in (IEnumerable) this.List)
      {
        if (exception1.Message == exception.Message)
          return -1;
      }
      this.lastException = exception;
      return this.List.Add((object) exception);
    }

    public bool Contains(Exception exception) => this.List.Contains((object) exception);

    public void CopyTo(Exception[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(Exception exception) => this.List.IndexOf((object) exception);

    public void Insert(int index, Exception exception) => this.List.Insert(index, (object) exception);

    public void Remove(Exception exception) => this.List.Remove((object) exception);

    public Exception LastException => this.lastException;
  }
}
