using System;
using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ModelCreatorTest
    {
        [Test]
        public void TestXmiRetrieval()
        {
            Repository repository = new EARepositoryModelCreator();
            ModelCreator creator = new ModelCreator(repository);
            string fileUri = "http://www.umm-dev.org/xmi/";
            string fileName = "primlibrary.xmi";
                    
            string xmiContent = creator.RetrieveXmiFromUri(fileUri, fileName);

            Assert.AreNotEqual(xmiContent, "");        
        }

        [Test]
        public void TestXmiRetrievalAndWriteToFileSystem()
        {
            Repository repository = new EARepositoryModelCreator();
            ModelCreator creator = new ModelCreator(repository);
            string fileUri = "http://www.umm-dev.org/xmi/";
            string fileName = "primlibrary.xmi";
            string directoryLocation = Directory.GetCurrentDirectory() + "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\";

            
            string xmiContent = creator.RetrieveXmiFromUri(fileUri, fileName);
            Assert.AreNotEqual(xmiContent, "");

            creator.WriteXmiContentToFile(directoryLocation, fileName, xmiContent);
            AssertXmi(directoryLocation + fileName, xmiContent);
        }

        private static void AssertXmi(string currentXmiFileLocation, string newXmiContent)
        {
            string currentXmiContent = System.IO.File.ReadAllText(currentXmiFileLocation);

            if (!currentXmiContent.Equals(newXmiContent))
            {
                Assert.Fail("XMI file couldn't be updated at: {0}", currentXmiFileLocation);
            }
        }
        








        private Repository GetFileBasedRepository(string filePath, string fileName)
        {
            string sourceRepositoryFile = filePath + fileName;
            string backupRepositoryFile = filePath + fileName.Remove(fileName.Length - 4, 4) + "_tmp.eap";
            System.IO.File.Delete(backupRepositoryFile); 
            System.IO.File.Copy(sourceRepositoryFile, backupRepositoryFile);

            Console.WriteLine("Original repositoy file: \"{0}\"", sourceRepositoryFile);
            Console.WriteLine("Backup repository file: \"{0}\"", backupRepositoryFile);
            
            Repository repository = new Repository();
            repository.OpenFile(backupRepositoryFile);

            return repository;
        }

        private static void RetrieveAndOutlineRepositoryStructure(Repository repository)
        {
            foreach (Package model in repository.Models)
            {
                Console.WriteLine("Model: " + model.Name);

                foreach (Package businessLibrary in model.Packages)
                {
                    Console.WriteLine("Business Library: " + businessLibrary.Name + " (Stereotype: " + businessLibrary.StereotypeEx + ")");
                    
                    foreach (Package library in businessLibrary.Packages)
                    {
                        Console.WriteLine("Library: " + library.Name + " (Stereotype: " + library.StereotypeEx + ")");
                    }
                }
            }            
        }

        private static void DeatailedRetrieveAndOutlineRepository(Repository repository)
        {
            foreach (Package model in repository.Models)
            {
                Console.WriteLine("Model: " + model.Name);

                foreach (Package package in model.Packages)
                {
                    Console.WriteLine("Package: " + package.Name + " (Stereotype: " + package.Element.Stereotype + ")");

                    foreach (Diagram diagram in package.Diagrams)
                    {
                        Console.WriteLine("Diagram: " + diagram.Name + " (ID: " + diagram.DiagramID + ", Author: " + diagram.Author + ", Stereotype: " + diagram.Stereotype + ")");

                        foreach (DiagramObject o in diagram.DiagramObjects)
                        {
                            Console.WriteLine("Element ID: " + o.ElementID + " (attached to: " + o.DiagramID + ")");                            
                        }
                    }
                }
            }
        }

        [Test]
        public void TestFileBasedRepositoryAccess()
        {
            Repository repository = GetFileBasedRepository("C:\\Temp\\", "cc-for-ebInterface-0.3.eap");

            RetrieveAndOutlineRepositoryStructure(repository);
        }

        [Test]
        public void AddModelToRepository()
        {
            Repository repository = GetFileBasedRepository("C:\\Temp\\", "cc-for-ebInterface-0.3.eap");

            // ADD NEW MODEL
            // add new model to the repository named "Model"
            Package model = (Package) repository.Models.AddNew("Model", "");
            model.Update();
            repository.Models.Refresh();

            // ADD VIEW TO THE MODEL
            // add a new view named "ebInterface Data Model" to the model created in 
            // previous step 
            // TODO: icon of the view is "Simple View" instead of "Class View"
            Package view = (Package)model.Packages.AddNew("ebInterface Data Model", "");
            view.Update();
            view.Element.Stereotype = "bLibrary";
            view.Update();
            
            // ADD PACKAGE DIAGRAM TO THE VIEW
            // add a new package diagram named "ebInterface Data Model" to the view created in 
            // previous step
            Diagram packages = (Diagram)view.Diagrams.AddNew("ebInterface Data Model", "Package");
            packages.Author = "Wizard";
            packages.Update();

            // ADD PACKAGE TO THE VIEW
            // add new package named "BDTLibrary" to the view created in previous step
            Package libraryPackage = (Package) view.Packages.AddNew("BDTLibrary", "Package");
            libraryPackage.Update();
            libraryPackage.Element.Stereotype = "BDTLibrary";
            libraryPackage.Update();

            // ADD TAGGED VALUE TO THE PACKAGE
            TaggedValue tv = (TaggedValue) libraryPackage.Element.TaggedValues.AddNew("businessTerm", "");
            tv.Update();            

            // ADD DEFAULT CLASS DIAGRAM TO THE PACKAGE
            // per default add an empty class diagram to the package created above
            Diagram defaultDiagram = (Diagram)libraryPackage.Diagrams.AddNew("BDTLibrary", "Class");           
            defaultDiagram.Update();

            // ADD LIBRARY PACKAGE TO THE PACKAGE DIAGRAM
            DiagramObject newDiagramObject2 = (DiagramObject)packages.DiagramObjects.AddNew("", "");
            newDiagramObject2.ElementID = libraryPackage.Element.ElementID;            
            newDiagramObject2.Update();
          
            repository.RefreshModelView(0); 
                       
            repository.Models.Refresh();            

            //RetrieveAndOutlineRepositoryStructure(repository);            
            DeatailedRetrieveAndOutlineRepository(repository);
        }        
    }
}