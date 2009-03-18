namespace VIENNAAddIn.CCTS {
    partial class QDTGenerator {
        /// <sUMM2ary>
        /// Erforderliche Designervariable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Verwendete Ressourcen bereinigen.
        /// </sUMM2ary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <sUMM2ary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </sUMM2ary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDTGenerator));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.selectedQDTLibrary = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.annotateElementBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIncludeLinkedSchema = new System.Windows.Forms.CheckBox();
            this.chkUseAlias = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(235, 277);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Generate Schema";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnGenerateSchema_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(15, 136);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(425, 108);
            this.statusTextBox.TabIndex = 10;
            this.statusTextBox.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 250);
            this.progressBar1.Maximum = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // selectedQDTLibrary
            // 
            this.selectedQDTLibrary.AutoSize = true;
            this.selectedQDTLibrary.Location = new System.Drawing.Point(12, 32);
            this.selectedQDTLibrary.Name = "selectedQDTLibrary";
            this.selectedQDTLibrary.Size = new System.Drawing.Size(35, 13);
            this.selectedQDTLibrary.TabIndex = 8;
            this.selectedQDTLibrary.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Selected QDTLibrary:";
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
            this.groupBox1.Controls.Add(this.chkIncludeLinkedSchema);
            this.groupBox1.Controls.Add(this.annotateElementBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 70);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkIncludeLinkedSchema
            // 
            this.chkIncludeLinkedSchema.AutoSize = true;
            this.chkIncludeLinkedSchema.Location = new System.Drawing.Point(9, 42);
            this.chkIncludeLinkedSchema.Name = "chkIncludeLinkedSchema";
            this.chkIncludeLinkedSchema.Size = new System.Drawing.Size(132, 17);
            this.chkIncludeLinkedSchema.TabIndex = 2;
            this.chkIncludeLinkedSchema.Text = "Include linked schema";
            this.chkIncludeLinkedSchema.UseVisualStyleBackColor = true;
            // 
            // chkUseAlias
            // 
            this.chkUseAlias.AutoSize = true;
            this.chkUseAlias.Location = new System.Drawing.Point(202, 19);
            this.chkUseAlias.Name = "chkUseAlias";
            this.chkUseAlias.Size = new System.Drawing.Size(142, 17);
            this.chkUseAlias.TabIndex = 23;
            this.chkUseAlias.Text = "Use alias for folder name";
            this.chkUseAlias.UseVisualStyleBackColor = true;
            // 
            // QDTGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectedQDTLibrary);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QDTGenerator";
            this.Text = "QDTGenerator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label selectedQDTLibrary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox annotateElementBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkIncludeLinkedSchema;
        private System.Windows.Forms.CheckBox chkUseAlias;
    }
}