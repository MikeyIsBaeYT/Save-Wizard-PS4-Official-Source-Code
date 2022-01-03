// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipFile
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using Ionic.Zlib;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  public class ZipFile : IEnumerable, IEnumerable<ZipEntry>, IDisposable
  {
    private TextWriter _StatusMessageTextWriter;
    private bool _CaseSensitiveRetrieval;
    private Stream _readstream;
    private Stream _writestream;
    private ushort _versionMadeBy;
    private ushort _versionNeededToExtract;
    private uint _diskNumberWithCd;
    private int _maxOutputSegmentSize;
    private uint _numberOfSegmentsForMostRecentSave;
    private ZipErrorAction _zipErrorAction;
    private bool _disposed;
    private Dictionary<string, ZipEntry> _entries;
    private List<ZipEntry> _zipEntriesAsList;
    private string _name;
    private string _readName;
    private string _Comment;
    internal string _Password;
    private bool _emitNtfsTimes = true;
    private bool _emitUnixTimes;
    private CompressionStrategy _Strategy = CompressionStrategy.Default;
    private CompressionMethod _compressionMethod = CompressionMethod.Deflate;
    private bool _fileAlreadyExists;
    private string _temporaryFileName;
    private bool _contentsChanged;
    private bool _hasBeenSaved;
    private string _TempFileFolder;
    private bool _ReadStreamIsOurs = true;
    private object LOCK = new object();
    private bool _saveOperationCanceled;
    private bool _extractOperationCanceled;
    private bool _addOperationCanceled;
    private EncryptionAlgorithm _Encryption;
    private bool _JustSaved;
    private long _locEndOfCDS = -1;
    private uint _OffsetOfCentralDirectory;
    private long _OffsetOfCentralDirectory64;
    private bool? _OutputUsesZip64;
    internal bool _inExtractAll;
    private Encoding _alternateEncoding = Encoding.GetEncoding("IBM437");
    private ZipOption _alternateEncodingUsage = ZipOption.Default;
    private static Encoding _defaultEncoding = Encoding.GetEncoding("IBM437");
    private int _BufferSize = ZipFile.BufferSizeDefault;
    internal ParallelDeflateOutputStream ParallelDeflater;
    private long _ParallelDeflateThreshold;
    private int _maxBufferPairs = 16;
    internal Zip64Option _zip64 = Zip64Option.Default;
    private bool _SavingSfx;
    public static readonly int BufferSizeDefault = 32768;
    private long _lengthOfReadStream = -99;
    private static ZipFile.ExtractorSettings[] SettingsList = new ZipFile.ExtractorSettings[2]
    {
      new ZipFile.ExtractorSettings()
      {
        Flavor = SelfExtractorFlavor.WinFormsApplication,
        ReferencedAssemblies = new List<string>()
        {
          "System.dll",
          "System.Windows.Forms.dll",
          "System.Drawing.dll"
        },
        CopyThroughResources = new List<string>()
        {
          "Ionic.Zip.WinFormsSelfExtractorStub.resources",
          "Ionic.Zip.Forms.PasswordDialog.resources",
          "Ionic.Zip.Forms.ZipContentsDialog.resources"
        },
        ResourcesToCompile = new List<string>()
        {
          "WinFormsSelfExtractorStub.cs",
          "WinFormsSelfExtractorStub.Designer.cs",
          "PasswordDialog.cs",
          "PasswordDialog.Designer.cs",
          "ZipContentsDialog.cs",
          "ZipContentsDialog.Designer.cs",
          "FolderBrowserDialogEx.cs"
        }
      },
      new ZipFile.ExtractorSettings()
      {
        Flavor = SelfExtractorFlavor.ConsoleApplication,
        ReferencedAssemblies = new List<string>()
        {
          "System.dll"
        },
        CopyThroughResources = (List<string>) null,
        ResourcesToCompile = new List<string>()
        {
          "CommandLineSelfExtractorStub.cs"
        }
      }
    };

    public ZipEntry AddItem(string fileOrDirectoryName) => this.AddItem(fileOrDirectoryName, (string) null);

    public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
    {
      if (File.Exists(fileOrDirectoryName))
        return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
      return Directory.Exists(fileOrDirectoryName) ? this.AddDirectory(fileOrDirectoryName, directoryPathInArchive) : throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", (object) fileOrDirectoryName));
    }

    public ZipEntry AddFile(string fileName) => this.AddFile(fileName, (string) null);

    public ZipEntry AddFile(string fileName, string directoryPathInArchive)
    {
      string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
      ZipEntry fromFile = ZipEntry.CreateFromFile(fileName, nameInArchive);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("adding {0}...", (object) fileName);
      return this._InternalAddEntry(fromFile);
    }

    public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
    {
      if (entriesToRemove == null)
        throw new ArgumentNullException(nameof (entriesToRemove));
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) entriesToRemove)
        this.RemoveEntry(entry);
    }

    public void RemoveEntries(ICollection<string> entriesToRemove)
    {
      if (entriesToRemove == null)
        throw new ArgumentNullException(nameof (entriesToRemove));
      foreach (string fileName in (IEnumerable<string>) entriesToRemove)
        this.RemoveEntry(fileName);
    }

    public void AddFiles(IEnumerable<string> fileNames) => this.AddFiles(fileNames, (string) null);

    public void UpdateFiles(IEnumerable<string> fileNames) => this.UpdateFiles(fileNames, (string) null);

    public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive) => this.AddFiles(fileNames, false, directoryPathInArchive);

    public void AddFiles(
      IEnumerable<string> fileNames,
      bool preserveDirHierarchy,
      string directoryPathInArchive)
    {
      if (fileNames == null)
        throw new ArgumentNullException(nameof (fileNames));
      this._addOperationCanceled = false;
      this.OnAddStarted();
      if (preserveDirHierarchy)
      {
        foreach (string fileName in fileNames)
        {
          if (!this._addOperationCanceled)
          {
            if (directoryPathInArchive != null)
            {
              string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(fileName)));
              this.AddFile(fileName, fullPath);
            }
            else
              this.AddFile(fileName, (string) null);
          }
          else
            break;
        }
      }
      else
      {
        foreach (string fileName in fileNames)
        {
          if (!this._addOperationCanceled)
            this.AddFile(fileName, directoryPathInArchive);
          else
            break;
        }
      }
      if (this._addOperationCanceled)
        return;
      this.OnAddCompleted();
    }

    public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
    {
      if (fileNames == null)
        throw new ArgumentNullException(nameof (fileNames));
      this.OnAddStarted();
      foreach (string fileName in fileNames)
        this.UpdateFile(fileName, directoryPathInArchive);
      this.OnAddCompleted();
    }

    public ZipEntry UpdateFile(string fileName) => this.UpdateFile(fileName, (string) null);

    public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
    {
      string fileName1 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
      if (this[fileName1] != null)
        this.RemoveEntry(fileName1);
      return this.AddFile(fileName, directoryPathInArchive);
    }

    public ZipEntry UpdateDirectory(string directoryName) => this.UpdateDirectory(directoryName, (string) null);

    public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive) => this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);

    public void UpdateItem(string itemName) => this.UpdateItem(itemName, (string) null);

    public void UpdateItem(string itemName, string directoryPathInArchive)
    {
      if (File.Exists(itemName))
      {
        this.UpdateFile(itemName, directoryPathInArchive);
      }
      else
      {
        if (!Directory.Exists(itemName))
          throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", (object) itemName));
        this.UpdateDirectory(itemName, directoryPathInArchive);
      }
    }

    public ZipEntry AddEntry(string entryName, string content) => this.AddEntry(entryName, content, Encoding.Default);

    public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
    {
      MemoryStream memoryStream = new MemoryStream();
      StreamWriter streamWriter = new StreamWriter((Stream) memoryStream, encoding);
      streamWriter.Write(content);
      streamWriter.Flush();
      memoryStream.Seek(0L, SeekOrigin.Begin);
      return this.AddEntry(entryName, (Stream) memoryStream);
    }

    public ZipEntry AddEntry(string entryName, Stream stream)
    {
      ZipEntry forStream = ZipEntry.CreateForStream(entryName, stream);
      forStream.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("adding {0}...", (object) entryName);
      return this._InternalAddEntry(forStream);
    }

    public ZipEntry AddEntry(string entryName, WriteDelegate writer)
    {
      ZipEntry forWriter = ZipEntry.CreateForWriter(entryName, writer);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("adding {0}...", (object) entryName);
      return this._InternalAddEntry(forWriter);
    }

    public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
    {
      ZipEntry jitStreamProvider = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
      jitStreamProvider.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("adding {0}...", (object) entryName);
      return this._InternalAddEntry(jitStreamProvider);
    }

    private ZipEntry _InternalAddEntry(ZipEntry ze)
    {
      ze._container = new ZipContainer((object) this);
      ze.CompressionMethod = this.CompressionMethod;
      ze.CompressionLevel = this.CompressionLevel;
      ze.ExtractExistingFile = this.ExtractExistingFile;
      ze.ZipErrorAction = this.ZipErrorAction;
      ze.SetCompression = this.SetCompression;
      ze.AlternateEncoding = this.AlternateEncoding;
      ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
      ze.Password = this._Password;
      ze.Encryption = this.Encryption;
      ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
      ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
      this.InternalAddEntry(ze.FileName, ze);
      this.AfterAddEntry(ze);
      return ze;
    }

    public ZipEntry UpdateEntry(string entryName, string content) => this.UpdateEntry(entryName, content, Encoding.Default);

    public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
    {
      this.RemoveEntryForUpdate(entryName);
      return this.AddEntry(entryName, content, encoding);
    }

    public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
    {
      this.RemoveEntryForUpdate(entryName);
      return this.AddEntry(entryName, writer);
    }

    public ZipEntry UpdateEntry(
      string entryName,
      OpenDelegate opener,
      CloseDelegate closer)
    {
      this.RemoveEntryForUpdate(entryName);
      return this.AddEntry(entryName, opener, closer);
    }

    public ZipEntry UpdateEntry(string entryName, Stream stream)
    {
      this.RemoveEntryForUpdate(entryName);
      return this.AddEntry(entryName, stream);
    }

    private void RemoveEntryForUpdate(string entryName)
    {
      if (string.IsNullOrEmpty(entryName))
        throw new ArgumentNullException(nameof (entryName));
      string directoryPathInArchive = (string) null;
      if (entryName.IndexOf('\\') != -1)
      {
        directoryPathInArchive = Path.GetDirectoryName(entryName);
        entryName = Path.GetFileName(entryName);
      }
      string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
      if (this[fileName] == null)
        return;
      this.RemoveEntry(fileName);
    }

    public ZipEntry AddEntry(string entryName, byte[] byteContent)
    {
      MemoryStream memoryStream = byteContent != null ? new MemoryStream(byteContent) : throw new ArgumentException("bad argument", nameof (byteContent));
      return this.AddEntry(entryName, (Stream) memoryStream);
    }

    public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
    {
      this.RemoveEntryForUpdate(entryName);
      return this.AddEntry(entryName, byteContent);
    }

    public ZipEntry AddDirectory(string directoryName) => this.AddDirectory(directoryName, (string) null);

    public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive) => this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);

    public ZipEntry AddDirectoryByName(string directoryNameInArchive)
    {
      ZipEntry fromNothing = ZipEntry.CreateFromNothing(directoryNameInArchive);
      fromNothing._container = new ZipContainer((object) this);
      fromNothing.MarkAsDirectory();
      fromNothing.AlternateEncoding = this.AlternateEncoding;
      fromNothing.AlternateEncodingUsage = this.AlternateEncodingUsage;
      fromNothing.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
      fromNothing.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
      fromNothing.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
      fromNothing._Source = ZipEntrySource.Stream;
      this.InternalAddEntry(fromNothing.FileName, fromNothing);
      this.AfterAddEntry(fromNothing);
      return fromNothing;
    }

    private ZipEntry AddOrUpdateDirectoryImpl(
      string directoryName,
      string rootDirectoryPathInArchive,
      AddOrUpdateAction action)
    {
      if (rootDirectoryPathInArchive == null)
        rootDirectoryPathInArchive = "";
      return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
    }

    internal void InternalAddEntry(string name, ZipEntry entry)
    {
      this._entries.Add(name, entry);
      this._zipEntriesAsList = (List<ZipEntry>) null;
      this._contentsChanged = true;
    }

    private ZipEntry AddOrUpdateDirectoryImpl(
      string directoryName,
      string rootDirectoryPathInArchive,
      AddOrUpdateAction action,
      bool recurse,
      int level)
    {
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("{0} {1}...", action == AddOrUpdateAction.AddOnly ? (object) "adding" : (object) "Adding or updating", (object) directoryName);
      if (level == 0)
      {
        this._addOperationCanceled = false;
        this.OnAddStarted();
      }
      if (this._addOperationCanceled)
        return (ZipEntry) null;
      string str = rootDirectoryPathInArchive;
      ZipEntry entry = (ZipEntry) null;
      if (level > 0)
      {
        int num = directoryName.Length;
        for (int index = level; index > 0; --index)
          num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
        string path2 = directoryName.Substring(num + 1);
        str = Path.Combine(rootDirectoryPathInArchive, path2);
      }
      if (level > 0 || rootDirectoryPathInArchive != "")
      {
        entry = ZipEntry.CreateFromFile(directoryName, str);
        entry._container = new ZipContainer((object) this);
        entry.AlternateEncoding = this.AlternateEncoding;
        entry.AlternateEncodingUsage = this.AlternateEncodingUsage;
        entry.MarkAsDirectory();
        entry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
        entry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
        if (!this._entries.ContainsKey(entry.FileName))
        {
          this.InternalAddEntry(entry.FileName, entry);
          this.AfterAddEntry(entry);
        }
        str = entry.FileName;
      }
      if (!this._addOperationCanceled)
      {
        string[] files = Directory.GetFiles(directoryName);
        if (recurse)
        {
          foreach (string fileName in files)
          {
            if (!this._addOperationCanceled)
            {
              if (action == AddOrUpdateAction.AddOnly)
                this.AddFile(fileName, str);
              else
                this.UpdateFile(fileName, str);
            }
            else
              break;
          }
          if (!this._addOperationCanceled)
          {
            foreach (string directory in Directory.GetDirectories(directoryName))
            {
              FileAttributes attributes = File.GetAttributes(directory);
              if (this.AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == (FileAttributes) 0)
                this.AddOrUpdateDirectoryImpl(directory, rootDirectoryPathInArchive, action, recurse, level + 1);
            }
          }
        }
      }
      if (level == 0)
        this.OnAddCompleted();
      return entry;
    }

    public static bool CheckZip(string zipFileName) => ZipFile.CheckZip(zipFileName, false, (TextWriter) null);

    public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
    {
      ZipFile zipFile1 = (ZipFile) null;
      ZipFile zipFile2 = (ZipFile) null;
      bool flag = true;
      try
      {
        zipFile1 = new ZipFile();
        zipFile1.FullScan = true;
        zipFile1.Initialize(zipFileName);
        zipFile2 = ZipFile.Read(zipFileName);
        foreach (ZipEntry zipEntry1 in zipFile1)
        {
          foreach (ZipEntry zipEntry2 in zipFile2)
          {
            if (zipEntry1.FileName == zipEntry2.FileName)
            {
              if (zipEntry1._RelativeOffsetOfLocalHeader != zipEntry2._RelativeOffsetOfLocalHeader)
              {
                flag = false;
                writer?.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", (object) zipEntry1.FileName, (object) zipEntry1._RelativeOffsetOfLocalHeader, (object) zipEntry2._RelativeOffsetOfLocalHeader);
              }
              if (zipEntry1._CompressedSize != zipEntry2._CompressedSize)
              {
                flag = false;
                writer?.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", (object) zipEntry1.FileName, (object) zipEntry1._CompressedSize, (object) zipEntry2._CompressedSize);
              }
              if (zipEntry1._UncompressedSize != zipEntry2._UncompressedSize)
              {
                flag = false;
                writer?.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", (object) zipEntry1.FileName, (object) zipEntry1._UncompressedSize, (object) zipEntry2._UncompressedSize);
              }
              if (zipEntry1.CompressionMethod != zipEntry2.CompressionMethod)
              {
                flag = false;
                writer?.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", (object) zipEntry1.FileName, (object) zipEntry1.CompressionMethod, (object) zipEntry2.CompressionMethod);
              }
              if (zipEntry1.Crc != zipEntry2.Crc)
              {
                flag = false;
                writer?.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", (object) zipEntry1.FileName, (object) zipEntry1.Crc, (object) zipEntry2.Crc);
                break;
              }
              break;
            }
          }
        }
        zipFile2.Dispose();
        zipFile2 = (ZipFile) null;
        if (!flag & fixIfNecessary)
        {
          string fileName = string.Format("{0}_fixed.zip", (object) Path.GetFileNameWithoutExtension(zipFileName));
          zipFile1.Save(fileName);
        }
      }
      finally
      {
        zipFile1?.Dispose();
        zipFile2?.Dispose();
      }
      return flag;
    }

    public static void FixZipDirectory(string zipFileName)
    {
      using (ZipFile zipFile = new ZipFile())
      {
        zipFile.FullScan = true;
        zipFile.Initialize(zipFileName);
        zipFile.Save(zipFileName);
      }
    }

    public static bool CheckZipPassword(string zipFileName, string password)
    {
      bool flag = false;
      try
      {
        using (ZipFile zipFile = ZipFile.Read(zipFileName))
        {
          foreach (ZipEntry zipEntry in zipFile)
          {
            if (!zipEntry.IsDirectory && zipEntry.UsesEncryption)
              zipEntry.ExtractWithPassword(Stream.Null, password);
          }
        }
        flag = true;
      }
      catch (BadPasswordException ex)
      {
      }
      return flag;
    }

    public string Info
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(string.Format("          ZipFile: {0}\n", (object) this.Name));
        if (!string.IsNullOrEmpty(this._Comment))
          stringBuilder.Append(string.Format("          Comment: {0}\n", (object) this._Comment));
        if (this._versionMadeBy > (ushort) 0)
          stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", (object) this._versionMadeBy));
        if (this._versionNeededToExtract > (ushort) 0)
          stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", (object) this._versionNeededToExtract));
        stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", (object) this.InputUsesZip64));
        stringBuilder.Append(string.Format("     disk with CD: {0}\n", (object) this._diskNumberWithCd));
        if (this._OffsetOfCentralDirectory == uint.MaxValue)
          stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", (object) this._OffsetOfCentralDirectory64));
        else
          stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", (object) this._OffsetOfCentralDirectory));
        stringBuilder.Append("\n");
        foreach (ZipEntry zipEntry in this._entries.Values)
          stringBuilder.Append(zipEntry.Info);
        return stringBuilder.ToString();
      }
    }

    public bool FullScan { get; set; }

    public bool SortEntriesBeforeSaving { get; set; }

    public bool AddDirectoryWillTraverseReparsePoints { get; set; }

    public int BufferSize
    {
      get => this._BufferSize;
      set => this._BufferSize = value;
    }

    public int CodecBufferSize { get; set; }

    public bool FlattenFoldersOnExtract { get; set; }

    public CompressionStrategy Strategy
    {
      get => this._Strategy;
      set => this._Strategy = value;
    }

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public CompressionLevel CompressionLevel { get; set; }

    public CompressionMethod CompressionMethod
    {
      get => this._compressionMethod;
      set => this._compressionMethod = value;
    }

    public string Comment
    {
      get => this._Comment;
      set
      {
        this._Comment = value;
        this._contentsChanged = true;
      }
    }

    public bool EmitTimesInWindowsFormatWhenSaving
    {
      get => this._emitNtfsTimes;
      set => this._emitNtfsTimes = value;
    }

    public bool EmitTimesInUnixFormatWhenSaving
    {
      get => this._emitUnixTimes;
      set => this._emitUnixTimes = value;
    }

    internal bool Verbose => this._StatusMessageTextWriter != null;

    public bool ContainsEntry(string name) => this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));

    public bool CaseSensitiveRetrieval
    {
      get => this._CaseSensitiveRetrieval;
      set
      {
        if (value == this._CaseSensitiveRetrieval)
          return;
        this._CaseSensitiveRetrieval = value;
        this._initEntriesDictionary();
      }
    }

    [Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
    public bool UseUnicodeAsNecessary
    {
      get => this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
      set
      {
        if (value)
        {
          this._alternateEncoding = Encoding.GetEncoding("UTF-8");
          this._alternateEncodingUsage = ZipOption.AsNecessary;
        }
        else
        {
          this._alternateEncoding = ZipFile.DefaultEncoding;
          this._alternateEncodingUsage = ZipOption.Default;
        }
      }
    }

    public Zip64Option UseZip64WhenSaving
    {
      get => this._zip64;
      set => this._zip64 = value;
    }

    public bool? RequiresZip64
    {
      get
      {
        if (this._entries.Count > 65534)
          return new bool?(true);
        if (!this._hasBeenSaved || this._contentsChanged)
          return new bool?();
        foreach (ZipEntry zipEntry in this._entries.Values)
        {
          if (zipEntry.RequiresZip64.Value)
            return new bool?(true);
        }
        return new bool?(false);
      }
    }

    public bool? OutputUsedZip64 => this._OutputUsesZip64;

    public bool? InputUsesZip64
    {
      get
      {
        if (this._entries.Count > 65534)
          return new bool?(true);
        foreach (ZipEntry zipEntry in this)
        {
          if (zipEntry.Source != ZipEntrySource.ZipFile)
            return new bool?();
          if (zipEntry._InputUsesZip64)
            return new bool?(true);
        }
        return new bool?(false);
      }
    }

    [Obsolete("use AlternateEncoding instead.")]
    public Encoding ProvisionalAlternateEncoding
    {
      get => this._alternateEncodingUsage == ZipOption.AsNecessary ? this._alternateEncoding : (Encoding) null;
      set
      {
        this._alternateEncoding = value;
        this._alternateEncodingUsage = ZipOption.AsNecessary;
      }
    }

    public Encoding AlternateEncoding
    {
      get => this._alternateEncoding;
      set => this._alternateEncoding = value;
    }

    public ZipOption AlternateEncodingUsage
    {
      get => this._alternateEncodingUsage;
      set => this._alternateEncodingUsage = value;
    }

    public static Encoding DefaultEncoding => ZipFile._defaultEncoding;

    public TextWriter StatusMessageTextWriter
    {
      get => this._StatusMessageTextWriter;
      set => this._StatusMessageTextWriter = value;
    }

    public string TempFileFolder
    {
      get => this._TempFileFolder;
      set
      {
        this._TempFileFolder = value;
        if (value != null && !Directory.Exists(value))
          throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", (object) value));
      }
    }

    public string Password
    {
      set
      {
        this._Password = value;
        if (this._Password == null)
        {
          this.Encryption = EncryptionAlgorithm.None;
        }
        else
        {
          if (this.Encryption != EncryptionAlgorithm.None)
            return;
          this.Encryption = EncryptionAlgorithm.PkzipWeak;
        }
      }
      private get => this._Password;
    }

    public ExtractExistingFileAction ExtractExistingFile { get; set; }

    public ZipErrorAction ZipErrorAction
    {
      get
      {
        if (this.ZipError != null)
          this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
        return this._zipErrorAction;
      }
      set
      {
        this._zipErrorAction = value;
        if (this._zipErrorAction == ZipErrorAction.InvokeErrorEvent || this.ZipError == null)
          return;
        this.ZipError = (EventHandler<ZipErrorEventArgs>) null;
      }
    }

    public EncryptionAlgorithm Encryption
    {
      get => this._Encryption;
      set => this._Encryption = value != EncryptionAlgorithm.Unsupported ? value : throw new InvalidOperationException("You may not set Encryption to that value.");
    }

    public SetCompressionCallback SetCompression { get; set; }

    public int MaxOutputSegmentSize
    {
      get => this._maxOutputSegmentSize;
      set => this._maxOutputSegmentSize = value >= 65536 || (uint) value <= 0U ? value : throw new ZipException("The minimum acceptable segment size is 65536.");
    }

    public int NumberOfSegmentsForMostRecentSave => (int) this._numberOfSegmentsForMostRecentSave + 1;

    public long ParallelDeflateThreshold
    {
      set => this._ParallelDeflateThreshold = value == 0L || value == -1L || value >= 65536L ? value : throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
      get => this._ParallelDeflateThreshold;
    }

    public int ParallelDeflateMaxBufferPairs
    {
      get => this._maxBufferPairs;
      set => this._maxBufferPairs = value >= 4 ? value : throw new ArgumentOutOfRangeException(nameof (ParallelDeflateMaxBufferPairs), "Value must be 4 or greater.");
    }

    public override string ToString() => string.Format("ZipFile::{0}", (object) this.Name);

    public static Version LibraryVersion => Assembly.GetExecutingAssembly().GetName().Version;

    internal void NotifyEntryChanged() => this._contentsChanged = true;

    internal Stream StreamForDiskNumber(uint diskNumber) => (int) diskNumber + 1 == (int) this._diskNumberWithCd || diskNumber == 0U && this._diskNumberWithCd == 0U ? this.ReadStream : (Stream) ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);

    internal void Reset(bool whileSaving)
    {
      if (!this._JustSaved)
        return;
      using (ZipFile zf = new ZipFile())
      {
        zf._readName = zf._name = whileSaving ? this._readName ?? this._name : this._name;
        zf.AlternateEncoding = this.AlternateEncoding;
        zf.AlternateEncodingUsage = this.AlternateEncodingUsage;
        ZipFile.ReadIntoInstance(zf);
        foreach (ZipEntry source in zf)
        {
          foreach (ZipEntry zipEntry in this)
          {
            if (source.FileName == zipEntry.FileName)
            {
              zipEntry.CopyMetaData(source);
              break;
            }
          }
        }
      }
      this._JustSaved = false;
    }

    public ZipFile(string fileName)
    {
      try
      {
        this._InitInstance(fileName, (TextWriter) null);
      }
      catch (Exception ex)
      {
        throw new ZipException(string.Format("Could not read {0} as a zip file", (object) fileName), ex);
      }
    }

    public ZipFile(string fileName, Encoding encoding)
    {
      try
      {
        this.AlternateEncoding = encoding;
        this.AlternateEncodingUsage = ZipOption.Always;
        this._InitInstance(fileName, (TextWriter) null);
      }
      catch (Exception ex)
      {
        throw new ZipException(string.Format("{0} is not a valid zip file", (object) fileName), ex);
      }
    }

    public ZipFile() => this._InitInstance((string) null, (TextWriter) null);

    public ZipFile(Encoding encoding)
    {
      this.AlternateEncoding = encoding;
      this.AlternateEncodingUsage = ZipOption.Always;
      this._InitInstance((string) null, (TextWriter) null);
    }

    public ZipFile(string fileName, TextWriter statusMessageWriter)
    {
      try
      {
        this._InitInstance(fileName, statusMessageWriter);
      }
      catch (Exception ex)
      {
        throw new ZipException(string.Format("{0} is not a valid zip file", (object) fileName), ex);
      }
    }

    public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
    {
      try
      {
        this.AlternateEncoding = encoding;
        this.AlternateEncodingUsage = ZipOption.Always;
        this._InitInstance(fileName, statusMessageWriter);
      }
      catch (Exception ex)
      {
        throw new ZipException(string.Format("{0} is not a valid zip file", (object) fileName), ex);
      }
    }

    public void Initialize(string fileName)
    {
      try
      {
        this._InitInstance(fileName, (TextWriter) null);
      }
      catch (Exception ex)
      {
        throw new ZipException(string.Format("{0} is not a valid zip file", (object) fileName), ex);
      }
    }

    private void _initEntriesDictionary()
    {
      StringComparer stringComparer = this.CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
      this._entries = this._entries == null ? new Dictionary<string, ZipEntry>((IEqualityComparer<string>) stringComparer) : new Dictionary<string, ZipEntry>((IDictionary<string, ZipEntry>) this._entries, (IEqualityComparer<string>) stringComparer);
    }

    private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
    {
      this._name = zipFileName;
      this._StatusMessageTextWriter = statusMessageWriter;
      this._contentsChanged = true;
      this.AddDirectoryWillTraverseReparsePoints = true;
      this.CompressionLevel = CompressionLevel.Default;
      this.ParallelDeflateThreshold = 524288L;
      this._initEntriesDictionary();
      if (!File.Exists(this._name))
        return;
      if (this.FullScan)
        ZipFile.ReadIntoInstance_Orig(this);
      else
        ZipFile.ReadIntoInstance(this);
      this._fileAlreadyExists = true;
    }

    private List<ZipEntry> ZipEntriesAsList
    {
      get
      {
        if (this._zipEntriesAsList == null)
          this._zipEntriesAsList = new List<ZipEntry>((IEnumerable<ZipEntry>) this._entries.Values);
        return this._zipEntriesAsList;
      }
    }

    public ZipEntry this[int ix] => this.ZipEntriesAsList[ix];

    public ZipEntry this[string fileName]
    {
      get
      {
        string key1 = SharedUtilities.NormalizePathForUseInZipFile(fileName);
        if (this._entries.ContainsKey(key1))
          return this._entries[key1];
        string key2 = key1.Replace("/", "\\");
        return this._entries.ContainsKey(key2) ? this._entries[key2] : (ZipEntry) null;
      }
    }

    public ICollection<string> EntryFileNames => (ICollection<string>) this._entries.Keys;

    public ICollection<ZipEntry> Entries => (ICollection<ZipEntry>) this._entries.Values;

    public ICollection<ZipEntry> EntriesSorted
    {
      get
      {
        List<ZipEntry> zipEntryList = new List<ZipEntry>();
        foreach (ZipEntry entry in (IEnumerable<ZipEntry>) this.Entries)
          zipEntryList.Add(entry);
        StringComparison sc = this.CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        zipEntryList.Sort((Comparison<ZipEntry>) ((x, y) => string.Compare(x.FileName, y.FileName, sc)));
        return (ICollection<ZipEntry>) zipEntryList.AsReadOnly();
      }
    }

    public int Count => this._entries.Count;

    public void RemoveEntry(ZipEntry entry)
    {
      if (entry == null)
        throw new ArgumentNullException(nameof (entry));
      this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
      this._zipEntriesAsList = (List<ZipEntry>) null;
      this._contentsChanged = true;
    }

    public void RemoveEntry(string fileName) => this.RemoveEntry(this[ZipEntry.NameInArchive(fileName, (string) null)] ?? throw new ArgumentException("The entry you specified was not found in the zip archive."));

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposeManagedResources)
    {
      if (this._disposed)
        return;
      if (disposeManagedResources)
      {
        if (this._ReadStreamIsOurs && this._readstream != null)
        {
          this._readstream.Dispose();
          this._readstream = (Stream) null;
        }
        if (this._temporaryFileName != null && this._name != null && this._writestream != null)
        {
          this._writestream.Dispose();
          this._writestream = (Stream) null;
        }
        if (this.ParallelDeflater != null)
        {
          this.ParallelDeflater.Dispose();
          this.ParallelDeflater = (ParallelDeflateOutputStream) null;
        }
      }
      this._disposed = true;
    }

    internal Stream ReadStream
    {
      get
      {
        if (this._readstream == null && (this._readName != null || this._name != null))
        {
          this._readstream = (Stream) File.Open(this._readName ?? this._name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
          this._ReadStreamIsOurs = true;
        }
        return this._readstream;
      }
    }

    private Stream WriteStream
    {
      get
      {
        if (this._writestream != null || this._name == null)
          return this._writestream;
        if ((uint) this._maxOutputSegmentSize > 0U)
        {
          this._writestream = (Stream) ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
          return this._writestream;
        }
        SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
        return this._writestream;
      }
      set
      {
        if (value != null)
          throw new ZipException("Cannot set the stream to a non-null value.");
        this._writestream = (Stream) null;
      }
    }

    private string ArchiveNameForEvent => this._name != null ? this._name : "(stream)";

    public event EventHandler<SaveProgressEventArgs> SaveProgress;

    internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
    {
      EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
      if (saveProgress != null)
      {
        SaveProgressEventArgs e = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
        saveProgress((object) this, e);
        if (e.Cancel)
          this._saveOperationCanceled = true;
      }
      return this._saveOperationCanceled;
    }

    private void OnSaveEntry(int current, ZipEntry entry, bool before)
    {
      EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
      if (saveProgress == null)
        return;
      SaveProgressEventArgs e = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
      saveProgress((object) this, e);
      if (e.Cancel)
        this._saveOperationCanceled = true;
    }

    private void OnSaveEvent(ZipProgressEventType eventFlavor)
    {
      EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
      if (saveProgress == null)
        return;
      SaveProgressEventArgs e = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
      saveProgress((object) this, e);
      if (e.Cancel)
        this._saveOperationCanceled = true;
    }

    private void OnSaveStarted()
    {
      EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
      if (saveProgress == null)
        return;
      SaveProgressEventArgs e = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
      saveProgress((object) this, e);
      if (e.Cancel)
        this._saveOperationCanceled = true;
    }

    private void OnSaveCompleted()
    {
      EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
      if (saveProgress == null)
        return;
      SaveProgressEventArgs e = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
      saveProgress((object) this, e);
    }

    public event EventHandler<ReadProgressEventArgs> ReadProgress;

    private void OnReadStarted()
    {
      EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
      if (readProgress == null)
        return;
      ReadProgressEventArgs e = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
      readProgress((object) this, e);
    }

    private void OnReadCompleted()
    {
      EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
      if (readProgress == null)
        return;
      ReadProgressEventArgs e = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
      readProgress((object) this, e);
    }

    internal void OnReadBytes(ZipEntry entry)
    {
      EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
      if (readProgress == null)
        return;
      ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
      readProgress((object) this, e);
    }

    internal void OnReadEntry(bool before, ZipEntry entry)
    {
      EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
      if (readProgress == null)
        return;
      ReadProgressEventArgs e = before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count);
      readProgress((object) this, e);
    }

    private long LengthOfReadStream
    {
      get
      {
        if (this._lengthOfReadStream == -99L)
          this._lengthOfReadStream = this._ReadStreamIsOurs ? SharedUtilities.GetFileLength(this._name) : -1L;
        return this._lengthOfReadStream;
      }
    }

    public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

    private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress == null)
        return;
      ExtractProgressEventArgs e = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
      extractProgress((object) this, e);
      if (e.Cancel)
        this._extractOperationCanceled = true;
    }

    internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress != null)
      {
        ExtractProgressEventArgs e = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
        extractProgress((object) this, e);
        if (e.Cancel)
          this._extractOperationCanceled = true;
      }
      return this._extractOperationCanceled;
    }

    internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress != null)
      {
        ExtractProgressEventArgs e = before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path);
        extractProgress((object) this, e);
        if (e.Cancel)
          this._extractOperationCanceled = true;
      }
      return this._extractOperationCanceled;
    }

    internal bool OnExtractExisting(ZipEntry entry, string path)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress != null)
      {
        ExtractProgressEventArgs existing = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
        extractProgress((object) this, existing);
        if (existing.Cancel)
          this._extractOperationCanceled = true;
      }
      return this._extractOperationCanceled;
    }

    private void OnExtractAllCompleted(string path)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress == null)
        return;
      ExtractProgressEventArgs allCompleted = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
      extractProgress((object) this, allCompleted);
    }

    private void OnExtractAllStarted(string path)
    {
      EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
      if (extractProgress == null)
        return;
      ExtractProgressEventArgs allStarted = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
      extractProgress((object) this, allStarted);
    }

    public event EventHandler<AddProgressEventArgs> AddProgress;

    private void OnAddStarted()
    {
      EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
      if (addProgress == null)
        return;
      AddProgressEventArgs e = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
      addProgress((object) this, e);
      if (e.Cancel)
        this._addOperationCanceled = true;
    }

    private void OnAddCompleted()
    {
      EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
      if (addProgress == null)
        return;
      AddProgressEventArgs e = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
      addProgress((object) this, e);
    }

    internal void AfterAddEntry(ZipEntry entry)
    {
      EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
      if (addProgress == null)
        return;
      AddProgressEventArgs e = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
      addProgress((object) this, e);
      if (e.Cancel)
        this._addOperationCanceled = true;
    }

    public event EventHandler<ZipErrorEventArgs> ZipError;

    internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
    {
      if (this.ZipError != null)
      {
        lock (this.LOCK)
        {
          ZipErrorEventArgs e = ZipErrorEventArgs.Saving(this.Name, entry, exc);
          this.ZipError((object) this, e);
          if (e.Cancel)
            this._saveOperationCanceled = true;
        }
      }
      return this._saveOperationCanceled;
    }

    public void ExtractAll(string path) => this._InternalExtractAll(path, true);

    public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
    {
      this.ExtractExistingFile = extractExistingFile;
      this._InternalExtractAll(path, true);
    }

    private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
    {
      bool flag = this.Verbose;
      this._inExtractAll = true;
      try
      {
        this.OnExtractAllStarted(path);
        int current = 0;
        foreach (ZipEntry currentEntry in this._entries.Values)
        {
          if (flag)
          {
            this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", (object) "Name", (object) "Modified", (object) "Size", (object) "Ratio", (object) "Packed");
            this.StatusMessageTextWriter.WriteLine(new string('-', 72));
            flag = false;
          }
          if (this.Verbose)
          {
            this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", (object) currentEntry.FileName, (object) currentEntry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), (object) currentEntry.UncompressedSize, (object) currentEntry.CompressionRatio, (object) currentEntry.CompressedSize);
            if (!string.IsNullOrEmpty(currentEntry.Comment))
              this.StatusMessageTextWriter.WriteLine("  Comment: {0}", (object) currentEntry.Comment);
          }
          currentEntry.Password = this._Password;
          this.OnExtractEntry(current, true, currentEntry, path);
          if (overrideExtractExistingProperty)
            currentEntry.ExtractExistingFile = this.ExtractExistingFile;
          currentEntry.Extract(path);
          ++current;
          this.OnExtractEntry(current, false, currentEntry, path);
          if (this._extractOperationCanceled)
            break;
        }
        if (this._extractOperationCanceled)
          return;
        foreach (ZipEntry zipEntry in this._entries.Values)
        {
          if (zipEntry.IsDirectory || zipEntry.FileName.EndsWith("/"))
          {
            string fileOrDirectory = zipEntry.FileName.StartsWith("/") ? Path.Combine(path, zipEntry.FileName.Substring(1)) : Path.Combine(path, zipEntry.FileName);
            zipEntry._SetTimes(fileOrDirectory, false);
          }
        }
        this.OnExtractAllCompleted(path);
      }
      finally
      {
        this._inExtractAll = false;
      }
    }

    public static ZipFile Read(string fileName) => ZipFile.Read(fileName, (TextWriter) null, (Encoding) null, (EventHandler<ReadProgressEventArgs>) null);

    public static ZipFile Read(string fileName, ReadOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
    }

    private static ZipFile Read(
      string fileName,
      TextWriter statusMessageWriter,
      Encoding encoding,
      EventHandler<ReadProgressEventArgs> readProgress)
    {
      ZipFile zf = new ZipFile();
      zf.AlternateEncoding = encoding ?? ZipFile.DefaultEncoding;
      zf.AlternateEncodingUsage = ZipOption.Always;
      zf._StatusMessageTextWriter = statusMessageWriter;
      zf._name = fileName;
      if (readProgress != null)
        zf.ReadProgress = readProgress;
      if (zf.Verbose)
        zf._StatusMessageTextWriter.WriteLine("reading from {0}...", (object) fileName);
      ZipFile.ReadIntoInstance(zf);
      zf._fileAlreadyExists = true;
      return zf;
    }

    public static ZipFile Read(Stream zipStream) => ZipFile.Read(zipStream, (TextWriter) null, (Encoding) null, (EventHandler<ReadProgressEventArgs>) null);

    public static ZipFile Read(Stream zipStream, ReadOptions options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
    }

    private static ZipFile Read(
      Stream zipStream,
      TextWriter statusMessageWriter,
      Encoding encoding,
      EventHandler<ReadProgressEventArgs> readProgress)
    {
      if (zipStream == null)
        throw new ArgumentNullException(nameof (zipStream));
      ZipFile zf = new ZipFile();
      zf._StatusMessageTextWriter = statusMessageWriter;
      zf._alternateEncoding = encoding ?? ZipFile.DefaultEncoding;
      zf._alternateEncodingUsage = ZipOption.Always;
      if (readProgress != null)
        zf.ReadProgress += readProgress;
      zf._readstream = zipStream.Position == 0L ? zipStream : (Stream) new OffsetStream(zipStream);
      zf._ReadStreamIsOurs = false;
      if (zf.Verbose)
        zf._StatusMessageTextWriter.WriteLine("reading from stream...");
      ZipFile.ReadIntoInstance(zf);
      return zf;
    }

    private static void ReadIntoInstance(ZipFile zf)
    {
      Stream readStream = zf.ReadStream;
      try
      {
        zf._readName = zf._name;
        if (!readStream.CanSeek)
        {
          ZipFile.ReadIntoInstance_Orig(zf);
          return;
        }
        zf.OnReadStarted();
        if (ZipFile.ReadFirstFourBytes(readStream) == 101010256U)
          return;
        int num1 = 0;
        bool flag = false;
        long offset = readStream.Length - 64L;
        long num2 = Math.Max(readStream.Length - 16384L, 10L);
        do
        {
          if (offset < 0L)
            offset = 0L;
          readStream.Seek(offset, SeekOrigin.Begin);
          if (SharedUtilities.FindSignature(readStream, 101010256) != -1L)
            flag = true;
          else if (offset != 0L)
          {
            ++num1;
            offset -= (long) (32 * (num1 + 1) * num1);
          }
          else
            break;
        }
        while (!flag && offset > num2);
        if (flag)
        {
          zf._locEndOfCDS = readStream.Position - 4L;
          byte[] buffer = new byte[16];
          readStream.Read(buffer, 0, buffer.Length);
          zf._diskNumberWithCd = (uint) BitConverter.ToUInt16(buffer, 2);
          if (zf._diskNumberWithCd == (uint) ushort.MaxValue)
            throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
          ++zf._diskNumberWithCd;
          int startIndex = 12;
          uint uint32 = BitConverter.ToUInt32(buffer, startIndex);
          if (uint32 == uint.MaxValue)
          {
            ZipFile.Zip64SeekToCentralDirectory(zf);
          }
          else
          {
            zf._OffsetOfCentralDirectory = uint32;
            readStream.Seek((long) uint32, SeekOrigin.Begin);
          }
          ZipFile.ReadCentralDirectory(zf);
        }
        else
        {
          readStream.Seek(0L, SeekOrigin.Begin);
          ZipFile.ReadIntoInstance_Orig(zf);
        }
      }
      catch (Exception ex)
      {
        if (zf._ReadStreamIsOurs && zf._readstream != null)
        {
          try
          {
            zf._readstream.Dispose();
            zf._readstream = (Stream) null;
          }
          finally
          {
          }
        }
        throw new ZipException("Cannot read that as a ZipFile", ex);
      }
      zf._contentsChanged = false;
    }

    private static void Zip64SeekToCentralDirectory(ZipFile zf)
    {
      Stream readStream = zf.ReadStream;
      byte[] buffer1 = new byte[16];
      readStream.Seek(-40L, SeekOrigin.Current);
      readStream.Read(buffer1, 0, 16);
      long int64_1 = BitConverter.ToInt64(buffer1, 8);
      zf._OffsetOfCentralDirectory = uint.MaxValue;
      zf._OffsetOfCentralDirectory64 = int64_1;
      readStream.Seek(int64_1, SeekOrigin.Begin);
      uint num = (uint) SharedUtilities.ReadInt(readStream);
      if (num != 101075792U)
        throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", (object) num, (object) readStream.Position));
      readStream.Read(buffer1, 0, 8);
      byte[] buffer2 = new byte[BitConverter.ToInt64(buffer1, 0)];
      readStream.Read(buffer2, 0, buffer2.Length);
      long int64_2 = BitConverter.ToInt64(buffer2, 36);
      readStream.Seek(int64_2, SeekOrigin.Begin);
    }

    private static uint ReadFirstFourBytes(Stream s) => (uint) SharedUtilities.ReadInt(s);

    private static void ReadCentralDirectory(ZipFile zf)
    {
      bool flag = false;
      Dictionary<string, object> previouslySeen = new Dictionary<string, object>();
      ZipEntry zipEntry;
      while ((zipEntry = ZipEntry.ReadDirEntry(zf, previouslySeen)) != null)
      {
        zipEntry.ResetDirEntry();
        zf.OnReadEntry(true, (ZipEntry) null);
        if (zf.Verbose)
          zf.StatusMessageTextWriter.WriteLine("entry {0}", (object) zipEntry.FileName);
        zf._entries.Add(zipEntry.FileName, zipEntry);
        if (zipEntry._InputUsesZip64)
          flag = true;
        previouslySeen.Add(zipEntry.FileName, (object) null);
      }
      if (flag)
        zf.UseZip64WhenSaving = Zip64Option.Always;
      if (zf._locEndOfCDS > 0L)
        zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
      ZipFile.ReadCentralDirectoryFooter(zf);
      if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
        zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", (object) zf.Comment);
      if (zf.Verbose)
        zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", (object) zf._entries.Count);
      zf.OnReadCompleted();
    }

    private static void ReadIntoInstance_Orig(ZipFile zf)
    {
      zf.OnReadStarted();
      zf._entries = new Dictionary<string, ZipEntry>();
      if (zf.Verbose)
      {
        if (zf.Name == null)
          zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
        else
          zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", (object) zf.Name);
      }
      bool first = true;
      ZipEntry zipEntry1;
      for (ZipContainer zc = new ZipContainer((object) zf); (zipEntry1 = ZipEntry.ReadEntry(zc, first)) != null; first = false)
      {
        if (zf.Verbose)
          zf.StatusMessageTextWriter.WriteLine("  {0}", (object) zipEntry1.FileName);
        zf._entries.Add(zipEntry1.FileName, zipEntry1);
      }
      try
      {
        Dictionary<string, object> previouslySeen = new Dictionary<string, object>();
        ZipEntry zipEntry2;
        while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, previouslySeen)) != null)
        {
          ZipEntry entry = zf._entries[zipEntry2.FileName];
          if (entry != null)
          {
            entry._Comment = zipEntry2.Comment;
            if (zipEntry2.IsDirectory)
              entry.MarkAsDirectory();
          }
          previouslySeen.Add(zipEntry2.FileName, (object) null);
        }
        if (zf._locEndOfCDS > 0L)
          zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
        ZipFile.ReadCentralDirectoryFooter(zf);
        if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
          zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", (object) zf.Comment);
      }
      catch (ZipException ex)
      {
      }
      catch (IOException ex)
      {
      }
      zf.OnReadCompleted();
    }

    private static void ReadCentralDirectoryFooter(ZipFile zf)
    {
      Stream readStream = zf.ReadStream;
      int num1 = SharedUtilities.ReadSignature(readStream);
      int startIndex1 = 0;
      if (num1 == 101075792)
      {
        byte[] buffer1 = new byte[52];
        readStream.Read(buffer1, 0, buffer1.Length);
        long int64 = BitConverter.ToInt64(buffer1, 0);
        if (int64 < 44L)
          throw new ZipException("Bad size in the ZIP64 Central Directory.");
        zf._versionMadeBy = BitConverter.ToUInt16(buffer1, startIndex1);
        int startIndex2 = startIndex1 + 2;
        zf._versionNeededToExtract = BitConverter.ToUInt16(buffer1, startIndex2);
        int startIndex3 = startIndex2 + 2;
        zf._diskNumberWithCd = BitConverter.ToUInt32(buffer1, startIndex3);
        int num2 = startIndex3 + 2;
        byte[] buffer2 = new byte[int64 - 44L];
        readStream.Read(buffer2, 0, buffer2.Length);
        if (SharedUtilities.ReadSignature(readStream) != 117853008)
          throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
        byte[] buffer3 = new byte[16];
        readStream.Read(buffer3, 0, buffer3.Length);
        num1 = SharedUtilities.ReadSignature(readStream);
      }
      if (num1 != 101010256)
      {
        readStream.Seek(-4L, SeekOrigin.Current);
        throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", (object) num1, (object) readStream.Position));
      }
      byte[] buffer = new byte[16];
      zf.ReadStream.Read(buffer, 0, buffer.Length);
      if (zf._diskNumberWithCd == 0U)
        zf._diskNumberWithCd = (uint) BitConverter.ToUInt16(buffer, 2);
      ZipFile.ReadZipFileComment(zf);
    }

    private static void ReadZipFileComment(ZipFile zf)
    {
      byte[] buffer = new byte[2];
      zf.ReadStream.Read(buffer, 0, buffer.Length);
      short num = (short) ((int) buffer[0] + (int) buffer[1] * 256);
      if (num <= (short) 0)
        return;
      byte[] numArray = new byte[(int) num];
      zf.ReadStream.Read(numArray, 0, numArray.Length);
      string str = zf.AlternateEncoding.GetString(numArray, 0, numArray.Length);
      zf.Comment = str;
    }

    public static bool IsZipFile(string fileName) => ZipFile.IsZipFile(fileName, false);

    public static bool IsZipFile(string fileName, bool testExtract)
    {
      bool flag = false;
      try
      {
        if (!File.Exists(fileName))
          return false;
        using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          flag = ZipFile.IsZipFile((Stream) fileStream, testExtract);
      }
      catch (IOException ex)
      {
      }
      catch (ZipException ex)
      {
      }
      return flag;
    }

    public static bool IsZipFile(Stream stream, bool testExtract)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      bool flag = false;
      try
      {
        if (!stream.CanRead)
          return false;
        Stream stream1 = Stream.Null;
        using (ZipFile zipFile = ZipFile.Read(stream, (TextWriter) null, (Encoding) null, (EventHandler<ReadProgressEventArgs>) null))
        {
          if (testExtract)
          {
            foreach (ZipEntry zipEntry in zipFile)
            {
              if (!zipEntry.IsDirectory)
                zipEntry.Extract(stream1);
            }
          }
        }
        flag = true;
      }
      catch (IOException ex)
      {
      }
      catch (ZipException ex)
      {
      }
      return flag;
    }

    private void DeleteFileWithRetry(string filename)
    {
      bool flag = false;
      int num = 3;
      for (int index = 0; index < num && !flag; ++index)
      {
        try
        {
          File.Delete(filename);
          flag = true;
        }
        catch (UnauthorizedAccessException ex)
        {
          Console.WriteLine("************************************************** Retry delete.");
          Thread.Sleep(200 + index * 200);
        }
      }
    }

    public void Save()
    {
      try
      {
        bool flag1 = false;
        this._saveOperationCanceled = false;
        this._numberOfSegmentsForMostRecentSave = 0U;
        this.OnSaveStarted();
        if (this.WriteStream == null)
          throw new BadStateException("You haven't specified where to save the zip.");
        if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
          throw new BadStateException("You specified an EXE for a plain zip file.");
        if (!this._contentsChanged)
        {
          this.OnSaveCompleted();
          if (!this.Verbose)
            return;
          this.StatusMessageTextWriter.WriteLine("No save is necessary....");
        }
        else
        {
          this.Reset(true);
          if (this.Verbose)
            this.StatusMessageTextWriter.WriteLine("saving....");
          if (this._entries.Count >= (int) ushort.MaxValue && this._zip64 == Zip64Option.Default)
            throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
          int current = 0;
          ICollection<ZipEntry> zipEntries = this.SortEntriesBeforeSaving ? this.EntriesSorted : this.Entries;
          foreach (ZipEntry entry in (IEnumerable<ZipEntry>) zipEntries)
          {
            this.OnSaveEntry(current, entry, true);
            entry.Write(this.WriteStream);
            if (!this._saveOperationCanceled)
            {
              ++current;
              this.OnSaveEntry(current, entry, false);
              if (!this._saveOperationCanceled)
              {
                if (entry.IncludedInMostRecentSave)
                  flag1 |= entry.OutputUsedZip64.Value;
              }
              else
                break;
            }
            else
              break;
          }
          if (this._saveOperationCanceled)
            return;
          this._numberOfSegmentsForMostRecentSave = this.WriteStream is ZipSegmentedStream writeStream4 ? writeStream4.CurrentSegment : 1U;
          bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, zipEntries, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer((object) this));
          this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
          this._hasBeenSaved = true;
          this._contentsChanged = false;
          this._OutputUsesZip64 = new bool?(flag1 | flag2);
          if (this._name != null && (this._temporaryFileName != null || writeStream4 != null))
          {
            this.WriteStream.Dispose();
            if (this._saveOperationCanceled)
              return;
            if (this._fileAlreadyExists && this._readstream != null)
            {
              this._readstream.Close();
              this._readstream = (Stream) null;
              foreach (ZipEntry zipEntry in (IEnumerable<ZipEntry>) zipEntries)
              {
                if (zipEntry._archiveStream is ZipSegmentedStream archiveStream10)
                  archiveStream10.Dispose();
                zipEntry._archiveStream = (Stream) null;
              }
            }
            string str = (string) null;
            if (File.Exists(this._name))
            {
              str = this._name + "." + Path.GetRandomFileName();
              if (File.Exists(str))
                this.DeleteFileWithRetry(str);
              File.Move(this._name, str);
            }
            this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
            File.Move(writeStream4 != null ? writeStream4.CurrentTempName : this._temporaryFileName, this._name);
            this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
            if (str != null)
            {
              try
              {
                if (File.Exists(str))
                  File.Delete(str);
              }
              catch
              {
              }
            }
            this._fileAlreadyExists = true;
          }
          ZipFile.NotifyEntriesSaveComplete(zipEntries);
          this.OnSaveCompleted();
          this._JustSaved = true;
        }
      }
      finally
      {
        this.CleanupAfterSaveOperation();
      }
    }

    private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
    {
      foreach (ZipEntry zipEntry in (IEnumerable<ZipEntry>) c)
        zipEntry.NotifySaveComplete();
    }

    private void RemoveTempFile()
    {
      try
      {
        if (!File.Exists(this._temporaryFileName))
          return;
        File.Delete(this._temporaryFileName);
      }
      catch (IOException ex)
      {
        if (!this.Verbose)
          return;
        this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", (object) ex.Message);
      }
    }

    private void CleanupAfterSaveOperation()
    {
      if (this._name == null)
        return;
      if (this._writestream != null)
      {
        try
        {
          this._writestream.Dispose();
        }
        catch (IOException ex)
        {
        }
      }
      this._writestream = (Stream) null;
      if (this._temporaryFileName != null)
      {
        this.RemoveTempFile();
        this._temporaryFileName = (string) null;
      }
    }

    public void Save(string fileName)
    {
      if (this._name == null)
        this._writestream = (Stream) null;
      else
        this._readName = this._name;
      this._name = fileName;
      if (Directory.Exists(this._name))
        throw new ZipException("Bad Directory", (Exception) new ArgumentException("That name specifies an existing directory. Please specify a filename.", nameof (fileName)));
      this._contentsChanged = true;
      this._fileAlreadyExists = File.Exists(this._name);
      this.Save();
    }

    public void Save(Stream outputStream)
    {
      if (outputStream == null)
        throw new ArgumentNullException(nameof (outputStream));
      if (!outputStream.CanWrite)
        throw new ArgumentException("Must be a writable stream.", nameof (outputStream));
      this._name = (string) null;
      this._writestream = (Stream) new CountingStream(outputStream);
      this._contentsChanged = true;
      this._fileAlreadyExists = false;
      this.Save();
    }

    public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor) => this.SaveSelfExtractor(exeToGenerate, new SelfExtractorSaveOptions()
    {
      Flavor = flavor
    });

    public void SaveSelfExtractor(string exeToGenerate, SelfExtractorSaveOptions options)
    {
      if (this._name == null)
        this._writestream = (Stream) null;
      this._SavingSfx = true;
      this._name = exeToGenerate;
      if (Directory.Exists(this._name))
        throw new ZipException("Bad Directory", (Exception) new ArgumentException("That name specifies an existing directory. Please specify a filename.", nameof (exeToGenerate)));
      this._contentsChanged = true;
      this._fileAlreadyExists = File.Exists(this._name);
      this._SaveSfxStub(exeToGenerate, options);
      this.Save();
      this._SavingSfx = false;
    }

    private static void ExtractResourceToFile(Assembly a, string resourceName, string filename)
    {
      byte[] buffer = new byte[1024];
      using (Stream manifestResourceStream = a.GetManifestResourceStream(resourceName))
      {
        if (manifestResourceStream == null)
          throw new ZipException(string.Format("missing resource '{0}'", (object) resourceName));
        using (FileStream fileStream = File.OpenWrite(filename))
        {
          int count;
          do
          {
            count = manifestResourceStream.Read(buffer, 0, buffer.Length);
            fileStream.Write(buffer, 0, count);
          }
          while (count > 0);
        }
      }
    }

    private void _SaveSfxStub(string exeToGenerate, SelfExtractorSaveOptions options)
    {
      string path = (string) null;
      string str1 = (string) null;
      try
      {
        if (File.Exists(exeToGenerate) && this.Verbose)
          this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", (object) exeToGenerate);
        if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
          this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
        string dir = this.TempFileFolder ?? Path.GetDirectoryName(exeToGenerate);
        path = ZipFile.GenerateTempPathname(dir, "exe");
        Assembly assembly = typeof (ZipFile).Assembly;
        using (CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider())
        {
          ZipFile.ExtractorSettings extractorSettings = (ZipFile.ExtractorSettings) null;
          foreach (ZipFile.ExtractorSettings settings in ZipFile.SettingsList)
          {
            if (settings.Flavor == options.Flavor)
            {
              extractorSettings = settings;
              break;
            }
          }
          if (extractorSettings == null)
            throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", (object) options.Flavor));
          CompilerParameters options1 = new CompilerParameters();
          options1.ReferencedAssemblies.Add(assembly.Location);
          if (extractorSettings.ReferencedAssemblies != null)
          {
            foreach (string referencedAssembly in extractorSettings.ReferencedAssemblies)
              options1.ReferencedAssemblies.Add(referencedAssembly);
          }
          options1.GenerateInMemory = false;
          options1.GenerateExecutable = true;
          options1.IncludeDebugInformation = false;
          options1.CompilerOptions = "";
          Assembly executingAssembly = Assembly.GetExecutingAssembly();
          StringBuilder stringBuilder = new StringBuilder();
          string tempPathname = ZipFile.GenerateTempPathname(dir, "cs");
          using (ZipFile zipFile = ZipFile.Read(executingAssembly.GetManifestResourceStream("Ionic.Zip.Resources.ZippedResources.zip")))
          {
            str1 = ZipFile.GenerateTempPathname(dir, "tmp");
            if (string.IsNullOrEmpty(options.IconFile))
            {
              Directory.CreateDirectory(str1);
              ZipEntry zipEntry = zipFile["zippedFile.ico"];
              if ((zipEntry.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                zipEntry.Attributes ^= FileAttributes.ReadOnly;
              zipEntry.Extract(str1);
              string str2 = Path.Combine(str1, "zippedFile.ico");
              options1.CompilerOptions += string.Format("/win32icon:\"{0}\"", (object) str2);
            }
            else
              options1.CompilerOptions += string.Format("/win32icon:\"{0}\"", (object) options.IconFile);
            options1.OutputAssembly = path;
            if (options.Flavor == SelfExtractorFlavor.WinFormsApplication)
              options1.CompilerOptions += " /target:winexe";
            if (!string.IsNullOrEmpty(options.AdditionalCompilerSwitches))
            {
              CompilerParameters compilerParameters = options1;
              compilerParameters.CompilerOptions = compilerParameters.CompilerOptions + " " + options.AdditionalCompilerSwitches;
            }
            if (string.IsNullOrEmpty(options1.CompilerOptions))
              options1.CompilerOptions = (string) null;
            if (extractorSettings.CopyThroughResources != null && (uint) extractorSettings.CopyThroughResources.Count > 0U)
            {
              if (!Directory.Exists(str1))
                Directory.CreateDirectory(str1);
              foreach (string copyThroughResource in extractorSettings.CopyThroughResources)
              {
                string filename = Path.Combine(str1, copyThroughResource);
                ZipFile.ExtractResourceToFile(executingAssembly, copyThroughResource, filename);
                options1.EmbeddedResources.Add(filename);
              }
            }
            options1.EmbeddedResources.Add(assembly.Location);
            stringBuilder.Append("// " + Path.GetFileName(tempPathname) + "\n").Append("// --------------------------------------------\n//\n").Append("// This SFX source file was generated by DotNetZip ").Append(ZipFile.LibraryVersion.ToString()).Append("\n//         at ").Append(DateTime.Now.ToString("yyyy MMMM dd  HH:mm:ss")).Append("\n//\n// --------------------------------------------\n\n\n");
            if (!string.IsNullOrEmpty(options.Description))
              stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"" + options.Description.Replace("\"", "") + "\")]\n");
            else
              stringBuilder.Append("[assembly: System.Reflection.AssemblyTitle(\"DotNetZip SFX Archive\")]\n");
            if (!string.IsNullOrEmpty(options.ProductVersion))
              stringBuilder.Append("[assembly: System.Reflection.AssemblyInformationalVersion(\"" + options.ProductVersion.Replace("\"", "") + "\")]\n");
            string str3 = string.IsNullOrEmpty(options.Copyright) ? "Extractor: Copyright © Dino Chiesa 2008-2011" : options.Copyright.Replace("\"", "");
            if (!string.IsNullOrEmpty(options.ProductName))
              stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"").Append(options.ProductName.Replace("\"", "")).Append("\")]\n");
            else
              stringBuilder.Append("[assembly: System.Reflection.AssemblyProduct(\"DotNetZip\")]\n");
            stringBuilder.Append("[assembly: System.Reflection.AssemblyCopyright(\"" + str3 + "\")]\n").Append(string.Format("[assembly: System.Reflection.AssemblyVersion(\"{0}\")]\n", (object) ZipFile.LibraryVersion.ToString()));
            if (options.FileVersion != (Version) null)
              stringBuilder.Append(string.Format("[assembly: System.Reflection.AssemblyFileVersion(\"{0}\")]\n", (object) options.FileVersion.ToString()));
            stringBuilder.Append("\n\n\n");
            string newValue1 = options.DefaultExtractDirectory;
            if (newValue1 != null)
              newValue1 = newValue1.Replace("\"", "").Replace("\\", "\\\\");
            string newValue2 = options.PostExtractCommandLine;
            if (newValue2 != null)
              newValue2 = newValue2.Replace("\\", "\\\\").Replace("\"", "\\\"");
            foreach (string fileName in extractorSettings.ResourcesToCompile)
            {
              using (Stream stream = (Stream) zipFile[fileName].OpenReader())
              {
                if (stream == null)
                  throw new ZipException(string.Format("missing resource '{0}'", (object) fileName));
                using (StreamReader streamReader = new StreamReader(stream))
                {
                  while (streamReader.Peek() >= 0)
                  {
                    string str4 = streamReader.ReadLine();
                    if (newValue1 != null)
                      str4 = str4.Replace("@@EXTRACTLOCATION", newValue1);
                    string str5 = str4.Replace("@@REMOVE_AFTER_EXECUTE", options.RemoveUnpackedFilesAfterExecute.ToString()).Replace("@@QUIET", options.Quiet.ToString());
                    if (!string.IsNullOrEmpty(options.SfxExeWindowTitle))
                      str5 = str5.Replace("@@SFX_EXE_WINDOW_TITLE", options.SfxExeWindowTitle);
                    string str6 = str5.Replace("@@EXTRACT_EXISTING_FILE", ((int) options.ExtractExistingFile).ToString());
                    if (newValue2 != null)
                      str6 = str6.Replace("@@POST_UNPACK_CMD_LINE", newValue2);
                    stringBuilder.Append(str6).Append("\n");
                  }
                }
                stringBuilder.Append("\n\n");
              }
            }
          }
          string str7 = stringBuilder.ToString();
          CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(options1, str7);
          if (compilerResults == null)
            throw new SfxGenerationException("Cannot compile the extraction logic!");
          if (this.Verbose)
          {
            foreach (string str8 in compilerResults.Output)
              this.StatusMessageTextWriter.WriteLine(str8);
          }
          if ((uint) compilerResults.Errors.Count > 0U)
          {
            using (TextWriter textWriter = (TextWriter) new StreamWriter(tempPathname))
            {
              textWriter.Write(str7);
              textWriter.Write("\n\n\n// ------------------------------------------------------------------\n");
              textWriter.Write("// Errors during compilation: \n//\n");
              string fileName = Path.GetFileName(tempPathname);
              foreach (CompilerError error in (CollectionBase) compilerResults.Errors)
                textWriter.Write(string.Format("//   {0}({1},{2}): {3} {4}: {5}\n//\n", (object) fileName, (object) error.Line, (object) error.Column, error.IsWarning ? (object) "Warning" : (object) "error", (object) error.ErrorNumber, (object) error.ErrorText));
            }
            throw new SfxGenerationException(string.Format("Errors compiling the extraction logic!  {0}", (object) tempPathname));
          }
          this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
          using (Stream stream = (Stream) File.OpenRead(path))
          {
            byte[] buffer = new byte[4000];
            int count = 1;
            while ((uint) count > 0U)
            {
              count = stream.Read(buffer, 0, buffer.Length);
              if ((uint) count > 0U)
                this.WriteStream.Write(buffer, 0, count);
            }
          }
        }
        this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
      }
      finally
      {
        try
        {
          if (Directory.Exists(str1))
          {
            try
            {
              Directory.Delete(str1, true);
            }
            catch (IOException ex)
            {
              this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", (object) ex);
            }
          }
          if (File.Exists(path))
          {
            try
            {
              File.Delete(path);
            }
            catch (IOException ex)
            {
              this.StatusMessageTextWriter.WriteLine("Warning: Exception: {0}", (object) ex);
            }
          }
        }
        catch (IOException ex)
        {
        }
      }
    }

    internal static string GenerateTempPathname(string dir, string extension)
    {
      string name = Assembly.GetExecutingAssembly().GetName().Name;
      string path;
      do
      {
        string str = Guid.NewGuid().ToString();
        string path2 = string.Format("{0}-{1}-{2}.{3}", (object) name, (object) DateTime.Now.ToString("yyyyMMMdd-HHmmss"), (object) str, (object) extension);
        path = Path.Combine(dir, path2);
      }
      while (File.Exists(path) || Directory.Exists(path));
      return path;
    }

    public void AddSelectedFiles(string selectionCriteria) => this.AddSelectedFiles(selectionCriteria, ".", (string) null, false);

    public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories) => this.AddSelectedFiles(selectionCriteria, ".", (string) null, recurseDirectories);

    public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk) => this.AddSelectedFiles(selectionCriteria, directoryOnDisk, (string) null, false);

    public void AddSelectedFiles(
      string selectionCriteria,
      string directoryOnDisk,
      bool recurseDirectories)
    {
      this.AddSelectedFiles(selectionCriteria, directoryOnDisk, (string) null, recurseDirectories);
    }

    public void AddSelectedFiles(
      string selectionCriteria,
      string directoryOnDisk,
      string directoryPathInArchive)
    {
      this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
    }

    public void AddSelectedFiles(
      string selectionCriteria,
      string directoryOnDisk,
      string directoryPathInArchive,
      bool recurseDirectories)
    {
      this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
    }

    public void UpdateSelectedFiles(
      string selectionCriteria,
      string directoryOnDisk,
      string directoryPathInArchive,
      bool recurseDirectories)
    {
      this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
    }

    private string EnsureendInSlash(string s) => s.EndsWith("\\") ? s : s + "\\";

    private void _AddOrUpdateSelectedFiles(
      string selectionCriteria,
      string directoryOnDisk,
      string directoryPathInArchive,
      bool recurseDirectories,
      bool wantUpdate)
    {
      if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
      {
        directoryOnDisk = selectionCriteria;
        selectionCriteria = "*.*";
      }
      else if (string.IsNullOrEmpty(directoryOnDisk))
        directoryOnDisk = ".";
      while (directoryOnDisk.EndsWith("\\"))
        directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", (object) selectionCriteria, (object) directoryOnDisk);
      ReadOnlyCollection<string> readOnlyCollection = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectFiles(directoryOnDisk, recurseDirectories);
      if (this.Verbose)
        this.StatusMessageTextWriter.WriteLine("found {0} files...", (object) readOnlyCollection.Count);
      this.OnAddStarted();
      AddOrUpdateAction action = wantUpdate ? AddOrUpdateAction.AddOrUpdate : AddOrUpdateAction.AddOnly;
      foreach (string str1 in readOnlyCollection)
      {
        string str2 = directoryPathInArchive == null ? (string) null : ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(str1), directoryOnDisk, directoryPathInArchive);
        if (File.Exists(str1))
        {
          if (wantUpdate)
            this.UpdateFile(str1, str2);
          else
            this.AddFile(str1, str2);
        }
        else
          this.AddOrUpdateDirectoryImpl(str1, str2, action, false, 0);
      }
      this.OnAddCompleted();
    }

    private static string ReplaceLeadingDirectory(
      string original,
      string pattern,
      string replacement)
    {
      string upper1 = original.ToUpper();
      string upper2 = pattern.ToUpper();
      return (uint) upper1.IndexOf(upper2) > 0U ? original : replacement + original.Substring(upper2.Length);
    }

    public ICollection<ZipEntry> SelectEntries(string selectionCriteria) => new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectEntries(this);

    public ICollection<ZipEntry> SelectEntries(
      string selectionCriteria,
      string directoryPathInArchive)
    {
      return new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints).SelectEntries(this, directoryPathInArchive);
    }

    public int RemoveSelectedEntries(string selectionCriteria)
    {
      ICollection<ZipEntry> entriesToRemove = this.SelectEntries(selectionCriteria);
      this.RemoveEntries(entriesToRemove);
      return entriesToRemove.Count;
    }

    public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
    {
      ICollection<ZipEntry> entriesToRemove = this.SelectEntries(selectionCriteria, directoryPathInArchive);
      this.RemoveEntries(entriesToRemove);
      return entriesToRemove.Count;
    }

    public void ExtractSelectedEntries(string selectionCriteria)
    {
      foreach (ZipEntry selectEntry in (IEnumerable<ZipEntry>) this.SelectEntries(selectionCriteria))
      {
        selectEntry.Password = this._Password;
        selectEntry.Extract();
      }
    }

    public void ExtractSelectedEntries(
      string selectionCriteria,
      ExtractExistingFileAction extractExistingFile)
    {
      foreach (ZipEntry selectEntry in (IEnumerable<ZipEntry>) this.SelectEntries(selectionCriteria))
      {
        selectEntry.Password = this._Password;
        selectEntry.Extract(extractExistingFile);
      }
    }

    public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
    {
      foreach (ZipEntry selectEntry in (IEnumerable<ZipEntry>) this.SelectEntries(selectionCriteria, directoryPathInArchive))
      {
        selectEntry.Password = this._Password;
        selectEntry.Extract();
      }
    }

    public void ExtractSelectedEntries(
      string selectionCriteria,
      string directoryInArchive,
      string extractDirectory)
    {
      foreach (ZipEntry selectEntry in (IEnumerable<ZipEntry>) this.SelectEntries(selectionCriteria, directoryInArchive))
      {
        selectEntry.Password = this._Password;
        selectEntry.Extract(extractDirectory);
      }
    }

    public void ExtractSelectedEntries(
      string selectionCriteria,
      string directoryPathInArchive,
      string extractDirectory,
      ExtractExistingFileAction extractExistingFile)
    {
      foreach (ZipEntry selectEntry in (IEnumerable<ZipEntry>) this.SelectEntries(selectionCriteria, directoryPathInArchive))
      {
        selectEntry.Password = this._Password;
        selectEntry.Extract(extractDirectory, extractExistingFile);
      }
    }

    public IEnumerator<ZipEntry> GetEnumerator()
    {
      foreach (ZipEntry e in this._entries.Values)
        yield return e;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    [DispId(-4)]
    public IEnumerator GetNewEnum() => (IEnumerator) this.GetEnumerator();

    private class ExtractorSettings
    {
      public SelfExtractorFlavor Flavor;
      public List<string> ReferencedAssemblies;
      public List<string> CopyThroughResources;
      public List<string> ResourcesToCompile;
    }
  }
}
