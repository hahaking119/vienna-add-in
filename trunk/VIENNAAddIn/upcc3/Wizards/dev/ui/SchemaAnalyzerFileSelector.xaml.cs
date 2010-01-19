// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzerFileSelector
    {
        private SchemaAnalyzer parent;
        public SchemaAnalyzerFileSelector(string file1, string file2, SchemaAnalyzer _parent)
        {
            InitializeComponent();
            fileSelector1.FileName = file1;
            fileSelector2.FileName = file2;
            parent = _parent;
        }

        private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            parent.SetFiles(fileSelector1.FileName, fileSelector2.FileName);
            Close();
        }

        private void buttonClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            parent.FileSelectorCancelled();
        }

        public static void ShowForm(string file1, string file2, SchemaAnalyzer parent)
        {
            new SchemaAnalyzerFileSelector(file1, file1, parent).ShowDialog();
        }
    }
}
