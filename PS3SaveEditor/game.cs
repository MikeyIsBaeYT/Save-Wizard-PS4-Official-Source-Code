// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.game
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  [XmlRoot("game", Namespace = "")]
  public class game
  {
    public bool LocalCheatExists;

    public string id { get; set; }

    public int acts { get; set; }

    public string notes { get; set; }

    public string diskcode { get; set; }

    public string aliasid { get; set; }

    public string name { get; set; }

    public string version { get; set; }

    public aliases aliases { get; set; }

    public containers containers { get; set; }

    public int region { get; set; }

    public string Client { get; set; }

    public long updated { get; set; }

    public string LocalSaveFolder { get; set; }

    [XmlIgnore]
    public ZipEntry PFSZipEntry { get; set; }

    [XmlIgnore]
    public ZipEntry BinZipEntry { get; set; }

    [XmlIgnore]
    public ZipFile ZipFile { get; set; }

    [XmlIgnore]
    public DateTime UpdatedTime { get; set; }

    public string PSN_ID
    {
      get
      {
        try
        {
          if (this.LocalSaveFolder != null)
            return Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(this.LocalSaveFolder)));
        }
        catch
        {
        }
        return (string) null;
      }
    }

    public game() => this.containers = new containers();

    public override string ToString() => this.ToString(false, this.GetSaveFiles());

    public int GetCheatsCount()
    {
      int num = 0;
      foreach (container container in this.containers._containers)
        num += container.GetCheatsCount();
      return num;
    }

    public string ToString(List<string> selectedSaveFiles, string mode = "decrypt")
    {
      container targetGameFolder = this.GetTargetGameFolder();
      List<string> containerFiles = this.GetContainerFiles();
      string str1 = string.Format("<game id=\"{0}\" mode=\"{1}\"><key><name>{2}</name></key><pfs><name>{3}</name></pfs><files>", (object) this.id, (object) mode, (object) Path.GetFileName(containerFiles[0]), (object) Path.GetFileName(containerFiles[1]));
      List<string> stringList = new List<string>();
      foreach (string selectedSaveFile in selectedSaveFiles)
      {
        stringList.Add(Path.GetFileName(selectedSaveFile));
        str1 = !(mode == "encrypt") ? str1 + "<file><name>" + Path.GetFileName(selectedSaveFile) + "</name></file>" : str1 + "<file><name>" + Path.GetFileName(selectedSaveFile).Replace("_file_", "") + "</name></file>";
      }
      if (targetGameFolder == null)
        ;
      string str2;
      return str2 = str1 + "</files></game>";
    }

    public string ToString(bool bSelectedCheatFilesOnly, List<string> lstSaveFiles)
    {
      container targetGameFolder = this.GetTargetGameFolder();
      List<string> containerFiles = this.GetContainerFiles();
      string str1 = string.Format("<game id=\"{0}\" mode=\"{1}\"><key><name>{2}</name></key><pfs><name>{3}</name></pfs><files>", (object) this.id, (object) "patch", (object) Path.GetFileName(containerFiles[0]), (object) Path.GetFileName(containerFiles[1]));
      this.GetSaveFiles();
      if (targetGameFolder != null)
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string lstSaveFile in lstSaveFiles)
        {
          file gameFile = file.GetGameFile(targetGameFolder, this.LocalSaveFolder, lstSaveFile);
          if (gameFile != null)
          {
            bool flag = false;
            if (!bSelectedCheatFilesOnly)
            {
              flag = true;
            }
            else
            {
              for (int index = 0; index < gameFile.Cheats.Count; ++index)
              {
                if (gameFile.Cheats[index].Selected)
                  flag = true;
              }
              if (gameFile.groups != null)
              {
                foreach (group group in gameFile.groups)
                {
                  if (group.CheatsSelected)
                    flag = true;
                }
              }
            }
            if (flag)
            {
              string str2 = lstSaveFile;
              if (dictionary.ContainsKey(str2))
              {
                str1 = str1.Replace("<file><fileid>" + gameFile.id + "</fileid><name>" + str2 + "</name></file>", "");
                dictionary.Remove(str2);
              }
              if (!dictionary.ContainsKey(str2) && gameFile.GetParent(targetGameFolder) == null)
              {
                str1 += "<file>";
                str1 = str1 + "<name>" + str2 + "</name>";
                dictionary.Add(str2, gameFile.id);
                if (gameFile.GetAllCheats().Count > 0)
                {
                  str1 += "<cheats>";
                  foreach (cheat cheat1 in gameFile.Cheats)
                  {
                    if (cheat1.Selected)
                    {
                      string str3 = str1;
                      cheat cheat2 = cheat1;
                      int? quickmode = targetGameFolder.quickmode;
                      int num1 = 0;
                      int num2 = quickmode.GetValueOrDefault() > num1 ? (quickmode.HasValue ? 1 : 0) : 0;
                      string str4 = cheat2.ToString(num2 != 0);
                      str1 = str3 + str4;
                    }
                  }
                  if (gameFile.groups != null)
                  {
                    foreach (group group in gameFile.groups)
                      str1 += group.SelectedCheats;
                  }
                  str1 += "</cheats>";
                }
                str1 += "</file>";
              }
              if (gameFile.GetParent(targetGameFolder) != null)
              {
                file parent = gameFile.GetParent(targetGameFolder);
                if (parent.internals != null)
                {
                  foreach (file file in parent.internals.files)
                  {
                    if (!dictionary.ContainsValue(file.id))
                    {
                      if (lstSaveFile.IndexOf(file.filename) > 0)
                      {
                        str1 += "<file>";
                        str1 = str1 + "<fileid>" + gameFile.id + "</fileid>";
                        str1 = str1 + "<name>" + Path.GetFileName(str2) + "</name>";
                        dictionary.Add(Path.GetFileName(str2), gameFile.id);
                        if (gameFile.Cheats.Count > 0)
                        {
                          str1 += "<cheats>";
                          foreach (cheat cheat3 in gameFile.Cheats)
                          {
                            string str5 = str1;
                            cheat cheat4 = cheat3;
                            int? quickmode = targetGameFolder.quickmode;
                            int num3 = 0;
                            int num4 = quickmode.GetValueOrDefault() > num3 ? (quickmode.HasValue ? 1 : 0) : 0;
                            string str6 = cheat4.ToString(num4 != 0);
                            str1 = str5 + str6;
                          }
                          str1 += "</cheats>";
                        }
                        str1 += "</file>";
                      }
                      else
                      {
                        string path = Path.Combine(this.LocalSaveFolder, file.filename);
                        str1 = str1 + "<file><fileid>" + file.id + "</fileid>";
                        str1 = str1 + "<name>" + file.filename + "</name></file>";
                        dictionary.Add(Path.GetFileName(path), file.id);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      string str7;
      return str7 = str1.Replace("<cheats></cheats>", "") + "</files></game>";
    }

    public List<string> GetContainerFiles()
    {
      if (!Directory.Exists(Path.GetDirectoryName(this.LocalSaveFolder)))
        return (List<string>) null;
      List<string> stringList = new List<string>();
      this.GetTargetGameFolder();
      stringList.Add(this.LocalSaveFolder);
      stringList.Add(this.LocalSaveFolder.Substring(0, this.LocalSaveFolder.Length - 4));
      return stringList;
    }

    public container GetTargetGameFolder()
    {
      container container1 = (container) null;
      if (!Directory.Exists(Path.GetDirectoryName(this.LocalSaveFolder)))
        return (container) null;
      foreach (container container2 in this.containers._containers)
      {
        if ((Path.GetFileNameWithoutExtension(this.LocalSaveFolder) == container2.pfs || Util.IsMatch(Path.GetFileNameWithoutExtension(this.LocalSaveFolder), container2.pfs)) && File.Exists(this.LocalSaveFolder))
        {
          container1 = container2;
          break;
        }
      }
      return container1;
    }

    internal static game Copy(game gameItem)
    {
      game game = new game();
      game.id = gameItem.id;
      game.notes = gameItem.notes;
      game.name = gameItem.name;
      game.acts = gameItem.acts;
      game.diskcode = gameItem.diskcode;
      game.aliasid = gameItem.aliasid;
      game.updated = gameItem.updated;
      game.version = gameItem.version;
      game.region = gameItem.region;
      if (gameItem.aliases != null)
        game.aliases = aliases.Copy(gameItem.aliases);
      foreach (container container in gameItem.containers._containers)
        game.containers._containers.Add(container.Copy(container));
      game.Client = gameItem.Client;
      game.LocalCheatExists = gameItem.LocalCheatExists;
      game.LocalSaveFolder = gameItem.LocalSaveFolder;
      return game;
    }

    internal int GetCheatCount()
    {
      int num = 0;
      foreach (container container in this.containers._containers)
      {
        if (container != null)
        {
          foreach (file file1 in container.files._files)
          {
            num += file1.Cheats.Count;
            if (file1.internals != null)
            {
              foreach (file file2 in file1.internals.files)
                num += file2.TotalCheats;
            }
            foreach (group group in file1.groups)
              num += group.TotalCheats;
          }
        }
      }
      return num;
    }

    internal List<cheat> GetAllCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      foreach (container container in this.containers._containers)
      {
        foreach (file file1 in container.files._files)
        {
          cheatList.AddRange((IEnumerable<cheat>) file1.Cheats);
          if (file1.internals != null)
          {
            foreach (file file2 in file1.internals.files)
              cheatList.AddRange((IEnumerable<cheat>) file2.Cheats);
          }
          foreach (group group in file1.groups)
            cheatList.AddRange((IEnumerable<cheat>) group.GetAllCheats());
        }
      }
      return cheatList;
    }

    internal List<string> GetSaveFiles() => this.GetSaveFiles(false);

    internal List<string> GetSaveFiles(bool bOnlySelectedCheats)
    {
      List<string> stringList = new List<string>();
      container targetGameFolder = this.GetTargetGameFolder();
      bool flag = false;
      if (targetGameFolder != null)
      {
        foreach (file file in targetGameFolder.files._files)
          stringList.Add(file.filename);
      }
      if (flag)
        stringList.Clear();
      return stringList;
    }

    internal List<cheat> GetCheats(string saveFolder, string savefile)
    {
      List<cheat> cheatList = new List<cheat>();
      foreach (container container in this.containers._containers)
      {
        string[] files1 = Directory.GetFiles(Path.GetDirectoryName(saveFolder));
        List<string> stringList1 = new List<string>();
        foreach (string path in files1)
        {
          if (Path.GetFileName(path) == container.pfs || Util.IsMatch(Path.GetFileName(path), container.pfs))
            stringList1.Add(path);
        }
        if (files1.Length != 0 && stringList1.IndexOf(saveFolder) >= 0)
        {
          foreach (file file in container.files._files)
          {
            string[] files2 = Directory.GetFiles(Util.GetTempFolder(), "*");
            List<string> stringList2 = new List<string>();
            foreach (string a in files2)
            {
              if (a == file.filename || Util.IsMatch(a, file.filename))
                stringList2.Add(a);
            }
            foreach (string path in stringList2.ToArray())
            {
              if (savefile == Path.GetFileName(path) && (file.filename == Path.GetFileName(path) || Util.IsMatch(Path.GetFileName(savefile), file.filename)))
              {
                cheatList.AddRange((IEnumerable<cheat>) file.Cheats);
                foreach (group group in file.groups)
                {
                  List<cheat> cheats = group.GetCheats();
                  if (cheats != null)
                    cheatList.AddRange((IEnumerable<cheat>) cheats);
                }
                return cheatList;
              }
            }
          }
        }
      }
      return (List<cheat>) null;
    }

    internal file GetGameFile(container folder, string savefile)
    {
      if (savefile == null)
        return folder.files._files[0];
      foreach (file file in folder.files._files)
      {
        if (savefile == file.filename || Util.IsMatch(savefile, file.filename))
          return file;
      }
      foreach (file file1 in folder.files._files)
      {
        foreach (string file2 in Directory.GetFiles(Util.GetTempFolder(), "*"))
        {
          if (Path.GetFileName(file2) == file1.filename || Util.IsMatch(Path.GetFileName(file2), file1.filename))
            return file1;
        }
      }
      return (file) null;
    }

    internal bool IsAlias(string gameCode, out string saveId)
    {
      if (this.aliases != null)
      {
        foreach (alias alias in this.aliases._aliases)
        {
          if (gameCode.IndexOf(alias.id) >= 0)
          {
            saveId = alias.id;
            return true;
          }
        }
      }
      saveId = (string) null;
      return false;
    }

    internal bool IsSupported(Dictionary<string, List<game>> m_dictLocalSaves, out string saveID)
    {
      if (this.aliases != null)
      {
        foreach (alias alias in this.aliases._aliases)
        {
          if (m_dictLocalSaves.ContainsKey(alias.id))
          {
            saveID = alias.id;
            return true;
          }
        }
      }
      saveID = (string) null;
      return false;
    }

    internal List<alias> GetAllAliases(bool bAsc = true, bool distinct = false)
    {
      List<alias> aliasList = new List<alias>();
      aliasList.Add(new alias()
      {
        id = this.id,
        name = this.name,
        region = this.region,
        acts = this.acts,
        diskcode = this.diskcode
      });
      List<string> stringList = new List<string>();
      stringList.Add(this.name);
      if (this.aliases != null && this.aliases._aliases != null && this.aliases._aliases.Count > 0)
      {
        if (!distinct)
        {
          aliasList.AddRange((IEnumerable<alias>) this.aliases._aliases);
        }
        else
        {
          foreach (alias alias in this.aliases._aliases)
          {
            if (stringList.IndexOf(alias.name) < 0)
            {
              aliasList.Add(alias);
              stringList.Add(alias.name);
            }
          }
        }
      }
      aliasList.Sort((Comparison<alias>) ((a1, a2) => a1.id.CompareTo(a2.id)));
      if (!bAsc)
        aliasList.Reverse();
      return aliasList;
    }

    internal cheat GetCheat(string id, string title)
    {
      foreach (container container in this.containers._containers)
      {
        foreach (file file in container.files._files)
        {
          foreach (cheat allCheat in file.GetAllCheats())
          {
            if (allCheat.id == id && allCheat.name == title)
              return allCheat;
          }
        }
      }
      return (cheat) null;
    }
  }
}
