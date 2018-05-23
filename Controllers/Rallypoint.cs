using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rallypoint.Models;

namespace Rallypoint.Controllers{
    public class RallypointController:Controller{
        private RallypointContext _context;
        public RallypointController(RallypointContext context){
            _context = context;
        }


        [HttpGet]
        [Route("/")]
        public IActionResult Index(){

            ViewBag.log = "Login";
            
            return View("Index");
        }

        [HttpGet]
        [Route("/gameinfo/{gameid}")]

        public IActionResult gameinfo(int gameid){
            ViewBag.Game = _context.Games.Where(g => g.Id == gameid).Include(up => up.playerone).Include(u =>u.playertwo);
            return View("gameinfo");
        }
    }
}
