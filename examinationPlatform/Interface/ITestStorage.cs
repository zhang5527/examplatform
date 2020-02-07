using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Common;
using examinationPlatform.Models;

namespace examinationPlatform.Interface
{
    public interface ITestStorage
    {
        bool AddTest(TestStorage test);

        void ModifyTest(TestStorage test);

        void DeleteTest(int id);

        IQueryable<TestStorage> FindTestBySort(TestSort sort);

        IQueryable<TestStorage> FindTestBySort(TestSort sort,string search);
        TestStorage FindTestById(int id);

        bool JudgeIsExist(string title);
    }
}
