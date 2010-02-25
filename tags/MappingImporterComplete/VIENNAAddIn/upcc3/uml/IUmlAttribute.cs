using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlAttribute
    {
        int Id { get; }
        string Name { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IUmlClassifier Type { get; }

        IEnumerable<IUmlTaggedValue> GetTaggedValues();
        IUmlTaggedValue GetTaggedValue(string name);
    }
}