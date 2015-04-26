# Standard CC Libraries #

The VIENNA Add-In helps you to automatically download several versions of the standard Core Component Libraries (CCL) and import them into a UPCC model.

# Import standard CC Libraries into an existing UPCC3 model #

  1. Prepare an Enterprise Architect model and set it as UPCC3 model
  1. In the _Project Browser_, select the model and right-click on the bLibrary
  1. Select _Add-In / VIENNAAddIn / Import Standard CC Libraries_ from the context menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_01.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_01.gif)
  1. In the _Standard Core Component Library Importer_ dialog, you can choose the desired version of the CCL from two drop-down lists
  1. After choosing the CCL version, detailed information about the selected version is shown in the _Comments_ section:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_02.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_02.gif)
  1. Finally, click _Import_
  1. If there are existing libraries in the current project with the same name as the Core Components Libraries, you will need to confirm that these old liberaries may be overwritten:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_03.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_03.gif)
  1. The Add-In will now download the Core Component Libraries as XMI files and import them into the current project
  1. You can observe the import process by watching the status messages that are printed to the _Status_ section regularly:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_04.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/ImportStandardCCLibraries_04.gif)


# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In