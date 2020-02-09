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
         Users login(string account,string password);

        Users GetUser(string account);
        bool collect(int UserId, int TestId);
        IQueryable<TestStorage> GetTests(string sort,int count,string method,string difficulty,Users user);

        void RecordTest(int testid, string answer, int userid,int state);
    }
}
