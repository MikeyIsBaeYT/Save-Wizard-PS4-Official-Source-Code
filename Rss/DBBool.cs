// Decompiled with JetBrains decompiler
// Type: Rss.DBBool
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  [Serializable]
  public struct DBBool
  {
    public static readonly DBBool Null = new DBBool(0);
    public static readonly DBBool False = new DBBool(-1);
    public static readonly DBBool True = new DBBool(1);
    private sbyte value;

    private DBBool(int value) => this.value = (sbyte) value;

    public bool IsNull => this.value == (sbyte) 0;

    public bool IsFalse => this.value < (sbyte) 0;

    public bool IsTrue => this.value > (sbyte) 0;

    public static implicit operator DBBool(bool x) => x ? DBBool.True : DBBool.False;

    public static explicit operator bool(DBBool x)
    {
      if (x.value == (sbyte) 0)
        throw new InvalidOperationException();
      return x.value > (sbyte) 0;
    }

    public static DBBool operator ==(DBBool x, DBBool y) => x.value == (sbyte) 0 || y.value == (sbyte) 0 ? DBBool.Null : ((int) x.value == (int) y.value ? DBBool.True : DBBool.False);

    public static DBBool operator !=(DBBool x, DBBool y) => x.value == (sbyte) 0 || y.value == (sbyte) 0 ? DBBool.Null : ((int) x.value != (int) y.value ? DBBool.True : DBBool.False);

    public static DBBool operator !(DBBool x) => new DBBool((int) -x.value);

    public static DBBool operator &(DBBool x, DBBool y) => new DBBool((int) x.value < (int) y.value ? (int) x.value : (int) y.value);

    public static DBBool operator |(DBBool x, DBBool y) => new DBBool((int) x.value > (int) y.value ? (int) x.value : (int) y.value);

    public static bool operator true(DBBool x) => x.value > (sbyte) 0;

    public static bool operator false(DBBool x) => x.value < (sbyte) 0;

    public override bool Equals(object o)
    {
      try
      {
        return (bool) (this == (DBBool) o);
      }
      catch
      {
        return false;
      }
    }

    public override int GetHashCode() => (int) this.value;

    public override string ToString()
    {
      switch (this.value)
      {
        case -1:
          return "false";
        case 0:
          return "DBBool.Null";
        case 1:
          return "true";
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
