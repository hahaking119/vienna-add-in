using System;
using EA;
using CctsRepository;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;
using Stereotype=CctsRepository.Stereotype;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// CCTS specific extension methods for EA repository classes.
    /// </summary>
    public static class CCTSEAExtensions
    {
        public static Attribute AddCON(this Element e, Element type)
        {
            return e.AddAttribute("Content", type).With((Action<Attribute>) (attribute => { attribute.Stereotype = Stereotype.CON; }));
        }

        public static Attribute AddSUP(this Element e, Element type, string name)
        {
            return e.AddAttribute(name, type).With((Action<Attribute>) (attribute => { attribute.Stereotype = Stereotype.SUP; }));
        }

        public static void AddSUPs(this Element e, Element type, params string[] names)
        {
            foreach (string name in names)
            {
                AddSUP(e, type, name);
            }
        }

        public static void AddBCCs(this Element e, Element type, params string[] names)
        {
            foreach (string name in names)
            {
                AddBCC(e, type, name);
            }
        }

        public static Attribute AddBCC(this Element e, Element type, string name)
        {
            return e.AddAttribute(name, type).With((Action<Attribute>) (attribute => { attribute.Stereotype = Stereotype.BCC; }));
        }

        public static void AddBBIEs(this Element e, Element type, params string[] names)
        {
            foreach (string name in names)
            {
                AddBBIE(e, type, name);
            }
        }

        public static Attribute AddBBIE(this Element e, Element type, string name)
        {
            return e.AddAttribute(name, type).With(attribute => { attribute.Stereotype = Stereotype.BBIE; });
        }

        public static Element AddPRIM(this Package primLib1, string name)
        {
            return primLib1.AddClass(name).With(EARepository.ElementStereotype(Stereotype.PRIM));
        }

        public static Element AddENUM(this Package enumLib1, string name, Element type, params string[][] codelistEntries)
        {
            return enumLib1.AddClass(name).With(EARepository.ElementStereotype(Stereotype.ENUM),
                                                e =>
                                                {
                                                    foreach (var codelistEntry in codelistEntries)
                                                    {
                                                        AddENUMValue(e, codelistEntry[0], codelistEntry[1], codelistEntry[2]);
                                                    }
                                                });
        }

        public static void AddENUMValue(this Element e, string name, string codeName, string status)
        {
            e.AddAttribute(name, "string").With(a =>
                                                {
                                                    a.Stereotype = Stereotype.CodelistEntry;
                                                    a.AddTaggedValue(TaggedValues.codeName.ToString()).WithValue(codeName);
                                                    a.AddTaggedValue(TaggedValues.status.ToString()).WithValue(status);
                                                });
        }

        public static void AddASCC(this Element client, Element supplier, string name)
        {
            AddASCC(client, supplier, name, "1", "1");
        }

        public static void AddASCC(this Element client, Element supplier, string name, string lowerBound, string upperBound)
        {
            client.AddConnector(name, EAConnectorTypes.Aggregation.ToString(), c =>
                                                                               {
                                                                                   c.Stereotype = Stereotype.ASCC;
                                                                                   c.ClientEnd.Aggregation = (int) EAAggregationKind.Shared;
                                                                                   c.SupplierID = supplier.ElementID;
                                                                                   c.SupplierEnd.Role = name;
                                                                                   c.SupplierEnd.Cardinality = lowerBound + ".." + upperBound;
                                                                               });
        }

        public static void AddASBIE(this Element client, Element supplier, string name, EAAggregationKind aggregationKind)
        {
            AddASBIE(client, supplier, name, aggregationKind, "1", "1");
        }

        public static void AddASBIE(this Element client, Element supplier, string name, EAAggregationKind aggregationKind, string lowerBound, string upperBound)
        {
            client.AddConnector(name, EAConnectorTypes.Aggregation.ToString(), c =>
                                                                               {
                                                                                   c.Stereotype = Stereotype.ASBIE;
                                                                                   c.ClientEnd.Aggregation = (int) aggregationKind;
                                                                                   c.SupplierID = supplier.ElementID;
                                                                                   c.SupplierEnd.Role = name;
                                                                                   c.SupplierEnd.Cardinality = lowerBound + ".." + upperBound;
                                                                               });
        }

        public static void AddBasedOnDependency(this Element client, Element supplier)
        {
            client.AddConnector("basedOn", EAConnectorTypes.Dependency.ToString(), c =>
                                                                                   {
                                                                                       c.Stereotype = Stereotype.BasedOn;
                                                                                       c.ClientEnd.Aggregation = (int) EAAggregationKind.None;
                                                                                       c.SupplierID = supplier.ElementID;
                                                                                       c.SupplierEnd.Role = "basedOn";
                                                                                       c.SupplierEnd.Cardinality = "1";
                                                                                   });
        }

        public static void AddIsEquivalentToDependency(this Element client, Element supplier)
        {
            client.AddConnector("isEquivalentTo", EAConnectorTypes.Dependency.ToString(), c =>
                                                                                   {
                                                                                       c.Stereotype = Stereotype.IsEquivalentTo;
                                                                                       c.ClientEnd.Aggregation = (int) EAAggregationKind.None;
                                                                                       c.SupplierID = supplier.ElementID;
                                                                                       c.SupplierEnd.Role = "isEquivalentTo";
                                                                                       c.SupplierEnd.Cardinality = "1";
                                                                                   });
        }

        public static Element AddACC(this Package package, string name)
        {
            return package.AddClass(name).With(EARepository.ElementStereotype(Stereotype.ACC));
        }

        public static Element AddABIE(this Package package, string name)
        {
            return package.AddClass(name).With(EARepository.ElementStereotype(Stereotype.ABIE));
        }

        public static Element AddBDT(this Package package, string name)
        {
            return package.AddClass(name).With(EARepository.ElementStereotype(Stereotype.BDT));
        }

        public static Element AddCDT(this Package package, string name)
        {
            return package.AddClass(name).With(EARepository.ElementStereotype(Stereotype.CDT));
        }
    }
}