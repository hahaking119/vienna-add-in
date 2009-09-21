using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class MapForceMappingImporterTest
    {
        private const string CCAddressKey = "78607696";
        private const string CCCityNameKey = "78577400";
        private const string EBInterfaceAddressKey = "81471920";
        private const string EBInterfaceTownKey = "80765288";

        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                  {
                                                      new SchemaComponent("Invoice.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                          new Entry("Invoice", InputOutputKey.None, new[]
                                                                                                                    {
                                                                                                                        new Entry("Address", InputOutputKey.Output(EBInterfaceAddressKey),
                                                                                                                                  new[]
                                                                                                                                  {
                                                                                                                                      new Entry("Town", InputOutputKey.Output(EBInterfaceTownKey)),
                                                                                                                                  })
                                                                                                                    })),
                                                      new SchemaComponent("CoreComponent_1.xsd", new Namespace[0],
                                                                          new Entry("Address", InputOutputKey.Input(CCAddressKey),
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("CityName", InputOutputKey.Input(CCCityNameKey)),
                                                                                    })
                                                          ),
                                                  },
                                                  new List<ConstantComponent>
                                                  {
                                                      new ConstantComponent("Root: Invoice"),
                                                  },
                                                  new Graph(new[]
                                                            {
                                                                new Vertex(EBInterfaceAddressKey, new[]
                                                                                        {
                                                                                            new Edge("84250472", CCAddressKey),
                                                                                        }),
                                                                new Vertex(EBInterfaceTownKey, new[]
                                                                                        {
                                                                                            new Edge("105919872", CCCityNameKey),
                                                                                        }),
                                                            }));
        }

        #endregion

        private MapForceMapping expectedMapping;

        [Test]
        public void TestImportSchemaComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\simple-mapping.mfd");

            MapForceMapping mapping = LinqToXmlMapForceMappingImporter.ImportFromFile(mappingFile);

            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Expected mapping: ");
            expectedMapping.PrettyPrint(Console.Out, "  ");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Imported mapping: ");
            mapping.PrettyPrint(Console.Out, "  ");
            Console.WriteLine("----------------------------------------------------");

            Assert.AreEqual(expectedMapping, mapping);
        }
    }

}