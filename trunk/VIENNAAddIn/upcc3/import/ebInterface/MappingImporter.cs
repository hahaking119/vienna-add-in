using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class MappingImporter
    {
        private readonly ICcLibrary ccLibrary;
        private readonly IBLibrary bLibrary;
        private readonly string[] mapForceMappingFiles;
        private readonly string[] xmlSchemaFiles; 
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;
        private readonly string rootElementName;

        /// <summary>
        /// </summary>
        /// <param name="mapForceMappingFiles">The MapForce mapping file.</param>
        /// <param name="docLibraryName">The name of the DOCLibrary to be created.</param>
        /// <param name="bieLibraryName">The name of the BIELibrary to be created.</param>
        /// <param name="bdtLibraryName">The name of the BDTLibrary to be created.</param>
        /// <param name="qualifier">The qualifier for the business domain (e.g. "ebInterface").</param>
        public MappingImporter(IEnumerable<string> mapForceMappingFiles, IEnumerable<string> xmlSchemaFiles, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName)
        {
            this.mapForceMappingFiles = new List<string>(mapForceMappingFiles).ToArray();
            this.xmlSchemaFiles = new List<string>(xmlSchemaFiles).ToArray();
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
        }

        /// <summary>
        /// </summary>
        /// <param name="ccLibrary">The CC Library.</param>
        /// <param name="bLibrary">The bLibrary.</param>
        /// <param name="mapForceMappingFiles">The MapForce mapping file.</param>
        /// <param name="docLibraryName">The name of the DOCLibrary to be created.</param>
        /// <param name="bieLibraryName">The name of the BIELibrary to be created.</param>
        /// <param name="bdtLibraryName">The name of the BDTLibrary to be created.</param>
        /// <param name="qualifier">The qualifier for the business domain (e.g. "ebInterface").</param>
        public MappingImporter(ICcLibrary ccLibrary, IBLibrary bLibrary, IEnumerable<string> mapForceMappingFiles, IEnumerable<string> xmlSchemaFiles, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName)
        {
            this.ccLibrary = ccLibrary;
            this.bLibrary = bLibrary;
            this.mapForceMappingFiles = new List<string>(mapForceMappingFiles).ToArray();
            this.xmlSchemaFiles = new List<string>(xmlSchemaFiles).ToArray();
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
        }

        /// <summary>
        /// </summary>
        /// <param name="cctsRepository"></param>
        public void ImportMapping(ICctsRepository cctsRepository)
        {
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mapForceMappingFiles);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            foreach (string xmlSchema in xmlSchemaFiles)
            {
                xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xmlSchema), null));    
            }

            var mappings = new SchemaMapping(mapForceMapping, xmlSchemaSet, ccLibrary);
            var mappedLibraryGenerator = new MappedLibraryGenerator(mappings, bLibrary, docLibraryName, bieLibraryName, bdtLibraryName, qualifier, rootElementName);
            mappedLibraryGenerator.GenerateLibraries();
        }
    }
}