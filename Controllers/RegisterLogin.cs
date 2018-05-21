using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rallypoint.Models;

namespace Rallypoint.Controllers
{
    public class RegisterLoginController : Controller
    {

        [HttpGet]
        [Route("")]
        public IActionResult Register()
        {
            // Setting default nav link to login, should change after login
            HttpContext.Session.SetString("Log", "Login");
            string Log = HttpContext.Session.GetString("Log");
            ViewBag.log = Log;

            ViewBag.errors = "";

            return View("Register");
        }

        [HttpGet]
        [Route("existing-user")]
        public IActionResult ExistingUser() {
            // ViewBag.log = "login";

            return View("Login");
        }

        // [HttpPost]
        // [Route("process-register")]
        // public IActionResult ProcessRegister(User model, string Password_Confirm) {

    }
}