using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.import.mapForceMapping
{
    [TestFixture]
    public class MapForceMappingImporterTest
    {
        private const string CCCityNameKey = "81307160";
        private const string EbInterfaceTownKey = "91669944";

        [Test]
        public void TestImportSchemaComponents()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\MapForceMappingImporterTests\MapForceMappingImporter_ImportSchemaComponents_Test.mfd");

            MapForceMapping expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                                      {
                                                                          new SchemaComponent("CoreComponent_1.xsd", "{}Address", new Namespace[0],
                                                                                              new Entry("Address", InputOutputKey.None,
                                                                                                        new[]
                                                                                                            {
                                                                                                                new Entry("CityName", InputOutputKey.Input(null, CCCityNameKey)),
                                                                                                            })
                                                                              ),
                                                                          new SchemaComponent("Invoice.xsd", "{http://www.ebinterface.at/schema/3p0/}Address", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                                              new Entry("Address", InputOutputKey.None,
                                                                                                        new[]
                                                                                                            {
                                                                                                                new Entry("Town", InputOutputKey.Output(null, EbInterfaceTownKey)),
                                                                                                            })),
                                                                      },
                                                                  new List<ConstantComponent>(),
                                                                  new List<FunctionComponent>(),
                                                                  new Graph(new[]
                                                                                {
                                                                                    new Vertex(EbInterfaceTownKey, new[]
                                                                                                                       {
                                                                                                                           new Edge("72603008", CCCityNameKey, null),
                                                                                                                       }, null),
                                                                                }));

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

        [Test]
        public void TestImportSchemaComponentsWithMappingFunctionSplit()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\MapForceMappingImporterTests\mapping_simple_element_and_attributes_to_acc_with_mapping_function_split\mapping.mfd");

            const string ebInterfaceStreetKey = "72359800";
            const string ebInterfaceTownKey = "72359696";

            const string cclBuildingNumberKey = "72345456";
            const string cclCityNameKey = "72355472";
            const string cclStreetNameKey = "72345296";

            const string splitFunctionNameConstantKey = "69602728";

            const string splitFunctionNameKey = "72342768";
            const string splitFunctionInputKey = "72455912";
            const string splitFunctionOutputKey1 = "72357424";
            const string splitFunctionOutputKey2 = "72357888";

            MapForceMapping expectedMapping = new MapForceMapping(new List<SchemaComponent>
                                                                      {
                                                                          new SchemaComponent("source.xsd", "{http://www.ebinterface.at/schema/3p0/}Address", new[]{new Namespace("http://www.ebinterface.at/schema/3p0/"), },
                                                                                              new Entry("Address", InputOutputKey.None,
                                                                                                        new[]
                                                                                                            {
                                                                                                                new Entry("Street", InputOutputKey.Output(null, ebInterfaceStreetKey)),
                                                                                                                new Entry("Town", InputOutputKey.Output(null, ebInterfaceTownKey)),
                                                                                                            })
                                                                              ),
                                                                          new SchemaComponent("target.xsd", "{ccts.org}Address", new[] {new Namespace("ccts.org"),},
                                                                                              new Entry("Address", InputOutputKey.None,
                                                                                                        new[]
                                                                                                            {
                                                                                                                new Entry("BuildingNumber", InputOutputKey.Input(null, cclBuildingNumberKey)),
                                                                                                                new Entry("CityName", InputOutputKey.Input(null, cclCityNameKey)),
                                                                                                                new Entry("StreetName", InputOutputKey.Input(null, cclStreetNameKey)),
                                                                                                            })),
                                                                      },
                                                                  new List<ConstantComponent>
                                                                      {
                                                                          new ConstantComponent("split1", InputOutputKey.Output(null, splitFunctionNameConstantKey))
                                                                      },
                                                                  new List<FunctionComponent>
                                                                      {
                                                                          new FunctionComponent("split", InputOutputKey.Input(null, splitFunctionNameKey), new []{InputOutputKey.Input(null, splitFunctionInputKey)}, new []{InputOutputKey.Output(null, splitFunctionOutputKey1), InputOutputKey.Output(null, splitFunctionOutputKey2)}),
                                                                      },
                                                                  new Graph(new[]
                                                                                {
                                                                                    new Vertex(splitFunctionNameConstantKey, new[]
                                                                                                                                 {
                                                                                                                                     new Edge("72352528", splitFunctionNameKey, null),
                                                                                                                                 }, null),
                                                                                    new Vertex(splitFunctionOutputKey1, new[]
                                                                                                                            {
                                                                                                                                new Edge("72363680", cclStreetNameKey, null),
                                                                                                                            }, null),
                                                                                    new Vertex(splitFunctionOutputKey2, new[]
                                                                                                                            {
                                                                                                                                new Edge("72363864", cclBuildingNumberKey, null),
                                                                                                                            }, null),
                                                                                    new Vertex(ebInterfaceTownKey, new[]
                                                                                                                       {
                                                                                                                           new Edge("72363312", cclCityNameKey, null),
                                                                                                                       }, null),
                                                                                    new Vertex(ebInterfaceStreetKey, new[]
                                                                                                                         {
                                                                                                                             new Edge("72363496", splitFunctionInputKey, null),
                                                                                                                         }, null),
                                                                                }));

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