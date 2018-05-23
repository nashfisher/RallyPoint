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

namespace Rallypoint.Controllers{

    public class ForumController:Controller{

        private RallypointContext _context;
        public ForumController(RallypointContext context){
            _context = context;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Index(){
            // for testing individual user
            var CurrentUser = _context.Users.SingleOrDefault(u => u.Id == 1);
            var test = _context.Posts;
            ViewBag.RecentPost = test;
            return View("Index","_ForumLayout");
        }

        [HttpGet]
        [Route("test/create")]
        public IActionResult CreatePost()
        {
            var CurrentUser = _context.Users.SingleOrDefault(u => u.Id == 1);
            return View();
        }

        [HttpPost]
        [Route("test/process")]
        public IActionResult SavePost(Post newpost)
        {
            if(ModelState.IsValid)
            {
                newpost.UserId = 1;
                _context.Posts.Add(newpost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("CreatePost",newpost);
        }
    }
}