// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.DateTimeClusteringStrategy
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Globalization;

namespace BrightIdeasSoftware
{
  public class DateTimeClusteringStrategy : ClusteringStrategy
  {
    private string format;
    private DateTimePortion portions = DateTimePortion.Year | DateTimePortion.Month;

    public DateTimeClusteringStrategy()
      : this(DateTimePortion.Year | DateTimePortion.Month, "MMMM yyyy")
    {
    }

    public DateTimeClusteringStrategy(DateTimePortion portions, string format)
    {
      this.Portions = portions;
      this.Format = format;
    }

    public string Format
    {
      get => this.format;
      set => this.format = value;
    }

    public DateTimePortion Portions
    {
      get => this.portions;
      set => this.portions = value;
    }

    public override object GetClusterKey(object model)
    {
      DateTime? nullable = this.Column.GetValue(model) as DateTime?;
      return !nullable.HasValue ? (object) null : (object) new DateTime((this.Portions & DateTimePortion.Year) == DateTimePortion.Year ? nullable.Value.Year : 1, (this.Portions & DateTimePortion.Month) == DateTimePortion.Month ? nullable.Value.Month : 1, (this.Portions & DateTimePortion.Day) == DateTimePortion.Day ? nullable.Value.Day : 1, (this.Portions & DateTimePortion.Hour) == DateTimePortion.Hour ? nullable.Value.Hour : 0, (this.Portions & DateTimePortion.Minute) == DateTimePortion.Minute ? nullable.Value.Minute : 0, (this.Portions & DateTimePortion.Second) == DateTimePortion.Second ? nullable.Value.Second : 0);
    }

    public override string GetClusterDisplayLabel(ICluster cluster)
    {
      DateTime? clusterKey = cluster.ClusterKey as DateTime?;
      return this.ApplyDisplayFormat(cluster, clusterKey.HasValue ? this.DateToString(clusterKey.Value) : ClusteringStrategy.NULL_LABEL);
    }

    protected virtual string DateToString(DateTime dateTime)
    {
      if (string.IsNullOrEmpty(this.Format))
        return dateTime.ToString((IFormatProvider) CultureInfo.CurrentUICulture);
      try
      {
        return dateTime.ToString(this.Format);
      }
      catch (FormatException ex)
      {
        return string.Format("Bad format string '{0}' for value '{1}'", (object) this.Format, (object) dateTime);
      }
    }
  }
}
