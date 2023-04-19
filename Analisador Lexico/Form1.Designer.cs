namespace Analisador_Lexico
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelTokens = new Label();
            textBoxCode = new TextBox();
            btnOpenFileDialog = new Button();
            btnCompilar = new Button();
            tabControl1 = new TabControl();
            tabPageLexical = new TabPage();
            label2 = new Label();
            dataGridViewTableSymbol = new DataGridView();
            dataGridViewTableTokens = new DataGridView();
            labelSymbols = new Label();
            label1 = new Label();
            dataGridViewTableErrors = new DataGridView();
            tabPageSyntactic = new TabPage();
            tabControl2 = new TabControl();
            TabPageTest = new TabPage();
            dataGridParserTrace = new DataGridView();
            State = new DataGridViewTextBoxColumn();
            NewState = new DataGridViewTextBoxColumn();
            Input = new DataGridViewTextBoxColumn();
            Action = new DataGridViewTextBoxColumn();
            labelParserTrace = new Label();
            tabPageParserStates = new TabPage();
            tbParserStates = new TextBox();
            tabPageNonTerminals = new TabPage();
            tbNonTerminals = new TextBox();
            tabControl1.SuspendLayout();
            tabPageLexical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableSymbol).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableTokens).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableErrors).BeginInit();
            tabPageSyntactic.SuspendLayout();
            tabControl2.SuspendLayout();
            TabPageTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridParserTrace).BeginInit();
            tabPageParserStates.SuspendLayout();
            tabPageNonTerminals.SuspendLayout();
            SuspendLayout();
            // 
            // labelTokens
            // 
            labelTokens.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            labelTokens.AutoSize = true;
            labelTokens.Location = new Point(333, -144);
            labelTokens.Margin = new Padding(4, 0, 4, 0);
            labelTokens.Name = "labelTokens";
            labelTokens.Size = new Size(97, 15);
            labelTokens.TabIndex = 12;
            labelTokens.Text = "Tabela de tokens:";
            // 
            // textBoxCode
            // 
            textBoxCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxCode.Font = new Font("Courier New", 18F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxCode.Location = new Point(23, 45);
            textBoxCode.Margin = new Padding(4, 3, 4, 3);
            textBoxCode.Multiline = true;
            textBoxCode.Name = "textBoxCode";
            textBoxCode.Size = new Size(733, 275);
            textBoxCode.TabIndex = 8;
            // 
            // btnOpenFileDialog
            // 
            btnOpenFileDialog.Location = new Point(117, 12);
            btnOpenFileDialog.Name = "btnOpenFileDialog";
            btnOpenFileDialog.Size = new Size(99, 27);
            btnOpenFileDialog.TabIndex = 14;
            btnOpenFileDialog.Text = "Abrir arquivo";
            btnOpenFileDialog.UseVisualStyleBackColor = true;
            btnOpenFileDialog.Click += btnOpenFileDialog_Click;
            // 
            // btnCompilar
            // 
            btnCompilar.Location = new Point(23, 12);
            btnCompilar.Name = "btnCompilar";
            btnCompilar.Size = new Size(88, 27);
            btnCompilar.TabIndex = 15;
            btnCompilar.Text = "Compilar";
            btnCompilar.UseVisualStyleBackColor = true;
            btnCompilar.Click += btnCompilar_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageLexical);
            tabControl1.Controls.Add(tabPageSyntactic);
            tabControl1.Location = new Point(23, 326);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1042, 549);
            tabControl1.TabIndex = 18;
            // 
            // tabPageLexical
            // 
            tabPageLexical.Controls.Add(label2);
            tabPageLexical.Controls.Add(dataGridViewTableSymbol);
            tabPageLexical.Controls.Add(dataGridViewTableTokens);
            tabPageLexical.Controls.Add(labelSymbols);
            tabPageLexical.Controls.Add(label1);
            tabPageLexical.Controls.Add(dataGridViewTableErrors);
            tabPageLexical.Location = new Point(4, 24);
            tabPageLexical.Name = "tabPageLexical";
            tabPageLexical.Padding = new Padding(3);
            tabPageLexical.Size = new Size(1034, 521);
            tabPageLexical.TabIndex = 0;
            tabPageLexical.Text = "Analizador Lexico";
            tabPageLexical.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(468, 14);
            label2.Name = "label2";
            label2.Size = new Size(97, 15);
            label2.TabIndex = 18;
            label2.Text = "Tabela de tokens:";
            // 
            // dataGridViewTableSymbol
            // 
            dataGridViewTableSymbol.AllowUserToAddRows = false;
            dataGridViewTableSymbol.AllowUserToDeleteRows = false;
            dataGridViewTableSymbol.AllowUserToResizeColumns = false;
            dataGridViewTableSymbol.AllowUserToResizeRows = false;
            dataGridViewTableSymbol.Anchor = AnchorStyles.Top;
            dataGridViewTableSymbol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewTableSymbol.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTableSymbol.Location = new Point(6, 32);
            dataGridViewTableSymbol.Name = "dataGridViewTableSymbol";
            dataGridViewTableSymbol.ReadOnly = true;
            dataGridViewTableSymbol.Size = new Size(435, 230);
            dataGridViewTableSymbol.TabIndex = 9;
            // 
            // dataGridViewTableTokens
            // 
            dataGridViewTableTokens.AllowUserToAddRows = false;
            dataGridViewTableTokens.AllowUserToDeleteRows = false;
            dataGridViewTableTokens.AllowUserToResizeColumns = false;
            dataGridViewTableTokens.AllowUserToResizeRows = false;
            dataGridViewTableTokens.Anchor = AnchorStyles.Top;
            dataGridViewTableTokens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewTableTokens.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTableTokens.Location = new Point(468, 35);
            dataGridViewTableTokens.Name = "dataGridViewTableTokens";
            dataGridViewTableTokens.ReadOnly = true;
            dataGridViewTableTokens.Size = new Size(435, 431);
            dataGridViewTableTokens.TabIndex = 11;
            // 
            // labelSymbols
            // 
            labelSymbols.Anchor = AnchorStyles.Top;
            labelSymbols.AutoSize = true;
            labelSymbols.Location = new Point(6, 14);
            labelSymbols.Name = "labelSymbols";
            labelSymbols.Size = new Size(110, 15);
            labelSymbols.TabIndex = 10;
            labelSymbols.Text = "Tabela de simbolos:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(6, 275);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 17;
            label1.Text = "Tabela de erros:";
            label1.Click += label1_Click;
            // 
            // dataGridViewTableErrors
            // 
            dataGridViewTableErrors.AllowUserToAddRows = false;
            dataGridViewTableErrors.AllowUserToDeleteRows = false;
            dataGridViewTableErrors.AllowUserToResizeColumns = false;
            dataGridViewTableErrors.AllowUserToResizeRows = false;
            dataGridViewTableErrors.Anchor = AnchorStyles.Top;
            dataGridViewTableErrors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewTableErrors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewTableErrors.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewTableErrors.Location = new Point(6, 297);
            dataGridViewTableErrors.Name = "dataGridViewTableErrors";
            dataGridViewTableErrors.ReadOnly = true;
            dataGridViewTableErrors.Size = new Size(435, 169);
            dataGridViewTableErrors.TabIndex = 16;
            // 
            // tabPageSyntactic
            // 
            tabPageSyntactic.Controls.Add(tabControl2);
            tabPageSyntactic.Controls.Add(labelTokens);
            tabPageSyntactic.Location = new Point(4, 24);
            tabPageSyntactic.Name = "tabPageSyntactic";
            tabPageSyntactic.Padding = new Padding(3);
            tabPageSyntactic.Size = new Size(1034, 521);
            tabPageSyntactic.TabIndex = 1;
            tabPageSyntactic.Text = "Analizador Sintatico";
            tabPageSyntactic.UseVisualStyleBackColor = true;
            tabPageSyntactic.Click += tabPageSyntactic_Click;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(TabPageTest);
            tabControl2.Controls.Add(tabPageParserStates);
            tabControl2.Controls.Add(tabPageNonTerminals);
            tabControl2.Location = new Point(6, 6);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(1025, 500);
            tabControl2.TabIndex = 17;
            // 
            // TabPageTest
            // 
            TabPageTest.Controls.Add(dataGridParserTrace);
            TabPageTest.Controls.Add(labelParserTrace);
            TabPageTest.Location = new Point(4, 24);
            TabPageTest.Name = "TabPageTest";
            TabPageTest.Padding = new Padding(3);
            TabPageTest.Size = new Size(1017, 472);
            TabPageTest.TabIndex = 0;
            TabPageTest.Text = "Test";
            TabPageTest.UseVisualStyleBackColor = true;
            // 
            // dataGridParserTrace
            // 
            dataGridParserTrace.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridParserTrace.Columns.AddRange(new DataGridViewColumn[] { State, NewState, Input, Action });
            dataGridParserTrace.Location = new Point(6, 29);
            dataGridParserTrace.Name = "dataGridParserTrace";
            dataGridParserTrace.RowTemplate.Height = 25;
            dataGridParserTrace.ScrollBars = ScrollBars.Vertical;
            dataGridParserTrace.Size = new Size(891, 406);
            dataGridParserTrace.TabIndex = 14;
            // 
            // State
            // 
            State.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            State.HeaderText = "State";
            State.Name = "State";
            // 
            // NewState
            // 
            NewState.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NewState.HeaderText = "New State";
            NewState.Name = "NewState";
            // 
            // Input
            // 
            Input.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Input.HeaderText = "Input";
            Input.Name = "Input";
            // 
            // Action
            // 
            Action.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Action.HeaderText = "Action";
            Action.Name = "Action";
            // 
            // labelParserTrace
            // 
            labelParserTrace.AutoSize = true;
            labelParserTrace.Location = new Point(6, 11);
            labelParserTrace.Name = "labelParserTrace";
            labelParserTrace.Size = new Size(69, 15);
            labelParserTrace.TabIndex = 13;
            labelParserTrace.Text = "Parser Trace";
            // 
            // tabPageParserStates
            // 
            tabPageParserStates.Controls.Add(tbParserStates);
            tabPageParserStates.Location = new Point(4, 24);
            tabPageParserStates.Name = "tabPageParserStates";
            tabPageParserStates.Padding = new Padding(3);
            tabPageParserStates.Size = new Size(1017, 472);
            tabPageParserStates.TabIndex = 1;
            tabPageParserStates.Text = "Parser States";
            tabPageParserStates.UseVisualStyleBackColor = true;
            // 
            // tbParserStates
            // 
            tbParserStates.Location = new Point(7, 14);
            tbParserStates.Multiline = true;
            tbParserStates.Name = "tbParserStates";
            tbParserStates.ScrollBars = ScrollBars.Vertical;
            tbParserStates.Size = new Size(849, 440);
            tbParserStates.TabIndex = 0;
            // 
            // tabPageNonTerminals
            // 
            tabPageNonTerminals.Controls.Add(tbNonTerminals);
            tabPageNonTerminals.Location = new Point(4, 24);
            tabPageNonTerminals.Name = "tabPageNonTerminals";
            tabPageNonTerminals.Size = new Size(1017, 472);
            tabPageNonTerminals.TabIndex = 2;
            tabPageNonTerminals.Text = "Non-Terminals";
            tabPageNonTerminals.UseVisualStyleBackColor = true;
            // 
            // tbNonTerminals
            // 
            tbNonTerminals.Location = new Point(14, 17);
            tbNonTerminals.Multiline = true;
            tbNonTerminals.Name = "tbNonTerminals";
            tbNonTerminals.ScrollBars = ScrollBars.Vertical;
            tbNonTerminals.Size = new Size(849, 440);
            tbNonTerminals.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 868);
            Controls.Add(textBoxCode);
            Controls.Add(btnCompilar);
            Controls.Add(btnOpenFileDialog);
            Controls.Add(tabControl1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            tabPageLexical.ResumeLayout(false);
            tabPageLexical.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableSymbol).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableTokens).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableErrors).EndInit();
            tabPageSyntactic.ResumeLayout(false);
            tabPageSyntactic.PerformLayout();
            tabControl2.ResumeLayout(false);
            TabPageTest.ResumeLayout(false);
            TabPageTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridParserTrace).EndInit();
            tabPageParserStates.ResumeLayout(false);
            tabPageParserStates.PerformLayout();
            tabPageNonTerminals.ResumeLayout(false);
            tabPageNonTerminals.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTokens;
        private TextBox textBoxCode;
        private Button btnOpenFileDialog;
        private Button btnCompilar;
        private TabControl tabControl1;
        private TabPage tabPageLexical;
        private TabPage tabPageSyntactic;
        private Label label2;
        private DataGridView dataGridViewTableSymbol;
        private DataGridView dataGridViewTableTokens;
        private Label labelSymbols;
        private Label label1;
        private DataGridView dataGridViewTableErrors;
        private DataGridView dataGridParserTrace;
        private Label labelParserTrace;
        private TabControl tabControl2;
        private TabPage TabPageTest;
        private TabPage tabPageParserStates;
        private TextBox tbParserStates;
        private TabPage tabPageNonTerminals;
        private TextBox tbNonTerminals;
        private DataGridViewTextBoxColumn State;
        private DataGridViewTextBoxColumn NewState;
        private DataGridViewTextBoxColumn Input;
        private DataGridViewTextBoxColumn Action;
    }
}