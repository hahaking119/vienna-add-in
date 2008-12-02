namespace VIENNAAddIn.CCTS {
    partial class CCWindow {
        /// <sUMM2ary>
        /// Required designer variable.
        /// </sUMM2ary>
        private System.ComponentModel.IContainer components = null;

        /// <sUMM2ary>
        /// Clean up any resources being used.
        /// </sUMM2ary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing) {
        //    this.Visible = false;
        //    this.resetWindow("");
        //}

        
        #region Windows Form Designer generated code

        /// <sUMM2ary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </sUMM2ary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCWindow));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnInsertABIE = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ccomponents = new System.Windows.Forms.ComboBox();
            this.attributeBox = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkSelectDeselectAttr = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.clbAssociation = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.prefix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sourceCCLibrary = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(309, 388);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnInsertABIE
            // 
            this.btnInsertABIE.Enabled = false;
            this.btnInsertABIE.Location = new System.Drawing.Point(219, 388);
            this.btnInsertABIE.Name = "btnInsertABIE";
            this.btnInsertABIE.Size = new System.Drawing.Size(84, 23);
            this.btnInsertABIE.TabIndex = 1;
            this.btnInsertABIE.Text = "Insert ABIE";
            this.btnInsertABIE.UseVisualStyleBackColor = true;
            this.btnInsertABIE.Click += new System.EventHandler(this.btnInsertABIE_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select CC:";
            // 
            // ccomponents
            // 
            this.ccomponents.FormattingEnabled = true;
            this.ccomponents.Location = new System.Drawing.Point(108, 48);
            this.ccomponents.Name = "ccomponents";
            this.ccomponents.Size = new System.Drawing.Size(259, 21);
            this.ccomponents.TabIndex = 3;
            this.ccomponents.SelectedIndexChanged += new System.EventHandler(this.ccomponents_SelectedIndexChanged);
            // 
            // attributeBox
            // 
            this.attributeBox.CheckOnClick = true;
            this.attributeBox.FormattingEnabled = true;
            this.attributeBox.Location = new System.Drawing.Point(3, 53);
            this.attributeBox.Name = "attributeBox";
            this.attributeBox.Size = new System.Drawing.Size(338, 184);
            this.attributeBox.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleName = "";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(15, 79);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(351, 271);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.Tag = "";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkSelectDeselectAttr);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.attributeBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(343, 245);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Attributes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkSelectDeselectAttr
            // 
            this.chkSelectDeselectAttr.AutoSize = true;
            this.chkSelectDeselectAttr.Location = new System.Drawing.Point(7, 27);
            this.chkSelectDeselectAttr.Name = "chkSelectDeselectAttr";
            this.chkSelectDeselectAttr.Size = new System.Drawing.Size(112, 17);
            this.chkSelectDeselectAttr.TabIndex = 6;
            this.chkSelectDeselectAttr.Text = "select/deselect all";
            this.chkSelectDeselectAttr.UseVisualStyleBackColor = true;
            this.chkSelectDeselectAttr.CheckedChanged += new System.EventHandler(this.chkSelectDeselectAttr_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select the attributes you want to include";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.clbAssociation);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(343, 245);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Associations";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Select element you want to link";
            // 
            // clbAssociation
            // 
            this.clbAssociation.CheckOnClick = true;
            this.clbAssociation.FormattingEnabled = true;
            this.clbAssociation.Location = new System.Drawing.Point(3, 33);
            this.clbAssociation.Name = "clbAssociation";
            this.clbAssociation.Size = new System.Drawing.Size(337, 199);
            this.clbAssociation.TabIndex = 0;
            this.clbAssociation.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbAssociation_ItemCheck);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Prefix for ABIE:";
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(97, 356);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(269, 20);
            this.prefix.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 30);
            this.label4.TabIndex = 10;
            this.label4.Text = "Select Source CCLibrary:";
            // 
            // sourceCCLibrary
            // 
            this.sourceCCLibrary.FormattingEnabled = true;
            this.sourceCCLibrary.Location = new System.Drawing.Point(108, 9);
            this.sourceCCLibrary.Name = "sourceCCLibrary";
            this.sourceCCLibrary.Size = new System.Drawing.Size(259, 21);
            this.sourceCCLibrary.TabIndex = 11;
            this.sourceCCLibrary.SelectedIndexChanged += new System.EventHandler(this.sourceLibrary_SelectedIndexChanged);
            // 
            // CCWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 426);
            this.Controls.Add(this.sourceCCLibrary);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.prefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ccomponents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInsertABIE);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CCWindow";
            this.Text = "ABIE Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CCWindow_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnInsertABIE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ccomponents;
        private System.Windows.Forms.CheckedListBox attributeBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox prefix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox sourceCCLibrary;
        private System.Windows.Forms.CheckBox chkSelectDeselectAttr;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox clbAssociation;
    }
}