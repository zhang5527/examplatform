using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using examinationPlatform.Service;
using Microsoft.Extensions.Caching.Memory;

namespace examinationPlatform.Controllers
{
    public class UserController : Controller
    {
        private IMemoryCache MemoryCache;

        private IUserService UserService;
        public UserController(IMemoryCache memoryCache,IUserService userService)
        {
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
    }
}