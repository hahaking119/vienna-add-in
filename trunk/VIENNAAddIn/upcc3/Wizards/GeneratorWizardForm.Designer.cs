﻿namespace VIENNAAddIn.upcc3.Wizards
{
    partial class GeneratorWizardForm
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboModels = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textPrefixTargetNS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkedlistboxDOCs = new System.Windows.Forms.CheckedListBox();
            this.textTargetNS = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBIVs = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(9, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 132);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(14, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(507, 99);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.comboModels);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.textPrefixTargetNS);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.checkedlistboxDOCs);
            this.groupBox4.Controls.Add(this.textTargetNS);
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(12, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(533, 240);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Generation Settings";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(179, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Define Prefix for Target Namespace:";
            // 
            // comboModels
            // 
            this.comboModels.FormattingEnabled = true;
            this.comboModels.Location = new System.Drawing.Point(201, 20);
            this.comboModels.Name = "comboModels";
            this.comboModels.Size = new System.Drawing.Size(320, 21);
            this.comboModels.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Choose Document Model:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Define Target Namespace:";
            // 
            // textPrefixTargetNS
            // 
            this.textPrefixTargetNS.Location = new System.Drawing.Point(201, 190);
            this.textPrefixTargetNS.Name = "textPrefixTargetNS";
            this.textPrefixTargetNS.Size = new System.Drawing.Size(320, 20);
            this.textPrefixTargetNS.TabIndex = 37;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Check Documents to Generate:";
            // 
            // checkedlistboxDOCs
            // 
            this.checkedlistboxDOCs.CheckOnClick = true;
            this.checkedlistboxDOCs.FormattingEnabled = true;
            this.checkedlistboxDOCs.Location = new System.Drawing.Point(201, 47);
            this.checkedlistboxDOCs.Name = "checkedlistboxDOCs";
            this.checkedlistboxDOCs.Size = new System.Drawing.Size(320, 109);
            this.checkedlistboxDOCs.TabIndex = 28;
            this.checkedlistboxDOCs.SelectedIndexChanged += new System.EventHandler(this.checkedlistboxDOCs_SelectedIndexChanged);
            // 
            // textTargetNS
            // 
            this.textTargetNS.Location = new System.Drawing.Point(201, 164);
            this.textTargetNS.Name = "textTargetNS";
            this.textTargetNS.Size = new System.Drawing.Size(320, 20);
            this.textTargetNS.TabIndex = 35;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(201, 216);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 33;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Enable Documentation Annotations:";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(354, 430);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(108, 430);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(149, 23);
            this.buttonGenerate.TabIndex = 2;
            this.buttonGenerate.Text = "&Generate XML Schema...";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(166, 13);
            this.label16.TabIndex = 30;
            this.label16.Text = "Select Business Information View:";
            // 
            // comboBIVs
            // 
            this.comboBIVs.FormattingEnabled = true;
            this.comboBIVs.Location = new System.Drawing.Point(213, 12);
            this.comboBIVs.Name = "comboBIVs";
            this.comboBIVs.Size = new System.Drawing.Size(332, 21);
            this.comboBIVs.TabIndex = 41;
            this.comboBIVs.SelectionChangeCommitted += new System.EventHandler(this.comboBIVs_SelectionChangeCommitted);
            // 
            // GeneratorWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 465);
            this.Controls.Add(this.comboBIVs);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "GeneratorWizardForm";
            this.Text = "GeneratorWizardForm";
            this.Load += new System.EventHandler(this.GeneratorWizardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textTargetNS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox checkedlistboxDOCs;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textPrefixTargetNS;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboModels;
        private System.Windows.Forms.ComboBox comboBIVs;
    }
}