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
    public class SellsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sells
        public async Task<ActionResult> Index()
        {
            var sells = db.Sells.Include(s => s.Item).Include(s => s.Player);
            return View(await sells.ToListAsync());
        }

        // GET: Sells/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell sell = await db.Sells.FindAsync(id);
            if (sell == null)
            {
                return HttpNotFound();
            }
            return View(sell);
        }

        // GET: Sells/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname");
            return View();
        }

        // POST: Sells/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PlayerId,ItemId,Quantity,Price,IsSold")] Sell sell)
        {
            if (ModelState.IsValid)
            {
                db.Sells.Add(sell);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", sell.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", sell.PlayerId);
            return View(sell);
        }

        // GET: Sells/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell sell = await db.Sells.FindAsync(id);
            if (sell == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", sell.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", sell.PlayerId);
            return View(sell);
        }

        // POST: Sells/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PlayerId,ItemId,Quantity,Price,IsSold")] Sell sell)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sell).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", sell.ItemId);
            ViewBag.PlayerId = new SelectList(db.Players, "Id", "Nickname", sell.PlayerId);
            return View(sell);
        }

        // GET: Sells/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sell sell = await db.Sells.FindAsync(id);
            if (sell == null)
            {
                return HttpNotFound();
            }
            return View(sell);
        }

        // POST: Sells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sell sell = await db.Sells.FindAsync(id);
            db.Sells.Remove(sell);
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
