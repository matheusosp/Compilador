using Analizador_Lexico.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico;
public static class TableErrors
{
    public static List<Error> Table { get; } = new();
    public static void AddError(Error error)
    {
        Table.Add(error);
    }
    public static void ClearTable()
    {
        Table.Clear();
    }
}
