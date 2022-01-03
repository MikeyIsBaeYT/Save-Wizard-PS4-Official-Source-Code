// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.CustomScrollbar.Resource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PS3SaveEditor.CustomScrollbar
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resource
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resource()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Resource.resourceMan == null)
          Resource.resourceMan = new ResourceManager("PS3SaveEditor.CustomScrollbar.Resource", typeof (Resource).Assembly);
        return Resource.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Resource.resourceCulture;
      set => Resource.resourceCulture = value;
    }

    internal static Bitmap downarrow => (Bitmap) Resource.ResourceManager.GetObject(nameof (downarrow), Resource.resourceCulture);

    internal static Bitmap leftarrow => (Bitmap) Resource.ResourceManager.GetObject(nameof (leftarrow), Resource.resourceCulture);

    internal static Bitmap rightarrow => (Bitmap) Resource.ResourceManager.GetObject(nameof (rightarrow), Resource.resourceCulture);

    internal static Bitmap ThumbBottom => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbBottom), Resource.resourceCulture);

    internal static Bitmap ThumbLeft => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbLeft), Resource.resourceCulture);

    internal static Bitmap ThumbMiddle => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbMiddle), Resource.resourceCulture);

    internal static Bitmap ThumbMiddleH => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbMiddleH), Resource.resourceCulture);

    internal static Bitmap ThumbRight => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbRight), Resource.resourceCulture);

    internal static Bitmap ThumbSpanBottom => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbSpanBottom), Resource.resourceCulture);

    internal static Bitmap ThumbSpanBottom1 => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbSpanBottom1), Resource.resourceCulture);

    internal static Bitmap ThumbSpanLeft => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbSpanLeft), Resource.resourceCulture);

    internal static Bitmap ThumbSpanRight => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbSpanRight), Resource.resourceCulture);

    internal static Bitmap ThumbSpanTop => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbSpanTop), Resource.resourceCulture);

    internal static Bitmap ThumbTop => (Bitmap) Resource.ResourceManager.GetObject(nameof (ThumbTop), Resource.resourceCulture);

    internal static Bitmap uparrow => (Bitmap) Resource.ResourceManager.GetObject(nameof (uparrow), Resource.resourceCulture);
  }
}
