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
            Collection taggedValues = abie.TaggedValues;
            foreach (var taggedValueSpec in spec.GetTaggedValues())
            {
                taggedValues.AddTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }
            taggedValues.Refresh();
            Collection connectors = abie.Connectors;
            if (spec.BasedOn != null)
            {
                connectors.AddConnector(util.Stereotype.BasedOn, spec.BasedOn.Id);
            }
            if (spec.IsEquivalentTo != null)
            {
                connectors.AddConnector(util.Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id);
            }
            connectors.Refresh();
            Collection attributes = abie.Attributes;
            foreach (var bbie in spec.BBIEs)
            {
                attributes.AddAttribute(util.Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, "1", "1", bbie.GetTaggedValues());
            }
            attributes.Refresh();
            abie.Update();
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