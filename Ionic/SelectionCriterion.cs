// Decompiled with JetBrains decompiler
// Type: Ionic.SelectionCriterion
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System.Diagnostics;

namespace Ionic
{
  internal abstract class SelectionCriterion
  {
    internal virtual bool Verbose { get; set; }

    internal abstract bool Evaluate(string filename);

    [Conditional("SelectorTrace")]
    protected static void CriterionTrace(string format, params object[] args)
    {
    }

    internal abstract bool Evaluate(ZipEntry entry);
  }
}
