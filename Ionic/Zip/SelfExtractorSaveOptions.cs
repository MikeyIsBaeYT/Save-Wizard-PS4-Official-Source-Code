// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SelfExtractorSaveOptions
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;

namespace Ionic.Zip
{
  public class SelfExtractorSaveOptions
  {
    public SelfExtractorFlavor Flavor { get; set; }

    public string PostExtractCommandLine { get; set; }

    public string DefaultExtractDirectory { get; set; }

    public string IconFile { get; set; }

    public bool Quiet { get; set; }

    public ExtractExistingFileAction ExtractExistingFile { get; set; }

    public bool RemoveUnpackedFilesAfterExecute { get; set; }

    public Version FileVersion { get; set; }

    public string ProductVersion { get; set; }

    public string Copyright { get; set; }

    public string Description { get; set; }

    public string ProductName { get; set; }

    public string SfxExeWindowTitle { get; set; }

    public string AdditionalCompilerSwitches { get; set; }
  }
}
