namespace Analizador_Lexico
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
            this.labelTokens = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.dataGridViewTableTokens = new System.Windows.Forms.DataGridView();
            this.labelSymbols = new System.Windows.Forms.Label();
            this.dataGridViewTableSymbol = new System.Windows.Forms.DataGridView();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.btnCompilar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableTokens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTokens
            // 
            this.labelTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTokens.AutoSize = true;
            this.labelTokens.Location = new System.Drawing.Point(680, 361);
            this.labelTokens.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTokens.Name = "labelTokens";
            this.labelTokens.Size = new System.Drawing.Size(97, 15);
            this.labelTokens.TabIndex = 12;
            this.labelTokens.Text = "Tabela de tokens:";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCode.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxCode.Location = new System.Drawing.Point(23, 74);
            this.textBoxCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxCode.MinimumSize = new System.Drawing.Size(470, 700);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(631, 700);
            this.textBoxCode.TabIndex = 8;
            // 
            // dataGridViewTableTokens
            // 
            this.dataGridViewTableTokens.AllowUserToAddRows = false;
            this.dataGridViewTableTokens.AllowUserToDeleteRows = false;
            this.dataGridViewTableTokens.AllowUserToResizeColumns = false;
            this.dataGridViewTableTokens.AllowUserToResizeRows = false;
            this.dataGridViewTableTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTableTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTableTokens.Location = new System.Drawing.Point(680, 379);
            this.dataGridViewTableTokens.Name = "dataGridViewTableTokens";
            this.dataGridViewTableTokens.ReadOnly = true;
            this.dataGridViewTableTokens.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTableTokens.Size = new System.Drawing.Size(435, 395);
            this.dataGridViewTableTokens.TabIndex = 11;
            // 
            // labelSymbols
            // 
            this.labelSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSymbols.AutoSize = true;
            this.labelSymbols.Location = new System.Drawing.Point(680, 46);
            this.labelSymbols.Name = "labelSymbols";
            this.labelSymbols.Size = new System.Drawing.Size(110, 15);
            this.labelSymbols.TabIndex = 10;
            this.labelSymbols.Text = "Tabela de simbolos:";
            // 
            // dataGridViewTableSymbol
            // 
            this.dataGridViewTableSymbol.AllowUserToAddRows = false;
            this.dataGridViewTableSymbol.AllowUserToDeleteRows = false;
            this.dataGridViewTableSymbol.AllowUserToResizeColumns = false;
            this.dataGridViewTableSymbol.AllowUserToResizeRows = false;
            this.dataGridViewTableSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTableSymbol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTableSymbol.Location = new System.Drawing.Point(680, 74);
            this.dataGridViewTableSymbol.Name = "dataGridViewTableSymbol";
            this.dataGridViewTableSymbol.ReadOnly = true;
            this.dataGridViewTableSymbol.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTableSymbol.Size = new System.Drawing.Size(435, 265);
            this.dataGridViewTableSymbol.TabIndex = 9;
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Location = new System.Drawing.Point(117, 34);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(99, 27);
            this.btnOpenFileDialog.TabIndex = 14;
            this.btnOpenFileDialog.Text = "Abrir arquivo";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // btnCompilar
            // 
            this.btnCompilar.Location = new System.Drawing.Point(23, 34);
            this.btnCompilar.Name = "btnCompilar";
            this.btnCompilar.Size = new System.Drawing.Size(88, 27);
            this.btnCompilar.TabIndex = 15;
            this.btnCompilar.Text = "Compilar";
            this.btnCompilar.UseVisualStyleBackColor = true;
            this.btnCompilar.Click += new System.EventHandler(this.btnCompilar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 823);
            this.Controls.Add(this.btnCompilar);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Controls.Add(this.labelTokens);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.dataGridViewTableTokens);
            this.Controls.Add(this.labelSymbols);
            this.Controls.Add(this.dataGridViewTableSymbol);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableTokens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTableSymbol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelTokens;
        private TextBox textBoxCode;
        private DataGridView dataGridViewTableTokens;
        private Label labelSymbols;
        private DataGridView dataGridViewTableSymbol;
        private Button btnOpenFileDialog;
        private Button btnCompilar;
    }
}