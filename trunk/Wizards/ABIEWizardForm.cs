using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace Wizards
{
    public partial class ABIEWizardForm : Form
    {
        #region Variable Declarations

        private CCRepository repository;
        private Cache cache;
        private Label errorMessageABIE;
        private Label errorMessageBDT;
        private TextBox editboxBBIEName;
        private TextBox editboxBDTName;

        private string selectedCCLName;
        private string selectedACCName;
        private string selectedBCCName;
        private string selectedBBIEName;
        private string selectedBDTName;
        private string selectedBDTLName;
        private string selectedBIELName;

        #endregion

        #region Convenience Methods

        private void GatherUserInput()
        {
            selectedCCLName = comboCCLs.SelectedIndex >= 0 ? comboCCLs.SelectedItem.ToString() : "";

            selectedACCName = comboACCs.SelectedIndex >= 0 ? comboACCs.SelectedItem.ToString() : "";

            selectedBCCName = checkedlistboxBCCs.SelectedIndex >= 0 ? checkedlistboxBCCs.SelectedItem.ToString() : "";

            selectedBBIEName = checkedlistboxBBIEs.SelectedIndex >= 0 ? checkedlistboxBBIEs.SelectedItem.ToString() : "";

            selectedBDTName = checkedlistboxBDTs.SelectedIndex >= 0 ? checkedlistboxBDTs.SelectedItem.ToString() : "";

            selectedBDTLName = comboBDTLs.SelectedIndex >= 0 ? comboBDTLs.SelectedItem.ToString() : "";

            selectedBIELName = comboBIELs.SelectedIndex >= 0 ? comboBIELs.SelectedItem.ToString() : "";    
        }

        private void MirrorCCLibrariesToUI()
        {
            comboCCLs.Items.Clear();

            foreach (CCCL ccl in cache.CCLs.Values)
            {
                comboCCLs.Items.Add(ccl.Name);
            }
        }

        private void MirrorBDTLibrariesToUI()
        {
            comboBDTLs.Items.Clear();

            foreach (CBDTL bdtl in cache.CBDTLs.Values)
            {
                comboBDTLs.Items.Add(bdtl.Name);
            }
        }

        private void MirrorBIELibrariesToUI()
        {
            comboBIELs.Items.Clear();

            foreach (CBIEL biel in cache.CBIELs.Values)
            {
                comboBIELs.Items.Add(biel.Name);
            }
        }

        private void MirrorACCsToUI()
        {
            comboACCs.Items.Clear();
            
            foreach (CACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
            {
                comboACCs.Items.Add(acc.Name);
            }
        }

        private void MirrorBCCsToUI()
        {
            checkedlistboxBCCs.Items.Clear();

            foreach (CBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
            {
                checkedlistboxBCCs.Items.Add(bcc.Name, bcc.State);
            }
        }
        
        private void MirrorBBIEsToUI()
        {
            checkedlistboxBBIEs.Items.Clear();
            
            foreach (CBBIE bbie in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Values)
            {
                checkedlistboxBBIEs.Items.Add(bbie.Name, bbie.State);
            }
        }

        private void MirrorBDTsToUI()
        {
            GatherUserInput();
            
            checkedlistboxBDTs.Items.Clear();

            foreach (CBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
            {
                checkedlistboxBDTs.Items.Add(bdt.Name, bdt.State);
            }
        }

        private IList<CBDT> GetRelevantBDTs(int cdtid)
        {
            IList<CBDT> relevantBdts = new List<CBDT>();

            foreach (CBDTL bdtl in cache.CBDTLs.Values)
            {
                foreach (CBDT bdt in bdtl.BDTs.Values)
                {
                    if (bdt.BasedOn == cdtid)
                    {
                        relevantBdts.Add(new CBDT(bdt.Name, bdt.Id, CheckState.Unchecked, bdt.BasedOn, bdt.InLibrary));
                    }
                }
            }

            relevantBdts.Add(new CBDT("Create new BDT", -1, CheckState.Unchecked, cdtid, -1));                      

            return relevantBdts;
        }

        #endregion

        #region Constructor

        public ABIEWizardForm(EA.Repository eaRepository)
        {
            InitializeComponent();

            repository = new CCRepository(eaRepository);
            cache = new Cache();

            foreach (ICCLibrary ccLibrary in repository.Libraries<CCLibrary>())
            {
                cache.CCLs.Add(ccLibrary.Name, new CCCL(ccLibrary.Name, ccLibrary.Id));
            }
            
            foreach (IBIELibrary bieLibrary in repository.Libraries<IBIELibrary>())
            {
                IDictionary<string, CABIE> abies = new Dictionary<string, CABIE>();

                foreach (IABIE abie in bieLibrary.BIEs)
                {
                    abies.Add(abie.Name, new CABIE(abie.Name, abie.Id));
                }

                cache.CBIELs.Add(bieLibrary.Name, new CBIEL(bieLibrary.Name, bieLibrary.Id, abies));
            }

            foreach (IBDTLibrary bdtLibrary in repository.Libraries<IBDTLibrary>())
            {
                IDictionary<string, CBDT> bdts = new Dictionary<string, CBDT>();

                foreach (IBDT bdt in bdtLibrary.BDTs)
                {
                    bdts.Add(bdt.Name, new CBDT(bdt.Name, bdt.Id, bdt.BasedOn.CDT.Id, bdtLibrary.Id));
                }

                cache.CBDTLs.Add(bdtLibrary.Name, new CBDTL(bdtLibrary.Name, bdtLibrary.Id, bdts));
            }

            errorMessageABIE = new Label
                                   {
                                       Size = new Size(200, 17),
                                       Location = new Point(0, 0),
                                       BorderStyle = BorderStyle.FixedSingle,
                                       ForeColor = Color.Black,
                                       BackColor = Color.Yellow                                       
                                   };
            groupboxSettings.Controls.Add(errorMessageABIE);
            errorMessageABIE.Hide();

            errorMessageBDT = new Label
                                  {
                                      Size = new Size(199, 17),
                                      Location = new Point(0, 0),
                                      BorderStyle = BorderStyle.FixedSingle,
                                      ForeColor = Color.Black,
                                      BackColor = Color.Yellow
                                  };
            groupboxSettings.Controls.Add(errorMessageBDT);
            errorMessageBDT.Hide();

            editboxBBIEName = new TextBox
                                  {
                                      Size = new Size(0, 0),
                                      Location = new Point(0, 0),
                                      Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel),
                                      ForeColor = Color.Black,
                                      BackColor = Color.White,
                                      BorderStyle = BorderStyle.FixedSingle,
                                      Text = ""                                      
                                  };
            editboxBBIEName.KeyPress += EditBBIENameOver;
            editboxBBIEName.LostFocus += FocusLeftEditBBIEName;
            checkedlistboxBBIEs.Controls.Add(editboxBBIEName);
            editboxBBIEName.Hide();

            editboxBDTName = new TextBox
            {
                Size = new Size(0, 0),
                Location = new Point(0, 0),
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular, GraphicsUnit.Pixel),
                ForeColor = Color.Black,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = ""
            };
            editboxBDTName.KeyPress += EditBDTNameOver;
            editboxBDTName.LostFocus += FocusLeftEditBDTName;
            checkedlistboxBDTs.Controls.Add(editboxBDTName);
            editboxBDTName.Hide();

            comboCCLs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboACCs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBDTLs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBIELs.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion

        #region Event Handlers

        private void ABIEWizardForm_Load(object sender, System.EventArgs e)
        {
            MirrorCCLibrariesToUI();
            MirrorBDTLibrariesToUI();
            MirrorBIELibrariesToUI();

            comboBDTLs.SelectedIndex = 0;
            comboBIELs.SelectedIndex = 0;
        }

        private void comboCCLs_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            selectedCCLName = comboCCLs.SelectedItem.ToString();

            int cclID = cache.CCLs[selectedCCLName].Id;
            ICCLibrary ccl = (ICCLibrary)repository.GetLibrary(cclID);

            if (cache.CCLs[selectedCCLName].ACCs.Count == 0)
            {
                foreach (IACC acc in ccl.ACCs)
                {
                    cache.CCLs[selectedCCLName].ACCs.Add(acc.Name, new CACC(acc.Name, acc.Id));
                }
            }

            MirrorACCsToUI();            
        }

        private void comboACCs_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            selectedACCName = comboACCs.SelectedItem.ToString();

            int accID = cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id;
            IACC acc = repository.GetACC(accID);

            if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Count == 0)
            {
                foreach (IBCC bcc in acc.BCCs)
                {
                    IList<CBDT> appropriateBDTs = GetRelevantBDTs(bcc.Type.Id);

                    IDictionary<string, CBBIE> newbbies = new Dictionary<string, CBBIE>();
                    newbbies.Add(bcc.Name, new CBBIE(bcc.Name, bcc.Id, CheckState.Unchecked, appropriateBDTs));

                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Add(bcc.Name, new CBCC(bcc.Name, bcc.Id, bcc.Type.Id, CheckState.Unchecked, newbbies));
                }
            }

            textABIEName.Text = selectedACCName;

            MirrorBCCsToUI();

            checkedlistboxBCCs.SelectedIndex = 0;

            
        }

        private void checkedlistboxBCCs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GatherUserInput();

            if (!(selectedBCCName.Equals("")))
            {
                selectedBCCName = checkedlistboxBCCs.SelectedItem.ToString();

                MirrorBBIEsToUI();

                checkedlistboxBBIEs.SelectedIndex = 0;
            }
        }

        private void checkedlistboxBBIEs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GatherUserInput();

            if (!(selectedBCCName.Equals("")) &&
                !(selectedBBIEName.Equals("")))
            {
                selectedBBIEName = checkedlistboxBBIEs.SelectedItem.ToString();

                MirrorBDTsToUI();

                checkedlistboxBDTs.SelectedIndex = 0;
            }
        }

        private void checkedlistboxBDTs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            GatherUserInput();

            if (!(selectedBCCName.Equals("")) &&
                !(selectedBBIEName.Equals("")) &&
                !(selectedBDTName.Equals("")))
            {
                selectedBDTName = checkedlistboxBDTs.SelectedItem.ToString();
            }
        }

        private void buttonGenerate_Click(object sender, System.EventArgs e)
        {
            GatherUserInput();
            int selectedBDTLID = cache.CBDTLs[selectedBDTLName].Id;                
            IBDTLibrary selectedBDTL = (IBDTLibrary)repository.GetLibrary(selectedBDTLID);

            int selectedBIELID = cache.CBIELs[selectedBIELName].Id;
            IBIELibrary selectedBIEL = (IBIELibrary)repository.GetLibrary(selectedBIELID);

            /* get the selected ACC which we as a basis to generate the new ABIE */
            int selectedACCID = cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id;
            IACC selectedACC = repository.GetACC(selectedACCID);

            List<BBIESpec> newBBIEs = new List<BBIESpec>();
            IDictionary<string, CBDT> generatedBDTs = new Dictionary<string, CBDT>();

            /* iterate through the bccs */            
            foreach (CBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
            {
                /* only process those bccs that are selected */
                if (bcc.State == CheckState.Checked)
                {
                    /* iterate through the bbies that are based on the bcc */
                    foreach (CBBIE bbie in bcc.BBIEs.Values)
                    {
                        if (bbie.State == CheckState.Checked)
                        {
                            /* iterate through the bdts that are available for the bbie */
                            foreach (CBDT bdt in bbie.BDTs)
                            {
                                if (bdt.State == CheckState.Checked)
                                {
                                    IBDT bdtUsed;

                                    if (bdt.Id == -1)
                                    {
                                        // check if datatype has been generated previously
                                        if (!(generatedBDTs.ContainsKey(bdt.Name)))
                                        {
                                            /* the BDT to be used is to be created based on the CDT used in the BCC */
                                            ICDT baseCDT = repository.GetCDT(bcc.Type);
                                            BDTSpec bdtSpec = BDTSpec.CloneCDT(baseCDT, bdt.Name);
                                            IBDT newBDT = selectedBDTL.CreateBDT(bdtSpec);
                                            bdtUsed = newBDT;

                                            generatedBDTs.Add(newBDT.Name, new CBDT(newBDT.Name, newBDT.Id, newBDT.BasedOn.CDT.Id, selectedBDTL.Id));                                            
                                        }
                                        else
                                        {
                                            bdtUsed = repository.GetBDT(generatedBDTs[bdt.Name].Id);
                                        }
                                    }
                                    else
                                    {
                                        bdtUsed = repository.GetBDT(bdt.Id);
                                    }

                                    foreach (IBCC currentBCC in selectedACC.BCCs)
                                    {
                                        if (currentBCC.Id == bcc.Id)
                                        {
                                            /* now create the new bbie */
                                            BBIESpec x = BBIESpec.CloneBCC(currentBCC, bdtUsed);
                                            x.Name = bbie.Name;
                                            newBBIEs.Add(x);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }


            ABIESpec abieSpec = new ABIESpec
            {
                Name = textABIEName.Text,
                DictionaryEntryName = selectedACC.DictionaryEntryName,
                Definition = selectedACC.Definition,
                UniqueIdentifier = selectedACC.UniqueIdentifier,
                VersionIdentifier = selectedACC.VersionIdentifier,
                LanguageCode = selectedACC.LanguageCode,
                BusinessTerms = selectedACC.BusinessTerms,
                UsageRules = selectedACC.UsageRules,
                BasedOn = selectedACC,
                BBIEs = newBBIEs,
            };

            IABIE newABIE = selectedBIEL.CreateABIE(abieSpec);
            cache.CBIELs[selectedBIELName].ABIEs.Add(newABIE.Name, new CABIE(newABIE.Name, newABIE.Id));
            MessageBox.Show("ABIE generated! Kiddo, check your libraries!");
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion

        private void checkedlistboxBCCs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if ((!selectedCCLName.Equals("")) &&
                (!selectedACCName.Equals("")) &&
                (!selectedBCCName.Equals("")))
            {
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].State = e.NewValue;                                         
            }                              
        }

        private void checkedlistboxBBIEs_DoubleClick(object sender, System.EventArgs e)
        {
            Rectangle r = checkedlistboxBBIEs.GetItemRectangle(checkedlistboxBBIEs.SelectedIndex);

            editboxBBIEName.Text = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].Name;
            editboxBBIEName.Location = new Point(r.X + 15, r.Y);
            editboxBBIEName.Size = new Size(r.Width - 15, r.Height + 100);

            editboxBBIEName.Show();
        }

        private void UpdateBBIEName()
        {
            string newBBIEName = editboxBBIEName.Text;

            if (!(cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.ContainsKey(newBBIEName)))
            {
                CBBIE updatedBBIE = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName];

                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Remove(updatedBBIE.Name);

                updatedBBIE.Name = newBBIEName;

                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Add(newBBIEName, updatedBBIE);                
            }
        }

        private void EditBBIENameOver(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                UpdateBBIEName();
                editboxBBIEName.Hide();
                MirrorBBIEsToUI(); 
            }
            else if(e.KeyChar == 27)
            {
                editboxBBIEName.Hide();
            }
        }

        private void FocusLeftEditBBIEName(object sender, EventArgs e)
        {
            UpdateBBIEName();
            editboxBBIEName.Hide();
            MirrorBBIEsToUI();             
        }

        private void checkedlistboxBDTs_DoubleClick(object sender, System.EventArgs e)
        {
            if (checkedlistboxBDTs.SelectedItem.ToString() == "Create new BDT")
            {
                Rectangle r = checkedlistboxBDTs.GetItemRectangle(checkedlistboxBDTs.SelectedIndex);

                editboxBDTName.Text = checkedlistboxBDTs.SelectedItem.ToString();

                editboxBDTName.Location = new Point(r.X + 15, r.Y);
                editboxBDTName.Size = new Size(r.Width - 15, r.Height + 100);

                editboxBDTName.Show();
            }
        }

        private void UpdateBDTName()
        {
            IDictionary<string, CBDT> bdts = cache.CBDTLs[selectedBDTLName].BDTs;
            
            if (bdts.ContainsKey(editboxBDTName.Text))
            {
                errorMessageBDT.Location = new Point(tabcontrolACC.Location.X + tabcontrolACC.Width - 217, tabcontrolACC.Location.Y + 42);
                errorMessageBDT.Text = "BDT named \"" + editboxBDTName.Text + "\" alreay exists!";
                errorMessageBDT.BringToFront();
                errorMessageBDT.Show();
                Update();                 
            }
            else
            {
                errorMessageBDT.Hide();

                string newBDTName = editboxBDTName.Text;
                int cdtType = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].Type;
                int motherBDTLID = cache.CBDTLs[selectedBDTLName].Id;

                //// bug - the new datatypes get assigned for the current ACC only

                foreach (CACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
                {
                    foreach (CBCC bcc in acc.BCCs.Values)
                    {
                        foreach (CBBIE bbie in bcc.BBIEs.Values)
                        {
                            if (bbie.BDTs[0].BasedOn == cdtType)
                            {
                                bbie.BDTs.Add(new CBDT(newBDTName, -1, CheckState.Unchecked, bcc.Type, motherBDTLID));
                            }
                        }
                    }
                }
            }                        
        }

        private void EditBDTNameOver(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                UpdateBDTName();
                editboxBDTName.Hide();
                MirrorBDTsToUI();
            }
            else if (e.KeyChar == 27)
            {
                editboxBDTName.Hide();
            }
        }

        private void FocusLeftEditBDTName(object sender, EventArgs e)
        {
            UpdateBDTName();
            editboxBDTName.Hide();
            MirrorBDTsToUI();
        }


        private void textABIEName_TextChanged(object sender, System.EventArgs e)
        {
            if (cache.CBIELs[selectedBIELName].ABIEs.ContainsKey(textABIEName.Text))
            {
                errorMessageABIE.Location = new Point(textABIEName.Location.X + textABIEName.Width - 210, textABIEName.Location.Y);
                errorMessageABIE.Text = "ABIE named \"" + textABIEName.Text + "\" alreay exists!";
                errorMessageABIE.BringToFront();
                errorMessageABIE.Show();
            }
            else
            {
                errorMessageABIE.Hide();
            }
        }

        private void comboBDTLs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            selectedBDTLName = comboBDTLs.SelectedItem.ToString();
        }

        private void comboBIELs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            selectedBIELName = comboBIELs.SelectedItem.ToString();
        }

        private void buttonAddBBIE_Click(object sender, System.EventArgs e)
        {
            CBCC baseBCC = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName];

            IList<CBDT> relevantBdts = GetRelevantBDTs(baseBCC.Type);

            /* generate new name */

            string newBBIEName = "";

            for (int i = 1; i != -1; i++)
            {
                newBBIEName = baseBCC.Name + i;

                if (!(baseBCC.BBIEs.ContainsKey(newBBIEName)))
                {
                    break;
                }
            }

            baseBCC.BBIEs.Add(newBBIEName, new CBBIE(newBBIEName, 0, CheckState.Unchecked, relevantBdts));

            MirrorBBIEsToUI();
        }

        private void checkedlistboxBBIEs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if ((!selectedCCLName.Equals("")) &&
                (!selectedACCName.Equals("")) &&
                (!selectedBCCName.Equals("")) &&
                (!selectedBBIEName.Equals("")))
            {
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].State = e.NewValue;
            }                         
        }

        private void checkedlistboxBDTs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if ((!selectedCCLName.Equals("")) &&
                (!selectedACCName.Equals("")) &&
                (!selectedBCCName.Equals("")) &&
                (!selectedBBIEName.Equals("")) &&
                (!selectedBDTName.Equals("")))
            {

                foreach (CBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
                {
                    if (bdt.Name == selectedBDTName)
                    {
                        bdt.State = e.NewValue;
                    }
                }                
            } 
        }
    }
}