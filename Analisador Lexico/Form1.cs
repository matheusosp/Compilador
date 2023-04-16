using Analisador_Lexico.Irony.Parsing.Data;
using Analisador_Lexico.Irony.Parsing.Grammar;
using Analisador_Lexico.Irony.Parsing.Parser;
using Analisador_Lexico.Irony.Parsing.Scanner;
using Analisador_Lexico.TestGrammars;
using CsvHelper;

namespace Analisador_Lexico;

public partial class Form1 : Form
{
    private string _filePathCode = Path.Combine(Directory.GetCurrentDirectory(), "javaexample.txt");
    public Form1()
    {
        InitializeComponent();
        if (File.Exists(_filePathCode))
        {
            textBoxCode.Text = File.ReadAllText(_filePathCode);
        }
    }

    private void btnCompilar_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateCode();
            var lexicalAnalyzer = new LexicalAnalyzer();
            lexicalAnalyzer.AnalyzeCode(_filePathCode);

            var syntacticAnalyzer = new SyntacticAnalyzer();
            //syntacticAnalyzer.AnalyzeCode(TableToken.Table);
            var grammar = new SimpleCGrammar();
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            parser.Context.TracingEnabled = true;

            tbNonTerminals.Text = ParserDataPrinter.PrintNonTerminals(language);
            tbParserStates.Text = ParserDataPrinter.PrintStateList(language);

            parser.Parse(textBoxCode.Text, "<source>");
            ShowParseTrace(parser);
            ShowCompilerErrors(parser);
            ClearGrids();
        }
        catch (Exception ex)
        {
            MessageBox.Show(@"Error: " + ex.Message);
        }

    }
    private void ShowCompilerErrors(Parser parser)
    {
        dataGridParserOutput.Rows.Clear();
        if (parser.Context.CurrentParseTree.ParserMessages.Count == 0) return;
        foreach (var err in parser.Context.CurrentParseTree.ParserMessages)
            dataGridParserOutput.Rows.Add(err.Location, err, err.ParserState);
    }
    private void ShowParseTrace(Parser parser)
    {
        dataGridParserTrace.Rows.Clear();
        foreach (var entry in parser.Context.ParserTrace)
        {
            var index = dataGridParserTrace.Rows.Add(entry.State, entry.StackTop, entry.Input, entry.Message);
            if (entry.IsError)
                dataGridParserTrace.Rows[^1].DefaultCellStyle.ForeColor = Color.Red;
        }

    }
    private void UpdateCode()
    {
        using var sw = new StreamWriter(_filePathCode);
        sw.Write(textBoxCode?.Text);

        sw.Close();
    }

    private void btnOpenFileDialog_Click(object sender, EventArgs e)
    {
        var file = new OpenFileDialog();

        file.InitialDirectory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
        file.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
        file.FilterIndex = 2;
        file.RestoreDirectory = true;

        if (file.ShowDialog() != DialogResult.OK) return;
        try
        {
            textBoxCode.Text = File.ReadAllText(file.FileName);
            _filePathCode = file.FileName;
        }
        catch (Exception ex)
        {
            MessageBox.Show(@"Error: Could not read file from disk. Original error: " + ex.Message);
        }
    }
    private void ClearGrids()
    {
        dataGridViewTableSymbol.DataSource = null;
        dataGridViewTableTokens.DataSource = null;
        dataGridViewTableErrors.DataSource = null;
        dataGridViewTableSymbol.Rows.Clear();
        dataGridViewTableTokens.Rows.Clear();
        dataGridViewTableErrors.Rows.Clear();
        dataGridViewTableSymbol.DataSource = TableSymbol.Table;
        dataGridViewTableTokens.DataSource = TableToken.Table;
        dataGridViewTableErrors.DataSource = TableErrors.Table;

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void tabPageSyntactic_Click(object sender, EventArgs e)
    {

    }
}
