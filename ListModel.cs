using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LimitedSizeStack;

class HistoryAction<TItem>
{
    public ListModel<TItem>.Action Action { get; private set; }
    public int Index { get; private set; }
    public TItem Item { get; private set; }

    public HistoryAction(ListModel<TItem>.Action action, int index, TItem item)
    {
        Action = action;
        Index = index;
        Item = item;
    }
}

public class ListModel<TItem>
{
    public enum Action
    {
        Add,
        Remove
    }

    public List<TItem> Items { get; }
    private LimitedSizeStack<HistoryAction<TItem>> LimitedStack;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
        Items = new List<TItem>();
        LimitedStack = new LimitedSizeStack<HistoryAction<TItem>>(undoLimit);
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        LimitedStack = new LimitedSizeStack<HistoryAction<TItem>>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        LimitedStack.Push(new HistoryAction<TItem>(Action.Add, Items.Count, item));
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        LimitedStack.Push(new HistoryAction<TItem>(Action.Remove, index, Items[index]));
        Items.RemoveAt(index);
    }

    public bool CanUndo() => LimitedStack.Count > 0;

    public void Undo()
    {
        var historyAction = LimitedStack.Pop();
        var action = historyAction.Action;
        var index = historyAction.Index;
        var item = historyAction.Item;
        if (action == Action.Add)
            Items.RemoveAt(index);
        else
            Items.Insert(index, item);
    }

    public void MoveItem(int index, int step)
    {
        (Items[index], Items[index + step]) = (Items[index + step], Items[index]);
    }

}