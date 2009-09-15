// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using EA;
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
        private readonly Repository repository;
        private readonly ResourceDescriptor resourceDescriptor;

        ///<summary>
        /// The constructor of the ModelCreator requires one input parameter specifying the EA 
        /// repository that the ModelCreator operates on. Furthermore, the constructor sets default
        /// values for the resources to be downloaded, the URI where the resources are to be retrieved
        /// from and the the storage directory where the downloaded resources are to be stored. 
        ///</summary>
        ///<param name="eaRepository">
        /// An EA repository representing the repository that the ModelCreator operates on. 
        /// </param>
        public ModelCreator(Repository eaRepository)
        {
            repository = eaRepository;
            resourceDescriptor = new ResourceDescriptor();
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
        public ModelCreator(Repository eaRepository, ResourceDescriptor resourceDescriptor)
        {
            repository = eaRepository;

            this.resourceDescriptor = new ResourceDescriptor(resourceDescriptor);
        }

        #region Class Methods

        ///<summary>
        /// The method creates a default model having the structure as specified in the UPCC 3 
        /// specification. The default structure includes per definition a PRIM library, an ENUM
        /// library, a CDT library, a CC library, a BDT library, a BIE library and a DOC library. 
        /// The names of the libraries created are specified through certain input paramaters of
        /// the method. However, the method is capable of creating selected libraries only. Deciding 
        /// whether a library is created or not is done based on the library name passed through the
        /// parameters. In case the library name for a particular library is empty then creating the
        /// library is omitted. Furthermore the method allows to specifiy whether in addition to 
        /// creating the default libraries all standard CC libraries containing default CC shall be
        /// imported. Realize that in case the method is instructed to import the default CC libraries 
        /// it opens an internet connection and retrieves the latest set of CC libraries. 
        ///</summary>
        ///<param name="modelName">
        /// A string representing the name of the root model to be created. 
        /// An example would be "ebInterface Data Model".</param>
        ///<param name="primLibraryName">
        /// A string representing the name of the PRIM library to be created. Passing an empty 
        /// string ("") to the method causes that creating the PRIM library is ommited. 
        ///</param>
        ///<param name="enumLibraryName">
        /// A string representing the name of the ENUM library to be created. Passing an empty 
        /// string ("") to the method causes that creating the ENUM library is ommited. 
        /// </param>
        ///<param name="cdtLibraryName">
        /// A string representing the name of the CDT library to be created. Passing an empty 
        /// string ("") to the method causes that creating the CDT library is ommited. 
        /// </param>
        ///<param name="ccLibraryName">
        /// A string representing the name of the CC library to be created. Passing an empty 
        /// string ("") to the method causes that creating the CC library is ommited. 
        /// </param>
        ///<param name="bdtLibraryName">
        /// A string representing the name of the BDT library to be created. Passing an empty 
        /// string ("") to the method causes that creating the BDT library is ommited. 
        /// </param>
        ///<param name="bieLibraryName">
        /// A string representing the name of the BIE library to be created. Passing an empty 
        /// string ("") to the method causes that creating the BIE library is ommited. 
        /// </param>
        ///<param name="docLibraryName">
        /// A string representing the name of the DOC library to be created. Passing an empty 
        /// string ("") to the method causes that creating the DOC library is ommited. 
        /// </param>
        ///<param name="importStandardLibraries">
        /// A boolean value indicating whether to import the standard CC libraries in addition 
        /// to creating a default model structure. Realize that setting the parameter to true
        /// causes the method to open an internet connection and retrieving the latest set of
        /// CC libraries. 
        ///</param>
        public void CreateUpccModel(string modelName, string primLibraryName, string enumLibraryName,
                                    string cdtLibraryName, string ccLibraryName, string bdtLibraryName,
                                    string bieLibraryName, string docLibraryName, bool importStandardLibraries)
        {
            repository.Models.Refresh();
            CCRepository ccRepository = new CCRepository(repository);

            Package model = GetEmptyRootModel();

            IBLibrary bLibrary = ccRepository.CreateBLibrary(new LibrarySpec {Name = modelName}, model);


            if (importStandardLibraries)
            {
                new LibraryImporter(repository, resourceDescriptor).ImportStandardCcLibraries(repository.GetPackageByID(bLibrary.Id));
            }
            else
            {
                CreateCCLibraries(bLibrary, primLibraryName, enumLibraryName, cdtLibraryName, ccLibraryName);
            }

            CreateBIELibraries(bLibrary, bdtLibraryName, bieLibraryName, docLibraryName);

            repository.RefreshModelView(0);
            repository.Models.Refresh();
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
            foreach (Package model in repository.Models)
            {
                // For each model check if it contains any Packages or Diagrams. If the current model
                // does not contain any Packages or Diagrams it is assumed that the model is empty.
                if ((model.Packages.Count == 0) && (model.Diagrams.Count == 0))
                {
                    return model;
                }
            }

            Package newEmptyModel = (Package) repository.Models.AddNew("Model", "");
            newEmptyModel.Update();
            repository.Models.Refresh();

            return newEmptyModel;
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
        private static void CreateBIELibraries(IBLibrary bLibrary, string bdtLibraryName, string bieLibraryName,
                                               string docLibraryName)
        {
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
        private static void CreateCCLibraries(IBLibrary bLibrary, string primLibraryName, string enumLibraryName,
                                              string cdtLibraryName, string ccLibraryName)
        {
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
        }

        #endregion
    }
}