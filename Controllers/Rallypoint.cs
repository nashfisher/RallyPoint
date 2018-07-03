using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rallypoint.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Rallypoint.Controllers{
    public class RallypointController:Controller{
        private RallypointContext _context;
        public RallypointController(RallypointContext context){
            _context = context;
        }
        // public async Task<IActionResult> Index(string searchString)
        // {
        //     var player = from p in _context.Users select p;

        //     if (!String.IsNullOrEmpty(searchString))
        //     {
        //         player = player.Where(u => u.username.Contains(searchString));
        //     }

        //     return View(await player.ToListAsync());
        // }

        

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

            string userName = HttpContext.Session.GetString("Username");
            int? userId = HttpContext.Session.GetInt32("Id");
            IQueryable<Game> userGames = 
                // _context.Games.Where(g => g.playe roneId == userId || g.playertwoId == userId).Include(u1 => u1.playerone).Include(u2 => u2.playertwo);
                _context.Games.Where(g => g.playeroneUsername == userName || g.playertwoUsername == userName).Include(u1 => u1.playerone).Include(u2 => u2.playertwo);
            IQueryable<Game> availableGames =
                _context.Games.Where(g => (
                    (g.playertwoUsername == null &&
                    
                    // g.playeroneId != userId));
                    g.playeroneUsername != userName) || (g.playeroneUsername == null && g.playertwoUsername != userName)));
            IQueryable<Game> otherGames =
                _context.Games.Where(g => (
                    // g.playeroneId != userId &&
                    g.playeroneUsername != userName &&
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
            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");

            List<User> users = _context.Users.ToList();
            ViewBag.Users = users;
            return View("ScoreBoard");
        }

        [HttpPost]
        [Route("/games/join")]
        public IActionResult JoinGame(int GameId, bool join){
            string userName = HttpContext.Session.GetString("Username");

            // Display username in nav
            ViewBag.log = HttpContext.Session.GetString("Username");
            Game toJoin = _context.Games
                .Where(g => g.Id == GameId).SingleOrDefault();
                
                if (toJoin.playertwoUsername == null) {

                    toJoin.playertwoUsername = join ? userName : (string) null;
                }
                else {

                    toJoin.playeroneUsername = join ? userName : (string) null;
                }
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
        
        [HttpGet]
        [Route("games/new")]
        public IActionResult NewGame(){
            ViewBag.log = HttpContext.Session.GetString("Username");
            List<User> users = _context.Users.ToList();
            // ViewBag.Users = users;


            return View();
        }


        [HttpPost]
        [Route("/games/new")]
        public IActionResult NewGame(GameViewModel game) {
            int? playeroneId = HttpContext.Session.GetInt32("Id");

            // Username in navbar
            ViewBag.log = HttpContext.Session.GetString("Username");

            if(!ModelState.IsValid){
                
                return View("NewGame", game);
            }

            else if (ModelState.IsValid) {
                // Validations for username inputs - seperated for individual inputs
                if(((!_context.Users.Any(u => u.username == game.playeroneUsername) && game.playeroneUsername != null) || ((!_context.Users.Any(u => u.username == game.playertwoUsername) && game.playertwoUsername != null))))
                {
                    if(_context.Users.Any(u => u.username == game.playertwoUsername))
                    {
                        ModelState.AddModelError("playeroneUsername", "Username not found.");

                        return View("NewGame", game);
                    }
                    else if(_context.Users.Any(u => u.username == game.playeroneUsername))
                    {
                        ModelState.AddModelError("playertwoUsername", "Username not found.");

                        return View("NewGame", game);
                    }
                    else
                    {
                        ModelState.AddModelError("playeroneUsername", "Player one not found.");
                        ModelState.AddModelError("playertwoUsername", "Player two not found.");

                        return View("NewGame", game);
                    }
                }

                // Query for Users to add them to the created Game
                User p1 = _context.Users.FirstOrDefault(u => u.username == game.playeroneUsername);
                User p2 = _context.Users.FirstOrDefault(u => u.username == game.playertwoUsername);
                
                Game newGame = new Game(){
                    // playeroneId = game.playeroneId,
                    
                    playeroneUsername = game.playeroneUsername,
                
                    playerone = p1,
                    
                    playertwoUsername = game.playertwoUsername,

                    playertwo = p1,
                    // playertwoId = game.playertwoId,
                    date = (DateTime) game.date,
                    address = game.address
                };
                _context.Add(newGame);
                _context.SaveChanges();
                return RedirectToAction("GamesIndex");
            }
            return RedirectToAction("NewGame");
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

            // 3 wins isn't technically required, but just in case... 
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

            // Working above (fix the issue with wins/losses record)
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            

            _context.SaveChanges();
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
