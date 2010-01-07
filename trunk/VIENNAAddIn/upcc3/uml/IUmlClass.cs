using System.Collections;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClass : IUmlClassifier
    {
        IEnumerable<IUmlDependency<IUmlClass>> GetDependenciesByStereotype(string stereotype);
        IUmlDependency<IUmlClass> GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency<IUmlClass> CreateDependency(UmlDependencySpec<IUmlClass> spec);

        IEnumerable<IUmlAttribute> GetAttributesByStereotype(string stereotype);
        IUmlAttribute GetFirstAttributeByStereotype(string stereotype);
        IUmlAttribute CreateAttribute(UmlAttributeSpec spec);
        IUmlAttribute UpdateAttribute(IUmlAttribute attribute, UmlAttributeSpec spec);
        void RemoveAttribute(IUmlAttribute attribute);

        IEnumerable<IUmlAssociation> GetAssociationsByStereotype(string stereotype);
        IUmlAssociation CreateAssociation(UmlAssociationSpec spec);
        IUmlAssociation UpdateAssociation(IUmlAssociation association, UmlAssociationSpec spec);
        void RemoveAssociation(IUmlAssociation association);
    }
}