using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.import.ebInterface;

namespace VIENNAAddInUnitTests.upcc3.import.ebInterface
{
    [TestFixture]
    public class MapForceSourceElementTreeTests
    {
        [Test]
        public void ShouldWorkWithASingleInputSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("input_schema_1.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry 3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      null);
            var expectedRoot = new SourceElement("Entry 1", string.Empty);
            var expectedChild1 = new SourceElement("Entry 2", "2");
            var expectedChild2 = new SourceElement("Entry 3", "3");
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldWorkWithTwoIndependentInputSchemaComponents()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("input_schema_1.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry 3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("input_schema_2.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry 1"),
                                                      },
                                                      null);
            var expectedRoot = new SourceElement("Entry 1", string.Empty);
            var expectedChild1 = new SourceElement("Entry 2", "2");
            var expectedChild2 = new SourceElement("Entry 3", "3");
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        public void ShouldAttachConnectedSchemaComponentsToRootSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("input_schema_1.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry 3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                            new Entry("Entry 4", InputOutputKey.None),
                                                                                        })),
                                                          new SchemaComponent("input_schema_2.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry 1"),
                                                      },
                                                      null);
            var entry1 = new SourceElement("Entry 1", string.Empty);
            var entry2 = new SourceElement("Entry 2", "2");
            var entry3 = new SourceElement("Entry 3", "3");
            var entry4 = new SourceElement("Entry 4", "4");
            var entry5 = new SourceElement("Entry 5", "5");
            entry1.AddChild(entry2);
            entry2.AddChild(entry3);
            entry1.AddChild(entry4);
            entry4.AddChild(entry5);

            var sourceElementTree = new MapForceSourceElementTree(mapForceMapping);

            AssertTreesAreEqual(entry1, sourceElementTree.RootSourceElement, string.Empty);
        }

        [Test]
        [ExpectedException(ExceptionType = typeof (ArgumentException))]
        public void ShouldThrowExceptionIfNoRootIsDefinedForMultipleInputSchemaComponents()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("input_schema_1.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 1", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 2", InputOutputKey.Output(null, "2"),
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry 3", InputOutputKey.Output(null, "3")),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("input_schema_2.xsd", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry 4", InputOutputKey.None,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry 5", InputOutputKey.Output(null, "5")),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      null);
            new MapForceSourceElementTree(mapForceMapping);
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