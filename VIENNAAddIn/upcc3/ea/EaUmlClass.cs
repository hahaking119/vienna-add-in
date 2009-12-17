using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlClass : EaUmlClassifier, IUmlClass
    {
        public EaUmlClass(Repository repository, Element element)
            : base(repository, element)
        {
        }

        #region IUmlClass Members

        public IEnumerable<IUmlDependency<IUmlClass>> GetDependenciesByStereotype(string stereotype)
        {
            foreach (Connector connector in eaElement.Connectors)
            {
                if (connector.Type == EAConnectorTypes.Dependency.ToString())
                {
                    if (connector.Stereotype == stereotype)
                    {
                        yield return new EaUmlDependency<IUmlClass>(eaRepository, connector, targetElement => new EaUmlClass(eaRepository, targetElement));
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
            throw new NotImplementedException();
        }

        #endregion
    }
}