#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System.Text;
using Analisador_Lexico.Irony.Parsing.Data;
using Analisador_Lexico.Irony.Parsing.Data.Construction;
using Analisador_Lexico.Irony.Parsing.Grammar;
using Analisador_Lexico.Irony.Parsing.Parser.ParserActions;
using Analisador_Lexico.Irony.Parsing.Terminals;
using Analisador_Lexico.Irony.Utilities;

namespace Analisador_Lexico.Irony.Parsing.Parser {
  public static class ParserDataPrinter {

        public static string PrintStateList(LanguageData language)
        {
            StringBuilder sb = new StringBuilder();
            var statesAction = new List<(int, List<(string, string,int, int)>)>();
            var statesGOTO = new List<(int, List<(string, string, int)>)>();
            var tableProductions = GetNonTerminals(language);
            foreach (ParserState state in language.ParserData.States)
            {
                var stateActions = new List<(string,string, int, int)>();
                var stateGOTO = new List<(string, string, int)>();
                sb.Append("State " + state.Name);
                var stateNumber = int.Parse(new string(state.Name.Where(char.IsDigit).ToArray()));
                if (state.BuilderData.IsInadequate) sb.Append(" (Inadequate)");
                sb.AppendLine();
                var srConflicts = state.BuilderData.GetShiftReduceConflicts();
                if (srConflicts.Count > 0)
                    sb.AppendLine("  Shift-reduce conflicts on inputs: " + srConflicts.ToString());
                var ssConflicts = state.BuilderData.GetReduceReduceConflicts();
                if (ssConflicts.Count > 0)
                    sb.AppendLine("  Reduce-reduce conflicts on inputs: " + ssConflicts.ToString());
                //LRItems
                if (state.BuilderData.ShiftItems.Count > 0)
                {
                    sb.AppendLine("  Shift items:");
                    foreach (var item in state.BuilderData.ShiftItems)
                        sb.AppendLine("    " + item.ToString());
                }
                if (state.BuilderData.ReduceItems.Count > 0)
                {
                    sb.AppendLine("  Reduce items:");
                    foreach (LRItem item in state.BuilderData.ReduceItems)
                    {
                        var sItem = item.ToString();
                        if (state.DefaultAction != null)
                        {
                            var production = tableProductions.First(p =>
                                string.Compare(p.Item2, sItem.Replace("·", ""), StringComparison.InvariantCultureIgnoreCase) == 0);

                            stateActions.Add(
                                (
                                    sItem,
                                    production.Item1,
                                    production.Item3,
                                    -1
                                )
                            );
                        }

                        if (item.Lookaheads.Count > 0)
                        {
                            sItem += " [" + item.Lookaheads.ToString() + "]";
                            foreach (var head in item.Lookaheads)
                            {
                                var production = tableProductions.First(p =>
                                    string.Compare(p.Item2, item.Core.Production.ToString().Replace("·", ""), StringComparison.InvariantCultureIgnoreCase) == 0);
                                stateActions.Add(
                                    (
                                        head.Name,
                                        production.Item1,
                                        production.Item3,
                                        - 1
                                    )
                                );
                            }
                        }

                        sb.AppendLine("    " + sItem);


                    }
                }
                sb.Append("  Transitions: ");
                bool atFirst = true;
                foreach (BnfTerm key in state.Actions.Keys)
                {
                    var acceptAction = state.Actions[key] as AcceptParserAction;
                    if (acceptAction != null)
                    {
                        stateActions.Add(
                            (
                                key.Name,
                                "Accept",
                                0,
                                0
                            )
                        );
                    }
                    var action = state.Actions[key] as ShiftParserAction;
                    if (action == null)
                        continue;
                    if (!atFirst) sb.Append(", ");
                    atFirst = false;
                    sb.Append(key.ToString());
                    sb.Append("->");
                    sb.Append(action.NewState.Name);
                    if (key is Terminal)
                    {
                        stateActions.Add(
                                (
                                    key.Name,
                                    "S" + new string(action.NewState.Name.Where(char.IsDigit).ToArray()),
                                    0,
                                    1
                                )
                            );
                    }
                    else if (key is NonTerminal)
                    {
                        stateGOTO.Add(
                            (
                                key.Name,
                                new string(action.NewState.Name.Where(char.IsDigit).ToArray()),
                                1
                            )
                        );
                        stateActions.Add(
                            (
                                key.Name,
                                new string(action.NewState.Name.Where(char.IsDigit).ToArray()),
                                0,
                                1
                            )
                        );
                    }

                }
                sb.AppendLine();
                sb.AppendLine();
                ParserTable.ACTION.Add((stateNumber, stateActions));
                ParserTable.GOTO.Add((stateNumber, stateGOTO));

            }//foreach
            foreach (var i in statesAction)
            {
                ParserTable.ACTION.Add((i.Item1, i.Item2));
                Console.Write($"State {i.Item1}: actions: (");
                foreach (var j in i.Item2)
                {
                    Console.Write($" {j.Item1} newStates: {j.Item2}");
                }
                Console.WriteLine(" )");
            }
            return sb.ToString();
        }
        public static List<(string, string, int)> GetNonTerminals(LanguageData language)
        {
            var ntList = language.GrammarData.NonTerminals.ToList();
            //ntList.Sort((x, y) => string.Compare(x.Name, y.Name));
            var tableProductions = new List<(string, string, int)>();
            var index = 0;
            foreach (var pr in ntList.SelectMany(nt => nt.Productions))
            {
                ParserTable.ProductionToString.Add(pr.ToString());
                tableProductions.Add(("r" + index, pr.ToString(), pr.RValues.Count));
                ParserTable.LHS.Add(("r" + index, pr.LValue.ToString()));
                index++;
            }
            return tableProductions;
        }
        public static string PrintTerminals(LanguageData language) {
          var termList = language.GrammarData.Terminals.ToList();
          termList.Sort((x, y) => string.Compare(x.Name, y.Name));
          var result = string.Join(Environment.NewLine, termList);
          return result; 
        }

    public static string PrintNonTerminals(LanguageData language) {
      StringBuilder sb = new StringBuilder();
      var ntList = language.GrammarData.NonTerminals.ToList();
      ntList.Sort((x, y) => string.Compare(x.Name, y.Name));
      foreach (var nt in ntList) {
        sb.Append(nt.Name);
        sb.Append(nt.Flags.IsSet(TermFlags.IsNullable) ? "  (Nullable) " : string.Empty);
        sb.AppendLine();
        foreach (Production pr in nt.Productions) {
          sb.Append("   ");
          sb.AppendLine(pr.ToString());
        }
      }//foreachc nt
      return sb.ToString();
    }

  }//class
}//namespace
