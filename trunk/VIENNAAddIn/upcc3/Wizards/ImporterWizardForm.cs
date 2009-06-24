using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter;

namespace VIENNAAddIn.upcc3.Wizards
{
    public partial class ImporterWizardForm : Form
    {
        private CCRepository CcRepository;
        private string FileName;
        private string InputDirectory;

        public ImporterWizardForm()
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

        public ImporterWizardForm(EA.Repository eaRepository)
        {
            InitializeComponent();

            CcRepository = new CCRepository(eaRepository);

            comboModels.DropDownStyle = ComboBoxStyle.DropDownList;

            MirrorModelsToUI();
            SetSafeIndex(comboModels, 0);

        }

        public static void ShowImporterWizard(AddInContext context)
        {            
            new ImporterWizardForm(context.EARepository).Show();
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

            List <SchemaInfo> schemas = new List<SchemaInfo>();

            XmlReader reader = XmlReader.Create(InputDirectory + FileName);
            XmlSchema rootSchema = XmlSchema.Read(reader, null);

            schemas.Add(new SchemaInfo(rootSchema, FileName));

            foreach (XmlSchemaObject schemaObject in rootSchema.Includes)
            {
                if (schemaObject is XmlSchemaInclude)
                {
                    XmlSchemaInclude include = (XmlSchemaInclude) schemaObject;
                    reader = XmlReader.Create(InputDirectory + include.SchemaLocation);
                    XmlSchema includedSchema = XmlSchema.Read(reader, null);
                    schemas.Add(new SchemaInfo(includedSchema, include.SchemaLocation));
                }
            }

            buttonImport.Enabled = false;

            ImporterContext context = new ImporterContext(CcRepository, InputDirectory, schemas, new SchemaInfo(rootSchema, FileName));
            XSDImporter.ccts.XSDImporter.ImportSchemas(context);

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 100;

            richtextStatus.Text += "\nImporting the XML schema named \"" + FileName + "\" completed!";

            Cursor.Current = Cursors.Default;
        }

        private void buttonBrowseFolders_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFileDialog = new OpenFileDialog();

            browseFileDialog.Filter = "XML Schema Document Files (*.xsd)|*.xsd";

            DialogResult dialogResult = browseFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string file = browseFileDialog.FileName;
                
                // extract directory and file name
                int indexLastBackslash = file.LastIndexOf("\\");
                InputDirectory = file.Substring(0, indexLastBackslash + 1);
                FileName = file.Substring(indexLastBackslash + 1);

                textboxRootSchema.Text = InputDirectory + FileName;
            }
        }
    }
}
