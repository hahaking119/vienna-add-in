// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using VIENNAAddIn.upcc3.Wizards.util;
using Visifire.Charts;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private SchemaAnalyzerResults results;

        public SchemaAnalyzer(string File1, string File2)
        {
            InitializeComponent();
            results = new SchemaAnalyzerResults(new SchemaAnalyzerResult("item1", 5)).Add(new SchemaAnalyzerResult("item2", 3)).Add(new SchemaAnalyzerResult("item 3", 10));
            chart1.Series[0].DataPoints.Clear();
            foreach (SchemaAnalyzerResult dataset in results.Items)
            {
                DataPoint item = new DataPoint();
                item.YValue = dataset.Count;
                //item.
                chart1.Series[0].DataPoints.Add(item);
            }
            Chart dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
        }

        public static void ShowForm(AddInContext context)
        {
            new SchemaAnalyzer("","").ShowDialog();
        }
    }
}