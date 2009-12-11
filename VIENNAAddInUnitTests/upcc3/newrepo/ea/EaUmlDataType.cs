using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlDataType : IUmlDataType
    {
        private readonly Repository eaRepository;
        private readonly Element eaElement;

        public EaUmlDataType(Repository eaRepository, Element eaElement)
        {
            this.eaRepository = eaRepository;
            this.eaElement = eaElement;
        }

        #region IUmlClass Members

        public int Id
        {
            get { return eaElement.ElementID; }
        }

        public string GUID
        {
            get { return eaElement.ElementGUID; }
        }

        public string Name
        {
            get { return eaElement.Name; }
        }

        public IUmlPackage Package
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaElement.PackageID)); }
        }

        public IEnumerable<IUmlDependency<IUmlClassifier>> GetDependenciesByStereotype(string stereotype)
        {
            foreach (Connector connector in eaElement.Connectors)
            {
                if (connector.Type == EAConnectorTypes.Dependency.ToString())
                {
                    if (connector.Stereotype == stereotype)
                    {
                        yield return new EaUmlDependency<IUmlClassifier>(eaRepository, connector, CreateUmlClass);
                    }
                }
            }
        }

        private static EaUmlDataType CreateUmlClass(Repository repository, Element element)
        {
            return new EaUmlDataType(repository, element);
        }

        public IUmlTaggedValue GetTaggedValue(TaggedValues name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name.ToString()));
        }

        #endregion

        private TaggedValue GetEATaggedValueByName(string name)
        {
            foreach (TaggedValue eaTaggedValue in eaElement.TaggedValues)
            {
                if (eaTaggedValue.Name.Equals(name))
                {
                    return eaTaggedValue;
                }
            }
            return null;
        }
    }
}