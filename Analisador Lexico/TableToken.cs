using Analisador_Lexico.Domain;

namespace Analisador_Lexico;

public static class TableToken
{
    public static List<Token> Table { get; } = new();

    public static void AddToken(Token token)
    {
        Table.Add(token);
    }
    
    public static void ClearTable()
    {
        Table.Clear();
    }
}

