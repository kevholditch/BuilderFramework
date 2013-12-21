using System;

namespace BuilderFramework.Tests.TestTypes
{
    public class Step1 : IBuildStep
    {
        private readonly Action<Type, string> _callInterceptor;

        public Step1(Action<Type, string> callInterceptor)
        {
            _callInterceptor = callInterceptor;
        }

        public void Commit()
        {
            _callInterceptor(GetType(), "Commit");
        }
        public void Rollback()
        {
            _callInterceptor(GetType(), "Rollback");
        }
    }

    public class Step2 : IBuildStep
    {
        private readonly Action<Type, string> _callInterceptor;

        public Step2(Action<Type, string> callInterceptor)
        {
            _callInterceptor = callInterceptor;
        }

        public void Commit()
        {
            _callInterceptor(GetType(), "Commit");
        }
        public void Rollback()
        {
            _callInterceptor(GetType(), "Rollback");
        }
    }
}
