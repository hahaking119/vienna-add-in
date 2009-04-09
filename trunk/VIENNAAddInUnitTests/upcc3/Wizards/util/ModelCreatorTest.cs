using System;
using EA;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ModelCreatorTest
    {
        public Repository GetFileBasedRepository(string filePath, string fileName)
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

        public void RetrieveAndOutlineRepositoryStructure(Repository repository)
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

            
            /*****************************************************************************
             * Model
             **/
            Package newModel = (Package) repository.Models.AddNew("Model", "");

            newModel.Update();
            repository.Models.Refresh();

            /*****************************************************************************
             * Class View
             **/
            Element x = (Element) newModel.Elements.AddNew("Class View", "ClassView");
            x.Update();
            //Package p = (Package) newModel.Packages.AddNew("Class View", "ClassView");
            //newModel.Update();
            //p.Update();

            
            repository.Models.Refresh();    
        

            RetrieveAndOutlineRepositoryStructure(repository);
        }
    }
}