This guide shows how to create a very simple MapForce mapping with Altova MapForce 2009 SP1 which can be used with the [Xml Schema Importer](XmlSchemaImporter.md). Please be aware that the VIENNA Add-In currently only supports MapForce mappings created with version 2009 SP1.
You can check the version of an existing MapForce mapping file by opening it with your favorite Text Editor instead of MapForce.
```
<mapping xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" version="12">
```
The version attribute represents the MapForce version - 12 corresponds to MapForce 2009 SP1.

# How to create a basic mapping #
  1. The simple Xml Schema below will be used to create a MapForce mapping to CCTS:

```
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns="http://www.ebinterface.at/schema/3p0/" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:dsig="http://www.w3.org/2000/09/xmldsig#" targetNamespace="http://www.ebinterface.at/schema/3p0/" elementFormDefault="qualified" attributeFormDefault="unqualified">
	<xs:import namespace="http://www.w3.org/2000/09/xmldsig#" schemaLocation="http://www.w3.org/TR/2002/REC-xmldsig-core-20020212/xmldsig-core-schema.xsd"/>
	<xs:annotation>
		<xs:documentation>
           schema version: 3.0
           last update: 2008-11-05
           documentation: http://www.ebinterface.at/download/documentation/ebInvoice_3p0.pdf (NOT YET)
       </xs:documentation>
	</xs:annotation>
	<!-- === Root Element === -->
	<xs:element name="Invoice" type="InvoiceType"/>
	<!-- === Element Declarations === -->
	<xs:element name="Address" type="AddressType"/>
	<!-- === Complex Types === -->
	<xs:complexType name="AddressType">
		<xs:sequence>
			<xs:element name="Town" type="xs:string"/>
		</xs:sequence>
	</xs:complexType>
	<xs:complexType name="PersonType">
		<xs:sequence>
			<xs:element name="Name" type="xs:string"/>
		</xs:sequence>
	</xs:complexType>
	<xs:complexType name="InvoiceType">
		<xs:sequence>
			<xs:element ref="Address" minOccurs="0"/>
			<xs:element name="Person" type="PersonType" minOccurs="0"/>
		</xs:sequence>
		<xs:attribute name="GeneratingSystem" type="xs:string" use="required"/>
	</xs:complexType>
</xs:schema>
```

  1. On the other hand we will use the Address Core Component from the current CCL (find it [here](http://code.google.com/p/vienna-add-in/wiki/files_mapping/CoreComponent_1.xsd)).

  1. Inside Altova MapForce go to _Insert / Xml-Schema/File..._
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_loadxsd.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_loadxsd.png)
  1. Selecting the Invoice Schema above will show a informative message which can be skipped.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_skipinfoscreen.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_skipinfoscreen.png)
  1. Select _Address_ as document root.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_chooseAddress.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_chooseAddress.png)
  1. The Xml Schema should now be visualized within Altova MapForce.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_invoice.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_invoice.png)
  1. Proceed with the previous steps for the core component Xml Schema and choose Address as document root again.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_chooseAddressforcc.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_chooseAddressforcc.png)
  1. Now both Xml schemas should be displayed within MapForce.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_invoice+corecomponent.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_invoice+corecomponent.png)
  1. A mapping between two elements or attributes can be defined by draging a connection between the white triangles next to their name.
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_drawmapping.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_drawmapping.png)
  1. The final mapping looks like this:
> > ![http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_mapping.png](http://vienna-add-in.googlecode.com/svn/wiki/files_mapping/MapForce_mapping.png)
  1. Saving this mapping results into a mfd file which can be used for the [Xml Schema Importer](XmlSchemaImporter.md).

# References #
(1) UMM Development Site http://umm-dev.org/

(2) CCTS standard http://www.untmg.org/wp-content/uploads/2008/06/specification_ccts3p0odp620080207.pdf

(3) W3C XML Schema http://www.w3.org/XML/Schema.html

(4) Altova MapForce http://www.altova.com/mapforce.html

# Related Tasks #
[Create a model in Enterprise Architect](CreateaModelinEA.md): This page explains how to open a project file and create a model in Enterprise Architect

[Getting Started Videos](GettingStartedVideos.md): This page contains a few videos explaining common tasks using the VIENNA Add-In

[Import an XML Schema](XmlSchemaImporter.md): This page explains how to import an XML Schema using the _XML Schema Importer_