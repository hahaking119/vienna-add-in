using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IEAPackage : IEAItem
    {
        IEAPackage ParentPackage { get; set; }
        IEnumerable<IEAElement> EAElements { get; }
        IEnumerable<IEAPackage> SubPackages { get; }
        int ParentId { get; }
        void WithEachSubPackageDo<T>(Action<T> action);
        void WithAllItemsDo(Action<IEAItem> action);
        void AddSubPackage(IEAPackage package);
        void ReplaceSubPackage(IEAPackage package);
        void ReplaceElement(IEAElement element);
        void AddElement(IEAElement element);
        void CopyChildren(IEAPackage package);
        void RemoveSubPackage(int id);
        void RemoveElement(int id);
    }
}