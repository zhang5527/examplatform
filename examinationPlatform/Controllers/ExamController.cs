using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace examinationPlatform.Controllers
{
    public class ExamController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITestStorage Test;
        private readonly IExamService Exam;
     
        public ExamController(IWebHostEnvironment hostEnvironment,IExamService examService,ITestStorage testStorage)
        {
            _hostingEnvironment = hostEnvironment;
            Test = testStorage;
            Exam = examService;
        }
        public IActionResult GetExams()
        {
            return View();
        }

    }
}