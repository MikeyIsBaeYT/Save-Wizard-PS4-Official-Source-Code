// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.Design.OverlayConverter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.ComponentModel;
using System.Globalization;

namespace BrightIdeasSoftware.Design
{
  internal class OverlayConverter : ExpandableObjectConverter
  {
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof (string) || base.CanConvertTo(context, destinationType);

    public override object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      if (destinationType == typeof (string))
      {
        switch (value)
        {
          case ImageOverlay imageOverlay2:
            return imageOverlay2.Image == null ? (object) "(none)" : (object) "(set)";
          case TextOverlay textOverlay2:
            return string.IsNullOrEmpty(textOverlay2.Text) ? (object) "(none)" : (object) "(set)";
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
