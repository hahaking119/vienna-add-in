// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace UPCCRepositoryInterface
{
    public class CDTSpec : DTSpec
    {
        public CDTSpec(ICDT cdt) : base(cdt)
        {
        }

        public CDTSpec()
        {
        }

        [Dependency]
        public ICDT IsEquivalentTo { get; set; }
    }
}