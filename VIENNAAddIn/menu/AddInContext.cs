using EA;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class AddInContext
    {
        ///<summary>
        ///</summary>
        public Repository Repository { get; set; }

        public string MenuLocation { get; set; }

        public CCRepository CCRepository
        {
            get { return new CCRepository(Repository); }
        }

        public string MenuName { get; set; }

        public string MenuItem { get; set; }
    }
}