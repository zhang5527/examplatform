using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace examinationPlatform.Controllers
{
    public class ExamController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITestStorage Test;
        private readonly IExamService Exam;
        private readonly IAdminService admin;
     
        public ExamController(IWebHostEnvironment hostEnvironment,IExamService examService,ITestStorage testStorage,IAdminService adminService)
        {
            _hostingEnvironment = hostEnvironment;
            Test = testStorage;
            Exam = examService;
            admin = adminService;
        }
        public IActionResult GetExams()
        {
            string search = HttpContext.Request.Query["search"];
            if (search == "")
                ViewBag.exams = Exam.FindAllExam();
            else
                ViewBag.exams = Exam.FindAllExam(search);
            return View();
        }
        [HttpPost]
        public IActionResult AddExam([FromBody]ExamStorage exam)
        {
            string account = HttpContext.Session.GetString("admin");
            if (exam.Code != "") { exam.Group = "验证码";               
            }
            exam.Publisher = admin.GetByAccount(account).Id;
            exam.PublishTime = DateTime.Now.ToString("yyyy-MM-dd");
            Exam.AddExam(exam);
            return Content("1");
        }

        public IActionResult EditExam()
        {
            int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            ViewBag.exam = Exam.FindExamById(id);
            return View();
        }
        [HttpPost]
        public IActionResult ModifyExam([FromBody] ExamStorage exam)
        {
            if (exam.Code != "")
            {
                exam.Group = "验证码";
            }
            exam.PublishTime = DateTime.Now.ToString("yyyy-MM-dd");
            Exam.ModifyExam(exam);
            return Content("1");
        }

        [HttpPost]
        public IActionResult DeleteExam([FromBody] int id)
        {
            Exam.DeleteExam(id);
            return Content("1");
        }

        public IActionResult GetTestsJson()
        {
           Test.FindAllTest()
        }
    }
}