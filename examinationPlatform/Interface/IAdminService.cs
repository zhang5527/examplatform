using examinationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Interface
{
    public interface IAdminService
    {
        bool login(Users user);
        IQueryable<object> GetAll();

        public Users GetByAccount(string account);

        public IQueryable<object> FindUser(string name);
        public bool AddUser(Users user);

        public void ModifyUser(Users user);        
        
        public bool JudgeUserExist(string account);

        public bool DeleteUser(int id);
    }
    
}
