using Analisador_Lexico.Irony.Parsing.Data;
using Analisador_Lexico.Irony.Parsing.Parser;

namespace Analisador_Lexico.Irony.Parsing.Grammar {
  // Should be implemented by Grammar class to be able to run samples in Grammar Explorer.
  public interface ICanRunSample {
    string RunSample(RunSampleArgs args);
  }

  public class RunSampleArgs {
    public LanguageData Language;
    public string Sample;
    public ParseTree ParsedSample;
    public IConsoleAdapter Console;
    public RunSampleArgs(LanguageData language, string sample, ParseTree parsedSample, IConsoleAdapter console = null) {
      Language = language;
      Sample = sample;
      ParsedSample = parsedSample;
      Console = console;
    }
  }
}
