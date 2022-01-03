// Decompiled with JetBrains decompiler
// Type: Rss.RssPhotoAlbumCategory
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;

namespace Rss
{
  public sealed class RssPhotoAlbumCategory : RssModuleItemCollection
  {
    public RssPhotoAlbumCategory(
      string categoryName,
      string categoryDescription,
      DateTime categoryDateFrom,
      DateTime categoryDateTo,
      RssPhotoAlbumCategoryPhoto categoryPhoto)
    {
      this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhoto);
    }

    private int Add(
      string categoryName,
      string categoryDescription,
      DateTime categoryDateFrom,
      DateTime categoryDateTo,
      RssPhotoAlbumCategoryPhoto categoryPhoto)
    {
      RssModuleItemCollection subElements = new RssModuleItemCollection();
      subElements.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom.ToUniversalTime().ToString("r"))));
      subElements.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo.ToUniversalTime().ToString("r"))));
      this.Add(new RssModuleItem(nameof (categoryName), true, RssDefault.Check(categoryName)));
      this.Add(new RssModuleItem(nameof (categoryDescription), true, RssDefault.Check(categoryDescription)));
      this.Add(new RssModuleItem("categoryDateRange", true, "", subElements));
      this.Add(new RssModuleItem(nameof (categoryPhoto), true, "", (RssModuleItemCollection) categoryPhoto));
      return -1;
    }

    public RssPhotoAlbumCategory(
      string categoryName,
      string categoryDescription,
      string categoryDateFrom,
      string categoryDateTo,
      RssPhotoAlbumCategoryPhoto categoryPhoto)
    {
      this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhoto);
    }

    private int Add(
      string categoryName,
      string categoryDescription,
      string categoryDateFrom,
      string categoryDateTo,
      RssPhotoAlbumCategoryPhoto categoryPhoto)
    {
      RssModuleItemCollection subElements = new RssModuleItemCollection();
      subElements.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom)));
      subElements.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo)));
      this.Add(new RssModuleItem(nameof (categoryName), true, RssDefault.Check(categoryName)));
      this.Add(new RssModuleItem(nameof (categoryDescription), true, RssDefault.Check(categoryDescription)));
      this.Add(new RssModuleItem("categoryDateRange", true, "", subElements));
      this.Add(new RssModuleItem(nameof (categoryPhoto), true, "", (RssModuleItemCollection) categoryPhoto));
      return -1;
    }

    public RssPhotoAlbumCategory(
      string categoryName,
      string categoryDescription,
      DateTime categoryDateFrom,
      DateTime categoryDateTo,
      RssPhotoAlbumCategoryPhotos categoryPhotos)
    {
      this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhotos);
    }

    private int Add(
      string categoryName,
      string categoryDescription,
      DateTime categoryDateFrom,
      DateTime categoryDateTo,
      RssPhotoAlbumCategoryPhotos categoryPhotos)
    {
      RssModuleItemCollection subElements = new RssModuleItemCollection();
      subElements.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom.ToUniversalTime().ToString("r"))));
      subElements.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo.ToUniversalTime().ToString("r"))));
      this.Add(new RssModuleItem(nameof (categoryName), true, RssDefault.Check(categoryName)));
      this.Add(new RssModuleItem(nameof (categoryDescription), true, RssDefault.Check(categoryDescription)));
      this.Add(new RssModuleItem("categoryDateRange", true, "", subElements));
      foreach (RssModuleItemCollection categoryPhoto in (CollectionBase) categoryPhotos)
        this.Add(new RssModuleItem("categoryPhoto", true, "", categoryPhoto));
      return -1;
    }

    public RssPhotoAlbumCategory(
      string categoryName,
      string categoryDescription,
      string categoryDateFrom,
      string categoryDateTo,
      RssPhotoAlbumCategoryPhotos categoryPhotos)
    {
      this.Add(categoryName, categoryDescription, categoryDateFrom, categoryDateTo, categoryPhotos);
    }

    private int Add(
      string categoryName,
      string categoryDescription,
      string categoryDateFrom,
      string categoryDateTo,
      RssPhotoAlbumCategoryPhotos categoryPhotos)
    {
      RssModuleItemCollection subElements = new RssModuleItemCollection();
      subElements.Add(new RssModuleItem("from", true, RssDefault.Check(categoryDateFrom)));
      subElements.Add(new RssModuleItem("to", true, RssDefault.Check(categoryDateTo)));
      this.Add(new RssModuleItem(nameof (categoryName), true, RssDefault.Check(categoryName)));
      this.Add(new RssModuleItem(nameof (categoryDescription), true, RssDefault.Check(categoryDescription)));
      this.Add(new RssModuleItem("categoryDateRange", true, "", subElements));
      foreach (RssModuleItemCollection categoryPhoto in (CollectionBase) categoryPhotos)
        this.Add(new RssModuleItem("categoryPhoto", true, "", categoryPhoto));
      return -1;
    }
  }
}
