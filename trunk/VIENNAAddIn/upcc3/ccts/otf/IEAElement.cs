namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IEAElement : IEAItem
    {
        int PackageId { get; }
        IEAPackage Package { get; set; }
    }
}