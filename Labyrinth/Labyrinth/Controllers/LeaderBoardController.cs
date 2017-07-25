using Labyrinth;
using Labyrinth.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        public ActionResult banUser(Guid id)
        {
            ApplicationUser user = db.Users.Where(u => u.Id == id.ToString() ).FirstOrDefault();
            var database = new UserStore<ApplicationUser>(db);
            var manager = new UserManager<ApplicationUser>(database);
            manager.AddToRole(user.Id, "Banned");
            manager.RemoveFromRole(user.Id, "Regular");
            manager.RemoveFromRole(user.Id, "Administrator");
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}