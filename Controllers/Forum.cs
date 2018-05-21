using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rallypoint.Models;

namespace Rallypoint.Controllers{

    public class ForumController:Controller{
        [HttpGet]
        [Route("test")]
        public IActionResult Index(){
            return View("Index","_ForumLayout");
        }
    }
}