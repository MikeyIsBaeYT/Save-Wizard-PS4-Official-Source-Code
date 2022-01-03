// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AbstractVirtualGroups
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;

namespace BrightIdeasSoftware
{
  public class AbstractVirtualGroups : IVirtualGroups
  {
    public virtual IList<OLVGroup> GetGroups(GroupingParameters parameters) => (IList<OLVGroup>) new List<OLVGroup>();

    public virtual int GetGroupMember(OLVGroup group, int indexWithinGroup) => -1;

    public virtual int GetGroup(int itemIndex) => -1;

    public virtual int GetIndexWithinGroup(OLVGroup group, int itemIndex) => -1;

    public virtual void CacheHint(
      int fromGroupIndex,
      int fromIndex,
      int toGroupIndex,
      int toIndex)
    {
    }
  }
}
