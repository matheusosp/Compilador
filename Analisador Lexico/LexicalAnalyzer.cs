using System.Globalization;
using System.Text.RegularExpressions;
using Analisador_Lexico.Domain;
using CsvHelper;
using CsvHelper.Configuration;

namespace Analisador_Lexico;

public class LexicalAnalyzer
{
    private readonly List<int> _regressionStates;
    private readonly string[,] _transitionTable;
    private readonly Dictionary<int, string> _endState;
    private Dictionary<string, int> _symbolsIdentificators;
    private readonly string[] _reservedWords =
    {
            "int","float","char","boolean","void","if","else","for","while",
            "scanf","println","main","return","EOF"
    };

    
    public LexicalAnalyzer()
    {
        _transitionTable = LoadExcel(Path.Combine(Directory.GetCurrentDirectory(), "Tabela de transicoes.csv"));
        _endState = new Dictionary<int, string>();
        _regressionStates = new List<int>();
        AddSymbolsFromCsv();
        
        _symbolsIdentificators = new Dictionary<string, int>();
        TableSymbol.ClearTable();
        TableToken.ClearTable();
        TableErrors.ClearTable();
    }

    public (int,string) GetNextToken(string code, int index)
    {
        var lexema = "";
        var estado = 0;
        var token = string.Empty;
        while (index < code.Length)
        {
            var car = code[index];
            lexema += car;
            var coluna = GetColumn(car);
            int.TryParse(_transitionTable[estado +1, coluna], out estado);

            if (_endState.ContainsKey(estado))
            {
                if (_regressionStates.Contains(estado))
                {
                    index -= 1;
                    lexema = lexema.Substring(0, lexema.Length - 1).Trim();
                }

                token = AddLexemeToTable(estado, lexema);
                estado = 0;
                lexema = "";
                return (index+=1, token);
                
            }
            
            index += 1;
            
        }

        //if(_endState.ContainsKey(estado) == false && estado != 0)
        //    TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });

        return (0, token);
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
        else if (char.IsWhiteSpace(caractere) && caractere != '\r')
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
        var columnNumber = firstRow.IndexOf(stringCaractere);
        if(columnNumber == -1)
            columnNumber = firstRow.IndexOf("Outros");
        return columnNumber;
    }
    private string AddLexemeToTable(int state, string lexema)
    {
        if (_endState.GetValueOrDefault(state)!.ToUpper().Contains("ERRO"))
        {
            TableErrors.AddError(new Error { ErrorMessage = $"Error: {lexema}" });
            return string.Empty;
        }
        if (_endState.GetValueOrDefault(state)!.ToUpper().Contains("COMENTARIO"))
            return string.Empty;

        var tokenType = "UNDEFINED";
        if (_reservedWords.Contains(lexema)) 
        {
            tokenType = "PALAVRA RESERVADA";
            TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
            return lexema;
        }
        else if (_endState.ContainsKey(state)) 
        {
            tokenType = _endState.GetValueOrDefault(state);

            if (tokenType!.Contains("ID")) 
            {
                var symbolValue = AddSymbolToTableAndList(lexema);
                TableToken.AddToken(new Token { Id = state, Name = symbolValue, Type = tokenType });
                return tokenType.Replace("*","");
            }
            else if (tokenType!.Contains("num"))
            {
                var symbolValue = AddSymbolToTableAndList(lexema);
                TableToken.AddToken(new Token { Id = state, Name = symbolValue, Type = tokenType });
                return tokenType.Replace("*", "");
            }
            else
            {
                TableToken.AddToken(new Token { Id = state, Name = lexema, Type = tokenType });
                return lexema;
            }

            
        }

        return string.Empty;
    }

    private string AddSymbolToTableAndList(string lexema)
    {
        if (_symbolsIdentificators.TryGetValue(lexema, out var index))
            return $"ID, {index}";

        index = _symbolsIdentificators.Count;
        _symbolsIdentificators[lexema] = index;
        TableSymbol.AddSymbol(new Symbol { Id = index, Name = lexema });

        return $"ID, {index}";
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
            if (symbolName.Substring(symbolName.Length-1,1).Contains('*'))
            {
                _regressionStates.Add(lineNumber);
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

    private string[] GetRow(int rowNumber)
    {
        return Enumerable.Range(0, _transitionTable.GetLength(1))
                .Select(x => _transitionTable[rowNumber, x])
                .ToArray();
    }
}


