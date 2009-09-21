using System;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class DeletePackage : QuickFix
    {
        private readonly string packageGUID;

        public DeletePackage(string packageGUID)
        {
            this.packageGUID = packageGUID;
        }

        #region QuickFix Members

        public void Execute(Repository repository, object item)
        {
            // TODO ask the user for a confirmation?
            // TODO if confirmed, delete the package
            throw new NotImplementedException();
        }

        #endregion
    }
}