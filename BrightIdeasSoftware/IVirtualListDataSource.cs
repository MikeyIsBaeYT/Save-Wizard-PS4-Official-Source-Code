// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.IVirtualListDataSource
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public interface IVirtualListDataSource
  {
    object GetNthObject(int n);

    int GetObjectCount();

    int GetObjectIndex(object model);

    void PrepareCache(int first, int last);

    int SearchText(string value, int first, int last, OLVColumn column);

    void Sort(OLVColumn column, SortOrder order);

    void AddObjects(ICollection modelObjects);

    void RemoveObjects(ICollection modelObjects);

    void SetObjects(IEnumerable collection);

    void UpdateObject(int index, object modelObject);
  }
}
