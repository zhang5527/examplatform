using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using examinationPlatform.Models;
using examinationPlatform.Interface;
using examinationPlatform.Service;
namespace examinationPlatform.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login()
        {

            return View();
        }

        public IActionResult Begin()
        {
            return View();
        }

        public IActionResult ChoiceTest()
        {
            
            return View();
        }
    }
}