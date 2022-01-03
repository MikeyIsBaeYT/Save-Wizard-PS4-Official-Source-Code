// Decompiled with JetBrains decompiler
// Type: Rss.RssFeed
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace Rss
{
  [Serializable]
  public class RssFeed
  {
    private RssChannelCollection channels = new RssChannelCollection();
    private RssModuleCollection modules = new RssModuleCollection();
    private ExceptionCollection exceptions = (ExceptionCollection) null;
    private DateTime lastModified = RssDefault.DateTime;
    private RssVersion rssVersion = RssVersion.Empty;
    private bool cached = false;
    private string etag = "";
    private string url = "";
    private Encoding encoding = (Encoding) null;

    public RssFeed()
    {
    }

    public RssFeed(Encoding encoding) => this.encoding = encoding;

    public override string ToString() => this.url;

    public RssChannelCollection Channels => this.channels;

    public RssModuleCollection Modules => this.modules;

    public ExceptionCollection Exceptions => this.exceptions == null ? new ExceptionCollection() : this.exceptions;

    public RssVersion Version
    {
      get => this.rssVersion;
      set => this.rssVersion = value;
    }

    public string ETag => this.etag;

    public DateTime LastModified => this.lastModified;

    public bool Cached => this.cached;

    public string Url => this.url;

    public Encoding Encoding
    {
      get => this.encoding;
      set => this.encoding = value;
    }

    public static RssFeed Read(string url) => RssFeed.read(url, (HttpWebRequest) null, (RssFeed) null);

    public static RssFeed Read(HttpWebRequest Request) => RssFeed.read(Request.RequestUri.ToString(), Request, (RssFeed) null);

    public static RssFeed Read(RssFeed oldFeed) => RssFeed.read(oldFeed.url, (HttpWebRequest) null, oldFeed);

    public static RssFeed Read(HttpWebRequest Request, RssFeed oldFeed) => RssFeed.read(oldFeed.url, Request, oldFeed);

    private static RssFeed read(string url, HttpWebRequest request, RssFeed oldFeed)
    {
      RssFeed rssFeed = new RssFeed();
      Stream stream = (Stream) null;
      Uri requestUri = new Uri(url);
      rssFeed.url = url;
      string scheme = requestUri.Scheme;
      if (!(scheme == "file"))
      {
        if (scheme == "https" || scheme == "http")
        {
          if (request == null)
            request = (HttpWebRequest) WebRequest.Create(requestUri);
          request.Credentials = (ICredentials) Util.GetNetworkCredential();
          string str = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
          request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
          request.Headers.Add("Authorization", str);
          request.UserAgent = Util.GetUserAgent();
          request.PreAuthenticate = true;
          if (oldFeed != null)
          {
            request.IfModifiedSince = oldFeed.LastModified;
            request.Headers.Add("If-None-Match", oldFeed.ETag);
          }
          try
          {
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            rssFeed.lastModified = response.LastModified;
            rssFeed.etag = response.Headers["ETag"];
            try
            {
              if (response.ContentEncoding != "")
                rssFeed.encoding = Encoding.GetEncoding(response.ContentEncoding);
            }
            catch
            {
            }
            stream = response.GetResponseStream();
          }
          catch (WebException ex)
          {
            if (oldFeed == null)
              throw ex;
            oldFeed.cached = true;
            return oldFeed;
          }
        }
      }
      else
      {
        rssFeed.lastModified = System.IO.File.GetLastWriteTime(url);
        if (oldFeed != null && rssFeed.LastModified == oldFeed.LastModified)
        {
          oldFeed.cached = true;
          return oldFeed;
        }
        stream = (Stream) new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      }
      if (stream == null)
        throw new ApplicationException("Not a valid Url");
      RssReader rssReader = (RssReader) null;
      try
      {
        rssReader = new RssReader(stream);
        RssElement rssElement;
        do
        {
          rssElement = rssReader.Read();
          if (rssElement is RssChannel)
            rssFeed.Channels.Add((RssChannel) rssElement);
        }
        while (rssElement != null);
        rssFeed.rssVersion = rssReader.Version;
      }
      finally
      {
        rssFeed.exceptions = rssReader.Exceptions;
        rssReader.Close();
      }
      return rssFeed;
    }

    public void Write(Stream stream) => this.write(this.encoding != null ? new RssWriter(stream, this.encoding) : new RssWriter(stream));

    public void Write(string fileName) => this.write(new RssWriter(fileName));

    private void write(RssWriter writer)
    {
      try
      {
        if (this.channels.Count == 0)
          throw new InvalidOperationException("Feed must contain at least one channel.");
        writer.Version = this.rssVersion;
        writer.Modules = this.modules;
        foreach (RssChannel channel in (CollectionBase) this.channels)
        {
          if (channel.Items.Count == 0)
            throw new InvalidOperationException("Channel must contain at least one item.");
          writer.Write(channel);
        }
      }
      finally
      {
        writer?.Close();
      }
    }
  }
}
