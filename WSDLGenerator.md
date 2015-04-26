The WSDL Generator can be used to generate WSDL files out of a UMM model.
Please note that the sample models from http://umm-dev.org/ will not work with the _WSDL Generator_. We are going to publish a guideline on designing compatible UMM models soon.

# Generate WSDL files #
  1. Prepare a UMM2 model in Enterprise Architect
  1. Select _Add-Ins / VIENNAAddIn / Wizards / WSDL Generator_ from the main menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_EAmenuentry.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_EAmenuentry.png)
  1. After launching the WSDL Generator, an empty form in which the target Business Choreography can be chosen is presented.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_startscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_startscreen.png)
  1. First the Business Choreography View can be selected from the available drop down list.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_bchview.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_bchview.png)
  1. After selecting the Business Choreography View, a corresponding Business Collaboration View can be chosen.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_bcollview.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_bcollview.png)
  1. The directory where the final generated WSDL files will be saved to is set using a standard Windows folder selection dialog.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_selectoutputdir.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_selectoutputdir.png)
  1. After setting the output directory Message Names have to be assigned to the corresponding XML Schemas. These Schemas have to be generated in advance using the [XML Schema Generator](XmlSchemaGenerator.md).
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_outputdirset.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_outputdirset.png)
  1. When an Xml Schema is assigned to a Message Name it's target namespace is also displayed.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_xmlschemaselected.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_xmlschemaselected.png)
  1. After clicking the "Generate WSDL" button, you can follow the generation process in the status bar.

> ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_finished.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/wsdlgenerator_finished.png)

# Troubleshooting #
  * **Problem:** I can not generate wsdl files, the WSDL Generator either displays no Messages or shows an error message (depends on actual version).
  * **Solution:** Every bTransaction and bCollaboration must include an initial Activity with an InitFlow to the desired Initiator. An example from the official UMM 2.0 Specification is shown below:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/UMM_Spec_Initial.png](http://vienna-add-in.googlecode.com/svn/wiki/images_wsdlgenerator/UMM_Spec_Initial.png)
  * **Problem:** Certain bTransactions are missing in the final wsdl files
  * **Solution:** Please check if every desired bTransaction has an initial Activity and InitFlow. Otherwise it will not be included in the final wsdl file.

# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

(3) W3C XML Schema http://www.w3.org/XML/Schema.html

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In

[Generate an XML Schema](XmlSchemaGenerator.md): This page explains how to generate an XML Schema using the _XML Schema Generator_