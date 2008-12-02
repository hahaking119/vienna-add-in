namespace VIENNAAddIn.WSDLGenerator
{
    partial class TransactionModuleArtefact
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
            this.lblTransModule = new System.Windows.Forms.Label();
            this.btnWSDLLocation = new System.Windows.Forms.Button();
            this.txtWSDLLocation = new System.Windows.Forms.TextBox();
            this.btnSavingPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSavingPath = new System.Windows.Forms.TextBox();
            this.btnSchemaLocation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.selectedBusinessTransaction = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dlgWSDLLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.btnGenerateBPEL = new System.Windows.Forms.Button();
            this.dlgSchemaLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.dlgSavePath = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSchemaLocation = new System.Windows.Forms.TextBox();
            this.chkBindingService = new System.Windows.Forms.CheckBox();
            this.chkUseAlias = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblTransModule
            // 
            this.lblTransModule.AutoSize = true;
            this.lblTransModule.Location = new System.Drawing.Point(13, 145);
            this.lblTransModule.Name = "lblTransModule";
            this.lblTransModule.Size = new System.Drawing.Size(83, 13);
            this.lblTransModule.TabIndex = 105;
            this.lblTransModule.Text = "WSDL Location";
            // 
            // btnWSDLLocation
            // 
            this.btnWSDLLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWSDLLocation.Enabled = false;
            this.btnWSDLLocation.Location = new System.Drawing.Point(366, 161);
            this.btnWSDLLocation.Name = "btnWSDLLocation";
            this.btnWSDLLocation.Size = new System.Drawing.Size(75, 23);
            this.btnWSDLLocation.TabIndex = 104;
            this.btnWSDLLocation.Text = "Browse";
            this.btnWSDLLocation.UseVisualStyleBackColor = true;
            this.btnWSDLLocation.Click += new System.EventHandler(this.btnWSDLLocation_Click);
            // 
            // txtWSDLLocation
            // 
            this.txtWSDLLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWSDLLocation.Location = new System.Drawing.Point(17, 162);
            this.txtWSDLLocation.Name = "txtWSDLLocation";
            this.txtWSDLLocation.ReadOnly = true;
            this.txtWSDLLocation.Size = new System.Drawing.Size(343, 20);
            this.txtWSDLLocation.TabIndex = 103;
            // 
            // btnSavingPath
            // 
            this.btnSavingPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavingPath.Location = new System.Drawing.Point(366, 67);
            this.btnSavingPath.Name = "btnSavingPath";
            this.btnSavingPath.Size = new System.Drawing.Size(75, 23);
            this.btnSavingPath.TabIndex = 102;
            this.btnSavingPath.Text = "Browse";
            this.btnSavingPath.UseVisualStyleBackColor = true;
            this.btnSavingPath.Click += new System.EventHandler(this.btnSavingPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 101;
            this.label3.Text = "Target Directory";
            // 
            // txtSavingPath
            // 
            this.txtSavingPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSavingPath.Location = new System.Drawing.Point(17, 68);
            this.txtSavingPath.Name = "txtSavingPath";
            this.txtSavingPath.ReadOnly = true;
            this.txtSavingPath.Size = new System.Drawing.Size(343, 20);
            this.txtSavingPath.TabIndex = 100;
            // 
            // btnSchemaLocation
            // 
            this.btnSchemaLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSchemaLocation.Enabled = false;
            this.btnSchemaLocation.Location = new System.Drawing.Point(366, 113);
            this.btnSchemaLocation.Name = "btnSchemaLocation";
            this.btnSchemaLocation.Size = new System.Drawing.Size(75, 23);
            this.btnSchemaLocation.TabIndex = 99;
            this.btnSchemaLocation.Text = "Browse";
            this.btnSchemaLocation.UseVisualStyleBackColor = true;
            this.btnSchemaLocation.Click += new System.EventHandler(this.btnSchemaLocation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 13);
            this.label1.TabIndex = 91;
            this.label1.Text = "Selected BusinessTransaction";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(17, 373);
            this.progressBar1.Maximum = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(424, 24);
            this.progressBar1.TabIndex = 93;
            // 
            // selectedBusinessTransaction
            // 
            this.selectedBusinessTransaction.AutoSize = true;
            this.selectedBusinessTransaction.Location = new System.Drawing.Point(14, 27);
            this.selectedBusinessTransaction.MaximumSize = new System.Drawing.Size(420, 13);
            this.selectedBusinessTransaction.Name = "selectedBusinessTransaction";
            this.selectedBusinessTransaction.Size = new System.Drawing.Size(35, 13);
            this.selectedBusinessTransaction.TabIndex = 92;
            this.selectedBusinessTransaction.Text = "label2";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(220, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 96;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateBPEL
            // 
            this.btnGenerateBPEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateBPEL.Location = new System.Drawing.Point(301, 410);
            this.btnGenerateBPEL.Name = "btnGenerateBPEL";
            this.btnGenerateBPEL.Size = new System.Drawing.Size(140, 24);
            this.btnGenerateBPEL.TabIndex = 95;
            this.btnGenerateBPEL.Text = "Generate BPEL";
            this.btnGenerateBPEL.UseVisualStyleBackColor = true;
            this.btnGenerateBPEL.Click += new System.EventHandler(this.btnGenerateTMArtefact_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Location = new System.Drawing.Point(15, 245);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new System.Drawing.Size(424, 122);
            this.statusTextBox.TabIndex = 94;
            this.statusTextBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 98;
            this.label2.Text = "Schema Location :";
            // 
            // txtSchemaLocation
            // 
            this.txtSchemaLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSchemaLocation.Location = new System.Drawing.Point(17, 114);
            this.txtSchemaLocation.Name = "txtSchemaLocation";
            this.txtSchemaLocation.ReadOnly = true;
            this.txtSchemaLocation.Size = new System.Drawing.Size(343, 20);
            this.txtSchemaLocation.TabIndex = 97;
            // 
            // chkBindingService
            // 
            this.chkBindingService.AutoSize = true;
            this.chkBindingService.Location = new System.Drawing.Point(17, 220);
            this.chkBindingService.Name = "chkBindingService";
            this.chkBindingService.Size = new System.Drawing.Size(337, 17);
            this.chkBindingService.TabIndex = 106;
            this.chkBindingService.Text = "Include simple SOAP binding and services on Transaction Module";
            this.chkBindingService.UseVisualStyleBackColor = true;
            this.chkBindingService.CheckStateChanged += new System.EventHandler(this.chkBindingService_CheckStateChanged);
            // 
            // chkUseAlias
            // 
            this.chkUseAlias.AutoSize = true;
            this.chkUseAlias.Location = new System.Drawing.Point(17, 197);
            this.chkUseAlias.Name = "chkUseAlias";
            this.chkUseAlias.Size = new System.Drawing.Size(142, 17);
            this.chkUseAlias.TabIndex = 107;
            this.chkUseAlias.Text = "Use alias for folder name";
            this.chkUseAlias.UseVisualStyleBackColor = true;
            // 
            // TransactionModuleArtefact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 446);
            this.Controls.Add(this.chkUseAlias);
            this.Controls.Add(this.chkBindingService);
            this.Controls.Add(this.lblTransModule);
            this.Controls.Add(this.btnWSDLLocation);
            this.Controls.Add(this.txtWSDLLocation);
            this.Controls.Add(this.btnSavingPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSavingPath);
            this.Controls.Add(this.btnSchemaLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.selectedBusinessTransaction);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateBPEL);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSchemaLocation);
            this.Name = "TransactionModuleArtefact";
            this.Text = "Transaction Module Artefact";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTransModule;
        private System.Windows.Forms.Button btnWSDLLocation;
        private System.Windows.Forms.TextBox txtWSDLLocation;
        private System.Windows.Forms.Button btnSavingPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSavingPath;
        private System.Windows.Forms.Button btnSchemaLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label selectedBusinessTransaction;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog dlgWSDLLocation;
        private System.Windows.Forms.Button btnGenerateBPEL;
        private System.Windows.Forms.FolderBrowserDialog dlgSchemaLocation;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.FolderBrowserDialog dlgSavePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSchemaLocation;
        private System.Windows.Forms.CheckBox chkBindingService;
        private System.Windows.Forms.CheckBox chkUseAlias;

    }
}