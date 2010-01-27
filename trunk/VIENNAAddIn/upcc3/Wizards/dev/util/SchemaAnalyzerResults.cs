﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class SchemaAnalyzerResults:List<SchemaAnalyzerResult>
    {

    }

    public class SchemaAnalyzerResult 
    {
        public string Caption { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }

        public SchemaAnalyzerResult(string newCaption, int newCount, double newWeight)
        {
            Caption = newCaption;
            Count = newCount;
            Weight = newWeight;
        }
    }
    public class SchemaAnalyzerResultComparer:IComparer<SchemaAnalyzerResult>
    {
        public int Compare(SchemaAnalyzerResult x, SchemaAnalyzerResult y)
        {
            int returnValue = 1;
            if (x != null && y == null)
            {
                returnValue = 0;
            }
            else if (x == null && y != null)
            {
                returnValue = 0;
            }
            else if (x != null && y != null)
            {
                returnValue = x.Weight.CompareTo(y.Weight);
            }
            return returnValue;
        }
    }
}
