// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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

        public IEnumerable<IBIE> BIEs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new ABIE(repository, element);
                }
            }
        }

        public IABIE CreateABIE(ABIESpec spec)
        {
            var abie = (Element) package.Elements.AddNew(spec.Name, "Class");
            abie.Stereotype = util.Stereotype.ABIE;
            abie.PackageID = Id;

            foreach (TaggedValueSpec taggedValueSpec in spec.GetTaggedValues())
            {
                abie.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }

            if (spec.BasedOn != null)
            {
                abie.AddConnector(util.Stereotype.BasedOn, "basedOn", spec.BasedOn.Id);
            }
            if (spec.IsEquivalentTo != null)
            {
                abie.AddConnector(util.Stereotype.IsEquivalentTo, "isEquivalentTo", spec.IsEquivalentTo.Id);
            }
            if (spec.ASBIEs != null)
            {
                foreach (var asbie in spec.ASBIEs)
                {
                    abie.AddConnector(util.Stereotype.ASBIE, asbie.Name, asbie.AssociatedABIEId);
                }
            }
            abie.Connectors.Refresh();

            if (spec.BBIEs != null)
            {
                foreach (BBIESpec bbie in spec.BBIEs)
                {
                    abie.AddAttribute(util.Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, "1", "1",
                                      bbie.GetTaggedValues());
                }
            }
            abie.Attributes.Refresh();

            abie.Update();
            abie.Refresh();
            package.Elements.Refresh();
            return new ABIE(repository, abie);
        }

        public ICCTSElement ElementByName(string name)
        {
            return BIEs.First(e => e.Name == name);
        }

        #endregion
    }
}