// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.NameAndSizeFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
  [Obsolete("Use ExtendedPathFilter instead")]
  public class NameAndSizeFilter : PathFilter
  {
    private long minSize_;
    private long maxSize_ = long.MaxValue;

    public NameAndSizeFilter(string filter, long minSize, long maxSize)
      : base(filter)
    {
      this.MinSize = minSize;
      this.MaxSize = maxSize;
    }

    public override bool IsMatch(string name)
    {
      bool flag = base.IsMatch(name);
      if (flag)
      {
        long length = new FileInfo(name).Length;
        flag = this.MinSize <= length && this.MaxSize >= length;
      }
      return flag;
    }

    public long MinSize
    {
      get => this.minSize_;
      set => this.minSize_ = value >= 0L && this.maxSize_ >= value ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public long MaxSize
    {
      get => this.maxSize_;
      set => this.maxSize_ = value >= 0L && this.minSize_ <= value ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
