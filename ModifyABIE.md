# Aggregate Business Information Entities #

Every Aggregate Business Information Entity (ABIE) is based on an Aggregated Core Component (ACC). When creating a new ABIE, this ABIE is derivated from an ACC by specifying which attributes and associations of the ACC should be inherited by the ABIE. The VIENNA Add-In provides the _ABIE Editor_ dialog for creating new ABIEs based on specific ACCs or modifying existing ones.

# Modify ABIEs #

  1. Prepare a UPCC3 model in Enterprise Architect
  1. In the _Project Browser_, select a BIELibrary and right-click on a contained ABIE
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/selectABIE.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/selectABIE.png)
  1. Select _Add-In / VIENNAAddIn / Modify ABIE_ from the context menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/rightclickABIE.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/rightclickABIE.png)
  1. The _ABIE Editor_ dialog provides all options available for modifying ABIEs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/abieeditorstartscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/abieeditorstartscreen.png)
  1. The underlying ACC, the is based on is initially selected. You cannot change this value using the _ABIE Editor_
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/availableAccs.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/availableAccs.png)

  1. The section _Associations_ shows all existing ABIEs that are associated with the current ABIE. You can change the selection of the ABIEs you want to associate from the first column, and for each of those ABIE you can select the Association Business Information Entities (ASBIEs) that should be used from the second column:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/abieeditorasbies.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/abieeditorasbies.png)
  1. Finally, click the _Update ABIE_ button
  1. After the ABIE was updated successfully, a confirmation message is shown:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/notification.png](http://vienna-add-in.googlecode.com/svn/wiki/images_modifyABIE/notification.png)


# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Create your first UPCC model](CreateaUpccModel.md): This page explains how to create a UPCC model

[Import standard CC Libraries](ImportStandardCCLibraries.md): This page explains how to download and import standard Core Component Libraries into an existing UPCC3 model

[Create your own BDTs](CreateyourownBdts.md): This page explains how to create BDTs using the _BDT Editor_

[Create your own ABIEs](CreateyourownAbies.md): This page explains how to create ABIEs using the _ABIE Editor_

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In