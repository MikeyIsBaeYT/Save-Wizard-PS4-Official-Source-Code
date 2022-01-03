// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ComHelper
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Runtime.InteropServices;

namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  public class ComHelper
  {
    public bool IsZipFile(string filename) => ZipFile.IsZipFile(filename);

    public bool IsZipFileWithExtract(string filename) => ZipFile.IsZipFile(filename, true);

    public bool CheckZip(string filename) => ZipFile.CheckZip(filename);

    public bool CheckZipPassword(string filename, string password) => ZipFile.CheckZipPassword(filename, password);

    public void FixZipDirectory(string filename) => ZipFile.FixZipDirectory(filename);

    public string GetZipLibraryVersion() => ZipFile.LibraryVersion.ToString();
  }
}
