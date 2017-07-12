using Labyrinth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labyrinth.Controllers
{
    public class LeaderBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: LeaderBoard
        public ActionResult Index()
        {
            IOrderedEnumerable<Player> players = db.Players.ToList().OrderByDescending(x => x.Points);
            return View(players);
        }
    }
}