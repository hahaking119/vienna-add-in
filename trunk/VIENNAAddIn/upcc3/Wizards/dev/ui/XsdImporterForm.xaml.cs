using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using Microsoft.Win32;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.util;
using System.Linq;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for XsdImporterForm.xaml
    /// </summary>
    public partial class XsdImporterForm : Window
    {
        private readonly ICctsRepository cctsRepository;
        public XsdImporterViewModel Model { get; set; }

        public XsdImporterForm(ICctsRepository cctsRepository)
        {
            Model = new XsdImporterViewModel();
            DataContext = this;
            this.cctsRepository = cctsRepository;

            InitializeComponent();

            mappedSchemaFileSelector.FileNameChanged += mappedSchemaFileSelector_FileNameChanged;
            mappedSchemaFileSelector.FileName = " ";
            mappedSchemaFileSelector.FileName = "";
        }

        private void mappedSchemaFileSelector_FileNameChanged(object sender, RoutedEventArgs args)
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
            new XsdImporterForm(context.CctsRepository).Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              DefaultExt = ".xsd",
                              Filter = "XML Schema files (.xsd)|*.xsd",
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
                        MessageBox.Show("The file '" + file + "' already exists in the list!", this.Title,
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

/*    public class XsdImporterViewModel : INotifyPropertyChanged
    {
        private List<Filename> mappingFiles;

        public XsdImporterViewModel()
        {
            Filename newFilename1 = new Filename();
            Filename newFilename2 = new Filename();
            newFilename1.ValueChanged += FileNameValueChanged;
            newFilename2.ValueChanged += FileNameValueChanged;
            mappingFiles = new List<Filename> { newFilename1, newFilename2 };
        }

        public List<Filename> MappingFiles
        {
            get { return mappingFiles; }
            set
            {
                mappingFiles = value;
                OnPropertyChanged("MappingFiles");
            }
        }

        private void FileNameValueChanged(string newValue)
        {
            if (mappingFiles[mappingFiles.Count - 2].File.Length != 0 || mappingFiles.Last().File.Length != 0)
            {
                var emptyFilename = new Filename();
                emptyFilename.ValueChanged += FileNameValueChanged;
                var files = new List<Filename>(mappingFiles)
                {
                    emptyFilename
                };
                MappingFiles = files;
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

    public class Filename
    {
        private string file;

        public string File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                ValueChanged(value);
            }
        }

        public event Action<string> ValueChanged = s => { };

        public Filename()
        {
            File = string.Empty;
        }

        public Filename(string newFile)
        {
            File = newFile;
        }
    }*/
}