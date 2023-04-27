using Analisador_Lexico.Domain;

namespace Analisador_Lexico;
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
