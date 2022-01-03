// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.TestStatus
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace ICSharpCode.SharpZipLib.Zip
{
  public class TestStatus
  {
    private ZipFile file_;
    private ZipEntry entry_;
    private bool entryValid_;
    private int errorCount_;
    private long bytesTested_;
    private TestOperation operation_;

    public TestStatus(ZipFile file) => this.file_ = file;

    public TestOperation Operation => this.operation_;

    public ZipFile File => this.file_;

    public ZipEntry Entry => this.entry_;

    public int ErrorCount => this.errorCount_;

    public long BytesTested => this.bytesTested_;

    public bool EntryValid => this.entryValid_;

    internal void AddError()
    {
      ++this.errorCount_;
      this.entryValid_ = false;
    }

    internal void SetOperation(TestOperation operation) => this.operation_ = operation;

    internal void SetEntry(ZipEntry entry)
    {
      this.entry_ = entry;
      this.entryValid_ = true;
      this.bytesTested_ = 0L;
    }

    internal void SetBytesTested(long value) => this.bytesTested_ = value;
  }
}
