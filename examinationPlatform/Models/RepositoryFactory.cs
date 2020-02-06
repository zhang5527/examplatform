
using examinationPlatform;
using examinationPlatform.Interface;

namespace examinationPlatform
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<T> CreateRepository<T>(IExam_Platform_Context mydbcontext) where T : class
        {
            return new Repository<T>(mydbcontext);
        }
    }
}