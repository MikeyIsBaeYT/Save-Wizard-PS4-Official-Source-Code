// Decompiled with JetBrains decompiler
// Type: Rss.RssReader
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Rss
{
  public class RssReader
  {
    private Stack xmlNodeStack = new Stack();
    private StringBuilder elementText = new StringBuilder();
    private XmlTextReader reader = (XmlTextReader) null;
    private bool wroteChannel = false;
    private RssVersion rssVersion = RssVersion.Empty;
    private ExceptionCollection exceptions = new ExceptionCollection();
    private RssTextInput textInput = (RssTextInput) null;
    private RssImage image = (RssImage) null;
    private RssCloud cloud = (RssCloud) null;
    private RssChannel channel = (RssChannel) null;
    private RssSource source = (RssSource) null;
    private RssEnclosure enclosure = (RssEnclosure) null;
    private RssGuid guid = (RssGuid) null;
    private RssCategory category = (RssCategory) null;
    private RssItem item = (RssItem) null;

    private void InitReader()
    {
      this.reader.WhitespaceHandling = WhitespaceHandling.None;
      this.reader.XmlResolver = (XmlResolver) null;
    }

    public RssReader(string url)
    {
      try
      {
        this.reader = new XmlTextReader(url);
        this.InitReader();
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Unable to retrieve file containing the RSS data.", ex);
      }
    }

    public RssReader(TextReader textReader)
    {
      try
      {
        this.reader = new XmlTextReader(textReader);
        this.InitReader();
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Unable to retrieve file containing the RSS data.", ex);
      }
    }

    public RssReader(Stream stream)
    {
      try
      {
        this.reader = new XmlTextReader(stream);
        this.InitReader();
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Unable to retrieve file containing the RSS data.", ex);
      }
    }

    public RssElement Read()
    {
      bool flag1 = false;
      RssElement rssElement = (RssElement) null;
      int num1 = -1;
      int num2 = -1;
      if (this.reader == null)
        throw new InvalidOperationException("RssReader has been closed, and can not be read.");
      do
      {
        bool flag2 = true;
        try
        {
          flag1 = this.reader.Read();
        }
        catch (EndOfStreamException ex)
        {
          throw new EndOfStreamException("Unable to read an RssElement. Reached the end of the stream.", (Exception) ex);
        }
        catch (XmlException ex)
        {
          if ((num1 != -1 || num2 != -1) && this.reader.LineNumber == num1 && this.reader.LinePosition == num2)
            throw this.exceptions.LastException;
          num1 = this.reader.LineNumber;
          num2 = this.reader.LinePosition;
          this.exceptions.Add((Exception) ex);
        }
        if (flag1)
        {
          string lower1 = this.reader.Name.ToLower();
          switch (this.reader.NodeType)
          {
            case XmlNodeType.Element:
              if (!this.reader.IsEmptyElement)
              {
                this.elementText = new StringBuilder();
                switch (lower1)
                {
                  case "category":
                    this.category = new RssCategory();
                    if ((string) this.xmlNodeStack.Peek() == "channel")
                      this.channel.Categories.Add(this.category);
                    else
                      this.item.Categories.Add(this.category);
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      string lower2 = this.reader.Name.ToLower();
                      if (lower2 == "url" || lower2 == "domain")
                        this.category.Domain = this.reader.Value;
                    }
                    break;
                  case "channel":
                    this.channel = new RssChannel();
                    this.textInput = (RssTextInput) null;
                    this.image = (RssImage) null;
                    this.cloud = (RssCloud) null;
                    this.source = (RssSource) null;
                    this.enclosure = (RssEnclosure) null;
                    this.category = (RssCategory) null;
                    this.item = (RssItem) null;
                    break;
                  case "cloud":
                    flag2 = false;
                    this.cloud = new RssCloud();
                    this.channel.Cloud = this.cloud;
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      string lower3 = this.reader.Name.ToLower();
                      if (!(lower3 == "domain"))
                      {
                        if (!(lower3 == "port"))
                        {
                          if (!(lower3 == "path"))
                          {
                            if (!(lower3 == "registerprocedure"))
                            {
                              if (lower3 == "protocol")
                              {
                                string lower4 = this.reader.Value.ToLower();
                                this.cloud.Protocol = lower4 == "xml-rpc" ? RssCloudProtocol.XmlRpc : (lower4 == "soap" ? RssCloudProtocol.Soap : (lower4 == "http-post" ? RssCloudProtocol.HttpPost : RssCloudProtocol.Empty));
                              }
                            }
                            else
                              this.cloud.RegisterProcedure = this.reader.Value;
                          }
                          else
                            this.cloud.Path = this.reader.Value;
                        }
                        else
                        {
                          try
                          {
                            this.cloud.Port = (int) ushort.Parse(this.reader.Value);
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                          }
                        }
                      }
                      else
                        this.cloud.Domain = this.reader.Value;
                    }
                    break;
                  case "enclosure":
                    this.enclosure = new RssEnclosure();
                    this.item.Enclosure = this.enclosure;
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      string lower5 = this.reader.Name.ToLower();
                      if (!(lower5 == "url"))
                      {
                        if (!(lower5 == "length"))
                        {
                          if (lower5 == "type")
                            this.enclosure.Type = this.reader.Value;
                        }
                        else
                        {
                          try
                          {
                            this.enclosure.Length = int.Parse(this.reader.Value);
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                          }
                        }
                      }
                      else
                      {
                        try
                        {
                          this.enclosure.Url = new Uri(this.reader.Value);
                        }
                        catch (Exception ex)
                        {
                          this.exceptions.Add(ex);
                        }
                      }
                    }
                    break;
                  case "guid":
                    this.guid = new RssGuid();
                    this.item.Guid = this.guid;
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      if (this.reader.Name.ToLower() == "ispermalink")
                      {
                        try
                        {
                          this.guid.PermaLink = (DBBool) bool.Parse(this.reader.Value);
                        }
                        catch (Exception ex)
                        {
                          this.exceptions.Add(ex);
                        }
                      }
                    }
                    break;
                  case "image":
                    this.image = new RssImage();
                    this.channel.Image = this.image;
                    break;
                  case "item":
                    if (!this.wroteChannel)
                    {
                      this.wroteChannel = true;
                      rssElement = (RssElement) this.channel;
                      flag1 = false;
                    }
                    this.item = new RssItem();
                    this.channel.Items.Add(this.item);
                    break;
                  case "rdf":
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      if (this.reader.Name.ToLower() == "version")
                      {
                        string str = this.reader.Value;
                        this.rssVersion = str == "0.90" ? RssVersion.RSS090 : (str == "1.0" ? RssVersion.RSS10 : RssVersion.NotSupported);
                      }
                    }
                    break;
                  case "rss":
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      if (this.reader.Name.ToLower() == "version")
                      {
                        string str = this.reader.Value;
                        this.rssVersion = str == "0.91" ? RssVersion.RSS091 : (str == "0.92" ? RssVersion.RSS092 : (str == "2.0" ? RssVersion.RSS20 : RssVersion.NotSupported));
                      }
                    }
                    break;
                  case "source":
                    this.source = new RssSource();
                    this.item.Source = this.source;
                    for (int i = 0; i < this.reader.AttributeCount; ++i)
                    {
                      this.reader.MoveToAttribute(i);
                      if (this.reader.Name.ToLower() == "url")
                      {
                        try
                        {
                          this.source.Url = new Uri(this.reader.Value);
                        }
                        catch (Exception ex)
                        {
                          this.exceptions.Add(ex);
                        }
                      }
                    }
                    break;
                  case "textinput":
                    this.textInput = new RssTextInput();
                    this.channel.TextInput = this.textInput;
                    break;
                }
                if (flag2)
                {
                  this.xmlNodeStack.Push((object) lower1);
                  break;
                }
                break;
              }
              break;
            case XmlNodeType.Text:
              this.elementText.Append(this.reader.Value);
              break;
            case XmlNodeType.CDATA:
              this.elementText.Append(this.reader.Value);
              break;
            case XmlNodeType.EndElement:
              if (this.xmlNodeStack.Count != 1)
              {
                string str1 = (string) this.xmlNodeStack.Pop();
                string str2 = (string) this.xmlNodeStack.Peek();
                switch (str1)
                {
                  case "category":
                    this.category.Name = this.elementText.ToString();
                    rssElement = (RssElement) this.category;
                    flag1 = false;
                    break;
                  case "channel":
                    if (this.wroteChannel)
                    {
                      this.wroteChannel = false;
                      break;
                    }
                    this.wroteChannel = true;
                    rssElement = (RssElement) this.channel;
                    flag1 = false;
                    break;
                  case "cloud":
                    rssElement = (RssElement) this.cloud;
                    flag1 = false;
                    break;
                  case "enclosure":
                    rssElement = (RssElement) this.enclosure;
                    flag1 = false;
                    break;
                  case "guid":
                    this.guid.Name = this.elementText.ToString();
                    rssElement = (RssElement) this.guid;
                    flag1 = false;
                    break;
                  case "image":
                    rssElement = (RssElement) this.image;
                    flag1 = false;
                    break;
                  case "item":
                    rssElement = (RssElement) this.item;
                    flag1 = false;
                    break;
                  case "source":
                    this.source.Name = this.elementText.ToString();
                    rssElement = (RssElement) this.source;
                    flag1 = false;
                    break;
                  case "textinput":
                    rssElement = (RssElement) this.textInput;
                    flag1 = false;
                    break;
                }
                string str3 = str2;
                if (!(str3 == "item"))
                {
                  if (!(str3 == "channel"))
                  {
                    if (!(str3 == "image"))
                    {
                      if (!(str3 == "textinput"))
                      {
                        if (!(str3 == "skipdays"))
                        {
                          if (str3 == "skiphours" && str1 == "hour")
                          {
                            this.channel.SkipHours[(int) byte.Parse(this.elementText.ToString().ToLower())] = true;
                            break;
                          }
                          break;
                        }
                        if (str1 == "day")
                        {
                          switch (this.elementText.ToString().ToLower())
                          {
                            case "friday":
                              this.channel.SkipDays[4] = true;
                              break;
                            case "monday":
                              this.channel.SkipDays[0] = true;
                              break;
                            case "saturday":
                              this.channel.SkipDays[5] = true;
                              break;
                            case "sunday":
                              this.channel.SkipDays[6] = true;
                              break;
                            case "thursday":
                              this.channel.SkipDays[3] = true;
                              break;
                            case "tuesday":
                              this.channel.SkipDays[1] = true;
                              break;
                            case "wednesday":
                              this.channel.SkipDays[2] = true;
                              break;
                          }
                        }
                        else
                          break;
                      }
                      else
                      {
                        string str4 = str1;
                        if (!(str4 == "title"))
                        {
                          if (!(str4 == "description"))
                          {
                            if (!(str4 == "name"))
                            {
                              if (str4 == "link")
                              {
                                try
                                {
                                  this.textInput.Link = new Uri(this.elementText.ToString());
                                  break;
                                }
                                catch (Exception ex)
                                {
                                  this.exceptions.Add(ex);
                                  break;
                                }
                              }
                              else
                                break;
                            }
                            else
                            {
                              this.textInput.Name = this.elementText.ToString();
                              break;
                            }
                          }
                          else
                          {
                            this.textInput.Description = this.elementText.ToString();
                            break;
                          }
                        }
                        else
                        {
                          this.textInput.Title = this.elementText.ToString();
                          break;
                        }
                      }
                    }
                    else
                    {
                      string str5 = str1;
                      if (!(str5 == "url"))
                      {
                        if (!(str5 == "title"))
                        {
                          if (!(str5 == "link"))
                          {
                            if (!(str5 == "description"))
                            {
                              if (!(str5 == "width"))
                              {
                                if (str5 == "height")
                                {
                                  try
                                  {
                                    this.image.Height = (int) byte.Parse(this.elementText.ToString());
                                    break;
                                  }
                                  catch (Exception ex)
                                  {
                                    this.exceptions.Add(ex);
                                    break;
                                  }
                                }
                                else
                                  break;
                              }
                              else
                              {
                                try
                                {
                                  this.image.Width = (int) byte.Parse(this.elementText.ToString());
                                  break;
                                }
                                catch (Exception ex)
                                {
                                  this.exceptions.Add(ex);
                                  break;
                                }
                              }
                            }
                            else
                            {
                              this.image.Description = this.elementText.ToString();
                              break;
                            }
                          }
                          else
                          {
                            try
                            {
                              this.image.Link = new Uri(this.elementText.ToString());
                              break;
                            }
                            catch (Exception ex)
                            {
                              this.exceptions.Add(ex);
                              break;
                            }
                          }
                        }
                        else
                        {
                          this.image.Title = this.elementText.ToString();
                          break;
                        }
                      }
                      else
                      {
                        try
                        {
                          this.image.Url = new Uri(this.elementText.ToString());
                          break;
                        }
                        catch (Exception ex)
                        {
                          this.exceptions.Add(ex);
                          break;
                        }
                      }
                    }
                  }
                  else
                  {
                    string s = str1;
                    // ISSUE: reference to a compiler-generated method
                    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
                    {
                      case 105518051:
                        if (s == "lastbuilddate")
                        {
                          try
                          {
                            this.channel.LastBuildDate = DateTime.Parse(this.elementText.ToString());
                            break;
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                            break;
                          }
                        }
                        else
                          break;
                      case 232457833:
                        if (s == "link")
                        {
                          try
                          {
                            this.channel.Link = new Uri(this.elementText.ToString());
                            break;
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                            break;
                          }
                        }
                        else
                          break;
                      case 310887988:
                        if (s == "managingeditor")
                        {
                          this.channel.ManagingEditor = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 686503122:
                        if (s == "docs")
                        {
                          this.channel.Docs = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 879704937:
                        if (s == "description")
                        {
                          this.channel.Description = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 1860974018:
                        if (s == "generator")
                        {
                          this.channel.Generator = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 2207416544:
                        if (s == "pubdate")
                        {
                          try
                          {
                            this.channel.PubDate = DateTime.Parse(this.elementText.ToString());
                            break;
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                            break;
                          }
                        }
                        else
                          break;
                      case 2223235319:
                        if (s == "webmaster")
                        {
                          this.channel.WebMaster = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 2556802313:
                        if (s == "title")
                        {
                          this.channel.Title = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 3104697662:
                        if (s == "copyright")
                        {
                          this.channel.Copyright = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 3119462523:
                        if (s == "language")
                        {
                          this.channel.Language = this.elementText.ToString();
                          break;
                        }
                        break;
                      case 3173728859:
                        if (s == "ttl")
                        {
                          try
                          {
                            this.channel.TimeToLive = int.Parse(this.elementText.ToString());
                            break;
                          }
                          catch (Exception ex)
                          {
                            this.exceptions.Add(ex);
                            break;
                          }
                        }
                        else
                          break;
                      case 4069068880:
                        if (s == "rating")
                        {
                          this.channel.Rating = this.elementText.ToString();
                          break;
                        }
                        break;
                    }
                  }
                }
                else
                {
                  string str6 = str1;
                  if (!(str6 == "title"))
                  {
                    if (!(str6 == "link"))
                    {
                      if (!(str6 == "description"))
                      {
                        if (!(str6 == "author"))
                        {
                          if (!(str6 == "comments"))
                          {
                            if (str6 == "pubdate")
                            {
                              try
                              {
                                this.item.PubDate = DateTime.Parse(this.elementText.ToString());
                                break;
                              }
                              catch (Exception ex)
                              {
                                try
                                {
                                  string str7 = this.elementText.ToString();
                                  this.item.PubDate = DateTime.Parse(str7.Substring(0, str7.Length - 5) + "GMT");
                                  break;
                                }
                                catch
                                {
                                  this.exceptions.Add(ex);
                                  break;
                                }
                              }
                            }
                            else
                              break;
                          }
                          else
                          {
                            this.item.Comments = this.elementText.ToString();
                            break;
                          }
                        }
                        else
                        {
                          this.item.Author = this.elementText.ToString();
                          break;
                        }
                      }
                      else
                      {
                        this.item.Description = this.elementText.ToString();
                        break;
                      }
                    }
                    else
                    {
                      this.item.Link = this.elementText.Length <= 0 ? (Uri) null : new Uri(this.elementText.ToString());
                      break;
                    }
                  }
                  else
                  {
                    this.item.Title = this.elementText.ToString();
                    break;
                  }
                }
              }
              else
                break;
              break;
          }
        }
      }
      while (flag1);
      return rssElement;
    }

    public ExceptionCollection Exceptions => this.exceptions;

    public RssVersion Version => this.rssVersion;

    public void Close()
    {
      this.textInput = (RssTextInput) null;
      this.image = (RssImage) null;
      this.cloud = (RssCloud) null;
      this.channel = (RssChannel) null;
      this.source = (RssSource) null;
      this.enclosure = (RssEnclosure) null;
      this.category = (RssCategory) null;
      this.item = (RssItem) null;
      if (this.reader != null)
      {
        this.reader.Close();
        this.reader = (XmlTextReader) null;
      }
      this.elementText = (StringBuilder) null;
      this.xmlNodeStack = (Stack) null;
    }
  }
}
