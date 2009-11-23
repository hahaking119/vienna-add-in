// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository
{
    public class ABIESpec : BIESpec
    {
        private List<ASBIESpec> asbies;
        private List<BBIESpec> bbies;

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

        public IEnumerable<BBIESpec> BBIEs
        {
            get { return bbies; }
            set { bbies = new List<BBIESpec>(value); }
        }

        public IEnumerable<ASBIESpec> ASBIEs
        {
            get { return asbies; }
            set { asbies = new List<ASBIESpec>(value); }
        }

        public IABIE IsEquivalentTo { get; set; }

        public IACC BasedOn { get; set; }

        public void AddASBIE(ASBIESpec spec)
        {
            asbies.Add(spec);
        }

        public void RemoveASBIE(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void AddBBIE(BBIESpec spec)
        {
            bbies.Add(spec);
        }

        public void RemoveBBIE(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }
    }
}