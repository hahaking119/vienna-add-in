# Aggregate Business Information Entities #

Every Aggregate Business Information Entity (ABIE) is based on an Aggregated Core Component (ACC). When creating a new ABIE, this ABIE is derivated from an ACC by specifying which attributes and associations of the ACC should be inherited by the ABIE. The VIENNA Add-In provides the _ABIE Editor_ dialog for creating new ABIEs based on specific ACCs.

**Update:** The current version of the VIENNA Add-In provides on-the-fly creation of ABIEs by dragging the desired ACC into any BIE Library. A screencast describing the procedure is available [here](HowToCreateNewAbieAlternativeVideo.md).

# Create ABIEs #

  1. Prepare a UPCC3 model in Enterprise Architect
  1. In the _Project Browser_, select the model and right-click on the BIELibrary
  1. Select _Add-In / VIENNAAddIn / Create new ABIE_ from the context menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_01.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_01.gif)
  1. The _ABIE Editor_ dialog provides all options available for creating ABIEs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_02.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_02.gif)
  1. First, you have to decide which ACC the new ABIE should be based on. From the first drop down list, select the CC Library the chosen ACC is located in. Then, you can select the desired ACC from the second drop down list.
  1. After selecting an ACC, all potential attributes and associations the ACC contains are shown in the center of the dialog
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_03.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_03.gif)
  1. In the _Attributes_ section, you can decide which Basic Business Information Entities (BBIEs) the new ABIE should contain.
  1. In the first column, select all Basic Core Components (BCCs) that should be added to the ABIE as BBIEs.
  1. For each BCC selected in the first column, the second and third columns show which  BBIEs will be created based on this BCC, and which Business Data Type (BDT) will be used to typify this BBIE:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_04.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_04.gif)
  1. In the second column, you can rename BBIES by simply selecting the describing text and entering a new name:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_05.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_05.gif)
  1. In addition, it is possible to add new BBIEs by clicking the _+_ button:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_06.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_06.gif)
  1. The same applies to the third column: Create new BDTs by clicking the _+_ button:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_08.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_08.gif)
  1. In addition, it is possible to rename the newly created BDTs (BDTs that existed already cannot be renamed):
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_07.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_07.gif)
  1. The section _Associations_ shows all existing ABIEs that can be associated with the new one. You can select the ABIEs you want to associate from the first column, and for each of those ABIE you can select the Association Business Information Entities (ASBIEs) that should be used from the second column:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_09.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_09.gif)
  1. Enter a name and a prefix fot the ABIE to be created:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_10.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_10.gif)
  1. Select the BIE Library the new ABIE should be located in, as well as the BDT Library the newly created BDTs should be stored in, from the two drop down lists at the bottom of the dialog:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_11.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_11.gif)
  1. Finally, clickt the _Create ABIE_ button:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_12.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_12.gif)
  1. After the ABIE was created successfully, a confirmation message is shown:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_13.gif](http://vienna-add-in.googlecode.com/svn/wiki/images/CreateyourownAbies_13.gif)


# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Import standard CC Libraries](ImportStandardCCLibraries.md): This page explains how to download and import standard Core Component Libraries into an existing UPCC3 model

[Create your own BDTs](CreateyourownBdts.md): This page explains how to create BDTs using the _BDT Editor_

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In