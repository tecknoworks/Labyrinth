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
    [Authorize(Roles = "Administrator, Regular")]
    public class GameController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Game
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult EndGame()
        {
            return View("EndGame");

        }

        public ActionResult Chat()
        {
            return View("Chat");

        }

        public ActionResult Update(int lifes,int score,int deathStones)
        {
            var userId = new Guid(User.Identity.GetUserId());
            var playerItems = db.PlayerItems.Where(m => m.PlayerId == userId);
            db.Players.Find(userId).Gold += score;
            db.Players.Find(userId).Points += score;
            foreach (var item in playerItems)
            {
                //if (item.Item.Name == "LifeVial")
                //{
                //    item.Quantity = lifes;
                //}
                //if (item.Item.Name == "IronGaze")
                //{
                //    this.data.ironGaze = true;
                //}
                //if (item.Item.Name == "DeathStone")
                //{
                //    item.Quantity = deathStones;
                //}
                //if (item.Item.Name == "Stompy")
                //{
                //    this.data.stompy = true;
                //}
            }
            db.SaveChanges();
            return RedirectToAction("EndGame", "Game");

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