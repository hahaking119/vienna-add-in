using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RepositoryContentLoader
    {
        private readonly Repository eaRepository;

        public RepositoryContentLoader(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        public event Action<IRepositoryItemData> ItemLoaded;

        public void LoadRepositoryContent()
        {
            foreach (Package model in eaRepository.Models)
            {
                LoadPackageRecursively(model);
            }
        }

        private void LoadPackageRecursively(Package package)
        {
            LoadPackage(package);
            foreach (Package subPackage in package.Packages)
            {
                LoadPackageRecursively(subPackage);
            }
            foreach (Element element in package.Elements)
            {
                LoadElement(element);
            }
        }

        private void LoadElement(Element element)
        {
            if (ItemLoaded != null)
            {
                ItemLoaded(new ElementData(element));
            }
        }

        private void LoadPackage(Package package)
        {
            if (ItemLoaded != null)
            {
                ItemLoaded(new PackageData(package));
            }
        }

        public void LoadPackageByID(int id)
        {
            LoadPackage(eaRepository.GetPackageByID(id));
        }

        public void LoadElementByID(int id)
        {
            LoadElement(eaRepository.GetElementByID(id));
        }

        private void LoadElementByGUID(string guid)
        {
            LoadElement(eaRepository.GetElementByGuid(guid));
        }

        private void LoadPackageByGUID(string guid)
        {
            LoadPackage(eaRepository.GetPackageByGuid(guid));
        }

        public void LoadItemByGUID(ObjectType objectType, string guid)
        {
            switch (objectType)
            {
                case ObjectType.otPackage:
                    LoadPackageByGUID(guid);
                    break;
                case ObjectType.otElement:
                    LoadElementByGUID(guid);
                    break;
            }
        }
    }

    internal class PackageData : IRepositoryItemData
    {
        private readonly Dictionary<string, string> taggedValues;

        public PackageData(Package package)
        {
            Id = ItemId.ForPackage(package.PackageID);
            ParentId = ItemId.ForPackage(package.ParentID);
            Name = package.Name;
            if (package.Element != null)
            {
                Stereotype = package.Element.Stereotype;
                taggedValues = new Dictionary<string, string>();
                foreach (TaggedValue taggedValue in package.Element.TaggedValues)
                {
                    taggedValues[taggedValue.Name] = taggedValue.Value;
                }
            }
        }

        #region IRepositoryItemData Members

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }
        public string Name { get; private set; }
        public string Stereotype { get; private set; }

        public string GetTaggedValue(TaggedValues key)
        {
            string value;
            taggedValues.TryGetValue(key.ToString(), out value);
            return value.DefaultTo(string.Empty);
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return new string[0];
        }

        #endregion
    }

    internal class ElementData : IRepositoryItemData
    {
        public ElementData(Element element)
        {
            Id = ItemId.ForElement(element.ElementID);
            ParentId = ItemId.ForPackage(element.PackageID);
            Name = element.Name;
            Stereotype = element.Stereotype;
        }

        #region IRepositoryItemData Members

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }
        public string Name { get; private set; }
        public string Stereotype { get; private set; }

        public string GetTaggedValue(TaggedValues key)
        {
            return null;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return new string[0];
        }

        #endregion
    }
}