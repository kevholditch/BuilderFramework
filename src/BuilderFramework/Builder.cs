using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderFramework
{
    public class Builder
    {
        private readonly IBuildStepDependencySorter _buildStepDependencySorter;
        public List<IBuildStep> BuildSteps { get; private set; }

        public Builder(IBuildStepDependencySorter buildStepDependencySorter)
        {
            _buildStepDependencySorter = buildStepDependencySorter;
            BuildSteps = new List<IBuildStep>();
        }

        public Builder With<TStepBuilder>(Func<TStepBuilder, IBuildStep> addStep) where TStepBuilder : new()
        {
            BuildSteps.Add(addStep(new TStepBuilder()));
            return this;
        }

        public Builder With(IBuildStep buildStep) 
        {
            BuildSteps.Add(buildStep);
            return this;
        }

        private void Execute(IEnumerable<IBuildStep> buildSteps, Action<IBuildStep> action)
        {

            foreach (var buildStep in buildSteps)
            {
                action(buildStep);
            }
        }

        public void Commit()
        {
            Execute(_buildStepDependencySorter.Sort(BuildSteps), 
                        buildStep => buildStep.Commit());
        }

        public void Rollback()
        {
            Execute(_buildStepDependencySorter.Sort(BuildSteps).Reverse(), 
                        buildStep => buildStep.Rollback());
        }

    }
}
