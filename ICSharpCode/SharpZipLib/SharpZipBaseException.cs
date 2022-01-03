// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.SharpZipBaseException
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib
{
  [Serializable]
  public class SharpZipBaseException : ApplicationException
  {
    protected SharpZipBaseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public SharpZipBaseException()
    {
    }

    public SharpZipBaseException(string message)
      : base(message)
    {
    }

    public SharpZipBaseException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
