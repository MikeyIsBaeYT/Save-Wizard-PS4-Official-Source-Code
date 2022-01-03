// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.BooleanCellEditor
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [ToolboxItem(false)]
  public class BooleanCellEditor : ComboBox
  {
    public BooleanCellEditor()
    {
      this.DropDownStyle = ComboBoxStyle.DropDownList;
      this.ValueMember = "Key";
      this.DataSource = (object) new ArrayList()
      {
        (object) new ComboBoxItem((object) false, "False"),
        (object) new ComboBoxItem((object) true, "True")
      };
    }
  }
}
