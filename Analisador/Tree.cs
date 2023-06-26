namespace Analisador_Lexico;

public class Tree<T>
{
    public Stack<TreeNode<T>> m_Stack = new Stack<TreeNode<T>>();

    public List<TreeNode<T>> Nodes { get; } = new List<TreeNode<T>>();

    public Tree<T> Begin(T val)
    {
        if (m_Stack.Count == 0)
        {
            var node = new TreeNode<T>(val, null);
            Nodes.Add(node);
            m_Stack.Push(node);
        }
        else
        {
            var node = m_Stack.Peek().Add(val);
            m_Stack.Push(node);
        }

        return this;
    }

    public Tree<T> Add(T val)
    {
        m_Stack.Peek().Add(val);
        return this;
    }

    public Tree<T> End()
    {
        m_Stack.Pop();
        return this;
    }
}