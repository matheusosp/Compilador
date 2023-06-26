namespace Analisador_Lexico;

public class Context
{
    public string Id { get; }
    public int Offset { get; set; }

    public Context(string id, int offset)
    {
        Id = id;
        Offset = offset;
    }
}