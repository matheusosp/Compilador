using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico.Domain;
public enum TokenType
{
    UNDEFINED = 0,
    IDENTIFICADOR = 1,
    SEPARADOR,
    OPERADOR,
    LITERAL,
    NUMERO,
    COMENTARIO,
    PALAVRA_RESERVADA,
}

