using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analisador_Lexico.Domain;

namespace Analisador_Lexico;

public static class ParserTable
{
    public static List<(int stateNumber, List<StateAction> stateActions)> ACTION = new();
    //State, ListReductionsOfState<NonTerminal, NewStateToPush>
    public static List<(int, List<(string, string)>)> GOTO = new ();
    //NewStateReduction, ProductionToReduce 
    public static List<(string, string)> LHS = new();
    public static List<string> ProductionToString = new();
};

