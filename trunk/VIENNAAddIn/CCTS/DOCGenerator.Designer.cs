
/*******************************************************************************
This file is part of the VIENNAAddIn-AddIn project.

Copyright (C) Digital Memory Engineering, ARC Seibersdorf research GmbH.
All rights reserved.

This software is the proprietary information of Digital Memory Engineering, ARC Seibersdorf research GmbH.
Use is subject to license terms.
For further information visit http://dme.researchstudio.at.
*******************************************************************************/
namespace VIENNAAddIn.CCTS {
    partial class DOCGenerator {
        /// <sUMM2ary>
        /// Required designer variable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Clean up any resources being used.
        /// </sUMM2ary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <sUMM2ary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </sUMM2ary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DOCGenerator));
            this.generateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxRootElement = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selectedBusinessInformationView = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkUseAlias = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIncludeLinkedSchema = new System.Windows.Forms.CheckBox();
            this.chkNillable = new System.Windows.Forms.CheckBox();
            this.chkAnnotate = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(6, 46);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(112, 23);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate Schema";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(124, 46);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxRootElement);
            this.groupBox1.Controls.Add(this.generateButton);
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 77);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Please choose root element";
            // 
            // cbxRootElement
            // 
            this.cbxRootElement.FormattingEnabled = true;
            this.cbxRootElement.Location = new System.Drawing.Point(7, 19);
            this.cbxRootElement.Name = "cbxRootElement";
            this.cbxRootElement.Size = new System.Drawing.Size(218, 21);
            this.cbxRootElement.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.statusTextBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 124);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(6, 19);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(416, 99);
            this.statusTextBox.TabIndex = 0;
            this.statusTextBox.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.selectedBusinessInformationView);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 40);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected BusinessInformationView";
            // 
            // selectedBusinessInformationView
            // 
            this.selectedBusinessInformationView.Enabled = false;
            this.selectedBusinessInformationView.Location = new System.Drawing.Point(7, 14);
            this.selectedBusinessInformationView.Name = "selectedBusinessInformationView";
            this.selectedBusinessInformationView.Size = new System.Drawing.Size(219, 20);
            this.selectedBusinessInformationView.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.progressBar1);
            this.groupBox4.Location = new System.Drawing.Point(12, 291);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(428, 47);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Progress";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 19);
            this.progressBar1.Maximum = 300;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(415, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkUseAlias);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.chkIncludeLinkedSchema);
            this.groupBox5.Controls.Add(this.chkNillable);
            this.groupBox5.Controls.Add(this.chkAnnotate);
            this.groupBox5.Location = new System.Drawing.Point(250, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(190, 135);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Settings";
            // 
            // chkUseAlias
            // 
            this.chkUseAlias.AutoSize = true;
            this.chkUseAlias.Location = new System.Drawing.Point(7, 104);
            this.chkUseAlias.Name = "chkUseAlias";
            this.chkUseAlias.Size = new System.Drawing.Size(142, 17);
            this.chkUseAlias.TabIndex = 13;
            this.chkUseAlias.Text = "Use alias for folder name";
            this.chkUseAlias.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "to BBIEs with a cardinality of 0..X";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // chkIncludeLinkedSchema
            // 
            this.chkIncludeLinkedSchema.AutoSize = true;
            this.chkIncludeLinkedSchema.Location = new System.Drawing.Point(7, 45);
            this.chkIncludeLinkedSchema.Name = "chkIncludeLinkedSchema";
            this.chkIncludeLinkedSchema.Size = new System.Drawing.Size(138, 17);
            this.chkIncludeLinkedSchema.TabIndex = 1;
            this.chkIncludeLinkedSchema.Text = "Include Linked Schema";
            this.chkIncludeLinkedSchema.UseVisualStyleBackColor = true;
            // 
            // chkNillable
            // 
            this.chkNillable.Location = new System.Drawing.Point(7, 68);
            this.chkNillable.Name = "chkNillable";
            this.chkNillable.Size = new System.Drawing.Size(160, 17);
            this.chkNillable.TabIndex = 11;
            this.chkNillable.Text = "Add attribute Nillable=”true”";
            this.chkNillable.UseVisualStyleBackColor = true;
            this.chkNillable.Click += new System.EventHandler(this.chkNillable_CheckedChanged);
            // 
            // chkAnnotate
            // 
            this.chkAnnotate.AutoSize = true;
            this.chkAnnotate.Checked = true;
            this.chkAnnotate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnnotate.Location = new System.Drawing.Point(7, 22);
            this.chkAnnotate.Name = "chkAnnotate";
            this.chkAnnotate.Size = new System.Drawing.Size(132, 17);
            this.chkAnnotate.TabIndex = 0;
            this.chkAnnotate.Text = "Annotate the elements";
            this.chkAnnotate.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select a folder for saving the generated schemas";
            // 
            // DOCGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 354);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DOCGenerator";
            this.Text = "Generate XSD from DOC";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox selectedBusinessInformationView;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkAnnotate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ComboBox cbxRootElement;
        private System.Windows.Forms.CheckBox chkIncludeLinkedSchema;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNillable;
        private System.Windows.Forms.CheckBox chkUseAlias;
    }
}