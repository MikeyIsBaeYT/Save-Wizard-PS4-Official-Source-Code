// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.SimpleMunger
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Reflection;

namespace BrightIdeasSoftware
{
  public class SimpleMunger
  {
    private readonly string aspectName;
    private Type cachedTargetType;
    private string cachedName;
    private int cachedNumberParameters;
    private FieldInfo resolvedFieldInfo;
    private PropertyInfo resolvedPropertyInfo;
    private MethodInfo resolvedMethodInfo;
    private PropertyInfo indexerPropertyInfo;

    public SimpleMunger(string aspectName) => this.aspectName = aspectName;

    public string AspectName => this.aspectName;

    public object GetValue(object target)
    {
      if (target == null)
        return (object) null;
      this.ResolveName(target, this.AspectName, 0);
      try
      {
        if (this.resolvedPropertyInfo != (PropertyInfo) null)
          return this.resolvedPropertyInfo.GetValue(target, (object[]) null);
        if (this.resolvedMethodInfo != (MethodInfo) null)
          return this.resolvedMethodInfo.Invoke(target, (object[]) null);
        if (this.resolvedFieldInfo != (FieldInfo) null)
          return this.resolvedFieldInfo.GetValue(target);
        if (this.indexerPropertyInfo != (PropertyInfo) null)
          return this.indexerPropertyInfo.GetValue(target, new object[1]
          {
            (object) this.AspectName
          });
      }
      catch (Exception ex)
      {
        throw new MungerException(this, target, ex);
      }
      throw new MungerException(this, target, (Exception) new MissingMethodException());
    }

    public bool PutValue(object target, object value)
    {
      if (target == null)
        return false;
      this.ResolveName(target, this.AspectName, 1);
      try
      {
        if (this.resolvedPropertyInfo != (PropertyInfo) null)
        {
          this.resolvedPropertyInfo.SetValue(target, value, (object[]) null);
          return true;
        }
        if (this.resolvedMethodInfo != (MethodInfo) null)
        {
          this.resolvedMethodInfo.Invoke(target, new object[1]
          {
            value
          });
          return true;
        }
        if (this.resolvedFieldInfo != (FieldInfo) null)
        {
          this.resolvedFieldInfo.SetValue(target, value);
          return true;
        }
        if (this.indexerPropertyInfo != (PropertyInfo) null)
        {
          this.indexerPropertyInfo.SetValue(target, value, new object[1]
          {
            (object) this.AspectName
          });
          return true;
        }
      }
      catch (Exception ex)
      {
        throw new MungerException(this, target, ex);
      }
      return false;
    }

    private void ResolveName(object target, string name, int numberMethodParameters)
    {
      if (this.cachedTargetType == target.GetType() && this.cachedName == name && this.cachedNumberParameters == numberMethodParameters)
        return;
      this.cachedTargetType = target.GetType();
      this.cachedName = name;
      this.cachedNumberParameters = numberMethodParameters;
      this.resolvedFieldInfo = (FieldInfo) null;
      this.resolvedPropertyInfo = (PropertyInfo) null;
      this.resolvedMethodInfo = (MethodInfo) null;
      this.indexerPropertyInfo = (PropertyInfo) null;
      foreach (PropertyInfo property in target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.Name == name)
        {
          this.resolvedPropertyInfo = property;
          return;
        }
        if (this.indexerPropertyInfo == (PropertyInfo) null && property.Name == "Item")
        {
          ParameterInfo[] parameters = property.GetGetMethod().GetParameters();
          if ((uint) parameters.Length > 0U)
          {
            Type parameterType = parameters[0].ParameterType;
            if (parameterType == typeof (string) || parameterType == typeof (object))
              this.indexerPropertyInfo = property;
          }
        }
      }
      foreach (FieldInfo field in target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        if (field.Name == name)
        {
          this.resolvedFieldInfo = field;
          return;
        }
      }
      foreach (MethodInfo method in target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
      {
        if (method.Name == name && method.GetParameters().Length == numberMethodParameters)
        {
          this.resolvedMethodInfo = method;
          break;
        }
      }
    }
  }
}
