﻿namespace Analisador_Lexico.Domain;

public class Token
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public override string ToString()
    {
        if (string.IsNullOrEmpty(Name))
            return String.Format("{0} - {1}", Id, Type);
        else
            return String.Format("{0} - {1}", Id, Name);

    }
}
