using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards
{
    public partial class StandardLibraryImporterForm : Form
    {
        VersionHandler versionHandler;

        public static void ShowForm(AddInContext context)
        {
            new StandardLibraryImporterForm().ShowDialog();
        }
        
        public StandardLibraryImporterForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StandardLibraryImporterForm_Load(object sender, EventArgs e)
        {
            cbxMajor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMinor.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                versionHandler = new VersionHandler(new WebClientMediator(), "http://www.umm-dev.org/xmi/ccl_versions.txt");

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

        private void cbxMajor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateCbxMinor();
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

        private void cbxMinor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateTxtComment();
        }

        private void PopulateTxtComment()
        {
            txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            const string warnMessage = "Importing the standard CC libraries will overwrite all existing:\n\n"
                           + "    - ENUM libraries named \"ENUMLibrary\",\n"
                           + "    - PRIM libraries named \"PRIMLibrary\",\n"
                           + "    - CDT libraries named \"CDTLibrary \", and \n"
                           + "    - CC libraries named \"CCLibrary\"\n\n"
                           + "Are you sure you want to proceed?";
            const string caption = "VIENNA Add-In Warning";

            DialogResult dialogResult = MessageBox.Show(warnMessage, caption, MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.Yes)
            {
                //Cursor.Current = Cursors.WaitCursor;

                //string bLibraryGuid = repository.GetTreeSelectedPackage().Element.ElementGUID;
                //Package bLibrary = repository.GetPackageByGuid(bLibraryGuid);

                //LibraryImporter importer = new LibraryImporter(repository);
                //importer.ImportStandardCcLibraries(bLibrary);

                //Cursor.Current = Cursors.Default;
            }
        }
    }
}
