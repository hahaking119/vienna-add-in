using System.Linq;
using System.Xml;
using System.Xml.Schema;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.import.cctsndr;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.import.cctsndr
{
    [TestFixture]
    public class BDTImporterTests
    {
        private const string SimpleTypeSchema = "BusinessDataType_simple_type.xsd";
        private const string ComplexTypeSchema = "BusinessDataType_complex_type.xsd";

        private static IImporterContext CreateContext(string schemaFile)
        {
            XmlSchema bdtSchema = XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"..\upcc3\import\cctsndr\BDTImporterTestResources\" + schemaFile)), null);

            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("bLibrary", bLibrary =>
                                                                         {
                                                                             bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                                             bLibrary.AddPackage("PRIMLibrary", primLib =>
                                                                                                                {
                                                                                                                    primLib.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                                                    primLib.AddPRIM("String");
                                                                                                                    primLib.AddPRIM("Date");
                                                                                                                });
                                                                             bLibrary.AddPackage("CDTLibrary", cdtLib =>
                                                                                                               {
                                                                                                                   cdtLib.Element.Stereotype = Stereotype.CDTLibrary;
                                                                                                                   cdtLib.AddCDT("Text");
                                                                                                               });
                                                                             bLibrary.AddPackage("BDTLibrary", bdtLib => { bdtLib.Element.Stereotype = Stereotype.BDTLibrary; });
                                                                         }));
            var ccRepository = new CCRepository(eaRepository);
            var primLibrary = ccRepository.LibraryByName<IPRIMLibrary>("PRIMLibrary");
            var bdtLibrary = ccRepository.LibraryByName<IBDTLibrary>("BDTLibrary");
            var cdtLibrary = ccRepository.LibraryByName<ICDTLibrary>("CDTLibrary");

            var contextMock = new Mock<IImporterContext>();
            contextMock.SetupGet(c => c.PRIMLibrary).Returns(primLibrary);
            contextMock.SetupGet(c => c.CDTLibrary).Returns(cdtLibrary);
            contextMock.SetupGet(c => c.BDTLibrary).Returns(bdtLibrary);
            contextMock.SetupGet(c => c.BDTSchema).Returns(bdtSchema);

            return contextMock.Object;
        }

        private static IBDT AssertThatBDTLibraryContainsBDT(IBDTLibrary bdtLibrary, string bdtName)
        {
            IBDT bdtText = bdtLibrary.ElementByName(bdtName);
            Assert.That(bdtText, Is.Not.Null, string.Format("Expected BDT named '{0}' not generated.", bdtName));
            return bdtText;
        }

        /// <summary>
        /// [R 9908]
        /// Every BDT devoid of ccts:supplementaryComponents, or whose
        /// ccts:supplementaryComponents BVD facets map directly to the
        /// facets of an XML Schema built-in data type, MUST be defined as a
        /// named xsd:simpleType.
        /// 
        /// Deviation: We currently always use complex types if the BDT has any SUPs,
        /// because we do not really understand the specification in regard to this point.
        /// </summary>
        [Test]
        public void CreatesBDTsWithoutSUPsForSimpleTypes()
        {
            IImporterContext context = CreateContext(SimpleTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");
            Assert.That(bdtText.SUPs, Is.Empty);
        }

        /// <summary>
        /// [R B91F]
        /// The xsd:simpleType definition of a BDT whose content
        /// component BVD is defined by a primitive whose facets map
        /// directly to the facets of an XML Schema built-in datatype MUST
        /// contain an xsd:restriction element with the xsd:base
        /// attribute set to the XML Schema built-in data type that represents
        /// the primitive.
        /// </summary>
        [Test]
        public void SetsTheCONsBasicTypeToTheBuiltinDatatypesOfSimpleTypes()
        {
            IImporterContext context = CreateContext(SimpleTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");

            var primString = context.PRIMLibrary.ElementByName("String");
            Assert.That(bdtText.CON, Is.Not.Null, "CON is null");
            Assert.That(bdtText.CON.BasicType, Is.Not.Null, "BasicType is null");
            Assert.That(bdtText.CON.BasicType.Id, Is.EqualTo(primString.Id), string.Format("Wrong basic type for CON:\nExpected: <{0}>\nBut was: <{1}>", primString.Name, bdtText.CON.BasicType.Name));
        }

        [Test]
        public void SetsBasedOnDependencyForImportedBDTs()
        {
            IImporterContext context = CreateContext(SimpleTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");

            var cdtText = context.CDTLibrary.ElementByName("Text");
            Assert.That(bdtText.BasedOn, Is.Not.Null, "BasedOn is null");
            Assert.That(bdtText.BasedOn.CDT, Is.Not.Null, "BasedOn.CDT is null");
            Assert.That(bdtText.BasedOn.CDT.Id, Is.EqualTo(cdtText.Id), string.Format("Based on wrong CDT:\nExpected: <{0}>\nBut was:  <{1}>", cdtText.Name, bdtText.BasedOn.CDT.Name));

            var bdtQualifiedText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Qualified_Text");

            Assert.That(bdtQualifiedText.BasedOn, Is.Not.Null, "BasedOn is null");
            Assert.That(bdtQualifiedText.BasedOn.CDT, Is.Not.Null, "BasedOn.CDT is null");
            Assert.That(bdtQualifiedText.BasedOn.CDT.Id, Is.EqualTo(cdtText.Id), string.Format("Based on wrong CDT:\nExpected: <{0}>\nBut was:  <{1}>", cdtText.Name, bdtQualifiedText.BasedOn.CDT.Name));
        }

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
            IImporterContext context = CreateContext(SimpleTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");
            AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Qualified_Text");
            AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Qualifier1_Qualifier2_Text");
        }

        /// <summary>
        /// [R AB05]
        /// Every BDT that includes one or more Supplementary Components
        /// that do not map directly to the facets of an XSD built-in datatype
        /// MUST be defined as an xsd:complexType.
        /// 
        /// [R 890A]
        /// Every BDT xsd:complexType definition MUST include an
        /// xsd:attribute declaration for each Supplementary Component.
        /// 
        /// [R ABC1]
        /// The name of the Supplementary Component xsd:attribute
        /// must be the the Supplementary Component Property Term Name
        /// and Representation Term Name with periods, spaces, and other
        /// separators removed.
        /// </summary>
        [Test]
        public void CreatesSUPsForXsdAttributesInComplexTypes()
        {
            IImporterContext context = CreateContext(ComplexTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");
            Assert.That(bdtText.SUPs, Is.Not.Null);
            Assert.That(bdtText.SUPs.Count(), Is.EqualTo(1));
            var sup = bdtText.SUPs.ElementAt(0);
            var primString = context.PRIMLibrary.ElementByName("String");
            Assert.That(sup.Name, Is.EqualTo("propertyTermName"));
            Assert.That(sup.BasicType.Id, Is.EqualTo(primString.Id));
        }
        /// <summary>
        /// [R BBCB]
        /// The xsd:complexType definition of a BDT whose Content
        /// Component BVD is defined by a primitive whose facets do not map
        /// directly to the facets of an XML Schema built-in datatype MUST
        /// contain an xsd:simpleContent element that contains an
        /// xsd:extension whose base attribute is set to the XML Schema
        /// built-in data type that represents the primitive.
        /// </summary>
        [Test]
        public void SetsTheCONsBasicTypeToTheBuiltinDatatypesOfTheSimpleContentExtensionOfComplexTypes()
        {
            IImporterContext context = CreateContext(ComplexTypeSchema);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context.BDTLibrary, "Text");

            var primString = context.PRIMLibrary.ElementByName("String");
            Assert.That(bdtText.CON, Is.Not.Null, "CON is null");
            Assert.That(bdtText.CON.BasicType, Is.Not.Null, "BasicType is null");
            Assert.That(bdtText.CON.BasicType.Id, Is.EqualTo(primString.Id), string.Format("Wrong basic type for CON:\nExpected: <{0}>\nBut was: <{1}>", primString.Name, bdtText.CON.BasicType.Name));
        }

    }
}