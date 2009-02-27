using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository1 : EARepository
    {
        public EARepository1()
        {
            Path stringType = new Path(this)/"blib1"/"primlib1"/"String";
            Path decimalType = new Path(this)/"blib1"/"primlib1"/"Decimal";
            Path cdtDate = new Path(this)/"blib1"/"cdtlib1"/"Date";
            Path cdtMeasure = new Path(this)/"blib1"/"cdtlib1"/"Measure";
            models = new List<Model>
                     {
                         new Model("test model")
                         {
                             Libraries = new List<UpccLibrary>
                                    {
                                        new BLibrary("blib1")
                                        {
                                            BaseURN = "http://test/blib1",
                                            Libraries = new List<UpccLibrary>
                                                   {
                                                       new PRIMLibrary("primlib1")
                                                       {
                                                           BaseURN = "primlib1",
                                                           PRIMs = new List<PRIM>
                                                                   {
                                                                       new PRIM("String"),
                                                                       new PRIM("Decimal"),
                                                                   },
                                                       },
                                                       new CDTLibrary("cdtlib1")
                                                       {
                                                           BaseURN = "cdtlib1",
                                                           CDTs = new List<CDT>
                                                                  {
                                                                      new CDT("Date")
                                                                      {
                                                                          CON = stringType,
                                                                          SUPs = new List<SUP>
                                                                                 {
                                                                                     new SUP("Format")
                                                                                     {
                                                                                         Type = stringType,
                                                                                     },
                                                                                 },
                                                                      },
                                                                      new CDT("Measure")
                                                                      {
                                                                          CON = decimalType,
                                                                          SUPs = new List<SUP>
                                                                                 {
                                                                                     new SUP("MeasureUnit")
                                                                                     {
                                                                                         Type = stringType,
                                                                                     },
                                                                                     new SUP("MeasureUnit.CodeListVersion")
                                                                                     {
                                                                                         Type = stringType,
                                                                                         LowerBound = "1",
                                                                                         UpperBound = "*",
                                                                                     },
                                                                                 },
                                                                      },
                                                                  },
                                                       },
                                                       new BDTLibrary("bdtlib1")
                                                       {
                                                           BaseURN = "bdtlib1",
                                                           BDTs = new List<BDT>
                                                                  {
                                                                      new BDT("Date")
                                                                      {
                                                                          BasedOn = cdtDate,
                                                                          CON = stringType,
                                                                          SUPs = new List<SUP>
                                                                                 {
                                                                                     new SUP("Format")
                                                                                     {
                                                                                         Type = stringType,
                                                                                     },
                                                                                 },
                                                                      },
                                                                      new BDT("Measure")
                                                                      {
                                                                          BasedOn = cdtMeasure,
                                                                          CON = decimalType,
                                                                          SUPs = new List<SUP>
                                                                                 {
                                                                                     new SUP("MeasureUnit")
                                                                                     {
                                                                                         Type = stringType,
                                                                                     },
                                                                                     new SUP("MeasureUnit.CodeListVersion")
                                                                                     {
                                                                                         Type = stringType,
                                                                                         LowerBound = "1",
                                                                                         UpperBound = "*",
                                                                                     },
                                                                                 },
                                                                      },
                                                                  }
                                                       },
                                                       new CCLibrary("cclib1")
                                                       {
                                                           BaseURN = "cclib1",
                                                           ACCs = new List<ACC>
                                                                  {
                                                                      new ACC("Address")
                                                                      {
                                                                          BCCs = new List<BCC>
                                                                                 {
                                                                                     new BCC("foo")
                                                                                     {
                                                                                         
                                                                                     },
                                                                                 },
                                                                      }
                                                                  }
                                                       }
                                                   },
                                        },
                                    },
                         },
                     };
        }
    }
}