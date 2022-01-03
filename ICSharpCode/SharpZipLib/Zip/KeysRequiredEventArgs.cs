// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.KeysRequiredEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class KeysRequiredEventArgs : EventArgs
  {
    private string fileName;
    private byte[] key;

    public KeysRequiredEventArgs(string name) => this.fileName = name;

    public KeysRequiredEventArgs(string name, byte[] keyValue)
    {
      this.fileName = name;
      this.key = keyValue;
    }

    public string FileName => this.fileName;

    public byte[] Key
    {
      get => this.key;
      set => this.key = value;
    }
  }
}
