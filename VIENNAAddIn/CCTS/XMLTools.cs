/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.common;
using VIENNAAddIn.constants; 


namespace VIENNAAddIn.CCTS {
    /// <summary>
    /// A set of methods useful for the XSD generation
    /// </summary>
    class XMLTools {



        //ommited and replace with method "getNameSpace" base on request that baseURN should be generate hierachically
        /// <summary>
        /// Get the targetnamespace for the given package
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //internal static String getNameSpaceForPackage(EA.Package p) {
        //    //Every package within a CCTS model must have a tagged value
        //    //baseURN
        //    //If that value is not present or empty an exception must be thrown
        //    //which aborts the schema generation process
            
        //    String baseURN = "";
        //    EA.TaggedValue tv = Utility.getTaggedValue(p, CCTS_TV.baseURN.ToString());
        //    if (tv == null || tv.Value == null || tv.Value == "")
        //        throw new XMLException("TaggedValue 'baseURN' not found. ", p);

        //    return tv.Value;
        //}


        internal static String getNameSpace(EA.Repository repository, EA.Package p) {

            string strBaseURN = getBaseURN(repository, p);
            if (strBaseURN == "")
                throw new Exception("baseURN tagged value of package " + "<<"+ p.Element.Stereotype +">>" + p.Name + " doesn't exist or empty. Please add it or fill it.");
            return getBaseURN(repository, p);
        }


        /// <summary>
        /// climb up every package, then concatenate every baseURN in each package until it reach topmost package
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string getBaseURN(EA.Repository repository, EA.Package p)
        {
            object objBaseURN;
            String baseURN = "";

            while ((p.ParentID != 0) )//&& (p.Element.Stereotype.ToString() != "Core Component Model"))
            {
                try
                {
                    //baseURN = getElementTVValue(CCTS_TV.baseURN, p.Element);

                    objBaseURN = p.Element.TaggedValues.GetByName(CCTS_TV.baseURN.ToString());
                    baseURN = ((EA.TaggedValue)objBaseURN).Value + baseURN;
                }
                catch (Exception ex) { }

                p = repository.GetPackageByID(p.ParentID);
            }

            return baseURN;
        }

        /// <summary>
        /// Get only baseURN of the package
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string getBaseURNofPackage(EA.Repository repository, EA.Package p)
        {
            object objBaseURN;
            String baseURN = "";

            try
            {
                objBaseURN = p.Element.TaggedValues.GetByName(CCTS_TV.baseURN.ToString());
                baseURN = ((EA.TaggedValue)objBaseURN).Value + baseURN;
            }
            catch (Exception ex) { }

            return baseURN;
        }

        /// <summary>
        /// Get the schemaversion for the given package
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static String getSchemaVersionFromPackage(EA.Package p) {
            //A package might

            String version = "";
            foreach (EA.TaggedValue tv in p.Element.TaggedValuesEx) {
                if (tv.Name.ToString().ToLower() == "version") {
                    version = tv.Value;
                    break;
                }
            }

            if (version == "")
                return "notSpecified";
            else
                return version;
        }



        /// <summary>
        /// Returns the namespace prefix
        /// a) if the tagged value NamespacePrefix is set in the package
        ///    the tagged value's string is returned
        /// b) if not the generic namespace prefix is returned
        /// </summary>
        /// <param name="p"></param>
        /// <param name="genericPrefix"></param>
        /// <returns></returns>
        internal static String getNameSpacePrefix(EA.Package p, String genericPrefix) {

            foreach (EA.TaggedValue tv in p.Element.TaggedValuesEx) {
                if (tv.Name.ToLower() == "namespaceprefix") {
                    return tv.Value;
                }
            }
            return genericPrefix;
        }


        /// <summary>
        /// Returns the save path for the XSD schema with UseAlias (package name alias)option
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static String getSavePathForSchema(EA.Package package, EA.Repository repository, bool blnUseAlias) {
            String p = "";
            bool skippedFirstLevel = false;
            

            while (package.ParentID != 0) {
                if (skippedFirstLevel) {
                    if (package.Element.Stereotype.ToString() != "Core Component Model") {
                        if (blnUseAlias && (package.Alias != ""))
                            p = package.Alias + "\\" + p;
                        else
                            p = package.Name + "\\" + p;
                    }
                    else {
                        break;
                    }
                }
                package = repository.GetPackageByID(package.ParentID);
                skippedFirstLevel = true;
            }
            p = p.Replace(":", "_");
            p = p.Replace(" ", "");
            return p;
        }


        /// <summary>
        /// Returns the save path for the XBRL schema
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static String getSavePathForSchema(EA.Package package, EA.Repository repository)
        {
            String p = "";
            bool skippedFirstLevel = false;

            while (package.ParentID != 0)
            {
                if (skippedFirstLevel)
                {
                    if (package.Element.Stereotype.ToString() != "Core Component Model")
                    {
                        p = package.Name + "\\" + p;
                    }
                    else
                    {
                        break;
                    }
                }
                package = repository.GetPackageByID(package.ParentID);
                skippedFirstLevel = true;
            }
            p = p.Replace(":", "_");
            p = p.Replace(" ", "");
            return p;
        }


        /// <summary>
        /// Get baseURL of the top most library
        /// If exist use absolute path else use relative path
        /// </summary>
        /// <param name="packageID">ID of selection package</param>
        /// <returns></returns>
        internal static string getBaseURL(EA.Repository repository, string packageID)
        {
            EA.Package package = repository.GetPackageByID(Int32.Parse(packageID));

            object objBaseURL;
            String baseURL = "";

            while ((package.ParentID != 0) && (package.Element.Stereotype.ToString() != "Core Component Model"))
            {
                try
                {
                    objBaseURL = package.Element.TaggedValues.GetByName(CCTS_TV.baseURL.ToString());
                    baseURL = ((EA.TaggedValue)objBaseURL).Value;
                    //return baseURL;
                }
                catch (Exception ex) { }

                package = repository.GetPackageByID(package.ParentID);
            }

            return baseURL;

        }


        /// <summary>
        /// Returns the import path for the schema
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static String getImportPathForSchema(EA.Package tobeImportedPackage, EA.Repository repository, String schemaName, String importerPackageID, bool blnUseAlias) {
            String p = "";
            int dUMM2y = tobeImportedPackage.PackageID;
            int importerPackageDepth = 0;
            int tobeImportedPackageDepth = 0;


            //Calculate the depth of the importerPackage
            EA.Package importerPackage = repository.GetPackageByID(Int32.Parse(importerPackageID));
            while (importerPackage.ParentID != 0)
            {
                int countChar = 0;
                if (importerPackage.Element != null) //&&
                //importerPackage.Element.Stereotype.ToString() != "Core Component Model") 
                {
                    if (blnUseAlias && (importerPackage.Alias != ""))
                    {
                        //see if there is any "/" or "\" in the alias name, which is indicating directory separator
                        //if yes, it will also determine how deep the package is
                        countChar += countDirectoryChar(importerPackage.Alias);
                        importerPackageDepth++;
                        importerPackageDepth += countChar;
                    }
                    else
                        importerPackageDepth++;
                }
                else
                    break;

                importerPackage = repository.GetPackageByID(importerPackage.ParentID);
            }

            //Calculate the depth of the tobeImportedPackage            
            while (tobeImportedPackage.ParentID != 0)
            {
                int countChar = 0;
                if (tobeImportedPackage.Element != null)
                //&& tobeImportedPackage.Element.Stereotype.ToString() != "Core Component Model") 
                {
                    if (blnUseAlias && (tobeImportedPackage.Alias != ""))
                    {
                        //see if there is any "/" or "\" in the alias name, which is indicating directory separator
                        //if yes, it will also determine how deep the package is
                        countChar += countDirectoryChar(tobeImportedPackage.Alias);
                        tobeImportedPackageDepth++;
                        tobeImportedPackageDepth += countChar;
                    }
                    else
                        tobeImportedPackageDepth++;

                }
                else
                    break;

                tobeImportedPackage = repository.GetPackageByID(tobeImportedPackage.ParentID);
            }

            //Set the tobeImportedPackage again, coz I has been overwritten
            tobeImportedPackage = repository.GetPackageByID(dUMM2y);


            //ImporterPackage is on a higher level than the one to be imported
            //First, add the steps until the top

            //Subtract one because the importer is not located on the very
            //botton
            //importerPackageDepth--;

            //string baseURLExist = getBaseURL(repository,importerPackageID);

            //if (baseURLExist == "")
            //{
            //    while (importerPackageDepth > 0)
            //    {
            //        p += "../";
            //        importerPackageDepth--;
            //    }
            //}
            //else
            //    p = baseURLExist + "/";


            //Then add as many packages as the depth of the tobeimportedpackage is deep
            String sub = "";

            bool skippedFirstLevel = false;
            while (tobeImportedPackageDepth > 0)
            {
                if (skippedFirstLevel)
                {
                    if (tobeImportedPackage.Element != null)  //&&
                    //tobeImportedPackage.Element.Stereotype != "Core Component Model") 
                    {
                        if (blnUseAlias && (tobeImportedPackage.Alias != ""))
                        {
                            string tempAlias = tobeImportedPackage.Alias;
                            //countChar += countDirectoryChar(tempAlias);
                            tempAlias = tempAlias.Replace(":", "_");
                            tempAlias = tempAlias.Replace(" ", "");
                            sub = tempAlias + "/" + sub;
                        }
                        else
                            sub = tobeImportedPackage.Name + "/" + sub;

                        tobeImportedPackageDepth--;
                    }
                    else
                    {
                        break;
                    }
                }
                tobeImportedPackage = repository.GetPackageByID(tobeImportedPackage.ParentID);
                skippedFirstLevel = true;
            }

            //Subtract one because the importer is not located on the very
            //botton
            importerPackageDepth--;
            //importerPackageDepth += countChar;

            string baseURLExist = getBaseURL(repository, importerPackageID);

            if (baseURLExist == "")
            {
                while (importerPackageDepth > 0)
                {
                    p += "../";
                    importerPackageDepth--;
                }
            }
            else
                p = baseURLExist + "/";

            p = p + sub;

            if (baseURLExist == "")
            {
                p = p.Replace(":", "_");
                p = p.Replace(" ", "");

                //tidy up the path coz the path could contain both "\" and "/"
                //if base URL exist, it means it URL path, should not contain any "\"
                //and vice versa
                p = p.Replace(@"\", "/");
            }
            else
                //relative path should not contain any "\"
                p.Replace("/", @"\");

            //Build the final string
            p = p + schemaName;

            return p;
        }

        private static int countDirectoryChar(string aliasName)
        {
            int result = 0;

            
            int prevPositionDirectoryChar = -1; 
            

            for (int i = 0; i < aliasName.Length; i++)
            {
                if ((aliasName[i].ToString() == @"\") || (aliasName[i].ToString() == @"/"))
                {
                    //to avoid consecutive "/" or  "\" 
                    //and first character of alias name is "/" or "\"
                    //and last character of alias name is "\" or "/"
                    if (((i - 1) != prevPositionDirectoryChar) && (i != aliasName.Length - 1))
                    {
                        result++;
                        prevPositionDirectoryChar = i;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the import path for the schema for XBRL generator and WSDL generator
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static String getImportPathForSchema(EA.Package tobeImportedPackage, EA.Repository repository, String schemaName, String importerPackageID)
        {
            String p = "";
            int dUMM2y = tobeImportedPackage.PackageID;
            int importerPackageDepth = 0;
            int tobeImportedPackageDepth = 0;

            //Calculate the depth of the importerPackage
            EA.Package importerPackage = repository.GetPackageByID(Int32.Parse(importerPackageID));
            while (importerPackage.ParentID != 0)
            {
                if (importerPackage.Element != null &&
                    importerPackage.Element.Stereotype.ToString() != "Core Component Model")
                {
                    importerPackageDepth++;
                }
                else
                {
                    break;
                }
                importerPackage = repository.GetPackageByID(importerPackage.ParentID);
            }

            //Calculate the depth of the tobeImportedPackage            
            while (tobeImportedPackage.ParentID != 0)
            {
                if (tobeImportedPackage.Element != null &&
                    tobeImportedPackage.Element.Stereotype.ToString() != "Core Component Model")
                {
                    tobeImportedPackageDepth++;
                }
                else
                {
                    break;
                }
                tobeImportedPackage = repository.GetPackageByID(tobeImportedPackage.ParentID);
            }

            //Set the tobeImportedPackage again, coz I has been overwritten
            tobeImportedPackage = repository.GetPackageByID(dUMM2y);


            //ImporterPackage is on a higher level than the one to be imported
            //First, add the steps until the top

            //Subtract one because the importer is not located on the very
            //botton
            importerPackageDepth--;

            string baseURLExist = getBaseURL(repository, importerPackageID);

            if (baseURLExist == "")
            {
                while (importerPackageDepth > 0)
                {
                    p += "../";
                    importerPackageDepth--;
                }
            }
            else
                p = baseURLExist + "/";


            //Then add as many packages as the depth of the tobeimportedpackage is deep
            String sub = "";
            bool skippedFirstLevel = false;
            while (tobeImportedPackageDepth > 0)
            {
                if (skippedFirstLevel)
                {
                    if (tobeImportedPackage.Element != null &&
                    tobeImportedPackage.Element.Stereotype != "Core Component Model")
                    {
                        sub = tobeImportedPackage.Name + "/" + sub;
                        tobeImportedPackageDepth--;
                    }
                    else
                    {
                        break;
                    }
                }
                tobeImportedPackage = repository.GetPackageByID(tobeImportedPackage.ParentID);
                skippedFirstLevel = true;
            }

            p = p + sub;

            if (baseURLExist == "")
            {
                p = p.Replace(":", "_");
                p = p.Replace(" ", "");
            }

            //Build the final string
            p = p + schemaName;

            return p;
        }


        /// <summary>
        /// Returns the name for the schema
        /// </summary>
        /// <returns></returns>
        internal static String getSchemaName(EA.Package package) {
            String name = package.Name.Replace(":", "_");
            name = name.Replace(" ", "");
            
            //string versionOfSchema = getElementTVValue("version", package.Element.TaggedValues);
            //string typeOfSchema = getElementTVValue("type", package.Element.TaggedValues);
            //string filetypeOfSchema = getElementTVValue("filetype", package.Element.TaggedValues);

            //name = name + "." + typeOfSchema + "." + versionOfSchema + "." + filetypeOfSchema + ".";

            return name + ".xsd";

        }






        /// <summary>
        /// Returns the value of the tagged value with the given name
        /// from an attribute
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static String getAttributeTVValue(CCTS_TV c, EA.Attribute attribute) {
            String name = c.ToString().ToLower();
            if (attribute.TaggedValues != null && attribute.TaggedValues.Count != 0) {
                foreach (EA.AttributeTag atag in attribute.TaggedValues) {
                    if (atag.Name.ToLower() == name) {
                        return atag.Value;
                    }
                }
            }
            return "";
        }




        /// <summary>
        /// Returns the value of the tagged value with the given name
        /// from an element
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static String getElementTVValue(CCTS_TV c, EA.Element element) {
            return getElementTVValue(c.ToString(), element);
        }

        internal static String getElementTVValue(string tv, EA.Element element)
        {
            var name = tv.ToLower();
            if (element.TaggedValues != null && element.TaggedValues.Count != 0)
            {
                foreach (EA.TaggedValue tag in element.TaggedValues)
                {
                    if (tag.Name.ToLower() == name)
                    {
                        return tag.Value;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Returns the value of the tagged value with the given name
        /// from an ASBIE
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static String getConnectorTVValue(CCTS_TV c, EA.Connector con)
        {
            String name = c.ToString().ToLower();
            if (con.TaggedValues != null && con.TaggedValues.Count != 0)
            {
                foreach (EA.ConnectorTag tag in con.TaggedValues)
                {
                    if (tag.Name.ToLower() == name)
                    {
                        return tag.Value;
                    }
                }
            }
            return "";
        }


        /// <summary>
        /// Replaces white spaces in the parameter n with _
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        internal static String getXMLName(String n) {
            if (n != null && n != "") {
                n = n.Replace(' ', '_');
                n = n.Replace('/', '_');
            }
            else
                return "";

            return n;
        }




        /// <summary>
        /// Returns true, if the passed element is already included in the passed
        /// schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        internal static bool isElementAlreadyIncludedInSchema(XmlSchema schema, String elementName) {

            foreach (XmlSchemaObject xmlObj in schema.Items) {
                if (xmlObj.GetType().Equals(typeof(XmlSchemaElement))) {
                    XmlSchemaElement xmlElem = (XmlSchemaElement)xmlObj;
                    if (xmlElem.Name == elementName)
                        return true;
                }
                else if (xmlObj.GetType().Equals(typeof(XmlSchemaComplexType))) {
                    XmlSchemaComplexType ct = (XmlSchemaComplexType)xmlObj;
                    if (ct.Name == elementName)
                        return true;
                }
            }
            return false;
        }





        /// <summary>
        /// Returns the XSD-Type for the passed EA Type
        /// </summary>
        /// <returns></returns>
        internal static String getXSDType(EA.Attribute a,  EA.Element e) {
            String determinedElementType = "";
            switch (a.Type.ToLower()) {
                case "string": determinedElementType = "string"; break;
                case "decimal": determinedElementType = "decimal"; break;
                case "binary": determinedElementType = "base64Binary"; break;
                case "base64binary": determinedElementType = "base64Binary"; break;
                case "token": determinedElementType = "token"; break;
                case "double": determinedElementType = "double"; break;
                case "integer": determinedElementType = "integer"; break;
                case "long": determinedElementType = "long"; break;
                case "datetime": determinedElementType = "dateTime"; break;
                case "date": determinedElementType = "date"; break;
                case "time": determinedElementType = "time"; break;
                case "boolean": determinedElementType = "boolean"; break;
                default: {
                        determinedElementType = "string";
                        //because no build in type was found we should present a warning to the user
                        throw new XMLException("No build in datatype found for the attribute " + a.Name + " in element "+e.Name+". Taking xsd:string instead.");
                        //break;
                    }
                    
            }
            return determinedElementType;
        }

        /// <summary>
        /// Returns the XSD-Type for the passed EA Type
        /// </summary>
        /// <returns></returns>
        internal static String getXBRLType(EA.Attribute a, EA.Element e)
        {
            String determinedElementType = "";
            switch (a.Type.ToLower())
            {
                case "string": determinedElementType = "stringItemType"; break;
                case "decimal": determinedElementType = "decimalItemType"; break;
                case "binary": determinedElementType = "base64BinaryItemType"; break;
                case "base64binary": determinedElementType = "base64BinaryItemType"; break;
                case "token": determinedElementType = "tokenItemType"; break;
                case "double": determinedElementType = "doubleItemType"; break;
                case "integer": determinedElementType = "integerItemType"; break;
                case "long": determinedElementType = "longItemType"; break;
                case "datetime": determinedElementType = "dateTimeItemType"; break;
                case "date": determinedElementType = "dateItemType"; break;
                case "time": determinedElementType = "timeItemType"; break;
                case "boolean": determinedElementType = "booleanItemType"; break;
                default:
                    {
                        determinedElementType = "stringItemType";
                        //because no build in type was found we should present a warning to the user
                        throw new XMLException("No build in datatype found for the attribute " + a.Name + " in element " + e.Name + ". Taking xbrli:stringItemType instead.");
                        //break;
                    }

            }
            return determinedElementType;
        }


        /// <summary>
        /// Get schema name without .xsd
        /// </summary>
        /// <param name="package">Package</param>
        /// <returns></returns>
        internal static String getSchemaNameWithoutExtention(EA.Package package)
        {
            String name = package.Name.Replace(":", "_");
            name = name.Replace(" ", "");
            return name;

        }

        /// <summary>
        /// Get XBRL name for attribute/bbie
        /// </summary>
        /// <param name="e"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        internal static string getXBRLBbieName(EA.Attribute attr, ArrayList name)
        {
            //pattern : ABIE.(ASBIE.ABIE)*.BBIE.Type
            string tempName = "";
            int counter = 0;
            foreach (string str in name)
            {
                counter++;
                if (counter < name.Count)
                    tempName += str + ".";
                else
                    tempName += str;
            }
            return tempName + "." + attr.Name + "." + attr.Type;
        }
        
        //this function is only temporary used Nov 20th 2007
        internal static string getXBRLBbieName(EA.Element e, EA.Attribute attr)
        {
            return e.Name + "." + attr.Name + "." + attr.Type;
        }

        internal static string getXBRLAsbieName(ArrayList name)
        {
            //pattern : ABIE.(ASBIE.ABIE)*.BBIE.Type
            string tempName = "";
            int counter = 0;
            foreach (string str in name)
            {
                counter++;
                if (counter < name.Count)
                    tempName += str + ".";
                else
                    tempName += str;
            }
            return tempName;
        }

        //internal static string getXBRLLinkbaseAsbieName(ArrayList name)
        //{
        //    string tempName = "";
        //    ArrayList truncName = new ArrayList();
        //    name.CopyTo(0, truncName, 0, name.Count - 2);
        //    int counter = 0;

        //    foreach (string str in truncName)
        //    {
        //        counter++;
        //        if (counter < name.Count)
        //            tempName += str + ".";
        //        else
        //            tempName += str;
        //    }
        //    return tempName;
        //}

        internal static string getSchemaVersion()
        {

            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();

            System.Reflection.AssemblyName an = a.GetName();
            Version v = an.Version;

            return "XML Schema produced by VIENNAAddIn Add-In version " + v;
        }

        /// <summary>
        /// Add version of current Add-In on the schema
        /// </summary>
        /// <param name="schema"></param>
        internal static void addVersionComment(XmlSchema schema)
        {
            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            XmlSchemaDocumentation documentation = new XmlSchemaDocumentation();
            XmlDocument helperDocument = new XmlDocument();
            XmlComment comment = helperDocument.CreateComment(getSchemaVersion());
            documentation.Markup = new XmlNode[1] { comment };
            annotation.Items.Add(documentation);
            schema.Items.Add(annotation);
        }

    }
}
