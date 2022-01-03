// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.BadCrcException
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
  [Serializable]
  public class BadCrcException : ZipException
  {
    public BadCrcException()
    {
    }

    public BadCrcException(string message)
      : base(message)
    {
    }

    protected BadCrcException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
