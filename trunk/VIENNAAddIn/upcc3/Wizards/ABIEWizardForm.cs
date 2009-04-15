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
    public partial class ABIEWizardForm : Form
    {
        #region Variable Declarations

        private CCRepository repository;
        private Cache cache;
        private Label errorMessageABIE;
        private Label errorMessageBDT;
        private TextBox editboxBBIEName;
        private TextBox editboxBDTName;
        private int mouseDownPosX;
        private bool editMode = true;
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

        private const string CAPTION_ERROR_WINDOW = "ABIE Wizard Error";
        private const string CAPTION_INFO_WINDOW = "ABIE Wizard";
        private const string INFO_MSG_BBIE_EXISTS = "A BBIE having the name \"{0}\" already exists within the current ABIE.";
        private const string INFO_MSG_BDT_EXISTS = "A BDT named \"{0}\" already exists!";

        #endregion

        #region Constructor

        ///<summary>
        /// The constructor of the ABIE has one input parameter which is the EA
        /// repository that the wizard operates on. Based on the EA repository the 
        /// constructor pre-poluates the internal cache of the wizard with all CC 
        /// libraries, all BDT libraries and their corresponding BDTs, all BIE libraries 
        /// and their corresponding ABIEs. Furthermore, the constructor initializes
        /// different textboxes and labels used for on-the-fly editing or validation. 
        /// An example for on-the-fly editing and validation is renaming a BBIE and
        /// validation that the name of the BBIE is already in use. 
        ///</summary>
        ///<param name="eaRepo"></param>
        public ABIEWizardForm(EA.Repository eaRepo)
        {
            InitializeComponent();

            try
            {
                repository = new CCRepository(eaRepo);            

                cache = new Cache();

                /*
                 * Populate the internal cache with all the CC libraries currently
                 * available the repository.
                 **/
                cache.LoadCCLs(repository);                

                /*
                 * Populate the internal cache with all the BIE libraries currently
                 * available the repository. For each BIE library all ABIEs contained
                 * in the library are cached as well. 
                 **/
                cache.LoadBIELs(repository);

                /*
                 * Populate the internal cache with all the BDT libraries currently
                 * available the repository. For each BDT library all BDTs contained
                 * in the library are cached as well. 
                 **/
                cache.LoadBDTLs(repository);
            }
            catch (CacheException ce)
            {
                InformativeMessage(ce.Message);
                ResetForm(0);   
            }
            catch (Exception e)
            {
                CriticalErrorMessage(e.ToString());
                ResetForm(0);
            }
        }
                ///<summary>
        /// The constructor of the ABIE has two input parameter, where one is the EA
        /// repository to save the edited Element into and the other one is the element
        /// which should be manipulated. Furthermore, the constructor initializes
        /// different textboxes and labels used for on-the-fly editing or validation. 
        /// An example for on-the-fly editing and validation is renaming a BBIE and
        /// validation that the name of the BBIE is already in use. 
        ///</summary>
        ///<param name="eaRepo"></param>
        /// <param name="element"></param>
        public ABIEWizardForm(EA.Repository eaRepo, EA.Element element)
                {
                    editMode = false;
                    InitializeComponent();
                }
        #endregion

        #region Event Handlers

        private void ABIEWizardForm_Load(object sender, EventArgs e)
        {
            /*
              * Disable all input controls on the UI. The controls will be enabled
              * depending of the input provided through the user.
              **/
            ResetForm(0);

            /*
             * As part of the wizard being launched the items already stored in 
             * the cache are displayed to the user. These items include the cached 
             * CC libraries, BDT libraries and BIE libraries. 
             **/
            MirrorCCLibrariesToUI();
            MirrorBDTLibrariesToUI();
            MirrorBIELibrariesToUI();

            /*
             * Also default entries are selected in the combo boxes for the
             * BDT and BIE libraries. 
             **/
            SetSafeIndex(comboBDTLs, 0);
            SetSafeIndex(comboBIELs, 0);

            /*
             * Also set a default prefix for the generated Artifacts
             **/
            textPrefix.Text = DEFAULT_PREFIX;

            /*
             * Per default all of the user input controls are disabled except the
             * combo box containing all the CC libraries. 
             **/
            if (editMode)
            {
                ResetForm(1);
                ResetForm(2);
                ResetForm(3);
                buttonGenerate.Show();
            }
            else
            {
                buttonSave.Show();
            }

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
            CreateOnTheFlyControls();
        }

        private void comboCCLs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            /*
             * This event is triggered in case the user selected an entry in the combo
             * box containing all CC libraries. As a result the relevant ACCs contained
             * in the CC library selected need to be cached as well as displayed to the
             * user. 
             **/

            try
            {
                /*
                 * To retrieve the currently selected user input the method GatherUserInput()
                 * is used. 
                 */
                GatherUserInput();

                if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName }))
                {
                    /*
                     * First check, if the CC library has not been selectd the first time because
                     * if so then the ACCs contained in the CC library have already been cashed. 
                     * And if the ACCs are already cashed they can be directlay displayed to the
                     * user. 
                     **/
                    cache.CCLs[selectedCCLName].LoadACCs(repository);

                    /*
                     * Display the relevant ACCs contained in the currently selected CC library
                     * to the user by using the method MirrorACCsToUI().
                     */
                    MirrorACCsToUI();

                    /*
                     * Accordingly, enable the combo box containing all ACCs to allow the 
                     * user to choose an ACC.
                     **/
                    ResetForm(2);                    
                }
            }
            catch (CacheException ce)
            {
                InformativeMessage(ce.Message);
                ResetForm(0);
            }            
        }

        private void comboACCs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            /*
             * This event is triggered in case the user chose an ACC from the 
             * combo box containing all ACCs. In case an ACC is selected it is 
             * necessary to display the BCCs of the ACC to the user. Along with 
             * displaying the BCCs to the user it is necessary to create a default
             * BBIE for each BCC and a list of relevant datatypes for each BBIE. 
             **/

            try
            {
                /*
                 * To operate on the correct ACC the currently selected user input 
                 * is retrieved from the UI. 
                 **/
                GatherUserInput();

                if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName }))
                {
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].LoadBCCsAndCreateDefaults(repository, cache.BDTLs);                    
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].LoadASCCs(repository, cache.BIELs);
                }

                textABIEName.Text = textPrefix.Text + "_" + selectedACCName;

                ResetForm(3);
            }
            catch (CacheException ce)
            {
                ResetForm(0);
                ResetForm(1);
                ResetForm(2);
                InformativeMessage(ce.Message);
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName }))
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName }))
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName }))
            {
                if (mouseDownPosX > MARGIN)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].State = e.NewValue;
                    preselectDefaultsForBBIE(selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName);
                }

                MirrorBDTsToUI();                
            }

            CheckIfConfigurationValid();
        }

        private void checkedlistboxBBIEs_DoubleClick(object sender, EventArgs e)
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName }))
            {
                if (!(cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.ContainsKey(newBBIEName)))
                {
                    cBBIE updatedBBIE = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName];

                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Remove(updatedBBIE.Name);

                    updatedBBIE.Name = newBBIEName;

                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Add(newBBIEName, updatedBBIE);
                }
                else
                {                    
                    InformativeMessage(INFO_MSG_BBIE_EXISTS.Replace("{0}", newBBIEName));
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
                editboxBBIEName.Hide();
            }
        }

        private void FocusLeftEditBBIEName(object sender, EventArgs e)
        {
            editboxBBIEName.Hide();
        }

        private void buttonAddBBIE_Click(object sender, EventArgs e)
        {
            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName }))
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName, selectedBDTName }))
            {
                if (mouseDownPosX > MARGIN)
                {
                    e.NewValue = e.CurrentValue;
                }
                else
                {
                    foreach (cBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName, selectedBDTName }))
            {
                if (selectedBDTName == "Create new BDT")
                {
                    Rectangle r = checkedlistboxBDTs.GetItemRectangle(checkedlistboxBDTs.SelectedIndex);

                    editboxBDTName.Text = checkedlistboxBDTs.SelectedItem.ToString();

                    editboxBDTName.Location = new Point(r.X + 15, r.Y);
                    editboxBDTName.Size = new Size(r.Width - 15, r.Height + 100);

                    editboxBDTName.Show();
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
                    errorMessageBDT.Location = new Point(tabcontrolACC.Location.X + tabcontrolACC.Width - 217, tabcontrolACC.Location.Y + 42);

                    errorMessageBDT.Text = INFO_MSG_BDT_EXISTS.Replace("{0}", newBDTName);
                    errorMessageBDT.BringToFront();
                    errorMessageBDT.Show();  
                    return;
                }
            }

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName, selectedBDTName }))
            {
                errorMessageBDT.Hide();

                int cdtType = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].Type;

                cache.BDTLs[selectedBDTLName].BDTs.Add(newBDTName, new cBDT(newBDTName, -1, cdtType, CheckState.Unchecked));

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
                                    bbie.BDTs.Insert(bbie.BDTs.Count - 1, new cBDT(newBDTName, -1, bcc.Type, CheckState.Unchecked));
                                    break;
                                }
                            }
                        }
                    }
                }

                // check the bdt that was added above
                foreach (cBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
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
                editboxBDTName.Hide();
            }
        }

        private void FocusLeftEditBDTName(object sender, EventArgs e)
        {
            editboxBDTName.Hide();
        }

        private void textABIEName_TextChanged(object sender, EventArgs e)
        {
            if (cache.BIELs[selectedBIELName].ABIEs.ContainsKey(textABIEName.Text))
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
            GatherUserInput();

            if ((cache.PathIsValid(CacheConstants.PATH_BDTLs, new[] {selectedBDTLName})) &&
                (cache.PathIsValid(CacheConstants.PATH_BIELs, new[] {selectedBIELName})) &&
                (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName})))
            {
                IBDTLibrary selectedBDTL = (IBDTLibrary)repository.GetLibrary(cache.BDTLs[selectedBDTLName].Id);
                IBIELibrary selectedBIEL = (IBIELibrary)repository.GetLibrary(cache.BIELs[selectedBIELName].Id);

                /* get the selected ACC which we as a basis to generate the new ABIE */
                IACC selectedACC = repository.GetACC(cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id);

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
                                                IBDT newBDT = selectedBDTL.CreateBDT(bdtSpec);
                                                bdtUsed = newBDT;

                                                generatedBDTs.Add(newBDT.Name, new cBDT(newBDT.Name, newBDT.Id, newBDT.BasedOn.CDT.Id, CheckState.Unchecked));
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
                        //if (cascc.State == CheckState.Checked)
                        //{
                        // rip the checked item apart
                        int indexBrace = selectedASCCName.IndexOf('(');
                        //string asccName = selectedASCCName.Substring(0, selectedASCCName.Length - indexBrace - 3);
                        string abieName = selectedASCCName.Substring(indexBrace + 1);
                        abieName = abieName.Remove(abieName.Length - 1);


                        int accId = cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id;
                        IACC acc = repository.GetACC(accId);

                        IASCC origASCC = null;
                        foreach (IASCC ascc in acc.ASCCs)
                        {
                            if (ascc.Id == cascc.Id)
                            {
                                origASCC = ascc;
                            }
                        }

                        // todo: check if origASCC is null
                        newASBIEs.Add(ASBIESpec.CloneASCC(origASCC, textPrefix.Text + "_" + cascc.Name, cascc.ABIEs[abieName].Id));
                        //}
                        // else don't worry about it
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
                    ASBIEs = newASBIEs,
                };

                IABIE newABIE = selectedBIEL.CreateABIE(abieSpec);
                cache.BIELs[selectedBIELName].ABIEs.Add(newABIE.Name, new cABIE(newABIE.Name, newABIE.Id, selectedACC.Id));

                textABIEName.Text = "";
                textABIEName.Text = newABIE.Name;
            }
        }
        
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ABIEWizardForm_SizeChanged(object sender, EventArgs e)
        {
            errorMessageABIE.Location = new Point(textABIEName.Location.X + textABIEName.Width - 210, textABIEName.Location.Y);
            errorMessageBDT.Location = new Point(tabcontrolACC.Location.X + tabcontrolACC.Width - 217, tabcontrolACC.Location.Y + 42);
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

            if (cache.PathIsValid(CacheConstants.PATH_ASCCs, new[] { selectedCCLName, selectedACCName, selectedASCCName }))
            {
                int indexBrace = selectedASCCName.IndexOf('(');
                //string asccName = selectedASCCName.Substring(0, selectedASCCName.Length - indexBrace - 3);
                string abieName = selectedASCCName.Substring(indexBrace + 1);
                abieName = abieName.Remove(abieName.Length - 1);

                foreach (cASCC ascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
                {
                    foreach (cABIE abie in ascc.ABIEs.Values)
                    {
                        if (abie.Name == abieName)
                        {
                            ascc.State = e.NewValue;
                            return;
                        }
                    }
                }
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
             * Label to display error messages in case an ABIE with name of the ABIE to be 
             * generated already exists in the currently selected BIE library. 
             **/
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

            /*
             * Label to display error messages in case a BDT with name of the BDT to be 
             * generated already exists in any of the availalbe BDT libraries. 
             **/
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

            if ((cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName })) &&
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

            if (cache.PathIsValid(CacheConstants.PATH_ASCCs, new[] { selectedCCLName, selectedACCName }))
            {
                foreach (cASCC ascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
                {
                    foreach (cABIE abie in ascc.ABIEs.Values)
                    {
                        checkedlistboxASCCs.Items.Add(ascc.Name + " (" + abie.Name + ")", ascc.State);
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName }))
            {
                foreach (cBBIE bbie in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Values)
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

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] { selectedCCLName, selectedACCName, selectedBCCName, selectedBBIEName }))
            {
                foreach (cBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
                {
                    checkedlistboxBDTs.Items.Add(bdt.Name, bdt.State);
                }

                SetSafeIndex(checkedlistboxBDTs, oldIndex);
            }
        }

        private static void CriticalErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, CAPTION_ERROR_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void InformativeMessage(string infoMessage)
        {
            MessageBox.Show(infoMessage, CAPTION_INFO_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CheckIfConfigurationValid()
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BCCs, new[] {selectedCCLName, selectedACCName}))
            {
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
    }
}