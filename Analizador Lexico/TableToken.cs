using Analizador_Lexico.Domain;

namespace Analizador_Lexico;

public static class TableToken
{
    public static List<Token> Table { get; set; } = new List<Token>();

    public static void AddToken(Token token)
    {
        Table.Add(token);
    }

    public static bool ContainsToken(Token token)
    {
        foreach (var item in Table)
        {
            if (item.Name.Equals(token.Name))
                return true;
        }
        return false;
    }

    public static void ClearTable()
    {
        Table.Clear();
    }
}

