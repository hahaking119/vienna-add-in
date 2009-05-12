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
            if (Equals(p.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                missingValues.Add(TaggedValues.uniqueIdentifier.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.versionIdentifier), null))
                missingValues.Add(TaggedValues.versionIdentifier.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.baseURN), null))
                missingValues.Add(TaggedValues.baseURN.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.businessTerm), null))
                missingValues.Add(TaggedValues.businessTerm.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.copyright), null))
                missingValues.Add(TaggedValues.copyright.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.owner), null))
                missingValues.Add(TaggedValues.owner.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.reference), null))
                missingValues.Add(TaggedValues.reference.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.status), null))
                missingValues.Add(TaggedValues.status.ToString());
            if (Equals(p.GetTaggedValue(TaggedValues.namespacePrefix), null))
                missingValues.Add(TaggedValues.namespacePrefix.ToString());
            missingValues.Insert(0, p.Name);
            missingValuesLists.Add(missingValues);

            // check TaggedValues of current package's elements
            foreach (Element e in p.Elements)
            {
                missingValues = Check(e);
                missingValues.Insert(0, e.Name);
                missingValuesLists.Add(missingValues);
            }

            // check TaggedValues of sub-packages recursively
            List<List<string>> missingValuesL;
            foreach (Package pp in p.Packages)
            {
                missingValuesL = Check(pp);
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
                    if (Equals(e.GetTaggedValue(TaggedValues.sequencingKey), null))
                        missingValues.Add(TaggedValues.sequencingKey.ToString());
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
                    if (Equals(e.GetTaggedValue(TaggedValues.agencyIdentifier), null))
                        missingValues.Add(TaggedValues.agencyIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.agencyName), null))
                        missingValues.Add(TaggedValues.agencyName.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        missingValues.Add(TaggedValues.businessTerm.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        missingValues.Add(TaggedValues.languageCode.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        missingValues.Add(TaggedValues.uniqueIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        missingValues.Add(TaggedValues.versionIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        missingValues.Add(TaggedValues.dictionaryEntryName.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.enumerationURI), null))
                        missingValues.Add(TaggedValues.enumerationURI.ToString());
                    break;
                case "PRIM": //finished
                    Debug.WriteLine("Checking PRIM.");
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        missingValues.Add(TaggedValues.businessTerm.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        missingValues.Add(TaggedValues.languageCode.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        missingValues.Add(TaggedValues.uniqueIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        missingValues.Add(TaggedValues.versionIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        missingValues.Add(TaggedValues.dictionaryEntryName.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.definition), null))
                        missingValues.Add(TaggedValues.definition.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.pattern), null))
                        missingValues.Add(TaggedValues.pattern.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.fractionDigits), null))
                        missingValues.Add(TaggedValues.fractionDigits.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.length), null))
                        missingValues.Add(TaggedValues.length.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.maxExclusive), null))
                        missingValues.Add(TaggedValues.maxExclusive.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.maxInclusive), null))
                        missingValues.Add(TaggedValues.maxInclusive.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.maxLength), null))
                        missingValues.Add(TaggedValues.maxLength.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.minExclusive), null))
                        missingValues.Add(TaggedValues.minExclusive.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.minInclusive), null))
                        missingValues.Add(TaggedValues.minInclusive.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.minLength), null))
                        missingValues.Add(TaggedValues.minLength.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.totalDigits), null))
                        missingValues.Add(TaggedValues.totalDigits.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.whiteSpace), null))
                        missingValues.Add(TaggedValues.whiteSpace.ToString());
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    Debug.WriteLine("Checking SUP.");
                    if (Equals(e.GetTaggedValue(TaggedValues.modificationAllowedIndicator), null))
                        missingValues.Add(TaggedValues.modificationAllowedIndicator.ToString());
                    goto default;
                case "BDT": //finished
                    Debug.WriteLine("Checking BDT.");
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        missingValues.Add(TaggedValues.businessTerm.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.definition), null))
                        missingValues.Add(TaggedValues.definition.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        missingValues.Add(TaggedValues.dictionaryEntryName.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        missingValues.Add(TaggedValues.languageCode.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        missingValues.Add(TaggedValues.uniqueIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        missingValues.Add(TaggedValues.versionIdentifier.ToString());
                    if (Equals(e.GetTaggedValue(TaggedValues.usageRule), null))
                        missingValues.Add(TaggedValues.usageRule.ToString());
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
                    if (Equals(c.GetTaggedValue(TaggedValues.sequencingKey), null))
                    {
                        Debug.WriteLine("Found Missing SequencingKey");
                        missingValues.Add(TaggedValues.sequencingKey.ToString());
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.businessTerm), null))
                    {
                        missingValues.Add(TaggedValues.businessTerm.ToString());
                        Debug.WriteLine("Found Missing BusinessTerm");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.definition), null))
                    {
                        missingValues.Add(TaggedValues.definition.ToString());
                        Debug.WriteLine("Found Missing Definition");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                    {
                        missingValues.Add(TaggedValues.dictionaryEntryName.ToString());
                        Debug.WriteLine("Found Missing DictionaryEntryName");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.languageCode), null))
                    {
                        missingValues.Add(TaggedValues.languageCode.ToString());
                        Debug.WriteLine("Found Missing LanguageCode");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                    {
                        missingValues.Add(TaggedValues.uniqueIdentifier.ToString());
                        Debug.WriteLine("Found Missing UniqueIdentifier");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.versionIdentifier), null))
                    {
                        missingValues.Add(TaggedValues.versionIdentifier.ToString());
                        Debug.WriteLine("Found Missing VersionIdentifier");
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.usageRule), null))
                    {
                        missingValues.Add(TaggedValues.usageRule.ToString());
                        Debug.WriteLine("Found Missing UsageRule");
                    }
                    break;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.applyTo), null))
                        missingValues.Add(TaggedValues.applyTo.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.pattern), null))
                        missingValues.Add(TaggedValues.pattern.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.fractionDigits), null))
                        missingValues.Add(TaggedValues.fractionDigits.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.length), null))
                        missingValues.Add(TaggedValues.length.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.maxExclusive), null))
                        missingValues.Add(TaggedValues.maxExclusive.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.maxInclusive), null))
                        missingValues.Add(TaggedValues.maxInclusive.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.maxLength), null))
                        missingValues.Add(TaggedValues.maxLength.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.minExclusive), null))
                        missingValues.Add(TaggedValues.minExclusive.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.minInclusive), null))
                        missingValues.Add(TaggedValues.minInclusive.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.minLength), null))
                        missingValues.Add(TaggedValues.minLength.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.totalDigits), null))
                        missingValues.Add(TaggedValues.totalDigits.ToString());
                    if (Equals(c.GetTaggedValue(TaggedValues.whiteSpace), null))
                        missingValues.Add(TaggedValues.whiteSpace.ToString());
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
                    if (Equals(e.GetTaggedValue(TaggedValues.sequencingKey), null))
                        e.SetTaggedValue(TaggedValues.sequencingKey, "");
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
                    if (Equals(e.GetTaggedValue(TaggedValues.agencyIdentifier), null))
                        e.SetTaggedValue(TaggedValues.agencyIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.agencyName), null))
                        e.SetTaggedValue(TaggedValues.agencyName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        e.SetTaggedValue(TaggedValues.businessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        e.SetTaggedValue(TaggedValues.languageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.uniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.versionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.dictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.enumerationURI), null))
                        e.SetTaggedValue(TaggedValues.enumerationURI, "");
                    break;
                case "PRIM": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        e.SetTaggedValue(TaggedValues.businessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        e.SetTaggedValue(TaggedValues.languageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.uniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.versionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.dictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.definition), null))
                        e.SetTaggedValue(TaggedValues.definition, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.pattern), null))
                        e.SetTaggedValue(TaggedValues.pattern, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.fractionDigits), null))
                        e.SetTaggedValue(TaggedValues.fractionDigits, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.length), null))
                        e.SetTaggedValue(TaggedValues.length, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.maxExclusive), null))
                        e.SetTaggedValue(TaggedValues.maxExclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.maxInclusive), null))
                        e.SetTaggedValue(TaggedValues.maxInclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.maxLength), null))
                        e.SetTaggedValue(TaggedValues.maxLength, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.minExclusive), null))
                        e.SetTaggedValue(TaggedValues.minExclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.minInclusive), null))
                        e.SetTaggedValue(TaggedValues.minInclusive, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.minLength), null))
                        e.SetTaggedValue(TaggedValues.minLength, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.totalDigits), null))
                        e.SetTaggedValue(TaggedValues.totalDigits, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.whiteSpace), null))
                        e.SetTaggedValue(TaggedValues.whiteSpace, "");
                    break;
                case "QDT":
                    //not found yet!
                    break;
                case "SUP": //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.modificationAllowedIndicator), null))
                        e.SetTaggedValue(TaggedValues.modificationAllowedIndicator, "");
                    goto default;
                case "BDT": //finished
                    goto default;
                default: //finished
                    if (Equals(e.GetTaggedValue(TaggedValues.businessTerm), null))
                        e.SetTaggedValue(TaggedValues.businessTerm, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.definition), null))
                        e.SetTaggedValue(TaggedValues.definition, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                        e.SetTaggedValue(TaggedValues.dictionaryEntryName, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.languageCode), null))
                        e.SetTaggedValue(TaggedValues.languageCode, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                        e.SetTaggedValue(TaggedValues.uniqueIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.versionIdentifier), null))
                        e.SetTaggedValue(TaggedValues.versionIdentifier, "");
                    if (Equals(e.GetTaggedValue(TaggedValues.usageRule), null))
                        e.SetTaggedValue(TaggedValues.usageRule, "");
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
                    if (Equals(c.GetTaggedValue(TaggedValues.sequencingKey), null))
                    {
                        c.SetTaggedValue(TaggedValues.sequencingKey, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.sequencingKey);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.businessTerm), null))
                    {
                        c.SetTaggedValue(TaggedValues.businessTerm, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.businessTerm);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.definition), null))
                    {
                        c.SetTaggedValue(TaggedValues.definition, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.definition);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.dictionaryEntryName), null))
                    {
                        c.SetTaggedValue(TaggedValues.dictionaryEntryName, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.dictionaryEntryName);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.languageCode), null))
                    {
                        c.SetTaggedValue(TaggedValues.languageCode, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.languageCode);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
                    {
                        c.SetTaggedValue(TaggedValues.uniqueIdentifier, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.uniqueIdentifier);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.versionIdentifier), null))
                    {
                        c.SetTaggedValue(TaggedValues.versionIdentifier, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.versionIdentifier);
                    }
                    if (Equals(c.GetTaggedValue(TaggedValues.usageRule), null))
                    {
                        c.SetTaggedValue(TaggedValues.usageRule, "");
                        Debug.WriteLine("Added new TaggedValue: " + TaggedValues.usageRule);
                    }
                    break;
                case "basedOn": //finished
                    if (Equals(c.GetTaggedValue(TaggedValues.applyTo), null))
                        c.SetTaggedValue(TaggedValues.applyTo, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.pattern), null))
                        c.SetTaggedValue(TaggedValues.pattern, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.fractionDigits), null))
                        c.SetTaggedValue(TaggedValues.fractionDigits, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.length), null))
                        c.SetTaggedValue(TaggedValues.length, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.maxExclusive), null))
                        c.SetTaggedValue(TaggedValues.maxExclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.maxInclusive), null))
                        c.SetTaggedValue(TaggedValues.maxInclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.maxLength), null))
                        c.SetTaggedValue(TaggedValues.maxLength, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.minExclusive), null))
                        c.SetTaggedValue(TaggedValues.minExclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.minInclusive), null))
                        c.SetTaggedValue(TaggedValues.minInclusive, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.minLength), null))
                        c.SetTaggedValue(TaggedValues.minLength, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.totalDigits), null))
                        c.SetTaggedValue(TaggedValues.totalDigits, "");
                    if (Equals(c.GetTaggedValue(TaggedValues.whiteSpace), null))
                        c.SetTaggedValue(TaggedValues.whiteSpace, "");
                    break;
            }
        }

        /// <summary>
        /// Fix a whole Package by first fixing the TaggedValues of the package itself and then its suppackages recursivley.
        /// </summary>
        /// <param name="p">The package to fix</param>
        public void Fix(Package p)
        {
            if (Equals(p.GetTaggedValue(TaggedValues.uniqueIdentifier), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.uniqueIdentifier.ToString());
                p.SetTaggedValue(TaggedValues.uniqueIdentifier, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.versionIdentifier), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.versionIdentifier.ToString());
                p.SetTaggedValue(TaggedValues.versionIdentifier, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.baseURN), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.baseURN.ToString());
                p.SetTaggedValue(TaggedValues.baseURN, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.businessTerm), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.businessTerm.ToString());
                p.SetTaggedValue(TaggedValues.businessTerm, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.copyright), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.copyright.ToString());
                p.SetTaggedValue(TaggedValues.copyright, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.owner), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.owner.ToString());
                p.SetTaggedValue(TaggedValues.owner, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.reference), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.reference.ToString());
                p.SetTaggedValue(TaggedValues.reference, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.status), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.status.ToString());
                p.SetTaggedValue(TaggedValues.status, "");
            }
            if (Equals(p.GetTaggedValue(TaggedValues.namespacePrefix), null))
            {
                Debug.WriteLine("Added TaggedValue: " + TaggedValues.namespacePrefix.ToString());
                p.SetTaggedValue(TaggedValues.namespacePrefix, "");
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