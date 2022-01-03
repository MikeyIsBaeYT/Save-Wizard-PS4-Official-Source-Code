﻿// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.ZipNameTransform
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using ICSharpCode.SharpZipLib.Core;
using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Zip
{
  public class ZipNameTransform : INameTransform
  {
    private string trimPrefix_;
    private static readonly char[] InvalidEntryChars;
    private static readonly char[] InvalidEntryCharsRelaxed;

    public ZipNameTransform()
    {
    }

    public ZipNameTransform(string trimPrefix) => this.TrimPrefix = trimPrefix;

    static ZipNameTransform()
    {
      char[] invalidPathChars = Path.GetInvalidPathChars();
      int length1 = invalidPathChars.Length + 2;
      ZipNameTransform.InvalidEntryCharsRelaxed = new char[length1];
      Array.Copy((Array) invalidPathChars, 0, (Array) ZipNameTransform.InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
      ZipNameTransform.InvalidEntryCharsRelaxed[length1 - 1] = '*';
      ZipNameTransform.InvalidEntryCharsRelaxed[length1 - 2] = '?';
      int length2 = invalidPathChars.Length + 4;
      ZipNameTransform.InvalidEntryChars = new char[length2];
      Array.Copy((Array) invalidPathChars, 0, (Array) ZipNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
      ZipNameTransform.InvalidEntryChars[length2 - 1] = ':';
      ZipNameTransform.InvalidEntryChars[length2 - 2] = '\\';
      ZipNameTransform.InvalidEntryChars[length2 - 3] = '*';
      ZipNameTransform.InvalidEntryChars[length2 - 4] = '?';
    }

    public string TransformDirectory(string name)
    {
      name = this.TransformFile(name);
      if (name.Length <= 0)
        throw new ZipException("Cannot have an empty directory name");
      if (!name.EndsWith("/"))
        name += "/";
      return name;
    }

    public string TransformFile(string name)
    {
      if (name != null)
      {
        string lower = name.ToLower();
        if (this.trimPrefix_ != null && lower.IndexOf(this.trimPrefix_) == 0)
          name = name.Substring(this.trimPrefix_.Length);
        name = name.Replace("\\", "/");
        name = WindowsPathUtils.DropPathRoot(name);
        while (name.Length > 0 && name[0] == '/')
          name = name.Remove(0, 1);
        while (name.Length > 0 && name[name.Length - 1] == '/')
          name = name.Remove(name.Length - 1, 1);
        for (int startIndex = name.IndexOf("//"); startIndex >= 0; startIndex = name.IndexOf("//"))
          name = name.Remove(startIndex, 1);
        name = ZipNameTransform.MakeValidName(name, '_');
      }
      else
        name = string.Empty;
      return name;
    }

    public string TrimPrefix
    {
      get => this.trimPrefix_;
      set
      {
        this.trimPrefix_ = value;
        if (this.trimPrefix_ == null)
          return;
        this.trimPrefix_ = this.trimPrefix_.ToLower();
      }
    }

    private static string MakeValidName(string name, char replacement)
    {
      int index = name.IndexOfAny(ZipNameTransform.InvalidEntryChars);
      if (index >= 0)
      {
        StringBuilder stringBuilder = new StringBuilder(name);
        for (; index >= 0; index = index < name.Length ? name.IndexOfAny(ZipNameTransform.InvalidEntryChars, index + 1) : -1)
          stringBuilder[index] = replacement;
        name = stringBuilder.ToString();
      }
      return name.Length <= (int) ushort.MaxValue ? name : throw new PathTooLongException();
    }

    public static bool IsValidName(string name, bool relaxed)
    {
      bool flag = name != null;
      if (flag)
        flag = !relaxed ? name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && (uint) name.IndexOf('/') > 0U : name.IndexOfAny(ZipNameTransform.InvalidEntryCharsRelaxed) < 0;
      return flag;
    }

    public static bool IsValidName(string name) => name != null && name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && (uint) name.IndexOf('/') > 0U;
  }
}
