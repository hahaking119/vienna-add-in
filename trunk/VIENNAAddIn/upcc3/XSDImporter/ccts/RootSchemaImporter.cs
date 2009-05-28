using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ccts
{
    public class RootSchemaImporter
    {
        public static void ImportXSD(ImporterContext context)
        {
            ICCRepository repo = context.Repository;
            XmlSchema root = context.Root.Schema;
            
            int index = context.Root.FileName.IndexOf('_');
            String docname = context.Root.FileName.Substring(0, index);


            IBLibrary bLibrary = repo.Libraries<IBLibrary>().ElementAt(0);
            bLibrary.CreateDOCLibrary(new LibrarySpec { Name = docname });

            foreach (var currentElement in root.Items)
            {
                if (currentElement.GetType().Name == "XmlSchemaElement")
                {
                    XmlSchemaElement element = (XmlSchemaElement) currentElement;
                    Console.WriteLine("Elementqualifiedname: " + element.QualifiedName.ToString() + element.SchemaTypeName.Name.ToString());
                    Console.WriteLine(element);
                }

            }
            //throw new NotImplementedException();
        }
    }
}
