# Troubleshooting #


**_Q: After I have installed the VIENNA Add-In cannot see the VIENNA Add-In menu item in the Enterprise Architect Add-Ins menu._**

A: If you have installed the VIENNA Add-In choosing the option "Install for all users of this computer" you have to manually add the registry entries, necessary for the VIENNA Add-In. Go to the folder to which the VIENNA Add-In was installed - usually C:\Program Files\VIENNAAddIn\VIENNAAddIn . Select the subfolder \ext and double click on the file **viennaaddin.reg**.

**_Q: After I have started the installation of the VIENNA Add-In the installer wants to download the .NET framework_**

A: The VIENNA Add-In requires the .NET framework and will not be installed without the framework being present.

**_Q: After I have started the installation of the VIENNA Add-In the installer starts to download updates for the .NET framework._**

A: The installer automatically checks, whether you are using the most recent version of the .NET. If not, the most recent version will be downloaded.