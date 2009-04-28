using System;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards
{
    ///<summary>
    ///</summary>
    public partial class UpccModelWizardForm : Form
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
        private Repository repository;

        #endregion

        ///<summary>
        ///</summary>
        public UpccModelWizardForm(Repository eaRepository)
        {
            InitializeComponent();

            repository = eaRepository;
        }

        #region Convenience Methods

        private void EnableAllLibraryCheckBoxes(bool newState)
        {
            checkboxPRIML.Enabled = newState;
            checkboxENUML.Enabled = newState;
            checkboxCDTL.Enabled = newState;
            checkboxCCL.Enabled = newState;
            checkboxBDTL.Enabled = newState;
            checkboxBIEL.Enabled = newState;
            checkboxDOCL.Enabled = newState;
        }

        private void CheckAllLibraries(CheckState newState)
        {
            checkboxPRIML.CheckState = newState;
            checkboxENUML.CheckState = newState;
            checkboxCDTL.CheckState = newState;
            checkboxCCL.CheckState = newState;
            checkboxBDTL.CheckState = newState;
            checkboxBIEL.CheckState = newState;
            checkboxDOCL.CheckState = newState;
        }

        private void EnableAllLibraryTextFields(bool newState)
        {
            textboxPRIMLName.Enabled = newState;
            textboxENUMLName.Enabled = newState;
            textboxCDTLName.Enabled = newState;
            textboxCCLName.Enabled = newState;
            textboxBDTLName.Enabled = newState;
            textboxBIELName.Enabled = newState;
            textboxDOCLName.Enabled = newState;
        }

        private void SetModelDefaultName()
        {
            textboxModelName.Text = "Default Model";
        }

        private void SetLibraryDefaultNames()
        {
            textboxPRIMLName.Text = "PRIMLibrary";
            textboxENUMLName.Text = "ENUMLibrary";
            textboxCDTLName.Text = "CDTLibrary";
            textboxCCLName.Text = "CCLibrary";
            textboxBDTLName.Text = "BDTLibrary";
            textboxBIELName.Text = "BIELibrary";
            textboxDOCLName.Text = "DOCLibrary";
        }

        private void ResetForm(short resetLevel)
        {
            switch (resetLevel)
            {
                case 1:
                    checkboxDefaultValues.CheckState = CheckState.Checked;
                    CheckAllLibraries(CheckState.Checked);
                    EnableAllLibraryCheckBoxes(false);
                    EnableAllLibraryTextFields(false);
                    break;

                case 2:
                    EnableAllLibraryCheckBoxes(true);
                    EnableAllLibraryTextFields(true);
                    break;

                case 3:
                    break;
            }
        }

        private void GatherUserInput()
        {
            modelName = textboxModelName.Text;

            primLibraryName = checkboxPRIML.Checked ? textboxPRIMLName.Text : "";
            enumLibraryName = checkboxENUML.Checked ? textboxENUMLName.Text : "";
            cdtLibraryName = checkboxCDTL.Checked ? textboxCDTLName.Text : "";
            ccLibraryName = checkboxCCL.Checked ? textboxCCLName.Text : "";
            bdtLibraryName = checkboxBDTL.Checked ? textboxBDTLName.Text : "";
            bieLibraryName = checkboxBIEL.Checked ? textboxBIELName.Text : "";
            docLibraryName = checkboxDOCL.Checked ? textboxDOCLName.Text : "";
        }

        private void CheckIfInputIsValid()
        {
            GatherUserInput();

            if ((modelName.Equals("")) ||
                (checkboxPRIML.Checked && primLibraryName.Equals("")) ||
                (checkboxENUML.Checked && enumLibraryName.Equals("")) ||
                (checkboxCDTL.Checked && cdtLibraryName.Equals("")) ||
                (checkboxCCL.Checked && ccLibraryName.Equals("")) ||
                (checkboxBDTL.Checked && bdtLibraryName.Equals("")) ||
                (checkboxBIEL.Checked && bieLibraryName.Equals("")) ||
                (checkboxDOCL.Checked && docLibraryName.Equals("")))
            {
                buttonGenerate.Enabled = false;
            }
            else
            {
                buttonGenerate.Enabled = true;
            }
        }

        #endregion

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            buttonGenerate.Enabled = false;

            GatherUserInput();

            ModelCreator creator = new ModelCreator(repository);

            creator.CreateUpccModel(modelName, primLibraryName, enumLibraryName, cdtLibraryName,
                                    ccLibraryName, bdtLibraryName, bieLibraryName, docLibraryName);

            buttonGenerate.Enabled = true;
        }

        private void checkboxDefaultValues_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxDefaultValues.CheckState == CheckState.Checked)
            {
                ResetForm(1);

                SetLibraryDefaultNames();
            }
            else
            {
                ResetForm(2);
            }
        }

        private void UPCCModelWizardForm_Load(object sender, EventArgs e)
        {
            ResetForm(1);

            SetModelDefaultName();
            SetLibraryDefaultNames();
        }

        private void textboxModelName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxPRIMLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxENUMLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCDTLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCCLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBDTLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBIELName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxDOCLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxPRIML_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        public static void ShowForm(AddInContext context)
        {
            new UpccModelWizardForm(context.Repository).Show();
        }
    }
}