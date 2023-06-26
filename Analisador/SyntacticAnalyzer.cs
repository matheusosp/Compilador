using System.Text.RegularExpressions;
using Analisador_Lexico.Domain;
using static Analisador_Lexico.Domain.StateAction;

namespace Analisador_Lexico;

public class SyntacticAnalyzer
{
    
    private Stack<int> stack = new();
    private Token? currentToken;
    private List<(string? Name, string? Type, int? Width, int? Offset, string? Id)> semanticTbSymbol = new();
    private string message = "\n";

    public List<(string, string, string, string)> AnalyzeCode(LexicalAnalyzer lexicalAnalyzer,string filePathCode)
    {
        lexicalAnalyzer.AnalyzeCode(filePathCode);
        var logTrace = new List<(string, string, string, string)>();
        currentToken = lexicalAnalyzer.GetNextToken();
        stack.Push(0);

        var syntaxTree = new Tree<string>();
        while (true)
        {
            var stackString = string.Join(" ", stack.ToArray().Reverse());
            var state = stack.Peek();
            var stateTable = ParserTable.ACTION.First(a => a.Item1 == state);

            string[] array = { "id", "num", "texto" };
            var currentSymbol = array.Any(currentToken!.Type.ToLower().Contains)
                ? currentToken!.Type.ToLower()
                : currentToken!.Name.ToLower();
            var action = stateTable.Item2
                .FirstOrDefault(a => a.Symbol.ToLower() == currentSymbol);

            if (action == null && stateTable.Item2.First().NewState.Contains("r"))
                action = stateTable.Item2.First();

            if(action == null)
            {
                logTrace.Add((stackString, "", "", "Erro de sintaxe. Token atual: " + currentToken.Type));
                break;
            }

            if (action.Type == ActionType.Shift)
            {
                stack.Push(int.Parse(new string(action.NewState.Where(char.IsDigit).ToArray())));
                logTrace.Add((stackString, action.NewState, currentToken.Type, "shift " + action.Symbol));
                currentToken = lexicalAnalyzer.GetNextToken();
                //syntaxTree.Add(currentSymbol);

            }
            if (action.Type == ActionType.Reduce)
            {
                var numSymbolsToPop = Math.Abs(action.RValuesCount);
                var lhsSymbol = ParserTable.LHS.First(l => l.Item1 == action.NewState);
                var productionString = ParserTable.ProductionToString[int.Parse(new string(action.NewState.Where(char.IsDigit).ToArray()))];
                syntaxTree.Begin(lhsSymbol.Item2);
                for (var i = 0; i < numSymbolsToPop; i++)
                {
                    stack.Pop();
                    syntaxTree.Add(productionString.Split(" ")[i + 2]);

                }
                state = stack.Peek();
                
                var stateGOTO = ParserTable.GOTO.First(a => a.Item1 == state);
                var gotoState = stateGOTO.Item2.First(g => g.Item1 == lhsSymbol.Item2);
                stack.Push(int.Parse(gotoState.Item2));
                
                
                logTrace.Add((stackString, action.NewState, currentToken!.Type, "reduce " + productionString));
                //ApplySemanticRules(newNode, productionString, logTrace);
            }
            else if (action.Type == ActionType.Accept)
            {
                logTrace.Add((stackString, "", currentToken!.Type, action.NewState));
                var level = 1;
                message += "PROGRAMA\n";
                var firstExecution = true;
                foreach (var element in syntaxTree.m_Stack.ToArray())
                {
                    PrintNode(element, level, firstExecution);
                    level = 1;
                    firstExecution = false;
                }
                // PrintNode(syntaxTree.m_Stack.ToArray().Last(), 0);
                throw new Exception(message);
                break;
            }
        }

        return logTrace;
    }
    void PrintNode<T>(TreeNode<T> node, int level, bool firstExecution)
    {
        if(firstExecution == false)
            message+=$"{node.Value}\n";
        var i = 0;
        foreach (var element in node.Children)
        {
            if (i != node.Children.Count - 1 || firstExecution)
            {
                string indentation = new string(' ', level * 3);
                message+=$"{indentation}{element.Value}\n";
                i++;
            }

        }
    }
    private void ApplySemanticRules(SyntaxTreeNode node, string production,
        List<(string, string, string, string)> logTrace)
    {
        if (production.Equals("CONTEXT1 -> "))
        {
            var functionName = node.Parent.Children[0].Token!.Name; 
            var context = new Context(functionName, 0); 
            node.Context = context;
        }
        if (production.Equals("CONTEXT2 -> "))
        {
            var functionName = "main"; 
            var context = new Context(functionName, 0); 
            node.Context = context;
        }
        if (production.Equals("TIPO -> TIPOBASE DIMENSAO "))
        {
            var dimensao = FindNode(node,"DIMENSAO");

            var tipoInfo = new TypeInfo { Type = dimensao.TypeInfo.Type, Width = dimensao.TypeInfo.Width };
            node.TypeInfo = tipoInfo;
        }
        if (production.Equals("TIPOBASE -> char "))
        {
            node.TypeInfo = new TypeInfo { Type = "char", Width = 1 };
        }
        if (production.Equals("TIPOBASE -> float "))
        {
            node.TypeInfo = new TypeInfo { Type = "float", Width = 8 };
        }
        if (production.Equals("TIPOBASE -> int "))
        {
            node.TypeInfo = new TypeInfo { Type = "int", Width = 4 };
        }
        if (production.Equals("TIPOBASE -> boolean "))
        {
            node.TypeInfo = new TypeInfo { Type = "boolean", Width = 1 };
        }
        if (production.Equals("DIMENSAO -> "))
        {
            var tipoBase = FindNode(node,"TIPOBASE"); // Nó tipoBase
            node.TypeInfo = new TypeInfo { Type = tipoBase.TypeInfo.Type, Width = tipoBase.TypeInfo.Width };
        }
        if (production.Equals("DIMENSAO -> DIMENSAO [ num_int ] "))
        {
            var dimensao1 = FindNode(node,"DIMENSAO");
            var dimensao = FindNode(dimensao1,"DIMENSAO");
            var dimensao2 = FindNode(dimensao,"DIMENSAO");
            
            node!.TypeInfo = new TypeInfo
            {
                Type = $"array({dimensao1?.Children[1].Token!.Name},{dimensao1?.TypeInfo.Type})", 
                Width = int.Parse(dimensao1?.Children[1].Token!.Name!) * dimensao1!.TypeInfo.Width
            };
        }
        if (production.Equals("LISTAID -> ID "))
        {
            var tipo = FindNode(node, "TIPO");
            var context = FindNode(node,"TIPORETORNO").Children[1];

            semanticTbSymbol.Add((
                tipo.Token.Name,
                tipo.TypeInfo.Type,
                tipo.TypeInfo.Width,
                context.Context.Offset,
                context.Context.Id
                ));
            context.Context.Offset += tipo.TypeInfo.Width;
        }
        if (production.Equals("LISTAID -> LISTAID , id "))
        {
            var tipo = FindNode(node, "TIPO");
            var context = FindContext(node);
            semanticTbSymbol.Add((
                tipo?.Token!.Name,
                tipo?.TypeInfo.Type,
                tipo?.TypeInfo.Width,
                node.Context.Offset,
                node.Context.Id
            ));
            context.Context.Offset += tipo.TypeInfo.Width;
        }
        
    }
    private SyntaxTreeNode? FindTypeInfo(SyntaxTreeNode currentNode, string nodeToFind)
    {
        while (true)
        {
            var i = 0;
            while (i < currentNode.Children?.Count)
            {
                var node = currentNode.Children[i];
                if (node?.TypeInfo != null && node?.Symbol == nodeToFind) return node;
                var nodeChild = FindNodeWithoutNullTypeInfo(node,nodeToFind);
                if (nodeChild != null && nodeChild?.TypeInfo != null)
                {
                    return nodeChild;
                }

                i++;
            }

            if (currentNode?.Parent == null)
                return null;
            currentNode = currentNode.Parent;
        }
    }
    public SyntaxTreeNode? FindNodeWithoutNullTypeInfo(SyntaxTreeNode node, string nodeToFind)
    {
        if (node?.TypeInfo != null && node?.Symbol == nodeToFind)
        {
            return node;
        }
    
        foreach (var child in node.Children)
        {
            var result = FindNodeWithoutNullTypeInfo(child, nodeToFind);
        
            if (result != null)
            {
                return result;
            }
        }
    
        return null;
    }
    private SyntaxTreeNode? FindContext(SyntaxTreeNode currentNode)
    {
        while (true)
        {
            if (currentNode?.Context != null) return currentNode;
            
            var i = 0;
            while (i < currentNode.Children?.Count)
            {
                var node = currentNode.Children[i];
                if (node?.Context != null) return node;
                var nodeChild = FindNodeWithoutNullContext(node);
                if (nodeChild != null && nodeChild?.Context != null)
                {
                    return nodeChild;
                }
            
                i++;
            }
            
            
            if (currentNode?.Parent == null)
                return null;
            currentNode = currentNode.Parent;
        }
    }

    private SyntaxTreeNode? FindNodeWithoutNullContext(SyntaxTreeNode node)
    {
        if (node?.Context != null)
        {
            return node;
        }
        if (node?.Parent.Context != null)
        {
            return node?.Parent;
        }
        foreach (var child in node.Children)
        {
            var result = FindNodeWithoutNullContext(child);
        
            if (result != null)
            {
                return result;
            }
        }
    
        return null;
    }

    private SyntaxTreeNode? FindNode(SyntaxTreeNode currentNode, string nodeToFind)
    {
        if (currentNode?.Parent != null)
            currentNode = currentNode.Parent;
        
        if (currentNode?.Symbol == nodeToFind) return currentNode;
        
        while (true)
        {
            var i = currentNode.Children?.Count -1 ?? 0;
            while (i >= 0)
            {
                var node = currentNode.Children[i];
                if (node?.Symbol == nodeToFind) return node;
                i--;
            }

            if (currentNode?.Parent != null)
                currentNode = currentNode.Parent;
        }
    }
}
