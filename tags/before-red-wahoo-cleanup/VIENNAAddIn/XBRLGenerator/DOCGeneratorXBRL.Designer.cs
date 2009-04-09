namespace VIENNAAddIn.XBRLGenerator
{
    partial class DOCGeneratorXBRL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerateSchema = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dlgSavePath = new System.Windows.Forms.FolderBrowserDialog();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.selectedBIELibrary = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIncludeLinkedSchema = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(235, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateSchema
            // 
            this.btnGenerateSchema.Location = new System.Drawing.Point(316, 276);
            this.btnGenerateSchema.Name = "btnGenerateSchema";
            this.btnGenerateSchema.Size = new System.Drawing.Size(124, 24);
            this.btnGenerateSchema.TabIndex = 26;
            this.btnGenerateSchema.Text = "Generate Schema";
            this.btnGenerateSchema.UseVisualStyleBackColor = true;
            this.btnGenerateSchema.Click += new System.EventHandler(this.btnGenerateSchema_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(18, 237);
            this.progressBar1.Maximum = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 24);
            this.progressBar1.TabIndex = 25;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(17, 107);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(425, 124);
            this.statusTextBox.TabIndex = 24;
            this.statusTextBox.Text = "";
            // 
            // selectedBIELibrary
            // 
            this.selectedBIELibrary.AutoSize = true;
            this.selectedBIELibrary.Location = new System.Drawing.Point(12, 32);
            this.selectedBIELibrary.Name = "selectedBIELibrary";
            this.selectedBIELibrary.Size = new System.Drawing.Size(123, 13);
            this.selectedBIELibrary.TabIndex = 23;
            this.selectedBIELibrary.Text = "selectedBIELibraryName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Selected BIELibrary:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIncludeLinkedSchema);
            this.groupBox1.Location = new System.Drawing.Point(15, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 53);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkIncludeLinkedSchema
            // 
            this.chkIncludeLinkedSchema.AutoSize = true;
            this.chkIncludeLinkedSchema.Location = new System.Drawing.Point(6, 19);
            this.chkIncludeLinkedSchema.Name = "chkIncludeLinkedSchema";
            this.chkIncludeLinkedSchema.Size = new System.Drawing.Size(132, 17);
            this.chkIncludeLinkedSchema.TabIndex = 1;
            this.chkIncludeLinkedSchema.Text = "Include linked schema";
            this.chkIncludeLinkedSchema.UseVisualStyleBackColor = true;
            // 
            // DOCGeneratorXBRL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateSchema);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.selectedBIELibrary);
            this.Controls.Add(this.label1);
            this.Name = "DOCGeneratorXBRL";
            this.Text = "XBRL Generator for DOCLibrary";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkIncludeLinkedSchema;
        
    }
}