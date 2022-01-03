// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVColumnAttribute
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [AttributeUsage(AttributeTargets.Property)]
  public class OLVColumnAttribute : Attribute
  {
    private string aspectToStringFormat;
    private bool checkBoxes;
    internal bool IsCheckBoxesSet = false;
    private int displayIndex = -1;
    private bool fillsFreeSpace;
    private int freeSpaceProportion;
    internal bool IsFreeSpaceProportionSet = false;
    private object[] groupCutoffs;
    private string[] groupDescriptions;
    private string groupWithItemCountFormat;
    private string groupWithItemCountSingularFormat;
    private bool hyperlink;
    private string imageAspectName;
    private bool isEditable = true;
    internal bool IsEditableSet = false;
    private bool isVisible = true;
    private bool isTileViewColumn;
    private int maximumWidth = -1;
    private int minimumWidth = -1;
    private string name;
    private HorizontalAlignment textAlign = HorizontalAlignment.Left;
    internal bool IsTextAlignSet = false;
    private string tag;
    private string title;
    private string toolTipText;
    private bool triStateCheckBoxes;
    internal bool IsTriStateCheckBoxesSet = false;
    private bool useInitialLetterForGroup;
    private int width = 150;

    public OLVColumnAttribute()
    {
    }

    public OLVColumnAttribute(string title) => this.Title = title;

    public string AspectToStringFormat
    {
      get => this.aspectToStringFormat;
      set => this.aspectToStringFormat = value;
    }

    public bool CheckBoxes
    {
      get => this.checkBoxes;
      set
      {
        this.checkBoxes = value;
        this.IsCheckBoxesSet = true;
      }
    }

    public int DisplayIndex
    {
      get => this.displayIndex;
      set => this.displayIndex = value;
    }

    public bool FillsFreeSpace
    {
      get => this.fillsFreeSpace;
      set => this.fillsFreeSpace = value;
    }

    public int FreeSpaceProportion
    {
      get => this.freeSpaceProportion;
      set
      {
        this.freeSpaceProportion = value;
        this.IsFreeSpaceProportionSet = true;
      }
    }

    public object[] GroupCutoffs
    {
      get => this.groupCutoffs;
      set => this.groupCutoffs = value;
    }

    public string[] GroupDescriptions
    {
      get => this.groupDescriptions;
      set => this.groupDescriptions = value;
    }

    public string GroupWithItemCountFormat
    {
      get => this.groupWithItemCountFormat;
      set => this.groupWithItemCountFormat = value;
    }

    public string GroupWithItemCountSingularFormat
    {
      get => this.groupWithItemCountSingularFormat;
      set => this.groupWithItemCountSingularFormat = value;
    }

    public bool Hyperlink
    {
      get => this.hyperlink;
      set => this.hyperlink = value;
    }

    public string ImageAspectName
    {
      get => this.imageAspectName;
      set => this.imageAspectName = value;
    }

    public bool IsEditable
    {
      get => this.isEditable;
      set
      {
        this.isEditable = value;
        this.IsEditableSet = true;
      }
    }

    public bool IsVisible
    {
      get => this.isVisible;
      set => this.isVisible = value;
    }

    public bool IsTileViewColumn
    {
      get => this.isTileViewColumn;
      set => this.isTileViewColumn = value;
    }

    public int MaximumWidth
    {
      get => this.maximumWidth;
      set => this.maximumWidth = value;
    }

    public int MinimumWidth
    {
      get => this.minimumWidth;
      set => this.minimumWidth = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public HorizontalAlignment TextAlign
    {
      get => this.textAlign;
      set
      {
        this.textAlign = value;
        this.IsTextAlignSet = true;
      }
    }

    public string Tag
    {
      get => this.tag;
      set => this.tag = value;
    }

    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    public string ToolTipText
    {
      get => this.toolTipText;
      set => this.toolTipText = value;
    }

    public bool TriStateCheckBoxes
    {
      get => this.triStateCheckBoxes;
      set
      {
        this.triStateCheckBoxes = value;
        this.IsTriStateCheckBoxesSet = true;
      }
    }

    public bool UseInitialLetterForGroup
    {
      get => this.useInitialLetterForGroup;
      set => this.useInitialLetterForGroup = value;
    }

    public int Width
    {
      get => this.width;
      set => this.width = value;
    }
  }
}
