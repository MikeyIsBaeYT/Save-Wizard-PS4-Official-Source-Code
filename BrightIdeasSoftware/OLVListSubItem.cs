// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.OLVListSubItem
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  [Browsable(false)]
  public class OLVListSubItem : ListViewItem.ListViewSubItem
  {
    private Rectangle? cellPadding;
    private StringAlignment? cellVerticalAlignment;
    private object modelValue;
    private IList<IDecoration> decorations;
    private object imageSelector;
    private string url;
    internal ImageRenderer.AnimationState AnimationState;

    public OLVListSubItem()
    {
    }

    public OLVListSubItem(object modelValue, string text, object image)
    {
      this.ModelValue = modelValue;
      this.Text = text;
      this.ImageSelector = image;
    }

    public Rectangle? CellPadding
    {
      get => this.cellPadding;
      set => this.cellPadding = value;
    }

    public StringAlignment? CellVerticalAlignment
    {
      get => this.cellVerticalAlignment;
      set => this.cellVerticalAlignment = value;
    }

    public object ModelValue
    {
      get => this.modelValue;
      private set => this.modelValue = value;
    }

    public bool HasDecoration => this.decorations != null && this.decorations.Count > 0;

    public IDecoration Decoration
    {
      get => this.HasDecoration ? this.Decorations[0] : (IDecoration) null;
      set
      {
        this.Decorations.Clear();
        if (value == null)
          return;
        this.Decorations.Add(value);
      }
    }

    public IList<IDecoration> Decorations
    {
      get
      {
        if (this.decorations == null)
          this.decorations = (IList<IDecoration>) new List<IDecoration>();
        return this.decorations;
      }
    }

    public object ImageSelector
    {
      get => this.imageSelector;
      set => this.imageSelector = value;
    }

    public string Url
    {
      get => this.url;
      set => this.url = value;
    }
  }
}
