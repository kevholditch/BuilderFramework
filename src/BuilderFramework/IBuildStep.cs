namespace BuilderFramework
{
    public interface IBuildStep
    {
        void Commit();
        void Rollback();
    }
}