// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TreeListView
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using PS3SaveEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BrightIdeasSoftware
{
  public class TreeListView : VirtualObjectListView
  {
    private TreeListView.ParentGetterDelegate parentGetter;
    private bool hierarchicalCheckboxes;
    private bool revealAfterExpand = true;
    private TreeListView.TreeRenderer treeRenderer;
    private TreeListView.TreeFactoryDelegate treeFactory;
    private bool useWaitCursorWhenExpanding = true;
    private TreeListView.Tree treeModel;
    private bool isRecalculatingHierarchicalCheckBox;

    [Category("ObjectListView")]
    [Description("This event is triggered when a branch is about to expand.")]
    public event EventHandler<TreeBranchExpandingEventArgs> Expanding;

    [Category("ObjectListView")]
    [Description("This event is triggered when a branch is about to collapsed.")]
    public event EventHandler<TreeBranchCollapsingEventArgs> Collapsing;

    [Category("ObjectListView")]
    [Description("This event is triggered when a branch has been expanded.")]
    public event EventHandler<TreeBranchExpandedEventArgs> Expanded;

    [Category("ObjectListView")]
    [Description("This event is triggered when a branch has been collapsed.")]
    public event EventHandler<TreeBranchCollapsedEventArgs> Collapsed;

    protected virtual void OnExpanding(TreeBranchExpandingEventArgs e)
    {
      if (this.Expanding == null)
        return;
      this.Expanding((object) this, e);
    }

    protected virtual void OnCollapsing(TreeBranchCollapsingEventArgs e)
    {
      if (this.Collapsing == null)
        return;
      this.Collapsing((object) this, e);
    }

    protected virtual void OnExpanded(TreeBranchExpandedEventArgs e)
    {
      if (this.Expanded == null)
        return;
      this.Expanded((object) this, e);
    }

    protected virtual void OnCollapsed(TreeBranchCollapsedEventArgs e)
    {
      if (this.Collapsed == null)
        return;
      this.Collapsed((object) this, e);
    }

    public TreeListView()
    {
      this.OwnerDraw = true;
      this.View = View.Details;
      this.CheckedObjectsMustStillExistInList = false;
      this.RegenerateTree();
      this.TreeColumnRenderer = new TreeListView.TreeRenderer();
      this.SmallImageList = new ImageList();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual TreeListView.CanExpandGetterDelegate CanExpandGetter
    {
      get => this.TreeModel.CanExpandGetter;
      set => this.TreeModel.CanExpandGetter = value;
    }

    [Browsable(false)]
    public override bool CanShowGroups => false;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual TreeListView.ChildrenGetterDelegate ChildrenGetter
    {
      get => this.TreeModel.ChildrenGetter;
      set => this.TreeModel.ChildrenGetter = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeListView.ParentGetterDelegate ParentGetter
    {
      get => this.parentGetter;
      set => this.parentGetter = value;
    }

    public override IList CheckedObjects
    {
      get => base.CheckedObjects;
      set
      {
        ArrayList arrayList = new ArrayList((ICollection) this.CheckedObjects);
        if (value != null)
          arrayList.AddRange((ICollection) value);
        base.CheckedObjects = value;
        if (!this.HierarchicalCheckboxes)
          return;
        this.RecalculateHierarchicalCheckBoxGraph((IList) arrayList);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IEnumerable ExpandedObjects
    {
      get => (IEnumerable) this.TreeModel.mapObjectToExpanded.Keys;
      set
      {
        this.TreeModel.mapObjectToExpanded.Clear();
        if (value == null)
          return;
        foreach (object model in value)
          this.TreeModel.SetModelExpanded(model, true);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IListFilter ListFilter
    {
      get => (IListFilter) null;
      set
      {
      }
    }

    [Category("ObjectListView")]
    [Description("Show hierarchical checkboxes be enabled?")]
    [DefaultValue(false)]
    public virtual bool HierarchicalCheckboxes
    {
      get => this.hierarchicalCheckboxes;
      set
      {
        if (this.hierarchicalCheckboxes == value)
          return;
        this.hierarchicalCheckboxes = value;
        this.CheckBoxes = value;
        if (!value)
          return;
        this.TriStateCheckBoxes = false;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IEnumerable Objects
    {
      get => this.Roots;
      set => this.Roots = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IEnumerable ObjectsForClustering
    {
      get
      {
        for (int i = 0; i < this.TreeModel.GetObjectCount(); ++i)
          yield return this.TreeModel.GetNthObject(i);
      }
    }

    [Category("ObjectListView")]
    [Description("Should the parent of an expand subtree be scrolled to the top revealing the children?")]
    [DefaultValue(true)]
    public bool RevealAfterExpand
    {
      get => this.revealAfterExpand;
      set => this.revealAfterExpand = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual IEnumerable Roots
    {
      get => this.TreeModel.RootObjects;
      set
      {
        this.TreeColumnRenderer = this.TreeColumnRenderer;
        this.TreeModel.RootObjects = value ?? (IEnumerable) new ArrayList();
        this.UpdateVirtualListSize();
      }
    }

    protected virtual void EnsureTreeRendererPresent(TreeListView.TreeRenderer renderer)
    {
      if (this.Columns.Count == 0)
        return;
      foreach (OLVColumn column in this.Columns)
      {
        if (column.Renderer is TreeListView.TreeRenderer)
        {
          column.Renderer = (IRenderer) renderer;
          return;
        }
      }
      OLVColumn column1 = this.GetColumn(0);
      column1.Renderer = (IRenderer) renderer;
      column1.WordWrap = column1.WordWrap;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual TreeListView.TreeRenderer TreeColumnRenderer
    {
      get => this.treeRenderer ?? (this.treeRenderer = new TreeListView.TreeRenderer());
      set
      {
        this.treeRenderer = value ?? new TreeListView.TreeRenderer();
        this.EnsureTreeRendererPresent(this.treeRenderer);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeListView.TreeFactoryDelegate TreeFactory
    {
      get => this.treeFactory;
      set => this.treeFactory = value;
    }

    [Category("ObjectListView")]
    [Description("Should a wait cursor be shown when a branch is being expanded?")]
    [DefaultValue(true)]
    public virtual bool UseWaitCursorWhenExpanding
    {
      get => this.useWaitCursorWhenExpanding;
      set => this.useWaitCursorWhenExpanding = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeListView.Tree TreeModel
    {
      get => this.treeModel;
      protected set => this.treeModel = value;
    }

    public virtual bool IsExpanded(object model)
    {
      TreeListView.Branch branch = this.TreeModel.GetBranch(model);
      return branch != null && branch.IsExpanded;
    }

    public virtual void Collapse(object model)
    {
      if (this.GetItemCount() == 0)
        return;
      OLVListItem olvListItem = this.ModelToItem(model);
      TreeBranchCollapsingEventArgs e = new TreeBranchCollapsingEventArgs(model, olvListItem);
      this.OnCollapsing(e);
      if (e.Canceled)
        return;
      IList selectedObjects = this.SelectedObjects;
      int startIndex = this.TreeModel.Collapse(model);
      if (startIndex < 0)
        return;
      this.UpdateVirtualListSize();
      this.SelectedObjects = selectedObjects;
      if (startIndex < this.GetItemCount())
        this.RedrawItems(startIndex, this.GetItemCount() - 1, true);
      this.OnCollapsed(new TreeBranchCollapsedEventArgs(model, olvListItem));
    }

    public virtual void CollapseAll()
    {
      if (this.GetItemCount() == 0)
        return;
      TreeBranchCollapsingEventArgs e = new TreeBranchCollapsingEventArgs((object) null, (OLVListItem) null);
      this.OnCollapsing(e);
      if (e.Canceled)
        return;
      IList selectedObjects = this.SelectedObjects;
      int startIndex = this.TreeModel.CollapseAll();
      if (startIndex < 0)
        return;
      this.UpdateVirtualListSize();
      this.SelectedObjects = selectedObjects;
      if (startIndex < this.GetItemCount())
        this.RedrawItems(startIndex, this.GetItemCount() - 1, true);
      this.OnCollapsed(new TreeBranchCollapsedEventArgs((object) null, (OLVListItem) null));
    }

    public override void ClearObjects()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(((ObjectListView) this).ClearObjects));
      }
      else
      {
        this.Roots = (IEnumerable) null;
        this.DiscardAllState();
      }
    }

    public virtual void DiscardAllState()
    {
      this.CheckStateMap.Clear();
      this.RebuildAll(false);
    }

    public virtual void Expand(object model)
    {
      if (this.GetItemCount() == 0)
        return;
      OLVListItem olvListItem = this.ModelToItem(model);
      TreeBranchExpandingEventArgs e = new TreeBranchExpandingEventArgs(model, olvListItem);
      this.OnExpanding(e);
      if (e.Canceled)
        return;
      IList selectedObjects = this.SelectedObjects;
      int startIndex = this.TreeModel.Expand(model);
      if (startIndex < 0)
        return;
      this.UpdateVirtualListSize();
      using (this.SuspendSelectionEventsDuring())
        this.SelectedObjects = selectedObjects;
      this.RedrawItems(startIndex, this.GetItemCount() - 1, true);
      this.OnExpanded(new TreeBranchExpandedEventArgs(model, olvListItem));
      if (!this.RevealAfterExpand || startIndex <= 0)
        return;
      this.BeginUpdate();
      try
      {
        int countPerPage = NativeMethods.GetCountPerPage((System.Windows.Forms.ListView) this);
        int visibleDescendentCount = this.TreeModel.GetVisibleDescendentCount(model);
        if (visibleDescendentCount < countPerPage)
          this.EnsureVisible(startIndex + visibleDescendentCount);
        else
          this.TopItemIndex = startIndex;
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public virtual void ExpandAll()
    {
      if (this.GetItemCount() == 0)
        return;
      TreeBranchExpandingEventArgs e = new TreeBranchExpandingEventArgs((object) null, (OLVListItem) null);
      this.OnExpanding(e);
      if (e.Canceled)
        return;
      IList selectedObjects = this.SelectedObjects;
      int startIndex = this.TreeModel.ExpandAll();
      if (startIndex < 0)
        return;
      this.UpdateVirtualListSize();
      using (this.SuspendSelectionEventsDuring())
        this.SelectedObjects = selectedObjects;
      this.RedrawItems(startIndex, this.GetItemCount() - 1, true);
      this.OnExpanded(new TreeBranchExpandedEventArgs((object) null, (OLVListItem) null));
    }

    public virtual void RebuildAll(bool preserveState)
    {
      int num = preserveState ? this.TopItemIndex : -1;
      this.RebuildAll(preserveState ? this.SelectedObjects : (IList) null, preserveState ? this.ExpandedObjects : (IEnumerable) null, preserveState ? this.CheckedObjects : (IList) null);
      if (!preserveState)
        return;
      this.TopItemIndex = num;
    }

    protected virtual void RebuildAll(IList selected, IEnumerable expanded, IList checkedObjects)
    {
      IEnumerable roots = this.Roots;
      TreeListView.CanExpandGetterDelegate canExpandGetter = this.CanExpandGetter;
      TreeListView.ChildrenGetterDelegate childrenGetter = this.ChildrenGetter;
      try
      {
        this.BeginUpdate();
        this.RegenerateTree();
        this.CanExpandGetter = canExpandGetter;
        this.ChildrenGetter = childrenGetter;
        if (expanded != null)
          this.ExpandedObjects = expanded;
        this.Roots = roots;
        if (selected != null)
          this.SelectedObjects = selected;
        if (checkedObjects == null)
          return;
        this.CheckedObjects = checkedObjects;
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public virtual void Reveal(object modelToReveal, bool selectAfterReveal)
    {
      ArrayList arrayList = new ArrayList();
      foreach (object ancestor in this.GetAncestors(modelToReveal))
        arrayList.Add(ancestor);
      arrayList.Reverse();
      try
      {
        this.BeginUpdate();
        foreach (object model in arrayList)
          this.Expand(model);
        this.EnsureModelVisible(modelToReveal);
        if (!selectAfterReveal)
          return;
        this.SelectObject(modelToReveal, true);
      }
      finally
      {
        this.EndUpdate();
      }
    }

    public override void RefreshObjects(IList modelObjects)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.RefreshObjects(modelObjects)));
      }
      else
      {
        if (this.GetItemCount() == 0)
          return;
        IList selectedObjects = this.SelectedObjects;
        ArrayList arrayList = new ArrayList();
        Hashtable hashtable = new Hashtable();
        foreach (object modelObject in (IEnumerable) modelObjects)
        {
          if (modelObject != null)
          {
            hashtable[modelObject] = (object) true;
            object parent = this.GetParent(modelObject);
            if (parent == null)
              arrayList.Add(modelObject);
            else
              hashtable[parent] = (object) true;
          }
        }
        if (arrayList.Count > 0)
        {
          ArrayList array = ObjectListView.EnumerableToArray(this.Roots, false);
          bool flag = false;
          foreach (object obj in arrayList)
          {
            int index = array.IndexOf(obj);
            if (index >= 0 && array[index] != obj)
            {
              array[index] = obj;
              flag = true;
            }
          }
          if (flag)
            this.Roots = (IEnumerable) array;
        }
        int num = int.MaxValue;
        foreach (object key in (IEnumerable) hashtable.Keys)
        {
          if (key != null)
          {
            int val2 = this.TreeModel.RebuildChildren(key);
            if (val2 >= 0)
              num = Math.Min(num, val2);
          }
        }
        if (num >= this.GetItemCount())
          return;
        this.ClearCachedInfo();
        this.UpdateVirtualListSize();
        this.SelectedObjects = selectedObjects;
        this.RedrawItems(num, this.GetItemCount() - 1, true);
      }
    }

    protected override bool SetObjectCheckedness(object modelObject, CheckState state)
    {
      if (!base.SetObjectCheckedness(modelObject, state))
        return false;
      if (!this.HierarchicalCheckboxes)
        return true;
      CheckState? checkState = this.GetCheckState(modelObject);
      if (!checkState.HasValue || checkState.Value == CheckState.Indeterminate)
        return true;
      foreach (object modelObject1 in this.GetChildrenWithoutExpanding(modelObject))
        this.SetObjectCheckedness(modelObject1, checkState.Value);
      this.RecalculateHierarchicalCheckBoxGraph((IList) new ArrayList()
      {
        modelObject
      });
      return true;
    }

    private IEnumerable GetChildrenWithoutExpanding(object model)
    {
      TreeListView.Branch branch = this.TreeModel.GetBranch(model);
      return branch == null || !branch.CanExpand ? (IEnumerable) new ArrayList() : branch.Children;
    }

    public virtual void ToggleExpansion(object model)
    {
      if (this.IsExpanded(model))
        this.Collapse(model);
      else
        this.Expand(model);
    }

    public virtual bool CanExpand(object model)
    {
      TreeListView.Branch branch = this.TreeModel.GetBranch(model);
      return branch != null && branch.CanExpand;
    }

    public virtual object GetParent(object model)
    {
      TreeListView.Branch branch = this.TreeModel.GetBranch(model);
      return branch == null || branch.ParentBranch == null ? (object) null : branch.ParentBranch.Model;
    }

    public virtual IEnumerable GetChildren(object model)
    {
      TreeListView.Branch branch = this.TreeModel.GetBranch(model);
      if (branch == null || !branch.CanExpand)
        return (IEnumerable) new ArrayList();
      branch.FetchChildren();
      return branch.Children;
    }

    protected override bool ProcessLButtonDown(OlvListViewHitTestInfo hti)
    {
      if (hti.HitTestLocation != HitTestLocation.ExpandButton)
        return base.ProcessLButtonDown(hti);
      this.PossibleFinishCellEditing();
      this.ToggleExpansion(hti.RowObject);
      return true;
    }

    public override OLVListItem MakeListViewItem(int itemIndex)
    {
      OLVListItem olvListItem = base.MakeListViewItem(itemIndex);
      TreeListView.Branch branch = this.TreeModel.GetBranch(olvListItem.RowObject);
      if (branch != null)
        olvListItem.IndentCount = branch.Level;
      return olvListItem;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, this.Width, this.Height));
      base.OnPaint(e);
    }

    protected virtual void RegenerateTree()
    {
      this.TreeModel = this.TreeFactory == null ? new TreeListView.Tree(this) : this.TreeFactory(this);
      this.VirtualListDataSource = (IVirtualListDataSource) this.TreeModel;
    }

    protected virtual void RecalculateHierarchicalCheckBoxGraph(IList toCheck)
    {
      if (toCheck == null || toCheck.Count == 0)
        return;
      if (this.isRecalculatingHierarchicalCheckBox)
        return;
      try
      {
        this.isRecalculatingHierarchicalCheckBox = true;
        foreach (object distinctAncestor in this.CalculateDistinctAncestors(toCheck))
          this.RecalculateSingleHierarchicalCheckBox(distinctAncestor);
      }
      finally
      {
        this.isRecalculatingHierarchicalCheckBox = false;
      }
    }

    protected virtual void RecalculateSingleHierarchicalCheckBox(object modelObject)
    {
      if (modelObject == null || !this.CanExpand(modelObject))
        return;
      CheckState? nullable = new CheckState?();
      foreach (object child in this.GetChildren(modelObject))
      {
        CheckState? checkState = this.GetCheckState(child);
        if (checkState.HasValue)
        {
          if (nullable.HasValue)
          {
            if (nullable.Value != checkState.Value)
            {
              nullable = new CheckState?(CheckState.Indeterminate);
              break;
            }
          }
          else
            nullable = checkState;
        }
      }
      base.SetObjectCheckedness(modelObject, (CheckState) ((int) nullable ?? 2));
    }

    protected virtual IEnumerable CalculateDistinctAncestors(IList toCheck)
    {
      if (toCheck.Count == 1)
      {
        foreach (object ancestor in this.GetAncestors(toCheck[0]))
          yield return ancestor;
      }
      else
      {
        ArrayList allAncestors = new ArrayList();
        foreach (object child in (IEnumerable) toCheck)
        {
          foreach (object ancestor in this.GetAncestors(child))
            allAncestors.Add(ancestor);
        }
        ArrayList uniqueAncestors = new ArrayList();
        Dictionary<object, bool> alreadySeen = new Dictionary<object, bool>();
        allAncestors.Reverse();
        foreach (object ancestor in allAncestors)
        {
          if (!alreadySeen.ContainsKey(ancestor))
          {
            alreadySeen[ancestor] = true;
            uniqueAncestors.Add(ancestor);
          }
        }
        uniqueAncestors.Reverse();
        foreach (object x in uniqueAncestors)
          yield return x;
        allAncestors = (ArrayList) null;
        uniqueAncestors = (ArrayList) null;
        alreadySeen = (Dictionary<object, bool>) null;
      }
    }

    protected virtual IEnumerable GetAncestors(object model)
    {
      TreeListView.ParentGetterDelegate parentGetterDelegate = this.ParentGetter ?? new TreeListView.ParentGetterDelegate(this.GetParent);
      for (object parent = parentGetterDelegate(model); parent != null; parent = parentGetterDelegate(parent))
        yield return parent;
    }

    protected override void HandleApplicationIdle(object sender, EventArgs e)
    {
      base.HandleApplicationIdle(sender, e);
      this.Invalidate();
    }

    protected override bool IsInputKey(Keys keyData)
    {
      Keys keys = keyData & Keys.KeyCode;
      return keys == Keys.Left || keys == Keys.Right || base.IsInputKey(keyData);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (!(this.FocusedItem is OLVListItem focusedItem))
      {
        base.OnKeyDown(e);
      }
      else
      {
        object rowObject = focusedItem.RowObject;
        TreeListView.Branch branch = this.TreeModel.GetBranch(rowObject);
        switch (e.KeyCode)
        {
          case Keys.Left:
            if (branch.IsExpanded)
              this.Collapse(rowObject);
            else if (branch.ParentBranch != null && branch.ParentBranch.Model != null)
              this.SelectObject(branch.ParentBranch.Model, true);
            e.Handled = true;
            break;
          case Keys.Right:
            if (branch.IsExpanded)
            {
              List<TreeListView.Branch> filteredChildBranches = branch.FilteredChildBranches;
              if (filteredChildBranches.Count > 0)
                this.SelectObject(filteredChildBranches[0].Model, true);
            }
            else if (branch.CanExpand)
              this.Expand(rowObject);
            e.Handled = true;
            break;
        }
        base.OnKeyDown(e);
      }
    }

    public class TreeRenderer : HighlightTextRenderer
    {
      private Pen linePen;
      private bool isShowLines = false;
      public static int PIXELS_PER_LEVEL = 17;

      public TreeRenderer()
      {
        this.LinePen = new Pen(Color.FromArgb(160, 160, 160), 1f);
        this.LinePen.DashStyle = DashStyle.Solid;
      }

      private TreeListView.Branch Branch => this.TreeListView.TreeModel.GetBranch(this.RowObject);

      public Pen LinePen
      {
        get => this.linePen;
        set => this.linePen = value;
      }

      public TreeListView TreeListView => (TreeListView) this.ListView;

      public bool IsShowLines
      {
        get => this.isShowLines;
        set => this.isShowLines = value;
      }

      public override void Render(Graphics g, Rectangle r)
      {
        this.DrawBackground(g, r);
        TreeListView.Branch branch = this.Branch;
        Rectangle rectangle1 = this.ApplyCellPadding(r);
        Rectangle rectangle2 = rectangle1;
        rectangle2.Offset((branch.Level - 1) * TreeListView.TreeRenderer.PIXELS_PER_LEVEL, 0);
        rectangle2.Width = TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
        rectangle2.Height = TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
        rectangle2.Y = this.AlignVertically(rectangle1, rectangle2);
        int glyphMidVertical = rectangle2.Y + rectangle2.Height / 2;
        if (this.IsShowLines)
          this.DrawLines(g, r, this.LinePen, branch, glyphMidVertical);
        if (branch.CanExpand)
          this.DrawExpansionGlyph(g, rectangle2, branch.IsExpanded);
        int x = branch.Level * TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
        rectangle1.Offset(x, 0);
        rectangle1.Width -= x;
        this.DrawImageAndText(g, rectangle1);
      }

      protected virtual void DrawExpansionGlyph(Graphics g, Rectangle r, bool isExpanded)
      {
        if (this.UseStyles)
          this.DrawExpansionGlyphStyled(g, r, isExpanded);
        else
          this.DrawExpansionGlyphManual(g, r, isExpanded);
      }

      protected virtual bool UseStyles => false;

      protected virtual void DrawExpansionGlyphStyled(Graphics g, Rectangle r, bool isExpanded)
      {
        VisualStyleElement element = VisualStyleElement.TreeView.Glyph.Closed;
        if (isExpanded)
          element = VisualStyleElement.TreeView.Glyph.Opened;
        new VisualStyleRenderer(element).DrawBackground((IDeviceContext) g, r);
      }

      protected virtual void DrawExpansionGlyphManual(Graphics g, Rectangle r, bool isExpanded)
      {
        Brush brush = Brushes.Black;
        if (this.TreeListView.ModelToItem(this.RowObject).Selected)
          brush = Brushes.White;
        if (isExpanded)
          g.DrawString("▼", new Font("Arial", Util.ScaleSize(7f)), brush, (PointF) new Point(r.X + 1, r.Y + 1));
        else
          g.DrawString("►", new Font("Arial", Util.ScaleSize(7f)), brush, (PointF) new Point(r.X + 1, r.Y + 1));
      }

      protected virtual void DrawLines(
        Graphics g,
        Rectangle r,
        Pen p,
        TreeListView.Branch br,
        int glyphMidVertical)
      {
        Rectangle rectangle = r;
        rectangle.Width = TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
        int top = rectangle.Top;
        foreach (TreeListView.Branch ancestor in (IEnumerable<TreeListView.Branch>) br.Ancestors)
        {
          if (!ancestor.IsLastChild && !ancestor.IsOnlyBranch)
          {
            int num = rectangle.Left + rectangle.Width / 2;
            g.DrawLine(p, num, top, num, rectangle.Bottom);
          }
          rectangle.Offset(TreeListView.TreeRenderer.PIXELS_PER_LEVEL, 0);
        }
        int num1 = rectangle.Left + rectangle.Width / 2;
        g.DrawLine(p, num1, glyphMidVertical, rectangle.Right, glyphMidVertical);
        if (br.IsFirstBranch)
        {
          if (br.IsLastChild || br.IsOnlyBranch)
            return;
          g.DrawLine(p, num1, glyphMidVertical, num1, rectangle.Bottom);
        }
        else if (br.IsLastChild)
          g.DrawLine(p, num1, top, num1, glyphMidVertical);
        else
          g.DrawLine(p, num1, top, num1, rectangle.Bottom);
      }

      protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
      {
        TreeListView.Branch branch = this.Branch;
        Rectangle bounds = this.Bounds;
        if (branch.CanExpand)
        {
          bounds.Offset((branch.Level - 1) * TreeListView.TreeRenderer.PIXELS_PER_LEVEL, 0);
          bounds.Width = TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
          if (bounds.Contains(x, y))
          {
            hti.HitTestLocation = HitTestLocation.ExpandButton;
            return;
          }
        }
        bounds = this.Bounds;
        int num = branch.Level * TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
        bounds.X += num;
        bounds.Width -= num;
        if (x < bounds.Left)
          hti.HitTestLocation = HitTestLocation.Nothing;
        else
          this.StandardHitTest(g, hti, bounds, x, y);
      }

      protected override Rectangle HandleGetEditRectangle(
        Graphics g,
        Rectangle cellBounds,
        OLVListItem item,
        int subItemIndex,
        Size preferredSize)
      {
        return this.StandardGetEditRectangle(g, cellBounds, preferredSize);
      }
    }

    public delegate bool CanExpandGetterDelegate(object model);

    public delegate IEnumerable ChildrenGetterDelegate(object model);

    public delegate object ParentGetterDelegate(object model);

    public delegate TreeListView.Tree TreeFactoryDelegate(TreeListView view);

    public class Tree : IVirtualListDataSource, IFilterableDataSource
    {
      private TreeListView.CanExpandGetterDelegate canExpandGetter;
      private TreeListView.ChildrenGetterDelegate childrenGetter;
      private OLVColumn lastSortColumn;
      private SortOrder lastSortOrder;
      private readonly Dictionary<object, TreeListView.Branch> mapObjectToBranch = new Dictionary<object, TreeListView.Branch>();
      internal Dictionary<object, bool> mapObjectToExpanded = new Dictionary<object, bool>();
      private readonly Dictionary<object, int> mapObjectToIndex = new Dictionary<object, int>();
      private ArrayList objectList = new ArrayList();
      private readonly TreeListView treeView;
      private readonly TreeListView.Branch trunk;
      protected IModelFilter modelFilter;
      protected IListFilter listFilter;

      public Tree(TreeListView treeView)
      {
        this.treeView = treeView;
        this.trunk = new TreeListView.Branch((TreeListView.Branch) null, this, (object) null);
        this.trunk.IsExpanded = true;
      }

      public TreeListView.CanExpandGetterDelegate CanExpandGetter
      {
        get => this.canExpandGetter;
        set => this.canExpandGetter = value;
      }

      public TreeListView.ChildrenGetterDelegate ChildrenGetter
      {
        get => this.childrenGetter;
        set => this.childrenGetter = value;
      }

      public IEnumerable RootObjects
      {
        get => this.trunk.Children;
        set
        {
          this.trunk.Children = value;
          foreach (TreeListView.Branch childBranch in this.trunk.ChildBranches)
            childBranch.RefreshChildren();
          this.RebuildList();
        }
      }

      public TreeListView TreeView => this.treeView;

      public virtual int Collapse(object model)
      {
        TreeListView.Branch branch = this.GetBranch(model);
        if (branch == null || !branch.IsExpanded)
          return -1;
        if (!branch.Visible)
        {
          branch.Collapse();
          return -1;
        }
        int visibleDescendents = branch.NumberVisibleDescendents;
        branch.Collapse();
        int objectIndex = this.GetObjectIndex(model);
        this.objectList.RemoveRange(objectIndex + 1, visibleDescendents);
        this.RebuildObjectMap(objectIndex + 1);
        return objectIndex;
      }

      public virtual int CollapseAll()
      {
        this.trunk.CollapseAll();
        this.RebuildList();
        return 0;
      }

      public virtual int Expand(object model)
      {
        TreeListView.Branch branch = this.GetBranch(model);
        if (branch == null || !branch.CanExpand || branch.IsExpanded)
          return -1;
        branch.Expand();
        if (!branch.Visible)
          return -1;
        int objectIndex = this.GetObjectIndex(model);
        this.InsertChildren(branch, objectIndex + 1);
        return objectIndex;
      }

      public virtual int ExpandAll()
      {
        this.trunk.ExpandAll();
        this.Sort(this.lastSortColumn, this.lastSortOrder);
        return 0;
      }

      public virtual TreeListView.Branch GetBranch(object model)
      {
        if (model == null)
          return (TreeListView.Branch) null;
        TreeListView.Branch branch;
        this.mapObjectToBranch.TryGetValue(model, out branch);
        return branch;
      }

      public virtual int GetVisibleDescendentCount(object model)
      {
        TreeListView.Branch branch = this.GetBranch(model);
        return branch == null || !branch.IsExpanded ? 0 : branch.NumberVisibleDescendents;
      }

      public virtual int RebuildChildren(object model)
      {
        TreeListView.Branch branch = this.GetBranch(model);
        if (branch == null || !branch.Visible)
          return -1;
        int visibleDescendents = branch.NumberVisibleDescendents;
        int objectIndex = this.GetObjectIndex(model);
        if (visibleDescendents > 0)
          this.objectList.RemoveRange(objectIndex + 1, visibleDescendents);
        branch.RefreshChildren();
        if (branch.CanExpand && branch.IsExpanded)
          this.InsertChildren(branch, objectIndex + 1);
        return objectIndex;
      }

      internal bool IsModelExpanded(object model)
      {
        if (model == null)
          return true;
        bool flag;
        this.mapObjectToExpanded.TryGetValue(model, out flag);
        return flag;
      }

      internal void SetModelExpanded(object model, bool isExpanded)
      {
        if (model == null)
          return;
        if (isExpanded)
          this.mapObjectToExpanded[model] = true;
        else
          this.mapObjectToExpanded.Remove(model);
      }

      protected virtual void InsertChildren(TreeListView.Branch br, int index)
      {
        br.Expand();
        br.Sort(this.GetBranchComparer());
        this.objectList.InsertRange(index, (ICollection) br.Flatten());
        this.RebuildObjectMap(index);
      }

      protected virtual void RebuildList()
      {
        this.objectList = ArrayList.Adapter(this.trunk.Flatten());
        List<TreeListView.Branch> filteredChildBranches = this.trunk.FilteredChildBranches;
        if (filteredChildBranches.Count > 0)
        {
          filteredChildBranches[0].IsFirstBranch = true;
          filteredChildBranches[0].IsOnlyBranch = filteredChildBranches.Count == 1;
        }
        this.RebuildObjectMap(0);
      }

      protected virtual void RebuildObjectMap(int startIndex)
      {
        if (startIndex == 0)
          this.mapObjectToIndex.Clear();
        for (int index = startIndex; index < this.objectList.Count; ++index)
          this.mapObjectToIndex[this.objectList[index]] = index;
      }

      internal TreeListView.Branch MakeBranch(TreeListView.Branch parent, object model)
      {
        TreeListView.Branch branch = new TreeListView.Branch(parent, this, model);
        this.mapObjectToBranch[model] = branch;
        return branch;
      }

      public virtual object GetNthObject(int n) => this.objectList[n];

      public virtual int GetObjectCount() => this.trunk.NumberVisibleDescendents;

      public virtual int GetObjectIndex(object model)
      {
        int num;
        return model != null && this.mapObjectToIndex.TryGetValue(model, out num) ? num : -1;
      }

      public virtual void PrepareCache(int first, int last)
      {
      }

      public virtual int SearchText(string value, int first, int last, OLVColumn column) => AbstractVirtualListDataSource.DefaultSearchText(value, first, last, column, (IVirtualListDataSource) this);

      public virtual void Sort(OLVColumn column, SortOrder order)
      {
        this.lastSortColumn = column;
        this.lastSortOrder = order;
        foreach (TreeListView.Branch childBranch in this.trunk.ChildBranches)
          childBranch.IsFirstBranch = false;
        this.trunk.Sort(this.GetBranchComparer());
        this.RebuildList();
      }

      protected virtual TreeListView.BranchComparer GetBranchComparer() => this.lastSortColumn == null ? (TreeListView.BranchComparer) null : new TreeListView.BranchComparer((IComparer) new ModelObjectComparer(this.lastSortColumn, this.lastSortOrder, this.treeView.SecondarySortColumn ?? this.treeView.GetColumn(0), this.treeView.SecondarySortColumn == null ? this.lastSortOrder : this.treeView.SecondarySortOrder));

      public virtual void AddObjects(ICollection modelObjects)
      {
        ArrayList array = ObjectListView.EnumerableToArray(this.treeView.Roots, true);
        foreach (object modelObject in (IEnumerable) modelObjects)
          array.Add(modelObject);
        this.SetObjects((IEnumerable) array);
      }

      public virtual void RemoveObjects(ICollection modelObjects)
      {
        ArrayList arrayList = new ArrayList();
        foreach (object root in this.treeView.Roots)
          arrayList.Add(root);
        foreach (object modelObject in (IEnumerable) modelObjects)
        {
          arrayList.Remove(modelObject);
          this.mapObjectToIndex.Remove(modelObject);
        }
        this.SetObjects((IEnumerable) arrayList);
      }

      public virtual void SetObjects(IEnumerable collection) => this.treeView.Roots = collection;

      public void UpdateObject(int index, object modelObject)
      {
        ArrayList array = ObjectListView.EnumerableToArray(this.treeView.Roots, false);
        if (index < array.Count)
          array[index] = modelObject;
        this.SetObjects((IEnumerable) array);
      }

      public void ApplyFilters(IModelFilter mFilter, IListFilter lFilter)
      {
        this.modelFilter = mFilter;
        this.listFilter = lFilter;
        this.RebuildList();
      }

      internal bool IsFiltering => this.treeView.UseFiltering && (this.modelFilter != null || this.listFilter != null);

      internal bool IncludeModel(object model) => !this.treeView.UseFiltering || this.modelFilter == null || this.modelFilter.Filter(model);
    }

    public class Branch
    {
      private List<TreeListView.Branch> childBranches = new List<TreeListView.Branch>();
      private object model;
      private TreeListView.Branch parentBranch;
      private TreeListView.Tree tree;
      private bool alreadyHasChildren;
      private TreeListView.Branch.BranchFlags flags;

      public Branch(TreeListView.Branch parent, TreeListView.Tree tree, object model)
      {
        this.ParentBranch = parent;
        this.Tree = tree;
        this.Model = model;
      }

      public virtual IList<TreeListView.Branch> Ancestors
      {
        get
        {
          List<TreeListView.Branch> branchList = new List<TreeListView.Branch>();
          if (this.ParentBranch != null)
            this.ParentBranch.PushAncestors((IList<TreeListView.Branch>) branchList);
          return (IList<TreeListView.Branch>) branchList;
        }
      }

      private void PushAncestors(IList<TreeListView.Branch> list)
      {
        if (this.ParentBranch == null)
          return;
        this.ParentBranch.PushAncestors(list);
        list.Add(this);
      }

      public virtual bool CanExpand => this.Tree.CanExpandGetter != null && this.Model != null && this.Tree.CanExpandGetter(this.Model);

      public List<TreeListView.Branch> ChildBranches
      {
        get => this.childBranches;
        set => this.childBranches = value;
      }

      public virtual IEnumerable Children
      {
        get
        {
          ArrayList arrayList = new ArrayList();
          foreach (TreeListView.Branch childBranch in this.ChildBranches)
            arrayList.Add(childBranch.Model);
          return (IEnumerable) arrayList;
        }
        set
        {
          this.ChildBranches.Clear();
          TreeListView treeView = this.Tree.TreeView;
          CheckState? nullable = new CheckState?();
          if (treeView != null && treeView.HierarchicalCheckboxes)
            nullable = treeView.GetCheckState(this.Model);
          foreach (object obj in value)
          {
            this.AddChild(obj);
            if (nullable.HasValue && nullable.Value == CheckState.Checked)
              treeView.SetObjectCheckedness(obj, nullable.Value);
          }
        }
      }

      private void AddChild(object childModel)
      {
        TreeListView.Branch branch = this.Tree.GetBranch(childModel);
        if (branch == null)
        {
          branch = this.Tree.MakeBranch(this, childModel);
        }
        else
        {
          branch.ParentBranch = this;
          branch.Model = childModel;
          branch.ClearCachedInfo();
        }
        this.ChildBranches.Add(branch);
      }

      public List<TreeListView.Branch> FilteredChildBranches
      {
        get
        {
          if (!this.IsExpanded)
            return new List<TreeListView.Branch>();
          if (!this.Tree.IsFiltering)
            return this.ChildBranches;
          List<TreeListView.Branch> branchList = new List<TreeListView.Branch>();
          foreach (TreeListView.Branch childBranch in this.ChildBranches)
          {
            if (this.Tree.IncludeModel(childBranch.Model))
              branchList.Add(childBranch);
            else if (childBranch.FilteredChildBranches.Count > 0)
              branchList.Add(childBranch);
          }
          return branchList;
        }
      }

      public bool IsExpanded
      {
        get => this.Tree.IsModelExpanded(this.Model);
        set => this.Tree.SetModelExpanded(this.Model, value);
      }

      public virtual bool IsFirstBranch
      {
        get => (uint) (this.flags & TreeListView.Branch.BranchFlags.FirstBranch) > 0U;
        set
        {
          if (value)
            this.flags |= TreeListView.Branch.BranchFlags.FirstBranch;
          else
            this.flags &= ~TreeListView.Branch.BranchFlags.FirstBranch;
        }
      }

      public virtual bool IsLastChild
      {
        get => (uint) (this.flags & TreeListView.Branch.BranchFlags.LastChild) > 0U;
        set
        {
          if (value)
            this.flags |= TreeListView.Branch.BranchFlags.LastChild;
          else
            this.flags &= ~TreeListView.Branch.BranchFlags.LastChild;
        }
      }

      public virtual bool IsOnlyBranch
      {
        get => (uint) (this.flags & TreeListView.Branch.BranchFlags.OnlyBranch) > 0U;
        set
        {
          if (value)
            this.flags |= TreeListView.Branch.BranchFlags.OnlyBranch;
          else
            this.flags &= ~TreeListView.Branch.BranchFlags.OnlyBranch;
        }
      }

      public int Level => this.ParentBranch == null ? 0 : this.ParentBranch.Level + 1;

      public object Model
      {
        get => this.model;
        set => this.model = value;
      }

      public virtual int NumberVisibleDescendents
      {
        get
        {
          if (!this.IsExpanded)
            return 0;
          List<TreeListView.Branch> filteredChildBranches = this.FilteredChildBranches;
          int count = filteredChildBranches.Count;
          foreach (TreeListView.Branch branch in filteredChildBranches)
            count += branch.NumberVisibleDescendents;
          return count;
        }
      }

      public TreeListView.Branch ParentBranch
      {
        get => this.parentBranch;
        set => this.parentBranch = value;
      }

      public TreeListView.Tree Tree
      {
        get => this.tree;
        set => this.tree = value;
      }

      public virtual bool Visible => this.ParentBranch == null || this.ParentBranch.IsExpanded && this.ParentBranch.Visible;

      public virtual void ClearCachedInfo()
      {
        this.Children = (IEnumerable) new ArrayList();
        this.alreadyHasChildren = false;
      }

      public virtual void Collapse() => this.IsExpanded = false;

      public virtual void Expand()
      {
        if (!this.CanExpand)
          return;
        this.IsExpanded = true;
        this.FetchChildren();
      }

      public virtual void ExpandAll()
      {
        this.Expand();
        foreach (TreeListView.Branch childBranch in this.ChildBranches)
        {
          if (childBranch.CanExpand)
            childBranch.ExpandAll();
        }
      }

      public virtual void CollapseAll()
      {
        this.Collapse();
        foreach (TreeListView.Branch childBranch in this.ChildBranches)
        {
          if (childBranch.IsExpanded)
            childBranch.CollapseAll();
        }
      }

      public virtual void FetchChildren()
      {
        if (this.alreadyHasChildren)
          return;
        this.alreadyHasChildren = true;
        if (this.Tree.ChildrenGetter == null)
          return;
        Cursor current = Cursor.Current;
        try
        {
          if (this.Tree.TreeView.UseWaitCursorWhenExpanding)
            Cursor.Current = Cursors.WaitCursor;
          this.Children = this.Tree.ChildrenGetter(this.Model);
        }
        finally
        {
          Cursor.Current = current;
        }
      }

      public virtual IList Flatten()
      {
        ArrayList arrayList = new ArrayList();
        if (this.IsExpanded)
          this.FlattenOnto((IList) arrayList);
        return (IList) arrayList;
      }

      public virtual void FlattenOnto(IList flatList)
      {
        TreeListView.Branch branch = (TreeListView.Branch) null;
        foreach (TreeListView.Branch filteredChildBranch in this.FilteredChildBranches)
        {
          branch = filteredChildBranch;
          filteredChildBranch.IsLastChild = false;
          flatList.Add(filteredChildBranch.Model);
          if (filteredChildBranch.IsExpanded)
          {
            filteredChildBranch.FetchChildren();
            filteredChildBranch.FlattenOnto(flatList);
          }
        }
        if (branch == null)
          return;
        branch.IsLastChild = true;
      }

      public virtual void RefreshChildren()
      {
        this.ClearCachedInfo();
        if (!this.IsExpanded || !this.CanExpand)
          return;
        this.FetchChildren();
        foreach (TreeListView.Branch childBranch in this.ChildBranches)
          childBranch.RefreshChildren();
      }

      public virtual void Sort(TreeListView.BranchComparer comparer)
      {
        if (this.ChildBranches.Count == 0)
          return;
        if (comparer != null)
          this.ChildBranches.Sort((IComparer<TreeListView.Branch>) comparer);
        foreach (TreeListView.Branch childBranch in this.ChildBranches)
          childBranch.Sort(comparer);
      }

      [Flags]
      public enum BranchFlags
      {
        FirstBranch = 1,
        LastChild = 2,
        OnlyBranch = 4,
      }
    }

    public class BranchComparer : IComparer<TreeListView.Branch>
    {
      private readonly IComparer actualComparer;

      public BranchComparer(IComparer actualComparer) => this.actualComparer = actualComparer;

      public int Compare(TreeListView.Branch x, TreeListView.Branch y) => this.actualComparer.Compare(x.Model, y.Model);
    }
  }
}
