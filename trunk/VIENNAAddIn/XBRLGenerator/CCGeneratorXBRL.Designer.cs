namespace VIENNAAddIn.XBRLGenerator
{
    partial class CCGeneratorXBRL
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
            this.btnGenerateSchema = new System.Windows.Forms.Button();
            this.annotateElementBox = new System.Windows.Forms.CheckBox();
            this.dlgSavePath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSelectedCClibrary = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerateSchema
            // 
            this.btnGenerateSchema.Location = new System.Drawing.Point(318, 272);
            this.btnGenerateSchema.Name = "btnGenerateSchema";
            this.btnGenerateSchema.Size = new System.Drawing.Size(124, 23);
            this.btnGenerateSchema.TabIndex = 29;
            this.btnGenerateSchema.Text = "Generate Schema";
            this.btnGenerateSchema.UseVisualStyleBackColor = true;
            this.btnGenerateSchema.Click += new System.EventHandler(this.btnGenerateSchema_Click);
            // 
            // annotateElementBox
            // 
            this.annotateElementBox.AutoSize = true;
            this.annotateElementBox.Location = new System.Drawing.Point(6, 19);
            this.annotateElementBox.Name = "annotateElementBox";
            this.annotateElementBox.Size = new System.Drawing.Size(132, 17);
            this.annotateElementBox.TabIndex = 28;
            this.annotateElementBox.Text = "Annotate the elements";
            this.annotateElementBox.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(237, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 243);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 23);
            this.progressBar1.TabIndex = 31;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(15, 129);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(425, 108);
            this.statusTextBox.TabIndex = 32;
            this.statusTextBox.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.annotateElementBox);
            this.groupBox2.Location = new System.Drawing.Point(15, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 70);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Selected CCLibrary";
            // 
            // lblSelectedCClibrary
            // 
            this.lblSelectedCClibrary.AutoSize = true;
            this.lblSelectedCClibrary.Location = new System.Drawing.Point(15, 37);
            this.lblSelectedCClibrary.Name = "lblSelectedCClibrary";
            this.lblSelectedCClibrary.Size = new System.Drawing.Size(35, 13);
            this.lblSelectedCClibrary.TabIndex = 35;
            this.lblSelectedCClibrary.Text = "label3";
            // 
            // CCGeneratorXBRL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.lblSelectedCClibrary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnGenerateSchema);
            this.Controls.Add(this.btnCancel);
            this.Name = "CCGeneratorXBRL";
            this.Text = "CCGeneratorXBRL";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateSchema;
        private System.Windows.Forms.CheckBox annotateElementBox;
        private System.Windows.Forms.FolderBrowserDialog dlgSavePath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSelectedCClibrary;

    }
}