using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlPackage
    {
        int Id { get; }
        string Name { get; }
        string Stereotype { get; }

        IUmlPackage Parent { get; }

        IUmlTaggedValue GetTaggedValue(string name);

        IEnumerable<IUmlClass> Classes { get; }
        IUmlClass CreateClass(UmlClassSpec umlClassSpec);
        IUmlClass UpdateClass(IUmlClass umlClass, UmlClassSpec umlClassSpec);
        void RemoveClass(IUmlClass umlClass);

        IEnumerable<IUmlDataType> DataTypes { get; }
        IUmlDataType GetDataTypeById(int id);
        IUmlDataType CreateDataType(UmlDataTypeSpec spec);
        IUmlDataType UpdateDataType(IUmlDataType dataType, UmlDataTypeSpec spec);
        void RemoveDataType(IUmlDataType dataType);

        IEnumerable<IUmlEnumeration> Enumerations { get; }

        IUmlEnumeration CreateEnumeration(UmlEnumerationSpec umlEnumerationSpec);
        IUmlEnumeration UpdateEnumeration(IUmlEnumeration umlEnumeration, UmlEnumerationSpec umlEnumerationSpec);
        void RemoveEnumeration(IUmlEnumeration umlEnumeration);
    }
}