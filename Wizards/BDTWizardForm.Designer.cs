namespace Wizards
{
    partial class BDTWizardForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBDTLibraries = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBDTPrefix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAttributes = new System.Windows.Forms.TabPage();
            this.checkedlistboxAttributes = new System.Windows.Forms.CheckedListBox();
            this.checkboxAttributes = new System.Windows.Forms.CheckBox();
            this.comboCDTs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboCDTLibraries = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonGenerateBDT = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabAttributes.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBDTLibraries);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBDTPrefix);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.comboCDTs);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboCDTLibraries);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 462);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // comboBDTLibraries
            // 
            this.comboBDTLibraries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBDTLibraries.FormattingEnabled = true;
            this.comboBDTLibraries.Location = new System.Drawing.Point(215, 426);
            this.comboBDTLibraries.Name = "comboBDTLibraries";
            this.comboBDTLibraries.Size = new System.Drawing.Size(202, 21);
            this.comboBDTLibraries.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 429);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "BDT Library used to store the BDT:";
            // 
            // textBDTPrefix
            // 
            this.textBDTPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBDTPrefix.Location = new System.Drawing.Point(215, 400);
            this.textBDTPrefix.Name = "textBDTPrefix";
            this.textBDTPrefix.Size = new System.Drawing.Size(202, 20);
            this.textBDTPrefix.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Prefix used for the generated BDT:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabAttributes);
            this.tabControl1.Location = new System.Drawing.Point(16, 74);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(401, 310);
            this.tabControl1.TabIndex = 5;
            // 
            // tabAttributes
            // 
            this.tabAttributes.Controls.Add(this.checkedlistboxAttributes);
            this.tabAttributes.Controls.Add(this.checkboxAttributes);
            this.tabAttributes.Location = new System.Drawing.Point(4, 22);
            this.tabAttributes.Name = "tabAttributes";
            this.tabAttributes.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttributes.Size = new System.Drawing.Size(393, 284);
            this.tabAttributes.TabIndex = 0;
            this.tabAttributes.Text = "Attributes";
            this.tabAttributes.UseVisualStyleBackColor = true;
            // 
            // checkedlistboxAttributes
            // 
            this.checkedlistboxAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxAttributes.FormattingEnabled = true;
            this.checkedlistboxAttributes.Location = new System.Drawing.Point(11, 28);
            this.checkedlistboxAttributes.Name = "checkedlistboxAttributes";
            this.checkedlistboxAttributes.Size = new System.Drawing.Size(369, 244);
            this.checkedlistboxAttributes.TabIndex = 1;
            // 
            // checkboxAttributes
            // 
            this.checkboxAttributes.AutoSize = true;
            this.checkboxAttributes.Location = new System.Drawing.Point(14, 9);
            this.checkboxAttributes.Name = "checkboxAttributes";
            this.checkboxAttributes.Size = new System.Drawing.Size(116, 17);
            this.checkboxAttributes.TabIndex = 0;
            this.checkboxAttributes.Text = "Select all Attributes";
            this.checkboxAttributes.UseVisualStyleBackColor = true;
            this.checkboxAttributes.CheckedChanged += new System.EventHandler(this.checkboxAttributes_CheckedChanged);
            // 
            // comboCDTs
            // 
            this.comboCDTs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCDTs.FormattingEnabled = true;
            this.comboCDTs.Location = new System.Drawing.Point(216, 47);
            this.comboCDTs.Name = "comboCDTs";
            this.comboCDTs.Size = new System.Drawing.Size(201, 21);
            this.comboCDTs.TabIndex = 4;
            this.comboCDTs.SelectedIndexChanged += new System.EventHandler(this.comboCDTs_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Choose CDT used to generate the BDT:";
            // 
            // comboCDTLibraries
            // 
            this.comboCDTLibraries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCDTLibraries.FormattingEnabled = true;
            this.comboCDTLibraries.Location = new System.Drawing.Point(215, 20);
            this.comboCDTLibraries.Name = "comboCDTLibraries";
            this.comboCDTLibraries.Size = new System.Drawing.Size(202, 21);
            this.comboCDTLibraries.TabIndex = 2;
            this.comboCDTLibraries.SelectedIndexChanged += new System.EventHandler(this.comboCDTLibraries_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose CDT Library containing CDTs:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Location = new System.Drawing.Point(12, 494);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(433, 45);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(16, 19);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(401, 13);
            this.progressBar1.TabIndex = 0;
            // 
            // buttonGenerateBDT
            // 
            this.buttonGenerateBDT.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonGenerateBDT.Location = new System.Drawing.Point(88, 553);
            this.buttonGenerateBDT.Name = "buttonGenerateBDT";
            this.buttonGenerateBDT.Size = new System.Drawing.Size(101, 22);
            this.buttonGenerateBDT.TabIndex = 3;
            this.buttonGenerateBDT.Text = "&Generate BDT ...";
            this.buttonGenerateBDT.UseVisualStyleBackColor = true;
            this.buttonGenerateBDT.Click += new System.EventHandler(this.buttonGenerateBDT_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(272, 553);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(101, 22);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // BDTWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 586);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerateBDT);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "BDTWizardForm";
            this.Text = "Business Data Type (BDT) Wizard";
            this.Load += new System.EventHandler(this.BDTWizardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabAttributes.ResumeLayout(false);
            this.tabAttributes.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboCDTLibraries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboCDTs;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAttributes;
        private System.Windows.Forms.CheckBox checkboxAttributes;
        private System.Windows.Forms.CheckedListBox checkedlistboxAttributes;
        private System.Windows.Forms.TextBox textBDTPrefix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonGenerateBDT;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox comboBDTLibraries;
        private System.Windows.Forms.Label label4;
    }
}