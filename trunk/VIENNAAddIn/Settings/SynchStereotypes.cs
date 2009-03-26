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
        /// <summary>
        /// Fix a whole Repository by fixing all of its packages
        /// </summary>
        /// <param name="r">The repository to fix</param>
        public void Fix(Repository r)
        {
            foreach (Package p in r.Models)
            {
                foreach (Package pp in p.Packages)
                    Fix(pp);
            }
        }

        /// <summary>
        /// Check Packages for missing TaggedValues and return a List of them
        /// </summary>
        /// <param name="p">The Package to be checked</param>
        /// <returns>A List<String> with missing TaggedValues according to UPCC 3 specification</returns>
        public List<List<String>> Check(Package p)
        {
            var missingValuesLists = new List<List<String>>();

            // check TaggedValues of current package
            var missingValues = new List<String>();
            if (Equals(p.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                missingValues.Add(TaggedValues.VersionIdentifier.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.BaseURN), null))
                missingValues.Add(TaggedValues.BaseURN.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.BusinessTerm), null))
                missingValues.Add(TaggedValues.BusinessTerm.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Copyright), null))
                missingValues.Add(TaggedValues.Copyright.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Owner), null))
                missingValues.Add(TaggedValues.Owner.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Reference), null))
                missingValues.Add(TaggedValues.Reference.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.Status), null))
                missingValues.Add(TaggedValues.Status.AsString());
            if (Equals(p.GetTaggedValue(TaggedValues.NamespacePrefix), null))
                missingValues.Add(TaggedValues.NamespacePrefix.AsString());
            if (missingValues.Count > 0)
            {
                missingValues.Insert(0, p.Name);
                missingValuesLists.Add(missingValues);
            }

            // check TaggedValues of current package's elements
            foreach (Element e in p.Elements)
            {
                missingValues = Check(e);
                if (missingValues.Count > 0)
                {
                    missingValues.Insert(0, e.Name);
                    missingValuesLists.Add(missingValues);
                }
            }

            // check TaggedValues of sub-packages recursively
            var missingValuesL = new List<List<String>>();
            foreach (Package pp in p.Packages)
            {
                missingValuesL = Check(pp);
                if (missingValuesL.Count > 0)
                    missingValuesLists.AddRange(missingValuesL);
            }

            return missingValuesLists;
        }

        /// <summary>
        /// Check elements and connectors for missing TaggedValues and return a List of them
        /// </summary>
        /// <param name="e">The Element to be checked</param>
        /// <returns>A List<String> with missing TaggedValues according to UPCC 3 specification</returns>
        public List<String> Check(Element e)
        {
            var missingValues = new List<String>();
            switch (e.Stereotype)
            {
                case "ABIE": //finished!
                    Debug.WriteLine("Checking ABIE.");
                    goto default;
                case "ACC": //finished!
                    Debug.WriteLine("Checking ACC.");
                    goto default;
                case "BBIE": //finished!
                    Debug.WriteLine("Checking BBIE.");
                    goto case "BCC";
                case "BCC": //finished!
                    Debug.WriteLine("Checking BCC.");
                    if (Equals(e.GetTaggedValue(TaggedValues.SequencingKey), null))
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
                    Debug.WriteLine("Checking CDT.");
                    goto default; //finished
                case "CON": //finished
                    Debug.WriteLine("Checking CON.");
                    goto case "SUP";
                case "ENUM": //finished
                    Debug.WriteLine("Checking ENUM.");
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyIdentifier), null))
                        missingValues.Add(TaggedValues.AgencyIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyName), null))
                        missingValues.Add(TaggedValues.AgencyName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.EnumerationURI), null))
                        missingValues.Add(TaggedValues.EnumerationURI.AsString());
                    break;
                case "PRIM": //finished
                    Debug.WriteLine("Checking PRIM.");
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), null))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), null))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), null))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), null))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), null))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), null))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), null))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), null))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), null))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), null))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), null))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), null))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    Debug.WriteLine("Checking SUP.");
                    if (Equals(e.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), null))
                        missingValues.Add(TaggedValues.ModificationAllowedIndicator.AsString());
                    goto default;
                case "BDT": //finished
                    Debug.WriteLine("Checking BDT.");
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), null))
                        missingValues.Add(TaggedValues.Definition.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                    if (Equals(e.GetTaggedValue(TaggedValues.UsageRule), null))
                        missingValues.Add(TaggedValues.UsageRule.AsString());
                    break;
            }
            return missingValues;
        }

        /// <summary>
        /// Check elements and connectors for missing TaggedValues and return a List of them
        /// </summary>
        /// <param name="c">The Connector to be checked</param>
        /// <returns>A List<String> with missing TaggedValues according to UPCC 3 specification</returns>
        public List<String> Check(Connector c)
        {
            var missingValues = new List<String>();
            switch (c.Stereotype)
            {
                case "ASBIE": //finished!
                    goto case "ASCC";
                case "ASCC": //finished!
                    if (Equals(c.GetTaggedValue(TaggedValues.SequencingKey), null))
                    {
                        Debug.WriteLine("Found Missing SequencingKey");
                        missingValues.Add(TaggedValues.SequencingKey.AsString());
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), null))
                    {
                        missingValues.Add(TaggedValues.BusinessTerm.AsString());
                        Debug.WriteLine("Found Missing BusinessTerm");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), null))
                    {
                        missingValues.Add(TaggedValues.Definition.AsString());
                        Debug.WriteLine("Found Missing Definition");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                    {
                        missingValues.Add(TaggedValues.DictionaryEntryName.AsString());
                        Debug.WriteLine("Found Missing DictionaryEntryName");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), null))
                    {
                        missingValues.Add(TaggedValues.LanguageCode.AsString());
                        Debug.WriteLine("Found Missing LanguageCode");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                    {
                        missingValues.Add(TaggedValues.UniqueIdentifier.AsString());
                        Debug.WriteLine("Found Missing UniqueIdentifier");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                    {
                        missingValues.Add(TaggedValues.VersionIdentifier.AsString());
                        Debug.WriteLine("Found Missing VersionIdentifier");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.UsageRule), null))
                    {
                        missingValues.Add(TaggedValues.UsageRule.AsString());
                        Debug.WriteLine("Found Missing UsageRule");
                    }
                    break;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ApplyTo), null))
                        missingValues.Add(TaggedValues.ApplyTo.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), null))
                        missingValues.Add(TaggedValues.Pattern.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), null))
                        missingValues.Add(TaggedValues.FractionDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), null))
                        missingValues.Add(TaggedValues.Length.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), null))
                        missingValues.Add(TaggedValues.MaxExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), null))
                        missingValues.Add(TaggedValues.MaxInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), null))
                        missingValues.Add(TaggedValues.MaxLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), null))
                        missingValues.Add(TaggedValues.MinExclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), null))
                        missingValues.Add(TaggedValues.MinInclusive.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), null))
                        missingValues.Add(TaggedValues.MinLength.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), null))
                        missingValues.Add(TaggedValues.TotalDigits.AsString());
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), null))
                        missingValues.Add(TaggedValues.WhiteSpace.AsString());
                    break;
                }
            return missingValues;
        }

        /// <summary>
        /// Add missing TaggedValues to Elements
        /// </summary>
        /// <param name="e">The Element to fix</param>
        /// 
        public void Fix(Element e)
        {
            switch (e.Stereotype)
            {
                case "ABIE": //finished!
                    goto default;
                case "ACC": //finished!
                    goto default;
                case "BBIE": //finished!
                    goto case "BCC";
                case "BCC": //finished!
                    if (Equals(e.GetTaggedValue(TaggedValues.SequencingKey), null))
                        e.SetTaggedValue(TaggedValues.SequencingKey, "");
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
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyIdentifier), null))
                        e.SetTaggedValue(TaggedValues.AgencyIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.AgencyName), null))
                        e.SetTaggedValue(TaggedValues.AgencyName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        e.SetTaggedValue(TaggedValues.BusinessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        e.SetTaggedValue(TaggedValues.LanguageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.UniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.VersionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.DictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.EnumerationURI), null))
                        e.SetTaggedValue(TaggedValues.EnumerationURI, "");
                    break;
                case "PRIM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        e.SetTaggedValue(TaggedValues.BusinessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        e.SetTaggedValue(TaggedValues.LanguageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.UniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.VersionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.DictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), null))
                        e.SetTaggedValue(TaggedValues.Definition, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.Pattern), null))
                        e.SetTaggedValue(TaggedValues.Pattern, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.FractionDigits), null))
                        e.SetTaggedValue(TaggedValues.FractionDigits, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.Length), null))
                        e.SetTaggedValue(TaggedValues.Length, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxExclusive), null))
                        e.SetTaggedValue(TaggedValues.MaxExclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxInclusive), null))
                        e.SetTaggedValue(TaggedValues.MaxInclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MaxLength), null))
                        e.SetTaggedValue(TaggedValues.MaxLength, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MinExclusive), null))
                        e.SetTaggedValue(TaggedValues.MinExclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MinInclusive), null))
                        e.SetTaggedValue(TaggedValues.MinInclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.MinLength), null))
                        e.SetTaggedValue(TaggedValues.MinLength, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.TotalDigits), null))
                        e.SetTaggedValue(TaggedValues.TotalDigits, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.WhiteSpace), null))
                        e.SetTaggedValue(TaggedValues.WhiteSpace, "");
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.ModificationAllowedIndicator), null))
                        e.SetTaggedValue(TaggedValues.ModificationAllowedIndicator, "");
                    goto default;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.BusinessTerm), null))
                        e.SetTaggedValue(TaggedValues.BusinessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.Definition), null))
                        e.SetTaggedValue(TaggedValues.Definition, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.DictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.LanguageCode), null))
                        e.SetTaggedValue(TaggedValues.LanguageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.UniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.VersionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.UsageRule), null))
                        e.SetTaggedValue(TaggedValues.UsageRule, "");
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
                case "ASBIE": //finisehd!
                    goto case "ASCC";
                case "ASCC": //finished!
                    if (Equals(c.GetTaggedValue(TaggedValues.SequencingKey), null))
                    {
                        c.SetTaggedValue(TaggedValues.SequencingKey, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.SequencingKey);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.BusinessTerm), null))
                    {
                        c.SetTaggedValue(TaggedValues.BusinessTerm, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.BusinessTerm);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.Definition), null))
                    {
                        c.SetTaggedValue(TaggedValues.Definition, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.Definition);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.DictionaryEntryName), null))
                    {
                        c.SetTaggedValue(TaggedValues.DictionaryEntryName, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.DictionaryEntryName);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.LanguageCode), null))
                    {
                        c.SetTaggedValue(TaggedValues.LanguageCode, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.LanguageCode);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
                    {
                        c.SetTaggedValue(TaggedValues.UniqueIdentifier, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.UniqueIdentifier);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.VersionIdentifier), null))
                    {
                        c.SetTaggedValue(TaggedValues.VersionIdentifier, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.VersionIdentifier);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.UsageRule), null))
                    {
                        c.SetTaggedValue(TaggedValues.UsageRule, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.UsageRule);
                    }
                    break;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.ApplyTo), null))
                        c.SetTaggedValue(TaggedValues.ApplyTo, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.Pattern), null))
                        c.SetTaggedValue(TaggedValues.Pattern, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.FractionDigits), null))
                        c.SetTaggedValue(TaggedValues.FractionDigits, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.Length), null))
                        c.SetTaggedValue(TaggedValues.Length, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxExclusive), null))
                        c.SetTaggedValue(TaggedValues.MaxExclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxInclusive), null))
                        c.SetTaggedValue(TaggedValues.MaxInclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MaxLength), null))
                        c.SetTaggedValue(TaggedValues.MaxLength, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MinExclusive), null))
                        c.SetTaggedValue(TaggedValues.MinExclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MinInclusive), null))
                        c.SetTaggedValue(TaggedValues.MinInclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.MinLength), null))
                        c.SetTaggedValue(TaggedValues.MinLength, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.TotalDigits), null))
                        c.SetTaggedValue(TaggedValues.TotalDigits, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.WhiteSpace), null))
                        c.SetTaggedValue(TaggedValues.WhiteSpace, "");
                    break;
            }
        }

        /// <summary>
        /// Fix a whole Package by first fixing the TaggedValues of the package itself and then its suppackages recursivley.
        /// </summary>
        /// <param name="p">The package to fix</param>
        public void Fix(Package p)
        {
            if (Equals(p.GetTaggedValue(TaggedValues.UniqueIdentifier), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.UniqueIdentifier.AsString());
                p.SetTaggedValue(TaggedValues.UniqueIdentifier, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.VersionIdentifier), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.VersionIdentifier.AsString());
                p.SetTaggedValue(TaggedValues.VersionIdentifier, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.BaseURN), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.BaseURN.AsString());
                p.SetTaggedValue(TaggedValues.BaseURN, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.BusinessTerm), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.BusinessTerm.AsString());
                p.SetTaggedValue(TaggedValues.BusinessTerm, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.Copyright), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Copyright.AsString());
                p.SetTaggedValue(TaggedValues.Copyright, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.Owner), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Owner.AsString());
                p.SetTaggedValue(TaggedValues.Owner, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.Reference), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Reference.AsString());
                p.SetTaggedValue(TaggedValues.Reference, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.Status), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.Status.AsString());
                p.SetTaggedValue(TaggedValues.Status, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.NamespacePrefix), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.NamespacePrefix.AsString());
                p.SetTaggedValue(TaggedValues.NamespacePrefix, "");
            }
            foreach (Element e in p.Elements)
            {
                Fix(e);
            }
            foreach (Package pp in p.Packages)
            {
                Fix(pp);
            }
            p.Update();
        }
    }
}