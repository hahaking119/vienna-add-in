using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.upcc3.Wizards
{
    /// <summary>
    /// Interaction logic for ImporterWizardForm.xaml
    /// </summary>
    public partial class ImporterWizard : Window
    {
        private int mappingfilesEbInterface = 1;
        private Dictionary<string, TextBox> textboxes = new Dictionary<string, TextBox>();
        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        public ImporterWizard()
        {
            InitializeComponent();
            buttons.Add("mappingFileB_1", this.mappingFileB_1);
            textboxes.Add("mappingFileT_1", this.mappingFileT_1);
        }

        public static void ShowForm(AddInContext context)
        {
            new ImporterWizard().Show();
        }

        private string OpenFileDialog(string ext, string filter)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ext; // Default file extension
            dlg.Filter = filter; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
           if (result == true)
            {
                return dlg.FileName;
            }
            return null;
        }

        private void AddControlMappingFiles()
        {
            gridEbInterface.Height += 100;
            textboxes["mappingFileT_" + mappingfilesEbInterface].TextChanged -= new TextChangedEventHandler(textboxSelectMappingFile1_TextChanged);
            mappingfilesEbInterface++;

            Label newLbl = new Label();
            newLbl.Content = "Select another mapping file:";
            gridEbInterface.Children.Add(newLbl);
            //Canvas.SetTop(newLbl, 130);
            Canvas.SetLeft(newLbl, 5);

            Button newBtn = new Button();
            buttons.Add("mappingFileB_" + mappingfilesEbInterface, newBtn);
            newBtn.Content = "...";
            newBtn.Name = "mappingFileB_" + mappingfilesEbInterface;
            newBtn.Width = 28;
            newBtn.Height = 23;
            gridEbInterface.Children.Add(newBtn);
            //Canvas.SetTop(newBtn, 153);
            Canvas.SetLeft(newBtn, 40);
            newBtn.Click += new RoutedEventHandler(buttonBrowseFoldersMapping1_Click);

            TextBox newTxt = new TextBox();
            textboxes.Add("mappingFileT_" + mappingfilesEbInterface, newTxt);
            newTxt.Name = "mappingFileT_" + mappingfilesEbInterface;
            newTxt.Width = 291;
            newTxt.Height = 23;
            gridEbInterface.Children.Add(newTxt);
            //Canvas.SetTop(newTxt, 153);
            Canvas.SetLeft(newTxt, 0);
            newTxt.TextChanged += new TextChangedEventHandler(textboxSelectMappingFile1_TextChanged);

        }

        private void textboxSelectMappingFile1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                AddControlMappingFiles();
            }
        }
        
        private void buttonBrowseFolders_Click(object sender, RoutedEventArgs e)
        {
            string filename = OpenFileDialog(".xsc", "XML Schema files (.xsd)|*.xsd");
            if (!string.IsNullOrEmpty(filename))
            {
                textboxRootSchema.Text = filename;
            }
        }

        private void buttonBrowseFoldersMapping1_Click(object sender, RoutedEventArgs e)
        {
            string filename = OpenFileDialog(".*", "Mapping files|*.*");
            if (!string.IsNullOrEmpty(filename))
            {
                string receiver = "mappingFileT_" + ((Button)sender).Name.Split('_')[1];
                textboxes[receiver].Text = filename;
            }
        }

        private void buttonBrowseFolders2_Click(object sender, RoutedEventArgs e)
        {
            string filename = OpenFileDialog(".xsc", "XML Schema files (.xsd)|*.xsd");
            if (!string.IsNullOrEmpty(filename))
            {
                textboxEbInterfaceSchema.Text = filename;
            }
        }
    }
}
