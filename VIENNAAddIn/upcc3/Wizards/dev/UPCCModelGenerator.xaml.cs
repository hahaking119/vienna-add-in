using System;
using System.Windows.Forms;
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// This class implements the functionality for the UPCC Wizard Form which
    /// allows to create default UPCC models. Basically, the wizard allows the
    /// user to create (a) empty default models which contain only the packages 
    /// according to UPCC and (b) default models which also import the standard
    /// CC libraries. The wizard utilizes the class "ModelCreator" for creating
    /// the default structure. 
    /// </summary>
    public partial class UPCCModelGenerator
    {
        #region Local Class Fields

        private string modelName = "";
        private string primLibraryName = "";
        private string enumLibraryName = "";
        private string cdtLibraryName = "";
        private string ccLibraryName = "";
        private string bdtLibraryName = "";
        private string bieLibraryName = "";
        private string docLibraryName = "";
        // ReSharper disable InconsistentNaming
        private const string wizardTitle = "UPCC Model Wizard";
        private const string statusMessage = "Creating a default model named \"{0}\" completed successfully.";
        // ReSharper restore InconsistentNaming

        private bool importStandardLibraries;

        private readonly Repository repository;
        private readonly ICCRepository ccRepository;
        private FileBasedVersionHandler versionHandler;

        #endregion

        /// <summary>
        /// The constructor of the wizard initializes the form. Furthermore, 
        /// the constructor is private since he is called by the method "ShowForm" 
        /// which is part of the same class. 
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        private UPCCModelGenerator(Repository eaRepository, ICCRepository ccRepository)
        {
            InitializeComponent();
            repository = eaRepository;
            this.ccRepository = ccRepository;
            WindowLoaded();
        }

        ///<summary>
        /// The method is called from the menu manager of the VIENNAAddIn and
        /// creates creates as well as launches a new instance of the wizard. 
        ///</summary>
        ///<param name="context">
        /// TODO: what exactly is the context parameter for?
        ///</param>
        public static void ShowForm(AddInContext context)
        {
            new UPCCModelGenerator(context.EARepository, context.CCRepository).ShowDialog();
        }

        #region Convenience Methods

        private void WindowLoaded()
        {
            CheckAllLibraries(true);
            checkboxDefaultValues.IsChecked = true;
            SetEnabledForAllLibraryTextFields(false);
            SetModelDefaultName();
            SetLibraryDefaultNames();
        }

        private void OnStatusChanged(string statusMessage)
        {
            rtxtStatus.Text = statusMessage + "\n" + rtxtStatus.Text;
        }

        private void CheckCCLibraries(bool newState)
        {
            checkboxPRIML.IsChecked = newState;
            checkboxENUML.IsChecked = newState;
            checkboxCDTL.IsChecked = newState;
            checkboxCCL.IsChecked = newState;
        }

        private void CheckBIELibraries(bool newState)
        {
            checkboxBDTL.IsChecked = newState;
            checkboxBIEL.IsChecked = newState;
            checkboxDOCL.IsChecked = newState;
        }

        private void CheckAllLibraries(bool newState)
        {
            CheckCCLibraries(newState);
            CheckBIELibraries(newState);
        }

        private void SetEnabledForCCLibraryTextFields(bool newState)
        {
            textboxPRIMLName.IsEnabled = newState;
            textboxENUMLName.IsEnabled = newState;
            textboxCDTLName.IsEnabled = newState;
            textboxCCLName.IsEnabled = newState;
        }

        private void SetEnabledForCCLibraryCheckBoxes(bool newState)
        {
            checkboxPRIML.IsEnabled = newState;
            checkboxENUML.IsEnabled = newState;
            checkboxCDTL.IsEnabled = newState;
            checkboxCCL.IsEnabled = newState;
        }

        private void SetEnabledForBIELibraryCheckBoxes(bool newState)
        {
            checkboxBDTL.IsEnabled = newState;
            checkboxBIEL.IsEnabled = newState;
            checkboxDOCL.IsEnabled = newState;
        }

        private void SetEnabledForBIELibraryTextFields(bool newState)
        {
            textboxBDTLName.IsEnabled = newState;
            textboxBIELName.IsEnabled = newState;
            textboxDOCLName.IsEnabled = newState;
        }

        private void SetEnabledForAllLibraryTextFields(bool newState)
        {
            SetEnabledForCCLibraryTextFields(newState);
            SetEnabledForBIELibraryTextFields(newState);
        }

        private void SetModelDefaultName()
        {
            textboxModelName.Text = "Default Model";
        }

        private void SetCCLibraryDefaultNames()
        {
            textboxPRIMLName.Text = "PRIMLibrary";
            textboxENUMLName.Text = "ENUMLibrary";
            textboxCDTLName.Text = "CDTLibrary";
            textboxCCLName.Text = "CCLibrary";
        }

        private void SetBIELibraryDefaultNames()
        {
            textboxBDTLName.Text = "BDTLibrary";
            textboxBIELName.Text = "BIELibrary";
            textboxDOCLName.Text = "DOCLibrary";
        }

        private void SetLibraryDefaultNames()
        {
            SetCCLibraryDefaultNames();
            SetBIELibraryDefaultNames();
        }

        private void checkboxDefaultValues_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void checkboxImportStandardLibraries_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }
            UpdateFormState();
        }

        private void UpdateFormState()
        {
            if (checkboxDefaultValues.IsChecked == true)
            {
                if (checkboxImportStandardLibraries.IsChecked == true)
                {
                    SetEnabledForAllLibraryTextFields(false);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetLibraryDefaultNames();

                    CheckCCLibraries(true);
                }
                else
                {
                    SetEnabledForAllLibraryTextFields(false);

                    SetEnabledForAllLibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(false);

                    SetLibraryDefaultNames();
                }
            }
            else
            {
                if (checkboxImportStandardLibraries.IsChecked == true)
                {
                    SetEnabledForCCLibraryTextFields(false);
                    SetEnabledForBIELibraryTextFields(true);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetCCLibraryDefaultNames();

                    CheckCCLibraries(true);
                }
                else
                {
                    SetEnabledForAllLibraryTextFields(true);

                    SetEnabledForAllLibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(false);
                }
            }
        }

        private void SetEnabledForStandardCCControls(bool newState)
        {
            cbxMajor.IsEnabled = newState;
            cbxMinor.IsEnabled = newState;
            txtComment.IsEnabled = newState;
        }

        private void SetEnabledForAllLibraryCheckBoxes(bool newState)
        {
            SetEnabledForCCLibraryCheckBoxes(newState);
            SetEnabledForBIELibraryCheckBoxes(newState);
        }

        private void LoadStandardLibraryVersions()
        {
            if (versionHandler == null)
            {
                try
                {
                    versionHandler = new FileBasedVersionHandler(new RemoteVersionsFile("http://www.umm-dev.org/xmi/ccl_versions.txt"));

                    versionHandler.RetrieveAvailableVersions();

                    foreach (string majorVersion in versionHandler.GetMajorVersions())
                    {
                        cbxMajor.Items.Add(majorVersion);
                    }

                    cbxMajor.SelectedIndex = cbxMajor.Items.Count - 1;

                    PopulateCbxMinor();
                }
                catch (Exception)
                {
                    // TODO
                }
            }
        }

        private void GatherUserInput()
        {
            modelName = textboxModelName.Text;

            importStandardLibraries = (bool) checkboxImportStandardLibraries.IsChecked ? true : false;

            primLibraryName = (bool) checkboxPRIML.IsChecked ? textboxPRIMLName.Text : "";
            enumLibraryName = (bool) checkboxENUML.IsChecked ? textboxENUMLName.Text : "";
            cdtLibraryName = (bool) checkboxCDTL.IsChecked ? textboxCDTLName.Text : "";
            ccLibraryName = (bool) checkboxCCL.IsChecked ? textboxCCLName.Text : "";
            bdtLibraryName = (bool) checkboxBDTL.IsChecked ? textboxBDTLName.Text : "";
            bieLibraryName = (bool) checkboxBIEL.IsChecked ? textboxBIELName.Text : "";
            docLibraryName = (bool) checkboxDOCL.IsChecked ? textboxDOCLName.Text : "";
        }

        private void PopulateCbxMinor()
        {
            cbxMinor.Items.Clear();

            foreach (string minorVersion in versionHandler.GetMinorVersions(cbxMajor.SelectedItem.ToString()))
            {
                cbxMinor.Items.Add(minorVersion);
            }

            cbxMinor.SelectedIndex = cbxMinor.Items.Count - 1;
            PopulateTxtComment();
        }

        private void PopulateTxtComment()
        {
            txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void CheckIfInputIsValid()
        {
            GatherUserInput();

            if ((modelName.Equals("")) ||
                ((bool) checkboxPRIML.IsChecked && primLibraryName.Equals("")) ||
                ((bool) checkboxENUML.IsChecked && enumLibraryName.Equals("")) ||
                ((bool) checkboxCDTL.IsChecked && cdtLibraryName.Equals("")) ||
                ((bool) checkboxCCL.IsChecked && ccLibraryName.Equals("")) ||
                ((bool) checkboxBDTL.IsChecked && bdtLibraryName.Equals("")) ||
                ((bool) checkboxBIEL.IsChecked && bieLibraryName.Equals("")) ||
                ((bool) checkboxDOCL.IsChecked && docLibraryName.Equals("")))
            {
                buttonGenerate.IsEnabled = false;
            }
            else
            {
                buttonGenerate.IsEnabled = true;
            }
        }

        #endregion

        #region Event Handler Methods

        private void textboxModelName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxPRIMLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxENUMLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCDTLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCCLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBDTLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBIELName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxDOCLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDefaultValues_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateFormState();
        }

        private void checkboxPRIML_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxImportStandardLibraries_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }
            UpdateFormState();
        }

        private void cbxMinor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateTxtComment();
        }

        private void cbxMajor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateCbxMinor();
        }

        private void buttonGenerate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            buttonGenerate.IsEnabled = false;

            GatherUserInput();

            ModelCreator creator = new ModelCreator(repository, ccRepository);
            creator.StatusChanged += OnStatusChanged;

            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
                creator.CreateUpccModel(modelName, bdtLibraryName, bieLibraryName, docLibraryName, resourceDescriptor);
            }
            else
            {
                creator.CreateUpccModel(modelName, primLibraryName, enumLibraryName, cdtLibraryName,
                                        ccLibraryName, bdtLibraryName, bieLibraryName, docLibraryName);
            }

            MessageBox.Show(string.Format(statusMessage, modelName), wizardTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            buttonGenerate.IsEnabled = true;
        }

        private void buttonClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void checkboxPRIML_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxImportStandardLibraries_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }
            UpdateFormState();
        }

        private void checkboxDefaultValues_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateFormState();
        }

        #endregion Event Handler Methods
    }
}