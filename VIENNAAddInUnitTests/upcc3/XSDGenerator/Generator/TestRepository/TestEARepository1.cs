namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class TestEARepository1 : TestEARepository
    {
        public TestEARepository1()
        {
            Path stringType = new Path(this)/"blib1"/"primlib1"/"String";
            Path decimalType = new Path(this)/"blib1"/"primlib1"/"Decimal";
            BLibrary(() =>
                         {
                             Name("blib1");
                             BaseURN("http://test/blib1");
                             PRIMLibrary(() =>
                                             {
                                                 Name("primlib1");
                                                 BaseURN("primlib1");
                                                 PRIM(() => Name("String"));
                                                 PRIM(() => Name("Decimal"));
                                             });
                             CDTLibrary(() =>
                                            {
                                                Name("cdtlib1");
                                                BaseURN("cdtlib1");
                                                CDT(() =>
                                                        {
                                                            Name("Date");
                                                            ContentType(stringType);
                                                            SUP(() =>
                                                                    {
                                                                        Name("Format");
                                                                        OfType(stringType);
                                                                    });
                                                        });
                                                CDT(() =>
                                                        {
                                                            Name("Measure");
                                                            ContentType(decimalType);
                                                            SUP(() =>
                                                                    {
                                                                        Name("MeasureUnit");
                                                                        OfType(stringType);
                                                                    });
                                                            SUP(() =>
                                                                    {
                                                                        Name("MeasureUnit.CodeListVersion");
                                                                        OfType(stringType);
                                                                        LowerBound("1");
                                                                        UpperBound("*");
                                                                    });
                                                        });
                                            });
                         });
        }
    }
}