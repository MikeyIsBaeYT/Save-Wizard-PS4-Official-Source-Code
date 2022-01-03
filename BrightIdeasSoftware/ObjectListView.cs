// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.ObjectListView
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using BrightIdeasSoftware.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
  [Designer(typeof (ObjectListViewDesigner))]
  public class ObjectListView : System.Windows.Forms.ListView, ISupportInitialize
  {
    private int lastValidatingEvent = 0;
    private static bool? sIsVistaOrLater;
    private static bool? sIsWin7OrLater;
    private static SmoothingMode sSmoothingMode = SmoothingMode.HighQuality;
    private static TextRenderingHint sTextRendereringHint = TextRenderingHint.SystemDefault;
    private static string sGroupTitleDefault = "{null}";
    private static bool sShowCellPaddingBounds;
    private static SimpleItemStyle sDefaultDisabledItemStyle;
    private IModelFilter additionalFilter;
    private List<OLVColumn> allColumns = new List<OLVColumn>();
    private Color alternateRowBackColor = Color.Empty;
    private OLVColumn alwaysGroupByColumn;
    private SortOrder alwaysGroupBySortOrder = SortOrder.None;
    private ObjectListView.CellEditActivateMode cellEditActivation = ObjectListView.CellEditActivateMode.None;
    private CellEditKeyEngine cellEditKeyEngine;
    private bool cellEditTabChangesRows;
    private bool cellEditEnterChangesRows;
    private ToolTipControl cellToolTip;
    private Rectangle? cellPadding;
    private StringAlignment cellVerticalAlignment = StringAlignment.Center;
    private bool copySelectionOnControlC = true;
    private bool copySelectionOnControlCUsesDragSource = true;
    private readonly List<IDecoration> decorations = new List<IDecoration>();
    private IRenderer defaultRenderer = (IRenderer) new BaseRenderer();
    private SimpleItemStyle disabledItemStyle;
    private readonly Hashtable disabledObjects = new Hashtable();
    private IDragSource dragSource;
    private IDropSink dropSink;
    public static EditorRegistry EditorRegistry = new EditorRegistry();
    private IOverlay emptyListMsgOverlay;
    private FilterMenuBuilder filterMenuBuilder = new FilterMenuBuilder();
    private ImageList groupImageList;
    private string groupWithItemCountFormat;
    private string groupWithItemCountSingularFormat;
    private bool hasCollapsibleGroups = true;
    private HeaderControl headerControl;
    private HeaderFormatStyle headerFormatStyle;
    private int headerMaximumHeight = -1;
    private bool headerUsesThemes = false;
    private bool headerWordWrap;
    private int hotRowIndex;
    private int hotColumnIndex;
    private HitTestLocation hotCellHitLocation;
    private HitTestLocationEx hotCellHitLocationEx;
    private OLVGroup hotGroup;
    private HotItemStyle hotItemStyle;
    private HyperlinkStyle hyperlinkStyle;
    private Color highlightBackgroundColor = Color.Empty;
    private Color highlightForegroundColor = Color.Empty;
    private bool includeHiddenColumnsInDataTransfer;
    private bool includeColumnHeadersInCopy;
    private bool isSearchOnSortColumn = true;
    private IRenderer itemRenderer;
    private IListFilter listFilter;
    private IModelFilter modelFilter;
    private OlvListViewHitTestInfo mouseMoveHitTest;
    private IList<OLVGroup> olvGroups;
    private bool ownerDrawnHeader;
    private IEnumerable objects;
    private ImageOverlay imageOverlay;
    private TextOverlay textOverlay;
    private int overlayTransparency = 128;
    private readonly List<IOverlay> overlays = new List<IOverlay>();
    private bool persistentCheckBoxes = true;
    private Dictionary<object, CheckState> checkStateMap;
    private OLVColumn primarySortColumn;
    private SortOrder primarySortOrder;
    private bool renderNonEditableCheckboxesAsDisabled;
    private int rowHeight = -1;
    private OLVColumn secondarySortColumn;
    private SortOrder secondarySortOrder = SortOrder.None;
    private bool selectAllOnControlA = true;
    private ObjectListView.ColumnSelectBehaviour selectColumnsOnRightClickBehaviour = ObjectListView.ColumnSelectBehaviour.InlineMenu;
    private bool selectColumnsMenuStaysOpen = true;
    private OLVColumn selectedColumn;
    private readonly TintedColumnDecoration selectedColumnDecoration = new TintedColumnDecoration();
    private IDecoration selectedRowDecoration;
    private Color selectedColumnTint = Color.Empty;
    private bool showCommandMenuOnRightClick;
    private bool showFilterMenuOnRightClick = true;
    private bool showSortIndicators;
    private bool showImagesOnSubItems;
    private bool showItemCountOnGroups;
    private bool showHeaderInAllViews = true;
    private ImageList shadowedImageList;
    private bool sortGroupItemsByPrimaryColumn = true;
    private int spaceBetweenGroups;
    private bool tintSortColumn;
    private bool triStateCheckBoxes;
    private bool triggerCellOverEventsWhenOverHeader = true;
    private bool updateSpaceFillingColumnsWhenDraggingColumnDivider = true;
    private Color unfocusedHighlightBackgroundColor = Color.Empty;
    private Color unfocusedHighlightForegroundColor = Color.Empty;
    private bool useAlternatingBackColors;
    private bool useCellFormatEvents;
    private bool useCustomSelectionColors;
    private bool useExplorerTheme;
    private bool useFiltering;
    private bool useFilterIndicator;
    private bool useHotItem;
    private bool useHyperlinks;
    private bool useOverlays = true;
    private bool useSubItemCheckBoxes;
    private bool useTranslucentSelection;
    private bool useTranslucentHotItem;
    private bool canUseApplicationIdle = true;
    private CellToolTipGetterDelegate cellToolTipGetter;
    private string checkedAspectName;
    private Munger checkedAspectMunger;
    private CheckStateGetterDelegate checkStateGetter;
    private CheckStatePutterDelegate checkStatePutter;
    private SortDelegate customSorter;
    private HeaderToolTipGetterDelegate headerToolTipGetter;
    private RowFormatterDelegate rowFormatter;
    private GroupingParameters lastGroupingParameters;
    private bool useNotifyPropertyChanged;
    private Hashtable subscribedModels = new Hashtable();
    private int timeLastCharEvent;
    private string lastSearchString;
    private readonly IntPtr minusOne = new IntPtr(-1);
    private bool isAfterItemPaint;
    private List<OLVListItem> drawnItems;
    private bool fakeRightClick = false;
    private int prePaintLevel;
    private string menuLabelSortAscending = "Sort ascending by '{0}'";
    private string menuLabelSortDescending = "Sort descending by '{0}'";
    private string menuLabelGroupBy = "Group by '{0}'";
    private string menuLabelLockGroupingOn = "Lock grouping on '{0}'";
    private string menuLabelUnlockGroupingOn = "Unlock grouping from '{0}'";
    private string menuLabelTurnOffGroups = "Turn off groups";
    private string menuLabelUnsort = "Unsort";
    private string menuLabelColumns = nameof (Columns);
    private string menuLabelSelectColumns = "Select Columns...";
    private bool contextMenuStaysOpen;
    private int freezeCount;
    public const string SORT_INDICATOR_UP_KEY = "sort-indicator-up";
    public const string SORT_INDICATOR_DOWN_KEY = "sort-indicator-down";
    public const string CHECKED_KEY = "checkbox-checked";
    public const string UNCHECKED_KEY = "checkbox-unchecked";
    public const string INDETERMINATE_KEY = "checkbox-indeterminate";
    private int lastMouseDownClickCount;
    private Control cellEditor;
    internal CellEditEventArgs CellEditEventArgs;
    private bool isOwnerOfObjects;
    private bool hasIdleHandler;
    private bool hasResizeColumnsHandler;
    private bool isInWmPaintEvent;
    private bool shouldDoCustomDrawing;
    private bool isMarqueSelecting;
    private int suspendSelectionEventCount;
    private readonly List<GlassPanelForm> glassPanels = new List<GlassPanelForm>();
    private Dictionary<string, bool> visitedUrlMap = new Dictionary<string, bool>();

    [Category("ObjectListView")]
    [Description("This event is triggered after the control has done a search-by-typing action.")]
    public event EventHandler<AfterSearchingEventArgs> AfterSearching;

    [Category("ObjectListView")]
    [Description("This event is triggered after the items in the list have been sorted.")]
    public event EventHandler<AfterSortingEventArgs> AfterSorting;

    [Category("ObjectListView")]
    [Description("This event is triggered before the control does a search-by-typing action.")]
    public event EventHandler<BeforeSearchingEventArgs> BeforeSearching;

    [Category("ObjectListView")]
    [Description("This event is triggered before the items in the list are sorted.")]
    public event EventHandler<BeforeSortingEventArgs> BeforeSorting;

    [Category("ObjectListView")]
    [Description("This event is triggered after the groups are created.")]
    public event EventHandler<CreateGroupsEventArgs> AfterCreatingGroups;

    [Category("ObjectListView")]
    [Description("This event is triggered before the groups are created.")]
    public event EventHandler<CreateGroupsEventArgs> BeforeCreatingGroups;

    [Category("ObjectListView")]
    [Description("This event is triggered when the groups are just about to be created.")]
    public event EventHandler<CreateGroupsEventArgs> AboutToCreateGroups;

    [Category("ObjectListView")]
    [Description("Can the user drop the currently dragged items at the current mouse location?")]
    public event EventHandler<OlvDropEventArgs> CanDrop;

    [Category("ObjectListView")]
    [Description("This event is triggered cell edit operation is finishing.")]
    public event CellEditEventHandler CellEditFinishing;

    [Category("ObjectListView")]
    [Description("This event is triggered when cell edit is about to begin.")]
    public event CellEditEventHandler CellEditStarting;

    [Category("ObjectListView")]
    [Description("This event is triggered when a cell editor is about to lose focus and its new contents need to be validated.")]
    public event CellEditEventHandler CellEditValidating;

    [Category("ObjectListView")]
    [Description("This event is triggered when the user left clicks a cell.")]
    public event EventHandler<CellClickEventArgs> CellClick;

    [Category("ObjectListView")]
    [Description("This event is triggered when the mouse is over a cell.")]
    public event EventHandler<CellOverEventArgs> CellOver;

    [Category("ObjectListView")]
    [Description("This event is triggered when the user right clicks a cell.")]
    public event EventHandler<CellRightClickEventArgs> CellRightClick;

    [Category("ObjectListView")]
    [Description("This event is triggered when a cell needs a tool tip.")]
    public event EventHandler<ToolTipShowingEventArgs> CellToolTipShowing;

    [Category("ObjectListView")]
    [Description("This event is triggered when a checkbox is checked/unchecked on a subitem.")]
    public event EventHandler<SubItemCheckingEventArgs> SubItemChecking;

    [Category("ObjectListView")]
    [Description("This event is triggered when the user right clicks a column header.")]
    public event ColumnRightClickEventHandler ColumnRightClick;

    [Category("ObjectListView")]
    [Description("This event is triggered when the user dropped items onto the control.")]
    public event EventHandler<OlvDropEventArgs> Dropped;

    [Category("ObjectListView")]
    [Description("This event is triggered when the control needs to filter its collection of objects.")]
    public event EventHandler<FilterEventArgs> Filter;

    [Category("ObjectListView")]
    [Description("This event is triggered when a cell needs to be formatted.")]
    public event EventHandler<FormatCellEventArgs> FormatCell;

    [Category("ObjectListView")]
    [Description("This event is triggered when frozeness of the control changes.")]
    public event EventHandler<FreezeEventArgs> Freezing;

    [Category("ObjectListView")]
    [Description("This event is triggered when a row needs to be formatted.")]
    public event EventHandler<FormatRowEventArgs> FormatRow;

    [Category("ObjectListView")]
    [Description("This event is triggered when a group is about to collapse or expand.")]
    public event EventHandler<GroupExpandingCollapsingEventArgs> GroupExpandingCollapsing;

    [Category("ObjectListView")]
    [Description("This event is triggered when a group changes state.")]
    public event EventHandler<GroupStateChangedEventArgs> GroupStateChanged;

    [Category("ObjectListView")]
    [Description("This event is triggered when a header checkbox changes value.")]
    public event EventHandler<HeaderCheckBoxChangingEventArgs> HeaderCheckBoxChanging;

    [Category("ObjectListView")]
    [Description("This event is triggered when a header needs a tool tip.")]
    public event EventHandler<ToolTipShowingEventArgs> HeaderToolTipShowing;

    [Category("ObjectListView")]
    [Description("This event is triggered when the hot item changed.")]
    public event EventHandler<HotItemChangedEventArgs> HotItemChanged;

    [Category("ObjectListView")]
    [Description("This event is triggered when a hyperlink cell is clicked.")]
    public event EventHandler<HyperlinkClickedEventArgs> HyperlinkClicked;

    [Category("ObjectListView")]
    [Description("This event is triggered when the task text of a group is clicked.")]
    public event EventHandler<GroupTaskClickedEventArgs> GroupTaskClicked;

    [Category("ObjectListView")]
    [Description("This event is triggered when the control needs to know if a given cell contains a hyperlink.")]
    public event EventHandler<IsHyperlinkEventArgs> IsHyperlink;

    [Category("ObjectListView")]
    [Description("This event is triggered when objects are about to be added to the control")]
    public event EventHandler<ItemsAddingEventArgs> ItemsAdding;

    [Category("ObjectListView")]
    [Description("This event is triggered when the contents of the control have changed.")]
    public event EventHandler<ItemsChangedEventArgs> ItemsChanged;

    [Category("ObjectListView")]
    [Description("This event is triggered when the contents of the control changes.")]
    public event EventHandler<ItemsChangingEventArgs> ItemsChanging;

    [Category("ObjectListView")]
    [Description("This event is triggered when objects are removed from the control.")]
    public event EventHandler<ItemsRemovingEventArgs> ItemsRemoving;

    [Category("ObjectListView")]
    [Description("Can the dragged collection of model objects be dropped at the current mouse location")]
    public event EventHandler<ModelDropEventArgs> ModelCanDrop;

    [Category("ObjectListView")]
    [Description("A collection of model objects from a ObjectListView has been dropped on this control")]
    public event EventHandler<ModelDropEventArgs> ModelDropped;

    [Category("ObjectListView")]
    [Description("This event is triggered once per user action that changes the selection state of one or more rows.")]
    public event EventHandler SelectionChanged;

    [Category("ObjectListView")]
    [Description("This event is triggered when the contents of the ObjectListView has scrolled.")]
    public event EventHandler<ScrollEventArgs> Scroll;

    protected virtual void OnAboutToCreateGroups(CreateGroupsEventArgs e)
    {
      if (this.AboutToCreateGroups == null)
        return;
      this.AboutToCreateGroups((object) this, e);
    }

    protected virtual void OnBeforeCreatingGroups(CreateGroupsEventArgs e)
    {
      if (this.BeforeCreatingGroups == null)
        return;
      this.BeforeCreatingGroups((object) this, e);
    }

    protected virtual void OnAfterCreatingGroups(CreateGroupsEventArgs e)
    {
      if (this.AfterCreatingGroups == null)
        return;
      this.AfterCreatingGroups((object) this, e);
    }

    protected virtual void OnAfterSearching(AfterSearchingEventArgs e)
    {
      if (this.AfterSearching == null)
        return;
      this.AfterSearching((object) this, e);
    }

    protected virtual void OnAfterSorting(AfterSortingEventArgs e)
    {
      if (this.AfterSorting == null)
        return;
      this.AfterSorting((object) this, e);
    }

    protected virtual void OnBeforeSearching(BeforeSearchingEventArgs e)
    {
      if (this.BeforeSearching == null)
        return;
      this.BeforeSearching((object) this, e);
    }

    protected virtual void OnBeforeSorting(BeforeSortingEventArgs e)
    {
      if (this.BeforeSorting == null)
        return;
      this.BeforeSorting((object) this, e);
    }

    protected virtual void OnCanDrop(OlvDropEventArgs args)
    {
      if (this.CanDrop == null)
        return;
      this.CanDrop((object) this, args);
    }

    protected virtual void OnCellClick(CellClickEventArgs args)
    {
      if (this.CellClick == null)
        return;
      this.CellClick((object) this, args);
    }

    protected virtual void OnCellOver(CellOverEventArgs args)
    {
      if (this.CellOver == null)
        return;
      this.CellOver((object) this, args);
    }

    protected virtual void OnCellRightClick(CellRightClickEventArgs args)
    {
      if (this.CellRightClick == null)
        return;
      this.CellRightClick((object) this, args);
    }

    protected virtual void OnCellToolTip(ToolTipShowingEventArgs args)
    {
      if (this.CellToolTipShowing == null)
        return;
      this.CellToolTipShowing((object) this, args);
    }

    protected virtual void OnSubItemChecking(SubItemCheckingEventArgs args)
    {
      if (this.SubItemChecking == null)
        return;
      this.SubItemChecking((object) this, args);
    }

    protected virtual void OnColumnRightClick(ColumnClickEventArgs e)
    {
      if (this.ColumnRightClick == null)
        return;
      this.ColumnRightClick((object) this, e);
    }

    protected virtual void OnDropped(OlvDropEventArgs args)
    {
      if (this.Dropped == null)
        return;
      this.Dropped((object) this, args);
    }

    protected virtual void OnFilter(FilterEventArgs e)
    {
      if (this.Filter == null)
        return;
      this.Filter((object) this, e);
    }

    protected virtual void OnFormatCell(FormatCellEventArgs args)
    {
      if (this.FormatCell == null)
        return;
      this.FormatCell((object) this, args);
    }

    protected virtual void OnFormatRow(FormatRowEventArgs args)
    {
      if (this.FormatRow == null)
        return;
      this.FormatRow((object) this, args);
    }

    protected virtual void OnFreezing(FreezeEventArgs args)
    {
      if (this.Freezing == null)
        return;
      this.Freezing((object) this, args);
    }

    protected virtual void OnGroupExpandingCollapsing(GroupExpandingCollapsingEventArgs args)
    {
      if (this.GroupExpandingCollapsing == null)
        return;
      this.GroupExpandingCollapsing((object) this, args);
    }

    protected virtual void OnGroupStateChanged(GroupStateChangedEventArgs args)
    {
      if (this.GroupStateChanged == null)
        return;
      this.GroupStateChanged((object) this, args);
    }

    protected virtual void OnHeaderCheckBoxChanging(HeaderCheckBoxChangingEventArgs args)
    {
      if (this.HeaderCheckBoxChanging == null)
        return;
      this.HeaderCheckBoxChanging((object) this, args);
    }

    protected virtual void OnHeaderToolTip(ToolTipShowingEventArgs args)
    {
      if (this.HeaderToolTipShowing == null)
        return;
      this.HeaderToolTipShowing((object) this, args);
    }

    protected virtual void OnHotItemChanged(HotItemChangedEventArgs e)
    {
      if (this.HotItemChanged == null)
        return;
      this.HotItemChanged((object) this, e);
    }

    protected virtual void OnHyperlinkClicked(HyperlinkClickedEventArgs e)
    {
      if (this.HyperlinkClicked == null)
        return;
      this.HyperlinkClicked((object) this, e);
    }

    protected virtual void OnGroupTaskClicked(GroupTaskClickedEventArgs e)
    {
      if (this.GroupTaskClicked == null)
        return;
      this.GroupTaskClicked((object) this, e);
    }

    protected virtual void OnIsHyperlink(IsHyperlinkEventArgs e)
    {
      if (this.IsHyperlink == null)
        return;
      this.IsHyperlink((object) this, e);
    }

    protected virtual void OnItemsAdding(ItemsAddingEventArgs e)
    {
      if (this.ItemsAdding == null)
        return;
      this.ItemsAdding((object) this, e);
    }

    protected virtual void OnItemsChanged(ItemsChangedEventArgs e)
    {
      if (this.ItemsChanged == null)
        return;
      this.ItemsChanged((object) this, e);
    }

    protected virtual void OnItemsChanging(ItemsChangingEventArgs e)
    {
      if (this.ItemsChanging == null)
        return;
      this.ItemsChanging((object) this, e);
    }

    protected virtual void OnItemsRemoving(ItemsRemovingEventArgs e)
    {
      if (this.ItemsRemoving == null)
        return;
      this.ItemsRemoving((object) this, e);
    }

    protected virtual void OnModelCanDrop(ModelDropEventArgs args)
    {
      if (this.ModelCanDrop == null)
        return;
      this.ModelCanDrop((object) this, args);
    }

    protected virtual void OnModelDropped(ModelDropEventArgs args)
    {
      if (this.ModelDropped == null)
        return;
      this.ModelDropped((object) this, args);
    }

    protected virtual void OnSelectionChanged(EventArgs e)
    {
      if (this.SelectionChanged == null)
        return;
      this.SelectionChanged((object) this, e);
    }

    protected virtual void OnScroll(ScrollEventArgs e)
    {
      if (this.Scroll == null)
        return;
      this.Scroll((object) this, e);
    }

    protected virtual void OnCellEditStarting(CellEditEventArgs e)
    {
      if (this.CellEditStarting == null)
        return;
      this.CellEditStarting((object) this, e);
    }

    protected virtual void OnCellEditorValidating(CellEditEventArgs e)
    {
      if (Environment.TickCount - this.lastValidatingEvent < 100)
      {
        e.Cancel = true;
      }
      else
      {
        this.lastValidatingEvent = Environment.TickCount;
        if (this.CellEditValidating != null)
          this.CellEditValidating((object) this, e);
      }
      this.lastValidatingEvent = Environment.TickCount;
    }

    protected virtual void OnCellEditFinishing(CellEditEventArgs e)
    {
      if (this.CellEditFinishing == null)
        return;
      this.CellEditFinishing((object) this, e);
    }

    public ObjectListView()
    {
      this.ColumnClick += new ColumnClickEventHandler(this.HandleColumnClick);
      this.Layout += new LayoutEventHandler(this.HandleLayout);
      this.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.HandleColumnWidthChanging);
      this.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.HandleColumnWidthChanged);
      base.View = View.Details;
      this.DoubleBuffered = true;
      this.ShowSortIndicators = true;
      this.InitializeStandardOverlays();
      this.InitializeEmptyListMsgOverlay();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      foreach (GlassPanelForm glassPanel in this.glassPanels)
      {
        glassPanel.Unbind();
        glassPanel.Dispose();
      }
      this.glassPanels.Clear();
      this.UnsubscribeNotifications((IEnumerable) null);
    }

    public static bool IsVistaOrLater
    {
      get
      {
        if (!ObjectListView.sIsVistaOrLater.HasValue)
          ObjectListView.sIsVistaOrLater = new bool?(Environment.OSVersion.Version.Major >= 6);
        return ObjectListView.sIsVistaOrLater.Value;
      }
    }

    public static bool IsWin7OrLater
    {
      get
      {
        if (!ObjectListView.sIsWin7OrLater.HasValue)
        {
          Version version = Environment.OSVersion.Version;
          ObjectListView.sIsWin7OrLater = new bool?(version.Major > 6 || version.Major == 6 && version.Minor > 0);
        }
        return ObjectListView.sIsWin7OrLater.Value;
      }
    }

    public static SmoothingMode SmoothingMode
    {
      get => ObjectListView.sSmoothingMode;
      set => ObjectListView.sSmoothingMode = value;
    }

    public static TextRenderingHint TextRenderingHint
    {
      get => ObjectListView.sTextRendereringHint;
      set => ObjectListView.sTextRendereringHint = value;
    }

    public static string GroupTitleDefault
    {
      get => ObjectListView.sGroupTitleDefault;
      set => ObjectListView.sGroupTitleDefault = value ?? "{null}";
    }

    public static ArrayList EnumerableToArray(IEnumerable collection, bool alwaysCreate)
    {
      if (collection == null)
        return new ArrayList();
      if (!alwaysCreate)
      {
        if (collection is ArrayList arrayList2)
          return arrayList2;
        if (collection is IList list2)
          return ArrayList.Adapter(list2);
      }
      if (collection is ICollection c)
        return new ArrayList(c);
      ArrayList arrayList3 = new ArrayList();
      foreach (object obj in collection)
        arrayList3.Add(obj);
      return arrayList3;
    }

    public static int EnumerableCount(IEnumerable collection)
    {
      if (collection == null)
        return 0;
      if (collection is ICollection collection1)
        return collection1.Count;
      int num = 0;
      foreach (object obj in collection)
        ++num;
      return num;
    }

    public static bool IsEnumerableEmpty(IEnumerable collection)
    {
      int num;
      switch (collection)
      {
        case null:
        case string _:
          num = 1;
          break;
        default:
          num = !collection.GetEnumerator().MoveNext() ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public static bool IgnoreMissingAspects
    {
      get => Munger.IgnoreMissingAspects;
      set => Munger.IgnoreMissingAspects = value;
    }

    public static bool ShowCellPaddingBounds
    {
      get => ObjectListView.sShowCellPaddingBounds;
      set => ObjectListView.sShowCellPaddingBounds = value;
    }

    public static SimpleItemStyle DefaultDisabledItemStyle
    {
      get
      {
        if (ObjectListView.sDefaultDisabledItemStyle == null)
        {
          ObjectListView.sDefaultDisabledItemStyle = new SimpleItemStyle();
          ObjectListView.sDefaultDisabledItemStyle.ForeColor = Color.DarkGray;
        }
        return ObjectListView.sDefaultDisabledItemStyle;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IModelFilter AdditionalFilter
    {
      get => this.additionalFilter;
      set
      {
        if (this.additionalFilter == value)
          return;
        this.additionalFilter = value;
        this.UpdateColumnFiltering();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public virtual List<OLVColumn> AllColumns
    {
      get => this.allColumns;
      set => this.allColumns = value ?? new List<OLVColumn>();
    }

    [Category("ObjectListView")]
    [Description("If using alternate colors, what color should the background of alterate rows be?")]
    [DefaultValue(typeof (Color), "")]
    public Color AlternateRowBackColor
    {
      get => this.alternateRowBackColor;
      set => this.alternateRowBackColor = value;
    }

    [Browsable(false)]
    public virtual Color AlternateRowBackColorOrDefault => this.alternateRowBackColor == Color.Empty ? Color.LemonChiffon : this.alternateRowBackColor;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OLVColumn AlwaysGroupByColumn
    {
      get => this.alwaysGroupByColumn;
      set => this.alwaysGroupByColumn = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual SortOrder AlwaysGroupBySortOrder
    {
      get => this.alwaysGroupBySortOrder;
      set => this.alwaysGroupBySortOrder = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual ImageList BaseSmallImageList
    {
      get => base.SmallImageList;
      set => base.SmallImageList = value;
    }

    [Category("ObjectListView")]
    [Description("How does the user indicate that they want to edit a cell?")]
    [DefaultValue(ObjectListView.CellEditActivateMode.None)]
    public virtual ObjectListView.CellEditActivateMode CellEditActivation
    {
      get => this.cellEditActivation;
      set
      {
        this.cellEditActivation = value;
        if (!this.Created)
          return;
        this.Invalidate();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public CellEditKeyEngine CellEditKeyEngine
    {
      get => this.cellEditKeyEngine ?? (this.cellEditKeyEngine = new CellEditKeyEngine());
      set => this.cellEditKeyEngine = value;
    }

    [Browsable(false)]
    public Control CellEditor => this.cellEditor;

    [Category("ObjectListView")]
    [Description("Should Tab/Shift-Tab change rows while cell editing?")]
    [DefaultValue(false)]
    public virtual bool CellEditTabChangesRows
    {
      get => this.cellEditTabChangesRows;
      set
      {
        this.cellEditTabChangesRows = value;
        if (this.cellEditTabChangesRows)
        {
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Tab, CellEditCharacterBehaviour.ChangeColumnRight, CellEditAtEdgeBehaviour.ChangeRow);
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Tab | Keys.Shift, CellEditCharacterBehaviour.ChangeColumnLeft, CellEditAtEdgeBehaviour.ChangeRow);
        }
        else
        {
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Tab, CellEditCharacterBehaviour.ChangeColumnRight, CellEditAtEdgeBehaviour.Wrap);
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Tab | Keys.Shift, CellEditCharacterBehaviour.ChangeColumnLeft, CellEditAtEdgeBehaviour.Wrap);
        }
      }
    }

    [Category("ObjectListView")]
    [Description("Should Enter change rows while cell editing?")]
    [DefaultValue(false)]
    public virtual bool CellEditEnterChangesRows
    {
      get => this.cellEditEnterChangesRows;
      set
      {
        this.cellEditEnterChangesRows = value;
        if (this.cellEditEnterChangesRows)
        {
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Return, CellEditCharacterBehaviour.ChangeRowDown, CellEditAtEdgeBehaviour.ChangeColumn);
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Return | Keys.Shift, CellEditCharacterBehaviour.ChangeRowUp, CellEditAtEdgeBehaviour.ChangeColumn);
        }
        else
        {
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Return, CellEditCharacterBehaviour.EndEdit, CellEditAtEdgeBehaviour.EndEdit);
          this.CellEditKeyEngine.SetKeyBehaviour(Keys.Return | Keys.Shift, CellEditCharacterBehaviour.EndEdit, CellEditAtEdgeBehaviour.EndEdit);
        }
      }
    }

    [Browsable(false)]
    public ToolTipControl CellToolTip
    {
      get
      {
        if (this.cellToolTip == null)
          this.CreateCellToolTip();
        return this.cellToolTip;
      }
    }

    [Category("ObjectListView")]
    [Description("How much padding will be applied to each cell in this control?")]
    [DefaultValue(null)]
    public Rectangle? CellPadding
    {
      get => this.cellPadding;
      set => this.cellPadding = value;
    }

    [Category("ObjectListView")]
    [Description("How will cell values be vertically aligned?")]
    [DefaultValue(StringAlignment.Center)]
    public virtual StringAlignment CellVerticalAlignment
    {
      get => this.cellVerticalAlignment;
      set => this.cellVerticalAlignment = value;
    }

    public new bool CheckBoxes
    {
      get => base.CheckBoxes;
      set
      {
        base.CheckBoxes = value;
        this.InitializeStateImageList();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual object CheckedObject
    {
      get
      {
        IList checkedObjects = this.CheckedObjects;
        return checkedObjects.Count == 1 ? checkedObjects[0] : (object) null;
      }
      set => this.CheckedObjects = (IList) new ArrayList((ICollection) new object[1]
      {
        value
      });
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IList CheckedObjects
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        if (this.CheckBoxes)
        {
          for (int index = 0; index < this.GetItemCount(); ++index)
          {
            OLVListItem olvListItem = this.GetItem(index);
            if (olvListItem.CheckState == CheckState.Checked)
              arrayList.Add(olvListItem.RowObject);
          }
        }
        return (IList) arrayList;
      }
      set
      {
        if (!this.CheckBoxes)
          return;
        Stopwatch.StartNew();
        Hashtable hashtable = new Hashtable(this.GetItemCount());
        if (value != null)
        {
          foreach (object key in (IEnumerable) value)
            hashtable[key] = (object) true;
        }
        this.BeginUpdate();
        foreach (object obj in this.Objects)
          this.SetObjectCheckedness(obj, hashtable.ContainsKey(obj) ? CheckState.Checked : CheckState.Unchecked);
        this.EndUpdate();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable CheckedObjectsEnumerable
    {
      get => (IEnumerable) this.CheckedObjects;
      set => this.CheckedObjects = (IList) ObjectListView.EnumerableToArray(value, true);
    }

    [Editor("BrightIdeasSoftware.Design.OLVColumnCollectionEditor", "System.Drawing.Design.UITypeEditor")]
    public new System.Windows.Forms.ListView.ColumnHeaderCollection Columns => base.Columns;

    [Browsable(false)]
    [Obsolete("Use GetFilteredColumns() and OLVColumn.IsTileViewColumn instead")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<OLVColumn> ColumnsForTileView => this.GetFilteredColumns(View.Tile);

    [Browsable(false)]
    public virtual List<OLVColumn> ColumnsInDisplayOrder
    {
      get
      {
        OLVColumn[] olvColumnArray = new OLVColumn[this.Columns.Count];
        foreach (OLVColumn column in this.Columns)
          olvColumnArray[column.DisplayIndex] = column;
        return new List<OLVColumn>((IEnumerable<OLVColumn>) olvColumnArray);
      }
    }

    [Browsable(false)]
    public Rectangle ContentRectangle
    {
      get
      {
        Rectangle clientRectangle = this.ClientRectangle;
        if ((this.View == View.Details || this.ShowHeaderInAllViews) && this.HeaderControl != null)
        {
          Rectangle r = new Rectangle();
          NativeMethods.GetClientRect(this.HeaderControl.Handle, ref r);
          clientRectangle.Y = r.Height;
          clientRectangle.Height -= r.Height;
        }
        return clientRectangle;
      }
    }

    [Category("ObjectListView")]
    [Description("Should the control copy the selection to the clipboard when the user presses Ctrl-C?")]
    [DefaultValue(true)]
    public virtual bool CopySelectionOnControlC
    {
      get => this.copySelectionOnControlC;
      set => this.copySelectionOnControlC = value;
    }

    [Category("ObjectListView")]
    [Description("Should the Ctrl-C copy process use the DragSource to create the Clipboard data object?")]
    [DefaultValue(true)]
    public bool CopySelectionOnControlCUsesDragSource
    {
      get => this.copySelectionOnControlCUsesDragSource;
      set => this.copySelectionOnControlCUsesDragSource = value;
    }

    [Browsable(false)]
    protected IList<IDecoration> Decorations => (IList<IDecoration>) this.decorations;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IRenderer DefaultRenderer
    {
      get => this.defaultRenderer;
      set => this.defaultRenderer = value ?? (IRenderer) new BaseRenderer();
    }

    [Category("ObjectListView")]
    [Description("The style that will be applied to disabled items")]
    [DefaultValue(null)]
    public SimpleItemStyle DisabledItemStyle
    {
      get => this.disabledItemStyle;
      set => this.disabledItemStyle = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable DisabledObjects
    {
      get => (IEnumerable) this.disabledObjects.Keys;
      set
      {
        this.disabledObjects.Clear();
        this.DisableObjects(value);
      }
    }

    public bool IsDisabled(object model) => model != null && this.disabledObjects.ContainsKey(model);

    public void DisableObject(object model) => this.DisableObjects((IEnumerable) new ArrayList()
    {
      model
    });

    public void DisableObjects(IEnumerable models)
    {
      if (models == null)
        return;
      ArrayList array = ObjectListView.EnumerableToArray(models, false);
      foreach (object obj in array)
      {
        if (obj != null)
        {
          this.disabledObjects[obj] = (object) true;
          int index = this.IndexOf(obj);
          if (index >= 0)
            NativeMethods.DeselectOneItem((System.Windows.Forms.ListView) this, index);
        }
      }
      this.RefreshObjects((IList) array);
    }

    public void EnableObject(object model)
    {
      this.disabledObjects.Remove(model);
      this.RefreshObject(model);
    }

    public void EnableObjects(IEnumerable models)
    {
      if (models == null)
        return;
      ArrayList array = ObjectListView.EnumerableToArray(models, false);
      foreach (object key in array)
      {
        if (key != null)
          this.disabledObjects.Remove(key);
      }
      this.RefreshObjects((IList) array);
    }

    protected void ClearDisabledObjects() => this.disabledObjects.Clear();

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IDragSource DragSource
    {
      get => this.dragSource;
      set => this.dragSource = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IDropSink DropSink
    {
      get => this.dropSink;
      set
      {
        if (this.dropSink == value)
          return;
        if (this.dropSink is SimpleDropSink dropSink)
        {
          dropSink.CanDrop -= new EventHandler<OlvDropEventArgs>(this.DropSinkCanDrop);
          dropSink.Dropped -= new EventHandler<OlvDropEventArgs>(this.DropSinkDropped);
          dropSink.ModelCanDrop -= new EventHandler<ModelDropEventArgs>(this.DropSinkModelCanDrop);
          dropSink.ModelDropped -= new EventHandler<ModelDropEventArgs>(this.DropSinkModelDropped);
        }
        this.dropSink = value;
        this.AllowDrop = value != null;
        if (this.dropSink != null)
          this.dropSink.ListView = this;
        if (!(value is SimpleDropSink simpleDropSink))
          return;
        simpleDropSink.CanDrop += new EventHandler<OlvDropEventArgs>(this.DropSinkCanDrop);
        simpleDropSink.Dropped += new EventHandler<OlvDropEventArgs>(this.DropSinkDropped);
        simpleDropSink.ModelCanDrop += new EventHandler<ModelDropEventArgs>(this.DropSinkModelCanDrop);
        simpleDropSink.ModelDropped += new EventHandler<ModelDropEventArgs>(this.DropSinkModelDropped);
      }
    }

    private void DropSinkCanDrop(object sender, OlvDropEventArgs e) => this.OnCanDrop(e);

    private void DropSinkDropped(object sender, OlvDropEventArgs e) => this.OnDropped(e);

    private void DropSinkModelCanDrop(object sender, ModelDropEventArgs e) => this.OnModelCanDrop(e);

    private void DropSinkModelDropped(object sender, ModelDropEventArgs e) => this.OnModelDropped(e);

    [Category("ObjectListView")]
    [Description("When the list has no items, show this message in the control")]
    [DefaultValue(null)]
    [Localizable(true)]
    public virtual string EmptyListMsg
    {
      get => !(this.EmptyListMsgOverlay is TextOverlay emptyListMsgOverlay) ? (string) null : emptyListMsgOverlay.Text;
      set
      {
        if (!(this.EmptyListMsgOverlay is TextOverlay emptyListMsgOverlay))
          return;
        emptyListMsgOverlay.Text = value;
        this.Invalidate();
      }
    }

    [Category("ObjectListView")]
    [Description("What font should the 'list empty' message be drawn in?")]
    [DefaultValue(null)]
    public virtual Font EmptyListMsgFont
    {
      get => !(this.EmptyListMsgOverlay is TextOverlay emptyListMsgOverlay) ? (Font) null : emptyListMsgOverlay.Font;
      set
      {
        if (!(this.EmptyListMsgOverlay is TextOverlay emptyListMsgOverlay))
          return;
        emptyListMsgOverlay.Font = value;
      }
    }

    [Browsable(false)]
    public virtual Font EmptyListMsgFontOrDefault => this.EmptyListMsgFont ?? new Font("Tahoma", 14f);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IOverlay EmptyListMsgOverlay
    {
      get => this.emptyListMsgOverlay;
      set
      {
        if (this.emptyListMsgOverlay == value)
          return;
        this.emptyListMsgOverlay = value;
        this.Invalidate();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable FilteredObjects => this.IsFiltering ? this.FilterObjects(this.Objects, this.ModelFilter, this.ListFilter) : this.Objects;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FilterMenuBuilder FilterMenuBuildStrategy
    {
      get => this.filterMenuBuilder;
      set => this.filterMenuBuilder = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new ListViewGroupCollection Groups => base.Groups;

    [Category("ObjectListView")]
    [Description("The image list from which group header will take their images")]
    [DefaultValue(null)]
    public ImageList GroupImageList
    {
      get => this.groupImageList;
      set
      {
        this.groupImageList = value;
        if (!this.Created)
          return;
        NativeMethods.SetGroupImageList(this, value);
      }
    }

    [Category("ObjectListView")]
    [Description("The format to use when suffixing item counts to group titles")]
    [DefaultValue(null)]
    [Localizable(true)]
    public virtual string GroupWithItemCountFormat
    {
      get => this.groupWithItemCountFormat;
      set => this.groupWithItemCountFormat = value;
    }

    [Browsable(false)]
    public virtual string GroupWithItemCountFormatOrDefault => string.IsNullOrEmpty(this.GroupWithItemCountFormat) ? "{0} [{1} items]" : this.GroupWithItemCountFormat;

    [Category("ObjectListView")]
    [Description("The format to use when suffixing item counts to group titles")]
    [DefaultValue(null)]
    [Localizable(true)]
    public virtual string GroupWithItemCountSingularFormat
    {
      get => this.groupWithItemCountSingularFormat;
      set => this.groupWithItemCountSingularFormat = value;
    }

    [Browsable(false)]
    public virtual string GroupWithItemCountSingularFormatOrDefault => string.IsNullOrEmpty(this.GroupWithItemCountSingularFormat) ? "{0} [{1} item]" : this.GroupWithItemCountSingularFormat;

    [Browsable(true)]
    [Category("ObjectListView")]
    [Description("Should the groups in this control be collapsible (Vista and later only).")]
    [DefaultValue(true)]
    public bool HasCollapsibleGroups
    {
      get => this.hasCollapsibleGroups;
      set => this.hasCollapsibleGroups = value;
    }

    [Browsable(false)]
    public virtual bool HasEmptyListMsg => !string.IsNullOrEmpty(this.EmptyListMsg);

    [Browsable(false)]
    public bool HasOverlays => this.Overlays.Count > 2 || this.imageOverlay.Image != null || !string.IsNullOrEmpty(this.textOverlay.Text);

    [Browsable(false)]
    public HeaderControl HeaderControl => this.headerControl ?? (this.headerControl = new HeaderControl(this));

    [DefaultValue(null)]
    [Browsable(false)]
    [Obsolete("Use a HeaderFormatStyle instead", false)]
    public Font HeaderFont
    {
      get => this.HeaderFormatStyle == null ? (Font) null : this.HeaderFormatStyle.Normal.Font;
      set
      {
        if (value == null && this.HeaderFormatStyle == null)
          return;
        if (this.HeaderFormatStyle == null)
          this.HeaderFormatStyle = new HeaderFormatStyle();
        this.HeaderFormatStyle.SetFont(value);
      }
    }

    [Category("ObjectListView")]
    [Description("What style will be used to draw the control's header")]
    [DefaultValue(null)]
    public HeaderFormatStyle HeaderFormatStyle
    {
      get => this.headerFormatStyle;
      set => this.headerFormatStyle = value;
    }

    [Category("ObjectListView")]
    [Description("What is the maximum height of the header? -1 means no maximum")]
    [DefaultValue(-1)]
    public int HeaderMaximumHeight
    {
      get => this.headerMaximumHeight;
      set => this.headerMaximumHeight = value;
    }

    [Category("ObjectListView")]
    [Description("Will the column headers be drawn strictly according to OS theme?")]
    [DefaultValue(false)]
    public bool HeaderUsesThemes
    {
      get => this.headerUsesThemes;
      set => this.headerUsesThemes = value;
    }

    [Category("ObjectListView")]
    [Description("Will the text of the column headers be word wrapped?")]
    [DefaultValue(false)]
    public bool HeaderWordWrap
    {
      get => this.headerWordWrap;
      set
      {
        this.headerWordWrap = value;
        if (this.headerControl == null)
          return;
        this.headerControl.WordWrap = value;
      }
    }

    [Browsable(false)]
    public ToolTipControl HeaderToolTip => this.HeaderControl.ToolTip;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int HotRowIndex
    {
      get => this.hotRowIndex;
      protected set => this.hotRowIndex = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int HotColumnIndex
    {
      get => this.hotColumnIndex;
      protected set => this.hotColumnIndex = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual HitTestLocation HotCellHitLocation
    {
      get => this.hotCellHitLocation;
      protected set => this.hotCellHitLocation = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual HitTestLocationEx HotCellHitLocationEx
    {
      get => this.hotCellHitLocationEx;
      protected set => this.hotCellHitLocationEx = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVGroup HotGroup
    {
      get => this.hotGroup;
      internal set => this.hotGroup = value;
    }

    [Browsable(false)]
    [Obsolete("Use HotRowIndex instead", false)]
    public virtual int HotItemIndex => this.HotRowIndex;

    [Category("ObjectListView")]
    [Description("How should the row under the cursor be highlighted")]
    [DefaultValue(null)]
    public virtual HotItemStyle HotItemStyle
    {
      get => this.hotItemStyle;
      set
      {
        if (this.HotItemStyle != null)
          this.RemoveOverlay(this.HotItemStyle.Overlay);
        this.hotItemStyle = value;
        if (this.HotItemStyle == null)
          return;
        this.AddOverlay(this.HotItemStyle.Overlay);
      }
    }

    [Category("ObjectListView")]
    [Description("How should hyperlinks be drawn")]
    [DefaultValue(null)]
    public virtual HyperlinkStyle HyperlinkStyle
    {
      get => this.hyperlinkStyle;
      set => this.hyperlinkStyle = value;
    }

    [Category("ObjectListView")]
    [Description("The background foregroundColor of selected rows when the control is owner drawn")]
    [DefaultValue(typeof (Color), "")]
    public virtual Color HighlightBackgroundColor
    {
      get => this.highlightBackgroundColor;
      set => this.highlightBackgroundColor = value;
    }

    [Browsable(false)]
    public virtual Color HighlightBackgroundColorOrDefault => this.HighlightBackgroundColor.IsEmpty ? SystemColors.Highlight : this.HighlightBackgroundColor;

    [Category("ObjectListView")]
    [Description("The foreground foregroundColor of selected rows when the control is owner drawn")]
    [DefaultValue(typeof (Color), "")]
    public virtual Color HighlightForegroundColor
    {
      get => this.highlightForegroundColor;
      set => this.highlightForegroundColor = value;
    }

    [Browsable(false)]
    public virtual Color HighlightForegroundColorOrDefault => this.HighlightForegroundColor.IsEmpty ? SystemColors.HighlightText : this.HighlightForegroundColor;

    [Category("ObjectListView")]
    [Description("When rows are copied or dragged, will data in hidden columns be included in the text? If this is false, only visible columns will be included.")]
    [DefaultValue(false)]
    public virtual bool IncludeHiddenColumnsInDataTransfer
    {
      get => this.includeHiddenColumnsInDataTransfer;
      set => this.includeHiddenColumnsInDataTransfer = value;
    }

    [Category("ObjectListView")]
    [Description("When rows are copied, will column headers be in the text?.")]
    [DefaultValue(false)]
    public virtual bool IncludeColumnHeadersInCopy
    {
      get => this.includeColumnHeadersInCopy;
      set => this.includeColumnHeadersInCopy = value;
    }

    [Browsable(false)]
    public virtual bool IsCellEditing => this.cellEditor != null;

    [Browsable(false)]
    public virtual bool IsDesignMode => this.DesignMode;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool IsFiltering => this.UseFiltering && (this.ModelFilter != null || this.ListFilter != null);

    [Category("ObjectListView")]
    [Description("When the user types into a list, should the values in the current sort column be searched to find a match?")]
    [DefaultValue(true)]
    public virtual bool IsSearchOnSortColumn
    {
      get => this.isSearchOnSortColumn;
      set => this.isSearchOnSortColumn = value;
    }

    [Category("ObjectListView")]
    [Description("Should this control will use a SimpleDropSink to receive drops.")]
    [DefaultValue(false)]
    public virtual bool IsSimpleDropSink
    {
      get => this.DropSink != null;
      set => this.DropSink = value ? (IDropSink) new SimpleDropSink() : (IDropSink) null;
    }

    [Category("ObjectListView")]
    [Description("Should this control use a SimpleDragSource to initiate drags out from this control")]
    [DefaultValue(false)]
    public virtual bool IsSimpleDragSource
    {
      get => this.DragSource != null;
      set => this.DragSource = value ? (IDragSource) new SimpleDragSource() : (IDragSource) null;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new System.Windows.Forms.ListView.ListViewItemCollection Items => base.Items;

    [Category("ObjectListView")]
    [Description("The owner drawn renderer that draws items when the list is in non-Details view.")]
    [DefaultValue(null)]
    public IRenderer ItemRenderer
    {
      get => this.itemRenderer;
      set => this.itemRenderer = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OLVColumn LastSortColumn
    {
      get => this.PrimarySortColumn;
      set => this.PrimarySortColumn = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual SortOrder LastSortOrder
    {
      get => this.PrimarySortOrder;
      set => this.PrimarySortOrder = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IListFilter ListFilter
    {
      get => this.listFilter;
      set
      {
        this.listFilter = value;
        if (!this.UseFiltering)
          return;
        this.UpdateFiltering();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IModelFilter ModelFilter
    {
      get => this.modelFilter;
      set
      {
        this.modelFilter = value;
        if (!this.UseFiltering)
          return;
        this.UpdateFiltering();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OlvListViewHitTestInfo MouseMoveHitTest
    {
      get => this.mouseMoveHitTest;
      private set => this.mouseMoveHitTest = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList<OLVGroup> OLVGroups
    {
      get => this.olvGroups;
      set => this.olvGroups = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IEnumerable<OLVGroup> CollapsedGroups
    {
      get
      {
        if (this.OLVGroups != null)
        {
          foreach (OLVGroup group in (IEnumerable<OLVGroup>) this.OLVGroups)
          {
            if (group.Collapsed)
              yield return group;
          }
        }
      }
      set
      {
        if (this.OLVGroups == null)
          return;
        Hashtable hashtable = new Hashtable();
        if (value != null)
        {
          foreach (OLVGroup olvGroup in value)
            hashtable[olvGroup.Key] = (object) true;
        }
        foreach (OLVGroup olvGroup in (IEnumerable<OLVGroup>) this.OLVGroups)
          olvGroup.Collapsed = hashtable.ContainsKey(olvGroup.Key);
      }
    }

    [Category("ObjectListView")]
    [Description("Should the DrawColumnHeader event be triggered")]
    [DefaultValue(false)]
    public bool OwnerDrawnHeader
    {
      get => this.ownerDrawnHeader;
      set => this.ownerDrawnHeader = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable Objects
    {
      get => this.objects;
      set => this.SetObjects(value, true);
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable ObjectsForClustering => this.Objects;

    [Category("ObjectListView")]
    [Description("The image that will be drawn over the top of the ListView")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ImageOverlay OverlayImage
    {
      get => this.imageOverlay;
      set
      {
        if (this.imageOverlay == value)
          return;
        this.RemoveOverlay((IOverlay) this.imageOverlay);
        this.imageOverlay = value;
        this.AddOverlay((IOverlay) this.imageOverlay);
      }
    }

    [Category("ObjectListView")]
    [Description("The text that will be drawn over the top of the ListView")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TextOverlay OverlayText
    {
      get => this.textOverlay;
      set
      {
        if (this.textOverlay == value)
          return;
        this.RemoveOverlay((IOverlay) this.textOverlay);
        this.textOverlay = value;
        this.AddOverlay((IOverlay) this.textOverlay);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int OverlayTransparency
    {
      get => this.overlayTransparency;
      set => this.overlayTransparency = Math.Min((int) byte.MaxValue, Math.Max(0, value));
    }

    [Browsable(false)]
    protected IList<IOverlay> Overlays => (IList<IOverlay>) this.overlays;

    [Category("ObjectListView")]
    [Description("Will primary checkboxes persistent their values across list rebuilds")]
    [DefaultValue(true)]
    public virtual bool PersistentCheckBoxes
    {
      get => this.persistentCheckBoxes;
      set
      {
        if (this.persistentCheckBoxes == value)
          return;
        this.persistentCheckBoxes = value;
        this.ClearPersistentCheckState();
      }
    }

    protected Dictionary<object, CheckState> CheckStateMap
    {
      get => this.checkStateMap ?? (this.checkStateMap = new Dictionary<object, CheckState>());
      set => this.checkStateMap = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OLVColumn PrimarySortColumn
    {
      get => this.primarySortColumn;
      set
      {
        this.primarySortColumn = value;
        if (!this.TintSortColumn)
          return;
        this.SelectedColumn = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual SortOrder PrimarySortOrder
    {
      get => this.primarySortOrder;
      set => this.primarySortOrder = value;
    }

    [Category("ObjectListView")]
    [Description("Should non-editable checkboxes be drawn as disabled?")]
    [DefaultValue(false)]
    public virtual bool RenderNonEditableCheckboxesAsDisabled
    {
      get => this.renderNonEditableCheckboxesAsDisabled;
      set => this.renderNonEditableCheckboxesAsDisabled = value;
    }

    [Category("ObjectListView")]
    [Description("Specify the height of each row in pixels. -1 indicates default height")]
    [DefaultValue(-1)]
    public virtual int RowHeight
    {
      get => this.rowHeight;
      set
      {
        this.rowHeight = value >= 1 ? value : -1;
        if (this.DesignMode)
          return;
        this.SetupBaseImageList();
        if (!this.CheckBoxes)
          return;
        this.InitializeStateImageList();
      }
    }

    [Browsable(false)]
    public virtual int RowHeightEffective
    {
      get
      {
        switch (this.View)
        {
          case View.LargeIcon:
            return this.LargeImageList == null ? this.Font.Height : Math.Max(this.LargeImageList.ImageSize.Height, this.Font.Height);
          case View.Details:
          case View.SmallIcon:
          case View.List:
            return Math.Max(this.SmallImageSize.Height, this.Font.Height);
          case View.Tile:
            return this.TileSize.Height;
          default:
            return 0;
        }
      }
    }

    [Browsable(false)]
    public virtual int RowsPerPage => NativeMethods.GetCountPerPage((System.Windows.Forms.ListView) this);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OLVColumn SecondarySortColumn
    {
      get => this.secondarySortColumn;
      set => this.secondarySortColumn = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual SortOrder SecondarySortOrder
    {
      get => this.secondarySortOrder;
      set => this.secondarySortOrder = value;
    }

    [Category("ObjectListView")]
    [Description("Should the control select all rows when the user presses Ctrl-A?")]
    [DefaultValue(true)]
    public virtual bool SelectAllOnControlA
    {
      get => this.selectAllOnControlA;
      set => this.selectAllOnControlA = value;
    }

    [Category("ObjectListView")]
    [Description("When the user right clicks on the column headers, should a menu be presented which will allow them to choose which columns will be shown in the view?")]
    [DefaultValue(true)]
    public virtual bool SelectColumnsOnRightClick
    {
      get => (uint) this.SelectColumnsOnRightClickBehaviour > 0U;
      set
      {
        if (value)
        {
          if (this.SelectColumnsOnRightClickBehaviour != ObjectListView.ColumnSelectBehaviour.None)
            return;
          this.SelectColumnsOnRightClickBehaviour = ObjectListView.ColumnSelectBehaviour.InlineMenu;
        }
        else
          this.SelectColumnsOnRightClickBehaviour = ObjectListView.ColumnSelectBehaviour.None;
      }
    }

    [Category("ObjectListView")]
    [Description("When the user right clicks on the column headers, how will the user be able to select columns?")]
    [DefaultValue(ObjectListView.ColumnSelectBehaviour.InlineMenu)]
    public virtual ObjectListView.ColumnSelectBehaviour SelectColumnsOnRightClickBehaviour
    {
      get => this.selectColumnsOnRightClickBehaviour;
      set => this.selectColumnsOnRightClickBehaviour = value;
    }

    [Category("ObjectListView")]
    [Description("When the column select inline menu is open, should it stay open after an item is selected?")]
    [DefaultValue(true)]
    public virtual bool SelectColumnsMenuStaysOpen
    {
      get => this.selectColumnsMenuStaysOpen;
      set => this.selectColumnsMenuStaysOpen = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public OLVColumn SelectedColumn
    {
      get => this.selectedColumn;
      set
      {
        this.selectedColumn = value;
        if (value == null)
          this.RemoveDecoration((IDecoration) this.selectedColumnDecoration);
        else if (!this.HasDecoration((IDecoration) this.selectedColumnDecoration))
          this.AddDecoration((IDecoration) this.selectedColumnDecoration);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IDecoration SelectedRowDecoration
    {
      get => this.selectedRowDecoration;
      set => this.selectedRowDecoration = value;
    }

    [Category("ObjectListView")]
    [Description("The color that will be used to tint the selected column")]
    [DefaultValue(typeof (Color), "")]
    public virtual Color SelectedColumnTint
    {
      get => this.selectedColumnTint;
      set
      {
        this.selectedColumnTint = value.A == byte.MaxValue ? Color.FromArgb(15, value) : value;
        this.selectedColumnDecoration.Tint = this.selectedColumnTint;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int SelectedIndex
    {
      get => this.SelectedIndices.Count == 1 ? this.SelectedIndices[0] : -1;
      set
      {
        this.SelectedIndices.Clear();
        if (value < 0 || value >= this.Items.Count)
          return;
        this.SelectedIndices.Add(value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual OLVListItem SelectedItem
    {
      get => this.SelectedIndices.Count == 1 ? this.GetItem(this.SelectedIndices[0]) : (OLVListItem) null;
      set
      {
        this.SelectedIndices.Clear();
        if (value == null)
          return;
        this.SelectedIndices.Add(value.Index);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual object SelectedObject
    {
      get => this.SelectedIndices.Count == 1 ? this.GetModelObject(this.SelectedIndices[0]) : (object) null;
      set
      {
        object selectedObject = this.SelectedObject;
        if (selectedObject != null && selectedObject.Equals(value))
          return;
        this.SelectedIndices.Clear();
        this.SelectObject(value, true);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IList SelectedObjects
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        foreach (int selectedIndex in this.SelectedIndices)
          arrayList.Add(this.GetModelObject(selectedIndex));
        return (IList) arrayList;
      }
      set
      {
        this.SelectedIndices.Clear();
        this.SelectObjects(value);
      }
    }

    [Category("ObjectListView")]
    [Description("When the user right clicks on the column headers, should a menu be presented which will allow them to perform common tasks on the listview?")]
    [DefaultValue(false)]
    public virtual bool ShowCommandMenuOnRightClick
    {
      get => this.showCommandMenuOnRightClick;
      set => this.showCommandMenuOnRightClick = value;
    }

    [Category("ObjectListView")]
    [Description("If this is true, right clicking on a column header will show a Filter menu option")]
    [DefaultValue(true)]
    public bool ShowFilterMenuOnRightClick
    {
      get => this.showFilterMenuOnRightClick;
      set => this.showFilterMenuOnRightClick = value;
    }

    [Category("Appearance")]
    [Description("Should the list view show items in groups?")]
    [DefaultValue(true)]
    public new virtual bool ShowGroups
    {
      get => base.ShowGroups;
      set
      {
        this.GroupImageList = this.GroupImageList;
        base.ShowGroups = value;
      }
    }

    [Category("ObjectListView")]
    [Description("Should the list view show sort indicators in the column headers?")]
    [DefaultValue(true)]
    public virtual bool ShowSortIndicators
    {
      get => this.showSortIndicators;
      set => this.showSortIndicators = value;
    }

    [Category("ObjectListView")]
    [Description("Should the list view show images on subitems?")]
    [DefaultValue(false)]
    public virtual bool ShowImagesOnSubItems
    {
      get => this.showImagesOnSubItems;
      set
      {
        this.showImagesOnSubItems = value;
        if (this.Created)
          this.ApplyExtendedStyles();
        if (!value || !this.VirtualMode)
          return;
        this.OwnerDraw = true;
      }
    }

    [Category("ObjectListView")]
    [Description("Will group titles be suffixed with a count of the items in the group?")]
    [DefaultValue(false)]
    public virtual bool ShowItemCountOnGroups
    {
      get => this.showItemCountOnGroups;
      set => this.showItemCountOnGroups = value;
    }

    [Category("ObjectListView")]
    [Description("Will the control will show column headers in all views?")]
    [DefaultValue(true)]
    public bool ShowHeaderInAllViews
    {
      get => ObjectListView.IsVistaOrLater && this.showHeaderInAllViews;
      set
      {
        if (this.showHeaderInAllViews == value)
          return;
        this.showHeaderInAllViews = value;
        if (!this.Created)
          return;
        if (this.showHeaderInAllViews)
          this.ApplyExtendedStyles();
        else
          this.RecreateHandle();
        if (this.View == View.Details)
          return;
        View view = this.View;
        this.View = View.Details;
        this.View = view;
      }
    }

    public new ImageList SmallImageList
    {
      get => this.shadowedImageList;
      set
      {
        this.shadowedImageList = value;
        if (this.UseSubItemCheckBoxes)
          this.SetupSubItemCheckBoxes();
        this.SetupBaseImageList();
      }
    }

    [Browsable(false)]
    public virtual Size SmallImageSize => this.BaseSmallImageList == null ? new Size(16, 16) : this.BaseSmallImageList.ImageSize;

    [Category("ObjectListView")]
    [Description("When the listview is grouped, should the items be sorted by the primary column? If this is false, the items will be sorted by the same column as they are grouped.")]
    [DefaultValue(true)]
    public virtual bool SortGroupItemsByPrimaryColumn
    {
      get => this.sortGroupItemsByPrimaryColumn;
      set => this.sortGroupItemsByPrimaryColumn = value;
    }

    [Category("ObjectListView")]
    [Description("How many pixels of space will be between groups")]
    [DefaultValue(0)]
    public virtual int SpaceBetweenGroups
    {
      get => this.spaceBetweenGroups;
      set
      {
        if (this.spaceBetweenGroups == value)
          return;
        this.spaceBetweenGroups = value;
        this.SetGroupSpacing();
      }
    }

    private void SetGroupSpacing()
    {
      if (!this.IsHandleCreated)
        return;
      NativeMethods.SetGroupMetrics(this, new NativeMethods.LVGROUPMETRICS()
      {
        cbSize = (uint) Marshal.SizeOf(typeof (NativeMethods.LVGROUPMETRICS)),
        mask = 1U,
        Bottom = (uint) this.SpaceBetweenGroups
      });
    }

    [Category("ObjectListView")]
    [Description("Should the sort column show a slight tinting?")]
    [DefaultValue(false)]
    public virtual bool TintSortColumn
    {
      get => this.tintSortColumn;
      set
      {
        this.tintSortColumn = value;
        if (value && this.PrimarySortColumn != null)
          this.SelectedColumn = this.PrimarySortColumn;
        else
          this.SelectedColumn = (OLVColumn) null;
      }
    }

    [Category("ObjectListView")]
    [Description("Should the primary column have a checkbox that behaves as a tri-state checkbox?")]
    [DefaultValue(false)]
    public virtual bool TriStateCheckBoxes
    {
      get => this.triStateCheckBoxes;
      set
      {
        this.triStateCheckBoxes = value;
        if (value && !this.CheckBoxes)
          this.CheckBoxes = true;
        this.InitializeStateImageList();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int TopItemIndex
    {
      get => this.View == View.Details && this.IsHandleCreated ? NativeMethods.GetTopIndex((System.Windows.Forms.ListView) this) : -1;
      set
      {
        int index = Math.Min(value, this.GetItemCount() - 1);
        if (this.View != View.Details || index < 0)
          return;
        try
        {
          this.TopItem = this.Items[index];
          if (this.TopItem == null || this.TopItem.Index == index)
            return;
          this.TopItem = (ListViewItem) this.GetItem(index);
        }
        catch (NullReferenceException ex)
        {
        }
      }
    }

    [Category("ObjectListView")]
    [Description("Should moving the mouse over the header trigger CellOver events?")]
    [DefaultValue(true)]
    public bool TriggerCellOverEventsWhenOverHeader
    {
      get => this.triggerCellOverEventsWhenOverHeader;
      set => this.triggerCellOverEventsWhenOverHeader = value;
    }

    [Category("ObjectListView")]
    [Description("When resizing a column by dragging its divider, should any space filling columns be resized at each mouse move?")]
    [DefaultValue(true)]
    public virtual bool UpdateSpaceFillingColumnsWhenDraggingColumnDivider
    {
      get => this.updateSpaceFillingColumnsWhenDraggingColumnDivider;
      set => this.updateSpaceFillingColumnsWhenDraggingColumnDivider = value;
    }

    [Category("ObjectListView")]
    [Description("The background color of selected rows when the control is owner drawn and doesn't have the focus")]
    [DefaultValue(typeof (Color), "")]
    public virtual Color UnfocusedHighlightBackgroundColor
    {
      get => this.unfocusedHighlightBackgroundColor;
      set => this.unfocusedHighlightBackgroundColor = value;
    }

    [Browsable(false)]
    public virtual Color UnfocusedHighlightBackgroundColorOrDefault => this.UnfocusedHighlightBackgroundColor.IsEmpty ? SystemColors.Control : this.UnfocusedHighlightBackgroundColor;

    [Category("ObjectListView")]
    [Description("The foreground color of selected rows when the control is owner drawn and doesn't have the focus")]
    [DefaultValue(typeof (Color), "")]
    public virtual Color UnfocusedHighlightForegroundColor
    {
      get => this.unfocusedHighlightForegroundColor;
      set => this.unfocusedHighlightForegroundColor = value;
    }

    [Browsable(false)]
    public virtual Color UnfocusedHighlightForegroundColorOrDefault => this.UnfocusedHighlightForegroundColor.IsEmpty ? SystemColors.ControlText : this.UnfocusedHighlightForegroundColor;

    [Category("ObjectListView")]
    [Description("Should the list view use a different backcolor to alternate rows?")]
    [DefaultValue(false)]
    public virtual bool UseAlternatingBackColors
    {
      get => this.useAlternatingBackColors;
      set => this.useAlternatingBackColors = value;
    }

    [Category("ObjectListView")]
    [Description("Should FormatCell events be triggered to every cell that is built?")]
    [DefaultValue(false)]
    public bool UseCellFormatEvents
    {
      get => this.useCellFormatEvents;
      set => this.useCellFormatEvents = value;
    }

    [Category("ObjectListView")]
    [Description("Should the selected row be drawn with non-standard foreground and background colors?")]
    [DefaultValue(false)]
    public bool UseCustomSelectionColors
    {
      get => this.useCustomSelectionColors;
      set
      {
        this.useCustomSelectionColors = value;
        if (!(!this.DesignMode & value))
          return;
        this.OwnerDraw = true;
      }
    }

    [Category("ObjectListView")]
    [Description("Should the list use the same hot item and selection mechanism as Vista?")]
    [DefaultValue(false)]
    public bool UseExplorerTheme
    {
      get => this.useExplorerTheme;
      set
      {
        this.useExplorerTheme = value;
        if (!this.Created)
          return;
        NativeMethods.SetWindowTheme(this.Handle, value ? "explorer" : "", (string) null);
      }
    }

    [Category("ObjectListView")]
    [Description("Should the list enable filtering?")]
    [DefaultValue(false)]
    public virtual bool UseFiltering
    {
      get => this.useFiltering;
      set
      {
        if (this.useFiltering == value)
          return;
        this.useFiltering = value;
        this.UpdateFiltering();
      }
    }

    [Category("ObjectListView")]
    [Description("Should an image be drawn in a column's header when that column is being used for filtering?")]
    [DefaultValue(false)]
    public virtual bool UseFilterIndicator
    {
      get => this.useFilterIndicator;
      set
      {
        if (this.useFilterIndicator == value)
          return;
        this.useFilterIndicator = value;
        if (this.useFilterIndicator)
          this.HeaderUsesThemes = false;
        this.Invalidate();
      }
    }

    [Category("ObjectListView")]
    [Description("Should HotTracking be used? Hot tracking applies special formatting to the row under the cursor")]
    [DefaultValue(false)]
    public bool UseHotItem
    {
      get => this.useHotItem;
      set
      {
        this.useHotItem = value;
        if (this.HotItemStyle == null)
          return;
        if (value)
          this.AddOverlay(this.HotItemStyle.Overlay);
        else
          this.RemoveOverlay(this.HotItemStyle.Overlay);
      }
    }

    [Category("ObjectListView")]
    [Description("Should hyperlinks be shown on this control?")]
    [DefaultValue(false)]
    public bool UseHyperlinks
    {
      get => this.useHyperlinks;
      set
      {
        this.useHyperlinks = value;
        if (!value || this.HyperlinkStyle != null)
          return;
        this.HyperlinkStyle = new HyperlinkStyle();
      }
    }

    [Category("ObjectListView")]
    [Description("Should this control show overlays")]
    [DefaultValue(true)]
    public bool UseOverlays
    {
      get => this.useOverlays;
      set => this.useOverlays = value;
    }

    [Category("ObjectListView")]
    [Description("Should this control be configured to show check boxes on subitems.")]
    [DefaultValue(false)]
    public bool UseSubItemCheckBoxes
    {
      get => this.useSubItemCheckBoxes;
      set
      {
        this.useSubItemCheckBoxes = value;
        if (!value)
          return;
        this.SetupSubItemCheckBoxes();
      }
    }

    [Category("ObjectListView")]
    [Description("Should the list use a translucent selection mechanism (like Vista)")]
    [DefaultValue(false)]
    public bool UseTranslucentSelection
    {
      get => this.useTranslucentSelection;
      set
      {
        this.useTranslucentSelection = value;
        if (value)
        {
          RowBorderDecoration borderDecoration = new RowBorderDecoration();
          borderDecoration.BorderPen = new Pen(Color.FromArgb(154, 223, 251));
          borderDecoration.FillBrush = (Brush) new SolidBrush(Color.FromArgb(48, 163, 217, 225));
          borderDecoration.BoundsPadding = new Size(0, 0);
          borderDecoration.CornerRounding = 6f;
          this.SelectedRowDecoration = (IDecoration) borderDecoration;
        }
        else
          this.SelectedRowDecoration = (IDecoration) null;
      }
    }

    [Category("ObjectListView")]
    [Description("Should the list use a translucent hot row highlighting mechanism (like Vista)")]
    [DefaultValue(false)]
    public bool UseTranslucentHotItem
    {
      get => this.useTranslucentHotItem;
      set
      {
        this.useTranslucentHotItem = value;
        if (value)
        {
          this.HotItemStyle = new HotItemStyle();
          RowBorderDecoration borderDecoration = new RowBorderDecoration();
          borderDecoration.BorderPen = new Pen(Color.FromArgb(154, 223, 251));
          borderDecoration.BoundsPadding = new Size(0, 0);
          borderDecoration.CornerRounding = 6f;
          borderDecoration.FillGradientFrom = new Color?(Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
          borderDecoration.FillGradientTo = new Color?(Color.FromArgb(64, 183, 237, 240));
          this.HotItemStyle.Decoration = (IDecoration) borderDecoration;
        }
        else
          this.HotItemStyle = (HotItemStyle) null;
        this.UseHotItem = value;
      }
    }

    public new View View
    {
      get => base.View;
      set
      {
        if (base.View == value)
          return;
        if (this.Frozen)
        {
          base.View = value;
          this.SetupBaseImageList();
        }
        else
        {
          this.Freeze();
          if (value == View.Tile)
            this.CalculateReasonableTileSize();
          base.View = value;
          this.SetupBaseImageList();
          this.Unfreeze();
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual BooleanCheckStateGetterDelegate BooleanCheckStateGetter
    {
      set
      {
        if (value == null)
          this.CheckStateGetter = (CheckStateGetterDelegate) null;
        else
          this.CheckStateGetter = (CheckStateGetterDelegate) (x => value(x) ? CheckState.Checked : CheckState.Unchecked);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual BooleanCheckStatePutterDelegate BooleanCheckStatePutter
    {
      set
      {
        if (value == null)
          this.CheckStatePutter = (CheckStatePutterDelegate) null;
        else
          this.CheckStatePutter = (CheckStatePutterDelegate) ((x, state) =>
          {
            bool newValue = state == CheckState.Checked;
            return value(x, newValue) ? CheckState.Checked : CheckState.Unchecked;
          });
      }
    }

    [Browsable(false)]
    public virtual bool CanShowGroups => true;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool CanUseApplicationIdle
    {
      get => this.canUseApplicationIdle;
      set => this.canUseApplicationIdle = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual CellToolTipGetterDelegate CellToolTipGetter
    {
      get => this.cellToolTipGetter;
      set => this.cellToolTipGetter = value;
    }

    [Category("ObjectListView")]
    [Description("The name of the property or field that holds the 'checkedness' of the model")]
    [DefaultValue(null)]
    public virtual string CheckedAspectName
    {
      get => this.checkedAspectName;
      set
      {
        this.checkedAspectName = value;
        if (string.IsNullOrEmpty(this.checkedAspectName))
        {
          this.checkedAspectMunger = (Munger) null;
          this.CheckStateGetter = (CheckStateGetterDelegate) null;
          this.CheckStatePutter = (CheckStatePutterDelegate) null;
        }
        else
        {
          this.checkedAspectMunger = new Munger(this.checkedAspectName);
          this.CheckStateGetter = (CheckStateGetterDelegate) (modelObject =>
          {
            bool? nullable = this.checkedAspectMunger.GetValue(modelObject) as bool?;
            return nullable.HasValue ? (nullable.Value ? CheckState.Checked : CheckState.Unchecked) : (this.TriStateCheckBoxes ? CheckState.Indeterminate : CheckState.Unchecked);
          });
          this.CheckStatePutter = (CheckStatePutterDelegate) ((modelObject, newValue) =>
          {
            if (this.TriStateCheckBoxes && newValue == CheckState.Indeterminate)
              this.checkedAspectMunger.PutValue(modelObject, (object) null);
            else
              this.checkedAspectMunger.PutValue(modelObject, (object) (newValue == CheckState.Checked));
            return this.CheckStateGetter(modelObject);
          });
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual CheckStateGetterDelegate CheckStateGetter
    {
      get => this.checkStateGetter;
      set => this.checkStateGetter = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual CheckStatePutterDelegate CheckStatePutter
    {
      get => this.checkStatePutter;
      set => this.checkStatePutter = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual SortDelegate CustomSorter
    {
      get => this.customSorter;
      set => this.customSorter = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual HeaderToolTipGetterDelegate HeaderToolTipGetter
    {
      get => this.headerToolTipGetter;
      set => this.headerToolTipGetter = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual RowFormatterDelegate RowFormatter
    {
      get => this.rowFormatter;
      set => this.rowFormatter = value;
    }

    public virtual void AddObject(object modelObject)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.AddObject(modelObject)));
      else
        this.AddObjects((ICollection) new object[1]
        {
          modelObject
        });
    }

    public virtual void AddObjects(ICollection modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.AddObjects(modelObjects)));
      }
      else
      {
        this.InsertObjects(ObjectListView.EnumerableCount(this.Objects), modelObjects);
        this.Sort(this.PrimarySortColumn, this.PrimarySortOrder);
      }
    }

    public virtual void AutoResizeColumns()
    {
      foreach (OLVColumn column in this.Columns)
        column.Width = -2;
    }

    public virtual void AutoSizeColumns()
    {
      ColumnHeaderAutoResizeStyle headerAutoResize = ColumnHeaderAutoResizeStyle.ColumnContent;
      if (this.GetItemCount() == 0)
        headerAutoResize = ColumnHeaderAutoResizeStyle.HeaderSize;
      foreach (ColumnHeader column in this.Columns)
      {
        switch (column.Width)
        {
          case -1:
            this.AutoResizeColumn(column.Index, ColumnHeaderAutoResizeStyle.HeaderSize);
            break;
          case 0:
            this.AutoResizeColumn(column.Index, headerAutoResize);
            break;
        }
      }
    }

    public virtual void BuildGroups() => this.BuildGroups(this.PrimarySortColumn, this.PrimarySortOrder == SortOrder.None ? SortOrder.Ascending : this.PrimarySortOrder);

    public virtual void BuildGroups(OLVColumn column, SortOrder order)
    {
      if (this.GetItemCount() == 0 || this.Columns.Count == 0)
        return;
      BeforeSortingEventArgs sortingEventArgs = this.BuildBeforeSortingEventArgs(column, order);
      this.OnBeforeSorting(sortingEventArgs);
      if (sortingEventArgs.Canceled)
        return;
      this.BuildGroups(sortingEventArgs.ColumnToGroupBy, sortingEventArgs.GroupByOrder, sortingEventArgs.ColumnToSort, sortingEventArgs.SortOrder, sortingEventArgs.SecondaryColumnToSort, sortingEventArgs.SecondarySortOrder);
      this.OnAfterSorting(new AfterSortingEventArgs(sortingEventArgs));
    }

    private BeforeSortingEventArgs BuildBeforeSortingEventArgs(
      OLVColumn column,
      SortOrder order)
    {
      OLVColumn groupColumn = this.AlwaysGroupByColumn ?? column ?? this.GetColumn(0);
      SortOrder groupOrder = this.AlwaysGroupBySortOrder;
      if (order == SortOrder.None)
      {
        order = this.Sorting;
        if (order == SortOrder.None)
          order = SortOrder.Ascending;
      }
      if (groupOrder == SortOrder.None)
        groupOrder = order;
      BeforeSortingEventArgs sortingEventArgs = new BeforeSortingEventArgs(groupColumn, groupOrder, column, order, this.SecondarySortColumn ?? this.GetColumn(0), this.SecondarySortOrder == SortOrder.None ? order : this.SecondarySortOrder);
      if (column != null)
        sortingEventArgs.Canceled = !column.Sortable;
      return sortingEventArgs;
    }

    public virtual void BuildGroups(
      OLVColumn groupByColumn,
      SortOrder groupByOrder,
      OLVColumn column,
      SortOrder order,
      OLVColumn secondaryColumn,
      SortOrder secondaryOrder)
    {
      if (groupByColumn == null)
        return;
      int count = this.Items.Count;
      GroupingParameters parms = this.CollectGroupingParameters(groupByColumn, groupByOrder, column, order, secondaryColumn, secondaryOrder);
      CreateGroupsEventArgs e = new CreateGroupsEventArgs(parms);
      if (parms.GroupByColumn != null)
        e.Canceled = !parms.GroupByColumn.Groupable;
      this.OnBeforeCreatingGroups(e);
      if (e.Canceled)
        return;
      if (e.Groups == null)
        e.Groups = this.MakeGroups(parms);
      this.OnAboutToCreateGroups(e);
      if (e.Canceled)
        return;
      this.OLVGroups = e.Groups;
      this.CreateGroups((IEnumerable<OLVGroup>) e.Groups);
      this.OnAfterCreatingGroups(e);
      this.lastGroupingParameters = e.Parameters;
    }

    protected virtual GroupingParameters CollectGroupingParameters(
      OLVColumn groupByColumn,
      SortOrder groupByOrder,
      OLVColumn sortByColumn,
      SortOrder sortByOrder,
      OLVColumn secondaryColumn,
      SortOrder secondaryOrder)
    {
      if (!groupByColumn.Groupable && this.lastGroupingParameters != null)
      {
        sortByColumn = groupByColumn;
        sortByOrder = groupByOrder;
        groupByColumn = this.lastGroupingParameters.GroupByColumn;
        groupByOrder = this.lastGroupingParameters.GroupByOrder;
      }
      string titleFormat = this.ShowItemCountOnGroups ? groupByColumn.GroupWithItemCountFormatOrDefault : (string) null;
      string titleSingularFormat = this.ShowItemCountOnGroups ? groupByColumn.GroupWithItemCountSingularFormatOrDefault : (string) null;
      return new GroupingParameters(this, groupByColumn, groupByOrder, sortByColumn, sortByOrder, secondaryColumn, secondaryOrder, titleFormat, titleSingularFormat, this.SortGroupItemsByPrimaryColumn);
    }

    protected virtual IList<OLVGroup> MakeGroups(GroupingParameters parms)
    {
      NullableDictionary<object, List<OLVListItem>> nullableDictionary = new NullableDictionary<object, List<OLVListItem>>();
      foreach (OLVListItem olvListItem in parms.ListView.Items)
      {
        object groupKey = parms.GroupByColumn.GetGroupKey(olvListItem.RowObject);
        if (!nullableDictionary.ContainsKey(groupKey))
          nullableDictionary[groupKey] = new List<OLVListItem>();
        nullableDictionary[groupKey].Add(olvListItem);
      }
      OLVColumn col = parms.SortItemsByPrimaryColumn ? parms.ListView.GetColumn(0) : parms.PrimarySort;
      if (col != null && (uint) parms.PrimarySortOrder > 0U)
      {
        IComparer<OLVListItem> comparer = parms.ItemComparer ?? (IComparer<OLVListItem>) new ColumnComparer(col, parms.PrimarySortOrder, parms.SecondarySort, parms.SecondarySortOrder);
        foreach (object key in (IEnumerable) nullableDictionary.Keys)
          nullableDictionary[key].Sort(comparer);
      }
      List<OLVGroup> olvGroupList = new List<OLVGroup>();
      foreach (object key in (IEnumerable) nullableDictionary.Keys)
      {
        string header = parms.GroupByColumn.ConvertGroupKeyToTitle(key);
        if (!string.IsNullOrEmpty(parms.TitleFormat))
        {
          int count = nullableDictionary[key].Count;
          string format = count == 1 ? parms.TitleSingularFormat : parms.TitleFormat;
          try
          {
            header = string.Format(format, (object) header, (object) count);
          }
          catch (FormatException ex)
          {
            header = "Invalid group format: " + format;
          }
        }
        OLVGroup group = new OLVGroup(header);
        group.Collapsible = this.HasCollapsibleGroups;
        group.Key = key;
        group.SortValue = key as IComparable;
        group.Items = (IList<OLVListItem>) nullableDictionary[key];
        if (parms.GroupByColumn.GroupFormatter != null)
          parms.GroupByColumn.GroupFormatter(group, parms);
        olvGroupList.Add(group);
      }
      if ((uint) parms.GroupByOrder > 0U)
        olvGroupList.Sort(parms.GroupComparer ?? (IComparer<OLVGroup>) new OLVGroupComparer(parms.GroupByOrder));
      return (IList<OLVGroup>) olvGroupList;
    }

    public virtual void BuildList()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new MethodInvoker(this.BuildList));
      else
        this.BuildList(true);
    }

    public virtual void BuildList(bool shouldPreserveState)
    {
      if (this.Frozen)
        return;
      Stopwatch.StartNew();
      this.ApplyExtendedStyles();
      this.ClearHotItem();
      int topItemIndex = this.TopItemIndex;
      Point levelScrollPosition = this.LowLevelScrollPosition;
      IList list = (IList) new ArrayList();
      object modelObject = (object) null;
      if (shouldPreserveState && this.objects != null)
      {
        list = this.SelectedObjects;
        if (this.FocusedItem is OLVListItem focusedItem2)
          modelObject = focusedItem2.RowObject;
      }
      IEnumerable filteredObjects = this.FilteredObjects;
      this.BeginUpdate();
      try
      {
        this.Items.Clear();
        this.ListViewItemSorter = (IComparer) null;
        if (filteredObjects != null)
        {
          List<ListViewItem> listViewItemList = new List<ListViewItem>();
          foreach (object rowObject in filteredObjects)
          {
            OLVListItem lvi = new OLVListItem(rowObject);
            this.FillInValues(lvi, rowObject);
            listViewItemList.Add((ListViewItem) lvi);
          }
          this.Items.AddRange(listViewItemList.ToArray());
          this.Sort();
          if (shouldPreserveState)
          {
            this.SelectedObjects = list;
            this.FocusedItem = (ListViewItem) this.ModelToItem(modelObject);
          }
          this.RefreshHotItem();
        }
      }
      finally
      {
        this.EndUpdate();
      }
      if (!shouldPreserveState)
        return;
      this.RefreshHotItem();
      if (this.ShowGroups)
        this.LowLevelScroll(levelScrollPosition.X, levelScrollPosition.Y);
      else
        this.TopItemIndex = topItemIndex;
    }

    public virtual void ClearCachedInfo()
    {
    }

    protected virtual void ApplyExtendedStyles()
    {
      int style = 0;
      if (this.ShowImagesOnSubItems && !this.VirtualMode)
        style ^= 2;
      if (this.ShowHeaderInAllViews)
        style ^= 33554432;
      NativeMethods.SetExtendedStyle((System.Windows.Forms.ListView) this, style, 33554434);
    }

    public virtual void CalculateReasonableTileSize()
    {
      if (this.Columns.Count <= 0)
        return;
      List<OLVColumn> all = this.AllColumns.FindAll((Predicate<OLVColumn>) (x => x.Index == 0 || x.IsTileViewColumn));
      int val1 = this.LargeImageList == null ? 16 : this.LargeImageList.ImageSize.Height;
      int val2 = (this.Font.Height + 1) * all.Count;
      Size tileSize = this.TileSize;
      int num;
      if (tileSize.Width != 0)
      {
        tileSize = this.TileSize;
        num = tileSize.Width;
      }
      else
        num = 200;
      int width = num;
      tileSize = this.TileSize;
      int height = Math.Max(tileSize.Height, Math.Max(val1, val2));
      this.TileSize = new Size(width, height);
    }

    public virtual void ChangeToFilteredColumns(View view)
    {
      this.SuspendSelectionEvents();
      IList selectedObjects = this.SelectedObjects;
      int topItemIndex = this.TopItemIndex;
      this.Freeze();
      this.Clear();
      List<OLVColumn> filteredColumns = this.GetFilteredColumns(view);
      if (view == View.Details || this.ShowHeaderInAllViews)
      {
        for (int index = 0; index < filteredColumns.Count; ++index)
        {
          if (filteredColumns[index].LastDisplayIndex == -1)
            filteredColumns[index].LastDisplayIndex = index;
        }
        List<OLVColumn> olvColumnList = new List<OLVColumn>((IEnumerable<OLVColumn>) filteredColumns);
        olvColumnList.Sort((Comparison<OLVColumn>) ((x, y) => x.LastDisplayIndex - y.LastDisplayIndex));
        int num = 0;
        foreach (ColumnHeader columnHeader in olvColumnList)
          columnHeader.DisplayIndex = num++;
      }
      this.Columns.AddRange((ColumnHeader[]) filteredColumns.ToArray());
      if (view == View.Details || this.ShowHeaderInAllViews)
        this.ShowSortIndicator();
      this.UpdateFiltering();
      this.Unfreeze();
      this.SelectedObjects = selectedObjects;
      this.TopItemIndex = topItemIndex;
      this.ResumeSelectionEvents();
    }

    public virtual void ClearObjects()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new MethodInvoker(this.ClearObjects));
      else
        this.SetObjects((IEnumerable) null);
    }

    public virtual void ClearUrlVisited() => this.visitedUrlMap = new Dictionary<string, bool>();

    public virtual void CopySelectionToClipboard()
    {
      IList selectedObjects = this.SelectedObjects;
      if (selectedObjects.Count == 0)
        return;
      object obj = (object) null;
      if (this.CopySelectionOnControlCUsesDragSource && this.DragSource != null)
        obj = this.DragSource.StartDrag(this, MouseButtons.Left, this.ModelToItem(selectedObjects[0]));
      Clipboard.SetDataObject(obj ?? (object) new OLVDataObject(this, selectedObjects));
    }

    public virtual void CopyObjectsToClipboard(IList objectsToCopy)
    {
      if (objectsToCopy.Count == 0)
        return;
      OLVDataObject olvDataObject = new OLVDataObject(this, objectsToCopy);
      olvDataObject.CreateTextFormats();
      Clipboard.SetDataObject((object) olvDataObject);
    }

    public virtual string ObjectsToHtml(IList objectsToConvert) => objectsToConvert.Count == 0 ? string.Empty : new OLVExporter(this, (IEnumerable) objectsToConvert).ExportTo(OLVExporter.ExportFormat.HTML);

    public virtual void DeselectAll() => NativeMethods.DeselectAllItems((System.Windows.Forms.ListView) this);

    public virtual void EnableCustomSelectionColors() => this.UseCustomSelectionColors = true;

    public virtual OLVListItem GetNextItem(OLVListItem itemToFind)
    {
      if (this.ShowGroups)
      {
        bool flag = itemToFind == null;
        foreach (ListViewGroup group in this.Groups)
        {
          foreach (OLVListItem olvListItem in group.Items)
          {
            if (flag)
              return olvListItem;
            flag = itemToFind == olvListItem;
          }
        }
        return (OLVListItem) null;
      }
      if (this.GetItemCount() == 0)
        return (OLVListItem) null;
      if (itemToFind == null)
        return this.GetItem(0);
      return itemToFind.Index == this.GetItemCount() - 1 ? (OLVListItem) null : this.GetItem(itemToFind.Index + 1);
    }

    public virtual OLVListItem GetLastItemInDisplayOrder()
    {
      if (!this.ShowGroups)
        return this.GetItem(this.GetItemCount() - 1);
      if (this.Groups.Count > 0)
      {
        ListViewGroup group = this.Groups[this.Groups.Count - 1];
        if (group.Items.Count > 0)
          return (OLVListItem) group.Items[group.Items.Count - 1];
      }
      return (OLVListItem) null;
    }

    public virtual OLVListItem GetNthItemInDisplayOrder(int n)
    {
      if (!this.ShowGroups || this.Groups.Count == 0)
        return this.GetItem(n);
      foreach (ListViewGroup group in this.Groups)
      {
        if (n < group.Items.Count)
          return (OLVListItem) group.Items[n];
        n -= group.Items.Count;
      }
      return (OLVListItem) null;
    }

    public virtual int GetDisplayOrderOfItemIndex(int itemIndex)
    {
      if (!this.ShowGroups || this.Groups.Count == 0)
        return itemIndex;
      int num = 0;
      foreach (ListViewGroup group in this.Groups)
      {
        foreach (ListViewItem listViewItem in group.Items)
        {
          if (listViewItem.Index == itemIndex)
            return num;
          ++num;
        }
      }
      return -1;
    }

    public virtual OLVListItem GetPreviousItem(OLVListItem itemToFind)
    {
      if (this.ShowGroups)
      {
        OLVListItem olvListItem1 = (OLVListItem) null;
        foreach (ListViewGroup group in this.Groups)
        {
          foreach (OLVListItem olvListItem2 in group.Items)
          {
            if (olvListItem2 == itemToFind)
              return olvListItem1;
            olvListItem1 = olvListItem2;
          }
        }
        return itemToFind == null ? olvListItem1 : (OLVListItem) null;
      }
      if (this.GetItemCount() == 0)
        return (OLVListItem) null;
      if (itemToFind == null)
        return this.GetItem(this.GetItemCount() - 1);
      return itemToFind.Index == 0 ? (OLVListItem) null : this.GetItem(itemToFind.Index - 1);
    }

    public virtual void InsertObjects(int index, ICollection modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.InsertObjects(index, modelObjects)));
      }
      else
      {
        if (modelObjects == null)
          return;
        this.BeginUpdate();
        try
        {
          ItemsAddingEventArgs e = new ItemsAddingEventArgs(modelObjects);
          this.OnItemsAdding(e);
          if (e.Canceled)
            return;
          modelObjects = e.ObjectsToAdd;
          this.TakeOwnershipOfObjects();
          ArrayList array = ObjectListView.EnumerableToArray(this.Objects, false);
          if (this.IsFiltering)
          {
            index = Math.Max(0, Math.Min(index, array.Count));
            array.InsertRange(index, modelObjects);
            this.BuildList(true);
          }
          else
          {
            this.ListViewItemSorter = (IComparer) null;
            index = Math.Max(0, Math.Min(index, this.GetItemCount()));
            int index1 = index;
            foreach (object modelObject in (IEnumerable) modelObjects)
            {
              if (modelObject != null)
              {
                array.Insert(index1, modelObject);
                OLVListItem lvi = new OLVListItem(modelObject);
                this.FillInValues(lvi, modelObject);
                this.Items.Insert(index1, (ListViewItem) lvi);
                ++index1;
              }
            }
            for (int index2 = index; index2 < this.GetItemCount(); ++index2)
            {
              OLVListItem olvListItem = this.GetItem(index2);
              this.SetSubItemImages(olvListItem.Index, olvListItem);
            }
            this.PostProcessRows();
          }
          this.SubscribeNotifications((IEnumerable) modelObjects);
          this.OnItemsChanged(new ItemsChangedEventArgs());
        }
        finally
        {
          this.EndUpdate();
        }
      }
    }

    public bool IsSelected(object model)
    {
      OLVListItem olvListItem = this.ModelToItem(model);
      return olvListItem != null && olvListItem.Selected;
    }

    public virtual bool IsUrlVisited(string url) => this.visitedUrlMap.ContainsKey(url);

    public void LowLevelScroll(int dx, int dy) => NativeMethods.Scroll((System.Windows.Forms.ListView) this, dx, dy);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Point LowLevelScrollPosition => new Point(NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, true), NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, false));

    public virtual void MarkUrlVisited(string url) => this.visitedUrlMap[url] = true;

    public virtual void MoveObjects(int index, ICollection modelObjects)
    {
      this.TakeOwnershipOfObjects();
      ArrayList array = ObjectListView.EnumerableToArray(this.Objects, false);
      List<int> intList = new List<int>();
      foreach (object modelObject in (IEnumerable) modelObjects)
      {
        if (modelObject != null)
        {
          int num = this.IndexOf(modelObject);
          if (num >= 0)
          {
            intList.Add(num);
            array.Remove(modelObject);
            if (num <= index)
              --index;
          }
        }
      }
      intList.Sort();
      intList.Reverse();
      try
      {
        this.BeginUpdate();
        foreach (int index1 in intList)
          this.Items.RemoveAt(index1);
        this.InsertObjects(index, modelObjects);
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public new ListViewHitTestInfo HitTest(int x, int y)
    {
      try
      {
        return base.HitTest(x, y);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        return new ListViewHitTestInfo((ListViewItem) null, (ListViewItem.ListViewSubItem) null, ListViewHitTestLocations.None);
      }
    }

    protected OlvListViewHitTestInfo LowLevelHitTest(int x, int y)
    {
      if (!this.ClientRectangle.Contains(x, y))
        return new OlvListViewHitTestInfo((OLVListItem) null, (OLVListSubItem) null, 0, (OLVGroup) null, 0);
      OlvListViewHitTestInfo.HeaderHitTestInfo headerHitTestInfo = this.HeaderControl.HitTest(x, y);
      if (headerHitTestInfo != null)
        return new OlvListViewHitTestInfo(this, headerHitTestInfo.ColumnIndex, headerHitTestInfo.IsOverCheckBox, headerHitTestInfo.OverDividerIndex);
      NativeMethods.LVHITTESTINFO hittest = new NativeMethods.LVHITTESTINFO();
      hittest.pt_x = x;
      hittest.pt_y = y;
      int index = NativeMethods.HitTest(this, ref hittest);
      bool flag = (uint) (hittest.flags & -218103808) > 0U;
      OLVListItem olvListItem = flag || index == -1 ? (OLVListItem) null : this.GetItem(index);
      OLVListSubItem subItem = this.View != View.Details || olvListItem == null ? (OLVListSubItem) null : olvListItem.GetSubItem(hittest.iSubItem);
      OLVGroup group = (OLVGroup) null;
      if (this.ShowGroups && this.OLVGroups != null)
      {
        if (this.VirtualMode)
          group = hittest.iGroup < 0 || hittest.iGroup >= this.OLVGroups.Count ? (OLVGroup) null : this.OLVGroups[hittest.iGroup];
        else if (flag)
        {
          foreach (OLVGroup olvGroup in (IEnumerable<OLVGroup>) this.OLVGroups)
          {
            if (olvGroup.GroupId == index)
            {
              group = olvGroup;
              break;
            }
          }
        }
      }
      return new OlvListViewHitTestInfo(olvListItem, subItem, hittest.flags, group, hittest.iSubItem);
    }

    public virtual OlvListViewHitTestInfo OlvHitTest(int x, int y)
    {
      OlvListViewHitTestInfo hti = this.LowLevelHitTest(x, y);
      if (hti.Item == null && !this.FullRowSelect && this.View == View.Details)
      {
        Point scrolledColumnSides = NativeMethods.GetScrolledColumnSides((System.Windows.Forms.ListView) this, 0);
        if (x >= scrolledColumnSides.X && x <= scrolledColumnSides.Y)
        {
          hti = this.LowLevelHitTest(scrolledColumnSides.Y + 4, y);
          if (hti.Item == null)
            hti = this.LowLevelHitTest(scrolledColumnSides.X - 4, y);
          if (hti.Item == null)
            hti = this.LowLevelHitTest(4, y);
          if (hti.Item != null)
          {
            hti.ColumnIndex = 0;
            hti.SubItem = hti.Item.GetSubItem(0);
            hti.Location = ListViewHitTestLocations.None;
            hti.HitTestLocation = HitTestLocation.InCell;
          }
        }
      }
      if (this.OwnerDraw)
        this.CalculateOwnerDrawnHitTest(hti, x, y);
      else
        this.CalculateStandardHitTest(hti, x, y);
      return hti;
    }

    protected virtual void CalculateStandardHitTest(OlvListViewHitTestInfo hti, int x, int y)
    {
      if (this.View != View.Details || hti.ColumnIndex == 0 || hti.SubItem == null || hti.Column == null)
        return;
      Rectangle bounds = hti.SubItem.Bounds;
      bool flag = this.GetActualImageIndex(hti.SubItem.ImageSelector) != -1;
      hti.HitTestLocation = HitTestLocation.InCell;
      Rectangle rectangle1 = bounds;
      rectangle1.Width = this.SmallImageSize.Width;
      if (rectangle1.Contains(x, y))
      {
        if (hti.Column.CheckBoxes)
        {
          hti.HitTestLocation = HitTestLocation.CheckBox;
          return;
        }
        if (flag)
        {
          hti.HitTestLocation = HitTestLocation.Image;
          return;
        }
      }
      Rectangle rectangle2 = bounds;
      rectangle2.X += 4;
      if (flag)
        rectangle2.X += this.SmallImageSize.Width;
      Size proposedSize = new Size(rectangle2.Width, rectangle2.Height);
      Size size = TextRenderer.MeasureText(hti.SubItem.Text, this.Font, proposedSize, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
      rectangle2.Width = size.Width;
      switch (hti.Column.TextAlign)
      {
        case HorizontalAlignment.Right:
          rectangle2.X = bounds.Right - size.Width;
          break;
        case HorizontalAlignment.Center:
          rectangle2.X += (bounds.Right - bounds.Left - size.Width) / 2;
          break;
      }
      if (!rectangle2.Contains(x, y))
        return;
      hti.HitTestLocation = HitTestLocation.Text;
    }

    protected virtual void CalculateOwnerDrawnHitTest(OlvListViewHitTestInfo hti, int x, int y)
    {
      if (hti.Item == null || this.View == View.Details && hti.Column == null)
        return;
      (this.View == View.Details ? hti.Column.Renderer ?? this.DefaultRenderer : this.ItemRenderer)?.HitTest(hti, x, y);
    }

    public virtual void PauseAnimations(bool isPause)
    {
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        if (this.GetColumn(index).Renderer is ImageRenderer renderer1)
          renderer1.Paused = isPause;
      }
    }

    public virtual void RebuildColumns() => this.ChangeToFilteredColumns(this.View);

    public virtual void RemoveObject(object modelObject)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.RemoveObject(modelObject)));
      else
        this.RemoveObjects((ICollection) new object[1]
        {
          modelObject
        });
    }

    public virtual void RemoveObjects(ICollection modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.RemoveObjects(modelObjects)));
      }
      else
      {
        if (modelObjects == null)
          return;
        this.BeginUpdate();
        try
        {
          ItemsRemovingEventArgs e = new ItemsRemovingEventArgs(modelObjects);
          this.OnItemsRemoving(e);
          if (e.Canceled)
            return;
          modelObjects = e.ObjectsToRemove;
          this.TakeOwnershipOfObjects();
          ArrayList array = ObjectListView.EnumerableToArray(this.Objects, false);
          foreach (object modelObject in (IEnumerable) modelObjects)
          {
            if (modelObject != null)
            {
              int index1 = array.IndexOf(modelObject);
              if (index1 >= 0)
                array.RemoveAt(index1);
              int index2 = this.IndexOf(modelObject);
              if (index2 >= 0)
                this.Items.RemoveAt(index2);
            }
          }
          this.PostProcessRows();
          this.UnsubscribeNotifications((IEnumerable) modelObjects);
          this.OnItemsChanged(new ItemsChangedEventArgs());
        }
        finally
        {
          this.EndUpdate();
        }
      }
    }

    public virtual void SelectAll() => NativeMethods.SelectAllItems((System.Windows.Forms.ListView) this);

    public void SetNativeBackgroundWatermark(Image image) => NativeMethods.SetBackgroundImage((System.Windows.Forms.ListView) this, image, true, false, 0, 0);

    public void SetNativeBackgroundImage(Image image, int xOffset, int yOffset) => NativeMethods.SetBackgroundImage((System.Windows.Forms.ListView) this, image, false, false, xOffset, yOffset);

    public void SetNativeBackgroundTiledImage(Image image) => NativeMethods.SetBackgroundImage((System.Windows.Forms.ListView) this, image, false, true, 0, 0);

    public virtual void SetObjects(IEnumerable collection) => this.SetObjects(collection, false);

    public virtual void SetObjects(IEnumerable collection, bool preserveState)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.SetObjects(collection, preserveState)));
      }
      else
      {
        ItemsChangingEventArgs e = new ItemsChangingEventArgs(this.objects, collection);
        this.OnItemsChanging(e);
        if (e.Canceled)
          return;
        collection = e.NewObjects;
        if (this.isOwnerOfObjects && this.objects != collection)
          this.isOwnerOfObjects = false;
        this.objects = collection;
        this.BuildList(preserveState);
        this.UpdateNotificationSubscriptions(this.objects);
        this.OnItemsChanged(new ItemsChangedEventArgs());
      }
    }

    public virtual void UpdateObject(object modelObject)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.UpdateObject(modelObject)));
      else
        this.UpdateObjects((ICollection) new object[1]
        {
          modelObject
        });
    }

    public virtual void UpdateObjects(ICollection modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.UpdateObjects(modelObjects)));
      }
      else
      {
        if (modelObjects == null || modelObjects.Count == 0)
          return;
        this.BeginUpdate();
        try
        {
          this.UnsubscribeNotifications((IEnumerable) modelObjects);
          ArrayList arrayList = new ArrayList();
          this.TakeOwnershipOfObjects();
          ArrayList array = ObjectListView.EnumerableToArray(this.Objects, false);
          foreach (object modelObject in (IEnumerable) modelObjects)
          {
            if (modelObject != null)
            {
              int index = array.IndexOf(modelObject);
              if (index < 0)
              {
                arrayList.Add(modelObject);
              }
              else
              {
                array[index] = modelObject;
                OLVListItem olvi = this.ModelToItem(modelObject);
                if (olvi != null)
                {
                  olvi.RowObject = modelObject;
                  this.RefreshItem(olvi);
                }
              }
            }
          }
          this.PostProcessRows();
          this.AddObjects((ICollection) arrayList);
          this.SubscribeNotifications((IEnumerable) modelObjects);
          this.OnItemsChanged(new ItemsChangedEventArgs());
        }
        finally
        {
          this.EndUpdate();
        }
      }
    }

    protected virtual void UpdateNotificationSubscriptions(IEnumerable collection)
    {
      if (!this.UseNotifyPropertyChanged)
        return;
      this.UnsubscribeNotifications((IEnumerable) null);
      this.SubscribeNotifications(collection ?? this.Objects);
    }

    [Category("ObjectListView")]
    [Description("Should ObjectListView listen for property changed events on the model objects?")]
    [DefaultValue(false)]
    public bool UseNotifyPropertyChanged
    {
      get => this.useNotifyPropertyChanged;
      set
      {
        if (this.useNotifyPropertyChanged == value)
          return;
        this.useNotifyPropertyChanged = value;
        if (value)
          this.SubscribeNotifications(this.Objects);
        else
          this.UnsubscribeNotifications((IEnumerable) null);
      }
    }

    protected void SubscribeNotifications(IEnumerable models)
    {
      if (!this.UseNotifyPropertyChanged || models == null)
        return;
      foreach (object model in models)
      {
        if (model is INotifyPropertyChanged notifyPropertyChanged1 && !this.subscribedModels.ContainsKey((object) notifyPropertyChanged1))
        {
          notifyPropertyChanged1.PropertyChanged += new PropertyChangedEventHandler(this.HandleModelOnPropertyChanged);
          this.subscribedModels[(object) notifyPropertyChanged1] = (object) notifyPropertyChanged1;
        }
      }
    }

    protected void UnsubscribeNotifications(IEnumerable models)
    {
      if (models == null)
      {
        foreach (INotifyPropertyChanged key in (IEnumerable) this.subscribedModels.Keys)
          key.PropertyChanged -= new PropertyChangedEventHandler(this.HandleModelOnPropertyChanged);
        this.subscribedModels = new Hashtable();
      }
      else
      {
        foreach (object model in models)
        {
          if (model is INotifyPropertyChanged notifyPropertyChanged2)
          {
            notifyPropertyChanged2.PropertyChanged -= new PropertyChangedEventHandler(this.HandleModelOnPropertyChanged);
            this.subscribedModels.Remove((object) notifyPropertyChanged2);
          }
        }
      }
    }

    private void HandleModelOnPropertyChanged(
      object sender,
      PropertyChangedEventArgs propertyChangedEventArgs)
    {
      this.RefreshObject(sender);
    }

    public virtual byte[] SaveState()
    {
      ObjectListView.ObjectListViewState objectListViewState = new ObjectListView.ObjectListViewState();
      objectListViewState.VersionNumber = 1;
      objectListViewState.NumberOfColumns = this.AllColumns.Count;
      objectListViewState.CurrentView = this.View;
      if (this.PrimarySortColumn != null)
        objectListViewState.SortColumn = this.AllColumns.IndexOf(this.PrimarySortColumn);
      objectListViewState.LastSortOrder = this.PrimarySortOrder;
      objectListViewState.IsShowingGroups = this.ShowGroups;
      if (this.AllColumns.Count > 0 && this.AllColumns[0].LastDisplayIndex == -1)
        this.RememberDisplayIndicies();
      foreach (OLVColumn allColumn in this.AllColumns)
      {
        objectListViewState.ColumnIsVisible.Add((object) allColumn.IsVisible);
        objectListViewState.ColumnDisplayIndicies.Add((object) allColumn.LastDisplayIndex);
        objectListViewState.ColumnWidths.Add((object) allColumn.Width);
      }
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter()
        {
          AssemblyFormat = FormatterAssemblyStyle.Simple
        }.Serialize((Stream) memoryStream, (object) objectListViewState);
        return memoryStream.ToArray();
      }
    }

    public virtual bool RestoreState(byte[] state)
    {
      using (MemoryStream memoryStream = new MemoryStream(state))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        ObjectListView.ObjectListViewState objectListViewState;
        try
        {
          objectListViewState = binaryFormatter.Deserialize((Stream) memoryStream) as ObjectListView.ObjectListViewState;
        }
        catch (SerializationException ex)
        {
          return false;
        }
        if (objectListViewState == null || objectListViewState.NumberOfColumns != this.AllColumns.Count)
          return false;
        if (objectListViewState.SortColumn == -1)
        {
          this.PrimarySortColumn = (OLVColumn) null;
          this.PrimarySortOrder = SortOrder.None;
        }
        else
        {
          this.PrimarySortColumn = this.AllColumns[objectListViewState.SortColumn];
          this.PrimarySortOrder = objectListViewState.LastSortOrder;
        }
        for (int index = 0; index < objectListViewState.NumberOfColumns; ++index)
        {
          OLVColumn allColumn = this.AllColumns[index];
          allColumn.Width = (int) objectListViewState.ColumnWidths[index];
          allColumn.IsVisible = (bool) objectListViewState.ColumnIsVisible[index];
          allColumn.LastDisplayIndex = (int) objectListViewState.ColumnDisplayIndicies[index];
        }
        if (objectListViewState.IsShowingGroups != this.ShowGroups)
          this.ShowGroups = objectListViewState.IsShowingGroups;
        if (this.View == objectListViewState.CurrentView)
          this.RebuildColumns();
        else
          this.View = objectListViewState.CurrentView;
      }
      return true;
    }

    protected virtual void HandleApplicationIdle(object sender, EventArgs e)
    {
      Application.Idle -= new EventHandler(this.HandleApplicationIdle);
      this.hasIdleHandler = false;
      this.OnSelectionChanged(new EventArgs());
    }

    protected virtual void HandleApplicationIdleResizeColumns(object sender, EventArgs e)
    {
      Application.Idle -= new EventHandler(this.HandleApplicationIdleResizeColumns);
      this.hasResizeColumnsHandler = false;
      this.ResizeFreeSpaceFillingColumns();
    }

    protected virtual bool HandleBeginScroll(ref Message m)
    {
      NativeMethods.NMLVSCROLL lparam = (NativeMethods.NMLVSCROLL) m.GetLParam(typeof (NativeMethods.NMLVSCROLL));
      if ((uint) lparam.dx > 0U)
      {
        int scrollPosition = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, true);
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, scrollPosition - lparam.dx, scrollPosition, ScrollOrientation.HorizontalScroll));
        if (this.GetItemCount() == 0)
          this.Invalidate();
      }
      if ((uint) lparam.dy > 0U)
      {
        int scrollPosition = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, false);
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, scrollPosition - lparam.dy, scrollPosition, ScrollOrientation.VerticalScroll));
      }
      return false;
    }

    protected virtual bool HandleEndScroll(ref Message m)
    {
      if (!ObjectListView.IsVistaOrLater && Control.MouseButtons == MouseButtons.Left && this.GridLines)
      {
        this.Invalidate();
        this.Update();
      }
      return false;
    }

    protected virtual bool HandleLinkClick(ref Message m)
    {
      NativeMethods.NMLVLINK lparam = (NativeMethods.NMLVLINK) m.GetLParam(typeof (NativeMethods.NMLVLINK));
      foreach (OLVGroup olvGroup in (IEnumerable<OLVGroup>) this.OLVGroups)
      {
        if (olvGroup.GroupId == lparam.iSubItem)
        {
          this.OnGroupTaskClicked(new GroupTaskClickedEventArgs(olvGroup));
          return true;
        }
      }
      return false;
    }

    protected virtual void HandleCellToolTipShowing(object sender, ToolTipShowingEventArgs e)
    {
      this.BuildCellEvent((CellEventArgs) e, this.PointToClient(Cursor.Position));
      if (e.Item == null)
        return;
      e.Text = this.GetCellToolTip(e.ColumnIndex, e.RowIndex);
      this.OnCellToolTip(e);
    }

    internal void HeaderToolTipShowingCallback(object sender, ToolTipShowingEventArgs e) => this.HandleHeaderToolTipShowing(sender, e);

    protected virtual void HandleHeaderToolTipShowing(object sender, ToolTipShowingEventArgs e)
    {
      e.ColumnIndex = this.HeaderControl.ColumnIndexUnderCursor;
      if (e.ColumnIndex < 0)
        return;
      e.RowIndex = -1;
      e.Model = (object) null;
      e.Column = this.GetColumn(e.ColumnIndex);
      e.Text = this.GetHeaderToolTip(e.ColumnIndex);
      e.ListView = this;
      this.OnHeaderToolTip(e);
    }

    protected virtual void HandleColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (!this.PossibleFinishCellEditing())
        return;
      this.PrimarySortOrder = this.PrimarySortColumn == null || e.Column != this.PrimarySortColumn.Index ? SortOrder.Ascending : (this.PrimarySortOrder == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending);
      this.BeginUpdate();
      try
      {
        this.Sort(e.Column);
      }
      finally
      {
        this.EndUpdate();
      }
    }

    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case 2:
          if (this.HandleDestroy(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 15:
          if (this.HandlePaint(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 70:
          if (!this.PossibleFinishCellEditing() || this.HandleWindowPosChanging(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 78:
          if (this.HandleNotify(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 123:
          if (this.HandleContextMenu(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 256:
          if (this.HandleKeyDown(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 258:
          if (this.HandleChar(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 276:
        case 277:
          if (!this.PossibleFinishCellEditing())
            break;
          base.WndProc(ref m);
          break;
        case 512:
          if (this.HandleMouseMove(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 513:
          if (!this.PossibleFinishCellEditing() || this.HandleLButtonDown(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 514:
          if (!this.PossibleFinishCellEditing() || this.HandleLButtonUp(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 515:
          if (!this.PossibleFinishCellEditing() || this.HandleLButtonDoubleClick(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 516:
          if (!this.PossibleFinishCellEditing() || this.HandleRButtonDown(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 518:
          if (!this.PossibleFinishCellEditing() || this.HandleRButtonDoubleClick(ref m))
            break;
          base.WndProc(ref m);
          break;
        case 522:
        case 526:
          if (!this.PossibleFinishCellEditing())
            break;
          base.WndProc(ref m);
          break;
        case 8270:
          if (this.HandleReflectNotify(ref m))
            break;
          base.WndProc(ref m);
          break;
        default:
          base.WndProc(ref m);
          break;
      }
    }

    protected virtual bool HandleChar(ref Message m)
    {
      if (this.ProcessKeyEventArgs(ref m))
        return true;
      char int32 = (char) m.WParam.ToInt32();
      if (int32 == '\b')
      {
        this.timeLastCharEvent = 0;
        return true;
      }
      this.lastSearchString = Environment.TickCount >= this.timeLastCharEvent + 1000 ? int32.ToString((IFormatProvider) CultureInfo.InvariantCulture) : this.lastSearchString + int32.ToString();
      if (this.CheckBoxes && this.lastSearchString == " ")
      {
        this.timeLastCharEvent = 0;
        return true;
      }
      int startSearchFrom1 = 0;
      ListViewItem focusedItem = this.FocusedItem;
      if (focusedItem != null)
      {
        startSearchFrom1 = this.GetDisplayOrderOfItemIndex(focusedItem.Index);
        if (this.lastSearchString.Length == 1)
        {
          ++startSearchFrom1;
          if (startSearchFrom1 == this.GetItemCount())
            startSearchFrom1 = 0;
        }
      }
      BeforeSearchingEventArgs e1 = new BeforeSearchingEventArgs(this.lastSearchString, startSearchFrom1);
      this.OnBeforeSearching(e1);
      if (e1.Canceled)
        return true;
      string stringToFind = e1.StringToFind;
      int startSearchFrom2 = e1.StartSearchFrom;
      int matchingRow = this.FindMatchingRow(stringToFind, startSearchFrom2, SearchDirectionHint.Down);
      if (matchingRow >= 0)
      {
        this.BeginUpdate();
        try
        {
          this.SelectedIndices.Clear();
          OLVListItem itemInDisplayOrder = this.GetNthItemInDisplayOrder(matchingRow);
          if (itemInDisplayOrder != null)
          {
            if (itemInDisplayOrder.Enabled)
              itemInDisplayOrder.Selected = true;
            itemInDisplayOrder.Focused = true;
            this.EnsureVisible(itemInDisplayOrder.Index);
          }
        }
        finally
        {
          this.EndUpdate();
        }
      }
      AfterSearchingEventArgs e2 = new AfterSearchingEventArgs(stringToFind, matchingRow);
      this.OnAfterSearching(e2);
      if (!e2.Handled && matchingRow < 0)
        SystemSounds.Beep.Play();
      this.timeLastCharEvent = Environment.TickCount;
      return true;
    }

    protected virtual bool HandleContextMenu(ref Message m)
    {
      if (this.DesignMode || m.LParam == this.minusOne || m.WParam != this.HeaderControl.Handle)
        return false;
      return !this.PossibleFinishCellEditing() || this.HandleHeaderRightClick(this.HeaderControl.ColumnIndexUnderCursor);
    }

    protected virtual bool HandleCustomDraw(ref Message m)
    {
      if (!this.isInWmPaintEvent || !this.shouldDoCustomDrawing)
        return true;
      NativeMethods.NMLVCUSTOMDRAW lparam = (NativeMethods.NMLVCUSTOMDRAW) m.GetLParam(typeof (NativeMethods.NMLVCUSTOMDRAW));
      if (lparam.dwItemType == 1)
        return true;
      switch (lparam.nmcd.dwDrawStage)
      {
        case 1:
          if (this.prePaintLevel == 0)
            this.drawnItems = new List<OLVListItem>();
          this.isAfterItemPaint = this.GetItemCount() == 0;
          ++this.prePaintLevel;
          base.WndProc(ref m);
          m.Result = (IntPtr) ((int) m.Result | 16 | 64);
          return true;
        case 2:
          --this.prePaintLevel;
          if (this.prePaintLevel == 0 && (this.isMarqueSelecting || this.isAfterItemPaint))
          {
            this.shouldDoCustomDrawing = false;
            using (Graphics g = Graphics.FromHdc(lparam.nmcd.hdc))
            {
              this.DrawAllDecorations(g, this.drawnItems);
              break;
            }
          }
          else
            break;
        case 65537:
          this.isAfterItemPaint = true;
          if (this.View == View.Tile)
          {
            if (this.OwnerDraw && this.ItemRenderer != null)
              base.WndProc(ref m);
          }
          else
            base.WndProc(ref m);
          m.Result = (IntPtr) ((int) m.Result | 16 | 64);
          return true;
        case 65538:
          if (this.Columns.Count > 0)
          {
            OLVListItem olvListItem = this.GetItem((int) lparam.nmcd.dwItemSpec);
            if (olvListItem != null)
              this.drawnItems.Add(olvListItem);
            break;
          }
          break;
        case 196609:
          if (!this.OwnerDraw || (uint) lparam.iSubItem > 0U || this.Columns[0].DisplayIndex == 0)
            return false;
          int dwItemSpec = (int) lparam.nmcd.dwItemSpec;
          OLVListItem olvListItem1 = this.GetItem(dwItemSpec);
          if (olvListItem1 == null)
            return false;
          using (Graphics graphics = Graphics.FromHdc(lparam.nmcd.hdc))
          {
            Rectangle subItemBounds = olvListItem1.GetSubItemBounds(0);
            this.OnDrawSubItem(new DrawListViewSubItemEventArgs(graphics, subItemBounds, (ListViewItem) olvListItem1, olvListItem1.SubItems[0], dwItemSpec, 0, this.Columns[0], (ListViewItemStates) lparam.nmcd.uItemState));
          }
          m.Result = (IntPtr) 4;
          return true;
      }
      return false;
    }

    protected virtual bool HandleDestroy(ref Message m)
    {
      this.BeginInvoke((Delegate) (() =>
      {
        this.headerControl = (HeaderControl) null;
        this.HeaderControl.WordWrap = this.HeaderWordWrap;
      }));
      if (this.cellToolTip == null)
        return false;
      this.cellToolTip.PushSettings();
      base.WndProc(ref m);
      this.BeginInvoke((Delegate) (() =>
      {
        this.UpdateCellToolTipHandle();
        this.cellToolTip.PopSettings();
      }));
      return true;
    }

    protected virtual bool HandleFindItem(ref Message m)
    {
      NativeMethods.LVFINDINFO lparam = (NativeMethods.LVFINDINFO) m.GetLParam(typeof (NativeMethods.LVFINDINFO));
      if ((lparam.flags & 2) != 2)
        return false;
      int int32 = m.WParam.ToInt32();
      m.Result = (IntPtr) this.FindMatchingRow(lparam.psz, int32, SearchDirectionHint.Down);
      return true;
    }

    public virtual int FindMatchingRow(string text, int start, SearchDirectionHint direction)
    {
      int itemCount = this.GetItemCount();
      if (itemCount == 0)
        return -1;
      OLVColumn column = this.GetColumn(0);
      if (this.IsSearchOnSortColumn && this.View == View.Details && this.PrimarySortColumn != null)
        column = this.PrimarySortColumn;
      int matchInRange;
      if (direction == SearchDirectionHint.Down)
      {
        matchInRange = this.FindMatchInRange(text, start, itemCount - 1, column);
        if (matchInRange == -1 && start > 0)
          matchInRange = this.FindMatchInRange(text, 0, start - 1, column);
      }
      else
      {
        matchInRange = this.FindMatchInRange(text, start, 0, column);
        if (matchInRange == -1 && start != itemCount)
          matchInRange = this.FindMatchInRange(text, itemCount - 1, start + 1, column);
      }
      return matchInRange;
    }

    protected virtual int FindMatchInRange(string text, int first, int last, OLVColumn column)
    {
      if (first <= last)
      {
        for (int n = first; n <= last; ++n)
        {
          if (column.GetStringValue(this.GetNthItemInDisplayOrder(n).RowObject).StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      else
      {
        for (int n = first; n >= last; --n)
        {
          if (column.GetStringValue(this.GetNthItemInDisplayOrder(n).RowObject).StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
            return n;
        }
      }
      return -1;
    }

    protected virtual bool HandleGroupInfo(ref Message m)
    {
      NativeMethods.NMLVGROUP lparam = (NativeMethods.NMLVGROUP) m.GetLParam(typeof (NativeMethods.NMLVGROUP));
      if (((int) lparam.uOldState & 49) == ((int) lparam.uNewState & 49))
        return false;
      foreach (OLVGroup olvGroup in (IEnumerable<OLVGroup>) this.OLVGroups)
      {
        if (olvGroup.GroupId == lparam.iGroupId)
        {
          this.OnGroupStateChanged(new GroupStateChangedEventArgs(olvGroup, (GroupState) lparam.uOldState, (GroupState) lparam.uNewState));
          break;
        }
      }
      return false;
    }

    protected virtual bool HandleKeyDown(ref Message m)
    {
      if (this.CheckBoxes && m.WParam.ToInt32() == 32 && this.SelectedIndices.Count > 0)
      {
        this.ToggleSelectedRowCheckBoxes();
        return true;
      }
      int scrollPosition1 = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, true);
      int scrollPosition2 = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, false);
      base.WndProc(ref m);
      if (this.IsDisposed)
        return true;
      int scrollPosition3 = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, true);
      int scrollPosition4 = NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, false);
      if (scrollPosition1 != scrollPosition3)
      {
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, scrollPosition1, scrollPosition3, ScrollOrientation.HorizontalScroll));
        this.RefreshHotItem();
      }
      if (scrollPosition2 != scrollPosition4)
      {
        this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, scrollPosition2, scrollPosition4, ScrollOrientation.VerticalScroll));
        this.RefreshHotItem();
      }
      return true;
    }

    private void ToggleSelectedRowCheckBoxes()
    {
      object rowObject = this.GetItem(this.SelectedIndices[0]).RowObject;
      this.ToggleCheckObject(rowObject);
      CheckState? checkState = this.GetCheckState(rowObject);
      if (!checkState.HasValue)
        return;
      foreach (object selectedObject in (IEnumerable) this.SelectedObjects)
        this.SetObjectCheckedness(selectedObject, checkState.Value);
    }

    protected virtual bool HandleLButtonDown(ref Message m) => this.ProcessLButtonDown(this.OlvHitTest(m.LParam.ToInt32() & (int) ushort.MaxValue, m.LParam.ToInt32() >> 16 & (int) ushort.MaxValue));

    protected virtual bool ProcessLButtonDown(OlvListViewHitTestInfo hti)
    {
      if (hti.Item == null || hti.HitTestLocation != HitTestLocation.CheckBox)
        return false;
      if (!hti.Item.Enabled)
        return true;
      if (hti.Column != null && hti.Column.Index > 0)
      {
        if (hti.Column.IsEditable && hti.Item.Enabled)
          this.ToggleSubItemCheckBox(hti.RowObject, hti.Column);
        return true;
      }
      this.ToggleCheckObject(hti.RowObject);
      if (hti.Item.Selected)
      {
        CheckState? checkState = this.GetCheckState(hti.RowObject);
        if (checkState.HasValue)
        {
          foreach (object selectedObject in (IEnumerable) this.SelectedObjects)
            this.SetObjectCheckedness(selectedObject, checkState.Value);
        }
      }
      return true;
    }

    protected virtual bool HandleLButtonUp(ref Message m)
    {
      if (this.MouseMoveHitTest == null)
        return false;
      if (this.MouseMoveHitTest.HitTestLocation == HitTestLocation.GroupExpander && this.TriggerGroupExpandCollapse(this.MouseMoveHitTest.Group))
        return true;
      if (ObjectListView.IsVistaOrLater && this.HasCollapsibleGroups)
        this.DefWndProc(ref m);
      return false;
    }

    protected virtual bool TriggerGroupExpandCollapse(OLVGroup group)
    {
      GroupExpandingCollapsingEventArgs args = new GroupExpandingCollapsingEventArgs(group);
      this.OnGroupExpandingCollapsing(args);
      return args.Canceled;
    }

    protected virtual bool HandleRButtonDown(ref Message m) => this.ProcessRButtonDown(this.OlvHitTest(m.LParam.ToInt32() & (int) ushort.MaxValue, m.LParam.ToInt32() >> 16 & (int) ushort.MaxValue));

    protected virtual bool ProcessRButtonDown(OlvListViewHitTestInfo hti) => hti.Item != null && hti.HitTestLocation == HitTestLocation.CheckBox;

    protected virtual bool HandleLButtonDoubleClick(ref Message m) => this.ProcessLButtonDoubleClick(this.OlvHitTest(m.LParam.ToInt32() & (int) ushort.MaxValue, m.LParam.ToInt32() >> 16 & (int) ushort.MaxValue));

    protected virtual bool ProcessLButtonDoubleClick(OlvListViewHitTestInfo hti) => hti.HitTestLocation == HitTestLocation.CheckBox;

    protected virtual bool HandleRButtonDoubleClick(ref Message m) => this.ProcessRButtonDoubleClick(this.OlvHitTest(m.LParam.ToInt32() & (int) ushort.MaxValue, m.LParam.ToInt32() >> 16 & (int) ushort.MaxValue));

    protected virtual bool ProcessRButtonDoubleClick(OlvListViewHitTestInfo hti) => hti.HitTestLocation == HitTestLocation.CheckBox;

    protected virtual bool HandleMouseMove(ref Message m) => false;

    protected virtual bool HandleReflectNotify(ref Message m)
    {
      bool flag1 = false;
      NativeMethods.NMHDR lparam1 = (NativeMethods.NMHDR) m.GetLParam(typeof (NativeMethods.NMHDR));
      switch (lparam1.code)
      {
        case -188:
          flag1 = this.HandleGroupInfo(ref m);
          break;
        case -184:
          flag1 = this.HandleLinkClick(ref m);
          break;
        case -181:
          flag1 = this.HandleEndScroll(ref m);
          break;
        case -180:
          flag1 = this.HandleBeginScroll(ref m);
          break;
        case -158:
          flag1 = ((NativeMethods.NMLVGETINFOTIP) m.GetLParam(typeof (NativeMethods.NMLVGETINFOTIP))).iItem >= this.GetItemCount();
          break;
        case -156:
          this.isMarqueSelecting = true;
          break;
        case -101:
          NativeMethods.NMLISTVIEW lparam2 = (NativeMethods.NMLISTVIEW) m.GetLParam(typeof (NativeMethods.NMLISTVIEW));
          if ((uint) (lparam2.uChanged & 8) > 0U)
          {
            if (this.CalculateCheckState(lparam2.uOldState) != this.CalculateCheckState(lparam2.uNewState))
            {
              lparam2.uOldState &= 4095;
              lparam2.uNewState &= 4095;
              Marshal.StructureToPtr((object) lparam2, m.LParam, false);
            }
            else if ((lparam2.uNewState & 2) == 2)
            {
              bool flag2 = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
              if (lparam2.iItem == -1 | flag2)
              {
                Stopwatch.StartNew();
                foreach (object disabledObject in this.DisabledObjects)
                {
                  int index = this.IndexOf(disabledObject);
                  if (index >= 0)
                    NativeMethods.DeselectOneItem((System.Windows.Forms.ListView) this, index);
                }
              }
              else
              {
                OLVListItem olvListItem = this.GetItem(lparam2.iItem);
                if (olvListItem != null && !olvListItem.Enabled)
                  NativeMethods.DeselectOneItem((System.Windows.Forms.ListView) this, lparam2.iItem);
              }
            }
            break;
          }
          break;
        case -100:
          NativeMethods.NMLISTVIEW lparam3 = (NativeMethods.NMLISTVIEW) m.GetLParam(typeof (NativeMethods.NMLISTVIEW));
          if ((uint) (lparam3.uChanged & 8) > 0U && this.CalculateCheckState(lparam3.uOldState) != this.CalculateCheckState(lparam3.uNewState))
          {
            lparam3.uChanged &= -9;
            Marshal.StructureToPtr((object) lparam3, m.LParam, false);
            break;
          }
          break;
        case -16:
          this.isMarqueSelecting = false;
          this.Invalidate();
          break;
        case -12:
          flag1 = this.HandleCustomDraw(ref m);
          break;
        case -3:
          if (this.CheckBoxes)
          {
            lparam1.code = -6;
            Marshal.StructureToPtr((object) lparam1, m.LParam, false);
            break;
          }
          break;
        case -2:
          this.fakeRightClick = true;
          lparam1.code = -5;
          Marshal.StructureToPtr((object) lparam1, m.LParam, false);
          break;
      }
      return flag1;
    }

    private CheckState CalculateCheckState(int state)
    {
      switch ((state & 61440) >> 12)
      {
        case 1:
          return CheckState.Unchecked;
        case 2:
          return CheckState.Checked;
        case 3:
          return CheckState.Indeterminate;
        default:
          return CheckState.Checked;
      }
    }

    protected bool HandleNotify(ref Message m)
    {
      bool flag = false;
      NativeMethods.NMHEADER lparam1 = (NativeMethods.NMHEADER) m.GetLParam(typeof (NativeMethods.NMHEADER));
      switch (lparam1.nhdr.code)
      {
        case -530:
          if (this.CellToolTip.Handle == lparam1.nhdr.hwndFrom)
          {
            flag = this.CellToolTip.HandleGetDispInfo(ref m);
            break;
          }
          break;
        case -522:
          if (this.CellToolTip.Handle == lparam1.nhdr.hwndFrom)
          {
            flag = this.CellToolTip.HandlePop(ref m);
            break;
          }
          break;
        case -521:
          if (this.CellToolTip.Handle == lparam1.nhdr.hwndFrom)
          {
            flag = this.CellToolTip.HandleShow(ref m);
            break;
          }
          break;
        case -328:
        case -308:
          if (lparam1.iItem >= 0 && lparam1.iItem < this.Columns.Count)
          {
            NativeMethods.HDITEM structure = (NativeMethods.HDITEM) Marshal.PtrToStructure(lparam1.pHDITEM, typeof (NativeMethods.HDITEM));
            OLVColumn column = this.GetColumn(lparam1.iItem);
            if (structure.cxy < column.MinimumWidth)
              structure.cxy = column.MinimumWidth;
            else if (column.MaximumWidth != -1 && structure.cxy > column.MaximumWidth)
              structure.cxy = column.MaximumWidth;
            Marshal.StructureToPtr((object) structure, lparam1.pHDITEM, false);
            break;
          }
          break;
        case -326:
        case -325:
        case -306:
        case -305:
          if (!this.PossibleFinishCellEditing())
          {
            m.Result = (IntPtr) 1;
            flag = true;
            break;
          }
          if (lparam1.iItem >= 0 && lparam1.iItem < this.Columns.Count && this.GetColumn(lparam1.iItem).FillsFreeSpace)
          {
            m.Result = (IntPtr) 1;
            flag = true;
            break;
          }
          break;
        case -322:
        case -302:
          if (!this.PossibleFinishCellEditing())
          {
            m.Result = (IntPtr) 1;
            flag = true;
            break;
          }
          break;
        case -320:
        case -300:
          NativeMethods.NMHEADER lparam2 = (NativeMethods.NMHEADER) m.GetLParam(typeof (NativeMethods.NMHEADER));
          if (lparam2.iItem >= 0 && lparam2.iItem < this.Columns.Count)
          {
            NativeMethods.HDITEM structure = (NativeMethods.HDITEM) Marshal.PtrToStructure(lparam2.pHDITEM, typeof (NativeMethods.HDITEM));
            OLVColumn column = this.GetColumn(lparam2.iItem);
            if ((structure.mask & 1) == 1 && (structure.cxy < column.MinimumWidth || column.MaximumWidth != -1 && structure.cxy > column.MaximumWidth))
            {
              m.Result = (IntPtr) 1;
              flag = true;
            }
            break;
          }
          break;
        case -12:
          if (!this.OwnerDrawnHeader)
          {
            flag = this.HeaderControl.HandleHeaderCustomDraw(ref m);
            break;
          }
          break;
      }
      return flag;
    }

    protected virtual void CreateCellToolTip()
    {
      this.cellToolTip = new ToolTipControl();
      this.cellToolTip.AssignHandle(NativeMethods.GetTooltipControl((System.Windows.Forms.ListView) this));
      this.cellToolTip.Showing += new EventHandler<ToolTipShowingEventArgs>(this.HandleCellToolTipShowing);
      this.cellToolTip.SetMaxWidth();
      NativeMethods.MakeTopMost((IWin32Window) this.cellToolTip);
    }

    protected virtual void UpdateCellToolTipHandle()
    {
      if (this.cellToolTip == null || !(this.cellToolTip.Handle == IntPtr.Zero))
        return;
      this.cellToolTip.AssignHandle(NativeMethods.GetTooltipControl((System.Windows.Forms.ListView) this));
    }

    protected virtual bool HandlePaint(ref Message m)
    {
      this.isInWmPaintEvent = true;
      this.shouldDoCustomDrawing = true;
      this.prePaintLevel = 0;
      this.ShowOverlays();
      this.HandlePrePaint();
      base.WndProc(ref m);
      this.HandlePostPaint();
      this.isInWmPaintEvent = false;
      return true;
    }

    protected virtual void HandlePrePaint()
    {
    }

    protected virtual void HandlePostPaint()
    {
      using (Graphics graphics = Graphics.FromHwnd(this.Handle))
        graphics.DrawRectangle(Pens.Gray, this.HeaderControl.ClientRectangle);
    }

    protected virtual bool HandleWindowPosChanging(ref Message m)
    {
      NativeMethods.WINDOWPOS lparam = (NativeMethods.WINDOWPOS) m.GetLParam(typeof (NativeMethods.WINDOWPOS));
      if ((lparam.flags & 1) == 0)
      {
        int cx1 = lparam.cx;
        Rectangle bounds = this.Bounds;
        int width = bounds.Width;
        if (cx1 < width)
        {
          int cx2 = lparam.cx;
          bounds = this.Bounds;
          int num = bounds.Width - this.ClientSize.Width;
          this.ResizeFreeSpaceFillingColumns(cx2 - num);
        }
      }
      return false;
    }

    protected virtual bool HandleHeaderRightClick(int columnIndex)
    {
      this.OnColumnRightClick(new ColumnClickEventArgs(columnIndex));
      return this.ShowHeaderRightClickMenu(columnIndex, Cursor.Position);
    }

    protected virtual bool ShowHeaderRightClickMenu(int columnIndex, Point pt)
    {
      ToolStripDropDown toolStripDropDown = this.MakeHeaderRightClickMenu(columnIndex);
      if (toolStripDropDown.Items.Count <= 0)
        return false;
      toolStripDropDown.Show(pt);
      return true;
    }

    protected virtual ToolStripDropDown MakeHeaderRightClickMenu(int columnIndex)
    {
      ToolStripDropDown strip = (ToolStripDropDown) new ContextMenuStrip();
      if (columnIndex >= 0 && this.UseFiltering && this.ShowFilterMenuOnRightClick)
        strip = this.MakeFilteringMenu(strip, columnIndex);
      if (columnIndex >= 0 && this.ShowCommandMenuOnRightClick)
        strip = this.MakeColumnCommandMenu(strip, columnIndex);
      if ((uint) this.SelectColumnsOnRightClickBehaviour > 0U)
        strip = this.MakeColumnSelectMenu(strip);
      return strip;
    }

    [Obsolete("Use HandleHeaderRightClick(int) instead")]
    protected virtual bool HandleHeaderRightClick() => false;

    [Obsolete("Use ShowHeaderRightClickMenu instead")]
    protected virtual void ShowColumnSelectMenu(Point pt) => this.MakeColumnSelectMenu((ToolStripDropDown) new ContextMenuStrip()).Show(pt);

    [Obsolete("Use ShowHeaderRightClickMenu instead")]
    protected virtual void ShowColumnCommandMenu(int columnIndex, Point pt)
    {
      ToolStripDropDown strip = this.MakeColumnCommandMenu((ToolStripDropDown) new ContextMenuStrip(), columnIndex);
      if (this.SelectColumnsOnRightClick)
      {
        if (strip.Items.Count > 0)
          strip.Items.Add((ToolStripItem) new ToolStripSeparator());
        this.MakeColumnSelectMenu(strip);
      }
      strip.Show(pt);
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Sort ascending by '{0}'")]
    [Localizable(true)]
    public string MenuLabelSortAscending
    {
      get => this.menuLabelSortAscending;
      set => this.menuLabelSortAscending = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Sort descending by '{0}'")]
    [Localizable(true)]
    public string MenuLabelSortDescending
    {
      get => this.menuLabelSortDescending;
      set => this.menuLabelSortDescending = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Group by '{0}'")]
    [Localizable(true)]
    public string MenuLabelGroupBy
    {
      get => this.menuLabelGroupBy;
      set => this.menuLabelGroupBy = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Lock grouping on '{0}'")]
    [Localizable(true)]
    public string MenuLabelLockGroupingOn
    {
      get => this.menuLabelLockGroupingOn;
      set => this.menuLabelLockGroupingOn = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Unlock grouping from '{0}'")]
    [Localizable(true)]
    public string MenuLabelUnlockGroupingOn
    {
      get => this.menuLabelUnlockGroupingOn;
      set => this.menuLabelUnlockGroupingOn = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Turn off groups")]
    [Localizable(true)]
    public string MenuLabelTurnOffGroups
    {
      get => this.menuLabelTurnOffGroups;
      set => this.menuLabelTurnOffGroups = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Unsort")]
    [Localizable(true)]
    public string MenuLabelUnsort
    {
      get => this.menuLabelUnsort;
      set => this.menuLabelUnsort = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Columns")]
    [Localizable(true)]
    public string MenuLabelColumns
    {
      get => this.menuLabelColumns;
      set => this.menuLabelColumns = value;
    }

    [Category("Labels - ObjectListView")]
    [DefaultValue("Select Columns...")]
    [Localizable(true)]
    public string MenuLabelSelectColumns
    {
      get => this.menuLabelSelectColumns;
      set => this.menuLabelSelectColumns = value;
    }

    public virtual ToolStripDropDown MakeColumnCommandMenu(
      ToolStripDropDown strip,
      int columnIndex)
    {
      OLVColumn column = this.GetColumn(columnIndex);
      if (column == null)
        return strip;
      if (strip.Items.Count > 0)
        strip.Items.Add((ToolStripItem) new ToolStripSeparator());
      string.Format(this.MenuLabelSortAscending, (object) column.Text);
      if (this.CanShowGroups)
      {
        string text = string.Format(this.MenuLabelGroupBy, (object) column.Text);
        if (column.Groupable && !string.IsNullOrEmpty(text))
          strip.Items.Add(text, (Image) null, (EventHandler) ((sender, args) =>
          {
            this.ShowGroups = true;
            this.PrimarySortColumn = column;
            this.PrimarySortOrder = SortOrder.Ascending;
            this.BuildList();
          }));
      }
      if (this.ShowGroups)
      {
        if (this.AlwaysGroupByColumn == column)
        {
          string text = string.Format(this.MenuLabelUnlockGroupingOn, (object) column.Text);
          if (!string.IsNullOrEmpty(text))
            strip.Items.Add(text, (Image) null, (EventHandler) ((sender, args) =>
            {
              this.AlwaysGroupByColumn = (OLVColumn) null;
              this.AlwaysGroupBySortOrder = SortOrder.None;
              this.BuildList();
            }));
        }
        else
        {
          string text = string.Format(this.MenuLabelLockGroupingOn, (object) column.Text);
          if (column.Groupable && !string.IsNullOrEmpty(text))
            strip.Items.Add(text, (Image) null, (EventHandler) ((sender, args) =>
            {
              this.ShowGroups = true;
              this.AlwaysGroupByColumn = column;
              this.AlwaysGroupBySortOrder = SortOrder.Ascending;
              this.BuildList();
            }));
        }
        string text1 = string.Format(this.MenuLabelTurnOffGroups, (object) column.Text);
        if (!string.IsNullOrEmpty(text1))
          strip.Items.Add(text1, (Image) null, (EventHandler) ((sender, args) =>
          {
            this.ShowGroups = false;
            this.BuildList();
          }));
      }
      else
      {
        string text = string.Format(this.MenuLabelUnsort, (object) column.Text);
        if (column.Sortable && !string.IsNullOrEmpty(text) && (uint) this.PrimarySortOrder > 0U)
          strip.Items.Add(text, (Image) null, (EventHandler) ((sender, args) => this.Unsort()));
      }
      return strip;
    }

    public virtual ToolStripDropDown MakeColumnSelectMenu(ToolStripDropDown strip)
    {
      if (strip.Items.Count > 0 && !(strip.Items[strip.Items.Count - 1] is ToolStripSeparator))
        strip.Items.Add((ToolStripItem) new ToolStripSeparator());
      if (this.AllColumns.Count > 0 && this.AllColumns[0].LastDisplayIndex == -1)
        this.RememberDisplayIndicies();
      if (this.SelectColumnsOnRightClickBehaviour == ObjectListView.ColumnSelectBehaviour.ModelDialog)
        strip.Items.Add(this.MenuLabelSelectColumns, (Image) null, (EventHandler) ((sender, args) => new ColumnSelectionForm().OpenOn(this)));
      if (this.SelectColumnsOnRightClickBehaviour == ObjectListView.ColumnSelectBehaviour.Submenu)
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(this.MenuLabelColumns);
        toolStripMenuItem.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.ColumnSelectMenuItemClicked);
        strip.Items.Add((ToolStripItem) toolStripMenuItem);
        this.AddItemsToColumnSelectMenu(toolStripMenuItem.DropDownItems);
      }
      if (this.SelectColumnsOnRightClickBehaviour == ObjectListView.ColumnSelectBehaviour.InlineMenu)
      {
        strip.ItemClicked += new ToolStripItemClickedEventHandler(this.ColumnSelectMenuItemClicked);
        strip.Closing += new ToolStripDropDownClosingEventHandler(this.ColumnSelectMenuClosing);
        this.AddItemsToColumnSelectMenu(strip.Items);
      }
      return strip;
    }

    protected void AddItemsToColumnSelectMenu(ToolStripItemCollection items)
    {
      List<OLVColumn> olvColumnList = new List<OLVColumn>((IEnumerable<OLVColumn>) this.AllColumns);
      olvColumnList.Sort((Comparison<OLVColumn>) ((x, y) => x.LastDisplayIndex - y.LastDisplayIndex));
      foreach (OLVColumn olvColumn in olvColumnList)
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(olvColumn.Text);
        toolStripMenuItem.Checked = olvColumn.IsVisible;
        toolStripMenuItem.Tag = (object) olvColumn;
        toolStripMenuItem.Enabled = !olvColumn.IsVisible || olvColumn.CanBeHidden;
        items.Add((ToolStripItem) toolStripMenuItem);
      }
    }

    private void ColumnSelectMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      this.contextMenuStaysOpen = false;
      if (!(e.ClickedItem is ToolStripMenuItem clickedItem) || !(clickedItem.Tag is OLVColumn tag))
        return;
      clickedItem.Checked = !clickedItem.Checked;
      tag.IsVisible = clickedItem.Checked;
      this.contextMenuStaysOpen = this.SelectColumnsMenuStaysOpen;
      this.BeginInvoke((Delegate) new MethodInvoker(this.RebuildColumns));
    }

    private void ColumnSelectMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
    {
      e.Cancel = this.contextMenuStaysOpen && e.CloseReason == ToolStripDropDownCloseReason.ItemClicked;
      this.contextMenuStaysOpen = false;
    }

    public virtual ToolStripDropDown MakeFilteringMenu(
      ToolStripDropDown strip,
      int columnIndex)
    {
      OLVColumn column = this.GetColumn(columnIndex);
      if (column == null)
        return strip;
      FilterMenuBuilder menuBuildStrategy = this.FilterMenuBuildStrategy;
      return menuBuildStrategy == null ? strip : menuBuildStrategy.MakeFilterMenu(strip, this, column);
    }

    protected override void OnColumnReordered(ColumnReorderedEventArgs e)
    {
      base.OnColumnReordered(e);
      this.BeginInvoke((Delegate) new MethodInvoker(this.RememberDisplayIndicies));
    }

    private void RememberDisplayIndicies()
    {
      foreach (OLVColumn allColumn in this.AllColumns)
        allColumn.LastDisplayIndex = allColumn.DisplayIndex;
    }

    protected virtual void HandleColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
    {
      if (!this.UpdateSpaceFillingColumnsWhenDraggingColumnDivider || this.GetColumn(e.ColumnIndex).FillsFreeSpace)
        return;
      int width = this.GetColumn(e.ColumnIndex).Width;
      if (e.NewWidth > width)
        this.ResizeFreeSpaceFillingColumns(this.ClientSize.Width - (e.NewWidth - width));
      else
        this.ResizeFreeSpaceFillingColumns();
    }

    protected virtual void HandleColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
      if (this.GetColumn(e.ColumnIndex).FillsFreeSpace)
        return;
      this.ResizeFreeSpaceFillingColumns();
    }

    protected virtual void HandleLayout(object sender, LayoutEventArgs e)
    {
      if (this.hasResizeColumnsHandler)
        return;
      this.hasResizeColumnsHandler = true;
      this.RunWhenIdle(new EventHandler(this.HandleApplicationIdleResizeColumns));
    }

    private void RunWhenIdle(EventHandler eventHandler)
    {
      Application.Idle += eventHandler;
      if (this.CanUseApplicationIdle)
        return;
      SynchronizationContext.Current.Post((SendOrPostCallback) (x => Application.RaiseIdle(EventArgs.Empty)), (object) null);
    }

    protected virtual void ResizeFreeSpaceFillingColumns() => this.ResizeFreeSpaceFillingColumns(this.ClientSize.Width);

    protected virtual void ResizeFreeSpaceFillingColumns(int freeSpace)
    {
      if (this.DesignMode || this.Frozen)
        return;
      int num1 = 0;
      List<OLVColumn> olvColumnList = new List<OLVColumn>();
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        OLVColumn column = this.GetColumn(index);
        if (column.FillsFreeSpace)
        {
          olvColumnList.Add(column);
          num1 += column.FreeSpaceProportion;
        }
        else
          freeSpace -= column.Width;
      }
      freeSpace = Math.Max(0, freeSpace);
      foreach (OLVColumn olvColumn in olvColumnList.ToArray())
      {
        int num2 = freeSpace * olvColumn.FreeSpaceProportion / num1;
        int num3 = olvColumn.MinimumWidth == -1 || num2 >= olvColumn.MinimumWidth ? (olvColumn.MaximumWidth == -1 || num2 <= olvColumn.MaximumWidth ? 0 : olvColumn.MaximumWidth) : olvColumn.MinimumWidth;
        if (num3 > 0)
        {
          olvColumn.Width = num3;
          freeSpace -= num3;
          num1 -= olvColumn.FreeSpaceProportion;
          olvColumnList.Remove(olvColumn);
        }
      }
      foreach (OLVColumn olvColumn in olvColumnList)
        olvColumn.Width = freeSpace * olvColumn.FreeSpaceProportion / num1;
    }

    public virtual void CheckAll() => this.CheckedObjects = (IList) ObjectListView.EnumerableToArray(this.Objects, false);

    public virtual void CheckHeaderCheckBox(OLVColumn column)
    {
      if (column == null)
        return;
      this.ChangeHeaderCheckBoxState(column, CheckState.Unchecked);
    }

    public virtual void CheckIndeterminateHeaderCheckBox(OLVColumn column)
    {
      if (column == null)
        return;
      this.ChangeHeaderCheckBoxState(column, CheckState.Indeterminate);
    }

    public virtual void CheckIndeterminateObject(object modelObject) => this.SetObjectCheckedness(modelObject, CheckState.Indeterminate);

    public virtual void CheckObject(object modelObject) => this.SetObjectCheckedness(modelObject, CheckState.Checked);

    public virtual void CheckObjects(IEnumerable modelObjects)
    {
      foreach (object modelObject in modelObjects)
        this.CheckObject(modelObject);
    }

    public virtual void CheckSubItem(object rowObject, OLVColumn column)
    {
      if (column == null || rowObject == null || !column.CheckBoxes)
        return;
      column.PutCheckState(rowObject, CheckState.Checked);
      this.RefreshObject(rowObject);
    }

    public virtual void CheckIndeterminateSubItem(object rowObject, OLVColumn column)
    {
      if (column == null || rowObject == null || !column.CheckBoxes)
        return;
      column.PutCheckState(rowObject, CheckState.Indeterminate);
      this.RefreshObject(rowObject);
    }

    public virtual bool IsChecked(object modelObject)
    {
      CheckState? checkState1 = this.GetCheckState(modelObject);
      CheckState checkState2 = CheckState.Checked;
      return checkState1.GetValueOrDefault() == checkState2 && checkState1.HasValue;
    }

    public virtual bool IsCheckedIndeterminate(object modelObject)
    {
      CheckState? checkState1 = this.GetCheckState(modelObject);
      CheckState checkState2 = CheckState.Indeterminate;
      return checkState1.GetValueOrDefault() == checkState2 && checkState1.HasValue;
    }

    public virtual bool IsSubItemChecked(object rowObject, OLVColumn column) => column != null && rowObject != null && column.CheckBoxes && column.GetCheckState(rowObject) == CheckState.Checked;

    protected virtual CheckState? GetCheckState(object modelObject) => this.CheckStateGetter != null ? new CheckState?(this.CheckStateGetter(modelObject)) : (this.PersistentCheckBoxes ? new CheckState?(this.GetPersistentCheckState(modelObject)) : new CheckState?());

    protected virtual CheckState PutCheckState(object modelObject, CheckState state) => this.CheckStatePutter != null ? this.CheckStatePutter(modelObject, state) : (this.PersistentCheckBoxes ? this.SetPersistentCheckState(modelObject, state) : state);

    protected virtual bool SetObjectCheckedness(object modelObject, CheckState state)
    {
      CheckState? checkState1 = this.GetCheckState(modelObject);
      CheckState checkState2 = state;
      if (checkState1.GetValueOrDefault() == checkState2 && checkState1.HasValue)
        return false;
      OLVListItem olvi = this.ModelToItem(modelObject);
      if (olvi == null)
      {
        int num = (int) this.PutCheckState(modelObject, state);
        return true;
      }
      ItemCheckEventArgs ice = new ItemCheckEventArgs(olvi.Index, state, olvi.CheckState);
      this.OnItemCheck(ice);
      if (ice.NewValue == olvi.CheckState)
        return false;
      olvi.CheckState = this.PutCheckState(modelObject, state);
      this.RefreshItem(olvi);
      this.OnItemChecked(new ItemCheckedEventArgs((ListViewItem) olvi));
      return true;
    }

    public virtual void ToggleCheckObject(object modelObject)
    {
      OLVListItem olvListItem = this.ModelToItem(modelObject);
      if (olvListItem == null)
        return;
      CheckState state = CheckState.Checked;
      if (olvListItem.CheckState == CheckState.Checked)
        state = this.TriStateCheckBoxes ? CheckState.Indeterminate : CheckState.Unchecked;
      else if (olvListItem.CheckState == CheckState.Indeterminate && this.TriStateCheckBoxes)
        state = CheckState.Unchecked;
      this.SetObjectCheckedness(modelObject, state);
    }

    public virtual void ToggleHeaderCheckBox(OLVColumn column)
    {
      if (column == null)
        return;
      CheckState toggledCheckState = ObjectListView.CalculateToggledCheckState(column.HeaderCheckState, column.HeaderTriStateCheckBox, column.HeaderCheckBoxDisabled);
      this.ChangeHeaderCheckBoxState(column, toggledCheckState);
    }

    private void ChangeHeaderCheckBoxState(OLVColumn column, CheckState newState)
    {
      HeaderCheckBoxChangingEventArgs args = new HeaderCheckBoxChangingEventArgs();
      args.Column = column;
      args.NewCheckState = newState;
      this.OnHeaderCheckBoxChanging(args);
      if (args.Cancel || column.HeaderCheckState == args.NewCheckState)
        return;
      Stopwatch.StartNew();
      column.HeaderCheckState = args.NewCheckState;
      this.HeaderControl.Invalidate(column);
      if (!column.HeaderCheckBoxUpdatesRowCheckBoxes)
        return;
      if (column.Index == 0)
        this.UpdateAllPrimaryCheckBoxes(column);
      else
        this.UpdateAllSubItemCheckBoxes(column);
    }

    private void UpdateAllPrimaryCheckBoxes(OLVColumn column)
    {
      if (!this.CheckBoxes || column.HeaderCheckState == CheckState.Indeterminate)
        return;
      if (column.HeaderCheckState == CheckState.Checked)
        this.CheckAll();
      else
        this.UncheckAll();
    }

    private void UpdateAllSubItemCheckBoxes(OLVColumn column)
    {
      if (!column.CheckBoxes || column.HeaderCheckState == CheckState.Indeterminate)
        return;
      foreach (object rowObject in this.Objects)
        column.PutCheckState(rowObject, column.HeaderCheckState);
      this.BuildList(true);
    }

    public virtual void ToggleSubItemCheckBox(object rowObject, OLVColumn column)
    {
      CheckState checkState = column.GetCheckState(rowObject);
      CheckState toggledCheckState = ObjectListView.CalculateToggledCheckState(checkState, column.TriStateCheckBoxes, false);
      SubItemCheckingEventArgs args = new SubItemCheckingEventArgs(column, this.ModelToItem(rowObject), column.Index, checkState, toggledCheckState);
      this.OnSubItemChecking(args);
      if (args.Canceled)
        return;
      switch (args.NewValue)
      {
        case CheckState.Unchecked:
          this.UncheckSubItem(rowObject, column);
          break;
        case CheckState.Checked:
          this.CheckSubItem(rowObject, column);
          break;
        case CheckState.Indeterminate:
          this.CheckIndeterminateSubItem(rowObject, column);
          break;
      }
    }

    public virtual void UncheckAll() => this.CheckedObjects = (IList) null;

    public virtual void UncheckObject(object modelObject) => this.SetObjectCheckedness(modelObject, CheckState.Unchecked);

    public virtual void UncheckObjects(IEnumerable modelObjects)
    {
      foreach (object modelObject in modelObjects)
        this.UncheckObject(modelObject);
    }

    public virtual void UncheckHeaderCheckBox(OLVColumn column)
    {
      if (column == null)
        return;
      this.ChangeHeaderCheckBoxState(column, CheckState.Unchecked);
    }

    public virtual void UncheckSubItem(object rowObject, OLVColumn column)
    {
      if (column == null || rowObject == null || !column.CheckBoxes)
        return;
      column.PutCheckState(rowObject, CheckState.Unchecked);
      this.RefreshObject(rowObject);
    }

    public virtual OLVColumn GetColumn(int index) => (OLVColumn) this.Columns[index];

    public virtual OLVColumn GetColumn(string name)
    {
      foreach (ColumnHeader column in this.Columns)
      {
        if (column.Text == name)
          return (OLVColumn) column;
      }
      return (OLVColumn) null;
    }

    public virtual List<OLVColumn> GetFilteredColumns(View view)
    {
      int index = 0;
      return this.AllColumns.FindAll((Predicate<OLVColumn>) (x => index++ == 0 || x.IsVisible));
    }

    public virtual int GetItemCount() => this.Items.Count;

    public virtual OLVListItem GetItem(int index) => index < 0 || index >= this.GetItemCount() ? (OLVListItem) null : (OLVListItem) this.Items[index];

    public virtual object GetModelObject(int index)
    {
      OLVListItem olvListItem = this.GetItem(index);
      return olvListItem == null ? (object) null : olvListItem.RowObject;
    }

    public virtual OLVListItem GetItemAt(int x, int y, out OLVColumn hitColumn)
    {
      hitColumn = (OLVColumn) null;
      ListViewHitTestInfo listViewHitTestInfo = this.HitTest(x, y);
      if (listViewHitTestInfo.Item == null)
        return (OLVListItem) null;
      if (listViewHitTestInfo.SubItem != null)
      {
        int index = listViewHitTestInfo.Item.SubItems.IndexOf(listViewHitTestInfo.SubItem);
        hitColumn = this.GetColumn(index);
      }
      return (OLVListItem) listViewHitTestInfo.Item;
    }

    public virtual OLVListSubItem GetSubItem(int index, int columnIndex)
    {
      OLVListItem olvListItem = this.GetItem(index);
      return olvListItem == null ? (OLVListSubItem) null : olvListItem.GetSubItem(columnIndex);
    }

    public virtual void EnsureGroupVisible(ListViewGroup lvg)
    {
      if (!this.ShowGroups || lvg == null)
        return;
      int num = this.Groups.IndexOf(lvg);
      if (num <= 0)
      {
        NativeMethods.Scroll((System.Windows.Forms.ListView) this, 0, -NativeMethods.GetScrollPosition((System.Windows.Forms.ListView) this, false));
      }
      else
      {
        ListViewGroup group = this.Groups[num - 1];
        Rectangle itemRect = this.GetItemRect(group.Items[group.Items.Count - 1].Index);
        NativeMethods.Scroll((System.Windows.Forms.ListView) this, 0, itemRect.Y + itemRect.Height / 2);
      }
    }

    public virtual void EnsureModelVisible(object modelObject)
    {
      int index = this.IndexOf(modelObject);
      if (index < 0)
        return;
      this.EnsureVisible(index);
    }

    [Obsolete("Use SelectedObject property instead of this method")]
    public virtual object GetSelectedObject() => this.SelectedObject;

    [Obsolete("Use SelectedObjects property instead of this method")]
    public virtual ArrayList GetSelectedObjects() => ObjectListView.EnumerableToArray((IEnumerable) this.SelectedObjects, false);

    [Obsolete("Use CheckedObject property instead of this method")]
    public virtual object GetCheckedObject() => this.CheckedObject;

    [Obsolete("Use CheckedObjects property instead of this method")]
    public virtual ArrayList GetCheckedObjects() => ObjectListView.EnumerableToArray((IEnumerable) this.CheckedObjects, false);

    public virtual int IndexOf(object modelObject)
    {
      for (int index = 0; index < this.GetItemCount(); ++index)
      {
        if (this.GetModelObject(index).Equals(modelObject))
          return index;
      }
      return -1;
    }

    public virtual void RefreshItem(OLVListItem olvi)
    {
      olvi.UseItemStyleForSubItems = true;
      olvi.SubItems.Clear();
      this.FillInValues(olvi, olvi.RowObject);
      this.PostProcessOneRow(olvi.Index, this.GetDisplayOrderOfItemIndex(olvi.Index), olvi);
    }

    public virtual void RefreshObject(object modelObject) => this.RefreshObjects((IList) new object[1]
    {
      modelObject
    });

    public virtual void RefreshObjects(IList modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.RefreshObjects(modelObjects)));
      }
      else
      {
        foreach (object modelObject in (IEnumerable) modelObjects)
        {
          OLVListItem olvi = this.ModelToItem(modelObject);
          if (olvi != null)
          {
            this.ReplaceModel(olvi, modelObject);
            this.RefreshItem(olvi);
          }
        }
      }
    }

    private void ReplaceModel(OLVListItem olvi, object newModel)
    {
      if (olvi.RowObject == newModel)
        return;
      this.TakeOwnershipOfObjects();
      ArrayList array = ObjectListView.EnumerableToArray(this.Objects, false);
      int index = array.IndexOf(olvi.RowObject);
      if (index >= 0)
        array[index] = newModel;
      olvi.RowObject = newModel;
    }

    public virtual void RefreshSelectedObjects()
    {
      foreach (OLVListItem selectedItem in this.SelectedItems)
        this.RefreshItem(selectedItem);
    }

    public virtual void SelectObject(object modelObject) => this.SelectObject(modelObject, false);

    public virtual void SelectObject(object modelObject, bool setFocus)
    {
      OLVListItem olvListItem = this.ModelToItem(modelObject);
      if (olvListItem == null || !olvListItem.Enabled)
        return;
      olvListItem.Selected = true;
      if (setFocus)
        olvListItem.Focused = true;
    }

    public virtual void SelectObjects(IList modelObjects)
    {
      this.SelectedIndices.Clear();
      if (modelObjects == null)
        return;
      foreach (object modelObject in (IEnumerable) modelObjects)
      {
        OLVListItem olvListItem = this.ModelToItem(modelObject);
        if (olvListItem != null && olvListItem.Enabled)
          olvListItem.Selected = true;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool Frozen
    {
      get => this.freezeCount > 0;
      set
      {
        if (value)
        {
          this.Freeze();
        }
        else
        {
          if (this.freezeCount <= 0)
            return;
          this.freezeCount = 1;
          this.Unfreeze();
        }
      }
    }

    public virtual void Freeze()
    {
      if (this.freezeCount == 0)
        this.DoFreeze();
      ++this.freezeCount;
      this.OnFreezing(new FreezeEventArgs(this.freezeCount));
    }

    public virtual void Unfreeze()
    {
      if (this.freezeCount <= 0)
        return;
      --this.freezeCount;
      if (this.freezeCount == 0)
        this.DoUnfreeze();
      this.OnFreezing(new FreezeEventArgs(this.freezeCount));
    }

    protected virtual void DoFreeze() => this.BeginUpdate();

    protected virtual void DoUnfreeze()
    {
      this.EndUpdate();
      this.ResizeFreeSpaceFillingColumns();
      this.BuildList();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected bool SelectionEventsSuspended => this.suspendSelectionEventCount > 0;

    protected void SuspendSelectionEvents() => ++this.suspendSelectionEventCount;

    protected void ResumeSelectionEvents() => --this.suspendSelectionEventCount;

    protected IDisposable SuspendSelectionEventsDuring() => (IDisposable) new ObjectListView.SuspendSelectionDisposable(this);

    public new void Sort() => this.Sort(this.PrimarySortColumn, this.PrimarySortOrder);

    public virtual void Sort(string columnToSortName) => this.Sort(this.GetColumn(columnToSortName), this.PrimarySortOrder);

    public virtual void Sort(int columnToSortIndex)
    {
      if (columnToSortIndex < 0 || columnToSortIndex >= this.Columns.Count)
        return;
      this.Sort(this.GetColumn(columnToSortIndex), this.PrimarySortOrder);
    }

    public virtual void Sort(OLVColumn columnToSort)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.Sort(columnToSort)));
      else
        this.Sort(columnToSort, this.PrimarySortOrder);
    }

    public virtual void Sort(OLVColumn columnToSort, SortOrder order)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.Sort(columnToSort, order)));
      }
      else
      {
        this.DoSort(columnToSort, order);
        this.PostProcessRows();
      }
    }

    private void DoSort(OLVColumn columnToSort, SortOrder order)
    {
      if (this.GetItemCount() == 0 || this.Columns.Count == 0)
        return;
      if (this.ShowGroups)
      {
        columnToSort = columnToSort ?? this.GetColumn(0);
        if (order == SortOrder.None)
        {
          order = this.Sorting;
          if (order == SortOrder.None)
            order = SortOrder.Ascending;
        }
      }
      BeforeSortingEventArgs sortingEventArgs = this.BuildBeforeSortingEventArgs(columnToSort, order);
      this.OnBeforeSorting(sortingEventArgs);
      if (sortingEventArgs.Canceled)
        return;
      IList list = this.VirtualMode ? this.SelectedObjects : (IList) null;
      this.SuspendSelectionEvents();
      this.ClearHotItem();
      if (!sortingEventArgs.Handled && sortingEventArgs.ColumnToSort != null && (uint) sortingEventArgs.SortOrder > 0U)
      {
        if (this.ShowGroups)
          this.BuildGroups(sortingEventArgs.ColumnToGroupBy, sortingEventArgs.GroupByOrder, sortingEventArgs.ColumnToSort, sortingEventArgs.SortOrder, sortingEventArgs.SecondaryColumnToSort, sortingEventArgs.SecondarySortOrder);
        else if (this.CustomSorter != null)
          this.CustomSorter(sortingEventArgs.ColumnToSort, sortingEventArgs.SortOrder);
        else
          this.ListViewItemSorter = (IComparer) new ColumnComparer(sortingEventArgs.ColumnToSort, sortingEventArgs.SortOrder, sortingEventArgs.SecondaryColumnToSort, sortingEventArgs.SecondarySortOrder);
      }
      if (this.ShowSortIndicators)
        this.ShowSortIndicator(sortingEventArgs.ColumnToSort, sortingEventArgs.SortOrder);
      this.PrimarySortColumn = sortingEventArgs.ColumnToSort;
      this.PrimarySortOrder = sortingEventArgs.SortOrder;
      if (list != null && list.Count > 0)
        this.SelectedObjects = list;
      this.ResumeSelectionEvents();
      this.RefreshHotItem();
      this.OnAfterSorting(new AfterSortingEventArgs(sortingEventArgs));
    }

    public virtual void ShowSortIndicator()
    {
      if (!this.ShowSortIndicators || (uint) this.PrimarySortOrder <= 0U)
        return;
      this.ShowSortIndicator(this.PrimarySortColumn, this.PrimarySortOrder);
    }

    protected virtual void ShowSortIndicator(OLVColumn columnToSort, SortOrder sortOrder)
    {
      int imageIndex = -1;
      if (!NativeMethods.HasBuiltinSortIndicators())
      {
        if (this.SmallImageList == null || !this.SmallImageList.Images.ContainsKey("sort-indicator-up"))
          this.MakeSortIndicatorImages();
        if (this.SmallImageList != null)
          imageIndex = this.SmallImageList.Images.IndexOfKey(sortOrder == SortOrder.Ascending ? "sort-indicator-up" : "sort-indicator-down");
      }
      for (int columnIndex = 0; columnIndex < this.Columns.Count; ++columnIndex)
      {
        if (columnToSort != null && columnIndex == columnToSort.Index)
          NativeMethods.SetColumnImage((System.Windows.Forms.ListView) this, columnIndex, sortOrder, imageIndex);
        else
          NativeMethods.SetColumnImage((System.Windows.Forms.ListView) this, columnIndex, SortOrder.None, -1);
      }
    }

    protected virtual void MakeSortIndicatorImages()
    {
      if (this.DesignMode)
        return;
      ImageList imageList = this.SmallImageList;
      if (imageList == null)
      {
        imageList = new ImageList();
        imageList.ImageSize = new Size(16, 16);
        imageList.ColorDepth = ColorDepth.Depth32Bit;
      }
      Size imageSize = imageList.ImageSize;
      int x = imageSize.Width / 2;
      imageSize = imageList.ImageSize;
      int num1 = imageSize.Height / 2 - 1;
      int num2 = x - 2;
      int num3 = num2 / 2;
      if (imageList.Images.IndexOfKey("sort-indicator-up") == -1)
      {
        Point point1 = new Point(x - num2, num1 + num3);
        Point point2 = new Point(x, num1 - num3 - 1);
        Point point3 = new Point(x + num2, num1 + num3);
        imageList.Images.Add("sort-indicator-up", (Image) this.MakeTriangleBitmap(imageList.ImageSize, new Point[3]
        {
          point1,
          point2,
          point3
        }));
      }
      if (imageList.Images.IndexOfKey("sort-indicator-down") == -1)
      {
        Point point4 = new Point(x - num2, num1 - num3);
        Point point5 = new Point(x, num1 + num3);
        Point point6 = new Point(x + num2, num1 - num3);
        imageList.Images.Add("sort-indicator-down", (Image) this.MakeTriangleBitmap(imageList.ImageSize, new Point[3]
        {
          point4,
          point5,
          point6
        }));
      }
      this.SmallImageList = imageList;
    }

    private Bitmap MakeTriangleBitmap(Size sz, Point[] pts)
    {
      Bitmap bitmap = new Bitmap(sz.Width, sz.Height);
      Graphics.FromImage((Image) bitmap).FillPolygon((Brush) new SolidBrush(Color.Gray), pts);
      return bitmap;
    }

    public virtual void Unsort()
    {
      this.ShowGroups = false;
      this.PrimarySortColumn = (OLVColumn) null;
      this.PrimarySortOrder = SortOrder.None;
      this.BuildList();
    }

    private static CheckState CalculateToggledCheckState(
      CheckState currentState,
      bool isTriState,
      bool isDisabled)
    {
      if (isDisabled)
        return currentState;
      switch (currentState)
      {
        case CheckState.Checked:
          return isTriState ? CheckState.Indeterminate : CheckState.Unchecked;
        case CheckState.Indeterminate:
          return CheckState.Unchecked;
        default:
          return CheckState.Checked;
      }
    }

    protected virtual void CreateGroups(IEnumerable<OLVGroup> groups)
    {
      this.Groups.Clear();
      foreach (OLVGroup group in groups)
      {
        group.InsertGroupOldStyle(this);
        group.SetItemsOldStyle();
      }
    }

    protected virtual void CorrectSubItemColors(ListViewItem olvi)
    {
    }

    protected virtual void FillInValues(OLVListItem lvi, object rowObject)
    {
      if (this.Columns.Count == 0)
        return;
      OLVListSubItem olvListSubItem = this.MakeSubItem(rowObject, this.GetColumn(0));
      lvi.SubItems[0] = (ListViewItem.ListViewSubItem) olvListSubItem;
      lvi.ImageSelector = olvListSubItem.ImageSelector;
      lvi.Font = this.Font;
      lvi.BackColor = this.BackColor;
      lvi.ForeColor = this.ForeColor;
      lvi.Enabled = !this.IsDisabled(rowObject);
      switch (this.View)
      {
        case View.Details:
          for (int index = 1; index < this.Columns.Count; ++index)
            lvi.SubItems.Add((ListViewItem.ListViewSubItem) this.MakeSubItem(rowObject, this.GetColumn(index)));
          break;
        case View.Tile:
          for (int index = 1; index < this.Columns.Count; ++index)
          {
            OLVColumn column = this.GetColumn(index);
            if (column.IsTileViewColumn)
              lvi.SubItems.Add((ListViewItem.ListViewSubItem) this.MakeSubItem(rowObject, column));
          }
          break;
      }
      if (!lvi.Enabled)
      {
        lvi.UseItemStyleForSubItems = false;
        this.ApplyRowStyle(lvi, (IItemStyle) (this.DisabledItemStyle ?? ObjectListView.DefaultDisabledItemStyle), false);
      }
      if (this.CheckBoxes)
      {
        CheckState? checkState = this.GetCheckState(lvi.RowObject);
        lvi.CheckState = (CheckState) ((int) checkState ?? 0);
      }
      if (this.RowFormatter == null)
        return;
      this.RowFormatter(lvi);
    }

    private OLVListSubItem MakeSubItem(object rowObject, OLVColumn column)
    {
      object modelValue = column.GetValue(rowObject);
      OLVListSubItem olvListSubItem = new OLVListSubItem(modelValue, column.ValueToString(modelValue), column.GetImage(rowObject));
      if (this.UseHyperlinks && column.Hyperlink)
      {
        IsHyperlinkEventArgs e = new IsHyperlinkEventArgs();
        e.ListView = this;
        e.Model = rowObject;
        e.Column = column;
        e.Text = olvListSubItem.Text;
        e.Url = olvListSubItem.Text;
        e.IsHyperlink = !this.IsDisabled(rowObject);
        this.OnIsHyperlink(e);
        olvListSubItem.Url = e.IsHyperlink ? e.Url : (string) null;
      }
      return olvListSubItem;
    }

    private void ApplyHyperlinkStyle(OLVListItem olvi)
    {
      olvi.UseItemStyleForSubItems = false;
      Color backColor = olvi.BackColor;
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        OLVListSubItem subItem = olvi.GetSubItem(index);
        if (subItem != null)
        {
          OLVColumn column = this.GetColumn(index);
          subItem.BackColor = backColor;
          if (column.Hyperlink && !string.IsNullOrEmpty(subItem.Url))
            this.ApplyCellStyle(olvi, index, this.IsUrlVisited(subItem.Url) ? (IItemStyle) this.HyperlinkStyle.Visited : (IItemStyle) this.HyperlinkStyle.Normal);
        }
      }
    }

    protected virtual void ForceSubItemImagesExStyle()
    {
      if (this.VirtualMode)
        return;
      NativeMethods.ForceSubItemImagesExStyle((System.Windows.Forms.ListView) this);
    }

    protected virtual int GetActualImageIndex(object imageSelector)
    {
      int num1;
      switch (imageSelector)
      {
        case null:
          return -1;
        case int num2:
          return num2;
        case string key:
          num1 = this.SmallImageList != null ? 1 : 0;
          break;
        default:
          num1 = 0;
          break;
      }
      return num1 != 0 ? this.SmallImageList.Images.IndexOfKey(key) : -1;
    }

    public virtual string GetHeaderToolTip(int columnIndex)
    {
      OLVColumn column = this.GetColumn(columnIndex);
      if (column == null)
        return (string) null;
      string str = column.ToolTipText;
      if (this.HeaderToolTipGetter != null)
        str = this.HeaderToolTipGetter(column);
      return str;
    }

    public virtual string GetCellToolTip(int columnIndex, int rowIndex)
    {
      if (this.CellToolTipGetter != null)
        return this.CellToolTipGetter(this.GetColumn(columnIndex), this.GetModelObject(rowIndex));
      if (columnIndex >= 0)
      {
        OLVListSubItem subItem = this.GetSubItem(rowIndex, columnIndex);
        if (subItem != null && !string.IsNullOrEmpty(subItem.Url) && subItem.Url != subItem.Text && this.HotCellHitLocation == HitTestLocation.Text)
          return subItem.Url;
      }
      return (string) null;
    }

    public virtual OLVListItem ModelToItem(object modelObject)
    {
      if (modelObject == null)
        return (OLVListItem) null;
      foreach (OLVListItem olvListItem in this.Items)
      {
        if (olvListItem.RowObject != null && olvListItem.RowObject.Equals(modelObject))
          return olvListItem;
      }
      return (OLVListItem) null;
    }

    protected virtual void PostProcessRows()
    {
      int count = this.Items.Count;
      int displayIndex = 0;
      if (this.ShowGroups)
      {
        foreach (ListViewGroup group in this.Groups)
        {
          foreach (OLVListItem olvi in group.Items)
          {
            this.PostProcessOneRow(olvi.Index, displayIndex, olvi);
            ++displayIndex;
          }
        }
      }
      else
      {
        foreach (OLVListItem olvi in this.Items)
        {
          this.PostProcessOneRow(olvi.Index, displayIndex, olvi);
          ++displayIndex;
        }
      }
    }

    protected virtual void PostProcessOneRow(int rowIndex, int displayIndex, OLVListItem olvi)
    {
      if (this.UseAlternatingBackColors && this.View == View.Details && olvi.Enabled)
        olvi.BackColor = displayIndex % 2 == 1 ? this.AlternateRowBackColorOrDefault : this.BackColor;
      if (this.ShowImagesOnSubItems && !this.VirtualMode)
        this.SetSubItemImages(rowIndex, olvi);
      if (this.UseHyperlinks)
        this.ApplyHyperlinkStyle(olvi);
      this.TriggerFormatRowEvent(rowIndex, displayIndex, olvi);
    }

    [Obsolete("This method is no longer used. Override PostProcessOneRow() to achieve a similar result")]
    protected virtual void PrepareAlternateBackColors()
    {
    }

    [Obsolete("This method is not longer maintained and will be removed", false)]
    protected virtual void SetAllSubItemImages()
    {
    }

    protected virtual void SetSubItemImages(int rowIndex, OLVListItem item) => this.SetSubItemImages(rowIndex, item, false);

    protected virtual void SetSubItemImages(int rowIndex, OLVListItem item, bool shouldClearImages)
    {
      if (!this.ShowImagesOnSubItems || this.OwnerDraw)
        return;
      for (int index = 1; index < item.SubItems.Count; ++index)
        this.SetSubItemImage(rowIndex, index, item.GetSubItem(index), shouldClearImages);
    }

    public virtual void SetSubItemImage(
      int rowIndex,
      int subItemIndex,
      OLVListSubItem subItem,
      bool shouldClearImages)
    {
      int actualImageIndex = this.GetActualImageIndex(subItem.ImageSelector);
      if (!shouldClearImages && actualImageIndex == -1)
        return;
      NativeMethods.SetSubItemImage((System.Windows.Forms.ListView) this, rowIndex, subItemIndex, actualImageIndex);
    }

    protected virtual void TakeOwnershipOfObjects()
    {
      if (this.isOwnerOfObjects)
        return;
      this.isOwnerOfObjects = true;
      this.objects = (IEnumerable) ObjectListView.EnumerableToArray(this.objects, true);
    }

    protected virtual void TriggerFormatRowEvent(int rowIndex, int displayIndex, OLVListItem olvi)
    {
      FormatRowEventArgs args1 = new FormatRowEventArgs();
      args1.ListView = this;
      args1.RowIndex = rowIndex;
      args1.DisplayIndex = displayIndex;
      args1.Item = olvi;
      args1.UseCellFormatEvents = this.UseCellFormatEvents;
      this.OnFormatRow(args1);
      if (!args1.UseCellFormatEvents || this.View != View.Details)
        return;
      olvi.UseItemStyleForSubItems = false;
      Color backColor = olvi.BackColor;
      Color foreColor = olvi.ForeColor;
      foreach (OLVListSubItem subItem in olvi.SubItems)
      {
        subItem.BackColor = backColor;
        subItem.ForeColor = foreColor;
      }
      FormatCellEventArgs args2 = new FormatCellEventArgs();
      args2.ListView = this;
      args2.RowIndex = rowIndex;
      args2.DisplayIndex = displayIndex;
      args2.Item = olvi;
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        args2.ColumnIndex = index;
        args2.Column = this.GetColumn(index);
        args2.SubItem = olvi.GetSubItem(index);
        this.OnFormatCell(args2);
      }
    }

    public virtual void Reset()
    {
      this.Clear();
      this.AllColumns.Clear();
      this.ClearObjects();
      this.PrimarySortColumn = (OLVColumn) null;
      this.SecondarySortColumn = (OLVColumn) null;
      this.ClearDisabledObjects();
      this.ClearPersistentCheckState();
      this.ClearUrlVisited();
      this.ClearHotItem();
    }

    void ISupportInitialize.BeginInit() => this.Frozen = true;

    void ISupportInitialize.EndInit()
    {
      if (this.RowHeight != -1)
      {
        this.SmallImageList = this.SmallImageList;
        if (this.CheckBoxes)
          this.InitializeStateImageList();
      }
      if (this.UseCustomSelectionColors)
        this.EnableCustomSelectionColors();
      if (this.UseSubItemCheckBoxes || this.VirtualMode && this.CheckBoxes)
        this.SetupSubItemCheckBoxes();
      this.Frozen = false;
    }

    private void SetupBaseImageList()
    {
      Size imageSize;
      int num;
      if (this.rowHeight != -1 && this.View == View.Details)
      {
        if (this.shadowedImageList != null)
        {
          imageSize = this.shadowedImageList.ImageSize;
          num = imageSize.Height == this.rowHeight ? 1 : 0;
        }
        else
          num = 0;
      }
      else
        num = 1;
      if (num != 0)
      {
        this.BaseSmallImageList = this.shadowedImageList;
      }
      else
      {
        int width;
        if (this.shadowedImageList != null)
        {
          imageSize = this.shadowedImageList.ImageSize;
          width = imageSize.Width;
        }
        else
          width = 16;
        this.BaseSmallImageList = this.MakeResizedImageList(width, this.rowHeight, this.shadowedImageList);
      }
    }

    private ImageList MakeResizedImageList(int width, int height, ImageList source)
    {
      ImageList imageList = new ImageList();
      imageList.ImageSize = new Size(width, height);
      if (source == null)
        return imageList;
      imageList.TransparentColor = source.TransparentColor;
      imageList.ColorDepth = source.ColorDepth;
      for (int index = 0; index < source.Images.Count; ++index)
      {
        Bitmap bitmap = this.MakeResizedImage(width, height, source.Images[index], source.TransparentColor);
        imageList.Images.Add((Image) bitmap);
      }
      foreach (string key in source.Images.Keys)
        imageList.Images.SetKeyName(source.Images.IndexOfKey(key), key);
      return imageList;
    }

    private Bitmap MakeResizedImage(int width, int height, Image image, Color transparent)
    {
      Bitmap bitmap = new Bitmap(width, height);
      Graphics graphics1 = Graphics.FromImage((Image) bitmap);
      graphics1.Clear(transparent);
      Size size1 = bitmap.Size;
      int width1 = size1.Width;
      size1 = image.Size;
      int width2 = size1.Width;
      int num1 = Math.Max(0, (width1 - width2) / 2);
      Size size2 = bitmap.Size;
      int height1 = size2.Height;
      size2 = image.Size;
      int height2 = size2.Height;
      int num2 = Math.Max(0, (height1 - height2) / 2);
      Graphics graphics2 = graphics1;
      Image image1 = image;
      int x = num1;
      int y = num2;
      Size size3 = image.Size;
      int width3 = size3.Width;
      size3 = image.Size;
      int height3 = size3.Height;
      graphics2.DrawImage(image1, x, y, width3, height3);
      return bitmap;
    }

    protected virtual void InitializeStateImageList()
    {
      if (this.DesignMode || !this.CheckBoxes)
        return;
      if (this.StateImageList == null)
      {
        this.StateImageList = new ImageList();
        this.StateImageList.ImageSize = new Size(16, this.RowHeight == -1 ? 16 : this.RowHeight);
        this.StateImageList.ColorDepth = ColorDepth.Depth32Bit;
      }
      if (this.RowHeight != -1 && this.View == View.Details && this.StateImageList.ImageSize.Height != this.RowHeight)
      {
        this.StateImageList = new ImageList();
        this.StateImageList.ImageSize = new Size(16, this.RowHeight);
        this.StateImageList.ColorDepth = ColorDepth.Depth32Bit;
      }
      if (this.StateImageList.Images.Count == 0)
        this.AddCheckStateBitmap(this.StateImageList, "checkbox-unchecked", CheckBoxState.UncheckedNormal);
      if (this.StateImageList.Images.Count <= 1)
        this.AddCheckStateBitmap(this.StateImageList, "checkbox-checked", CheckBoxState.CheckedNormal);
      if (this.TriStateCheckBoxes && this.StateImageList.Images.Count <= 2)
        this.AddCheckStateBitmap(this.StateImageList, "checkbox-indeterminate", CheckBoxState.MixedNormal);
      else if (this.StateImageList.Images.ContainsKey("checkbox-indeterminate"))
        this.StateImageList.Images.RemoveByKey("checkbox-indeterminate");
    }

    public virtual void SetupSubItemCheckBoxes()
    {
      this.ShowImagesOnSubItems = true;
      if (this.SmallImageList != null && this.SmallImageList.Images.ContainsKey("checkbox-checked"))
        return;
      this.InitializeSubItemCheckBoxImages();
    }

    protected virtual void InitializeSubItemCheckBoxImages()
    {
      if (this.DesignMode)
        return;
      ImageList il = this.SmallImageList;
      if (il == null)
      {
        il = new ImageList();
        il.ImageSize = new Size(16, 16);
        il.ColorDepth = ColorDepth.Depth32Bit;
      }
      this.AddCheckStateBitmap(il, "checkbox-checked", CheckBoxState.CheckedNormal);
      this.AddCheckStateBitmap(il, "checkbox-unchecked", CheckBoxState.UncheckedNormal);
      this.AddCheckStateBitmap(il, "checkbox-indeterminate", CheckBoxState.MixedNormal);
      this.SmallImageList = il;
    }

    private void AddCheckStateBitmap(ImageList il, string key, CheckBoxState boxState)
    {
      Size imageSize = il.ImageSize;
      int width = imageSize.Width;
      imageSize = il.ImageSize;
      int height = imageSize.Height;
      Bitmap bitmap = new Bitmap(width, height);
      Graphics g = Graphics.FromImage((Image) bitmap);
      g.Clear(il.TransparentColor);
      Point glyphLocation = new Point(bitmap.Width / 2 - 5, bitmap.Height / 2 - 6);
      CheckBoxRenderer.DrawCheckBox(g, glyphLocation, boxState);
      il.Images.Add(key, (Image) bitmap);
    }

    protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
    {
      e.DrawDefault = true;
      base.OnDrawColumnHeader(e);
    }

    protected override void OnDrawItem(DrawListViewItemEventArgs e)
    {
      if (this.View == View.Details)
        e.DrawDefault = false;
      else if (this.ItemRenderer == null)
      {
        e.DrawDefault = true;
      }
      else
      {
        object rowObject = ((OLVListItem) e.Item).RowObject;
        e.DrawDefault = !this.ItemRenderer.RenderItem(e, e.Graphics, e.Bounds, rowObject);
      }
      if (!e.DrawDefault)
        return;
      base.OnDrawItem(e);
    }

    protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
    {
      if (this.DesignMode)
      {
        e.DrawDefault = true;
      }
      else
      {
        Rectangle bounds = e.Bounds;
        IRenderer renderer = this.GetColumn(e.ColumnIndex).Renderer ?? this.DefaultRenderer;
        BufferedGraphics bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, bounds);
        Graphics graphics = bufferedGraphics.Graphics;
        graphics.TextRenderingHint = ObjectListView.TextRenderingHint;
        graphics.SmoothingMode = ObjectListView.SmoothingMode;
        e.DrawDefault = !renderer.RenderSubItem(e, graphics, bounds, ((OLVListItem) e.Item).RowObject);
        if (!e.DrawDefault)
          bufferedGraphics.Render();
        using (Pen pen = new Pen(Color.FromArgb(160, 160, 160), 1f))
          graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
        bufferedGraphics.Dispose();
      }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      this.lastMouseDownClickCount = e.Clicks;
      base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      if (!this.Created)
        return;
      this.UpdateHotItem(new Point(-1, -1));
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!this.Created)
        return;
      this.HandleMouseMove(e.Location);
    }

    internal void HandleMouseMove(Point pt)
    {
      CellOverEventArgs args = new CellOverEventArgs();
      this.BuildCellEvent((CellEventArgs) args, pt);
      this.OnCellOver(args);
      this.MouseMoveHitTest = args.HitTest;
      if (args.Handled)
        return;
      this.UpdateHotItem(args.HitTest);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      if (!this.Created)
        return;
      if (e.Button == MouseButtons.Right && !this.fakeRightClick)
      {
        this.OnRightMouseUp(e);
      }
      else
      {
        this.fakeRightClick = false;
        CellClickEventArgs args = new CellClickEventArgs();
        this.BuildCellEvent((CellEventArgs) args, e.Location);
        args.ClickCount = this.lastMouseDownClickCount;
        this.OnCellClick(args);
        if (args.Handled)
          return;
        if (this.UseHyperlinks && args.HitTest.HitTestLocation == HitTestLocation.Text && args.SubItem != null && !string.IsNullOrEmpty(args.SubItem.Url))
          this.BeginInvoke((Delegate) (() => this.ProcessHyperlinkClicked(args)));
        if (!this.ShouldStartCellEdit(e) || args.HitTest.HitTestLocation == HitTestLocation.Nothing || this.CellEditActivation == ObjectListView.CellEditActivateMode.SingleClick && args.ColumnIndex <= 0 || args.Column != null && args.Column.CheckBoxes)
          return;
        this.EditSubItem(args.Item, args.ColumnIndex);
      }
    }

    protected virtual void ProcessHyperlinkClicked(CellClickEventArgs e)
    {
      HyperlinkClickedEventArgs clickedEventArgs = new HyperlinkClickedEventArgs();
      clickedEventArgs.HitTest = e.HitTest;
      clickedEventArgs.ListView = this;
      clickedEventArgs.Location = new Point(-1, -1);
      clickedEventArgs.Item = e.Item;
      clickedEventArgs.SubItem = e.SubItem;
      clickedEventArgs.Model = e.Model;
      clickedEventArgs.ColumnIndex = e.ColumnIndex;
      clickedEventArgs.Column = e.Column;
      clickedEventArgs.RowIndex = e.RowIndex;
      clickedEventArgs.ModifierKeys = Control.ModifierKeys;
      clickedEventArgs.Url = e.SubItem.Url;
      this.OnHyperlinkClicked(clickedEventArgs);
      if (clickedEventArgs.Handled)
        return;
      this.StandardHyperlinkClickedProcessing(clickedEventArgs);
    }

    protected virtual void StandardHyperlinkClickedProcessing(HyperlinkClickedEventArgs args)
    {
      Cursor cursor = this.Cursor;
      try
      {
        this.Cursor = Cursors.WaitCursor;
        Process.Start(args.Url);
      }
      catch (Win32Exception ex)
      {
        SystemSounds.Beep.Play();
      }
      finally
      {
        this.Cursor = cursor;
      }
      this.MarkUrlVisited(args.Url);
      this.RefreshHotItem();
    }

    protected virtual void OnRightMouseUp(MouseEventArgs e)
    {
      CellRightClickEventArgs args = new CellRightClickEventArgs();
      this.BuildCellEvent((CellEventArgs) args, e.Location);
      this.OnCellRightClick(args);
      if (args.Handled || args.MenuStrip == null)
        return;
      args.MenuStrip.Show((Control) this, args.Location);
    }

    internal void BuildCellEvent(CellEventArgs args, Point location)
    {
      OlvListViewHitTestInfo listViewHitTestInfo = this.OlvHitTest(location.X, location.Y);
      args.HitTest = listViewHitTestInfo;
      args.ListView = this;
      args.Location = location;
      args.Item = listViewHitTestInfo.Item;
      args.SubItem = listViewHitTestInfo.SubItem;
      args.Model = listViewHitTestInfo.RowObject;
      args.ColumnIndex = listViewHitTestInfo.ColumnIndex;
      args.Column = listViewHitTestInfo.Column;
      if (listViewHitTestInfo.Item != null)
        args.RowIndex = listViewHitTestInfo.Item.Index;
      args.ModifierKeys = Control.ModifierKeys;
      if (args.Item == null || args.ListView.View == View.Details)
        return;
      args.ColumnIndex = 0;
      args.Column = args.ListView.GetColumn(0);
      args.SubItem = args.Item.GetSubItem(0);
    }

    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      if (this.SelectionEventsSuspended)
        return;
      base.OnSelectedIndexChanged(e);
      if (this.hasIdleHandler)
        return;
      this.hasIdleHandler = true;
      this.RunWhenIdle(new EventHandler(this.HandleApplicationIdle));
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);
      this.Invoke((Delegate) new MethodInvoker(this.OnControlCreated));
    }

    protected virtual void OnControlCreated()
    {
      this.HeaderControl.WordWrap = this.HeaderWordWrap;
      this.HotItemStyle = this.HotItemStyle;
      NativeMethods.SetGroupImageList(this, this.GroupImageList);
      this.UseExplorerTheme = this.UseExplorerTheme;
      this.RememberDisplayIndicies();
      this.SetGroupSpacing();
      if (!this.VirtualMode)
        return;
      this.ApplyExtendedStyles();
    }

    protected virtual bool ShouldStartCellEdit(MouseEventArgs e)
    {
      if (this.IsCellEditing || e.Button != MouseButtons.Left && (e.Button != MouseButtons.Right || !this.fakeRightClick) || (uint) (Control.ModifierKeys & (Keys.Shift | Keys.Control | Keys.Alt)) > 0U)
        return false;
      return this.lastMouseDownClickCount == 1 && this.CellEditActivation == ObjectListView.CellEditActivateMode.SingleClick || this.lastMouseDownClickCount == 2 && this.CellEditActivation == ObjectListView.CellEditActivateMode.DoubleClick;
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (this.IsCellEditing)
        return this.CellEditKeyEngine.HandleKey(this, keyData);
      if (keyData == Keys.F2)
      {
        this.EditSubItem((OLVListItem) this.FocusedItem, 0);
        return base.ProcessDialogKey(keyData);
      }
      if (this.CopySelectionOnControlC && keyData == (Keys.C | Keys.Control))
      {
        this.CopySelectionToClipboard();
        return true;
      }
      if (!this.SelectAllOnControlA || keyData != (Keys.A | Keys.Control))
        return base.ProcessDialogKey(keyData);
      this.SelectAll();
      return true;
    }

    public virtual void EditModel(object rowModel)
    {
      OLVListItem olvListItem = this.ModelToItem(rowModel);
      if (olvListItem == null)
        return;
      for (int index = 0; index < olvListItem.SubItems.Count; ++index)
      {
        if (this.GetColumn(index).IsEditable)
        {
          this.StartCellEdit(olvListItem, index);
          break;
        }
      }
    }

    public virtual void EditSubItem(OLVListItem item, int subItemIndex)
    {
      if (item == null || subItemIndex < 0 && subItemIndex >= item.SubItems.Count || this.CellEditActivation == ObjectListView.CellEditActivateMode.None || !this.GetColumn(subItemIndex).IsEditable || !item.Enabled)
        return;
      this.StartCellEdit(item, subItemIndex);
    }

    public virtual void StartCellEdit(OLVListItem item, int subItemIndex)
    {
      OLVColumn column = this.GetColumn(subItemIndex);
      Control cellEditor = this.GetCellEditor(item, subItemIndex);
      Rectangle cellEditorBounds = this.CalculateCellEditorBounds(item, subItemIndex, cellEditor.PreferredSize);
      cellEditor.Bounds = cellEditorBounds;
      Munger.PutProperty((object) cellEditor, "TextAlign", (object) column.TextAlign);
      this.SetControlValue(cellEditor, column.GetValue(item.RowObject), column.GetStringValue(item.RowObject));
      this.CellEditEventArgs = new CellEditEventArgs(column, cellEditor, cellEditorBounds, item, subItemIndex);
      this.OnCellEditStarting(this.CellEditEventArgs);
      if (this.CellEditEventArgs.Cancel)
        return;
      this.cellEditor = this.CellEditEventArgs.Control;
      if (this.View != View.Tile && !this.OwnerDraw && this.cellEditor.Height != cellEditorBounds.Height)
        this.cellEditor.Top += (cellEditorBounds.Height - this.cellEditor.Height) / 2;
      this.Invalidate();
      this.Controls.Add(this.cellEditor);
      this.ConfigureControl();
      this.PauseAnimations(true);
    }

    public Rectangle CalculateCellEditorBounds(
      OLVListItem item,
      int subItemIndex,
      Size preferredSize)
    {
      Rectangle rectangle = this.View == View.Details ? item.GetSubItemBounds(subItemIndex) : this.GetItemRect(item.Index, ItemBoundsPortion.Label);
      return this.OwnerDraw ? this.CalculateCellEditorBoundsOwnerDrawn(item, subItemIndex, rectangle, preferredSize) : this.CalculateCellEditorBoundsStandard(item, subItemIndex, rectangle, preferredSize);
    }

    protected Rectangle CalculateCellEditorBoundsOwnerDrawn(
      OLVListItem item,
      int subItemIndex,
      Rectangle r,
      Size preferredSize)
    {
      IRenderer renderer = this.View == View.Details ? this.GetColumn(subItemIndex).Renderer ?? this.DefaultRenderer : this.ItemRenderer;
      if (renderer == null)
        return r;
      using (Graphics graphics = this.CreateGraphics())
        return renderer.GetEditRectangle(graphics, r, item, subItemIndex, preferredSize);
    }

    protected Rectangle CalculateCellEditorBoundsStandard(
      OLVListItem item,
      int subItemIndex,
      Rectangle cellBounds,
      Size preferredSize)
    {
      if (this.View != View.Details)
        return cellBounds;
      int num = 0;
      object imageSelector = (object) null;
      if (subItemIndex == 0)
        imageSelector = item.ImageSelector;
      else if (this.OwnerDraw || this.ShowImagesOnSubItems)
        imageSelector = item.GetSubItem(subItemIndex).ImageSelector;
      if (this.GetActualImageIndex(imageSelector) != -1)
        num += this.SmallImageSize.Width + 2;
      if (this.CheckBoxes && this.StateImageList != null && subItemIndex == 0)
        num += this.StateImageList.ImageSize.Width + 2;
      if (subItemIndex == 0 && item.IndentCount > 0)
        num += this.SmallImageSize.Width * item.IndentCount;
      if (num > 0)
      {
        cellBounds.X += num;
        cellBounds.Width -= num;
      }
      return cellBounds;
    }

    protected virtual void SetControlValue(Control control, object value, string stringValue)
    {
      System.Windows.Forms.ComboBox cb = control as System.Windows.Forms.ComboBox;
      if (cb != null)
      {
        if (cb.Created)
          cb.SelectedValue = value;
        else
          this.BeginInvoke((Delegate) (() => cb.SelectedValue = value));
      }
      else
      {
        if (Munger.PutProperty((object) control, "Value", value))
          return;
        try
        {
          string str = value as string;
          control.Text = str ?? stringValue;
        }
        catch (ArgumentOutOfRangeException ex)
        {
        }
      }
    }

    protected virtual void ConfigureControl()
    {
      this.cellEditor.Validating += new CancelEventHandler(this.CellEditor_Validating);
      this.cellEditor.Select();
    }

    protected virtual object GetControlValue(Control control)
    {
      switch (control)
      {
        case null:
          return (object) null;
        case System.Windows.Forms.TextBox textBox:
          return (object) textBox.Text;
        case System.Windows.Forms.ComboBox comboBox:
          return comboBox.SelectedValue;
        case System.Windows.Forms.CheckBox checkBox:
          return (object) checkBox.Checked;
        default:
          try
          {
            return control.GetType().InvokeMember("Value", BindingFlags.GetProperty, (Binder) null, (object) control, (object[]) null);
          }
          catch (MissingMethodException ex)
          {
            return (object) control.Text;
          }
          catch (MissingFieldException ex)
          {
            return (object) control.Text;
          }
      }
    }

    protected virtual void CellEditor_Validating(object sender, CancelEventArgs e)
    {
      this.CellEditEventArgs.Cancel = false;
      this.CellEditEventArgs.NewValue = this.GetControlValue(this.cellEditor);
      this.OnCellEditorValidating(this.CellEditEventArgs);
      if (this.CellEditEventArgs.Cancel)
      {
        this.CellEditEventArgs.Control.Select();
        e.Cancel = true;
      }
      else
        this.FinishCellEdit();
    }

    public virtual Rectangle CalculateCellBounds(OLVListItem item, int subItemIndex) => this.CalculateCellBounds(item, subItemIndex, ItemBoundsPortion.Label);

    public virtual Rectangle CalculateCellTextBounds(OLVListItem item, int subItemIndex) => this.CalculateCellBounds(item, subItemIndex, ItemBoundsPortion.ItemOnly);

    private Rectangle CalculateCellBounds(
      OLVListItem item,
      int subItemIndex,
      ItemBoundsPortion portion)
    {
      if (subItemIndex > 0)
        return item.SubItems[subItemIndex].Bounds;
      Rectangle itemRect = this.GetItemRect(item.Index, portion);
      if (itemRect.Y < -10000000 || itemRect.Y > 10000000)
        itemRect.Y = item.Bounds.Y;
      if (this.View != View.Details)
        return itemRect;
      Point scrolledColumnSides = NativeMethods.GetScrolledColumnSides((System.Windows.Forms.ListView) this, 0);
      itemRect.X = scrolledColumnSides.X + 4;
      itemRect.Width = scrolledColumnSides.Y - scrolledColumnSides.X - 5;
      return itemRect;
    }

    public virtual Rectangle CalculateColumnVisibleBounds(
      Rectangle bounds,
      OLVColumn column)
    {
      if (column == null || this.View != View.Details || this.GetItemCount() == 0 || !column.IsVisible)
        return Rectangle.Empty;
      Point scrolledColumnSides = NativeMethods.GetScrolledColumnSides((System.Windows.Forms.ListView) this, column.Index);
      if (scrolledColumnSides.X == -1)
        return Rectangle.Empty;
      Rectangle rectangle = new Rectangle(scrolledColumnSides.X, bounds.Top, scrolledColumnSides.Y - scrolledColumnSides.X, bounds.Bottom);
      OLVListItem itemInDisplayOrder = this.GetLastItemInDisplayOrder();
      if (itemInDisplayOrder != null)
      {
        Rectangle bounds1 = itemInDisplayOrder.Bounds;
        if (!bounds1.IsEmpty && bounds1.Bottom < rectangle.Bottom)
          rectangle.Height = bounds1.Bottom - rectangle.Top;
      }
      return rectangle;
    }

    protected virtual Control GetCellEditor(OLVListItem item, int subItemIndex)
    {
      OLVColumn column = this.GetColumn(subItemIndex);
      object obj = column.GetValue(item.RowObject) ?? this.GetFirstNonNullValue(column);
      return ObjectListView.EditorRegistry.GetEditor(item.RowObject, column, obj) ?? this.MakeDefaultCellEditor(column);
    }

    internal object GetFirstNonNullValue(OLVColumn column)
    {
      for (int index = 0; index < Math.Min(this.GetItemCount(), 1000); ++index)
      {
        object obj = column.GetValue(this.GetModelObject(index));
        if (obj != null)
          return obj;
      }
      return (object) null;
    }

    protected virtual Control MakeDefaultCellEditor(OLVColumn column)
    {
      System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
      if (column.AutoCompleteEditor)
        this.ConfigureAutoComplete(tb, column);
      return (Control) tb;
    }

    public void ConfigureAutoComplete(System.Windows.Forms.TextBox tb, OLVColumn column) => this.ConfigureAutoComplete(tb, column, 1000);

    public void ConfigureAutoComplete(System.Windows.Forms.TextBox tb, OLVColumn column, int maxRows)
    {
      maxRows = Math.Min(this.GetItemCount(), maxRows);
      tb.AutoCompleteCustomSource.Clear();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      List<string> stringList = new List<string>();
      for (int index = 0; index < maxRows; ++index)
      {
        string stringValue = column.GetStringValue(this.GetModelObject(index));
        if (!string.IsNullOrEmpty(stringValue) && !dictionary.ContainsKey(stringValue))
        {
          stringList.Add(stringValue);
          dictionary[stringValue] = true;
        }
      }
      tb.AutoCompleteCustomSource.AddRange(stringList.ToArray());
      tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
      tb.AutoCompleteMode = column.AutoCompleteEditorMode;
    }

    public virtual void CancelCellEdit()
    {
      if (!this.IsCellEditing)
        return;
      this.CellEditEventArgs.Cancel = true;
      this.CellEditEventArgs.NewValue = this.GetControlValue(this.cellEditor);
      this.OnCellEditFinishing(this.CellEditEventArgs);
      this.CleanupCellEdit(false, this.CellEditEventArgs.AutoDispose);
    }

    public virtual bool PossibleFinishCellEditing() => this.PossibleFinishCellEditing(false);

    public virtual bool PossibleFinishCellEditing(bool expectingCellEdit)
    {
      if (!this.IsCellEditing)
        return true;
      this.CellEditEventArgs.Cancel = false;
      this.CellEditEventArgs.NewValue = this.GetControlValue(this.cellEditor);
      this.OnCellEditorValidating(this.CellEditEventArgs);
      if (this.CellEditEventArgs.Cancel)
        return false;
      this.FinishCellEdit(expectingCellEdit);
      return true;
    }

    public virtual void FinishCellEdit() => this.FinishCellEdit(false);

    public virtual void FinishCellEdit(bool expectingCellEdit)
    {
      if (!this.IsCellEditing)
        return;
      this.CellEditEventArgs.Cancel = false;
      this.CellEditEventArgs.NewValue = this.GetControlValue(this.cellEditor);
      this.OnCellEditFinishing(this.CellEditEventArgs);
      if (!this.CellEditEventArgs.Cancel)
      {
        this.CellEditEventArgs.Column.PutValue(this.CellEditEventArgs.RowObject, this.CellEditEventArgs.NewValue);
        this.RefreshItem(this.CellEditEventArgs.ListViewItem);
      }
      this.CleanupCellEdit(expectingCellEdit, this.CellEditEventArgs.AutoDispose);
    }

    protected virtual void CleanupCellEdit(bool expectingCellEdit, bool disposeOfCellEditor)
    {
      if (this.cellEditor == null)
        return;
      this.cellEditor.Validating -= new CancelEventHandler(this.CellEditor_Validating);
      Control soonToBeOldCellEditor = this.cellEditor;
      this.cellEditor = (Control) null;
      EventHandler toBeRun = (EventHandler) null;
      toBeRun = (EventHandler) ((sender, e) =>
      {
        Application.Idle -= toBeRun;
        this.Controls.Remove(soonToBeOldCellEditor);
        if (disposeOfCellEditor)
          soonToBeOldCellEditor.Dispose();
        this.Invalidate();
        if (this.IsCellEditing)
          return;
        if (this.Focused)
          this.Select();
        this.PauseAnimations(false);
      });
      if (expectingCellEdit)
        this.RunWhenIdle(toBeRun);
      else
        toBeRun((object) null, (EventArgs) null);
    }

    public virtual void ClearHotItem() => this.UpdateHotItem(new Point(-1, -1));

    public virtual void RefreshHotItem() => this.UpdateHotItem(this.PointToClient(Cursor.Position));

    protected virtual void UpdateHotItem(Point pt) => this.UpdateHotItem(this.OlvHitTest(pt.X, pt.Y));

    protected virtual void UpdateHotItem(OlvListViewHitTestInfo hti)
    {
      if (!this.UseHotItem && !this.UseHyperlinks)
        return;
      int rowIndex = hti.RowIndex;
      int num = hti.ColumnIndex;
      HitTestLocation hitTestLocation = hti.HitTestLocation;
      HitTestLocationEx hitTestLocationEx = hti.HitTestLocationEx;
      OLVGroup group = hti.Group;
      if (rowIndex >= 0 && this.View != View.Details)
        num = 0;
      if (this.HotRowIndex == rowIndex && this.HotColumnIndex == num && this.HotCellHitLocation == hitTestLocation && this.HotCellHitLocationEx == hitTestLocationEx && this.HotGroup == group)
        return;
      HotItemChangedEventArgs e = new HotItemChangedEventArgs();
      e.HotCellHitLocation = hitTestLocation;
      e.HotCellHitLocationEx = hitTestLocationEx;
      e.HotColumnIndex = num;
      e.HotRowIndex = rowIndex;
      e.HotGroup = group;
      e.OldHotCellHitLocation = this.HotCellHitLocation;
      e.OldHotCellHitLocationEx = this.HotCellHitLocationEx;
      e.OldHotColumnIndex = this.HotColumnIndex;
      e.OldHotRowIndex = this.HotRowIndex;
      e.OldHotGroup = this.HotGroup;
      this.OnHotItemChanged(e);
      this.HotRowIndex = rowIndex;
      this.HotColumnIndex = num;
      this.HotCellHitLocation = hitTestLocation;
      this.HotCellHitLocationEx = hitTestLocationEx;
      this.HotGroup = group;
      if (e.Handled)
        return;
      this.BeginUpdate();
      try
      {
        this.Invalidate();
        if (e.OldHotRowIndex != -1)
          this.UnapplyHotItem(e.OldHotRowIndex);
        if (this.HotRowIndex != -1)
        {
          if (this.VirtualMode)
          {
            this.ClearCachedInfo();
            this.RedrawItems(this.HotRowIndex, this.HotRowIndex, true);
          }
          else
            this.UpdateHotRow(this.HotRowIndex, this.HotColumnIndex, this.HotCellHitLocation, hti.Item);
        }
        if (!this.UseHotItem || this.HotItemStyle == null || this.HotItemStyle.Overlay == null)
          return;
        this.RefreshOverlays();
      }
      finally
      {
        this.EndUpdate();
      }
    }

    protected virtual void UpdateHotRow(OLVListItem olvi) => this.UpdateHotRow(this.HotRowIndex, this.HotColumnIndex, this.HotCellHitLocation, olvi);

    protected virtual void UpdateHotRow(
      int rowIndex,
      int columnIndex,
      HitTestLocation hitLocation,
      OLVListItem olvi)
    {
      if (rowIndex < 0 || columnIndex < 0)
        return;
      if (this.UseHyperlinks)
      {
        OLVColumn column = this.GetColumn(columnIndex);
        OLVListSubItem subItem = olvi.GetSubItem(columnIndex);
        if (column.Hyperlink && hitLocation == HitTestLocation.Text && !string.IsNullOrEmpty(subItem.Url))
        {
          this.ApplyCellStyle(olvi, columnIndex, (IItemStyle) this.HyperlinkStyle.Over);
          Cursor overCursor = this.HyperlinkStyle.OverCursor;
          if ((object) overCursor == null)
            overCursor = Cursors.Default;
          this.Cursor = overCursor;
        }
        else
          this.Cursor = Cursors.Default;
      }
      if (!this.UseHotItem || olvi.Selected || !olvi.Enabled)
        return;
      this.ApplyRowStyle(olvi, (IItemStyle) this.HotItemStyle, !this.FullRowSelect);
    }

    protected virtual void ApplyRowStyle(
      OLVListItem olvi,
      IItemStyle style,
      bool primaryColumnOnly)
    {
      if (style == null)
        return;
      if (!primaryColumnOnly || this.View != View.Details)
      {
        Font font = style.Font ?? olvi.Font;
        if ((uint) style.FontStyle > 0U)
          font = new Font(font ?? this.Font, style.FontStyle);
        if (!object.Equals((object) font, (object) olvi.Font))
        {
          if (olvi.UseItemStyleForSubItems)
          {
            olvi.Font = font;
          }
          else
          {
            foreach (ListViewItem.ListViewSubItem subItem in olvi.SubItems)
              subItem.Font = font;
          }
        }
        if (!style.ForeColor.IsEmpty)
        {
          if (olvi.UseItemStyleForSubItems)
          {
            olvi.ForeColor = style.ForeColor;
          }
          else
          {
            foreach (ListViewItem.ListViewSubItem subItem in olvi.SubItems)
              subItem.ForeColor = style.ForeColor;
          }
        }
        if (style.BackColor.IsEmpty)
          return;
        if (olvi.UseItemStyleForSubItems)
        {
          olvi.BackColor = style.BackColor;
        }
        else
        {
          foreach (ListViewItem.ListViewSubItem subItem in olvi.SubItems)
            subItem.BackColor = style.BackColor;
        }
      }
      else
      {
        olvi.UseItemStyleForSubItems = false;
        foreach (ListViewItem.ListViewSubItem subItem in olvi.SubItems)
        {
          ListViewItem.ListViewSubItem listViewSubItem1 = subItem;
          Color color1 = style.BackColor;
          Color color2 = color1.IsEmpty ? olvi.BackColor : style.BackColor;
          listViewSubItem1.BackColor = color2;
          ListViewItem.ListViewSubItem listViewSubItem2 = subItem;
          color1 = style.ForeColor;
          Color color3 = color1.IsEmpty ? olvi.ForeColor : style.ForeColor;
          listViewSubItem2.ForeColor = color3;
        }
        this.ApplyCellStyle(olvi, 0, style);
      }
    }

    protected virtual void ApplyCellStyle(OLVListItem olvi, int columnIndex, IItemStyle style)
    {
      if (style == null || this.View != View.Details && columnIndex > 0)
        return;
      olvi.UseItemStyleForSubItems = false;
      ListViewItem.ListViewSubItem subItem = olvi.SubItems[columnIndex];
      if (style.Font != null)
        subItem.Font = style.Font;
      if ((uint) style.FontStyle > 0U)
        subItem.Font = new Font(subItem.Font ?? olvi.Font ?? this.Font, style.FontStyle);
      if (!style.ForeColor.IsEmpty)
        subItem.ForeColor = style.ForeColor;
      if (style.BackColor.IsEmpty)
        return;
      subItem.BackColor = style.BackColor;
    }

    protected virtual void UnapplyHotItem(int index)
    {
      this.Cursor = Cursors.Default;
      if (this.VirtualMode)
      {
        if (index >= this.VirtualListSize)
          return;
        this.RedrawItems(index, index, true);
      }
      else
      {
        OLVListItem olvi = this.GetItem(index);
        if (olvi != null)
          this.RefreshItem(olvi);
      }
    }

    protected override void OnItemDrag(ItemDragEventArgs e)
    {
      base.OnItemDrag(e);
      if (this.DragSource == null)
        return;
      object obj = this.DragSource.StartDrag(this, e.Button, (OLVListItem) e.Item);
      if (obj == null)
        return;
      DragDropEffects effect = this.DoDragDrop(obj, this.DragSource.GetAllowedEffects(obj));
      this.DragSource.EndDrag(obj, effect);
    }

    protected override void OnDragEnter(DragEventArgs args)
    {
      base.OnDragEnter(args);
      if (this.DropSink == null)
        return;
      this.DropSink.Enter(args);
    }

    protected override void OnDragOver(DragEventArgs args)
    {
      base.OnDragOver(args);
      if (this.DropSink == null)
        return;
      this.DropSink.Over(args);
    }

    protected override void OnDragDrop(DragEventArgs args)
    {
      base.OnDragDrop(args);
      if (this.DropSink == null)
        return;
      this.DropSink.Drop(args);
    }

    protected override void OnDragLeave(EventArgs e)
    {
      base.OnDragLeave(e);
      if (this.DropSink == null)
        return;
      this.DropSink.Leave();
    }

    protected override void OnGiveFeedback(GiveFeedbackEventArgs args)
    {
      base.OnGiveFeedback(args);
      if (this.DropSink == null)
        return;
      this.DropSink.GiveFeedback(args);
    }

    protected override void OnQueryContinueDrag(QueryContinueDragEventArgs args)
    {
      base.OnQueryContinueDrag(args);
      if (this.DropSink == null)
        return;
      this.DropSink.QueryContinue(args);
    }

    public virtual void AddDecoration(IDecoration decoration)
    {
      if (decoration == null)
        return;
      this.Decorations.Add(decoration);
      this.Invalidate();
    }

    public virtual void AddOverlay(IOverlay overlay)
    {
      if (overlay == null)
        return;
      this.Overlays.Add(overlay);
      this.Invalidate();
    }

    protected virtual void DrawAllDecorations(Graphics g, List<OLVListItem> itemsThatWereRedrawn)
    {
      g.TextRenderingHint = ObjectListView.TextRenderingHint;
      g.SmoothingMode = ObjectListView.SmoothingMode;
      Rectangle contentRectangle = this.ContentRectangle;
      if (this.HasEmptyListMsg && this.GetItemCount() == 0)
        this.EmptyListMsgOverlay.Draw(this, g, contentRectangle);
      if (this.DropSink != null)
        this.DropSink.DrawFeedback(g, contentRectangle);
      foreach (OLVListItem olvListItem in itemsThatWereRedrawn)
      {
        if (olvListItem.HasDecoration)
        {
          foreach (IDecoration decoration in (IEnumerable<IDecoration>) olvListItem.Decorations)
          {
            decoration.ListItem = olvListItem;
            decoration.SubItem = (OLVListSubItem) null;
            decoration.Draw(this, g, contentRectangle);
          }
        }
        foreach (OLVListSubItem subItem in olvListItem.SubItems)
        {
          if (subItem.HasDecoration)
          {
            foreach (IDecoration decoration in (IEnumerable<IDecoration>) subItem.Decorations)
            {
              decoration.ListItem = olvListItem;
              decoration.SubItem = subItem;
              decoration.Draw(this, g, contentRectangle);
            }
          }
        }
        if (this.SelectedRowDecoration != null && olvListItem.Selected && olvListItem.Enabled)
        {
          this.SelectedRowDecoration.ListItem = olvListItem;
          this.SelectedRowDecoration.SubItem = (OLVListSubItem) null;
          this.SelectedRowDecoration.Draw(this, g, contentRectangle);
        }
      }
      foreach (IDecoration decoration in (IEnumerable<IDecoration>) this.Decorations)
      {
        decoration.ListItem = (OLVListItem) null;
        decoration.SubItem = (OLVListSubItem) null;
        decoration.Draw(this, g, contentRectangle);
      }
      if (this.UseHotItem && this.HotItemStyle != null && this.HotItemStyle.Decoration != null)
      {
        IDecoration decoration = this.HotItemStyle.Decoration;
        decoration.ListItem = this.GetItem(this.HotRowIndex);
        if (decoration.ListItem == null || decoration.ListItem.Enabled)
        {
          decoration.SubItem = decoration.ListItem == null ? (OLVListSubItem) null : decoration.ListItem.GetSubItem(this.HotColumnIndex);
          decoration.Draw(this, g, contentRectangle);
        }
      }
      if (!this.DesignMode)
        return;
      foreach (IOverlay overlay in (IEnumerable<IOverlay>) this.Overlays)
        overlay.Draw(this, g, contentRectangle);
    }

    public virtual bool HasDecoration(IDecoration decoration) => this.Decorations.Contains(decoration);

    public virtual bool HasOverlay(IOverlay overlay) => this.Overlays.Contains(overlay);

    public virtual void HideOverlays()
    {
      foreach (GlassPanelForm glassPanel in this.glassPanels)
        glassPanel.HideGlass();
    }

    protected virtual void InitializeEmptyListMsgOverlay()
    {
      TextOverlay textOverlay = new TextOverlay();
      textOverlay.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
      textOverlay.TextColor = SystemColors.ControlDarkDark;
      textOverlay.BackColor = Color.BlanchedAlmond;
      textOverlay.BorderColor = SystemColors.ControlDark;
      textOverlay.BorderWidth = 2f;
      this.EmptyListMsgOverlay = (IOverlay) textOverlay;
    }

    protected virtual void InitializeStandardOverlays()
    {
      this.OverlayImage = new ImageOverlay();
      this.AddOverlay((IOverlay) this.OverlayImage);
      this.OverlayText = new TextOverlay();
      this.AddOverlay((IOverlay) this.OverlayText);
    }

    public virtual void ShowOverlays()
    {
      if (!this.ShouldShowOverlays())
        return;
      if (this.Overlays.Count != this.glassPanels.Count)
      {
        foreach (IOverlay overlay in (IEnumerable<IOverlay>) this.Overlays)
        {
          if (this.FindGlassPanelForOverlay(overlay) == null)
          {
            GlassPanelForm glassPanelForm = new GlassPanelForm();
            glassPanelForm.Bind(this, overlay);
            this.glassPanels.Add(glassPanelForm);
          }
        }
      }
      foreach (GlassPanelForm glassPanel in this.glassPanels)
        glassPanel.ShowGlass();
    }

    private bool ShouldShowOverlays() => !this.DesignMode && this.UseOverlays && this.HasOverlays && Screen.PrimaryScreen.BitsPerPixel >= 32;

    private GlassPanelForm FindGlassPanelForOverlay(IOverlay overlay) => this.glassPanels.Find((Predicate<GlassPanelForm>) (x => x.Overlay == overlay));

    public virtual void RefreshOverlays()
    {
      foreach (Control glassPanel in this.glassPanels)
        glassPanel.Invalidate();
    }

    public virtual void RefreshOverlay(IOverlay overlay) => this.FindGlassPanelForOverlay(overlay)?.Invalidate();

    public virtual void RemoveDecoration(IDecoration decoration)
    {
      if (decoration == null)
        return;
      this.Decorations.Remove(decoration);
      this.Invalidate();
    }

    public virtual void RemoveOverlay(IOverlay overlay)
    {
      if (overlay == null)
        return;
      this.Overlays.Remove(overlay);
      GlassPanelForm glassPanelForOverlay = this.FindGlassPanelForOverlay(overlay);
      if (glassPanelForOverlay == null)
        return;
      this.glassPanels.Remove(glassPanelForOverlay);
      glassPanelForOverlay.Unbind();
      glassPanelForOverlay.Dispose();
    }

    public virtual IModelFilter CreateColumnFilter()
    {
      List<IModelFilter> filters = new List<IModelFilter>();
      foreach (OLVColumn column in this.Columns)
      {
        IModelFilter valueBasedFilter = column.ValueBasedFilter;
        if (valueBasedFilter != null)
          filters.Add(valueBasedFilter);
      }
      return filters.Count == 0 ? (IModelFilter) null : (IModelFilter) new CompositeAllFilter(filters);
    }

    protected virtual IEnumerable FilterObjects(
      IEnumerable originalObjects,
      IModelFilter aModelFilter,
      IListFilter aListFilter)
    {
      originalObjects = originalObjects ?? (IEnumerable) new ArrayList();
      FilterEventArgs e = new FilterEventArgs(originalObjects);
      this.OnFilter(e);
      if (e.FilteredObjects != null)
        return e.FilteredObjects;
      if (aListFilter != null)
        originalObjects = aListFilter.Filter(originalObjects);
      if (aModelFilter != null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (object originalObject in originalObjects)
        {
          if (aModelFilter.Filter(originalObject))
            arrayList.Add(originalObject);
        }
        originalObjects = (IEnumerable) arrayList;
      }
      return originalObjects;
    }

    public virtual void ResetColumnFiltering()
    {
      foreach (OLVColumn column in this.Columns)
        column.ValuesChosenForFiltering.Clear();
      this.UpdateColumnFiltering();
    }

    public virtual void UpdateColumnFiltering()
    {
      if (this.AdditionalFilter == null)
      {
        this.ModelFilter = this.CreateColumnFilter();
      }
      else
      {
        IModelFilter columnFilter = this.CreateColumnFilter();
        if (columnFilter == null)
          this.ModelFilter = this.AdditionalFilter;
        else
          this.ModelFilter = (IModelFilter) new CompositeAllFilter(new List<IModelFilter>()
          {
            columnFilter,
            this.AdditionalFilter
          });
      }
    }

    protected virtual void UpdateFiltering() => this.BuildList(true);

    protected virtual CheckState GetPersistentCheckState(object model)
    {
      CheckState checkState;
      return model != null && this.CheckStateMap.TryGetValue(model, out checkState) ? checkState : CheckState.Unchecked;
    }

    protected virtual CheckState SetPersistentCheckState(object model, CheckState state)
    {
      if (model == null)
        return CheckState.Unchecked;
      this.CheckStateMap[model] = state;
      return state;
    }

    protected virtual void ClearPersistentCheckState() => this.CheckStateMap = (Dictionary<object, CheckState>) null;

    public enum CellEditActivateMode
    {
      None,
      SingleClick,
      DoubleClick,
      F2Only,
    }

    public enum ColumnSelectBehaviour
    {
      None,
      InlineMenu,
      Submenu,
      ModelDialog,
    }

    [Serializable]
    internal class ObjectListViewState
    {
      public int VersionNumber = 1;
      public int NumberOfColumns = 1;
      public View CurrentView;
      public int SortColumn = -1;
      public bool IsShowingGroups;
      public SortOrder LastSortOrder = SortOrder.None;
      public ArrayList ColumnIsVisible = new ArrayList();
      public ArrayList ColumnDisplayIndicies = new ArrayList();
      public ArrayList ColumnWidths = new ArrayList();
    }

    private class SuspendSelectionDisposable : IDisposable
    {
      private readonly ObjectListView objectListView;

      public SuspendSelectionDisposable(ObjectListView objectListView)
      {
        this.objectListView = objectListView;
        this.objectListView.SuspendSelectionEvents();
      }

      public void Dispose() => this.objectListView.ResumeSelectionEvents();
    }
  }
}
