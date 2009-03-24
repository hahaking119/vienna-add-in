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
    internal class BDTLibrary : BusinessLibrary, IBDTLibrary
    {
        public BDTLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        #region IBDTLibrary Members

        public IEnumerable<IBDT> BDTs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new BDT(repository, element);
                }
            }
        }

        public IBDT CreateBDT(BDTSpec spec)
        {
            var bdt = (Element) package.Elements.AddNew(spec.Name, "Class");
            bdt.Stereotype = util.Stereotype.BDT;
            bdt.PackageID = Id;

            bdt.SetTaggedValues(TaggedValues.BusinessTerm, spec.BusinessTerms);
            bdt.SetTaggedValues(TaggedValues.UsageRule, spec.UsageRules);
            bdt.SetTaggedValue(TaggedValues.Definition, spec.Definition);
            bdt.SetTaggedValue(TaggedValues.DictionaryEntryName, spec.DictionaryEntryName);
            bdt.SetTaggedValue(TaggedValues.LanguageCode, spec.LanguageCode);
            bdt.SetTaggedValue(TaggedValues.UniqueIdentifier, spec.UniqueIdentifier);
            bdt.SetTaggedValue(TaggedValues.VersionIdentifier, spec.VersionIdentifier);

            bdt.AddConnector(util.Stereotype.BasedOn, "basedOn", spec.BasedOn.Id);
            bdt.Connectors.Refresh();

            bdt.AddAttribute(util.Stereotype.CON, "Content", spec.CON.BasicType.Name, spec.CON.BasicType.Id,
                             spec.CON.LowerBound, spec.CON.UpperBound, spec.CON.GetTaggedValues());
            foreach (SUPSpec sup in spec.SUPs)
            {
                bdt.AddAttribute(util.Stereotype.SUP, sup.Name, sup.BasicType.Name, sup.BasicType.Id,
                                 sup.LowerBound, sup.UpperBound, sup.GetTaggedValues());
            }
            bdt.Attributes.Refresh();

            bdt.Update();
            bdt.Refresh();
            package.Elements.Refresh();
            return new BDT(repository, bdt);
        }

        public ICCTSElement ElementByName(string name)
        {
            return BDTs.First(e => e.Name == name);
        }

        #endregion
    }
}