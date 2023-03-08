namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var initialPathToExampleJava = Path.Combine(Directory.GetCurrentDirectory(),"javaexample.txt");
            if (File.Exists(initialPathToExampleJava)) { 
                textBoxCode.Text = File.ReadAllText(initialPathToExampleJava);
            }
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            var arquivo = new OpenFileDialog();

            arquivo.InitialDirectory = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            arquivo.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            arquivo.FilterIndex = 2;
            arquivo.RestoreDirectory = true;

            if (arquivo.ShowDialog() != DialogResult.OK) return;
            try
            {
                textBoxCode.Text = File.ReadAllText(arquivo.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        private void btnCompilar_Click(object sender, EventArgs e)
        {
            ClearGrids();
        }
        private void ClearGrids()
        {
            dataGridViewTableSymbol.DataSource = null;
            dataGridViewTableTokens.DataSource = null;
            dataGridViewTableSymbol.Rows.Clear();
            dataGridViewTableTokens.Rows.Clear();
            dataGridViewTableSymbol.Refresh();
            dataGridViewTableTokens.Refresh();
        }
    }
}