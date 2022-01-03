// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.CellEditKeyEngine
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
  public class CellEditKeyEngine
  {
    private ObjectListView listView;
    private IDictionary<Keys, CellEditCharacterBehaviour> cellEditKeyMap;
    private IDictionary<Keys, CellEditAtEdgeBehaviour> cellEditKeyAtEdgeBehaviourMap;

    public virtual void SetKeyBehaviour(
      Keys key,
      CellEditCharacterBehaviour normalBehaviour,
      CellEditAtEdgeBehaviour atEdgeBehaviour)
    {
      this.CellEditKeyMap[key] = normalBehaviour;
      this.CellEditKeyAtEdgeBehaviourMap[key] = atEdgeBehaviour;
    }

    public virtual bool HandleKey(ObjectListView olv, Keys keyData)
    {
      if (olv == null)
        throw new ArgumentNullException(nameof (olv));
      CellEditCharacterBehaviour behaviour;
      if (!this.CellEditKeyMap.TryGetValue(keyData, out behaviour))
        return false;
      this.ListView = olv;
      switch (behaviour)
      {
        case CellEditCharacterBehaviour.Ignore:
          return true;
        case CellEditCharacterBehaviour.ChangeColumnLeft:
        case CellEditCharacterBehaviour.ChangeColumnRight:
          this.HandleColumnChange(keyData, behaviour);
          goto case CellEditCharacterBehaviour.Ignore;
        case CellEditCharacterBehaviour.ChangeRowUp:
        case CellEditCharacterBehaviour.ChangeRowDown:
          this.HandleRowChange(keyData, behaviour);
          goto case CellEditCharacterBehaviour.Ignore;
        case CellEditCharacterBehaviour.CancelEdit:
          this.HandleCancelEdit();
          goto case CellEditCharacterBehaviour.Ignore;
        case CellEditCharacterBehaviour.EndEdit:
          this.HandleEndEdit();
          goto case CellEditCharacterBehaviour.Ignore;
        default:
          return this.HandleCustomVerb(keyData, behaviour);
      }
    }

    protected ObjectListView ListView
    {
      get => this.listView;
      set => this.listView = value;
    }

    protected OLVListItem ItemBeingEdited => this.ListView.CellEditEventArgs.ListViewItem;

    protected int SubItemIndexBeingEdited => this.ListView.CellEditEventArgs.SubItemIndex;

    protected IDictionary<Keys, CellEditCharacterBehaviour> CellEditKeyMap
    {
      get
      {
        if (this.cellEditKeyMap == null)
          this.InitializeCellEditKeyMaps();
        return this.cellEditKeyMap;
      }
      set => this.cellEditKeyMap = value;
    }

    protected IDictionary<Keys, CellEditAtEdgeBehaviour> CellEditKeyAtEdgeBehaviourMap
    {
      get
      {
        if (this.cellEditKeyAtEdgeBehaviourMap == null)
          this.InitializeCellEditKeyMaps();
        return this.cellEditKeyAtEdgeBehaviourMap;
      }
      set => this.cellEditKeyAtEdgeBehaviourMap = value;
    }

    protected virtual void InitializeCellEditKeyMaps()
    {
      this.cellEditKeyMap = (IDictionary<Keys, CellEditCharacterBehaviour>) new Dictionary<Keys, CellEditCharacterBehaviour>();
      this.cellEditKeyMap[Keys.Escape] = CellEditCharacterBehaviour.CancelEdit;
      this.cellEditKeyMap[Keys.Return] = CellEditCharacterBehaviour.EndEdit;
      this.cellEditKeyMap[Keys.Return] = CellEditCharacterBehaviour.EndEdit;
      this.cellEditKeyMap[Keys.Tab] = CellEditCharacterBehaviour.ChangeColumnRight;
      this.cellEditKeyMap[Keys.Tab | Keys.Shift] = CellEditCharacterBehaviour.ChangeColumnLeft;
      this.cellEditKeyMap[Keys.Left | Keys.Alt] = CellEditCharacterBehaviour.ChangeColumnLeft;
      this.cellEditKeyMap[Keys.Right | Keys.Alt] = CellEditCharacterBehaviour.ChangeColumnRight;
      this.cellEditKeyMap[Keys.Up | Keys.Alt] = CellEditCharacterBehaviour.ChangeRowUp;
      this.cellEditKeyMap[Keys.Down | Keys.Alt] = CellEditCharacterBehaviour.ChangeRowDown;
      this.cellEditKeyAtEdgeBehaviourMap = (IDictionary<Keys, CellEditAtEdgeBehaviour>) new Dictionary<Keys, CellEditAtEdgeBehaviour>();
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Tab] = CellEditAtEdgeBehaviour.Wrap;
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Tab | Keys.Shift] = CellEditAtEdgeBehaviour.Wrap;
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Left | Keys.Alt] = CellEditAtEdgeBehaviour.Wrap;
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Right | Keys.Alt] = CellEditAtEdgeBehaviour.Wrap;
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Up | Keys.Alt] = CellEditAtEdgeBehaviour.ChangeColumn;
      this.cellEditKeyAtEdgeBehaviourMap[Keys.Down | Keys.Alt] = CellEditAtEdgeBehaviour.ChangeColumn;
    }

    protected virtual void HandleEndEdit() => this.ListView.PossibleFinishCellEditing();

    protected virtual void HandleCancelEdit() => this.ListView.CancelCellEdit();

    protected virtual bool HandleCustomVerb(Keys keyData, CellEditCharacterBehaviour behaviour) => false;

    protected virtual void HandleRowChange(Keys keyData, CellEditCharacterBehaviour behaviour)
    {
      if (!this.ListView.PossibleFinishCellEditing())
        return;
      OLVListItem itemBeingEdited = this.ItemBeingEdited;
      int indexBeingEdited = this.SubItemIndexBeingEdited;
      bool up = behaviour == CellEditCharacterBehaviour.ChangeRowUp;
      OLVListItem adjacentItemOrNull = this.GetAdjacentItemOrNull(itemBeingEdited, up);
      if (adjacentItemOrNull != null)
      {
        this.StartCellEditIfDifferent(adjacentItemOrNull, indexBeingEdited);
      }
      else
      {
        CellEditAtEdgeBehaviour editAtEdgeBehaviour;
        if (!this.CellEditKeyAtEdgeBehaviourMap.TryGetValue(keyData, out editAtEdgeBehaviour))
          editAtEdgeBehaviour = CellEditAtEdgeBehaviour.Wrap;
        switch (editAtEdgeBehaviour)
        {
          case CellEditAtEdgeBehaviour.Wrap:
            this.StartCellEditIfDifferent(this.GetAdjacentItemOrNull((OLVListItem) null, up), indexBeingEdited);
            break;
          case CellEditAtEdgeBehaviour.ChangeColumn:
            List<OLVColumn> columnsInDisplayOrder = this.EditableColumnsInDisplayOrder;
            int num = Math.Max(0, columnsInDisplayOrder.IndexOf(this.ListView.GetColumn(indexBeingEdited)));
            int index1 = !up ? (num + 1) % columnsInDisplayOrder.Count : (columnsInDisplayOrder.Count + num - 1) % columnsInDisplayOrder.Count;
            int index2 = columnsInDisplayOrder[index1].Index;
            this.StartCellEditIfDifferent(this.GetAdjacentItemOrNull((OLVListItem) null, up), index2);
            break;
          case CellEditAtEdgeBehaviour.EndEdit:
            this.ListView.PossibleFinishCellEditing();
            break;
        }
      }
    }

    protected virtual void HandleColumnChange(Keys keyData, CellEditCharacterBehaviour behaviour)
    {
      if (!this.ListView.PossibleFinishCellEditing() || this.ListView.View != View.Details)
        return;
      List<OLVColumn> columnsInDisplayOrder = this.EditableColumnsInDisplayOrder;
      OLVListItem olvi = this.ItemBeingEdited;
      int index1 = Math.Max(0, columnsInDisplayOrder.IndexOf(this.ListView.GetColumn(this.SubItemIndexBeingEdited)));
      bool flag = behaviour == CellEditCharacterBehaviour.ChangeColumnLeft;
      if (flag && index1 == 0 || !flag && index1 == columnsInDisplayOrder.Count - 1)
      {
        CellEditAtEdgeBehaviour editAtEdgeBehaviour;
        if (!this.CellEditKeyAtEdgeBehaviourMap.TryGetValue(keyData, out editAtEdgeBehaviour))
          editAtEdgeBehaviour = CellEditAtEdgeBehaviour.Wrap;
        switch (editAtEdgeBehaviour)
        {
          case CellEditAtEdgeBehaviour.Ignore:
            return;
          case CellEditAtEdgeBehaviour.Wrap:
          case CellEditAtEdgeBehaviour.ChangeRow:
            if (editAtEdgeBehaviour == CellEditAtEdgeBehaviour.ChangeRow)
              olvi = this.GetAdjacentItem(olvi, flag && index1 == 0);
            index1 = !flag ? 0 : columnsInDisplayOrder.Count - 1;
            break;
          case CellEditAtEdgeBehaviour.EndEdit:
            this.HandleEndEdit();
            return;
        }
      }
      else if (flag)
        --index1;
      else
        ++index1;
      int index2 = columnsInDisplayOrder[index1].Index;
      this.StartCellEditIfDifferent(olvi, index2);
    }

    protected void StartCellEditIfDifferent(OLVListItem olvi, int subItemIndex)
    {
      if (this.ItemBeingEdited == olvi && this.SubItemIndexBeingEdited == subItemIndex)
        return;
      this.ListView.EnsureVisible(olvi.Index);
      this.ListView.StartCellEdit(olvi, subItemIndex);
    }

    protected OLVListItem GetAdjacentItemOrNull(OLVListItem olvi, bool up)
    {
      OLVListItem itemToFind = up ? this.ListView.GetPreviousItem(olvi) : this.ListView.GetNextItem(olvi);
      while (itemToFind != null && !itemToFind.Enabled)
        itemToFind = up ? this.ListView.GetPreviousItem(itemToFind) : this.ListView.GetNextItem(itemToFind);
      return itemToFind;
    }

    protected OLVListItem GetAdjacentItem(OLVListItem olvi, bool up) => this.GetAdjacentItemOrNull(olvi, up) ?? this.GetAdjacentItemOrNull((OLVListItem) null, up);

    protected List<OLVColumn> EditableColumnsInDisplayOrder
    {
      get
      {
        List<OLVColumn> olvColumnList = new List<OLVColumn>();
        foreach (OLVColumn olvColumn in this.ListView.ColumnsInDisplayOrder)
        {
          if (olvColumn.IsEditable)
            olvColumnList.Add(olvColumn);
        }
        return olvColumnList;
      }
    }
  }
}
