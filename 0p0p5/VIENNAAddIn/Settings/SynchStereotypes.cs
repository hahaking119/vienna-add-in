/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.Settings
{
    /// <summary>
    /// This class should be used to check for missing TaggedValues of Elements/Connectors/Packages according to UPCC 3.0 spefication.
    /// Fix methods to add missing TaggedValues are also available.
    /// </summary>
    public class SynchStereotypes
    {
        private readonly Dictionary<string, List<TaggedValues>> taggedValues = new Dictionary<string, List<TaggedValues>>();

        ///<summary>
        ///</summary>
        public SynchStereotypes()
        {
            var defaultElementTaggedValues = new[]
                                             {
                                                 TaggedValues.businessTerm,
                                                 TaggedValues.definition,
                                                 TaggedValues.dictionaryEntryName,
                                                 TaggedValues.languageCode,
                                                 TaggedValues.uniqueIdentifier,
                                                 TaggedValues.versionIdentifier,
                                                 TaggedValues.usageRule,
                                             };

            taggedValues[Stereotype.BDT] = new List<TaggedValues>(defaultElementTaggedValues);
            taggedValues[Stereotype.ABIE] = new List<TaggedValues>(defaultElementTaggedValues);
            taggedValues[Stereotype.CDT] = new List<TaggedValues>(defaultElementTaggedValues);
            taggedValues[Stereotype.ACC] = new List<TaggedValues>(defaultElementTaggedValues);

            taggedValues[Stereotype.CON] = new List<TaggedValues>(defaultElementTaggedValues) {TaggedValues.modificationAllowedIndicator};
            taggedValues[Stereotype.SUP] = taggedValues[Stereotype.CON];

            taggedValues[Stereotype.BBIE] = new List<TaggedValues>(defaultElementTaggedValues) {TaggedValues.sequencingKey};
            taggedValues[Stereotype.BCC] = taggedValues[Stereotype.BBIE];

            taggedValues[Stereotype.ENUM] = new List<TaggedValues>
                                            {
                                                TaggedValues.agencyIdentifier,
                                                TaggedValues.agencyName,
                                                TaggedValues.businessTerm,
                                                TaggedValues.languageCode,
                                                TaggedValues.uniqueIdentifier,
                                                TaggedValues.versionIdentifier,
                                                TaggedValues.dictionaryEntryName,
                                                TaggedValues.enumerationURI,
                                            };

            taggedValues[Stereotype.PRIM] = new List<TaggedValues>
                                            {
                                                TaggedValues.businessTerm,
                                                TaggedValues.languageCode,
                                                TaggedValues.uniqueIdentifier,
                                                TaggedValues.versionIdentifier,
                                                TaggedValues.dictionaryEntryName,
                                                TaggedValues.definition,
                                                TaggedValues.pattern,
                                                TaggedValues.fractionDigits,
                                                TaggedValues.length,
                                                TaggedValues.maxExclusive,
                                                TaggedValues.maxInclusive,
                                                TaggedValues.maxLength,
                                                TaggedValues.minExclusive,
                                                TaggedValues.minInclusive,
                                                TaggedValues.minLength,
                                                TaggedValues.totalDigits,
                                                TaggedValues.whiteSpace,
                                            };

            taggedValues[Stereotype.ASCC] = new List<TaggedValues>
                                            {
                                                TaggedValues.sequencingKey,
                                                TaggedValues.businessTerm,
                                                TaggedValues.definition,
                                                TaggedValues.dictionaryEntryName,
                                                TaggedValues.languageCode,
                                                TaggedValues.uniqueIdentifier,
                                                TaggedValues.versionIdentifier,
                                                TaggedValues.usageRule,
                                            };
            taggedValues[Stereotype.ASBIE] = taggedValues[Stereotype.ASCC];
            taggedValues[Stereotype.BasedOn] = new List<TaggedValues>
                                               {
                                                   TaggedValues.applyTo,
                                                   TaggedValues.pattern,
                                                   TaggedValues.fractionDigits,
                                                   TaggedValues.length,
                                                   TaggedValues.maxExclusive,
                                                   TaggedValues.maxInclusive,
                                                   TaggedValues.maxLength,
                                                   TaggedValues.minExclusive,
                                                   TaggedValues.minInclusive,
                                                   TaggedValues.minLength,
                                                   TaggedValues.totalDigits,
                                                   TaggedValues.whiteSpace,
                                               };

            var libraryTaggedValues = new[]
                                      {
                                          TaggedValues.uniqueIdentifier,
                                          TaggedValues.versionIdentifier,
                                          TaggedValues.baseURN,
                                          TaggedValues.businessTerm,
                                          TaggedValues.copyright,
                                          TaggedValues.owner,
                                          TaggedValues.reference,
                                          TaggedValues.status,
                                          TaggedValues.namespacePrefix,
                                      };
            taggedValues[Stereotype.BDTLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.CDTLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.BIELibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.CCLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.DOCLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.PRIMLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.ENUMLibrary] = new List<TaggedValues>(libraryTaggedValues);
            taggedValues[Stereotype.bLibrary] = new List<TaggedValues>(libraryTaggedValues);
        }

        private IEnumerable<TaggedValues> GetTaggedValuesForStereotype(string stereotype)
        {
            List<TaggedValues> stereotypeTaggedValues;
            if (taggedValues.TryGetValue(stereotype, out stereotypeTaggedValues))
            {
                return stereotypeTaggedValues;
            }
            return new TaggedValues[0];
        }

        private IEnumerable<TaggedValues> GetMissingTaggedValues(Connector c)
        {
            foreach (TaggedValues taggedValue in GetTaggedValuesForStereotype(c.Stereotype))
            {
                if (!c.HasTaggedValue(taggedValue.ToString()))
                {
                    yield return taggedValue;
                }
            }
        }

        private IEnumerable<TaggedValues> GetMissingTaggedValues(Package p)
        {
            foreach (TaggedValues taggedValue in GetTaggedValuesForStereotype(p.Element.Stereotype))
            {
                if (!p.HasTaggedValue(taggedValue.ToString()))
                {
                    yield return taggedValue;
                }
            }
        }

        private IEnumerable<TaggedValues> GetMissingTaggedValues(Element e)
        {
            foreach (TaggedValues taggedValue in GetTaggedValuesForStereotype(e.Stereotype))
            {
                if (!e.HasTaggedValue(taggedValue.ToString()))
                {
                    yield return taggedValue;
                }
            }
        }

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
        /// Add missing TaggedValues to Elements
        /// </summary>
        /// <param name="e">The Element to fix</param>
        /// 
        public void Fix(Element e)
        {
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(e))
            {
                e.AddTaggedValue(missingTaggedValue.ToString());
            }
            e.TaggedValues.Refresh();
        }

        /// <summary>
        /// Add missing TaggedValues to Connectors
        /// </summary>
        /// <param name="c">Connector to fix</param>
        public void Fix(Connector c)
        {
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(c))
            {
                c.AddTaggedValue(missingTaggedValue.ToString());
            }
            c.TaggedValues.Refresh();
        }

        /// <summary>
        /// Fix a whole Package by first fixing the TaggedValues of the package itself and then its suppackages recursivley.
        /// </summary>
        /// <param name="p">The package to fix</param>
        public void Fix(Package p)
        {
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(p))
            {
                p.AddTaggedValue(missingTaggedValue.ToString());
            }
            p.Element.TaggedValues.Refresh();
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
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(p))
            {
                missingValues.Add(missingTaggedValue.ToString());
            }
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
            foreach (Package pp in p.Packages)
            {
                missingValuesLists.AddRange(Check(pp));
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
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(e))
            {
                missingValues.Add(missingTaggedValue.ToString());
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
            foreach (TaggedValues missingTaggedValue in GetMissingTaggedValues(c))
            {
                missingValues.Add(missingTaggedValue.ToString());
            }
            return missingValues;
        }
    }
}