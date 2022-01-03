// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.MungerException
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class MungerException : ApplicationException
  {
    private readonly SimpleMunger munger;
    private readonly object target;

    public MungerException(SimpleMunger munger, object target, Exception ex)
      : base("Munger failed", ex)
    {
      this.munger = munger;
      this.target = target;
    }

    public SimpleMunger Munger => this.munger;

    public object Target => this.target;
  }
}
