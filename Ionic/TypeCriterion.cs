// Decompiled with JetBrains decompiler
// Type: Ionic.TypeCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

namespace Ionic
{
  internal class TypeCriterion : SelectionCriterion
  {
    private char ObjectType;
    internal ComparisonOperator Operator;

    internal string AttributeString
    {
      get => this.ObjectType.ToString();
      set => this.ObjectType = value.Length == 1 && (value[0] == 'D' || value[0] == 'F') ? value[0] : throw new ArgumentException("Specify a single character: either D or F");
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("type ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.AttributeString);
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(string filename)
    {
      bool flag = this.ObjectType == 'D' ? Directory.Exists(filename) : File.Exists(filename);
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      bool flag = this.ObjectType == 'D' ? entry.IsDirectory : !entry.IsDirectory;
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }
  }
}
