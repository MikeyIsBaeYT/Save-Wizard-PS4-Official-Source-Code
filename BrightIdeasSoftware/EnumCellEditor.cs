// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.EnumCellEditor
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class EnumCellEditor : ComboBox
  {
    public EnumCellEditor(Type type)
    {
      this.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ValueMember = "Key";
      ArrayList arrayList = new ArrayList();
      foreach (object key in Enum.GetValues(type))
        arrayList.Add((object) new ComboBoxItem(key, Enum.GetName(type, key)));
      this.DataSource = (object) arrayList;
    }
  }
}
