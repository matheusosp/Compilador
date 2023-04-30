using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analisador_Lexico.Domain;

public class StateAction
{
    public enum ActionType { Shift, Reduce, Accept }

    public string Symbol { get; set; }
    public ActionType Type { get; set; }
    public int RValuesCount { get; set; }
    public string NewState { get; set; }

    public StateAction(string symbol, ActionType type, int rValuesCount, string newState)
    {
        Symbol = symbol;
        Type = type;
        RValuesCount = rValuesCount;
        NewState = newState;
    }
}