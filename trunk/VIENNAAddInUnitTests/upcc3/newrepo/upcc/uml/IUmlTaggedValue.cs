using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    public interface IUmlTaggedValue
    {
        string Value { get; }

        /// <summary>
        /// Split the tagged value's value using <see cref="MultiPartTaggedValue.ValueSeparator"/>.
        /// </summary>
        string[] SplitValues { get; }
    }
}