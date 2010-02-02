// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
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

        private void SetFiles()
        {
            if (file1.Length == 0) {
                FileSelectorCancelled();
                return;
            }
//            try
//            {
                Analyze(file1, 1);
/*            }
            catch (FileNotFoundException fnfe)
            {
                System.Windows.Forms.MessageBox.Show("The file could not be openend!", Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                LoadFiles();
                return;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                System.Windows.Forms.MessageBox.Show("The file could not be openend!", Title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                LoadFiles();
                return;
            }*/
            if (file2.Length > 0)
            {
                Analyze(file2, 2);
                tab2.IsEnabled = true;
                tab3.IsEnabled = true;
            }
            else
            {
                tab2.IsEnabled = false;
                tab3.IsEnabled = false;
            }
        }

        private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            file1 = fileSelector1.FileName;
            file2 = fileSelector2.FileName;
            innerCanvas.Visibility = System.Windows.Visibility.Collapsed;
            SetFiles();
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

        private void Analyze(string file, int chart)
        {
            results = XMLSchemaReader.Read(file);
            started = true;
            (chart==1 ? chart1 : chart2).Series[0].DataPoints.Clear();
            foreach (SchemaAnalyzerResult dataset in results)
            {
                DataPoint item = new DataPoint {YValue = dataset.Count, AxisXLabel = dataset.Caption};
                (chart == 1 ? chart1 : chart2).Series[0].DataPoints.Add(item);
            }
            (chart == 1 ? tab1 : tab2).Header = file.Substring(file.LastIndexOf("\\")+1);
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