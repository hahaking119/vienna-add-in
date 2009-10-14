using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    [TestFixture]
    public class MapForceMappingImporterTest
    {
        private const string CCCityNameKey = "81307160";
        private const string EBInterfaceTownKey = "91669944";

        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                  {
                                                      new SchemaComponent("CoreComponent_1.xsd", new Namespace[0],
                                                                          new Entry("Address", InputOutputKey.None,
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("CityName", InputOutputKey.Input(null, CCCityNameKey)),
                                                                                    })
                                                          ),
                                                      new SchemaComponent("Invoice.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                          new Entry("Address", InputOutputKey.None,
                                                                                    new[]
                                                                                    {
                                                                                        new Entry("Town", InputOutputKey.Output(null, EBInterfaceTownKey)),
                                                                                    })),
                                                  },
                                                  new List<ConstantComponent>(),
                                                  new Graph(new[]
                                                            {
                                                                new Vertex(EBInterfaceTownKey, new[]
                                                                                               {
                                                                                                   new Edge("72603008", CCCityNameKey, null),
                                                                                               }, null),
                                                            }));
        }

        #endregion

        private MapForceMapping expectedMapping;

        [Test]
        public void TestImportSchemaComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\MapForceMappingImporter_ImportSchemaComponents_Test.mfd");

            MapForceMapping mapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFile);

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