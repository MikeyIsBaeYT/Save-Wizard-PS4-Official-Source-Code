// Decompiled with JetBrains decompiler
// Type: Rss.RssPhotoAlbumCategoryPhoto
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  public sealed class RssPhotoAlbumCategoryPhoto : RssModuleItemCollection
  {
    public RssPhotoAlbumCategoryPhoto(DateTime photoDate, string photoDescription, Uri photoLink) => this.Add(photoDate, photoDescription, photoLink);

    public RssPhotoAlbumCategoryPhoto(
      DateTime photoDate,
      string photoDescription,
      Uri photoLink,
      RssPhotoAlbumCategoryPhotoPeople photoPeople)
    {
      this.Add(photoDate, photoDescription, photoLink, photoPeople);
    }

    private int Add(
      DateTime photoDate,
      string photoDescription,
      Uri photoLink,
      RssPhotoAlbumCategoryPhotoPeople photoPeople)
    {
      this.Add(photoDate, photoDescription, photoLink);
      this.Add(new RssModuleItem(nameof (photoPeople), true, "", (RssModuleItemCollection) photoPeople));
      return -1;
    }

    private int Add(DateTime photoDate, string photoDescription, Uri photoLink)
    {
      this.Add(new RssModuleItem(nameof (photoDate), true, RssDefault.Check(photoDate.ToUniversalTime().ToString("r"))));
      this.Add(new RssModuleItem(nameof (photoDescription), false, RssDefault.Check(photoDescription)));
      this.Add(new RssModuleItem(nameof (photoLink), true, RssDefault.Check(photoLink).ToString()));
      return -1;
    }

    public RssPhotoAlbumCategoryPhoto(string photoDate, string photoDescription, Uri photoLink) => this.Add(photoDate, photoDescription, photoLink);

    public RssPhotoAlbumCategoryPhoto(
      string photoDate,
      string photoDescription,
      Uri photoLink,
      RssPhotoAlbumCategoryPhotoPeople photoPeople)
    {
      this.Add(photoDate, photoDescription, photoLink, photoPeople);
    }

    private int Add(
      string photoDate,
      string photoDescription,
      Uri photoLink,
      RssPhotoAlbumCategoryPhotoPeople photoPeople)
    {
      this.Add(photoDate, photoDescription, photoLink);
      this.Add(new RssModuleItem(nameof (photoPeople), true, "", (RssModuleItemCollection) photoPeople));
      return -1;
    }

    private int Add(string photoDate, string photoDescription, Uri photoLink)
    {
      this.Add(new RssModuleItem(nameof (photoDate), true, RssDefault.Check(photoDate)));
      this.Add(new RssModuleItem(nameof (photoDescription), false, RssDefault.Check(photoDescription)));
      this.Add(new RssModuleItem(nameof (photoLink), true, RssDefault.Check(photoLink).ToString()));
      return -1;
    }
  }
}
