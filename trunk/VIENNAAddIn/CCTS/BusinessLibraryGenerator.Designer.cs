namespace VIENNAAddIn.CCTS
{
    partial class BusinessLibraryGenerator
    {
        /// <sUMM2ary>
        /// Required designer variable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Clean up any resources being used.
        /// </sUMM2ary>
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

        /// <sUMM2ary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </sUMM2ary>
        private void InitializeComponent()
        {
            this.chkIncludeLinkedSchema = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkNillable = new System.Windows.Forms.CheckBox();
            this.chkUseAlias = new System.Windows.Forms.CheckBox();
            this.chkAnnotateElement = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.selectedBIELibrary = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerateSchema = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIncludeLinkedSchema
            // 
            this.chkIncludeLinkedSchema.AutoSize = true;
            this.chkIncludeLinkedSchema.Location = new System.Drawing.Point(9, 88);
            this.chkIncludeLinkedSchema.Name = "chkIncludeLinkedSchema";
            this.chkIncludeLinkedSchema.Size = new System.Drawing.Size(132, 17);
            this.chkIncludeLinkedSchema.TabIndex = 1;
            this.chkIncludeLinkedSchema.Text = "Include linked schema";
            this.chkIncludeLinkedSchema.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkNillable);
            this.groupBox1.Controls.Add(this.chkUseAlias);
            this.groupBox1.Controls.Add(this.chkIncludeLinkedSchema);
            this.groupBox1.Controls.Add(this.chkAnnotateElement);
            this.groupBox1.Location = new System.Drawing.Point(9, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 136);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkNillable
            // 
            this.chkNillable.AutoSize = true;
            this.chkNillable.Location = new System.Drawing.Point(9, 42);
            this.chkNillable.Name = "chkNillable";
            this.chkNillable.Size = new System.Drawing.Size(314, 17);
            this.chkNillable.TabIndex = 29;
            this.chkNillable.Text = "Add attribute Nillable=”true” to BBIEs with a cardinality of 0..X";
            this.chkNillable.UseVisualStyleBackColor = true;
            this.chkNillable.CheckedChanged += new System.EventHandler(this.chkNillable_CheckedChanged);
            // 
            // chkUseAlias
            // 
            this.chkUseAlias.AutoSize = true;
            this.chkUseAlias.Location = new System.Drawing.Point(9, 111);
            this.chkUseAlias.Name = "chkUseAlias";
            this.chkUseAlias.Size = new System.Drawing.Size(142, 17);
            this.chkUseAlias.TabIndex = 28;
            this.chkUseAlias.Text = "Use alias for folder name";
            this.chkUseAlias.UseVisualStyleBackColor = true;
            // 
            // chkAnnotateElement
            // 
            this.chkAnnotateElement.AutoSize = true;
            this.chkAnnotateElement.Location = new System.Drawing.Point(9, 19);
            this.chkAnnotateElement.Name = "chkAnnotateElement";
            this.chkAnnotateElement.Size = new System.Drawing.Size(132, 17);
            this.chkAnnotateElement.TabIndex = 0;
            this.chkAnnotateElement.Text = "Annotate the elements";
            this.chkAnnotateElement.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 298);
            this.progressBar1.Maximum = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 24);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 23;
            this.progressBar1.UseWaitCursor = true;
            // 
            // selectedBIELibrary
            // 
            this.selectedBIELibrary.AutoSize = true;
            this.selectedBIELibrary.Location = new System.Drawing.Point(9, 24);
            this.selectedBIELibrary.Name = "selectedBIELibrary";
            this.selectedBIELibrary.Size = new System.Drawing.Size(35, 13);
            this.selectedBIELibrary.TabIndex = 22;
            this.selectedBIELibrary.Text = "label2";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(232, 335);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateSchema
            // 
            this.btnGenerateSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateSchema.Location = new System.Drawing.Point(313, 335);
            this.btnGenerateSchema.Name = "btnGenerateSchema";
            this.btnGenerateSchema.Size = new System.Drawing.Size(124, 24);
            this.btnGenerateSchema.TabIndex = 25;
            this.btnGenerateSchema.Text = "Generate Schema";
            this.btnGenerateSchema.UseVisualStyleBackColor = true;
            this.btnGenerateSchema.Click += new System.EventHandler(this.btnGenerateSchema_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(12, 182);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(425, 103);
            this.statusTextBox.TabIndex = 24;
            this.statusTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Selected BusinessLibrary:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "(Valid only for CCLibrary, BIELibrary, DOCLibrary)";
            // 
            // BusinessLibraryGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 372);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectedBIELibrary);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateSchema);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.label1);
            this.Name = "BusinessLibraryGenerator";
            this.Text = "BusinessLibraryGenerator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIncludeLinkedSchema;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAnnotateElement;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label selectedBIELibrary;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateSchema;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox chkUseAlias;
        private System.Windows.Forms.CheckBox chkNillable;
        private System.Windows.Forms.Label label2;
    }
}