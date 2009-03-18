namespace VIENNAAddIn.CCTS {
    partial class QDTWindow {
        /// <sUMM2ary>
        /// Required designer variable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Clean up any resources being used.
        /// </sUMM2ary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            this.Visible = false;            
        }
        
        
        #region Windows Form Designer generated code

        /// <sUMM2ary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </sUMM2ary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDTWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.coredatatypes = new System.Windows.Forms.ComboBox();
            this.attributeBox = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.prefix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sourceCDTLibrary = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(219, 391);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Insert QDT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select CDT:";
            // 
            // coredatatypes
            // 
            this.coredatatypes.FormattingEnabled = true;
            this.coredatatypes.Location = new System.Drawing.Point(108, 46);
            this.coredatatypes.Name = "coredatatypes";
            this.coredatatypes.Size = new System.Drawing.Size(256, 21);
            this.coredatatypes.TabIndex = 3;
            this.coredatatypes.SelectedIndexChanged += new System.EventHandler(this.ccomponents_SelectedIndexChanged);
            // 
            // attributeBox
            // 
            this.attributeBox.FormattingEnabled = true;
            this.attributeBox.Location = new System.Drawing.Point(3, 51);
            this.attributeBox.Name = "attributeBox";
            this.attributeBox.Size = new System.Drawing.Size(332, 199);
            this.attributeBox.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleName = "";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(15, 77);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(349, 282);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.attributeBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(341, 256);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Attributes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select the attributes you want to include";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 365);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Prefix for QDT:";
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(97, 365);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(263, 20);
            this.prefix.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 34);
            this.label4.TabIndex = 10;
            this.label4.Text = "Select Source CDT Library:";
            // 
            // sourceCDTLibrary
            // 
            this.sourceCDTLibrary.FormattingEnabled = true;
            this.sourceCDTLibrary.Location = new System.Drawing.Point(108, 9);
            this.sourceCDTLibrary.Name = "sourceCDTLibrary";
            this.sourceCDTLibrary.Size = new System.Drawing.Size(256, 21);
            this.sourceCDTLibrary.TabIndex = 11;
            this.sourceCDTLibrary.SelectedIndexChanged += new System.EventHandler(this.sourceCDTLibrary_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 27);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(112, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "select/deselect all";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // QDTWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 426);
            this.Controls.Add(this.sourceCDTLibrary);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.prefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.coredatatypes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QDTWindow";
            this.Text = "QDT Wizard";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox coredatatypes;
        private System.Windows.Forms.CheckedListBox attributeBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox prefix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox sourceCDTLibrary;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}