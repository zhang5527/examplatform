using examinationPlatform.Common;
using examinationPlatform.Interface;
using examinationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Service
{
    public class TestService : BaseService,ITestStorage
    {
        public TestService(IRepositoryFactory repositoryFactory, IExam_Platform_Context mydbcontext) : base(repositoryFactory, mydbcontext)
        {

        }
        public bool AddTest(TestStorage test)
        {
            return CreateService<TestStorage>().Add(test) != null;
        }


        public void DeleteTest(int id)
        {
            CreateService<TestStorage>().Delete(id);
        }

        public IQueryable<TestStorage> FindTestBySort(TestSort sort)
        {
            return CreateService<TestStorage>().Where(a => a.Type == sort.ToString());
        }

        public TestStorage FindTestById(int id)
        {
            return CreateService<TestStorage>().GetById<int>(id);
        }

        public void ModifyTest(TestStorage test)
        {
            CreateService<TestStorage>().Update(test);
        }

        public IQueryable<TestStorage> FindTestBySort(TestSort sort, string search)
        {
            return CreateService<TestStorage>().Where(a => a.Type == sort.ToString() && (a.Title.Contains(search)||a.Content.Contains(search)||a.Subject.Contains(search)));
        }

        public List<TestStorage> FindAllTest(string search)
        {
            if (search == null) return CreateService<TestStorage>().GetAll().ToList();
            switch (search.Trim(' '))
            {
                case "填空":
                    search = "blank";
                    break;
                case "选择":
                    search = "choice";
                    break;
                case "判断":
                    search = "judege";
                    break;
                case "问答":
                    search = "answer";
                    break;
                default:
                    break;
            }
            return CreateService<TestStorage>().Where(a => (a.Type.Contains(search)||a.Title.Contains(search) || a.Content.Contains(search) || a.Subject.Contains(search))).ToList();
        }

        public bool JudgeIsExist(string title)
        {
            return CreateService<TestStorage>().Count(a => a.Title == title) > 0;      
        }
    }
}
