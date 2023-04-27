using Analisador_Lexico.Domain;

namespace Analisador_Lexico;

public static class TableSymbol
{
    public static List<Symbol> Table { get;} = new();

    public static void AddSymbol(Symbol symbol)
    {
        Table.Add(symbol);
    }

    public static void ClearTable()
    {
        Table.Clear();
    }
}

