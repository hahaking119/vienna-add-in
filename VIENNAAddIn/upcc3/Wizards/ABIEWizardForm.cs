// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards
{
    ///<summary>
    ///</summary>
    public partial class ABIEWizardForm : Form
    {
        #region Field Declarations

        private CCRepository repository;
        private ABIE abie;
        private Cache cache;
        private TextBox editboxBBIEName;
        private TextBox editboxBDTName;
        private bool editboxBBIENameEsc;
        private bool editboxBDTNameEsc;
        private List<string> editboxBDTNameList = new List<string>();
        private int mouseDownPosX;
        private bool wizardModeCreate = true;
        private bool userHasClickedCheckbox;
        private const int MARGIN = 15;
        private const string DEFAULT_PREFIX = "My";

        private string selectedCCLName;
        private string selectedACCName;
        private string selectedBCCName;
        private string selectedBBIEName;
        private string selectedBDTName;
        private string selectedBDTLName;
        private string selectedBIELName;
        private string selectedASCCName;

        #endregion

        #region Constructor

        ///<summary>
        /// The method prepares the wizard cache for creating new ABIEs as 
        /// well as editing existing ABIEs. In particular the method caches
        /// the CC libraries, the BDT libraries, as well as the BIE libraries
        /// from the EA repository passed through the parameter eaRepo.
        /// The method is used by the constructors only. 
        ///</summary>
        ///<param name="eaRepo">
        /// Specifies an EA repository that the wizard operates on. 
        /// eaRepo an EA repository that the wizard
        /// cache is created for. 
        ///</param>
        private void InitializeWizardCache(Repository eaRepo)
        {
            try
            {
                repository = new CCRepository(eaRepo);

                cache = new Cache();

                cache.LoadCCLs(repository);
                cache.LoadBIELsAndTheirABIEs(repository);
                cache.LoadBDTLsAndTheirBDTs(repository);
            }
            catch (CacheException ce)
            {
                richtextStatus.Text = ce.Message;
                ResetForm(0);
            }
            catch (Exception e)
            {
                CriticalErrorMessage(e.ToString());
                ResetForm(0);
            }
        }

        ///<summary>
        /// This constructor is typically used to prepare the ABIE wizard for 
        /// creating new ABIEs in an EA repository. The constructor of the ABIE 
        /// wizard has one input parameter which is the EA repository that the 
        /// wizard operates on. 
        ///</summary>
        ///<param name="eaRepository">
        /// The EA repository that the wizard operates on. 
        ///</param>
        private ABIEWizardForm(Repository eaRepository)
        {
            InitializeComponent();

            InitializeWizardCache(eaRepository);           
        }

        ///<summary>
        /// This constructor is typically used to prepare the ABIE wizard for
        /// editing an existing ABIE in an EA repository. The constructor of the 
        /// ABIE wizard has two input parameters which include (1) the EA repository 
        /// that the wizard operates on and (2) the ABIE to be modified. 
        ///</summary>
        ///<param name="eaRepo">
        /// The EA repository that the wizard operates on.
        ///</param>
        ///<param name="element">
        /// The ABIE to be modified. 
        ///</param>
        private ABIEWizardForm(Repository eaRepo, Element element)
        {
            InitializeComponent();

            InitializeWizardCache(eaRepo);

            // retrieve the current ABIE to be edited 
            abie = (ABIE)repository.GetABIE(element.ElementID);
            wizardModeCreate = false;
        }

        #endregion

        #region Wizard Launch Methods
        ///<summary>
        /// This Method is used to show the Generate new ABIE Wizard Window.
        /// There is no initial Element to load.
        ///</summary>
        ///<param name="context"></param>
        public static void ShowABIEWizard(AddInContext context)
        {
            new ABIEWizardForm(context.EARepository).Show();
        }

        ///<summary>
        /// This Method is used to show the Modify existing ABIE Wizard Window.
        /// The selected ABIE has to be passed over to the ABIE Wizard.
        ///</summary>
        ///<param name="context"></param>
        public static void ShowModifyABIEWizard(AddInContext context)
        {
            new ABIEWizardForm(context.EARepository, (Element)context.SelectedItem).Show();
        }

        #endregion


        #region Event Handlers

        ///<summary>
        /// The method initializes the windows form. Depending on whether the wizard
        /// is launched for creating new ABIEs or editing an existing ABIE, the user
        /// interface of the wizard is adapted accordingly. 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void ABIEWizardForm_Load(object sender, EventArgs e)
        {
            ResetForm(0);

            MirrorCCLibrariesToUI();
            MirrorBDTLibrariesToUI();
            MirrorBIELibrariesToUI();

            SetSafeIndex(comboBDTLs, 0);
            SetSafeIndex(comboBIELs, 0);

            textPrefix.Text = DEFAULT_PREFIX;

            if (wizardModeCreate)
            {
                ResetForm(1);
                ResetForm(2);
                ResetForm(3);
                
                buttonGenerate.Text = "&Create ABIE ...";
            }
            else
            {
                // TODO andik/fabiank: this code needs to be cleaned up!
                #region code to be cleaned up
                textABIEName.Text = abie.Name;
                int correctCCL = -1;
                bool found = false;
                foreach (cCCLibrary ccl in cache.CCLs.Values)
                {
                    ICCLibrary realCCL = repository.LibraryByName<ICCLibrary>(ccl.Name);
                    correctCCL++;
                    foreach (IACC acc in realCCL.Elements)
                    {
                        if (acc.Name == abie.BasedOn.Name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (found)
                {
                    comboCCLs.SelectedIndex = correctCCL;
                    comboCCLs_SelectionChangeCommitted(null, null);
                }
                else
                {
                    InformativeMessage("no corresponding CCLibrary found.");
                    Close();
                }

                comboACCs.SelectedIndex = comboACCs.FindString(abie.BasedOn.Name);
                comboACCs_SelectionChangeCommitted(null, null);

                
                foreach (IASBIE asbie in abie.ASBIEs)
                {
                    if (asbie.AssociatingElement.Name == abie.Name)
                    {
                        for (int i = 0; i < checkedlistboxASCCs.Items.Count; i++)
                        {
                            int indexBrace = checkedlistboxASCCs.Items[i].ToString().IndexOf('(');
                            string itemName = checkedlistboxASCCs.Items[i].ToString().Substring(indexBrace + 1,
                                                                                                checkedlistboxASCCs.
                                                                                                    Items[i].ToString().
                                                                                                    Length - indexBrace -
                                                                                                2);
                            if (itemName.Equals(asbie.AssociatedElement.Name))
                            {
                                checkedlistboxASCCs.SetSelected(i, true);
                                checkedlistboxASCCs.SetItemChecked(i, true);
                                checkedlistboxASCCs_ItemCheck(null,
                                                              new ItemCheckEventArgs(i, CheckState.Checked,
                                                                                     CheckState.Unchecked));
                            }
                        }
                    }
                }

                #endregion
                
                ResetForm(0);                
                ResetForm(4);

                buttonGenerate.Text = "&Save ABIE ...";
            }

            CreateOnTheFlyControls();
        }

        ///<summary>
        /// The method is invoked in case the user selects a CC library from the 
        /// combo box. The method causes the cache to load all ACCs contained in 
        /// the CC library chosen. Finally, the method mirrors the ACCs loaded
        /// to the wizard UI. 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void comboCCLs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                GatherUserInput();

                if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName}))
                {
                    cache.CCLs[selectedCCLName].LoadACCs(repository);

                    MirrorACCsToUI();

                    ResetForm(2);
                }
            }
            catch (CacheException ce)
            {
                richtextStatus.Text = ce.Message;
                ResetForm(0);
            }
        }

        ///<summary>
        /// The method is invoked in case the user selects an ACC from the combo box. 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void comboACCs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                GatherUserInput();

                if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName}))
                {
                    if (wizardModeCreate)
                    {
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].LoadBCCsAndCreateDefaults(repository,
                                                                                                    cache.BDTLs);
                    }
                    else
                    {
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].LoadBCCsAndBBIEs(repository,
                                                                                           cache.BDTLs, abie);
                    }
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].LoadASCCs(repository, cache.BIELs);
                }

                textABIEName.Text = textPrefix.Text + "_" + selectedACCName;
                richtextStatus.Text = "";
                editboxBDTNameList.Clear();
                editboxBDTNameList.Add("Create new BDT");
            }
            catch (CacheException ce)
            {
                richtextStatus.Text = ce.Message;
                ResetForm(0);
                ResetForm(1);
                ResetForm(2);
            }
            finally
            {
                MirrorBCCsToUI();
                MirrorASCCsToUI();
                CheckIfConfigurationValid();
            }
        }


        private void checkedlistboxBCCs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName, selectedBCCName}))
            {
                if (mouseDownPosX > MARGIN)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].State = e.NewValue;

                    preselectDefaultsForBCC(selectedCCLName, selectedACCName, selectedBCCName);

                    if (e.NewValue == CheckState.Unchecked)
                    {
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].State = CheckState.Unchecked;
                        checkboxAttributes.CheckState = CheckState.Unchecked;
                    }
                }
            }

            MirrorBBIEsToUI();
            CheckIfConfigurationValid();
        }

        private void checkboxAttributes_CheckedChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName}))
            {
                CheckState newState = CheckState.Unchecked;

                if (checkboxAttributes.Checked)
                {
                    newState = CheckState.Checked;
                }

                cache.CCLs[selectedCCLName].ACCs[selectedACCName].State = newState;

                if (userHasClickedCheckbox)
                {
                    foreach (KeyValuePair<string, cBCC> bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs)
                    {
                        bcc.Value.State = newState;

                        preselectDefaultsForBCC(selectedCCLName, selectedACCName, bcc.Value.Name);
                    }

                    userHasClickedCheckbox = false;
                }

                MirrorBCCsToUI();
            }
        }

        private void checkedlistboxBBIEs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[] {selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName}))
            {
                if (mouseDownPosX > MARGIN)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].
                        State = e.NewValue;
                    preselectDefaultsForBBIE(selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName);
                }

                MirrorBDTsToUI();
            }

            CheckIfConfigurationValid();
        }

        private void checkedlistboxBBIEs_DoubleClick(object sender, EventArgs e)
        {
            Rectangle r = checkedlistboxBBIEs.GetItemRectangle(checkedlistboxBBIEs.SelectedIndex);

            editboxBBIEName.Text =
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].Name;
            editboxBBIEName.Location = new Point(r.X + 15, r.Y);
            editboxBBIEName.Size = new Size(r.Width - 15, r.Height + 100);
            editboxBBIEName.Show();
            editboxBBIEName.Focus();
        }

        private void UpdateBBIEName()
        {
            string newBBIEName = editboxBBIEName.Text;

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[] {selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName}))
            {
                if (
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.ContainsKey(
                        newBBIEName))
                {
                    richtextStatus.Text = "WARNING: The name of the BBIE couldn't be changed since the current ABIE alreday contains another BBIE named \"{0}\". Please choose a different name before proceeding to edit the name of the BBIE.".Replace("{0}", newBBIEName);
                }
                else
                {
                    richtextStatus.Text = "";

                    cBBIE updatedBBIE =
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName];

                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Remove(
                        updatedBBIE.Name);

                    updatedBBIE.Name = newBBIEName;

                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Add(newBBIEName,
                                                                                                      updatedBBIE);
                }
            }
        }

        private void KeyPressedEditBBIEName(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                UpdateBBIEName();
                editboxBBIEName.Hide();
                MirrorBBIEsToUI();
            }
            else if (e.KeyChar == 27)
            {
                editboxBBIENameEsc = true;
                editboxBBIEName.Hide();
            }
        }

        private void FocusLeftEditBBIEName(object sender, EventArgs e)
        {
            if (!editboxBBIENameEsc)
            {
                UpdateBBIEName();
                editboxBBIEName.Hide();
                MirrorBBIEsToUI();
            }
            editboxBBIENameEsc = false;
        }

        private void buttonAddBBIE_Click(object sender, EventArgs e)
        {
            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName, selectedBCCName}))
            {
                cBCC baseBCC = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName];

                string newBBIEName = "";

                for (int i = 1; i != -1; i++)
                {
                    newBBIEName = baseBCC.Name + i;

                    if (!(baseBCC.BBIEs.ContainsKey(newBBIEName)))
                    {
                        break;
                    }
                }

                baseBCC.BBIEs.Add(newBBIEName, new cBBIE(newBBIEName, -1, baseBCC.Type, CheckState.Unchecked));
                baseBCC.BBIEs[newBBIEName].SearchAndAssignRelevantBDTs(baseBCC.Type, cache.BDTLs);

                MirrorBBIEsToUI();
            }
        }

        private void checkedlistboxBDTs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[]
                                      {
                                          selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName,
                                          selectedBDTName
                                      }))
            {
                if (mouseDownPosX > MARGIN)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    foreach (
                        cBDT bdt in
                            cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[
                                selectedBBIEName].BDTs)
                    {
                        bdt.State = bdt.Name == selectedBDTName ? e.NewValue : CheckState.Unchecked;
                    }
                }

                MirrorBDTsToUI();
            }

            CheckIfConfigurationValid();
        }

        private void checkedlistboxBDTs_DoubleClick(object sender, EventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[]
                                      {
                                          selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName,
                                          selectedBDTName
                                      }))
            {
                bool nameAlreadyExists = false;
                foreach(string curName in editboxBDTNameList)
                {
                    if(curName == selectedBDTName)
                        nameAlreadyExists = true;
                }
                if (nameAlreadyExists)
                {
                    Rectangle r = checkedlistboxBDTs.GetItemRectangle(checkedlistboxBDTs.SelectedIndex);

                    editboxBDTName.Text = checkedlistboxBDTs.SelectedItem.ToString();

                    editboxBDTName.Location = new Point(r.X + 15, r.Y);
                    editboxBDTName.Size = new Size(r.Width - 15, r.Height + 100);

                    editboxBDTName.Show();
                    editboxBDTName.Focus();
                }
            }
        }

        private void UpdateBDTName()
        {
            string newBDTName = editboxBDTName.Text;

            foreach (cBDTLibrary bdtl in cache.BDTLs.Values)
            {
                if (bdtl.BDTs.ContainsKey(newBDTName))
                {
                    richtextStatus.Text += "A BDT named \"{0}\" already exists!".Replace("{0}", newBDTName);

                    return;
                }
            }

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[]
                                      {
                                          selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName,
                                          selectedBDTName
                                      }))
            {
                int cdtType = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].Type;

                cache.BDTLs[selectedBDTLName].BDTs.Add(newBDTName,
                                                       new cBDT(newBDTName, -1, cdtType, CheckState.Unchecked));

                foreach (cACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
                {
                    foreach (cBCC bcc in acc.BCCs.Values)
                    {
                        foreach (cBBIE bbie in bcc.BBIEs.Values)
                        {
                            foreach (cBDT bdt in bbie.BDTs)
                            {
                                if (bdt.BasedOn == cdtType)
                                {
                                    bbie.BDTs.Insert(bbie.BDTs.Count - 1,
                                                     new cBDT(newBDTName, -1, bcc.Type, CheckState.Unchecked));
                                    editboxBDTNameList.Add(newBDTName);
                                    break;
                                }
                            }
                        }
                    }
                }

                // check the bdt that was added above
                foreach (
                    cBDT bdt in
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].
                            BDTs)
                {
                    bdt.State = bdt.Name == newBDTName ? CheckState.Checked : CheckState.Unchecked;
                }
            }
        }

        private void KeyPressedEditBDTName(object sender, KeyPressEventArgs e)
        {
          if (e.KeyChar == 13)
            {
                UpdateBDTName();
                editboxBDTName.Hide();
                MirrorBDTsToUI();
            }
            else if (e.KeyChar == 27)
            {
                editboxBDTNameEsc = true;
                editboxBDTName.Hide();
            }
        }

        private void FocusLeftEditBDTName(object sender, EventArgs e)
        {
            if (!editboxBDTNameEsc)
            {
                UpdateBDTName();
                editboxBDTName.Hide();
                MirrorBDTsToUI();
            }
            editboxBDTNameEsc = false;
        }

        private void textABIEName_TextChanged(object sender, EventArgs e)
        {
            if (wizardModeCreate)
            {
                CheckIfConfigurationValid();
            }
        }

        private void comboBDTLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            GatherUserInput();
        }

        private void comboBIELs_SelectedIndexChanged(object sender, EventArgs e)
        {
            GatherUserInput();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (wizardModeCreate)
            {
                GatherUserInput();
                IBIELibrary selectedBIEL = (IBIELibrary) repository.GetLibrary(cache.BIELs[selectedBIELName].Id);

                /* get the selected ACC which we as a basis to generate the new ABIE */
                IACC selectedACC = repository.GetACC(cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id);
                ABIESpec abieSpec = createABISpec(selectedACC);
                IABIE newABIE = selectedBIEL.CreateElement(abieSpec);
                cache.BIELs[selectedBIELName].ABIEs.Add(newABIE.Name,
                                                        new cABIE(newABIE.Name, newABIE.Id, selectedACC.Id));

                richtextStatus.Text = "SUCCESS: an ABIE named " + newABIE.Name + " has been created.";
                buttonGenerate.Enabled = false;
                //textABIEName.Text = "";
                //textABIEName.Text = newABIE.Name;
            }
            else
            {
                GatherUserInput();
                IBIELibrary selectedBIEL = (IBIELibrary) repository.GetLibrary(cache.BIELs[selectedBIELName].Id);

                /* get the selected ACC which we as a basis to generate the new ABIE */
                IACC selectedACC = repository.GetACC(cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id);
                ABIESpec abieSpec = createABISpec(selectedACC);
                IABIE newABIE = selectedBIEL.UpdateElement(abie, abieSpec);
                //todo: find a better way to update internal cache
                cache.BIELs[selectedBIELName].ABIEs.Remove(newABIE.Name);
                cache.BIELs[selectedBIELName].ABIEs.Add(newABIE.Name,
                                                        new cABIE(newABIE.Name, newABIE.Id, selectedACC.Id));
                if (newABIE.Id > -1)
                {
                    richtextStatus.Text = "SUCCESS: the ABIE named " + newABIE.Name + " has been udpated.";

                    ResetForm(0);
                }               
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ABIEWizardForm_SizeChanged(object sender, EventArgs e)
        {
        }

        private void checkedlistboxBCCs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownPosX = e.X;
            else
                mouseDownPosX = -1;
        }

        private void checkedlistboxBBIEs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownPosX = e.X;
            else
                mouseDownPosX = -1;
        }

        private void checkedlistboxBDTs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownPosX = e.X;
            else
                mouseDownPosX = -1;
        }

        private void textPrefix_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            if (!(selectedACCName.Equals("")))
            {
                textABIEName.Text = textPrefix.Text + "_" + comboACCs.SelectedItem;
            }
        }

        private void checkedlistboxASCCs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            // todo: implement error handling (e.g. selectedASCCName is empty)

            int indexBrace = selectedASCCName.IndexOf('(');
            string asccName = selectedASCCName.Substring(0, indexBrace - 1);

            if (cache.PathIsValid(CacheConstants.PATH_ASCCs, new[] {selectedCCLName, selectedACCName, asccName}))
            {
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs[asccName].State = e.NewValue;

                //foreach (cASCC ascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
                //{
                //    foreach (cABIE abie in ascc.ABIEs.Values)
                //    {
                //        if (abie.Name == abieName)
                //        {
                //            ascc.State = e.NewValue;
                //            return;
                //        }
                //    }
                //}
            }
        }

        private void checkboxAttributes_MouseDown(object sender, MouseEventArgs e)
        {
            userHasClickedCheckbox = true;
        }

        #endregion

        #region Convenience Methods

        private void CreateOnTheFlyControls()
        {
            /*
             * In the following two labels and two edit boxes are defined. The labels are used
             * to display error messages to the user for validation performed on the fly. An
             * example for validation performed on the fly is to check if an ABIE already exists
             * in the currently selected BIE library that the user is about to generate. The
             * edit boxes are used to enable to user to edit BBIE and BDT names in checked list 
             * boxes. The reason for using edit boxes is that checked list boxes currently do not
             * support to edit items in the checked listbox. Therefore a workaround based on edit
             * boxes is used in the following way: in case the user double-clicks on an entry
             * in the checked listbox an edit box is displayed overlaying the double-clicked item. 
             * This allows the user to enter a new name for the item in the checked listbox. 
             **/


            /*
             * Edit box to edit the name of a BBIE listed in the checked listbox 
             * displaying all BBIEs. 
             **/
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
            editboxBBIEName.KeyPress += KeyPressedEditBBIEName;
            editboxBBIEName.LostFocus += FocusLeftEditBBIEName;
            checkedlistboxBBIEs.Controls.Add(editboxBBIEName);
            editboxBBIEName.Hide();

            /*
             * Edit box to edit the name of a BDT listed in the checked listbox 
             * displaying all BDTs for a currently selected BBIE. 
             **/
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
            editboxBDTName.KeyPress += KeyPressedEditBDTName;
            editboxBDTName.LostFocus += FocusLeftEditBDTName;
            checkedlistboxBDTs.Controls.Add(editboxBDTName);
            editboxBDTName.Hide();
        }

        private void preselectDefaultsForBBIE(string cclName, string accName, string bccName, string bbieName)
        {
            int countSelectedBDTs = 0;

            foreach (cBDT bdt in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs[bbieName].BDTs)
            {
                if (bdt.State == CheckState.Checked)
                {
                    countSelectedBDTs++;
                }
            }

            if (countSelectedBDTs == 0)
            {
                foreach (cBDT bdt in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs[bbieName].BDTs)
                {
                    bdt.State = CheckState.Checked;
                    break;
                }
            }
        }

        private void preselectDefaultsForBCC(string cclName, string accName, string bccName)
        {
            int countSelectedBBIEs = 0;

            foreach (cBBIE bbie in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs.Values)
            {
                if (bbie.State == CheckState.Checked)
                {
                    countSelectedBBIEs++;
                }

                preselectDefaultsForBBIE(cclName, accName, bccName, bbie.Name);
            }

            if (countSelectedBBIEs == 0)
            {
                foreach (cBBIE bbie in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs.Values)
                {
                    bbie.State = CheckState.Checked;
                    break;
                }
            }
        }

        /*
         * The method safely set's an index of a combo box control therefore having
         * two input parameters which are the combo box that the index of the selected
         * item is to be set for and the actual index to be set. The method first checks 
         * if the combo box contains any items and if it does if the index is a valid 
         * index. In case both conditions evaluate to true the index of the selected
         * item is set in the control. 
         **/

        private static void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private static void SetSafeIndex(CheckedListBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        /*
         * The method is used to disable and enable different fields in the wizard UI.          
         **/

        private void ResetForm(int level)
        {
            switch (level)
            {
                case 0:
                    comboCCLs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboACCs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBDTLs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBIELs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboCCLs.Enabled = false;
                    comboACCs.Enabled = false;
                    tabcontrolACC.Enabled = false;
                    textPrefix.Enabled = false;
                    textABIEName.Enabled = false;
                    comboBDTLs.Enabled = false;
                    comboBIELs.Enabled = false;
                    buttonGenerate.Enabled = false;
                    break;

                case 1:
                    comboCCLs.Enabled = true;
                    break;

                case 2:
                    comboACCs.Enabled = true;
                    break;

                case 3:
                    tabcontrolACC.Enabled = true;
                    textPrefix.Enabled = true;
                    textABIEName.Enabled = true;
                    comboBDTLs.Enabled = true;
                    comboBIELs.Enabled = true;
                    break;
                case 4:
                    tabcontrolACC.Enabled = true;
                    break;
            }
        }

        /*
         * The method retrieves all user input from the wizard UI which includes the 
         * currently selected item in the combo boxes and the currently selected items 
         * the checked list boxes. In case no item is selected then the variables are
         * filled with an empty string. 
         **/

        private void GatherUserInput()
        {
            selectedCCLName = comboCCLs.SelectedIndex >= 0 ? comboCCLs.SelectedItem.ToString() : "";
            selectedACCName = comboACCs.SelectedIndex >= 0 ? comboACCs.SelectedItem.ToString() : "";
            selectedBCCName = checkedlistboxBCCs.SelectedIndex >= 0 ? checkedlistboxBCCs.SelectedItem.ToString() : "";
            selectedBBIEName = checkedlistboxBBIEs.SelectedIndex >= 0 ? checkedlistboxBBIEs.SelectedItem.ToString() : "";
            selectedBDTName = checkedlistboxBDTs.SelectedIndex >= 0 ? checkedlistboxBDTs.SelectedItem.ToString() : "";
            selectedBDTLName = comboBDTLs.SelectedIndex >= 0 ? comboBDTLs.SelectedItem.ToString() : "";
            selectedBIELName = comboBIELs.SelectedIndex >= 0 ? comboBIELs.SelectedItem.ToString() : "";
            selectedASCCName = checkedlistboxASCCs.SelectedIndex >= 0 ? checkedlistboxASCCs.SelectedItem.ToString() : "";
        }

        private void MirrorCCLibrariesToUI()
        {
            comboCCLs.Items.Clear();

            foreach (cCCLibrary ccl in cache.CCLs.Values)
            {
                comboCCLs.Items.Add(ccl.Name);
            }
        }

        /*
         * The method clears all entries from the combo box containing a set of ACCs and 
         * adds all ACCs for the currently selected CC library to the combo box. The ACCs
         * to be added are retrieved from the internal wizard cache. 
         **/

        private void MirrorACCsToUI()
        {
            GatherUserInput();

            comboACCs.Items.Clear();

            if ((cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName})) &&
                (cache.CCLs[selectedCCLName].ACCs.Count > 0))
            {
                foreach (cACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
                {
                    comboACCs.Items.Add(acc.Name);
                }
            }
        }

        private void MirrorBDTLibrariesToUI()
        {
            comboBDTLs.Items.Clear();

            if (cache.BDTLs.Count > 0)
            {
                foreach (cBDTLibrary bdtl in cache.BDTLs.Values)
                {
                    comboBDTLs.Items.Add(bdtl.Name);
                }
            }
        }

        private void MirrorBIELibrariesToUI()
        {
            comboBIELs.Items.Clear();

            if (cache.BIELs.Count > 0)
            {
                foreach (cBIELibrary biel in cache.BIELs.Values)
                {
                    comboBIELs.Items.Add(biel.Name);
                }
            }
        }

        private void MirrorBCCsToUI()
        {
            int oldIndex = 0;

            // are there any items in the list box and if so is there a particular item selected? 
            // if yes then save the currently selected item
            if (checkedlistboxBCCs.Items.Count > 0)
            {
                oldIndex = checkedlistboxBCCs.SelectedIndex;
            }

            checkedlistboxBCCs.Items.Clear();

            // are there any BBIEs to be displayed?
            if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].HasBCCs())
            {
                foreach (cBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
                {
                    checkedlistboxBCCs.Items.Add(bcc.Name, bcc.State);
                }

                checkboxAttributes.CheckState = cache.CCLs[selectedCCLName].ACCs[selectedACCName].State;

                // select first entry in the list per default
                SetSafeIndex(checkedlistboxBCCs, oldIndex);

                MirrorBBIEsToUI();
            }
            else
            {
                checkedlistboxBBIEs.Items.Clear();
                checkedlistboxBDTs.Items.Clear();
            }
        }

        private void MirrorASCCsToUI()
        {
            checkedlistboxASCCs.Items.Clear();

            if (cache.PathIsValid(CacheConstants.PATH_ASCCs, new[] {selectedCCLName, selectedACCName}))
            {
                foreach (cASCC ascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
                {
                    foreach (cABIE localAbie in ascc.ABIEs.Values)
                    {
                        checkedlistboxASCCs.Items.Add(ascc.Name + " (" + localAbie.Name + ")", ascc.State);
                    }
                }
            }
        }

        private void MirrorBBIEsToUI()
        {
            GatherUserInput();

            int oldIndex = 0;

            if (checkedlistboxBBIEs.Items.Count > 0)
            {
                oldIndex = checkedlistboxBBIEs.SelectedIndex;
            }

            checkedlistboxBBIEs.Items.Clear();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName, selectedBCCName}))
            {
                foreach (
                    cBBIE bbie in
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Values)
                {
                    checkedlistboxBBIEs.Items.Add(bbie.Name, bbie.State);
                }
                SetSafeIndex(checkedlistboxBBIEs, oldIndex);

                MirrorBDTsToUI();
            }
            else
            {
                checkedlistboxBDTs.Items.Clear();
            }
        }

        private void MirrorBDTsToUI()
        {
            GatherUserInput();

            int oldIndex = 0;

            if (checkedlistboxBDTs.Items.Count > 0)
            {
                oldIndex = checkedlistboxBDTs.SelectedIndex;
            }

            checkedlistboxBDTs.Items.Clear();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs,
                                  new[] {selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName}))
            {
                foreach (
                    cBDT bdt in
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].
                            BDTs)
                {
                    checkedlistboxBDTs.Items.Add(bdt.Name, bdt.State);
                }

                SetSafeIndex(checkedlistboxBDTs, oldIndex);
            }
        }

        private static void CriticalErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "ABIE Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void InformativeMessage(string infoMessage)
        {
            MessageBox.Show(infoMessage, "ABIE Wizard", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CheckIfConfigurationValid()
        {
            GatherUserInput();

            buttonGenerate.Enabled = true;
            richtextStatus.Text = "";

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName}))
            {
                if (textABIEName.Text.Equals(""))
                {
                    richtextStatus.Text = "WARNING: The name of an ABIEs must not be empty.\n";
                    buttonGenerate.Enabled = false;
                    return;
                }
                //richtextStatus.Text = "";

                if ((cache.BIELs[selectedBIELName].ABIEs.ContainsKey(textABIEName.Text)) && (wizardModeCreate))
                {
                    richtextStatus.Text = "WARNING: An ABIE with the name " + textABIEName.Text +
                                          " already exists. Please change the name of your ABIE accordingly.\n";
                    buttonGenerate.Enabled = false;
                    return;
                }

                //richtextStatus.Text = "";


                //if (!(richtextStatus.Text.Equals("")))
                //{
                //    buttonGenerate.Enabled = false;
                //    return;
                //}

                if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].HasBCCs())
                {
                    foreach (cBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
                    {
                        if (bcc.State == CheckState.Checked)
                        {
                            foreach (cBBIE bbie in bcc.BBIEs.Values)
                            {
                                if (bbie.State == CheckState.Checked)
                                {
                                    foreach (cBDT bdt in bbie.BDTs)
                                    {
                                        if (bdt.State == CheckState.Checked)
                                        {
                                            buttonGenerate.Enabled = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    buttonGenerate.Enabled = true;
                    return;
                }
            }

            buttonGenerate.Enabled = false;
        }

        #endregion



        private ABIESpec createABISpec(IACC selectedACC)
        {
            ABIESpec abieSpec = null;
            if ((cache.PathIsValid(CacheConstants.PATH_BDTLs, new[] {selectedBDTLName})) &&
                (cache.PathIsValid(CacheConstants.PATH_BIELs, new[] {selectedBIELName})) &&
                (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName})))
            {
                IBDTLibrary selectedBDTL = (IBDTLibrary) repository.GetLibrary(cache.BDTLs[selectedBDTLName].Id);


                List<BBIESpec> newBBIEs = new List<BBIESpec>();
                IDictionary<string, cBDT> generatedBDTs = new Dictionary<string, cBDT>();

                foreach (cBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
                {
                    /* only process those bccs that are selected */
                    if (bcc.State == CheckState.Checked)
                    {
                        /* iterate through the bbies that are based on the bcc */
                        foreach (cBBIE bbie in bcc.BBIEs.Values)
                        {
                            if (bbie.State == CheckState.Checked)
                            {
                                /* iterate through the bdts that are available for the bbie */
                                foreach (cBDT bdt in bbie.BDTs)
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
                                                IBDT newBDT = selectedBDTL.CreateElement(bdtSpec);
                                                bdtUsed = newBDT;

                                                generatedBDTs.Add(newBDT.Name,
                                                                  new cBDT(newBDT.Name, newBDT.Id, newBDT.BasedOn.CDT.Id,
                                                                           CheckState.Unchecked));
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

                IList<ASBIESpec> newASBIEs = new List<ASBIESpec>();
                if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].HasASCCs())
                {
                    foreach (cASCC cascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
                    {
                        if (cascc.State == CheckState.Checked)
                        {
                            // get the original ACC that we currently process
                            int accId = cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id;
                            IACC acc = repository.GetACC(accId);

                            // get the original ASCC of the above ACC that we currently process
                            IASCC origASCC = null;
                            foreach (IASCC ascc in acc.ASCCs)
                            {
                                if (ascc.Id == cascc.Id)
                                {
                                    origASCC = ascc;
                                }
                            }

                            // get the name of the abie
                            // todo: there is always one relevant abie only. therefore we should
                            // change the abie dictionary to storing the abie name and id only (e.g. cABIE 
                            // instead of cABIE dictionary).
                            string abieName = "";
                            foreach (cABIE cabie in cascc.ABIEs.Values)
                            {
                                abieName = cabie.Name;
                                break;
                            }

                            newASBIEs.Add(ASBIESpec.CloneASCC(origASCC, textPrefix.Text + "_" + cascc.Name,
                                                              cascc.ABIEs[abieName].Id));
                        }
                        // else don't worry about it
                    }
                }


                abieSpec = new ABIESpec
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
                                   ASBIEs = newASBIEs,
                               };
            }
            return abieSpec;
        }


        private void richtextStatus_TextChanged(object sender, EventArgs e)
        {
        }

        private void richtextStatus_BackColorChanged(object sender, EventArgs e)
        {
        }
    }
}