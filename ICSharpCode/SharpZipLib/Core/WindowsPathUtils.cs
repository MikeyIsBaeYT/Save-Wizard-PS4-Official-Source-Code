// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.WindowsPathUtils
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace ICSharpCode.SharpZipLib.Core
{
  public abstract class WindowsPathUtils
  {
    internal WindowsPathUtils()
    {
    }

    public static string DropPathRoot(string path)
    {
      string str = path;
      if (path != null && path.Length > 0)
      {
        if (path[0] == '\\' || path[0] == '/')
        {
          if (path.Length > 1 && (path[1] == '\\' || path[1] == '/'))
          {
            int index = 2;
            int num = 2;
            while (index <= path.Length && (path[index] != '\\' && path[index] != '/' || --num > 0))
              ++index;
            int startIndex = index + 1;
            str = startIndex >= path.Length ? "" : path.Substring(startIndex);
          }
        }
        else if (path.Length > 1 && path[1] == ':')
        {
          int count = 2;
          if (path.Length > 2 && (path[2] == '\\' || path[2] == '/'))
            count = 3;
          str = str.Remove(0, count);
        }
      }
      return str;
    }
  }
}
