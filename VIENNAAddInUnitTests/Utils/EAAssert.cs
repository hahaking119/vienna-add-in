using System;
using System.Collections;
using Castle.DynamicProxy;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.Utils
{
    public static class EAAssert
    {
        #region Delegates

        public delegate void AssertAreEqual<T>(T expected, T actual, Path path);

        #endregion

        public static void AssertCollectionsAreEqual<T>(Collection expectedElements, Collection actualElements,
                                                        Path path, AssertAreEqual<T> assertAreEqual)
        {
            Assert.AreEqual(expectedElements.Count, actualElements.Count,
                            "Different number of " + typeof (T).Name + "s in " + path);
            IEnumerator actualElementsEnumerator = actualElements.GetEnumerator();
            foreach (T expectedElement in expectedElements)
            {
                actualElementsEnumerator.MoveNext();
                var actualElement = (T) actualElementsEnumerator.Current;
                assertAreEqual(expectedElement, actualElement, path);
            }
        }

        public static void RepositoriesAreEqual(Repository expected, Repository actual, Path path)
        {
            AssertCollectionsAreEqual<Package>(expected.Models, actual.Models, path, PackagesAreEqual);
        }

        public static void PackagesAreEqual(Package expectedPackage, Package actualPackage, Path path)
        {
            Assert.AreEqual(expectedPackage.Name, actualPackage.Name);
            Path packagePath = path/expectedPackage.Name;
            ElementsAreEqual(expectedPackage.Element, actualPackage.Element, packagePath);
            AssertCollectionsAreEqual<Package>(expectedPackage.Packages, actualPackage.Packages, packagePath,
                                               PackagesAreEqual);
            AssertCollectionsAreEqual<Element>(expectedPackage.Elements, actualPackage.Elements, packagePath,
                                               ElementsAreEqual);
            AssertCollectionsAreEqual<Diagram>(expectedPackage.Diagrams, actualPackage.Diagrams, packagePath,
                                               DiagramsAreEqual);
        }

        public static void DiagramsAreEqual(Diagram expectedDiagram, Diagram actualDiagram, Path path)
        {
            // TODO
        }

        public static void ElementsAreEqual(Element expectedElement, Element actualElement, Path path)
        {
            if (expectedElement == null)
            {
                Assert.IsNull(actualElement);
            }
            else
            {
                Assert.IsNotNull(actualElement, "Target element for " + expectedElement.Name + " is null at " + path);
                Path elementPath = path/expectedElement.Name;
                AssertPropertiesAreEqual(expectedElement, actualElement, elementPath, o => new[]
                                                                                           {
                                                                                               o.Name,
                                                                                               o.Stereotype,
                                                                                               o.StereotypeEx,
                                                                                           });
                AssertCollectionsAreEqual<Attribute>(expectedElement.Attributes, actualElement.Attributes, elementPath,
                                                     AttributesAreEqual);
                AssertCollectionsAreEqual<Connector>(expectedElement.Connectors, actualElement.Connectors, elementPath,
                                                     ConnectorsAreEqual);
                AssertCollectionsAreEqual<TaggedValue>(expectedElement.TaggedValues, actualElement.TaggedValues,
                                                       elementPath, TaggedValuesAreEqual);
            }
        }

        public static void TaggedValuesAreEqual(TaggedValue expectedTag, TaggedValue actualTag, Path path)
        {
            if (expectedTag == null)
            {
                Assert.IsNull(actualTag);
            }
            else
            {
                Assert.IsNotNull(actualTag, "Target tagged value for " + expectedTag.Name + " is null at " + path);
                AssertPropertiesAreEqual(expectedTag, actualTag, path / expectedTag.Name, o => new[]
                                                                                               {
                                                                                                   o.Name,
                                                                                                   o.Value,
                                                                                               });
            }
        }

        public static void AttributeTagsAreEqual(AttributeTag expectedTag, AttributeTag actualTag, Path path)
        {
            if (expectedTag == null)
            {
                Assert.IsNull(actualTag);
            }
            else
            {
                Assert.IsNotNull(actualTag, "Target attribute tag for " + expectedTag.Name + " is null at " + path);
                AssertPropertiesAreEqual(expectedTag, actualTag, path/expectedTag.Name, o => new[]
                                                                                             {
                                                                                                 o.Name,
                                                                                                 o.Value,
                                                                                             });
            }
        }

        private static void AssertPropertiesAreEqual<T>(T expected, T actual, Path path,
                                                        Func<T, object> invokeProperties)
        {
            invokeProperties((T)new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(typeof(T), expected, new AssertPropertiesAreEqualInterceptor<T>(expected, actual, path)));
        }

        public static void ConnectorsAreEqual(Connector expected, Connector actual, Path path)
        {
            // TODO
        }

        public static void AttributesAreEqual(Attribute expectedAttribute, Attribute actualAttribute, Path path)
        {
            if (expectedAttribute == null)
            {
                Assert.IsNull(actualAttribute);
            }
            else
            {
                Assert.IsNotNull(actualAttribute,
                                 "Target attribute for " + expectedAttribute.Name + " is null at " + path);
                Path attributePath = path/expectedAttribute.Name;
                AssertPropertiesAreEqual(expectedAttribute, actualAttribute, attributePath,o => new[]
                                                                                                {
                                                                                                    o.Name,
                                                                                                    o.Default,
                                                                                                    o.LowerBound,
                                                                                                    o.UpperBound,
                                                                                                });
                AssertCollectionsAreEqual<AttributeTag>(expectedAttribute.TaggedValues, actualAttribute.TaggedValues,
                                                        attributePath, AttributeTagsAreEqual);
            }
        }
    }
}