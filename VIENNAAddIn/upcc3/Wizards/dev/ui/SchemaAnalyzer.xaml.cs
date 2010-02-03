// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using System.Xml;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using Visifire.Charts;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private SchemaAnalyzerResults results;
        private bool started;
        private string file1 = "";
        private string file2 = "";

        public SchemaAnalyzer()
        {
            InitializeComponent();
            Chart dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
            LoadFiles();
        }

        public SchemaAnalyzer(string file1, string file2)
        {
            InitializeComponent();
            Chart dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
            this.file1 = file1;
            this.file2 = file2;
            LoadFiles();
        }

        private void LoadFiles()
        {
            fileSelector1.FileName = file1;
            fileSelector2.FileName = file2;
            innerCanvas.Visibility = System.Windows.Visibility.Visible;
        }

        private bool SetFiles()
        {
            if (file1.Length == 0) {
                FileSelectorCancelled();
                return false;
            }
            if(!Analyze(file1, 1))
                return false;
            if (file2.Length > 0)
            {
                if (!Analyze(file2, 2))
                    return false;
                tab2.IsEnabled = true;
                tab3.IsEnabled = false;
            }
            else
            {
                tab2.IsEnabled = true;
                tab3.IsEnabled = false;
            }
            return true;
        }

        private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            file1 = fileSelector1.FileName;
            file2 = fileSelector2.FileName;
            if(SetFiles())
                innerCanvas.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void buttonClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FileSelectorCancelled();
            innerCanvas.Visibility = System.Windows.Visibility.Collapsed;
        }
        
        private void FileSelectorCancelled()
        {
            if(!started) Close();
        }

        private bool Analyze(string file, int chart)
        {
            try
            {
                results = new XMLSchemaReader().Read(file);
            }
            catch (FileNotFoundException fnfe)
            {
                System.Windows.Forms.MessageBox.Show("The given file could not be opened!", Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                System.Windows.Forms.MessageBox.Show("The given path could not be opened!", Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            catch (XmlException xe)
            {
                System.Windows.Forms.MessageBox.Show("The given file could not be read!", Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            started = true;
            (chart==1 ? chart1 : chart2).Series[0].DataPoints.Clear();
            foreach (SchemaAnalyzerResult dataset in results)
            {
                DataPoint item = new DataPoint {YValue = dataset.Count, AxisXLabel = dataset.Caption};
                (chart == 1 ? chart1 : chart2).Series[0].DataPoints.Add(item);
            }
            (chart == 1 ? tab1 : tab2).Header = file.Substring(file.LastIndexOf("\\")+1);
            return true;
        }

        public static void ShowForm(AddInContext context)
        {
            new SchemaAnalyzer().ShowDialog();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadFiles();
        }
    }
}