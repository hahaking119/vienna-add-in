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
        // TODO: document the purpose of inhibitEventToFire
        private bool inhibitEventToFire;

        /*
         * Internal variables used to store the currently selected items in the
         * various combo boxes and checked listboxes of the wizard UI. The variables
         * are not modified directly but through the method GatherUserInput(). 
         **/
        private string selectedCCLName;
        private string selectedACCName;
        private string selectedBCCName;
        private string selectedBBIEName;
        private string selectedBDTName;
        private string selectedBDTLName;
        private string selectedBIELName;

        private const string CAPTION_ERROR_WINDOW = "ABIE Wizard Error";
        private const string CAPTION_INFO_WINDOW = "ABIE Wizard";
        private const string ERROR_MSG_CCLIBRARIES = "The Wizard was not able to detect any CC Libraries in the currently chosen model. Note that CC libraries are required for successful ABIE generation.";
        private const string ERROR_MSG_BDTLIBRARIES = "The Wizard was not able to detect any BDT Libraries in the currently chosen model. Note that BDT libraries are required for successful ABIE generation.";
        private const string ERROR_MSG_BIELIBRARIES = "The Wizard was not able to detect any BIE Libraries in the currently chosen model. Note that BIE libraries are required for successful ABIE generation.";
        private const string ERROR_MSG_RETRIEVING_LIBS_FAILED = "An exception occured while retrieving the CC Libraries, BIE Libraries and BDT Libraries from the repository.";
        private const string ERROR_MSG_RETRIEVING_ACCS_FAILED = "An exception occured while retrieving the ACCs from the currently selected CC Library.";
        private const string ERROR_MSG_RETRIEVING_BCCS_ASCCS_FAILED = "An exception occured while retrieving the BCCs and ASCCs from the currently selected ACC.";
        private const string INFO_MSG_BBIE_EXISTS = "A BBIE having the name \"{0}\" already exists within the current ABIE.";

        #endregion

        #region Constructor

        ///<summary>
        /// The constructor of the ABIE has one input parameter which is the EA
        /// Repository that the wizard operates on. Furthermore, the constructor
        /// pre-poluates the internal cache of the wizard with all CC libraries, 
        /// all BDT libraries and their corresponding BDTs, all BIE libraries and
        /// their corresponding ABIEs contained in the EA repository passed through
        /// the parameter. The constructor (as well as all other methods used in 
        /// the wizard) do not directory operate on the EA repository but operate on
        /// the CC repository instead. 
        ///</summary>
        ///<param name="eaRepository"></param>
        public ABIEWizardForm(EA.Repository eaRepository)
        {
            InitializeComponent();

            try
            {
                /* 
                 * Create CCTS repository based on the EA repository that is currently 
                 * selected in the tree view of Enterprise Architect. To create the CCTS
                 * repository the constructor of the CCTS repository is used. 
                 **/
                repository = new CCRepository(eaRepository);            

                /*
                 * The internal cache for the wizards is used to keep all information in 
                 * memory that is currently displayed or going to be displayed on the UI.
                 **/
                cache = new Cache();

                /*
                 * Populate the internal cache with all the CC libraries currently
                 * available the repository.
                 **/            
                foreach (ICCLibrary ccLibrary in repository.Libraries<CCLibrary>())
                {
                    cache.CCLs.Add(ccLibrary.Name, new CCCL(ccLibrary.Name, ccLibrary.Id));
                }

                /*
                 * Populate the internal cache with all the BIE libraries currently
                 * available the repository. In addition all currently available ABIEs
                 * within each BIE library are cached as well. 
                 **/
                foreach (IBIELibrary bieLibrary in repository.Libraries<IBIELibrary>())
                {
                    IDictionary<string, CABIE> abies = new Dictionary<string, CABIE>();

                    foreach (IABIE abie in bieLibrary.BIEs)
                    {
                        abies.Add(abie.Name, new CABIE(abie.Name, abie.Id, abie.BasedOn.Id));
                    }

                    cache.CBIELs.Add(bieLibrary.Name, new CBIEL(bieLibrary.Name, bieLibrary.Id, abies));
                }

                /*
                 * Populate the internal cache with all the BDT libraries currently
                 * available the repository. In addition all currently available BDTs
                 * within each BDT library are cached as well. 
                 **/
                foreach (IBDTLibrary bdtLibrary in repository.Libraries<IBDTLibrary>())
                {
                    IDictionary<string, CBDT> bdts = new Dictionary<string, CBDT>();

                    foreach (IBDT bdt in bdtLibrary.BDTs)
                    {
                        bdts.Add(bdt.Name, new CBDT(bdt.Name, bdt.Id, bdt.BasedOn.CDT.Id, bdtLibrary.Id));
                    }

                    cache.CBDTLs.Add(bdtLibrary.Name, new CBDTL(bdtLibrary.Name, bdtLibrary.Id, bdts));
                }
            }
            catch (Exception e)
            {
                CriticalErrorMessage(ERROR_MSG_RETRIEVING_LIBS_FAILED, e);
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

            /*
             * Disable the combo cox controls for editing which means that the user can still
             * choose from the combo box but can't type text in the combo box. That's exactly
             * what we want since the user should only be able to choose from a provided list
             * of values (e.g. existing CC Libraries). 
             **/
            comboCCLs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboACCs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBDTLs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBIELs.DropDownStyle = ComboBoxStyle.DropDownList;

            // TODO: document
            inhibitEventToFire = false;
        }

        #endregion

        #region Convenience Methods

        private void preselectDefaultsForBCC(string cclName, string accName, string bccName)
        {
            int countSelectedBBIEs = 0;

            foreach (CBBIE bbie in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs.Values)
            {
                if (bbie.State == CheckState.Checked)
                {
                    countSelectedBBIEs++;
                }

                int countSelectedBDTs = 0;
                foreach (CBDT bdt in bbie.BDTs)
                {
                    if (bdt.State == CheckState.Checked)
                    {
                        countSelectedBDTs++;
                    }
                }

                if (countSelectedBDTs == 0)
                {
                    foreach (CBDT bdt in bbie.BDTs)
                    {
                        bdt.State = CheckState.Checked;
                        break;
                    }
                }
            }

            if (countSelectedBBIEs == 0)
            {
                foreach (CBBIE bbie in cache.CCLs[cclName].ACCs[accName].BCCs[bccName].BBIEs.Values)
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
        private void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if ((box.Items.Count > 0) && (indexToBeSet < box.Items.Count))
            {
                box.SelectedIndex = indexToBeSet;
            }
        }

        private void SetSafeIndex(CheckedListBox box, int indexToBeSet)
        {
            if ((box.Items.Count > 0) && (indexToBeSet < box.Items.Count))
            {
                box.SelectedIndex = indexToBeSet;
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
                    comboCCLs.Enabled = false;
                    comboACCs.Enabled = false;
                    tabcontrolACC.Enabled = false;
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
                    textABIEName.Enabled = true;
                    comboBDTLs.Enabled = true;
                    comboBIELs.Enabled = true;
                    buttonGenerate.Enabled = true;
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
        }

        /*
         * The method clears all entries from the combo box containing a set of ACCs and 
         * adds all ACCs for the currently selected CC library to the combo box. The ACCs
         * to be added are retrieved from the internal wizard cache. 
         **/
        private void MirrorACCsToUI()
        {
            comboACCs.Items.Clear();

            foreach (CACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
            {
                comboACCs.Items.Add(acc.Name);
            }
        }

        // TODO: document
        private void MirrorCCLibrariesToUI()
        {
            comboCCLs.Items.Clear();

            foreach (CCCL ccl in cache.CCLs.Values)
            {
                comboCCLs.Items.Add(ccl.Name);
            }
        }

        // TODO: document
        private void MirrorBDTLibrariesToUI()
        {
            comboBDTLs.Items.Clear();

            foreach (CBDTL bdtl in cache.CBDTLs.Values)
            {
                comboBDTLs.Items.Add(bdtl.Name);
            }
        }

        // TODO: document
        private void MirrorBIELibrariesToUI()
        {
            comboBIELs.Items.Clear();

            foreach (CBIEL biel in cache.CBIELs.Values)
            {
                comboBIELs.Items.Add(biel.Name);
            }
        }

        // TODO: document
        private void MirrorBCCsToUI()
        {
            int oldIndex = 0;

            if (checkedlistboxBBIEs.Items.Count > 0)
            {
                oldIndex = checkedlistboxBCCs.SelectedIndex;
            } 

            checkedlistboxBCCs.Items.Clear();

            foreach (CBCC bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Values)
            {
                checkedlistboxBCCs.Items.Add(bcc.Name, bcc.State);
            }

            checkboxAttributes.CheckState = cache.CCLs[selectedCCLName].ACCs[selectedACCName].AllAttributesChecked;

            // select first entry in the list per default
            SetSafeIndex(checkedlistboxBCCs, oldIndex);  
 
            MirrorBBIEsToUI();
        }

        // TODO: document
        private void MirrorASCCsToUI()
        {
            checkedlistboxASCCs.Items.Clear();

            foreach (CASCC ascc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Values)
            {
                checkedlistboxASCCs.Items.Add(ascc.Name, ascc.State);
            }

            //checkedlistboxASCCs.SelectedIndex = 0;
        }

        // TODO: document
        private void MirrorBBIEsToUI()
        {
            GatherUserInput();

            int oldIndex = 0;

            if (checkedlistboxBBIEs.Items.Count > 0)
            {
                oldIndex = checkedlistboxBBIEs.SelectedIndex;
            }            
            
            checkedlistboxBBIEs.Items.Clear();

            foreach (CBBIE bbie in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs.Values)
            {
                checkedlistboxBBIEs.Items.Add(bbie.Name, bbie.State);
            }

            // select first entry in the list per default
            SetSafeIndex(checkedlistboxBBIEs, oldIndex);            

            MirrorBDTsToUI();
        }

        // TODO: document
        private void MirrorBDTsToUI()
        {
            GatherUserInput();
            int oldIndex = 0;

            if (!(selectedBDTName.Equals("")))
            {
                if (checkedlistboxBDTs.Items.Count > 0)
                {
                    oldIndex = checkedlistboxBDTs.SelectedIndex;
                }
            }
           
            checkedlistboxBDTs.Items.Clear();

            foreach (CBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
            {
                checkedlistboxBDTs.Items.Add(bdt.Name, bdt.State);
            }

            SetSafeIndex(checkedlistboxBDTs, oldIndex);
        }

        // TODO: document
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

        private void CriticalErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, CAPTION_ERROR_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CriticalErrorMessage(string errorMessage, Exception e)
        {
            MessageBox.Show(errorMessage + "\n\n(" + e + ")", CAPTION_ERROR_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InformativeMessage(string infoMessage)
        {
            MessageBox.Show(infoMessage, CAPTION_INFO_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion 

        #region Event Handlers

        private void ABIEWizardForm_Load(object sender, EventArgs e)
        {
            /*
             * At first we disable all the controls on the UI. As the user enters information
             * the UI is enabled step by step. An example is that the combo box containing
             * all the ACCs is not enabled as long as no CC library was chosen. 
             **/
            ResetForm(0);

            /*
             * Before displaying information to the user it is checked whether
             * all the information could be retrieved from the repository. 
             **/
            if ((cache.CCLs.Count > 0) &&
                (cache.CBIELs.Count > 0) &&
                (cache.CBDTLs.Count > 0))
            {
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
                 * Per default all of the user input controls are disabled except the
                 * combo box containing all the CC libraries. 
                 **/
                ResetForm(1);   
            }
            else
            {
                /*
                 * If any of the libraries was missing we display an appropriate error
                 * message to the user. 
                 **/
                if (cache.CCLs.Count < 1)
                {
                    CriticalErrorMessage(ERROR_MSG_CCLIBRARIES);
                }
                if (cache.CBIELs.Count < 1)
                {
                    CriticalErrorMessage(ERROR_MSG_BIELIBRARIES);
                }
                if (cache.CBDTLs.Count < 1)
                {
                    CriticalErrorMessage(ERROR_MSG_BDTLIBRARIES);
                }

                /*
                 * Also, since one of the libraries was missing we clear all of the
                 * cached libraries so far since the user can't generate any ABIEs having
                 * any of the libraries not available. 
                 **/
                cache.CCLs.Clear();
                cache.CBIELs.Clear();
                cache.CBDTLs.Clear();
            }
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

                /*
                 * First check, if the CC library has not been selectd the first time because
                 * if so then the ACCs contained in the CC library have already been cashed. 
                 * And if the ACCs are already cashed they can be directlay displayed to the
                 * user. 
                 **/
                if (cache.CCLs[selectedCCLName].ACCs.Count == 0)
                {
                    /*
                     * Retrieve the currently selected CC library from the CC repository since we
                     * need to iterate through the CC library to retrieve all ACCs contained in the 
                     * CC library. 
                     **/
                    int cclID = cache.CCLs[selectedCCLName].Id;

                    ICCLibrary ccl = (ICCLibrary)repository.GetLibrary(cclID);

                    foreach (var acc in ccl.ACCs)
                    {
                        cache.CCLs[selectedCCLName].ACCs.Add(acc.Name, new CACC(acc.Name, acc.Id));
                    }
                }

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
            catch (Exception ex)
            {
                /*
                 * In case an error occured we do not know if the error occured while
                 * caching the first ACC or any other ACC. Therefore, we clear all the
                 * cached ACCs. 
                 **/
                cache.CCLs[selectedCCLName].ACCs.Clear();

                /*
                 * Display error message.
                 **/
                CriticalErrorMessage(ERROR_MSG_RETRIEVING_ACCS_FAILED, ex);
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

                /*
                 * Retrieve the currently selected ACC from the CC repository since we need 
                 * to iterate through the ACC to retrieve all BCCs contained in the ACC.
                 **/
                int accID = cache.CCLs[selectedCCLName].ACCs[selectedACCName].Id;
                IACC acc = repository.GetACC(accID);

                /*
                 * First check, if the ACC has not been selected the first time because if so 
                 * then the BCCs contained in the ACC have already been cashed. And if the BCCs 
                 * are already cashed they can be directlay displayed to the user. 
                 **/
                if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Count == 0)
                {
                    /*
                     * Process each BCC which means that for each BCC a default BBIE is 
                     * created as well as the BDT libraries are scanned for suitable data
                     * types that may be used to typify the BBIEs. The decision whether a 
                     * datatype is suitable for the BBIE is made based on the data type
                     * of the BCC. Therefore, suitable data types are a list of BDTs that 
                     * are based on the CDT used in the BCC. 
                     **/
                    foreach (IBCC bcc in acc.BCCs)
                    {
                        /* As a first step retrieve all the relevant BDTs from the various
                         * BDT libraries and temporarily store them in a list. 
                         */
                        IList<CBDT> appropriateBDTs = GetRelevantBDTs(bcc.Type.Id);

                        /*
                         * As a second step create a default set of BBIEs which means that
                         * for each BCC a BBIE is created having the same name as the BCC. 
                         **/
                        IDictionary<string, CBBIE> newbbies = new Dictionary<string, CBBIE>();

                        /* 
                         * Realize that the BDTs gathered in previous step are attached to 
                         * the current BBIE that was generated. 
                         **/
                        newbbies.Add(bcc.Name, new CBBIE(bcc.Name, bcc.Id, CheckState.Unchecked, appropriateBDTs));

                        /*
                         * As a third step a BCC is created that has the default BBIE created
                         * in previous step attached. 
                         **/
                        cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Add(bcc.Name, new CBCC(bcc.Name, bcc.Id, bcc.Type.Id, CheckState.Unchecked, newbbies));                    
                    }

                    /*
                     * After iterating through all BCCs the internal cache is populated with 
                     * all BCCs, a default BBIE for each BBIE and a list of appropriate BDTs
                     * that may be used to typifiy the BBIE. 
                     **/
                }

                /*
                 * In addition to caching all BCCs it is necessary to enable the user to 
                 * create associations between different ABIEs. Considering that the ABIE
                 * to be generated is based on an ACC also the associations of the ABIE need
                 * to be based on the associations that the ACC has. To create associations
                 * between ABIEs the wizard assumes that all ABIEs to be associated with the 
                 * ABIE to be generated must exist. If the ABIE to be associated with the 
                 * ABIE to be generated doesn't exist the wizard won't allow the user to 
                 * create that associaton. 
                 */
                if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Count == 0)
                {                
                    IDictionary<string, CASCC> validASCCs = new Dictionary<string, CASCC>();

                    /*
                     * Loop through all ASCCs that the current ACC contains in order
                     * to process each of the ASCCs.
                     **/
                    foreach (IASCC ascc in acc.ASCCs)
                    {                    
                        foreach (CBIEL biel in cache.CBIELs.Values)
                        {
                            foreach (CABIE abie in biel.ABIEs.Values)
                            {
                                // TODO: continue
                                //if (abie.BasedOn == ascc.AssociatedElement.Id)
                                //{
                                //    validASCCs.Add(abie.Name, new CASCC());
                                //    XXXXXXXXXXXXXXXXXX
                                //    //validASCCs.Add(ascc.AssociatedElement.Name, new CASCC(ascc.AssociatedElement.Name, ascc.Id, CheckState.Unchecked));                                    
                                //}
                            }

                            if (biel.ABIEs.ContainsKey(ascc.AssociatedElement.Name))
                            {
                                
                                break;
                            }                            
                        }
                    }

                    /* 
                     * We add the for associations relevant ASCCs to the internal wizard cache. 
                     **/
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs = validASCCs;
                }

                /*
                 * Assign the name of the ABIE to be generated the name of the currently 
                 * selected ACC per default.
                 **/
                textABIEName.Text = selectedACCName;

                /* 
                 * After caching the BCCs, corresponding BBIEs, their corresponding BDTs,
                 * and associations to the cache we can mirror the information gathered and 
                 * stored in the cache to the user. 
                 */
                // TODO: document inhibit..
                inhibitEventToFire = true;
                MirrorBCCsToUI();
                inhibitEventToFire = false;
                MirrorASCCsToUI();

                /*
                 * Enable the checked listboxes in the UI to enable the user to choose 
                 * and modify BCCs, BBIEs and BDTs. 
                 **/
                ResetForm(3);
            }
            catch (Exception ex)
            {
                /*
                 * In case an error occured we do not know if the error occured while
                 * caching the first BCC, any BCC or any ASCC. Therefore, we clear all the
                 * cached BCCs and ASCCs. 
                 **/
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs.Clear();
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.Clear();

                /*
                 * Display error message.
                 **/
                CriticalErrorMessage(ERROR_MSG_RETRIEVING_BCCS_ASCCS_FAILED, ex);
            }       
        }

        //private void checkedlistboxBCCs_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ///*
        //    // * This method is triggerd in case the user clicks (not checks) an item
        //    // * in the checked listbox containing all the BCCs. 
        //    // **/

        //    ///*
        //    // * At first retrieve the latest user input is retrieved from the UI. 
        //    // **/
        //    //GatherUserInput();

        //    ///*
        //    // * In case a valid item was clicked in the checked listbox we can safely
        //    // * update the corresponding user controls which is the checked listbox
        //    // * containing all the BBIEs. 
        //    // **/
        //    //if (!(selectedBCCName.Equals("")))
        //    //{
        //    //    MirrorBBIEsToUI();
        //    //}
        //}

        // TODO: document
        private void checkedlistboxBCCs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            /*
             * At first retrieve the latest user input is retrieved from the UI. 
             **/
            GatherUserInput();

            if (!selectedBCCName.Equals(""))
            {
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].State = e.NewValue;

                preselectDefaultsForBCC(selectedCCLName, selectedACCName, selectedBCCName);

                if (e.NewValue == CheckState.Unchecked)
                {
                    inhibitEventToFire = true;
                    cache.CCLs[selectedCCLName].ACCs[selectedACCName].AllAttributesChecked = CheckState.Unchecked;
                    checkboxAttributes.CheckState = CheckState.Unchecked;
                    inhibitEventToFire = false;
                }

                MirrorBBIEsToUI();
            }
        }

        // TODO: document
        private void checkboxAttributes_CheckedChanged(object sender, EventArgs e)
        {
            /*
             * The event is allowed to fire
             **/
            if (!(inhibitEventToFire))
            {
                GatherUserInput();
                CheckState newState = CheckState.Unchecked;

                if (checkboxAttributes.Checked)
                {
                    newState = CheckState.Checked;
                }

                foreach (KeyValuePair<string, CBCC> bcc in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs)
                {
                    bcc.Value.State = newState;
                    
                    preselectDefaultsForBCC(selectedCCLName, selectedACCName, bcc.Value.Name);
                }

                cache.CCLs[selectedCCLName].ACCs[selectedACCName].AllAttributesChecked = newState;

                MirrorBCCsToUI();
            }

            inhibitEventToFire = false;
        }

        //private void checkedlistboxBBIEs_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    /*
        //     * This method is triggerd in case the user clicks (not checks) an item
        //     * in the checked listbox containing all the BBIEs. 
        //     **/

        //    /*
        //     * At first the latest user input is retrieved from the UI. 
        //     **/
        //    GatherUserInput();

        //    /*
        //     * In case a valid item was clicked in the checked listbox we can safely
        //     * update the corresponding user controls which is the checked listbox
        //     * containing all the BDTs. 
        //     **/
        //    if (!(selectedBCCName.Equals("")) &&
        //        !(selectedBBIEName.Equals("")))
        //    {
        //        // TODO: remove                 
        //        // selectedBBIEName = checkedlistboxBBIEs.SelectedItem.ToString();

        //        MirrorBDTsToUI();
        //    }
        //}

        // TODO: document
        private void checkedlistboxBBIEs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if ((!selectedCCLName.Equals("")) &&
                (!selectedACCName.Equals("")) &&
                (!selectedBCCName.Equals("")) &&
                (!selectedBBIEName.Equals("")))
            {
                cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].State = e.NewValue;

                MirrorBDTsToUI();
            }
        }

        // TODO: document
        private void checkedlistboxBBIEs_DoubleClick(object sender, EventArgs e)
        {
            Rectangle r = checkedlistboxBBIEs.GetItemRectangle(checkedlistboxBBIEs.SelectedIndex);

            editboxBBIEName.Text = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].Name;
            editboxBBIEName.Location = new Point(r.X + 15, r.Y);
            editboxBBIEName.Size = new Size(r.Width - 15, r.Height + 100);

            editboxBBIEName.Show();
        }

        // TODO: document
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
            else
            {
                string infoMessage = INFO_MSG_BBIE_EXISTS;
                infoMessage = infoMessage.Replace("{0}", newBBIEName);
                InformativeMessage(infoMessage);
            }
        }

        // TODO: document
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

        // TODO: document
        private void FocusLeftEditBBIEName(object sender, EventArgs e)
        {
            //UpdateBBIEName();
            editboxBBIEName.Hide();
            //MirrorBBIEsToUI();
        }

        // TODO: document
        private void buttonAddBBIE_Click(object sender, EventArgs e)
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

        // TODO: document 
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
                    else
                    {
                        bdt.State = CheckState.Unchecked;
                    }
                }

                MirrorBDTsToUI();
            }
        }

        // TODO: document
        private void checkedlistboxBDTs_DoubleClick(object sender, EventArgs e)
        {
            GatherUserInput();

            if (!(selectedBDTName.Equals("")))
            {
                int i = 0;
                foreach (CBDT bdt in cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].BBIEs[selectedBBIEName].BDTs)
                {
                    if (bdt.Name == selectedBDTName)
                    {
                        if (checkedlistboxBDTs.SelectedItem.ToString() == "Create new BDT")
                        {
                            Rectangle r = checkedlistboxBDTs.GetItemRectangle(i);

                            editboxBDTName.Text = checkedlistboxBDTs.SelectedItem.ToString();

                            editboxBDTName.Location = new Point(r.X + 15, r.Y);
                            editboxBDTName.Size = new Size(r.Width - 15, r.Height + 100);

                            editboxBDTName.Show();
                        }

                        break;
                    }

                    i++;
                }
            }
        }

        // TODO: document
        private void UpdateBDTName()
        {
            //IDictionary<string, CBDT> bdts = cache.CBDTLs[selectedBDTLName].BDTs;
            string newBDTName = editboxBDTName.Text;
            errorMessageBDT.Text = "";

            foreach (CBDTL bdtl in cache.CBDTLs.Values)
            {
                if (bdtl.BDTs.ContainsKey(newBDTName))
                {
                    errorMessageBDT.Text = "BDT named \"" + newBDTName + "\" alreay exists!";
                }
            }

            if (errorMessageBDT.Text.Contains("exists"))
            {
                errorMessageBDT.Location = new Point(tabcontrolACC.Location.X + tabcontrolACC.Width - 217, tabcontrolACC.Location.Y + 42);
                errorMessageBDT.BringToFront();
                errorMessageBDT.Show();               
            }
            else
            {
                errorMessageBDT.Hide();

                int cdtType = cache.CCLs[selectedCCLName].ACCs[selectedACCName].BCCs[selectedBCCName].Type;
                int motherBDTLID = cache.CBDTLs[selectedBDTLName].Id;

                cache.CBDTLs[selectedBDTLName].BDTs.Add(newBDTName, new CBDT(newBDTName, -1, CheckState.Unchecked, cdtType, motherBDTLID));

                foreach (CACC acc in cache.CCLs[selectedCCLName].ACCs.Values)
                {
                    foreach (CBCC bcc in acc.BCCs.Values)
                    {
                        foreach (CBBIE bbie in bcc.BBIEs.Values)
                        {
                            foreach (CBDT bdt in bbie.BDTs)
                            {
                                if (bdt.BasedOn == cdtType)
                                {
                                    //set the new bdt
                                    bbie.BDTs.Insert(bbie.BDTs.Count - 1, new CBDT(newBDTName, -1, CheckState.Unchecked, bcc.Type, motherBDTLID));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        // TODO: document
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

        // TODO: document
        private void FocusLeftEditBDTName(object sender, EventArgs e)
        {
            //UpdateBDTName();
            editboxBDTName.Hide();
            //MirrorBDTsToUI();
        }

        // TODO: document

        private void textABIEName_TextChanged(object sender, EventArgs e)
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

        // TODO: document
        private void comboBDTLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBDTLName = comboBDTLs.SelectedItem.ToString();
        }

        // TODO: document
        private void comboBIELs_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBIELName = comboBIELs.SelectedItem.ToString();
        }

        // TODO: document
        private void buttonGenerate_Click(object sender, EventArgs e)
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
   
            ///* iterate through the ASBIEs */
            //IList<ASBIESpec> asbies = new List<ASBIESpec>();

            //// loop through the ASCCs of the ACC
            //foreach (IASCC ascc in selectedACC.ASCCs)
            //{
            //    if (cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs.ContainsKey(ascc.AssociatedElement.Name))
            //    {
            //        CASCC cascc = cache.CCLs[selectedCCLName].ACCs[selectedACCName].ASCCs[ascc.AssociatedElement.Name];
                    
            //        if(cascc.State == CheckState.Checked)
            //        {
            //            asbies.Add(ASBIESpec.CloneASCC(ascc));
            //        }
            //    }
            //}
            



           
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
                //ASBIEs = asbies,
            };            

            IABIE newABIE = selectedBIEL.CreateABIE(abieSpec);
            cache.CBIELs[selectedBIELName].ABIEs.Add(newABIE.Name, new CABIE(newABIE.Name, newABIE.Id, selectedACC.Id));

            textABIEName.Text = "";
            textABIEName.Text = newABIE.Name;
        }
        
        // TODO: document
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ABIEWizardForm_SizeChanged(object sender, EventArgs e)
        {
            errorMessageABIE.Location = new Point(textABIEName.Location.X + textABIEName.Width - 210, textABIEName.Location.Y);
            //editboxBBIEName.Location = new Point(r.X + 15, r.Y);
            //editboxBDTName.Location = new Point(r.X + 15, r.Y);
            errorMessageBDT.Location = new Point(tabcontrolACC.Location.X + tabcontrolACC.Width - 217, tabcontrolACC.Location.Y + 42);
        }

        #endregion
    }
}