namespace Analisador_Lexico.Domain;

public class Symbol
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return string.Format("{0}", Name);
    }
}
