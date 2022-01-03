// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVDataObject
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class OLVDataObject : DataObject
  {
    private bool includeHiddenColumns;
    private bool includeColumnHeaders;
    private ObjectListView objectListView;
    private IList modelObjects = (IList) new ArrayList();

    public OLVDataObject(ObjectListView olv)
      : this(olv, olv.SelectedObjects)
    {
    }

    public OLVDataObject(ObjectListView olv, IList modelObjects)
    {
      this.objectListView = olv;
      this.modelObjects = modelObjects;
      this.includeHiddenColumns = olv.IncludeHiddenColumnsInDataTransfer;
      this.includeColumnHeaders = olv.IncludeColumnHeadersInCopy;
    }

    public bool IncludeHiddenColumns => this.includeHiddenColumns;

    public bool IncludeColumnHeaders => this.includeColumnHeaders;

    public ObjectListView ListView => this.objectListView;

    public IList ModelObjects => this.modelObjects;

    public void CreateTextFormats()
    {
      IList<OLVColumn> olvColumnList = this.IncludeHiddenColumns ? (IList<OLVColumn>) this.ListView.AllColumns : (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder("<table>");
      if (this.includeColumnHeaders)
      {
        stringBuilder2.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) olvColumnList)
        {
          if (olvColumn != olvColumnList[0])
          {
            stringBuilder1.Append("\t");
            stringBuilder2.Append("</td><td>");
          }
          string text = olvColumn.Text;
          stringBuilder1.Append(text);
          stringBuilder2.Append(text);
        }
        stringBuilder1.AppendLine();
        stringBuilder2.AppendLine("</td></tr>");
      }
      foreach (object modelObject in (IEnumerable) this.ModelObjects)
      {
        stringBuilder2.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) olvColumnList)
        {
          if (olvColumn != olvColumnList[0])
          {
            stringBuilder1.Append("\t");
            stringBuilder2.Append("</td><td>");
          }
          string stringValue = olvColumn.GetStringValue(modelObject);
          stringBuilder1.Append(stringValue);
          stringBuilder2.Append(stringValue);
        }
        stringBuilder1.AppendLine();
        stringBuilder2.AppendLine("</td></tr>");
      }
      stringBuilder2.AppendLine("</table>");
      this.SetData((object) stringBuilder1.ToString());
      this.SetText(this.ConvertToHtmlFragment(stringBuilder2.ToString()), TextDataFormat.Html);
    }

    public string CreateHtml()
    {
      IList<OLVColumn> columnsInDisplayOrder = (IList<OLVColumn>) this.ListView.ColumnsInDisplayOrder;
      StringBuilder stringBuilder = new StringBuilder("<table>");
      foreach (object modelObject in (IEnumerable) this.ModelObjects)
      {
        stringBuilder.Append("<tr><td>");
        foreach (OLVColumn olvColumn in (IEnumerable<OLVColumn>) columnsInDisplayOrder)
        {
          if (olvColumn != columnsInDisplayOrder[0])
            stringBuilder.Append("</td><td>");
          string stringValue = olvColumn.GetStringValue(modelObject);
          stringBuilder.Append(stringValue);
        }
        stringBuilder.AppendLine("</td></tr>");
      }
      stringBuilder.AppendLine("</table>");
      return stringBuilder.ToString();
    }

    private string ConvertToHtmlFragment(string fragment)
    {
      string str1 = "http://www.codeproject.com/KB/list/ObjectListView.aspx";
      int length = string.Format("Version:1.0\r\nStartHTML:{0,8}\r\nEndHTML:{1,8}\r\nStartFragment:{2,8}\r\nEndFragment:{3,8}\r\nStartSelection:{2,8}\r\nEndSelection:{3,8}\r\nSourceURL:{4}\r\n{5}", (object) 0, (object) 0, (object) 0, (object) 0, (object) str1, (object) "").Length;
      string str2 = string.Format("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD></HEAD><BODY><!--StartFragment-->{0}<!--EndFragment--></BODY></HTML>", (object) fragment);
      int num1 = length + str2.IndexOf(fragment);
      int num2 = num1 + fragment.Length;
      return string.Format("Version:1.0\r\nStartHTML:{0,8}\r\nEndHTML:{1,8}\r\nStartFragment:{2,8}\r\nEndFragment:{3,8}\r\nStartSelection:{2,8}\r\nEndSelection:{3,8}\r\nSourceURL:{4}\r\n{5}", (object) length, (object) (length + str2.Length), (object) num1, (object) num2, (object) str1, (object) str2);
    }
  }
}
