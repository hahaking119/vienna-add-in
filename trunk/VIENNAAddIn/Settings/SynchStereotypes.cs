using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.Settings
{
    public class SynchStereotypes2
    {
        private readonly List<String> stereotypes = new List<String>(new String[] { "ABIE", "ACC", "ASBIE", "ASCC", "BBIE", "BCC", "BCSS", "BIE", "CC", "CCTS", "CDT", "CON", "PRIM", "QDT", "SUP", "BDT" });
        private readonly List<String> libraries = new List<string>(new String[] { "BIELibrary", "bLibrary", "BusinessLibrary", "CCLibrary", "CDTLibrary", "DOCLibrary", "ENUMLibrary", "PRIMLibrary", "QDTLibrary" });

        public void FixPackage(Package p)
        {
            foreach (Element e in p.Elements)
            {
                Fix(e);
                foreach (Package pp in p.Packages)
                    FixPackage(pp);
            }
        }

        /***********************************************************************************
         * Check elements and connectors for missing TaggedValues and return a List of them
         ***********************************************************************************/

        public List<String> Check(Element e)
        {
            var missingValues = new List<String>();
            if (stereotypes.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.Definition) == null)
                    missingValues.Add(TaggedValues.Definition.AsString());
                if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                    missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
            }
            else if (libraries.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                    missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                    missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                    missingValues.Add(TaggedValues.BaseURN.AsString());
            }
            else switch (e.Stereotype)
                {
                    case "ENUM":
                        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                            missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                        break;
                    case "basedOn":
                        if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
                            missingValues.Add(TaggedValues.ApplyTo.AsString());
                        break;
                }
            return missingValues;
        }
        public List<String> Check(Connector e)
        {
            var missingValues = new List<String>();
            if (stereotypes.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.Definition) == null)
                    missingValues.Add(TaggedValues.Definition.AsString());
                if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                    missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
            }
            else if (libraries.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                    missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                    missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                    missingValues.Add(TaggedValues.BaseURN.AsString());
            }
            else switch (e.Stereotype)
                {
                    case "ENUM":
                        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                            missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                        break;
                    case "basedOn":
                        if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
                            missingValues.Add(TaggedValues.ApplyTo.AsString());
                        break;
                }
            return missingValues;
        }

        /******************************************************
         * Add missing TaggedValues to elements and connectors
         *****************************************************/

        public void Fix(Element e)
        {
            if (stereotypes.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.Definition) == null)
                    e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
                if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                    e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(),
                                          TaggedValues.DictionaryEntryName.AsString());
            }
            else if (libraries.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                    e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(),
                                          TaggedValues.UniqueIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                    e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(),
                                          TaggedValues.VersionIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                    e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            }
            else switch (e.Stereotype)
                {
                    case "ENUM":
                        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                            e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(),
                                                  TaggedValues.DictionaryEntryName.AsString());
                        break;
                    case "basedOn":
                        if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
                            e.TaggedValues.AddNew(TaggedValues.ApplyTo.AsString(), TaggedValues.ApplyTo.AsString());
                        break;
                }
        }
        public void Fix(Connector e)
        {
            if (stereotypes.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.Definition) == null)
                    e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
                if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                    e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(),
                                          TaggedValues.DictionaryEntryName.AsString());
            }
            else if (libraries.Contains(e.Stereotype))
            {
                if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                    e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(),
                                          TaggedValues.UniqueIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                    e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(),
                                          TaggedValues.VersionIdentifier.AsString());
                if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                    e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            }
            else switch (e.Stereotype)
                {
                    case "ENUM":
                        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                            e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(),
                                                  TaggedValues.DictionaryEntryName.AsString());
                        break;
                    case "basedOn":
                        if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
                            e.TaggedValues.AddNew(TaggedValues.ApplyTo.AsString(), TaggedValues.ApplyTo.AsString());
                        break;
                }
        }
    }
}
#region oldImplementation (switch-fallthrough)
//switch (e.Stereotype)           //yeah we like fallthroughs!
//{
//    case "ABIE":
//    case "ACC":
//    case "ASBIE":
//    case "ASCC":
//    case "BBIE":
//    case "BCC":
//    case "BCSS":
//    case "BIE":
//    case "CC":
//    case "CCTS":
//    case "CDT":
//    case "CON":
//    case "PRIM":
//    case "QDT":
//    case "SUP":
//    case "BDT":
//        if (e.GetTaggedValues(TaggedValues.Definition) == null)
//            missingValues.Add(TaggedValues.Definition.AsString());
//        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//            missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
//        break;
//    case "BIELibrary":
//    case "bLibrary":
//    case "BusinessLibrary":
//    case "CCLibrary":
//    case "CDTLibrary":
//    case "DOCLibrary":
//    case "ENUMLibrary":
//    case "PRIMLibrary":
//    case "QDTLibrary":
//        if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
//            missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
//        if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
//            missingValues.Add(TaggedValues.VersionIdentifier.AsString());
//        if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
//            missingValues.Add(TaggedValues.BaseURN.AsString());
//        break;
//    case "ENUM":
//        if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
//            missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
//        break;
//    case "basedOn":
//        if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
//            missingValues.Add(TaggedValues.ApplyTo.AsString());
//        break;
//}
#endregion