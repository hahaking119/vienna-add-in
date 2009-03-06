using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.Settings
{
//    public class SynchStereotypes
//    {
//        public void FixPackage(Package p)
//        {
//            foreach (Element e in p.Elements)
//            {
//                Fix(e);
//                foreach (Package pp in p.Packages)
//                    FixPackage(pp);
//            }
//        }
//
//        /**************************************
//         * Check elements for missing TaggedValues
//         **************************************/
//
//        public List<String> Check(Element e)
//        {
//            var missingValues = new List<String>();
//            switch (e.Stereotype)
//            {
//                case "ABIE", "ACC", "ASBIE", "ASCC", "BBIE", "BCC", "BCSS", "BIE", "CC", "CCTS", "CDT", "CON", "PRIM", "QDT", "SUP", "BDT":
//                    if (e.GetTaggedValues(TaggedValues.Definition) == null)
//                        missingValues.Add(TaggedValues.Definition.AsString());
//                    if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
//                    break;
//                case "BIELibrary", "bLibrary", "BusinessLibrary", "CCLibrary", "CDTLibrary", "DOCLibrary", "ENUMLibrary", "PRIMLibrary", "QDTLibrary":
//                    if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
//                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
//                    if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
//                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
//                    if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
//                        missingValues.Add(TaggedValues.BaseURN.AsString());
//                    break;
//                case "ENUM":
//                    if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
//                    break;
//                case "basedOn":
//                    if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
//                        missingValues.Add(TaggedValues.ApplyTo.AsString());
//                    break;
//            }
//            return missingValues;
//        }
//
//        /**************************************
//         * Repair missing TaggedValues
//         **************************************/
//
//        public void Fix(Element e)
//        {
//            switch (e.Stereotype)
//            {
//                case "ABIE", "ACC", "ASBIE", "ASCC", "BBIE", "BCC", "BCSS", "BIE", "CC", "CCTS", "CDT", "CON", "PRIM", "QDT", "SUP", "BDT":
//                    if (e.GetTaggedValues(TaggedValues.Definition) == null)
//                        e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
//                    if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//                        e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
//                    break;
//                case "BIELibrary", "bLibrary", "BusinessLibrary", "CCLibrary", "CDTLibrary", "DOCLibrary", "ENUMLibrary", "PRIMLibrary", "QDTLibrary":
//                    if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
//                        e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
//                    if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
//                        e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
//                    if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
//                        e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
//                    break;
//                case "ENUM":
//                    if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//                        e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
//                    break;
//                case "basedOn":
//                    if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
//                        e.TaggedValues.AddNew(TaggedValues.ApplyTo.AsString(), TaggedValues.ApplyTo.AsString());
//                    break;
//            }
//        }
//    }
}
