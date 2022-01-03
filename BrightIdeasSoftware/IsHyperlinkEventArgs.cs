// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.IsHyperlinkEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace BrightIdeasSoftware
{
  public class IsHyperlinkEventArgs : EventArgs
  {
    private ObjectListView listView;
    private object model;
    private OLVColumn column;
    private string text;
    private bool isHyperlink;
    public string Url;

    public ObjectListView ListView
    {
      get => this.listView;
      internal set => this.listView = value;
    }

    public object Model
    {
      get => this.model;
      internal set => this.model = value;
    }

    public OLVColumn Column
    {
      get => this.column;
      internal set => this.column = value;
    }

    public string Text
    {
      get => this.text;
      internal set => this.text = value;
    }

    public bool IsHyperlink
    {
      get => this.isHyperlink;
      set => this.isHyperlink = value;
    }
  }
}
