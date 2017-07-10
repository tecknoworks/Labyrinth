using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labyrinth.Models;

namespace Labyrinth.Controllers
{
    public class PlayerItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlayerItems
        public async Task<ActionResult> Index()
        {
            var playerItems = db.PlayerItems.Include(p => p.Item).Include(p => p.Player);
            return View(await playerItems.ToListAsync());
        }

        // GET: PlayerItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerItem playerItem = await db.PlayerItems.FindAsync(id);
            if (playerItem == null)
            {
                return HttpNotFound();
            }
            return View(playerItem);
        }

        // GET: PlayerItems/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname");
            return View();
        }

        // POST: PlayerItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PlayerId,ItemId,Quantity")] PlayerItem playerItem)
        {
            if (ModelState.IsValid)
            {
                db.PlayerItems.Add(playerItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", playerItem.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", playerItem.PlayerId);
            return View(playerItem);
        }

        // GET: PlayerItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerItem playerItem = await db.PlayerItems.FindAsync(id);
            if (playerItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", playerItem.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", playerItem.PlayerId);
            return View(playerItem);
        }

        // POST: PlayerItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PlayerId,ItemId,Quantity")] PlayerItem playerItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", playerItem.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", playerItem.PlayerId);
            return View(playerItem);
        }

        // GET: PlayerItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerItem playerItem = await db.PlayerItems.FindAsync(id);
            if (playerItem == null)
            {
                return HttpNotFound();
            }
            return View(playerItem);
        }

        // POST: PlayerItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PlayerItem playerItem = await db.PlayerItems.FindAsync(id);
            db.PlayerItems.Remove(playerItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
