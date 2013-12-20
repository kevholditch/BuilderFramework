using System;
using System.Collections.Generic;

namespace BuilderFramework
{
    public class Builder
    {
        public List<IBuildStep> BuildSteps { get; private set; }

        public Builder()
        {
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

        private void Execute(Action<IBuildStep> action)
        {
            foreach (var buildStep in BuildSteps)
            {
                action(buildStep);
            }
        }

        public void Commit()
        {
            Execute(buildStep => buildStep.Commit());
        }

        public void Rollback()
        {
            Execute(buildStep => buildStep.Rollback());
        }

    }
}
