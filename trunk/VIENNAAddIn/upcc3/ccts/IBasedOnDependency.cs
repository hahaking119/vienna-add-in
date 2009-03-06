using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBasedOnDependency : IPRIMRestrictions
    {
        [TaggedValue(TaggedValues.ApplyTo)]
        string ApplyTo { get; }

        ICDT CDT { get; }
    }
}