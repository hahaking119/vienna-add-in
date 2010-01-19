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
using VIENNAAddIn.upcc3.Wizards.util;
using Visifire.Charts;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SchemaAnalyzer
    {
        private AnalyzerResults results;

        public SchemaAnalyzer(string File1, string File2)
        {
            InitializeComponent();
            results = new AnalyzerResults(new AnalyzerResult("item1", 5)).Add(new AnalyzerResult("item2", 3)).Add(new AnalyzerResult("item 3", 10));
            foreach(AnalyzerResult dataset in results.Items)
            {
                DataPoint item = new DataPoint();
                item.YValue = dataset.Count;
                chart1.Series[0].DataPoints.Add(item);
            }
            Chart dummy = new Chart(); // workaround for assembly-error, we will never use this dummy object
        }

        public static void ShowForm(AddInContext context)
        {
            new SchemaAnalyzer("","").ShowDialog();
        }
    }

    public class AnalyzerResult
    {
        public string Caption { get; set; }
        public int Count { get; set; }

        public AnalyzerResult(string newCaption, int newCount)
        {
            Caption = newCaption;
            Count = newCount;
        }
    }

    public class AnalyzerResults
    {
        private List<AnalyzerResult> items;
        public List<AnalyzerResult> Items
        {
            get
            {
                return items;
            }
        }

        public AnalyzerResults()
        {
            items = new List<AnalyzerResult>();
        }

        public AnalyzerResults(AnalyzerResult newItem)
        {
            items = new List<AnalyzerResult>();
            Add(newItem);
        }

        public AnalyzerResults Add(AnalyzerResult newItem)
        {
            items.Add(newItem);
            return this;
        }
    }
}