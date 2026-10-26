using System.Collections;

namespace DataStructure.Tests;

public class LinkedListTests
{
    // ---------- helpers ----------

    private static LinkedList<T> NewList<T>(params T[] items)
    {
        var list = new DataStructure.LinkedList<T>();
        foreach (var item in items)
            list.Add(item);
        return list;
    }

    private static void AssertSequence<T>(LinkedList<T> list, params T[] expected)
    {
        Assert.Equal(expected.Length, list.Count);
        Assert.Equal(expected, list.ToArray());
    }

    // ==========================================================
    //  КОНСТРУКТОР / НАЧАЛЬНОЕ СОСТОЯНИЕ
    // ==========================================================

    [Fact]
    public void New_IsEmpty()
    {
        var list = new LinkedList<int>();
        Assert.Equal(0, list.Count);
        Assert.Null(list.head);
        Assert.Empty(list);
    }

    // ==========================================================
    //  ADD
    // ==========================================================

    [Fact]
    public void Add_ToEmpty_SetsHeadAndIncrementsCount()
    {
        var list = NewList(42);
        Assert.Equal(1, list.Count);
        Assert.NotNull(list.head);
        Assert.Equal(42, list.head!.Value);
        Assert.Null(list.head.Next);
    }

    [Fact]
    public void Add_Multiple_PreservesOrder()
    {
        var list = NewList(1, 2, 3, 4, 5);
        AssertSequence(list, 1, 2, 3, 4, 5);
    }

    [Fact]
    public void Add_NullString_Allowed()
    {
        var list = NewList<string?>("a", null, "b");
        AssertSequence(list, "a", null, "b");
    }

    // ==========================================================
    //  ИНДЕКСАТОР — GET
    // ==========================================================

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 20)]
    [InlineData(2, 30)]
    [InlineData(3, 40)]
    public void Indexer_Get_ValidIndex_ReturnsValue(int index, int expected)
    {
        var list = NewList(10, 20, 30, 40);
        Assert.Equal(expected, list[index]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(4)]     // == Count
    [InlineData(5)]
    [InlineData(int.MaxValue)]
    public void Indexer_Get_OutOfRange_Throws(int index)
    {
        var list = NewList(10, 20, 30, 40);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = list[index]);
    }

    [Fact]
    public void Indexer_Get_OnEmpty_Throws()
    {
        var list = new LinkedList<int>();
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = list[0]);
    }

    // ==========================================================
    //  ИНДЕКСАТОР — SET
    // ==========================================================

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Indexer_Set_ReplacesValueWithoutChangingCount(int index)
    {
        var list = NewList(10, 20, 30);
        list[index] = 999;
        Assert.Equal(3, list.Count);
        Assert.Equal(999, list[index]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void Indexer_Set_OutOfRange_Throws(int index)
    {
        var list = NewList(10, 20, 30);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[index] = 0);
    }

    // ==========================================================
    //  INSERT
    // ==========================================================

    [Fact]
    public void Insert_AtIndexZero_IntoEmpty_CreatesHead()
    {
        var list = new LinkedList<int>();
        list.Insert(0, 5);
        AssertSequence(list, 5);
        Assert.Equal(5, list.head!.Value);
    }

    [Fact]
    public void Insert_AtIndexZero_IntoNonEmpty_Prepends()
    {
        var list = NewList(2, 3);
        list.Insert(0, 1);
        AssertSequence(list, 1, 2, 3);
        Assert.Equal(1, list.head!.Value);
    }

    [Fact]
    public void Insert_InMiddle()
    {
        var list = NewList(1, 2, 4, 5);
        list.Insert(2, 3);
        AssertSequence(list, 1, 2, 3, 4, 5);
    }

    [Fact]
    public void Insert_AtIndexEqualToCount_Appends()
    {
        var list = NewList(1, 2, 3);
        list.Insert(3, 4);
        AssertSequence(list, 1, 2, 3, 4);
    }

    [Fact]
    public void Insert_AtIndexOne_InsertsAfterHead()
    {
        var list = NewList(1, 3);
        list.Insert(1, 2);
        AssertSequence(list, 1, 2, 3);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    [InlineData(100)]
    public void Insert_OutOfRange_Throws(int index)
    {
        var list = NewList(1, 2, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(index, 0));
        AssertSequence(list, 1, 2, 3);       // state unchanged
    }

    // ==========================================================
    //  CONTAINS
    // ==========================================================

    [Fact]
    public void Contains_EmptyList_False()
    {
        Assert.False(new LinkedList<int>().Contains(1));
    }

    [Fact]
    public void Contains_FirstElement_True()
    {
        Assert.True(NewList(1, 2, 3).Contains(1));
    }

    [Fact]
    public void Contains_MiddleElement_True()
    {
        Assert.True(NewList(1, 2, 3).Contains(2));
    }

    [Fact]
    public void Contains_NotPresent_False()
    {
        Assert.False(NewList(1, 2, 3).Contains(99));
    }

    [Fact]
    public void Contains_LastElement_True()
    {
        Assert.True(NewList(1, 2, 3).Contains(3));
    }

    [Fact]
    public void Contains_SingleElement_True()
    {
        Assert.True(NewList(42).Contains(42));
    }

    [Fact]
    public void Contains_NullString_True()
    {
        var list = NewList<string?>(null, "a");
        Assert.True(list.Contains(null!));
    }

    // ==========================================================
    //  REMOVE
    // ==========================================================
    [Fact]
    public void Remove_OnlyElement_ThenAdd_WorksFromEmpty()
    {
        var list = NewList(7);
        Assert.True(list.Remove(7));
        Assert.Equal(0, list.Count);

        list.Add(42);
        AssertSequence(list, 42);              // ← упадёт, если _tail висячий
    }

    [Fact]
    public void Remove_Tail_ThenAdd_Appends()
    {
        var list = NewList(1, 2, 3);
        Assert.True(list.Remove(3));
        list.Add(99);
        AssertSequence(list, 1, 2, 99);
    }

    [Fact]
    public void RemoveAll_Everything_ThenAdd_WorksFromEmpty()
    {
        var list = NewList(1, 1, 1);
        Assert.Equal(3, list.RemoveAll(1));
        list.Add(42);
        AssertSequence(list, 42);
    }

    [Fact]
    public void RemoveAll_Tail_ThenAdd_Appends()
    {
        var list = NewList(1, 2, 3, 3);
        Assert.Equal(2, list.RemoveAll(3));
        list.Add(99);
        AssertSequence(list, 1, 2, 99);
    }

    [Fact]
    public void Remove_EmptyList_ReturnsFalse()
    {
        var list = new LinkedList<int>();
        Assert.False(list.Remove(1));
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Remove_NotPresent_ReturnsFalse_KeepsCount()
    {
        var list = NewList(1, 2, 3);
        Assert.False(list.Remove(99));
        AssertSequence(list, 1, 2, 3);
    }

    [Fact]
    public void Remove_Head_ReturnsTrue_UpdatesHead()
    {
        var list = NewList(1, 2, 3);
        Assert.True(list.Remove(1));
        AssertSequence(list, 2, 3);
        Assert.Equal(2, list.head!.Value);
    }

    [Fact]
    public void Remove_Middle_ReturnsTrue()
    {
        var list = NewList(1, 2, 3);
        Assert.True(list.Remove(2));
        AssertSequence(list, 1, 3);
    }

    [Fact]
    public void Remove_Tail_ReturnsTrue()
    {
        var list = NewList(1, 2, 3);
        Assert.True(list.Remove(3));
        AssertSequence(list, 1, 2);
    }

    [Fact]
    public void Remove_OnlyElement_BecomesEmpty()
    {
        var list = NewList(7);
        Assert.True(list.Remove(7));
        Assert.Equal(0, list.Count);
        Assert.Null(list.head);
        Assert.Empty(list);
    }

    [Fact]
    public void Remove_FirstOfDuplicates_RemovesOnlyOne()
    {
        var list = NewList(1, 1, 1);
        Assert.True(list.Remove(1));
        AssertSequence(list, 1, 1);
    }

    [Fact]
    public void Remove_NullHeadValue_True()
    {
        var list = NewList<string?>(null, "a");
        Assert.True(list.Remove(null!));
        AssertSequence(list, "a");
    }

    // ==========================================================
    //  REMOVEAT
    // ==========================================================

    [Fact]
    public void RemoveAt_Head()
    {
        var list = NewList(1, 2, 3);
        list.RemoveAt(0);
        AssertSequence(list, 2, 3);
    }

    [Fact]
    public void RemoveAt_Middle()
    {
        var list = NewList(1, 2, 3);
        list.RemoveAt(1);
        AssertSequence(list, 1, 3);
    }

    [Fact]
    public void RemoveAt_Tail()
    {
        var list = NewList(1, 2, 3);
        list.RemoveAt(2);
        AssertSequence(list, 1, 2);
    }

    [Fact]
    public void RemoveAt_OnlyElement()
    {
        var list = NewList(7);
        list.RemoveAt(0);
        Assert.Equal(0, list.Count);
        Assert.Null(list.head);
        Assert.Empty(list);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]       // == Count
    [InlineData(100)]
    public void RemoveAt_OutOfRange_Throws(int index)
    {
        var list = NewList(1, 2, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(index));
        AssertSequence(list, 1, 2, 3);
    }

    [Fact]
    public void RemoveAt_EmptyList_Throws()
    {
        var list = new LinkedList<int>();
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(0));
    }

    // ==========================================================
    //  REMOVEALL
    // ==========================================================

    [Fact]
    public void RemoveAll_EmptyList_ReturnsZero()
    {
        var list = new LinkedList<int>();
        Assert.Equal(0, list.RemoveAll(1));
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void RemoveAll_None_ReturnsZero()
    {
        var list = NewList(1, 2, 3);
        Assert.Equal(0, list.RemoveAll(99));
        AssertSequence(list, 1, 2, 3);
    }

    [Fact]
    public void RemoveAll_All_EmptiesList()
    {
        var list = NewList(1, 1, 1);
        Assert.Equal(3, list.RemoveAll(1));
        Assert.Equal(0, list.Count);
        Assert.Null(list.head);
        Assert.Empty(list);
    }

    [Fact]
    public void RemoveAll_HeadOnly()
    {
        var list = NewList(1, 2, 3);
        Assert.Equal(1, list.RemoveAll(1));
        AssertSequence(list, 2, 3);
    }

    [Fact]
    public void RemoveAll_MiddleOnly()
    {
        var list = NewList(1, 2, 2, 3);
        Assert.Equal(2, list.RemoveAll(2));
        AssertSequence(list, 1, 3);
    }

    [Fact]
    public void RemoveAll_TailOnly()
    {
        var list = NewList(1, 2, 3, 3);
        Assert.Equal(2, list.RemoveAll(3));
        AssertSequence(list, 1, 2);
    }

    [Fact]
    public void RemoveAll_Mixed()
    {
        var list = NewList(1, 1, 2, 1, 2, 1);
        Assert.Equal(4, list.RemoveAll(1));
        AssertSequence(list, 2, 2);
    }

    [Fact]
    public void RemoveAll_Empty_NoSideEffects()
    {
        var list = new LinkedList<int>();
        Assert.Equal(0, list.RemoveAll(0));
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void RemoveAll_NullStrings()
    {
        var list = NewList<string?>(null, "a", null, "b", null);
        Assert.Equal(3, list.RemoveAll(null!));
        AssertSequence(list, "a", "b");
    }

    // ==========================================================
    //  ENUMERATION
    // ==========================================================

    [Fact]
    public void Foreach_Empty_YieldsNothing()
    {
        var list = new LinkedList<int>();
        var seen = list.ToList();
        Assert.Empty(seen);
    }

    [Fact]
    public void Foreach_PreservesOrder()
    {
        var list = NewList(1, 2, 3, 4, 5);
        var seen = new List<int>();
        foreach (var x in list) seen.Add(x);
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, seen);
    }

    [Fact]
    public void Linq_ToArray_Works()
    {
        var list = NewList(1, 2, 3);
        Assert.Equal(new[] { 1, 2, 3 }, list.ToArray());
    }

    [Fact]
    public void Linq_Filter_Works()
    {
        var list = NewList(1, 2, 3, 4, 5, 6);
        Assert.Equal(new[] { 2, 4, 6 }, list.Where(x => x % 2 == 0).ToArray());
    }

    [Fact]
    public void NonGenericIEnumerable_Works()
    {
        var list = NewList(1, 2, 3);
        IEnumerable nonGeneric = list;

        var seen = new List<object?>();
        foreach (var x in nonGeneric) seen.Add(x);

        Assert.Equal(new object[] { 1, 2, 3 }, seen);
    }

    [Fact]
    public void Enumerator_Reset_AllowsReiteration()
    {
        var list = NewList(1, 2, 3);
        using var e = list.GetEnumerator();

        var first = new List<int>();
        while (e.MoveNext()) first.Add(e.Current);

        e.Reset();

        var second = new List<int>();
        while (e.MoveNext()) second.Add(e.Current);

        Assert.Equal(new[] { 1, 2, 3 }, first);
        Assert.Equal(first, second);
    }

    [Fact]
    public void Enumerator_Reset_ThenCurrentThrows()
    {
        var list = NewList(1, 2, 3);
        using var e = list.GetEnumerator();

        e.MoveNext();
        e.Reset();

        // после Reset и до MoveNext — «not started»
        Assert.Throws<InvalidOperationException>(() => _ = e.Current);
    }

    [Fact]
    public void Enumerator_Reset_OnEmpty_ThenMoveNextReturnsFalse()
    {
        var list = new LinkedList<int>();
        using var e = list.GetEnumerator();

        e.Reset();
        Assert.False(e.MoveNext());
    }

    [Fact]
    public void Enumerator_CurrentBeforeMoveNext_Throws()
    {
        var list = NewList(1, 2, 3);
        using var e = list.GetEnumerator();
        Assert.Throws<InvalidOperationException>(() => _ = e.Current);
    }

    [Fact]
    public void Enumerator_AfterEnd_CurrentThrows()
    {
        var list = NewList(1);
        using var e = list.GetEnumerator();
        while (e.MoveNext()) { }
        Assert.Throws<InvalidOperationException>(() => _ = e.Current);
    }

    // ==========================================================
    //  ЦЕЛОСТНОСТЬ COUNT (интеграция)
    // ==========================================================

    [Fact]
    public void Count_IsConsistent_AfterManyOperations()
    {
        var list = new LinkedList<int>();

        for (int i = 0; i < 100; i++) list.Add(i);
        Assert.Equal(100, list.Count);

        list.Insert(50, -1);
        Assert.Equal(101, list.Count);

        list.Remove(0);
        Assert.Equal(100, list.Count);

        list.RemoveAt(99);
        Assert.Equal(99, list.Count);

        list.Remove(999);                // не найден
        Assert.Equal(99, list.Count);

        var removed = list.RemoveAll(50);
        Assert.Equal(99 - removed, list.Count);
    }

    [Fact]
    public void Stress_AddGetRemove_1000Elements()
    {
        const int n = 1000;
        var list = new LinkedList<int>();

        for (int i = 0; i < n; i++)
        {
            list.Add(i);
        }
        Assert.Equal(n, list.Count);

        for (int i = 0; i < n; i++)
        {
            Assert.Equal(i, list[i]);
        }

        Assert.Equal(0, list.RemoveAll(-1));
        Assert.Equal(n, list.Count);

        while (list.Count > 0)
        {
            list.RemoveAt(0);
        }
        Assert.Empty(list);
        Assert.Null(list.head);
    }
}