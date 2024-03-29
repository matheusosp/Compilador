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

using Analisador_Lexico.Irony.Parsing.Grammar;
using Analisador_Lexico.Irony.Parsing.Parser.ParserActions;
using Analisador_Lexico.Irony.Parsing.Terminals;
using Analisador_Lexico.Irony.Utilities;

//Helper data classes for ParserDataBuilder
// Note about using LRItemSet vs LRItemList. 
// It appears that in many places the LRItemList would be a better (and faster) choice than LRItemSet. 
// Many of the sets are actually lists and don't require hashset's functionality. 
// But surprisingly, using LRItemSet proved to have much better performance (twice faster for lookbacks/lookaheads computation), so LRItemSet
// is used everywhere.
namespace Analisador_Lexico.Irony.Parsing.Data.Construction { 

  public class ParserStateData {
    public readonly ParserState State;
    public readonly LRItemSet AllItems = new LRItemSet();
    public readonly LRItemSet ShiftItems = new LRItemSet();
    public readonly LRItemSet ReduceItems = new LRItemSet();
    public readonly LRItemSet InitialItems = new LRItemSet();
    public readonly BnfTermSet ShiftTerms = new BnfTermSet();
    public readonly TerminalSet ShiftTerminals = new TerminalSet();
    public readonly TerminalSet Conflicts = new TerminalSet();
    public readonly bool IsInadequate;
    public LR0ItemSet AllCores = new LR0ItemSet();

    //used for creating canonical states from core set
    public ParserStateData(ParserState state, LR0ItemSet kernelCores) {
      State = state;
      foreach (var core in kernelCores)
        AddItem(core);
      IsInadequate =  ReduceItems.Count > 1 || ReduceItems.Count == 1 && ShiftItems.Count > 0;
    }

    public void AddItem(LR0Item core) {
      //Check if a core had been already added. If yes, simply return
      if(!AllCores.Add(core))return ; 
      //Create new item, add it to AllItems, InitialItems, ReduceItems or ShiftItems
      var item = new LRItem(State, core);
      AllItems.Add(item); 
      if (item.Core.IsFinal)
        ReduceItems.Add(item);
      else
        ShiftItems.Add(item);
      if (item.Core.IsInitial)
        InitialItems.Add(item); 
      if (core.IsFinal) return; 
      //Add current term to ShiftTerms
      if (!ShiftTerms.Add(core.Current)) return; 
      if (core.Current is Terminal)
        ShiftTerminals.Add(core.Current as Terminal); 
      //If current term (core.Current) is a new non-terminal, expand it
      var currNt = core.Current as NonTerminal;
      if (currNt == null) return; 
      foreach(var prod in currNt.Productions) 
        AddItem(prod.LR0Items[0]);
    }//method

    public TransitionTable Transitions {
      get {
        if(_transitions == null)
          _transitions = new TransitionTable();
        return _transitions;
      }
    } TransitionTable _transitions; 

    //A set of states reachable through shifts over nullable non-terminals. Computed on demand
    public ParserStateSet ReadStateSet {
      get {
        if(_readStateSet == null) {
          _readStateSet = new ParserStateSet(); 
          foreach(var shiftTerm in State.BuilderData.ShiftTerms)
            if (shiftTerm.Flags.IsSet(TermFlags.IsNullable)) {
              var shift = State.Actions[shiftTerm] as ShiftParserAction;
              var targetState = shift.NewState;
              _readStateSet.Add(targetState);
              _readStateSet.UnionWith(targetState.BuilderData.ReadStateSet); //we shouldn't get into loop here, the chain of reads is finite
            }
        }//if 
        return _readStateSet;
      }
    } ParserStateSet _readStateSet;

    public ParserState GetNextState(BnfTerm shiftTerm) {
      var shift = ShiftItems.FirstOrDefault(item => item.Core.Current == shiftTerm);
      if (shift == null) return null;
      return shift.ShiftedItem.State; 
    }

    public TerminalSet GetShiftReduceConflicts() {
      var result = new TerminalSet();
      result.UnionWith(Conflicts);
      result.IntersectWith(ShiftTerminals);
      return result;
    }
    public TerminalSet GetReduceReduceConflicts() {
      var result = new TerminalSet();
      result.UnionWith(Conflicts);
      result.ExceptWith(ShiftTerminals);
      return result;
    }

  }//class

  //An object representing inter-state transitions. Defines Includes, IncludedBy that are used for efficient lookahead computation 
  public class Transition {
    public readonly ParserState FromState;
    public readonly ParserState ToState;
    public readonly NonTerminal OverNonTerminal;
    public readonly LRItemSet Items;
    public readonly TransitionSet Includes = new TransitionSet();
    public readonly TransitionSet IncludedBy = new TransitionSet();
    int _hashCode;

    public Transition(ParserState fromState, NonTerminal overNonTerminal) {
      FromState = fromState;
      OverNonTerminal = overNonTerminal;
      var shiftItem = fromState.BuilderData.ShiftItems.First(item=>item.Core.Current == overNonTerminal);
      ToState = FromState.BuilderData.GetNextState(overNonTerminal);
      _hashCode = unchecked(FromState.GetHashCode() - overNonTerminal.GetHashCode());
      FromState.BuilderData.Transitions.Add(overNonTerminal, this);   
      Items = FromState.BuilderData.ShiftItems.SelectByCurrent(overNonTerminal);
      foreach(var item in Items) {
        item.Transition = this;
      }
      
    }//constructor

    public void Include(Transition other) {
      if (other == this)  return;
      if (!IncludeTransition(other)) return; 
      //include children
      foreach(var child in other.Includes) {
        IncludeTransition(child); 
      }
    }
    private bool IncludeTransition(Transition other) {
      if (!Includes.Add(other)) return false; 
      other.IncludedBy.Add(this);
      //propagate "up"
      foreach(var incBy in IncludedBy)
        incBy.IncludeTransition(other);
      return true; 
    }

    public override string ToString() {
      return FromState.Name + " -> (over " + OverNonTerminal.Name + ") -> " + ToState.Name;
    }
    public override int GetHashCode() {
      return _hashCode;
    }
  }//class

  public class TransitionSet : HashSet<Transition> { }
  public class TransitionList : List<Transition> { }
  public class TransitionTable : Dictionary<NonTerminal, Transition> { }

  public class LRItem {
    public readonly ParserState State;
    public readonly LR0Item Core;
    //these properties are used in lookahead computations
    public LRItem ShiftedItem;
    public Transition Transition; 
    int _hashCode;

    //Lookahead info for reduce items
    public TransitionSet Lookbacks = new TransitionSet(); 
    public TerminalSet Lookaheads = new TerminalSet();

    public LRItem(ParserState state, LR0Item core) {
      State = state;
      Core = core;
      _hashCode = unchecked(state.GetHashCode() + core.GetHashCode());
    }
    public override string ToString() {
      return Core.ToString();
    }
    public override int GetHashCode() {
      return _hashCode; 
    }

    public TerminalSet GetLookaheadsInConflict() {
      var lkhc = new TerminalSet();
      lkhc.UnionWith(Lookaheads);
      lkhc.IntersectWith(State.BuilderData.Conflicts);
      return lkhc; 
    }

  }//LRItem class

  public class LRItemList : List<LRItem> {}

  public class LRItemSet : HashSet<LRItem> {

    public LRItem FindByCore(LR0Item core) {
      foreach (LRItem item in this)
        if (item.Core == core) return item;
      return null;
    }
    public LRItemSet SelectByCurrent(BnfTerm current) {
      var result = new LRItemSet();
      foreach (var item in this)
        if (item.Core.Current == current)
          result.Add(item);
      return result;
    }

    public LR0ItemSet GetShiftedCores() {
      var result = new LR0ItemSet();
      foreach (var item in this)
        if (item.Core.ShiftedItem != null) 
          result.Add(item.Core.ShiftedItem);
      return result;
    }
    public LRItemSet SelectByLookahead(Terminal lookahead) {
      var result = new LRItemSet();
      foreach (var item in this)
        if (item.Lookaheads.Contains(lookahead))
          result.Add(item);
      return result;
    }

  }//class

  public partial class LR0Item {
    public readonly Production Production;
    public readonly int Position;
    public readonly BnfTerm Current;
    public bool TailIsNullable;
    public GrammarHintList Hints = new GrammarHintList();

    //automatically generated IDs - used for building keys for lists of kernel LR0Items
    // which in turn are used to quickly lookup parser states in hash
    internal readonly int ID;

    public LR0Item(int id, Production production, int position, GrammarHintList hints) {
      ID = id;
      Production = production;
      Position = position;
      Current = (Position < Production.RValues.Count) ? Production.RValues[Position] : null;  
      if (hints != null)
        Hints.AddRange(hints); 
      _hashCode = ID.ToString().GetHashCode();
    }//method

    public LR0Item ShiftedItem {
      get {
        if (Position >= Production.LR0Items.Count - 1)
          return null;
        else
          return Production.LR0Items[Position + 1];
      }
    }
    public bool IsKernel {
      get { return Position > 0; }
    }
    public bool IsInitial {
      get { return Position == 0; }
    }
    public bool IsFinal {
      get { return Position == Production.RValues.Count; }
    }
    public override string ToString() {
      return Production.ProductionToString(Production, Position);
    }
    public override int GetHashCode() {
      return _hashCode;
    } int _hashCode;

  }//LR0Item

  public class LR0ItemList : List<LR0Item> { }
  public class LR0ItemSet : HashSet<LR0Item> { }



}//namespace
