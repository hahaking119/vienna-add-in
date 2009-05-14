using System.Collections;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.Utils;
using VIENNAAddInUnitTests.TestRepository;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.Utils
{
    [TestFixture]
    public class EARepositoryCopierTest
    {
        private delegate void AssertAreEqual<T>(T expected, T actual, Path path);

        private void AssertCollectionsAreEqual<T>(Collection expectedElements, Collection actualElements, Path path, AssertAreEqual<T> assertAreEqual)
        {
            Assert.AreEqual(expectedElements.Count, actualElements.Count, "Different number of " + typeof(T).Name + "s in " + path);
            IEnumerator actualElementsEnumerator = actualElements.GetEnumerator();
            foreach (T expectedElement in expectedElements)
            {
                actualElementsEnumerator.MoveNext();
                var actualElement = (T)actualElementsEnumerator.Current;
                assertAreEqual(expectedElement, actualElement, path);
            }
        }

        private void AssertRepositoriesAreEqual(Repository expected, Repository actual, Path path)
        {
            AssertCollectionsAreEqual<Package>(expected.Models, actual.Models, path, AssertPackagesAreEqual);
        }

        private void AssertPackagesAreEqual(Package expectedPackage, Package actualPackage, Path path)
        {
            Assert.AreEqual(expectedPackage.Name, actualPackage.Name);
            Path packagePath = path/expectedPackage.Name;
            AssertElementsAreEqual(expectedPackage.Element, actualPackage.Element, packagePath);
            AssertCollectionsAreEqual<Package>(expectedPackage.Packages, actualPackage.Packages, packagePath, AssertPackagesAreEqual);
            AssertCollectionsAreEqual<Element>(expectedPackage.Elements, actualPackage.Elements, packagePath, AssertElementsAreEqual);
            AssertCollectionsAreEqual<Diagram>(expectedPackage.Diagrams, actualPackage.Diagrams, packagePath, AssertDiagramsAreEqual);
        }

        private void AssertDiagramsAreEqual(Diagram expectedDiagram, Diagram actualDiagram, Path path)
        {
            // TODO
        }

        private void AssertElementsAreEqual(Element expectedElement, Element actualElement, Path path)
        {
            if (expectedElement == null)
            {
                Assert.IsNull(actualElement);
            }
            else
            {
                Assert.IsNotNull(actualElement, "Target element for " + expectedElement.Name + " is null at " + path);
                Assert.AreEqual(expectedElement.Name, actualElement.Name, "Different element names in " + path);
                Path elementPath = path/expectedElement.Name;

                Assert.AreEqual(expectedElement.Stereotype, actualElement.Stereotype, "Different Stereotype in element " + path);
                Assert.AreEqual(expectedElement.StereotypeEx, actualElement.StereotypeEx, "Different StereotypeEx in element " + path);
                AssertCollectionsAreEqual<Attribute>(expectedElement.Attributes, actualElement.Attributes, elementPath, AssertAttributesAreEqual);
                AssertCollectionsAreEqual<Connector>(expectedElement.Connectors, actualElement.Connectors, elementPath, AssertConnectorsAreEqual);
                AssertCollectionsAreEqual<TaggedValue>(expectedElement.TaggedValues, actualElement.TaggedValues, elementPath, AssertTaggedValuesAreEqual);
            }
        }

        private void AssertTaggedValuesAreEqual(TaggedValue expected, TaggedValue actual, Path path)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.IsNotNull(actual, "Target tagged value for " + expected.Name + " is null at " + path);
                Assert.AreEqual(expected.Name, actual.Name, "Different tagged value names at " + path);
                Assert.AreEqual(expected.Value, actual.Value, "Different tagged value values at " + path/expected.Name);
            }
        }

        private void AssertConnectorsAreEqual(Connector expected, Connector actual, Path path)
        {
            // TODO
        }

        private void AssertAttributesAreEqual(Attribute expected, Attribute actual, Path path)
        {
            // TODO
        }

        [Test]
        public void TestCopyRepository()
        {
            var original = new EARepository1();
            var copy = new EARepository();
            RepositoryCopier.CopyRepository(original, copy);
            AssertRepositoriesAreEqual(original, copy, Path.EmptyPath);
        }
    }
}