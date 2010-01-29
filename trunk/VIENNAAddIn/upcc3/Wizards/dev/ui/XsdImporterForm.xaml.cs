using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CctsRepository;
using Microsoft.Win32;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class XsdImporterForm
    {
        private readonly ICctsRepository cctsRepository;
        public XsdImporterViewModel Model { get; set; }

        public XsdImporterForm(ICctsRepository cctsRepository)
        {
            Model = new XsdImporterViewModel();
            DataContext = this;
            this.cctsRepository = cctsRepository;

            InitializeComponent();

            mappedSchemaFileSelector.FileNameChanged += MappedSchemaFileSelectorFileNameChanged;
            mappedSchemaFileSelector.FileName = " ";
            mappedSchemaFileSelector.FileName = "";
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
            //buttonImport.Visibility = Visibility.Collapsed;

            switch (tabControl1.SelectedIndex)
            {
                case 0: // ebInterface
                    //new MappingImporter(mappingFilesSelector.FileNames, "ebInterface Invoice", "ebInterface", "ebInterface Types", "ebInterface", "Invoice").ImportMapping(cctsRepository);
                    new MappingImporter(Model.MappingFiles, new[] { mappedSchemaFileSelector.FileName }, docLibraryNameTextBox.Text, bieLibraryNameTextBox.Text, bdtLibraryNameTextBox.Text, qualifierTextBox.Text, rootElementNameTextBox.Text).ImportMapping(cctsRepository);
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
        }

        private void RemoveSelection_Click(object sender, RoutedEventArgs e)
        {
            if (mappingFilesListBox.SelectedIndex > -1)
            {
                List<string> tempList = new List<string>(Model.MappingFiles);
                tempList.RemoveAt(mappingFilesListBox.SelectedIndex);
                Model.MappingFiles = tempList;
            }
        }

        private void ButtonSchemaAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            new SchemaAnalyzer(mappedSchemaFileSelector.FileName, "").ShowDialog();
        }
    }

    public class XsdImporterViewModel : INotifyPropertyChanged
    {
        private List<string> mappingFiles;

        public XsdImporterViewModel()
        {
            mappingFiles = new List<string>();
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