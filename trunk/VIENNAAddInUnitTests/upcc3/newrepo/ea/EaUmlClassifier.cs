using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal abstract class EaUmlClassifier : IUmlClassifier
    {
        private readonly Element eaElement;
        protected readonly Repository eaRepository;

        public EaUmlClassifier(Repository eaRepository, Element eaElement)
        {
            this.eaRepository = eaRepository;
            this.eaElement = eaElement;
        }

        #region IUmlClassifier Members

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

        public abstract UmlClassifierType Type { get; }

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
                        yield return new EaUmlDependency<IUmlClassifier>(eaRepository, connector, CreateUmlClassifier);
                    }
                }
            }
        }

        protected abstract IUmlClassifier CreateUmlClassifier(Element eaElement);

        public IUmlTaggedValue GetTaggedValue(TaggedValues name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name.ToString()));
        }

        public IUmlDependency<TTarget> CreateDependency<TTarget>(UmlDependencySpec<TTarget> spec)
        {
            throw new NotImplementedException();
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