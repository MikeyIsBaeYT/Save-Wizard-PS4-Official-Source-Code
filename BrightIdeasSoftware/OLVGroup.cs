// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVGroup
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVGroup
  {
    private static int nextId;
    private string bottomDescription;
    private IList contents;
    private object extendedImage;
    private string footer;
    private static PropertyInfo groupIdPropInfo;
    private string header;
    private HorizontalAlignment headerAlignment;
    private int id;
    private IList<OLVListItem> items = (IList<OLVListItem>) new List<OLVListItem>();
    private object key;
    private ObjectListView listView;
    private string name;
    private string subsetTitle;
    private string subtitle;
    private IComparable sortValue;
    private GroupState state;
    private GroupState stateMask;
    private object tag;
    private string task;
    private object titleImage;
    private string topDescription;
    private int virtualItemCount;
    private ListViewGroup listViewGroup;

    public OLVGroup()
      : this("Default group header")
    {
    }

    public OLVGroup(string header)
    {
      this.Header = header;
      this.Id = OLVGroup.nextId++;
      this.TitleImage = (object) -1;
      this.ExtendedImage = (object) -1;
    }

    public string BottomDescription
    {
      get => this.bottomDescription;
      set => this.bottomDescription = value;
    }

    public bool Collapsed
    {
      get => this.GetOneState(GroupState.LVGS_COLLAPSED);
      set => this.SetOneState(value, GroupState.LVGS_COLLAPSED);
    }

    public bool Collapsible
    {
      get => this.GetOneState(GroupState.LVGS_COLLAPSIBLE);
      set => this.SetOneState(value, GroupState.LVGS_COLLAPSIBLE);
    }

    public IList Contents
    {
      get => this.contents;
      set => this.contents = value;
    }

    public bool Created => this.ListView != null;

    public object ExtendedImage
    {
      get => this.extendedImage;
      set => this.extendedImage = value;
    }

    public string Footer
    {
      get => this.footer;
      set => this.footer = value;
    }

    public int GroupId
    {
      get
      {
        if (this.ListViewGroup == null)
          return this.Id;
        if (OLVGroup.groupIdPropInfo == (PropertyInfo) null)
          OLVGroup.groupIdPropInfo = typeof (ListViewGroup).GetProperty("ID", BindingFlags.Instance | BindingFlags.NonPublic);
        int? nullable = OLVGroup.groupIdPropInfo.GetValue((object) this.ListViewGroup, (object[]) null) as int?;
        return nullable.HasValue ? nullable.Value : -1;
      }
    }

    public string Header
    {
      get => this.header;
      set => this.header = value;
    }

    public HorizontalAlignment HeaderAlignment
    {
      get => this.headerAlignment;
      set => this.headerAlignment = value;
    }

    public int Id
    {
      get => this.id;
      set => this.id = value;
    }

    public IList<OLVListItem> Items
    {
      get => this.items;
      set => this.items = value;
    }

    public object Key
    {
      get => this.key;
      set => this.key = value;
    }

    public ObjectListView ListView
    {
      get => this.listView;
      protected set => this.listView = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public bool Focused
    {
      get => this.GetOneState(GroupState.LVGS_FOCUSED);
      set => this.SetOneState(value, GroupState.LVGS_FOCUSED);
    }

    public bool Selected
    {
      get => this.GetOneState(GroupState.LVGS_SELECTED);
      set => this.SetOneState(value, GroupState.LVGS_SELECTED);
    }

    public string SubsetTitle
    {
      get => this.subsetTitle;
      set => this.subsetTitle = value;
    }

    public string Subtitle
    {
      get => this.subtitle;
      set => this.subtitle = value;
    }

    public IComparable SortValue
    {
      get => this.sortValue;
      set => this.sortValue = value;
    }

    public GroupState State
    {
      get => this.state;
      set => this.state = value;
    }

    public GroupState StateMask
    {
      get => this.stateMask;
      set => this.stateMask = value;
    }

    public bool Subseted
    {
      get => this.GetOneState(GroupState.LVGS_SUBSETED);
      set => this.SetOneState(value, GroupState.LVGS_SUBSETED);
    }

    public object Tag
    {
      get => this.tag;
      set => this.tag = value;
    }

    public string Task
    {
      get => this.task;
      set => this.task = value;
    }

    public object TitleImage
    {
      get => this.titleImage;
      set => this.titleImage = value;
    }

    public string TopDescription
    {
      get => this.topDescription;
      set => this.topDescription = value;
    }

    public int VirtualItemCount
    {
      get => this.virtualItemCount;
      set => this.virtualItemCount = value;
    }

    protected ListViewGroup ListViewGroup
    {
      get => this.listViewGroup;
      set => this.listViewGroup = value;
    }

    public int GetImageIndex(object imageSelector)
    {
      if (imageSelector == null || this.ListView == null || this.ListView.GroupImageList == null)
        return -1;
      switch (imageSelector)
      {
        case int num:
          return num;
        case string key:
          return this.ListView.GroupImageList.Images.IndexOfKey(key);
        default:
          return -1;
      }
    }

    public override string ToString() => this.Header;

    public void InsertGroupNewStyle(ObjectListView olv)
    {
      this.ListView = olv;
      NativeMethods.InsertGroup(olv, this.AsNativeGroup(true));
    }

    public void InsertGroupOldStyle(ObjectListView olv)
    {
      this.ListView = olv;
      if (this.ListViewGroup == null)
        this.ListViewGroup = new ListViewGroup();
      this.ListViewGroup.Header = this.Header;
      this.ListViewGroup.HeaderAlignment = this.HeaderAlignment;
      this.ListViewGroup.Name = this.Name;
      this.ListViewGroup.Tag = (object) this;
      olv.Groups.Add(this.ListViewGroup);
      NativeMethods.SetGroupInfo(olv, this.GroupId, this.AsNativeGroup(false));
    }

    public void SetItemsOldStyle()
    {
      if (!(this.Items is List<OLVListItem> items))
      {
        foreach (ListViewItem listViewItem in (IEnumerable<OLVListItem>) this.Items)
          this.ListViewGroup.Items.Add(listViewItem);
      }
      else
        this.ListViewGroup.Items.AddRange((ListViewItem[]) items.ToArray());
    }

    internal NativeMethods.LVGROUP2 AsNativeGroup(bool withId)
    {
      NativeMethods.LVGROUP2 lvgrouP2 = new NativeMethods.LVGROUP2();
      lvgrouP2.cbSize = (uint) Marshal.SizeOf(typeof (NativeMethods.LVGROUP2));
      lvgrouP2.mask = 13U;
      lvgrouP2.pszHeader = this.Header;
      lvgrouP2.uAlign = (uint) this.HeaderAlignment;
      lvgrouP2.stateMask = (uint) this.StateMask;
      lvgrouP2.state = (uint) this.State;
      if (withId)
      {
        lvgrouP2.iGroupId = this.GroupId;
        lvgrouP2.mask ^= 16U;
      }
      if (!string.IsNullOrEmpty(this.Footer))
      {
        lvgrouP2.pszFooter = this.Footer;
        lvgrouP2.mask ^= 2U;
      }
      if (!string.IsNullOrEmpty(this.Subtitle))
      {
        lvgrouP2.pszSubtitle = this.Subtitle;
        lvgrouP2.mask ^= 256U;
      }
      if (!string.IsNullOrEmpty(this.Task))
      {
        lvgrouP2.pszTask = this.Task;
        lvgrouP2.mask ^= 512U;
      }
      if (!string.IsNullOrEmpty(this.TopDescription))
      {
        lvgrouP2.pszDescriptionTop = this.TopDescription;
        lvgrouP2.mask ^= 1024U;
      }
      if (!string.IsNullOrEmpty(this.BottomDescription))
      {
        lvgrouP2.pszDescriptionBottom = this.BottomDescription;
        lvgrouP2.mask ^= 2048U;
      }
      int imageIndex1 = this.GetImageIndex(this.TitleImage);
      if (imageIndex1 >= 0)
      {
        lvgrouP2.iTitleImage = imageIndex1;
        lvgrouP2.mask ^= 4096U;
      }
      int imageIndex2 = this.GetImageIndex(this.ExtendedImage);
      if (imageIndex2 >= 0)
      {
        lvgrouP2.iExtendedImage = imageIndex2;
        lvgrouP2.mask ^= 8192U;
      }
      if (!string.IsNullOrEmpty(this.SubsetTitle))
      {
        lvgrouP2.pszSubsetTitle = this.SubsetTitle;
        lvgrouP2.mask ^= 32768U;
      }
      if (this.VirtualItemCount > 0)
      {
        lvgrouP2.cItems = this.VirtualItemCount;
        lvgrouP2.mask ^= 16384U;
      }
      return lvgrouP2;
    }

    private bool GetOneState(GroupState mask)
    {
      if (this.Created)
        this.State = this.GetState();
      return (this.State & mask) == mask;
    }

    protected GroupState GetState() => NativeMethods.GetGroupState(this.ListView, this.GroupId, GroupState.LVGS_ALL);

    protected int SetState(GroupState newState, GroupState mask) => NativeMethods.SetGroupInfo(this.ListView, this.GroupId, new NativeMethods.LVGROUP2()
    {
      cbSize = (uint) Marshal.SizeOf(typeof (NativeMethods.LVGROUP2)),
      mask = 4U,
      state = (uint) newState,
      stateMask = (uint) mask
    });

    private void SetOneState(bool value, GroupState mask)
    {
      this.StateMask ^= mask;
      if (value)
        this.State ^= mask;
      else
        this.State &= ~mask;
      if (!this.Created)
        return;
      this.SetState(this.State, mask);
    }
  }
}
