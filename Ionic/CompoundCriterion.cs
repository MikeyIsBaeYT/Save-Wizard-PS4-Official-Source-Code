// Decompiled with JetBrains decompiler
// Type: Ionic.CompoundCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.Text;

namespace Ionic
{
  internal class CompoundCriterion : SelectionCriterion
  {
    internal LogicalConjunction Conjunction;
    internal SelectionCriterion Left;
    private SelectionCriterion _Right;

    internal SelectionCriterion Right
    {
      get => this._Right;
      set
      {
        this._Right = value;
        if (value == null)
        {
          this.Conjunction = LogicalConjunction.NONE;
        }
        else
        {
          if (this.Conjunction != LogicalConjunction.NONE)
            return;
          this.Conjunction = LogicalConjunction.AND;
        }
      }
    }

    internal override bool Evaluate(string filename)
    {
      bool flag = this.Left.Evaluate(filename);
      switch (this.Conjunction)
      {
        case LogicalConjunction.AND:
          if (flag)
          {
            flag = this.Right.Evaluate(filename);
            break;
          }
          break;
        case LogicalConjunction.OR:
          if (!flag)
          {
            flag = this.Right.Evaluate(filename);
            break;
          }
          break;
        case LogicalConjunction.XOR:
          flag ^= this.Right.Evaluate(filename);
          break;
        default:
          throw new ArgumentException("Conjunction");
      }
      return flag;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("(").Append(this.Left != null ? this.Left.ToString() : "null").Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append(this.Right != null ? this.Right.ToString() : "null").Append(")");
      return stringBuilder.ToString();
    }

    internal override bool Evaluate(ZipEntry entry)
    {
      bool flag = this.Left.Evaluate(entry);
      switch (this.Conjunction)
      {
        case LogicalConjunction.AND:
          if (flag)
          {
            flag = this.Right.Evaluate(entry);
            break;
          }
          break;
        case LogicalConjunction.OR:
          if (!flag)
          {
            flag = this.Right.Evaluate(entry);
            break;
          }
          break;
        case LogicalConjunction.XOR:
          flag ^= this.Right.Evaluate(entry);
          break;
      }
      return flag;
    }
  }
}
