using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlDataType : EaUmlClassifier, IUmlDataType
    {
        public EaUmlDataType(Repository eaRepository, Element eaElement) : base(eaRepository, eaElement)
        {
        }

        #region IUmlDataType Members

        public IEnumerable<IUmlDependency<IUmlDataType>> GetDependenciesByStereotype(string stereotype)
        {
            foreach (Connector connector in eaElement.Connectors)
            {
                if (connector.Type == EAConnectorTypes.Dependency.ToString())
                {
                    if (connector.Stereotype == stereotype)
                    {
                        yield return new EaUmlDependency<IUmlDataType>(eaRepository, connector, targetElement => new EaUmlDataType(eaRepository, targetElement));
                    }
                }
            }
        }

        public IUmlDependency<IUmlDataType> GetFirstDependencyByStereotype(string stereotype)
        {
            var dependencies = new List<IUmlDependency<IUmlDataType>>(GetDependenciesByStereotype(stereotype));
            return dependencies.Count == 0 ? null : dependencies[0];
        }

        public IUmlDependency<IUmlDataType> CreateDependency(UmlDependencySpec<IUmlDataType> spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}