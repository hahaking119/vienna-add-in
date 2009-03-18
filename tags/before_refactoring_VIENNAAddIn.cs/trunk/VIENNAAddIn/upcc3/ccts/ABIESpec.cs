// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ABIESpec : BIESpec
    {
        public IEnumerable<BBIESpec> BBIEs { get; set; }
        public IABIE IsEquivalentTo { get; set;  }
        public IACC BasedOn { get; set; }
    }
}