// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
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
        public static void GenerateSchemas(ICCRepository ccRepository, IDOCLibrary docLibrary, string targetNamespace, string namespacePrefix, bool annotate)
        {
            var context = new GenerationContext(ccRepository, targetNamespace, namespacePrefix, annotate);
            BDTSchemaGenerator.GenerateXSD(context, CollectBDTs(context, docLibrary));
            BIESchemaGenerator.GenerateXSD(context, CollectBIEs(context, docLibrary));
            RootSchemaGenerator.GenerateXSD(context, docLibrary);
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="docLibrary"></param>
        ///<returns></returns>
        public static IEnumerable<IBDT> CollectBDTs(GenerationContext context, IDOCLibrary docLibrary)
        {
            foreach (var bdtLibrary in context.Repository.Libraries<IBDTLibrary>())
            {
                foreach (var bdt in bdtLibrary.BDTs)
                {
                    yield return bdt;
                }
            }
        }

        private static IEnumerable<IBIE> CollectBIEs(GenerationContext context, IDOCLibrary docLibrary)
        {
            yield break;
        }
    }
}