

namespace Analisador_Lexico.Irony.Parsing.Parser.ParserActions {
  public class AcceptParserAction: ParserAction {

    public override void Execute(ParsingContext context) {
      context.CurrentParseTree.Root = context.ParserStack.Pop(); //Pop root
      context.Status = ParserStatus.Accepted;
    }

    public override string ToString() {
      return Resources.LabelActionAccept;
    }
  }//class
}
