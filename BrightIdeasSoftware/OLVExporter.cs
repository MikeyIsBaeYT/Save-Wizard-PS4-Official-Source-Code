// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVExporter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BrightIdeasSoftware
{
  public class OLVExporter
  {
    private bool includeHiddenColumns;
    private bool includeColumnHeaders = true;
    private ObjectListView objectListView;
    private IList modelObjects = (IList) new ArrayList();
    private Dictionary<OLVExporter.ExportFormat, string> results;

    public OLVExporter()
    {
    }

    public OLVExporter(ObjectListView olv)
      : this(olv, olv.Objects)
    {
    }

    public OLVExporter(ObjectListView olv, IEnumerable objectsToExport)
    {
      if (olv == null)
        throw new ArgumentNullException(nameof (olv));
      if (objectsToExport == null)
        throw new ArgumentNullException(nameof (objectsToExport));
      this.ListView = olv;
      this.ModelObjects = (IList) ObjectListView.EnumerableToArray(objectsToExport, true);
    }

    public bool IncludeHiddenColumns
    {
      get => this.includeHiddenColumns;
      set => this.includeHiddenColumns = value;
    }

    public bool IncludeColumnHeaders
    {
      get => this.includeColumnHeaders;
      set => this.includeColumnHeaders = value;
    }

    public ObjectListView ListView
    {
      get => this.objectListView;
      set => this.objectListView = value;
    }

    public IList ModelObjects
    {
      get => this.modelObjects;
      set => this.modelObjects = value;
    }

    public string ExportTo(OLVExporter.ExportFormat format)
    {
      if (this.results == null)
        this.Convert();
      return this.results[format];
    }

    public void Convert()
    {
      IList<OLVColumn> olvColumnList = this.IncludeHiddenColumns ? (IList<OLVColumn>) this.ListView.AllColumns : (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder sb1 = new StringBuilder();
      StringBuilder sb2 = new StringBuilder();
      StringBuilder sb3 = new StringBuilder("<table>");
      if (this.IncludeColumnHeaders)
      {
        List<string> stringList = new List<string>();
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) olvColumnList)
          stringList.Add(olvColumn.Text);
        this.WriteOneRow(sb1, (IEnumerable<string>) stringList, "", "\t", "", (OLVExporter.StringToString) null);
        this.WriteOneRow(sb3, (IEnumerable<string>) stringList, "<tr><td>", "</td><td>", "</td></tr>", new OLVExporter.StringToString(OLVExporter.HtmlEncode));
        this.WriteOneRow(sb2, (IEnumerable<string>) stringList, "", ",", "", new OLVExporter.StringToString(OLVExporter.CsvEncode));
      }
      foreach (object modelObject in (IEnumerable) this.ModelObjects)
      {
        List<string> stringList = new List<string>();
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) olvColumnList)
          stringList.Add(olvColumn.GetStringValue(modelObject));
        this.WriteOneRow(sb1, (IEnumerable<string>) stringList, "", "\t", "", (OLVExporter.StringToString) null);
        this.WriteOneRow(sb3, (IEnumerable<string>) stringList, "<tr><td>", "</td><td>", "</td></tr>", new OLVExporter.StringToString(OLVExporter.HtmlEncode));
        this.WriteOneRow(sb2, (IEnumerable<string>) stringList, "", ",", "", new OLVExporter.StringToString(OLVExporter.CsvEncode));
      }
      sb3.AppendLine("</table>");
      this.results = new Dictionary<OLVExporter.ExportFormat, string>();
      this.results[OLVExporter.ExportFormat.TabSeparated] = sb1.ToString();
      this.results[OLVExporter.ExportFormat.CSV] = sb2.ToString();
      this.results[OLVExporter.ExportFormat.HTML] = sb3.ToString();
    }

    private void WriteOneRow(
      StringBuilder sb,
      IEnumerable<string> strings,
      string startRow,
      string betweenCells,
      string endRow,
      OLVExporter.StringToString encoder)
    {
      sb.Append(startRow);
      bool flag = true;
      foreach (string str in strings)
      {
        if (!flag)
          sb.Append(betweenCells);
        sb.Append(encoder == null ? str : encoder(str));
        flag = false;
      }
      sb.AppendLine(endRow);
    }

    private static string CsvEncode(string text)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder("\"");
      stringBuilder.Append(text.Replace("\"", "\"\""));
      stringBuilder.Append("\"");
      return stringBuilder.ToString();
    }

    private static string HtmlEncode(string text)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (text[index])
        {
          case '"':
            stringBuilder.Append("&quot;");
            break;
          case '&':
            stringBuilder.Append("&amp;");
            break;
          case '<':
            stringBuilder.Append("&lt;");
            break;
          case '>':
            stringBuilder.Append("&gt;");
            break;
          default:
            if (text[index] > '\u009F')
            {
              stringBuilder.Append("&#");
              stringBuilder.Append(((int) text[index]).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(";");
              break;
            }
            stringBuilder.Append(text[index]);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    public enum ExportFormat
    {
      TSV = 1,
      TabSeparated = 1,
      CSV = 2,
      HTML = 3,
    }

    private delegate string StringToString(string str);
  }
}
