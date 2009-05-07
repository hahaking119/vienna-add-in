﻿using System.IO;
using System.Net;
using EA;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    /// The ModelCreator class can be used to perform operations on an existing EA repository 
    /// such as adding a default model to the repository containing all libraries according to 
    /// UPCC3. Another example of the ModelCreator's capabilities includes the option to not only
    /// create a default model but furthermore import a standard set of CC libraries into the 
    /// (empty) libraries within the default model. 
    ///</summary>
    public class ModelCreator
    {
        #region Class Fields

        private readonly Repository repository;
        //        private string StorageDirectory = AddInSettings.HomeDirectory + "upcc3\\resources\\xmi\\";
        //        private string DownloadUri = "http://www.umm-dev.org/xmi/";
        //        private string[] Resources = new [] {"enumlibrary.xmi", "primlibrary.xmi", "cdtlibrary.xmi", "cclibrary.xmi"};

        #endregion

        ///<summary>
        ///</summary>
        ///<param name="eaRepository"></param>
        public ModelCreator(Repository eaRepository)
        {
            repository = eaRepository;
        }


        #region Class Methods

        ///<summary>
        /// The method creates a default model having the structure as specified in the UPCC 3 specification. 
        /// TODO: to be continued
        ///</summary>
        ///<param name="modelName"></param>
        ///<param name="primLibraryName"></param>
        ///<param name="enumLibraryName"></param>
        ///<param name="cdtLibraryName"></param>
        ///<param name="ccLibraryName"></param>
        ///<param name="bdtLibraryName"></param>
        ///<param name="bieLibraryName"></param>
        ///<param name="docLibraryName"></param>
        ///<param name="importStandardLibraries"></param>
        public void CreateUpccModel(string modelName, string primLibraryName, string enumLibraryName,
                                      string cdtLibraryName, string ccLibraryName, string bdtLibraryName,
                                      string bieLibraryName, string docLibraryName, bool importStandardLibraries)
        {

            // Before operating on the repository we need to create a CC repository since
            // the CC repository offers a lot of methods supporing the creation of new libraries
            // such as the bLibrary. 
            repository.Models.Refresh();
            CCRepository ccRepository = new CCRepository(repository);

            // Next, get an empty root model within the repository. If the repository doesn't contain
            // an empty root model or any model at all the method GetEmptyRootModel creates a new, 
            // empty, model for us. 
            Package model = GetEmptyRootModel();

            // After having an empty root model we can create a new bLibrary using the CCRepository's
            // CreateBLibrary method.
            IBLibrary bLibrary = ccRepository.CreateBLibrary(new LibrarySpec { Name = modelName }, model);

            if (importStandardLibraries)
            {
                ImportStandardCcLibraries(repository.GetPackageByID(bLibrary.Id));
            }
            else
            {
                CreateCCLibraries(bLibrary, primLibraryName, enumLibraryName, cdtLibraryName, ccLibraryName);
            }

            CreateBIELibraries(bLibrary, bdtLibraryName, bieLibraryName, docLibraryName);

            repository.RefreshModelView(0);
            repository.Models.Refresh();
        }

        public void ImportStandardCcLibraries(Package bLibrary)
        {
            string[] resources = new[] { "enumlibrary.xmi", "primlibrary.xmi", "cdtlibrary.xmi", "cclibrary.xmi" };
            string downloadUri = "http://www.umm-dev.org/xmi/";
            string storageDirectory = AddInSettings.HomeDirectory + "upcc3\\resources\\xmi\\";

            // As a first step it is necessary to cache the latest XMI files located
            // on the web to the local system. 
            new ResourceHandler(resources, downloadUri, storageDirectory).CacheResourcesLocally();

            // Secondly we need a project object which is needed to import XMI files
            // into the repository. 
            Project project = repository.GetProjectInterface();

            // Before adding the default libraries we need to remove all existing CC 
            // libraries from the bLibrary.             
            CleanUpUpccModel(bLibrary);

            // Finally we import the libraries contained in the XMI files to our repository. 
            foreach (string xmiFile in resources)
            {
                project.ImportPackageXMI(bLibrary.PackageGUID, storageDirectory + xmiFile, 1, 0);
            }
        }

        #endregion

        #region Internal Class Methods

        ///<summary>
        /// Iterates through the repository specified when initializing the ModelCreator instance using
        /// the constructor. In case the repository doesn't contain a root model which is empty or doesn't 
        /// contain any root model at all, an empty root model is added to the repository and returned through
        /// the return parameter. 
        ///</summary>
        /// <returns>
        ///  A package representing an empty root model in the repository which may be used to be 
        ///  populated with default CC libraries. 
        /// </returns>
        private Package GetEmptyRootModel()
        {
            // Iterate through all models in the Repository. 
            foreach (Package model in repository.Models)
            {
                // For each model check if it contains any Packages or Diagrams. If the current model
                // does not contain any Packages or Diagrams it is assumed that the model is empty.
                if ((model.Packages.Count == 0) && (model.Diagrams.Count == 0))
                {
                    // An empty model was found. Therefore return the empty model to the caller
                    // of the method.
                    return model;
                }
            }

            // If the method didn't find an empty model and return that model it is executing
            // the following statements. The statements add an empty model to the repository, 
            // udpate the repository.
            Package newEmptyModel = (Package)repository.Models.AddNew("Model", "");
            newEmptyModel.Update();
            repository.Models.Refresh();

            // Finally, the newly created, empty model, is returned to the caller of the method. 
            return newEmptyModel;
        }

        ///<summary>
        /// The method operates on a business library (bLibrary) passed as a parameter and removes
        /// existing libraries within the bLibrary. The libraries being removed include all ENUM 
        /// libraries named "ENUMLibrary", all PRIM libraries named "PRIMLibrary", all CDT libraries 
        /// named "CDTLibrary", and all CC libraries named "CCLibrary".
        ///</summary>
        ///<param name="bLibrary">
        /// The bLibrary that the method operates on. 
        /// </param>                
        private void CleanUpUpccModel(Package bLibrary)
        {
            short index = 0;

            // First we need to iterate through all the packages contained in the bLibrary
            foreach (Package library in bLibrary.Packages)
            {
                // If a package equals a particular stereotype and in addition has a particular name 
                // then it is removed from the bLibrary. 
                if ((library.Name == "ENUMLibrary" && library.Element.Stereotype == ccts.util.Stereotype.ENUMLibrary)
                    || (library.Name == "PRIMLibrary" && library.Element.Stereotype == ccts.util.Stereotype.PRIMLibrary)
                    || (library.Name == "CDTLibrary" && library.Element.Stereotype == ccts.util.Stereotype.CDTLibrary)
                    || (library.Name == "CCLibrary" && library.Element.Stereotype == ccts.util.Stereotype.CCLibrary))
                {
                    // A package was found that matches a particular stereotype and a particular name. It
                    // is therefore deleted from the bLibrary. 
                    bLibrary.Packages.Delete(index);
                }

                // The changes need to be updated in the repository. 
                bLibrary.Update();
                index++;
            }

            // Finally we need to write our changes to the model to the repository to make the changes
            // permanent.
            repository.Models.Refresh();
            repository.RefreshModelView(bLibrary.PackageID);
        }

        ///<summary>
        /// The method operates on a business library (bLibrary) passed as a parameter and adds all
        /// libraries on the BIE level. These libraries include a BDT library, a BIE library, and a
        /// DOC library. The names of the generated libraries are specified in the parameters of the
        /// method.         
        ///</summary>
        ///<param name="bLibrary">
        /// The bLibrary that the method operates on. 
        /// </param> 
        ///<param name="bdtLibraryName">
        /// The name of the BDT library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        ///<param name="bieLibraryName">
        /// The name of the BIE library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        ///<param name="docLibraryName">
        /// The name of the DOC library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        private static void CreateBIELibraries(IBLibrary bLibrary, string bdtLibraryName, string bieLibraryName, string docLibraryName)
        {
            if (!bdtLibraryName.Equals(""))
            {
                bLibrary.CreateBDTLibrary(new LibrarySpec { Name = bdtLibraryName });
            }
            if (!bieLibraryName.Equals(""))
            {
                bLibrary.CreateBIELibrary(new LibrarySpec { Name = bieLibraryName });
            }
            if (!docLibraryName.Equals(""))
            {
                bLibrary.CreateDOCLibrary(new LibrarySpec { Name = docLibraryName });
            }
        }

        ///<summary>
        /// The method operates on a business library (bLibrary) passed as a parameter and adds all 
        /// libraries on the CC level. These libraries include an ENUM library, a PRIM library, a 
        /// CDT library, and a CC library. The names of the generated libraries are specified in the 
        /// parameters of the method.       
        ///</summary>
        ///<param name="bLibrary">
        /// The bLibrary that the method operates on. 
        /// </param> 
        ///<param name="primLibraryName">
        /// The name of the PRIM library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        ///<param name="enumLibraryName">
        /// The name of the ENUM library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        ///<param name="cdtLibraryName">
        /// The name of the CDT library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted.  
        /// </param> 
        ///<param name="ccLibraryName">
        /// The name of the CC library created. If the parameter equals an empty string ("") then
        /// creating the library is omitted. 
        /// </param> 
        private static void CreateCCLibraries(IBLibrary bLibrary, string primLibraryName, string enumLibraryName, string cdtLibraryName, string ccLibraryName)
        {
            if (!primLibraryName.Equals(""))
            {
                bLibrary.CreatePRIMLibrary(new LibrarySpec { Name = primLibraryName });
            }
            if (!enumLibraryName.Equals(""))
            {
                bLibrary.CreateENUMLibrary(new LibrarySpec { Name = enumLibraryName });
            }
            if (!cdtLibraryName.Equals(""))
            {
                bLibrary.CreateCDTLibrary(new LibrarySpec { Name = cdtLibraryName });
            }
            if (!ccLibraryName.Equals(""))
            {
                bLibrary.CreateCCLibrary(new LibrarySpec { Name = ccLibraryName });
            }
        }

        #endregion
    }
}