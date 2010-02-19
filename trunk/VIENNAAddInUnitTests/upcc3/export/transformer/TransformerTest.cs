using CctsRepository;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddIn.upcc3.export.transformer;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.export.transformer
{
    [TestFixture]
    public class TransformerTest
    {
        [Test]
        [Ignore("manual test case")]
        public void Test_transforming_ebinterface_to_ubl()
        {
            string ublInvoiceSchema = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\ubl_invoice\UBL-Invoice-2.0.xsd");
            string ebInterfaceAsUblSchemaDirectory = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\ebinterface_as_ubl\");
            string repositoryFile = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\invoice.eap");

            Repository eaRepository = new Repository();
            eaRepository.OpenFile(repositoryFile);
            
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary ublDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "UBL DOC Library");
            IDocLibrary ebiDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "ebInterface DOCLibrary");

            Transformer.Transform(ebiDocLibrary, ublDocLibrary);

            SubsetExporter.ExportSubset(ublDocLibrary, ublInvoiceSchema, ebInterfaceAsUblSchemaDirectory);
        }
    }
}