# Step 3 - Document modeling #

  1. A DOCLibrary is used to aggregate different ABIEs. If the _UPCC DOCLibrary_ toolbox is not visible, load it by clicking the _More tools..._ button...
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild44.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild44.gif)
  1. ...and choose _UPCC 3.0 / UPCC3 - DOCLIbrary Abstract Syntax_ from the menu:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild45.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild45.gif)
  1. ABIEs are aggregated in a DOCLibrary using the concept of message assemblies (MA). A message assembly is connected to each ABIE it aggregates by an association message assembly (ASMA). To create a new message assembly, drag-and-drop the UPCC artifact _MA_ from the toolbox onto the canvas:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild46.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild46.gif)
  1. Rename the newly created MA to _Passport_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild47.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild47.gif)
  1. Select the UPCC artifact _ASMA_ from the toolbox
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild48.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild48.gif)
  1. Drag an ASMA from the ABIE _Period_ to the MA _Passport_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild49.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild49.gif)
  1. Repeat the previous step and drag-and-drop a MA from the ABIE _Person_ to the MA _Passport_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild50.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild50.gif)
  1. A passport contains information about exactly one person, and it has only one period of validity. To model this you must change the cardinality values of the two ASMAs: Double-click the ASMAs, in the _Aggregation Properties_ dialog switch to the _Source Role_ tab and set the _Multiplicity_ value to 1:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild51.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild51.gif)
  1. The final model should look as follows:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild52.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild52.gif)


---


[Passport Modeling Step 3 - Video](http://umm-dev.org/wp-content/videos/passport_part_3.htm): This page shows the above steps in a video tutorial

[Passport Modeling - Outlook](PassportModelStep4.md): This page explains what to do with the newly created passport model


---
