// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel
    {
        private string Name { get; set; }
        private string Prefix { get; set; }
        private readonly List<CandidateCcLibrary> candidateCcLibraries;

        public TemporaryAbieModel(string initName, string initPrefix)
        {
            Name = initName;
            Prefix = initPrefix;
            candidateCcLibraries = new List<CandidateCcLibrary>();
        }
    }
}