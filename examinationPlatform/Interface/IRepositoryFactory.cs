using examinationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Interface
{
    public interface IRepositoryFactory
    {
        IRepository<T> CreateRepository<T>(IExam_Platform_Context context) where T : class;
    }
}
