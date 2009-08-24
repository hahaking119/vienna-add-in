// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace VIENNAAddIn.upcc3.ccts.util
{
    /// <summary>
    /// Tagged value keys defined by UPCC.
    /// 
    /// The enum member names have been chosen to exactly match the UPCC tagged value definitions (rather than adhering to C# naming conventions).
    /// </summary>
    public enum TaggedValues
    {
#pragma warning disable 1591
        undefined,
        status,
        uniqueIdentifier,
        versionIdentifier,
        namespacePrefix,
        baseURN,
        businessTerm,
        copyright,
        owner,
        reference,
        definition,
        dictionaryEntryName,
        languageCode,
        usageRule,
        modificationAllowedIndicator,
        pattern,
        fractionDigits,
        length,
        maxExclusive,
        maxInclusive,
        maxLength,
        minExclusive,
        minInclusive,
        minLength,
        totalDigits,
        whiteSpace,
        applyTo,
        sequencingKey,
        agencyIdentifier,
        agencyName,
        enumerationURI,
#pragma warning restore 1591
    }
}