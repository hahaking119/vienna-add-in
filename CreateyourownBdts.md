# Business Data Types #

Every Business Data Type (BDT) is based on a Core Data Type (CDT). The VIENNA Add-In provides the _BDT Editor_ dialog for creating new BDTs based on specific CDTs.

# Create BDTs #

  1. Prepare a UPCC3 model in Enterprise Architect
  1. In the _Project Browser_, select the model and right-click on the BDTLibrary
  1. Select _Add-In / VIENNAAddIn / Create new BDT_ from the context menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_01.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_01.gif)
  1. The _BDT Editor_ dialog provides all options available for creating BDTs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_02.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_02.gif)
  1. First, you have to decide which CDT the new BDT should be based on. From the first drop down list, select the CDT Library the chosen CDT is located in. Then, you can select the desired CDT from the second drop down list.
  1. After selecting a CDT, all potential Content Components and Supplementary Components the CDT contains are shown in the center of the dialog
  1. Each CDT contains exactly one Content Component that is already selected, therefore you cannot change the selection in the _CON_ list:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_03.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_03.gif)
  1. From the _SUP_ list, you can select all Supplementary Components the BDT should contain:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_04.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_04.gif)
  1. Enter a name and a prefix for the BDT to be created:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_05.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_05.gif)
  1. Select the BDT Library the new BDT should be located in from the drop down list at the bottom of the dialog
  1. Finally, click the _Create BDT_ button:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_06.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_06.gif)
  1. After the BDT was created successfully, a confirmation message is shown:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_08.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_08.gif)


If a BDT with the desired name exists already, a warning will be displayed:

![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_07.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownBdts_07.gif)

# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Import standard CC Libraries](ImportStandardCCLibraries.md): This page explains how to download and import standard Core Component Libraries into an existing UPCC3 model

[Create your own ABIEs](CreateyourownAbies.md): This page explains how to create ABIEs using the _ABIE Editor_

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In