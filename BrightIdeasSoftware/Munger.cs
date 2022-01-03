// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.Munger
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class Munger
  {
    private static bool ignoreMissingAspects = true;
    private string aspectName;
    private IList<SimpleMunger> aspectParts;

    public Munger()
    {
    }

    public Munger(string aspectName) => this.AspectName = aspectName;

    public static bool PutProperty(object target, string propertyName, object value)
    {
      try
      {
        return new Munger(propertyName).PutValue(target, value);
      }
      catch (MungerException ex)
      {
      }
      return false;
    }

    public static bool IgnoreMissingAspects
    {
      get => Munger.ignoreMissingAspects;
      set => Munger.ignoreMissingAspects = value;
    }

    public string AspectName
    {
      get => this.aspectName;
      set
      {
        this.aspectName = value;
        this.aspectParts = (IList<SimpleMunger>) null;
      }
    }

    public object GetValue(object target)
    {
      if (this.Parts.Count == 0)
        return (object) null;
      try
      {
        return this.EvaluateParts(target, this.Parts);
      }
      catch (MungerException ex)
      {
        return Munger.IgnoreMissingAspects ? (object) null : (object) string.Format("'{0}' is not a parameter-less method, property or field of type '{1}'", (object) ex.Munger.AspectName, (object) ex.Target.GetType());
      }
    }

    public object GetValueEx(object target) => this.Parts.Count == 0 ? (object) null : this.EvaluateParts(target, this.Parts);

    public bool PutValue(object target, object value)
    {
      if (this.Parts.Count == 0)
        return false;
      SimpleMunger part = this.Parts[this.Parts.Count - 1];
      if (this.Parts.Count > 1)
      {
        List<SimpleMunger> simpleMungerList = new List<SimpleMunger>((IEnumerable<SimpleMunger>) this.Parts);
        simpleMungerList.RemoveAt(simpleMungerList.Count - 1);
        try
        {
          target = this.EvaluateParts(target, (IList<SimpleMunger>) simpleMungerList);
        }
        catch (MungerException ex)
        {
          this.ReportPutValueException(ex);
          return false;
        }
      }
      if (target != null)
      {
        try
        {
          return part.PutValue(target, value);
        }
        catch (MungerException ex)
        {
          this.ReportPutValueException(ex);
        }
      }
      return false;
    }

    private IList<SimpleMunger> Parts
    {
      get
      {
        if (this.aspectParts == null)
          this.aspectParts = this.BuildParts(this.AspectName);
        return this.aspectParts;
      }
    }

    private IList<SimpleMunger> BuildParts(string aspect)
    {
      List<SimpleMunger> simpleMungerList = new List<SimpleMunger>();
      if (!string.IsNullOrEmpty(aspect))
      {
        string str1 = aspect;
        char[] chArray = new char[1]{ '.' };
        foreach (string str2 in str1.Split(chArray))
          simpleMungerList.Add(new SimpleMunger(str2.Trim()));
      }
      return (IList<SimpleMunger>) simpleMungerList;
    }

    private object EvaluateParts(object target, IList<SimpleMunger> parts)
    {
      foreach (SimpleMunger part in (IEnumerable<SimpleMunger>) parts)
      {
        if (target != null)
          target = part.GetValue(target);
        else
          break;
      }
      return target;
    }

    private void ReportPutValueException(MungerException ex)
    {
    }
  }
}
