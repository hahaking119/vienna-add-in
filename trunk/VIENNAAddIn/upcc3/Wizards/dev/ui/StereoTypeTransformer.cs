using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using Attribute = EA.Attribute;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    internal partial class StereoTypeTransformer : Form
    {
        private readonly Repository _eaRepo;

        public StereoTypeTransformer(Repository repository)
        {
            InitializeComponent();
            _eaRepo = repository;
        }

        public static void ShowForm(AddInContext context)
        {
            new StereoTypeTransformer(context.EARepository).ShowDialog();
        }

        private void status_update(string new_tatus)
        {
            status.Text = status.Text + Environment.NewLine + new_tatus;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            okButton.Enabled = false;
            UseWaitCursor = true;
            _eaRepo.Stereotypes.AddNew("ACC", "Class");
            _eaRepo.Stereotypes.AddNew("BCC", "Attribute");
            _eaRepo.Stereotypes.AddNew("CDT", "Class");
            _eaRepo.Stereotypes.AddNew("ABIE", "Class");
            _eaRepo.Stereotypes.AddNew("ASCC", "Connector");
            _eaRepo.Stereotypes.AddNew("CCLibrary", "Package");
            _eaRepo.Stereotypes.AddNew("BIELibrary", "Package");
            _eaRepo.Stereotypes.AddNew("CDTLibrary", "Package");
            _eaRepo.Stereotypes.AddNew("BDTLibrary", "Package");
            _eaRepo.Stereotypes.Refresh();
            foreach (Package model in _eaRepo.Models)
            {
                status_update("adding new Libraries ...");
                var bieLibrary = (Package) model.Packages.AddNew("BIELibrary", "Package");
                bieLibrary.Update();
                bieLibrary.Element.Stereotype = "BIELibrary";
                var bieLibraryDiagram = (Diagram) bieLibrary.Diagrams.AddNew("BieLibrary", "Class");
                bieLibraryDiagram.Update();
                var ccLibrary = (Package) model.Packages.AddNew("CCLibrary", "Package");
                ccLibrary.Update();
                ccLibrary.Element.Stereotype = "CCLibrary";
                var ccLibraryDiagram = (Diagram) ccLibrary.Diagrams.AddNew("CCLibrary", "Class");
                ccLibraryDiagram.Update();
                var cdtLibrary = (Package) model.Packages.AddNew("CDTLibrary", "Package");
                cdtLibrary.Update();
                cdtLibrary.Element.Stereotype = "CDTLibrary";
                var bdtLibrary = (Package)model.Packages.AddNew("BDTLibrary", "Package");
                bdtLibrary.Update();
                bdtLibrary.Element.Stereotype = "BDTLibrary";
                bdtLibrary.Update();
                bieLibrary.Update();
                ccLibrary.Update();
                cdtLibrary.Update();
                ccLibrary.Diagrams.Refresh();
                bieLibrary.Diagrams.Refresh();
                model.Packages.Refresh();
                foreach (Package package in model.Packages)
                {
                    if (package.Name.Equals("Class"))
                    {
                        status_update("updating Class Stereotypes ...");
                        loop_Class_Packages(package, ccLibrary, cdtLibrary, ccLibraryDiagram);
                    }
                }
                AddConnectors(model);
            }
            okButton.Enabled = true;
            UseWaitCursor = false;
        }

        private void AddConnectors(Package model)
        {
            var ccLibrary = (Package) model.Packages.GetByName("CCLibrary");
            var existingConnectors = new Dictionary<int, List<int>>();
            foreach (Element element in ccLibrary.Elements)
            {
                List<Element> originalElements = GetClassElementsByName(element.Name,
                                                                        (Package) model.Packages.GetByName("Class"),
                                                                        new List<Element>());
                foreach (Element originalElement in originalElements)
                {
                    foreach (Connector originalConnector in originalElement.Connectors)
                    {
                        Element targetElement =
                            GetClassElementsByName(_eaRepo.GetElementByID(originalConnector.SupplierID).Name,
                                                   ccLibrary, new List<Element>())[0];
                        Element sourceElement =
                            GetClassElementsByName(_eaRepo.GetElementByID(originalConnector.ClientID).Name,
                                                   ccLibrary, new List<Element>())[0];

                        List<int> output;
                        existingConnectors.TryGetValue(sourceElement.ElementID, out output);
                        if (output == null)
                        {
                            output = new List<int>();
                            var connector =
                                (Connector)
                                ccLibrary.Connectors.AddNew(originalConnector.Name, originalConnector.Type);
                            connector.StereotypeEx = originalConnector.Stereotype;
                            connector.Stereotype = "ASCC";
                            connector.ClientID = sourceElement.ElementID;
                            connector.SupplierID = targetElement.ElementID;
                            output.Add(targetElement.ElementID);
                            existingConnectors.Add(sourceElement.ElementID, output);
                            connector.Update();
                        }
                        else
                        {
                            if (!output.Contains(targetElement.ElementID))
                            {
                                var connector =
                                    (Connector)
                                    ccLibrary.Connectors.AddNew(originalConnector.Name, originalConnector.Type);
                                connector.StereotypeEx = originalConnector.Stereotype;
                                connector.Stereotype = "ASCC";
                                connector.ClientID = sourceElement.ElementID;
                                connector.SupplierID = targetElement.ElementID;
                                output.Add(targetElement.ElementID);
                                existingConnectors[sourceElement.ElementID] = output;
                                connector.Update();
                            }
                        }
                    }
                    element.Connectors.Refresh();
                }
            }
        }

        private static List<Element> GetClassElementsByName(string name, Package _package, List<Element> returnList)
        {
            foreach (Element element in _package.Elements)
            {
                if (element.Type.Equals("Class"))
                {
                    if (element.Name.Equals(name))
                    {
                        returnList.Add(element);
                    }
                }
            }
            foreach (Package subPackage in _package.Packages)
            {
                returnList = GetClassElementsByName(name, subPackage, returnList);
            }
            return returnList;
        }

        private void loop_Class_Packages(Package package, Package ccLibrary, Package cdtLibrary,
                                         Diagram ccLibraryDiagram)
        {
            if (!package.Name.Equals("Basic Type") && !package.Name.Equals("Typedefs"))
            {
                foreach (Element originalElement in package.Elements)
                {
                    if (originalElement.Type.Equals("Interface"))
                    {
                        continue;
                    }
                    try
                    {
                        if (ccLibrary.Elements.GetByName(originalElement.Name) == null)
                        {
                            throw new IndexOutOfRangeException("ccLibrary is empty");
                        }
                    }
                    catch (Exception)
                    {
                        status_update("adding ACC: " + originalElement.Name);
                        var acc = (Element) ccLibrary.Elements.AddNew(originalElement.Name, "Class");
                        acc.StereotypeEx = originalElement.Stereotype;
                        acc.Stereotype = "ACC";
                        foreach (Attribute originalAttribute in originalElement.Attributes)
                        {
                            var attribute =
                                (Attribute) acc.Attributes.AddNew(originalAttribute.Name, originalAttribute.Type);
                            attribute.StereotypeEx = originalAttribute.Stereotype;
                            attribute.Stereotype = "BCC";
                            //attribute. = originalAttribute.Type;
                            try
                            {
                                if (cdtLibrary.Elements.GetByName(originalAttribute.Type) == null)
                                {
                                    throw new IndexOutOfRangeException("cdtLibrary is empty!");
                                }
                                attribute.ClassifierID =
                                    ((Element) cdtLibrary.Elements.GetByName(originalAttribute.Type)).ElementID;
                                attribute.Update();
                                acc.Attributes.Refresh();
                            }
                            catch (Exception)
                            {
                                if (!originalAttribute.Type.Equals(string.Empty))
                                {
                                    var cdt = (Element) cdtLibrary.Elements.AddNew(originalAttribute.Type, "Class");
                                    cdt.Stereotype = "CDT";
                                    cdt.Update();
                                    cdtLibrary.Elements.Refresh();
                                    attribute.ClassifierID = cdt.ElementID;
                                    attribute.Update();
                                    acc.Attributes.Refresh();
                                }
                            }
                            attribute.Update();
                            acc.Attributes.Refresh();
                        }
                        acc.Update();
                        var newDiagramObject =
                            (DiagramObject) ccLibraryDiagram.DiagramObjects.AddNew(string.Empty, string.Empty);
                        newDiagramObject.DiagramID = ccLibraryDiagram.DiagramID;
                        newDiagramObject.ElementID = acc.ElementID;
                        newDiagramObject.Update();
                        ccLibraryDiagram.DiagramObjects.Refresh();
                        ccLibrary.Elements.Refresh();
                    }
                }
                foreach (Package subPackage in package.Packages)
                {
                    loop_Class_Packages(subPackage, ccLibrary, cdtLibrary, ccLibraryDiagram);
                }
            }
        }
    }
}