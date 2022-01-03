// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.AutoCompleteCellEditor
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class AutoCompleteCellEditor : ComboBox
  {
    public AutoCompleteCellEditor(ObjectListView lv, OLVColumn column)
    {
      this.DropDownStyle = ComboBoxStyle.DropDown;
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      for (int index = 0; index < Math.Min(lv.GetItemCount(), 1000); ++index)
      {
        string stringValue = column.GetStringValue(lv.GetModelObject(index));
        if (!dictionary.ContainsKey(stringValue))
        {
          this.Items.Add((object) stringValue);
          dictionary[stringValue] = true;
        }
      }
      this.Sorted = true;
      this.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.AutoCompleteMode = AutoCompleteMode.Append;
    }
  }
}
