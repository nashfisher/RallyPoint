using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rallypoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rallypoint.Controllers{
	
    public class RallypointController:Controller{
        private RallypointContext _context;
        public RallypointController(RallypointContext context){
            _context = context;
        }


        [HttpGet]
        [Route("/")]
        public IActionResult Index(){
            return View();
        }

        [HttpGet]
        [Route("/games")]
        public IActionResult GamesIndex(){
            int userId = 1;
            IQueryable<Game> userGames = 
                _context.Games.Where(g => g.playeroneId == userId);
            IQueryable<Game> availableGames =
                _context.Games.Where(g => (
                    g.playertwoId == null &&
                    g.playeroneId != userId));
            IQueryable<Game> otherGames =
                _context.Games.Where(g => (
                    g.playeroneId != userId &&
                    g.playertwoId != null));
            ViewBag.userGames = userGames;
            ViewBag.availableGames = availableGames;
            ViewBag.otherGames = otherGames;
            return View();
        }

        [HttpPost]
        [Route("/games/join")]
        public IActionResult JoinGame(int GameId, bool join){
            int userId = 1;
            Game toJoin = _context.Games
                .Where(g => g.Id == GameId).SingleOrDefault();
            toJoin.playertwoId = join ? userId : (int?) null;
            _context.SaveChanges();
            return RedirectToAction("GamesIndex");
        }

        [HttpPost]
        [Route("/games/delete")]
        public IActionResult DeleteGame(int Id){
            Game toDelete = _context.Games
                .Where(g => g.Id == Id).SingleOrDefault();
            _context.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("GamesIndex");
        }

        [HttpPost]
        [Route("/games/new")]
        public IActionResult NewGame(GameViewModel game) {
            //Some logic to get the current user id.
            int playeroneId = 1;

            if (ModelState.IsValid) {
                Game newGame = new Game(){
                    playeroneId = playeroneId,
                    date = (DateTime) game.date,
                    address = game.address
                };
                _context.Add(newGame);
                _context.SaveChanges();
                return RedirectToAction("GamesIndex");
            }
            return View();
        }

        [HttpGet]
        [Route("games/new")]
        public IActionResult NewGame(){
            return View();
        }
    }
}
