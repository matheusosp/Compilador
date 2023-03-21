namespace Analisador_Lexico;

public partial class Form1 : Form
{
    string _filePathCode = Path.Combine(Directory.GetCurrentDirectory(), "javaexample.txt");
    public Form1()
    {
        InitializeComponent();
        if (File.Exists(_filePathCode)) { 
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
            
            ClearGrids();
        }
        catch (Exception ex)
        {
            MessageBox.Show(@"Error: " + ex.Message);
        }

    }

    private void UpdateCode()
    {


        // Abra o arquivo existente
        using (StreamReader sr = new StreamReader(_filePathCode))
        {
            // Leia o conteúdo atual do arquivo
            string content = sr.ReadToEnd();

            // Feche o StreamReader
            sr.Close();

            // Abra o StreamWriter
            using (StreamWriter sw = new StreamWriter(_filePathCode))
            {
                // Escreva o novo conteúdo no arquivo
                sw.Write(textBoxCode?.Text);

                // Feche o StreamWriter
                sw.Close();
            }
        }
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
}
