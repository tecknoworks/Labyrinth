using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labyrinth.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return new FilePathResult("Content/Game/boilerplate/index.html", "text/html");

        }
    }
}