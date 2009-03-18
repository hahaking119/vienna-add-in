/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.Setting
{
    public partial class SynchTaggedValue : Form
    {
        #region Variables

        private readonly Repository repository;
        private string scope;

        #endregion

        #region Constructor

        public SynchTaggedValue()
        {
            InitializeComponent();
        }

        public SynchTaggedValue(Repository repository, string scope)
        {
            InitializeComponent();
            this.repository = repository;
            this.scope = scope;
        }

        #endregion

        private void SynchTaggedValue_Load(object sender, EventArgs e)
        {
            cmbProfile.Items.Add("Business Requirement View");
            cmbProfile.Items.Add("Business Information View");
            cmbProfile.Items.Add("Business Choreography View");
        }

        private void cmbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsbStereotype.Items.Clear();
            if (cmbProfile.Items[cmbProfile.SelectedIndex].ToString().Equals("Business Requirement View"))
            {
                foreach (string item in Enum.GetNames(typeof (UMM)))
                {
                    lsbStereotype.Items.Add(item);
                }
            }
            else if (cmbProfile.Items[cmbProfile.SelectedIndex].ToString().Equals("Business Information View"))
            {
                foreach (string item in Enum.GetNames(typeof (UMM)))
                {
                    lsbStereotype.Items.Add(item);
                }
            }
            else if (cmbProfile.Items[cmbProfile.SelectedIndex].ToString().Equals("Business Choreography View"))
            {
                foreach (string item in Enum.GetNames(typeof (UMM)))
                {
                    lsbStereotype.Items.Add(item);
                }
            }
        }

        private void btnSynchronize_Click(object sender, EventArgs e)
        {
            DialogResult result =
                MessageBox.Show("This process may need some time to finish.\nAre you sure want to continue?",
                                "Confirmation", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                if (rbtOneByOne.Checked)
                {
                    //get combo value
                    switch (cmbProfile.Items[cmbProfile.SelectedIndex].ToString())
                    {
                        case "Business Requirement View":
                            repository.CustomCommand("Repository", "SynchProfile",
                                                     "Profile=BRV;Stereotype=" +
                                                     lsbStereotype.Items[lsbStereotype.SelectedIndex] + ";");
                            break;
                        case "Business Information View":
                            repository.CustomCommand("Repository", "SynchProfile",
                                                     "Profile=BIV;Stereotype=" +
                                                     lsbStereotype.Items[lsbStereotype.SelectedIndex] + ";");
                            break;
                        case "Business Choreography View":
                            repository.CustomCommand("Repository", "SynchProfile",
                                                     "Profile=BCV;Stereotype=" +
                                                     lsbStereotype.Items[lsbStereotype.SelectedIndex] + ";");
                            break;
                    }
                }
                else if (rbtAll.Checked)
                {
                    foreach (string enumStereotype in Enum.GetNames(typeof (UMM)))
                    {
                        string tempStereotype = enumStereotype;
                        repository.CustomCommand("Repository", "SynchProfile",
                                                 "Profile=BIV;Stereotype=" + tempStereotype + ";");
                    }
                    foreach (string enumStereotype in Enum.GetNames(typeof (UMM)))
                    {
                        string tempStereotype = enumStereotype;
                        repository.CustomCommand("Repository", "SynchProfile",
                                                 "Profile=BRV;Stereotype=" + tempStereotype + ";");
                    }
                    foreach (string enumStereotype in Enum.GetNames(typeof (UMM)))
                    {
                        string tempStereotype = enumStereotype;
                        repository.CustomCommand("Repository", "SynchProfile",
                                                 "Profile=BCV;Stereotype=" + tempStereotype + ";");
                    }
                }
            }
        }

        private void rbtAll_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = false;
        }

        private void rbtOneByOne_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}