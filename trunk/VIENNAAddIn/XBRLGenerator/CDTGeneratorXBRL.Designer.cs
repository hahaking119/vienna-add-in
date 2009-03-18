namespace VIENNAAddIn.XBRLGenerator
{
    partial class CDTGeneratorXBRL
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

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox annotateElementBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label selectedBIELibrary;
        private System.Windows.Forms.Label label1;

        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.annotateElementBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.selectedBIELibrary = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.annotateElementBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 70);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(235, 276);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Generate Schema";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(15, 124);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(425, 108);
            this.statusTextBox.TabIndex = 25;
            this.statusTextBox.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 242);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 23);
            this.progressBar1.TabIndex = 24;
            // 
            // selectedBIELibrary
            // 
            this.selectedBIELibrary.AutoSize = true;
            this.selectedBIELibrary.Location = new System.Drawing.Point(12, 32);
            this.selectedBIELibrary.Name = "selectedBIELibrary";
            this.selectedBIELibrary.Size = new System.Drawing.Size(35, 13);
            this.selectedBIELibrary.TabIndex = 23;
            this.selectedBIELibrary.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Selected CDTLibrary:";
            // 
            // CDTGeneratorXBRL
            // 
            this.ClientSize = new System.Drawing.Size(452, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectedBIELibrary);
            this.Controls.Add(this.label1);
            this.Name = "CDTGeneratorXBRL";
            this.Text = "CDTGeneratorXBRL";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}