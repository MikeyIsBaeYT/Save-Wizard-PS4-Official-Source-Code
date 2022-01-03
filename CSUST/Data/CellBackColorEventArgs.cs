// Decompiled with JetBrains decompiler
// Type: CSUST.Data.CellBackColorEventArgs
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Drawing;

namespace CSUST.Data
{
  public class CellBackColorEventArgs : EventArgs
  {
    private int m_RowIndex;
    private int m_ColIndex;
    private Color m_BackColor = Color.Empty;

    public CellBackColorEventArgs(int row, int col)
    {
      this.m_RowIndex = row;
      this.m_ColIndex = col;
    }

    public int RowIndex => this.m_RowIndex;

    public int ColIndex => this.m_ColIndex;

    public Color BackColor
    {
      get => this.m_BackColor;
      set => this.m_BackColor = value;
    }
  }
}
