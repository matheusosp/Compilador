namespace Analisador_Lexico.Irony.Parsing.Grammar {
  //An abstraction of a Console.
  public interface IConsoleAdapter {
    bool Canceled { get; set; }
    void Write(string text);
    void WriteLine(string text);
    void SetTextStyle(ConsoleTextStyle style);
    int Read(); //reads a key
    string ReadLine(); //reads a line; returns null if Ctrl-C is pressed
    void SetTitle(string title);
    void Clear();
    string GetOutput();
  }

  public enum ConsoleTextStyle {
    Normal,
    Error,
  }
}
