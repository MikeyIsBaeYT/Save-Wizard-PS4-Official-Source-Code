// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.WindowsNameTransform
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class WindowsNameTransform : INameTransform
  {
    private const int MaxPath = 260;
    private string _baseDirectory;
    private bool _trimIncomingPaths;
    private char _replacementChar = '_';
    private static readonly char[] InvalidEntryChars;

    public WindowsNameTransform(string baseDirectory) => this.BaseDirectory = baseDirectory != null ? baseDirectory : throw new ArgumentNullException(nameof (baseDirectory), "Directory name is invalid");

    public WindowsNameTransform()
    {
    }

    public string BaseDirectory
    {
      get => this._baseDirectory;
      set => this._baseDirectory = value != null ? Path.GetFullPath(value) : throw new ArgumentNullException(nameof (value));
    }

    public bool TrimIncomingPaths
    {
      get => this._trimIncomingPaths;
      set => this._trimIncomingPaths = value;
    }

    public string TransformDirectory(string name)
    {
      name = this.TransformFile(name);
      if (name.Length <= 0)
        throw new ZipException("Cannot have an empty directory name");
      while (name.EndsWith("\\"))
        name = name.Remove(name.Length - 1, 1);
      return name;
    }

    public string TransformFile(string name)
    {
      if (name != null)
      {
        name = WindowsNameTransform.MakeValidName(name, this._replacementChar);
        if (this._trimIncomingPaths)
          name = Path.GetFileName(name);
        if (this._baseDirectory != null)
          name = Path.Combine(this._baseDirectory, name);
      }
      else
        name = string.Empty;
      return name;
    }

    public static bool IsValidName(string name) => name != null && name.Length <= 260 && string.Compare(name, WindowsNameTransform.MakeValidName(name, '_')) == 0;

    static WindowsNameTransform()
    {
      char[] invalidPathChars = Path.GetInvalidPathChars();
      int length = invalidPathChars.Length + 3;
      WindowsNameTransform.InvalidEntryChars = new char[length];
      Array.Copy((Array) invalidPathChars, 0, (Array) WindowsNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
      WindowsNameTransform.InvalidEntryChars[length - 1] = '*';
      WindowsNameTransform.InvalidEntryChars[length - 2] = '?';
      WindowsNameTransform.InvalidEntryChars[length - 3] = ':';
    }

    public static string MakeValidName(string name, char replacement)
    {
      name = name != null ? WindowsPathUtils.DropPathRoot(name.Replace("/", "\\")) : throw new ArgumentNullException(nameof (name));
      while (name.Length > 0 && name[0] == '\\')
        name = name.Remove(0, 1);
      while (name.Length > 0 && name[name.Length - 1] == '\\')
        name = name.Remove(name.Length - 1, 1);
      for (int startIndex = name.IndexOf("\\\\"); startIndex >= 0; startIndex = name.IndexOf("\\\\"))
        name = name.Remove(startIndex, 1);
      int index = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars);
      if (index >= 0)
      {
        StringBuilder stringBuilder = new StringBuilder(name);
        for (; index >= 0; index = index < name.Length ? name.IndexOfAny(WindowsNameTransform.InvalidEntryChars, index + 1) : -1)
          stringBuilder[index] = replacement;
        name = stringBuilder.ToString();
      }
      return name.Length <= 260 ? name : throw new PathTooLongException();
    }

    public char Replacement
    {
      get => this._replacementChar;
      set
      {
        for (int index = 0; index < WindowsNameTransform.InvalidEntryChars.Length; ++index)
        {
          if ((int) WindowsNameTransform.InvalidEntryChars[index] == (int) value)
            throw new ArgumentException("invalid path character");
        }
        this._replacementChar = value != '\\' && value != '/' ? value : throw new ArgumentException("invalid replacement character");
      }
    }
  }
}
