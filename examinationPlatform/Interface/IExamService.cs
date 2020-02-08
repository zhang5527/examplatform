using examinationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examinationPlatform.Interface
{
    public interface IExamService
    {
        bool AddExam(ExamStorage Exam);

        void ModifyExam(ExamStorage Exam);

        void DeleteExam(int id);

        IQueryable<ExamStorage> FindAllExam(string search="");

        ExamStorage FindExamById(int id);

        void AddTestToExam(List<ExamContent> contents);

        ICollection<ExamContent> FindAllTest(int ExamId);

        void DeleteTest(int ExamId,int TestId);
    }
}
