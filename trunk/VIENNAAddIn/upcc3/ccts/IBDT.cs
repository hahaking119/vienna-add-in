namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBDT : IDT
    {
//        IEnumerable<IBasedOnDependency> BasedOn { get; }
        ICDT BasedOn { get; }
    }

//    public interface IBasedOnDependency
//    {
//        string Pattern { get; }
//        string FractionDigits { get; }
//        string Length { get; }
//        string MaxExclusive { get; }
//        string MaxInclusive { get; }
//        string MaxLength { get; }
//        string MinExclusive { get; }
//        string MinInclusive { get; }
//        string MinLength { get; }
//        string TotalDigits { get; }
//        string WhiteSpace { get; }
//        string ApplyTo { get; }
//    }
}