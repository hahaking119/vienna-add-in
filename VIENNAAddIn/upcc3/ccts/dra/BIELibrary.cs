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
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BIELibrary : BusinessLibrary, IBIELibrary
    {
        public BIELibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBIELibrary Members

        public IEnumerable<IABIE> BIEs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    if (element.IsABIE())
                    {
                        yield return new ABIE(repository, element);
                    }
                }
            }
        }

        public IABIE CreateABIE(ABIESpec spec)
        {
            var element = (Element) package.Elements.AddNew(spec.Name, "Class");
            element.Stereotype = util.Stereotype.ABIE;
            element.PackageID = Id;
//            foreach (TaggedValueSpec taggedValueSpec in spec.GetTaggedValues())
//            {
//                abie.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
//            }
//
//            if (spec.BasedOn != null)
//            {
//                abie.AddDependency(util.Stereotype.BasedOn, spec.BasedOn.Id, "1", "1");
//            }
//            if (spec.IsEquivalentTo != null)
//            {
//                abie.AddDependency(util.Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
//            }
//            if (spec.ASBIEs != null)
//            {
//                foreach (var asbie in spec.ASBIEs)
//                {
//                    abie.AddAggregation(AggregationKind.Composite, util.Stereotype.ASBIE, asbie.Name, asbie.AssociatedABIEId, asbie.LowerBound, asbie.UpperBound);
//                }
//            }
//            abie.Connectors.Refresh();
//
//            if (spec.BBIEs != null)
//            {
//                foreach (BBIESpec bbie in spec.BBIEs)
//                {
//                    abie.AddAttribute(util.Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, bbie.LowerBound, bbie.UpperBound,
//                                      bbie.GetTaggedValues());
//                }
//            }
//            abie.Attributes.Refresh();
//            abie.Update();
//            abie.Refresh();
            package.Elements.Refresh();
            AddElementToDiagram(element);
            var abie = new ABIE(repository, element);
            abie.Update(spec);
            return abie;
        }

        public IABIE UpdateABIE(IABIE abie, ABIESpec spec)
        {
            return repository.Update(abie, spec);
        }

        public ICCTSElement ElementByName(string name)
        {
            return BIEs.First(e => e.Name == name);
        }

        #endregion
    }
}