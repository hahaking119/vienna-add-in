namespace VIENNAAddIn.XBRLGenerator
{
    partial class XBRLLinkbase
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
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.selectedDiagrmName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dlgSavePath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(237, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateSchema
            // 
            this.btnGenerateSchema.Location = new System.Drawing.Point(318, 275);
            this.btnGenerateSchema.Name = "btnGenerateSchema";
            this.btnGenerateSchema.Size = new System.Drawing.Size(121, 24);
            this.btnGenerateSchema.TabIndex = 27;
            this.btnGenerateSchema.Text = "Generate Schema";
            this.btnGenerateSchema.UseVisualStyleBackColor = true;
            this.btnGenerateSchema.Click += new System.EventHandler(this.btnGenerateSchema_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(15, 246);
            this.progressBar1.Maximum = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(424, 24);
            this.progressBar1.TabIndex = 26;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(15, 61);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(424, 179);
            this.statusTextBox.TabIndex = 25;
            this.statusTextBox.Text = "";
            // 
            // selectedDiagrmName
            // 
            this.selectedDiagrmName.AutoSize = true;
            this.selectedDiagrmName.Location = new System.Drawing.Point(12, 34);
            this.selectedDiagrmName.Name = "selectedDiagrmName";
            this.selectedDiagrmName.Size = new System.Drawing.Size(114, 13);
            this.selectedDiagrmName.TabIndex = 24;
            this.selectedDiagrmName.Text = "selectedDiagramName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Selected Diagram";
            // 
            // XBRLLinkbase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateSchema);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.selectedDiagrmName);
            this.Controls.Add(this.label1);
            this.Name = "XBRLLinkbase";
            this.Text = "XBRLLinkbase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateSchema;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.Label selectedDiagrmName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog dlgSavePath;

    }
}