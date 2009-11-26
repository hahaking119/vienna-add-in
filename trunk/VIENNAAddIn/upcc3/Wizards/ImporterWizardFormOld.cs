﻿using System;
using System.Windows.Forms;
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.cctsndr;

namespace VIENNAAddIn.upcc3.Wizards
{
    public partial class ImporterWizardFormOld : Form
    {
        private readonly ICCRepository CcRepository;

        public ImporterWizardFormOld()
        {
            InitializeComponent();
        }

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

        private void MirrorModelsToUI()
        {
            comboModels.Items.Add("CCTS");
        }

        public ImporterWizardFormOld(ICCRepository ccRepository)
        {
            InitializeComponent();

            CcRepository = ccRepository;

            comboModels.DropDownStyle = ComboBoxStyle.DropDownList;

            MirrorModelsToUI();
            SetSafeIndex(comboModels, 0);

        }

        public static void ShowImporterWizard(AddInContext context)
        {
            new ImporterWizardFormOld(context.CCRepository).Show();
        }

        private void ImporterWizardForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            buttonImport.Enabled = false;

            ImporterContext context = new ImporterContext(CcRepository, textboxRootSchema.Text);
            import.cctsndr.XSDImporter.ImportSchemas(context);

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 100;

            richtextStatus.Text += "\nImporting the XML schema named \"" + context.RootSchemaFileName + "\" completed!";

            Cursor.Current = Cursors.Default;
        }

        private void buttonBrowseFolders_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFileDialog = new OpenFileDialog
                                              {
                                                  Filter = "XML Schema Document Files (*.xsd)|*.xsd"
                                              };

            DialogResult dialogResult = browseFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                textboxRootSchema.Text = browseFileDialog.FileName;
            }
        }
    }
}
