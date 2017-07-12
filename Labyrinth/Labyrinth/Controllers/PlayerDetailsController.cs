using Labyrinth.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Labyrinth.Controllers
{
    public class PlayerDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PlayerDetails
        public ActionResult Index()
        {
             IOrderedEnumerable<Player> players = db.Players.ToList().OrderBy(x => x.Points);
             var currentUserId = User.Identity.GetUserId();
             Player currentUser = players.SingleOrDefault(x => x.Id.Equals(new Guid(currentUserId)));
             return View(currentUser);
        }
    }
}