// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ReadOptions
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
  public class ReadOptions
  {
    public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

    public TextWriter StatusMessageWriter { get; set; }

    public Encoding Encoding { get; set; }
  }
}
