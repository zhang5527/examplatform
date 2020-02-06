using examinationPlatform.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Service
{
    public interface IBaseService
    {
        IRepository<T> CreateService<T>() where T : class, new();
    }
}
