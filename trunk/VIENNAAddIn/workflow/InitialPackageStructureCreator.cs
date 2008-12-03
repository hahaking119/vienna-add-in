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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIENNAAddIn.WorkFlow;


namespace VIENNAAddIn.workflow
{
    public partial class InitialPackageStructureCreator : Form
    {

        private EA.Repository repository = null;




        public InitialPackageStructureCreator(EA.Repository repository)
        {
            InitializeComponent();

            this.repository = repository;

        }

        /// <summary>
        /// Set the name of all vies to the model name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useForAllButton_Click(object sender, EventArgs e)
        {
            //Get the text of the model name field
            String s = this.modelName.Text;

            if (s == null || s.Equals(""))
            {
                MessageBox.Show("Please specificy a model name first.", "Error");
            }
            else
            {
                this.nameBCV.Text = s;
                this.nameBIV.Text = s;
                this.nameBRV.Text = s;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Create the inital package structure
        private void createButton_Click(object sender, EventArgs e)
        {

            //BRV checked?
            if (checkBRV.Checked == true)
            {
                //Name for BRV set
                if (nameBRV.Text == null || nameBRV.Text.Equals(""))
                {
                    MessageBox.Show("Please select a name for the Business Requirements View.", "Error");
                    return;
                }          
            }

            //Name for BCV set?
            if (nameBCV.Text == null || nameBCV.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Choreography View.", "Error");
                return;             
            }


            //Name for BIV set?
            if (nameBIV.Text == null || nameBIV.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Information View.", "Error");
                return;
            }

            //No errors - Last Check

			DialogResult dr = MessageBox.Show("This will DELETE ALL CONTENTS from current selected model.\n"+
					"Are you sure?","UMMAddIn Question",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);
			// if yes proceed
            if (dr.Equals(DialogResult.Yes))
            {
                    try {
                		ModelStructureCreator msc = new ModelStructureCreator(this.repository);
						msc.create(modelName.Text,nameBRV.Text,nameBCV.Text,nameBIV.Text, checkBRV.Checked);
						this.Close();
						return;
					}
					catch(Exception ex) {
						this.Close();
						MessageBox.Show("An error occured during creating the UMM model structure: " + ex.Message + "\n"+ex.InnerException.StackTrace,"UMMAddIn Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
            }
            else if (dr.Equals(DialogResult.Cancel)) {
                this.Close();
                return;
            }
            else {
                return;
            }
        }
    }
}
