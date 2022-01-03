// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.FlagClusteringStrategy
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace BrightIdeasSoftware
{
  public class FlagClusteringStrategy : ClusteringStrategy
  {
    private long[] values;
    private string[] labels;

    public FlagClusteringStrategy(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException(nameof (enumType));
      if (!enumType.IsEnum)
        throw new ArgumentException("Type must be enum", nameof (enumType));
      if (enumType.GetCustomAttributes(typeof (FlagsAttribute), false) == null)
        throw new ArgumentException("Type must have [Flags] attribute", nameof (enumType));
      List<long> longList = new List<long>();
      foreach (object obj in Enum.GetValues(enumType))
        longList.Add(Convert.ToInt64(obj));
      List<string> stringList = new List<string>();
      foreach (string name in Enum.GetNames(enumType))
        stringList.Add(name);
      this.SetValues(longList.ToArray(), stringList.ToArray());
    }

    public FlagClusteringStrategy(long[] values, string[] labels) => this.SetValues(values, labels);

    public long[] Values
    {
      get => this.values;
      private set => this.values = value;
    }

    public string[] Labels
    {
      get => this.labels;
      private set => this.labels = value;
    }

    private void SetValues(long[] flags, string[] flagLabels)
    {
      if (flags == null || flags.Length == 0)
        throw new ArgumentNullException(nameof (flags));
      if (flagLabels == null || flagLabels.Length == 0)
        throw new ArgumentNullException(nameof (flagLabels));
      if (flags.Length != flagLabels.Length)
        throw new ArgumentException("values and labels must have the same number of entries", nameof (flags));
      this.Values = flags;
      this.Labels = flagLabels;
    }

    public override object GetClusterKey(object model)
    {
      List<long> longList = new List<long>();
      try
      {
        long int64 = Convert.ToInt64(this.Column.GetValue(model));
        foreach (long num in this.Values)
        {
          if ((num & int64) == num)
            longList.Add(num);
        }
        return (object) longList;
      }
      catch (InvalidCastException ex)
      {
        return (object) longList;
      }
      catch (FormatException ex)
      {
        return (object) longList;
      }
    }

    public override string GetClusterDisplayLabel(ICluster cluster)
    {
      long int64 = Convert.ToInt64(cluster.ClusterKey);
      for (int index = 0; index < this.Values.Length; ++index)
      {
        if (int64 == this.Values[index])
          return this.ApplyDisplayFormat(cluster, this.Labels[index]);
      }
      return this.ApplyDisplayFormat(cluster, int64.ToString((IFormatProvider) CultureInfo.CurrentUICulture));
    }

    public override IModelFilter CreateFilter(IList valuesChosenForFiltering) => (IModelFilter) new FlagBitSetFilter(new AspectGetterDelegate(((ClusteringStrategy) this).GetClusterKey), (ICollection) valuesChosenForFiltering);
  }
}
