namespace BuilderFramework.Exceptions
{
    public class CircularDependencyException : BuilderExeception
    {
        public CircularDependencyException() : base("Circular depdency dectected")
        {
        }
    }
}