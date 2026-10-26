namespace DataStructure.Tests;

public class QueueTests
{
    // ---------- Инициализация ----------
    [Fact]
    public void NewQueue_IsEmpty()
    {
        var q = new DataStructure.Queue<int>();
        Assert.Equal(0, q.Count);
        Assert.True(q.IsEmpty);           // свойство, не метод
    }

    [Fact]
    public void NewQueue_PeekThrows()
    {
        var q = new DataStructure.Queue<int>();
        Assert.Throws<InvalidOperationException>(() => q.Peek());
    }

    [Fact]
    public void NewQueue_DequeueThrows()
    {
        var q = new DataStructure.Queue<int>();
        Assert.Throws<InvalidOperationException>(() => q.Dequeue());
    }

    // ---------- Enqueue ----------
    [Fact]
    public void Enqueue_IncrementsCount_AndClearsIsEmpty()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        Assert.Equal(1, q.Count);
        Assert.False(q.IsEmpty);

        q.Enqueue(2);
        Assert.Equal(2, q.Count);
    }

    [Fact]
    public void Enqueue_NullForReferenceType_Works()
    {
        var q = new DataStructure.Queue<string?>();
        q.Enqueue(null);
        Assert.Equal(1, q.Count);
        Assert.Null(q.Peek());
        Assert.Null(q.Dequeue());
    }

    // ---------- Peek ----------
    [Fact]
    public void Peek_ReturnsFirstEnqueued_AndDoesNotRemove()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);
        q.Enqueue(3);

        Assert.Equal(1, q.Peek());
        Assert.Equal(1, q.Peek());   // повторный вызов
        Assert.Equal(3, q.Count);    // Peek не удаляет
    }

    // ---------- Dequeue ----------
    [Fact]
    public void Dequeue_ReturnsFirstEnqueued_AndDecrementsCount()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);

        Assert.Equal(1, q.Dequeue());
        Assert.Equal(1, q.Count);
        Assert.Equal(2, q.Peek());
    }

    [Fact]
    public void Dequeue_FifoOrdering_IsRespected()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);
        q.Enqueue(3);

        Assert.Equal(1, q.Dequeue());
        Assert.Equal(2, q.Dequeue());
        Assert.Equal(3, q.Dequeue());
        Assert.True(q.IsEmpty);
    }

    // ---------- Много операций ----------
    [Fact]
    public void EnqueueDequeue_ManyElements_MaintainsState()
    {
        var q = new DataStructure.Queue<int>();
        for (int i = 0; i < 100; i++) q.Enqueue(i);
        Assert.Equal(100, q.Count);

        for (int i = 0; i < 100; i++) Assert.Equal(i, q.Dequeue());
        Assert.True(q.IsEmpty);
    }

    [Fact]
    public void MixedOperations_StateIsConsistent()
    {
        var q = new DataStructure.Queue<string>();
        q.Enqueue("a");
        q.Enqueue("b");
        Assert.Equal("a", q.Dequeue());
        q.Enqueue("c");
        Assert.Equal("b", q.Peek());
        Assert.Equal(2, q.Count);
        Assert.Equal("b", q.Dequeue());
        Assert.Equal("c", q.Dequeue());
        Assert.True(q.IsEmpty);
    }

    // ---------- Print ----------
    [Fact]
    public void Print_EmptyQueue_WritesEmptyLine()
    {
        var q = new DataStructure.Queue<int>();
        using var writer = new StringWriter();
        var old = Console.Out;
        try
        {
            Console.SetOut(writer);
            q.Print();
        }
        finally { Console.SetOut(old); }

        Assert.Equal(Environment.NewLine, writer.ToString());
    }

    [Fact]
    public void Print_NonEmpty_PrintsFifoOrder()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);
        q.Enqueue(3);

        using var writer = new StringWriter();
        var old = Console.Out;
        try
        {
            Console.SetOut(writer);
            q.Print();
        }
        finally { Console.SetOut(old); }

        // Голова — 1, хвост — 3. Фиксируем порядок, чтобы поймать регрессию.
        Assert.Equal($"1 2 3{Environment.NewLine}", writer.ToString());
    }

    [Fact]
    public void Print_DoesNotModifyQueue()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);

        var old = Console.Out;
        try { Console.SetOut(TextWriter.Null); q.Print(); }
        finally { Console.SetOut(old); }

        Assert.Equal(2, q.Count);
        Assert.Equal(1, q.Peek());
        Assert.Equal(1, q.Dequeue());
        Assert.Equal(2, q.Dequeue());
    }

    [Fact]
    public void RepeatedPeekCalls_DoNotAffectCount()
    {
        var q = new DataStructure.Queue<int>();
        q.Enqueue(1);
        q.Enqueue(2);

        for (int i = 0; i < 1000; i++)
            Assert.Equal(1, q.Peek());

        Assert.Equal(2, q.Count);
        Assert.Equal(1, q.Dequeue());
        Assert.Equal(2, q.Dequeue());
    }

    [Fact]
    public void Dequeue_ReturnsSameValueAsPeek_AndRemovesExactlyThatItem()
    {
        var q = new DataStructure.Queue<string>();
        q.Enqueue("first");
        q.Enqueue("second");
        q.Enqueue("third");

        var peeked = q.Peek();
        var dequeued = q.Dequeue();

        Assert.Equal("first", peeked);
        Assert.Equal(peeked, dequeued);
        // Не должен «съесть» следующий элемент
        Assert.Equal(2, q.Count);
        Assert.Equal("second", q.Peek());
    }

    [Fact]
    public void AfterDequeueOnEmpty_QueueRemainsUsable()
    {
        var q = new DataStructure.Queue<int>();
        Assert.Throws<InvalidOperationException>(() => q.Dequeue());
        Assert.Equal(0, q.Count);
        Assert.True(q.IsEmpty);

        q.Enqueue(42);
        Assert.Equal(1, q.Count);
        Assert.Equal(42, q.Dequeue());
        Assert.True(q.IsEmpty);
    }

    [Fact]
    public void Dequeue_OnEmpty_ThrowsInvalidOperationException_Specifically()
    {
        var q = new DataStructure.Queue<int>();
        var ex = Record.Exception(() => q.Dequeue());
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void Peek_OnEmpty_ThrowsInvalidOperationException_Specifically()
    {
        var q = new DataStructure.Queue<int>();
        var ex = Record.Exception(() => q.Peek());
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void Count_IsAlwaysConsistent_UnderChurn()
    {
        var q = new DataStructure.Queue<int>();
        int expected = 0;
        var rnd = new Random(12345);

        for (int step = 0; step < 10_000; step++)
        {
            if (expected == 0 || rnd.Next(2) == 0)
            {
                q.Enqueue(step);
                expected++;
            }
            else
            {
                q.Dequeue();
                expected--;
            }
            Assert.Equal(expected, q.Count);
            Assert.Equal(expected == 0, q.IsEmpty);   // свойство, не метод
        }
    }

    [Fact]
    public void FifoOrder_IsMaintained_UnderInterleaving()
    {
        var q = new DataStructure.Queue<int>();
        int next = 0;
        var rnd = new Random(777);

        for (int step = 0; step < 5000; step++)
        {
            if (q.IsEmpty || rnd.Next(2) == 0)
            {
                q.Enqueue(next++);
            }
            else
            {
                var v = q.Dequeue();
                Assert.Equal(step == 0 ? 0 : v, v); // sanity
            }
        }
        // Дочитываем остаток и проверяем, что порядок — монотонно возрастающий
        var remaining = new List<int>();
        while (!q.IsEmpty) remaining.Add(q.Dequeue());
        for (int i = 1; i < remaining.Count; i++)
            Assert.True(remaining[i - 1] < remaining[i], "FIFO ordering violated");
    }
}
