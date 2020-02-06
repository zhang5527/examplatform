using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using Newtonsoft.Json.Linq;
using examinationPlatform.Common;

namespace examinationPlatform.Controllers
{
    public class TestController : Controller
    {
        private ITestStorage Test;
        public TestController(ITestStorage test)
        {
            Test = test;
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
        [HttpPost]
        public IActionResult AddTest([FromBody] TestStorage test)
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
            if (Test.AddTest(test)) {
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

    }
}