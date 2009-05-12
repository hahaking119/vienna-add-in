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
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
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

        [Dependency]
        public IABIE IsEquivalentTo { get; set; }

        [Dependency]
        public IACC BasedOn { get; set; }

        public void RemoveASBIE(string name)
        {
            asbies.RemoveAll(asbie => asbie.Name == name);
        }

        public void RemoveBBIE(string name)
        {
            bbies.RemoveAll(bbie => bbie.Name == name);
        }

        public override IEnumerable<ConnectorSpec> GetCustomConnectors()
        {
            if (ASBIEs != null)
            {
                foreach (ASBIESpec asbie in ASBIEs)
                {
                    yield return
                        ConnectorSpec.CreateAggregation(AggregationKind.Composite, Stereotype.ASBIE, asbie.Name,
                                                        asbie.AssociatedABIEId, asbie.LowerBound, asbie.UpperBound);
                }
            }
        }

        public override IEnumerable<AttributeSpec> GetAttributes()
        {
            if (BBIEs != null)
            {
                foreach (BBIESpec bbie in BBIEs)
                {
                    yield return new AttributeSpec(Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, bbie.LowerBound, bbie.UpperBound, bbie.GetTaggedValues());
                }
            }

        }
    }
}