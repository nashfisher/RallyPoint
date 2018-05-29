using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rallypoint.Models;
using System;
using System.Collections.Generic;

namespace Rallypoint.Controllers{
    public class RallypointController:Controller{
        private RallypointContext _context;
        public RallypointController(RallypointContext context){
            _context = context;
        }

        


        public bool LoggedIn() {
            if (HttpContext.Session.GetString("Username") == null) {
                
                return false;
            }
            else {
                return true;
            }
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index(){

            ViewBag.splash = "True";

            ViewBag.log = "Login";

            return View();
        }

        [HttpGet]
        [Route("/games")]
        public IActionResult GamesIndex(){


            // Force user to login
            if (LoggedIn() == false) {
                
                return RedirectToAction("Loginpage", "RegisterLogin");
            }

            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");

            int? userId = HttpContext.Session.GetInt32("Id");
            IQueryable<Game> userGames = 
                _context.Games.Where(g => g.playeroneId == userId || g.playertwoId == userId).Include(u1 => u1.playerone).Include(u2 => u2.playertwo);
            IQueryable<Game> availableGames =
                _context.Games.Where(g => (
                    g.playertwoId == null &&
                    g.playeroneId != userId));
            IQueryable<Game> otherGames =
                _context.Games.Where(g => (
                    g.playeroneId != userId &&
                    g.playertwoId != userId));
            ViewBag.userId = userId;
            ViewBag.userGames = userGames;
            ViewBag.availableGames = availableGames;
            ViewBag.otherGames = otherGames;
            return View();
        }

        [HttpGet]
        [Route("/games/scoreboard")]
        public IActionResult ScoreBoard(){
            List<User> users = _context.Users.ToList();
            ViewBag.Users = users;
            return View("ScoreBoard");
        }

        [HttpPost]
        [Route("/games/join")]
        public IActionResult JoinGame(int GameId, bool join){
            int? userId = HttpContext.Session.GetInt32("Id");

            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");
            Game toJoin = _context.Games
                .Where(g => g.Id == GameId).SingleOrDefault();
            toJoin.playertwoId = join ? userId : (int?) null;
            _context.SaveChanges();
            return RedirectToAction("GamesIndex");
        }

        [HttpPost]
        [Route("/games/delete")]
        public IActionResult DeleteGame(int Id){

            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");

            Game toDelete = _context.Games
                .Where(g => g.Id == Id).SingleOrDefault();
            _context.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("GamesIndex");
        }

        [HttpPost]
        [Route("/games/new")]
        public IActionResult NewGame(GameViewModel game) {
            int? playeroneId = HttpContext.Session.GetInt32("Id");

            if (ModelState.IsValid) {
                Game newGame = new Game(){
                    playeroneId = game.playeroneId,
                    playertwoId = game.playertwoId,
                    date = (DateTime) game.date,
                    address = game.address
                };
                _context.Add(newGame);
                _context.SaveChanges();
                return RedirectToAction("NewGame");
            }
            return RedirectToAction("NewGame");
        }

        [HttpGet]
        [Route("games/new")]
        public IActionResult NewGame(){
            ViewBag.log = HttpContext.Session.GetString("Username");
            List<User> users = _context.Users.ToList();
            ViewBag.Users = users;
            return View();
        }

        [HttpPost]
        [Route("/gameinfo/{gameid}/updatescores")]
        public IActionResult UpdateScores(UpdateScoresViewModel model, int gameid){
            int p1subwins = 0;
            int p2subwins = 0;

            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");
        
            Game game = _context.Games.Include(up => up.playerone).Include(u =>u.playertwo).SingleOrDefault(g => g.Id == gameid);
            ViewBag.Game = game;
            User p1 = game.playerone;
            User p2 = game.playertwo;

            if (ModelState.IsValid) {
                game.playeroneroundoneScore = model.playeroneroundoneScore;
                game.playertworoundoneScore = model.playertworoundoneScore;
                game.playeroneroundtwoScore = model.playeroneroundtwoScore;
                game.playertworoundtwoScore = model.playertworoundtwoScore;
                game.playeroneroundthreeScore = model.playeroneroundthreeScore;
                game.playertworoundthreeScore = model.playertworoundthreeScore;

            }

            if(game.playeroneroundoneScore > game.playertworoundoneScore)
            {
                p1subwins++;
            }
            else
            {
                p2subwins++;
            }

            if(game.playeroneroundtwoScore > game.playertworoundtwoScore)
            {
                p1subwins++;
            }
            else
            {
                p2subwins++;
            }

            if(game.playeroneroundthreeScore > game.playertworoundthreeScore)
            {
                p1subwins++;
            }
            else
            {
                p2subwins++;
            }

            // 3 wins is unnecessary, but just in case...
            if(p1subwins == 2 || p1subwins == 3)
            {
                p1.wins++;
                p2.losses++;
            }
            else
            {
                p2.wins++;
                p1.losses++;
            }

            _context.SaveChanges();
            return View("gameinfo");
        }

        [HttpGet]
        [Route("/gameinfo/{gameid}")]
        public IActionResult gameinfo(int gameid){


            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");
            

            Game game = _context.Games.Where(g => g.Id == gameid).Include(up => up.playerone).Include(u =>u.playertwo).SingleOrDefault();
            ViewBag.Game = game;

            return View("gameinfo");
        }

        [HttpGet]
        [Route("/profile/{username}")]

        public IActionResult Showme(string username){
            List <User> singleuser = _context.Users.Where(u => u.username == username).Include(g => g.gamescreated).Include(j => j.gamesjoined).ToList();

            ViewBag.log = HttpContext.Session.GetString("Username");
            
            ViewBag.User = singleuser;
            return View("profile");
        }

        [HttpPost]
        [Route("/upload")]

        public IActionResult Upload(string link){
            User someuser = _context.Users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("Id"));
            someuser.imagelink = link;
            _context.SaveChanges();
            return RedirectToAction("Showme", new {username = someuser.username});

        }    
    }
}
