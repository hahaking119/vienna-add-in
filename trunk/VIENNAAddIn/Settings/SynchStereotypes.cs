/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.Settings
{
    /// <summary>
    /// This class should be used to check for missing TaggedValues of Elements/Connectors/Packages according to UPCC 3.0 spefication.
    /// Fix methods to add missing TaggedValues are also available.
    /// </summary>
    public class SynchStereotypes
    {
        //R.I.P. good old list solution .. 
        //private readonly List<String> stereotypes = new List<String>(new String[] { "ABIE", "ACC", "ASBIE", "ASCC", "BBIE", "BCC", "BCSS", "BIE", "CC", "CCTS", "CDT", "CON", "PRIM", "QDT", "SUP", "BDT" });
        //private readonly List<String> libraries = new List<string>(new String[] { "BIELibrary", "bLibrary", "BusinessLibrary", "CCLibrary", "CDTLibrary", "DOCLibrary", "ENUMLibrary", "PRIMLibrary", "QDTLibrary" });

        /// <summary>
        /// Fix a whole Package by first fixing the TaggedValues of the package itself and then its suppackages recursivley.
        /// </summary>
        /// <param name="p">The package to fix</param>
        public void FixPackage(Package p)
        {
            Fix(p);
            foreach (Element e in p.Elements)
            {
                Fix(e);
            }
            foreach (Package pp in p.Packages)
                FixPackage(pp);
        }

        /// <summary>
        /// Fix a whole Repository by fixing all of its packages.
        /// </summary>
        /// <param name="r">The repository to fix</param>
        public void FixRepository(Repository r)
        {
            foreach (Package p in r.Models)
            {
                foreach (Package pp in p.Packages)
                    FixPackage(pp);
            }
        }

        /// <summary>
        /// Check elements and connectors for missing TaggedValues and return a List of them
        /// </summary>
        /// <param name="e">The Element to be checked.</param>
        /// <returns>A List<String> with missing TaggedValues according to UPCC 3 specification</String></returns>
        public List<String> Check(Element e)
        {
            var missingValues = new List<String>();
            switch (e.Stereotype)
            {
                case "ABIE": //finished!
                    goto default;
                case "ACC": //finished!
                    goto default;
                case "ASBIE": //finisehd!
                    goto case "BCC";
                case "ASCC": //finished!
                    goto case "BCC";
                case "BBIE": //finished!
                    goto case "BCC";
                case "BCC": //finished!
                    if (Equals(e.GetTaggedValue(TaggedValues.SequencingKey), ""))
                        missingValues.Add(TaggedValues.SequencingKey.AsString());
                    goto default;
                case "BCSS":
                    //not found yet!
                    break;
                case "BIE":
                    //not found yet!
                    break;
                case "CC":
                    //not found yet! does it really mean CC?
                    break;
                case "CCTS":
                    //not found yet! does it really mean CCTS?
                    break;
                case "CDT":
                    goto default; //finished
                case "CON": //finished
                    goto case "SUP";
                case "ENUM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyIdentifier), ""))
                        missingValues.Add(TaggedValues.AgencyIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyName), ""))
                        missingValues.Add(TaggedValues.AgencyName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.EnumerationURI), ""))
                        missingValues.Add(TaggedValues.EnumerationURI.AsString());
                    break;
                case "PRIM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), ""))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), ""))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), ""))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), ""))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), ""))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), ""))
                        missingValues.Add(TaggedValues.ModificationAllowedIndicator.AsString());
                    goto default;
                case "basedOn": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.ApplyTo), ""))
                        missingValues.Add(TaggedValues.ApplyTo.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), ""))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), ""))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), ""))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), ""))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), ""))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UsageRule), ""))
                        missingValues.Add(TaggedValues.UsageRule.AsString());
                    break;
            }
            return missingValues;
        }

        /// <summary>
        /// Check elements and connectors for missing TaggedValues and return a List of them
        /// </summary>
        /// <param name="c">The Connector to be checked.</param>
        /// <returns>A List<String> with missing TaggedValues according to UPCC 3 specification</String></returns>
        public List<String> Check(Connector c)
        {
            var missingValues = new List<String>();
            switch (c.Stereotype)
            {
                case "ABIE": //finished!
                    goto default;
                case "ACC": //finished!
                    goto default;
                case "ASBIE": //finisehd!
                    goto case "BCC";
                case "ASCC": //finished!
                    goto case "BCC";
                case "BBIE": //finished!
                    goto case "BCC";
                case "BCC": //finished!
                    if (Equals(c.GetTaggedValue(TaggedValues.SequencingKey), ""))
                        missingValues.Add(TaggedValues.SequencingKey.AsString());
                    goto default;
                case "BCSS":
                    //not found yet!
                    break;
                case "BIE":
                    //not found yet!
                    break;
                case "CC":
                    //not found yet! does it really mean CC?
                    break;
                case "CCTS":
                    //not found yet! does it really mean CCTS?
                    break;
                case "CDT":
                    goto default; //finished
                case "CON": //finished
                    goto case "SUP";
                case "ENUM": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.AgencyIdentifier), ""))
                        missingValues.Add(TaggedValues.AgencyIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.AgencyName), ""))
                        missingValues.Add(TaggedValues.AgencyName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.EnumerationURI), ""))
                        missingValues.Add(TaggedValues.EnumerationURI.AsString());
                    break;
                case "PRIM": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), ""))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), ""))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), ""))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), ""))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), ""))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), ""))
                        missingValues.Add(TaggedValues.ModificationAllowedIndicator.AsString());
                    goto default;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ApplyTo), ""))
                        missingValues.Add(TaggedValues.ApplyTo.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), ""))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), ""))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), ""))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), ""))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), ""))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UsageRule), ""))
                        missingValues.Add(TaggedValues.UsageRule.AsString());
                    break;
            }
            return missingValues;
        }

        /// <summary>
        /// Add missing TaggedValues to Elements
        /// </summary>
        /// <param name="e">The Element to fix</param>
        public void Fix(Element e)
        {
            switch (e.Stereotype)
            {
                case "ABIE": //finished!
                    goto default;
                case "ACC": //finished!
                    goto default;
                case "ASBIE": //finisehd!
                    goto case "BCC";
                case "ASCC": //finished!
                    goto case "BCC";
                case "BBIE": //finished!
                    goto case "BCC";
                case "BCC": //finished!
                    if (Equals(e.GetTaggedValue(TaggedValues.SequencingKey), ""))
                        e.TaggedValues.AddNew("", TaggedValues.SequencingKey.AsString());
                    goto default;
                case "BCSS":
                    //not found yet!
                    break;
                case "BIE":
                    //not found yet!
                    break;
                case "CC":
                    //not found yet! does it really mean CC?
                    break;
                case "CCTS":
                    //not found yet! does it really mean CCTS?
                    break;
                case "CDT":
                    goto default; //finished
                case "CON": //finished
                    goto case "SUP";
                case "ENUM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.AgencyIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyName), ""))
                        e.TaggedValues.AddNew("", TaggedValues.AgencyName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        e.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        e.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        e.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.EnumerationURI), ""))
                        e.TaggedValues.AddNew("", TaggedValues.EnumerationURI.AsString());
                    break;
                case "PRIM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        e.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        e.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        e.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Pattern.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        e.TaggedValues.AddNew("", TaggedValues.FractionDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Length.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        e.TaggedValues.AddNew("", TaggedValues.TotalDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        e.TaggedValues.AddNew("", TaggedValues.WhiteSpace.AsString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), ""))
                        e.TaggedValues.AddNew("", TaggedValues.ModificationAllowedIndicator.AsString());
                    goto default;
                case "basedOn": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.ApplyTo), ""))
                        e.TaggedValues.AddNew("", TaggedValues.ApplyTo.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Pattern.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        e.TaggedValues.AddNew("", TaggedValues.FractionDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Length.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MaxLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), ""))
                        e.TaggedValues.AddNew("", TaggedValues.MinLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        e.TaggedValues.AddNew("", TaggedValues.TotalDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        e.TaggedValues.AddNew("", TaggedValues.WhiteSpace.AsString());
                    break;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        e.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), ""))
                        e.TaggedValues.AddNew("", TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        e.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        e.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        e.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UsageRule), ""))
                        e.TaggedValues.AddNew("", TaggedValues.UsageRule.AsString());
                    break;
            }
        }

        /// <summary>
        /// Add missing TaggedValues to Connectors
        /// </summary>
        /// <param name="c">Connector to fix</param>
        public void Fix(Connector c)
        {
            switch (c.Stereotype)
            {
                case "ABIE": //finished!
                    goto default;
                case "ACC": //finished!
                    goto default;
                case "ASBIE": //finisehd!
                    goto case "BCC";
                case "ASCC": //finished!
                    goto case "BCC";
                case "BBIE": //finished!
                    goto case "BCC";
                case "BCC": //finished!
                    if (Equals(c.GetTaggedValue(TaggedValues.SequencingKey), ""))
                        c.TaggedValues.AddNew("", TaggedValues.SequencingKey.AsString());
                    goto default;
                case "BCSS":
                    //not found yet!
                    break;
                case "BIE":
                    //not found yet!
                    break;
                case "CC":
                    //not found yet! does it really mean CC?
                    break;
                case "CCTS":
                    //not found yet! does it really mean CCTS?
                    break;
                case "CDT":
                    goto default; //finished
                case "CON": //finished
                    goto case "SUP";
                case "ENUM": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.AgencyIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.AgencyIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.AgencyName), ""))
                        c.TaggedValues.AddNew("", TaggedValues.AgencyName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        c.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        c.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        c.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.EnumerationURI), ""))
                        c.TaggedValues.AddNew("", TaggedValues.EnumerationURI.AsString());
                    break;
                case "PRIM": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        c.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        c.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        c.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Definition.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Pattern.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        c.TaggedValues.AddNew("", TaggedValues.FractionDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Length.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        c.TaggedValues.AddNew("", TaggedValues.TotalDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        c.TaggedValues.AddNew("", TaggedValues.WhiteSpace.AsString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), ""))
                        c.TaggedValues.AddNew("", TaggedValues.ModificationAllowedIndicator.AsString());
                    goto default;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ApplyTo), ""))
                        c.TaggedValues.AddNew("", TaggedValues.ApplyTo.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Pattern.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), ""))
                        c.TaggedValues.AddNew("", TaggedValues.FractionDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Length.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MaxLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), ""))
                        c.TaggedValues.AddNew("", TaggedValues.MinLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), ""))
                        c.TaggedValues.AddNew("", TaggedValues.TotalDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), ""))
                        c.TaggedValues.AddNew("", TaggedValues.WhiteSpace.AsString());
                    break;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                        c.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), ""))
                        c.TaggedValues.AddNew("", TaggedValues.Definition.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), ""))
                        c.TaggedValues.AddNew("", TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), ""))
                        c.TaggedValues.AddNew("", TaggedValues.LanguageCode.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                        c.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.UsageRule), ""))
                        c.TaggedValues.AddNew("", TaggedValues.UsageRule.AsString());
                    break;
            }
        }

        /// <summary>
        /// Add missing TaggedValues to Packages
        /// </summary>
        /// <param name="p">The package to fix</param>
        public void Fix(Package p)
        {
            if (Equals(p.GetTaggedValues(TaggedValues.UniqueIdentifier), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.UniqueIdentifier.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.UniqueIdentifier.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.VersionIdentifier), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.VersionIdentifier.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.VersionIdentifier.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.BaseURN), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.BaseURN.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.BaseURN.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.BusinessTerm), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.BusinessTerm.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.BusinessTerm.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Copyright), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Copyright.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.Copyright.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Owner), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Owner.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.Owner.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Reference), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Reference.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.Reference.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Status), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Status.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.Status.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.NamespacePrefix), ""))
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.NamespacePrefix.AsString());
            p.Element.TaggedValues.AddNew("", TaggedValues.NamespacePrefix.AsString());
        }
    }
}