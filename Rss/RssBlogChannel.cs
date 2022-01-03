// Decompiled with JetBrains decompiler
// Type: Rss.RssBlogChannel
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  public sealed class RssBlogChannel : RssModule
  {
    public RssBlogChannel(Uri blogRoll, Uri mySubscriptions, Uri blink, Uri changes)
    {
      this.NamespacePrefix = "blogChannel";
      this.NamespaceURL = new Uri("http://backend.userland.com/blogChannelModule");
      this.ChannelExtensions.Add(new RssModuleItem(nameof (blogRoll), true, RssDefault.Check(blogRoll.ToString())));
      this.ChannelExtensions.Add(new RssModuleItem(nameof (mySubscriptions), true, RssDefault.Check(mySubscriptions.ToString())));
      this.ChannelExtensions.Add(new RssModuleItem(nameof (blink), true, RssDefault.Check(blink.ToString())));
      this.ChannelExtensions.Add(new RssModuleItem(nameof (changes), true, RssDefault.Check(changes.ToString())));
    }
  }
}
