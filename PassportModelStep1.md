# Step 1 - Create a UPCC model & Import CC Libraries #

  1. From the main menu, select _File / New Project..._ to create a new Enterprise Architect project
  1. In the _New Project_ dialog, select a directory to store the EA project file and name it _passport\_model.eap_
  1. In the _Select model(s)_ dialog, choose _`<`default`>`_ from the _Technology_ list and select none of the check boxes in the second list
  1. After clicking _OK_, the new project is generated. To start modeling business documents using the newly created model, you must define the model as a UPCC Model by selecting _Add-Ins / VIENNAAddIn / Set Model as UMM2/UPCC3 Model_ from the main menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild04.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild04.gif)
  1. For creating the standard libraries, launch the _Default UPCC Model Creator_ dialog by selecting _Add-Ins / VIENNAAddIn / Create initial UPCC3 model structure_ from the main menu
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild06.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild06.gif)
  1. In this dialog, create a default model named "Passport Model", this name will be used for the newly created bLibrary. Furthermore, select the _Import standard CC Libraries_ checkbox for importing the standard Core Component Library. This options allows downloading a subset of the standard Core Component Libraries, in this case select the major version "democcl" and the minor version "2":
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild07.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild07.gif)
  1. Finally, click the _Generate Default Model_ button. After the default model was generated, a confirmation message is shown:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild09.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild09.gif)


---


[Passport Modeling Step 1 - Video](http://umm-dev.org/wp-content/videos/passport_part_1.htm): This page shows the above steps in a video tutorial

[Passport Modeling - Continue with step 2](PassportModelStep2.md): This page explains how to create the ABIEs needed for building a passport model


---


# Related Tasks #

[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Import standard CC Libraries](ImportStandardCCLibraries.md): This page explains how to download and import standard Core Component Libraries into an existing UPCC3 model