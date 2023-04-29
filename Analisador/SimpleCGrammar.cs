using Analisador_Lexico.Irony.Parsing.Grammar;
using Analisador_Lexico.Irony.Parsing.Terminals;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Analisador_Lexico;

public class SimpleCGrammar : Grammar
{
    public SimpleCGrammar()
    {

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
        DECFUNCAO.Rule = TIPORETORNO + ToTerm("ID") + ToTerm("(") + PARAMETROS + ToTerm(")") + BLOCO;
        TIPORETORNO.Rule = TIPO | ToTerm("void");
        TIPO.Rule = TIPOBASE + DIMENSAO ;
        TIPOBASE.Rule = ToTerm("char") | ToTerm("float") | ToTerm("int") | ToTerm("boolean");
        DIMENSAO.Rule = DIMENSAO + ToTerm("[") + ToTerm("num_int") + ToTerm("]") | Empty; 
        PARAMETROS.Rule = LISTAPARAMETROS | Empty;
        LISTAPARAMETROS.Rule = TIPO + ToTerm("ID") | LISTAPARAMETROS + ToTerm(",") + TIPO + ToTerm("ID");
        PRINCIPAL.Rule = ToTerm("main") + ToTerm("(") + ToTerm(")") + BLOCO;
        BLOCO.Rule = ToTerm("{") + SECAOVARIAVEIS + SECAOCOMANDOS + ToTerm("}");
        SECAOVARIAVEIS.Rule = LISTAVARIAVEIS | Empty;
        LISTAVARIAVEIS.Rule = TIPO + LISTAID + ToTerm(";") | LISTAVARIAVEIS + TIPO + LISTAID + ToTerm(";");
        LISTAID.Rule = ToTerm("ID") | LISTAID + ToTerm(",") + ToTerm("ID");
        SECAOCOMANDOS.Rule = LISTACOMANDOS | Empty;
        LISTACOMANDOS.Rule = COMANDO | LISTACOMANDOS + COMANDO;
        COMANDO.Rule = LEITURA | ESCRITA | ATRIBUIÇAO | FUNCAO | SELECAO | ENQUANTO | RETORNO;
        LEITURA.Rule = ToTerm("scanf") + ToTerm("(") + LISTATERMOLEITURA + ToTerm(")") + ToTerm(";");
        LISTATERMOLEITURA.Rule = TERMOLEITURA | LISTATERMOLEITURA + ToTerm(",") + TERMOLEITURA;
        TERMOLEITURA.Rule = ToTerm("ID") + DIMENSAO2;
        DIMENSAO2.Rule = DIMENSAO2 + ToTerm("[") + EXPR_ADITIVA + ToTerm("]") | Empty;
        ESCRITA.Rule = ToTerm("println") + ToTerm("(") + LISTATERMOESCRITA + ToTerm(")") + ToTerm(";");
        LISTATERMOESCRITA.Rule = TERMOESCRITA | LISTATERMOESCRITA + ToTerm(",") + TERMOESCRITA;
        TERMOESCRITA.Rule = ToTerm("ID") + DIMENSAO2 | CONSTANTE | ToTerm("TEXTO");
        SELECAO.Rule = ToTerm("if") + ToTerm("(") + EXPRESSAO + ToTerm(")") + BLOCO + SENAO;
        SENAO.Rule = ToTerm("else") + BLOCO | Empty;
        ENQUANTO.Rule = ToTerm("while") + ToTerm("(") + EXPRESSAO + ToTerm(")") + BLOCO;
        ATRIBUIÇAO.Rule = ToTerm("ID") + ToTerm("=") + COMPLEMENTO + ToTerm(";");
        COMPLEMENTO.Rule = EXPRESSAO | FUNCAO;
        FUNCAO.Rule = ToTerm("func") + ToTerm("ID") + ToTerm("(") + ARGUMENTOS + ToTerm(")");
        ARGUMENTOS.Rule = LISTAARGUMENTOS | Empty;
        LISTAARGUMENTOS.Rule = EXPRESSAO | LISTAARGUMENTOS + ToTerm(",") + EXPRESSAO;
        RETORNO.Rule = ToTerm("return") + EXPRESSAO + ToTerm(";");
        EXPRESSAO.Rule = EXPR_OU;
        EXPR_OU.Rule = EXPR_E | EXPR_OU + ToTerm("||") + EXPR_E; 
        EXPR_E.Rule = EXPR_RELACIONAL | EXPR_E + ToTerm("&&") + EXPR_RELACIONAL;
        EXPR_RELACIONAL.Rule = EXPR_ADITIVA | EXPR_ADITIVA + COMP + EXPR_ADITIVA;
        EXPR_ADITIVA.Rule = EXPR_MULTIPLICATIVA | EXPR_ADITIVA + OP_ADITIVO + EXPR_MULTIPLICATIVA;
        OP_ADITIVO.Rule = ToTerm("+") | ToTerm("-");
        EXPR_MULTIPLICATIVA.Rule = FATOR | EXPR_MULTIPLICATIVA + OP_MULTIPLICATIVO + FATOR;
        OP_MULTIPLICATIVO.Rule = ToTerm("*") | ToTerm("/") | ToTerm("%");
        FATOR.Rule = SINAL + ToTerm("ID") + DIMENSAO2 | SINAL + CONSTANTE | ToTerm("TEXTO") | ToTerm("!") + FATOR | ToTerm("(") + EXPRESSAO + ToTerm(")");
        CONSTANTE.Rule = ToTerm("num_int") | ToTerm("num_dec");
        SINAL.Rule = ToTerm("+") | ToTerm("-") | Empty; 
        

        COMP.Rule = ToTerm("<") | ToTerm("==") | ToTerm("!=") | ToTerm(">") | ToTerm("<=") | ToTerm(">=");
        this.MarkReservedWords(
            "int", "float", "char", "boolean", "void", "if", "else", "for", "while",
            "scanf", "println", "main", "return", "func"
            );
        
    }
}