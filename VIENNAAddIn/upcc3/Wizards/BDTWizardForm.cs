using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards
{
    ///<summary>
    ///</summary>
    public partial class BDTWizardForm : Form
    {
        private CCRepository repository;
        private Cache cache;
        private string selectedCDTLName;
        private string selectedCDTName;
        private string selectedBDTLName;
        private string selectedSUPName;
        private Label errorMessageBDTName;

        ///<summary>
        ///</summary>
        ///<param name="eaRepository"></param>
        public BDTWizardForm(EA.Repository eaRepository)
        {
            InitializeComponent();

            repository = new CCRepository(eaRepository);
            cache = new Cache();

            foreach (ICDTLibrary cdtl in repository.Libraries<ICDTLibrary>())
            {
                IDictionary<string, CCDT> cdts = new Dictionary<string, CCDT>();

                foreach (ICDT cdt in cdtl.CDTs)
                {
                    cdts.Add(cdt.Name, new CCDT(cdt.Name, cdt.Id));
                }

                cache.CCDTLs.Add(cdtl.Name, new CCDTL(cdtl.Name, cdtl.Id, cdts));
            }

            foreach (IBDTLibrary bdtl in repository.Libraries<IBDTLibrary>())
            {
                IDictionary<string, CBDT> bdts = new Dictionary<string, CBDT>();

                foreach (IBDT bdt in bdtl.BDTs)
                {
                    bdts.Add(bdt.Name, new CBDT(bdt.Name, bdt.Id, bdt.BasedOn.CDT.Id, bdtl.Id));
                }

                cache.CBDTLs.Add(bdtl.Name, new CBDTL(bdtl.Name, bdtl.Id, bdts));
            }

            comboCDTLs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCDTs.DropDownStyle = ComboBoxStyle.DropDownList;

            errorMessageBDTName = new Label
            {
                Size = new Size(100, 17),
                Location = new Point(0, 0),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                BackColor = Color.Yellow
            };

            groupboxSettings.Controls.Add(errorMessageBDTName);            
            errorMessageBDTName.Hide();
        }

        private void BDTWizardForm_Load(object sender, EventArgs e)
        {
            MirrorCDTLsToUI();
            MirrorBDTLsToUI();
            
            comboBDTLs.SelectedIndex = 0;
        }

        private void comboCDTLs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MirrorCDTsToUI();
        }

        private void GatherUserInput()
        {
            selectedCDTLName = comboCDTLs.SelectedIndex >= 0 ? comboCDTLs.SelectedItem.ToString() : "";
            selectedCDTName = comboCDTs.SelectedIndex >= 0 ? comboCDTs.SelectedItem.ToString() : "";
            selectedBDTLName = comboBDTLs.SelectedIndex >= 0 ? comboBDTLs.SelectedItem.ToString() : "";                     
            selectedSUPName = checkedlistboxSUPs.SelectedIndex >= 0 ? checkedlistboxSUPs.SelectedItem.ToString() : "";
        }

        private void MirrorCDTLsToUI()
        {
            comboCDTLs.Items.Clear();

            foreach (CCDTL cdtl in cache.CCDTLs.Values)
            {
                comboCDTLs.Items.Add(cdtl.Name);
            }
        }

        private void MirrorCDTsToUI()
        {
            GatherUserInput();
            
            comboCDTs.Items.Clear();

            foreach (CCDT cdt in cache.CCDTLs[selectedCDTLName].CDTs.Values)
            {
                comboCDTs.Items.Add(cdt.Name);
            }
        }

        private void MirrorBDTLsToUI()
        {
            comboBDTLs.Items.Clear();
            
            foreach (CBDTL bdtl in cache.CBDTLs.Values)
            {
                comboBDTLs.Items.Add(bdtl.Name);
            }
        }

        private void MirrorAttributesToUI()
        {
            GatherUserInput();
            CCDT currentCDT = cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName];


            checkedlistboxCON.Items.Clear();
            checkedlistboxCON.Items.Add(currentCDT.CON.Name, currentCDT.CON.State);
            
            checkedlistboxSUPs.Items.Clear();            
            foreach (CSUP sup in currentCDT.SUPs.Values)
            {
                checkedlistboxSUPs.Items.Add(sup.Name, sup.State);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboCDTs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GatherUserInput();

            textBDTName.Text = "My" + selectedCDTName;

            // CDT has not been cashed previously already
            if (cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].CON == null)
            {
                int cdtId = cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].Id;
                ICDT cdt = repository.GetCDT(cdtId);

                cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].CON = new CCON(cdt.CON.Name, cdt.CON.Id, CheckState.Checked);

                IDictionary<string, CSUP> sups = new Dictionary<string, CSUP>();

                foreach (ISUP sup in cdt.SUPs)
                {
                    sups.Add(sup.Name, new CSUP(sup.Name, sup.Id, CheckState.Unchecked));
                }

                cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs = sups;
            }

            MirrorAttributesToUI();
        }

        private void checkboxAttributes_CheckedChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            CheckState newState = CheckState.Unchecked;

            if (checkboxAttributes.Checked)
            {
                newState = CheckState.Checked;
            }

            foreach (KeyValuePair<string, CSUP> sup in cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs)
            {
                sup.Value.State = newState;
            }

            MirrorAttributesToUI();
        }

        private void buttonGenerateBDT_Click(object sender, EventArgs e)
        {
            GatherUserInput();

            int cdtId = cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].Id;            
            ICDT cdt = repository.GetCDT(cdtId);
            int bdtId = cache.CBDTLs[selectedBDTLName].Id;
            IBDTLibrary bdtl = (IBDTLibrary) repository.GetLibrary(bdtId);

            BDTSpec bdtSpec = BDTSpec.CloneCDT(cdt, textBDTName.Text);

            foreach (CSUP sup in cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs.Values)
            {
                if (sup.State == CheckState.Unchecked)
                {
                    bdtSpec.RemoveSUP(sup.Name);
                    //MessageBox.Show("remove SUP: " + sup.Name);
                }
            }            

            bdtl.CreateBDT(bdtSpec);
        }

        private void textBDTName_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();
            
            if (cache.CBDTLs[selectedBDTLName].BDTs.ContainsKey(textBDTName.Text))
            {
                errorMessageBDTName.Location = new Point(textBDTName.Location.X + textBDTName.Width - 105, textBDTName.Location.Y);
                errorMessageBDTName.Text = "BDT named \"" + textBDTName.Text + "\" alreay exists!";
                errorMessageBDTName.BringToFront();
                errorMessageBDTName.Show();                
            }
            else
            {
                errorMessageBDTName.Hide();
            }
        }

        private void checkedlistboxSUPs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (!(selectedCDTLName.Equals("")) &&
                !(selectedCDTName.Equals("")) &&
                !(selectedSUPName.Equals("")) &&
                !(selectedBDTLName.Equals("")))
            {
                cache.CCDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs[selectedSUPName].State = e.NewValue;
            }
        }
    }
}
