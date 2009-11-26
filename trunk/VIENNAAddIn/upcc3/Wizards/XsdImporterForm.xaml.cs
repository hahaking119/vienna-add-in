using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.cctsndr;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddIn.upcc3.Wizards.util;
using System.Linq;

namespace VIENNAAddIn.upcc3.Wizards
{
    /// <summary>
    /// Interaction logic for XsdImporterForm.xaml
    /// </summary>
    public partial class XsdImporterForm : Window
    {
        private readonly ICCRepository ccRepository;
        private readonly MultipleFilesSelector mappingFilesSelector;

        public XsdImporterForm(ICCRepository ccRepository)
        {
            this.ccRepository = ccRepository;

            InitializeComponent();

//            mappingFilesSelector = new MultipleFilesSelector(".*", "Mapping files|*.*")
//                                   {
//                                       Width = 415,
//                                   };
//            mappingFileSelectorScrollViewer.Content = mappingFilesSelector;
        }

        public static void ShowForm(AddInContext context)
        {
            new XsdImporterForm(context.CCRepository).Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = Resources["viewModel"] as XsdImporterFormViewModel;
            var files = new List<FileName>(viewModel.MappingFiles)
                        {
                            "foo"
                        };
            viewModel.MappingFiles = files;
//            MessageBox.Show("Mapped schema: " + viewModel.MappedSchemaFile););
            return;
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

    public class DelegateCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public DelegateCommand(Action execute) : this(execute, () => true)
        {
            this.execute = execute;
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T backingField, T newValue, string propertyName)
        {
            if (Equals(backingField, newValue))
            {
                return;
            }
            backingField = newValue;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class XsdImporterFormViewModel : ViewModel
    {
        public XsdImporterFormViewModel()
        {
            var emptyFileName = new FileName(string.Empty);
            emptyFileName.ValueChanged += FileNameValueChanged;
            mappingFiles = new List<FileName>
                           {
                               emptyFileName
                           };
        }

        private void FileNameValueChanged(string newValue)
        {
            if (mappingFiles.Last().Value.Length != 0)
            {
                var emptyFileName = new FileName(string.Empty);
                emptyFileName.ValueChanged += FileNameValueChanged;
                var files = new List<FileName>(mappingFiles)
                            {
                                emptyFileName
                            };

                MappingFiles = files;
            }
        }

        private string mappedSchemaFile;

        public string MappedSchemaFile
        {
            get { return mappedSchemaFile; }
            set { SetProperty(ref mappedSchemaFile, value, "mappedSchemaFile"); }
        }

        private List<FileName> mappingFiles;

        public List<FileName> MappingFiles
        {
            get { return mappingFiles; }
            set
            {
//                if (value.Last().Length != 0)
//                {
//                    value.Add(string.Empty);
//                }
                SetProperty(ref mappingFiles, value, "mappingFiles");
            }
        }
    }

    public class FileName
    {
        public FileName(string value)
        {
            this.value = value;
        }

        public static implicit operator FileName(string value)
        {
            return new FileName(value);
        }

        public event Action<string> ValueChanged = s => { };

        private string value;

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                ValueChanged(value);
            }
        }
    }
}