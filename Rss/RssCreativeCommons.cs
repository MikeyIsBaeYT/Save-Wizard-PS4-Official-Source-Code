// Decompiled with JetBrains decompiler
// Type: Rss.RssCreativeCommons
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Rss
{
  public sealed class RssCreativeCommons : RssModule
  {
    public RssCreativeCommons(Uri license, bool isChannelSubElement)
    {
      this.NamespacePrefix = "creativeCommons";
      this.NamespaceURL = new Uri("http://backend.userland.com/creativeCommonsRssModule");
      if (isChannelSubElement)
        this.ChannelExtensions.Add(new RssModuleItem(nameof (license), true, RssDefault.Check(license.ToString())));
      else
        this.ItemExtensions.Add(new RssModuleItemCollection()
        {
          new RssModuleItem(nameof (license), true, RssDefault.Check(license.ToString()))
        });
    }
  }
}
