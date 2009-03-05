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
            Collection taggedValues = bdt.TaggedValues;
            taggedValues.AddTaggedValues(TaggedValues.BusinessTerm, spec.BusinessTerms);
            taggedValues.AddTaggedValues(TaggedValues.UsageRule, spec.UsageRules);
            taggedValues.AddTaggedValue(TaggedValues.Definition, spec.Definition);
            taggedValues.AddTaggedValue(TaggedValues.DictionaryEntryName, spec.DictionaryEntryName);
            taggedValues.AddTaggedValue(TaggedValues.LanguageCode, spec.LanguageCode);
            taggedValues.AddTaggedValue(TaggedValues.UniqueIdentifier, spec.UniqueIdentifier);
            taggedValues.AddTaggedValue(TaggedValues.VersionIdentifier, spec.VersionIdentifier);
            taggedValues.Refresh();
            Collection connectors = bdt.Connectors;
            connectors.AddConnector(util.Stereotype.BasedOn, spec.BasedOn.Id);
            connectors.Refresh();
            Collection attributes = bdt.Attributes;
            attributes.AddAttribute(util.Stereotype.CON, "Content", spec.CON.BasicType.Name, spec.CON.BasicType.Id,
                                    spec.CON.LowerBound, spec.CON.UpperBound, spec.CON.GetTaggedValues());
            foreach (SUPSpec sup in spec.SUPs)
            {
                attributes.AddAttribute(util.Stereotype.SUP, sup.Name, sup.BasicType.Name, sup.BasicType.Id,
                                        sup.LowerBound, sup.UpperBound, sup.GetTaggedValues());
            }
            attributes.Refresh();
            bdt.Update();
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