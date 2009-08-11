// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using EA;
using VIENNAAddIn.Settings;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class LibraryImporter
    {
        private readonly Repository repository;
        private readonly string[] resources;
        private readonly string downloadUri;
        private readonly string storageDirectory;

        ///<summary>
        /// The constructor of the ModelCreator which allows to specify details about the resources
        /// to be cashed. The details are specified through the parameters of the constructor.  
        ///</summary>
        public LibraryImporter(Repository eaRepository)
        {
            repository = eaRepository;
            resources = ResourceHandler.DefaultResources;
            downloadUri = ResourceHandler.DefaultDownloadUri;
            storageDirectory = ResourceHandler.DefaultStorageDirectory;
        }

        ///<summary>
        /// The constructor of the ModelCreator which allows to specify details about the resources
        /// to be cashed. The details are specified through the parameters of the constructor.  
        ///</summary>
        ///<param name="resources">
        /// An array of strings specifying the names of the resources to be retrieved (e.g. 
        /// "enumlibrary.xmi").
        /// </param>
        ///<param name="downloadUri">
        /// A string representing the URI where the resources are to be retrieved from. 
        ///</param>
        ///<param name="storageDirectory">
        /// A string representing a directory location where the resources retrieved from the
        /// URI are to be retrieved from. 
        ///</param>
        public LibraryImporter(Repository eaRepository, string[] resources, string downloadUri, string storageDirectory)
        {
            repository = eaRepository;
            this.resources = resources;
            this.downloadUri = downloadUri;
            this.storageDirectory = storageDirectory;
        }

        ///<summary>
        /// The method's purpose is to first retrieve the latest CC libraries from the web and 
        /// second import the downloaded libraries into an existing business library. The method
        /// therefore has one input parameter specifying the business library that the CC libraries
        /// are imported into. Realize that the method deletes all existing CC libraries (i.e. a PRIM
        /// library named "PRIMLibrary", an ENUM library named "ENUMLibrary", a CDT library named
        /// "CDTLibrary" and a CC library named "CCLibrary") from the business library passed as a 
        /// parameter. 
        ///</summary>
        ///<param name="bLibrary">
        /// Specifies the particular business library within the Repository where the standard 
        /// CC libraries should be imported into.
        ///</param>
        public void ImportStandardCcLibraries(Package bLibrary)
        {
            new ResourceHandler(resources, downloadUri, storageDirectory).CacheResourcesLocally();

            Project project = repository.GetProjectInterface();

            CleanUpUpccModel(repository, bLibrary);

            foreach (string xmiFile in resources)
            {
                project.ImportPackageXMI(bLibrary.PackageGUID, storageDirectory + xmiFile, 1, 0);
            }
        }

        ///<summary>
        /// The method operates on a business library (bLibrary) passed as a parameter and removes
        /// existing libraries within the bLibrary. The libraries being removed include all ENUM 
        /// libraries named "ENUMLibrary", all PRIM libraries named "PRIMLibrary", all CDT libraries 
        /// named "CDTLibrary", and all CC libraries named "CCLibrary".
        ///</summary>
        ///<param name="repository">
        /// The EA Repository which contains the bLibrary that needs be cleaned up. 
        ///</param>
        ///<param name="bLibrary">
        /// The bLibrary that the method operates on. 
        ///</param>                
        private void CleanUpUpccModel(Repository repository, Package bLibrary)
        {
            short index = 0;

            foreach (Package library in bLibrary.Packages)
            {
                if ((library.Name == "ENUMLibrary" && library.Element.Stereotype == Stereotype.ENUMLibrary)
                    || (library.Name == "PRIMLibrary" && library.Element.Stereotype == Stereotype.PRIMLibrary)
                    || (library.Name == "CDTLibrary" && library.Element.Stereotype == Stereotype.CDTLibrary)
                    || (library.Name == "CCLibrary" && library.Element.Stereotype == Stereotype.CCLibrary))
                {
                    bLibrary.Packages.Delete(index);
                }

                bLibrary.Update();
                index++;
            }

            repository.Models.Refresh();
            repository.RefreshModelView(bLibrary.PackageID);
        }
    }
}