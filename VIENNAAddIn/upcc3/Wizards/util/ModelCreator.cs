using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;

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

            //            // ADD VIEW TO THE MODEL
//            // add a new view named "ebInterface Data Model" to the model created in 
//            // previous step 
//            // TODO: icon of the view is "Simple View" instead of "Class View"
//            Package view = (Package)model.Packages.AddNew(modelName, "");
//            view.Update();
//            view.Element.Stereotype = "bLibrary";
//            view.Update();
//
//            // ADD PACKAGE DIAGRAM TO THE VIEW
//            // add a new package diagram named "ebInterface Data Model" to the view created in 
//            // previous step
//            Diagram packages = (Diagram)view.Diagrams.AddNew(modelName, "Package");            
//            packages.Update();
//
//            // ADD PACKAGES TO THE VIEW
//            // ADD LIBRARY PACKAGES TO THE PACKAGE DIAGRAM            
//            CreateLibraryAndAttachToDiagram(packages, view, primLibraryName, "BDTLibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, enumLibraryName, "ENUMLibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, cdtLibraryName, "CDTLibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, ccLibraryName, "CCLibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, bdtLibraryName, "BDTLibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, bieLibraryName, "BIELibrary");
//            CreateLibraryAndAttachToDiagram(packages, view, docLibraryName, "DOCLibrary");            
            
            repository.RefreshModelView(0);

            repository.Models.Refresh();
        }

        internal void CreateLibraryAndAttachToDiagram(Diagram diagram, Package mother, string libName, string libStereotype)
        {
            if (!libName.Equals(""))
            {
                // ADD PACKAGE TO THE VIEW
                // add new package to the view
                Package libraryPackage = (Package)mother.Packages.AddNew(libName, "Package");
                libraryPackage.Update();
                libraryPackage.Element.Stereotype = libStereotype;
                libraryPackage.Update();

                // ADD TAGGED VALUES TO LIBRARY PACKAGE
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.BaseURN.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.BusinessTerm.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.Copyright.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.Reference.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.Status.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.UniqueIdentifier.ToString(), "");
                libraryPackage.Element.TaggedValues.AddNew(TaggedValues.VersionIdentifier.ToString(), "");
                libraryPackage.Element.TaggedValues.Refresh();

                // ADD DEFAULT CLASS DIAGRAM TO THE PACKAGE
                // per default add an empty class diagram to the package created above
                Diagram defaultDiagram = (Diagram)libraryPackage.Diagrams.AddNew(libName, "Class");
                defaultDiagram.Update();

                DiagramObject newDiagramObject = (DiagramObject)diagram.DiagramObjects.AddNew("", "");
                newDiagramObject.ElementID = libraryPackage.Element.ElementID;
                newDiagramObject.Update();                  
            }
        }
    }
}
