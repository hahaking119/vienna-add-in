# Step 2 - Create ABIEs #

In the Project Browser, right-click the BIELibrary and select _Add-Ins / VIENNAAddIn / Create new ABIE_ to launch the _ABIE Editor_ wizard:

![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild10.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild10.gif)

In the _ABIE Editor_, create the following ABIEs in the order listed. Note that the ABIEs are created based on the Core Components imported in the previous step. It is also necessary to create appropriate BDTs. In the following all tasks necessary will be illustrated.

### ABIE _Address_ ###
  1. Choose ACC _Address_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild11.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild11.gif)
  1. Check BCC _BuildingNumber_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild12.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild12.gif)
  1. Rename BDT from _New1Text_ to _Text_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild13.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild13.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild14.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild14.gif)
  1. Check BCC _CityName_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild16.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild16.gif)
  1. Check BCC _CountryName_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild15.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild15.gif)
  1. Check BCC _Postcode_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild17.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild17.gif)
  1. Rename BDT from _New1Code_ to _Code_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild18.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild18.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild19.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild19.gif)
  1. Check BCC _StreetName_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild20.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild20.gif)
  1. Optionally, the _Prefix used for the generated Artifacts_ textbox provides the possibility to assign a specific prefix to all artifact identifiers. Finally, click _Create ABIE_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild21.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild21.gif)
  1. A confirmation message shows that the ABIE was created successfully:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild22.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild22.gif)
  1. In the Project Browser you can see that the ABIE "Address" including all BBIEs was created as member of the BIELibrary:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild23.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild23.gif)
### ABIE _Country_ ###
  1. Choose ACC _Country_
  1. Check BCC _Identification_ and rename the BDT from _New1Identifier_ to _Identifier_
  1. Check BCC _Name_
  1. Click _Create ABIE_
### ABIE _Period_ ###
  1. Choose ACC _Period_
  1. Check BCC _End_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild25.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild25.gif)
  1. Rename the BBIE from _End_ to _DateOfExpiry\_End_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild26.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild26.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild27.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild27.gif)
  1. Rename the BDT from _New1DateTime_ to _DateTime_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild28.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild28.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild29.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild29.gif)
  1. Check BCC _Start_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild30.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild30.gif)
  1. Rename the BBIE from _Start_ to _DateOfIssue\_Start_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild31.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild31.gif)
  1. Click _Create ABIE_
### ABIE _Person_ ###
  1. Choose ACC _Person_
  1. Check BCC _Birth_
  1. Check BCC _BirthCountry_
  1. Check BCC _EyeColor_
  1. Check BCC _Gender_
  1. Check BCC _GivenName_
  1. Check BCC _Height_ and rename the BDT from _New1Measure_ to _Measure_
  1. Check BCC _Name_ and rename the BBIE from _Name_ to _SurName\_Name_
  1. Check BCC _Passport_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild32.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild32.gif)
  1. Add as well as check two additional BBIEs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild33.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild33.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild34.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild34.gif)
  1. Now rename the first BBIE from _Passport_ to _Code\_Passport_, the second BBIE from _New1Passport_ to _Number\_Passport_, and the third BBIE from _New2Passport_ to _Type\_Passport_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild35.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild35.gif)
  1. Switch to the Associations Tab. This view allows you to create connections and associations between two ABIEs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild36.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild36.gif)
  1. For ABIE _Country_ check the ASBIE _Nationality_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild37.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild37.gif)
  1. For ABIE _Address_ check the ASBIE _Residence_:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild38.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild38.gif)
  1. Click _Create ABIE_
  1. Open the _DOCLibrary_ by doucle-clicking it in the Project Browser:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild39.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild39.gif)
  1. Select the ABIEs created in the previous step, namely _Address_, _Country_, _Period_, and _Person_ in the Project Browser, and drag-and-drop them onto the canvas:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild40.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild40.gif)
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild41.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild41.gif)
  1. In the _Paste Element_ dialog, choose to paste the element as simple link:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild42.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild42.gif)
  1. The DOCLibrary canvas should now contain the four ABIEs, including the associating ASBIEs:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43.gif)
  1. Exactly one address and one country is assigned to a person, to model this you must change the cardinality values of the two ASBIEs: Double-click the ASBIEs, in the _Aggregation Properties_ dialog switch to the _Target Role_ tab and change the _Multiplicity_ value to 1:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43a.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43a.gif)
  1. Then click _OK_
> > ![http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43b.gif](http://vienna-add-in.googlecode.com/svn/wiki/PassportModel/Bild43b.gif)


---


[Passport Modeling Step 2 - Video](http://umm-dev.org/wp-content/videos/passport_part_2.htm): This page shows the above steps in a video tutorial

[Passport Modeling - Continue with step 3](PassportModelStep3.md): This page explains how to create associations inside the passport model


---


# Related Tasks #

[Create your own ABIEs](CreateyourownAbies.md): This page explains how to create ABIEs using the _ABIE Editor_