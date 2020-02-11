using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using examinationPlatform.Common;
using examinationPlatform.Interface;
using examinationPlatform.Models;

namespace examinationPlatform.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepositoryFactory repositoryFactory, IExam_Platform_Context mydbcontext) : base(repositoryFactory, mydbcontext)
        {

        }
        public IQueryable<TestStorage> GetTests(string sort, int count, string method, string difficulty, Users user)
        {
            //根据需求返回
            switch (method)
            {
                case "全部":
                    if (difficulty == "不限")
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.Type == sort).Take(count);
                    else
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.difficulty == difficulty && a.Type == sort).Take(count);
                case "未做":
                    if (difficulty == "不限")
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.UserHistory.Where(a => a.UsersId == user.Id).Count() == 0 && a.Type == sort).Take(count);
                    else
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.UserHistory.Where(a => a.UsersId == user.Id).Count() == 0 && a.difficulty == difficulty && a.Type == sort).Take(count);
                case "错误":
                    if (difficulty == "不限")
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.UserHistory.Where(a => a.UsersId == user.Id && a.State == 0).Count() != 0 && a.Type == sort).Take(count);
                    else
                        return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.UserHistory.Where(a => a.UsersId == user.Id && a.State == 0).Count() != 0 && a.difficulty == difficulty && a.Type == sort).Take(count);
                default:
                    break;
            }
            return CreateService<TestStorage>().Entities.Include(a => a.UserHistory).ThenInclude(a => a.Users).Where(a => a.difficulty == difficulty && a.Type == sort).Take(count);
        }

        public Users GetUser(string account)
        {
            return CreateService<Users>().Entities.Include(a => a.UserHistory).FirstOrDefault(a => a.UserAccount == account);
        }

        public Users login(string account, string password)
        {
            return CreateService<Users>().Entities.Include(a => a.UserHistory).FirstOrDefault(a => a.UserAccount == account && a.UserPassword == password);
        }

        public bool collect(int UserId, int TestId)
        {
            if (CreateService<Collection>().Count(a => a.TestId == TestId && a.UsersId == UserId) != 1)
                return CreateService<Collection>().Add(new Collection { TestId = TestId, UsersId = UserId }) != null;
            return false;
        }

        public void RecordTest(int testid, string answer, int userid, int state)
        {
            if (CreateService<UserHistory>().Count(a => a.TestId == testid && a.UsersId == userid && a.State == state) != 1)
                CreateService<UserHistory>().Add(new UserHistory { UpdateDate = DateTime.Now.ToString("yyyy-MM-dd"), Answer = answer, UsersId = userid, TestId = testid, State = state });
        }

        public IQueryable<ExamStorage> ExamList()
        {
            return CreateService<ExamStorage>().Where(a => a.Group != "验证码");
        }

        public ExamStorage GetTestsOfExam(int ExamId)
        {
             return  CreateService<ExamStorage>().Entities.Include(a => a.ExamContent).ThenInclude(a => a.Test).FirstOrDefault(a => a.Id == ExamId);     
        }


        public void SaveExam(List<UserHistory> histories)
        {
              CreateService<UserHistory>().AddRange(histories);
        }

        public void RefreshStorage(int ExamId,int UserId)
        {
            CreateService<UserHistory>().Delete(a => a.ExamId == ExamId && a.UsersId == UserId);
        }

        public IQueryable<UserHistory> GetExamHistory(int ExamId,int UserId)
        {
           return  CreateService<UserHistory>().Entities.Include(a => a.Test).Where(a => a.UsersId == UserId && a.ExamId == ExamId);
        }

        public void AddExamHistory(int userid,int examid,int wrongcount,int score)
        {
            CreateService<ExamRecord>().Add(new ExamRecord { UsersId = userid, PublishDate=DateTime.Now.ToString("yyyy-MM-dd"),ExamId = examid, WrongCount = wrongcount, Score = score.ToString() });
        }

        public IQueryable<ExamRecord> GetExamHistory()
        {
            return CreateService<ExamRecord>().GetAll(a=>a.PublishDate);
        }

        public ExamStorage GetExamByCode(string code)
        {
            return CreateService<ExamStorage>().Where(a => a.Code == code).FirstOrDefault();
        }

        public IQueryable<Collection> GetCollectionById(int userid)
        {
            return CreateService<Collection>().Entities.Include(a=>a.Test).Where(a => a.UsersId == userid);
        }

        public IQueryable<UserHistory> GetWrongHistory(int userid)
        {
            return CreateService<UserHistory>().Entities.Include(a => a.Test).Where(a => a.UsersId == userid && a.ExamId == null);    
        }
    }
}
