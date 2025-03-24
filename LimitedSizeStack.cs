using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> LimitedStack { get; set; }
    public readonly int UndoLimit;

    public LimitedSizeStack(int undoLimit)
    {
        LimitedStack = new LinkedList<T>();
        if (undoLimit < 0)
            throw new ArgumentException();
        UndoLimit = undoLimit;
    }

    public void Push(T item)
    {
        if (UndoLimit == 0)
            return;
        if (UndoLimit <= Count)
            LimitedStack.RemoveFirst();
        LimitedStack.AddLast(item);
    }

    public T Pop()
    {
        var lastItem = LimitedStack.Last.Value;
        LimitedStack.RemoveLast();
        return lastItem;
    }

    public int Count => LimitedStack.Count;
}