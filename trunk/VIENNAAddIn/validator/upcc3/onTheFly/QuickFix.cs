using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public interface QuickFix
    {
        void Execute(Repository repository, object item);
    }
}