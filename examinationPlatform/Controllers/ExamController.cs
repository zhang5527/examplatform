using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using examinationPlatform.Common;
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

        public IActionResult SearchTest()
        {
            int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            ViewBag.Exam = id;
            var tests = Exam.FindAllTest(id);
            ViewBag.tests= tests;
            ViewBag.count = tests.Count();
            return View();
        }
        [HttpPost]
        public IActionResult deletetest()
        {
            int id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            int exam = Convert.ToInt32(HttpContext.Request.Query["exam"]);
            Exam.DeleteTest(exam, id);
            return Content("1");
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
           string search = HttpContext.Request.Query["search"];
            var list =Test.FindAllTest(search);
            foreach (var item in list)
            {
                if (item.Type == Common.TestSort.choice.ToString())
                {
                    item.Content = $"选项A： {item.Content.Split('`')[0]}；选项B： {item.Content.Split('`')[1]}；" +
                        $"选项C： {item.Content.Split('`')[2]}；选项D： {item.Content.Split('`')[3]}；";
                }
                switch (item.Type)
                {
                    case "choice":
                        item.Type = "选择题";
                        break;
                    case "answer":
                        item.Type = "问答题";
                        break;
                    case "blank":
                        item.Type = "填空题";
                        break;
                    case "judege":
                        item.Type = "判断题";
                        break;
                    default:
                        break;
                }
            }
            var data = list.Select(a => new { a.Type, a.Content, a.Title, a.Id, a.difficulty, a.Answer });
            return Json( new {
                 data,
                code = 0,
                msg = 123,
                count =data.Count() ,
            });
        }

        public IActionResult ChooseTest()
        {
            int ExamId = Convert.ToInt32(HttpContext.Request.Query["exam"]);
            var exam= Exam.FindExamById(ExamId);
            ViewBag.Count1 = exam.ExamContent.Where(a => a.Test.Type == Common.TestSort.choice.ToString()).Count();
            ViewBag.Count2 = exam.ExamContent.Where(a => a.Test.Type == Common.TestSort.blank.ToString()).Count();
            ViewBag.Count3 = exam.ExamContent.Where(a => a.Test.Type == Common.TestSort.judege.ToString()).Count();
            ViewBag.Count4 = exam.ExamContent.Where(a => a.Test.Type == Common.TestSort.answer.ToString()).Count();
            ViewBag.id = exam.Id;
            return View();
        }
        [HttpPost]
        public IActionResult AddTestsToExam()
        {
            var exam = Exam.FindExamById(Convert.ToInt32(HttpContext.Request.Query["exam"]));
            var TestIds = HttpContext.Request.Form["id"].ToString().Split(',');
            List<ExamContent> contents = new List<ExamContent>();
            for (int i = 0; i < TestIds.Length; i++)
            {
                contents.Add(new ExamContent { ExamId = exam.Id, TestId = Convert.ToInt32(TestIds[i]) });
            }
            Exam.AddTestToExam(contents);
            return Content("1");
        }
    }
}