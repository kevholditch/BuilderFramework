namespace BuilderFramework.Tests.TestTypes
{
    [DependsOn(typeof(BuildStepB))]
    public class BuildStepA : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    public class BuildStepB : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(Circular2))]
    public class Circular1 : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(Circular3))]
    public class Circular2 : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(Circular1))]
    public class Circular3 : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(CircularFiveStepB))]
    public class CircularFiveStepA : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(CircularFiveStepC))]
    public class CircularFiveStepB : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(CircularFiveStepD))]
    public class CircularFiveStepC : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(CircularFiveStepE))]
    public class CircularFiveStepD : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(CircularFiveStepA))]
    public class CircularFiveStepE : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    public class SequentialDepdencyStepA : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(SequentialDepdencyStepA))]
    public class SequentialDepdencyStepB : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(SequentialDepdencyStepB))]
    public class SequentialDepdencyStepC : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(SequentialDepdencyStepC))]
    public class SequentialDepdencyStepD : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }


    public class MultipleDepdencyStepA : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    public class MultipleDepdencyStepB : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }

    [DependsOn(typeof(MultipleDepdencyStepA))]
    [DependsOn(typeof(MultipleDepdencyStepB))]
    public class MultipleDepdencyStepC : IBuildStep
    {
        public void Commit() { throw new System.NotImplementedException(); }
        public void Rollback() { throw new System.NotImplementedException(); }
    }
}
