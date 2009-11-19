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
using CctsRepository;
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class XSDGenerator
    {
        ///<summary>
        ///</summary>
        public static GeneratorContext GenerateSchemas(GeneratorContext context)
        {
            BDTSchemaGenerator.GenerateXSD(context, CollectBDTs(context));
            BIESchemaGenerator.GenerateXSD(context, CollectBIEs(context));
            RootSchemaGenerator.GenerateXSD(context);

            if(context.Allschemas)
            {
                CDTSchemaGenerator.GenerateXSD(context, CollectCDTs(context));
                CCSchemaGenerator.GenerateXSD(context, CollectACCs(context));
            }

            if (!Directory.Exists(context.OutputDirectory))
            {
                Directory.CreateDirectory(context.OutputDirectory);
            }
            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            Indent = true,
                                            Encoding = Encoding.UTF8,
                                        };
                using (
                    var xmlWriter = XmlWriter.Create(context.OutputDirectory + "\\" + schemaInfo.FileName,
                                                     xmlWriterSettings))
                {
// ReSharper disable AssignNullToNotNullAttribute
                    schemaInfo.Schema.Write(xmlWriter);
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
                    xmlWriter.Close();
// ReSharper restore PossibleNullReferenceException
                }
            }

            if (context.Annotate)
            {
                try
                {
                    CopyFolder(AddInSettings.CommonXSDPath, context.OutputDirectory);
                }
                catch (IOException ioe)
                {
                }                
            }

            return context;
        }

        private static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IBDT> CollectBDTs(GeneratorContext context)
        {
            foreach (IBDTLibrary bdtLibrary in context.Repository.Libraries<IBDTLibrary>())
            {
                foreach (IBDT bdt in bdtLibrary.Elements)
                {
                    yield return bdt;
                }
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IBIE> CollectBIEs(GeneratorContext context)
        {
            foreach (IBIELibrary bieLibrary in context.Repository.Libraries<IBIELibrary>())
            {
                foreach (IABIE bie in bieLibrary.Elements)
                {
                    yield return bie;
                }
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IACC> CollectACCs(GeneratorContext context)
        {
            foreach (ICCLibrary ccLibrary in context.Repository.Libraries<ICCLibrary>())
            {
                foreach (IACC acc in ccLibrary.Elements)
                {
                    yield return acc;
                }
            }
        }


        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<ICDT> CollectCDTs(GeneratorContext context)
        {
            foreach (ICDTLibrary cdtLibrary in context.Repository.Libraries<ICDTLibrary>())
            {
                foreach (ICDT cdt in cdtLibrary.Elements)
                {
                    yield return cdt;
                }
            }
        }
    }
}