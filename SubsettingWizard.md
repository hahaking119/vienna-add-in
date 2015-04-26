The Subsetting Wizard can be used to create a subset of an existing model.

# Transform an existing Model #
  1. Prepare a UPCC3 model in Enterprise Architect
  1. Select _Add-Ins / VIENNAAddIn / Wizards / Subsetting Wizard_ from the main menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/schemaanalyzer_EA_menuentry.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/schemaanalyzer_EA_menuentry.png)
  1. When started, the Subsetting Wizard shows an empty dialog with a dropdown box to select a doc library and three empty columns.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_startscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_startscreen.png)
  1. After choosing a DOC library from the list of available libraries, the corresponding ABIEs, BBIEs and ASBIEs will be loaded
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_chooseDocLib.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_chooseDocLib.png)
  1. While the loading operation is beeing performed, the progress is indicated with a spinning wheel.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_loading.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_loading.png)
  1. When loading has finished, a tree view will be displayed in the first column. It will be collapsed by default.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_collapsed_treeview.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_collapsed_treeview.png)
  1. You can expand the treeview at any level to show it's children which represent connected ABIEs or MAs.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview.png)
  1. You can select an ABIE to display it's corresponding BBIEs in the second column.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview_selectedAbie.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview_selectedAbie.png)
  1. When you uncheck an ABIE or BBIE it will be removed from the current model. If you select an ABIE, it's contained BBIEs will be removed as well if they are not used anywhere else. The wizard also takes care of dependencies between related elements. If you select a child element, it's parents will be selected as well.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview_uncheckAbie.png](http://vienna-add-in.googlecode.com/svn/wiki/images_subsettingwizard/subsettingwizward_expanded_treeview_uncheckAbie.png)

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

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In