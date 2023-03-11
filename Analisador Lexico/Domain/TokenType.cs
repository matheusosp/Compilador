namespace Analisador_Lexico.Domain;
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

