using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using examinationPlatform.Interface;
using examinationPlatform.Models;

namespace examinationPlatform.Service
{
    public class ExamService : BaseService,IExamService
    {
        public ExamService(IRepositoryFactory repositoryFactory, IExam_Platform_Context mydbcontext) : base(repositoryFactory, mydbcontext)
        {

        }

        public bool AddExam(ExamStorage Exam)
        {
           return  CreateService<ExamStorage>().Add(Exam)!=null;
        }

        public void DeleteExam(int id)
        {
            CreateService<ExamStorage>().Delete(id);
        }

        public IQueryable<ExamStorage> FindAllExam(string search = "")
        {
            if(search==""||search==null)
            return  CreateService<ExamStorage>().GetAll().Include(a=>a.PublisherNavigation);
            else
            return CreateService<ExamStorage>().Where(a=>a.Group.Contains(search)||a.Name.Contains(search));
        }

        public ExamStorage FindExamById(int id)
        {
            return CreateService<ExamStorage>().Entities.Include(a => a.PublisherNavigation).Include(a => a.ExamContent).ThenInclude(a=>a.Test).Where(a=>a.Id==id).FirstOrDefault();
        }

        public void ModifyExam(ExamStorage Exam)
        {
            CreateService<ExamStorage>().Update(Exam);
        }

        public void AddTestToExam(List<ExamContent> contents)
        {
             CreateService<ExamContent>().AddRange(contents);
        }
        public ICollection<ExamContent> FindAllTest(int ExamId) 
        {
            return CreateService<ExamStorage>().Entities.Include(a => a.ExamContent).ThenInclude(a => a.Test).FirstOrDefault(a=>a.Id==ExamId).ExamContent;
        }

 

        public void DeleteTest(int ExamId, int TestId)
        {
            CreateService<ExamContent>().Delete(a => a.ExamId == ExamId && a.TestId == TestId);
        }
    }
}
