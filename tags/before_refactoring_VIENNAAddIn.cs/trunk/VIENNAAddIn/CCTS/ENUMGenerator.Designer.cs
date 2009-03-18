namespace VIENNAAddIn.CCTS
{
    partial class ENUMGenerator
    {
        /// <sUMM2ary>
        /// Erforderliche Designervariable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Verwendete Ressourcen bereinigen.
        /// </sUMM2ary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <sUMM2ary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </sUMM2ary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ENUMGenerator));
            this.cancelButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedEnumLibrary = new System.Windows.Forms.Label();
            this.annotateElementBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseAlias = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(235, 279);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(316, 279);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(124, 23);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate Schema";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(15, 128);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(425, 108);
            this.statusTextBox.TabIndex = 1;
            this.statusTextBox.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 246);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Selected ENUMLibrary:";
            // 
            // selectedEnumLibrary
            // 
            this.selectedEnumLibrary.AutoSize = true;
            this.selectedEnumLibrary.Location = new System.Drawing.Point(12, 31);
            this.selectedEnumLibrary.Name = "selectedEnumLibrary";
            this.selectedEnumLibrary.Size = new System.Drawing.Size(35, 13);
            this.selectedEnumLibrary.TabIndex = 6;
            this.selectedEnumLibrary.Text = "label3";
            // 
            // annotateElementBox
            // 
            this.annotateElementBox.AutoSize = true;
            this.annotateElementBox.Location = new System.Drawing.Point(9, 19);
            this.annotateElementBox.Name = "annotateElementBox";
            this.annotateElementBox.Size = new System.Drawing.Size(132, 17);
            this.annotateElementBox.TabIndex = 0;
            this.annotateElementBox.Text = "Annotate the elements";
            this.annotateElementBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkUseAlias);
            this.groupBox1.Controls.Add(this.annotateElementBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 70);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkUseAlias
            // 
            this.chkUseAlias.AutoSize = true;
            this.chkUseAlias.Location = new System.Drawing.Point(9, 42);
            this.chkUseAlias.Name = "chkUseAlias";
            this.chkUseAlias.Size = new System.Drawing.Size(142, 17);
            this.chkUseAlias.TabIndex = 24;
            this.chkUseAlias.Text = "Use alias for folder name";
            this.chkUseAlias.UseVisualStyleBackColor = true;
            // 
            // ENUMGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.selectedEnumLibrary);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.generateButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ENUMGenerator";
            this.Text = "EnumForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label selectedEnumLibrary;
        private System.Windows.Forms.CheckBox annotateElementBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseAlias;
    }
}