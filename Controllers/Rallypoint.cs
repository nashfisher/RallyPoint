using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            ViewBag.log = HttpContext.Session.GetString("Username");
            
            return View("Index");
        }
    }
}
