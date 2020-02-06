using examinationPlatform.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Service
{
    public class BaseService : IBaseService
    {
        private IRepositoryFactory _repositoryFactory;
        private IExam_Platform_Context _mydbcontext;
        public BaseService(IRepositoryFactory repositoryFactory, IExam_Platform_Context mydbcontext)
        {
            this._repositoryFactory = repositoryFactory;
            this._mydbcontext = mydbcontext;
        }

        public IRepository<T> CreateService<T>() where T : class, new()
        {
            return _repositoryFactory.CreateRepository<T>(_mydbcontext);
        }
    }
}
