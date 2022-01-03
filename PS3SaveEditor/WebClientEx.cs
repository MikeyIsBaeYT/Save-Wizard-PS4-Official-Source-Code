// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.WebClientEx
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Net;
using System.Net.Security;
using System.Text;

namespace PS3SaveEditor
{
  internal class WebClientEx : WebClient
  {
    protected override WebRequest GetWebRequest(Uri address)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(address);
      webRequest.Timeout = 20000;
      webRequest.UserAgent = Util.GetUserAgent();
      webRequest.PreAuthenticate = true;
      string str = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Util.GetHtaccessUser() + ":" + Util.GetHtaccessPwd()));
      webRequest.AuthenticationLevel = AuthenticationLevel.MutualAuthRequested;
      webRequest.Headers.Add("Authorization", str);
      return (WebRequest) webRequest;
    }
  }
}
