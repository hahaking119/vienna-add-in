The XML Schema Generator can be used to export an existing model to an XML Schema.

# Generate an XML Schema #
  1. Prepare a UPCC3 model in Enterprise Architect
  1. Select _Add-Ins / VIENNAAddIn / Wizards / Generate XML Schema_ from the main menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_EAmenuentry.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_EAmenuentry.png)
  1. First the wizard will show only empty fields, you have to select a Business Information View to start.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_startscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_startscreen.png)
  1. After Selecting a Business Information View you can choose the Documents which should be exported to an XML schema.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_doclibselected.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_doclibselected.png)
  1. Every selected Document will be included in the exported XML schema set.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_rootMaChecked.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_rootMaChecked.png)
  1. Next a output directory for the XML schema file has to be selected. This can be done by clicking on the "..." button and selecting a file using a default Windows file selection dialog.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_outputdirectoryset.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_outputdirectoryset.png)
  1. Define a target namespace and a custom prefix - the next image shows some example entries.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_namespaceset.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_namespaceset.png)
  1. Select whether documentation annotations should be included in the generated XML schema files or not. The same applies to the generation of Core Component Schemas.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_checkboxeschecked.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_checkboxeschecked.png)
  1. After clicking on the "Export XML Schema" button, the generator starts to generate the XML schema files. The progress is shown in the Status textbox at the bottom.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_exportschemaclick.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_exportschemaclick.png)
  1. The status field is updated on every progress made by the generator.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_generationprogress.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_generationprogress.png)
  1. When the generation progress has finished, the status field will show "Generating XML schemas completed!" in the last line of the textbox.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_generationcompleted.png](http://vienna-add-in.googlecode.com/svn/wiki/images_xmlschemagenerator/schemagenerator_generationcompleted.png)

# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Import standard CC Libraries](ImportStandardCCLibraries.md): This page explains how to download and import standard Core Component Libraries into an existing UPCC3 model

[Import XML Schema](XmlSchemaImporter.md): This page explains how to import an XML Schema into an existing UPCC3 model

[Create your own BDTs](CreateyourownBdts.md): This page explains how to create BDTs using the _BDT Editor_

[Create your own ABIEs](CreateyourownAbies.md): This page explains how to create ABIEs using the _ABIE Editor_

[Transform a Model](TransformerWizard.md): This page explains how to transform an existing UPCC3 model

[Sub-set a Model](SubsettingWizard.md): This page explains how to create a sub-set of an existing UPCC3 model

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In