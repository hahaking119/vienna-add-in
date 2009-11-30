using System.Collections.Generic;

namespace UpccModel
{
    public class UpccPackages
    {
        public readonly MetaPackage BdtLibrary;
        public readonly MetaPackage BieLibrary;
        public readonly MetaPackage BLibrary;
        public readonly MetaPackage CcLibrary;
        public readonly MetaPackage CdtLibrary;
        public readonly MetaPackage DocLibrary;
        public readonly MetaPackage EnumLibrary;
        public readonly MetaPackage PrimLibrary;

        public UpccPackages(UpccTaggedValues taggedValues)
        {
            var bLibraryTaggedValues = new[]
                                       {
                                           taggedValues.BusinessTerm,
                                           taggedValues.Copyright,
                                           taggedValues.Owner,
                                           taggedValues.Reference,
                                           taggedValues.Status,
                                           taggedValues.UniqueIdentifier,
                                           taggedValues.VersionIdentifier,
                                       };

            MetaTaggedValue[] elementLibraryTaggedValues = new List<MetaTaggedValue>(bLibraryTaggedValues)
                                                           {
                                                               taggedValues.BaseUrn,
                                                               taggedValues.NamespacePrefix,
                                                           }.ToArray();

            PrimLibrary = new MetaPackage
                          {
                              Name = "PrimLibrary",
                              Stereotype = "PRIMLibrary",
                              TaggedValues = elementLibraryTaggedValues,
                          };

            EnumLibrary = new MetaPackage
                          {
                              Name = "EnumLibrary",
                              Stereotype = "ENUMLibrary",
                              TaggedValues = elementLibraryTaggedValues,
                          };

            CdtLibrary = new MetaPackage
                         {
                             Name = "CdtLibrary",
                             Stereotype = "CDTLibrary",
                             TaggedValues = elementLibraryTaggedValues,
                         };

            CcLibrary = new MetaPackage
                        {
                            Name = "CcLibrary",
                            Stereotype = "CCLibrary",
                            TaggedValues = elementLibraryTaggedValues,
                        };

            BdtLibrary = new MetaPackage
                         {
                             Name = "BdtLibrary",
                             Stereotype = "BDTLibrary",
                             TaggedValues = elementLibraryTaggedValues,
                         };

            BieLibrary = new MetaPackage
                         {
                             Name = "BieLibrary",
                             Stereotype = "BIELibrary",
                             TaggedValues = elementLibraryTaggedValues,
                         };

            DocLibrary = new MetaPackage
                         {
                             Name = "DocLibrary",
                             Stereotype = "DOCLibrary",
                             TaggedValues = elementLibraryTaggedValues,
                         };

            BLibrary = new MetaPackage
                       {
                           Name = "BLibrary",
                           Stereotype = "bLibrary",
                           TaggedValues = bLibraryTaggedValues,
                       };
        }
    }
}