using System.Collections.Generic;

namespace BuilderFramework
{
    public interface IBuildStepDependencySorter
    {
        IEnumerable<IBuildStep> Sort(IEnumerable<IBuildStep> buildSteps);
    }
}
