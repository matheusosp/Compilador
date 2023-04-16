using Analisador_Lexico.Domain;

namespace Analisador_Lexico;

public class SyntacticAnalyzer
{
    private Stack<int> stack = new();
    private Token currentToken;

    //public void AnalyzeCode(List<Token> tokens)
    //{
    //    currentToken = tokens[0];
    //    stack.Push(0);

    //    while (true)
    //    {
    //        var state = stack.Peek();
    //        int action = ParserTable.ACTION[state, currentToken.type];

    //        if (action > 0)
    //        {
    //            stack.Push(action);
    //            currentToken = tokens[tokens.IndexOf(currentToken) + 1];
    //        }
    //        else if (action < 0)
    //        {
    //            var numSymbolsToPop = Math.Abs(action);
    //            for (var i = 0; i < numSymbolsToPop; i++)
    //            {
    //                stack.Pop();
    //            }
    //            state = stack.Peek();
    //            stack.Push(ParserTable.GOTO[state, ParserTable.LHS[-action]]);
    //            Console.WriteLine(ParserTable.ProductionToString[-action]);
    //        }
    //        else if (action == 0)
    //        {
    //            Console.WriteLine("Análise sintática concluída com sucesso.");
    //            break;
    //        }
    //        else
    //        {
    //            Console.WriteLine("Erro de sintaxe. Token atual: " + currentToken);
    //            RecoveryRoutine();
    //            if (stack.Count != 0) continue;
    //            Console.WriteLine("Análise sintática concluída com erro.");
    //            break;
    //        }
    //    }
    //}

    private void RecoveryRoutine()
    {
        // Implemente a rotina de recuperação de erros aqui
    }
}
