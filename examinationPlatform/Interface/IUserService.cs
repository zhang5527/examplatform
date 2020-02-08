using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Models;
using examinationPlatform.Common;
namespace examinationPlatform.Interface
{
    public interface IUserService
    {
        bool login(Users user);

        void GetTests(Common.TestSort sort,int count,string method);
    }
}
