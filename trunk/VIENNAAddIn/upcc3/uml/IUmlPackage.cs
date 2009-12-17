using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlPackage
    {
        int Id { get; }
        string Name { get; }
        string Stereotype { get; }

        IUmlPackage Parent { get; }

        IEnumerable<IUmlClass> Classes { get; }

        IEnumerable<IUmlDataType> DataTypes { get; }
        IUmlDataType GetDataTypeById(int id);
        IUmlDataType CreateDataType(UmlDataTypeSpec spec);
        IUmlDataType UpdateDataType(IUmlDataType dataType, UmlDataTypeSpec spec);
        void RemoveDataType(IUmlDataType dataType);

        IEnumerable<IUmlEnumeration> Enumerations { get; }

        IUmlTaggedValue GetTaggedValue(TaggedValues name);
    }
}