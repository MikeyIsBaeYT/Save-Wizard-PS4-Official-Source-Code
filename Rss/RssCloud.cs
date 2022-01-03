// Decompiled with JetBrains decompiler
// Type: Rss.RssCloud
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Rss
{
  [Serializable]
  public class RssCloud : RssElement
  {
    private RssCloudProtocol protocol = RssCloudProtocol.Empty;
    private string domain = "";
    private string path = "";
    private string registerProcedure = "";
    private int port = -1;

    public string Domain
    {
      get => this.domain;
      set => this.domain = RssDefault.Check(value);
    }

    public int Port
    {
      get => this.port;
      set => this.port = RssDefault.Check(value);
    }

    public string Path
    {
      get => this.path;
      set => this.path = RssDefault.Check(value);
    }

    public string RegisterProcedure
    {
      get => this.registerProcedure;
      set => this.registerProcedure = RssDefault.Check(value);
    }

    public RssCloudProtocol Protocol
    {
      get => this.protocol;
      set => this.protocol = value;
    }
  }
}
