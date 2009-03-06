namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIM :  IBasicType, IPRIMRestrictions
    {
        IPRIM IsEquivalentTo { get; }
    }
}