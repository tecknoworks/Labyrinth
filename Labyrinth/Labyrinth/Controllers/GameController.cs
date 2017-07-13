using Labyrinth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Labyrinth.Controllers
{
    public class GameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Game
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult YouDied(string score)
        {
            int totalScore = Int32.Parse(score);
            var player = db.Players.Find(new Guid(User.Identity.GetUserId()));
            player.Gold += totalScore;
            player.Points += totalScore;
            db.SaveChanges();
            return View("YouDied");
        }
    }
}