namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public abstract class MenuItem
    {
        protected MenuItem(string name)
        {
            Name = name;
        }

        ///<summary>
        ///</summary>
        public string Name { get; private set; }
    }
}