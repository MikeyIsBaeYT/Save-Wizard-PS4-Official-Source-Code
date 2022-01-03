// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.group
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PS3SaveEditor
{
  public class group
  {
    public string name { get; set; }

    public string note { get; set; }

    public string options { get; set; }

    public string type { get; set; }

    [XmlElement("cheat")]
    public List<cheat> cheats { get; set; }

    [XmlElement(ElementName = "group")]
    public List<group> _group { get; set; }

    public int TotalCheats
    {
      get
      {
        int num = 0;
        if (this.cheats != null)
          num = this.cheats.Count;
        if (this._group != null)
        {
          foreach (group group in this._group)
            num += group.TotalCheats;
        }
        return num;
      }
    }

    public List<cheat> GetAllCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      if (this._group != null)
      {
        foreach (group group in this._group)
          cheatList.AddRange((IEnumerable<cheat>) group.cheats);
      }
      cheatList.AddRange((IEnumerable<cheat>) this.cheats);
      return cheatList;
    }

    public group() => this.cheats = new List<cheat>();

    internal static group Copy(group g)
    {
      group group = new group();
      group.name = g.name;
      group.note = g.note;
      group.options = g.options;
      group.type = g.type;
      if (g._group != null)
      {
        group._group = new List<group>();
        foreach (group g1 in g._group)
          group._group.Add(group.Copy(g1));
      }
      foreach (cheat cheat in g.cheats)
        group.cheats.Add(cheat.Copy(cheat));
      return group;
    }

    public cheat GetCheat(string cd)
    {
      foreach (cheat cheat in this.cheats)
      {
        if (cd == cheat.name)
          return cheat;
      }
      if (this._group != null)
      {
        foreach (group group in this._group)
        {
          cheat cheat = group.GetCheat(cd);
          if (cheat != null)
            return cheat;
        }
      }
      return (cheat) null;
    }

    internal int GetCheatsCount()
    {
      int count = this.cheats.Count;
      if (this._group != null)
      {
        using (List<group>.Enumerator enumerator = this._group.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            group current = enumerator.Current;
            return count + current.GetCheatsCount();
          }
        }
      }
      return count;
    }

    internal List<cheat> GetCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      cheatList.AddRange((IEnumerable<cheat>) cheatList);
      if (this._group != null)
      {
        foreach (group group in this._group)
          cheatList.AddRange((IEnumerable<cheat>) group.GetCheats());
      }
      return cheatList;
    }

    public bool CheatsSelected
    {
      get
      {
        foreach (cheat cheat in this.cheats)
        {
          if (cheat.Selected)
            return true;
        }
        if (this._group != null)
        {
          foreach (group group in this._group)
          {
            if (group.CheatsSelected)
              return true;
          }
        }
        return false;
      }
    }

    public string SelectedCheats
    {
      get
      {
        string str = "";
        foreach (cheat cheat in this.cheats)
        {
          if (cheat.Selected)
            str = str + "<id>" + cheat.id + "</id>";
        }
        if (this._group != null)
        {
          foreach (group group in this._group)
            str += group.SelectedCheats;
        }
        return str;
      }
    }

    internal List<cheat> GetGroupCheats()
    {
      List<cheat> cheatList = new List<cheat>();
      cheatList.AddRange((IEnumerable<cheat>) this.cheats);
      if (this._group != null)
      {
        foreach (group group in this._group)
          cheatList.AddRange((IEnumerable<cheat>) group.GetGroupCheats());
      }
      return cheatList;
    }
  }
}
