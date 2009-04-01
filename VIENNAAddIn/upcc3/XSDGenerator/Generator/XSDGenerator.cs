// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    ///<summary>
    ///</summary>
    public class XSDGenerator
    {
        ///<summary>
        ///</summary>
        ///<param name="ccRepository"></param>
        ///<param name="docLibrary"></param>
        ///<param name="targetNamespace"></param>
        ///<param name="namespacePrefix"></param>
        public static void GenerateSchemas(ICCRepository ccRepository, IDOCLibrary docLibrary, string targetNamespace,
                                           string namespacePrefix, bool annotate, string outputDirectory)
        {
            var context = new GenerationContext(ccRepository, targetNamespace, namespacePrefix, annotate);
            BDTSchemaGenerator.GenerateXSD(context, CollectBDTs(context, docLibrary));
            BIESchemaGenerator.GenerateXSD(context, CollectBIEs(context, docLibrary));
            RootSchemaGenerator.GenerateXSD(context, docLibrary);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            Indent = true,
                                            //                               NewLineOnAttributes = true,
                                            Encoding = Encoding.UTF8,
                                        };
// ReSharper disable AssignNullToNotNullAttribute
                schemaInfo.Schema.Write(XmlWriter.Create(outputDirectory + "\\" + schemaInfo.FileName, xmlWriterSettings));
// ReSharper restore AssignNullToNotNullAttribute
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="docLibrary"></param>
        ///<returns></returns>
        public static IEnumerable<IBDT> CollectBDTs(GenerationContext context, IDOCLibrary docLibrary)
        {
            foreach (IBDTLibrary bdtLibrary in context.Repository.Libraries<IBDTLibrary>())
            {
                foreach (IBDT bdt in bdtLibrary.BDTs)
                {
                    yield return bdt;
                }
            }
        }

        public static IEnumerable<IBIE> CollectBIEs(GenerationContext context, IDOCLibrary docLibrary)
        {
            foreach (IBIELibrary bieLibrary in context.Repository.Libraries<IBIELibrary>())
            {
                foreach (IABIE bie in bieLibrary.BIEs)
                {
                    yield return bie;
                }
            }
        }
    }
}