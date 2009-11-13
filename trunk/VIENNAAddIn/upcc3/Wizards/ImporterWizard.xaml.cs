using System.Windows;
using System.Windows.Input;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.cctsndr;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards
{
    /// <summary>
    /// Interaction logic for ImporterWizard.xaml
    /// </summary>
    public partial class ImporterWizard : Window
    {
        private readonly CCRepository ccRepository;
        private readonly FileSelector cctsSchemaFileSelector;
        private readonly FileSelector mappedSchemaFileSelector;
        private readonly MultipleFilesSelector mappingFilesSelector;

        public ImporterWizard(Repository eaRepository)
        {
            ccRepository = new CCRepository(eaRepository);

            InitializeComponent();

            cctsSchemaFileSelector = new FileSelector(".xsc", "XML Schema files (.xsd)|*.xsd")
                                     {
                                         Width = 415,
                                     };
            cctsSchemaFileSelectorPanel.Children.Add(cctsSchemaFileSelector);

            mappedSchemaFileSelector = new FileSelector(".xsc", "XML Schema files (.xsd)|*.xsd")
                                       {
                                           Width = 415,
                                       };
            mappedSchemaFileSelectorPanel.Children.Add(mappedSchemaFileSelector);

            mappingFilesSelector = new MultipleFilesSelector(".*", "Mapping files|*.*")
                                   {
                                       Width = 415,
                                   };
            mappingFileSelectorScrollViewer.Content = mappingFilesSelector;
        }

        public static void ShowForm(AddInContext context)
        {
            new ImporterWizard(context.EARepository).Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            buttonImport.Visibility = Visibility.Collapsed;

            switch (tabControl1.SelectedIndex)
            {
                case 0: // ebInterface
                    new MappingImporter(mappingFilesSelector.FileNames, "ebInterface Invoice", "ebInterface", "ebInterface Types", "ebInterface", "Invoice").ImportMapping(ccRepository);
                    break;
                case 1: // CCTS
                    XSDImporter.ImportSchemas(new ImporterContext(ccRepository, cctsSchemaFileSelector.FileName));
                    break;
            }

            progressBar.Value = 100;
            textboxStatus.Text += "Import completed!\n";
            Cursor = Cursors.Arrow;
        }
    }
}