// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FlagBitSetFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class FlagBitSetFilter : OneOfFilter
  {
    private List<ulong> possibleValuesAsUlongs = new List<ulong>();

    public FlagBitSetFilter(AspectGetterDelegate valueGetter, ICollection possibleValues)
      : base(valueGetter, possibleValues)
    {
      this.ConvertPossibleValues();
    }

    public override IList PossibleValues
    {
      get => base.PossibleValues;
      set
      {
        base.PossibleValues = value;
        this.ConvertPossibleValues();
      }
    }

    private void ConvertPossibleValues()
    {
      this.possibleValuesAsUlongs = new List<ulong>();
      foreach (object possibleValue in (IEnumerable) this.PossibleValues)
        this.possibleValuesAsUlongs.Add(Convert.ToUInt64(possibleValue));
    }

    protected override bool DoesValueMatch(object result)
    {
      try
      {
        ulong uint64 = Convert.ToUInt64(result);
        foreach (ulong possibleValuesAsUlong in this.possibleValuesAsUlongs)
        {
          if (((long) uint64 & (long) possibleValuesAsUlong) == (long) possibleValuesAsUlong)
            return true;
        }
        return false;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
      catch (FormatException ex)
      {
        return false;
      }
    }
  }
}
