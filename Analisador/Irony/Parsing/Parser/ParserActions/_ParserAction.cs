using Analisador_Lexico.Irony.Parsing.Grammar;


namespace Analisador_Lexico.Irony.Parsing.Parser.ParserActions {

  public abstract partial class ParserAction {

    public ParserAction() { }

    public virtual void Execute(ParsingContext context) {
    
    }

    public override string ToString() {
      return Resources.LabelActionUnknown; //should never happen
    }

  }//class ParserAction

  public class ParserActionTable : Dictionary<BnfTerm, ParserAction> { }


}
