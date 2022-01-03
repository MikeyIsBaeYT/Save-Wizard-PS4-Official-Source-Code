// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.container
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace PS3SaveEditor
{
  public class container
  {
    public string key { get; set; }

    public string pfs { get; set; }

    public string name { get; set; }

    public int preprocess { get; set; }

    public files files { get; set; }

    public int? quickmode { get; set; }

    public int? locked { get; set; }

    public container() => this.files = new files();

    public List<cheat> GetAllCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      foreach (file file in this.files._files)
        cheatList.AddRange((IEnumerable<cheat>) file.GetAllCheats());
      return cheatList;
    }

    internal static container Copy(container folder)
    {
      container container = new container();
      container.key = folder.key;
      container.pfs = folder.pfs;
      container.name = folder.name;
      container.preprocess = folder.preprocess;
      container.files = new files();
      foreach (file file in folder.files._files)
        container.files._files.Add(file.Copy(file));
      container.quickmode = folder.quickmode;
      container.locked = folder.locked;
      return container;
    }

    internal int GetCheatsCount()
    {
      int num = 0;
      foreach (file file in this.files._files)
        num += file.TotalCheats;
      return num;
    }

    internal file GetSaveFile(string fileName)
    {
      foreach (file file in this.files._files)
      {
        if (file.filename == fileName || Util.IsMatch(fileName, file.filename))
          return file;
      }
      return (file) null;
    }
  }
}
