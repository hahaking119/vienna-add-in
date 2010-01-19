// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using Visifire.Charts;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private SchemaAnalyzerResults results;
        private bool started = false;

        public SchemaAnalyzer()
        {
            InitializeComponent();
            Chart dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
            new SchemaAnalyzerFileSelector("", "", this).ShowDialog();
        }

        public void SetFiles(string file1, string file2)
        {
            if (file1.Length == 0) {
                FileSelectorCancelled();
                return;
            }
            Analyze(file1, 1);
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

        public void FileSelectorCancelled()
        {
            if(!started) Close();
        }

        private void Analyze(string file, int chart)
        {
            started = true;
            results = XMLSchemaReader.Read(file);
            (chart==1 ? chart1 : chart2).Series[0].DataPoints.Clear();
            foreach (SchemaAnalyzerResult dataset in results.Items)
            {
                DataPoint item = new DataPoint();
                item.YValue = dataset.Count;
                item.AxisXLabel = dataset.Caption;
                (chart == 1 ? chart1 : chart2).Series[0].DataPoints.Add(item);
            }
        }

        public static void ShowForm(AddInContext context)
        {
            new SchemaAnalyzer().ShowDialog();
        }
    }
}