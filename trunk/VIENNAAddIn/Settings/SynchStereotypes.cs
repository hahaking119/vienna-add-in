using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.Settings
{
    public class SynchStereotypes
    {
        public void FixPackage(Package p)
        {
            foreach (Element e in p.Elements)
            {
                switch (e.Stereotype)
                {
                    case "ABIE":
                        FixABIE(e);
                        break;
                    case "ACC":
                        FixACC(e);
                        break;
                    case "ASBIE":
                        FixASBIE(e);
                        break;
                    case "ASCC":
                        FixASCC(e);
                        break;
                    case "BBIE":
                        FixBBIE(e);
                        break;
                    case "BCC":
                        FixBCC(e);
                        break;
                    case "BCSS":
                        FixBCSS(e);
                        break;
                    case "BIE":
                        FixBIE(e);
                        break;
                    case "BIELibrary":
                        FixBIELibrary(e);
                        break;
                    case "bLibrary":
                        FixbLibrary(e);
                        break;
                    case "BusinessLibrary":
                        FixBusinessLibrary(e);
                        break;
                    case "CC":
                        FixCC(e);
                        break;
                    case "CCLibrary":
                        FixCCLibrary(e);
                        break;
                    case "CCTS":
                        FixCCTS(e);
                        break;
                    case "CDT":
                        FixCDT(e);
                        break;
                    case "CDTLibrary":
                        FixCDTLibrary(e);
                        break;
                    case "CON":
                        FixCON(e);
                        break;
                    case "DOCLibrary":
                        FixDOCLibrary(e);
                        break;
                    case "ENUM":
                        FixENUM(e);
                        break;
                    case "ENUMLibrary":
                        FixENUMLibrary(e);
                        break;
                    case "PRIM":
                        FixPRIM(e);
                        break;
                    case "PRIMLibrary":
                        FixPRIMLibrary(e);
                        break;
                    case "QDT":
                        FixQDT(e);
                        break;
                    case "QDTLibrary":
                        FixQDTLibrary(e);
                        break;
                    case "SUP":
                        FixSUP(e);
                        break;
                    case "basedOn":
                        FixbasedOn(e);
                        break;
                    case "BDT":
                        FixBDT(e);
                        break;
                }
            }
            foreach (Package pp in p.Packages)
                FixPackage(pp);
        }

        /**************************************
         * Check elements for missing TaggedValues
         **************************************/

        public List<String> CheckABIE(Element e)
        {
            var missingValues = new List<String>();
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                    missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
            return missingValues;
        }

        public List<String> CheckACC(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckASBIE(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckASCC(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckBBIE(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckBCC(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckBCSS(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckBIE(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckBIELibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckbLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckBusinessLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckCC(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckCCLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckCCTS(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckCDT(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckCDTLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckCON(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckDOCLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckENUM(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckENUMLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckPRIM(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckPRIMLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckQDT(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckQDTLibrary(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.UniqueIdentifier) == null)
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.VersionIdentifier) == null)
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValues(TaggedValues.BaseURN) == null)
                missingValues.Add(TaggedValues.BaseURN.AsString());

            return missingValues;
        }

        public List<String> CheckSUP(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        public List<String> CheckbasedOn(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.ApplyTo) == null)
                missingValues.Add(TaggedValues.ApplyTo.AsString());

            return missingValues;
        }

        public List<String> CheckBDT(Element e)
        {
            var missingValues = new List<String>();

            if (e.GetTaggedValues(TaggedValues.Definition) == null)
                missingValues.Add(TaggedValues.Definition.AsString());
            if (e.GetTaggedValues(TaggedValues.DictionaryEntryName) == null)
                missingValues.Add(TaggedValues.DictionaryEntryName.AsString());

            return missingValues;
        }

        /**************************************
         * Repair missing TaggedValues
         **************************************/

        public bool FixABIE(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixACC(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixASBIE(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixASCC(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixBBIE(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixBCC(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixBCSS(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixBIE(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixBIELibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixbLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixBusinessLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixCC(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixCCLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixCCTS(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixCDT(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixCDTLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixCON(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixDOCLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixENUM(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixENUMLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixPRIM(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixPRIMLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixQDT(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixQDTLibrary(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.UniqueIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.AsString(), TaggedValues.UniqueIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.VersionIdentifier) == null)
                e.TaggedValues.AddNew(TaggedValues.VersionIdentifier.AsString(), TaggedValues.VersionIdentifier.AsString());
            if (e.GetTaggedValue(TaggedValues.BaseURN) == null)
                e.TaggedValues.AddNew(TaggedValues.BaseURN.AsString(), TaggedValues.BaseURN.AsString());
            return true;
        }

        public bool FixSUP(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }

        public bool FixbasedOn(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.ApplyTo) == null)
                e.TaggedValues.AddNew(TaggedValues.ApplyTo.AsString(), TaggedValues.ApplyTo.AsString());
            return true;
        }

        public bool FixBDT(Element e)
        {
            if (e.GetTaggedValue(TaggedValues.Definition) == null)
                e.TaggedValues.AddNew(TaggedValues.Definition.AsString(), TaggedValues.Definition.AsString());
            if (e.GetTaggedValue(TaggedValues.DictionaryEntryName) == null)
                e.TaggedValues.AddNew(TaggedValues.DictionaryEntryName.AsString(), TaggedValues.DictionaryEntryName.AsString());
            return true;
        }
    }
}
