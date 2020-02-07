using System;
using System.Collections.Generic;
using System.Linq;
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
            return  CreateService<ExamStorage>().GetAll();
        }

        public ExamStorage FindExamById(int id)
        {
            return CreateService<ExamStorage>().GetById(id);
        }

        public void ModifyExam(ExamStorage Exam)
        {
            CreateService<ExamStorage>().Update(Exam);
        }
    }
}
