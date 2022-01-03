// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.file
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  [XmlRoot("file")]
  public class file
  {
    public internals internals;
    public string type;
    public string textmode;

    public string filename { get; set; }

    [XmlIgnore]
    public string original_filename { get; set; }

    public string id { get; set; }

    public string title { get; set; }

    public string dependency { get; set; }

    public string Option { get; set; }

    public string altname { get; set; }

    public string ucfilename { get; set; }

    public cheats cheats { get; set; }

    [XmlIgnore]
    public List<cheat> Cheats
    {
      get => this.cheats._cheats;
      set => this.cheats._cheats = value;
    }

    public List<cheat> GetAllCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      cheatList.AddRange((IEnumerable<cheat>) this.Cheats);
      foreach (group group in this.groups)
        cheatList.AddRange((IEnumerable<cheat>) group.GetGroupCheats());
      return cheatList;
    }

    [XmlIgnore]
    public List<group> groups
    {
      get => this.cheats.groups;
      set => this.cheats.groups = value;
    }

    public int TotalCheats => this.cheats.TotalCheats;

    public string VisibleFileName
    {
      get
      {
        string str = (string) null;
        if (!string.IsNullOrEmpty(this.altname))
          str = this.altname;
        if (!string.IsNullOrEmpty(this.altname) && this.original_filename != null && this.filename != this.original_filename && Util.IsMatch(this.filename, this.original_filename))
        {
          Match match = Regex.Match(this.filename, this.original_filename);
          if (match.Groups != null)
          {
            if (match.Groups.Count > 1)
              str = str.Replace("${1}", match.Groups[1].Value);
            if (match.Groups.Count > 2)
              str = str.Replace("${2}", match.Groups[2].Value);
          }
        }
        return !string.IsNullOrEmpty(str) ? string.Format("{0} ({1})", (object) str, (object) this.filename) : this.filename;
      }
    }

    public int TextMode
    {
      get
      {
        switch (this.textmode)
        {
          case "":
            return 0;
          case "utf-8":
            return 1;
          case "ascii":
            return 2;
          case "utf-16":
            return 3;
          case null:
            return 0;
          default:
            return 0;
        }
      }
    }

    public bool IsHidden => this.type == "hidden";

    public file() => this.cheats = new cheats();

    public string GetDependencyFile(container gameFolder, string folder)
    {
      if (string.IsNullOrEmpty(this.dependency))
        return (string) null;
      foreach (file file1 in gameFolder.files._files)
      {
        if (file1.id == this.dependency)
        {
          string path2 = file1.GetSaveFile(folder);
          if (path2 == null)
          {
            foreach (file file2 in gameFolder.files._files)
            {
              if (file2.id == file1.dependency)
                path2 = file2.filename;
            }
          }
          if (path2 != null)
            return Path.Combine(folder, path2);
        }
      }
      return (string) null;
    }

    internal static file Copy(file gameFile)
    {
      file file1 = new file();
      file1.original_filename = gameFile.original_filename;
      file1.filename = gameFile.filename;
      file1.dependency = gameFile.dependency;
      file1.title = gameFile.title;
      file1.id = gameFile.id;
      file1.Option = gameFile.Option;
      file1.altname = gameFile.altname;
      if (gameFile.internals != null)
      {
        file1.internals = new internals();
        foreach (file file2 in gameFile.internals.files)
          file1.internals.files.Add(file.Copy(file2));
      }
      file1.cheats = new cheats();
      foreach (group group in gameFile.groups)
        file1.groups.Add(group.Copy(group));
      file1.textmode = gameFile.textmode;
      file1.type = gameFile.type;
      file1.ucfilename = gameFile.ucfilename;
      foreach (cheat cheat in gameFile.Cheats)
        file1.Cheats.Add(cheat.Copy(cheat));
      return file1;
    }

    internal string GetSaveFile(string saveFolder)
    {
      string[] files = Directory.GetFiles(saveFolder, this.filename);
      return (uint) files.Length > 0U ? Path.GetFileName(files[0]) : (string) null;
    }

    internal List<string> GetSaveFiles(string saveFolder)
    {
      string[] files = Directory.GetFiles(saveFolder, this.filename);
      if ((uint) files.Length <= 0U)
        return (List<string>) null;
      List<string> stringList = new List<string>((IEnumerable<string>) files);
      stringList.Sort();
      return stringList;
    }

    internal static file GetGameFile(container gameFolder, string folder, string file)
    {
      foreach (file file1 in gameFolder.files._files)
      {
        if (file1.filename == file || Util.IsMatch(file, file1.filename))
          return file1;
      }
      return (file) null;
    }

    internal cheat GetCheat(string cd)
    {
      foreach (cheat cheat in this.Cheats)
      {
        if (cd == cheat.name)
          return cheat;
      }
      foreach (group group in this.groups)
      {
        cheat cheat = group.GetCheat(cd);
        if (cheat != null)
          return cheat;
      }
      return (cheat) null;
    }

    public file GetParent(container gamefolder)
    {
      foreach (file file1 in gamefolder.files._files)
      {
        if (file1.id == this.id)
          return (file) null;
        if (file1.internals != null)
        {
          foreach (file file2 in file1.internals.files)
          {
            if (file2.id == this.id)
              return file1;
          }
        }
      }
      return (file) null;
    }
  }
}
