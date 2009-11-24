using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using EAPackageExtensions=VIENNAAddIn.upcc3.ccts.util.EAPackageExtensions;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    internal class EARepositoryWithTwoPRIMs : EARepository
    {
        public EARepositoryWithTwoPRIMs()
        {
            this.AddModel("Model", m => m.AddPackage("bLibrary", bLibrary => bLibrary.AddPackage("PRIMLibrary", primLibrary =>
                                                                                                                {
                                                                                                                    primLibrary.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                                                    primLibrary.AddTaggedValue(TaggedValues.namespacePrefix.ToString()).WithValue("prim");
                                                                                                                    primLibrary.AddTaggedValue(TaggedValues.copyright.ToString()).WithValue("copyright1|copyright2");
                                                                                                                    Element primString = primLibrary.AddPRIM("String");
                                                                                                                    primString.AddTaggedValue(TaggedValues.dictionaryEntryName.ToString()).WithValue("String");
                                                                                                                    primString.AddTaggedValue(TaggedValues.businessTerm.ToString()).WithValue("a sequence of characters|a sequence of symbols");
                                                                                                                    primLibrary.AddPRIM("Zeichenfolge").With(prim => prim.AddIsEquivalentToDependency(primString));
                                                                                                                })));
        }
    }
}