using System;

namespace BuilderFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DependsOnAttribute : Attribute
    {
        public Type DependedOnStep { get; private set; }

        public DependsOnAttribute(Type buildStep)
        {
            DependedOnStep = buildStep;
        }

    }
}
