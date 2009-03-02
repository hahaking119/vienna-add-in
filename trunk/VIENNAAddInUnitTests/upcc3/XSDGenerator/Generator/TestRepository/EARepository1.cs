using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository1 : EARepository
    {
        public EARepository1()
        {
            Path stringType = (Path) "blib1"/"primlib1"/"String";
            Path decimalType = (Path) "blib1"/"primlib1"/"Decimal";
            Path cdtDate = (Path) "blib1"/"cdtlib1"/"Date";
            Path cdtMeasure = (Path) "blib1"/"cdtlib1"/"Measure";
            SetContent(new List<Model>
                       {
                           new Model
                           {
                               Name = "test model",
                               Libraries = new List<UpccLibrary>
                                           {
                                               new BLibrary
                                               {
                                                   Name = "blib1",
                                                   BaseURN = "http://test/blib1",
                                                   Libraries = new List<UpccLibrary>
                                                               {
                                                                   new PRIMLibrary
                                                                   {
                                                                       Name = "primlib1",
                                                                       BaseURN = "primlib1",
                                                                       PRIMs = new List<PRIM>
                                                                               {
                                                                                   new PRIM {Name = "String"},
                                                                                   new PRIM {Name = "Decimal"},
                                                                               },
                                                                   },
                                                                   new CDTLibrary
                                                                   {
                                                                       Name = "cdtlib1",
                                                                       BaseURN = "cdtlib1",
                                                                       CDTs = new List<CDT>
                                                                              {
                                                                                  new CDT
                                                                                  {
                                                                                      Name = "Date",
                                                                                      CON = stringType,
                                                                                      SUPs = new List<SUP>
                                                                                             {
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name = "Format",
                                                                                                     Type = stringType,
                                                                                                 },
                                                                                             },
                                                                                  },
                                                                                  new CDT
                                                                                  {
                                                                                      Name = "Measure",
                                                                                      CON = decimalType,
                                                                                      SUPs = new List<SUP>
                                                                                             {
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name =
                                                                                                         "MeasureUnit",
                                                                                                     Type = stringType,
                                                                                                 },
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name =
                                                                                                         "MeasureUnit.CodeListVersion",
                                                                                                     Type = stringType,
                                                                                                     LowerBound = "1",
                                                                                                     UpperBound = "*",
                                                                                                 },
                                                                                             },
                                                                                  },
                                                                              },
                                                                   },
                                                                   new BDTLibrary
                                                                   {
                                                                       Name = "bdtlib1",
                                                                       BaseURN = "bdtlib1",
                                                                       BDTs = new List<BDT>
                                                                              {
                                                                                  new BDT
                                                                                  {
                                                                                      Name = "Date",
                                                                                      BasedOn = cdtDate,
                                                                                      CON = stringType,
                                                                                      SUPs = new List<SUP>
                                                                                             {
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name = "Format",
                                                                                                     Type = stringType,
                                                                                                 },
                                                                                             },
                                                                                  },
                                                                                  new BDT
                                                                                  {
                                                                                      Name = "Measure",
                                                                                      BasedOn = cdtMeasure,
                                                                                      CON = decimalType,
                                                                                      SUPs = new List<SUP>
                                                                                             {
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name =
                                                                                                         "MeasureUnit",
                                                                                                     Type = stringType,
                                                                                                 },
                                                                                                 new SUP
                                                                                                 {
                                                                                                     Name =
                                                                                                         "MeasureUnit.CodeListVersion",
                                                                                                     Type = stringType,
                                                                                                     LowerBound = "1",
                                                                                                     UpperBound = "*",
                                                                                                 },
                                                                                             },
                                                                                  },
                                                                              }
                                                                   },
                                                                   new CCLibrary
                                                                   {
                                                                       Name = "cclib1",
                                                                       BaseURN = "cclib1",
                                                                       ACCs = new List<ACC>
                                                                              {
                                                                                  new ACC
                                                                                  {
                                                                                      Name = "Address",
                                                                                      BCCs = new List<BCC>
                                                                                             {
                                                                                                 new BCC
                                                                                                 {
                                                                                                     Name = "foo",
                                                                                                 },
                                                                                             },
                                                                                  }
                                                                              }
                                                                   }
                                                               },
                                               },
                                           },
                           },
                       });
        }
    }
}