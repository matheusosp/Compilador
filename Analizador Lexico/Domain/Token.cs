using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico.Domain;

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
