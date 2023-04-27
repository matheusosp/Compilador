using Analisador_Lexico.Irony.Parsing.Data;
using Analisador_Lexico.Irony.Parsing.Data.Construction;
using Analisador_Lexico.Irony.Parsing.Grammar;


namespace Analisador_Lexico.Irony.Parsing.Parser.ParserActions {
  public class ShiftParserAction: ParserAction {
    public readonly BnfTerm Term; 
    public readonly ParserState NewState;

    public ShiftParserAction(LRItem item) : this(item.Core.Current, item.ShiftedItem.State) {  }
    
    public ShiftParserAction(BnfTerm term, ParserState newState) {
      if (newState == null)
        throw new Exception("ParserShiftAction: newState may not be null. term: " + term.ToString());

      Term = term; 
      NewState = newState;
    }

    public override void Execute(ParsingContext context) {
      var currInput = context.CurrentParserInput;
      currInput.Term.OnShifting(context.SharedParsingEventArgs);
      context.ParserStack.Push(currInput, NewState);
      context.CurrentParserState = NewState;
      context.CurrentParserInput = null;
    }

    public override string ToString() {
      return string.Format(Resources.LabelActionShift, NewState.Name);
    }
  
  }//class
}
