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

            ViewBag.log = "Login";

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
                return RedirectToAction("index","Forum");

            }

            return View("Register", user);

        }

        [HttpGet]
        [Route("login")]
        public IActionResult Loginpage(){

            HttpContext.Session.Clear();

            ViewBag.log = "Login";

            return View("Login");
        }
        [HttpPost]
        [Route("signin")]
        public IActionResult Login(LoginViewModel user) {
            if(ModelState.IsValid){
                if(!_context.Users.Any(u => u.email == user.identity) && !_context.Users.Any(t =>t.username == user.identity))
                {
                    ModelState.AddModelError("identity", "Invalid Login Information");
                    return View("Login", user);

                }
                var lUser = _context.Users.SingleOrDefault(u => u.email == user.identity);
                var lHasher = new PasswordHasher<User>();
                var UUser = _context.Users.SingleOrDefault(uu => uu.username == user.identity);
                var Uhasher = new PasswordHasher<User>();
                if(lUser != null){
                    if(0 !=lHasher.VerifyHashedPassword(lUser, lUser.password, user.password))
                {
                    HttpContext.Session.SetInt32("Id", lUser.Id);
<<<<<<< HEAD

=======
>>>>>>> 764545461a0aa84d83fcf2710004625d47ffa0eb
                    HttpContext.Session.SetString("Username", lUser.username);
                    return RedirectToAction("index", "Forum");
                } 
<<<<<<< HEAD

=======
>>>>>>> 764545461a0aa84d83fcf2710004625d47ffa0eb
                }
               else{
                   if(0 !=Uhasher.VerifyHashedPassword(UUser,UUser.password, user.password)){
                    HttpContext.Session.SetInt32("Id", UUser.Id);
<<<<<<< HEAD

                    HttpContext.Session.SetString("Username", UUser.username);
                    return RedirectToAction("index", "Forum");

=======
                    HttpContext.Session.SetString("Username", UUser.username);
                    return RedirectToAction("Index", "Rallypoint");
>>>>>>> 764545461a0aa84d83fcf2710004625d47ffa0eb

                    }
               }
                
                ModelState.AddModelError("identity", "Invalid Login Information");
                return View("Login");
            }
<<<<<<< HEAD
            return View("Login");
        }
    }

       
}
=======
                return View("Login");
            }
           
        }
    }
>>>>>>> 764545461a0aa84d83fcf2710004625d47ffa0eb
