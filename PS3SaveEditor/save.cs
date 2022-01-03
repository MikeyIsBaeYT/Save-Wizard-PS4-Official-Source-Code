// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.save
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace PS3SaveEditor
{
  public class save
  {
    public string id { get; set; }

    public string gamecode { get; set; }

    public string title { get; set; }

    public string description { get; set; }

    public string note { get; set; }

    public string folder { get; set; }

    public string region { get; set; }

    public long updated { get; set; }

    internal static save Copy(save save) => new save()
    {
      folder = save.folder,
      region = save.region,
      updated = save.updated,
      description = save.description,
      gamecode = save.gamecode,
      note = save.note,
      title = save.title,
      id = save.id
    };
  }
}
