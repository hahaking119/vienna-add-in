using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using Microsoft.Win32;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class XsdImporterForm
    {
        private readonly ICctsRepository cctsRepository;
        private ICcLibrary selectedCcLibrary;
        private IBLibrary selectedBLibrary;
        public XsdImporterViewModel Model { get; set; }

        public XsdImporterForm(ICctsRepository cctsRepository)
        {
            Model = new XsdImporterViewModel();
            DataContext = this;
            this.cctsRepository = cctsRepository;

            InitializeComponent();

            buttonImport.IsEnabled = false;
            Model.CcLibraries = new List<ICcLibrary>(this.cctsRepository.GetCcLibraries());
            if (Model.CcLibraries.Count > 0)
                ccLibraryComboBox.SelectedIndex = 0;
            Model.BLibraries = new List<IBLibrary>(this.cctsRepository.GetBLibraries());
            if (Model.BLibraries.Count > 0)
                bLibraryComboBox.SelectedIndex = 0;

            mappedSchemaFileSelector.FileNameChanged += MappedSchemaFileSelectorFileNameChanged;
            mappedSchemaFileSelector.FileName = " ";
            mappedSchemaFileSelector.FileName = "";
        }

        private void CheckIfInputIsValid()
        {
            buttonImport.IsEnabled = (ccLibraryComboBox.SelectedIndex > -1 &&
                                      bLibraryComboBox.SelectedIndex > -1 &&
                                      mappingFilesListBox.Items.Count > 0 &&
                                      !string.IsNullOrEmpty(mappedSchemaFileSelector.FileName));
        }

        private void MappedSchemaFileSelectorFileNameChanged(object sender, RoutedEventArgs args)
        {
            if (mappedSchemaFileSelector.FileName.Length > 0)
            {
                ButtonSchemaAnalyzer.IsEnabled = true;
                ButtonSchemaAnalyzerImage.Opacity = 1;
            } else
            {
                ButtonSchemaAnalyzer.IsEnabled = false;
                ButtonSchemaAnalyzerImage.Opacity = 0.3;
            }
            CheckIfInputIsValid();
        }

        public static void ShowForm(AddInContext context)
        {
            new XsdImporterForm(context.CctsRepository).ShowDialog();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            buttonImport.IsEnabled = false;

            switch (tabControl1.SelectedIndex)
            {
                case 0: // ebInterface
                    //new MappingImporter(mappingFilesSelector.FileNames, "ebInterface Invoice", "ebInterface", "ebInterface Types", "ebInterface", "Invoice").ImportMapping(cctsRepository);
                    new MappingImporter(selectedCcLibrary, selectedBLibrary, Model.MappingFiles, new[] { mappedSchemaFileSelector.FileName }, docLibraryNameTextBox.Text, bieLibraryNameTextBox.Text, bdtLibraryNameTextBox.Text, qualifierTextBox.Text, rootElementNameTextBox.Text).ImportMapping(cctsRepository);
                    break;
                case 1: // CCTS
                    throw new NotImplementedException();
                    //XSDImporter.ImportSchemas(new ImporterContext(cctsRepository, cctsSchemaFileSelector.FileName));
            }
            
            textboxStatus.Text += "Import completed!\n";
            Cursor = Cursors.Arrow;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              DefaultExt = ".mfd",
                              Filter = "MapForce Mapping files (.mfd)|*.mfd",
                              Multiselect = true
                          };
            if (dlg.ShowDialog() == true)
            {
                List<string> tempList = new List<string>(Model.MappingFiles);
                foreach(string file in dlg.FileNames)
                {
                    bool exists = false;
                    foreach(string item in Model.MappingFiles)
                    {
                        if(item.Equals(file))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        tempList.Add(file);
                    else
                        MessageBox.Show("The file '" + file + "' already exists in the list!", Title,
                                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                Model.MappingFiles = tempList;
            }
            CheckIfInputIsValid();
        }

        private void RemoveSelection_Click(object sender, RoutedEventArgs e)
        {
            if (mappingFilesListBox.SelectedIndex > -1)
            {
                List<string> tempList = new List<string>(Model.MappingFiles);
                tempList.RemoveAt(mappingFilesListBox.SelectedIndex);
                Model.MappingFiles = tempList;
            }
            CheckIfInputIsValid();
        }

        private void ButtonSchemaAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            new SchemaAnalyzer(mappedSchemaFileSelector.FileName, "").ShowDialog();
        }

        private void ccLibraryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ccLibraryComboBox.SelectedIndex > -1)
            {
                selectedCcLibrary = Model.CcLibraries[ccLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }

        private void bLibraryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (bLibraryComboBox.SelectedIndex > -1)
            {
                selectedBLibrary = Model.BLibraries[bLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }
    }

    public class XsdImporterViewModel : INotifyPropertyChanged
    {
        private List<string> mappingFiles;
        private List<ICcLibrary> ccLibraries;
        private List<IBLibrary> bLibraries;

        public XsdImporterViewModel()
        {
            mappingFiles = new List<string>();
            ccLibraries = new List<ICcLibrary>();
            bLibraries = new List<IBLibrary>();
        }

        public List<ICcLibrary> CcLibraries
        {
            get { return ccLibraries; }
            set
            {
                ccLibraries = value;
                OnPropertyChanged("CcLibraries");
            }
        }

        public List<IBLibrary> BLibraries
        {
            get { return bLibraries; }
            set
            {
                bLibraries = value;
                OnPropertyChanged("BLibraries");
            }
        }

        public List<string> MappingFiles
        {
            get { return mappingFiles; }
            set
            {
                mappingFiles = value;
                OnPropertyChanged("MappingFiles");
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
    }
}