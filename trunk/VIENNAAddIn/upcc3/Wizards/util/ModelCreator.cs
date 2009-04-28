using System.IO;
using System.Net;
using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    internal class ModelCreator
    {
        private Repository repository;

        internal ModelCreator(Repository eaRepository)
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

        private static void RetrieveAndStoreXmiFromUri(string fileName, string directoryLocation, string fileUri)
        {
            using (StreamWriter writer = System.IO.File.CreateText(directoryLocation + fileName))
            {
                using (WebClient client = new WebClient())
                {
                    string xmiContent = client.DownloadString(fileUri + fileName);

                    if (!string.IsNullOrEmpty(xmiContent))
                    {
                        writer.Write(xmiContent);
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
            string storageDirectory = "C:\\Temp\\retrieval\\";
            string downloadUri = "http://www.umm-dev.org/xmi/";
            string enumFile = "enumlibrary.xmi";
            string primFile = "primlibrary.xmi";
            string cdtFile = "cdtlibrary.xmi";
            string ccFile = "cclibrary.xmi";


            RetrieveAndStoreXmiFromUri(enumFile, storageDirectory, downloadUri);
            RetrieveAndStoreXmiFromUri(primFile, storageDirectory, downloadUri);
            RetrieveAndStoreXmiFromUri(cdtFile, storageDirectory, downloadUri);
            RetrieveAndStoreXmiFromUri(ccFile, storageDirectory, downloadUri);

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
