// Decompiled with JetBrains decompiler
// Type: Ionic.TimeCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

namespace Ionic
{
  internal class TimeCriterion : SelectionCriterion
  {
    internal ComparisonOperator Operator;
    internal WhichTime Which;
    internal DateTime Time;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename)
    {
      DateTime universalTime;
      switch (this.Which)
      {
        case WhichTime.atime:
          universalTime = File.GetLastAccessTime(filename).ToUniversalTime();
          break;
        case WhichTime.mtime:
          universalTime = File.GetLastWriteTime(filename).ToUniversalTime();
          break;
        case WhichTime.ctime:
          universalTime = File.GetCreationTime(filename).ToUniversalTime();
          break;
        default:
          throw new ArgumentException("Operator");
      }
      return this._Evaluate(universalTime);
    }

    private bool _Evaluate(DateTime x)
    {
      bool flag;
      switch (this.Operator)
      {
        case ComparisonOperator.GreaterThan:
          flag = x > this.Time;
          break;
        case ComparisonOperator.GreaterThanOrEqualTo:
          flag = x >= this.Time;
          break;
        case ComparisonOperator.LesserThan:
          flag = x < this.Time;
          break;
        case ComparisonOperator.LesserThanOrEqualTo:
          flag = x <= this.Time;
          break;
        case ComparisonOperator.EqualTo:
          flag = x == this.Time;
          break;
        case ComparisonOperator.NotEqualTo:
          flag = x != this.Time;
          break;
        default:
          throw new ArgumentException("Operator");
      }
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      DateTime x;
      switch (this.Which)
      {
        case WhichTime.atime:
          x = entry.AccessedTime;
          break;
        case WhichTime.mtime:
          x = entry.ModifiedTime;
          break;
        case WhichTime.ctime:
          x = entry.CreationTime;
          break;
        default:
          throw new ArgumentException("??time");
      }
      return this._Evaluate(x);
    }
  }
}
