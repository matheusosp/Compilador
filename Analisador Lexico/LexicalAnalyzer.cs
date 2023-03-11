using System.Globalization;
using System.Text.RegularExpressions;
using Analisador_Lexico.Domain;
using CsvHelper;
using CsvHelper.Configuration;

namespace Analisador_Lexico;

public class LexicalAnalyzer
{
    private int _symbolsIdentificatorsId;
    private readonly List<int> _regressionState;
    private readonly string[,] _transitionTable;
    private readonly Dictionary<int, string> _endState;
    private Dictionary<int, string> _symbolsIdentificators;
    private readonly string[] _reservedWords =
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
        _symbolsIdentificators = new Dictionary<int, string>();
        AddSymbolsFromCsv();
        TableSymbol.ClearTable();
        TableToken.ClearTable();
        TableErrors.ClearTable();
    }

    public void AnalyzeCode(string filePathCode)
    {
        var reader = new StreamReader(filePathCode);
        var blockSize = 4096;

        while (!reader.EndOfStream)
        {
            var buffer = new char[blockSize];
            var bytesRead = reader.ReadBlock(buffer, 0, blockSize);
            var i = 0;
            var linha = new string(buffer, 0, bytesRead);
            var lexema = "";
            var estado = 0;

            while (i < linha.Length)
            {
                var car = linha[i];
                lexema += car;
                var coluna = GetColumn(car);
                int.TryParse(_transitionTable[estado +1, coluna], out estado);

                if (_endState.ContainsKey(estado))
                {
                    if (_regressionState.Contains(estado))
                    {
                        i -= 1;
                        lexema = lexema.Substring(0, lexema.Length - 1).Trim();
                    }

                    AddLexemeToTable(estado, lexema);
                    estado = 0;
                    lexema = "";
                }

                i += 1;
            }
            if(_endState.ContainsKey(estado) == false)
                TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });
        }

        reader.Close();
    }

    private int GetColumn(char caractere) {
        
        var firstRow = GetRow(0).ToList();
        var stringCaractere = string.Empty;
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
    private void AddLexemeToTable(int state, string lexema)
    {
        var tokenType = "UNDEFINED";
        if (_endState.GetValueOrDefault(state)!.Contains("ERRO"))
        {
            TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });
        }
        else if (_reservedWords.Contains(lexema)) 
        {
            tokenType = "PALAVRA RESERVADA";
            TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
        }
        else if (_endState.ContainsKey(state)) 
        {
            tokenType = _endState.GetValueOrDefault(state);
            var symbol = lexema;
            if (tokenType!.Contains("ID")) 
            {
                if (!_symbolsIdentificators.ContainsKey(_symbolsIdentificatorsId))
                {
                    _symbolsIdentificators.Add(_symbolsIdentificatorsId, lexema);
                    _symbolsIdentificatorsId++;
                }
                
                symbol = "ID," + _symbolsIdentificators.FirstOrDefault(s => s.Value == lexema);
                TableSymbol.AddSymbol(new Symbol { Id = state, Name = symbol });
                TableToken.AddToken(new Token { Id = state, Name = symbol, Type = tokenType });
            }
            else
                TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
        }
        
    }
    private void AddSymbolsFromCsv()
    {
        var lastColumn = new string[_transitionTable.GetLength(0)];
        for (var i = 0; i < _transitionTable.GetLength(0); i++)
        {
            lastColumn[i] = _transitionTable[i, _transitionTable.GetLength(1) - 1];
        }
        foreach (var row in lastColumn)
        {
            if (!row.Contains("//")) continue;
            var lineNumber = Array.IndexOf(lastColumn, row) - 1;
            var splitRow = row.Split("//", StringSplitOptions.RemoveEmptyEntries).First();
            var symbolName = splitRow.Trim();
            _endState.Add(lineNumber, symbolName);
            if (symbolName.Contains('*'))
            {
                _regressionState.Add(lineNumber);
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
            for (var i = 0; csvReader.TryGetField(i, out string? value); i++)
                if (value != null)
                    row.Add(value);

            rows.Add(row.ToArray());
        }

        var matriz = new string[rows.Count, rows[0].Length];

        for (var i = 0; i < rows.Count; i++)
        {
            for (var j = 0; j < rows[i].Length; j++)
            {
                matriz[i, j] = rows[i][j];
            }
        }

        return matriz;
    }
    private string[] GetColumn(int columnNumber)
    {
        return Enumerable.Range(0, _transitionTable.GetLength(0))
                .Select(x => _transitionTable[x, columnNumber])
                .ToArray();
    }

    private string[] GetRow(int rowNumber)
    {
        return Enumerable.Range(0, _transitionTable.GetLength(1))
                .Select(x => _transitionTable[rowNumber, x])
                .ToArray();
    }
}


