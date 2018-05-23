using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rallypoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Rallypoint.Controllers
{
    public class RegisterLoginController : Controller
    {
        private RallypointContext _context;

        public RegisterLoginController(RallypointContext context){
            _context = context;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult ShowPage(){
            return View("Register");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Register(RegisterViewModel user)
        {
            User ExistingUser = _context.Users.SingleOrDefault( u => u.email == user.email);
            if(ExistingUser != null){
                ModelState.AddModelError("email", "This Email already exists");
                return View("Register", user);
            }

            User ExistingUsername = _context.Users.SingleOrDefault(u => u.username == user.username);
            if(ExistingUsername != null){
                ModelState.AddModelError("username", "This username already exists, please use another username!!");
                return View("Register", user);

            }
            if(ModelState.IsValid)
            {
                PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                user.password = Hasher.HashPassword(user, user.password);
                User newuser = new User{
                    first_name = user.first_name,
                    last_name = user.last_name,
                    username = user.username,
                    email = user.email,
                    password = user.password,

                };
                _context.Users.Add(newuser);
                _context.SaveChanges();
                User ReturnedUser = _context.Users.SingleOrDefault(userID => userID.email == user.email);
                HttpContext.Session.SetInt32("Id", ReturnedUser.Id);
                return RedirectToAction("Index","Rallypoint");

            }

            return View("Register", user);

        }


        [HttpGet]
        [Route("login")]
        public IActionResult Login(LoginViewModel user) {
            if(ModelState.IsValid){
                if(!_context.Users.Any(u => u.email == user.identity) || !_context.Users.Any(t =>t.username == user.identity))
                {
                    ModelState.AddModelError("identity", "Invalid Login Information");
                    return View("Login");

                }
                var lUser = _context.Users.SingleOrDefault(u => u.email == user.identity);
                var lHasher = new PasswordHasher<User>();
                var UUser = _context.Users.SingleOrDefault(uu => uu.username == user.identity);
                var Uhasher = new PasswordHasher<User>();
                if(0 !=lHasher.VerifyHashedPassword(lUser, lUser.password, user.password))
                {
                    HttpContext.Session.SetInt32("Id", lUser.Id);
                    return RedirectToAction("Index","Rallypoint");
                }
                if(0 !=Uhasher.VerifyHashedPassword(UUser,UUser.password, user.password)){
                    HttpContext.Session.SetInt32("Id", UUser.Id);
                    return RedirectToAction("Index","Rallypoint");

                }
                ModelState.AddModelError("identity", "Invalid Login Information");
                return View("Login");
            }

            return View("Login");
        }

       
    }
}