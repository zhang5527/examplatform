using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using examinationPlatform.Common;
using Json.Net;
using Microsoft.Extensions.Caching.Memory;

namespace examinationPlatform.Controllers
{
    public class UserController : Controller
    {
        private IMemoryCache MemoryCache;
        private ITestStorage Test;
        private IUserService UserService;
        private IExamService ExamService;
        public UserController(IMemoryCache memoryCache,IUserService userService,ITestStorage testStorage,IExamService exam)
        {
            ExamService = exam;
            Test = testStorage;
            MemoryCache = memoryCache;
            UserService = userService;
        }
        public IActionResult Index()
        {
            Users user = UserService.GetUser(HttpContext.Session.GetString("account"));
         
            return View();
        }       
        public IActionResult login ()
        {
            if (HttpContext.Request.Method == "GET")
            {
                return View();
            }
            else
            {
                try
                {
                    string account = HttpContext.Request.Form["UserAccount"];
                    string password = HttpContext.Request.Form["UserPassword"];
                    if (UserService.login(account, password) != null)
                    {
                        HttpContext.Session.SetString("account", account);
                        return Redirect("/user/index");
                    }
                    else
                    {
                        return new ContentResult()
                        {
                            Content = "<script>alert('账户信息有误');history.go(-1);</script>",
                            ContentType = "text/html;charset=utf-8"
                        };
                    }
                }
                catch (Exception e)
                {

                    return new ContentResult()
                    {
                        Content = "<script>alert('账户信息有误');history.go(-1);</script>",
                        ContentType = "text/html;charset=utf-8"
                    };
                }

             }
        }

        public IActionResult  TestContent()
        {
            int point =0;
            if (HttpContext.Request.Query["point"] != "")
                point = Convert.ToInt32(HttpContext.Request.Query["point"]);
            ViewData["point"] = point;
            if(MemoryCache.Get<List<TestStorage>>("tests").Count==0)
             return new ContentResult()
             {
                    Content = "<script>alert('0条记录,请重新选择');history.go(-1);</script>",
                    ContentType = "text/html;charset=utf-8"
             };
            if((point+1)> MemoryCache.Get<List<TestStorage>>("tests").Count)
            {
                return new ContentResult()
                {
                    Content = "<script>alert('已经做完');location.replace('/html/test_begin.html');</script>",
                    ContentType = "text/html;charset=utf-8"
                };
            }
            var test = MemoryCache.Get<List<TestStorage>>("tests")[point];
            return View(test);
        }

        public IActionResult CollectTest()
        {
            int TestId = Convert.ToInt32(HttpContext.Request.Form["id"]);
            int UserId= UserService.GetUser(HttpContext.Session.GetString("account")).Id;
            if (UserService.collect(UserId, TestId))
                return Content("1");
            else
                return Content("0");
        }
      
        public IActionResult TestStart()
        {
            string scope = HttpContext.Request.Form["scope"];
            string difficulty = HttpContext.Request.Form["difficulty"];
            int count = Convert.ToInt32(HttpContext.Request.Form["count"]);
            string type= HttpContext.Request.Form["type"].ToString();
            Users user = UserService.GetUser(HttpContext.Session.GetString("account"));
            var tests= UserService.GetTests(type, count, scope, difficulty, user).ToList();
            MemoryCache.Set<List<TestStorage>>("tests", tests);
            MemoryCache.Set("type", type);
            return Redirect("/user/TestContent");
        }

        public IActionResult RecordHistory()
        {
            int state = Convert.ToInt32(HttpContext.Request.Query["state"]);
            string answer = HttpContext.Request.Form["answer"];
            int testid = Convert.ToInt32(HttpContext.Request.Form["testid"]);
            int userId = UserService.GetUser(HttpContext.Session.GetString("account")).Id;
            UserService.RecordTest(testid, answer, userId, state);
            return Content("1");
        }

        public IActionResult exam_list()
        {
            return View(UserService.ExamList().ToList());
        }

        public IActionResult exam_begin()
        {
            int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            int userId = UserService.GetUser(HttpContext.Session.GetString("account")).Id;
            HttpContext.Session.SetInt32("examID", id);
            UserService.RefreshStorage(id, userId);
            var exams = UserService.GetTestsOfExam(id);
            return View(exams);
        }
        public IActionResult exam_Result()
        {
            string js = HttpContext.Request.Form["data"];
            var jss = JsonNet.Deserialize<List<ExamTest>>(js);
            int testcount = jss.Count();  
            List<UserHistory> histories = new List<UserHistory>();
            Counter judegeC = new Counter() { All=0,right=0};
            Counter choiceC = new Counter() { All = 0, right = 0 };
            Counter blankC = new Counter() { All = 0, right = 0 };
            int EachScore = 100 /jss.Count();
            int score=0;
            Users user = UserService.GetUser(HttpContext.Session.GetString("account"));
            TestStorage test;
            foreach (var item in jss)
            {
                if (item.value == "") item.value = "未填写";
                test = Test.FindTestById(Convert.ToInt32(item.name));
                if (test.Type == "choice")
                {
                    choiceC.All++;
                    if(test.Answer==item.value)
                    {
                        score += EachScore;
                        choiceC.right++;
                        histories.Add(new UserHistory { Answer = item.value, State = 1, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                    else
                    {
                        histories.Add(new UserHistory { Answer = item.value, State = 0, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                }
                else if (test.Type == "judege")
                {
                    judegeC.All++;
                    if (test.Answer == item.value)
                    {
                        score += EachScore;
                        judegeC.right++;
                        histories.Add(new UserHistory { Answer = item.value, State = 1, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                    else
                    {
                        histories.Add(new UserHistory { Answer = item.value, State = 0, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                }
                else if (test.Type == "blank")
                {
                    blankC.All++;
                    if (test.Answer == item.value)
                    {
                        score += EachScore;
                        blankC.right++;
                        histories.Add(new UserHistory { Answer = item.value, State = 1, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                    else
                    {
                        histories.Add(new UserHistory { Answer = item.value, State = 0, UsersId = user.Id, ExamId = HttpContext.Session.GetInt32("examID"), TestId = test.Id });
                    }
                }
            }
            UserService.SaveExam(histories);
            MemoryCache.Set<Counter>("judegeC", judegeC);
            MemoryCache.Set<Counter>("choiceC", choiceC);
            MemoryCache.Set<Counter>("blankC", blankC);
            MemoryCache.Set<Counter>("choiceC", choiceC);
            MemoryCache.Set("score", score);
            MemoryCache.Set("EachScore", EachScore);
            MemoryCache.Set("name", ExamService.FindExamById(Convert.ToInt32(HttpContext.Session.GetInt32("examID"))).Name);
            //ViewBag.judegeC = judegeC;
            //ViewBag.choiceC = choiceC;
            //ViewBag.blankC = blankC;
            //ViewBag.score = score;
            //ViewBag.eachscore = EachScore;
            //ViewBag.name =;
            return Content("/user/exam_Result1");
            //return View("/user/resultpage?examid"+ HttpContext.Session.GetInt32("examID").ToString());
        }

        public IActionResult exam_Result1() {
            ViewBag.judegeC = MemoryCache.Get<Counter>("judegeC"); 
            ViewBag.choiceC = MemoryCache.Get<Counter>("choiceC");
            ViewBag.blankC = MemoryCache.Get<Counter>("blankC");
            ViewBag.score = MemoryCache.Get("score");
            ViewBag.eachscore = MemoryCache.Get("eachscore");
            ViewBag.name = MemoryCache.Get("name");
            return View();
        
        }

        public IActionResult ResultPage()
        {
            int userID;
            int examID;
            if (HttpContext.Request.Query["userid"] == "")
            {
                userID = Convert.ToInt32(HttpContext.Request.Query["userid"]);
            }
            else
            {
                userID= UserService.GetUser(HttpContext.Session.GetString("account")).Id;
            }
            examID= Convert.ToInt32(HttpContext.Request.Query["examid"]);
            var tests = UserService.GetExamHistory(userID, examID);
            return View(tests);
        }
    }
}