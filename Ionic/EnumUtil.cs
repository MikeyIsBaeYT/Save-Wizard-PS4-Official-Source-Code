// Decompiled with JetBrains decompiler
// Type: Ionic.EnumUtil
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;

namespace Ionic
{
  internal sealed class EnumUtil
  {
    private EnumUtil()
    {
    }

    internal static string GetDescription(Enum value)
    {
      DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
      return (uint) customAttributes.Length > 0U ? customAttributes[0].Description : value.ToString();
    }

    internal static object Parse(Type enumType, string stringRepresentation) => EnumUtil.Parse(enumType, stringRepresentation, false);

    internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
    {
      if (ignoreCase)
        stringRepresentation = stringRepresentation.ToLower();
      foreach (Enum @enum in Enum.GetValues(enumType))
      {
        string str = EnumUtil.GetDescription(@enum);
        if (ignoreCase)
          str = str.ToLower();
        if (str == stringRepresentation)
          return (object) @enum;
      }
      return Enum.Parse(enumType, stringRepresentation, ignoreCase);
    }
  }
}
