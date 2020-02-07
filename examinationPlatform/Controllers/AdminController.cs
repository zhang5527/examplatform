using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using examinationPlatform.Common;
using examinationPlatform.Interface;
using examinationPlatform.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace examinationPlatform.Controllers
{
    public class AdminController : Controller
    {

        IAdminService Admin;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminController(IAdminService admin, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Admin = admin;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult login(Users user)
        {
            if (HttpContext.Request.Method == "GET")
            {
                return View();                
            }
            else
            {
                if (Admin.login(user)) {
                    HttpContext.Session.SetString("admin", user.UserAccount);
                    return Redirect("/admin/index");
                }
                else  
                return new ContentResult()
                {
                    Content = "<script>alert('账户信息有误');history.go(-1);</script>",
                    ContentType = "text/html;charset=utf-8"
                };

            }

        }
        public IActionResult GetUsers()
        { 
                return View();      
        }
        public IActionResult GetUsersJson()
        {
            var users = Admin.GetAll();
            return Json(new
            {
                code = 0,
                msg = 123,
                count = users.Count(),
                data=users 
            }
            );
        }
        [HttpGet]
        public IActionResult FindUser(string name)
        {
          var user=Admin.FindUser(name);
            return Json(new
            {
                code = 0,
                msg = 123,
                count = 1,
                data = user
            }
        );
        }
        [HttpPost]
        public IActionResult AddOneUser([FromBody] Users user)
        {
            if (Admin.AddUser(user)) {
                return Content("1");            
            }
            else
            {
                return Content("2");
            }
        }
        [HttpPost]
        public IActionResult ModifyUser([FromBody] Users user)
        {
            Admin.ModifyUser(user);
            return Content("1");
        }
        [HttpPost]
        public IActionResult RemoveUsers()
        {
           string id=HttpContext.Request.Form["id"];
           string[] ids= id.Split(",");
            for (int i = 0; i < ids.Length; i++)
            {
                if (!Admin.DeleteUser(Convert.ToInt32(ids[i]))) return Content("0");
            } 
           return  Content("1");
        }
        public IActionResult AddManyUserFromExcel(IFormFile file)
        {
            //string rootdir = AppContext.BaseDirectory;
            //DirectoryInfo Dir = Directory.GetParent(rootdir);
            //string root = Dir.Parent.Parent.FullName;
            string newFileName = System.Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //随机生成新的文件名
            string path = "/upload/" + newFileName;
            string filepath = _hostingEnvironment.WebRootPath + @$"\{path}";

            using (FileStream fs = System.IO.File.Create(filepath))//创建文件流
            {
                file.CopyTo(fs);//将上载文件的内容复制到目标流
                fs.Flush();//清除此流的缓冲区并导致将任何缓冲数据写入
            }
            DataTable a = ExcelHelper.ExcelToTable(filepath);
            Users user;
            foreach (DataRow item in a.Rows)
            {
                try
                {
                    user= new Users() { UserAccount = item[0].ToString(), name = item[1].ToString(), College = item[2].ToString(), class_ = item[3].ToString(), major = item[4].ToString(), UserPassword = item[0].ToString().Substring(item[0].ToString().Length - 6, 6) };
                    if (Admin.JudgeUserExist(user.UserAccount)) continue;
                    Admin.AddUser(user);
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
            return Content("1");
        }
    }
}