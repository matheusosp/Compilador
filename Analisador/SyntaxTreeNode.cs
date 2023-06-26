using Analisador_Lexico.Domain;

namespace Analisador_Lexico;

public class SyntaxTreeNode
{
    public string Symbol { get; }
    public Token? Token { get; }
    public SyntaxTreeNode Parent { get; set; }
    public List<SyntaxTreeNode> Children { get; }
    public Context? Context { get; set; }
    public TypeInfo TypeInfo { get; set; }

    public SyntaxTreeNode(string symbol, Token? token, SyntaxTreeNode parent = null, Context? context = null)
    {
        Symbol = symbol;
        Token = token;
        Parent = parent;
        Children = new List<SyntaxTreeNode>();
        Context = context;
    }

    public void AddChild(SyntaxTreeNode child)
    {
        Children.Add(child);
        child.Parent = this;
    }
}