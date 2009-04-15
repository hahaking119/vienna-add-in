using System;

namespace VIENNAAddIn.menu
{
    public static class MenuStringExtensions
    {
        public static MenuAction OnClick(this string name, Action<AddInContext> onClick)
        {
            return new MenuAction(name, onClick);
        }
    }
}