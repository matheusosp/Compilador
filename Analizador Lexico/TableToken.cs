using Analizador_Lexico.Domain;

namespace Analizador_Lexico;

public static class TableToken
{
    public static List<Token> Table { get; } = new();

    public static void AddToken(Token token)
    {
        Table.Add(token);
    }

    public static bool ContainsToken(Token token)
    {
        return Table.Any(item => item.Name.Equals(token.Name));
    }

    public static void ClearTable()
    {
        Table.Clear();
    }
}

