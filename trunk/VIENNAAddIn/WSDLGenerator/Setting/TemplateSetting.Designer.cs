namespace VIENNAAddIn.WSDLGenerator.Setting
{
    partial class TemplateSetting
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
            this.rbtBPELTemplate = new System.Windows.Forms.RadioButton();
            this.rbtXSLTTemplate = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTemplateInitiator = new System.Windows.Forms.Button();
            this.txtTemplateInitiator = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBPELTemplate = new System.Windows.Forms.Panel();
            this.txtTemplateResponderSpecialCase = new System.Windows.Forms.TextBox();
            this.btnTemplateResponderSpecialCase = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTemplateResponderOneWay = new System.Windows.Forms.TextBox();
            this.btnTemplateResponderOneWay = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTemplateResponder = new System.Windows.Forms.TextBox();
            this.btnTemplateResponder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTemplateInitiatorSpecialCase = new System.Windows.Forms.TextBox();
            this.btnTemplateInitiatorSpecialCase = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTemplateInitiatorOneWay = new System.Windows.Forms.TextBox();
            this.btnTemplateInitiatorOneWay = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlXSLTTemplate = new System.Windows.Forms.Panel();
            this.txtMappingTransactionProtocolSuccess = new System.Windows.Forms.TextBox();
            this.btnMappingTransactionProtocolSuccess = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMappingTransactionProtocolFailure = new System.Windows.Forms.TextBox();
            this.btnMappingTransactionProtocolFailure = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMappingReceiptException = new System.Windows.Forms.TextBox();
            this.btnMappingReceiptException = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMappingReceiptAck = new System.Windows.Forms.TextBox();
            this.btnMappingReceiptAck = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMappingGeneralException = new System.Windows.Forms.TextBox();
            this.btnMappingGeneralException = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlReplaceAll = new System.Windows.Forms.Panel();
            this.txtReplaceAll = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.rbtOneByOne = new System.Windows.Forms.RadioButton();
            this.rbtReplaceAll = new System.Windows.Forms.RadioButton();
            this.dlgReplaceAll = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgBPELTemplate = new System.Windows.Forms.OpenFileDialog();
            this.dlgXSLTTemplate = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.pnlBPELTemplate.SuspendLayout();
            this.pnlXSLTTemplate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnlReplaceAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtBPELTemplate
            // 
            this.rbtBPELTemplate.AutoSize = true;
            this.rbtBPELTemplate.Location = new System.Drawing.Point(7, 29);
            this.rbtBPELTemplate.Name = "rbtBPELTemplate";
            this.rbtBPELTemplate.Size = new System.Drawing.Size(99, 17);
            this.rbtBPELTemplate.TabIndex = 0;
            this.rbtBPELTemplate.Text = "BPEL Template";
            this.rbtBPELTemplate.UseVisualStyleBackColor = true;
            this.rbtBPELTemplate.Click += new System.EventHandler(this.rbtBPELTemplate_Click);
            // 
            // rbtXSLTTemplate
            // 
            this.rbtXSLTTemplate.AutoSize = true;
            this.rbtXSLTTemplate.Checked = true;
            this.rbtXSLTTemplate.Location = new System.Drawing.Point(135, 29);
            this.rbtXSLTTemplate.Name = "rbtXSLTTemplate";
            this.rbtXSLTTemplate.Size = new System.Drawing.Size(99, 17);
            this.rbtXSLTTemplate.TabIndex = 1;
            this.rbtXSLTTemplate.TabStop = true;
            this.rbtXSLTTemplate.Text = "XSLT Template";
            this.rbtXSLTTemplate.UseVisualStyleBackColor = true;
            this.rbtXSLTTemplate.Click += new System.EventHandler(this.rbtXSLTTemplate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtBPELTemplate);
            this.groupBox1.Controls.Add(this.rbtXSLTTemplate);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 56);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Template to update";
            // 
            // btnTemplateInitiator
            // 
            this.btnTemplateInitiator.Location = new System.Drawing.Point(402, 3);
            this.btnTemplateInitiator.Name = "btnTemplateInitiator";
            this.btnTemplateInitiator.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateInitiator.TabIndex = 4;
            this.btnTemplateInitiator.Text = "Browse";
            this.btnTemplateInitiator.UseVisualStyleBackColor = true;
            this.btnTemplateInitiator.Click += new System.EventHandler(this.btnTemplateInitiator_Click);
            // 
            // txtTemplateInitiator
            // 
            this.txtTemplateInitiator.Location = new System.Drawing.Point(197, 4);
            this.txtTemplateInitiator.Name = "txtTemplateInitiator";
            this.txtTemplateInitiator.ReadOnly = true;
            this.txtTemplateInitiator.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateInitiator.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "TemplateInitiator.bpel";
            // 
            // pnlBPELTemplate
            // 
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateResponderSpecialCase);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateResponderSpecialCase);
            this.pnlBPELTemplate.Controls.Add(this.label6);
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateResponderOneWay);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateResponderOneWay);
            this.pnlBPELTemplate.Controls.Add(this.label5);
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateResponder);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateResponder);
            this.pnlBPELTemplate.Controls.Add(this.label4);
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateInitiatorSpecialCase);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateInitiatorSpecialCase);
            this.pnlBPELTemplate.Controls.Add(this.label3);
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateInitiatorOneWay);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateInitiatorOneWay);
            this.pnlBPELTemplate.Controls.Add(this.label2);
            this.pnlBPELTemplate.Controls.Add(this.label1);
            this.pnlBPELTemplate.Controls.Add(this.txtTemplateInitiator);
            this.pnlBPELTemplate.Controls.Add(this.btnTemplateInitiator);
            this.pnlBPELTemplate.Location = new System.Drawing.Point(7, 105);
            this.pnlBPELTemplate.Name = "pnlBPELTemplate";
            this.pnlBPELTemplate.Size = new System.Drawing.Size(460, 168);
            this.pnlBPELTemplate.TabIndex = 7;
            this.pnlBPELTemplate.Visible = false;
            // 
            // txtTemplateResponderSpecialCase
            // 
            this.txtTemplateResponderSpecialCase.Location = new System.Drawing.Point(197, 143);
            this.txtTemplateResponderSpecialCase.Name = "txtTemplateResponderSpecialCase";
            this.txtTemplateResponderSpecialCase.ReadOnly = true;
            this.txtTemplateResponderSpecialCase.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateResponderSpecialCase.TabIndex = 21;
            // 
            // btnTemplateResponderSpecialCase
            // 
            this.btnTemplateResponderSpecialCase.Location = new System.Drawing.Point(402, 142);
            this.btnTemplateResponderSpecialCase.Name = "btnTemplateResponderSpecialCase";
            this.btnTemplateResponderSpecialCase.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateResponderSpecialCase.TabIndex = 20;
            this.btnTemplateResponderSpecialCase.Text = "Browse";
            this.btnTemplateResponderSpecialCase.UseVisualStyleBackColor = true;
            this.btnTemplateResponderSpecialCase.Click += new System.EventHandler(this.btnTemplateResponderSpecialCase_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "TemplateResponderSpecialCase.bpel";
            // 
            // txtTemplateResponderOneWay
            // 
            this.txtTemplateResponderOneWay.Location = new System.Drawing.Point(197, 117);
            this.txtTemplateResponderOneWay.Name = "txtTemplateResponderOneWay";
            this.txtTemplateResponderOneWay.ReadOnly = true;
            this.txtTemplateResponderOneWay.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateResponderOneWay.TabIndex = 18;
            // 
            // btnTemplateResponderOneWay
            // 
            this.btnTemplateResponderOneWay.Location = new System.Drawing.Point(402, 116);
            this.btnTemplateResponderOneWay.Name = "btnTemplateResponderOneWay";
            this.btnTemplateResponderOneWay.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateResponderOneWay.TabIndex = 17;
            this.btnTemplateResponderOneWay.Text = "Browse";
            this.btnTemplateResponderOneWay.UseVisualStyleBackColor = true;
            this.btnTemplateResponderOneWay.Click += new System.EventHandler(this.btnTemplateResponderOneWay_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "TemplateResponderOneWay.bpel";
            // 
            // txtTemplateResponder
            // 
            this.txtTemplateResponder.Location = new System.Drawing.Point(197, 88);
            this.txtTemplateResponder.Name = "txtTemplateResponder";
            this.txtTemplateResponder.ReadOnly = true;
            this.txtTemplateResponder.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateResponder.TabIndex = 15;
            // 
            // btnTemplateResponder
            // 
            this.btnTemplateResponder.Location = new System.Drawing.Point(402, 87);
            this.btnTemplateResponder.Name = "btnTemplateResponder";
            this.btnTemplateResponder.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateResponder.TabIndex = 14;
            this.btnTemplateResponder.Text = "Browse";
            this.btnTemplateResponder.UseVisualStyleBackColor = true;
            this.btnTemplateResponder.Click += new System.EventHandler(this.btnTemplateResponder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "TemplateResponder.bpel";
            // 
            // txtTemplateInitiatorSpecialCase
            // 
            this.txtTemplateInitiatorSpecialCase.Location = new System.Drawing.Point(197, 59);
            this.txtTemplateInitiatorSpecialCase.Name = "txtTemplateInitiatorSpecialCase";
            this.txtTemplateInitiatorSpecialCase.ReadOnly = true;
            this.txtTemplateInitiatorSpecialCase.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateInitiatorSpecialCase.TabIndex = 12;
            // 
            // btnTemplateInitiatorSpecialCase
            // 
            this.btnTemplateInitiatorSpecialCase.Location = new System.Drawing.Point(402, 58);
            this.btnTemplateInitiatorSpecialCase.Name = "btnTemplateInitiatorSpecialCase";
            this.btnTemplateInitiatorSpecialCase.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateInitiatorSpecialCase.TabIndex = 11;
            this.btnTemplateInitiatorSpecialCase.Text = "Browse";
            this.btnTemplateInitiatorSpecialCase.UseVisualStyleBackColor = true;
            this.btnTemplateInitiatorSpecialCase.Click += new System.EventHandler(this.btnTemplateInitiatorSpecialCase_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "TemplateInitiatorSpecialCase.bpel";
            // 
            // txtTemplateInitiatorOneWay
            // 
            this.txtTemplateInitiatorOneWay.Location = new System.Drawing.Point(197, 33);
            this.txtTemplateInitiatorOneWay.Name = "txtTemplateInitiatorOneWay";
            this.txtTemplateInitiatorOneWay.ReadOnly = true;
            this.txtTemplateInitiatorOneWay.Size = new System.Drawing.Size(199, 20);
            this.txtTemplateInitiatorOneWay.TabIndex = 9;
            // 
            // btnTemplateInitiatorOneWay
            // 
            this.btnTemplateInitiatorOneWay.Location = new System.Drawing.Point(402, 32);
            this.btnTemplateInitiatorOneWay.Name = "btnTemplateInitiatorOneWay";
            this.btnTemplateInitiatorOneWay.Size = new System.Drawing.Size(55, 23);
            this.btnTemplateInitiatorOneWay.TabIndex = 8;
            this.btnTemplateInitiatorOneWay.Text = "Browse";
            this.btnTemplateInitiatorOneWay.UseVisualStyleBackColor = true;
            this.btnTemplateInitiatorOneWay.Click += new System.EventHandler(this.btnTemplateInitiatorOneWay_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "TemplateInitiatorOneWay.bpel";
            // 
            // pnlXSLTTemplate
            // 
            this.pnlXSLTTemplate.Controls.Add(this.txtMappingTransactionProtocolSuccess);
            this.pnlXSLTTemplate.Controls.Add(this.btnMappingTransactionProtocolSuccess);
            this.pnlXSLTTemplate.Controls.Add(this.label9);
            this.pnlXSLTTemplate.Controls.Add(this.txtMappingTransactionProtocolFailure);
            this.pnlXSLTTemplate.Controls.Add(this.btnMappingTransactionProtocolFailure);
            this.pnlXSLTTemplate.Controls.Add(this.label10);
            this.pnlXSLTTemplate.Controls.Add(this.txtMappingReceiptException);
            this.pnlXSLTTemplate.Controls.Add(this.btnMappingReceiptException);
            this.pnlXSLTTemplate.Controls.Add(this.label11);
            this.pnlXSLTTemplate.Controls.Add(this.txtMappingReceiptAck);
            this.pnlXSLTTemplate.Controls.Add(this.btnMappingReceiptAck);
            this.pnlXSLTTemplate.Controls.Add(this.label12);
            this.pnlXSLTTemplate.Controls.Add(this.label13);
            this.pnlXSLTTemplate.Controls.Add(this.txtMappingGeneralException);
            this.pnlXSLTTemplate.Controls.Add(this.btnMappingGeneralException);
            this.pnlXSLTTemplate.Location = new System.Drawing.Point(7, 105);
            this.pnlXSLTTemplate.Name = "pnlXSLTTemplate";
            this.pnlXSLTTemplate.Size = new System.Drawing.Size(460, 168);
            this.pnlXSLTTemplate.TabIndex = 11;
            // 
            // txtMappingTransactionProtocolSuccess
            // 
            this.txtMappingTransactionProtocolSuccess.Location = new System.Drawing.Point(219, 134);
            this.txtMappingTransactionProtocolSuccess.Name = "txtMappingTransactionProtocolSuccess";
            this.txtMappingTransactionProtocolSuccess.ReadOnly = true;
            this.txtMappingTransactionProtocolSuccess.Size = new System.Drawing.Size(178, 20);
            this.txtMappingTransactionProtocolSuccess.TabIndex = 36;
            // 
            // btnMappingTransactionProtocolSuccess
            // 
            this.btnMappingTransactionProtocolSuccess.Location = new System.Drawing.Point(403, 133);
            this.btnMappingTransactionProtocolSuccess.Name = "btnMappingTransactionProtocolSuccess";
            this.btnMappingTransactionProtocolSuccess.Size = new System.Drawing.Size(54, 23);
            this.btnMappingTransactionProtocolSuccess.TabIndex = 35;
            this.btnMappingTransactionProtocolSuccess.Text = "Browse";
            this.btnMappingTransactionProtocolSuccess.UseVisualStyleBackColor = true;
            this.btnMappingTransactionProtocolSuccess.Click += new System.EventHandler(this.btnMappingTransactionProtocolSuccess_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(202, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "MappingTransactionProtocolSuccess.xslt";
            // 
            // txtMappingTransactionProtocolFailure
            // 
            this.txtMappingTransactionProtocolFailure.Location = new System.Drawing.Point(218, 103);
            this.txtMappingTransactionProtocolFailure.Name = "txtMappingTransactionProtocolFailure";
            this.txtMappingTransactionProtocolFailure.ReadOnly = true;
            this.txtMappingTransactionProtocolFailure.Size = new System.Drawing.Size(179, 20);
            this.txtMappingTransactionProtocolFailure.TabIndex = 33;
            // 
            // btnMappingTransactionProtocolFailure
            // 
            this.btnMappingTransactionProtocolFailure.Location = new System.Drawing.Point(403, 102);
            this.btnMappingTransactionProtocolFailure.Name = "btnMappingTransactionProtocolFailure";
            this.btnMappingTransactionProtocolFailure.Size = new System.Drawing.Size(54, 23);
            this.btnMappingTransactionProtocolFailure.TabIndex = 32;
            this.btnMappingTransactionProtocolFailure.Text = "Browse";
            this.btnMappingTransactionProtocolFailure.UseVisualStyleBackColor = true;
            this.btnMappingTransactionProtocolFailure.Click += new System.EventHandler(this.btnMappingTransactionProtocolFailure_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(192, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "MappingTransactionProtocolFailure.xslt";
            // 
            // txtMappingReceiptException
            // 
            this.txtMappingReceiptException.Location = new System.Drawing.Point(218, 72);
            this.txtMappingReceiptException.Name = "txtMappingReceiptException";
            this.txtMappingReceiptException.ReadOnly = true;
            this.txtMappingReceiptException.Size = new System.Drawing.Size(178, 20);
            this.txtMappingReceiptException.TabIndex = 30;
            // 
            // btnMappingReceiptException
            // 
            this.btnMappingReceiptException.Location = new System.Drawing.Point(403, 71);
            this.btnMappingReceiptException.Name = "btnMappingReceiptException";
            this.btnMappingReceiptException.Size = new System.Drawing.Size(54, 23);
            this.btnMappingReceiptException.TabIndex = 29;
            this.btnMappingReceiptException.Text = "Browse";
            this.btnMappingReceiptException.UseVisualStyleBackColor = true;
            this.btnMappingReceiptException.Click += new System.EventHandler(this.btnMappingReceiptException_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "MappingReceiptException.xslt";
            // 
            // txtMappingReceiptAck
            // 
            this.txtMappingReceiptAck.Location = new System.Drawing.Point(218, 38);
            this.txtMappingReceiptAck.Name = "txtMappingReceiptAck";
            this.txtMappingReceiptAck.ReadOnly = true;
            this.txtMappingReceiptAck.Size = new System.Drawing.Size(179, 20);
            this.txtMappingReceiptAck.TabIndex = 27;
            // 
            // btnMappingReceiptAck
            // 
            this.btnMappingReceiptAck.Location = new System.Drawing.Point(403, 38);
            this.btnMappingReceiptAck.Name = "btnMappingReceiptAck";
            this.btnMappingReceiptAck.Size = new System.Drawing.Size(54, 23);
            this.btnMappingReceiptAck.TabIndex = 26;
            this.btnMappingReceiptAck.Text = "Browse";
            this.btnMappingReceiptAck.UseVisualStyleBackColor = true;
            this.btnMappingReceiptAck.Click += new System.EventHandler(this.btnMappingReceiptAck_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(191, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "MappingReceiptAcknowledgement.xslt";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(150, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "MappingGeneralException.xslt";
            // 
            // txtMappingGeneralException
            // 
            this.txtMappingGeneralException.Location = new System.Drawing.Point(218, 4);
            this.txtMappingGeneralException.Name = "txtMappingGeneralException";
            this.txtMappingGeneralException.ReadOnly = true;
            this.txtMappingGeneralException.Size = new System.Drawing.Size(179, 20);
            this.txtMappingGeneralException.TabIndex = 23;
            // 
            // btnMappingGeneralException
            // 
            this.btnMappingGeneralException.Location = new System.Drawing.Point(403, 3);
            this.btnMappingGeneralException.Name = "btnMappingGeneralException";
            this.btnMappingGeneralException.Size = new System.Drawing.Size(54, 23);
            this.btnMappingGeneralException.TabIndex = 22;
            this.btnMappingGeneralException.Text = "Browse";
            this.btnMappingGeneralException.UseVisualStyleBackColor = true;
            this.btnMappingGeneralException.Click += new System.EventHandler(this.btnMappingGeneralException_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(320, 383);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(401, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlXSLTTemplate);
            this.groupBox2.Controls.Add(this.pnlReplaceAll);
            this.groupBox2.Controls.Add(this.rbtOneByOne);
            this.groupBox2.Controls.Add(this.rbtReplaceAll);
            this.groupBox2.Controls.Add(this.pnlBPELTemplate);
            this.groupBox2.Location = new System.Drawing.Point(12, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 281);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Method of Replacement";
            // 
            // pnlReplaceAll
            // 
            this.pnlReplaceAll.Controls.Add(this.txtReplaceAll);
            this.pnlReplaceAll.Controls.Add(this.label7);
            this.pnlReplaceAll.Controls.Add(this.btnReplaceAll);
            this.pnlReplaceAll.Enabled = false;
            this.pnlReplaceAll.Location = new System.Drawing.Point(7, 40);
            this.pnlReplaceAll.Name = "pnlReplaceAll";
            this.pnlReplaceAll.Size = new System.Drawing.Size(460, 34);
            this.pnlReplaceAll.TabIndex = 13;
            // 
            // txtReplaceAll
            // 
            this.txtReplaceAll.Location = new System.Drawing.Point(108, 7);
            this.txtReplaceAll.Name = "txtReplaceAll";
            this.txtReplaceAll.ReadOnly = true;
            this.txtReplaceAll.Size = new System.Drawing.Size(261, 20);
            this.txtReplaceAll.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Source of template";
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(382, 6);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.btnReplaceAll.TabIndex = 11;
            this.btnReplaceAll.Text = "Browse";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // rbtOneByOne
            // 
            this.rbtOneByOne.AutoSize = true;
            this.rbtOneByOne.Checked = true;
            this.rbtOneByOne.Location = new System.Drawing.Point(7, 82);
            this.rbtOneByOne.Name = "rbtOneByOne";
            this.rbtOneByOne.Size = new System.Drawing.Size(80, 17);
            this.rbtOneByOne.TabIndex = 9;
            this.rbtOneByOne.TabStop = true;
            this.rbtOneByOne.Text = "One by one";
            this.rbtOneByOne.UseVisualStyleBackColor = true;
            this.rbtOneByOne.Click += new System.EventHandler(this.rbtOneByOne_Click);
            // 
            // rbtReplaceAll
            // 
            this.rbtReplaceAll.AutoSize = true;
            this.rbtReplaceAll.Location = new System.Drawing.Point(6, 19);
            this.rbtReplaceAll.Name = "rbtReplaceAll";
            this.rbtReplaceAll.Size = new System.Drawing.Size(78, 17);
            this.rbtReplaceAll.TabIndex = 8;
            this.rbtReplaceAll.Text = "Replace all";
            this.rbtReplaceAll.UseVisualStyleBackColor = true;
            this.rbtReplaceAll.Click += new System.EventHandler(this.rbtReplaceAll_Click);
            // 
            // dlgBPELTemplate
            // 
            this.dlgBPELTemplate.Filter = "BPEL files|*.bpel";
            // 
            // dlgXSLTTemplate
            // 
            this.dlgXSLTTemplate.Filter = "XSLT files|*.xslt";
            // 
            // TemplateSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 418);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupBox1);
            this.Name = "TemplateSetting";
            this.Text = "BPEL-XSLT Template Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlBPELTemplate.ResumeLayout(false);
            this.pnlBPELTemplate.PerformLayout();
            this.pnlXSLTTemplate.ResumeLayout(false);
            this.pnlXSLTTemplate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlReplaceAll.ResumeLayout(false);
            this.pnlReplaceAll.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtBPELTemplate;
        private System.Windows.Forms.RadioButton rbtXSLTTemplate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTemplateInitiator;
        private System.Windows.Forms.TextBox txtTemplateInitiator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlBPELTemplate;
        private System.Windows.Forms.TextBox txtTemplateResponderSpecialCase;
        private System.Windows.Forms.Button btnTemplateResponderSpecialCase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTemplateResponderOneWay;
        private System.Windows.Forms.Button btnTemplateResponderOneWay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTemplateResponder;
        private System.Windows.Forms.Button btnTemplateResponder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTemplateInitiatorSpecialCase;
        private System.Windows.Forms.Button btnTemplateInitiatorSpecialCase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTemplateInitiatorOneWay;
        private System.Windows.Forms.Button btnTemplateInitiatorOneWay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.TextBox txtReplaceAll;
        private System.Windows.Forms.RadioButton rbtOneByOne;
        private System.Windows.Forms.RadioButton rbtReplaceAll;
        private System.Windows.Forms.Panel pnlXSLTTemplate;
        private System.Windows.Forms.TextBox txtMappingTransactionProtocolSuccess;
        private System.Windows.Forms.Button btnMappingTransactionProtocolSuccess;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMappingTransactionProtocolFailure;
        private System.Windows.Forms.Button btnMappingTransactionProtocolFailure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMappingReceiptException;
        private System.Windows.Forms.Button btnMappingReceiptException;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMappingReceiptAck;
        private System.Windows.Forms.Button btnMappingReceiptAck;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMappingGeneralException;
        private System.Windows.Forms.Button btnMappingGeneralException;
        private System.Windows.Forms.FolderBrowserDialog dlgReplaceAll;
        private System.Windows.Forms.OpenFileDialog dlgBPELTemplate;
        private System.Windows.Forms.OpenFileDialog dlgXSLTTemplate;
        private System.Windows.Forms.Panel pnlReplaceAll;
    }
}