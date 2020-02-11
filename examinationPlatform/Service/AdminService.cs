using examinationPlatform.Interface;
using examinationPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Service
{
    public class AdminService : BaseService,IAdminService
    {
        public AdminService(IRepositoryFactory repositoryFactory, IExam_Platform_Context mydbcontext) : base(repositoryFactory, mydbcontext)
        {

        }

        public IQueryable<object> GetAll()
        {
            return CreateService<Users>().GetAll().Where(a=>a.UserGroup!=2).Select(a => new
            {
                a.Nickname,
                a.name,
                a.major,
                a.class_,
                a.Id,
                a.College,
                a.UserAccount
            })               
            ;
        }

        public bool login(Users user)
        {
           return CreateService<Users>().Count(a=>a.UserAccount==user.UserAccount&&a.UserPassword==user.UserPassword&&a.UserGroup==2)>0;
        }
        public bool AddUser(Users user)
        {
            return CreateService<Users>().Add(user) != null;
        }
        public bool JudgeUserExist(string account)
        {
            return CreateService<Users>().Count(a => a.UserAccount == account) > 0;
        }

        public bool DeleteUser(int id)
        {
            try
            {
                CreateService<Users>().Delete(id, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
        public void ModifyUser(Users user)
        {
            CreateService<Users>().Update(user);
        }

        public IQueryable<object> FindUser(string name)
        {
           return CreateService<Users>().Where(a => a.name == name).Select(a=>new {
               a.Nickname,
               a.name,
               a.major,
               a.class_,
               a.Id,
               a.College,
               a.UserAccount
           });
        }

        public Users GetByAccount(string account)
        {
            return CreateService<Users>().FirstOrDefault(a => a.UserAccount == account);
        }

        public IQueryable<Users> GetAdmins()
        {
            return CreateService<Users>().Where(a => a.UserGroup == 2);
        }
             
    }
}
