using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rallypoint.Models;
using System;

namespace Rallypoint.Controllers{
	
    public class RallypointController:Controller{
        private RallypointContext _context;
        public RallypointController(RallypointContext context){
            _context = context;
        }


        [HttpGet]
        [Route("/")]
        public IActionResult Index(){
            return View("Index");
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
                return RedirectToAction("Index");
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
