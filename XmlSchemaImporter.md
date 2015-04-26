The XML Schema Importer can be used to import an XML Schema into an existing model.

# Import an XML Schema #
  1. Prepare a UPCC3 model in Enterprise Architect
  1. Select _Add-Ins / VIENNAAddIn / Wizards / Import XML Schemas_ from the main menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_EAmenuEntry.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_EAmenuEntry.png)
  1. After launching the XML Schema Importer, the XSD tab will be displayed by default. Many of the fields needed for importing an XML schema are already filled out on startup. Of course any of these can be customized to your individual needs.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_startscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_startscreen.png)
  1. First you have to select an XML schema which should be imported using the Windows file selection dialog.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_importxsdselected.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_importxsdselected.png)
  1. For a successfull import MapForce mapping files have to be supplied in the following step. These can be added or removed at any time. A guideline for creating MapForce mapping files can be found [here](MapForce.md).
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_mappingfilesselected.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_mappingfilesselected.png)
  1. ~~Switching to the CCTS tab, there is only the root XML schema which has to be selected.~~ The CCTS import feature is disabled in the current build.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_selectCctsRoot.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_selectCctsRoot.png)
  1. You can select a file again using the Windows file selection dialog.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_CctsRootSelected.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_CctsRootSelected.png)
  1. After clicking the "Import XML Schemas" button, the message "Import completed!" will be displayed in the status bar, once the process is completed
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_importcompleted.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemaimporter/xmlschemaimporter_importcompleted.png)

# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

(3) W3C XML Schema http://www.w3.org/XML/Schema.html

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In

[How to create a MapForce mapping](HowToMapping.md): This page explains how to create a basic mapping using Altova MapForce