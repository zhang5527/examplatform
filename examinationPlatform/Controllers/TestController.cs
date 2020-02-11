using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using Newtonsoft.Json.Linq;
using examinationPlatform.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;

namespace examinationPlatform.Controllers
{
    public class TestController : Controller
    {
        private ITestStorage Test;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IExamService Exam;
        private readonly IAdminService admin;
        public TestController(IExamService examService,ITestStorage test, IWebHostEnvironment hostingEnvironment, IAdminService adminService)
        {
            Exam = examService;
            Test = test;
            _hostingEnvironment = hostingEnvironment;
            admin = adminService;
        }
        public IActionResult TestIndex()
        {       
            return View();
        }
   
        public IActionResult ChoiceList()
        {
            var command = HttpContext.Request.Query["search"].ToString();
            IQueryable<TestStorage> choices;
             if (command=="")
                choices = Test.FindTestBySort(Common.TestSort.choice);
            else
            {
                choices = Test.FindTestBySort(Common.TestSort.choice, command);
            }      
            ViewBag.choices = choices;
            ViewBag.count = choices.Count();
            return View();
        }

        public IActionResult BlankList()
        {
            var command = HttpContext.Request.Query["search"].ToString();
            IQueryable<TestStorage> blanks;
            if (command == "")
                blanks = Test.FindTestBySort(Common.TestSort.blank);
            else
            {
                blanks = Test.FindTestBySort(Common.TestSort.blank, command);
            }
            ViewBag.items = blanks;
            ViewBag.count = blanks.Count();
            return View();
        }

        public IActionResult JudgeList()
        {
            var command = HttpContext.Request.Query["search"].ToString();
            IQueryable<TestStorage> judges;
            if (command == "")
                judges = Test.FindTestBySort(Common.TestSort.judege);
            else
            {
                judges = Test.FindTestBySort(Common.TestSort.judege, command);
            }
            ViewBag.items = judges;
            ViewBag.count = judges.Count();
            return View();
        }
        public IActionResult AnswerList()
        {
            var command = HttpContext.Request.Query["search"].ToString();
            IQueryable<TestStorage> answers;
            if (command == "")
                answers = Test.FindTestBySort(Common.TestSort.answer);
            else
            {
                answers = Test.FindTestBySort(Common.TestSort.answer, command);
            }
            ViewBag.items = answers;
            ViewBag.count = answers.Count();
            return View();
        }
        [HttpGet]
        public IActionResult EditChoice()
        {
            String id = HttpContext.Request.Query["id"];
            ViewBag.item =Test.FindTestById(Convert.ToInt32(id));
            return View();
        }
        [HttpGet]
        public IActionResult EditBlank()
        {
            String id = HttpContext.Request.Query["id"];
            ViewBag.item = Test.FindTestById(Convert.ToInt32(id));
            return View();
        }
        [HttpGet]
        public IActionResult EditJudge()
        {
            String id = HttpContext.Request.Query["id"];
            ViewBag.item = Test.FindTestById(Convert.ToInt32(id));
            return View();
        }
        [HttpGet]
        public IActionResult EditAnswer()
        {
            String id = HttpContext.Request.Query["id"];
            ViewBag.item = Test.FindTestById(Convert.ToInt32(id));
            return View();
        }
        [HttpPost]
        public IActionResult AddTest([FromBody] TestStorage test)
        {
            test.PublishDate = DateTime.Now.ToString("yyyy-MM-dd");
            string type = HttpContext.Request.Query["type"];
            string direct= HttpContext.Request.Query["direct"];
            string account = HttpContext.Session.GetString("admin");
            test.Publisher = admin.GetByAccount(account).name;
            if (direct != "") {
                test.Grade = "exam";          
            }
            switch (type)
            {
                case "choice":
                    test.Type = TestSort.choice.ToString();
                    break;
                case "blank":
                    test.Type = TestSort.blank.ToString();
                    break;
                case "judege":
                    test.Type = TestSort.judege.ToString();
                    break;
                case "answer":
                    test.Type = TestSort.answer.ToString();
                    break;
                default:
                    break;
            }
            var a = Test.AddTest(test);
            if (a!=null) {
                if (direct != "")
                {
                    Exam.AddTestToExam(new List<ExamContent>() { new ExamContent { ExamId = Convert.ToInt32(direct), TestId = a.Id } });
                }
                return Content("1");            
            }
            else
                return Content("0");
        }
        [HttpPost]
        public IActionResult ModifyTest([FromBody] TestStorage test)
        {
            test.PublishDate = DateTime.Now.ToString("yyyy-MM-dd");
            string type = HttpContext.Request.Query["type"];
            switch (type)
            {
                case "choice":
                    test.Type = TestSort.choice.ToString();
                    break;
                case "blank":
                    test.Type = TestSort.blank.ToString();
                    break;
                case "judege":
                    test.Type = TestSort.judege.ToString();
                    break;
                case "answer":
                    test.Type = TestSort.answer.ToString();
                    break;
                default:
                    break;
            }       
            Test.ModifyTest(test);
            return Content("1");
        }
        [HttpPost]
        public IActionResult DeleteTest()
        {
            string id = HttpContext.Request.Form["id"];
            string[] ids = id.Split(",");
            for (int i = 0; i < ids.Length; i++)
            {
                Test.DeleteTest(Convert.ToInt32(ids[i]));
            }
            return Content("1");
        }

        public IActionResult DeleteOneTest()
        {
            string id = HttpContext.Request.Form["id"];
            Test.DeleteTest(Convert.ToInt32(id));          
            return Content("1");
        }
        [HttpPost]
        public IActionResult AddImg(IFormFile file)
        {
            string newFileName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //随机生成新的文件名
            string path = "/upload/imgs/" + newFileName;
            string filepath = _hostingEnvironment.WebRootPath + @$"\{path}";
            string type = HttpContext.Request.Query["type"];
            using (FileStream fs = System.IO.File.Create(filepath))//创建文件流
            {
                file.CopyTo(fs);//将上载文件的内容复制到目标流
                fs.Flush();//清除此流的缓冲区并导致将任何缓冲数据写入
            }
            return Json(new
            {
                code=0,
                msg="",
                data = new
                {
                    src= path
                }
            });
        }
        public IActionResult AddTestsFromExcel(IFormFile file)
        {
            //string rootdir = AppContext.BaseDirectory;
            //DirectoryInfo Dir = Directory.GetParent(rootdir);
            //string root = Dir.Parent.Parent.FullName;
            string newFileName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //随机生成新的文件名
            string path = "/upload/" + newFileName;
            string filepath = _hostingEnvironment.WebRootPath + @$"\{path}";
            string type = HttpContext.Request.Query["type"];
            int FalseFlag = 0;
            using (FileStream fs = System.IO.File.Create(filepath))//创建文件流
            {
                file.CopyTo(fs);//将上载文件的内容复制到目标流
                fs.Flush();//清除此流的缓冲区并导致将任何缓冲数据写入
            }
            DataTable a = ExcelHelper.ExcelToTable(filepath);
            TestStorage test=new TestStorage();
            foreach (DataRow item in a.Rows)
            {
                try
                {
                    switch (type)
                    {
                        case "choice":
                            if (a.Columns.Count != 9) FalseFlag=1;
                            test = new TestStorage()
                            {
                                Title = item[0].ToString(),
                                Subject = item[1].ToString(),
                                Content = $"{item[2].ToString()}`{item[3].ToString()}`{item[4].ToString()}`{item[5].ToString()}",
                                Type = Common.TestSort.choice.ToString(),
                                Answer = item[6].ToString(),
                                Explain = item[7].ToString(),
                                difficulty=item[8].ToString()
                            };
                            break;
                        case "blank":
                            if (a.Columns.Count != 5) FalseFlag = 1;
                            test = new TestStorage()
                            {
                                Title = item[0].ToString(),                              
                                Type = Common.TestSort.blank.ToString(),
                                Subject=item[1].ToString(),
                                Answer = item[2].ToString(),
                                Explain = item[3].ToString(),
                                difficulty=item[4].ToString(),
                            };
                            break;
                        case "judege":
                            if (a.Columns.Count != 5) FalseFlag = 1;
                            test = new TestStorage()
                            {
                                Title = item[0].ToString(),
                                Type = Common.TestSort.judege.ToString(),
                                Subject = item[1].ToString(),
                                Answer = item[2].ToString(),
                                Explain = item[3].ToString(),
                                difficulty = item[4].ToString(),
                            };
                            break;
                        case "answer":
                            if (a.Columns.Count != 4) FalseFlag = 1;
                            test = new TestStorage()
                            {
                                Title = item[0].ToString(),
                                Type = Common.TestSort.answer.ToString(),
                                Subject = item[1].ToString(),
                                Answer = item[2].ToString(),
                                Explain = item[3].ToString(),
                                difficulty = item[4].ToString(),
                            };
                            break;
                        default:
                            break;
                    }
                    if (FalseFlag == 1) break;
                    test.PublishDate = DateTime.Now.ToString("yyyy-MM-dd");
                    if (Test.JudgeIsExist(item[0].ToString())) continue;
                    Test.AddTest(test);
                }
                catch (Exception e)
                {
                    return new ContentResult()
                    {
                        Content = "0",
                        ContentType = "text/html;charset=utf-8"
                    };
                }
            }
            System.IO.File.Delete(filepath);
            if(FalseFlag==1) return Content("0");
            return Content("1");
        }
    }
}