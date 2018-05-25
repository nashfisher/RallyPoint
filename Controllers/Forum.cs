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
        [Route("forum")]
        public IActionResult Index(){

            if (HttpContext.Session.GetString("Username") == null) {
                ViewBag.log = "Login";
            }
            else {
                ViewBag.log = HttpContext.Session.GetString("Username");
            }
            // for testing individual user
            int? sessionid = HttpContext.Session.GetInt32("Id");
            var CurrentUser = _context.Users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("Id"));
            var test = _context.Posts.Include(p => p.user);
            ViewBag.RecentPost = test;
            return View("Index","_ForumLayout");
        }

        [HttpGet]
        [Route("forum/create")]
        public IActionResult CreatePost()
        {
            ViewBag.log = HttpContext.Session.GetString("Username");

            var CurrentUser = _context.Users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("Id"));
            return View();
        }

        [HttpPost]
        [Route("forum/process")]
        public IActionResult SavePost(Post newpost)
        {
            if(ModelState.IsValid)
            {
                newpost.UserId = HttpContext.Session.GetInt32("Id");
                _context.Posts.Add(newpost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("CreatePost",newpost);
        }

        [HttpPost]
        [Route("forum/like/{postId}")]
        public IActionResult LikePost(int postId)
        {
            Like stuff = _context.Likes.SingleOrDefault(j => j.PostId == postId && j.UserId == HttpContext.Session.GetInt32("Id"));
            if(stuff != null){
                return RedirectToAction("Index");
            }
            else {
                
                Like likedstuff = new Like {
                    PostId = postId,
                    UserId =(int)HttpContext.Session.GetInt32("Id")
                };
                _context.Likes.Add(likedstuff);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
        }
    }
}