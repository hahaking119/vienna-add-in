namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IEAElement : IEAItem
    {
        ItemId PackageId { get; }
        IEAPackage Package { get; set; }
    }
}