// Decompiled with JetBrains decompiler
// Type: Rss.RssModule
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  [Serializable]
  public abstract class RssModule
  {
    private ArrayList _alBindTo = new ArrayList();
    private RssModuleItemCollection _rssChannelExtensions = new RssModuleItemCollection();
    private RssModuleItemCollectionCollection _rssItemExtensions = new RssModuleItemCollectionCollection();
    private string _sNamespacePrefix = "";
    private Uri _uriNamespaceURL = RssDefault.Uri;

    internal RssModuleItemCollection ChannelExtensions
    {
      get => this._rssChannelExtensions;
      set => this._rssChannelExtensions = value;
    }

    internal RssModuleItemCollectionCollection ItemExtensions
    {
      get => this._rssItemExtensions;
      set => this._rssItemExtensions = value;
    }

    public string NamespacePrefix
    {
      get => this._sNamespacePrefix;
      set => this._sNamespacePrefix = RssDefault.Check(value);
    }

    public Uri NamespaceURL
    {
      get => this._uriNamespaceURL;
      set => this._uriNamespaceURL = RssDefault.Check(value);
    }

    public void BindTo(int channelHashCode) => this._alBindTo.Add((object) channelHashCode);

    public bool IsBoundTo(int channelHashCode) => this._alBindTo.BinarySearch(0, this._alBindTo.Count, (object) channelHashCode, (IComparer) null) >= 0;
  }
}
