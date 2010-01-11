using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlDataTypeSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlAttributeSpec> Attributes { get; set; }
        public IEnumerable<UmlDependencySpec<IUmlDataType>> Dependencies { get; set; }
        public IEnumerable<UmlAssociationSpec> Associations { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
    }

    public class UmlClassSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlAttributeSpec> Attributes { get; set; }
        public IEnumerable<UmlDependencySpec<IUmlClass>> Dependencies { get; set; }
        public IEnumerable<UmlAssociationSpec> Associations { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
    }

    public class UmlEnumerationSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlEnumerationLiteralSpec> EnumerationLiterals { get; set; }
        public IEnumerable<UmlDependencySpec<IUmlEnumeration>> Dependencies { get; set; }
        public IEnumerable<UmlAssociationSpec> Associations { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
    }
}