using Analizador_Lexico.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico;

public static class TableSymbol
{
    public static List<Symbol> Table { get; set; } = new List<Symbol>();

    public static void AddSymbol(Symbol symbol)
    {
        Table.Add(symbol);
    }

    public static void ClearTable()
    {
        Table.Clear();
    }
}

