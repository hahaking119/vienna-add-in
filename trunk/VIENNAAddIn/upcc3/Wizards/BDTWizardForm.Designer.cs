﻿namespace VIENNAAddIn.upcc3.Wizards
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
            this.groupboxSettings = new System.Windows.Forms.GroupBox();
            this.comboBDTLs = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBDTName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAttributes = new System.Windows.Forms.TabPage();
            this.checkedlistboxSUPs = new System.Windows.Forms.CheckedListBox();
            this.checkboxAttributes = new System.Windows.Forms.CheckBox();
            this.comboCDTs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboCDTLs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGenerateBDT = new System.Windows.Forms.Button();
            this.groupboxSUPs = new System.Windows.Forms.GroupBox();
            this.groupboxCON = new System.Windows.Forms.GroupBox();
            this.checkedlistboxCON = new System.Windows.Forms.CheckedListBox();
            this.groupboxSettings.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabAttributes.SuspendLayout();
            this.groupboxSUPs.SuspendLayout();
            this.groupboxCON.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupboxSettings
            // 
            this.groupboxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxSettings.Controls.Add(this.comboBDTLs);
            this.groupboxSettings.Controls.Add(this.label4);
            this.groupboxSettings.Controls.Add(this.textBDTName);
            this.groupboxSettings.Controls.Add(this.label3);
            this.groupboxSettings.Controls.Add(this.tabControl1);
            this.groupboxSettings.Controls.Add(this.comboCDTs);
            this.groupboxSettings.Controls.Add(this.label2);
            this.groupboxSettings.Controls.Add(this.comboCDTLs);
            this.groupboxSettings.Controls.Add(this.label1);
            this.groupboxSettings.Location = new System.Drawing.Point(12, 5);
            this.groupboxSettings.Name = "groupboxSettings";
            this.groupboxSettings.Size = new System.Drawing.Size(451, 586);
            this.groupboxSettings.TabIndex = 3;
            this.groupboxSettings.TabStop = false;
            this.groupboxSettings.Text = "Settings";
            // 
            // comboBDTLs
            // 
            this.comboBDTLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBDTLs.FormattingEnabled = true;
            this.comboBDTLs.Location = new System.Drawing.Point(215, 550);
            this.comboBDTLs.Name = "comboBDTLs";
            this.comboBDTLs.Size = new System.Drawing.Size(220, 21);
            this.comboBDTLs.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 553);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "BDT Library used to store the BDT:";
            // 
            // textBDTName
            // 
            this.textBDTName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBDTName.Location = new System.Drawing.Point(215, 524);
            this.textBDTName.Name = "textBDTName";
            this.textBDTName.Size = new System.Drawing.Size(220, 20);
            this.textBDTName.TabIndex = 7;
            this.textBDTName.TextChanged += new System.EventHandler(this.textBDTName_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 527);
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
            this.tabControl1.Size = new System.Drawing.Size(419, 432);
            this.tabControl1.TabIndex = 5;
            // 
            // tabAttributes
            // 
            this.tabAttributes.Controls.Add(this.groupboxCON);
            this.tabAttributes.Controls.Add(this.groupboxSUPs);
            this.tabAttributes.Location = new System.Drawing.Point(4, 22);
            this.tabAttributes.Name = "tabAttributes";
            this.tabAttributes.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttributes.Size = new System.Drawing.Size(411, 406);
            this.tabAttributes.TabIndex = 0;
            this.tabAttributes.Text = "Attributes";
            this.tabAttributes.UseVisualStyleBackColor = true;
            // 
            // checkedlistboxSUPs
            // 
            this.checkedlistboxSUPs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxSUPs.FormattingEnabled = true;
            this.checkedlistboxSUPs.Location = new System.Drawing.Point(11, 44);
            this.checkedlistboxSUPs.Name = "checkedlistboxSUPs";
            this.checkedlistboxSUPs.Size = new System.Drawing.Size(376, 274);
            this.checkedlistboxSUPs.TabIndex = 1;
            this.checkedlistboxSUPs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxSUPs_ItemCheck);
            // 
            // checkboxAttributes
            // 
            this.checkboxAttributes.AutoSize = true;
            this.checkboxAttributes.Location = new System.Drawing.Point(14, 21);
            this.checkboxAttributes.Name = "checkboxAttributes";
            this.checkboxAttributes.Size = new System.Drawing.Size(99, 17);
            this.checkboxAttributes.TabIndex = 0;
            this.checkboxAttributes.Text = "Select all SUPs";
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
            this.comboCDTs.Size = new System.Drawing.Size(219, 21);
            this.comboCDTs.TabIndex = 4;
            this.comboCDTs.SelectionChangeCommitted += new System.EventHandler(this.comboCDTs_SelectionChangeCommitted);
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
            // comboCDTLs
            // 
            this.comboCDTLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCDTLs.FormattingEnabled = true;
            this.comboCDTLs.Location = new System.Drawing.Point(215, 20);
            this.comboCDTLs.Name = "comboCDTLs";
            this.comboCDTLs.Size = new System.Drawing.Size(220, 21);
            this.comboCDTLs.TabIndex = 2;
            this.comboCDTLs.SelectionChangeCommitted += new System.EventHandler(this.comboCDTLs_SelectionChangeCommitted);
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
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(275, 608);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(101, 22);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGenerateBDT
            // 
            this.buttonGenerateBDT.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonGenerateBDT.Location = new System.Drawing.Point(91, 608);
            this.buttonGenerateBDT.Name = "buttonGenerateBDT";
            this.buttonGenerateBDT.Size = new System.Drawing.Size(101, 22);
            this.buttonGenerateBDT.TabIndex = 5;
            this.buttonGenerateBDT.Text = "&Generate BDT ...";
            this.buttonGenerateBDT.UseVisualStyleBackColor = true;
            this.buttonGenerateBDT.Click += new System.EventHandler(this.buttonGenerateBDT_Click);
            // 
            // groupboxSUPs
            // 
            this.groupboxSUPs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxSUPs.Controls.Add(this.checkedlistboxSUPs);
            this.groupboxSUPs.Controls.Add(this.checkboxAttributes);
            this.groupboxSUPs.Location = new System.Drawing.Point(6, 66);
            this.groupboxSUPs.Name = "groupboxSUPs";
            this.groupboxSUPs.Size = new System.Drawing.Size(399, 334);
            this.groupboxSUPs.TabIndex = 2;
            this.groupboxSUPs.TabStop = false;
            this.groupboxSUPs.Text = "SUPs";
            // 
            // groupboxCON
            // 
            this.groupboxCON.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxCON.Controls.Add(this.checkedlistboxCON);
            this.groupboxCON.Location = new System.Drawing.Point(6, 6);
            this.groupboxCON.Name = "groupboxCON";
            this.groupboxCON.Size = new System.Drawing.Size(399, 51);
            this.groupboxCON.TabIndex = 3;
            this.groupboxCON.TabStop = false;
            this.groupboxCON.Text = "CON";
            // 
            // checkedlistboxCON
            // 
            this.checkedlistboxCON.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxCON.Enabled = false;
            this.checkedlistboxCON.FormattingEnabled = true;
            this.checkedlistboxCON.Location = new System.Drawing.Point(11, 19);
            this.checkedlistboxCON.Name = "checkedlistboxCON";
            this.checkedlistboxCON.Size = new System.Drawing.Size(376, 19);
            this.checkedlistboxCON.TabIndex = 4;
            // 
            // BDTWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 642);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerateBDT);
            this.Controls.Add(this.groupboxSettings);
            this.Name = "BDTWizardForm";
            this.Text = "BDTWizardForm";
            this.Load += new System.EventHandler(this.BDTWizardForm_Load);
            this.groupboxSettings.ResumeLayout(false);
            this.groupboxSettings.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabAttributes.ResumeLayout(false);
            this.groupboxSUPs.ResumeLayout(false);
            this.groupboxSUPs.PerformLayout();
            this.groupboxCON.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupboxSettings;
        private System.Windows.Forms.ComboBox comboBDTLs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBDTName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAttributes;
        private System.Windows.Forms.CheckedListBox checkedlistboxSUPs;
        private System.Windows.Forms.CheckBox checkboxAttributes;
        private System.Windows.Forms.ComboBox comboCDTs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboCDTLs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGenerateBDT;
        private System.Windows.Forms.CheckedListBox checkedlistboxCON;
        private System.Windows.Forms.GroupBox groupboxCON;
        private System.Windows.Forms.GroupBox groupboxSUPs;
    }
}