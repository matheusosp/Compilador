using System.Text.RegularExpressions;
using static Analisador_Lexico.Domain.StateAction;

namespace Analisador_Lexico;

public class SyntacticAnalyzer
{
    private Stack<int> stack = new();
    private string currentToken;

    public List<(string, string, string, string)> AnalyzeCode(LexicalAnalyzer lexicalAnalyzer,string filePathCode)
    {
        lexicalAnalyzer.AnalyzeCode(filePathCode);
        var logTrace = new List<(string, string, string, string)>();
        currentToken = lexicalAnalyzer.GetNextToken();
        stack.Push(0);

        while (true)
        {
            var stackString = string.Join(" ", stack.ToArray().Reverse());
            var state = stack.Peek();
            var stateTable = ParserTable.ACTION.First(a => a.Item1 == state);
            
            var action = stateTable.Item2
                .FirstOrDefault(a => a.Symbol.ToLower() == currentToken.ToLower());

            if (action == null && stateTable.Item2.First().NewState.Contains("r"))
                action = stateTable.Item2.First();

            if(action == null)
            {
                logTrace.Add((stackString, "", "", "Erro de sintaxe. Token atual: " + currentToken));
                break;
            }

            if (action.Type == ActionType.Shift)
            {
                stack.Push(int.Parse(new string(action.NewState.Where(char.IsDigit).ToArray())));
                logTrace.Add((stackString, action.NewState, currentToken, "shift " + action.Symbol));
                currentToken = lexicalAnalyzer.GetNextToken();
            }
            if (action.Type == ActionType.Reduce)
            {
                var numSymbolsToPop = Math.Abs(action.RValuesCount);
                for (var i = 0; i < numSymbolsToPop; i++)
                {
                    stack.Pop();
                }

                state = stack.Peek();

                var lhsSymbol = ParserTable.LHS.First(l => l.Item1 == action.NewState);
                var stateGOTO = ParserTable.GOTO.First(a => a.Item1 == state);
                var gotoState = stateGOTO.Item2.First(g => g.Item1 == lhsSymbol.Item2);

                stack.Push(int.Parse(gotoState.Item2));

                var productionString = ParserTable.ProductionToString[int.Parse(new string(action.NewState.Where(char.IsDigit).ToArray()))];
                logTrace.Add((stackString, action.NewState, currentToken, "reduce " + productionString));
            }
            else if (action.Type == ActionType.Accept)
            {
                logTrace.Add((stackString, "", currentToken, action.NewState));
                break;
            }
        }

        return logTrace;
    }

}
