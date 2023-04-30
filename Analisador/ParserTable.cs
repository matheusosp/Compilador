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
    public static List<(int, List<(string, string, int)>)> GOTO = new ();
    public static List<(string, string)> LHS = new();
    public static List<string> ProductionToString = new();
};

