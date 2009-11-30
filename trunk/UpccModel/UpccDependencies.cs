using System.Collections.Generic;

namespace UpccModel
{
    public class UpccDependencies
    {
        public UpccDependencies(UpccClasses classes)
        {
            All = new[] {
                new MetaDependency
                {
                    Stereotype = "isEquivalentTo",
                    SourceClassifierType = classes.Acc,
                    TargetClassifierType = classes.Acc,
                }
            };
        }

        public IEnumerable<MetaDependency> All { get; private set; }

        public IEnumerable<MetaDependency> GetDependenciesForSource(MetaClassifier classifier)
        {
            foreach (var dependency in All)
            {
                if (dependency.SourceClassifierType == classifier)
                {
                    yield return dependency;
                }
            }
        }
    }
}