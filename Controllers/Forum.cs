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
            List<Post> test = _context.Posts.Include(p => p.user).Include(l => l.likedpost).ToList();
            ViewBag.RecentPost = test;
            ViewBag.CurrentUser = CurrentUser;
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
        [Route("forum/{like}/{postId}/{routeID}")]
        public IActionResult LikePost(int postId, bool like, string routeID)
        {
            Like stuff = _context.Likes.SingleOrDefault(j => j.PostId == postId && j.UserId == HttpContext.Session.GetInt32("Id"));
            if(stuff == null )
            {
                Like newlike = new Like
                {
                    PostId = postId,
                    UserId = (int)HttpContext.Session.GetInt32("Id"),
                    liked = like
                };
                _context.Likes.Add(newlike);
                _context.SaveChanges();
                if(routeID == "post")
                {
                    return RedirectToAction("GetPost");
                }
                return RedirectToAction("Index");
            }
            else if(stuff.liked == null)
            {

                stuff.liked = like;
                _context.SaveChanges();
                if(routeID == "post")
                {
                    return RedirectToAction("GetPost");
                }
                return RedirectToAction("Index");
            }
            switch(like)
            {
                case true:
                    if((bool)stuff.liked)
                    {
                        stuff.liked = null;
                        _context.SaveChanges();
                        break;
                    }
                    else if(!(bool)stuff.liked)
                    {
                        stuff.liked = like;
                        _context.SaveChanges();
                        break;
                    }
                    else {
                        
                        Like likedstuff = new Like {
                            PostId = postId,
                            UserId =(int)HttpContext.Session.GetInt32("Id"),
                            liked = true
                        };
                        // _context.Posts.SingleOrDefault(p => p.Id == postId); will be used to grab post
                        _context.Likes.Add(likedstuff);
                        _context.SaveChanges();
                        break;
                    }
                case false:
                    if(!(bool)stuff.liked)
                    {
                        stuff.liked = null;
                        _context.SaveChanges();
                        break;
                    }
                    else if((bool)stuff.liked)
                    {
                        stuff.liked = like;
                        _context.SaveChanges();
                        break;
                    }
                    else {
                        
                        Like likedstuff = new Like {
                            PostId = postId,
                            UserId =(int)HttpContext.Session.GetInt32("Id"),
                            liked = false
                        };
                        // _context.Posts.SingleOrDefault(p => p.Id == postId); will be used to grab post
                        _context.Likes.Add(likedstuff);
                        _context.SaveChanges();
                        break;
                    }
            }
            if(routeID == "post")
            {
                return RedirectToAction("GetPost");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("forum/post/{postId}")]
        public IActionResult GetPost(int postId)
        {
            if (HttpContext.Session.GetString("Username") == null) {
                ViewBag.log = "Login";
            }
            else {
                ViewBag.log = HttpContext.Session.GetString("Username");
            }
            List<Post> result = _context.Posts.Where(p => p.Id == postId).Include(lp => lp.likedpost).ToList();
            var CurrentUser = _context.Users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("Id"));
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.Post = result;
            return View();
        }

        [HttpPost]
        [Route("forum/commentpost/{postId")]
        public IActionResult CommentPost(int postId)
        {
            if(HttpContext.Session.GetString("Username") == null)
            {
                ViewBag.log = "Login";
            }
            else
            {
                ViewBag.log = HttpContext.Session.GetString("Username");
            }
            return RedirectToAction("GetPost");
        }
    }
}