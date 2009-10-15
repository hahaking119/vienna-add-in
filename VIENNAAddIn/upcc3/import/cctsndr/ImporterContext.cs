using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using Path=System.IO.Path;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    public interface IImporterContext
    {
        string RootSchemaPath { get; }
        string RootSchemaFileName { get; }
        XmlSchema BDTSchema { get; }
        string BIESchemaPath { get; }
        ICDTLibrary CDTLibrary { get; }
        ICCLibrary CCLibrary { get; }
        IBDTLibrary BDTLibrary { get; }
        IBIELibrary BIELibrary { get; }
        IPRIMLibrary PRIMLibrary { get; }
        IBLibrary BLibrary { get; }
    }

    public class ImporterContext : IImporterContext
    {
        /// <exception cref="Exception">Root schema does not include a BIE schema.</exception>
        public ImporterContext(ICCRepository ccRepository, string rootSchemaPath)
        {
            RootSchemaPath = rootSchemaPath;
            RootSchemaFileName = Path.GetFileName(rootSchemaPath);

            string inputDirectory = Path.GetDirectoryName(rootSchemaPath) + @"\";

            foreach (string schemaLocation in GetIncludedSchemaLocations(rootSchemaPath))
            {
                if (schemaLocation.StartsWith("BusinessInformationEntity"))
                {
                    BIESchemaPath = inputDirectory + schemaLocation;
                }
                else if (schemaLocation.StartsWith("BusinessDataType"))
                {
                    BDTSchemaPath = inputDirectory + schemaLocation;
                }
            }
            if (BDTSchemaPath == null)
            {
                throw new Exception("Root schema does not include a BDT schema.");
            }
            if (BIESchemaPath == null)
            {
                throw new Exception("Root schema does not include a BIE schema.");
            }

            CDTLibrary = ccRepository.Libraries<ICDTLibrary>().ElementAt(0);
            CCLibrary = ccRepository.Libraries<ICCLibrary>().ElementAt(0);
            BDTLibrary = ccRepository.Libraries<IBDTLibrary>().ElementAt(0);
            BIELibrary = ccRepository.Libraries<IBIELibrary>().ElementAt(0);
            PRIMLibrary = ccRepository.Libraries<IPRIMLibrary>().ElementAt(0);
            BLibrary = ccRepository.Libraries<IBLibrary>().ElementAt(0);
        }

        public string RootSchemaPath { get; private set; }

        public string RootSchemaFileName { get; private set; }

        private string BDTSchemaPath { get; set; }

        public XmlSchema BDTSchema
        {
            get { return XmlSchema.Read(XmlReader.Create(BDTSchemaPath), null); }
        }

        public string BIESchemaPath { get; private set; }

        public ICDTLibrary CDTLibrary { get; private set; }
        
        public ICCLibrary CCLibrary { get; private set; }

        public IBDTLibrary BDTLibrary { get; private set; }

        public IBIELibrary BIELibrary { get; private set; }

        public IPRIMLibrary PRIMLibrary { get; private set; }

        public IBLibrary BLibrary { get; private set; }

        private static IEnumerable<string> GetIncludedSchemaLocations(string rootSchemaPath)
        {
            XmlSchema rootSchema = XmlSchema.Read(XmlReader.Create(rootSchemaPath), null);
            foreach (XmlSchemaObject schemaObject in rootSchema.Includes)
            {
                if (schemaObject is XmlSchemaInclude)
                {
                    yield return ((XmlSchemaInclude) schemaObject).SchemaLocation;
                }
            }
        }
    }
}