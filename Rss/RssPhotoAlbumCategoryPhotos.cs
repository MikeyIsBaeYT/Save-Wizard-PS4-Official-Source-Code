// Decompiled with JetBrains decompiler
// Type: Rss.RssPhotoAlbumCategoryPhotos
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Rss
{
  public sealed class RssPhotoAlbumCategoryPhotos : RssModuleItemCollectionCollection
  {
    public int Add(RssPhotoAlbumCategoryPhoto photo) => this.Add((RssModuleItemCollection) photo);
  }
}
