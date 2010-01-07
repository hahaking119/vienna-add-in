using System.Collections;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlEnumeration : IUmlClassifier
    {
        IUmlDependency<IUmlEnumeration> GetFirstDependencyByStereotype(string stereotype);

        IEnumerable<IUmlEnumerationLiteral> GetEnumerationLiteralsByStereotype(string stereotype);
        IUmlEnumerationLiteral CreateEnumerationLiteral(UmlEnumerationLiteralSpec spec);
        IUmlEnumerationLiteral UpdateEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral, UmlEnumerationLiteralSpec spec);
        void RemoveEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral);
    }
}