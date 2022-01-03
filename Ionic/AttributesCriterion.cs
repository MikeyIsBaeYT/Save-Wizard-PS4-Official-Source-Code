// Decompiled with JetBrains decompiler
// Type: Ionic.AttributesCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.IO;
using System.Text;

namespace Ionic
{
  internal class AttributesCriterion : SelectionCriterion
  {
    private FileAttributes _Attributes;
    internal ComparisonOperator Operator;

    internal string AttributeString
    {
      get
      {
        string str = "";
        if ((uint) (this._Attributes & FileAttributes.Hidden) > 0U)
          str += "H";
        if ((uint) (this._Attributes & FileAttributes.System) > 0U)
          str += "S";
        if ((uint) (this._Attributes & FileAttributes.ReadOnly) > 0U)
          str += "R";
        if ((uint) (this._Attributes & FileAttributes.Archive) > 0U)
          str += "A";
        if ((uint) (this._Attributes & FileAttributes.ReparsePoint) > 0U)
          str += "L";
        if ((uint) (this._Attributes & FileAttributes.NotContentIndexed) > 0U)
          str += "I";
        return str;
      }
      set
      {
        this._Attributes = FileAttributes.Normal;
        foreach (char ch in value.ToUpper())
        {
          switch (ch)
          {
            case 'A':
              if ((uint) (this._Attributes & FileAttributes.Archive) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.Archive;
              break;
            case 'H':
              if ((uint) (this._Attributes & FileAttributes.Hidden) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.Hidden;
              break;
            case 'I':
              if ((uint) (this._Attributes & FileAttributes.NotContentIndexed) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.NotContentIndexed;
              break;
            case 'L':
              if ((uint) (this._Attributes & FileAttributes.ReparsePoint) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.ReparsePoint;
              break;
            case 'R':
              if ((uint) (this._Attributes & FileAttributes.ReadOnly) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.ReadOnly;
              break;
            case 'S':
              if ((uint) (this._Attributes & FileAttributes.System) > 0U)
                throw new ArgumentException(string.Format("Repeated flag. ({0})", (object) ch), nameof (value));
              this._Attributes |= FileAttributes.System;
              break;
            default:
              throw new ArgumentException(value);
          }
        }
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription((Enum) this.Operator)).Append(" ").Append(this.AttributeString);
      return stringBuilder.ToString();
    }

    private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs) => (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;

    internal override bool Evaluate(string filename) => Directory.Exists(filename) ? this.Operator != ComparisonOperator.EqualTo : this._Evaluate(File.GetAttributes(filename));

    private bool _Evaluate(FileAttributes fileAttrs)
    {
      bool flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden);
      if (flag)
        flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
      if (flag)
        flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
      if (flag)
        flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
      if (flag)
        flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
      if (flag)
        flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
      if (this.Operator != ComparisonOperator.EqualTo)
        flag = !flag;
      return flag;
    }

    internal override bool Evaluate(ZipEntry entry) => this._Evaluate(entry.Attributes);
  }
}
