using System.IO;
using System.Net;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    ///</summary>
    public class ModelCreator
    {
// ReSharper disable FieldCanBeMadeReadOnly.Local
        private Repository repository;
// ReSharper restore FieldCanBeMadeReadOnly.Local

        ///<summary>
        ///</summary>
        ///<param name="eaRepository"></param>
        public ModelCreator(Repository eaRepository)
        {
            repository = eaRepository;
        }

        internal void CreateUpccModel(string modelName, string primLibraryName, string enumLibraryName, string cdtLibraryName, string ccLibraryName, string bdtLibraryName, string bieLibraryName, string docLibraryName)
        {
            repository.Models.Refresh();
            
            Package model = null;
            foreach (Package m in repository.Models)
            {
                if ((m.Packages.Count == 0) && (m.Diagrams.Count == 0))                    
                {
                    // we found an existing root model that does not contain any packages and diagrams
                    model = m;
                    break;
                }
            }

            if (model == null)
            {
                // ADD NEW MODEL
                // add new model to the repository named "Model"
                model = (Package)repository.Models.AddNew("Model", "");
                model.Update();
                repository.Models.Refresh();
            }
            
            CCRepository ccRepository = new CCRepository(repository);
            IBLibrary bLibrary = ccRepository.CreateBLibrary(new LibrarySpec {Name = modelName}, model);

            if (!primLibraryName.Equals(""))
            {
                bLibrary.CreatePRIMLibrary(new LibrarySpec {Name = primLibraryName});
            }
            if (!enumLibraryName.Equals(""))
            {
                bLibrary.CreateENUMLibrary(new LibrarySpec {Name = enumLibraryName});
            }
            if (!cdtLibraryName.Equals(""))
            {
                bLibrary.CreateCDTLibrary(new LibrarySpec {Name = cdtLibraryName});
            }
            if (!ccLibraryName.Equals(""))
            {
                bLibrary.CreateCCLibrary(new LibrarySpec {Name = ccLibraryName});
            }
            if (!bdtLibraryName.Equals(""))
            {
                bLibrary.CreateBDTLibrary(new LibrarySpec {Name = bdtLibraryName});
            }
            if (!bieLibraryName.Equals(""))
            {
                bLibrary.CreateBIELibrary(new LibrarySpec {Name = bieLibraryName});
            }
            if (!docLibraryName.Equals(""))
            {
                bLibrary.CreateDOCLibrary(new LibrarySpec {Name = docLibraryName});
            }      
            
            repository.RefreshModelView(0);

            repository.Models.Refresh();
        }

        ///<summary>
        ///</summary>
        ///<param name="fileUri"></param>
        ///<param name="fileName"></param>
        ///<returns></returns>
        public string RetrieveXmiFromUri(string fileUri, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(fileUri + fileName);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="directoryLocation"></param>
        ///<param name="fileName"></param>
        ///<param name="newXmiContent"></param>
        public void WriteXmiContentToFile(string directoryLocation, string fileName, string newXmiContent)
        {
            string xmiFile = directoryLocation + fileName;
            string currentXmiContent = "";

            if (System.IO.File.Exists(xmiFile))
            {
                currentXmiContent = System.IO.File.ReadAllText(xmiFile);
            }                      

            if (!string.IsNullOrEmpty(newXmiContent))
            {
                if (!currentXmiContent.Equals(newXmiContent))
                {
                    using (StreamWriter writer = System.IO.File.CreateText(directoryLocation + fileName))
                    {
                        writer.Write(newXmiContent);
                    }
                }
            }           
        }

        private static void CleanUpUpccModel(Package bLibrary)
        {
            short index = 0;

            foreach (Package library in bLibrary.Packages)
            {
                if ((library.Name == "ENUMLibrary" && library.Element.Stereotype == ccts.util.Stereotype.ENUMLibrary)
                    || (library.Name == "PRIMLibrary" && library.Element.Stereotype == ccts.util.Stereotype.PRIMLibrary)
                    || (library.Name == "CDTLibrary" && library.Element.Stereotype == ccts.util.Stereotype.CDTLibrary)
                    || (library.Name == "CCLibrary" && library.Element.Stereotype == ccts.util.Stereotype.CCLibrary))
                {

                    bLibrary.Packages.Delete(index);
                }

                bLibrary.Update();
                index++;
            }
        }

        private static void ImportLibraryFromXMIFile(Package library, Project project, string fileURI)
        {
            project.ImportPackageXMI(library.PackageGUID, fileURI, 1, 1);
        }

        internal void ImportStandardCcLibraries()
        {
            const string downloadUri = "http://www.umm-dev.org/xmi/";
            const string enumFile = "enumlibrary.xmi";
            const string primFile = "primlibrary.xmi";
            const string cdtFile = "cdtlibrary.xmi";
            const string ccFile = "cclibrary.xmi"; 
            string storageDirectory = AddInSettings.HomeDirectory + "upcc3\\resources\\xmi\\";            

            WriteXmiContentToFile(storageDirectory, enumFile, RetrieveXmiFromUri(downloadUri, enumFile));
            WriteXmiContentToFile(storageDirectory, primFile, RetrieveXmiFromUri(downloadUri, primFile));
            WriteXmiContentToFile(storageDirectory, cdtFile, RetrieveXmiFromUri(downloadUri, cdtFile));
            WriteXmiContentToFile(storageDirectory, ccFile, RetrieveXmiFromUri(downloadUri, ccFile));

            // Retrieve the GUID of the currenlty selected package in the tree view 
            // which is of stereotype "bLibrary". 
            string bLibraryGuid = repository.GetTreeSelectedPackage().Element.ElementGUID;
            Package bLibrary = repository.GetPackageByGuid(bLibraryGuid);
            Project project = repository.GetProjectInterface();

            // delete all existing standard CC libraries within the bLibrary
            CleanUpUpccModel(bLibrary);

            repository.Models.Refresh();
            repository.RefreshModelView(bLibrary.PackageID);

            // TODO: importing generates an error message that the add-in can't write the log file
            // can i maybe somehow disable the logging?          
            ImportLibraryFromXMIFile(bLibrary, project, storageDirectory + enumFile);
            ImportLibraryFromXMIFile(bLibrary, project, storageDirectory + primFile);
            ImportLibraryFromXMIFile(bLibrary, project, storageDirectory + cdtFile);
            ImportLibraryFromXMIFile(bLibrary, project, storageDirectory + ccFile);
        }
    }
}
