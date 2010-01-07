using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlEnumeration : EaUmlClassifier, IUmlEnumeration
    {
        public EaUmlEnumeration(Repository eaRepository, Element eaElement) : base(eaRepository, eaElement)
        {
        }

        public IUmlDependency<IUmlEnumeration> GetFirstDependencyByStereotype(string stereotype)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUmlEnumerationLiteral> GetEnumerationLiteralsByStereotype(string stereotype)
        {
            throw new NotImplementedException();
        }

        public IUmlEnumerationLiteral CreateEnumerationLiteral(UmlEnumerationLiteralSpec spec)
        {
            throw new NotImplementedException();
        }

        public IUmlEnumerationLiteral UpdateEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral, UmlEnumerationLiteralSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral)
        {
            throw new NotImplementedException();
        }
    }
}