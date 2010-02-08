using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.import.mapForceMapping
{
    [TestFixture]
    public class MapForceSourceElementTreeTests
    {
        [Test]
        public void TestCreateSourceElementTree()
        {
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\CreateSourceElementTree\source.xsd");
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\CreateSourceElementTree\mapping.mfd"));

            var expectedAddress = new SourceElement("Address", "2");
            var expectedTown = new SourceElement("Town", "3");
            expectedAddress.AddChild(expectedTown);

            AssertTreesAreEqual(expectedAddress, new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet).RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldWorkWithASingleInputSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      new List<FunctionComponent>(),
                                                      null);
            var expectedRoot = new SourceElement("Entry1", string.Empty);
            var expectedChild1 = new SourceElement("Entry2", "2");
            var expectedChild2 = new SourceElement("Entry3", "3");
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);

            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema1.xsd");
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldWorkWithTwoIndependentInputSchemaComponents()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      null);
            var expectedRoot = new SourceElement("Entry1", string.Empty);
            var expectedChild1 = new SourceElement("Entry2", "2");
            var expectedChild2 = new SourceElement("Entry3", "3");
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);


            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema1.xsd")), null));
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema2.xsd")), null));
            
            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldAttachConnectedSchemaComponentsToRootSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema3.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      null);
            var entry1 = new SourceElement("Entry1", string.Empty);
            var entry2 = new SourceElement("Entry2", "2");
            var entry3 = new SourceElement("Entry3", "3");
            var entry4 = new SourceElement("Entry4", "4");
            var entry5 = new SourceElement("Entry5", "5");
            entry1.AddChild(entry2);
            entry2.AddChild(entry3);
            entry1.AddChild(entry4);
            entry4.AddChild(entry5);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema3.xsd")), null));

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(entry1, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldAttachXsdSequenceInformationToSourceElements()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema3.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      null);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema3.xsd")), null));

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            Assert.That(sourceElementTree.RootSourceElement.XsdTypeName, Is.EqualTo("Entry1Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[0].XsdTypeName, Is.EqualTo("Entry2Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[0].Children[0].XsdTypeName, Is.EqualTo("String"));
            Assert.That(sourceElementTree.RootSourceElement.Children[1].XsdTypeName, Is.EqualTo("Entry4Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[1].Children[0].XsdTypeName, Is.EqualTo("String"));
        }

        [Test]
        public void ShouldAttachXsdChoiceInformationToSourceElements()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema4.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      null);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema4.xsd")), null));

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);

            Assert.That(sourceElementTree.RootSourceElement.XsdTypeName, Is.EqualTo("Entry1Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[0].XsdTypeName, Is.EqualTo("Entry2Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[0].Children[0].XsdTypeName, Is.EqualTo("String"));
            Assert.That(sourceElementTree.RootSourceElement.Children[1].XsdTypeName, Is.EqualTo("Entry4Type"));
            Assert.That(sourceElementTree.RootSourceElement.Children[1].Children[0].XsdTypeName, Is.EqualTo("String"));
        }

        [Test]
        [ExpectedException(ExceptionType = typeof (ArgumentException))]
        public void ShouldThrowExceptionIfNoRootIsDefinedForMultipleInputSchemaComponents()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      new List<FunctionComponent>(),
                                                      null);
            
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema1.xsd")), null));
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\ebInterface\mapForceSourceElementTreeTests\Schema2.xsd")), null));

            new MapForceSourceElementTree(mapForceMapping, xmlSchemaSet);
        }

        private static void AssertTreesAreEqual(SourceElement expected, SourceElement actual, string path)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Name mismatch at " + path);
            Assert.AreEqual(expected.Children.Count, actual.Children.Count, "Unequal number of children at " + path + "/" + expected.Name);
            for (int i = 0; i < expected.Children.Count; i++)
            {
                AssertTreesAreEqual(expected.Children[i], actual.Children[i], path + "/" + expected.Name);
            }
        }
    }
}