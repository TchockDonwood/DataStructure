namespace DataStructure;

// узел для односвязного списка
public class Node<T>
{
    // значение элемента
    public T Value { get; set; }

    // ссылка на следующий элемент списка
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}
