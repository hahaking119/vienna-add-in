using UPCCRepositoryInterface;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    internal interface IUmlTaggedValue
    {
        string Value { get; }

        /// <summary>
        /// Split the tagged value's value using <see cref="MultiPartTaggedValue.ValueSeparator"/>.
        /// </summary>
        string[] SplitValues { get; }
    }
}