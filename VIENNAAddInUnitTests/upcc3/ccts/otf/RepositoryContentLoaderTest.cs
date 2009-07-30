using System;
using System.Collections.Generic;
using System.Text;
using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class RepositoryContentLoaderTest
    {
        private  RepositoryContentLoader contentLoader;
        private List<Package> packages;
        private List<Element> elements;

        [SetUp]
        public void Context()
        {
            packages = new List<Package>();
            elements = new List<Element>();

            var eaRepository = new EARepository();

            var model = eaRepository.AddModel("Model", m => { });
            packages.Add(model);

            var otherPackage = CreatePackage(model, "other package");

            var bInformationV = CreatePackage(model, Stereotype.BInformationV);

            var bLibrary = CreatePackage(bInformationV, Stereotype.BLibrary);

            CreatePackage(bLibrary, Stereotype.PRIMLibrary);
            CreatePackage(bLibrary, Stereotype.ENUMLibrary);
            CreatePackage(bLibrary, Stereotype.CDTLibrary);
            CreatePackage(bLibrary, Stereotype.CCLibrary);
            CreatePackage(bLibrary, Stereotype.BDTLibrary);
            CreatePackage(bLibrary, Stereotype.BIELibrary);
            CreatePackage(bLibrary, Stereotype.DOCLibrary);

            CreateElement(otherPackage, "other element");

            contentLoader = new RepositoryContentLoader(eaRepository);
        }

        private Package CreatePackage(Package parent, string stereotype)
        {
            Package package = parent.AddPackage(stereotype, p => p.Element.Stereotype = stereotype);
            packages.Add(package);
            return package;
        }

        private void CreateElement(Package parent, string stereotype)
        {
            elements.Add(parent.AddClass(stereotype).With(e => e.Stereotype = stereotype));
        }

        [Test]
        public void TestLoadAll()
        {
            contentLoader.ItemLoaded += RemoveItem;
            contentLoader.LoadRepositoryContent();
            Assert.AreEqual(0, packages.Count, "some packages where not loaded: " + ListToString(packages, package => (package.Element != null ? package.Element.Stereotype : "Model")));
            Assert.AreEqual(0, elements.Count, "some elements where not loaded: " + ListToString(elements, element => element.Stereotype));
        }

        private void RemoveItem(IRepositoryItemData item)
        {
            int numberOfItemsRemoved;
            if (item.Id.Type == ItemId.ItemType.Package)
            {
                numberOfItemsRemoved = packages.RemoveAll(package => package.PackageID == item.Id.Value);
            }
            else
            {
                numberOfItemsRemoved = elements.RemoveAll(element => element.ElementID == item.Id.Value);
            }
            Assert.AreEqual(1, numberOfItemsRemoved, "unexpected item loaded: " + item.Name);
        }

        private static string ListToString<TElement>(List<TElement> list, Func<TElement, string> toString)
        {
            var buf = new StringBuilder();
            buf.Append("{");
            foreach (var item in list)
            {
                buf.Append(toString(item)).Append(",");
            }
            buf.Append("}");
            return buf.ToString();
        }
    }
}