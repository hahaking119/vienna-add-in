using System;

namespace Wizards
{
    public class Wizards
    {
        public Object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            switch (MenuName)
            {
                case "":
                    // "-" indicates that current menu item contains further submenu items
                    // "&" specifies that the character following "&" is a short key
                    return "-Wi&zards";

                case "-Wi&zards":
                    string[] MenuItems = { "&ABIE Wizard", "&BDT Wizard", "About" };
                    return MenuItems;
            }

            return "";
        }

        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                case "&ABIE Wizard":
                    ABIEWizardForm ABIEWizard = new ABIEWizardForm(Repository);
                    ABIEWizard.Show();
                    break;

                case "&BDT Wizard":
                    BDTWizardForm BDTWizard = new BDTWizardForm(Repository);
                    BDTWizard.Show();
                    break;

                case "About":
                    AboutForm About = new AboutForm();
                    About.Show();
                    break;
            }
        }
    }
}
