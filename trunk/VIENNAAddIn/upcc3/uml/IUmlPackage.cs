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
        IEnumerable<IUmlEnumeration> Enumerations { get; }
        IUmlDataType CreateDataType(UmlDataTypeSpec spec);
        IUmlDataType GetDataTypeById(int id);

        IUmlTaggedValue GetTaggedValue(TaggedValues name);
    }
}