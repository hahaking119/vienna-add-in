using System;
using System.Windows.Forms;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.upcc3.Wizards
{
    public partial class ImporterWizardForm : Form
    {
        public ImporterWizardForm()
        {
            InitializeComponent();
        }

        public ImporterWizardForm(EA.Repository eaRepository)
        {
            InitializeComponent();
        }

        public static void ShowImporterWizard(AddInContext context)
        {            
            new ImporterWizardForm(context.Repository).Show();
        }

        private void ImporterWizardForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
