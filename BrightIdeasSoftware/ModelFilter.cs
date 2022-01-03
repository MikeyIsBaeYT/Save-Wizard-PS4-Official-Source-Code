// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ModelFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace BrightIdeasSoftware
{
  public class ModelFilter : IModelFilter
  {
    private System.Predicate<object> predicate;

    public ModelFilter(System.Predicate<object> predicate) => this.Predicate = predicate;

    protected System.Predicate<object> Predicate
    {
      get => this.predicate;
      set => this.predicate = value;
    }

    public virtual bool Filter(object modelObject) => this.Predicate == null || this.Predicate(modelObject);
  }
}
