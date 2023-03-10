using Analizador_Lexico.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Analizador_Lexico;

public class LexicalAnalyzer
{
    private readonly string[,] _transitionTable;
    private readonly Dictionary<int, string> _endState;
    private readonly List<int> _regressionState;
    public int _symbolsIdentificatorsTypeCount{ get; set; }
    public static Dictionary<int, string> _symbolsIdentificatorsType{ get; set; }
    private static readonly string[] _reservedWords =
{
            "abstract", "continue", "for", "new", "switch", "assert", "default", "if", "package", "synchronized",
            "boolean", "do", "goto", "private", "this", "break", "double", "implements", "protected", "throw",
            "byte", "else", "import", "public", "throws", "case", "enum", "instanceof", "return", "transient",
            "catch", "extends", "int", "short", "try", "char", "final", "interface", "static", "void",
            "class", "finally", "long", "strictfp", "volatile"
        };

    public LexicalAnalyzer()
    {
        _transitionTable = LoadExcel(Path.Combine(Directory.GetCurrentDirectory(), "Tabela de transicoes.csv"));
        _regressionState = new List<int>();
        _endState = new Dictionary<int, string>();
        _symbolsIdentificatorsType = new Dictionary<int, string>();
        AddSymbolsFromCsv();
        TableSymbol.ClearTable();
        TableToken.ClearTable();
        TableErrors.ClearTable();
    }

    private void AddSymbolsFromCsv()
    {
        var lastColumn = new string[_transitionTable.GetLength(0)];
        for (int i = 0; i < _transitionTable.GetLength(0); i++)
        {
            lastColumn[i] = _transitionTable[i, _transitionTable.GetLength(1) - 1];
        }
        foreach (var row in lastColumn)
        {
            if (row.Contains("//"))
            {
                int lineNumber = Array.IndexOf(lastColumn, row) - 1;
                string splitRow = row.Split("//", StringSplitOptions.RemoveEmptyEntries).First();
                string symbolName = splitRow.Trim();
                _endState.Add(lineNumber, symbolName);
                if (symbolName.Contains("*"))
                {
                    _regressionState.Add(lineNumber);
                }
            }
        }
    }

    private static string[,] LoadExcel(string filePath)
    {
        var rows = new List<string[]>();

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","

        };
        using var stream = File.OpenText(filePath);
        using var csvReader = new CsvReader(stream, csvConfig);

        while (csvReader.Read())
        {
            var row = new List<string>();
            string value;
            for (var i = 0; csvReader.TryGetField(i, out value); i++)
                row.Add(value.ToString());
            
            rows.Add(row.ToArray());
        }

        var matriz = new string[rows.Count, rows[0].Length];

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Length; j++)
            {
                matriz[i, j] = rows[i][j];
            }
        }

        return matriz;
    }
    public void AnalyzeCode(string filePathCode)
    {
        StreamReader reader = new StreamReader(filePathCode);
        int blockSize = 4096;

        while (!reader.EndOfStream)
        {
            char[] buffer = new char[blockSize];
            int bytesRead = reader.ReadBlock(buffer, 0, blockSize);
            int i = 0;
            string linha = new string(buffer, 0, bytesRead);
            string lexema = "";
            int estado = 0;

            while (i < linha.Length)
            {
                var car = linha[i];
                lexema += car;
                int coluna = GetColumn(car);
                int.TryParse(_transitionTable[estado +1, coluna], out estado);

                if (_endState.Keys.Contains(estado))
                {
                    if (_regressionState.Contains(estado))
                    {
                        i -= 1;
                        lexema = lexema.Substring(0, lexema.Length - 1).Trim();
                    }

                    AddToken(estado, lexema);
                    estado = 0;
                    lexema = "";
                }

                i += 1;
            }
            if(_endState.Keys.Contains(estado) == false)
                TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });
        }

        reader.Close();
    }

    private int GetColumn(char caractere) {
        
        var firstRow = GetRow(0).ToList();
        string stringCaractere = string.Empty;
        if (char.IsLetter(caractere))
        {
            stringCaractere = "L";
        }
        else if (char.IsDigit(caractere))
        {
            stringCaractere = "D";
        }
        else if (char.IsWhiteSpace(caractere))
        {
            stringCaractere += " ";
        }
        else if (char.IsControl(caractere))
        {
            stringCaractere = Regex.Escape(caractere.ToString());
        }
        else 
        {
            stringCaractere = caractere.ToString();
        }
        int columnNumber = firstRow.IndexOf(stringCaractere);
        if(columnNumber == -1)
            columnNumber = firstRow.IndexOf("Outros");
        return columnNumber;
    }
    private void AddToken(int state, string lexema)
    {
        var tokenType = "Undefined";
        if (_endState.GetValueOrDefault(state).Contains("ERRO"))
        {
            TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });
        }
        else if (_reservedWords.Contains(lexema)) 
        {
            tokenType = "PALAVRA RESERVADA";
            TableSymbol.AddSymbol(new Symbol { Id = state, Name = lexema });
            TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
        }
        else if (_endState.ContainsKey(state)) 
        {
            tokenType = _endState.GetValueOrDefault(state);
            var symbol = lexema;
            if (tokenType.Contains("ID")) 
            {
                if (!_symbolsIdentificatorsType.ContainsKey(_symbolsIdentificatorsTypeCount))
                {
                    _symbolsIdentificatorsType.Add(_symbolsIdentificatorsTypeCount, lexema);
                    _symbolsIdentificatorsTypeCount++;
                }
                
                symbol = "ID," + _symbolsIdentificatorsType.FirstOrDefault(s => s.Value == lexema);
            }
            TableSymbol.AddSymbol(new Symbol { Id = state, Name = symbol });
            TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
        }

        
    }
    public string[] GetColumn(int columnNumber)
    {
        return Enumerable.Range(0, _transitionTable.GetLength(0))
                .Select(x => _transitionTable[x, columnNumber])
                .ToArray();
    }

    public string[] GetRow(int rowNumber)
    {
        return Enumerable.Range(0, _transitionTable.GetLength(1))
                .Select(x => _transitionTable[rowNumber, x])
                .ToArray();
    }
}


