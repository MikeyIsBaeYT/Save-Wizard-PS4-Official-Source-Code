// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.IVirtualGroups
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public interface IVirtualGroups
  {
    IList<OLVGroup> GetGroups(GroupingParameters parameters);

    int GetGroupMember(OLVGroup group, int indexWithinGroup);

    int GetGroup(int itemIndex);

    int GetIndexWithinGroup(OLVGroup group, int itemIndex);

    void CacheHint(int fromGroupIndex, int fromIndex, int toGroupIndex, int toIndex);
  }
}
