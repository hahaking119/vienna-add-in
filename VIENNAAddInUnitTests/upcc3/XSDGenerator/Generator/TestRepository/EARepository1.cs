using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EARepository1 : EARepository
    {
        public EARepository1()
        {
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
                                                                   PRIMLib1(),
                                                                   CDTLib1(),
                                                                   BDTLib1(),
                                                                   CCLib1(),
                                                                   BIELib1(),
                                                               },
                                               },
                                           },
                           },
                       });
        }

        #region PRIMLib1

        private static UpccLibrary PRIMLib1()
        {
            return new PRIMLibrary
                   {
                       Name = "primlib1",
                       BaseURN = "primlib1",
                       PRIMs = new List<PRIM>
                               {
                                   new PRIM {Name = "String"},
                                   new PRIM {Name = "Decimal"},
                               },
                   };
        }

        #endregion

        #region CDTLib1

        private static UpccLibrary CDTLib1()
        {
            return new CDTLibrary
                   {
                       Name = "cdtlib1",
                       BaseURN = "cdtlib1",
                       CDTs = new List<CDT>
                              {
                                  Text(),
                                  Date(),
                                  Code(),
                                  Measure(),
                              },
                   };
        }

        private static CDT Measure()
        {
            return new CDT
                   {
                       Name = "Measure",
                       CON = PathToDecimal(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name =
                                          "MeasureUnit",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "MeasureUnit.CodeListVersion",
                                      Type = PathToString(),
                                      LowerBound = "1",
                                      UpperBound = "*",
                                  },
                              },
                   };
        }

        private static CDT Code()
        {
            return new CDT
                   {
                       Name = "Code",
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Name",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Agency",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.AgencyName",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name = "CodeList",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Name",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.UniformResourceIdentifier",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Version",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeListScheme.UniformResourceIdentifier",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name = "Language",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        private static CDT Date()
        {
            return new CDT
                   {
                       Name = "Date",
                       Definition = "A Date.",
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Format",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        private static CDT Text()
        {
            return new CDT
                   {
                       Name = "Text",
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Language",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "Language.Locale",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        #endregion

        #region BDTLib1

        private static UpccLibrary BDTLib1()
        {
            return new BDTLibrary
                   {
                       Name = "bdtlib1",
                       BaseURN = "bdtlib1",
                       BDTs = new List<BDT>
                              {
                                  BDTText(),
                                  BDTDate(),
                                  BDTCode(),
                                  BDTMeasure(),
                              }
                   }
                ;
        }

        private static BDT BDTCode()
        {
            return new BDT
                   {
                       BasedOn = PathToCode(),
                       Name = "Code",
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Name",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Agency",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.AgencyName",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name = "CodeList",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Name",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.UniformResourceIdentifier",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeList.Version",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "CodeListScheme.UniformResourceIdentifier",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name = "Language",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        private static BDT BDTText()
        {
            return new BDT
                   {
                       BasedOn = PathToText(),
                       Name = "Text",
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Language",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "Language.Locale",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        private static BDT BDTMeasure()
        {
            return new BDT
                   {
                       Name = "Measure",
                       BasedOn = PathToMeasure(),
                       CON = PathToDecimal(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name =
                                          "MeasureUnit",
                                      Type = PathToString(),
                                  },
                                  new SUP
                                  {
                                      Name =
                                          "MeasureUnit.CodeListVersion",
                                      Type = PathToString(),
                                      LowerBound = "1",
                                      UpperBound = "*",
                                  },
                              },
                   };
        }

        private static BDT BDTDate()
        {
            return new BDT
                   {
                       Name = "Date",
                       BasedOn = PathToDate(),
                       CON = PathToString(),
                       SUPs = new List<SUP>
                              {
                                  new SUP
                                  {
                                      Name = "Format",
                                      Type = PathToString(),
                                  },
                              },
                   };
        }

        #endregion

        #region CCLib1

        private static UpccLibrary CCLib1()
        {
            return new CCLibrary
                   {
                       Name = "cclib1",
                       BaseURN = "cclib1",
                       ACCs = new List<ACC>
                              {
                                  Address(),
                              }
                   };
        }

        private static ACC Address()
        {
            return new ACC
                   {
                       Name = "Address",
                       BCCs = new List<BCC>
                              {
                                  new BCC
                                  {
                                      Name = "CountryName",
                                      Type = PathToText(),
                                  },
                                  new BCC
                                  {
                                      Name = "CityName",
                                      Type = PathToText(),
                                  },
                                  new BCC
                                  {
                                      Name = "StreetName",
                                      Type = PathToText(),
                                  },
                                  new BCC
                                  {
                                      Name = "StreetNumber",
                                      Type = PathToText(),
                                  },
                                  new BCC
                                  {
                                      Name = "Postcode",
                                      Type = PathToText(),
                                  },
                              },
                   };
        }

        #endregion

        #region BIELib1

        private static UpccLibrary BIELib1()
        {
            return new BIELibrary
                   {
                       Name = "bielib1",
                       BaseURN = "bielib1",
                       ABIEs = new List<ABIE>(),
                   };
        }

        #endregion

        #region Paths

        private static Path PathToMeasure()
        {
            return (Path) "blib1"/"cdtlib1"/"Measure";
        }

        public static Path PathToDate()
        {
            return (Path) "blib1"/"cdtlib1"/"Date";
        }

        public static Path PathToCode()
        {
            return (Path) "blib1"/"cdtlib1"/"Code";
        }

        public static Path PathToText()
        {
            return (Path) "blib1"/"cdtlib1"/"Text";
        }

        public static Path PathToBDTText()
        {
            return (Path) "blib1"/"bdtlib1"/"Text";
        }

        public static Path PathToBDTCode()
        {
            return (Path) "blib1"/"bdtlib1"/"Code";
        }

        private static Path PathToDecimal()
        {
            return (Path) "blib1"/"primlib1"/"Decimal";
        }

        public static Path PathToString()
        {
            return (Path) "blib1"/"primlib1"/"String";
        }

        public static Path PathToAddress()
        {
            return (Path) "blib1"/"cclib1"/"Address";
        }

        #endregion
    }
}