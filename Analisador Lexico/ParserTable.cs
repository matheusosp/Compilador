using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analisador_Lexico
{
    public static class ParserTable
    {
        //public static string PrintStateList(LanguageData language)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (ParserState state in language.ParserData.States)
        //    {
        //        sb.Append("State " + state.Name);
        //        if (state.BuilderData.IsInadequate) sb.Append(" (Inadequate)");
        //        sb.AppendLine();
        //        var srConflicts = state.BuilderData.GetShiftReduceConflicts();
        //        if (srConflicts.Count > 0)
        //            sb.AppendLine("  Shift-reduce conflicts on inputs: " + srConflicts.ToString());
        //        var ssConflicts = state.BuilderData.GetReduceReduceConflicts();
        //        if (ssConflicts.Count > 0)
        //            sb.AppendLine("  Reduce-reduce conflicts on inputs: " + ssConflicts.ToString());
        //        //LRItems
        //        if (state.BuilderData.ShiftItems.Count > 0)
        //        {
        //            sb.AppendLine("  Shift items:");
        //            foreach (var item in state.BuilderData.ShiftItems)
        //                sb.AppendLine("    " + item.ToString());
        //        }
        //        if (state.BuilderData.ReduceItems.Count > 0)
        //        {
        //            sb.AppendLine("  Reduce items:");
        //            foreach (LRItem item in state.BuilderData.ReduceItems)
        //            {
        //                var sItem = item.ToString();
        //                if (item.Lookaheads.Count > 0)
        //                    sItem += " [" + item.Lookaheads.ToString() + "]";
        //                sb.AppendLine("    " + sItem);
        //            }
        //        }
        //        sb.Append("  Transitions: ");
        //        bool atFirst = true;
        //        foreach (BnfTerm key in state.Actions.Keys)
        //        {
        //            var action = state.Actions[key] as ShiftParserAction;
        //            if (action == null)
        //                continue;
        //            if (!atFirst) sb.Append(", ");
        //            atFirst = false;
        //            sb.Append(key.ToString());
        //            sb.Append("->");
        //            sb.Append(action.NewState.Name);
        //        }
        //        sb.AppendLine();
        //        sb.AppendLine();
        //    }//foreach
        //    return sb.ToString();
        //}
    }
}
