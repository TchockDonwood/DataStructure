using System.Collections;

namespace DataStructure;

public class Stack<T> : IEnumerable<T>
{
    // список, на базе которого работает этот стек
    private readonly DataStructure.LinkedList<T> _list = new();
    
    public int Count => _list.Count;

    public Stack() { }

    // добавляет элемент в стек
    public void Push(T item)
    {
        throw new NotImplementedException();
    }

    // удаляет элемент из стека
    public T Pop()
    {
        throw new NotImplementedException();
    }

    // возвращает значение верхнего элемента стека, не удаляя его
    public T Top()
    {
        throw new NotImplementedException();
    }

    // проверяет стек на пустоту
    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    // печатает содержимое стека в консоль
    public void Print()
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
