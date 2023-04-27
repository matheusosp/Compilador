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

using Analisador_Lexico.Irony.Parsing.Data.Construction;
using Analisador_Lexico.Irony.Parsing.Grammar;

namespace Analisador_Lexico.Irony.Parsing.Data { 
  public partial class LanguageData {
    public readonly Grammar.Grammar Grammar;
    public readonly GrammarData GrammarData; 
    public readonly ParserData ParserData;
    public readonly ScannerData ScannerData;
    public readonly GrammarErrorList Errors = new GrammarErrorList(); 
    public GrammarErrorLevel ErrorLevel = GrammarErrorLevel.NoError;
    public long ConstructionTime;
    public bool AstDataVerified;

    public LanguageData(Grammar.Grammar grammar) {
      Grammar = grammar;
      GrammarData = new GrammarData(this);
      ParserData = new ParserData(this);
      ScannerData = new ScannerData(this);
      ConstructAll(); 
    }
    public void ConstructAll() {
      var builder = new LanguageDataBuilder(this);
      builder.Build();
    }
    public bool CanParse() {
      return ErrorLevel < GrammarErrorLevel.Error;
    }
  }//class
}//namespace
