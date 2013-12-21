using System;

namespace BuilderFramework.Exceptions
{
    public abstract class BuilderExeception : Exception
    {
        protected BuilderExeception(string message) : base(message)
        {
            
        }
    }
}