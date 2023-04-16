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
        var number = new NumberLiteral("number");
        var comma = ToTerm(",");

        //RegisterOperators(1, "||");
        //RegisterOperators(2, "&&");
        //RegisterOperators(9, "+", "-");
        //RegisterOperators(10, "*", "/", "%");
        //this.MarkPunctuation(";", ",", "(", ")", "{", "}", "[", "]");

        var PROGRAMA = new NonTerminal("PROGRAMA");
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
        var COMANDOS = new NonTerminal("COMANDOS");
        var LISTAID = new NonTerminal("LISTAID");
        var COMANDO = new NonTerminal("COMANDO");
        var LEITURA = new NonTerminal("LEITURA");
        var ESCRITA = new NonTerminal("ESCRITA");
        var ATRIBUIÇAO = new NonTerminal("ATRIBUIÇAO");
        var FUNCAO = new NonTerminal("FUNCAO");
        var SELECAO = new NonTerminal("SELECAO");
        var ENQUANTO = new NonTerminal("ENQUANTO");
        var RETORNO = new NonTerminal("RETORNO");
        var TERMOLEITURA = new NonTerminal("TERMOLEITURA");
        var NOVOTERMOLEITURA = new NonTerminal("NOVOTERMOLEITURA");
        var DIMENSAO2 = new NonTerminal("DIMENSAO2");
        var EXPR_ADITIVA = new NonTerminal("EXPR_ADITIVA");
        var TERMOESCRITA = new NonTerminal("TERMOESCRITA");
        var NOVOTERMOESCRITA = new NonTerminal("NOVOTERMOESCRITA");
        var CONSTANTE = new NonTerminal("CONSTANTE");
        var EXPRESSAO = new NonTerminal("EXPRESSAO");
        var SENAO = new NonTerminal("SENAO");
        var COMPLEMENTO = new NonTerminal("COMPLEMENTO");
        var ARGUMENTOS = new NonTerminal("ARGUMENTOS");
        var NOVO_ARGUMENTO = new NonTerminal("NOVO_ARGUMENTO");
        var EXPR_OU = new NonTerminal("EXPR_OU");
        var EXPR_E = new NonTerminal("EXPR_E");
        var EXPR_OU2 = new NonTerminal("EXPR_OU2");
        var EXPR_RELACIONAL = new NonTerminal("EXPR_RELACIONAL");
        var EXPR_E2 = new NonTerminal("EXPR_E2");
        var EXPR_RELACIONAL2 = new NonTerminal("EXPR_RELACIONAL2");
        var EXPR_MULTIPLICATIVA = new NonTerminal("EXPR_MULTIPLICATIVA");
        var EXPR_ADITIVA2 = new NonTerminal("EXPR_ADITIVA2");
        var OP_ADITIVO = new NonTerminal("OP_ADITIVO");
        var FATOR = new NonTerminal("FATOR");
        var EXPR_MULTIPLICATIVA2 = new NonTerminal("EXPR_MULTIPLICATIVA2");
        var OP_MULTIPLICATIVO = new NonTerminal("OP_MULTIPLICATIVO");
        var SINAL = new NonTerminal("SINAL");
        var COMP = new NonTerminal("COMP");

        this.Root = PROGRAMA;

        PROGRAMA.Rule = LISTAFUNCOES | PRINCIPAL;
        LISTAFUNCOES.Rule = DECFUNCAO + LISTAFUNCOES | Empty;
        DECFUNCAO.Rule = TIPORETORNO + id + "(" + PARAMETROS + ")" + BLOCO;
        TIPORETORNO.Rule = TIPO | "void";
        TIPO.Rule = TIPOBASE + DIMENSAO ;
        TIPOBASE.Rule = ToTerm("char") | "float" | "int" | "boolean";
        DIMENSAO.Rule = ToTerm("[") + number + "]" + DIMENSAO | Empty; 
        PARAMETROS.Rule = TIPO + id + LISTAPARAMETROS | Empty;
        LISTAPARAMETROS.Rule = ToTerm(",") + TIPO + id + LISTAPARAMETROS | Empty;
        PRINCIPAL.Rule = TIPORETORNO + "main" + "(" + ")" + BLOCO;
        BLOCO.Rule = ToTerm("{") + LISTAVARIAVEIS + COMANDOS + "}";
        LISTAVARIAVEIS.Rule = TIPO + id + LISTAID + ToTerm(";") + LISTAVARIAVEIS | Empty;
        LISTAID.Rule = ToTerm(",") + id + LISTAID | Empty;
        COMANDOS.Rule = COMANDO + COMANDOS | Empty;
        COMANDO.Rule = LEITURA | ESCRITA | ATRIBUIÇAO | FUNCAO | SELECAO | ENQUANTO | RETORNO;
        LEITURA.Rule = "scanf" + "(" + TERMOLEITURA + NOVOTERMOLEITURA + ")" + ";";
        TERMOLEITURA.Rule = id + DIMENSAO2;
        NOVOTERMOLEITURA.Rule = ToTerm(",") + TERMOLEITURA + NOVOTERMOLEITURA | Empty;
        DIMENSAO2.Rule = ToTerm("[") + EXPR_ADITIVA + "]" + DIMENSAO2 | Empty;
        ESCRITA.Rule = "println" + ToTerm("(") + TERMOESCRITA + NOVOTERMOESCRITA + ")" + ";";
        TERMOESCRITA.Rule = id + DIMENSAO2 | CONSTANTE;
        NOVOTERMOESCRITA.Rule = ToTerm(",") + TERMOESCRITA + NOVOTERMOESCRITA | Empty;
        SELECAO.Rule = "if" + "(" + EXPRESSAO + ")" + BLOCO + SENAO;
        SENAO.Rule = "else" + BLOCO | Empty;
        ENQUANTO.Rule = "while" + "(" + EXPRESSAO + ")" + BLOCO;
        ATRIBUIÇAO.Rule = id + "=" + COMPLEMENTO + ";";
        COMPLEMENTO.Rule = FUNCAO | EXPRESSAO;
        FUNCAO.Rule = ToTerm("func") + id + "(" + ARGUMENTOS + ")";
        ARGUMENTOS.Rule = EXPRESSAO + NOVO_ARGUMENTO | Empty;
        NOVO_ARGUMENTO.Rule = ToTerm(",") + EXPRESSAO + NOVO_ARGUMENTO | Empty;
        RETORNO.Rule = "return" + EXPRESSAO + ";";
        EXPRESSAO.Rule = EXPR_OU;
        EXPR_OU.Rule = EXPR_E + EXPR_OU2;
        EXPR_OU2.Rule = ToTerm("||") + EXPR_E + EXPR_OU2 | Empty;
        EXPR_E.Rule = EXPR_RELACIONAL + EXPR_E2;
        EXPR_E2.Rule = ToTerm("&&") + EXPR_RELACIONAL + EXPR_E2 | Empty;
        EXPR_RELACIONAL.Rule = EXPR_ADITIVA + EXPR_RELACIONAL2;
        EXPR_RELACIONAL2.Rule = COMP + EXPR_ADITIVA | Empty;
        EXPR_ADITIVA.Rule = EXPR_MULTIPLICATIVA + EXPR_ADITIVA2;
        EXPR_ADITIVA2.Rule = OP_ADITIVO + EXPR_MULTIPLICATIVA + EXPR_ADITIVA2 | Empty;
        OP_ADITIVO.Rule = ToTerm("+") | "-";
        EXPR_MULTIPLICATIVA.Rule = FATOR + EXPR_MULTIPLICATIVA2;
        EXPR_MULTIPLICATIVA2.Rule = OP_MULTIPLICATIVO + FATOR + EXPR_MULTIPLICATIVA2 | Empty;
        OP_MULTIPLICATIVO.Rule = ToTerm("*") | "/" | "%";
        FATOR.Rule = SINAL + TERMOESCRITA | text | "!" + FATOR | "(" + EXPRESSAO + ")";
        SINAL.Rule = ToTerm("+") | "-" | Empty; 
        CONSTANTE.Rule = number;

        COMP.Rule = ToTerm("<") | "==" | "!=" | ">" | "<=" | ">=";
        this.MarkReservedWords(
            "int", "float", "char", "boolean", "void", "if", "else", "for", "while",
            "scanf", "println", "main", "return"
            );
        
    }
}