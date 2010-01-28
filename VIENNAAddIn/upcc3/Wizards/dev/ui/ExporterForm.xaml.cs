using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CctsRepository;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for ExporterForm.xaml
    /// </summary>
    public partial class ExporterForm : Window
    {
        private readonly ICctsRepository cctsRepository;
        private readonly Dictionary<string, StackPanel> documentModels = new Dictionary<string, StackPanel>();
        //public ExporterViewModel Model { get; set; }

        public ExporterForm(ICctsRepository cctsRepository)
        {
            //Model = new ExporterViewModel();
            //DataContext = this;
            this.cctsRepository = cctsRepository;

            InitializeComponent();

            documentModels.Add("CCTS", this.panelSettingsCCTS);
            documentModels.Add("ebInterface", this.panelSettingsEbInterface);
            foreach(string item in documentModels.Keys)
            {
                comboboxDocumentModel.Items.Add(item);
            }
        }

        public static void ShowForm(AddInContext context)
        {
            new XsdImporterForm(context.CctsRepository).Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            //Cursor = Cursors.Wait;
            //buttonImport.Visibility = Visibility.Collapsed;

            //switch (tabControl1.SelectedIndex)
            //{
            //    case 0: // ebInterface
            //        new MappingImporter(mappingFilesSelector.FileNames, "ebInterface Invoice", "ebInterface", "ebInterface Types", "ebInterface", "Invoice").ImportMapping(cctsRepository);
            //        break;
            //    case 1: // CCTS
            //        XSDImporter.ImportSchemas(new ImporterContext(cctsRepository, cctsSchemaFileSelector.FileName));
            //        break;
            //}

            //progressBar.Value = 100;
            //textboxStatus.Text += "Import completed!\n";
            //Cursor = Cursors.Arrow;
        }

        private void comboboxDocumentModel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StackPanel tempPanel;
            if(documentModels.TryGetValue((string)comboboxDocumentModel.SelectedItem, out tempPanel))
            {
                foreach(StackPanel panel in documentModels.Values)
                {
                    panel.Visibility = Visibility.Collapsed;
                }
                tempPanel.Visibility = Visibility.Visible;
            }
        }
    }

/*    public class ExporterViewModel : INotifyPropertyChanged
    {
        private List<string> documentModels;

        public ExporterViewModel()
        {
            documentModels = new List<string>();
        }

        public List<string> DocumentModels
        {
            get { return documentModels; }
            set
            {
                documentModels = value;
                OnPropertyChanged("documentModels");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
            }
        }
    }*/
}