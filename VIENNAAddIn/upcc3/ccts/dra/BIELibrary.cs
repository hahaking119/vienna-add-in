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
            taggedValues.AddTaggedValue(TaggedValues.DictionaryEntryName, spec.DictionaryEntryName);
            taggedValues.AddTaggedValue(TaggedValues.Definition, spec.Definition);
            taggedValues.AddTaggedValue(TaggedValues.UniqueIdentifier, spec.UniqueIdentifier);
            taggedValues.AddTaggedValue(TaggedValues.VersionIdentifier, spec.VersionIdentifier);
            taggedValues.AddTaggedValue(TaggedValues.LanguageCode, spec.LanguageCode);
            taggedValues.AddTaggedValues(TaggedValues.BusinessTerm, spec.BusinessTerms);
            taggedValues.AddTaggedValues(TaggedValues.UsageRule, spec.UsageRules);
            taggedValues.Refresh();
            Collection connectors = abie.Connectors;
            connectors.AddConnector(util.Stereotype.BasedOn, spec.BasedOn.Id);
            connectors.AddConnector(util.Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id);
            connectors.Refresh();
            Collection attributes = abie.Attributes;
            foreach (var bbie in spec.BBIEs)
            {
//                var bbieTaggedValues = new List<List<string>>
//                                                      {
//                                                          
//                                                      }
//                attributes.AddAttribute(util.Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, bbie.LowerBound,
//                                        bbie.UpperBound);
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