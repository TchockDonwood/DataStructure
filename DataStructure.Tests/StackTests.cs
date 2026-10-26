namespace DataStructure.Tests;

public class StackTests
{
    // ---------- Инициализация ----------
    [Fact]
    public void NewStack_IsEmpty()
    {
        var stack = new DataStructure.Stack<int>();
        Assert.Equal(0, stack.Count);
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void NewStack_TopThrows()
    {
        var stack = new DataStructure.Stack<int>();
        Assert.Throws<InvalidOperationException>(() => stack.Top());
    }

    [Fact]
    public void NewStack_PopThrows()
    {
        var stack = new DataStructure.Stack<int>();
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    // ---------- Push ----------
    [Fact]
    public void Push_IncrementsCount_AndClearsIsEmpty()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        Assert.Equal(1, stack.Count);
        Assert.False(stack.IsEmpty());

        stack.Push(2);
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void Push_NullForReferenceType_Works()
    {
        var stack = new DataStructure.Stack<string?>();
        stack.Push(null);
        Assert.Equal(1, stack.Count);
        Assert.Null(stack.Top());
        Assert.Null(stack.Pop());
    }

    // ---------- Top ----------
    [Fact]
    public void Top_ReturnsLastPushed_AndDoesNotRemove()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.Equal(3, stack.Top());
        Assert.Equal(3, stack.Top());   // повторный вызов
        Assert.Equal(3, stack.Count);   // Top не удаляет
    }

    // ---------- Pop ----------
    [Fact]
    public void Pop_ReturnsLastPushed_AndDecrementsCount()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        stack.Push(2);

        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Count);
        Assert.Equal(1, stack.Top());
    }

    [Fact]
    public void Pop_LifoOrdering_IsRespected()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    // ---------- Много операций ----------
    [Fact]
    public void PushPop_ManyElements_MaintainsState()
    {
        var stack = new DataStructure.Stack<int>();
        for (int i = 0; i < 100; i++) stack.Push(i);
        Assert.Equal(100, stack.Count);

        for (int i = 99; i >= 0; i--) Assert.Equal(i, stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void MixedOperations_StateIsConsistent()
    {
        var stack = new DataStructure.Stack<string>();
        stack.Push("a");
        stack.Push("b");
        Assert.Equal("b", stack.Pop());
        stack.Push("c");
        Assert.Equal("c", stack.Top());
        Assert.Equal(2, stack.Count);
        Assert.Equal("c", stack.Pop());
        Assert.Equal("a", stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    // ---------- Print ----------
    [Fact]
    public void Print_EmptyStack_WritesEmptyLine()
    {
        var stack = new DataStructure.Stack<int>();
        using var writer = new StringWriter();
        var old = Console.Out;
        try
        {
            Console.SetOut(writer);
            stack.Print();
        }
        finally { Console.SetOut(old); }

        Assert.Equal(Environment.NewLine, writer.ToString());
    }

    [Fact]
    public void Print_NonEmpty_PrintsTopFirst()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        using var writer = new StringWriter();
        var old = Console.Out;
        try
        {
            Console.SetOut(writer);
            stack.Print();
        }
        finally { Console.SetOut(old); }

        // Вершина — 3, дно — 1. Фиксируем порядок, чтобы поймать регрессию.
        Assert.Equal($"3 2 1{Environment.NewLine}", writer.ToString());
    }

    [Fact]
    public void Print_DoesNotModifyStack()
    {
        var stack = new DataStructure.Stack<int>();
        stack.Push(1);
        stack.Push(2);

        var old = Console.Out;
        try { Console.SetOut(TextWriter.Null); stack.Print(); }
        finally { Console.SetOut(old); }

        Assert.Equal(2, stack.Count);
        Assert.Equal(2, stack.Top());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
    }

    [Fact]
    public void AfterPopOnEmpty_StackRemainsUsable()
    {
        var stack = new DataStructure.Stack<int>();
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.Equal(0, stack.Count);
        Assert.True(stack.IsEmpty());

        // После исключения стек должен принимать новые элементы
        stack.Push(42);
        Assert.Equal(1, stack.Count);
        Assert.Equal(42, stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void Pop_OnEmpty_ThrowsInvalidOperationException_Specifically()
    {
        var stack = new DataStructure.Stack<int>();
        var ex = Record.Exception(() => stack.Pop());
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void Count_IsAlwaysConsistent_UnderChurn()
    {
        var stack = new DataStructure.Stack<int>();
        int expected = 0;
        var rnd = new Random(12345);

        for (int step = 0; step < 10_000; step++)
        {
            if (expected == 0 || rnd.Next(2) == 0)
            {
                stack.Push(step);
                expected++;
            }
            else
            {
                stack.Pop();
                expected--;
            }
            Assert.Equal(expected, stack.Count);
            Assert.Equal(expected == 0, stack.IsEmpty());
        }
    }
}
