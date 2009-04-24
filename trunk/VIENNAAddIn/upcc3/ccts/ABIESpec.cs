// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ABIESpec : BIESpec
    {
        public ABIESpec(IABIE abie) : base(abie)
        {
            BBIEs = abie.BBIEs.Convert(bbie => new BBIESpec(bbie));
            ASBIEs = abie.ASBIEs.Convert(asbie => new ASBIESpec(asbie));
            IsEquivalentTo = abie.IsEquivalentTo;
            BasedOn = abie.BasedOn;
        }

        public ABIESpec()
        {
            bbies = new List<BBIESpec>();
            asbies = new List<ASBIESpec>();
        }

        private List<BBIESpec> bbies;
        public IEnumerable<BBIESpec> BBIEs
        {
            get { return bbies; }
            set { bbies = new List<BBIESpec>(value); }
        }

        private List<ASBIESpec> asbies;
        public IEnumerable<ASBIESpec> ASBIEs
        {
            get { return asbies; }
            set { asbies = new List<ASBIESpec>(value); }
        }

        public IABIE IsEquivalentTo { get; set; }
        public IACC BasedOn { get; set; }

        public void RemoveASBIE(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void RemoveBBIE(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }

    }
}