// Decompiled with JetBrains decompiler
// Type: Ionic.SizeCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

namespace Ionic
{
  internal class SizeCriterion : SelectionCriterion
  {
    internal ComparisonOperator Operator;
    internal long Size;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("size ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.Size.ToString());
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename) => this._Evaluate(new FileInfo(filename).Length);

    private bool _Evaluate(long Length)
    {
      bool flag;
      switch (this.Operator)
      {
        case ComparisonOperator.GreaterThan:
          flag = Length > this.Size;
          break;
        case ComparisonOperator.GreaterThanOrEqualTo:
          flag = Length >= this.Size;
          break;
        case ComparisonOperator.LesserThan:
          flag = Length < this.Size;
          break;
        case ComparisonOperator.LesserThanOrEqualTo:
          flag = Length <= this.Size;
          break;
        case ComparisonOperator.EqualTo:
          flag = Length == this.Size;
          break;
        case ComparisonOperator.NotEqualTo:
          flag = Length != this.Size;
          break;
        default:
          throw new ArgumentException("Operator");
      }
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry) => this._Evaluate(entry.UncompressedSize);
  }
}
