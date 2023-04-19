﻿using Analisador_Lexico.Domain;
using Analisador_Lexico.Irony.Parsing.Scanner;
using static System.Windows.Forms.AxHost;
using System;
using System.Text.RegularExpressions;

namespace Analisador_Lexico;

public class SyntacticAnalyzer
{
    private Stack<int> stack = new();
    private string currentToken;

    public List<(string, string, string, string)> AnalyzeCode(LexicalAnalyzer lexicalAnalyzer,string filePathCode)
    {
        var reader = new StreamReader(filePathCode);
        const int blockSize = 2048;
        var logTrace = new List<(string, string, string, string)>();
        while (!reader.EndOfStream)
        {
            var buffer = new char[blockSize];
            var bytesRead = reader.ReadBlock(buffer, 0, blockSize);
            var index = 0;
            var code = new string(buffer, 0, bytesRead);

            code += " EOF ";
            (index, currentToken) = lexicalAnalyzer.GetNextToken(code, index);

            stack.Push(0);

            while (true)
            {
                var state = stack.Peek();
                var stateTable = ParserTable.ACTION.First(a => a.Item1 == state);
                
                var action = stateTable.Item2.FirstOrDefault(a => Regex.Replace(a.Item1.ToLower(), @"\d+", "") == currentToken.ToLower().Replace(" ","").Replace("\n","").Replace("\r",""));
                if (action.Item1 == null && stateTable.Item2.First().Item2.Contains("r"))
                    action = stateTable.Item2.First();
                else if(action.Item1 == null)
                {
                    logTrace.Add((state.ToString(), "", "", "Erro de sintaxe. Token atual: " + currentToken));
                    if (currentToken == "") break;
                    break;
                }
                if (action.Item4 > 0)
                {
                    stack.Push(int.Parse(new string(action.Item2.Where(char.IsDigit).ToArray())));
                    logTrace.Add((state.ToString(),action.Item2,currentToken, "shift " + action.Item1));
                    (index, currentToken) = lexicalAnalyzer.GetNextToken(code, index);
                }
                if (action.Item4 < 0)
                {
                    var numSymbolsToPop = Math.Abs(action.Item3);
                    for (var i = 0; i < numSymbolsToPop; i++)
                    {
                        stack.Pop();
                    }

                    state = stack.Peek();
                    var stateGOTO = ParserTable.GOTO.First(a => a.Item1 == state);
                    var lhsSymbol = ParserTable.LHS.First(l => l.Item1 == action.Item2);
                    var gotoState = stateGOTO.Item2.First(g => g.Item1 == lhsSymbol.Item2);
                    stack.Push(int.Parse(new string(gotoState.Item2.Where(char.IsDigit).ToArray())));
                    var productionString = ParserTable.ProductionToString[int.Parse(new string(action.Item2.Where(char.IsDigit).ToArray()))];
                    logTrace.Add((state.ToString(), action.Item2, currentToken, "reduce " + productionString));
                    Console.WriteLine(productionString);
                }
                else if (action.Item4 == 0)
                {
                    Console.WriteLine("Análise sintática concluída com sucesso.");
                    logTrace.Add((state.ToString(), "", currentToken, action.Item2));
                    break;
                }
            }

            if (stack.Count > 1)
            {
                //logTrace.Add((stack.Peek().ToString(), "", currentToken, "")!);
            }

            
        }
        reader.Close();
        return logTrace;
    }

    //private void RecoveryRoutine()
    //{
    //    // Implemente a rotina de recuperação de erros aqui
    //}
}
