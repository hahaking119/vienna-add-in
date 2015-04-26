# Storyboard #

  1. Create a new project in Enterprise Architect named "passport\_model.eap".
  1. Define the project as a UMM2/UPCC3 Model.
  1. Launch the "Default UPCC Model Creator" wizard and create a default model named "Passport Model". Furthermore, use the wizard to import the standard Core Component Library having the major version "democcl" and the minor version "2".
  1. Launch the "ABIE Editor" wizard and create the following ABIEs in the order listed. Note that the ABIEs are created based on the Core Components imported in previous step. It is also necessary to create appropriate BDTs. In the following all necessary steps are listed.
    1. ABIE "Address"
      * Choose the ACC "Address"
      * Check the BCC "BuildingNumber" and rename the BDT from "New1Text" to "Text"
      * Check the BCC "CityName"
      * Check the BCC "CountryName"
      * Check the BCC "Postcode" and rename the BDT from "New1Code" to "Code"
      * Check the BCC "StreetName"
      * Click "Create ABIE"
    1. ABIE "Country"
      * Choose the ACC "Country"
      * Check the BCC "Identification" and rename the BDT from "New1Identifier" to "Identifier"
      * Check the BCC "Name"
      * Click "Create ABIE"
    1. ABIE "Period"
      * Choose the ACC "Period"
      * Check the BCC "End", rename the BBIE from "End" to "DateOfExpiry\_End", and rename the BDT from "New1DateTime" to "DateTime"
      * Check the BCC "Start", rename the BBIE from "End" to "DateOfIssue\_Start"
      * Click "Create ABIE"
    1. ABIE "Person"
      * Choose the ACC "Person"
      * Check the BCC "Birth"
      * Check the BCC "BirthCountry"
      * Check the BCC "EyeColor"
      * Check the BCC "Gender"
      * Check the BCC "GivenName"
      * Check the BCC "Height" and rename the BDT from "New1Measure" to "Measure"
      * Check the BCC "Name" and rename the BBIE from "Name" to "SurName\_Name"
      * Check the BCC "Passport" and add as well as check two additional BBIEs. Now rename the first BBIE from "Passport" to "Code\_Passport", the second BBIE from "New1Passport" to "Number\_Passport", and the third BBIE from "New2Passport" to "Type\_Passport".
      * Switch to the Associations Tab and for the ABIE "Country" check the ASBIE "Nationality" and for the ABIE "Address" check the ASBIE "Residence".
      * Click "Create ABIE"
  1. Open the "DOCLibrary" and drag-and-drop the ABIEs created in previous step, namely "Address", "Country", "Period", and "Person" onto the canvas.
  1. From the toolbox drag-and-drop the UPCC Artefact "MA" onto the canvas and rename it to "Passport".
  1. Finally, select the UPCC Artefact "ASMA" from the toolbox and drag it from the ABIE "Period" to the MA "Passport". Repeat previous step and drag-and-drop a MA from the ABIE "Person" to the MA "Passport".