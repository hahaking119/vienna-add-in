using System.Collections.Generic;

namespace UpccModel
{
    public class UpccDependencies
    {
        public UpccDependencies(UpccClasses classes, UpccDataTypes dataTypes, UpccEnumerations enumerations)
        {
            All = new[]
                  {
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = dataTypes.Prim,
                          TargetClassifierType = dataTypes.Prim,
                      },
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = enumerations.Enum,
                          TargetClassifierType = enumerations.Enum,
                      },
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = classes.Cdt,
                          TargetClassifierType = classes.Cdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = classes.Acc,
                          TargetClassifierType = classes.Acc,
                      },
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = classes.Bdt,
                          TargetClassifierType = classes.Bdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = "isEquivalentTo",
                          SourceClassifierType = classes.Abie,
                          TargetClassifierType = classes.Abie,
                      },
                      new MetaDependency
                      {
                          Stereotype = "basedOn",
                          SourceClassifierType = classes.Bdt,
                          TargetClassifierType = classes.Cdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = "basedOn",
                          SourceClassifierType = classes.Abie,
                          TargetClassifierType = classes.Acc,
                      },
                  };
        }

        public IEnumerable<MetaDependency> All { get; private set; }

        public IEnumerable<MetaDependency> GetDependenciesForSource(MetaClassifier classifier)
        {
            foreach (MetaDependency dependency in All)
            {
                if (dependency.SourceClassifierType == classifier)
                {
                    yield return dependency;
                }
            }
        }
    }
}