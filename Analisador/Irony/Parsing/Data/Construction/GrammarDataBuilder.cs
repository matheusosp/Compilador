﻿#region License
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
using Analisador_Lexico.Irony.Parsing.Scanner;
using Analisador_Lexico.Irony.Parsing.Terminals;
using Analisador_Lexico.Irony.Utilities;

namespace Analisador_Lexico.Irony.Parsing.Data.Construction { 

  internal class GrammarDataBuilder {
    LanguageData _language;
    Grammar.Grammar _grammar;
    GrammarData _grammarData;
    int _unnamedCount; //internal counter for generating names for unnamed non-terminals
    internal int _lastItemId; //each LR0Item gets its unique ID, last assigned (max) Id is kept in this field

    internal GrammarDataBuilder(LanguageData language) {
      _language = language;
      _grammar = _language.Grammar;
    }

    internal void Build() {
      _grammarData = _language.GrammarData;
      CreateAugmentedRoots(); 
      CollectTermsFromGrammar();
      InitTermLists();
      FillOperatorReportGroup(); 
      CreateProductions();
      ComputeNonTerminalsNullability(_grammarData);
      ComputeTailsNullability(_grammarData);
      ValidateGrammar(); 
    }

    private void CreateAugmentedRoots() {
      _grammarData.AugmentedRoot = CreateAugmentedRoot(_grammar.Root); 
      foreach(var snippetRoot in _grammar.SnippetRoots) 
        _grammarData.AugmentedSnippetRoots.Add(CreateAugmentedRoot(snippetRoot));        
    }

    private NonTerminal CreateAugmentedRoot(NonTerminal root) {
      var result = new NonTerminal(root.Name + "'", root + _grammar.Eof);
      result.SetFlag(TermFlags.NoAstNode); //mark that we don't need AST node here 
      return result;
    }

    private void CollectTermsFromGrammar() {
      _unnamedCount = 0;
      _grammarData.AllTerms.Clear();
      //Start with NonGrammarTerminals, and set IsNonGrammar flag
      foreach (Terminal t in _grammarData.Grammar.NonGrammarTerminals) {
        t.SetFlag(TermFlags.IsNonGrammar);
        _grammarData.AllTerms.Add(t);
      }
      //Add main root
      CollectTermsRecursive(_grammarData.AugmentedRoot);
      foreach(var augmRoot in _grammarData.AugmentedSnippetRoots)
        CollectTermsRecursive(augmRoot); 
      //Add syntax error explicitly
      _grammarData.AllTerms.Add(_grammar.SyntaxError); 
    }

    private void CollectTermsRecursive(BnfTerm term) {
      if (_grammarData.AllTerms.Contains(term)) return;
      _grammarData.AllTerms.Add(term);
      NonTerminal nt = term as NonTerminal;
      if (nt == null) return;

      if (string.IsNullOrEmpty(nt.Name)) {
        if (nt.Rule != null && !string.IsNullOrEmpty(nt.Rule.Name))
          nt.Name = nt.Rule.Name;
        else
          nt.Name = "Unnamed" + (_unnamedCount++);
      }
      if (nt.Rule == null)
        _language.Errors.AddAndThrow(GrammarErrorLevel.Error, null, Resources.ErrNtRuleIsNull, nt.Name);
      //check all child elements
      foreach (BnfTermList elemList in nt.Rule.Data)
        for (int i = 0; i < elemList.Count; i++) {
          BnfTerm child = elemList[i];
          if (child == null) {
            _language.Errors.Add(GrammarErrorLevel.Error, null, Resources.ErrRuleContainsNull, nt.Name, i);
            continue; //for i loop 
          }
          //Check for nested expression - convert to non-terminal
          BnfExpression expr = child as BnfExpression;
          if (expr != null) {
            child = new NonTerminal(null, expr);
            elemList[i] = child;
          }
          CollectTermsRecursive(child);
        }//for i
    }//method

    private void FillOperatorReportGroup() {
      foreach(var group in _grammar.TermReportGroups)
        if(group.GroupType == TermReportGroupType.Operator) {
          foreach(var term in _grammarData.Terminals)
            if (term.Flags.IsSet(TermFlags.IsOperator))
              group.Terminals.Add(term); 
          return; 
        }
    }

    private void InitTermLists() {
      //Collect terminals and NonTerminals
      var empty = _grammar.Empty; 
      foreach (BnfTerm term in _grammarData.AllTerms) {  //remember - we may have hints, so it's not only terminals and non-terminals
        if (term is NonTerminal) _grammarData.NonTerminals.Add((NonTerminal)term);
        if (term is Terminal && term != empty) _grammarData.Terminals.Add((Terminal)term);
      }
      //Mark keywords - any "word" symbol directly mentioned in the grammar
      foreach (var term in _grammarData.Terminals) {
        var symTerm = term as KeyTerm;
        if (symTerm == null) continue;
        if (!string.IsNullOrEmpty(symTerm.Text) && char.IsLetter(symTerm.Text[0]))
          symTerm.SetFlag(TermFlags.IsKeyword); 
      }//foreach term
      //Init all terms
      foreach (var term in _grammarData.AllTerms)
        term.Init(_grammarData);
    }//method

    private void CreateProductions() {
      _lastItemId = 0;
      //CheckWrapTailHints() method may add non-terminals on the fly, so we have to use for loop here (not foreach)
      foreach (var nt in _grammarData.NonTerminals) {
        nt.Productions.Clear();
        //Get data (sequences) from both Rule and ErrorRule
        BnfExpressionData allData = new BnfExpressionData();
        allData.AddRange(nt.Rule.Data);
        if (nt.ErrorRule != null)
          allData.AddRange(nt.ErrorRule.Data);
        //actually create productions for each sequence
        foreach (BnfTermList prodOperands in allData) {
          Production prod = CreateProduction(nt, prodOperands);
          nt.Productions.Add(prod);
        } //foreach prodOperands
      }
    }

    private Production CreateProduction(NonTerminal lvalue, BnfTermList operands) {
      Production prod = new Production(lvalue);
      GrammarHintList hints = null;
      //create RValues list skipping Empty terminal and collecting grammar hints
      foreach (BnfTerm operand in operands) {
        if (operand ==  _grammar.Empty)
          continue;
        //Collect hints as we go - they will be added to the next non-hint element
        GrammarHint hint = operand as GrammarHint;
        if (hint != null) {
          if (hints == null) hints = new GrammarHintList();
          hints.Add(hint);
          continue;
        }
        //Add the operand and create LR0 Item
        prod.RValues.Add(operand);
        prod.LR0Items.Add(new LR0Item(_lastItemId++, prod, prod.RValues.Count - 1, hints));
        hints = null;
      }//foreach operand
      //set the flags
      if (prod.RValues.Count == 0)
        prod.Flags |= ProductionFlags.IsEmpty;
      //Add final LRItem
      ComputeProductionFlags(prod); 
      prod.LR0Items.Add(new LR0Item(_lastItemId++, prod, prod.RValues.Count, hints));
      return prod;
    }

    private void ComputeProductionFlags(Production production) {
      production.Flags = ProductionFlags.None;
      foreach (var rv in production.RValues) {
        //Check if it is a Terminal or Error element
        var t = rv as Terminal;
        if (t != null) {
          production.Flags |= ProductionFlags.HasTerminals;
          if (t.Category == TokenCategory.Error) production.Flags |= ProductionFlags.IsError;
        }
        if(rv.Flags.IsSet(TermFlags.IsPunctuation)) continue;
      }//foreach
    }//method

    private static void ComputeNonTerminalsNullability(GrammarData data) {
      var undecided = data.NonTerminals;
      while (undecided.Count > 0) {
        var newUndecided = new NonTerminalSet();
        foreach (NonTerminal nt in undecided)
          if (!ComputeNullability(nt))
            newUndecided.Add(nt);
        if (undecided.Count == newUndecided.Count) return;  //we didn't decide on any new, so we're done
        undecided = newUndecided;
      }//while
    }

    private static bool ComputeNullability(NonTerminal nonTerminal) {
      foreach (Production prod in nonTerminal.Productions) {
        if (prod.RValues.Count == 0) {
          nonTerminal.SetFlag(TermFlags.IsNullable);
          return true; //decided, Nullable
        }//if 
        //If production has terminals, it is not nullable and cannot contribute to nullability
        if (prod.Flags.IsSet(ProductionFlags.HasTerminals)) continue;
        //Go thru all elements of production and check nullability
        bool allNullable = true;
        foreach (BnfTerm child in prod.RValues) {
          allNullable &= child.Flags.IsSet(TermFlags.IsNullable);
        }//foreach child
        if (allNullable) {
          nonTerminal.SetFlag(TermFlags.IsNullable);
          return true;
        }
      }//foreach prod
      return false; //cannot decide
    }

    private static void ComputeTailsNullability(GrammarData data) {
      foreach (var nt in data.NonTerminals) {
        foreach (var prod in nt.Productions) {
          var count = prod.LR0Items.Count;
          for (int i = count - 1; i >= 0; i--) {
            var item = prod.LR0Items[i];
            item.TailIsNullable = true;
            if (item.Current == null) continue;
            if (!item.Current.Flags.IsSet(TermFlags.IsNullable))
              break; //for i
          }//for i
        }//foreach prod
      }
    }

    #region Grammar Validation
    private void ValidateGrammar() {
      var createAst = _grammar.LanguageFlags.IsSet(LanguageFlags.CreateAst); 
      var invalidTransSet = new NonTerminalSet();
      foreach(var nt in _grammarData.NonTerminals) {
        if(nt.Flags.IsSet(TermFlags.IsTransient)) {
          //List non-terminals cannot be marked transient - otherwise there may be some ambiguities and inconsistencies
          if (nt.Flags.IsSet(TermFlags.IsList))
            _language.Errors.Add(GrammarErrorLevel.Error, null, Resources.ErrListCannotBeTransient, nt.Name);
          //Count number of non-punctuation child nodes in each production
          foreach(var prod in nt.Productions)
            if (CountNonPunctuationTerms(prod) > 1) invalidTransSet.Add(nt); 
        }//if transient
        //Validate error productions
        foreach(var prod in nt.Productions)
          if(prod.Flags.IsSet(ProductionFlags.IsError)) {
            var lastTerm = prod.RValues[prod.RValues.Count -1];
            if (!(lastTerm is Terminal) || lastTerm == _grammar.SyntaxError)
              _language.Errors.Add(GrammarErrorLevel.Warning, null, Resources.ErrLastTermOfErrorProd, nt.Name);
                // "The last term of error production must be a terminal. NonTerminal: {0}"
          }//foreach prod
      }//foreac nt

      if (invalidTransSet.Count > 0)
        _language.Errors.Add(GrammarErrorLevel.Error, null, Resources.ErrTransientNtMustHaveOneTerm,invalidTransSet.ToString());
    }//method

    private int CountNonPunctuationTerms(Production production) {
      int count = 0;
      foreach(var rvalue in production.RValues)
        if (!rvalue.Flags.IsSet(TermFlags.IsPunctuation)) count++;
      return count; 
    }
    #endregion

  }//class
}
