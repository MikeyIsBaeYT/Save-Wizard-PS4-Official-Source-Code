// Decompiled with JetBrains decompiler
// Type: Rss.RssModuleItem
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

namespace Rss
{
  [Serializable]
  public class RssModuleItem : RssElement
  {
    private bool _bRequired = false;
    private string _sElementName = "";
    private string _sElementText = "";
    private RssModuleItemCollection _rssSubElements = new RssModuleItemCollection();

    public RssModuleItem()
    {
    }

    public RssModuleItem(string name) => this._sElementName = RssDefault.Check(name);

    public RssModuleItem(string name, bool required)
      : this(name)
    {
      this._bRequired = required;
    }

    public RssModuleItem(string name, string text)
      : this(name)
    {
      this._sElementText = RssDefault.Check(text);
    }

    public RssModuleItem(string name, bool required, string text)
      : this(name, required)
    {
      this._sElementText = RssDefault.Check(text);
    }

    public RssModuleItem(string name, string text, RssModuleItemCollection subElements)
      : this(name, text)
    {
      this._rssSubElements = subElements;
    }

    public RssModuleItem(
      string name,
      bool required,
      string text,
      RssModuleItemCollection subElements)
      : this(name, required, text)
    {
      this._rssSubElements = subElements;
    }

    public override string ToString()
    {
      if (this._sElementName != null)
        return this._sElementName;
      return this._sElementText != null ? this._sElementText : nameof (RssModuleItem);
    }

    public string Name
    {
      get => this._sElementName;
      set => this._sElementName = RssDefault.Check(value);
    }

    public string Text
    {
      get => this._sElementText;
      set => this._sElementText = RssDefault.Check(value);
    }

    public RssModuleItemCollection SubElements
    {
      get => this._rssSubElements;
      set => this._rssSubElements = value;
    }

    public bool IsRequired => this._bRequired;
  }
}
