/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VIENNAAddIn.common;
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.WSDLGenerator.Setting
{
    public partial class TemplateSetting : Form
    {
        #region Variables
        //private string pathMappingGeneralException = "";
        //private string pathMappingReceiptAck = "";
        
        private string pathReplaceAll = "";

        #endregion

        #region Constructor
        public TemplateSetting()
        {
            InitializeComponent();
        }
        #endregion


        public static void ShowForm()
        {
            new TemplateSetting().Show();
        }

        private void rbtBPELTemplate_Click(object sender, EventArgs e)
        {
            if (pnlBPELTemplate.Visible == false)
            {
                if (rbtOneByOne.Checked)
                {
                    pnlBPELTemplate.Visible = true;
                    pnlBPELTemplate.Enabled = true;

                    pnlXSLTTemplate.Visible = false;
                }
                else
                {
                    pnlBPELTemplate.Visible = true;
                    pnlBPELTemplate.Enabled = false;

                    pnlXSLTTemplate.Visible = false;
                }
            }
        }

        private void rbtXSLTTemplate_Click(object sender, EventArgs e)
        {
            if (pnlXSLTTemplate.Visible == false)
            {
                if (rbtOneByOne.Checked)
                {
                    pnlBPELTemplate.Visible = false;

                    pnlXSLTTemplate.Visible = true;
                    pnlXSLTTemplate.Enabled = true;
                }
                else
                {
                    pnlBPELTemplate.Visible = false;

                    pnlXSLTTemplate.Visible = true;
                    pnlXSLTTemplate.Enabled = false;
                }
            }
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgReplaceAll.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtReplaceAll.Text = dlgReplaceAll.SelectedPath;
                this.pathReplaceAll = dlgReplaceAll.SelectedPath;
            }
        }

        private void rbtReplaceAll_Click(object sender, EventArgs e)
        {
            if (rbtBPELTemplate.Checked)
                pnlBPELTemplate.Enabled = false;
            else
                pnlXSLTTemplate.Enabled = false;
            
            pnlReplaceAll.Enabled = true;
        }

        private void rbtOneByOne_Click(object sender, EventArgs e)
        {
            if (rbtBPELTemplate.Checked)
                pnlBPELTemplate.Enabled = true;
            else
                pnlXSLTTemplate.Enabled = true;

            pnlReplaceAll.Enabled = false;
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool error = false;
            try
            {
                if (rbtBPELTemplate.Checked) //BPEL Template checked
                {
                    if (rbtReplaceAll.Checked)
                    {
                        string sourcePath = txtReplaceAll.Text;
                        string destination = AddInSettings.BPELTemplatePath;

                        replaceTemplate(sourcePath, destination);
                    }
                    else
                    {
                        replaceBPELTemplateOneByOne();
                    }
                }
                else //XSLT Template Checked
                {
                    if (rbtReplaceAll.Checked)
                    {
                        string sourcePath = txtReplaceAll.Text;
                        string destination = AddInSettings.BPELTemplatePath + "Mappings";

                        replaceTemplate(sourcePath, destination);
                    }
                    else
                    {
                        replaceXSLTTemplateOneByOne();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed updating template caused by: " + ex.Message + ".\nPlease try again", "Error", MessageBoxButtons.OK);
                error = true;
            }

            if (!error)
                MessageBox.Show("Successfully update template.");

            this.Close();
        }

        private void replaceXSLTTemplateOneByOne()
        {
            string destination = AddInSettings.BPELTemplatePath + "Mappings";

            if (txtMappingGeneralException.Text != "")
            {
                string tempDest = destination + "MappingGeneralException.xslt";
                System.IO.File.Copy(txtMappingGeneralException.Text, tempDest, true);
            }

            if (txtMappingReceiptAck.Text != "")
            {
                string tempDest = destination + "MappingReceiptAck.xslt";
                System.IO.File.Copy(txtMappingReceiptAck.Text, tempDest, true);
            }

            if (txtMappingReceiptException.Text != "")
            {
                string tempDest = destination + "MappingReceiptException.xslt";
                System.IO.File.Copy(txtMappingReceiptException.Text, tempDest, true);
            }

            if (txtMappingTransactionProtocolFailure.Text != "") 
            {
                string tempDest = destination + "MappingTransactionProtocolFailure.xslt";
                System.IO.File.Copy(txtMappingTransactionProtocolFailure.Text, tempDest, true);
            }

            if (txtMappingTransactionProtocolSuccess.Text != "")
            {
                string tempDest = destination + "MappingTransactionProtocolSuccess.xslt";
                System.IO.File.Copy(txtMappingTransactionProtocolSuccess.Text, tempDest, true);
            }
        }

        private void replaceBPELTemplateOneByOne()
        {
            string destination = AddInSettings.BPELTemplatePath;

            if (txtTemplateInitiator.Text != "")
            {
                string tempDest = destination + "TemplateInitiator.bpel";
                System.IO.File.Copy(txtTemplateInitiator.Text, tempDest, true);
            }

            if (txtTemplateInitiatorOneWay.Text != "")
            {
                string tempDest = destination + "TemplateInitiatorOneWay.bpel";
                System.IO.File.Copy(txtTemplateInitiatorOneWay.Text, tempDest, true);
            }

            if (txtTemplateInitiatorSpecialCase.Text != "")
            {
                string tempDest = destination + "TemplateInitiatorSpecialCase.bpel";
                System.IO.File.Copy(txtTemplateInitiatorSpecialCase.Text, tempDest, true);
            }

            if (txtTemplateResponder.Text != "")
            {
                string tempDest = destination + "TemplateResponder.bpel";
                System.IO.File.Copy(txtTemplateResponder.Text, tempDest, true);
            }

            if (txtTemplateResponderOneWay.Text != "")
            {
                string tempDest = destination + "TemplateResponderOneWay.bpel";
                System.IO.File.Copy(txtTemplateResponderOneWay.Text, tempDest, true);
            }

            if (txtTemplateResponderSpecialCase.Text != "")
            {
                string tempDest = destination + "TemplateResponderSpecialCase.bpel";
                System.IO.File.Copy(txtTemplateResponderSpecialCase.Text, tempDest, true);
            }
        }

        private void replaceTemplate(string source, string destination)
        {
            String[] files;

            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                destination += Path.DirectorySeparatorChar;

            files = Directory.GetFileSystemEntries(source);
            foreach (string Element in files)
            {
                System.IO.File.Copy(Element, destination + Path.GetFileName(Element), true);
            }
        }

        #region Button XSLT
        
        private void btnMappingGeneralException_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgXSLTTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtMappingGeneralException.Text = dlgXSLTTemplate.FileName;
                //this.pathMappingGeneralException = dlgXSLTTemplate.FileName;
            }
        }

        private void btnMappingReceiptAck_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgXSLTTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtMappingReceiptAck.Text = dlgXSLTTemplate.FileName;
                //this.pathMappingReceiptAck = dlgXSLTTemplate.FileName;
            }
        }

        private void btnMappingReceiptException_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgXSLTTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtMappingReceiptException.Text = dlgXSLTTemplate.FileName;
                //this.pathMappingReceiptException = dlgXSLTTemplate.FileName;
            }
        }

        private void btnMappingTransactionProtocolFailure_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgXSLTTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtMappingTransactionProtocolFailure.Text = dlgXSLTTemplate.FileName;
                //this.pathMappingTransactionProtocolFailure = dlgXSLTTemplate.FileName;
            }
        }

        private void btnMappingTransactionProtocolSuccess_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgXSLTTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtMappingTransactionProtocolSuccess.Text = dlgXSLTTemplate.FileName;
                //this.pathMappingTransactionProtocolSuccess = dlgXSLTTemplate.FileName;
            }
        }
        #endregion

        #region Button BPEL
        private void btnTemplateInitiator_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateInitiator.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateInitiator = dlgBPELTemplate.FileName;
            }
        }

        private void btnTemplateInitiatorOneWay_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateInitiatorOneWay.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateInitiatorOneWay = dlgBPELTemplate.FileName;
            }
        }

        private void btnTemplateInitiatorSpecialCase_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateInitiatorSpecialCase.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateInitiatorSpecialCase = dlgBPELTemplate.FileName;
            }
        }

        private void btnTemplateResponder_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateResponder.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateResponder = dlgBPELTemplate.FileName;
            }
        }

        private void btnTemplateResponderOneWay_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateResponderOneWay.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateResponderOneWay = dlgBPELTemplate.FileName;
            }
        }


        private void btnTemplateResponderSpecialCase_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgBPELTemplate.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTemplateResponderSpecialCase.Text = dlgBPELTemplate.FileName;
                //this.pathTemplateResponderSpecialCase = dlgBPELTemplate.FileName;
            }
        }
        #endregion 

    }
}