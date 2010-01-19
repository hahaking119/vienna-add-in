// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Windows.Forms;
using System.Windows.Input;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private readonly Repository eaRepository;
        FileBasedVersionHandler versionHandler;

        public SchemaAnalyzer(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
            InitializeComponent();
            WindowLoaded();
        }

        public static void ShowForm(AddInContext context)
        {
            new StandardLibraryImporter(context.EARepository).ShowDialog();
        }

        private void WindowLoaded()
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
                // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
                // ReSharper restore EmptyGeneralCatchClause
            {
                // TODO
            }
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
            if (cbxMajor.SelectedItem != null && cbxMinor.SelectedItem != null)
                txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void OnStatusChanged(string statusMessage)
        {
            rtxtStatus.Text = statusMessage + "\n" + rtxtStatus.Text;
        }

        private void cbxMinor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateTxtComment();
        }

        private void cbxMajor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateCbxMinor();
        }

        private void buttonImport_Click(object sender, System.Windows.RoutedEventArgs e)
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

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                buttonClose.IsEnabled = false;
                buttonImport.IsEnabled = false;

                string bLibraryGuid = eaRepository.GetTreeSelectedPackage().Element.ElementGUID;
                Package bLibrary = eaRepository.GetPackageByGuid(bLibraryGuid);

                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());

                LibraryImporter importer = new LibraryImporter(eaRepository, resourceDescriptor);
                importer.StatusChanged += OnStatusChanged;
                importer.ImportStandardCcLibraries(bLibrary);

                buttonClose.IsEnabled = true;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void buttonClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}