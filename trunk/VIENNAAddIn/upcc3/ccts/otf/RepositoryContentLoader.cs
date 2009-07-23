using System;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class RepositoryContentLoader
    {
        private readonly Repository eaRepository;

        public RepositoryContentLoader(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        public event Action<IEAPackage> PackageLoaded;
        public event Action<IEAElement> ElementLoaded;

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
            var eaElement = new OtherElement(element.ElementID, element.Name, element.PackageID);
            if (ElementLoaded != null)
            {
                ElementLoaded(eaElement);
            }
        }

        private void LoadPackage(Package package)
        {
            IEAPackage eaPackage;
            if (package.ParentID == 0)
            {
                eaPackage = new EAModel(package.PackageID, package.Name, package.ParentID);
            }
            else
            {
                switch (package.Element.Stereotype)
                {
                    case Stereotype.BInformationV:
                    {
                        eaPackage = new BInformationV(package.PackageID, package.Name, package.ParentID);
                        break;
                    }
                    case Stereotype.BLibrary:
                    {
                        eaPackage = new BLibrary(package.PackageID,
                                                 package.Name,
                                                 package.ParentID,
                                                 package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                 package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                 package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                 package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                 package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                 package.GetTaggedValues(TaggedValues.businessTerm),
                                                 package.GetTaggedValues(TaggedValues.copyright),
                                                 package.GetTaggedValues(TaggedValues.owner),
                                                 package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.PRIMLibrary:
                    {
                        eaPackage = new PRIMLibrary(package.PackageID,
                                                    package.Name,
                                                    package.ParentID,
                                                    package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                    package.GetTaggedValues(TaggedValues.businessTerm),
                                                    package.GetTaggedValues(TaggedValues.copyright),
                                                    package.GetTaggedValues(TaggedValues.owner),
                                                    package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.ENUMLibrary:
                    {
                        eaPackage = new ENUMLibrary(package.PackageID,
                                                    package.Name,
                                                    package.ParentID,
                                                    package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                    package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                    package.GetTaggedValues(TaggedValues.businessTerm),
                                                    package.GetTaggedValues(TaggedValues.copyright),
                                                    package.GetTaggedValues(TaggedValues.owner),
                                                    package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.CDTLibrary:
                    {
                        eaPackage = new CDTLibrary(package.PackageID,
                                                   package.Name,
                                                   package.ParentID,
                                                   package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                   package.GetTaggedValues(TaggedValues.businessTerm),
                                                   package.GetTaggedValues(TaggedValues.copyright),
                                                   package.GetTaggedValues(TaggedValues.owner),
                                                   package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.CCLibrary:
                    {
                        eaPackage = new CCLibrary(package.PackageID,
                                                  package.Name,
                                                  package.ParentID,
                                                  package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                  package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                  package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                  package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                  package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                  package.GetTaggedValues(TaggedValues.businessTerm),
                                                  package.GetTaggedValues(TaggedValues.copyright),
                                                  package.GetTaggedValues(TaggedValues.owner),
                                                  package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.BDTLibrary:
                    {
                        eaPackage = new BDTLibrary(package.PackageID,
                                                   package.Name,
                                                   package.ParentID,
                                                   package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                   package.GetTaggedValues(TaggedValues.businessTerm),
                                                   package.GetTaggedValues(TaggedValues.copyright),
                                                   package.GetTaggedValues(TaggedValues.owner),
                                                   package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.BIELibrary:
                    {
                        eaPackage = new BIELibrary(package.PackageID,
                                                   package.Name,
                                                   package.ParentID,
                                                   package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                   package.GetTaggedValues(TaggedValues.businessTerm),
                                                   package.GetTaggedValues(TaggedValues.copyright),
                                                   package.GetTaggedValues(TaggedValues.owner),
                                                   package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    case Stereotype.DOCLibrary:
                    {
                        eaPackage = new DOCLibrary(package.PackageID,
                                                   package.Name,
                                                   package.ParentID,
                                                   package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty),
                                                   package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty),
                                                   package.GetTaggedValues(TaggedValues.businessTerm),
                                                   package.GetTaggedValues(TaggedValues.copyright),
                                                   package.GetTaggedValues(TaggedValues.owner),
                                                   package.GetTaggedValues(TaggedValues.reference));
                        break;
                    }
                    default:
                    {
                        eaPackage = new OtherPackage(package.PackageID, package.Name, package.ParentID);
                        break;
                    }
                }
            }
            if (PackageLoaded != null)
            {
                PackageLoaded(eaPackage);
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
}