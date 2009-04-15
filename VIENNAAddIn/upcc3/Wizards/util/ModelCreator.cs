﻿using EA;
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
            // ADD NEW MODEL
            // add new model to the repository named "Model"
            Package model = (Package)repository.Models.AddNew("Model", "");
            model.Update();
            repository.Models.Refresh();

            // ADD VIEW TO THE MODEL
            // add a new view named "ebInterface Data Model" to the model created in 
            // previous step 
            // TODO: icon of the view is "Simple View" instead of "Class View"
            Package view = (Package)model.Packages.AddNew(modelName, "");
            view.Update();
            view.Element.Stereotype = "bLibrary";
            view.Update();

            // ADD PACKAGE DIAGRAM TO THE VIEW
            // add a new package diagram named "ebInterface Data Model" to the view created in 
            // previous step
            Diagram packages = (Diagram)view.Diagrams.AddNew(modelName, "Package");            
            packages.Update();

            // ADD PACKAGES TO THE VIEW
            // ADD LIBRARY PACKAGES TO THE PACKAGE DIAGRAM            
            CreateLibraryAndAttachToDiagram(packages, view, primLibraryName, "BDTLibrary");
            CreateLibraryAndAttachToDiagram(packages, view, enumLibraryName, "ENUMLibrary");
            CreateLibraryAndAttachToDiagram(packages, view, cdtLibraryName, "CDTLibrary");
            CreateLibraryAndAttachToDiagram(packages, view, ccLibraryName, "CCLibrary");
            CreateLibraryAndAttachToDiagram(packages, view, bdtLibraryName, "BDTLibrary");
            CreateLibraryAndAttachToDiagram(packages, view, bieLibraryName, "BIELibrary");
            CreateLibraryAndAttachToDiagram(packages, view, docLibraryName, "DOCLibrary");            
            
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
