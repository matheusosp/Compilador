using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analisador_Lexico.Irony.Parsing.Grammar;

namespace Analisador_Lexico.Domain;

public class StateAction
{
    public enum ActionType { Shift, Reduce, Accept }

    public string Symbol { get; set; }
    public ActionType Type { get; set; }
    public int RValuesCount { get; set; }
    public string NewState { get; set; }
    public NonTerminal.FuncaoDelegate AcaoPraExecutar { get; set; }
    public void ExecutarFunção(params string[] valores)
    {
        // Verificar se a função está definida
        if (AcaoPraExecutar != null)
        {
            // Chamar a função definida com os parâmetros especificados
            AcaoPraExecutar(valores);
        }
    }
    public StateAction(string symbol, ActionType type, int rValuesCount, string newState, NonTerminal.FuncaoDelegate acaoPraExecutar)
    {
        Symbol = symbol;
        Type = type;
        RValuesCount = rValuesCount;
        NewState = newState;
        AcaoPraExecutar = acaoPraExecutar;
    }
}