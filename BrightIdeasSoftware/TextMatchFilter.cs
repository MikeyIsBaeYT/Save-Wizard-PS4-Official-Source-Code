// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TextMatchFilter
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace BrightIdeasSoftware
{
  public class TextMatchFilter : AbstractModelFilter
  {
    private OLVColumn[] columns;
    private OLVColumn[] additionalColumns;
    private ObjectListView listView;
    private RegexOptions? regexOptions;
    private StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase;
    private List<TextMatchFilter.TextMatchingStrategy> MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();

    public static TextMatchFilter Regex(ObjectListView olv, params string[] texts) => new TextMatchFilter(olv)
    {
      RegexStrings = (IEnumerable<string>) texts
    };

    public static TextMatchFilter Prefix(ObjectListView olv, params string[] texts) => new TextMatchFilter(olv)
    {
      PrefixStrings = (IEnumerable<string>) texts
    };

    public static TextMatchFilter Contains(ObjectListView olv, params string[] texts) => new TextMatchFilter(olv)
    {
      ContainsStrings = (IEnumerable<string>) texts
    };

    public TextMatchFilter(ObjectListView olv) => this.ListView = olv;

    public TextMatchFilter(ObjectListView olv, string text)
    {
      this.ListView = olv;
      this.ContainsStrings = (IEnumerable<string>) new string[1]
      {
        text
      };
    }

    public TextMatchFilter(ObjectListView olv, string text, StringComparison comparison)
    {
      this.ListView = olv;
      this.ContainsStrings = (IEnumerable<string>) new string[1]
      {
        text
      };
      this.StringComparison = comparison;
    }

    public OLVColumn[] Columns
    {
      get => this.columns;
      set => this.columns = value;
    }

    public OLVColumn[] AdditionalColumns
    {
      get => this.additionalColumns;
      set => this.additionalColumns = value;
    }

    public IEnumerable<string> ContainsStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy component in this.MatchingStrategies)
          yield return component.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextContainsMatchingStrategy(this, text));
      }
    }

    public bool HasComponents => this.MatchingStrategies.Count > 0;

    public ObjectListView ListView
    {
      get => this.listView;
      set => this.listView = value;
    }

    public IEnumerable<string> PrefixStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy component in this.MatchingStrategies)
          yield return component.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextBeginsMatchingStrategy(this, text));
      }
    }

    public RegexOptions RegexOptions
    {
      get
      {
        if (!this.regexOptions.HasValue)
        {
          switch (this.StringComparison)
          {
            case StringComparison.CurrentCulture:
              this.regexOptions = new RegexOptions?(RegexOptions.None);
              break;
            case StringComparison.CurrentCultureIgnoreCase:
              this.regexOptions = new RegexOptions?(RegexOptions.IgnoreCase);
              break;
            case StringComparison.InvariantCulture:
            case StringComparison.Ordinal:
              this.regexOptions = new RegexOptions?(RegexOptions.CultureInvariant);
              break;
            case StringComparison.InvariantCultureIgnoreCase:
            case StringComparison.OrdinalIgnoreCase:
              this.regexOptions = new RegexOptions?(RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
              break;
            default:
              this.regexOptions = new RegexOptions?(RegexOptions.None);
              break;
          }
        }
        return this.regexOptions.Value;
      }
      set => this.regexOptions = new RegexOptions?(value);
    }

    public IEnumerable<string> RegexStrings
    {
      get
      {
        foreach (TextMatchFilter.TextMatchingStrategy component in this.MatchingStrategies)
          yield return component.Text;
      }
      set
      {
        this.MatchingStrategies = new List<TextMatchFilter.TextMatchingStrategy>();
        if (value == null)
          return;
        foreach (string text in value)
          this.MatchingStrategies.Add((TextMatchFilter.TextMatchingStrategy) new TextMatchFilter.TextRegexMatchingStrategy(this, text));
      }
    }

    public StringComparison StringComparison
    {
      get => this.stringComparison;
      set => this.stringComparison = value;
    }

    protected virtual IEnumerable<OLVColumn> IterateColumns()
    {
      if (this.Columns == null)
      {
        foreach (OLVColumn column in this.ListView.Columns)
          yield return column;
      }
      else
      {
        OLVColumn[] olvColumnArray = this.Columns;
        for (int index = 0; index < olvColumnArray.Length; ++index)
        {
          OLVColumn column = olvColumnArray[index];
          yield return column;
          column = (OLVColumn) null;
        }
        olvColumnArray = (OLVColumn[]) null;
      }
      if (this.AdditionalColumns != null)
      {
        OLVColumn[] olvColumnArray = this.AdditionalColumns;
        for (int index = 0; index < olvColumnArray.Length; ++index)
        {
          OLVColumn column = olvColumnArray[index];
          yield return column;
          column = (OLVColumn) null;
        }
        olvColumnArray = (OLVColumn[]) null;
      }
    }

    public override bool Filter(object modelObject)
    {
      if (this.ListView == null || !this.HasComponents)
        return true;
      foreach (OLVColumn iterateColumn in this.IterateColumns())
      {
        if (iterateColumn.IsVisible && iterateColumn.Searchable)
        {
          string stringValue = iterateColumn.GetStringValue(modelObject);
          foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
          {
            if (string.IsNullOrEmpty(matchingStrategy.Text) || matchingStrategy.MatchesText(stringValue))
              return true;
          }
        }
      }
      return false;
    }

    public IEnumerable<CharacterRange> FindAllMatchedRanges(string cellText)
    {
      List<CharacterRange> characterRangeList = new List<CharacterRange>();
      foreach (TextMatchFilter.TextMatchingStrategy matchingStrategy in this.MatchingStrategies)
      {
        if (!string.IsNullOrEmpty(matchingStrategy.Text))
          characterRangeList.AddRange(matchingStrategy.FindAllMatchedRanges(cellText));
      }
      return (IEnumerable<CharacterRange>) characterRangeList;
    }

    public bool IsIncluded(OLVColumn column)
    {
      if (this.Columns == null)
        return column.ListView == this.ListView;
      foreach (OLVColumn column1 in this.Columns)
      {
        if (column1 == column)
          return true;
      }
      return false;
    }

    protected abstract class TextMatchingStrategy
    {
      private TextMatchFilter textFilter;
      private string text;

      public StringComparison StringComparison => this.TextFilter.StringComparison;

      public TextMatchFilter TextFilter
      {
        get => this.textFilter;
        set => this.textFilter = value;
      }

      public string Text
      {
        get => this.text;
        set => this.text = value;
      }

      public abstract IEnumerable<CharacterRange> FindAllMatchedRanges(
        string cellText);

      public abstract bool MatchesText(string cellText);
    }

    protected class TextContainsMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      public TextContainsMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public override bool MatchesText(string cellText) => cellText.IndexOf(this.Text, this.StringComparison) != -1;

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(
        string cellText)
      {
        List<CharacterRange> characterRangeList = new List<CharacterRange>();
        for (int First = cellText.IndexOf(this.Text, this.StringComparison); First != -1; First = cellText.IndexOf(this.Text, First + this.Text.Length, this.StringComparison))
          characterRangeList.Add(new CharacterRange(First, this.Text.Length));
        return (IEnumerable<CharacterRange>) characterRangeList;
      }
    }

    protected class TextBeginsMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      public TextBeginsMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public override bool MatchesText(string cellText) => cellText.StartsWith(this.Text, this.StringComparison);

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(
        string cellText)
      {
        List<CharacterRange> characterRangeList = new List<CharacterRange>();
        if (cellText.StartsWith(this.Text, this.StringComparison))
          characterRangeList.Add(new CharacterRange(0, this.Text.Length));
        return (IEnumerable<CharacterRange>) characterRangeList;
      }
    }

    protected class TextRegexMatchingStrategy : TextMatchFilter.TextMatchingStrategy
    {
      private System.Text.RegularExpressions.Regex regex;
      private static System.Text.RegularExpressions.Regex InvalidRegexMarker = new System.Text.RegularExpressions.Regex(".*");

      public TextRegexMatchingStrategy(TextMatchFilter filter, string text)
      {
        this.TextFilter = filter;
        this.Text = text;
      }

      public RegexOptions RegexOptions => this.TextFilter.RegexOptions;

      protected System.Text.RegularExpressions.Regex Regex
      {
        get
        {
          if (this.regex == null)
          {
            try
            {
              this.regex = new System.Text.RegularExpressions.Regex(this.Text, this.RegexOptions);
            }
            catch (ArgumentException ex)
            {
              this.regex = TextMatchFilter.TextRegexMatchingStrategy.InvalidRegexMarker;
            }
          }
          return this.regex;
        }
        set => this.regex = value;
      }

      protected bool IsRegexInvalid => this.Regex == TextMatchFilter.TextRegexMatchingStrategy.InvalidRegexMarker;

      public override bool MatchesText(string cellText) => this.IsRegexInvalid || this.Regex.Match(cellText).Success;

      public override IEnumerable<CharacterRange> FindAllMatchedRanges(
        string cellText)
      {
        List<CharacterRange> characterRangeList = new List<CharacterRange>();
        if (!this.IsRegexInvalid)
        {
          foreach (Match match in this.Regex.Matches(cellText))
          {
            if (match.Length > 0)
              characterRangeList.Add(new CharacterRange(match.Index, match.Length));
          }
        }
        return (IEnumerable<CharacterRange>) characterRangeList;
      }
    }
  }
}
