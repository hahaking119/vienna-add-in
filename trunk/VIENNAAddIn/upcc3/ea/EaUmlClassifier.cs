using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlClassifier : IUmlClass
    {
        protected readonly Element eaElement;
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

        public IUmlPackage Package
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaElement.PackageID)); }
        }

        public string Stereotype
        {
            get { return eaElement.Stereotype; }
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name));
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

        public IEnumerable<IUmlDependency<IUmlClass>> GetDependenciesByStereotype(string stereotype)
        {
            foreach (Connector connector in eaElement.Connectors)
            {
                if (connector.Type == EAConnectorTypes.Dependency.ToString())
                {
                    if (connector.Stereotype == stereotype)
                    {
                        yield return new EaUmlDependency<IUmlClass>(eaRepository, connector, targetElement => new EaUmlClassifier(eaRepository, targetElement));
                    }
                }
            }
        }

        public IUmlDependency<IUmlClass> GetFirstDependencyByStereotype(string stereotype)
        {
            var dependencies = new List<IUmlDependency<IUmlClass>>(GetDependenciesByStereotype(stereotype));
            return dependencies.Count == 0 ? null : dependencies[0];
        }

        public IUmlDependency<IUmlClass> CreateDependency(UmlDependencySpec<IUmlClass> spec)
        {
            var connector = (Connector) eaElement.Connectors.AddNew(string.Empty, EAConnectorTypes.Dependency.ToString());
            connector.Type = EAConnectorTypes.Dependency.ToString();
            connector.Stereotype = spec.Stereotype;
            connector.ClientID = Id;
            connector.SupplierID = spec.Target.Id;
            connector.SupplierEnd.Role = spec.Stereotype;
            connector.SupplierEnd.Cardinality = spec.LowerBound + ".." + spec.UpperBound;
            connector.Update();
            return new EaUmlDependency<IUmlClass>(eaRepository, connector, targetElement => new EaUmlClassifier(eaRepository, targetElement));
        }

        public IEnumerable<IUmlAttribute> GetAttributesByStereotype(string stereotype)
        {
            foreach (Attribute attribute in eaElement.Attributes)
            {
                if (attribute.Stereotype == stereotype)
                {
                    yield return new EaUmlAttribute(eaRepository, attribute);
                }
            }
        }

        public IUmlAttribute GetFirstAttributeByStereotype(string stereotype)
        {
            throw new NotImplementedException();
        }

        public IUmlAttribute CreateAttribute(UmlAttributeSpec spec)
        {
            throw new NotImplementedException();
        }

        public IUmlAttribute UpdateAttribute(IUmlAttribute attribute, UmlAttributeSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveAttribute(IUmlAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUmlAssociation> GetAssociationsByStereotype(string stereotype)
        {
            throw new NotImplementedException();
        }

        public IUmlAssociation CreateAssociation(UmlAssociationSpec spec)
        {
            throw new NotImplementedException();
        }

        public IUmlAssociation UpdateAssociation(IUmlAssociation association, UmlAssociationSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveAssociation(IUmlAssociation association)
        {
            throw new NotImplementedException();
        }
    }
}