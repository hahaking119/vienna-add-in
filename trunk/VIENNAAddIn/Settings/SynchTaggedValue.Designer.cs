namespace VIENNAAddIn.Setting
{
    partial class SynchTaggedValue
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
            this.lsbStereotype = new System.Windows.Forms.ListBox();
            this.lblProfile = new System.Windows.Forms.Label();
            this.cmbProfile = new System.Windows.Forms.ComboBox();
            this.btnSynchronize = new System.Windows.Forms.Button();
            this.grpProfile = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtOneByOne = new System.Windows.Forms.RadioButton();
            this.rbtAll = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpProfile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsbStereotype
            // 
            this.lsbStereotype.FormattingEnabled = true;
            this.lsbStereotype.Location = new System.Drawing.Point(188, 26);
            this.lsbStereotype.Name = "lsbStereotype";
            this.lsbStereotype.Size = new System.Drawing.Size(223, 95);
            this.lsbStereotype.TabIndex = 0;
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Location = new System.Drawing.Point(18, 3);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(81, 13);
            this.lblProfile.TabIndex = 1;
            this.lblProfile.Text = "Choose Profile :";
            // 
            // cmbProfile
            // 
            this.cmbProfile.FormattingEnabled = true;
            this.cmbProfile.Location = new System.Drawing.Point(21, 26);
            this.cmbProfile.MaxDropDownItems = 3;
            this.cmbProfile.Name = "cmbProfile";
            this.cmbProfile.Size = new System.Drawing.Size(161, 21);
            this.cmbProfile.TabIndex = 2;
            this.cmbProfile.SelectedIndexChanged += new System.EventHandler(this.cmbProfile_SelectedIndexChanged);
            // 
            // btnSynchronize
            // 
            this.btnSynchronize.Location = new System.Drawing.Point(376, 241);
            this.btnSynchronize.Name = "btnSynchronize";
            this.btnSynchronize.Size = new System.Drawing.Size(75, 23);
            this.btnSynchronize.TabIndex = 3;
            this.btnSynchronize.Text = "Synchronize";
            this.btnSynchronize.UseVisualStyleBackColor = true;
            this.btnSynchronize.Click += new System.EventHandler(this.btnSynchronize_Click);
            // 
            // grpProfile
            // 
            this.grpProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProfile.Controls.Add(this.panel1);
            this.grpProfile.Controls.Add(this.rbtOneByOne);
            this.grpProfile.Controls.Add(this.rbtAll);
            this.grpProfile.Location = new System.Drawing.Point(12, 12);
            this.grpProfile.Name = "grpProfile";
            this.grpProfile.Size = new System.Drawing.Size(439, 223);
            this.grpProfile.TabIndex = 4;
            this.grpProfile.TabStop = false;
            this.grpProfile.Text = "Method of synchronization";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblProfile);
            this.panel1.Controls.Add(this.lsbStereotype);
            this.panel1.Controls.Add(this.cmbProfile);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(6, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 143);
            this.panel1.TabIndex = 5;
            // 
            // rbtOneByOne
            // 
            this.rbtOneByOne.AutoSize = true;
            this.rbtOneByOne.Location = new System.Drawing.Point(6, 51);
            this.rbtOneByOne.Name = "rbtOneByOne";
            this.rbtOneByOne.Size = new System.Drawing.Size(139, 17);
            this.rbtOneByOne.TabIndex = 5;
            this.rbtOneByOne.Text = "Synchronize one by one";
            this.rbtOneByOne.UseVisualStyleBackColor = true;
            this.rbtOneByOne.CheckedChanged += new System.EventHandler(this.rbtOneByOne_CheckedChanged);
            // 
            // rbtAll
            // 
            this.rbtAll.AutoSize = true;
            this.rbtAll.Checked = true;
            this.rbtAll.Location = new System.Drawing.Point(6, 19);
            this.rbtAll.Name = "rbtAll";
            this.rbtAll.Size = new System.Drawing.Size(96, 17);
            this.rbtAll.TabIndex = 6;
            this.rbtAll.TabStop = true;
            this.rbtAll.Text = "Synchronize all";
            this.rbtAll.UseVisualStyleBackColor = true;
            this.rbtAll.CheckedChanged += new System.EventHandler(this.rbtAll_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 241);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Stereotype of selected profile :";
            // 
            // SynchTaggedValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 276);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpProfile);
            this.Controls.Add(this.btnSynchronize);
            this.MaximizeBox = false;
            this.Name = "SynchTaggedValue";
            this.Text = "SynchTaggedValue";
            this.Load += new System.EventHandler(this.SynchTaggedValue_Load);
            this.grpProfile.ResumeLayout(false);
            this.grpProfile.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsbStereotype;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.ComboBox cmbProfile;
        private System.Windows.Forms.Button btnSynchronize;
        private System.Windows.Forms.GroupBox grpProfile;
        private System.Windows.Forms.RadioButton rbtOneByOne;
        private System.Windows.Forms.RadioButton rbtAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
    }
}