using System.Collections;

namespace DataStructure;

public class LinkedList<T> : IEnumerable<T>
{
    // начальный элемент списка
    public Node<T>? head { get; private set; }

    //последний элеемнт списка
    private Node<T>? _tail;

    // количество элементов в списке
    public int Count { get; private set; }

    public LinkedList() { }

    // Перегрузка индексатора. Нужен, чтобы можно было сделать вызов вида: var first = list[0];
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var current = head!;
            for (int i = 0; i < index; i++)
                current = current.Next!;

            return current.Value;
        }
        set
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var current = head!;
            for (int i = 0; i < index; i++)
                current = current.Next!;

            current.Value = value;
        }
    }
    public IEnumerator<T> GetEnumerator() => new ListEnumerator<T>(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Добавляет элемент в конец списка
    public void Add(T item)
    {
        throw new NotImplementedException();
    }

    // Вставляет элементов в список по указанному индексу (не заменяя его!).
    // Проверьте, как работает этото метод у класса List<>.
    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    // Проверяет, находится ли указанный элемент в списке
    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    // Удаляет первый встречный элемент в списке, который соответсвует указанному.
    // Если такой элесент нашелся, возвращает true, иначе – false
    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    // Удаляет элемент из списка по указанному индексу
    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    //Удаляет все элементы из списка, соответсвующие указанному.
    //Возвращает число удаленных элементов
    public int RemoveAll(T item)
    {
        throw new NotImplementedException();
    }



    // Это реализация интерфейса IEnumerator. Без нее нельзя будет использовать foreach с этой коллекцией.
    private sealed class ListEnumerator<TItem> : IEnumerator<TItem>
    {
        private readonly LinkedList<TItem> _list;
        private Node<TItem>? _current;
        private bool _started;
        private bool _finished;

        public ListEnumerator(LinkedList<TItem> list) => _list = list;

        public TItem Current
        {
            get
            {
                if (!_started || _finished)
                    throw new InvalidOperationException(
                        "Enumeration has either not started or has already finished.");
                return _current!.Value;
            }
        }

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_finished) return false;
            _started = true;
            _current = _current is null ? _list.head : _current.Next;
            if (_current is null)
            {
                _finished = true;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            _current = null;
            _started = false;
            _finished = false;
        }

        public void Dispose() { }
    }
}