namespace Analisador_Lexico;

public class SemanticEventArgs : EventArgs
{
    public string[] Valores { get; }

    public SemanticEventArgs(string[] valores)
    {
        Valores = valores;
    }
}