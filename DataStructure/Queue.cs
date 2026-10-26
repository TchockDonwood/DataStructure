using System.Collections;

namespace DataStructure;

public class Queue<T> : IEnumerable<T>
{
    // список, на базе которого работает эта очередь
    private readonly DataStructure.LinkedList<T> _list = new();

    public int Count => _list.Count;

    // добавляет элемент в очередь
    public void Enqueue(T item)
    {
        throw new NotImplementedException();
    }

    // удаляет элемент из очереди
    public T Dequeue()
    {
        throw new NotImplementedException();
    }

    // возвращает значение верхнего элемента очереди, не удаляя его
    public T Peek()
    {
        throw new NotImplementedException();
    }

    // проверяет очередь на пустоту
    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    // печатает содержимое очереди в консоль
    public void Print()
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
