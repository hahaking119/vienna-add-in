using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.XSDImporter.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ebInterface
{
    [TestFixture]
    public class MapForceMappingImporterTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                  {
                                                      new SchemaComponent("CoreComponent_1.xsd", new Namespace[0],
                                                                          new Entry("Address", InputOutputKey.Input("106374552"),
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("CityName", InputOutputKey.Input("109158960")),
                                                                                    })
                                                          ),
                                                      new SchemaComponent("Invoice.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                          new Entry("Invoice", InputOutputKey.None, new[]
                                                                                                                    {
                                                                                                                        new Entry("Address", InputOutputKey.Output("105780248"),
                                                                                                                                  new[]
                                                                                                                                  {
                                                                                                                                      new Entry("Town", InputOutputKey.Output("105906872")),
                                                                                                                                  })
                                                                                                                    })),
                                                  },
                                                  new List<ConstantComponent>
                                                  {
                                                      new ConstantComponent("Root: Invoice"),
                                                  },
                                                  new Graph(new[]
                                                            {
                                                                new Vertex("105780248", new[]
                                                                                        {
                                                                                            new Edge("84250472", "106374552"),
                                                                                        }),
                                                                new Vertex("105906872", new[]
                                                                                        {
                                                                                            new Edge("105919872", "109158960"),
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