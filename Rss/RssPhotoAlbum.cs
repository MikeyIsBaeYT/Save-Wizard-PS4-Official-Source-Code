// Decompiled with JetBrains decompiler
// Type: Rss.RssPhotoAlbum
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public sealed class RssPhotoAlbum : RssModule
  {
    public RssPhotoAlbum(Uri link, RssPhotoAlbumCategory photoAlbumCategory)
    {
      this.NamespacePrefix = "photoAlbum";
      this.NamespaceURL = new Uri("http://xml.innothinx.com/photoAlbum");
      this.ChannelExtensions.Add(new RssModuleItem(nameof (link), true, RssDefault.Check(link).ToString()));
      this.ItemExtensions.Add((RssModuleItemCollection) photoAlbumCategory);
    }

    public RssPhotoAlbum(Uri link, RssPhotoAlbumCategories photoAlbumCategories)
    {
      this.NamespacePrefix = "photoAlbum";
      this.NamespaceURL = new Uri("http://xml.innothinx.com/photoAlbum");
      this.ChannelExtensions.Add(new RssModuleItem(nameof (link), true, RssDefault.Check(link).ToString()));
      foreach (RssModuleItemCollection photoAlbumCategory in (CollectionBase) photoAlbumCategories)
        this.ItemExtensions.Add(photoAlbumCategory);
    }

    public Uri Link
    {
      get => RssDefault.Check(this.ChannelExtensions[0].Text) == "" ? (Uri) null : new Uri(this.ChannelExtensions[0].Text);
      set => this.ChannelExtensions[0].Text = RssDefault.Check(value) == RssDefault.Uri ? "" : value.ToString();
    }
  }
}
