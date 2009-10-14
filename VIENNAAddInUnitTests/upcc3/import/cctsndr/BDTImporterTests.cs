using System.Xml;
using System.Xml.Schema;
using EA;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.cctsndr;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.import.cctsndr
{
    [TestFixture]
    public class BDTImporterTests
    {
        /// <summary>
        /// [R A7B8]
        /// The name of a BDT MUST be the:
        ///  - BDT ccts:DataTypeQualifierTerm(s) if any, plus.
        ///  - The ccts:DataTypeTerm, plus.
        ///  - The word Type, plus.
        ///  - The underscore character [_], plus.
        ///  - A six character unique identifier, unique within the given
        ///    namespace, consisting of lowercase alphabetic characters
        ///    [a-z], uppercase alphabetic characters [A-Z], and digit
        ///    characters [0-9].
        /// With the separators removed and approved abbreviations and
        /// acronyms applied.
        ///
        /// [R 8437]
        /// The six character unique identifier used for the BDT Type name
        /// MUST be unique within the namespace in which it is defined.
        /// </summary>
        [Test]
        public void ResolvesBDTTypeNamesAccordingToTheNDR()
        {
            // Text <= TextType_000001

            XmlSchema bdtSchema = XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"..\upcc3\import\cctsndr\BDTImporterTestResources\BusinessDataType_simple_type.xsd")), null);

            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("bLibrary", bLibrary =>
                                                                         {
                                                                             bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                                             bLibrary.AddPackage("PRIMLibrary", primLib =>
                                                                                                                {
                                                                                                                    primLib.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                                                    primLib.AddPRIM("String");
                                                                                                                });
                                                                             bLibrary.AddPackage("BDTLibrary", bdtLib => { bdtLib.Element.Stereotype = Stereotype.BDTLibrary; });
                                                                         }));
            var ccRepository = new CCRepository(eaRepository);
            IPRIMLibrary primLibrary = ccRepository.LibraryByName<IPRIMLibrary>("PRIMLibrary");
            IBDTLibrary bdtLibrary = ccRepository.LibraryByName<IBDTLibrary>("BDTLibrary");

            var contextMock = new Mock<IImporterContext>();
            contextMock.SetupGet(context => context.PRIMLibrary).Returns(primLibrary);
            contextMock.SetupGet(context => context.BDTLibrary).Returns(bdtLibrary);
            contextMock.SetupGet(context => context.BDTSchema).Returns(bdtSchema);

            new BDTSchemaImporter(contextMock.Object).ImportXSD();

            AssertThatBDTLibraryContainsBDT(bdtLibrary, "Text");
            AssertThatBDTLibraryContainsBDT(bdtLibrary, "Qualified_Text");
        }

        private static void AssertThatBDTLibraryContainsBDT(IBDTLibrary bdtLibrary, string bdtName)
        {
            IBDT bdtText = bdtLibrary.ElementByName(bdtName);
            Assert.That(bdtText, Is.Not.Null, string.Format("Expected BDT named '{0}' not generated.", bdtName));
        }
    }
}