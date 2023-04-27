using Analisador_Lexico.Irony.Parsing.Grammar;
using Analisador_Lexico.Irony.Parsing.Terminals;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Analisador_Lexico;

public class SimpleCGrammar : Grammar
{
    public SimpleCGrammar()
    {
        var id = new IdentifierTerminal("id");
        var text = new StringLiteral("texto", "\"", StringOptions.AllowsAllEscapes);

        var PROGRAMA = new NonTerminal("PROGRAMA");
        var SECAOFUNCOES = new NonTerminal("SECAOFUNCOES");
        var LISTAFUNCOES = new NonTerminal("LISTAFUNCOES");
        var PRINCIPAL = new NonTerminal("PRINCIPAL");
        var DECFUNCAO = new NonTerminal("DECFUNCAO");
        var TIPORETORNO = new NonTerminal("TIPORETORNO");
        var PARAMETROS = new NonTerminal("PARAMETROS");
        var TIPO = new NonTerminal("TIPO");
        var TIPOBASE = new NonTerminal("TIPOBASE");
        var DIMENSAO = new NonTerminal("DIMENSAO");
        var LISTAPARAMETROS = new NonTerminal("LISTAPARAMETROS");
        var BLOCO = new NonTerminal("BLOCO");
        var LISTAVARIAVEIS = new NonTerminal("LISTAVARIAVEIS");
        var SECAOVARIAVEIS = new NonTerminal("SECAOVARIAVEIS");
        var LISTAID = new NonTerminal("LISTAID");
        var SECAOCOMANDOS = new NonTerminal("SECAOCOMANDOS");
        var LISTACOMANDOS = new NonTerminal("LISTACOMANDOS");
        var COMANDO = new NonTerminal("COMANDO");
        var LEITURA = new NonTerminal("LEITURA");
        var LISTATERMOLEITURA = new NonTerminal("LISTATERMOLEITURA");
        var ESCRITA = new NonTerminal("ESCRITA");
        var LISTATERMOESCRITA = new NonTerminal("LISTATERMOESCRITA");
        var ATRIBUIÇAO = new NonTerminal("ATRIBUIÇAO");
        var FUNCAO = new NonTerminal("FUNCAO");
        var LISTAARGUMENTOS = new NonTerminal("LISTAARGUMENTOS");
        var SELECAO = new NonTerminal("SELECAO");
        var ENQUANTO = new NonTerminal("ENQUANTO");
        var RETORNO = new NonTerminal("RETORNO");
        var TERMOLEITURA = new NonTerminal("TERMOLEITURA");
        var DIMENSAO2 = new NonTerminal("DIMENSAO2");
        var EXPR_ADITIVA = new NonTerminal("EXPR_ADITIVA");
        var TERMOESCRITA = new NonTerminal("TERMOESCRITA");
        var CONSTANTE = new NonTerminal("CONSTANTE");
        var EXPRESSAO = new NonTerminal("EXPRESSAO");
        var SENAO = new NonTerminal("SENAO");
        var COMPLEMENTO = new NonTerminal("COMPLEMENTO");
        var ARGUMENTOS = new NonTerminal("ARGUMENTOS");
        var EXPR_OU = new NonTerminal("EXPR_OU");
        var EXPR_E = new NonTerminal("EXPR_E");
        var EXPR_RELACIONAL = new NonTerminal("EXPR_RELACIONAL");
        var EXPR_MULTIPLICATIVA = new NonTerminal("EXPR_MULTIPLICATIVA");
        var OP_ADITIVO = new NonTerminal("OP_ADITIVO");
        var FATOR = new NonTerminal("FATOR");
        var OP_MULTIPLICATIVO = new NonTerminal("OP_MULTIPLICATIVO");
        var SINAL = new NonTerminal("SINAL");
        var COMP = new NonTerminal("COMP");

        this.Root = PROGRAMA;

        PROGRAMA.Rule = SECAOFUNCOES + PRINCIPAL;
        SECAOFUNCOES.Rule = LISTAFUNCOES | Empty;
        LISTAFUNCOES.Rule = DECFUNCAO | LISTAFUNCOES + DECFUNCAO;
        DECFUNCAO.Rule = TIPORETORNO + id + "(" + PARAMETROS + ")" + BLOCO;
        TIPORETORNO.Rule = TIPO | "void";
        TIPO.Rule = TIPOBASE + DIMENSAO ;
        TIPOBASE.Rule = ToTerm("char") | "float" | "int" | "boolean";
        DIMENSAO.Rule = DIMENSAO + ToTerm("[") + "num_int" + "]" | Empty; 
        PARAMETROS.Rule = LISTAPARAMETROS | Empty;
        LISTAPARAMETROS.Rule = TIPO + id | LISTAPARAMETROS + "," + TIPO + id;
        PRINCIPAL.Rule = "main" + ToTerm("(") + ")" + BLOCO;
        BLOCO.Rule = ToTerm("{") + SECAOVARIAVEIS + SECAOCOMANDOS + "}";
        SECAOVARIAVEIS.Rule = LISTAVARIAVEIS | Empty;
        LISTAVARIAVEIS.Rule = TIPO + LISTAID + ToTerm(";") | LISTAVARIAVEIS + TIPO + LISTAID + ";";
        LISTAID.Rule = id | LISTAID + "," + id;
        SECAOCOMANDOS.Rule = LISTACOMANDOS | Empty;
        LISTACOMANDOS.Rule = COMANDO | LISTACOMANDOS + COMANDO;
        COMANDO.Rule = LEITURA | ESCRITA | ATRIBUIÇAO | FUNCAO | SELECAO | ENQUANTO | RETORNO;
        LEITURA.Rule = "scanf" + "(" + LISTATERMOLEITURA + ")" + ";";
        LISTATERMOLEITURA.Rule = TERMOLEITURA | LISTATERMOLEITURA + "," + TERMOLEITURA;
        TERMOLEITURA.Rule = id + DIMENSAO2;
        DIMENSAO2.Rule = ToTerm("[") + EXPR_ADITIVA + "]" | Empty;
        ESCRITA.Rule = "println" + ToTerm("(") + LISTATERMOESCRITA + ")" + ";";
        LISTATERMOESCRITA.Rule = TERMOESCRITA | LISTATERMOESCRITA + "," + TERMOESCRITA;
        TERMOESCRITA.Rule = id + DIMENSAO2 | CONSTANTE | text;
        SELECAO.Rule = "if" + "(" + EXPRESSAO + ")" + BLOCO + SENAO;
        SENAO.Rule = "else" + BLOCO | Empty;
        ENQUANTO.Rule = "while" + "(" + EXPRESSAO + ")" + BLOCO;
        ATRIBUIÇAO.Rule = id + "=" + COMPLEMENTO + ";";
        COMPLEMENTO.Rule = EXPRESSAO | FUNCAO;
        FUNCAO.Rule = ToTerm("func") + id + "(" + ARGUMENTOS + ")";
        ARGUMENTOS.Rule = LISTAARGUMENTOS | Empty;
        LISTAARGUMENTOS.Rule = EXPRESSAO | LISTAARGUMENTOS + "," + EXPRESSAO;
        RETORNO.Rule = "return" + EXPRESSAO + ";";
        EXPRESSAO.Rule = EXPR_OU;
        EXPR_OU.Rule = EXPR_E | EXPR_OU + ToTerm("||") + EXPR_E; 
        EXPR_E.Rule = EXPR_RELACIONAL | EXPR_E + ToTerm("&&") + EXPR_RELACIONAL;
        EXPR_RELACIONAL.Rule = EXPR_ADITIVA | EXPR_ADITIVA + COMP + EXPR_ADITIVA;
        EXPR_ADITIVA.Rule = EXPR_MULTIPLICATIVA | EXPR_ADITIVA + OP_ADITIVO + EXPR_MULTIPLICATIVA;
        OP_ADITIVO.Rule = ToTerm("+") | "-";
        EXPR_MULTIPLICATIVA.Rule = FATOR | EXPR_MULTIPLICATIVA + OP_MULTIPLICATIVO + FATOR;
        OP_MULTIPLICATIVO.Rule = ToTerm("*") | "/" | "%";
        FATOR.Rule = SINAL + id + DIMENSAO2 | SINAL + CONSTANTE | text | "!" + FATOR | "(" + EXPRESSAO + ")";
        CONSTANTE.Rule = ToTerm("num_int") | "num_dec";
        SINAL.Rule = ToTerm("+") | "-" | Empty; 
        

        COMP.Rule = ToTerm("<") | "==" | "!=" | ">" | "<=" | ">=";
        this.MarkReservedWords(
            "int", "float", "char", "boolean", "void", "if", "else", "for", "while",
            "scanf", "println", "main", "return"
            );
        
    }
}